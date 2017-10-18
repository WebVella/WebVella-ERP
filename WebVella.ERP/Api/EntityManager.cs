using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Api.Models.AutoMapper;
using WebVella.ERP.Database;
using WebVella.ERP.Storage;
using WebVella.ERP.Utilities;
using WebVella.ERP.Utilities.Dynamic;

namespace WebVella.ERP.Api
{
	public class EntityManager
	{
		public EntityManager()
		{
		}

		#region << Validation methods >>

		private List<ErrorModel> ValidateEntity(Entity entity, bool checkId = true)
		{
			List<ErrorModel> errorList = new List<ErrorModel>();

			if (entity.Id == Guid.Empty)
				errorList.Add(new ErrorModel("id", null, "Id is required!"));

			if (checkId)
			{
				//update
				if (entity.Id != Guid.Empty)
				{
					Entity verifiedEntity = ReadEntity(entity.Id).Object;

					if (verifiedEntity == null)
						errorList.Add(new ErrorModel("id", entity.Id.ToString(), "Entity with such Id does not exist!"));
				}
			}
			else
			{
				//create

			}

			errorList.AddRange(ValidationUtility.ValidateName(entity.Name));

			if (!string.IsNullOrWhiteSpace(entity.Name))
			{
				Entity verifiedEntity = ReadEntity(entity.Name).Object;

				if (verifiedEntity != null && verifiedEntity.Id != entity.Id)
					errorList.Add(new ErrorModel("name", entity.Name, "Entity with such Name exists already!"));
			}

			errorList.AddRange(ValidationUtility.ValidateLabel(entity.Label));

			errorList.AddRange(ValidationUtility.ValidateLabelPlural(entity.LabelPlural));

			if (entity.RecordPermissions == null)
				entity.RecordPermissions = new RecordPermissions();

			if (entity.RecordPermissions.CanRead == null)
				entity.RecordPermissions.CanRead = new List<Guid>();

			if (entity.RecordPermissions.CanCreate == null)
				entity.RecordPermissions.CanCreate = new List<Guid>();

			if (entity.RecordPermissions.CanUpdate == null)
				entity.RecordPermissions.CanUpdate = new List<Guid>();

			if (entity.RecordPermissions.CanDelete == null)
				entity.RecordPermissions.CanDelete = new List<Guid>();

			if (string.IsNullOrWhiteSpace(entity.IconName))
				entity.IconName = "database";

			return errorList;
		}

		private List<ErrorModel> ValidateFields(Guid entityId, List<InputField> fields, bool checkId = true)
		{
			List<ErrorModel> errorList = new List<ErrorModel>();

			Entity entity = ReadEntity(entityId).Object;

			if (fields.Count == 0)
			{
				errorList.Add(new ErrorModel("fields", null, "There should be at least one field!"));
				return errorList;
			}

			int primaryFieldCount = 0;

			foreach (var field in fields)
			{
				errorList.AddRange(ValidateField(entity, field, false));

				if (field is InputGuidField)
				{
					primaryFieldCount++;
				}
			}

			if (primaryFieldCount < 1)
				errorList.Add(new ErrorModel("fields.id", null, "Must have one unique identifier field!"));

			if (primaryFieldCount > 1)
				errorList.Add(new ErrorModel("fields.id", null, "Too many primary fields. Must have only one unique identifier!"));

			return errorList;
		}

		private List<ErrorModel> ValidateField(Entity entity, InputField field, bool checkId = true)
		{
			List<ErrorModel> errorList = new List<ErrorModel>();

			if (field.Id == Guid.Empty)
				errorList.Add(new ErrorModel("id", null, "Id is required!"));

			int fieldsSameIdCount = entity.Fields.Where(f => f.Id == field.Id).Count();

			if ((checkId && fieldsSameIdCount > 1) || (!checkId && fieldsSameIdCount > 0))
				errorList.Add(new ErrorModel("id", null, "There is already a field with such Id!"));

			int fieldsSameNameCount = entity.Fields.Where(f => f.Name == field.Name).Count();

			if ((checkId && fieldsSameNameCount > 1) || (!checkId && fieldsSameNameCount > 0))
				errorList.Add(new ErrorModel("name", null, "There is already a field with such Name!"));

			errorList.AddRange(ValidationUtility.ValidateName(field.Name));

			errorList.AddRange(ValidationUtility.ValidateLabel(field.Label));

			if (field is InputAutoNumberField)
			{
				if (field.Required.HasValue && field.Required.Value && !((InputAutoNumberField)field).DefaultValue.HasValue)
					errorList.Add(new ErrorModel("defaultValue", null, "Default Value is required!"));

				//if (((AutoNumberField)field).DisplayFormat == null)
				//    errorList.Add(new ErrorModel("DisplayFormat", null, "DisplayFormat is required!"));

				//if (!((AutoNumberField)field).StartingNumber.HasValue)
				//    errorList.Add(new ErrorModel("startingNumber", null, "Starting Number is required!"));

				//TODO:parse DisplayFormat field
			}
			else if (field is InputCheckboxField)
			{
				if (!((InputCheckboxField)field).DefaultValue.HasValue)
					((InputCheckboxField)field).DefaultValue = false;
			}
			else if (field is InputCurrencyField)
			{
				if (field.Required.HasValue && field.Required.Value && !((InputCurrencyField)field).DefaultValue.HasValue)
					errorList.Add(new ErrorModel("defaultValue", null, "Default Value is required!"));

				//if (!((CurrencyField)field).MinValue.HasValue)
				//    errorList.Add(new ErrorModel("minValue", null, "Min Value is required!"));

				//if (!((CurrencyField)field).MaxValue.HasValue)
				//    errorList.Add(new ErrorModel("maxValue", null, "Max Value is required!"));

				//if (((CurrencyField)field).MinValue.HasValue && ((CurrencyField)field).MaxValue.HasValue)
				//{
				//    if (((CurrencyField)field).MinValue.Value >= ((CurrencyField)field).MaxValue.Value)
				//        errorList.Add(new ErrorModel("MinValue", null, "Min Value must be less than Max Value!"));
				//}
			}
			else if (field is InputDateField)
			{
				//TODO:parse format and check if it is valid

				if (string.IsNullOrWhiteSpace(((InputDateField)field).Format))
					errorList.Add(new ErrorModel("format", null, "Date format is required!"));

				if (!((InputDateField)field).UseCurrentTimeAsDefaultValue.HasValue)
					((InputDateField)field).UseCurrentTimeAsDefaultValue = false;
				//errorList.Add(new ErrorModel("useCurrentTimeAsDefaultValue", null, "Use current Time is required!"));
			}
			else if (field is InputDateTimeField)
			{
				//TODO:parse format and check if it is valid

				if (string.IsNullOrWhiteSpace(((InputDateTimeField)field).Format))
					errorList.Add(new ErrorModel("format", null, "Datetime format is required!"));

				if (!((InputDateTimeField)field).UseCurrentTimeAsDefaultValue.HasValue)
					((InputDateTimeField)field).UseCurrentTimeAsDefaultValue = false;
				//errorList.Add(new ErrorModel("useCurrentTimeAsDefaultValue", null, "Use current Time is required!"));
			}
			else if (field is InputEmailField)
			{
				if (field.Required.HasValue && field.Required.Value && ((InputEmailField)field).DefaultValue == null)
					errorList.Add(new ErrorModel("defaultValue", null, "Default Value is required!"));

				//if (!((EmailField)field).MaxLength.HasValue)
				//    errorList.Add(new ErrorModel("maxLength", null, "Max Length is required!"));
			}
			else if (field is InputFileField)
			{
				if (field.Required.HasValue && field.Required.Value && ((InputFileField)field).DefaultValue == null)
					errorList.Add(new ErrorModel("defaultValue", null, "Default Value is required!"));
			}
			//else if (field is FormulaField)
			//{
			//    if (!string.IsNullOrWhiteSpace(((FormulaField)field).FormulaText))
			//    {
			//        //TODO: parse formula text and check if it is valid
			//    }
			//    else
			//        errorList.Add(new ErrorModel("fields.formulaText", null, "Formula Text is required!"));

			//    if (!((FormulaField)field).DecimalPlaces.HasValue)
			//        errorList.Add(new ErrorModel("fields.decimalPlaces", null, "Decimal Places is required!"));
			//}
			else if (field is InputGuidField)
			{
				if ((((InputGuidField)field).Unique.HasValue && ((InputGuidField)field).Unique.Value) &&
				   (!((InputGuidField)field).GenerateNewId.HasValue || !((InputGuidField)field).GenerateNewId.Value))
					errorList.Add(new ErrorModel("defaultValue", null, "Generate New Id is required when the field is marked as unique!"));

				if ((((InputGuidField)field).Required.HasValue && ((InputGuidField)field).Required.Value) &&
					(!((InputGuidField)field).GenerateNewId.HasValue || !((InputGuidField)field).GenerateNewId.Value) &&
					((InputGuidField)field).DefaultValue == null)
					errorList.Add(new ErrorModel("defaultValue", null, "Default Value is required when the field is marked as required and generate new id option is not selected!"));
			}
			else if (field is InputHtmlField)
			{
				if (field.Required.HasValue && field.Required.Value && ((InputHtmlField)field).DefaultValue == null)
					errorList.Add(new ErrorModel("defaultValue", null, "Default Value is required!"));
			}
			else if (field is InputImageField)
			{
				if (field.Required.HasValue && field.Required.Value && ((InputImageField)field).DefaultValue == null)
					errorList.Add(new ErrorModel("defaultValue", null, "Default Value is required!"));
			}
			else if (field is InputMultiLineTextField)
			{
				if (field.Required.HasValue && field.Required.Value && ((InputMultiLineTextField)field).DefaultValue == null)
					errorList.Add(new ErrorModel("defaultValue", null, "Default Value is required!"));

				//if (!((MultiLineTextField)field).MaxLength.HasValue)
				//    errorList.Add(new ErrorModel("maxLength", null, "Max Length is required!"));

				//if (!((MultiLineTextField)field).VisibleLineNumber.HasValue)
				//    errorList.Add(new ErrorModel("visibleLineNumber", null, "Visible Line Number is required!"));

				//if (((MultiLineTextField)field).VisibleLineNumber.HasValue && ((MultiLineTextField)field).VisibleLineNumber.Value > 20)
				//    errorList.Add(new ErrorModel("visibleLineNumber", null, "Visible Line Number cannot be greater than 20!"));
			}
			else if (field is InputMultiSelectField)
			{
				if (field.Required.HasValue && field.Required.Value &&
					(((InputMultiSelectField)field).DefaultValue == null || ((InputMultiSelectField)field).DefaultValue.Count() == 0))
					errorList.Add(new ErrorModel("defaultValue", null, "Default Value is required!"));

				if (((InputMultiSelectField)field).Options != null)
				{
					if (((InputMultiSelectField)field).Options.Count == 0)
						errorList.Add(new ErrorModel("options", null, "Options must contains at least one item!"));
				}
				else
					errorList.Add(new ErrorModel("options", null, "Options is required!"));
			}
			else if (field is InputNumberField)
			{
				if (field.Required.HasValue && field.Required.Value && !((InputNumberField)field).DefaultValue.HasValue)
					errorList.Add(new ErrorModel("defaultValue", null, "Default Value is required!"));

				//if (!((NumberField)field).MinValue.HasValue)
				//    errorList.Add(new ErrorModel("minValue", null, "Min Value is required!"));

				//if (!((NumberField)field).MaxValue.HasValue)
				//    errorList.Add(new ErrorModel("maxValue", null, "Max Value is required!"));

				//if (((NumberField)field).MinValue.HasValue && ((NumberField)field).MaxValue.HasValue)
				//{
				//    if (((NumberField)field).MinValue.Value >= ((NumberField)field).MaxValue.Value)
				//        errorList.Add(new ErrorModel("MinValue", null, "Min Value must be less than Max Value!"));
				//}

				if (!((InputNumberField)field).DecimalPlaces.HasValue)
					((InputNumberField)field).DecimalPlaces = 2;
				//errorList.Add(new ErrorModel("decimalPlaces", null, "Decimal Places is required!"));
			}
			else if (field is InputPasswordField)
			{
				//if (!((PasswordField)field).MaxLength.HasValue)
				//    errorList.Add(new ErrorModel("maxLength", null, "Max Length is required!"));

				if (!((InputPasswordField)field).Encrypted.HasValue)
					((InputPasswordField)field).Encrypted = true;
			}
			else if (field is InputPercentField)
			{
				if (field.Required.HasValue && field.Required.Value && !((InputPercentField)field).DefaultValue.HasValue)
					errorList.Add(new ErrorModel("defaultValue", null, "Default Value is required!"));

				//if (!((PercentField)field).MinValue.HasValue)
				//    errorList.Add(new ErrorModel("minValue", null, "Min Value is required!"));

				//if (!((PercentField)field).MaxValue.HasValue)
				//    errorList.Add(new ErrorModel("maxValue", null, "Max Value is required!"));

				//if (((PercentField)field).MinValue.HasValue && ((PercentField)field).MaxValue.HasValue)
				//{
				//    if (((PercentField)field).MinValue.Value >= ((PercentField)field).MaxValue.Value)
				//        errorList.Add(new ErrorModel("MinValue", null, "Min Value must be less than Max Value!"));
				//}

				if (!((InputPercentField)field).DecimalPlaces.HasValue)
					((InputPercentField)field).DecimalPlaces = 2;
				//errorList.Add(new ErrorModel("decimalPlaces", null, "Decimal Places is required!"));
			}
			else if (field is InputPhoneField)
			{
				if (field.Required.HasValue && field.Required.Value && ((InputPhoneField)field).DefaultValue == null)
					errorList.Add(new ErrorModel("defaultValue", null, "Default Value is required!"));

				//if (!string.IsNullOrWhiteSpace(((PhoneField)field).Format))
				//    errorList.Add(new ErrorModel("format", null, "Format is required!"));

				//if (!((PhoneField)field).MaxLength.HasValue)
				//    errorList.Add(new ErrorModel("maxLength", null, "Max Length is required!"));

				//TODO: parse format and check if it is valid
			}
			else if (field is InputSelectField)
			{
				if (field.Required.HasValue && field.Required.Value && string.IsNullOrWhiteSpace(((InputSelectField)field).DefaultValue))
					errorList.Add(new ErrorModel("defaultValue", null, "Default Value is required!"));

				if (((InputSelectField)field).Options != null)
				{
					if (((InputSelectField)field).Options.Count == 0)
						errorList.Add(new ErrorModel("options", null, "Options must contains at least one item!"));
				}
				else
					errorList.Add(new ErrorModel("options", null, "Options is required!"));
			}
			else if (field is InputTextField)
			{
				if (field.Required.HasValue && field.Required.Value && ((InputTextField)field).DefaultValue == null)
					errorList.Add(new ErrorModel("defaultValue", null, "Default Value is required!"));

				//if (!((TextField)field).MaxLength.HasValue)
				//    errorList.Add(new ErrorModel("maxLength", null, "Max Length is required!"));
			}
			else if (field is InputUrlField)
			{
				if (field.Required.HasValue && field.Required.Value && ((InputUrlField)field).DefaultValue == null)
					errorList.Add(new ErrorModel("defaultValue", null, "Default Value is required!"));

				//if (!((UrlField)field).MaxLength.HasValue)
				//    errorList.Add(new ErrorModel("maxLength", null, "Max Length is required!"));

				if (!((InputUrlField)field).OpenTargetInNewWindow.HasValue)
					((InputUrlField)field).OpenTargetInNewWindow = false;
				//errorList.Add(new ErrorModel("openTargetInNewWindow", null, "Open Target In New Window is required!"));
			}

			return errorList;
		}

		private List<ErrorModel> ValidateRecordLists(Guid entityId, List<InputRecordList> recordLists, bool checkId = true)
		{
			List<ErrorModel> errorList = new List<ErrorModel>();

			Entity entity = ReadEntity(entityId).Object;

			foreach (var recordList in recordLists)
			{
				errorList.AddRange(ValidateRecordList(entity, recordList, checkId));
			}

			return errorList;
		}

