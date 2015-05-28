using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Storage;
using WebVella.ERP.Utilities.Dynamic;

namespace WebVella.ERP.Api
{
    public class EntityManager
    {
        public IStorageService Storage { get; set; }

        public IStorageEntityRepository EntityRepository { get; set; }

        public IStorageObjectFactory StorageObjectFactory { get; set; }

        public EntityManager(IStorageService storage)
        {
            EntityRepository = storage.GetEntityRepository();
            StorageObjectFactory = storage.GetObjectFactory();
        }

        #region << Validation methods >>

        private List<ErrorModel> ValidateEntity(Entity entity, bool checkId = false)
        {
            List<ErrorModel> errorList = new List<ErrorModel>();

            IList<IStorageEntity> entities = EntityRepository.Read();

            if (entity.Id == Guid.Empty)
                errorList.Add(new ErrorModel("id", null, "Id is required!"));

            if (checkId)
            {
                //update
                if (entity.Id != Guid.Empty)
                {
                    IStorageEntity verifiedEntity = EntityRepository.Read(entity.Id);

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
                IStorageEntity verifiedEntity = EntityRepository.Read(entity.Name);

                if (verifiedEntity != null && verifiedEntity.Id != entity.Id)
                    errorList.Add(new ErrorModel("name", entity.Name, "Entity with such Name exists already!"));
            }

            errorList.AddRange(ValidationUtility.ValidateLabel(entity.Label));

            errorList.AddRange(ValidationUtility.ValidateLabelPlural(entity.LabelPlural));

            if (!entity.System)
                errorList.Add(new ErrorModel("system", null, "System is required!"));

            if (entity.RecordPermissions != null)
            {
                if (entity.RecordPermissions.CanRead == null || entity.RecordPermissions.CanRead.Count == 0)
                    errorList.Add(new ErrorModel("permissions.canRead", null, "CanRead is required! It must contains at least one item!"));

                if (entity.RecordPermissions.CanRead == null || entity.RecordPermissions.CanRead.Count == 0)
                    errorList.Add(new ErrorModel("permissions.canCreate", null, "CanCreate is required! It must contains at least one item!"));

                if (entity.RecordPermissions.CanUpdate == null || entity.RecordPermissions.CanUpdate.Count == 0)
                    errorList.Add(new ErrorModel("permissions.canUpdate", null, "CanUpdate is required! It must contains at least one item!"));

                if (entity.RecordPermissions.CanDelete == null || entity.RecordPermissions.CanDelete.Count == 0)
                    errorList.Add(new ErrorModel("permissions.canDelete", null, "CanDelete is required! It must contains at least one item!"));
            }
            else
                errorList.Add(new ErrorModel("permissions", null, "Permissions is required!"));

            if (string.IsNullOrWhiteSpace(entity.IconName))
                entity.IconName = "database";

            return errorList;
        }

        private List<ErrorModel> ValidateFields(Guid entityId, List<InputField> fields, bool checkId = false)
        {
            List<ErrorModel> errorList = new List<ErrorModel>();

            IStorageEntity storageEntity = EntityRepository.Read(entityId);
            Entity entity = ConvertEntityFromStorage(storageEntity);

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

        private List<ErrorModel> ValidateField(Entity entity, InputField field, bool checkId = false)
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

                if (!((InputDateField)field).UseCurrentTimeAsDefaultValue.HasValue)
                    ((InputDateField)field).UseCurrentTimeAsDefaultValue = false;
                //errorList.Add(new ErrorModel("useCurrentTimeAsDefaultValue", null, "Use current Time is required!"));
            }
            else if (field is InputDateTimeField)
            {
                //TODO:parse format and check if it is valid

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

        private List<ErrorModel> ValidateViews(Guid entityId, List<RecordsList> recordsLists, bool checkId = false)
        {
            List<ErrorModel> errorList = new List<ErrorModel>();

            IStorageEntity storageEntity = EntityRepository.Read(entityId);
            Entity entity = ConvertEntityFromStorage(storageEntity);

            foreach (var recordList in recordsLists)
            {
                errorList.AddRange(ValidateView(entity, recordList, checkId));
            }

            return errorList;
        }

        private List<ErrorModel> ValidateView(Entity entity, RecordsList recordslist, bool checkId = false)
        {
            List<ErrorModel> errorList = new List<ErrorModel>();

            if (!recordslist.Id.HasValue || recordslist.Id.Value == Guid.Empty)
                errorList.Add(new ErrorModel("id", null, "Id is required!"));

            if (checkId)
            {
                int listSameIdCount = entity.RecordsLists.Where(f => f.Id == recordslist.Id).Count();

                if (listSameIdCount > 1)
                    errorList.Add(new ErrorModel("id", null, "There is already a list with such Id!"));

                int listSameNameCount = entity.Fields.Where(f => f.Name == recordslist.Name).Count();

                if (listSameNameCount > 1)
                    errorList.Add(new ErrorModel("name", null, "There is already a list with such Name!"));
            }

            errorList.AddRange(ValidationUtility.ValidateName(recordslist.Name));

            errorList.AddRange(ValidationUtility.ValidateLabel(recordslist.Label));

            if (recordslist.Filters != null && recordslist.Filters.Count > 0)
            {

                foreach (var filter in recordslist.Filters)
                {
                    if (!filter.FieldId.HasValue || filter.FieldId.Value == Guid.Empty)
                        errorList.Add(new ErrorModel("recordsLists.filters.fieldId", null, "FieldId is required!"));

                    if (filter.EntityId.HasValue && filter.EntityId.Value != Guid.Empty)
                    {
                        IStorageEntity verifiedEntity = EntityRepository.Read(filter.EntityId.Value);

                        if (verifiedEntity != null || filter.EntityId == entity.Id)
                        {
                            Entity currentEntity = verifiedEntity != null ? ConvertEntityFromStorage(verifiedEntity) : entity;

                            if (currentEntity.Fields.Where(f => f.Id == filter.FieldId).Count() == 0)
                                errorList.Add(new ErrorModel("recordsLists.filters.fieldId", filter.FieldId.ToString(), "Filter with such Id does not exist!"));
                        }
                        else
                            errorList.Add(new ErrorModel("recordsLists.filters.entityId", filter.EntityId.ToString(), "Entity with such Id does not exist!"));
                    }
                    else
                        errorList.Add(new ErrorModel("recordsLists.filters.entityId", null, "EntityId is required!"));

                    if (string.IsNullOrWhiteSpace(filter.Value))
                        errorList.Add(new ErrorModel("recordsLists.filters.value", null, "Value is required!"));
                }
            }

            if (recordslist.Fields != null && recordslist.Fields.Count > 0)
            {
                foreach (var field in recordslist.Fields)
                {
                    if (!field.Id.HasValue || field.Id.Value == Guid.Empty)
                        errorList.Add(new ErrorModel("recordsLists.fields.id", null, "Id is required!"));

                    if (field.EntityId.HasValue && field.EntityId.Value != Guid.Empty)
                    {
                        IStorageEntity verifiedEntity = EntityRepository.Read(field.EntityId.Value);

                        if (verifiedEntity != null || field.EntityId == entity.Id)
                        {
                            Entity currentEntity = verifiedEntity != null ? ConvertEntityFromStorage(verifiedEntity) : entity;

                            if (currentEntity.Fields.Where(f => f.Id == field.Id).Count() == 0)
                                errorList.Add(new ErrorModel("recordsLists.fields.id", field.Id.ToString(), "Field with such Id does not exist!"));
                        }
                        else
                            errorList.Add(new ErrorModel("recordsLists.fields.entityId", field.EntityId.ToString(), "Entity with such Id does not exist!"));
                    }
                    else
                        errorList.Add(new ErrorModel("recordsLists.fields.entityId", null, "EntityId is required!"));

                    if (!field.Position.HasValue)
                        errorList.Add(new ErrorModel("recordsLists.fields.position", null, "Position is required!"));
                }
            }
            else
                errorList.Add(new ErrorModel("recordsLists.fields", recordslist.Fields.ToString(), "Fields cannot be null or empty. It must contain at least field!"));

            return errorList;
        }

        private List<ErrorModel> ValidateForms(Guid entityId, List<RecordView> recordViewList, bool checkId = false)
        {
            List<ErrorModel> errorList = new List<ErrorModel>();

            IStorageEntity storageEntity = EntityRepository.Read(entityId);
            Entity entity = ConvertEntityFromStorage(storageEntity);

            foreach (var recordView in recordViewList)
            {
                errorList.AddRange(ValidateForm(entity, recordView, checkId));
            }

            return errorList;
        }

        private List<ErrorModel> ValidateForm(Entity entity, RecordView recordView, bool checkId = false)
        {
            List<ErrorModel> errorList = new List<ErrorModel>();

            if (!recordView.Id.HasValue || recordView.Id.Value == Guid.Empty)
                errorList.Add(new ErrorModel("id", null, "Id is required!"));

            if (checkId)
            {
                int viewSameIdCount = entity.RecordsLists.Where(f => f.Id == recordView.Id).Count();

                if (viewSameIdCount > 1)
                    errorList.Add(new ErrorModel("id", null, "There is already a view with such Id!"));

                int viewSameNameCount = entity.Fields.Where(f => f.Name == recordView.Name).Count();

                if (viewSameNameCount > 1)
                    errorList.Add(new ErrorModel("name", null, "There is already a view with such Name!"));
            }

            errorList.AddRange(ValidationUtility.ValidateName(recordView.Name));

            errorList.AddRange(ValidationUtility.ValidateLabel(recordView.Label));

            foreach (var field in recordView.Fields)
            {
                if (!field.Id.HasValue && field.Id.Value == Guid.Empty)
                    errorList.Add(new ErrorModel("recordViewLists.fields.id", null, "Id is required!"));

                if (field.EntityId.HasValue && field.EntityId.Value != Guid.Empty)
                {
                    IStorageEntity verifiedEntity = EntityRepository.Read(field.EntityId.Value);

                    if (verifiedEntity != null || field.EntityId == entity.Id)
                    {
                        Entity currentEntity = verifiedEntity != null ? ConvertEntityFromStorage(verifiedEntity) : entity;

                        if (currentEntity.Fields.Where(f => f.Id == field.Id).Count() == 0)
                            errorList.Add(new ErrorModel("recordViewLists.fields.id", entity.Id.ToString(), "Field with such Id does not exist!"));
                    }
                    else
                        errorList.Add(new ErrorModel("recordViewLists.fields.entityId", field.EntityId.ToString(), "Entity with such Id does not exist!"));
                }
                else
                    errorList.Add(new ErrorModel("recordViewLists.fields.entityId", null, "EntityId is required!"));

                if (!field.Position.HasValue)
                    errorList.Add(new ErrorModel("recordViewLists.fields.position", null, "Position is required!"));

                if (!field.Column.HasValue)
                    errorList.Add(new ErrorModel("recordViewLists.column.position", null, "Column is required!"));
            }

            return errorList;
        }

        #endregion

        #region << Entity methods >>

        public Entity ConvertEntityFromStorage(IStorageEntity storageEntity)
        {
            //IERPService service = new 
            //EntityManager = new EntityManager(service.StorageService).CreateEntity(submitObj);
            Entity entity = new Entity(); ;

            entity.Id = storageEntity.Id;
            entity.Name = storageEntity.Name;
            entity.Label = storageEntity.Label;
            entity.LabelPlural = storageEntity.LabelPlural;
            entity.System = storageEntity.System;
            entity.IconName = storageEntity.IconName;
            entity.Weight = storageEntity.Weight;
            entity.RecordPermissions = new RecordPermissions();
            if (storageEntity.RecordPermissions != null)
            {
                entity.RecordPermissions.CanRead = storageEntity.RecordPermissions.CanRead;
                entity.RecordPermissions.CanCreate = storageEntity.RecordPermissions.CanCreate;
                entity.RecordPermissions.CanUpdate = storageEntity.RecordPermissions.CanUpdate;
                entity.RecordPermissions.CanDelete = storageEntity.RecordPermissions.CanDelete;
            }

            entity.Fields = new List<Field>();

            foreach (IStorageField storageField in storageEntity.Fields)
            {
                Field field = ConvertFieldFromStorage(storageField);

                entity.Fields.Add(field);
            }

            entity.RecordsLists = new List<RecordsList>();

            foreach (IStorageRecordsList storageRecordsList in storageEntity.RecordsLists)
            {
                RecordsList recordsList = new RecordsList();
                recordsList.Id = storageRecordsList.Id;
                recordsList.Name = storageRecordsList.Name;
                recordsList.Label = storageRecordsList.Label;
                recordsList.Type = storageRecordsList.Type;

                recordsList.Filters = new List<RecordsListFilter>();

                foreach (IStorageRecordsListFilter storageFilter in storageRecordsList.Filters)
                {
                    RecordsListFilter filter = new RecordsListFilter();

                    filter.EntityId = storageFilter.EntityId;
                    filter.FieldId = storageFilter.FieldId;
                    filter.Operator = storageFilter.Operator;
                    filter.Value = storageFilter.Value;

                    recordsList.Filters.Add(filter);
                }

                recordsList.Fields = new List<RecordsListField>();

                foreach (IStorageRecordsListField storageField in storageRecordsList.Fields)
                {
                    RecordsListField field = new RecordsListField();

                    field.EntityId = storageField.EntityId;
                    field.Id = storageField.Id;
                    field.Position = storageField.Position;

                    recordsList.Fields.Add(field);
                }

                entity.RecordsLists.Add(recordsList);
            }

            entity.RecordViewLists = new List<RecordView>();

            foreach (IStorageRecordView storageRecordView in storageEntity.RecordViewList)
            {
                RecordView recordView = new RecordView();
                recordView.Id = storageRecordView.Id;
                recordView.Name = storageRecordView.Name;
                recordView.Label = storageRecordView.Label;

                recordView.Fields = new List<RecordViewField>();

                foreach (IStorageRecordViewField storageField in storageRecordView.Fields)
                {
                    RecordViewField field = new RecordViewField();

                    field.EntityId = storageField.EntityId;
                    field.Id = storageField.Id;
                    field.Position = storageField.Position;

                    recordView.Fields.Add(field);
                }

                entity.RecordViewLists.Add(recordView);
            }

            return entity;
        }

        public IStorageEntity ConvertEntityToStorage(Entity entity)
        {
            IStorageEntity storageEntity = StorageObjectFactory.CreateEmptyEntityObject();

            storageEntity.Id = entity.Id;
            storageEntity.Name = entity.Name;
            storageEntity.Label = entity.Label;
            storageEntity.LabelPlural = entity.LabelPlural;
            storageEntity.System = entity.System;
            storageEntity.IconName = entity.IconName;
            storageEntity.Weight = entity.Weight;
            storageEntity.RecordPermissions = StorageObjectFactory.CreateEmptyRecordPermissionsObject();
            if (entity.RecordPermissions != null)
            {
                storageEntity.RecordPermissions.CanRead = entity.RecordPermissions.CanRead;
                storageEntity.RecordPermissions.CanCreate = entity.RecordPermissions.CanCreate;
                storageEntity.RecordPermissions.CanUpdate = entity.RecordPermissions.CanUpdate;
                storageEntity.RecordPermissions.CanDelete = entity.RecordPermissions.CanDelete;
            }
            storageEntity.Fields = new List<IStorageField>();

            foreach (Field field in entity.Fields)
            {
                IStorageField storageField = ConvertFieldToStorage(field);
                storageEntity.Fields.Add(storageField);
            }

            storageEntity.RecordsLists = new List<IStorageRecordsList>();

            foreach (RecordsList recordsList in entity.RecordsLists)
            {
                IStorageRecordsList storageRecordsList = StorageObjectFactory.CreateEmptyRecordsListObject();
                storageRecordsList.Id = recordsList.Id.Value;
                storageRecordsList.Name = recordsList.Name;
                storageRecordsList.Label = recordsList.Label;
                storageRecordsList.Type = recordsList.Type.Value;

                storageRecordsList.Filters = new List<IStorageRecordsListFilter>();

                foreach (RecordsListFilter filter in recordsList.Filters)
                {
                    IStorageRecordsListFilter storageFilter = StorageObjectFactory.CreateEmptyRecordsListFilterObject();

                    storageFilter.EntityId = filter.EntityId.Value;
                    storageFilter.FieldId = filter.FieldId.Value;
                    storageFilter.Operator = filter.Operator;
                    storageFilter.Value = filter.Value;

                    storageRecordsList.Filters.Add(storageFilter);
                }

                storageRecordsList.Fields = new List<IStorageRecordsListField>();

                foreach (RecordsListField field in recordsList.Fields)
                {
                    IStorageRecordsListField storageField = StorageObjectFactory.CreateEmptyRecordsListFieldObject();

                    storageField.EntityId = field.EntityId.Value;
                    storageField.Id = field.Id.Value;
                    storageField.Position = field.Position.Value;

                    storageRecordsList.Fields.Add(storageField);
                }

                storageEntity.RecordsLists.Add(storageRecordsList);
            }

            storageEntity.RecordViewList = new List<IStorageRecordView>();

            foreach (RecordView recordView in entity.RecordViewLists)
            {
                IStorageRecordView storageRecordView = StorageObjectFactory.CreateEmptyRecordViewObject();
                storageRecordView.Id = recordView.Id.Value;
                storageRecordView.Name = recordView.Name;
                storageRecordView.Label = recordView.Label;

                storageRecordView.Fields = new List<IStorageRecordViewField>();

                foreach (RecordViewField field in recordView.Fields)
                {
                    IStorageRecordViewField storageField = StorageObjectFactory.CreateEmptyRecordViewFieldObject();

                    storageField.EntityId = field.EntityId.Value;
                    storageField.Id = field.Id.Value;
                    storageField.Position = field.Position.Value;

                    storageRecordView.Fields.Add(storageField);
                }

                storageEntity.RecordViewList.Add(storageRecordView);
            }

            return storageEntity;
        }

        public EntityResponse CreateEntity(InputEntity inputEntity)
        {
            EntityResponse response = new EntityResponse
            {
                Success = true,
                Message = "The entity was successfully created!",
            };

            Entity entity = new Entity(inputEntity);

            try
            {
                response.Object = entity;
                //in order to support external IDs (while import in example)
                //we generate new ID only when it is not specified
                if (entity.Id == null)
                    entity.Id = Guid.NewGuid();

                response.Errors = ValidateEntity(entity, false);

                if (response.Errors.Count > 0)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The entity was not created. Validation error occurred!";
                    return response;
                }

                entity.Fields = CreateEntityDefaultFields(entity);
                entity.RecordsLists = CreateEntityDefaultRecordsLists(entity);
                entity.RecordViewLists = CreateEntityDefaultRecordViews(entity);

                IStorageEntity storageEntity = ConvertEntityToStorage(entity);
                bool result = EntityRepository.Create(storageEntity);
                if (!result)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The entity was not created! An internal error occurred!";
                    return response;
                }

                //TODO: create records collection

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

            IStorageEntity createdEntity = EntityRepository.Read(entity.Id);
            response.Object = ConvertEntityFromStorage(createdEntity);
            response.Timestamp = DateTime.UtcNow;

            return response;
        }

        public EntityResponse UpdateEntity(InputEntity inputEntity)
        {
            EntityResponse response = new EntityResponse
            {
                Success = true,
                Message = "The entity was successfully updated!",
            };

            Entity entity = new Entity(inputEntity);

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

                IStorageEntity storageEntity = EntityRepository.Read(entity.Id);

                storageEntity.Label = entity.Label;
                storageEntity.LabelPlural = entity.LabelPlural;
                storageEntity.System = entity.System;
                storageEntity.IconName = entity.IconName;
                storageEntity.Weight = entity.Weight;
                storageEntity.RecordPermissions.CanRead = entity.RecordPermissions.CanRead;
                storageEntity.RecordPermissions.CanCreate = entity.RecordPermissions.CanCreate;
                storageEntity.RecordPermissions.CanUpdate = entity.RecordPermissions.CanUpdate;
                storageEntity.RecordPermissions.CanDelete = entity.RecordPermissions.CanDelete;

                bool result = EntityRepository.Update(storageEntity);

                if (!result)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The entity was not updated! An internal error occurred!";
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
                response.Message = "The entity was not updated. An internal error occurred!";
#endif
                return response;
            }

            IStorageEntity updatedEntity = EntityRepository.Read(entity.Id);
            response.Object = ConvertEntityFromStorage(updatedEntity);
            response.Timestamp = DateTime.UtcNow;

            return response;
        }

