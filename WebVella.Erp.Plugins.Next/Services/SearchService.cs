using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.AutoMapper;
using WebVella.Erp.Eql;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Plugins.Next.Services
{
	public class SearchService : BaseService
	{
		public void RegenSearchField(string entityName, EntityRecord record, List<string> indexedFields) {
			var searchIndex = "";
			var relations = new EntityRelationManager().Read().Object;
			var entities = new EntityManager().ReadEntities().Object;
			var currentEntity = entities.FirstOrDefault(x => x.Name == entityName);
			if (currentEntity == null)
				throw new Exception($"Search index generation failed: Entity {entityName} not found");

			// Generate request columns
			var requestColumns = new List<string>();
			foreach (var fieldName in indexedFields)
			{
				if (!fieldName.StartsWith("$"))
				{
					//Entity field
					var field = currentEntity.Fields.FirstOrDefault(x => x.Name == fieldName);
					if (field == null)
						continue; // missing fields are ignored
					
					requestColumns.Add(fieldName);
				}
				else {
					//Relation field
					var fieldNameArray = fieldName.Replace("$", "").Split(".", StringSplitOptions.RemoveEmptyEntries);
					if (fieldNameArray.Length != 2)
						continue; //currently we process only fields defined as "$relation_name.field_name"

					var relation = relations.FirstOrDefault(x => x.Name == fieldNameArray[0]);
					if (relation == null)
						continue; //missing relations are ignored

					Guid? relatedEntityId = null;
					if (relation.OriginEntityId == currentEntity.Id)
						relatedEntityId = relation.TargetEntityId;
					else if (relation.TargetEntityId == currentEntity.Id)
						relatedEntityId = relation.OriginEntityId;

					if(relatedEntityId == null)
						continue; // the defined relation does not include the current entity

					var relatedEntity = entities.FirstOrDefault(x => x.Id == relatedEntityId);

					if (relatedEntity == null)
						continue; // related entity no longer exists.Ignore

					var relatedField = relatedEntity.Fields.FirstOrDefault(x => x.Name == fieldNameArray[1]);

					if (relatedField == null)
						continue; //related field does not exist

					requestColumns.Add(fieldName);
				}
			}

			//Generate request

			var eqlCommand = $"SELECT {String.Join(",", requestColumns)} FROM {entityName} WHERE id = @recordId PAGE 1 PAGESIZE 1";
			var eqlParameters = new List<EqlParameter>() { new EqlParameter("recordId", (Guid)record["id"]) };
			var eqlResult = new EqlCommand(eqlCommand, eqlParameters).Execute();

			//After update creation or update, the record is existing
			if (eqlResult.Count > 0)
			{
				var currentRecord = eqlResult[0];
				foreach (var columnName in requestColumns)
				{
					if (!columnName.StartsWith("$"))
					{
						//Record column
						if (currentRecord.Properties.ContainsKey(columnName) && currentRecord[columnName] != null)
						{
							try
							{
								var stringValue = GetStringValue(columnName,currentEntity,currentRecord);

								if (!String.IsNullOrWhiteSpace(stringValue))
									searchIndex += stringValue + " ";
							}
							catch
							{
								//Do nothing
							}
						}
					}
					else
					{
						//Related record column
						var columnNameArray = columnName.Split(".", StringSplitOptions.RemoveEmptyEntries);
						if (columnNameArray.Length == 2)
						{
							if (currentRecord.Properties.ContainsKey(columnNameArray[0]) && currentRecord[columnNameArray[0]] != null)
							{
								try
								{
									if (currentRecord[columnNameArray[0]] is List<EntityRecord>)
									{
										var relatedRecords = (List<EntityRecord>)currentRecord[columnNameArray[0]];
										foreach (var relatedRecord in relatedRecords)
										{
											if (relatedRecord.Properties.ContainsKey(columnNameArray[1]) && relatedRecord[columnNameArray[1]] != null)
											{
												var stringValue = relatedRecord[columnNameArray[1]].ToString();
												if (!String.IsNullOrWhiteSpace(stringValue))
													searchIndex += stringValue + " ";
											}
										}
									}
									else if (currentRecord[columnNameArray[0]] is EntityRecord)
									{
										var relatedRecord = (EntityRecord)currentRecord[columnNameArray[0]];

										if (relatedRecord.Properties.ContainsKey(columnNameArray[1]) && relatedRecord[columnNameArray[1]] != null)
										{
											var stringValue = relatedRecord[columnNameArray[1]].ToString();
											if (!String.IsNullOrWhiteSpace(stringValue))
												searchIndex += stringValue + " ";
										}
									}
								}
								catch
								{
									//Do nothing
								}
							}
						}
					}
				}
			}
			else {
				//Do nothing, the eql should find a record if all is OK
			}

			var patchRecord = new EntityRecord();
			patchRecord["id"] = (Guid)record["id"];
			patchRecord["x_search"] = searchIndex;
			var updateRecordResult = new RecordManager(executeHooks: false).UpdateRecord(entityName, patchRecord);
			if (!updateRecordResult.Success) {
				throw new ValidationException()
				{
					Message = updateRecordResult.Message,
					Errors = updateRecordResult.Errors.MapTo<ValidationError>()
				};
			}

		}

		private string GetStringValue(string fieldName, Entity entity, EntityRecord record) {
			var stringValue = "";
			if (!record.Properties.ContainsKey(fieldName) || record[fieldName] == null)
				return stringValue;

			var fieldMeta = entity.Fields.First(x => x.Name == fieldName);
			switch (fieldMeta.GetFieldType())
			{
				case FieldType.AutoNumberField:
					//Apply template
					{
						var exactMeta = (AutoNumberField)fieldMeta;
						if (!String.IsNullOrWhiteSpace(exactMeta.DisplayFormat))
							stringValue = string.Format(exactMeta.DisplayFormat, ((decimal)record[fieldName]).ToString("N0"));
					}
					break;

				case FieldType.CurrencyField:
					//as currency string
					{
						var exactMeta = (CurrencyField)fieldMeta;
						var currency = exactMeta.Currency;
						stringValue = currency.Code + " ";
						var amountString = ((decimal)record[fieldName]).ToString("N" + currency.DecimalDigits);
						if (exactMeta.Currency.SymbolPlacement == CurrencySymbolPlacement.Before)
						{
							stringValue += currency.SymbolNative + amountString;
						}
						else {
							stringValue += amountString + currency.SymbolNative;
						}
					}
					break;

				case FieldType.DateField:
					//Apply template
					{
						var exactMeta = (DateField)fieldMeta;
						stringValue = ((DateTime)record[fieldName]).ToString(exactMeta.Format);
					}
					break;

				case FieldType.DateTimeField:
					//Apply template
					{
						var exactMeta = (DateTimeField)fieldMeta;
						stringValue = ((DateTime)record[fieldName]).ToString(exactMeta.Format);
					}
					break;

				case FieldType.MultiSelectField:
					//option labels
					{
						var exactMeta = (MultiSelectField)fieldMeta;
						var values = new List<string>();
						var fieldValue = record[fieldName];
						if (fieldValue is List<string>)
							values = (List<string>)fieldValue;
						else if (fieldValue is string) {
							var fieldValueString = (string)fieldValue;
							if (fieldValueString.Contains(","))
							{
								values = fieldValueString.Split(",").ToList();
							}
							else {
								values.Add(fieldValueString);
							}
						}
						foreach (var value in values)
						{
							var option = exactMeta.Options.First(x => x.Value.ToLowerInvariant() == value.ToLowerInvariant());
							if (option != null)
							{
								stringValue += option.Label + " ";
							}
							else {
								stringValue += value + " ";
							}
						}
					}
					break;

				case FieldType.PasswordField:
					//ignore
					break;

				case FieldType.NumberField:
					{
						var exactMeta = (NumberField)fieldMeta;
						stringValue = ((decimal)record[fieldName]).ToString("N" + exactMeta.DecimalPlaces);
					}
					break;

				case FieldType.PercentField:
					//as percent, not float
					{
						var exactMeta = (PercentField)fieldMeta;
						stringValue = ((decimal)record[fieldName]).ToString("P" + exactMeta.DecimalPlaces);
					}
					break;

				case FieldType.SelectField:
					//option label
					{
						var exactMeta = (SelectField)fieldMeta;
						var value = (string)record[fieldName];
						var option = exactMeta.Options.First(x => x.Value.ToLowerInvariant() == value.ToLowerInvariant());
						if (option != null)
						{
							stringValue += option.Label + " ";
						}
						else
						{
							stringValue += value + " ";
						}
					}
					break;

				default:
					stringValue = record[fieldName].ToString();
					break;
			}

			return stringValue;
		}
	}
}