		private List<ErrorModel> ValidateRecordList(Entity entity, InputRecordList recordlist, bool checkId = true)
		{
			List<ErrorModel> errorList = new List<ErrorModel>();

			List<Entity> entities = ReadEntities().Object;

			//EntityRelationManager relationManager = new EntityRelationManager(Storage);
			//EntityRelationListResponse relationListResponse = relationManager.Read();
			List<EntityRelation> relationList = new EntityRelationManager().Read().Object;
			//if (relationListResponse.Object != null)
			//	relationList = relationListResponse.Object;

			//List<RecordList> recordLists = new List<RecordList>();
			//List<RecordView> recordViews = new List<RecordView>();
			//List<Field> fields = new List<Field>();

			//foreach (var ent in entities)
			//{
			//	recordLists.AddRange(ent.RecordLists);
			//	recordViews.AddRange(ent.RecordViews);
			//	fields.AddRange(ent.Fields);
			//}

			if (!recordlist.Id.HasValue || recordlist.Id.Value == Guid.Empty)
				errorList.Add(new ErrorModel("id", null, "Id is required!"));

			if (checkId)
			{
				int listSameIdCount = entity.RecordLists.Where(f => f.Id == recordlist.Id).Count();

				if (listSameIdCount > 0)
					errorList.Add(new ErrorModel("id", null, "There is already a list with such Id!"));

				int listSameNameCount = entity.RecordLists.Where(f => f.Name == recordlist.Name).Count();

				if (listSameNameCount > 0)
					errorList.Add(new ErrorModel("name", null, "There is already a list with such Name!"));
			}

			errorList.AddRange(ValidationUtility.ValidateName(recordlist.Name));

			errorList.AddRange(ValidationUtility.ValidateLabel(recordlist.Label));

			if (!recordlist.Default.HasValue)
				recordlist.Default = false;
			if (!recordlist.System.HasValue)
				recordlist.System = false;
			if (!recordlist.Weight.HasValue)
				recordlist.Weight = 10;
			if (!recordlist.PageSize.HasValue)
				recordlist.PageSize = 10;
			if (recordlist.CssClass != null)
				recordlist.CssClass = recordlist.CssClass.Trim();

			if (recordlist.Type != null)
			{
				RecordListType type;
				if (!Enum.TryParse<RecordListType>(recordlist.Type, true, out type))
					errorList.Add(new ErrorModel("type", recordlist.Type, "There is no such type!"));
			}
			else
				errorList.Add(new ErrorModel("type", recordlist.Type, "Type is required!"));

			if (recordlist.Columns != null && recordlist.Columns.Count > 0)
			{
				foreach (var column in recordlist.Columns)
				{
					if (column is InputRecordListFieldItem)
					{
						InputRecordListFieldItem inputColumn = (InputRecordListFieldItem)column;
						if (string.IsNullOrWhiteSpace(((InputRecordListFieldItem)column).FieldName) && ((InputRecordListFieldItem)column).FieldId == null)
						{
							errorList.Add(new ErrorModel("columns.fieldName", null, "Field name or id is required!"));
						}
						else
						{
							if (((InputRecordListFieldItem)column).FieldId == null)
							{
								if (recordlist.Columns.Where(i => i is InputRecordListFieldItem && ((InputRecordListFieldItem)i).FieldName == inputColumn.FieldName).Count() > 1)
									errorList.Add(new ErrorModel("columns.fieldName", null, "There is already an item with such field name!"));

								if (!entity.Fields.Any(f => f.Name == inputColumn.FieldName))
									errorList.Add(new ErrorModel("columns.fieldName", null, "Wrong name. There is no field with such name!"));
								else
									inputColumn.FieldId = entity.Fields.FirstOrDefault(f => f.Name == inputColumn.FieldName).Id;
							}
							else if (string.IsNullOrWhiteSpace(((InputRecordListFieldItem)column).FieldName))
							{
								if (recordlist.Columns.Where(i => i is InputRecordListFieldItem && ((InputRecordListFieldItem)i).FieldId == inputColumn.FieldId).Count() > 1)
									errorList.Add(new ErrorModel("columns.fieldId", null, "There is already an item with such field identifier!"));

								if (!entity.Fields.Any(f => f.Id == inputColumn.FieldId.Value))
									errorList.Add(new ErrorModel("columns.fieldId", null, "Wrong id. There is no field with such id!"));
								else
									inputColumn.FieldName = entity.Fields.FirstOrDefault(f => f.Id == inputColumn.FieldId).Name;
							}
							else
							{
								//TODO validate if id does not fit the name

								if (recordlist.Columns.Where(i => i is InputRecordListFieldItem && ((InputRecordListFieldItem)i).FieldId == inputColumn.FieldId).Count() > 1)
									errorList.Add(new ErrorModel("columns.fieldId", null, "There is already an item with such field identifier!"));

								if (!entity.Fields.Any(f => f.Id == inputColumn.FieldId.Value))
									errorList.Add(new ErrorModel("columns.fieldId", null, "Wrong id. There is no field with such id!"));

							}
						}

						inputColumn.EntityId = entity.Id;
						inputColumn.EntityName = entity.Name;
					}
					else if (column is InputRecordListListItem)
					{
						InputRecordListListItem inputColumn = (InputRecordListListItem)column;
						if (string.IsNullOrWhiteSpace(inputColumn.ListName) && inputColumn.ListId == null)
						{
							errorList.Add(new ErrorModel("columns.listName", null, "List name or id is required!"));
						}
						else
						{
							if (inputColumn.ListId == null)
							{
								if (recordlist.Columns.Where(i => i is InputRecordListListItem && ((InputRecordListListItem)i).ListName == inputColumn.ListName).Count() > 1)
									errorList.Add(new ErrorModel("columns.listName", null, "There is already an item with such list name!"));

								if (!entity.RecordLists.Any(f => f.Name == inputColumn.ListName))
									errorList.Add(new ErrorModel("columns.listName", null, "Wrong name. There is no list with such name!"));
								else
									inputColumn.ListId = entity.RecordLists.FirstOrDefault(l => l.Name == inputColumn.ListName).Id;
							}
							else if (string.IsNullOrWhiteSpace(inputColumn.ListName))
							{
								if (recordlist.Columns.Where(i => i is InputRecordListListItem && ((InputRecordListListItem)i).ListId == inputColumn.ListId).Count() > 1)
									errorList.Add(new ErrorModel("columns.listId", null, "There is already an item with sane id!"));

								if (!entity.RecordLists.Any(f => f.Id == inputColumn.ListId))
									errorList.Add(new ErrorModel("columns.listId", null, "Wrong list id. There is no list with such id!"));
								else
									inputColumn.ListName = entity.RecordLists.FirstOrDefault(l => l.Id == inputColumn.ListId).Name;
							}
							else
							{
								//TODO validate if id does not fit the name

								if (recordlist.Columns.Where(i => i is InputRecordListListItem && ((InputRecordListListItem)i).ListId == inputColumn.ListId).Count() > 1)
									errorList.Add(new ErrorModel("columns.listId", null, "There is already an item with sane id!"));

								if (!entity.RecordLists.Any(f => f.Id == inputColumn.ListId))
									errorList.Add(new ErrorModel("columns.listId", null, "Wrong list id. There is no list with such id!"));
							}

						}

						inputColumn.EntityId = entity.Id;
						inputColumn.EntityName = entity.Name;
					}
					else if (column is InputRecordListViewItem)
					{
						InputRecordListViewItem inputColumn = (InputRecordListViewItem)column;
						if (string.IsNullOrWhiteSpace(inputColumn.ViewName) && inputColumn.ViewId == null)
						{
							errorList.Add(new ErrorModel("columns.viewName", null, "View name or id is required!"));
						}
						else
						{
							if (inputColumn.ViewId == null)
							{
								if (recordlist.Columns.Where(i => i is InputRecordListViewItem && ((InputRecordListViewItem)i).ViewName == inputColumn.ViewName).Count() > 1)
									errorList.Add(new ErrorModel("columns.viewName", null, "There is already an item with such view name!"));

								if (!entity.RecordViews.Any(f => f.Name == inputColumn.ViewName))
									errorList.Add(new ErrorModel("columns.viewName", null, "Wrong name. There is no view with such name!"));
								else
									inputColumn.ViewId = entity.RecordViews.FirstOrDefault(v => v.Name == inputColumn.ViewName).Id;
							}
							else if (string.IsNullOrWhiteSpace(inputColumn.ViewName))
							{
								if (recordlist.Columns.Where(i => i is InputRecordListViewItem && ((InputRecordListViewItem)i).ViewId == inputColumn.ViewId).Count() > 1)
									errorList.Add(new ErrorModel("columns.viewId", null, "There is already an item with such view id!"));

								if (!entity.RecordViews.Any(f => f.Id == inputColumn.ViewId))
									errorList.Add(new ErrorModel("columns.viewId", null, "Wrong id. There is no view with such id!"));
								else
									inputColumn.ViewName = entity.RecordViews.FirstOrDefault(v => v.Id == inputColumn.ViewId).Name;
							}
							else
							{
								//TODO validate if id does not fit the name

								if (recordlist.Columns.Where(i => i is InputRecordListViewItem && ((InputRecordListViewItem)i).ViewId == inputColumn.ViewId).Count() > 1)
									errorList.Add(new ErrorModel("columns.viewId", null, "There is already an item with such view id!"));

								if (!entity.RecordViews.Any(f => f.Id == inputColumn.ViewId))
									errorList.Add(new ErrorModel("columns.viewId", null, "Wrong id. There is no view with such id!"));
							}
						}

						inputColumn.EntityId = entity.Id;
						inputColumn.EntityName = entity.Name;
					}
					else if (column is InputRecordListRelationFieldItem)
					{
						InputRecordListRelationFieldItem inputColumn = (InputRecordListRelationFieldItem)column;
						if (string.IsNullOrWhiteSpace(inputColumn.RelationName) && inputColumn.RelationId == null)
						{
							errorList.Add(new ErrorModel("columns.relationName", null, "Relation name or id is required!"));
						}
						else
						{
							EntityRelation relation = null;
							if (string.IsNullOrWhiteSpace(inputColumn.RelationName))
								relation = relationList.SingleOrDefault(x => x.Id == inputColumn.RelationId);
							else
								relation = relationList.SingleOrDefault(x => x.Name == inputColumn.RelationName);

							if (relation == null)
								errorList.Add(new ErrorModel("columns.relationName", null, "Wrong name. There is no relation with such name!"));
							else
							{
								inputColumn.RelationName = relation.Name;
								inputColumn.RelationId = relation.Id;
							}
						}

						if (string.IsNullOrWhiteSpace(inputColumn.FieldName) && inputColumn.FieldId == null)
						{
							errorList.Add(new ErrorModel("columns.fieldName", null, "Field name or id is required!"));
						}
						else if (inputColumn.RelationId.HasValue && inputColumn.RelationId != Guid.Empty)
						{
							if ((string.IsNullOrWhiteSpace(inputColumn.FieldName) &&
								recordlist.Columns.Where(i => i is InputRecordListRelationFieldItem &&
								((InputRecordListRelationFieldItem)i).FieldId == inputColumn.FieldId &&
								((InputRecordListRelationFieldItem)i).RelationId == inputColumn.RelationId).Count() > 1)
								||
								(!string.IsNullOrWhiteSpace(inputColumn.FieldName) &&
								recordlist.Columns.Where(i => i is InputRecordListRelationFieldItem &&
								((InputRecordListRelationFieldItem)i).FieldName == inputColumn.FieldName &&
								((InputRecordListRelationFieldItem)i).RelationId == inputColumn.RelationId).Count() > 1))

								errorList.Add(new ErrorModel("columns.fieldName", null, "There is already an item with such field name!"));
							else
							{
								EntityRelation relation = relationList.FirstOrDefault(r => r.Id == inputColumn.RelationId.Value);

								if (relation != null)
								{
									Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
									Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);

									if (relEntity != null)
									{
										inputColumn.EntityId = relEntity.Id;
										inputColumn.EntityName = relEntity.Name;

										Field relField = string.IsNullOrWhiteSpace(inputColumn.FieldName) ?
											relEntity.Fields.FirstOrDefault(f => f.Id == inputColumn.FieldId) :
											relEntity.Fields.FirstOrDefault(f => f.Name == inputColumn.FieldName);

										if (relField != null)
										{
											inputColumn.FieldName = relField.Name;
											inputColumn.FieldId = relField.Id;
										}
										else
											errorList.Add(new ErrorModel("columns.fieldName", null, "Wrong name. There is no field with such name!"));
									}
								}
							}
						}
					}
					else if (column is InputRecordListRelationTreeItem)
					{
						EntityRelation relation = null;
						InputRecordListRelationTreeItem inputColumn = (InputRecordListRelationTreeItem)column;
						if (string.IsNullOrWhiteSpace(inputColumn.RelationName) && inputColumn.RelationId == null)
						{
							errorList.Add(new ErrorModel("columns.relationName", null, "Relation name or id is required!"));
						}
						else
						{
							if (string.IsNullOrWhiteSpace(inputColumn.RelationName))
								relation = relationList.SingleOrDefault(x => x.Id == inputColumn.RelationId);
							else
								relation = relationList.SingleOrDefault(x => x.Name == inputColumn.RelationName);

							if (relation == null)
								errorList.Add(new ErrorModel("columns.relationName", null, "Wrong name. There is no relation with such name!"));
							else
							{
								inputColumn.RelationName = relation.Name;
								inputColumn.RelationId = relation.Id;
							}
						}

						if (string.IsNullOrWhiteSpace(inputColumn.TreeName) && inputColumn.TreeId == null)
						{
							errorList.Add(new ErrorModel("columns.listName", null, "Tree name or id is required!"));
						}
						else if (inputColumn.RelationId.HasValue && inputColumn.RelationId != Guid.Empty)
						{
							if (recordlist.Columns.Where(i => i is InputRecordListRelationTreeItem &&
							   ((InputRecordListRelationTreeItem)i).RelationId == inputColumn.RelationId &&
							   ((InputRecordListRelationTreeItem)i).TreeName == inputColumn.TreeName).Count() > 1)
								errorList.Add(new ErrorModel("columns.listName", null, "There is already an item with such tree name!"));
							else
							{
								if (relation != null)
								{

									Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
									Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);

									if (relEntity != null)
									{
										inputColumn.EntityId = relEntity.Id;
										inputColumn.EntityName = relEntity.Name;

										RecordTree relTree = string.IsNullOrWhiteSpace(inputColumn.TreeName) ?
											relEntity.RecordTrees.FirstOrDefault(l => l.Id == inputColumn.TreeId) :
											relEntity.RecordTrees.FirstOrDefault(l => l.Name == inputColumn.TreeName);

										if (relTree != null)
										{
											inputColumn.TreeName = relTree.Name;
											inputColumn.TreeId = relTree.Id;
										}
										else
											errorList.Add(new ErrorModel("columns.listId", null, "Wrong Id. There is no tree with such id or name!"));
									}
								}
							}
						}
					}
					else if (column is InputRecordListRelationListItem)
					{
						EntityRelation relation = null;
						InputRecordListRelationListItem inputColumn = (InputRecordListRelationListItem)column;
						if (string.IsNullOrWhiteSpace(inputColumn.RelationName) && inputColumn.RelationId == null)
						{
							errorList.Add(new ErrorModel("columns.relationName", null, "Relation name or id is required!"));
						}
						else
						{
							if (string.IsNullOrWhiteSpace(inputColumn.RelationName))
								relation = relationList.SingleOrDefault(x => x.Id == inputColumn.RelationId);
							else
								relation = relationList.SingleOrDefault(x => x.Name == inputColumn.RelationName);

							if (relation == null)
								errorList.Add(new ErrorModel("columns.relationName", null, "Wrong name. There is no relation with such name!"));
							else
							{
								inputColumn.RelationName = relation.Name;
								inputColumn.RelationId = relation.Id;
							}
						}

						if (string.IsNullOrWhiteSpace(inputColumn.ListName) && inputColumn.ListId == null)
						{
							errorList.Add(new ErrorModel("columns.listName", null, "List name or id is required!"));
						}
						else if (inputColumn.RelationId.HasValue && inputColumn.RelationId != Guid.Empty)
						{
							if (recordlist.Columns.Where(i => i is InputRecordListRelationListItem &&
							   ((InputRecordListRelationListItem)i).RelationId == inputColumn.RelationId &&
							   ((InputRecordListRelationListItem)i).ListName == inputColumn.ListName).Count() > 1)
								errorList.Add(new ErrorModel("columns.listName", null, "There is already an item with such list name!"));
							else
							{
								if (relation != null)
								{

									Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
									Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);

									if (relEntity != null)
									{
										inputColumn.EntityId = relEntity.Id;
										inputColumn.EntityName = relEntity.Name;

										RecordList relList = string.IsNullOrWhiteSpace(inputColumn.ListName) ?
											relEntity.RecordLists.FirstOrDefault(l => l.Id == inputColumn.ListId) :
											relEntity.RecordLists.FirstOrDefault(l => l.Name == inputColumn.ListName);

										if (relList != null)
										{
											inputColumn.ListName = relList.Name;
											inputColumn.ListId = relList.Id;
										}
										else
											errorList.Add(new ErrorModel("columns.listId", null, "Wrong Id. There is no list with such id or name!"));
									}
								}
							}
						}
					}
					else if (column is InputRecordListRelationViewItem)
					{
						InputRecordListRelationViewItem inputColumn = (InputRecordListRelationViewItem)column;
						EntityRelation relation = null;
						if (string.IsNullOrWhiteSpace(inputColumn.RelationName) && inputColumn.RelationId == null)
						{
							errorList.Add(new ErrorModel("columns.relationName", null, "Relation name or id is required!"));
						}
						else
						{
							if (string.IsNullOrWhiteSpace(inputColumn.RelationName))
								relation = relationList.SingleOrDefault(x => x.Id == inputColumn.RelationId);
							else
								relation = relationList.SingleOrDefault(x => x.Name == inputColumn.RelationName);

							if (relation == null)
								errorList.Add(new ErrorModel("columns.relationName", null, "Wrong name. There is no relation with such name!"));
							else
							{
								inputColumn.RelationName = relation.Name;
								inputColumn.RelationId = relation.Id;
							}
						}


						if (string.IsNullOrWhiteSpace(inputColumn.ViewName) && inputColumn.ViewId == null)
						{
							errorList.Add(new ErrorModel("columns.viewName", null, "View name or id is required!"));
						}
						else if (inputColumn.RelationId.HasValue && inputColumn.RelationId != Guid.Empty)
						{
							if (recordlist.Columns.Where(i => i is InputRecordListRelationViewItem &&
								((InputRecordListRelationViewItem)i).RelationId == inputColumn.RelationId &&
								((InputRecordListRelationViewItem)i).ViewName == inputColumn.ViewName).Count() > 1)
								errorList.Add(new ErrorModel("columns.viewName", null, "There is already an item with such view name!"));
							else
							{
								if (relation != null)
								{
									Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
									Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);

									if (relEntity != null)
									{
										inputColumn.EntityId = relEntity.Id;
										inputColumn.EntityName = relEntity.Name;

										RecordView relView = string.IsNullOrWhiteSpace(inputColumn.ViewName) ?
											relEntity.RecordViews.FirstOrDefault(v => v.Id == inputColumn.ViewId) :
											relEntity.RecordViews.FirstOrDefault(v => v.Name == inputColumn.ViewName);

										if (relView != null)
										{
											inputColumn.ViewId = relView.Id;
											inputColumn.ViewName = relView.Name;
										}
										else
											errorList.Add(new ErrorModel("columns.viewName", null, "Wrong name. There is no view with such name!"));
									}
								}
							}
						}
					}
				}
			}

			if (recordlist.Query != null)
			{
				List<ErrorModel> queryErrors = ValidateRecordListQuery(recordlist.Query);
				errorList.AddRange(queryErrors);
			}

			if (recordlist.Sorts != null)
			{
				foreach (var sort in recordlist.Sorts)
				{
					if (string.IsNullOrWhiteSpace(sort.FieldName))
						errorList.Add(new ErrorModel("sorts.fieldName", sort.FieldName, "FieldName is required!"));

					if (string.IsNullOrWhiteSpace(sort.SortType))
						errorList.Add(new ErrorModel("sorts.sortType", sort.SortType, "SortType is required!"));
					else
					{
						QuerySortType sortType;
						if (!Enum.TryParse<QuerySortType>(sort.SortType, true, out sortType))
							errorList.Add(new ErrorModel("sorts.sortType", sort.SortType, "There is no such sort type!"));
					}
				}
			}

			return errorList;
		}

		private List<ErrorModel> ValidateRecordListQuery(InputRecordListQuery query)
		{
			List<ErrorModel> errorList = new List<ErrorModel>();

			if (string.IsNullOrWhiteSpace(query.QueryType))
				errorList.Add(new ErrorModel("query.queryType", query.QueryType, "QueryType is required!"));
			else
			{
				QueryType queryType;
				if (!Enum.TryParse<QueryType>(query.QueryType, true, out queryType))
					errorList.Add(new ErrorModel("query.queryType", query.QueryType, "There is no such query type!"));
				else
				{
					if (queryType != QueryType.AND && queryType != QueryType.OR && string.IsNullOrWhiteSpace(query.FieldName))
						errorList.Add(new ErrorModel("query.fieldName", query.FieldName, "FieldName is required!"));

					if (queryType != QueryType.AND && queryType != QueryType.OR && query.FieldValue == null)
						errorList.Add(new ErrorModel("query.fieldValue", query.FieldValue, "FieldValue is required!"));

					if ((queryType == QueryType.AND || queryType == QueryType.OR) && (query.SubQueries == null || query.SubQueries.Count == 0))
						errorList.Add(new ErrorModel("query.subQueries", null, "SubQueries must have at least one item!"));

					if (query.SubQueries != null && query.SubQueries.Count > 0)
					{
						foreach (var subQuery in query.SubQueries)
						{
							List<ErrorModel> subQueryErrors = ValidateRecordListQuery(subQuery);
							errorList.AddRange(subQueryErrors);
						}
					}
				}
			}

			return errorList;
		}

		private List<ErrorModel> ValidateRecordViews(Guid entityId, List<InputRecordView> recordViewList, bool checkId = true)
		{
			List<ErrorModel> errorList = new List<ErrorModel>();

			Entity entity = ReadEntity(entityId).Object;

			foreach (var recordView in recordViewList)
			{
				errorList.AddRange(ValidateRecordView(entity, recordView, checkId));
			}

			return errorList;
		}

		private List<ErrorModel> ValidateRecordView(Entity entity, InputRecordView recordView, bool checkId = true)
		{
			List<ErrorModel> errorList = new List<ErrorModel>();

			List<Entity> entities = ReadEntities().Object;

			//EntityRelationManager relationManager = new EntityRelationManager(Storage);
			//EntityRelationListResponse relationListResponse = relationManager.Read();
			List<EntityRelation> relationList = new EntityRelationManager().Read().Object;
			//if (relationListResponse.Object != null)
			//	relationList = relationListResponse.Object;

			if (!recordView.Id.HasValue || recordView.Id == Guid.Empty)
				errorList.Add(new ErrorModel("id", null, "Id is required!"));

			if (checkId)
			{
				int viewSameIdCount = entity.RecordViews.Where(f => f.Id == recordView.Id).Count();

				if (viewSameIdCount > 0)
					errorList.Add(new ErrorModel("id", null, "There is already a view with such Id!"));

				int viewSameNameCount = entity.RecordViews.Where(f => f.Name == recordView.Name).Count();

				if (viewSameNameCount > 0)
					errorList.Add(new ErrorModel("name", null, "There is already a view with such Name!"));
			}

			errorList.AddRange(ValidationUtility.ValidateViewName(recordView.Name));

			errorList.AddRange(ValidationUtility.ValidateLabel(recordView.Label));

			if (!recordView.Weight.HasValue)
				recordView.Weight = 1;

			if (!recordView.Default.HasValue)
				recordView.Default = false;

			if (!recordView.System.HasValue)
				recordView.System = false;

			if (string.IsNullOrWhiteSpace(recordView.Type))
				errorList.Add(new ErrorModel("type", null, "Type is required!"));
			else
			{
				RecordViewType type;
				if (!Enum.TryParse(recordView.Type, true, out type))
					errorList.Add(new ErrorModel("type", recordView.Type, "Type is not valid!"));
			}

			if (recordView.Regions != null && recordView.Regions.Count > 0)
			{
				foreach (var region in recordView.Regions)
				{
					if (string.IsNullOrWhiteSpace(region.Name))
						errorList.Add(new ErrorModel("regions.name", region.Name, "Name is required!"));
					else
					{
						if (recordView.Regions.Where(r => r.Name == region.Name).Count() > 1)
							errorList.Add(new ErrorModel("regions.name", region.Name, "There is already region with such name!"));

						errorList.AddRange(ValidationUtility.ValidateName(region.Name, key: "regions.name"));
					}

					if (!region.Render.HasValue)
						region.Render = false;

					if (region.Sections != null && region.Sections.Count > 0)
					{
						foreach (var section in region.Sections)
						{
							if (!section.Id.HasValue || section.Id == Guid.Empty)
							{
								errorList.Add(new ErrorModel("regions.sections.id", null, "Id is required!"));
							}
							else
							{
								if (region.Sections.Where(s => s.Id == section.Id).Count() > 1)
									errorList.Add(new ErrorModel("regions.sections.id", null, "There is already a section with such Id!"));
							}

							if (string.IsNullOrWhiteSpace(section.Name))
							{
								errorList.Add(new ErrorModel("regions.sections.name", region.Name, "Name is required!"));
							}
							else
							{
								if (region.Sections.Where(s => s.Name == section.Name).Count() > 1)
									errorList.Add(new ErrorModel("regions.sections.name", section.Name, "There is already section with such name!"));

								errorList.AddRange(ValidationUtility.ValidateName(section.Name, key: "regions.sections.name"));
							}

							errorList.AddRange(ValidationUtility.ValidateLabel(section.Label, key: "regions.sections.label"));

							if (!section.ShowLabel.HasValue)
								section.ShowLabel = false;

							if (!section.Collapsed.HasValue)
								section.Collapsed = false;

							if (!section.Weight.HasValue)
								section.Weight = 1;

							if (section.Rows != null && section.Rows.Count > 0)
							{
								foreach (var row in section.Rows)
								{
									if (!row.Id.HasValue || row.Id == Guid.Empty)
									{
										errorList.Add(new ErrorModel("regions.sections.rows.id", null, "Id is required!"));
									}
									else
									{
										if (section.Rows.Where(r => r.Id == row.Id).Count() > 1)
											errorList.Add(new ErrorModel("regions.sections.rows.id", null, "There is already a row with such Id!"));
									}

									if (!row.Weight.HasValue)
										row.Weight = 1;

									if (row.Columns != null && row.Columns.Count > 0)
									{
										foreach (var column in row.Columns)
										{
											if (column.Items != null && column.Items.Count > 0)
											{
												foreach (var item in column.Items)
												{
													if (item is InputRecordViewFieldItem)
													{
														InputRecordViewFieldItem inputItem = (InputRecordViewFieldItem)item;
														if (string.IsNullOrWhiteSpace(inputItem.FieldName) && inputItem.FieldId == null)
															errorList.Add(new ErrorModel("regions.sections.rows.columns.items.fieldName", null, "Filed name and id are missing!"));
														else
														{
															if (inputItem.FieldId == null)
															{
																if (column.Items.Where(i => i is InputRecordViewFieldItem && ((InputRecordViewFieldItem)i).FieldName == inputItem.FieldName).Count() > 1)
																	errorList.Add(new ErrorModel("regions.sections.rows.columns.items.fieldName", null, "There is already an item with such field name!"));

																if (!entity.Fields.Any(f => f.Name == inputItem.FieldName))
																	errorList.Add(new ErrorModel("regions.sections.rows.columns.items.fieldName", null, "Wrong Name. There is no field with such name!"));
																else
																	inputItem.FieldId = entity.Fields.FirstOrDefault(f => f.Name == inputItem.FieldName).Id;
															}
															else if (string.IsNullOrWhiteSpace(inputItem.FieldName))
															{
																if (column.Items.Where(i => i is InputRecordViewFieldItem && ((InputRecordViewFieldItem)i).FieldId == inputItem.FieldId).Count() > 1)
																	errorList.Add(new ErrorModel("regions.sections.rows.columns.items.fieldId", null, "There is already an item with such field id!"));

																if (!entity.Fields.Any(f => f.Id == inputItem.FieldId))
																	errorList.Add(new ErrorModel("regions.sections.rows.columns.items.fieldId", null, "Wrong Id. There is no field with such id!"));
																else
																	inputItem.FieldName = entity.Fields.FirstOrDefault(f => f.Id == inputItem.FieldId).Name;
															}
														}

														inputItem.EntityId = entity.Id;
														inputItem.EntityName = entity.Name;
													}
													else if (item is InputRecordViewListItem)
													{
														InputRecordViewListItem inputItem = (InputRecordViewListItem)item;
														if (string.IsNullOrWhiteSpace(inputItem.ListName) && inputItem.ListId == null)
															errorList.Add(new ErrorModel("regions.sections.rows.columns.items.listName", null, "List name and id are missing!"));
														else
														{
															if (inputItem.ListId == null)
															{
																if (column.Items.Where(i => i is InputRecordViewListItem && ((InputRecordViewListItem)i).ListName == inputItem.ListName).Count() > 1)
																	errorList.Add(new ErrorModel("regions.sections.rows.columns.items.listName", null, "There is already an item with such list name!"));

																if (!entity.RecordLists.Any(l => l.Name == inputItem.ListName))
																	errorList.Add(new ErrorModel("regions.sections.rows.columns.items.listName", null, "Wrong name. There is no list with such name!"));
																else
																	inputItem.ListId = entity.RecordLists.FirstOrDefault(l => l.Name == inputItem.ListName).Id;
															}
															else if (string.IsNullOrWhiteSpace(inputItem.ListName))
															{
																if (column.Items.Where(i => i is InputRecordViewListItem && ((InputRecordViewListItem)i).ListId == inputItem.ListId).Count() > 1)
																	errorList.Add(new ErrorModel("regions.sections.rows.columns.items.listName", null, "There is already an item with such list id!"));

																if (!entity.RecordLists.Any(l => l.Id == inputItem.ListId))
																	errorList.Add(new ErrorModel("regions.sections.rows.columns.items.listName", null, "Wrong id. There is no list with such id!"));
																else

																	inputItem.ListName = entity.RecordLists.FirstOrDefault(l => l.Id == inputItem.ListId).Name;
															}
														}

														inputItem.EntityId = entity.Id;
														inputItem.EntityName = entity.Name;
													}
													else if (item is InputRecordViewViewItem)
													{
														InputRecordViewViewItem inputItem = (InputRecordViewViewItem)item;
														if (string.IsNullOrWhiteSpace(inputItem.ViewName) && inputItem.ViewId == null)
															errorList.Add(new ErrorModel("regions.sections.rows.columns.items.viewName", null, "View name and id are missing!"));
														else
														{
															if (inputItem.ViewId == null)
															{
																if (column.Items.Where(i => i is InputRecordViewViewItem && ((InputRecordViewViewItem)i).ViewName == inputItem.ViewName).Count() > 1)
																	errorList.Add(new ErrorModel("regions.sections.rows.columns.items.viewName", null, "There is already an item with such view name!"));

																if (!entity.RecordViews.Any(v => v.Name == inputItem.ViewName))
																	errorList.Add(new ErrorModel("regions.sections.rows.columns.items.viewName", null, "Wrong name. There is no view with such name!"));
																else
																	inputItem.ViewId = entity.RecordViews.FirstOrDefault(v => v.Name == inputItem.ViewName).Id;
															}
															else if (string.IsNullOrWhiteSpace(inputItem.ViewName))
															{
																if (column.Items.Where(i => i is InputRecordViewViewItem && ((InputRecordViewViewItem)i).ViewId == inputItem.ViewId).Count() > 1)
																	errorList.Add(new ErrorModel("regions.sections.rows.columns.items.viewName", null, "There is already an item with such view id!"));

																if (!entity.RecordViews.Any(v => v.Id == inputItem.ViewId))
																	errorList.Add(new ErrorModel("regions.sections.rows.columns.items.viewName", null, "Wrong id. There is no view with such id!"));
																else
																	inputItem.ViewName = entity.RecordViews.FirstOrDefault(v => v.Id == inputItem.ViewId).Name;
															}
														}

														inputItem.EntityId = entity.Id;
														inputItem.EntityName = entity.Name;
													}
													else if (item is InputRecordViewRelationFieldItem)
													{
														EntityRelation relation = null;

														InputRecordViewRelationFieldItem inputItem = (InputRecordViewRelationFieldItem)item;
														if (string.IsNullOrWhiteSpace(inputItem.RelationName) && inputItem.RelationId == null)
														{
															errorList.Add(new ErrorModel("regions.sections.rows.columns.items.relationName", null, "Relation name or id is required!"));
														}
														else
														{
															if (inputItem.RelationId != null)
																relation = relationList.FirstOrDefault(r => r.Id == inputItem.RelationId);
															else
																relation = relationList.FirstOrDefault(r => r.Name == inputItem.RelationName);

															if (relation == null)
																errorList.Add(new ErrorModel("regions.sections.rows.columns.items.relationName", null, "Wrong name or id. There is no relation with such name or id!"));
															else
															{
																inputItem.RelationId = relation.Id;
																inputItem.RelationName = relation.Name;
															}
														}

														if (string.IsNullOrWhiteSpace(inputItem.FieldName) && inputItem.FieldId == null)
														{
															errorList.Add(new ErrorModel("regions.sections.rows.columns.items.fieldName", null, "Field name or id is required!"));
														}
														else if (inputItem.RelationId.HasValue && inputItem.RelationId != Guid.Empty)
														{

															bool foundMoreThanOneTime = false;
															if (inputItem.FieldId == null)
																foundMoreThanOneTime = column.Items.Where(i => i is InputRecordViewRelationFieldItem
																					&& ((InputRecordViewRelationFieldItem)i).FieldName == inputItem.FieldName
																					&& ((InputRecordViewRelationFieldItem)i).RelationId == inputItem.RelationId).Count() > 1;
															else
																foundMoreThanOneTime = column.Items.Where(i => i is InputRecordViewRelationFieldItem
																				&& ((InputRecordViewRelationFieldItem)i).FieldId == inputItem.FieldId
																				&& ((InputRecordViewRelationFieldItem)i).RelationId == inputItem.RelationId).Count() > 1;

															if (foundMoreThanOneTime)
																errorList.Add(new ErrorModel("regions.sections.rows.columns.items.fieldName", null, "There is already an item with such field name or id!"));


															if (relation != null)
															{
																Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
																Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);

																if (relEntity != null)
																{
																	inputItem.EntityId = relEntity.Id;
																	inputItem.EntityName = relEntity.Name;


																	Field relField = null;
																	if (!string.IsNullOrWhiteSpace(inputItem.FieldName))
																		relField = relEntity.Fields.FirstOrDefault(f => f.Name == inputItem.FieldName);
																	else
																		relField = relEntity.Fields.FirstOrDefault(f => f.Id == inputItem.FieldId);

																	if (relField == null)
																		errorList.Add(new ErrorModel("regions.sections.rows.columns.items.fieldName", null, "Wrong name. There is no field with such name!"));
																	else
																	{
																		inputItem.FieldId = relField.Id;
																		inputItem.FieldName = relField.Name;
																	}
																}
															}
														}

													}
													else if (item is InputRecordViewRelationTreeItem)
													{
														EntityRelation relation = null;
														InputRecordViewRelationTreeItem inputItem = (InputRecordViewRelationTreeItem)item;
														if (string.IsNullOrWhiteSpace(inputItem.RelationName) && inputItem.RelationId == null)
														{
															errorList.Add(new ErrorModel("regions.sections.rows.columns.items.relationName", null, "Relation name or id is required!"));
														}
														else
														{
															if (string.IsNullOrWhiteSpace(inputItem.RelationName))
																relation = relationList.SingleOrDefault(r => r.Id == inputItem.RelationId);
															else
																relation = relationList.SingleOrDefault(r => r.Name == inputItem.RelationName);

															if (relation == null)
																errorList.Add(new ErrorModel("regions.sections.rows.columns.items.relationName", null, "Wrong name or id. There is no relation with such name or id!"));
															else
															{
																inputItem.RelationId = relation.Id;
																inputItem.RelationName = relation.Name;
															}
														}

														if (string.IsNullOrWhiteSpace(inputItem.TreeName) && inputItem.TreeId == null)
														{
															errorList.Add(new ErrorModel("regions.sections.rows.columns.items.listName", null, "Tree name or id is required!"));
														}
														else if (inputItem.RelationId.HasValue && inputItem.RelationId != Guid.Empty)
														{
															if (relation != null)
															{
																Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
																Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);

																if (relEntity != null)
																{
																	inputItem.EntityId = relEntity.Id;
																	inputItem.EntityName = relEntity.Name;

																	RecordTree tree = null;
																	if (string.IsNullOrWhiteSpace(inputItem.TreeName))
																		tree = relEntity.RecordTrees.FirstOrDefault(l => l.Id == inputItem.TreeId);
																	else
																		tree = relEntity.RecordTrees.FirstOrDefault(l => l.Name == inputItem.TreeName);

																	if (tree != null)
																	{
																		inputItem.TreeId = tree.Id;
																		inputItem.TreeName = tree.Name;
																	}
																	else
																		errorList.Add(new ErrorModel("regions.sections.rows.columns.items.listName", null, "Wrong Name. There is no tree with such name or id!"));
																}
															}
														}
													}
													else if (item is InputRecordViewRelationListItem)
													{
														EntityRelation relation = null;
														InputRecordViewRelationListItem inputItem = (InputRecordViewRelationListItem)item;
														if (string.IsNullOrWhiteSpace(inputItem.RelationName) && inputItem.RelationId == null)
														{
															errorList.Add(new ErrorModel("regions.sections.rows.columns.items.relationName", null, "Relation name or id is required!"));
														}
														else
														{
															if (string.IsNullOrWhiteSpace(inputItem.RelationName))
																relation = relationList.SingleOrDefault(r => r.Id == inputItem.RelationId);
															else
																relation = relationList.SingleOrDefault(r => r.Name == inputItem.RelationName);

															if (relation == null)
																errorList.Add(new ErrorModel("regions.sections.rows.columns.items.relationName", null, "Wrong name or id. There is no relation with such name or id!"));
															else
															{
																inputItem.RelationId = relation.Id;
																inputItem.RelationName = relation.Name;
															}
														}

														if (string.IsNullOrWhiteSpace(inputItem.ListName) && inputItem.ListId == null)
														{
															errorList.Add(new ErrorModel("regions.sections.rows.columns.items.listName", null, "List name or id is required!"));
														}
														else if (inputItem.RelationId.HasValue && inputItem.RelationId != Guid.Empty)
														{
															if (relation != null)
															{
																Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
																Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);

																if (relEntity != null)
																{
																	inputItem.EntityId = relEntity.Id;
																	inputItem.EntityName = relEntity.Name;

																	RecordList relList = null;
																	if (string.IsNullOrWhiteSpace(inputItem.ListName))
																		relList = relEntity.RecordLists.FirstOrDefault(l => l.Id == inputItem.ListId);
																	else
																		relList = relEntity.RecordLists.FirstOrDefault(l => l.Name == inputItem.ListName);

																	if (relList != null)
																	{
																		inputItem.ListId = relList.Id;
																		inputItem.ListName = relList.Name;
																	}
																	else
																		errorList.Add(new ErrorModel("regions.sections.rows.columns.items.listName", null, "Wrong Name. There is no list with such name or id!"));
																}
															}
														}

													}
													else if (item is InputRecordViewRelationViewItem)
													{
														EntityRelation relation = null;
														InputRecordViewRelationViewItem inputItem = (InputRecordViewRelationViewItem)item;
														if (string.IsNullOrWhiteSpace(inputItem.RelationName) && inputItem.RelationId == null)
														{
															errorList.Add(new ErrorModel("regions.sections.rows.columns.items.relationName", null, "Relation name or id is required!"));
														}
														else
														{
															if (string.IsNullOrWhiteSpace(inputItem.RelationName))
																relation = relationList.SingleOrDefault(r => r.Id == inputItem.RelationId);
															else
																relation = relationList.SingleOrDefault(r => r.Name == inputItem.RelationName);

															if (relation == null)
																errorList.Add(new ErrorModel("regions.sections.rows.columns.items.relationName", null, "Wrong name or. There is no relation with such name or id!"));
															else
																inputItem.RelationId = relation.Id;
														}

														if (string.IsNullOrWhiteSpace(inputItem.ViewName) && inputItem.ViewId == null)
														{
															errorList.Add(new ErrorModel("regions.sections.rows.columns.items.viewName", null, "View name is required!"));
														}
														else if (inputItem.RelationId.HasValue && inputItem.RelationId != Guid.Empty)
														{
															if (relation != null)
															{
																Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
																Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);

																if (relEntity != null)
																{
																	inputItem.EntityId = relEntity.Id;
																	inputItem.EntityName = relEntity.Name;


																	RecordView relView = null;
																	if (string.IsNullOrWhiteSpace(inputItem.ViewName))
																		relView = relEntity.RecordViews.FirstOrDefault(l => l.Id == inputItem.ViewId);
																	else
																		relView = relEntity.RecordViews.FirstOrDefault(l => l.Name == inputItem.ViewName);

																	if (relView != null)
																	{
																		inputItem.ViewId = relView.Id;
																		inputItem.ViewName = relView.Name;
																	}
																	else
																		errorList.Add(new ErrorModel("regions.sections.rows.columns.items.viewName", null, "Wrong name. There is no view with such name!"));
																}
															}

															if (column.Items.Where(i => i is InputRecordViewRelationViewItem && ((InputRecordViewRelationViewItem)i).ViewName == inputItem.ViewName).Count() > 1)
																errorList.Add(new ErrorModel("regions.sections.rows.columns.items.viewName", null, "There is already an item with such view name!"));
														}

													}
													else if (item is InputRecordViewHtmlItem)
													{
														((InputRecordViewHtmlItem)item).Tag = ((InputRecordViewHtmlItem)item).Tag.Trim();
														((InputRecordViewHtmlItem)item).Content = ((InputRecordViewHtmlItem)item).Content.Trim();
													}
												}
											}
										}
									}
								}
							}

						}
					}
				}
			}

			if (recordView.Sidebar != null)
			{
				if (recordView.Sidebar.Items != null && recordView.Sidebar.Items.Count > 0)
				{
					foreach (var item in recordView.Sidebar.Items)
					{
						if (item is InputRecordViewSidebarListItem)
						{
							InputRecordViewSidebarListItem inputItem = (InputRecordViewSidebarListItem)item;
							if (string.IsNullOrWhiteSpace(inputItem.ListName) && inputItem.ListId == null)
							{
								errorList.Add(new ErrorModel("sidebar.items.listName", null, "List name or id is required!"));
							}
							else
							{
								RecordList list = null;
								if (inputItem.ListId.HasValue)
									list = entity.RecordLists.SingleOrDefault(l => l.Id == inputItem.ListId);
								else
									list = entity.RecordLists.SingleOrDefault(l => l.Name == inputItem.ListName);


								if (list == null)
									errorList.Add(new ErrorModel("sidebar.items.listName", null, "Wrong name. There is no list with such name or id!"));
								else
								{
									inputItem.ListId = list.Id;
									inputItem.ListName = list.Name;
								}

								if (recordView.Sidebar.Items.Where(i => i is InputRecordViewSidebarListItem && ((InputRecordViewSidebarListItem)i).ListName == inputItem.ListName).Count() > 1)
									errorList.Add(new ErrorModel("sidebar.items.listName", null, "There is already an item with such list name or id!"));
							}
						}
						else if (item is InputRecordViewSidebarViewItem)
						{
							InputRecordViewSidebarViewItem inputItem = (InputRecordViewSidebarViewItem)item;
							if (string.IsNullOrWhiteSpace(inputItem.ViewName) && inputItem.ViewId == null)
							{
								errorList.Add(new ErrorModel("sidebar.items.viewName", null, "View name or id is required!"));
							}
							else
							{

								RecordView view = null;
								if (inputItem.ViewId.HasValue)
									view = entity.RecordViews.SingleOrDefault(v => v.Id == inputItem.ViewId);
								else
									view = entity.RecordViews.SingleOrDefault(v => v.Name == inputItem.ViewName);

								if (view == null)
									errorList.Add(new ErrorModel("sidebar.items.viewName", null, "Wrong name. There is no view with such name or id!"));
								else
								{
									inputItem.ViewId = view.Id;
									inputItem.ViewName = view.Name;
								}

								if (recordView.Sidebar.Items.Where(i => i is InputRecordViewSidebarViewItem && ((InputRecordViewSidebarViewItem)i).ViewName == inputItem.ViewName).Count() > 1)
									errorList.Add(new ErrorModel("sidebar.items.viewName", null, "There is already an item with such view name or id!"));

							}
						}
						else if (item is InputRecordViewSidebarRelationListItem)
						{
							EntityRelation relation = null;
							InputRecordViewSidebarRelationListItem inputItem = (InputRecordViewSidebarRelationListItem)item;
							if (string.IsNullOrWhiteSpace(inputItem.RelationName) && inputItem.RelationId == null)
							{
								errorList.Add(new ErrorModel("sidebar.items.relationName", null, "Relation name or id is required!"));
							}
							else
							{
								if (string.IsNullOrWhiteSpace(inputItem.RelationName))
									relation = relationList.SingleOrDefault(r => r.Id == inputItem.RelationId.Value);
								else
									relation = relationList.SingleOrDefault(r => r.Name == inputItem.RelationName);

								if (relation == null)
									errorList.Add(new ErrorModel("sidebar.items.relationName", null, "Wrong name. There is no relation with such name or id!"));
								else
								{
									inputItem.RelationId = relation.Id;
									inputItem.RelationName = relation.Name;
								}
							}

							if (string.IsNullOrWhiteSpace(inputItem.ListName) && inputItem.ListId == null)
							{
								errorList.Add(new ErrorModel("sidebar.items.listName", null, "List name or id is required!"));
							}
							else if (inputItem.RelationId.HasValue && inputItem.RelationId != Guid.Empty)
							{
								if (relation != null)
								{
									Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
									Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);

									if (relEntity != null)
									{
										inputItem.EntityId = relEntity.Id;
										inputItem.EntityName = relEntity.Name;

										RecordList relList = null;
										if (string.IsNullOrWhiteSpace(inputItem.ListName))
											relList = relEntity.RecordLists.FirstOrDefault(l => l.Id == inputItem.ListId);
										else
											relList = relEntity.RecordLists.FirstOrDefault(l => l.Name == inputItem.ListName);

										if (relList != null)
										{
											inputItem.ListId = relList.Id;
											inputItem.ListName = relList.Name;
										}
										else
											errorList.Add(new ErrorModel("sidebar.items.listName", null, "Wrong name. There is no list with such name or id!"));
									}
								}

								if (recordView.Sidebar.Items.Where(i => i is InputRecordViewSidebarRelationListItem &&
									   ((InputRecordViewSidebarRelationListItem)i).ListName == inputItem.ListName &&
									   ((InputRecordViewSidebarRelationListItem)i).RelationId == inputItem.RelationId).Count() > 1)
									errorList.Add(new ErrorModel("sidebar.items.listName", null, "There is already an item with such list name!"));

							}

						}
						else if (item is InputRecordViewSidebarRelationTreeItem)
						{
							EntityRelation relation = null;
							InputRecordViewSidebarRelationTreeItem inputItem = (InputRecordViewSidebarRelationTreeItem)item;
							if (string.IsNullOrWhiteSpace(inputItem.RelationName) && inputItem.RelationId == null)
							{
								errorList.Add(new ErrorModel("sidebar.items.relationName", null, "Relation name or id is required!"));
							}
							else
							{
								if (string.IsNullOrWhiteSpace(inputItem.RelationName))
									relation = relationList.SingleOrDefault(r => r.Id == inputItem.RelationId.Value);
								else
									relation = relationList.SingleOrDefault(r => r.Name == inputItem.RelationName);

								if (relation == null)
									errorList.Add(new ErrorModel("sidebar.items.relationName", null, "Wrong name. There is no relation with such name or id!"));
								else
								{
									inputItem.RelationId = relation.Id;
									inputItem.RelationName = relation.Name;
								}
							}

							if (string.IsNullOrWhiteSpace(inputItem.TreeName) && inputItem.TreeId == null)
							{
								errorList.Add(new ErrorModel("sidebar.items.treeName", null, "Tree name or id is required!"));
							}
							else if (inputItem.RelationId.HasValue && inputItem.RelationId != Guid.Empty)
							{
								if (relation != null)
								{
									Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
									Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);

									if (relEntity != null)
									{
										inputItem.EntityId = relEntity.Id;
										inputItem.EntityName = relEntity.Name;

										RecordTree tree = null;
										if (string.IsNullOrWhiteSpace(inputItem.TreeName))
											tree = relEntity.RecordTrees.FirstOrDefault(l => l.Id == inputItem.TreeId);
										else
											tree = relEntity.RecordTrees.FirstOrDefault(l => l.Name == inputItem.TreeName);

										if (tree != null)
										{
											inputItem.TreeId = tree.Id;
											inputItem.TreeName = tree.Name;
										}
										else
											errorList.Add(new ErrorModel("sidebar.items.listName", null, "Wrong name. There is no tree with such name or id!"));
									}
								}

								if (recordView.Sidebar.Items.Where(i => i is InputRecordViewSidebarRelationTreeItem &&
									   ((InputRecordViewSidebarRelationTreeItem)i).TreeName == inputItem.TreeName &&
									   ((InputRecordViewSidebarRelationTreeItem)i).RelationId == inputItem.RelationId).Count() > 1)
									errorList.Add(new ErrorModel("sidebar.items.listName", null, "There is already an item with such tree name!"));

							}

						}
						else if (item is InputRecordViewSidebarRelationViewItem)
						{
							EntityRelation relation = null;
							InputRecordViewSidebarRelationViewItem inputItem = (InputRecordViewSidebarRelationViewItem)item;
							if (string.IsNullOrWhiteSpace(inputItem.RelationName) && inputItem.RelationId == null)
							{
								errorList.Add(new ErrorModel("sidebar.items.relationName", null, "Relation name or id is required!"));
							}
							else
							{
								if (string.IsNullOrWhiteSpace(inputItem.RelationName))
									relation = relationList.SingleOrDefault(r => r.Id == inputItem.RelationId.Value);
								else
									relation = relationList.SingleOrDefault(r => r.Name == inputItem.RelationName);

								if (relation == null)
									errorList.Add(new ErrorModel("sidebar.items.relationName", null, "Wrong name. There is no relation with such name or id!"));
								else
								{
									inputItem.RelationId = relation.Id;
									inputItem.RelationName = relation.Name;
								}
							}

							if (string.IsNullOrWhiteSpace(inputItem.ViewName) && inputItem.ViewId == null)
							{
								errorList.Add(new ErrorModel("sidebar.items.viewName", null, "View name is required!"));
							}
							else if (inputItem.RelationId.HasValue && inputItem.RelationId != Guid.Empty)
							{
								if (relation != null)
								{
									Guid relEntityId = entity.Id == relation.OriginEntityId ? relation.TargetEntityId : relation.OriginEntityId;
									Entity relEntity = entities.FirstOrDefault(e => e.Id == relEntityId);

									if (relEntity != null)
									{
										inputItem.EntityId = relEntity.Id;
										inputItem.EntityName = relEntity.Name;

										RecordView relView = null;
										if (string.IsNullOrWhiteSpace(inputItem.ViewName))
											relView = relEntity.RecordViews.FirstOrDefault(l => l.Id == inputItem.ViewId);
										else
											relView = relEntity.RecordViews.FirstOrDefault(l => l.Name == inputItem.ViewName);

										if (relView != null)
										{
											inputItem.ViewId = relView.Id;
											inputItem.ViewName = relView.Name;
										}
										else
											errorList.Add(new ErrorModel("sidebar.items.viewName", null, "Wrong name. There is no view with such name or id!"));
									}
								}

								if (recordView.Sidebar.Items.Where(i => i is InputRecordViewSidebarRelationViewItem &&
									((InputRecordViewSidebarRelationViewItem)i).ViewName == inputItem.ViewName &&
									((InputRecordViewSidebarRelationViewItem)i).RelationId == inputItem.RelationId).Count() > 1)
									errorList.Add(new ErrorModel("sidebar.items.viewName", null, "There is already an item with such view name!"));

							}

						}
					}
				}
			}

			return errorList;
		}

		#endregion

		#region << Entity methods >>

		public EntityResponse CreateEntity(InputEntity inputEntity, bool createDefaultViews = true, bool createDefaultLists = true, Dictionary<string,Guid> sysIdDictionary = null)
		{
			EntityResponse response = new EntityResponse
			{
				Success = true,
				Message = "The entity was successfully created!",
			};

			//in order to support external IDs (while import in example)
			//we generate new ID only when it is not specified
			if (!inputEntity.Id.HasValue)
				inputEntity.Id = Guid.NewGuid();

			Entity entity = inputEntity.MapTo<Entity>();

			try
			{
				response.Object = entity;

				response.Errors = ValidateEntity(entity, false);

				if (response.Errors.Count > 0)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The entity was not created. Validation error occurred!";
					return response;
				}

				entity.Fields = CreateEntityDefaultFields(entity, sysIdDictionary);
				if (createDefaultViews)
				{
					entity.RecordViews = CreateEntityDefaultRecordViews(entity);
				}
				else
				{
					entity.RecordViews = new List<RecordView>();
				}
				if (createDefaultLists)
				{
					entity.RecordLists = CreateEntityDefaultRecordLists(entity);
				}
				else
				{
					entity.RecordLists = new List<RecordList>();
				}

				DbEntity storageEntity = entity.MapTo<DbEntity>();
				bool result = DbContext.Current.EntityRepository.Create(storageEntity, sysIdDictionary );
				if (!result)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The entity was not created! An internal error occurred!";
					return response;
				}
			}
			catch (Exception e)
			{
				response.Success = false;
				response.Object = entity;
				response.Timestamp = DateTime.UtcNow;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The entity was not created. An internal error occurred!";
#endif
				return response;
			}

			Cache.ClearEntities();

			var createdEntityResponse = ReadEntity(entity.Id);
			response.Object = createdEntityResponse.Object;
			response.Timestamp = DateTime.UtcNow;

			return response;
		}

		public EntityResponse CreateEntity(Guid id, string name, string label, string labelPlural, List<Guid> allowedRolesRead = null,
			List<Guid> allowedRolesCreate = null, List<Guid> allowedRolesUpdate = null, List<Guid> allowedRolesDelete = null)
		{
			InputEntity entity = new InputEntity();
			entity.Id = id;
			entity.Name = name;
			entity.Label = label;
			entity.LabelPlural = labelPlural;
			entity.System = false;
			entity.RecordPermissions = new RecordPermissions();
			entity.RecordPermissions.CanRead = allowedRolesRead ?? new List<Guid>() { SystemIds.AdministratorRoleId };
			entity.RecordPermissions.CanCreate = allowedRolesCreate ?? new List<Guid>() { SystemIds.AdministratorRoleId };
			entity.RecordPermissions.CanUpdate = allowedRolesUpdate ?? new List<Guid>() { SystemIds.AdministratorRoleId };
			entity.RecordPermissions.CanDelete = allowedRolesDelete ?? new List<Guid>() { SystemIds.AdministratorRoleId };

			return CreateEntity(entity);
		}

		public EntityResponse UpdateEntity(InputEntity inputEntity)
		{
			EntityResponse response = new EntityResponse
			{
				Success = true,
				Message = "The entity was successfully updated!",
			};

			Entity entity = inputEntity.MapTo<Entity>();

			try
			{
				response.Object = entity;
				response.Errors = ValidateEntity(entity, true);

				if (response.Errors.Count > 0)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The entity was not updated. Validation error occurred!";
					return response;
				}

				Entity storageEntity = ReadEntity(entity.Id).Object;

				storageEntity.Label = entity.Label;
				storageEntity.LabelPlural = entity.LabelPlural;
				storageEntity.System = entity.System;
				storageEntity.IconName = entity.IconName;
				storageEntity.Weight = entity.Weight;
				storageEntity.RecordPermissions.CanRead = entity.RecordPermissions.CanRead;
				storageEntity.RecordPermissions.CanCreate = entity.RecordPermissions.CanCreate;
				storageEntity.RecordPermissions.CanUpdate = entity.RecordPermissions.CanUpdate;
				storageEntity.RecordPermissions.CanDelete = entity.RecordPermissions.CanDelete;

				bool result = DbContext.Current.EntityRepository.Update(storageEntity.MapTo<DbEntity>());

				if (!result)
				{
					Cache.ClearEntities();
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The entity was not updated! An internal error occurred!";
					return response;
				}

			}
			catch (Exception e)
			{
				Cache.ClearEntities();
				response.Success = false;
				response.Object = entity;
				response.Timestamp = DateTime.UtcNow;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The entity was not updated. An internal error occurred!";
#endif
				return response;
			}

			Cache.ClearEntities();

			var updatedEntityResponse = ReadEntity(entity.Id);
			response.Object = updatedEntityResponse.Object;
			response.Timestamp = DateTime.UtcNow;

			return response;
		}

		//		public EntityResponse PartialUpdateEntity(Guid id, InputEntity inputEntity)
		//		{
		//			EntityResponse response = new EntityResponse
		//			{
		//				Success = true,
		//				Message = "The entity was successfully updated!",
		//			};

		//			Entity entity = null;

		//			try
		//			{
		//				IStorageEntity storageEntity = EntityRepository.Read(id);
		//				entity = storageEntity.MapTo<Entity>();

		//				if (inputEntity.Label != null)
		//					entity.Label = inputEntity.Label;
		//				if (inputEntity.LabelPlural != null)
		//					entity.LabelPlural = inputEntity.LabelPlural;
		//				if (inputEntity.System != null)
		//					entity.System = inputEntity.System.Value;
		//				if (inputEntity.IconName != null)
		//					entity.IconName = inputEntity.IconName;
		//				if (inputEntity.Weight != null)
		//					entity.Weight = inputEntity.Weight.Value;
		//				if (inputEntity.RecordPermissions != null)
		//					entity.RecordPermissions = inputEntity.RecordPermissions;

		//				response.Object = entity;
		//				response.Errors = ValidateEntity(entity, true);

		//				if (response.Errors.Count > 0)
		//				{
		//					response.Timestamp = DateTime.UtcNow;
		//					response.Success = false;
		//					response.Message = "The entity was not updated. Validation error occurred!";
		//					return response;
		//				}

		//				storageEntity = entity.MapTo<IStorageEntity>();

		//				bool result = EntityRepository.Update(storageEntity);

		//				if (!result)
		//				{
		//					response.Timestamp = DateTime.UtcNow;
		//					response.Success = false;
		//					response.Message = "The entity was not updated! An internal error occurred!";
		//					return response;
		//				}

		//			}
		//			catch (Exception e)
		//			{
		//				response.Success = false;
		//				response.Object = entity;
		//				response.Timestamp = DateTime.UtcNow;
		//#if DEBUG
		//				response.Message = e.Message + e.StackTrace;
		//#else
		//                response.Message = "The entity was not updated. An internal error occurred!";
		//#endif
		//				return response;
		//			}

		//			IStorageEntity updatedEntity = EntityRepository.Read(entity.Id);
		//			response.Object = updatedEntity.MapTo<Entity>();
		//			response.Timestamp = DateTime.UtcNow;

		//			return response;
		//		}

		public EntityResponse DeleteEntity(Guid id)
		{
			EntityResponse response = new EntityResponse
			{
				Success = true,
				Message = "The entity was successfully deleted!",
			};

			try
			{
				var entityResponse = ReadEntity(id);

				if (!entityResponse.Success)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = entityResponse.Message;
					return response;
				}
				else if (entityResponse.Object == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The entity was not deleted. Validation error occurred!";
					response.Errors.Add(new ErrorModel("id", id.ToString(), "Entity with such Id does not exist!"));
					return response;
				}

				//entity, entity records and relations are deleted in storage repository 
				DbContext.Current.EntityRepository.Delete(id);
			}
			catch (Exception e)
			{
				Cache.ClearEntities();

				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The entity was not deleted. An internal error occurred!";
#endif
				return response;
			}

			Cache.ClearEntities();

			response.Timestamp = DateTime.UtcNow;
			return response;
		}

		public EntityListResponse ReadEntities()
		{
			EntityListResponse response = new EntityListResponse
			{
				Success = true,
				Message = "The entity was successfully returned!",
			};

			//try return from cache			
			var entities = Cache.GetEntities();
			if (entities != null)
			{
				response.Object = entities;
				response.Hash = Cache.GetEntitiesHash();
				return response;
			}

			try
			{
				List<DbEntity> storageEntityList = DbContext.Current.EntityRepository.Read();
				entities = storageEntityList.MapTo<Entity>();

				List<EntityRelation> relationList = new EntityRelationManager().Read(storageEntityList).Object;


				//EntityRelationManager relationManager = new EntityRelationManager(Storage);
				//EntityRelationListResponse relationListResponse = relationManager.Read();
				//if (relationListResponse.Object != null)
				//	relationList = relationListResponse.Object;


				//TODO RUMEN - the unique key for finding fields, lists, views should be not only fieldId for example, but the fieldId+entityId combination. 
				//The problem occurs when there are two fields in two different entities with the same id.Same applies for view and list.
				List<RecordList> recordLists = new List<RecordList>();
				List<RecordView> recordViews = new List<RecordView>();
				List<Field> fields = new List<Field>();

				foreach (var entity in entities)
				{
					recordLists.AddRange(entity.RecordLists);
					recordViews.AddRange(entity.RecordViews);
					fields.AddRange(entity.Fields);
				}

				foreach (var entity in entities)
				{
					#region Process Lists

					if (entity.RecordLists != null)
					{
						foreach (var recordList in entity.RecordLists)
						{
							if (recordList.Columns != null)
							{
								foreach (var column in recordList.Columns)
								{
									if (column is RecordListFieldItem)
									{
										Field field = fields.SingleOrDefault(f => f.Id == ((RecordListFieldItem)column).FieldId);
										if (field != null)
										{
											//((RecordListFieldItem)column).DataName = string.Format("$field${0}", field.Name);
											((RecordListFieldItem)column).DataName = field.Name;
											((RecordListFieldItem)column).FieldName = field.Name;
											((RecordListFieldItem)column).Meta = field;

											((RecordListFieldItem)column).EntityName = entity.Name;
											((RecordListFieldItem)column).EntityLabel = entity.Label;
											((RecordListFieldItem)column).EntityLabelPlural = entity.LabelPlural;
										}
									}
									if (column is RecordListRelationFieldItem)
									{
										Entity relEntity = GetEntityByFieldId(((RecordListRelationFieldItem)column).FieldId, entities);
										if (relEntity != null)
										{
											((RecordListRelationFieldItem)column).EntityId = relEntity.Id;
											((RecordListRelationFieldItem)column).EntityName = relEntity.Name;
											((RecordListRelationFieldItem)column).EntityLabel = relEntity.Label;
											((RecordListRelationFieldItem)column).EntityLabelPlural = entity.LabelPlural;
										}

										var relation = relationList.SingleOrDefault(r => r.Id == ((RecordListRelationFieldItem)column).RelationId);
										((RecordListRelationFieldItem)column).RelationName = relation != null ? relation.Name : string.Empty;

										if (relation != null)
										{
											var relationOptions = recordList.RelationOptions.SingleOrDefault(x => x.RelationId == ((RecordListRelationFieldItem)column).RelationId);
											if (relationOptions != null)
												((RecordListRelationFieldItem)column).RelationDirection = relationOptions.Direction;
											else
												((RecordListRelationFieldItem)column).RelationDirection = "origin-target";
										}

										Field field = fields.SingleOrDefault(f => f.Id == ((RecordListRelationFieldItem)column).FieldId);
										if (field != null)
										{
											((RecordListRelationFieldItem)column).DataName = string.Format("$field${0}${1}", ((RecordListRelationFieldItem)column).RelationName, field.Name);
											((RecordListRelationFieldItem)column).FieldName = field.Name;
											((RecordListRelationFieldItem)column).Meta = field;
										}
									}
									if (column is RecordListViewItem)
									{
										RecordView view = recordViews.SingleOrDefault(v => v.Id == ((RecordListViewItem)column).ViewId);
										if (view != null)
										{
											((RecordListViewItem)column).DataName = string.Format("$view${0}", view.Name);
											((RecordListViewItem)column).ViewName = view.Name;
											((RecordListViewItem)column).Meta = view;

											((RecordListViewItem)column).EntityName = entity.Name;
											((RecordListViewItem)column).EntityLabel = entity.Label;
											((RecordListViewItem)column).EntityLabelPlural = entity.LabelPlural;
										}
									}
									if (column is RecordListRelationViewItem)
									{
										Entity relEntity = GetEntityByViewId(((RecordListRelationViewItem)column).ViewId, entities);
										if (relEntity != null)
										{
											((RecordListRelationViewItem)column).EntityId = relEntity.Id;
											((RecordListRelationViewItem)column).EntityName = relEntity.Name;
											((RecordListRelationViewItem)column).EntityLabel = relEntity.Label;
											((RecordListRelationViewItem)column).EntityLabelPlural = entity.LabelPlural;
										}

										var relation = relationList.SingleOrDefault(r => r.Id == ((RecordListRelationViewItem)column).RelationId);
										((RecordListRelationViewItem)column).RelationName = relation != null ? relation.Name : string.Empty;

										if (relation != null)
										{
											var relationOptions = recordList.RelationOptions.SingleOrDefault(x => x.RelationId == ((RecordListRelationViewItem)column).RelationId);
											if (relationOptions != null)
												((RecordListRelationViewItem)column).RelationDirection = relationOptions.Direction;
											else
												((RecordListRelationViewItem)column).RelationDirection = "origin-target";
										}


										RecordView view = recordViews.SingleOrDefault(v => v.Id == ((RecordListRelationViewItem)column).ViewId);
										if (view != null)
										{
											((RecordListRelationViewItem)column).DataName = string.Format("$view${0}${1}", ((RecordListRelationViewItem)column).RelationName, view.Name);
											((RecordListRelationViewItem)column).ViewName = view.Name;
											((RecordListRelationViewItem)column).Meta = view;
										}
									}
									if (column is RecordListListItem)
									{
										RecordList list = recordLists.SingleOrDefault(l => l.Id == ((RecordListListItem)column).ListId);
										if (list != null)
										{
											((RecordListListItem)column).DataName = string.Format("list${0}", list.Name);
											((RecordListListItem)column).ListName = list.Name;
											((RecordListListItem)column).Meta = list;

											((RecordListListItem)column).EntityName = entity.Name;
											((RecordListListItem)column).EntityLabel = entity.Label;
											((RecordListListItem)column).EntityLabelPlural = entity.LabelPlural;
										}
									}
									if (column is RecordListRelationListItem)
									{
										Entity relEntity = GetEntityByListId(((RecordListRelationListItem)column).ListId, entities);
										if (relEntity != null)
										{
											((RecordListRelationListItem)column).EntityId = relEntity.Id;
											((RecordListRelationListItem)column).EntityName = relEntity.Name;
											((RecordListRelationListItem)column).EntityLabel = relEntity.Label;
											((RecordListRelationListItem)column).EntityLabelPlural = entity.LabelPlural;
										}

										var relation = relationList.SingleOrDefault(r => r.Id == ((RecordListRelationListItem)column).RelationId);
										((RecordListRelationListItem)column).RelationName = relation != null ? relation.Name : string.Empty;

										if (relation != null)
										{
											var relationOptions = recordList.RelationOptions.SingleOrDefault(x => x.RelationId == ((RecordListRelationListItem)column).RelationId);
											if (relationOptions != null)
												((RecordListRelationListItem)column).RelationDirection = relationOptions.Direction;
											else
												((RecordListRelationListItem)column).RelationDirection = "origin-target";
										}

										RecordList list = recordLists.SingleOrDefault(l => l.Id == ((RecordListRelationListItem)column).ListId);
										if (list != null)
										{
											((RecordListRelationListItem)column).DataName = string.Format("$list${0}${1}", ((RecordListRelationListItem)column).RelationName, list.Name);
											((RecordListRelationListItem)column).ListName = list.Name;
											((RecordListRelationListItem)column).Meta = list;
										}
									}
									if (column is RecordListRelationTreeItem)
									{
										Entity relEntity = GetEntityByTreeId(((RecordListRelationTreeItem)column).TreeId, entities);
										if (relEntity != null)
										{
											((RecordListRelationTreeItem)column).EntityId = relEntity.Id;
											((RecordListRelationTreeItem)column).EntityName = relEntity.Name;
											((RecordListRelationTreeItem)column).EntityLabel = relEntity.Label;
											((RecordListRelationTreeItem)column).EntityLabelPlural = entity.LabelPlural;
										}

										var relation = relationList.SingleOrDefault(r => r.Id == ((RecordListRelationTreeItem)column).RelationId);
										((RecordListRelationTreeItem)column).RelationName = relation != null ? relation.Name : string.Empty;

										RecordTree tree = relEntity.RecordTrees.SingleOrDefault(l => l.Id == ((RecordListRelationTreeItem)column).TreeId);
										if (tree != null)
										{
											((RecordListRelationTreeItem)column).DataName = string.Format("$tree${0}${1}", ((RecordListRelationTreeItem)column).RelationName, tree.Name);
											((RecordListRelationTreeItem)column).TreeName = tree.Name;
											((RecordListRelationTreeItem)column).Meta = tree;
										}
									}
								}
							}
						}
					}

					#endregion

					#region Process Views

					if (entity.RecordViews != null)
					{
						foreach (var recordView in entity.RecordViews)
						{
							if (recordView.Regions == null)
								continue;

							foreach (var region in recordView.Regions)
							{
								if (region.Sections == null)
									continue;

								foreach (var section in region.Sections)
								{
									if (section.Rows == null)
										continue;

									foreach (var row in section.Rows)
									{
										if (row.Columns == null)
											continue;

										foreach (var column in row.Columns)
										{
											if (column.Items == null)
												continue;

											foreach (var item in column.Items)
											{
												if (item is RecordViewFieldItem)
												{
													Entity fieldEntity = entities.Single(f => f.Id == ((RecordViewFieldItem)item).EntityId);
													Field field = fieldEntity.Fields.Single(f => f.Id == ((RecordViewFieldItem)item).FieldId);
													if (field != null)
													{
														//((RecordViewFieldItem)item).DataName = string.Format("$field${0}", field.Name);
														((RecordViewFieldItem)item).DataName = field.Name;
														((RecordViewFieldItem)item).FieldName = field.Name;
														((RecordViewFieldItem)item).Meta = field;

														((RecordViewFieldItem)item).EntityId = entity.Id;
														((RecordViewFieldItem)item).EntityName = entity.Name;
														((RecordViewFieldItem)item).EntityLabel = entity.Label;
														((RecordViewFieldItem)item).EntityLabelPlural = entity.LabelPlural;
													}
												}
												if (item is RecordViewListItem)
												{
													RecordList list = entity.RecordLists.FirstOrDefault(l => l.Id == ((RecordViewListItem)item).ListId);
													if (list != null)
													{
														((RecordViewListItem)item).DataName = string.Format("$list${0}", list.Name);
														((RecordViewListItem)item).Meta = list;
														((RecordViewListItem)item).ListName = list.Name;

														((RecordViewListItem)item).EntityId = entity.Id;
														((RecordViewListItem)item).EntityName = entity.Name;
														((RecordViewListItem)item).EntityLabel = entity.Label;
														((RecordViewListItem)item).EntityLabelPlural = entity.LabelPlural;
													}

												}
												if (item is RecordViewViewItem)
												{
													RecordView recView = entity.RecordViews.FirstOrDefault(v => v.Id == ((RecordViewViewItem)item).ViewId);
													if (recView != null)
													{
														((RecordViewViewItem)item).DataName = string.Format("$view${0}", recView.Name);
														((RecordViewViewItem)item).Meta = recView;
														((RecordViewViewItem)item).ViewName = recView.Name;

														((RecordViewViewItem)item).EntityId = entity.Id;
														((RecordViewViewItem)item).EntityName = entity.Name;
														((RecordViewViewItem)item).EntityLabel = entity.Label;
														((RecordViewViewItem)item).EntityLabelPlural = entity.LabelPlural;
													}
												}

												if (item is RecordViewRelationFieldItem)
												{
													Entity relEntity = GetEntityByFieldId(((RecordViewRelationFieldItem)item).FieldId, entities);
													if (relEntity != null)
													{
														((RecordViewRelationFieldItem)item).EntityId = relEntity.Id;
														((RecordViewRelationFieldItem)item).EntityName = relEntity.Name;
														((RecordViewRelationFieldItem)item).EntityLabel = relEntity.Label;
													}

													var relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewRelationFieldItem)item).RelationId);
													((RecordViewRelationFieldItem)item).RelationName = relation != null ? relation.Name : string.Empty;

													if (relation != null)
													{
														var relationOptions = recordView.RelationOptions.SingleOrDefault(x => x.RelationId == ((RecordViewRelationFieldItem)item).RelationId);
														if (relationOptions != null)
															((RecordViewRelationFieldItem)item).RelationDirection = relationOptions.Direction;
														else
															((RecordViewRelationFieldItem)item).RelationDirection = "origin-target";
													}

													Field field = fields.FirstOrDefault(f => f.Id == ((RecordViewRelationFieldItem)item).FieldId);
													if (field != null)
													{
														((RecordViewRelationFieldItem)item).DataName = string.Format("$field${0}${1}", ((RecordViewRelationFieldItem)item).RelationName, field.Name);
														((RecordViewRelationFieldItem)item).Meta = field;
														((RecordViewRelationFieldItem)item).FieldName = field.Name;
													}
												}

												if (item is RecordViewRelationViewItem)
												{
													var relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewRelationViewItem)item).RelationId);
													((RecordViewRelationViewItem)item).RelationName = relation != null ? relation.Name : string.Empty;

													if (relation != null)
													{
														var relationOptions = recordView.RelationOptions.SingleOrDefault(x => x.RelationId == ((RecordViewRelationViewItem)item).RelationId);
														if (relationOptions != null)
															((RecordViewRelationViewItem)item).RelationDirection = relationOptions.Direction;
														else
															((RecordViewRelationViewItem)item).RelationDirection = "origin-target";
													}

													Entity relEntity = GetEntityByViewId(((RecordViewRelationViewItem)item).ViewId, entities);
													if (relEntity != null)
													{
														((RecordViewRelationViewItem)item).EntityId = relEntity.Id;
														((RecordViewRelationViewItem)item).EntityName = relEntity.Name;
														((RecordViewRelationViewItem)item).EntityLabel = relEntity.Label;

														RecordView view = relEntity.RecordViews.FirstOrDefault(f => f.Id == ((RecordViewRelationViewItem)item).ViewId);
														if (view != null)
														{
															((RecordViewRelationViewItem)item).DataName = string.Format("$view${0}${1}", ((RecordViewRelationViewItem)item).RelationName, view.Name);
															((RecordViewRelationViewItem)item).Meta = view;
															((RecordViewRelationViewItem)item).ViewName = view.Name;
														}
													}
												}

												if (item is RecordViewRelationListItem)
												{
													var relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewRelationListItem)item).RelationId);
													((RecordViewRelationListItem)item).RelationName = relation != null ? relation.Name : string.Empty;

													if (relation != null)
													{
														var relationOptions = recordView.RelationOptions.SingleOrDefault(x => x.RelationId == ((RecordViewRelationListItem)item).RelationId);
														if (relationOptions != null)
															((RecordViewRelationListItem)item).RelationDirection = relationOptions.Direction;
														else
															((RecordViewRelationListItem)item).RelationDirection = "origin-target";
													}

													Entity relEntity = GetEntityByListId(((RecordViewRelationListItem)item).ListId, entities);
													if (relEntity != null)
													{
														((RecordViewRelationListItem)item).EntityId = relEntity.Id;
														((RecordViewRelationListItem)item).EntityName = relEntity.Name;
														((RecordViewRelationListItem)item).EntityLabel = relEntity.Label;
														((RecordViewRelationListItem)item).EntityLabelPlural = relEntity.LabelPlural;

														RecordList list = relEntity.RecordLists.FirstOrDefault(f => f.Id == ((RecordViewRelationListItem)item).ListId);
														if (list != null)
														{
															((RecordViewRelationListItem)item).DataName = string.Format("$list${0}${1}", ((RecordViewRelationListItem)item).RelationName, list.Name);
															((RecordViewRelationListItem)item).Meta = list;
															((RecordViewRelationListItem)item).ListName = list.Name;
														}
													}
												}

												if (item is RecordViewRelationTreeItem)
												{
													Entity relEntity = GetEntityByTreeId(((RecordViewRelationTreeItem)item).TreeId, entities);
													if (relEntity != null)
													{
														((RecordViewRelationTreeItem)item).EntityId = relEntity.Id;
														((RecordViewRelationTreeItem)item).EntityName = relEntity.Name;
														((RecordViewRelationTreeItem)item).EntityLabel = relEntity.Label;
														((RecordViewRelationTreeItem)item).EntityLabelPlural = entity.LabelPlural;
													}

													var relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewRelationTreeItem)item).RelationId);
													((RecordViewRelationTreeItem)item).RelationName = relation != null ? relation.Name : string.Empty;

													RecordTree tree = relEntity.RecordTrees.FirstOrDefault(l => l.Id == ((RecordViewRelationTreeItem)item).TreeId);
													if (tree != null)
													{

														((RecordViewRelationTreeItem)item).DataName = string.Format("$tree${0}${1}", ((RecordViewRelationTreeItem)item).RelationName, tree.Name);
														((RecordViewRelationTreeItem)item).TreeName = tree.Name;
														((RecordViewRelationTreeItem)item).Meta = tree;
													}
												}
											}
										}
									}
								}
							}

							if (recordView.Sidebar != null)
							{
								foreach (var item in recordView.Sidebar.Items)
								{
									if (item is RecordViewSidebarListItem)
									{
										RecordList list = entity.RecordLists.FirstOrDefault(l => l.Id == ((RecordViewSidebarListItem)item).ListId);
										if (list != null)
										{
											((RecordViewSidebarListItem)item).DataName = string.Format("$list${0}", list.Name);
											((RecordViewSidebarListItem)item).Meta = list;
											((RecordViewSidebarListItem)item).ListName = list.Name;

											((RecordViewSidebarListItem)item).EntityId = entity.Id;
											((RecordViewSidebarListItem)item).EntityName = entity.Name;
											((RecordViewSidebarListItem)item).EntityLabel = entity.Label;
											((RecordViewSidebarListItem)item).EntityLabelPlural = entity.LabelPlural;
										}

									}
									if (item is RecordViewSidebarViewItem)
									{
										RecordView recView = entity.RecordViews.FirstOrDefault(v => v.Id == ((RecordViewSidebarViewItem)item).ViewId);
										if (recView != null)
										{
											((RecordViewSidebarViewItem)item).DataName = string.Format("$view${0}", recView.Name);
											((RecordViewSidebarViewItem)item).Meta = recView;
											((RecordViewSidebarViewItem)item).ViewName = recView.Name;

											((RecordViewSidebarViewItem)item).EntityId = entity.Id;
											((RecordViewSidebarViewItem)item).EntityName = entity.Name;
											((RecordViewSidebarViewItem)item).EntityLabel = entity.Label;
											((RecordViewSidebarViewItem)item).EntityLabelPlural = entity.LabelPlural;
										}
									}
									if (item is RecordViewSidebarRelationViewItem)
									{
										var relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewSidebarRelationViewItem)item).RelationId);
										((RecordViewSidebarRelationViewItem)item).RelationName = relation != null ? relation.Name : string.Empty;

										if (relation != null)
										{
											var relationOptions = recordView.RelationOptions.SingleOrDefault(x => x.RelationId == ((RecordViewSidebarRelationViewItem)item).RelationId);
											if (relationOptions != null)
												((RecordViewSidebarRelationViewItem)item).RelationDirection = relationOptions.Direction;
											else
												((RecordViewSidebarRelationViewItem)item).RelationDirection = "origin-target";
										}

										Entity relEntity = GetEntityByViewId(((RecordViewSidebarRelationViewItem)item).ViewId, entities);
										if (relEntity != null)
										{
											((RecordViewSidebarRelationViewItem)item).EntityId = relEntity.Id;
											((RecordViewSidebarRelationViewItem)item).EntityName = relEntity.Name;
											((RecordViewSidebarRelationViewItem)item).EntityLabel = relEntity.Label;

											RecordView view = relEntity.RecordViews.FirstOrDefault(f => f.Id == ((RecordViewSidebarRelationViewItem)item).ViewId);
											if (view != null)
											{
												((RecordViewSidebarRelationViewItem)item).DataName = string.Format("$view${0}${1}", ((RecordViewSidebarRelationViewItem)item).RelationName, view.Name);
												((RecordViewSidebarRelationViewItem)item).Meta = view;
												((RecordViewSidebarRelationViewItem)item).ViewName = view.Name;
											}
										}
									}

									if (item is RecordViewSidebarRelationListItem)
									{
										var relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewSidebarRelationListItem)item).RelationId);
										((RecordViewSidebarRelationListItem)item).RelationName = relation != null ? relation.Name : string.Empty;

										if (relation != null)
										{
											var relationOptions = recordView.RelationOptions.SingleOrDefault(x => x.RelationId == ((RecordViewSidebarRelationListItem)item).RelationId);
											if (relationOptions != null)
												((RecordViewSidebarRelationListItem)item).RelationDirection = relationOptions.Direction;
											else
												((RecordViewSidebarRelationListItem)item).RelationDirection = "origin-target";
										}

										Entity relEntity = GetEntityByListId(((RecordViewSidebarRelationListItem)item).ListId, entities);
										if (relEntity != null)
										{
											((RecordViewSidebarRelationListItem)item).EntityId = relEntity.Id;
											((RecordViewSidebarRelationListItem)item).EntityName = relEntity.Name;
											((RecordViewSidebarRelationListItem)item).EntityLabel = relEntity.Label;
											((RecordViewSidebarRelationListItem)item).EntityLabelPlural = relEntity.LabelPlural;

											RecordList list = relEntity.RecordLists.FirstOrDefault(f => f.Id == ((RecordViewSidebarRelationListItem)item).ListId);
											if (list != null)
											{
												((RecordViewSidebarRelationListItem)item).DataName = string.Format("$list${0}${1}", ((RecordViewSidebarRelationListItem)item).RelationName, list.Name);
												((RecordViewSidebarRelationListItem)item).Meta = list;
												((RecordViewSidebarRelationListItem)item).ListName = list.Name;
											}
										}
									}

									if (item is RecordViewSidebarRelationTreeItem)
									{
										Entity relEntity = GetEntityByTreeId(((RecordViewSidebarRelationTreeItem)item).TreeId, entities);
										if (relEntity != null)
										{
											((RecordViewSidebarRelationTreeItem)item).EntityId = relEntity.Id;
											((RecordViewSidebarRelationTreeItem)item).EntityName = relEntity.Name;
											((RecordViewSidebarRelationTreeItem)item).EntityLabel = relEntity.Label;
											((RecordViewSidebarRelationTreeItem)item).EntityLabelPlural = entity.LabelPlural;
										}

										var relation = relationList.FirstOrDefault(r => r.Id == ((RecordViewSidebarRelationTreeItem)item).RelationId);
										((RecordViewSidebarRelationTreeItem)item).RelationName = relation != null ? relation.Name : string.Empty;

										RecordTree tree = relEntity.RecordTrees.FirstOrDefault(l => l.Id == ((RecordViewSidebarRelationTreeItem)item).TreeId);
										if (tree != null)
										{
											((RecordViewSidebarRelationTreeItem)item).DataName = string.Format("$tree${0}${1}", ((RecordViewSidebarRelationTreeItem)item).RelationName, tree.Name);
											((RecordViewSidebarRelationTreeItem)item).TreeName = tree.Name;
											((RecordViewSidebarRelationTreeItem)item).Meta = tree;
										}
									}
								}
							}
						}
					}

					#endregion

					#region Process Trees

					if (entity.RecordTrees != null)
					{

						foreach (var recordTree in entity.RecordTrees)
						{
							foreach (RecordTreeNode node in recordTree.RootNodes)
							{
								var recData = DbContext.Current.RecordRepository.FindTreeNodeRecord(entity.Name, node.RecordId);
								if (recData != null)
								{
									var idField = entity.Fields.SingleOrDefault(x => x.Id == recordTree.NodeIdFieldId);
									if (idField == null)
										throw new Exception("Cannot initialize tree '" + recordTree.Name + "'. Node id field is missing in entity meta.");

									var parentIdField = entity.Fields.SingleOrDefault(x => x.Id == recordTree.NodeParentIdFieldId);
									if (parentIdField == null)
										throw new Exception("Cannot initialize tree '" + recordTree.Name + "'. Parent id field is missing in entity meta.");

									var nameField = entity.Fields.SingleOrDefault(x => x.Id == recordTree.NodeNameFieldId);
									if (nameField == null)
										throw new Exception("Cannot initialize tree '" + recordTree.Name + "'. Node name field is missing in entity meta.");

									var labelField = entity.Fields.SingleOrDefault(x => x.Id == recordTree.NodeLabelFieldId);
									if (labelField == null)
										throw new Exception("Cannot initialize tree '" + recordTree.Name + "'. Node label field is missing in entity meta.");

									var weigthField = entity.Fields.SingleOrDefault(x => x.Id == recordTree.NodeWeightFieldId);
									if (weigthField == null)
										throw new Exception("Cannot initialize tree '" + recordTree.Name + "'. Node weigth field is missing in entity meta.");

									var value = recData[idField.Name];
									node.Id = (value as Guid?) ?? Guid.Empty;

									value = recData[parentIdField.Name];
									node.ParentId = value as Guid?;

									value = recData[nameField.Name];
									node.Name = (value ?? string.Empty).ToString();

									value = recData[labelField.Name];
									node.Label = (value ?? string.Empty).ToString();

									node.Weight = (int?)(recData[weigthField.Name] as decimal?);
								}
							}
						}
					}

					#endregion

					//compute hash code
					entity.Hash = CryptoUtility.ComputeOddMD5Hash(JsonConvert.SerializeObject(entity));
				}

				Cache.AddEntities(entities);
				response.Object = entities;
				response.Hash = Cache.GetEntitiesHash();
			}
			catch (Exception e)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "An internal error occurred!";