        public EntityResponse PartialUpdateEntity(Guid id, InputEntity inputEntity)
        {
            EntityResponse response = new EntityResponse
            {
                Success = true,
                Message = "The entity was successfully updated!",
            };

            Entity entity = null;

            try
            {
                IStorageEntity storageEntity = EntityRepository.Read(id);
                entity = ConvertEntityFromStorage(storageEntity);

                if (inputEntity.Label != null)
                    entity.Label = inputEntity.Label;
                if (inputEntity.LabelPlural != null)
                    entity.LabelPlural = inputEntity.LabelPlural;
                if (inputEntity.System != null)
                    entity.System = inputEntity.System.Value;
                if (inputEntity.IconName != null)
                    entity.IconName = inputEntity.IconName;
                if (inputEntity.Weight != null)
                    entity.Weight = inputEntity.Weight.Value;
                if (inputEntity.RecordPermissions != null)
                    entity.RecordPermissions = inputEntity.RecordPermissions;

                response.Object = entity;
                response.Errors = ValidateEntity(entity, true);

                if (response.Errors.Count > 0)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The entity was not updated. Validation error occurred!";
                    return response;
                }

                storageEntity = ConvertEntityToStorage(entity);

                bool result = EntityRepository.Update(storageEntity);

                if (!result)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The entity was not updated! An internal error occurred!";
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
                response.Message = "The entity was not updated. An internal error occurred!";
#endif
                return response;
            }

