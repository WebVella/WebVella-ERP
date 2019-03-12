using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Web.DataSource
{
	public class EntityRecordListToSelectOptionConverter : CodeDataSource
	{
		public EntityRecordListToSelectOptionConverter() : base()
		{
			Id = new Guid("12dcdf08-af03-4347-8015-bd9bace17514");
			Name = "EntityRecordToSelectOptions";
			Description = "Mapper for EntityRecord to SelectOption";
			ResultModel = "List<SelectOption>";

			//define custom meta
			DataSourceModelFieldMeta dsMeta = new DataSourceModelFieldMeta();
			dsMeta.EntityName = string.Empty;
			dsMeta.Name = "value";
			dsMeta.Type = FieldType.TextField;
			Fields.Add(dsMeta);
			dsMeta = new DataSourceModelFieldMeta();
			dsMeta.EntityName = string.Empty;
			dsMeta.Name = "label";
			dsMeta.Type = FieldType.TextField;
			Fields.Add(dsMeta);
			dsMeta = new DataSourceModelFieldMeta();
			dsMeta.EntityName = string.Empty;
			dsMeta.Name = "icon_class";
			dsMeta.Type = FieldType.TextField;
			Fields.Add(dsMeta);
			dsMeta = new DataSourceModelFieldMeta();
			dsMeta.EntityName = string.Empty;
			dsMeta.Name = "color";
			dsMeta.Type = FieldType.TextField;
			Fields.Add(dsMeta);


			Parameters.Add(new DataSourceParameter { Name = "DataSourceName", Type = "text", Value = "" });
			Parameters.Add(new DataSourceParameter { Name = "KeyPropName", Type = "text", Value = "id" });
			Parameters.Add(new DataSourceParameter { Name = "ValuePropName", Type = "text", Value = "label" });
			Parameters.Add(new DataSourceParameter { Name = "IconClassPropName", Type = "text", Value = "icon_class" });
			Parameters.Add(new DataSourceParameter { Name = "ColorPropName", Type = "text", Value = "color" });
			Parameters.Add(new DataSourceParameter { Name = "SortOrderPropName", Type = "text", Value = "" });
			Parameters.Add(new DataSourceParameter { Name = "SortTypePropName", Type = "text", Value = "asc" });
		}

		public override object Execute(Dictionary<string, object> arguments)
		{
			var result = new List<SelectOption>();
			//if pageModel is not provided, returns empty List<SelectOption>()
			if (arguments.ContainsKey("PageModel") && arguments.ContainsKey("DataSourceName") && !string.IsNullOrWhiteSpace((string)arguments["DataSourceName"]))
			{
				//replace constants with your values
				string DATASOURCE_NAME = (string)arguments["DataSourceName"];
				string KEY_FIELD_NAME = (string)arguments["KeyPropName"];
				string VALUE_FIELD_NAME = (string)arguments["ValuePropName"];
				string ICON_CLASS_FIELD_NAME = (string)arguments["IconClassPropName"];
				string COLOR_FIELD_NAME = (string)arguments["ColorPropName"];
				string SORT_ORDER_FIELD_NAME = (string)arguments["SortOrderPropName"];
				string SORT_TYPE_NAME = (string)arguments["SortTypePropName"];

				PageDataModel pageModel = arguments["PageModel"] as PageDataModel;
				if (pageModel == null)
					return result;

				try
				{
					//try read data source by name and get result as specified type object
					var dataSource = pageModel.GetProperty(DATASOURCE_NAME);

					//if data source not found or different type, return empty List<SelectOption>()
					if (dataSource == null)
						return result;

					if (dataSource is List<EntityRecord>)
					{
						var recordsList = (List<EntityRecord>)dataSource;
						if (!String.IsNullOrWhiteSpace(SORT_ORDER_FIELD_NAME) && recordsList.Count > 1 && recordsList[0].Properties.ContainsKey(SORT_ORDER_FIELD_NAME)
							&& recordsList[0][SORT_ORDER_FIELD_NAME] != null) {
							bool isDecimal = false;
							if (decimal.TryParse((recordsList[0][SORT_ORDER_FIELD_NAME] ?? "").ToString(), out decimal outDecimal)) {
								isDecimal = true;
							}

							if (SORT_TYPE_NAME.ToLowerInvariant() == "desc")
							{
								if (isDecimal) {
									dataSource = recordsList.OrderByDescending(x => (decimal?)x[SORT_ORDER_FIELD_NAME]).ToList();
								}
								else {
									dataSource = recordsList.OrderByDescending(x => (string)x[SORT_ORDER_FIELD_NAME]).ToList();
								}
							}
							else {
								if (isDecimal)
								{
									dataSource = recordsList.OrderBy(x => (decimal?)x[SORT_ORDER_FIELD_NAME]).ToList();
								}
								else
								{
									dataSource = recordsList.OrderBy(x => (string)x[SORT_ORDER_FIELD_NAME]).ToList();
								}
							}
						}
						foreach (var record in (List<EntityRecord>)dataSource)
						{
							if (record.Properties.ContainsKey(ICON_CLASS_FIELD_NAME) && record[ICON_CLASS_FIELD_NAME] != null)
							{
								var color = "#999";
								if (record.Properties.ContainsKey(COLOR_FIELD_NAME) && record[COLOR_FIELD_NAME] != null)
								{
									color = record[COLOR_FIELD_NAME].ToString();
								}
								result.Add(new SelectOption(record[KEY_FIELD_NAME].ToString(), record[VALUE_FIELD_NAME].ToString(), record[ICON_CLASS_FIELD_NAME].ToString(), color));
							}
							else {
								result.Add(new SelectOption(record[KEY_FIELD_NAME].ToString(), record[VALUE_FIELD_NAME].ToString()));
							}
						}
					}
					else if (dataSource is EntityRecord)
					{
						var record = (EntityRecord)dataSource;
						if (record.Properties.ContainsKey(ICON_CLASS_FIELD_NAME) && record[ICON_CLASS_FIELD_NAME] != null)
						{
							var color = "#999";
							if (record.Properties.ContainsKey(COLOR_FIELD_NAME) && record[COLOR_FIELD_NAME] != null)
							{
								color = record[COLOR_FIELD_NAME].ToString();
							}
							result.Add(new SelectOption(record[KEY_FIELD_NAME].ToString(), record[VALUE_FIELD_NAME].ToString(), record[ICON_CLASS_FIELD_NAME].ToString(), color));
						}
						else
						{
							result.Add(new SelectOption(record[KEY_FIELD_NAME].ToString(), record[VALUE_FIELD_NAME].ToString()));
						}
					}
					else {
						throw new Exception("Input Datasource value should be of type List<EntityRecord> or EntityRecord");
					}

				}
				catch (PropertyDoesNotExistException)
				{
				}
			}
			return result;
		}
	}
}