#endif
				return response;
			}

			response.Timestamp = DateTime.Now;

			return response;
		}

		public EntityResponse ReadEntity(Guid id)
		{
			EntityResponse response = new EntityResponse
			{
				Success = true,
				Message = "The entity was successfully returned!",
				Timestamp = DateTime.UtcNow
			};

			try
			{
				EntityListResponse entityListResponse = ReadEntities();

				if (entityListResponse != null && entityListResponse.Object != null)
				{
					List<Entity> entities = entityListResponse.Object;

					Entity entity = entities.FirstOrDefault(e => e.Id == id);
					if (entity != null)
						response.Object = entity;
				}
			}
			catch (Exception e)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "An internal error occurred!";
#endif
				return response;
			}

			response.Timestamp = DateTime.Now;

			return response;
		}

		public EntityResponse ReadEntity(string name)
		{
			EntityResponse response = new EntityResponse
			{
				Success = true,
				Message = "The entity was successfully returned!",
				Timestamp = DateTime.UtcNow
			};

			try
			{
				EntityListResponse entityListResponse = ReadEntities();

				if (entityListResponse != null && entityListResponse.Object != null)
				{
					List<Entity> entities = entityListResponse.Object;

					Entity entity = entities.FirstOrDefault(e => e.Name == name);
					if (entity != null)
						response.Object = entity;
				}
			}
			catch (Exception e)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "An internal error occurred!";
#endif
				return response;
			}

			response.Timestamp = DateTime.Now;

			return response;
		}

		#endregion

		#region << Field methods >>

		public FieldResponse CreateField(Guid entityId, InputField inputField, bool transactional = true)
		{
			FieldResponse response = new FieldResponse
			{
				Success = true,
				Message = "The field was successfully created!",
			};

			Field field = null;

			try
			{
				var entityResponse = ReadEntity(entityId);

				if (!entityResponse.Success)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = entityResponse.Message;
					return response;
				}
				else if (entityResponse.Object == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "Entity with such Id does not exist!";
					return response;
				}
				Entity entity = entityResponse.Object;

				if (inputField.Id == null || inputField.Id == Guid.Empty)
					inputField.Id = Guid.NewGuid();

				response.Errors = ValidateField(entity, inputField, false);

				field = inputField.MapTo<Field>();

				if (response.Errors.Count > 0)
				{
					response.Object = field;
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The field was not created. Validation error occurred!";
					return response;
				}

				entity.Fields.Add(field);

				DbEntity editedEntity = entity.MapTo<DbEntity>();

				using (DbConnection con = DbContext.Current.CreateConnection())
				{
					con.BeginTransaction();

					try
					{
						bool result = DbContext.Current.EntityRepository.Update(editedEntity);
						if (!result)
						{
							response.Timestamp = DateTime.UtcNow;
							response.Success = false;
							response.Message = "The field was not created! An internal error occurred!";
							return response;
						}

						DbContext.Current.RecordRepository.CreateRecordField(entity.Name, field);

						con.CommitTransaction();
					}
					catch
					{
						con.RollbackTransaction();
						throw;
					}
				}

			}
			catch (Exception e)
			{
				Cache.ClearEntities();

				response.Success = false;
				response.Object = field;
				response.Timestamp = DateTime.UtcNow;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The field was not created. An internal error occurred!";
#endif
				return response;
			}

			Cache.ClearEntities();

			response.Object = field;
			response.Timestamp = DateTime.UtcNow;

			return response;
		}

		public FieldResponse CreateField(Guid entityId, FieldType type, Expando data, string name, string label, Guid? id = null,
					string placeholderText = "", string helpText = "", string description = "",
					bool system = false, bool required = false, bool unique = false, bool searchable = false, bool auditable = false,
					bool transactional = true)
		{
			Field field = null;

			if (data == null)
				data = new Expando();

			switch (type)
			{
				case FieldType.AutoNumberField:
					field = new AutoNumberField();
					if (HasKey(data, "defaultValue") && data["defaultValue"] != null)
						((AutoNumberField)field).DefaultValue = (decimal?)data["defaultValue"];
					if (HasKey(data, "startingNumber") && data["startingNumber"] != null)
						((AutoNumberField)field).StartingNumber = (decimal?)data["startingNumber"];
					if (HasKey(data, "displayFormat") && data["displayFormat"] != null)
						((AutoNumberField)field).DisplayFormat = (string)data["displayFormat"];
					break;
				case FieldType.CheckboxField:
					field = new CheckboxField();
					if (HasKey(data, "defaultValue") && data["defaultValue"] != null)
						((CheckboxField)field).DefaultValue = (bool?)data["defaultValue"] ?? false;
					break;
				case FieldType.CurrencyField:
					field = new CurrencyField();
					if (HasKey(data, "defaultValue") && data["defaultValue"] != null)
						((CurrencyField)field).DefaultValue = (decimal?)data["defaultValue"];
					if (HasKey(data, "minValue") && data["minValue"] != null)
						((CurrencyField)field).MinValue = (decimal?)data["minValue"];
					if (HasKey(data, "maxValue") && data["maxValue"] != null)
						((CurrencyField)field).MaxValue = (decimal?)data["maxValue"];
					if (HasKey(data, "currency") && data["currency"] != null)
					{
						((CurrencyField)field).Currency = (CurrencyType)data["currency"];
					}
					else
					{
						((CurrencyField)field).Currency = new CurrencyType();
						((CurrencyField)field).Currency.Code = "USD";
						((CurrencyField)field).Currency.DecimalDigits = 2;
						((CurrencyField)field).Currency.Name = "US dollar";
						((CurrencyField)field).Currency.NamePlural = "US dollars";
						((CurrencyField)field).Currency.Rounding = 0;
						((CurrencyField)field).Currency.Symbol = "$";
						((CurrencyField)field).Currency.SymbolNative = "$";
						((CurrencyField)field).Currency.SymbolPlacement = CurrencySymbolPlacement.Before;
						((CurrencyField)field).DefaultValue = 1;
					}
					break;
				case FieldType.DateField:
					field = new DateField();
					if (HasKey(data, "defaultValue") && data["defaultValue"] != null)
						((DateField)field).DefaultValue = (DateTime?)data["defaultValue"];
					if (HasKey(data, "format") && data["format"] != null)
						((DateField)field).Format = (string)data["format"];
					if (HasKey(data, "useCurrentTimeAsDefaultValue") && data["useCurrentTimeAsDefaultValue"] != null)
						((DateField)field).UseCurrentTimeAsDefaultValue = (bool?)data["useCurrentTimeAsDefaultValue"];
					break;
				case FieldType.DateTimeField:
					field = new DateTimeField();
					if (HasKey(data, "defaultValue") && data["defaultValue"] != null)
						((DateTimeField)field).DefaultValue = (DateTime?)data["defaultValue"];
					if (HasKey(data, "format") && data["format"] != null)
						((DateTimeField)field).Format = (string)data["format"];
					if (HasKey(data, "useCurrentTimeAsDefaultValue") && data["useCurrentTimeAsDefaultValue"] != null)
						((DateTimeField)field).UseCurrentTimeAsDefaultValue = (bool?)data["useCurrentTimeAsDefaultValue"];
					break;
				case FieldType.EmailField:
					field = new EmailField();
					if (HasKey(data, "defaultValue") && data["defaultValue"] != null)
						((EmailField)field).DefaultValue = (string)data["defaultValue"];
					if (HasKey(data, "maxLength") && data["maxLength"] != null)
						((EmailField)field).MaxLength = (int?)data["maxLength"];
					break;
				case FieldType.FileField:
					field = new FileField();
					if (HasKey(data, "defaultValue") && data["defaultValue"] != null)
						((FileField)field).DefaultValue = (string)data["defaultValue"];
					break;
				case FieldType.GuidField:
					field = new GuidField();
					if (HasKey(data, "defaultValue") && data["defaultValue"] != null)
						((GuidField)field).DefaultValue = (Guid?)data["defaultValue"];
					if (HasKey(data, "generateNewId") && data["generateNewId"] != null)
						((GuidField)field).GenerateNewId = (bool?)data["generateNewId"];
					break;
				case FieldType.HtmlField:
					field = new HtmlField();
					if (HasKey(data, "defaultValue") && data["defaultValue"] != null)
						((HtmlField)field).DefaultValue = (string)data["defaultValue"];
					break;
				case FieldType.ImageField:
					field = new ImageField();
					if (HasKey(data, "defaultValue") && data["defaultValue"] != null)
						((ImageField)field).DefaultValue = (string)data["defaultValue"];
					break;
				case FieldType.MultiLineTextField:
					field = new MultiLineTextField();
					if (HasKey(data, "defaultValue") && data["defaultValue"] != null)
						((MultiLineTextField)field).DefaultValue = (string)data["defaultValue"];
					if (HasKey(data, "maxLength") && data["maxLength"] != null)
						((MultiLineTextField)field).MaxLength = (int?)data["maxLength"];
					if (HasKey(data, "visibleLineNumber") && data["visibleLineNumber"] != null)
						((MultiLineTextField)field).VisibleLineNumber = (int?)data["visibleLineNumber"];
					break;
				case FieldType.MultiSelectField:
					field = new MultiSelectField();
					if (HasKey(data, "defaultValue") && data["defaultValue"] != null)
						((MultiSelectField)field).DefaultValue = (IEnumerable<string>)data["defaultValue"];
					if (HasKey(data, "options") && data["options"] != null)
						((MultiSelectField)field).Options = (List<MultiSelectFieldOption>)data["options"];
					break;
				case FieldType.NumberField:
					field = new NumberField();
					if (HasKey(data, "defaultValue") && data["defaultValue"] != null)
						((NumberField)field).DefaultValue = (int?)data["defaultValue"];
					if (HasKey(data, "minValue") && data["minValue"] != null)
						((NumberField)field).MinValue = (decimal?)data["minValue"];
					if (HasKey(data, "maxValue") && data["maxValue"] != null)
						((NumberField)field).MaxValue = (decimal?)data["maxValue"];
					if (HasKey(data, "decimalPlaces") && data["decimalPlaces"] != null)
						((NumberField)field).DecimalPlaces = (byte?)data["decimalPlaces"];
					break;
				case FieldType.PasswordField:
					field = new PasswordField();
					if (HasKey(data, "maxLength") && data["maxLength"] != null)
						((PasswordField)field).MaxLength = (int?)data["maxLength"];
					if (HasKey(data, "minLength") && data["minLength"] != null)
						((PasswordField)field).MinLength = (int?)data["minLength"];
					if (HasKey(data, "encrypted") && data["encrypted"] != null)
						((PasswordField)field).Encrypted = (bool?)data["encrypted"];
					break;
				case FieldType.PercentField:
					field = new PercentField();
					if (HasKey(data, "defaultValue") && data["defaultValue"] != null)
						((PercentField)field).DefaultValue = (decimal?)data["defaultValue"]; //0.01m;
					if (HasKey(data, "minValue") && data["minValue"] != null)
						((PercentField)field).MinValue = (decimal?)data["minValue"];
					if (HasKey(data, "maxValue") && data["maxValue"] != null)
						((PercentField)field).MaxValue = (decimal?)data["maxValue"];
					if (HasKey(data, "decimalPlaces") && data["decimalPlaces"] != null)
						((PercentField)field).DecimalPlaces = (byte?)data["decimalPlaces"];
					break;
				case FieldType.PhoneField:
					field = new PhoneField();
					if (HasKey(data, "defaultValue") && data["defaultValue"] != null)
						((PhoneField)field).DefaultValue = (string)data["defaultValue"];
					if (HasKey(data, "format") && data["format"] != null)
						((PhoneField)field).Format = (string)data["format"];
					if (HasKey(data, "maxLength") && data["maxLength"] != null)
						((PhoneField)field).DefaultValue = (string)data["maxLength"];
					break;
				case FieldType.SelectField:
					field = new SelectField();
					if (HasKey(data, "defaultValue") && data["defaultValue"] != null)
						((SelectField)field).DefaultValue = (string)data["defaultValue"];
					if (HasKey(data, "options") && data["options"] != null)
						((SelectField)field).Options = (List<SelectFieldOption>)data["options"];
					break;
				case FieldType.TextField:
					field = new TextField();
					if (HasKey(data, "defaultValue") && data["defaultValue"] != null)
						((TextField)field).DefaultValue = (string)data["defaultValue"];
					if (HasKey(data, "maxLength") && data["maxLength"] != null)
						((TextField)field).MaxLength = (int?)data["maxLength"];
					break;
				case FieldType.UrlField:
					field = new UrlField();
					if (HasKey(data, "defaultValue") && data["defaultValue"] != null)
						((UrlField)field).DefaultValue = (string)data["defaultValue"];
					if (HasKey(data, "maxLength") && data["maxLength"] != null)
						((UrlField)field).MaxLength = (int?)data["maxLength"];
					if (HasKey(data, "openTargetInNewWindow") && data["openTargetInNewWindow"] != null)
						((UrlField)field).OpenTargetInNewWindow = (bool?)data["openTargetInNewWindow"];
					break;
				default:
					{
						FieldResponse response = new FieldResponse();
						response.Timestamp = DateTime.UtcNow;
						response.Success = false;
						response.Message = "Not supported field type!";
						response.Success = false;
						return response;
					}
			}

			field.Id = id.HasValue && id.Value != Guid.Empty ? id.Value : Guid.NewGuid();
			field.Name = name;
			field.Label = label;
			field.PlaceholderText = placeholderText;
			field.Description = description;
			field.HelpText = helpText;
			field.Required = required;
			field.Unique = unique;
			field.Searchable = searchable;
			field.Auditable = auditable;
			field.System = system;

			return CreateField(entityId, field.MapTo<InputField>(), transactional);
		}

		public FieldResponse UpdateField(Guid entityId, InputField inputField)
		{
			FieldResponse response = new FieldResponse();

			var entityResponse = ReadEntity(entityId);

			if (!entityResponse.Success)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = entityResponse.Message;
				return response;
			}
			else if (entityResponse.Object == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such Id does not exist!";
				return response;
			}
			Entity entity = entityResponse.Object;

			return UpdateField(entity, inputField);
		}

		public FieldResponse UpdateField(Entity entity, InputField inputField)
		{
			FieldResponse response = new FieldResponse
			{
				Success = true,
				Message = "The field was successfully updated!",
			};

			Field field = null;

			try
			{
				response.Errors = ValidateField(entity, inputField, true);

				field = inputField.MapTo<Field>();

				if (response.Errors.Count > 0)
				{
					response.Object = field;
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The field was not updated. Validation error occurred!";
					return response;
				}

				Field fieldForDelete = entity.Fields.FirstOrDefault(f => f.Id == field.Id);
				if (fieldForDelete.Id == field.Id)
					entity.Fields.Remove(fieldForDelete);

				entity.Fields.Add(field);

				DbContext.Current.RecordRepository.UpdateRecordField(entity.Name, field);

				DbEntity updatedEntity = entity.MapTo<DbEntity>();
				bool result = DbContext.Current.EntityRepository.Update(updatedEntity);
				if (!result)
				{
					Cache.ClearEntities();
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The field was not updated! An internal error occurred!";
					return response;
				}

			}
			catch (Exception e)
			{
				Cache.ClearEntities();
				response.Success = false;
				response.Object = field;
				response.Timestamp = DateTime.UtcNow;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The field was not updated. An internal error occurred!";
#endif
				return response;
			}

			Cache.ClearEntities();

			response.Object = field;
			response.Timestamp = DateTime.UtcNow;

			return response;
		}

		//		public FieldResponse PartialUpdateField(Guid entityId, Guid id, InputField inputField)
		//		{
		//			FieldResponse response = new FieldResponse
		//			{
		//				Success = true,
		//				Message = "The field was successfully updated!",
		//			};

		//			Field updatedField = null;

		//			try
		//			{
		//				IStorageEntity storageEntity = EntityRepository.Read(entityId);

		//				if (storageEntity == null)
		//				{
		//					response.Timestamp = DateTime.UtcNow;
		//					response.Success = false;
		//					response.Message = "Entity with such Id does not exist!";
		//					return response;
		//				}

		//				Entity entity = storageEntity.MapTo<Entity>();

		//				updatedField = entity.Fields.FirstOrDefault(f => f.Id == id);

		//				if (updatedField == null)
		//				{
		//					response.Timestamp = DateTime.UtcNow;
		//					response.Success = false;
		//					response.Message = "Field with such Id does not exist!";
		//					return response;
		//				}

		//				if (updatedField is AutoNumberField)
		//				{
		//					if (((InputAutoNumberField)inputField).DefaultValue != null)
		//						((AutoNumberField)updatedField).DefaultValue = ((InputAutoNumberField)inputField).DefaultValue;
		//					if (((InputAutoNumberField)inputField).DisplayFormat != null)
		//						((AutoNumberField)updatedField).DisplayFormat = ((InputAutoNumberField)inputField).DisplayFormat;
		//					if (((InputAutoNumberField)inputField).StartingNumber != null)
		//						((AutoNumberField)updatedField).StartingNumber = ((InputAutoNumberField)inputField).StartingNumber;
		//				}
		//				else if (updatedField is CheckboxField)
		//				{
		//					if (((InputCheckboxField)inputField).DefaultValue != null)
		//						((CheckboxField)updatedField).DefaultValue = ((InputCheckboxField)inputField).DefaultValue;
		//				}
		//				else if (updatedField is CurrencyField)
		//				{
		//					if (((InputCurrencyField)inputField).DefaultValue != null)
		//						((CurrencyField)updatedField).DefaultValue = ((InputCurrencyField)inputField).DefaultValue;
		//					if (((InputCurrencyField)inputField).MinValue != null)
		//						((CurrencyField)updatedField).MinValue = ((InputCurrencyField)inputField).MinValue;
		//					if (((InputCurrencyField)inputField).MaxValue != null)
		//						((CurrencyField)updatedField).MaxValue = ((InputCurrencyField)inputField).MaxValue;
		//					if (((InputCurrencyField)inputField).Currency != null)
		//						((CurrencyField)updatedField).Currency = ((InputCurrencyField)inputField).Currency;
		//				}
		//				else if (updatedField is DateField)
		//				{
		//					if (((InputDateField)inputField).DefaultValue != null)
		//						((DateField)updatedField).DefaultValue = ((InputDateField)inputField).DefaultValue;
		//					if (((InputDateField)inputField).Format != null)
		//						((DateField)updatedField).Format = ((InputDateField)inputField).Format;
		//					if (((InputDateField)inputField).UseCurrentTimeAsDefaultValue != null)
		//						((DateField)updatedField).UseCurrentTimeAsDefaultValue = ((InputDateField)inputField).UseCurrentTimeAsDefaultValue;
		//				}
		//				else if (updatedField is DateTimeField)
		//				{
		//					if (((InputDateTimeField)inputField).DefaultValue != null)
		//						((DateTimeField)updatedField).DefaultValue = ((InputDateTimeField)inputField).DefaultValue;
		//					if (((InputDateTimeField)inputField).Format != null)
		//						((DateTimeField)updatedField).Format = ((InputDateTimeField)inputField).Format;
		//					if (((InputDateTimeField)inputField).UseCurrentTimeAsDefaultValue != null)
		//						((DateTimeField)updatedField).UseCurrentTimeAsDefaultValue = ((InputDateTimeField)inputField).UseCurrentTimeAsDefaultValue;
		//				}
		//				else if (updatedField is EmailField)
		//				{
		//					if (((InputEmailField)inputField).DefaultValue != null)
		//						((EmailField)updatedField).DefaultValue = ((InputEmailField)inputField).DefaultValue;
		//					if (((InputEmailField)inputField).MaxLength != null)
		//						((EmailField)updatedField).MaxLength = ((InputEmailField)inputField).MaxLength;
		//				}
		//				else if (updatedField is FileField)
		//				{
		//					if (((InputFileField)inputField).DefaultValue != null)
		//						((FileField)updatedField).DefaultValue = ((InputFileField)inputField).DefaultValue;
		//				}
		//				else if (updatedField is HtmlField)
		//				{
		//					if (((InputHtmlField)inputField).DefaultValue != null)
		//						((HtmlField)updatedField).DefaultValue = ((InputHtmlField)inputField).DefaultValue;
		//				}
		//				else if (updatedField is ImageField)
		//				{
		//					if (((InputImageField)inputField).DefaultValue != null)
		//						((ImageField)updatedField).DefaultValue = ((InputImageField)inputField).DefaultValue;
		//				}
		//				else if (updatedField is MultiLineTextField)
		//				{
		//					if (((InputMultiLineTextField)inputField).DefaultValue != null)
		//						((MultiLineTextField)updatedField).DefaultValue = ((InputMultiLineTextField)inputField).DefaultValue;
		//					if (((InputMultiLineTextField)inputField).MaxLength != null)
		//						((MultiLineTextField)updatedField).MaxLength = ((InputMultiLineTextField)inputField).MaxLength;
		//					if (((InputMultiLineTextField)inputField).VisibleLineNumber != null)
		//						((MultiLineTextField)updatedField).VisibleLineNumber = ((InputMultiLineTextField)inputField).VisibleLineNumber;
		//				}
		//				else if (updatedField is MultiSelectField)
		//				{
		//					if (((InputMultiSelectField)inputField).DefaultValue != null)
		//						((MultiSelectField)updatedField).DefaultValue = ((InputMultiSelectField)inputField).DefaultValue;
		//					if (((InputMultiSelectField)inputField).Options != null)
		//						((MultiSelectField)updatedField).Options = ((InputMultiSelectField)inputField).Options;
		//				}
		//				else if (updatedField is NumberField)
		//				{
		//					if (((InputNumberField)inputField).DefaultValue != null)
		//						((NumberField)updatedField).DefaultValue = ((InputNumberField)inputField).DefaultValue;
		//					if (((InputNumberField)inputField).MinValue != null)
		//						((NumberField)updatedField).MinValue = ((InputNumberField)inputField).MinValue;
		//					if (((InputNumberField)inputField).MaxValue != null)
		//						((NumberField)updatedField).MaxValue = ((InputNumberField)inputField).MaxValue;
		//					if (((InputNumberField)inputField).DecimalPlaces != null)
		//						((NumberField)updatedField).DecimalPlaces = ((InputNumberField)inputField).DecimalPlaces;
		//				}
		//				else if (updatedField is PasswordField)
		//				{
		//					if (((InputPasswordField)inputField).MaxLength != null)
		//						((PasswordField)updatedField).MaxLength = ((InputPasswordField)inputField).MaxLength;
		//					if (((InputPasswordField)inputField).MinLength != null)
		//						((PasswordField)updatedField).MinLength = ((InputPasswordField)inputField).MinLength;
		//					if (((InputPasswordField)inputField).Encrypted != null)
		//						((PasswordField)updatedField).Encrypted = ((InputPasswordField)inputField).Encrypted;
		//				}
		//				else if (updatedField is PercentField)
		//				{
		//					if (((InputPercentField)inputField).DefaultValue != null)
		//						((PercentField)updatedField).DefaultValue = ((InputPercentField)inputField).DefaultValue;
		//					if (((InputPercentField)inputField).MinValue != null)
		//						((PercentField)updatedField).MinValue = ((InputPercentField)inputField).MinValue;
		//					if (((InputPercentField)inputField).MaxValue != null)
		//						((PercentField)updatedField).MaxValue = ((InputPercentField)inputField).MaxValue;
		//					if (((InputPercentField)inputField).DecimalPlaces != null)
		//						((PercentField)updatedField).DecimalPlaces = ((InputPercentField)inputField).DecimalPlaces;
		//				}
		//				else if (updatedField is PhoneField)
		//				{
		//					if (((InputPhoneField)inputField).DefaultValue != null)
		//						((PhoneField)updatedField).DefaultValue = ((InputPhoneField)inputField).DefaultValue;
		//					if (((InputPhoneField)inputField).Format != null)
		//						((PhoneField)updatedField).Format = ((InputPhoneField)inputField).Format;
		//					if (((InputPhoneField)inputField).MaxLength != null)
		//						((PhoneField)updatedField).MaxLength = ((InputPhoneField)inputField).MaxLength;
		//				}
		//				else if (updatedField is GuidField)
		//				{
		//					if (((InputGuidField)inputField).DefaultValue != null)
		//						((GuidField)updatedField).DefaultValue = ((InputGuidField)inputField).DefaultValue;
		//					if (((InputGuidField)inputField).GenerateNewId != null)
		//						((GuidField)updatedField).GenerateNewId = ((InputGuidField)inputField).GenerateNewId;
		//				}
		//				else if (updatedField is SelectField)
		//				{
		//					if (((InputSelectField)inputField).DefaultValue != null)
		//						((SelectField)updatedField).DefaultValue = ((InputSelectField)inputField).DefaultValue;
		//					if (((InputSelectField)inputField).Options != null)
		//						((SelectField)updatedField).Options = ((InputSelectField)inputField).Options;
		//				}
		//				else if (updatedField is TextField)
		//				{
		//					if (((InputTextField)inputField).DefaultValue != null)
		//						((TextField)updatedField).DefaultValue = ((InputTextField)inputField).DefaultValue;
		//					if (((InputTextField)inputField).MaxLength != null)
		//						((TextField)updatedField).MaxLength = ((InputTextField)inputField).MaxLength;
		//				}
		//				else if (updatedField is UrlField)
		//				{
		//					if (((InputUrlField)inputField).DefaultValue != null)
		//						((UrlField)updatedField).DefaultValue = ((InputUrlField)inputField).DefaultValue;
		//					if (((InputUrlField)inputField).MaxLength != null)
		//						((UrlField)updatedField).MaxLength = ((InputUrlField)inputField).MaxLength;
		//					if (((InputUrlField)inputField).OpenTargetInNewWindow != null)
		//						((UrlField)updatedField).OpenTargetInNewWindow = ((InputUrlField)inputField).OpenTargetInNewWindow;
		//				}

		//				if (inputField.Label != null)
		//					updatedField.Label = inputField.Label;
		//				else if (inputField.PlaceholderText != null)
		//					updatedField.PlaceholderText = inputField.PlaceholderText;
		//				else if (inputField.Description != null)
		//					updatedField.Description = inputField.Description;
		//				else if (inputField.HelpText != null)
		//					updatedField.HelpText = inputField.HelpText;
		//				else if (inputField.Required != null)
		//					updatedField.Required = inputField.Required.Value;
		//				else if (inputField.Unique != null)
		//					updatedField.Unique = inputField.Unique.Value;
		//				else if (inputField.Searchable != null)
		//					updatedField.Searchable = inputField.Searchable.Value;
		//				else if (inputField.Auditable != null)
		//					updatedField.Auditable = inputField.Auditable.Value;
		//				else if (inputField.System != null)
		//					updatedField.System = inputField.System.Value;

		//				response.Object = updatedField;
		//				response.Errors = ValidateField(entity, updatedField.MapTo<InputField>(), true);

		//				if (response.Errors.Count > 0)
		//				{
		//					response.Timestamp = DateTime.UtcNow;
		//					response.Success = false;
		//					response.Message = "The field was not updated. Validation error occurred!";
		//					return response;
		//				}

		//				IStorageEntity updatedEntity = entity.MapTo<IStorageEntity>();
		//				bool result = EntityRepository.Update(updatedEntity);
		//				if (!result)
		//				{
		//					response.Timestamp = DateTime.UtcNow;
		//					response.Success = false;
		//					response.Message = "The field was not updated! An internal error occurred!";
		//					return response;
		//				}

		//			}
		//			catch (Exception e)
		//			{
		//				response.Success = false;
		//				response.Object = updatedField;
		//				response.Timestamp = DateTime.UtcNow;
		//#if DEBUG
		//				response.Message = e.Message + e.StackTrace;
		//#else
		//                response.Message = "The field was not updated. An internal error occurred!";
		//#endif
		//				return response;
		//			}

		//			response.Object = updatedField;
		//			response.Timestamp = DateTime.UtcNow;

		//			return response;
		//		}

		public FieldResponse DeleteField(Guid entityId, Guid id, bool transactional = true)
		{
			FieldResponse response = new FieldResponse
			{
				Success = true,
				Message = "The field was successfully deleted!",
			};

			try
			{
				var entityResponse = ReadEntity(entityId);

				if (!entityResponse.Success)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = entityResponse.Message;
					return response;
				}
				else if (entityResponse.Object == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "Entity with such Id does not exist!";
					return response;
				}
				Entity entity = entityResponse.Object;

				Field field = entity.Fields.FirstOrDefault(f => f.Id == id);

				if (field == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The field was not deleted. Validation error occurred!";
					response.Errors.Add(new ErrorModel("id", id.ToString(), "Field with such Id does not exist!"));
					return response;
				}

				//Validate if field is not included in list or view of its entity or another one. Check by ID

				#region << validation check >>
				var entityList = ReadEntities().Object;
				var validationErrors = new List<ErrorModel>();
				foreach (var entityObj in entityList)
				{
					//check views
					foreach (var viewObj in entityObj.RecordViews)
					{
						//region
						foreach(var region in viewObj.Regions) {
							foreach(var section in region.Sections) {
								foreach(var row in section.Rows) {
									foreach(var column in row.Columns) {
										foreach(var item in column.Items) {
											if(item.GetType().Name == "RecordViewFieldItem") {
												if(((RecordViewFieldItem)item).FieldId == id) {
													var error = new ErrorModel();
													error.Key = "field";
													error.Value = id.ToString();
													error.Message = "Field used in view: " + viewObj.Name + " of entity: " + entityObj.Name;
													validationErrors.Add(error);
												}
											}
										}
									}
								}
							}
						}
						//sidebar cannot have fields attached so it will not be checked

					}
					//check lists
					foreach (var listObj in entityObj.RecordLists)
					{
						foreach(var column in listObj.Columns) {
							if(column.GetType().Name == "RecordListFieldItem") {
								if(((RecordListFieldItem)column).FieldId == id) {
									var error = new ErrorModel();
									error.Key = "field";
									error.Value = id.ToString();
									error.Message = "Field is used in list: " + listObj.Name + " in entity: " + entityObj.Name;
									validationErrors.Add(error);
								}
							}
						}
					}
					//check trees
					foreach (var treeObj in entityObj.RecordTrees)
					{
						if(treeObj.NodeIdFieldId == id) {
								var error = new ErrorModel();
								error.Key = "field";
								error.Value = id.ToString();
								error.Message = "Field used as NodeIdFieldId in tree: " + treeObj.Name + " in entity: " + entityObj.Name;
								validationErrors.Add(error);
						}
						if(treeObj.NodeLabelFieldId == id) {
								var error = new ErrorModel();
								error.Key = "field";
								error.Value = id.ToString();
								error.Message = "Field used as NodeLabelFieldId in tree: " + treeObj.Name + " in entity: " + entityObj.Name;
								validationErrors.Add(error);
						}
						if(treeObj.NodeNameFieldId == id) {
								var error = new ErrorModel();
								error.Key = "field";
								error.Value = id.ToString();
								error.Message = "Field used as NodeNameFieldId in tree: " + treeObj.Name + " in entity: " + entityObj.Name;
								validationErrors.Add(error);
						}
						if(treeObj.NodeParentIdFieldId == id) {
								var error = new ErrorModel();
								error.Key = "field";
								error.Value = id.ToString();
								error.Message = "Field used as NodeParentIdFieldId in tree: " + treeObj.Name + " in entity: " + entityObj.Name;
								validationErrors.Add(error);
						}
						if(treeObj.NodeWeightFieldId == id) {
								var error = new ErrorModel();
								error.Key = "field";
								error.Value = id.ToString();
								error.Message = "Field used as NodeWeightFieldId in tree: " + treeObj.Name + " in entity: " + entityObj.Name;
								validationErrors.Add(error);
						}
					}
				}

				//Check relations
				var relations = new EntityRelationManager().Read().Object;

				foreach(var relation in relations) {
					if(relation.OriginFieldId == id) {
							var error = new ErrorModel();
							error.Key = "relation";
							error.Value = id.ToString();
							error.Message = "Field used as Origin field in relation: " + relation.Name;
							validationErrors.Add(error);						
					}
					else if(relation.TargetFieldId == id) {
							var error = new ErrorModel();
							error.Key = "relation";
							error.Value = id.ToString();
							error.Message = "Field used as Target field in relation: " + relation.Name;
							validationErrors.Add(error);						
					}
				}

				if(validationErrors.Count > 0) {
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The field was not deleted. Validation error occurred!";
					response.Errors = validationErrors;
					return response;				
				}
				#endregion

				entity.Fields.Remove(field);

				using (DbConnection con = DbContext.Current.CreateConnection())
				{
					con.BeginTransaction();
					try
					{
						DbContext.Current.RecordRepository.RemoveRecordField(entity.Name, field);

						DbEntity updatedEntity = entity.MapTo<DbEntity>();
						bool result = DbContext.Current.EntityRepository.Update(updatedEntity);
						if (!result)
						{
							response.Timestamp = DateTime.UtcNow;
							response.Success = false;
							response.Message = "The field was not updated! An internal error occurred!";
							return response;
						}

						con.CommitTransaction();
					}
					catch
					{
						con.RollbackTransaction();
						throw;
					}
				}
			}
			catch (Exception e)
			{
				Cache.ClearEntities();
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The field was not deleted. An internal error occurred!";
#endif
				return response;
			}

			Cache.ClearEntities();

			response.Timestamp = DateTime.UtcNow;
			return response;
		}

		public FieldListResponse ReadFields(Guid entityId)
		{
			FieldListResponse response = new FieldListResponse
			{
				Success = true,
				Message = "The field was successfully returned!",
			};

			try
			{
				var entityResponse = ReadEntity(entityId);

				if (!entityResponse.Success)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = entityResponse.Message;
					return response;
				}
				else if (entityResponse.Object == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "Entity with such Id does not exist!";
					return response;
				}
				Entity entity = entityResponse.Object;

				FieldList fieldList = new FieldList();
				fieldList.Fields = entity.Fields;

				response.Object = fieldList;
			}
			catch (Exception e)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "An internal error occurred!";