            IStorageEntity updatedEntity = EntityRepository.Read(entity.Id);
            response.Object = ConvertEntityFromStorage(updatedEntity);
            response.Timestamp = DateTime.UtcNow;

            return response;
        }

        public EntityResponse DeleteEntity(Guid id)
        {
            EntityResponse response = new EntityResponse
            {
                Success = true,
                Message = "The entity was successfully deleted!",
            };

            try
            {
                IStorageEntity entity = EntityRepository.Read(id);

                if (entity == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The entity was not deleted. Validation error occurred!";
                    response.Errors.Add(new ErrorModel("id", id.ToString(), "Entity with such Id does not exist!"));
                    return response;
                }

                //TODO: Delete records and delete records of related entities

                EntityRepository.Delete(id);
            }
            catch (Exception e)
            {
                response.Timestamp = DateTime.UtcNow;
                response.Success = false;
#if DEBUG
                response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The entity was not deleted. An internal error occurred!";
#endif
                return response;
            }

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

            try
            {
                List<IStorageEntity> storageEntityList = EntityRepository.Read();

                EntityList entityList = new EntityList();
                foreach (var storageEntity in storageEntityList)
                {
                    Entity entity = ConvertEntityFromStorage(storageEntity);
                    entityList.Entities.Add(entity);
                }

                response.Object = entityList;
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

            Entity entity = new Entity();

            try
            {
                IStorageEntity storageEntity = EntityRepository.Read(id);
                if (storageEntity != null)
                    response.Object = ConvertEntityFromStorage(storageEntity);
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

            Entity entity = new Entity();

            try
            {
                IStorageEntity storageEntity = EntityRepository.Read(name);
                if (storageEntity != null)
                    response.Object = ConvertEntityFromStorage(storageEntity);
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

        public Field ConvertFieldFromStorage(IStorageField storageField)
        {
            Field field = null;
            if (storageField is IStorageAutoNumberField)
            {
                field = new AutoNumberField();
                ((AutoNumberField)field).DefaultValue = ((IStorageAutoNumberField)storageField).DefaultValue;
                ((AutoNumberField)field).DisplayFormat = ((IStorageAutoNumberField)storageField).DisplayFormat;
                ((AutoNumberField)field).StartingNumber = ((IStorageAutoNumberField)storageField).StartingNumber;
            }
            else if (storageField is IStorageCheckboxField)
            {
                field = new CheckboxField();
                ((CheckboxField)field).DefaultValue = ((IStorageCheckboxField)storageField).DefaultValue;
            }
            else if (storageField is IStorageCurrencyField)
            {
                field = new CurrencyField();
                ((CurrencyField)field).DefaultValue = ((IStorageCurrencyField)storageField).DefaultValue;
                ((CurrencyField)field).MinValue = ((IStorageCurrencyField)storageField).MinValue;
                ((CurrencyField)field).MaxValue = ((IStorageCurrencyField)storageField).MaxValue;
                ((CurrencyField)field).Currency = ((IStorageCurrencyField)storageField).Currency;
            }
            else if (storageField is IStorageDateField)
            {
                field = new DateField();
                ((DateField)field).DefaultValue = ((IStorageDateField)storageField).DefaultValue;
                ((DateField)field).Format = ((IStorageDateField)storageField).Format;
                ((DateField)field).UseCurrentTimeAsDefaultValue = ((IStorageDateField)storageField).UseCurrentTimeAsDefaultValue;
            }
            else if (storageField is IStorageDateTimeField)
            {
                field = new DateTimeField();
                ((DateTimeField)field).DefaultValue = ((IStorageDateTimeField)storageField).DefaultValue;
                ((DateTimeField)field).Format = ((IStorageDateTimeField)storageField).Format;
                ((DateTimeField)field).UseCurrentTimeAsDefaultValue = ((IStorageDateTimeField)storageField).UseCurrentTimeAsDefaultValue;
            }
            else if (storageField is IStorageEmailField)
            {
                field = new EmailField();
                ((EmailField)field).DefaultValue = ((IStorageEmailField)storageField).DefaultValue;
                ((EmailField)field).MaxLength = ((IStorageEmailField)storageField).MaxLength;
            }
            else if (storageField is IStorageFileField)
            {
                field = new FileField();
                ((FileField)field).DefaultValue = ((IStorageFileField)storageField).DefaultValue;
            }
            //else if (storageField is IStorageFormulaField)
            //{
            //    field = new FormulaField();
            //    ((FormulaField)field).ReturnType = ((IStorageFormulaField)storageField).ReturnType;
            //    ((FormulaField)field).FormulaText = ((IStorageFormulaField)storageField).FormulaText;
            //    ((FormulaField)field).DecimalPlaces = ((IStorageFormulaField)storageField).DecimalPlaces;
            //}
            else if (storageField is IStorageHtmlField)
            {
                field = new HtmlField();
                ((HtmlField)field).DefaultValue = ((IStorageHtmlField)storageField).DefaultValue;
            }
            else if (storageField is IStorageImageField)
            {
                field = new ImageField();
                ((ImageField)field).DefaultValue = ((IStorageImageField)storageField).DefaultValue;
            }
            else if (storageField is IStorageImageField)
            {
                field = new ImageField();
                ((ImageField)field).DefaultValue = ((IStorageImageField)storageField).DefaultValue;
            }
            else if (storageField is IStorageMultiLineTextField)
            {
                field = new MultiLineTextField();
                ((MultiLineTextField)field).DefaultValue = ((IStorageMultiLineTextField)storageField).DefaultValue;
                ((MultiLineTextField)field).MaxLength = ((IStorageMultiLineTextField)storageField).MaxLength;
                ((MultiLineTextField)field).VisibleLineNumber = ((IStorageMultiLineTextField)storageField).VisibleLineNumber;
            }
            else if (storageField is IStorageMultiSelectField)
            {
                field = new MultiSelectField();
                ((MultiSelectField)field).DefaultValue = ((IStorageMultiSelectField)storageField).DefaultValue;
                ((MultiSelectField)field).Options = MultiSelectField.ConvertOptions(((IStorageMultiSelectField)storageField).Options);
            }
            else if (storageField is IStorageNumberField)
            {
                field = new NumberField();
                ((NumberField)field).DefaultValue = ((IStorageNumberField)storageField).DefaultValue;
                ((NumberField)field).MinValue = ((IStorageNumberField)storageField).MinValue;
                ((NumberField)field).MaxValue = ((IStorageNumberField)storageField).MaxValue;
                ((NumberField)field).DecimalPlaces = ((IStorageNumberField)storageField).DecimalPlaces;
            }
            else if (storageField is IStoragePasswordField)
            {
                field = new PasswordField();
                ((PasswordField)field).Encrypted = ((IStoragePasswordField)storageField).Encrypted;
                ((PasswordField)field).MaxLength = ((IStoragePasswordField)storageField).MaxLength;
                ((PasswordField)field).MinLength = ((IStoragePasswordField)storageField).MinLength;
            }
            else if (storageField is IStoragePercentField)
            {
                field = new PercentField();
                ((PercentField)field).DefaultValue = ((IStoragePercentField)storageField).DefaultValue;
                ((PercentField)field).MinValue = ((IStoragePercentField)storageField).MinValue;
                ((PercentField)field).MaxValue = ((IStoragePercentField)storageField).MaxValue;
                ((PercentField)field).DecimalPlaces = ((IStoragePercentField)storageField).DecimalPlaces;
            }
            else if (storageField is IStoragePhoneField)
            {
                field = new PhoneField();
                ((PhoneField)field).DefaultValue = ((IStoragePhoneField)storageField).DefaultValue;
                ((PhoneField)field).Format = ((IStoragePhoneField)storageField).Format;
                ((PhoneField)field).MaxLength = ((IStoragePhoneField)storageField).MaxLength;
            }
            else if (storageField is IStorageGuidField)
            {
                field = new GuidField();
                ((GuidField)field).DefaultValue = ((IStorageGuidField)storageField).DefaultValue;
                ((GuidField)field).GenerateNewId = ((IStorageGuidField)storageField).GenerateNewId;
            }
            else if (storageField is IStorageSelectField)
            {
                field = new SelectField();
                ((SelectField)field).DefaultValue = ((IStorageSelectField)storageField).DefaultValue;
                ((SelectField)field).Options = SelectField.ConvertOptions(((IStorageSelectField)storageField).Options);
            }
            else if (storageField is IStorageTextField)
            {
                field = new TextField();
                ((TextField)field).DefaultValue = ((IStorageTextField)storageField).DefaultValue;
                ((TextField)field).MaxLength = ((IStorageTextField)storageField).MaxLength;
            }
            else if (storageField is IStorageUrlField)
            {
                field = new UrlField();
                ((UrlField)field).DefaultValue = ((IStorageUrlField)storageField).DefaultValue;
                ((UrlField)field).MaxLength = ((IStorageUrlField)storageField).MaxLength;
                ((UrlField)field).OpenTargetInNewWindow = ((IStorageUrlField)storageField).OpenTargetInNewWindow;
            }

            field.Id = storageField.Id;
            field.Name = storageField.Name;
            field.Label = storageField.Label;
            field.PlaceholderText = storageField.PlaceholderText;
            field.Description = storageField.Description;
            field.HelpText = storageField.HelpText;
            field.Required = storageField.Required;
            field.Unique = storageField.Unique;
            field.Searchable = storageField.Searchable;
            field.Auditable = storageField.Auditable;
            field.System = storageField.System;

            return field;
        }

        public IStorageField ConvertFieldToStorage(Field field)
        {
            if (field == null)
                throw new ArgumentNullException("field");

            IStorageField storageField = null;
            if (field is AutoNumberField)
            {
                storageField = StorageObjectFactory.CreateEmptyFieldObject(typeof(AutoNumberField));
                ((IStorageAutoNumberField)storageField).DefaultValue = ((AutoNumberField)field).DefaultValue;
                ((IStorageAutoNumberField)storageField).DisplayFormat = ((AutoNumberField)field).DisplayFormat;
                ((IStorageAutoNumberField)storageField).StartingNumber = ((AutoNumberField)field).StartingNumber;
            }
            else if (field is CheckboxField)
            {
                storageField = StorageObjectFactory.CreateEmptyFieldObject(typeof(CheckboxField));
                ((IStorageCheckboxField)storageField).DefaultValue = ((CheckboxField)field).DefaultValue.Value;
            }
            else if (field is CurrencyField)
            {
                storageField = StorageObjectFactory.CreateEmptyFieldObject(typeof(CurrencyField));
                ((IStorageCurrencyField)storageField).DefaultValue = ((CurrencyField)field).DefaultValue;
                ((IStorageCurrencyField)storageField).MinValue = ((CurrencyField)field).MinValue;
                ((IStorageCurrencyField)storageField).MaxValue = ((CurrencyField)field).MaxValue;
                ((IStorageCurrencyField)storageField).Currency = ((CurrencyField)field).Currency;
            }
            else if (field is DateField)
            {
                storageField = StorageObjectFactory.CreateEmptyFieldObject(typeof(DateField));
                ((IStorageDateField)storageField).DefaultValue = ((DateField)field).DefaultValue;
                ((IStorageDateField)storageField).Format = ((DateField)field).Format;
                ((IStorageDateField)storageField).UseCurrentTimeAsDefaultValue = ((DateField)field).UseCurrentTimeAsDefaultValue.Value;
            }
            else if (field is DateTimeField)
            {
                storageField = StorageObjectFactory.CreateEmptyFieldObject(typeof(DateTimeField));
                ((IStorageDateTimeField)storageField).DefaultValue = ((DateTimeField)field).DefaultValue;
                ((IStorageDateTimeField)storageField).Format = ((DateTimeField)field).Format;
                ((IStorageDateTimeField)storageField).UseCurrentTimeAsDefaultValue = ((DateTimeField)field).UseCurrentTimeAsDefaultValue.Value;
            }
            else if (field is EmailField)
            {
                storageField = StorageObjectFactory.CreateEmptyFieldObject(typeof(EmailField));
                ((IStorageEmailField)storageField).DefaultValue = ((EmailField)field).DefaultValue;
                ((IStorageEmailField)storageField).MaxLength = ((EmailField)field).MaxLength;
            }
            else if (field is FileField)
            {
                storageField = StorageObjectFactory.CreateEmptyFieldObject(typeof(FileField));
                ((IStorageFileField)storageField).DefaultValue = ((FileField)field).DefaultValue;
            }
            //else if (field is FormulaField)
            //{
            //    storageField = new MongoFormulaField();
            //    ((MongoFormulaField)storageField).ReturnType = ((FormulaField)field).ReturnType;
            //    ((MongoFormulaField)storageField).FormulaText = ((FormulaField)field).FormulaText;
            //    ((MongoFormulaField)storageField).DecimalPlaces = ((FormulaField)field).DecimalPlaces;
            //}
            else if (field is HtmlField)
            {
                storageField = StorageObjectFactory.CreateEmptyFieldObject(typeof(HtmlField));
                ((IStorageHtmlField)storageField).DefaultValue = ((HtmlField)field).DefaultValue;
            }
            else if (field is ImageField)
            {
                storageField = StorageObjectFactory.CreateEmptyFieldObject(typeof(ImageField));
                ((IStorageImageField)storageField).DefaultValue = ((ImageField)field).DefaultValue;
            }
            else if (field is MultiLineTextField)
            {
                storageField = StorageObjectFactory.CreateEmptyFieldObject(typeof(MultiLineTextField));
                ((IStorageMultiLineTextField)storageField).DefaultValue = ((MultiLineTextField)field).DefaultValue;
                ((IStorageMultiLineTextField)storageField).MaxLength = ((MultiLineTextField)field).MaxLength;
                ((IStorageMultiLineTextField)storageField).VisibleLineNumber = ((MultiLineTextField)field).VisibleLineNumber;
            }
            else if (field is MultiSelectField)
            {
                storageField = StorageObjectFactory.CreateEmptyFieldObject(typeof(MultiSelectField));
                ((IStorageMultiSelectField)storageField).DefaultValue = ((MultiSelectField)field).DefaultValue;
                ((IStorageMultiSelectField)storageField).Options = new List<IStorageMultiSelectFieldOption>();
                foreach (var option in ((MultiSelectField)field).Options)
                {
                    IStorageMultiSelectFieldOption storeOption = StorageObjectFactory.CreateEmptyMultiSelectFieldOptionObject();
                    storeOption.Key = option.Key;
                    storeOption.Value = option.Value;

                    ((IStorageMultiSelectField)storageField).Options.Add(storeOption);
                }
            }
            else if (field is NumberField)
            {
                storageField = StorageObjectFactory.CreateEmptyFieldObject(typeof(NumberField));
                ((IStorageNumberField)storageField).DefaultValue = ((NumberField)field).DefaultValue;
                ((IStorageNumberField)storageField).MinValue = ((NumberField)field).MinValue;
                ((IStorageNumberField)storageField).MaxValue = ((NumberField)field).MaxValue;
                ((IStorageNumberField)storageField).DecimalPlaces = ((NumberField)field).DecimalPlaces.Value;
            }
            else if (field is PasswordField)
            {
                storageField = StorageObjectFactory.CreateEmptyFieldObject(typeof(PasswordField));
                ((IStoragePasswordField)storageField).MaxLength = ((PasswordField)field).MaxLength;
                ((IStoragePasswordField)storageField).MinLength = ((PasswordField)field).MinLength;
                ((IStoragePasswordField)storageField).Encrypted = ((PasswordField)field).Encrypted ?? true;
            }
            else if (field is PercentField)
            {
                storageField = StorageObjectFactory.CreateEmptyFieldObject(typeof(PercentField));
                ((IStoragePercentField)storageField).DefaultValue = ((PercentField)field).DefaultValue;
                ((IStoragePercentField)storageField).MinValue = ((PercentField)field).MinValue;
                ((IStoragePercentField)storageField).MaxValue = ((PercentField)field).MaxValue;
                ((IStoragePercentField)storageField).DecimalPlaces = ((PercentField)field).DecimalPlaces.Value;
            }
            else if (field is PhoneField)
            {
                storageField = StorageObjectFactory.CreateEmptyFieldObject(typeof(PhoneField));
                ((IStoragePhoneField)storageField).DefaultValue = ((PhoneField)field).DefaultValue;
                ((IStoragePhoneField)storageField).Format = ((PhoneField)field).Format;
                ((IStoragePhoneField)storageField).MaxLength = ((PhoneField)field).MaxLength;
            }
            else if (field is GuidField)
            {
                storageField = StorageObjectFactory.CreateEmptyFieldObject(typeof(GuidField));
                ((IStorageGuidField)storageField).DefaultValue = ((GuidField)field).DefaultValue;
                ((IStorageGuidField)storageField).GenerateNewId = ((GuidField)field).GenerateNewId;
            }
            else if (field is SelectField)
            {
                storageField = StorageObjectFactory.CreateEmptyFieldObject(typeof(SelectField));
                ((IStorageSelectField)storageField).DefaultValue = ((SelectField)field).DefaultValue;
                ((IStorageSelectField)storageField).Options = new List<IStorageSelectFieldOption>();
                foreach (var option in ((SelectField)field).Options)
                {
                    IStorageSelectFieldOption storeOption = StorageObjectFactory.CreateEmptySelectFieldOptionObject();
                    storeOption.Key = option.Key;
                    storeOption.Value = option.Value;

                    ((IStorageSelectField)storageField).Options.Add(storeOption);
                }

            }
            else if (field is TextField)
            {
                storageField = StorageObjectFactory.CreateEmptyFieldObject(typeof(TextField));
                ((IStorageTextField)storageField).DefaultValue = ((TextField)field).DefaultValue;
                ((IStorageTextField)storageField).MaxLength = ((TextField)field).MaxLength;
            }
            else if (field is UrlField)
            {
                storageField = StorageObjectFactory.CreateEmptyFieldObject(typeof(UrlField));
                ((IStorageUrlField)storageField).DefaultValue = ((UrlField)field).DefaultValue;
                ((IStorageUrlField)storageField).MaxLength = ((UrlField)field).MaxLength;
                ((IStorageUrlField)storageField).OpenTargetInNewWindow = ((UrlField)field).OpenTargetInNewWindow.Value;
            }

            storageField.Id = field.Id;
            storageField.Name = field.Name;
            storageField.Label = field.Label;
            storageField.PlaceholderText = field.PlaceholderText;
            storageField.Description = field.Description;
            storageField.HelpText = field.HelpText;
            storageField.Required = field.Required;
            storageField.Unique = field.Unique;
            storageField.Searchable = field.Searchable;
            storageField.Auditable = field.Auditable;
            storageField.System = field.System;

            return storageField;
        }

        public FieldResponse CreateField(Guid entityId, InputField inputField)
        {
            FieldResponse response = new FieldResponse
            {
                Success = true,
                Message = "The field was successfully created!",
            };

            Field field = null;

            try
            {
                IStorageEntity storageEntity = EntityRepository.Read(entityId);

                if (storageEntity == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Entity with such Id does not exist!";
                    return response;
                }

                if (inputField.Id == null || inputField.Id == Guid.Empty)
                    inputField.Id = Guid.NewGuid();

                Entity entity = ConvertEntityFromStorage(storageEntity);

                response.Errors = ValidateField(entity, inputField, false);

                field = Field.ConvertField(inputField);

                if (response.Errors.Count > 0)
                {
                    response.Object = field;
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The field was not created. Validation error occurred!";
                    return response;
                }

                entity.Fields.Add(field);

                IStorageEntity editedEntity = ConvertEntityToStorage(entity);

                var recRep = Storage.GetRecordRepository();
                var transaction = recRep.CreateTransaction();
                try
                {

                    transaction.Begin();

                    recRep.CreateRecordField(entity.Name, field.Name, field.GetDefaultValue());


                    bool result = EntityRepository.Update(editedEntity);
                    if (!result)
                    {
                        response.Timestamp = DateTime.UtcNow;
                        response.Success = false;
                        response.Message = "The field was not created! An internal error occurred!";
                        return response;
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }

            }
            catch (Exception e)
            {
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

            response.Object = field;
            response.Timestamp = DateTime.UtcNow;

            return response;
        }

        public FieldResponse UpdateField(Guid entityId, InputField inputField)
        {
            FieldResponse response = new FieldResponse
            {
                Success = true,
                Message = "The field was successfully updated!",
            };

            Field field = null;

            try
            {
                IStorageEntity storageEntity = EntityRepository.Read(entityId);

                if (storageEntity == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Entity with such Id does not exist!";
                    return response;
                }

                Entity entity = ConvertEntityFromStorage(storageEntity);

                response.Errors = ValidateField(entity, inputField, true);

                field = Field.ConvertField(inputField);

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

                IStorageEntity updatedEntity = ConvertEntityToStorage(entity);
                bool result = EntityRepository.Update(updatedEntity);
                if (!result)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The field was not updated! An internal error occurred!";
                    return response;
                }

            }
            catch (Exception e)
            {
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

            response.Object = field;
            response.Timestamp = DateTime.UtcNow;

            return response;
        }

        public FieldResponse PartialUpdateField(Guid entityId, Guid id, InputField inputField)
        {
            FieldResponse response = new FieldResponse
            {
                Success = true,
                Message = "The field was successfully updated!",
            };

            Field updatedField = null;

            try
            {
                IStorageEntity storageEntity = EntityRepository.Read(entityId);

                if (storageEntity == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Entity with such Id does not exist!";
                    return response;
                }

                Entity entity = ConvertEntityFromStorage(storageEntity);

                updatedField = entity.Fields.FirstOrDefault(f => f.Id == id);

                if (updatedField == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Field with such Id does not exist!";
                    return response;
                }

                if (updatedField is AutoNumberField)
                {
                    if (((InputAutoNumberField)inputField).DefaultValue != null)
                        ((AutoNumberField)updatedField).DefaultValue = ((InputAutoNumberField)inputField).DefaultValue;
                    if (((InputAutoNumberField)inputField).DisplayFormat != null)
                        ((AutoNumberField)updatedField).DisplayFormat = ((InputAutoNumberField)inputField).DisplayFormat;
                    if (((InputAutoNumberField)inputField).StartingNumber != null)
                        ((AutoNumberField)updatedField).StartingNumber = ((InputAutoNumberField)inputField).StartingNumber;
                }
                else if (updatedField is CheckboxField)
                {
                    if (((InputCheckboxField)inputField).DefaultValue != null)
                        ((CheckboxField)updatedField).DefaultValue = ((InputCheckboxField)inputField).DefaultValue;
                }
                else if (updatedField is CurrencyField)
                {
                    if (((InputCurrencyField)inputField).DefaultValue != null)
                        ((CurrencyField)updatedField).DefaultValue = ((InputCurrencyField)inputField).DefaultValue;
                    if (((InputCurrencyField)inputField).MinValue != null)
                        ((CurrencyField)updatedField).MinValue = ((InputCurrencyField)inputField).MinValue;
                    if (((InputCurrencyField)inputField).MaxValue != null)
                        ((CurrencyField)updatedField).MaxValue = ((InputCurrencyField)inputField).MaxValue;
                    if (((InputCurrencyField)inputField).Currency != null)
                        ((CurrencyField)updatedField).Currency = ((InputCurrencyField)inputField).Currency;
                }
                else if (updatedField is DateField)
                {
                    if (((InputDateField)inputField).DefaultValue != null)
                        ((DateField)updatedField).DefaultValue = ((InputDateField)inputField).DefaultValue;
                    if (((InputDateField)inputField).Format != null)
                        ((DateField)updatedField).Format = ((InputDateField)inputField).Format;
                    if (((InputDateField)inputField).UseCurrentTimeAsDefaultValue != null)
                        ((DateField)updatedField).UseCurrentTimeAsDefaultValue = ((InputDateField)inputField).UseCurrentTimeAsDefaultValue;
                }
                else if (updatedField is DateTimeField)
                {
                    if (((InputDateTimeField)inputField).DefaultValue != null)
                        ((DateTimeField)updatedField).DefaultValue = ((InputDateTimeField)inputField).DefaultValue;
                    if (((InputDateTimeField)inputField).Format != null)
                        ((DateTimeField)updatedField).Format = ((InputDateTimeField)inputField).Format;
                    if (((InputDateTimeField)inputField).UseCurrentTimeAsDefaultValue != null)
                        ((DateTimeField)updatedField).UseCurrentTimeAsDefaultValue = ((InputDateTimeField)inputField).UseCurrentTimeAsDefaultValue;
                }
                else if (updatedField is EmailField)
                {
                    if (((InputEmailField)inputField).DefaultValue != null)
                        ((EmailField)updatedField).DefaultValue = ((InputEmailField)inputField).DefaultValue;
                    if (((InputEmailField)inputField).MaxLength != null)
                        ((EmailField)updatedField).MaxLength = ((InputEmailField)inputField).MaxLength;
                }
                else if (updatedField is FileField)
                {
                    if (((InputFileField)inputField).DefaultValue != null)
                        ((FileField)updatedField).DefaultValue = ((InputFileField)inputField).DefaultValue;
                }
                else if (updatedField is HtmlField)
                {
                    if (((InputHtmlField)inputField).DefaultValue != null)
                        ((HtmlField)updatedField).DefaultValue = ((InputHtmlField)inputField).DefaultValue;
                }
                else if (updatedField is ImageField)
                {
                    if (((InputImageField)inputField).DefaultValue != null)
                        ((ImageField)updatedField).DefaultValue = ((InputImageField)inputField).DefaultValue;
                }
                else if (updatedField is MultiLineTextField)
                {
                    if (((InputMultiLineTextField)inputField).DefaultValue != null)
                        ((MultiLineTextField)updatedField).DefaultValue = ((InputMultiLineTextField)inputField).DefaultValue;
                    if (((InputMultiLineTextField)inputField).MaxLength != null)
                        ((MultiLineTextField)updatedField).MaxLength = ((InputMultiLineTextField)inputField).MaxLength;
                    if (((InputMultiLineTextField)inputField).VisibleLineNumber != null)
                        ((MultiLineTextField)updatedField).VisibleLineNumber = ((InputMultiLineTextField)inputField).VisibleLineNumber;
                }
                else if (updatedField is MultiSelectField)
                {
                    if (((InputMultiSelectField)inputField).DefaultValue != null)
                        ((MultiSelectField)updatedField).DefaultValue = ((InputMultiSelectField)inputField).DefaultValue;
                    if (((InputMultiSelectField)inputField).Options != null)
                        ((MultiSelectField)updatedField).Options = ((InputMultiSelectField)inputField).Options;
                }
                else if (updatedField is NumberField)
                {
                    if (((InputNumberField)inputField).DefaultValue != null)
                        ((NumberField)updatedField).DefaultValue = ((InputNumberField)inputField).DefaultValue;
                    if (((InputNumberField)inputField).MinValue != null)
                        ((NumberField)updatedField).MinValue = ((InputNumberField)inputField).MinValue;
                    if (((InputNumberField)inputField).MaxValue != null)
                        ((NumberField)updatedField).MaxValue = ((InputNumberField)inputField).MaxValue;
                    if (((InputNumberField)inputField).DecimalPlaces != null)
                        ((NumberField)updatedField).DecimalPlaces = ((InputNumberField)inputField).DecimalPlaces;
                }
                else if (updatedField is PasswordField)
                {
                    if (((InputPasswordField)inputField).MaxLength != null)
                        ((PasswordField)updatedField).MaxLength = ((InputPasswordField)inputField).MaxLength;
                    if (((InputPasswordField)inputField).MinLength != null)
                        ((PasswordField)updatedField).MinLength = ((InputPasswordField)inputField).MinLength;
                    if (((InputPasswordField)inputField).Encrypted != null)
                        ((PasswordField)updatedField).Encrypted = ((InputPasswordField)inputField).Encrypted;
                }
                else if (updatedField is PercentField)
                {
                    if (((InputPercentField)inputField).DefaultValue != null)
                        ((PercentField)updatedField).DefaultValue = ((InputPercentField)inputField).DefaultValue;
                    if (((InputPercentField)inputField).MinValue != null)
                        ((PercentField)updatedField).MinValue = ((InputPercentField)inputField).MinValue;
                    if (((InputPercentField)inputField).MaxValue != null)
                        ((PercentField)updatedField).MaxValue = ((InputPercentField)inputField).MaxValue;
                    if (((InputPercentField)inputField).DecimalPlaces != null)
                        ((PercentField)updatedField).DecimalPlaces = ((InputPercentField)inputField).DecimalPlaces;
                }
                else if (updatedField is PhoneField)
                {
                    if (((InputPhoneField)inputField).DefaultValue != null)
                        ((PhoneField)updatedField).DefaultValue = ((InputPhoneField)inputField).DefaultValue;
                    if (((InputPhoneField)inputField).Format != null)
                        ((PhoneField)updatedField).Format = ((InputPhoneField)inputField).Format;
                    if (((InputPhoneField)inputField).MaxLength != null)
                        ((PhoneField)updatedField).MaxLength = ((InputPhoneField)inputField).MaxLength;
                }
                else if (updatedField is GuidField)
                {
                    if (((InputGuidField)inputField).DefaultValue != null)
                        ((GuidField)updatedField).DefaultValue = ((InputGuidField)inputField).DefaultValue;
                    if (((InputGuidField)inputField).GenerateNewId != null)
                        ((GuidField)updatedField).GenerateNewId = ((InputGuidField)inputField).GenerateNewId;
                }
                else if (updatedField is SelectField)
                {
                    if (((InputSelectField)inputField).DefaultValue != null)
                        ((SelectField)updatedField).DefaultValue = ((InputSelectField)inputField).DefaultValue;
                    if (((InputSelectField)inputField).Options != null)
                        ((SelectField)updatedField).Options = ((InputSelectField)inputField).Options;
                }
                else if (updatedField is TextField)
                {
                    if (((InputTextField)inputField).DefaultValue != null)
                        ((TextField)updatedField).DefaultValue = ((InputTextField)inputField).DefaultValue;
                    if (((InputTextField)inputField).MaxLength != null)
                        ((TextField)updatedField).MaxLength = ((InputTextField)inputField).MaxLength;
                }
                else if (updatedField is UrlField)
                {
                    if (((InputUrlField)inputField).DefaultValue != null)
                        ((UrlField)updatedField).DefaultValue = ((InputUrlField)inputField).DefaultValue;
                    if (((InputUrlField)inputField).MaxLength != null)
                        ((UrlField)updatedField).MaxLength = ((InputUrlField)inputField).MaxLength;
                    if (((InputUrlField)inputField).OpenTargetInNewWindow != null)
                        ((UrlField)updatedField).OpenTargetInNewWindow = ((InputUrlField)inputField).OpenTargetInNewWindow;
                }

                if (inputField.Label != null)
                    updatedField.Label = inputField.Label;
                else if (inputField.PlaceholderText != null)
                    updatedField.PlaceholderText = inputField.PlaceholderText;
                else if (inputField.Description != null)
                    updatedField.Description = inputField.Description;
                else if (inputField.HelpText != null)
                    updatedField.HelpText = inputField.HelpText;
                else if (inputField.Required != null)
                    updatedField.Required = inputField.Required.Value;
                else if (inputField.Unique != null)
                    updatedField.Unique = inputField.Unique.Value;
                else if (inputField.Searchable != null)
                    updatedField.Searchable = inputField.Searchable.Value;
                else if (inputField.Auditable != null)
                    updatedField.Auditable = inputField.Auditable.Value;
                else if (inputField.System != null)
                    updatedField.System = inputField.System.Value;

                response.Object = updatedField;
                response.Errors = ValidateField(entity, InputField.ConvertField(updatedField), true);

                if (response.Errors.Count > 0)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The field was not updated. Validation error occurred!";
                    return response;
                }

                IStorageEntity updatedEntity = ConvertEntityToStorage(entity);
                bool result = EntityRepository.Update(updatedEntity);
                if (!result)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The field was not updated! An internal error occurred!";
                    return response;
                }

            }
            catch (Exception e)
            {
                response.Success = false;
                response.Object = updatedField;
                response.Timestamp = DateTime.UtcNow;
#if DEBUG
                response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The field was not updated. An internal error occurred!";
#endif
                return response;
            }

            response.Object = updatedField;
            response.Timestamp = DateTime.UtcNow;

            return response;
        }

        public FieldResponse DeleteField(Guid entityId, Guid id)
        {
            FieldResponse response = new FieldResponse
            {
                Success = true,
                Message = "The field was successfully deleted!",
            };

            try
            {
                IStorageEntity storageEntity = EntityRepository.Read(entityId);

                if (storageEntity == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Entity with such Id does not exist!";
                    return response;
                }

                Entity entity = ConvertEntityFromStorage(storageEntity);

                Field field = entity.Fields.FirstOrDefault(f => f.Id == id);

                if (field == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The field was not deleted. Validation error occurred!";
                    response.Errors.Add(new ErrorModel("id", id.ToString(), "Field with such Id does not exist!"));
                    return response;
                }

                entity.Fields.Remove(field);

                var recRep = Storage.GetRecordRepository();
                var transaction = recRep.CreateTransaction();
                try
                {
                    transaction.Begin();

                    recRep.RemoveRecordField(entity.Name, field.Name);

                    IStorageEntity updatedEntity = ConvertEntityToStorage(entity);
                    bool result = EntityRepository.Update(updatedEntity);
                    if (!result)
                    {
                        response.Timestamp = DateTime.UtcNow;
                        response.Success = false;
                        response.Message = "The field was not updated! An internal error occurred!";
                        return response;
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            catch (Exception e)
            {
                response.Timestamp = DateTime.UtcNow;
                response.Success = false;
#if DEBUG
                response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The field was not deleted. An internal error occurred!";
#endif
                return response;
            }

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
                IStorageEntity storageEntity = EntityRepository.Read(entityId);

                if (storageEntity == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Entity with such Id does not exist!";
                    return response;
                }

                FieldList fieldList = new FieldList();
                fieldList.Fields = new List<Field>();

                foreach (IStorageField storageField in storageEntity.Fields)
                {
                    fieldList.Fields.Add(ConvertFieldFromStorage(storageField));
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
                IStorageEntity storageEntity = EntityRepository.Read(entityId);

                if (storageEntity == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Entity with such Id does not exist!";
                    return response;
                }

                IStorageField storageField = storageEntity.Fields.FirstOrDefault(f => f.Id == id);

                if (storageField == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Validation error occurred!";
                    response.Errors.Add(new ErrorModel("id", id.ToString(), "Field with such Id does not exist!"));
                    return response;
                }

                Field field = ConvertFieldFromStorage(storageField);
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

        public RecordsListResponse CreateRecordsList(Guid entityId, RecordsList recordsList)
        {
            RecordsListResponse response = new RecordsListResponse
            {
                Success = true,
                Message = "The list was successfully created!",
            };

            try
            {
                IStorageEntity storageEntity = EntityRepository.Read(entityId);

                if (storageEntity == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Entity with such Id does not exist!";
                    return response;
                }

                Entity entity = ConvertEntityFromStorage(storageEntity);

                response.Object = recordsList;
                response.Errors = ValidateView(entity, recordsList, false);

                if (response.Errors.Count > 0)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The list was not created. Validation error occurred!";
                    return response;
                }

                entity.RecordsLists.Add(recordsList);

                IStorageEntity updatedEntity = ConvertEntityToStorage(entity);
                bool result = EntityRepository.Update(updatedEntity);
                if (!result)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The list was not created! An internal error occurred!";
                    return response;
                }

            }
            catch (Exception e)
            {
                response.Success = false;
                response.Object = recordsList;
                response.Timestamp = DateTime.UtcNow;
#if DEBUG
                response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The list was not created. An internal error occurred!";
#endif
                return response;
            }

            response.Object = recordsList;
            response.Timestamp = DateTime.UtcNow;

            return response;
        }

        public RecordsListResponse UpdateRecordsList(Guid entityId, RecordsList recordsList)
        {
            RecordsListResponse response = new RecordsListResponse
            {
                Success = true,
                Message = "The list was successfully updated!",
            };

            try
            {
                IStorageEntity storageEntity = EntityRepository.Read(entityId);

                if (storageEntity == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Entity with such Id does not exist!";
                    return response;
                }

                Entity entity = ConvertEntityFromStorage(storageEntity);

                response.Object = recordsList;
                response.Errors = ValidateView(entity, recordsList, true);

                if (response.Errors.Count > 0)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The list was not updated. Validation error occurred!";
                    return response;
                }

                RecordsList viewForDelete = entity.RecordsLists.FirstOrDefault(r => r.Id == recordsList.Id);
                if (viewForDelete.Id == recordsList.Id)
                    entity.RecordsLists.Remove(viewForDelete);

                entity.RecordsLists.Add(recordsList);

                IStorageEntity updatedEntity = ConvertEntityToStorage(entity);
                bool result = EntityRepository.Update(updatedEntity);
                if (!result)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The list was not updated! An internal error occurred!";
                    return response;
                }

            }
            catch (Exception e)
            {
                response.Success = false;
                response.Object = recordsList;
                response.Timestamp = DateTime.UtcNow;
#if DEBUG
                response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The list was not updated. An internal error occurred!";
#endif
                return response;
            }

            response.Object = recordsList;
            response.Timestamp = DateTime.UtcNow;

            return response;
        }

        public RecordsListResponse PartialUpdateRecordsList(Guid entityId, Guid id, RecordsList recordsList)
        {
            RecordsListResponse response = new RecordsListResponse
            {
                Success = true,
                Message = "The list was successfully updated!",
            };

            try
            {
                IStorageEntity storageEntity = EntityRepository.Read(entityId);

                if (storageEntity == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Entity with such Id does not exist!";
                    return response;
                }

                Entity entity = ConvertEntityFromStorage(storageEntity);

                RecordsList updatedList = entity.RecordsLists.FirstOrDefault(l => l.Id == id);

                if (updatedList == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "List with such Id does not exist!";
                    return response;
                }

                if (!string.IsNullOrWhiteSpace(recordsList.Label))
                    updatedList.Label = recordsList.Label;
                if (!recordsList.Type.HasValue)
                    updatedList.Type = recordsList.Type;
                if (recordsList.Filters != null)
                    updatedList.Filters = recordsList.Filters;
                if (recordsList.Fields != null)
                    updatedList.Fields = recordsList.Fields;

                response.Object = recordsList;
                response.Errors = ValidateView(entity, recordsList, true);

                if (response.Errors.Count > 0)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The list was not updated. Validation error occurred!";
                    return response;
                }

                IStorageEntity updatedEntity = ConvertEntityToStorage(entity);
                bool result = EntityRepository.Update(updatedEntity);
                if (!result)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The list was not updated! An internal error occurred!";
                    return response;
                }

            }
            catch (Exception e)
            {
                response.Success = false;
                response.Object = recordsList;
                response.Timestamp = DateTime.UtcNow;
#if DEBUG
                response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The list was not updated. An internal error occurred!";
#endif
                return response;
            }

            response.Object = recordsList;
            response.Timestamp = DateTime.UtcNow;

            return response;
        }

        public RecordsListResponse DeleteRecordsList(Guid entityId, Guid id)
        {
            RecordsListResponse response = new RecordsListResponse
            {
                Success = true,
                Message = "The list was successfully deleted!",
            };

            try
            {
                IStorageEntity storageEntity = EntityRepository.Read(entityId);

                if (storageEntity == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Entity with such Id does not exist!";
                    return response;
                }

                Entity entity = ConvertEntityFromStorage(storageEntity);

                RecordsList recordsList = entity.RecordsLists.FirstOrDefault(v => v.Id == id);

                if (recordsList == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The list was not deleted. Validation error occurred!";
                    response.Errors.Add(new ErrorModel("id", id.ToString(), "List with such Id does not exist!"));
                    return response;
                }

                entity.RecordsLists.Remove(recordsList);

                IStorageEntity updatedEntity = ConvertEntityToStorage(entity);
                bool result = EntityRepository.Update(updatedEntity);
                if (!result)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The list was not updated! An internal error occurred!";
                    return response;
                }
            }
            catch (Exception e)
            {
                response.Timestamp = DateTime.UtcNow;
                response.Success = false;
#if DEBUG
                response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The list was not deleted. An internal error occurred!";
#endif
                return response;
            }

            response.Timestamp = DateTime.UtcNow;
            return response;
        }

        public RecordsListCollectionResponse ReadRecordsLists(Guid entityId)
        {
            RecordsListCollectionResponse response = new RecordsListCollectionResponse
            {
                Success = true,
                Message = "The lists were successfully returned!",
            };

            try
            {
                IStorageEntity storageEntity = EntityRepository.Read(entityId);

                if (storageEntity == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Entity with such Id does not exist!";
                    return response;
                }

                Entity entity = ConvertEntityFromStorage(storageEntity);

                RecordsListCollection recordsListCollection = new RecordsListCollection();
                recordsListCollection.RecordsLists = entity.RecordsLists;

                response.Object = recordsListCollection;
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

        public RecordsListResponse ReadRecordsList(Guid entityId, Guid id)
        {
            RecordsListResponse response = new RecordsListResponse
            {
                Success = true,
                Message = "The list was successfully returned!",
            };

            try
            {
                IStorageEntity storageEntity = EntityRepository.Read(entityId);

                if (storageEntity == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Entity with such Id does not exist!";
                    return response;
                }

                Entity entity = ConvertEntityFromStorage(storageEntity);

                RecordsList recordsList = entity.RecordsLists.FirstOrDefault(v => v.Id == id);

                if (recordsList == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Validation error occurred!";
                    response.Errors.Add(new ErrorModel("id", id.ToString(), "List with such Id does not exist!"));
                    return response;
                }

                response.Object = recordsList;
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

        public RecordViewResponse CreateRecordView(Guid entityId, RecordView recordView)
        {
            RecordViewResponse response = new RecordViewResponse
            {
                Success = true,
                Message = "The record view was successfully created!",
            };

            try
            {
                IStorageEntity storageEntity = EntityRepository.Read(entityId);

                if (storageEntity == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Entity with such Id does not exist!";
                    return response;
                }

                Entity entity = ConvertEntityFromStorage(storageEntity);

                response.Object = recordView;
                response.Errors = ValidateForm(entity, recordView, false);

                if (response.Errors.Count > 0)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The record view was not created. Validation error occurred!";
                    return response;
                }

                entity.RecordViewLists.Add(recordView);

                IStorageEntity updatedEntity = ConvertEntityToStorage(entity);
                bool result = EntityRepository.Update(updatedEntity);
                if (!result)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The record view was not created! An internal error occurred!";
                    return response;
                }

            }
            catch (Exception e)
            {
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

            response.Object = recordView;
            response.Timestamp = DateTime.UtcNow;

            return response;
        }

        public RecordViewResponse UpdateRecordView(Guid entityId, RecordView recordView)
        {
            RecordViewResponse response = new RecordViewResponse
            {
                Success = true,
                Message = "The record view was successfully updated!",
            };

            try
            {
                IStorageEntity storageEntity = EntityRepository.Read(entityId);

                if (storageEntity == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Entity with such Id does not exist!";
                    return response;
                }

                Entity entity = ConvertEntityFromStorage(storageEntity);

                response.Object = recordView;
                response.Errors = ValidateForm(entity, recordView, true);

                if (response.Errors.Count > 0)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The record view was not updated. Validation error occurred!";
                    return response;
                }

                RecordView recordViewForDelete = entity.RecordViewLists.FirstOrDefault(r => r.Id == recordView.Id);
                if (recordViewForDelete.Id == recordView.Id)
                    entity.RecordViewLists.Remove(recordViewForDelete);

                entity.RecordViewLists.Add(recordView);

                IStorageEntity updatedEntity = ConvertEntityToStorage(entity);
                bool result = EntityRepository.Update(updatedEntity);
                if (!result)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The record view was not updated! An internal error occurred!";
                    return response;
                }

            }
            catch (Exception e)
            {
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

            response.Object = recordView;
            response.Timestamp = DateTime.UtcNow;

            return response;
        }

        public RecordViewResponse PartialUpdateRecordView(Guid entityId, Guid id, RecordView recordView)
        {
            RecordViewResponse response = new RecordViewResponse
            {
                Success = true,
                Message = "The record view was successfully updated!",
            };

            try
            {
                IStorageEntity storageEntity = EntityRepository.Read(entityId);

                if (storageEntity == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Entity with such Id does not exist!";
                    return response;
                }

                Entity entity = ConvertEntityFromStorage(storageEntity);

                RecordView updatedView = entity.RecordViewLists.FirstOrDefault(v => v.Id == id);

                if (updatedView == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "View with such Id does not exist!";
                    return response;
                }

                if (!string.IsNullOrWhiteSpace(recordView.Label))
                    updatedView.Label = recordView.Label;
                if (recordView.Fields != null)
                    updatedView.Fields = recordView.Fields;

                response.Object = recordView;
                response.Errors = ValidateForm(entity, recordView, true);

                if (response.Errors.Count > 0)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The record view was not updated. Validation error occurred!";
                    return response;
                }

                IStorageEntity updatedEntity = ConvertEntityToStorage(entity);
                bool result = EntityRepository.Update(updatedEntity);
                if (!result)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The record view was not updated! An internal error occurred!";
                    return response;
                }

            }
            catch (Exception e)
            {
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

            response.Object = recordView;
            response.Timestamp = DateTime.UtcNow;

            return response;
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
                IStorageEntity storageEntity = EntityRepository.Read(entityId);

                if (storageEntity == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Entity with such Id does not exist!";
                    return response;
                }

                Entity entity = ConvertEntityFromStorage(storageEntity);

                RecordView recordView = entity.RecordViewLists.FirstOrDefault(r => r.Id == id);

                if (recordView == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The record view was not deleted. Validation error occurred!";
                    response.Errors.Add(new ErrorModel("id", id.ToString(), "Record view with such Id does not exist!"));
                    return response;
                }

                entity.RecordViewLists.Remove(recordView);

                IStorageEntity updatedEntity = ConvertEntityToStorage(entity);
                bool result = EntityRepository.Update(updatedEntity);
                if (!result)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The record view was not updated! An internal error occurred!";
                    return response;
                }
            }
            catch (Exception e)
            {
                response.Timestamp = DateTime.UtcNow;
                response.Success = false;
#if DEBUG
                response.Message = e.Message + e.StackTrace;
#else
                response.Message = "The record view was not deleted. An internal error occurred!";
#endif
                return response;
            }

            response.Timestamp = DateTime.UtcNow;
            return response;
        }

        public RecordViewCollectionResponse ReadRecordViews(Guid entityId)
        {
            RecordViewCollectionResponse response = new RecordViewCollectionResponse
            {
                Success = true,
                Message = "The record views were successfully returned!",
            };

            try
            {
                IStorageEntity storageEntity = EntityRepository.Read(entityId);

                if (storageEntity == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Entity with such Id does not exist!";
                    return response;
                }

                Entity entity = ConvertEntityFromStorage(storageEntity);

                RecordViewCollection recordViewList = new RecordViewCollection();
                recordViewList.RecordViews = entity.RecordViewLists;

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
            RecordViewResponse response = new RecordViewResponse
            {
                Success = true,
                Message = "The record view was successfully returned!",
            };

            try
            {
                IStorageEntity storageEntity = EntityRepository.Read(entityId);

                if (storageEntity == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Entity with such Id does not exist!";
                    return response;
                }

                Entity entity = ConvertEntityFromStorage(storageEntity);

                RecordView recordView = entity.RecordViewLists.FirstOrDefault(r => r.Id == id);

                if (recordView == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Validation error occurred!";
                    response.Errors.Add(new ErrorModel("id", id.ToString(), "Record View with such Id does not exist!"));
                    return response;
                }

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

        #endregion

        #region << Help methods >>

        private List<Field> CreateEntityDefaultFields(Entity entity)
        {
            List<Field> fields = new List<Field>();

            GuidField primaryKeyField = new GuidField();

            primaryKeyField.Id = Guid.NewGuid();
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
            primaryKeyField.DefaultValue = Guid.Empty;
            primaryKeyField.GenerateNewId = true;

            fields.Add(primaryKeyField);

            GuidField createdBy = new GuidField();

            createdBy.Id = Guid.NewGuid();
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
            createdBy.DefaultValue = Guid.Empty;
            createdBy.GenerateNewId = false;

            fields.Add(createdBy);

            GuidField lastModifiedBy = new GuidField();

            lastModifiedBy.Id = Guid.NewGuid();
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
            lastModifiedBy.DefaultValue = Guid.Empty;
            lastModifiedBy.GenerateNewId = false;

            fields.Add(lastModifiedBy);

            DateTimeField createdOn = new DateTimeField();

            createdOn.Id = Guid.NewGuid();
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

            createdOn.Format = "MM/dd/YYYY";
            createdOn.UseCurrentTimeAsDefaultValue = true;

            fields.Add(createdOn);

            DateTimeField modifiedOn = new DateTimeField();

            modifiedOn.Id = Guid.NewGuid();
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

            modifiedOn.Format = "MM/dd/YYYY";
            modifiedOn.UseCurrentTimeAsDefaultValue = true;

            fields.Add(modifiedOn);

            return fields;
        }

        private List<RecordsList> CreateEntityDefaultRecordsLists(Entity entity)
        {
            List<RecordsList> recordsLists = new List<RecordsList>();


            return recordsLists;
        }

        private List<RecordView> CreateEntityDefaultRecordViews(Entity entity)
        {
            List<RecordView> recordViewList = new List<RecordView>();


            return recordViewList;
        }

        #endregion
    }
}