#endif
				return response;
			}

			response.Timestamp = DateTime.Now;

			return response;
		}

		public FieldListResponse ReadFields()
		{
			FieldListResponse response = new FieldListResponse
			{
				Success = true,
				Message = "The field was successfully returned!",
			};

			try
			{
				var entitiesResponse = ReadEntities();
				if (!entitiesResponse.Success)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = entitiesResponse.Message;
					return response;
				}
				else if (entitiesResponse.Object == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "There is no entities into database!";
					return response;
				}
				List<Entity> entities = entitiesResponse.Object;

				FieldList fieldList = new FieldList();

				foreach (Entity entity in entities)
				{
					fieldList.Fields.AddRange(entity.Fields.MapTo<Field>());
				}

				response.Object = fieldList;
			}
			catch (Exception e)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "An internal error occurred!";
#endif
				return response;
			}

			response.Timestamp = DateTime.Now;

			return response;
		}

		public FieldResponse ReadField(Guid entityId, Guid id)
		{
			FieldResponse response = new FieldResponse
			{
				Success = true,
				Message = "The field was successfully returned!",
			};

			try
			{
				var entityResponse = ReadEntity(entityId);

				if (!entityResponse.Success)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = entityResponse.Message;
					return response;
				}
				else if (entityResponse.Object == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "Entity with such Id does not exist!";
					return response;
				}
				Entity entity = entityResponse.Object;
				Field field = entity.Fields.FirstOrDefault(f => f.Id == id);

				if (field == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "Validation error occurred!";
					response.Errors.Add(new ErrorModel("id", id.ToString(), "Field with such Id does not exist!"));
					return response;
				}

				response.Object = field;
			}
			catch (Exception e)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "An internal error occurred!";
#endif
				return response;
			}

			response.Timestamp = DateTime.Now;

			return response;
		}

		#endregion

		#region << RecordsList methods >>

		public RecordListResponse CreateRecordList(Guid entityId, InputRecordList inputRecordList)
		{
			RecordListResponse response = new RecordListResponse();

			var entityResponse = ReadEntity(entityId);

			if (!entityResponse.Success)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = entityResponse.Message;
				return response;
			}
			else if (entityResponse.Object == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such Id does not exist!";
				return response;
			}
			Entity entity = entityResponse.Object;


			return CreateRecordList(entity, inputRecordList);
		}

		public RecordListResponse CreateRecordList(string entityName, InputRecordList inputRecordList)
		{
			RecordListResponse response = new RecordListResponse();

			var entityResponse = ReadEntity(entityName);

			if (!entityResponse.Success)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = entityResponse.Message;
				return response;
			}
			else if (entityResponse.Object == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such Id does not exist!";
				return response;
			}

			Entity entity = entityResponse.Object;

			return CreateRecordList(entity, inputRecordList);
		}

		private RecordListResponse CreateRecordList(Entity entity, InputRecordList inputRecordList)
		{
			RecordListResponse response = new RecordListResponse
			{
				Success = true,
				Message = "The list was successfully created!",
			};


			if (!inputRecordList.Id.HasValue)
				inputRecordList.Id = Guid.NewGuid();

			RecordList recordList = inputRecordList.MapTo<RecordList>();

			try
			{
				response.Object = recordList;
				response.Errors = ValidateRecordList(entity, inputRecordList, true);

				recordList = inputRecordList.MapTo<RecordList>();

				if (response.Errors.Count > 0)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The list was not created. Validation error occurred!";
					return response;
				}

				entity.RecordLists.Add(recordList);

				DbEntity updatedEntity = entity.MapTo<DbEntity>();
				bool result = DbContext.Current.EntityRepository.Update(updatedEntity);
				if (!result)
				{
					Cache.ClearEntities();
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The list was not created! An internal error occurred!";
					return response;
				}

			}
			catch (Exception e)
			{
				Cache.ClearEntities();
				response.Success = false;
				response.Object = recordList;
				response.Timestamp = DateTime.UtcNow;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The list was not created. An internal error occurred!";
#endif
				return response;
			}

			Cache.ClearEntities();

			return ReadRecordList(entity.Id, recordList.Id);
		}

		public RecordListResponse UpdateRecordList(Guid entityId, InputRecordList inputRecordList)
		{
			RecordListResponse response = new RecordListResponse();

			var entityResponse = ReadEntity(entityId);

			if (!entityResponse.Success)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = entityResponse.Message;
				return response;
			}
			else if (entityResponse.Object == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such Id does not exist!";
				return response;
			}
			Entity entity = entityResponse.Object;

			return UpdateRecordList(entity, inputRecordList);
		}

		public RecordListResponse UpdateRecordList(string entityName, InputRecordList inputRecordList)
		{
			RecordListResponse response = new RecordListResponse();

			var entityResponse = ReadEntity(entityName);

			if (!entityResponse.Success)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = entityResponse.Message;
				return response;
			}
			else if (entityResponse.Object == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such Id does not exist!";
				return response;
			}
			Entity entity = entityResponse.Object;

			return UpdateRecordList(entity, inputRecordList);
		}

		public RecordListResponse UpdateRecordList(Entity entity, InputRecordList inputRecordList)
		{
			RecordListResponse response = new RecordListResponse
			{
				Success = true,
				Message = "The list was successfully updated!",
			};

			RecordList recordList = inputRecordList.MapTo<RecordList>();

			try
			{
				response.Object = recordList;
				response.Errors = ValidateRecordList(entity, inputRecordList, false);

				recordList = inputRecordList.MapTo<RecordList>();

				if (response.Errors.Count > 0)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The list was not updated. Validation error occurred!";
					return response;
				}

				RecordList listForDelete = entity.RecordLists.FirstOrDefault(r => r.Id == recordList.Id);
				if (listForDelete.Id == recordList.Id)
					entity.RecordLists.Remove(listForDelete);

				entity.RecordLists.Add(recordList);

				DbEntity updatedEntity = entity.MapTo<DbEntity>();
				bool result = DbContext.Current.EntityRepository.Update(updatedEntity);
				if (!result)
				{
					Cache.ClearEntities();
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The list was not updated! An internal error occurred!";
					return response;
				}

			}
			catch (Exception e)
			{
				Cache.ClearEntities();
				response.Success = false;
				response.Object = recordList;
				response.Timestamp = DateTime.UtcNow;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The list was not updated. An internal error occurred!";
#endif
				return response;
			}

			Cache.ClearEntities();

			return ReadRecordList(entity.Id, recordList.Id);
		}

		public RecordListResponse DeleteRecordList(Guid entityId, Guid id)
		{
			RecordListResponse response = new RecordListResponse
			{
				Success = true,
				Message = "The list was successfully deleted!",
			};

			try
			{
				var entityResponse = ReadEntity(entityId);

				if (!entityResponse.Success)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = entityResponse.Message;
					return response;
				}
				else if (entityResponse.Object == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "Entity with such Id does not exist!";
					return response;
				}
				Entity entity = entityResponse.Object;

				RecordList recordList = entity.RecordLists.FirstOrDefault(v => v.Id == id);

				if (recordList == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The list was not deleted. Validation error occurred!";
					response.Errors.Add(new ErrorModel("id", id.ToString(), "List with such Id does not exist!"));
					return response;
				}

				//Validate if list is not included in list or view of its entity or another one. Check by ID

				#region << validation check >>
				var entityList = ReadEntities().Object;
				var validationErrors = new List<ErrorModel>();
				foreach (var entityObj in entityList)
				{
					//check views
					foreach (var viewObj in entityObj.RecordViews)
					{
						//region
						foreach(var region in viewObj.Regions) {
							foreach(var section in region.Sections) {
								foreach(var row in section.Rows) {
									foreach(var column in row.Columns) {
										foreach(var item in column.Items) {
											if(item.GetType().Name == "RecordViewListItem") {
												if(((RecordViewListItem)item).ListId == id) {
													var error = new ErrorModel();
													error.Key = "list";
													error.Value = id.ToString();
													error.Message = "List is used in view: " + viewObj.Name + " of entity: " + entityObj.Name;
													validationErrors.Add(error);
												}
											}
										}
									}
								}
							}
						}
						//sidebar
						foreach(var sidebarItem in viewObj.Sidebar.Items) {
							if(sidebarItem.GetType().Name == "RecordViewSidebarListItem") {
								if(((RecordViewSidebarListItem)sidebarItem).ListId == id) {
									var error = new ErrorModel();
									error.Key = "list";
									error.Value = id.ToString();
									error.Message = "List is used in the sidebar of view: " + viewObj.Name + " of entity: " + entityObj.Name;
									validationErrors.Add(error);
								}								
							}
						}
					}					

					//check lists
					foreach (var listObj in entityObj.RecordLists)
					{
						foreach(var column in listObj.Columns) {
							if(column.GetType().Name == "RecordListListItem") {
								if(((RecordListListItem)column).ListId == id) {
									var error = new ErrorModel();
									error.Key = "list";
									error.Value = id.ToString();
									error.Message = "List is used in list: " + listObj.Name + " in entity: " + entityObj.Name;
									validationErrors.Add(error);
								}
							}
						}
					}

				}


				if(validationErrors.Count > 0) {
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The list was not deleted. Validation error occurred!";
					response.Errors = validationErrors;
					return response;				
				}

				#endregion

				entity.RecordLists.Remove(recordList);

				DbEntity updatedEntity = entity.MapTo<DbEntity>();
				bool result = DbContext.Current.EntityRepository.Update(updatedEntity);
				if (!result)
				{
					Cache.ClearEntities();
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The list was not updated! An internal error occurred!";
					return response;
				}
			}
			catch (Exception e)
			{
				Cache.ClearEntities();
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The list was not deleted. An internal error occurred!";
#endif
				return response;
			}

			Cache.ClearEntities();

			response.Timestamp = DateTime.UtcNow;
			return response;
		}

		public RecordListResponse DeleteRecordList(string entityName, string name)
		{
			RecordListResponse response = new RecordListResponse
			{
				Success = true,
				Message = "The list was successfully deleted!",
			};

			try
			{
				var entityResponse = ReadEntity(entityName);

				if (!entityResponse.Success)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = entityResponse.Message;
					return response;
				}
				else if (entityResponse.Object == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "Entity with such Id does not exist!";
					return response;
				}
				Entity entity = entityResponse.Object;

				RecordList recordList = entity.RecordLists.FirstOrDefault(l => l.Name == name);

				if (recordList == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The list was not deleted. Validation error occurred!";
					response.Errors.Add(new ErrorModel("name", name, "List with such Name does not exist!"));
					return response;
				}


				//Validate if list is not included in list or view of its entity or another one. Check by ID
				var id = recordList.Id;
				#region << validation check >>
				var entityList = ReadEntities().Object;
				var validationErrors = new List<ErrorModel>();
				foreach (var entityObj in entityList)
				{
					//check views
					foreach (var viewObj in entityObj.RecordViews)
					{
						//region
						foreach(var region in viewObj.Regions) {
							foreach(var section in region.Sections) {
								foreach(var row in section.Rows) {
									foreach(var column in row.Columns) {
										foreach(var item in column.Items) {
											if(item.GetType().Name == "RecordViewListItem") {
												if(((RecordViewListItem)item).ListId == id) {
													var error = new ErrorModel();
													error.Key = "list";
													error.Value = id.ToString();
													error.Message = "List is used in view: " + viewObj.Name + " of entity: " + entityObj.Name;
													validationErrors.Add(error);
												}
											}
										}
									}
								}
							}
						}
						//sidebar
						foreach(var sidebarItem in viewObj.Sidebar.Items) {
							if(sidebarItem.GetType().Name == "RecordViewSidebarListItem") {
								if(((RecordViewSidebarListItem)sidebarItem).ListId == id) {
									var error = new ErrorModel();
									error.Key = "list";
									error.Value = id.ToString();
									error.Message = "List is used in the sidebar of view: " + viewObj.Name + " of entity: " + entityObj.Name;
									validationErrors.Add(error);
								}								
							}
						}
					}					

					//check lists
					foreach (var listObj in entityObj.RecordLists)
					{
						foreach(var column in listObj.Columns) {
							if(column.GetType().Name == "RecordListListItem") {
								if(((RecordListListItem)column).ListId == id) {
									var error = new ErrorModel();
									error.Key = "list";
									error.Value = id.ToString();
									error.Message = "List is used in list: " + listObj.Name + " in entity: " + entityObj.Name;
									validationErrors.Add(error);
								}
							}
						}
					}

				}


				if(validationErrors.Count > 0) {
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The list was not deleted. Validation error occurred!";
					response.Errors = validationErrors;
					return response;				
				}

				#endregion


				entity.RecordLists.Remove(recordList);

				DbEntity updatedEntity = entity.MapTo<DbEntity>();
				bool result = DbContext.Current.EntityRepository.Update(updatedEntity);
				if (!result)
				{
					Cache.ClearEntities();
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The list was not updated! An internal error occurred!";
					return response;
				}
			}
			catch (Exception e)
			{
				Cache.ClearEntities();
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The list was not deleted. An internal error occurred!";
#endif
				return response;
			}

			Cache.ClearEntities();

			response.Timestamp = DateTime.UtcNow;
			return response;
		}

		public RecordListCollectionResponse ReadRecordLists(Guid entityId)
		{
			RecordListCollectionResponse response = new RecordListCollectionResponse();

			EntityResponse entityResponse = ReadEntity(entityId);

			if (!entityResponse.Success)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = entityResponse.Message;
				return response;
			}
			else if (entityResponse.Object == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such Id does not exist!";
				return response;
			}

			Entity entity = entityResponse.Object;

			return ReadRecordLists(entity);
		}

		public RecordListCollectionResponse ReadRecordLists(string entityName)
		{
			RecordListCollectionResponse response = new RecordListCollectionResponse();

			EntityResponse entityResponse = ReadEntity(entityName);

			if (!entityResponse.Success)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = entityResponse.Message;
				return response;
			}
			else if (entityResponse.Object == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such name does not exist!";
				return response;
			}

			Entity entity = entityResponse.Object;

			return ReadRecordLists(entity);
		}

		public RecordListCollectionResponse ReadRecordLists(Entity entity)
		{
			RecordListCollectionResponse response = new RecordListCollectionResponse
			{
				Success = true,
				Message = "The lists were successfully returned!",
			};

			try
			{
				RecordListCollection recordListCollection = new RecordListCollection();
				recordListCollection.RecordLists = entity.RecordLists;

				response.Object = recordListCollection;
			}
			catch (Exception e)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "An internal error occurred!";
#endif
				return response;
			}

			response.Timestamp = DateTime.Now;

			return response;
		}

		public RecordListResponse ReadRecordList(Guid entityId, Guid id)
		{
			RecordListResponse response = new RecordListResponse();

			EntityResponse entityResponse = ReadEntity(entityId);

			if (!entityResponse.Success)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = entityResponse.Message;
				return response;
			}
			else if (entityResponse.Object == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such Id does not exist!";
				return response;
			}

			Entity entity = entityResponse.Object;

			RecordList recordList = entity.RecordLists.FirstOrDefault(r => r.Id == id);

			if (recordList == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Record List with such Id does not exist!";
				return response;
			}


			return ReadRecordList(entity, recordList);
		}

		public RecordListResponse ReadRecordList(string entityName, string name)
		{
			RecordListResponse response = new RecordListResponse();

			EntityResponse entityResponse = ReadEntity(entityName);

			if (!entityResponse.Success)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = entityResponse.Message;
				return response;
			}
			else if (entityResponse.Object == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such name does not exist!";
				return response;
			}

			Entity entity = entityResponse.Object;

			RecordList recordList = entity.RecordLists.FirstOrDefault(r => r.Name == name);

			if (recordList == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Record List with such Name does not exist!";
				return response;
			}

			return ReadRecordList(entity, recordList);
		}

		private RecordListResponse ReadRecordList(Entity entity, RecordList recordList)
		{
			RecordListResponse response = new RecordListResponse
			{
				Success = true,
				Message = "The list was successfully returned!",
			};

			try
			{
				response.Object = recordList;
			}
			catch (Exception e)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "An internal error occurred!";
#endif
				return response;
			}

			response.Timestamp = DateTime.Now;

			return response;
		}

		public RecordListCollectionResponse ReadRecordLists()
		{
			RecordListCollectionResponse response = new RecordListCollectionResponse
			{
				Success = true,
				Message = "The lists were successfully returned!",
			};

			try
			{
				var entitiesResponse = ReadEntities();

				if (!entitiesResponse.Success)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = entitiesResponse.Message;
					return response;
				}
				else if (entitiesResponse.Object == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "There is no entities into database!";
					return response;
				}

				List<Entity> entities = entitiesResponse.Object;


				RecordListCollection recordListCollection = new RecordListCollection();
				recordListCollection.RecordLists = new List<RecordList>();

				foreach (Entity entity in entities)
				{
					recordListCollection.RecordLists.AddRange(entity.RecordLists);
				}

				response.Object = recordListCollection;
			}
			catch (Exception e)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "An internal error occurred!";
#endif
				return response;
			}

			response.Timestamp = DateTime.Now;

			return response;
		}

		#endregion

		#region << RecordView methods >>

		public RecordViewResponse CreateRecordView(Guid entityId, InputRecordView inputRecordView)
		{
			RecordViewResponse response = new RecordViewResponse();

			var entityResponse = ReadEntity(entityId);

			if (!entityResponse.Success)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = entityResponse.Message;
				return response;
			}
			else if (entityResponse.Object == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such Id does not exist!";
				return response;
			}

			Entity entity = entityResponse.Object;

			return CreateRecordView(entity, inputRecordView);
		}

		public RecordViewResponse CreateRecordView(string entityName, InputRecordView inputRecordView)
		{
			RecordViewResponse response = new RecordViewResponse();

			var entityResponse = ReadEntity(entityName);

			if (!entityResponse.Success)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = entityResponse.Message;
				return response;
			}
			else if (entityResponse.Object == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such Id does not exist!";
				return response;
			}

			Entity entity = entityResponse.Object;


			return CreateRecordView(entity, inputRecordView);
		}

		private RecordViewResponse CreateRecordView(Entity entity, InputRecordView inputRecordView)
		{
			RecordViewResponse response = new RecordViewResponse
			{
				Success = true,
				Message = "The record view was successfully created!",
			};

			if (!inputRecordView.Id.HasValue)
				inputRecordView.Id = Guid.NewGuid();

			RecordView recordView = inputRecordView.MapTo<RecordView>();

			try
			{
				response.Object = recordView;
				response.Errors = ValidateRecordView(entity, inputRecordView, true);

				recordView = inputRecordView.MapTo<RecordView>();

				if (response.Errors.Count > 0)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The record view was not created. Validation error occurred!";
					return response;
				}

				entity.RecordViews.Add(recordView);

				DbEntity updatedEntity = entity.MapTo<DbEntity>();
				bool result = DbContext.Current.EntityRepository.Update(updatedEntity);
				if (!result)
				{
					Cache.ClearEntities();
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The record view was not created! An internal error occurred!";
					return response;
				}
			}
			catch (Exception e)
			{
				Cache.ClearEntities();
				response.Success = false;
				response.Object = recordView;
				response.Timestamp = DateTime.UtcNow;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The record view was not created. An internal error occurred!";
#endif
				return response;
			}

			Cache.ClearEntities();

			return ReadRecordView(entity.Id, recordView.Id);
		}

		public RecordViewResponse UpdateRecordView(Guid entityId, InputRecordView inputRecordView)
		{
			RecordViewResponse response = new RecordViewResponse();

			var entityResponse = ReadEntity(entityId);

			if (!entityResponse.Success)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = entityResponse.Message;
				return response;
			}
			else if (entityResponse.Object == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such Id does not exist!";
				return response;
			}

			Entity entity = entityResponse.Object;


			return UpdateRecordView(entity, inputRecordView);
		}

		public RecordViewResponse UpdateRecordView(string entityName, InputRecordView inputRecordView)
		{
			RecordViewResponse response = new RecordViewResponse();

			var entityResponse = ReadEntity(entityName);

			if (!entityResponse.Success)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = entityResponse.Message;
				return response;
			}
			else if (entityResponse.Object == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such Id does not exist!";
				return response;
			}

			Entity entity = entityResponse.Object;


			return UpdateRecordView(entity, inputRecordView);
		}

		public RecordViewResponse UpdateRecordView(Entity entity, InputRecordView inputRecordView)
		{
			RecordViewResponse response = new RecordViewResponse
			{
				Success = true,
				Message = "The record view was successfully updated!",
			};

			RecordView recordView = inputRecordView.MapTo<RecordView>();

			try
			{
				response.Object = recordView;
				response.Errors = ValidateRecordView(entity, inputRecordView, false);

				recordView = inputRecordView.MapTo<RecordView>();

				if (response.Errors.Count > 0)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The record view was not updated. Validation error occurred!";
					return response;
				}

				RecordView recordViewForDelete = entity.RecordViews.FirstOrDefault(r => r.Id == recordView.Id);
				if (recordViewForDelete.Id == recordView.Id)
					entity.RecordViews.Remove(recordViewForDelete);

				entity.RecordViews.Add(recordView);

				DbEntity updatedEntity = entity.MapTo<DbEntity>();
				bool result = DbContext.Current.EntityRepository.Update(updatedEntity);
				if (!result)
				{
					Cache.ClearEntities();
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The record view was not updated! An internal error occurred!";
					return response;
				}

			}
			catch (Exception e)
			{
				Cache.ClearEntities();
				response.Success = false;
				response.Object = recordView;
				response.Timestamp = DateTime.UtcNow;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The record view was not updated. An internal error occurred!";
#endif
				return response;
			}

			Cache.ClearEntities();

			return ReadRecordView(entity.Id, recordView.Id);
		}

		public RecordViewResponse DeleteRecordView(Guid entityId, Guid id)
		{
			RecordViewResponse response = new RecordViewResponse
			{
				Success = true,
				Message = "The record view was successfully deleted!",
			};

			try
			{
				var entityResponse = ReadEntity(entityId);

				if (!entityResponse.Success)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = entityResponse.Message;
					return response;
				}
				else if (entityResponse.Object == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "Entity with such Id does not exist!";
					return response;
				}

				Entity entity = entityResponse.Object;

				RecordView recordView = entity.RecordViews.FirstOrDefault(r => r.Id == id);

				if (recordView == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The record view was not deleted. Validation error occurred!";
					response.Errors.Add(new ErrorModel("id", id.ToString(), "Record view with such Id does not exist!"));
					return response;
				}

				//Validate if view is not included in list or view of its entity or another one. Check by ID

				#region << validation check >>
				var entityList = ReadEntities().Object;
				var validationErrors = new List<ErrorModel>();
				foreach (var entityObj in entityList)
				{
					//check views
					foreach (var viewObj in entityObj.RecordViews)
					{
						//region
						foreach(var region in viewObj.Regions) {
							foreach(var section in region.Sections) {
								foreach(var row in section.Rows) {
									foreach(var column in row.Columns) {
										foreach(var item in column.Items) {
											if(item.GetType().Name == "RecordViewViewItem") {
												if(((RecordViewViewItem)item).ViewId == id) {
													var error = new ErrorModel();
													error.Key = "view";
													error.Value = id.ToString();
													error.Message = "View is used in view: " + viewObj.Name + " of entity: " + entityObj.Name;
													validationErrors.Add(error);
												}
											}
										}
									}
								}
							}
						}
						//sidebar
						foreach(var sidebarItem in viewObj.Sidebar.Items) {
							if(sidebarItem.GetType().Name == "RecordViewSidebarViewItem") {
								if(((RecordViewSidebarViewItem)sidebarItem).ViewId == id) {
									var error = new ErrorModel();
									error.Key = "view";
									error.Value = id.ToString();
									error.Message = "View is used in the sidebar of view: " + viewObj.Name + " of entity: " + entityObj.Name;
									validationErrors.Add(error);
								}								
							}
						}
					}					

					//check lists
					foreach (var listObj in entityObj.RecordLists)
					{
						foreach(var column in listObj.Columns) {
							if(column.GetType().Name == "RecordListViewItem") {
								if(((RecordListViewItem)column).ViewId == id) {
									var error = new ErrorModel();
									error.Key = "view";
									error.Value = id.ToString();
									error.Message = "View is used in list: " + listObj.Name + " in entity: " + entityObj.Name;
									validationErrors.Add(error);
								}
							}
						}
					}
				}

				if(validationErrors.Count > 0) {
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The view was not deleted. Validation error occurred!";
					response.Errors = validationErrors;
					return response;				
				}

				#endregion

				entity.RecordViews.Remove(recordView);

				DbEntity updatedEntity = entity.MapTo<DbEntity>();
				bool result = DbContext.Current.EntityRepository.Update(updatedEntity);
				if (!result)
				{
					Cache.ClearEntities();
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The record view was not updated! An internal error occurred!";
					return response;
				}
			}
			catch (Exception e)
			{
				Cache.ClearEntities();
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The record view was not deleted. An internal error occurred!";
#endif
				return response;
			}

			Cache.ClearEntities();

			response.Timestamp = DateTime.UtcNow;
			return response;
		}

		public RecordViewResponse DeleteRecordView(string entityName, string name)
		{
			RecordViewResponse response = new RecordViewResponse
			{
				Success = true,
				Message = "The record view was successfully deleted!",
			};

			try
			{
				var entityResponse = ReadEntity(entityName);

				if (!entityResponse.Success)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = entityResponse.Message;
					return response;
				}
				else if (entityResponse.Object == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "Entity with such Id does not exist!";
					return response;
				}

				Entity entity = entityResponse.Object;

				RecordView recordView = entity.RecordViews.FirstOrDefault(r => r.Name == name);

				var id = recordView.Id;

				if (recordView == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The record view was not deleted. Validation error occurred!";
					response.Errors.Add(new ErrorModel("name", name, "Record view with such Name does not exist!"));
					return response;
				}

				#region << validation check >>
				var entityList = ReadEntities().Object;
				var validationErrors = new List<ErrorModel>();
				foreach (var entityObj in entityList)
				{
					//check views
					foreach (var viewObj in entityObj.RecordViews)
					{
						//region
						foreach(var region in viewObj.Regions) {
							foreach(var section in region.Sections) {
								foreach(var row in section.Rows) {
									foreach(var column in row.Columns) {
										foreach(var item in column.Items) {
											if(item.GetType().Name == "RecordViewViewItem") {
												if(((RecordViewViewItem)item).ViewId == id) {
													var error = new ErrorModel();
													error.Key = "view";
													error.Value = id.ToString();
													error.Message = "View is used in view: " + viewObj.Name + " of entity: " + entityObj.Name;
													validationErrors.Add(error);
												}
											}
										}
									}
								}
							}
						}
						//sidebar
						foreach(var sidebarItem in viewObj.Sidebar.Items) {
							if(sidebarItem.GetType().Name == "RecordViewSidebarViewItem") {
								if(((RecordViewSidebarViewItem)sidebarItem).ViewId == id) {
									var error = new ErrorModel();
									error.Key = "view";
									error.Value = id.ToString();
									error.Message = "View is used in the sidebar of view: " + viewObj.Name + " of entity: " + entityObj.Name;
									validationErrors.Add(error);
								}								
							}
						}
					}					

					//check lists
					foreach (var listObj in entityObj.RecordLists)
					{
						foreach(var column in listObj.Columns) {
							if(column.GetType().Name == "RecordListViewItem") {
								if(((RecordListViewItem)column).ViewId == id) {
									var error = new ErrorModel();
									error.Key = "view";
									error.Value = id.ToString();
									error.Message = "View is used in list: " + listObj.Name + " in entity: " + entityObj.Name;
									validationErrors.Add(error);
								}
							}
						}
					}
				}

				if(validationErrors.Count > 0) {
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The view was not deleted. Validation error occurred!";
					response.Errors = validationErrors;
					return response;				
				}

				#endregion

				entity.RecordViews.Remove(recordView);

				DbEntity updatedEntity = entity.MapTo<DbEntity>();
				bool result = DbContext.Current.EntityRepository.Update(updatedEntity);
				if (!result)
				{
					Cache.ClearEntities();
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The record view was not updated! An internal error occurred!";
					return response;
				}
			}
			catch (Exception e)
			{
				Cache.ClearEntities();
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The record view was not deleted. An internal error occurred!";
#endif
				return response;
			}

			Cache.ClearEntities();

			response.Timestamp = DateTime.UtcNow;
			return response;
		}

		public RecordViewCollectionResponse ReadRecordViews(Guid entityId)
		{
			RecordViewCollectionResponse response = new RecordViewCollectionResponse();

			EntityResponse entityResponse = ReadEntity(entityId);

			if (!entityResponse.Success)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = entityResponse.Message;
				return response;
			}
			else if (entityResponse.Object == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such Id does not exist!";
				return response;
			}

			Entity entity = entityResponse.Object;

			return ReadRecordViews(entity);
		}

		public RecordViewCollectionResponse ReadRecordViews(string entityName)
		{
			RecordViewCollectionResponse response = new RecordViewCollectionResponse();

			EntityResponse entityResponse = ReadEntity(entityName);

			if (!entityResponse.Success)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = entityResponse.Message;
				return response;
			}
			else if (entityResponse.Object == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such name does not exist!";
				return response;
			}

			Entity entity = entityResponse.Object;


			return ReadRecordViews(entity);
		}

		private RecordViewCollectionResponse ReadRecordViews(Entity entity)
		{
			RecordViewCollectionResponse response = new RecordViewCollectionResponse
			{
				Success = true,
				Message = "The record views were successfully returned!",
			};

			try
			{
				RecordViewCollection recordViewList = new RecordViewCollection();
				recordViewList.RecordViews = entity.RecordViews;

				response.Object = recordViewList;
			}
			catch (Exception e)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "An internal error occurred!";
#endif
				return response;
			}

			response.Timestamp = DateTime.Now;

			return response;
		}

		public RecordViewResponse ReadRecordView(Guid entityId, Guid id)
		{
			RecordViewResponse response = new RecordViewResponse();

			EntityResponse entityResponse = ReadEntity(entityId);

			if (!entityResponse.Success)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = entityResponse.Message;
				return response;
			}
			else if (entityResponse.Object == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such Id does not exist!";
				return response;
			}

			Entity entity = entityResponse.Object;

			RecordView recordView = entity.RecordViews.FirstOrDefault(r => r.Id == id);

			if (recordView == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Record View with such Id does not exist!";
				return response;
			}


			return ReadRecordView(entity, recordView);
		}

		public RecordViewResponse ReadRecordView(string entityName, string name)
		{
			RecordViewResponse response = new RecordViewResponse();

			EntityResponse entityResponse = ReadEntity(entityName);

			if (!entityResponse.Success)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = entityResponse.Message;
				return response;
			}
			else if (entityResponse.Object == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such name does not exist!";
				return response;
			}

			Entity entity = entityResponse.Object;

			RecordView recordView = entity.RecordViews.FirstOrDefault(r => r.Name == name);

			if (recordView == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Record View with such Name does not exist!";
				return response;
			}

			return ReadRecordView(entity, recordView);
		}

		private RecordViewResponse ReadRecordView(Entity entity, RecordView recordView)
		{
			RecordViewResponse response = new RecordViewResponse
			{
				Success = true,
				Message = "The record view was successfully returned!",
			};

			try
			{
				response.Object = recordView;
			}
			catch (Exception e)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "An internal error occurred!";
#endif
				return response;
			}

			response.Timestamp = DateTime.Now;

			return response;
		}

		public RecordViewCollectionResponse ReadRecordViews()
		{
			RecordViewCollectionResponse response = new RecordViewCollectionResponse
			{
				Success = true,
				Message = "The record views were successfully returned!",
			};

			try
			{
				var entitiesResponse = ReadEntities();

				if (!entitiesResponse.Success)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = entitiesResponse.Message;
					return response;
				}
				else if (entitiesResponse.Object == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "There is no entities into database!";
					return response;
				}

				List<Entity> entities = entitiesResponse.Object;

				RecordViewCollection recordViewList = new RecordViewCollection();
				recordViewList.RecordViews = new List<RecordView>();

				foreach (Entity entity in entities)
				{
					recordViewList.RecordViews.AddRange(entity.RecordViews);
				}

				response.Object = recordViewList;
			}
			catch (Exception e)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "An internal error occurred!";
#endif
				return response;
			}

			response.Timestamp = DateTime.Now;

			return response;
		}

		#endregion

		#region << RecordsTree methods >>

		public RecordTreeResponse CreateRecordTree(Guid entityId, InputRecordTree inputRecordTree)
		{
			RecordTreeResponse response = new RecordTreeResponse();

			var entityResponse = ReadEntity(entityId);

			if (!entityResponse.Success)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = entityResponse.Message;
				return response;
			}
			else if (entityResponse.Object == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such Id does not exist!";
				return response;
			}
			Entity entity = entityResponse.Object;

			return CreateRecordTree(entity, inputRecordTree);
		}

		public RecordTreeResponse CreateRecordTree(string entityName, InputRecordTree inputRecordTree)
		{
			RecordTreeResponse response = new RecordTreeResponse();

			var entityResponse = ReadEntity(entityName);

			if (!entityResponse.Success)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = entityResponse.Message;
				return response;
			}
			else if (entityResponse.Object == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such Id does not exist!";
				return response;
			}
			Entity entity = entityResponse.Object;

			return CreateRecordTree(entity, inputRecordTree);
		}

		private RecordTreeResponse CreateRecordTree(Entity entity, InputRecordTree inputRecordTree)
		{
			RecordTreeResponse response = new RecordTreeResponse
			{
				Success = true,
				Message = "The tree was successfully created!",
			};


			if (!inputRecordTree.Id.HasValue)
				inputRecordTree.Id = Guid.NewGuid();

			RecordTree recordTree = inputRecordTree.MapTo<RecordTree>();

			try
			{
				response.Object = recordTree;
				response.Errors = ValidateRecordTree(entity, inputRecordTree);

				recordTree = inputRecordTree.MapTo<RecordTree>();

				if (response.Errors.Count > 0)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The tree was not created. Validation error occurred!";
					return response;
				}

				entity.RecordTrees.Add(recordTree);

				DbEntity updatedEntity = entity.MapTo<DbEntity>();
				bool result = DbContext.Current.EntityRepository.Update(updatedEntity);
				if (!result)
				{
					Cache.ClearEntities();
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The tree was not created! An internal error occurred!";
					return response;
				}

			}
			catch (Exception e)
			{
				Cache.ClearEntities();
				response.Success = false;
				response.Object = recordTree;
				response.Timestamp = DateTime.UtcNow;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The tree was not created. An internal error occurred!";
#endif
				return response;
			}

			Cache.ClearEntities();

			return ReadRecordTree(entity.Id, recordTree.Id);
		}

		public RecordTreeResponse UpdateRecordTree(Guid entityId, InputRecordTree inputRecordTree)
		{
			RecordTreeResponse response = new RecordTreeResponse();

			var entityResponse = ReadEntity(entityId);

			if (!entityResponse.Success)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = entityResponse.Message;
				return response;
			}
			else if (entityResponse.Object == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such Id does not exist!";
				return response;
			}
			Entity entity = entityResponse.Object;

			return UpdateRecordTree(entity, inputRecordTree);
		}

		public RecordTreeResponse UpdateRecordTree(string entityName, InputRecordTree inputRecordTree)
		{
			RecordTreeResponse response = new RecordTreeResponse();

			var entityResponse = ReadEntity(entityName);

			if (!entityResponse.Success)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = entityResponse.Message;
				return response;
			}
			else if (entityResponse.Object == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such Id does not exist!";
				return response;
			}
			Entity entity = entityResponse.Object;

			return UpdateRecordTree(entity, inputRecordTree);
		}

		public RecordTreeResponse UpdateRecordTree(Entity entity, InputRecordTree inputRecordTree)
		{
			RecordTreeResponse response = new RecordTreeResponse
			{
				Success = true,
				Message = "The tree was successfully updated!",
			};

			RecordTree recordTree = inputRecordTree.MapTo<RecordTree>();

			try
			{
				response.Object = recordTree;
				response.Errors = ValidateRecordTree(entity, inputRecordTree);
				recordTree = inputRecordTree.MapTo<RecordTree>();

				if (response.Errors.Count > 0)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The tree was not updated. Validation error occurred!";
					return response;
				}

				RecordTree treeToUpdate = entity.RecordTrees.FirstOrDefault(r => r.Id == recordTree.Id);
				if (treeToUpdate.Id == recordTree.Id)
					entity.RecordTrees.Remove(treeToUpdate);

				entity.RecordTrees.Add(recordTree);

				DbEntity updatedEntity = entity.MapTo<DbEntity>();
				bool result = DbContext.Current.EntityRepository.Update(updatedEntity);
				if (!result)
				{
					Cache.ClearEntities();
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The tree was not updated! An internal error occurred!";
					return response;
				}

			}
			catch (Exception e)
			{
				Cache.ClearEntities();
				response.Success = false;
				response.Object = recordTree;
				response.Timestamp = DateTime.UtcNow;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The tree was not updated. An internal error occurred!";
#endif
				return response;
			}

			Cache.ClearEntities();

			return ReadRecordTree(entity.Id, recordTree.Id);
		}

		public RecordTreeResponse DeleteRecordTree(Guid entityId, Guid id)
		{
			RecordTreeResponse response = new RecordTreeResponse
			{
				Success = true,
				Message = "The tree was successfully deleted!",
			};

			try
			{
				var entityResponse = ReadEntity(entityId);

				if (!entityResponse.Success)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = entityResponse.Message;
					return response;
				}
				else if (entityResponse.Object == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "Entity with such Id does not exist!";
					return response;
				}
				Entity entity = entityResponse.Object;

				RecordTree recordTree = entity.RecordTrees.FirstOrDefault(v => v.Id == id);

				if (recordTree == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The tree was not deleted. Validation error occurred!";
					response.Errors.Add(new ErrorModel("id", id.ToString(), "Tree with such Id does not exist!"));
					return response;
				}

				entity.RecordTrees.Remove(recordTree);

				DbEntity updatedEntity = entity.MapTo<DbEntity>();
				bool result = DbContext.Current.EntityRepository.Update(updatedEntity);
				if (!result)
				{
					Cache.ClearEntities();
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The tree was not updated! An internal error occurred!";
					return response;
				}
			}
			catch (Exception e)
			{
				Cache.ClearEntities();
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The tree was not deleted. An internal error occurred!";
#endif
				return response;
			}

			Cache.ClearEntities();

			response.Timestamp = DateTime.UtcNow;
			return response;
		}

		public RecordTreeResponse DeleteRecordTree(string entityName, string name)
		{
			RecordTreeResponse response = new RecordTreeResponse
			{
				Success = true,
				Message = "The tree was successfully deleted!",
			};

			try
			{
				var entityResponse = ReadEntity(entityName);

				if (!entityResponse.Success)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = entityResponse.Message;
					return response;
				}
				else if (entityResponse.Object == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "Entity with such Id does not exist!";
					return response;
				}
				Entity entity = entityResponse.Object;

				RecordTree recordTree = entity.RecordTrees.FirstOrDefault(l => l.Name == name);

				if (recordTree == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The tree was not deleted. Validation error occurred!";
					response.Errors.Add(new ErrorModel("name", name, "Tree with such Name does not exist!"));
					return response;
				}

				entity.RecordTrees.Remove(recordTree);

				DbEntity updatedEntity = entity.MapTo<DbEntity>();
				bool result = DbContext.Current.EntityRepository.Update(updatedEntity);
				if (!result)
				{
					Cache.ClearEntities();
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "The tree was not updated! An internal error occurred!";
					return response;
				}
			}
			catch (Exception e)
			{
				Cache.ClearEntities();
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The tree was not deleted. An internal error occurred!";
#endif
				return response;
			}

			Cache.ClearEntities();

			response.Timestamp = DateTime.UtcNow;
			return response;
		}

		public RecordTreeCollectionResponse ReadRecordTrees(Guid entityId)
		{
			RecordTreeCollectionResponse response = new RecordTreeCollectionResponse();

			EntityResponse entityResponse = ReadEntity(entityId);

			if (!entityResponse.Success)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = entityResponse.Message;
				return response;
			}
			else if (entityResponse.Object == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such Id does not exist!";
				return response;
			}

			Entity entity = entityResponse.Object;

			return ReadRecordTrees(entity);
		}

		public RecordTreeCollectionResponse ReadRecordTrees(string entityName)
		{
			RecordTreeCollectionResponse response = new RecordTreeCollectionResponse();

			EntityResponse entityResponse = ReadEntity(entityName);

			if (!entityResponse.Success)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = entityResponse.Message;
				return response;
			}
			else if (entityResponse.Object == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such name does not exist!";
				return response;
			}

			Entity entity = entityResponse.Object;

			return ReadRecordTrees(entity);
		}

		public RecordTreeCollectionResponse ReadRecordTrees(Entity entity)
		{
			RecordTreeCollectionResponse response = new RecordTreeCollectionResponse
			{
				Success = true,
				Message = "The trees were successfully returned!",
			};

			try
			{
				RecordTreeCollection recordTreeCollection = new RecordTreeCollection();
				recordTreeCollection.RecordTrees = entity.RecordTrees;
				response.Object = recordTreeCollection;
			}
			catch (Exception e)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "An internal error occurred!";
#endif
				return response;
			}

			response.Timestamp = DateTime.Now;

			return response;
		}

		public RecordTreeResponse ReadRecordTree(Guid entityId, Guid id)
		{
			RecordTreeResponse response = new RecordTreeResponse();

			EntityResponse entityResponse = ReadEntity(entityId);

			if (!entityResponse.Success)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = entityResponse.Message;
				return response;
			}
			else if (entityResponse.Object == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such Id does not exist!";
				return response;
			}

			Entity entity = entityResponse.Object;

			RecordTree recordTree = entity.RecordTrees.FirstOrDefault(r => r.Id == id);

			if (recordTree == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Record tree with such Id does not exist!";
				return response;
			}


			return ReadRecordTree(entity, recordTree);
		}

		public RecordTreeResponse ReadRecordTree(string entityName, string name)
		{
			RecordTreeResponse response = new RecordTreeResponse();

			EntityResponse entityResponse = ReadEntity(entityName);

			if (!entityResponse.Success)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = entityResponse.Message;
				return response;
			}
			else if (entityResponse.Object == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Entity with such name does not exist!";
				return response;
			}

			Entity entity = entityResponse.Object;

			RecordTree recordTree = entity.RecordTrees.FirstOrDefault(r => r.Name == name);

			if (recordTree == null)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Message = "Record tree with such Name does not exist!";
				return response;
			}

			return ReadRecordTree(entity, recordTree);
		}

		private RecordTreeResponse ReadRecordTree(Entity entity, RecordTree recordTree)
		{
			RecordTreeResponse response = new RecordTreeResponse
			{
				Success = true,
				Message = "The list was successfully returned!",
			};

			try
			{
				response.Object = recordTree;
			}
			catch (Exception e)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "An internal error occurred!";
#endif
				return response;
			}

			response.Timestamp = DateTime.Now;

			return response;
		}

		public RecordTreeCollectionResponse ReadRecordTrees()
		{
			RecordTreeCollectionResponse response = new RecordTreeCollectionResponse
			{
				Success = true,
				Message = "The trees were successfully returned!",
			};

			try
			{
				var entityResponse = ReadEntities();

				if (!entityResponse.Success)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = entityResponse.Message;
					return response;
				}
				else if (entityResponse.Object == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "There is no entities into database!";
					return response;
				}
				List<Entity> entities = entityResponse.Object;

				RecordTreeCollection recordTreeCollection = new RecordTreeCollection();
				recordTreeCollection.RecordTrees = new List<RecordTree>();

				foreach (Entity entity in entities)
				{
					recordTreeCollection.RecordTrees.AddRange(entity.RecordTrees);
				}

				response.Object = recordTreeCollection;
			}
			catch (Exception e)
			{
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
#if DEBUG
				response.Message = e.Message + e.StackTrace;
#else
                response.Message = "An internal error occurred!";
#endif
				return response;
			}

			response.Timestamp = DateTime.Now;

			return response;
		}

		private List<ErrorModel> ValidateRecordTree(Entity entity, InputRecordTree recordTree)
		{
			List<ErrorModel> errorList = new List<ErrorModel>();

			if (!recordTree.Id.HasValue || recordTree.Id == Guid.Empty)
				errorList.Add(new ErrorModel("id", null, "Id is required!"));

			if (string.IsNullOrWhiteSpace(recordTree.Name))
				errorList.Add(new ErrorModel("name", null, "Name is required!"));

			if (string.IsNullOrWhiteSpace(recordTree.Name))
				errorList.Add(new ErrorModel("label", null, "Label is required!"));

			if (errorList.Any())
				return errorList;

			if (entity.RecordTrees.Any(f => f.Id != recordTree.Id && f.Name.ToLowerInvariant() == recordTree.Name.ToLowerInvariant()))
				errorList.Add(new ErrorModel("name", null, "There is already a tree with same name!"));

			if (entity.RecordTrees.Any(f => f.Id != recordTree.Id && f.Label.ToLowerInvariant() == recordTree.Label.ToLowerInvariant()))
				errorList.Add(new ErrorModel("label", null, "There is already a tree with same label!"));

			EntityRelation relation = null;
			if (recordTree.RelationId == null || recordTree.RelationId == Guid.Empty)
			{
				errorList.Add(new ErrorModel("relationId", null, "Relation is required!"));
				return errorList;
			}
			else
			{
				relation =  new EntityRelationManager().Read(recordTree.RelationId).Object;
			}

			if (relation == null)
			{
				errorList.Add(new ErrorModel("relationId", null, "Cannot find relation for specified id!"));
				return errorList;
			}

			if (!(relation.OriginEntityId == entity.Id && relation.TargetEntityId == relation.OriginEntityId) ||
				relation.RelationType == EntityRelationType.OneToOne)
			{
				errorList.Add(new ErrorModel("relationId", null, "Specified relation cannot be used as tree relation!"));
				return errorList;
			}

			if (recordTree.NodeIdFieldId == null)
				recordTree.NodeIdFieldId = relation.OriginFieldId;
			else if (recordTree.NodeIdFieldId != relation.OriginFieldId)
				errorList.Add(new ErrorModel("nodeIdFieldId", null, "Node field does not correspond to specified relation target field!"));

			if (recordTree.NodeParentIdFieldId == null)
				recordTree.NodeParentIdFieldId = relation.TargetFieldId;
			else if (recordTree.NodeParentIdFieldId != relation.TargetFieldId)
				errorList.Add(new ErrorModel("nodeParentIdFieldId", null, "Node parent field does not correspond to specified relation origin field!"));

			var idField = entity.Fields.Single(f => f.Name == "id");

			if (recordTree.NodeNameFieldId == null)
				recordTree.NodeNameFieldId = idField.Id;

			if (recordTree.NodeLabelFieldId == null)
				recordTree.NodeLabelFieldId = idField.Id;

			if (!entity.Fields.Any(f => f.Id == recordTree.NodeIdFieldId))
				errorList.Add(new ErrorModel("nodeIdFieldId", null, "Node field is not found in entity fields collection!"));

			if (!entity.Fields.Any(f => f.Id == recordTree.NodeParentIdFieldId))
				errorList.Add(new ErrorModel("nodeParentIdFieldId", null, "Node parent field is not found in entity fields collection!"));

			if (!entity.Fields.Any(f => f.Id == recordTree.NodeNameFieldId))
				errorList.Add(new ErrorModel("nodeNameFieldId", null, "Node name field is not found in entity fields collection!"));

			if (!entity.Fields.Any(f => f.Id == recordTree.NodeLabelFieldId))
				errorList.Add(new ErrorModel("nodeLabelFieldId", null, "Node label field is not found in entity fields collection!"));

			if (recordTree.DepthLimit == null)
				errorList.Add(new ErrorModel("depthLimit", null, "Depth limit is required!"));
			else if (recordTree.DepthLimit <= 0)
				errorList.Add(new ErrorModel("depthLimit", null, "Depth limit should be a positive number!"));
			else if (recordTree.DepthLimit > 50)
				errorList.Add(new ErrorModel("depthLimit", null, "Depth limit cannot be more than 20!"));

			if (errorList.Any())
				return errorList;

			if (recordTree.RootNodes != null)
			{
				List<RecordTreeNode> expiredNodes = new List<RecordTreeNode>();
				foreach (var node in recordTree.RootNodes)
				{
					var recData = DbContext.Current.RecordRepository.Find(entity.Name, node.RecordId);
					if (recData != null)
					{
						var parentIdField = entity.Fields.SingleOrDefault(x => x.Id == recordTree.NodeParentIdFieldId);
						var nameField = entity.Fields.SingleOrDefault(x => x.Id == recordTree.NodeNameFieldId);
						var labelField = entity.Fields.SingleOrDefault(x => x.Id == recordTree.NodeLabelFieldId);

						var value = recData[idField.Name];
						node.Id = (value as Guid?) ?? Guid.Empty;

						value = recData[parentIdField.Name];
						node.ParentId = (value as Guid?) ?? Guid.Empty;

						value = recData[nameField.Name];
						node.Name = (value ?? string.Empty).ToString();

						value = recData[labelField.Name];
						node.Label = (value ?? string.Empty).ToString();
					}
					else
						expiredNodes.Add(node);
				}

				foreach (var node in expiredNodes)
					errorList.Add(new ErrorModel("rootNodes", null, $"Root node Id '{node.Id}'  not found"));
			}
			else
				recordTree.RootNodes = new List<RecordTreeNode>();

			//verify and init node properties bellow
			if (recordTree.NodeObjectProperties == null)
				recordTree.NodeObjectProperties = new List<Guid>();

			List<Guid> expiredFields = new List<Guid>();
			foreach (var fieldId in recordTree.NodeObjectProperties)
			{
				if (!entity.Fields.Any(f => f.Id == fieldId))
					expiredFields.Add(fieldId);
			}

			foreach (var expiredId in expiredFields)
				recordTree.NodeObjectProperties.Remove(expiredId);

			if (!recordTree.NodeObjectProperties.Contains(idField.Id))
				recordTree.NodeObjectProperties.Add(idField.Id);

			if (!recordTree.NodeObjectProperties.Contains(recordTree.NodeIdFieldId.Value))
				recordTree.NodeObjectProperties.Add(recordTree.NodeIdFieldId.Value);

			if (!recordTree.NodeObjectProperties.Contains(recordTree.NodeIdFieldId.Value))
				recordTree.NodeObjectProperties.Add(recordTree.NodeIdFieldId.Value);

			if (!recordTree.NodeObjectProperties.Contains(recordTree.NodeParentIdFieldId.Value))
				recordTree.NodeObjectProperties.Add(recordTree.NodeParentIdFieldId.Value);

			if (!recordTree.NodeObjectProperties.Contains(recordTree.NodeNameFieldId.Value))
				recordTree.NodeObjectProperties.Add(recordTree.NodeNameFieldId.Value);

			if (!recordTree.NodeObjectProperties.Contains(recordTree.NodeLabelFieldId.Value))
				recordTree.NodeObjectProperties.Add(recordTree.NodeLabelFieldId.Value);

			return errorList;
		}

		#endregion

		#region << Help methods >>

		private List<Field> CreateEntityDefaultFields(Entity entity, Dictionary<string,Guid> sysFieldIdDictionary = null)
		{
			List<Field> fields = new List<Field>();

			GuidField primaryKeyField = new GuidField();

			if(sysFieldIdDictionary != null && sysFieldIdDictionary.ContainsKey("id")) {
				primaryKeyField.Id = sysFieldIdDictionary["id"];
			}
			else {
				primaryKeyField.Id = Guid.NewGuid();
			}
			primaryKeyField.Name = "id";
			primaryKeyField.Label = "Id";
			primaryKeyField.PlaceholderText = "";
			primaryKeyField.Description = "";
			primaryKeyField.HelpText = "";
			primaryKeyField.Required = true;
			primaryKeyField.Unique = true;
			primaryKeyField.Searchable = false;
			primaryKeyField.Auditable = false;
			primaryKeyField.System = true;
			primaryKeyField.DefaultValue = null;
			primaryKeyField.GenerateNewId = true;

			fields.Add(primaryKeyField);

			GuidField createdBy = new GuidField();

			if(sysFieldIdDictionary != null && sysFieldIdDictionary.ContainsKey("created_by")) {
				createdBy.Id = sysFieldIdDictionary["created_by"];
			}
			else {
				createdBy.Id = Guid.NewGuid();
			}
			createdBy.Name = "created_by";
			createdBy.Label = "Created By";
			createdBy.PlaceholderText = "";
			createdBy.Description = "";
			createdBy.HelpText = "";
			createdBy.Required = false;
			createdBy.Unique = false;
			createdBy.Searchable = false;
			createdBy.Auditable = false;
			createdBy.System = true;
			createdBy.DefaultValue = null;
			createdBy.GenerateNewId = false;

			fields.Add(createdBy);

			GuidField lastModifiedBy = new GuidField();

			if(sysFieldIdDictionary != null && sysFieldIdDictionary.ContainsKey("last_modified_by")) {
				lastModifiedBy.Id = sysFieldIdDictionary["last_modified_by"];
			}
			else {
				lastModifiedBy.Id = Guid.NewGuid();
			}
			lastModifiedBy.Name = "last_modified_by";
			lastModifiedBy.Label = "Last Modified By";
			lastModifiedBy.PlaceholderText = "";
			lastModifiedBy.Description = "";
			lastModifiedBy.HelpText = "";
			lastModifiedBy.Required = false;
			lastModifiedBy.Unique = false;
			lastModifiedBy.Searchable = false;
			lastModifiedBy.Auditable = false;
			lastModifiedBy.System = true;
			lastModifiedBy.DefaultValue = null;
			lastModifiedBy.GenerateNewId = false;

			fields.Add(lastModifiedBy);

			DateTimeField createdOn = new DateTimeField();

			if(sysFieldIdDictionary != null && sysFieldIdDictionary.ContainsKey("created_on")) {
				createdOn.Id = sysFieldIdDictionary["created_on"];
			}
			else {
				createdOn.Id = Guid.NewGuid();
			}
			createdOn.Name = "created_on";
			createdOn.Label = "Created On";
			createdOn.PlaceholderText = "";
			createdOn.Description = "";
			createdOn.HelpText = "";
			createdOn.Required = false;
			createdOn.Unique = false;
			createdOn.Searchable = false;
			createdOn.Auditable = false;
			createdOn.System = true;
			createdOn.DefaultValue = null;

			createdOn.Format = "dd MMM yyyy HH:mm";
			createdOn.UseCurrentTimeAsDefaultValue = true;

			fields.Add(createdOn);

			DateTimeField modifiedOn = new DateTimeField();

			if(sysFieldIdDictionary != null && sysFieldIdDictionary.ContainsKey("last_modified_on")) {
				modifiedOn.Id = sysFieldIdDictionary["last_modified_on"];
			}
			else {
				modifiedOn.Id = Guid.NewGuid();
			}
			modifiedOn.Name = "last_modified_on";
			modifiedOn.Label = "Last Modified On";
			modifiedOn.PlaceholderText = "";
			modifiedOn.Description = "";
			modifiedOn.HelpText = "";
			modifiedOn.Required = false;
			modifiedOn.Unique = false;
			modifiedOn.Searchable = false;
			modifiedOn.Auditable = false;
			modifiedOn.System = true;
			modifiedOn.DefaultValue = null;

			modifiedOn.Format = "dd MMM yyyy HH:mm";
			modifiedOn.UseCurrentTimeAsDefaultValue = true;

			fields.Add(modifiedOn);

			return fields;
		}

		private ActionItem GenerateListActionItem(string name)
		{
			var actionItem = new ActionItem();
			switch (name)
			{
				case "wv_create_record":
					actionItem.Name = "wv_create_record";
					actionItem.Menu = "page-title";
					actionItem.Weight = 1;
					actionItem.Template = "<a class=\"btn btn-default btn-outline hidden-xs\" ng-show=\"::ngCtrl.userHasRecordPermissions('canCreate')\" ng-href=\"{{::ngCtrl.getRecordCreateUrl()}}\">Add New</a>";
					break;
				case "wv_import_records":
					actionItem.Name = "wv_import_records";
					actionItem.Menu = "page-title-dropdown";
					actionItem.Weight = 10;
					actionItem.Template = "<a ng-click=\"ngCtrl.openImportModal()\" class=\"ng-hide\" ng-show=\"::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')\"><i class=\"fa fa-fw fa-upload\"></i> Import CSV</a>";
					break;
				case "wv_export_records":
					actionItem.Name = "wv_export_records";
					actionItem.Menu = "page-title-dropdown";
					actionItem.Weight = 11;
					actionItem.Template = "<a ng-click=\"ngCtrl.openExportModal()\" class=\"ng-hide\" ng-show=\"::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')\"><i class=\"fa fa-fw fa-download\"></i> Export CSV</a>";
					break;
				case "wv_manage_list":
					actionItem.Name = "wv_manage_list";
					actionItem.Menu = "page-title-dropdown";
					actionItem.Weight = 101;
					actionItem.Template = "<a target=\"_blank\" ng-href=\"{{ngCtrl.getListManageUrl()}}\" ng-if=\"::ngCtrl.userIsAdmin()\">\n\t<i class=\"fa fa-fw fa-cog\"></i> Manage List\n</a>";
					break;
				case "wv_record_details":
					actionItem.Name = "wv_record_details";
					actionItem.Menu = "record-row";
					actionItem.Weight = 1;
					actionItem.Template = "<a class=\"btn btn-default btn-outline\" ng-href=\"{{::ngCtrl.getRecordDetailsUrl(record)}}\"><i class=\"fa fa-fw fa-eye\"></i></a>";
					break;
				case "wv_recursive_list_add_existing":
					actionItem.Name = "wv_recursive_list_add_existing";
					actionItem.Menu = "recursive-list-title";
					actionItem.Weight = 1;
					actionItem.Template = "<a href=\"javascript:void(0)\" class=\"btn btn-outline btn-sm\" ng-if=\"::canAddExisting\" ng-click=\"addExistingItem()\"><i class=\"fa fa-download\"></i> Add existing</a>";
					break;
				case "wv_recursive_list_add_new":
					actionItem.Name = "wv_recursive_list_add_new";
					actionItem.Menu = "recursive-list-title";
					actionItem.Weight = 2;
					actionItem.Template = "<a href=\"javascript:void(0)\" class=\"btn btn-outline btn-sm\" ng-if=\"::canCreate\" ng-click=\"manageRelatedRecordItem(null)\"><i class=\"fa fa-plus\"></i> Create & Add</a>";
					break;
				case "wv_recursive_list_view":
					actionItem.Name = "wv_recursive_list_view";
					actionItem.Menu = "recursive-list-record-row";
					actionItem.Weight = 1;
					actionItem.Template = "<a href=\"javascript:void(0)\" title=\"quick view this record\" class=\"btn btn-sm btn-outline\" ng-click=\"viewRelatedRecordItem(record)\"><i class=\"fa fa-eye\"></i></a>";
					break;
				case "wv_recursive_list_edit":
					actionItem.Name = "wv_recursive_list_edit";
					actionItem.Menu = "recursive-list-record-row";
					actionItem.Weight = 2;
					actionItem.Template = "<a href=\"javascript:void(0)\" title=\"quick edit this record\" class=\"btn btn-sm btn-outline\" ng-click=\"manageRelatedRecordItem(record)\" ng-if=\"::canUpdate\"><i class=\"fa fa-pencil\"></i></a>";
					break;
				case "wv_recursive_list_unrelate":
					actionItem.Name = "wv_recursive_list_unrelate";
					actionItem.Menu = "recursive-list-record-row";
					actionItem.Weight = 3;
					actionItem.Template = "<a href=\"javascript:void(0)\" title=\"Detach records relation\" class=\"btn btn-sm btn-outline\" confirmed-click=\"instantDetachRecord(record)\" ng-confirm-click=\"Are you sure that you need this relation removed?\" ng-if=\"::canRemove\"><i class=\"fa fa-times go-red\"></i></a>";
					break;
				default:
					throw new Exception("no such action type");
			}
			return actionItem;
		}

		private List<RecordList> CreateEntityDefaultRecordLists(Entity entity)
		{
			List<RecordList> recordLists = new List<RecordList>();

			var create = new RecordList();
			create.Id = Guid.NewGuid();
			create.Name = "general";
			create.Label = "General";
			create.Title = "General";
			create.Default = true;
			create.System = false;
			create.Type = "general";
			create.IconName = "list";
			create.PageSize = 10;
			create.Weight = 10;
			create.VisibleColumnsCount = 5;
			create.ServiceCode = null;
			create.DynamicHtmlTemplate = null;
			create.DataSourceUrl = null;
			create.ActionItems = new List<ActionItem>();
			create.ActionItems.Add(GenerateListActionItem("wv_create_record"));
			create.ActionItems.Add(GenerateListActionItem("wv_import_records"));
			create.ActionItems.Add(GenerateListActionItem("wv_export_records"));
			create.ActionItems.Add(GenerateListActionItem("wv_record_details"));
			create.ActionItems.Add(GenerateListActionItem("wv_recursive_list_add_existing"));
			create.ActionItems.Add(GenerateListActionItem("wv_recursive_list_add_new"));
			create.ActionItems.Add(GenerateListActionItem("wv_recursive_list_view"));
			create.ActionItems.Add(GenerateListActionItem("wv_recursive_list_edit"));
			create.ActionItems.Add(GenerateListActionItem("wv_recursive_list_unrelate"));
			recordLists.Add(create);

			var lookup = new RecordList();
			lookup.Id = Guid.NewGuid();
			lookup.Name = "lookup";
			lookup.Label = "Lookup";
			lookup.Title = "Lookup";
			lookup.Default = true;
			lookup.System = false;
			lookup.Type = "lookup";
			lookup.IconName = "list";
			lookup.PageSize = 10;
			lookup.Weight = 10;
			lookup.VisibleColumnsCount = 5;
			lookup.ServiceCode = null;
			lookup.DynamicHtmlTemplate = null;
			lookup.DataSourceUrl = null;
			lookup.ActionItems = new List<ActionItem>();
			lookup.ActionItems.Add(GenerateListActionItem("wv_create_record"));
			lookup.ActionItems.Add(GenerateListActionItem("wv_import_records"));
			lookup.ActionItems.Add(GenerateListActionItem("wv_export_records"));
			lookup.ActionItems.Add(GenerateListActionItem("wv_record_details"));
			lookup.ActionItems.Add(GenerateListActionItem("wv_recursive_list_add_existing"));
			lookup.ActionItems.Add(GenerateListActionItem("wv_recursive_list_add_new"));
			lookup.ActionItems.Add(GenerateListActionItem("wv_recursive_list_view"));
			lookup.ActionItems.Add(GenerateListActionItem("wv_recursive_list_edit"));
			lookup.ActionItems.Add(GenerateListActionItem("wv_recursive_list_unrelate"));
			recordLists.Add(lookup);

			return recordLists;
		}

		private ActionItem GenerateViewActionItem(string name)
		{
			var actionItem = new ActionItem();
			switch (name)
			{
				case "wv_record_delete":
					actionItem.Name = "wv_record_delete";
					actionItem.Menu = "page-title-dropdown";
					actionItem.Weight = 1;
					actionItem.Template = "<a href=\"javascript:void(0)\" confirmed-click=\"::ngCtrl.deleteRecord(ngCtrl)\" ng-confirm-click=\"Are you sure?\" ng-if=\"::ngCtrl.userHasRecordPermissions('canDelete')\"><i class=\"fa fa-trash go-red\"></i> Delete Record</a>";
					break;
				case "wv_manage_view":
					actionItem.Name = "wv_manage_view";
					actionItem.Menu = "page-title-dropdown";
					actionItem.Weight = 101;
					actionItem.Template = "<a target=\"_blank\" ng-href=\"{{ngCtrl.getViewManageUrl()}}\" ng-if=\"::ngCtrl.userIsAdmin()\">\n\t<i class=\"fa fa-fw fa-cog\"></i> Manage View\n</a>";
					break;
				case "wv_create_and_list":
					actionItem.Name = "wv_create_and_list";
					actionItem.Menu = "create-bottom";
					actionItem.Weight = 1;
					actionItem.Template = "<a class=\"btn btn-primary\" ng-click='ngCtrl.create(\"default\")' ng-if=\"::ngCtrl.createViewRegion != null\">Create</a>";
					break;
				case "wv_create_and_details":
					actionItem.Name = "wv_create_and_details";
					actionItem.Menu = "create-bottom";
					actionItem.Weight = 2;
					actionItem.Template = "<a class=\"btn btn-default btn-outline\" ng-click='ngCtrl.create(\"details\")' ng-if=\"::ngCtrl.createViewRegion != null\">Create & Details</a>";
					break;
				case "wv_create_cancel":
					actionItem.Name = "wv_create_cancel";
					actionItem.Menu = "create-bottom";
					actionItem.Weight = 3;
					actionItem.Template = "<a class=\"btn btn-default btn-outline\" ng-click=\"ngCtrl.cancel()\">Cancel</a>";
					break;
				case "wv_back_button":
					actionItem.Name = "wv_back_button";
					actionItem.Menu = "sidebar-top";
					actionItem.Weight = 1;
					actionItem.Template = "<a class=\"back clearfix\" href=\"javascript:void(0)\" ng-click=\"sidebarData.goBack()\"><i class=\"fa fa-fw fa-arrow-left\"></i> <span class=\"text\">Back</span></a>";
					break;
				case "wv_recursive_view_add_existing":
					actionItem.Name = "wv_recursive_view_add_existing";
					actionItem.Menu = "recursive-view-title";
					actionItem.Weight = 1;
					actionItem.Template = "<a href=\"javascript:void(0)\" class=\"btn btn-sm btn-outline\" ng-if=\"::canAddExisting\" ng-click=\"addExistingItem()\"><i class=\"fa fa-download\"></i> Add existing</a>";
					break;
				case "wv_recursive_view_add_new":
					actionItem.Name = "wv_recursive_view_add_new";
					actionItem.Menu = "recursive-view-title";
					actionItem.Weight = 2;
					actionItem.Template = "<a href=\"javascript:void(0)\" class=\"btn btn-sm btn-outline\" ng-if=\"::canCreate\" ng-click=\"manageRelatedRecordItem(null)\"><i class=\"fa fa-plus\"></i> Create & Add</a>";
					break;
				case "wv_recursive_view_edit":
					actionItem.Name = "wv_recursive_view_edit";
					actionItem.Menu = "recursive-list-record-row";
					actionItem.Weight = 1;
					actionItem.Template = "<a href=\"javascript:void(0)\" title=\"quick edit\" class=\"btn btn-sm btn-outline\" ng-click=\"manageRelatedRecordItem(recordData)\" ng-if=\"::canUpdate\"><i class=\"fa fa-pencil\"></i></a>";
					break;
				case "wv_recursive_view_unrelate":
					actionItem.Name = "wv_recursive_view_unrelate";
					actionItem.Menu = "recursive-list-record-row";
					actionItem.Weight = 2;
					actionItem.Template = "<a href=\"javascript:void(0)\" title=\"remove relation\" class=\"btn btn-sm btn-outline\" confirmed-click=\"instantDetachRecord(recordData)\" ng-confirm-click=\"Are you sure that you need this relation removed?\" ng-if=\"::canRemove\"><i class=\"fa fa-times go-red\"></i></a>";
					break;
				default:
					throw new Exception("no such action type");
			}
			return actionItem;
		}

		private List<RecordView> CreateEntityDefaultRecordViews(Entity entity)
		{
			List<RecordView> recordViewList = new List<RecordView>();

			var headerRegion = new RecordViewRegion();
			headerRegion.Name = "header";
			headerRegion.Label = "Header";
			headerRegion.Sections = new List<RecordViewSection>();

			var create = new RecordView();
			create.Id = Guid.NewGuid();
			create.Name = "create";
			create.Label = "Create";
			create.Title = "Create";
			create.Default = true;
			create.System = false;
			create.Type = "create";
			create.Weight = 10;
			create.IconName = "file-text-o";
			create.Regions = new List<RecordViewRegion>();
			create.Regions.Add(headerRegion);
			create.ServiceCode = null;
			create.DynamicHtmlTemplate = null;
			create.DataSourceUrl = null;
			create.ActionItems = new List<ActionItem>();
			create.ActionItems.Add(GenerateViewActionItem("wv_back_button"));
			create.ActionItems.Add(GenerateViewActionItem("wv_create_and_list"));
			create.ActionItems.Add(GenerateViewActionItem("wv_create_and_details"));
			create.ActionItems.Add(GenerateViewActionItem("wv_create_cancel"));
			create.ActionItems.Add(GenerateViewActionItem("wv_recursive_view_add_existing"));
			create.ActionItems.Add(GenerateViewActionItem("wv_recursive_view_add_new"));
			create.ActionItems.Add(GenerateViewActionItem("wv_recursive_view_edit"));
			create.ActionItems.Add(GenerateViewActionItem("wv_recursive_view_unrelate"));
			recordViewList.Add(create);

			var quickCreate = new RecordView();
			quickCreate.Id = Guid.NewGuid();
			quickCreate.Name = "quick_create";
			quickCreate.Label = "Quick create";
			quickCreate.Title = "Quick create";
			quickCreate.Default = true;
			quickCreate.System = false;
			quickCreate.Type = "quick_create";
			quickCreate.IconName = "file-text-o";
			quickCreate.Weight = 10;
			quickCreate.Regions = new List<RecordViewRegion>();
			quickCreate.Regions.Add(headerRegion);
			quickCreate.ServiceCode = null;
			quickCreate.DynamicHtmlTemplate = null;
			quickCreate.DataSourceUrl = null;
			quickCreate.ActionItems = new List<ActionItem>();
			quickCreate.ActionItems.Add(GenerateViewActionItem("wv_back_button"));
			quickCreate.ActionItems.Add(GenerateViewActionItem("wv_create_and_list"));
			quickCreate.ActionItems.Add(GenerateViewActionItem("wv_create_and_details"));
			quickCreate.ActionItems.Add(GenerateViewActionItem("wv_create_cancel"));
			quickCreate.ActionItems.Add(GenerateViewActionItem("wv_recursive_view_add_existing"));
			quickCreate.ActionItems.Add(GenerateViewActionItem("wv_recursive_view_add_new"));
			quickCreate.ActionItems.Add(GenerateViewActionItem("wv_recursive_view_edit"));
			quickCreate.ActionItems.Add(GenerateViewActionItem("wv_recursive_view_unrelate"));
			recordViewList.Add(quickCreate);

			var quickView = new RecordView();
			quickView.Id = Guid.NewGuid();
			quickView.Name = "quick_view";
			quickView.Label = "Quick view";
			quickView.Title = "Quick view";
			quickView.Default = true;
			quickView.System = false;
			quickView.Type = "quick_view";
			quickView.IconName = "file-text-o";
			quickView.Weight = 10;
			quickView.Regions = new List<RecordViewRegion>();
			quickView.Regions.Add(headerRegion);
			quickView.ServiceCode = null;
			quickView.DynamicHtmlTemplate = null;
			quickView.DataSourceUrl = null;
			quickView.ActionItems = new List<ActionItem>();
			quickView.ActionItems.Add(GenerateViewActionItem("wv_record_delete"));
			quickView.ActionItems.Add(GenerateViewActionItem("wv_manage_view"));
			quickView.ActionItems.Add(GenerateViewActionItem("wv_back_button"));
			quickView.ActionItems.Add(GenerateViewActionItem("wv_recursive_view_add_existing"));
			quickView.ActionItems.Add(GenerateViewActionItem("wv_recursive_view_add_new"));
			quickView.ActionItems.Add(GenerateViewActionItem("wv_recursive_view_edit"));
			quickView.ActionItems.Add(GenerateViewActionItem("wv_recursive_view_unrelate"));
			recordViewList.Add(quickView);

			var general = new RecordView();
			general.Id = Guid.NewGuid();
			general.Name = "general";
			general.Label = "General";
			general.Title = "General";
			general.Default = true;
			general.System = false;
			general.Type = "general";
			general.Weight = 10;
			general.IconName = "file-text-o";
			general.Regions = new List<RecordViewRegion>();
			general.Regions.Add(headerRegion);
			general.ServiceCode = null;
			general.DynamicHtmlTemplate = null;
			general.DataSourceUrl = null;
			general.ActionItems = new List<ActionItem>();
			general.ActionItems.Add(GenerateViewActionItem("wv_record_delete"));
			general.ActionItems.Add(GenerateViewActionItem("wv_manage_view"));
			general.ActionItems.Add(GenerateViewActionItem("wv_back_button"));
			general.ActionItems.Add(GenerateViewActionItem("wv_recursive_view_add_existing"));
			general.ActionItems.Add(GenerateViewActionItem("wv_recursive_view_add_new"));
			general.ActionItems.Add(GenerateViewActionItem("wv_recursive_view_edit"));
			general.ActionItems.Add(GenerateViewActionItem("wv_recursive_view_unrelate"));
			recordViewList.Add(general);


			return recordViewList;
		}

		public static EntityRecord ConvertToEntityRecord(object inputRecord)
		{
			EntityRecord record = new EntityRecord();

			foreach (var prop in inputRecord.GetType().GetProperties())
			{
				record[prop.Name] = prop.GetValue(inputRecord);
			}

			return record;
		}

		private static bool HasKey(Expando expando, string key)
		{
			return expando.GetProperties().Any(p => p.Key == key);
		}

		private Entity GetEntityByListId(Guid listId)
		{
			var entityResponse = ReadEntities();
			if (!entityResponse.Success || entityResponse.Object == null)
				return null;

			List<Entity> entities = entityResponse.Object;

			return GetEntityByListId(listId, entities);
		}

		private static Entity GetEntityByListId(Guid listId, List<Entity> entities)
		{
			foreach (var entity in entities)
			{
				if (entity.RecordLists.Any(l => l.Id == listId))
					return entity;
			}

			return null;
		}

		private static Entity GetEntityByTreeId(Guid treeId, List<Entity> entities)
		{
			foreach (var entity in entities)
			{
				if (entity.RecordTrees == null)
					continue;

				if (entity.RecordTrees.Any(l => l.Id == treeId))
					return entity;
			}

			return null;
		}

		private Entity GetEntityByViewId(Guid viewId)
		{
			var entityResponse = ReadEntities();
			if (!entityResponse.Success || entityResponse.Object == null)
				return null;

			List<Entity> entities = entityResponse.Object;

			return GetEntityByViewId(viewId, entities);
		}

		private static Entity GetEntityByViewId(Guid viewId, List<Entity> entities)
		{
			foreach (var entity in entities)
			{
				if (entity.RecordViews.Any(v => v.Id == viewId))
					return entity;
			}

			return null;
		}

		private Entity GetEntityByFieldId(Guid fieldId)
		{
			var entityResponse = ReadEntities();
			if (!entityResponse.Success || entityResponse.Object == null)
				return null;

			List<Entity> entities = entityResponse.Object;

			return GetEntityByFieldId(fieldId, entities);
		}

		private static Entity GetEntityByFieldId(Guid fieldId, List<Entity> entities)
		{
			foreach (var entity in entities)
			{
				if (entity.Fields.Any(v => v.Id == fieldId))
					return entity;
			}

			return null;
		}

		#endregion
	}
}