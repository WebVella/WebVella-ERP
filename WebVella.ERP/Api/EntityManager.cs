using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Storage;
using WebVella.ERP.Utilities.Dynamic;

namespace WebVella.ERP
{
    public class EntityManager
    {
        public IStorageEntityRepository EntityRepository
        {
            get; set;
        }

        public EntityManager(IStorageService storage)
        {
            EntityRepository = storage.GetEntityRepository();
        }

        #region <-- Validation methods -->

        public List<ErrorModel> ValidateEntity(Entity entity, bool checkId = false)
        {
            List<ErrorModel> errorList = new List<ErrorModel>();

            IList<IStorageEntity> entities = EntityRepository.Read();

            if (!entity.Id.HasValue || entity.Id.Value == Guid.Empty)
                errorList.Add(new ErrorModel("id", null, "Id is required!"));

            if (checkId)
            {
                //update
                if (entity.Id.HasValue && entity.Id.Value != Guid.Empty)
                {
                    IStorageEntity verifiedEntity = EntityRepository.Read(entity.Id.Value);

                    if (verifiedEntity == null)
                        errorList.Add(new ErrorModel("id", entity.Id.ToString(), "Entity with such Id does not exist!"));
                }
            }
            else
            {
                //create

            }

            if (string.IsNullOrWhiteSpace(entity.Name))
                errorList.Add(new ErrorModel("name", entity.Name, "Name is required!"));
            else
            {
                if (entity.Name.Length < 2)
                    errorList.Add(new ErrorModel("name", entity.Name, "The Name must be at least 2 characters long!"));

                if (entity.Name.Length > 50)
                    errorList.Add(new ErrorModel("name", entity.Name, "The length of Name must be less than 50 characters!"));

                string pattern = @"^[a-z](?!.*__)[a-z0-9_]*[a-z0-9]$";

                Match match = Regex.Match(entity.Name, pattern);
                if (!match.Success || match.Value != entity.Name.Trim())
                    errorList.Add(new ErrorModel("name", entity.Name, "Name can only contains underscores and lowercase alphanumeric characters. It must begin with a letter, not include spaces, not end with an underscore, and not contain two consecutive underscores.!"));

                IStorageEntity verifiedEntity = EntityRepository.Read(entity.Name);

                if (verifiedEntity != null && verifiedEntity.Id != entity.Id)
                {
                    errorList.Add(new ErrorModel("name", entity.Name, "Entity with such Name exists already!"));
                }
            }

            if (string.IsNullOrWhiteSpace(entity.Label))
                errorList.Add(new ErrorModel("label", entity.Label, "Label is required!"));
            else
            {
                //TODO check if we need this validation

                if (entity.Label.Length > 50)
                    errorList.Add(new ErrorModel("label", entity.Label, "The length of Label must be less than 50 characters!"));

                //string pattern = @"[A-Za-z][A-Za-z0-9\s_.-]*";

                //Match match = Regex.Match(entity.Label, pattern);
                //if (!match.Success || match.Value != entity.Label.Trim())
                //    errorList.Add(new ErrorModel("label", entity.Label, "Label can only contains underscores, dashes, dots, spaces and alphanumeric characters.!"));

            }

            if (string.IsNullOrWhiteSpace(entity.PluralLabel))
                errorList.Add(new ErrorModel("pluralLabel", entity.PluralLabel, "Plural Label is required!"));
            else
            {
                //TODO check if we need this validation

                if (entity.PluralLabel.Length > 50)
                    errorList.Add(new ErrorModel("pluralLabel", entity.PluralLabel, "The length of Plural Label must be less than 50 characters!"));

                //string pattern = @"[A-Za-z][A-Za-z0-9\s_.-]*";

                //Match match = Regex.Match(entity.PluralLabel, pattern);
                //if (!match.Success || match.Value != entity.PluralLabel.Trim())
                //    errorList.Add(new ErrorModel("pluralLabel", entity.PluralLabel, "Plural Label can only contains underscores, dashes, dots, spaces and alphanumeric characters.!"));

            }

            if (!entity.System.HasValue)
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

            if (!entity.Weight.HasValue)
                entity.Weight = 1;

            return errorList;
        }

        public List<ErrorModel> ValidateFields(Guid entityId, List<Field> fields, bool checkId = false)
        {
            List<ErrorModel> errorList = new List<ErrorModel>();

            IStorageEntity storageEntity = EntityRepository.Read(entityId);
            Entity entity = new Entity(storageEntity);

            if (fields.Count == 0)
            {
                errorList.Add(new ErrorModel("fields", null, "There should be at least one field!"));
                return errorList;
            }

            int primaryFieldCount = 0;

            foreach (var field in fields)
            {
                errorList.AddRange(ValidateField(entity, field, false));

                if (field is PrimaryKeyField)
                {
                    primaryFieldCount++;
                }
            }

            if (primaryFieldCount < 1)
                errorList.Add(new ErrorModel("fields.id", null, "Must have one primary field!"));

            if (primaryFieldCount > 1)
                errorList.Add(new ErrorModel("fields.id", null, "Too many primary fields. Must have only one primary field!"));

            return errorList;
        }

        public List<ErrorModel> ValidateField(Entity entity, Field field, bool checkId = false)
        {
            List<ErrorModel> errorList = new List<ErrorModel>();

            if (field.Id == Guid.Empty)
                errorList.Add(new ErrorModel("fields.id", null, "Id is required!"));

            if (checkId)
            {
                int fieldSameIdCount = entity.Fields.Where(f => f.Id == field.Id).Count();

                if (fieldSameIdCount > 1)
                    errorList.Add(new ErrorModel("fields.id", null, "There are multiple fields with same Id!"));

                int fieldSameNameCount = entity.Fields.Where(f => f.Id == field.Id).Count();

                if (fieldSameNameCount > 1)
                    errorList.Add(new ErrorModel("fields.name", null, "There are multiple fields with same Name!"));
            }

            if (string.IsNullOrWhiteSpace(field.Name))
                errorList.Add(new ErrorModel("fields.name", field.Name, "Name is required!"));
            else
            {
                if (field.Name.Length < 2)
                    errorList.Add(new ErrorModel("fields.name", field.Name, "The Name must be at least 2 characters long!"));

                if (field.Name.Length > 30)
                    errorList.Add(new ErrorModel("fields.name", field.Name, "The length of Name must be less than 30 characters!"));

                string pattern = @"^[a-z](?!.*__)[a-z0-9_]*[a-z0-9]$";

                Match match = Regex.Match(field.Name, pattern);
                if (!match.Success || match.Value != field.Name.Trim())
                    errorList.Add(new ErrorModel("fields.name", field.Name, "Name can only contains underscores and alphanumeric characters. It must begin with a letter, not include spaces, not end with an underscore, and not contain two consecutive underscores.!"));

            }

            if (string.IsNullOrWhiteSpace(field.Label))
                errorList.Add(new ErrorModel("fields.label", field.Label, "Label is required!"));
            else
            {
                //TODO check if we need this validation
                if (field.Label.Length > 30)
                    errorList.Add(new ErrorModel("fields.label", field.Label, "The length of Label must be less than 30 characters!"));
                /*
                string pattern = @"[A-Za-z][A-Za-z0-9\s_.-]*"";

                Match match = Regex.Match(field.Label, pattern);
                if (!match.Success || match.Value != field.Label.Trim())
                    errorList.Add(new ErrorModel("fields.label", field.Label, "Label can only contains underscores, dashes, dots, spaces and alphanumeric characters.!"));*/
            }

            if (field is AutoNumberField)
            {
                if (!((AutoNumberField)field).DefaultValue.HasValue)
                    errorList.Add(new ErrorModel("fields.defaultValue", null, "Default Value is required!"));

                if (!((AutoNumberField)field).StartingNumber.HasValue)
                    errorList.Add(new ErrorModel("fields.startingNumber", null, "Starting Number is required!"));

                //TODO:parse DisplayFormat field
            }
            else if (field is CheckboxField)
            {
                if (!((CheckboxField)field).DefaultValue.HasValue)
                    errorList.Add(new ErrorModel("fields.defaultValue", null, "Default Value is required!"));
            }
            else if (field is CurrencyField)
            {
                if (!((CurrencyField)field).DefaultValue.HasValue)
                    errorList.Add(new ErrorModel("fields.defaultValue", null, "Default Value is required!"));

                if (!((CurrencyField)field).MinValue.HasValue)
                    errorList.Add(new ErrorModel("fields.minValue", null, "Min Value is required!"));

                if (!((CurrencyField)field).MaxValue.HasValue)
                    errorList.Add(new ErrorModel("fields.maxValue", null, "Max Value is required!"));

                if (((CurrencyField)field).MinValue.HasValue && ((CurrencyField)field).MaxValue.HasValue)
                {
                    if (((CurrencyField)field).MinValue.Value >= ((CurrencyField)field).MaxValue.Value)
                        errorList.Add(new ErrorModel("fields.MinValue", null, "Min Value must be less than Max Value!"));
                }
            }
            else if (field is DateField)
            {
                //because DateField support NULL now, we skipp this validation
                //if (!((DateField)field).DefaultValue.HasValue)
                //    errorList.Add(new ErrorModel("fields.defaultValue", null, "Default Value is required!"));

                //TODO:parse format and check if it is valid
            }
            else if (field is DateTimeField)
            {
                //because DateField support NULL now, we skipp this validation
                //if (!((DateTimeField)field).DefaultValue.HasValue)
                //    errorList.Add(new ErrorModel("fields.defaultValue", null, "Default Value is required!"));

                //TODO:parse format and check if it is valid
            }
            else if (field is EmailField)
            {
                if (((EmailField)field).DefaultValue == null)
                    errorList.Add(new ErrorModel("fields.defaultValue", null, "Default Value is required!"));

                if (!((EmailField)field).MaxLength.HasValue)
                    errorList.Add(new ErrorModel("fields.maxLength", null, "Max Length is required!"));
            }
            else if (field is FileField)
            {
                if (((FileField)field).DefaultValue == null)
                    errorList.Add(new ErrorModel("fields.defaultValue", null, "Default Value is required!"));
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
            else if (field is HtmlField)
            {
                if ((((HtmlField)field).DefaultValue) == null)
                    errorList.Add(new ErrorModel("fields.defaultValue", null, "Default Value is required!"));
            }
            else if (field is ImageField)
            {
                if (((ImageField)field).DefaultValue == null)
                    errorList.Add(new ErrorModel("fields.defaultValue", null, "Default Value is required!"));
            }
            else if (field is LookupRelationField)
            {
                if (!((LookupRelationField)field).RelatedEntityId.HasValue && ((LookupRelationField)field).RelatedEntityId.Value == Guid.Empty)
                    errorList.Add(new ErrorModel("fields.relatedEntityId", null, "Related Entity Id is required!"));
            }
            else if (field is MultiLineTextField)
            {
                if (((MultiLineTextField)field).DefaultValue == null)
                    errorList.Add(new ErrorModel("fields.defaultValue", null, "Default Value is required!"));

                if (!((MultiLineTextField)field).MaxLength.HasValue)
                    errorList.Add(new ErrorModel("fields.maxLength", null, "Max Length is required!"));

                if (!((MultiLineTextField)field).VisibleLineNumber.HasValue)
                    errorList.Add(new ErrorModel("fields.visibleLineNumber", null, "Visible Line Number is required!"));

                if (((MultiLineTextField)field).VisibleLineNumber.HasValue && ((MultiLineTextField)field).VisibleLineNumber.Value > 20)
                    errorList.Add(new ErrorModel("fields.visibleLineNumber", null, "Visible Line Number cannot be greater than 20!"));
            }
            else if (field is MultiSelectField)
            {
                if (((MultiSelectField)field).Options != null)
                {
                    if (((MultiSelectField)field).Options.Count == 0)
                        errorList.Add(new ErrorModel("fields.options", null, "Options must contains at least one item!"));
                }
                else
                    errorList.Add(new ErrorModel("fields.options", null, "Options is required!"));
            }
            else if (field is NumberField)
            {
                if (!((NumberField)field).DefaultValue.HasValue)
                    errorList.Add(new ErrorModel("fields.defaultValue", null, "Default Value is required!"));

                if (!((NumberField)field).MinValue.HasValue)
                    errorList.Add(new ErrorModel("fields.minValue", null, "Min Value is required!"));

                if (!((NumberField)field).MaxValue.HasValue)
                    errorList.Add(new ErrorModel("fields.maxValue", null, "Max Value is required!"));

                if (((NumberField)field).MinValue.HasValue && ((NumberField)field).MaxValue.HasValue)
                {
                    if (((NumberField)field).MinValue.Value >= ((NumberField)field).MaxValue.Value)
                        errorList.Add(new ErrorModel("fields.MinValue", null, "Min Value must be less than Max Value!"));
                }

                if (!((NumberField)field).DecimalPlaces.HasValue)
                    errorList.Add(new ErrorModel("fields.decimalPlaces", null, "Decimal Places is required!"));
            }
            else if (field is PasswordField)
            {
                if (!((PasswordField)field).MaxLength.HasValue)
                    errorList.Add(new ErrorModel("fields.maxLength", null, "Max Length is required!"));

                if (!((PasswordField)field).MaskCharacter.HasValue)
                    errorList.Add(new ErrorModel("fields.maskCharacter", null, "Mask Character is required!"));
            }
            else if (field is PercentField)
            {
                if (!((PercentField)field).DefaultValue.HasValue)
                    errorList.Add(new ErrorModel("fields.defaultValue", null, "Default Value is required!"));

                if (!((PercentField)field).MinValue.HasValue)
                    errorList.Add(new ErrorModel("fields.minValue", null, "Min Value is required!"));

                if (!((PercentField)field).MaxValue.HasValue)
                    errorList.Add(new ErrorModel("fields.maxValue", null, "Max Value is required!"));

                if (((PercentField)field).MinValue.HasValue && ((PercentField)field).MaxValue.HasValue)
                {
                    if (((PercentField)field).MinValue.Value >= ((PercentField)field).MaxValue.Value)
                        errorList.Add(new ErrorModel("fields.MinValue", null, "Min Value must be less than Max Value!"));
                }

                if (!((PercentField)field).DecimalPlaces.HasValue)
                    errorList.Add(new ErrorModel("fields.decimalPlaces", null, "Decimal Places is required!"));
            }
            else if (field is PhoneField)
            {
                if (((PhoneField)field).DefaultValue == null)
                    errorList.Add(new ErrorModel("fields.defaultValue", null, "Default Value is required!"));

                if (!((PhoneField)field).MaxLength.HasValue)
                    errorList.Add(new ErrorModel("fields.maxLength", null, "Max Length is required!"));

                //TODO: parse formula text and check if it is valid
            }
            else if (field is SelectField)
            {
                if (string.IsNullOrWhiteSpace(((SelectField)field).DefaultValue))
                    errorList.Add(new ErrorModel("fields.defaultValue", null, "Default Value is required!"));

                if (((SelectField)field).Options != null)
                {
                    if (((SelectField)field).Options.Count == 0)
                        errorList.Add(new ErrorModel("fields.options", null, "Options must contains at least one item!"));
                }
                else
                    errorList.Add(new ErrorModel("fields.options", null, "Options is required!"));
            }
            else if (field is TextField)
            {
                if (((TextField)field).DefaultValue == null)
                    errorList.Add(new ErrorModel("fields.defaultValue", null, "Default Value is required!"));

                if (!((TextField)field).MaxLength.HasValue)
                    errorList.Add(new ErrorModel("fields.maxLength", null, "Max Length is required!"));
            }
            else if (field is UrlField)
            {
                if (((UrlField)field).DefaultValue == null)
                    errorList.Add(new ErrorModel("fields.defaultValue", null, "Default Value is required!"));

                if (!((UrlField)field).MaxLength.HasValue)
                    errorList.Add(new ErrorModel("fields.maxLength", null, "Max Length is required!"));

                if (!((UrlField)field).OpenTargetInNewWindow.HasValue)
                    errorList.Add(new ErrorModel("fields.openTargetInNewWindow", null, "Open Target In New Window is required!"));
            }

            return errorList;
        }

        public List<ErrorModel> ValidateViews(Guid entityId, List<RecordsList> recordsLists, bool checkId = false)
        {
            List<ErrorModel> errorList = new List<ErrorModel>();

            IStorageEntity storageEntity = EntityRepository.Read(entityId);
            Entity entity = new Entity(storageEntity);

            foreach (var recordList in recordsLists)
            {
                errorList.AddRange(ValidateView(entity, recordList, checkId));
            }

            return errorList;
        }

        public List<ErrorModel> ValidateView(Entity entity, RecordsList recordslist, bool checkId = false)
        {
            List<ErrorModel> errorList = new List<ErrorModel>();

            if (!recordslist.Id.HasValue || recordslist.Id.Value == Guid.Empty)
                errorList.Add(new ErrorModel("recordsLists.id", null, "Id is required!"));

            if (string.IsNullOrWhiteSpace(recordslist.Name))
                errorList.Add(new ErrorModel("recordsLists.name", recordslist.Name, "Name is required!"));
            else
            {
                if (recordslist.Name.Length < 2)
                    errorList.Add(new ErrorModel("recordsLists.name", recordslist.Name, "The Name must be at least 2 characters long!"));

                if (recordslist.Name.Length > 30)
                    errorList.Add(new ErrorModel("recordsLists.name", recordslist.Name, "The length of Name must be less than 30 characters!"));

                string pattern = @"^[a-z](?!.*__)[a-z0-9_]*[a-z0-9]$";

                Match match = Regex.Match(recordslist.Name, pattern);
                if (!match.Success || match.Value != recordslist.Name.Trim())
                    errorList.Add(new ErrorModel("recordsLists.name", recordslist.Name, "Name can only contains underscores and alphanumeric characters. It must begin with a letter, not include spaces, not end with an underscore, and not contain two consecutive underscores.!"));
            }

            if (string.IsNullOrWhiteSpace(recordslist.Label))
                errorList.Add(new ErrorModel("recordsLists.label", recordslist.Label, "Label is required!"));
            else
            {
                //TODO check if we need this validation

                if (recordslist.Label.Length > 50)
                    errorList.Add(new ErrorModel("recordsLists.label", recordslist.Label, "The length of Label must be less than 50 characters!"));
                /*
                string pattern = @"[A-Za-z][A-Za-z0-9\s_.-]*";

                Match match = Regex.Match(view.Label, pattern);
                if (!match.Success || match.Value != view.Label.Trim())
                    errorList.Add(new ErrorModel("views.label", view.Label, "Label can only contains underscores, dashes, dots, spaces and alphanumeric characters.!"));
                */
            }

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
                            Entity currentEntity = verifiedEntity != null ? new Entity(verifiedEntity) : entity;

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
                            Entity currentEntity = verifiedEntity != null ? new Entity(verifiedEntity) : entity;

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

        public List<ErrorModel> ValidateForms(Guid entityId, List<RecordView> recordViewList, bool checkId = false)
        {
            List<ErrorModel> errorList = new List<ErrorModel>();

            IStorageEntity storageEntity = EntityRepository.Read(entityId);
            Entity entity = new Entity(storageEntity);

            foreach (var recordView in recordViewList)
            {
                errorList.AddRange(ValidateForm(entity, recordView, checkId));
            }

            return errorList;
        }

        public List<ErrorModel> ValidateForm(Entity entity, RecordView recordView, bool checkId = false)
        {
            List<ErrorModel> errorList = new List<ErrorModel>();

            if (!recordView.Id.HasValue || recordView.Id.Value == Guid.Empty)
                errorList.Add(new ErrorModel("recordViewLists.id", null, "Id is required!"));

            if (string.IsNullOrWhiteSpace(recordView.Name))
                errorList.Add(new ErrorModel("recordViewLists.name", recordView.Name, "Name is required!"));
            else
            {
                if (recordView.Name.Length < 2)
                    errorList.Add(new ErrorModel("recordViewLists.name", recordView.Name, "The Name must be at least 2 characters long!"));

                if (recordView.Name.Length > 30)
                    errorList.Add(new ErrorModel("recordViewLists.name", recordView.Name, "The length of Name must be less than 30 characters!"));

                string pattern = @"^[a-z](?!.*__)[a-z0-9_]*[a-z0-9]$";

                Match match = Regex.Match(recordView.Name, pattern);
                if (!match.Success || match.Value != recordView.Name.Trim())
                    errorList.Add(new ErrorModel("recordViewLists.name", recordView.Name, "Name can only contains underscores and alphanumeric characters. It must begin with a letter, not include spaces, not end with an underscore, and not contain two consecutive underscores.!"));
            }

            if (string.IsNullOrWhiteSpace(recordView.Label))
                errorList.Add(new ErrorModel("recordViewLists.label", recordView.Label, "Label is required!"));
            else
            {
                //TODO check if we need this validation

                if (recordView.Label.Length > 50)
                    errorList.Add(new ErrorModel("recordViewLists.label", recordView.Label, "The length of Label must be less than 50 characters!"));
                /*
                string pattern = @"[A-Za-z][A-Za-z0-9\s_.-]*";

                Match match = Regex.Match(form.Label, pattern);
                if (!match.Success || match.Value != form.Label.Trim())
                    errorList.Add(new ErrorModel("recordViewLists.label", form.Label, "Label can only contains underscores, dashes, dots, spaces and alphanumeric characters.!"));
               */
            }

            foreach (var field in recordView.Fields)
            {
                if (!field.Id.HasValue && field.Id.Value == Guid.Empty)
                    errorList.Add(new ErrorModel("recordViewLists.fields.id", null, "Id is required!"));

                if (field.EntityId.HasValue && field.EntityId.Value != Guid.Empty)
                {
                    IStorageEntity verifiedEntity = EntityRepository.Read(field.EntityId.Value);

                    if (verifiedEntity != null || field.EntityId == entity.Id)
                    {
                        Entity currentEntity = verifiedEntity != null ? new Entity(verifiedEntity) : entity;

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

        #region <-- Entity methods -->

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

                IStorageEntity storageEntity = EntityRepository.Convert(entity);
                bool result = EntityRepository.Create(storageEntity);
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

            IStorageEntity createdEntity = EntityRepository.Read(entity.Id.Value);
            response.Object = new Entity(createdEntity);
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

                IStorageEntity storageEntity = EntityRepository.Read(entity.Id.Value);

                storageEntity.Label = entity.Label;
                storageEntity.PluralLabel = entity.PluralLabel;
                storageEntity.System = entity.System.Value;
                storageEntity.IconName = entity.IconName;
                storageEntity.Weight = entity.Weight.Value;
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

            IStorageEntity updatedEntity = EntityRepository.Read(entity.Id.Value);
            response.Object = new Entity(updatedEntity);
            response.Timestamp = DateTime.UtcNow;

            return response;
        }

        public EntityResponse PartialUpdateEntity(Guid id, Expando inputEntity)
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
                entity = new Entity(storageEntity);

                foreach (var property in inputEntity.GetProperties())
                {
                    switch (property.Key)
                    {
                        case "label":
                            {
                                entity.Label = (string)property.Value;
                            }
                            break;
                        case "pluralLabel":
                            {
                                entity.PluralLabel = (string)property.Value;
                            }
                            break;
                        case "system":
                            {
                                entity.System = (bool)property.Value;
                            }
                            break;
                        case "iconName":
                            {
                                entity.IconName = (string)property.Value;
                            }
                            break;
                        case "weight":
                            {
                                entity.Weight = (decimal)property.Value;
                            }
                            break;
                        case "recordPermissions":
                            {
                                entity.RecordPermissions = (RecordPermissions)property.Value;
                            }
                            break;
                    }
                }

                response.Object = entity;
                response.Errors = ValidateEntity(entity, true);

                if (response.Errors.Count > 0)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The entity was not updated. Validation error occurred!";
                    return response;
                }

                storageEntity = EntityRepository.Convert(entity);

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

            IStorageEntity updatedEntity = EntityRepository.Read(entity.Id.Value);
            response.Object = new Entity(updatedEntity);
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

                EntityList entityList = new EntityList(storageEntityList);
                entityList.Offset = entityList.Entities != null && entityList.Entities.Count > 0 ? entityList.Entities.Last().Id.Value : Guid.Empty;

                response.Object = new EntityList(storageEntityList);
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
            };

            Entity entity = new Entity();

            try
            {
                IStorageEntity storageEntity = EntityRepository.Read(id);
                entity = new Entity(storageEntity);

                if (entity == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Validation error occurred!";
                    response.Errors.Add(new ErrorModel("id", id.ToString(), "Entity with such Id does not exist!"));
                    return response;
                }

                response.Object = entity;
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
            };

            Entity entity = new Entity();

            try
            {
                IStorageEntity storageEntity = EntityRepository.Read(name);
                entity = new Entity(storageEntity);

                if (entity == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Validation error occurred!";
                    response.Errors.Add(new ErrorModel("name", name, "Entity with such name does not exist!"));
                    return response;
                }

                response.Object = entity;
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

        #region <-- Field methods -->

        public FieldResponse CreateField(Guid entityId, Field field)
        {
            FieldResponse response = new FieldResponse
            {
                Success = true,
                Message = "The field was successfully created!",
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

                Entity entity = new Entity(storageEntity);

                response.Object = field;
                response.Errors = ValidateField(entity, field, false);

                if (response.Errors.Count > 0)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The field was not created. Validation error occurred!";
                    return response;
                }

                entity.Fields.Add(field);

                IStorageEntity editedEntity = EntityRepository.Convert(entity);
                bool result = EntityRepository.Update(editedEntity);
                if (!result)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The field was not created! An internal error occurred!";
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
                response.Message = "The field was not created. An internal error occurred!";
#endif
                return response;
            }

            response.Object = field;
            response.Timestamp = DateTime.UtcNow;

            return response;
        }

        public FieldResponse UpdateField(Guid entityId, Field field)
        {
            FieldResponse response = new FieldResponse
            {
                Success = true,
                Message = "The field was successfully updated!",
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

                Entity entity = new Entity(storageEntity);

                response.Object = field;
                response.Errors = ValidateField(entity, field, true);

                if (response.Errors.Count > 0)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The field was not updated. Validation error occurred!";
                    return response;
                }

                Field fieldForDelete = entity.Fields.FirstOrDefault(f => f.Id == field.Id);
                if (fieldForDelete.Id == field.Id)
                    entity.Fields.Remove(fieldForDelete);

                entity.Fields.Add(field);

                IStorageEntity updatedEntity = EntityRepository.Convert(entity);
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

                Entity entity = new Entity(storageEntity);

                updatedField = entity.Fields.FirstOrDefault(f => f.Id == id);

                if (updatedField == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Field with such Id does not exist!";
                    return response;
                }

                Field field = null;

                if (updatedField is AutoNumberField)
                {
                    field = new AutoNumberField(inputField);
                    if (((AutoNumberField)field).DefaultValue != null)
                        ((AutoNumberField)updatedField).DefaultValue = ((AutoNumberField)field).DefaultValue;
                    if (((AutoNumberField)field).DisplayFormat != null)
                        ((AutoNumberField)updatedField).DisplayFormat = ((AutoNumberField)field).DisplayFormat;
                    if (((AutoNumberField)field).StartingNumber != null)
                        ((AutoNumberField)updatedField).StartingNumber = ((AutoNumberField)field).StartingNumber;
                }
                else if (updatedField is CheckboxField)
                {
                    field = new CheckboxField(inputField);
                    if (((CheckboxField)field).DefaultValue != null)
                        ((CheckboxField)updatedField).DefaultValue = ((CheckboxField)field).DefaultValue;
                }
                else if (field is CurrencyField)
                {
                    field = new CurrencyField(inputField);
                    if (((CurrencyField)field).DefaultValue != null)
                        ((CurrencyField)updatedField).DefaultValue = ((CurrencyField)field).DefaultValue;
                    if (((CurrencyField)field).MinValue != null)
                        ((CurrencyField)updatedField).MinValue = ((CurrencyField)field).MinValue;
                    if (((CurrencyField)field).MaxValue != null)
                        ((CurrencyField)updatedField).MaxValue = ((CurrencyField)field).MaxValue;
                    if (((CurrencyField)field).Currency != null)
                        ((CurrencyField)updatedField).Currency = ((CurrencyField)field).Currency;
                }
                else if (field is DateField)
                {
                    field = new DateField(inputField);
                    if (((DateField)field).DefaultValue != null)
                        ((DateField)updatedField).DefaultValue = ((DateField)field).DefaultValue;
                    if (((DateField)field).Format != null)
                        ((DateField)updatedField).Format = ((DateField)field).Format;
                    if (((DateField)field).UseCurrentTimeAsDefaultValue != null)
                        ((DateField)updatedField).UseCurrentTimeAsDefaultValue = ((DateField)field).UseCurrentTimeAsDefaultValue;
                }
                else if (field is DateTimeField)
                {
                    field = new DateTimeField(inputField);
                    if (((DateTimeField)field).DefaultValue != null)
                        ((DateTimeField)updatedField).DefaultValue = ((DateTimeField)field).DefaultValue;
                    if (((DateTimeField)field).Format != null)
                        ((DateTimeField)updatedField).Format = ((DateTimeField)field).Format;
                    if (((DateTimeField)field).UseCurrentTimeAsDefaultValue != null)
                        ((DateTimeField)updatedField).UseCurrentTimeAsDefaultValue = ((DateTimeField)field).UseCurrentTimeAsDefaultValue;
                }
                else if (field is EmailField)
                {
                    field = new EmailField(inputField);
                    if (((EmailField)field).DefaultValue != null)
                        ((EmailField)updatedField).DefaultValue = ((EmailField)field).DefaultValue;
                    if (((EmailField)field).MaxLength != null)
                        ((EmailField)updatedField).MaxLength = ((EmailField)field).MaxLength;
                }
                else if (field is FileField)
                {
                    field = new FileField(inputField);
                    if (((FileField)field).DefaultValue != null)
                        ((FileField)updatedField).DefaultValue = ((FileField)field).DefaultValue;
                }
                else if (field is HtmlField)
                {
                    field = new HtmlField(inputField);
                    if (((HtmlField)field).DefaultValue != null)
                        ((HtmlField)updatedField).DefaultValue = ((HtmlField)field).DefaultValue;
                }
                else if (field is ImageField)
                {
                    field = new ImageField(inputField);
                    if (((ImageField)field).DefaultValue != null)
                        ((ImageField)updatedField).DefaultValue = ((ImageField)field).DefaultValue;
                }
                else if (field is LookupRelationField)
                {
                    field = new LookupRelationField(inputField);
                    if (((LookupRelationField)field).RelatedEntityId != null)
                        ((LookupRelationField)updatedField).RelatedEntityId = ((LookupRelationField)field).RelatedEntityId;
                }
                else if (field is MultiLineTextField)
                {
                    field = new MultiLineTextField(inputField);
                    if (((MultiLineTextField)field).DefaultValue != null)
                        ((MultiLineTextField)updatedField).DefaultValue = ((MultiLineTextField)field).DefaultValue;
                    if (((MultiLineTextField)field).MaxLength != null)
                        ((MultiLineTextField)updatedField).MaxLength = ((MultiLineTextField)field).MaxLength;
                    if (((MultiLineTextField)field).VisibleLineNumber != null)
                        ((MultiLineTextField)updatedField).VisibleLineNumber = ((MultiLineTextField)field).VisibleLineNumber;
                }
                else if (field is MultiSelectField)
                {
                    field = new MultiSelectField(inputField);
                    if (((MultiSelectField)field).DefaultValue != null)
                        ((MultiSelectField)updatedField).DefaultValue = ((MultiSelectField)field).DefaultValue;
                    if (((MultiSelectField)field).Options != null)
                        ((MultiSelectField)updatedField).Options = ((MultiSelectField)field).Options;
                }
                else if (field is NumberField)
                {
                    field = new NumberField(inputField);
                    if (((NumberField)field).DefaultValue != null)
                        ((NumberField)updatedField).DefaultValue = ((NumberField)field).DefaultValue;
                    if (((NumberField)field).MinValue != null)
                        ((NumberField)updatedField).MinValue = ((NumberField)field).MinValue;
                    if (((NumberField)field).MaxValue != null)
                        ((NumberField)updatedField).MaxValue = ((NumberField)field).MaxValue;
                    if (((NumberField)field).DecimalPlaces != null)
                        ((NumberField)updatedField).DecimalPlaces = ((NumberField)field).DecimalPlaces;
                }
                else if (field is PasswordField)
                {
                    field = new PasswordField(inputField);
                    if (((PasswordField)field).MaxLength != null)
                        ((PasswordField)updatedField).MaxLength = ((PasswordField)field).MaxLength;
                    if (((PasswordField)field).Encrypted != null)
                        ((PasswordField)updatedField).Encrypted = ((PasswordField)field).Encrypted;
                    //if (((PasswordField)field).MaskType != null)
                    //    ((PasswordField)updatedField).MaskType = ((PasswordField)field).MaskType;
                    if (((PasswordField)field).MaskCharacter != null)
                        ((PasswordField)updatedField).MaskCharacter = ((PasswordField)field).MaskCharacter;
                }
                else if (field is PercentField)
                {
                    field = new PercentField(inputField);
                    if (((PercentField)field).DefaultValue != null)
                        ((PercentField)updatedField).DefaultValue = ((PercentField)field).DefaultValue;
                    if (((PercentField)field).MinValue != null)
                        ((PercentField)updatedField).MinValue = ((PercentField)field).MinValue;
                    if (((PercentField)field).MaxValue != null)
                        ((PercentField)updatedField).MaxValue = ((PercentField)field).MaxValue;
                    if (((PercentField)field).DecimalPlaces != null)
                        ((PercentField)updatedField).DecimalPlaces = ((PercentField)field).DecimalPlaces;
                }
                else if (field is PhoneField)
                {
                    field = new PhoneField(inputField);
                    if (((PhoneField)field).DefaultValue != null)
                        ((PhoneField)updatedField).DefaultValue = ((PhoneField)field).DefaultValue;
                    if (((PhoneField)field).Format != null)
                        ((PhoneField)updatedField).Format = ((PhoneField)field).Format;
                    if (((PhoneField)field).MaxLength != null)
                        ((PhoneField)updatedField).MaxLength = ((PhoneField)field).MaxLength;
                }
                else if (field is PrimaryKeyField)
                {
                    field = new PrimaryKeyField(inputField);
                    if (((PrimaryKeyField)field).DefaultValue != null)
                        ((PrimaryKeyField)updatedField).DefaultValue = ((PrimaryKeyField)field).DefaultValue;
                }
                else if (field is SelectField)
                {
                    field = new SelectField(inputField);
                    if (((SelectField)field).DefaultValue != null)
                        ((SelectField)updatedField).DefaultValue = ((SelectField)field).DefaultValue;
                    if (((SelectField)field).Options != null)
                        ((SelectField)updatedField).Options = ((SelectField)field).Options;
                }
                else if (field is TextField)
                {
                    field = new TextField(inputField);
                    if (((TextField)field).DefaultValue != null)
                        ((TextField)updatedField).DefaultValue = ((TextField)field).DefaultValue;
                    if (((TextField)field).MaxLength != null)
                        ((TextField)updatedField).MaxLength = ((TextField)field).MaxLength;
                }
                else if (field is UrlField)
                {
                    field = new UrlField(inputField);
                    if (((UrlField)field).DefaultValue != null)
                        ((UrlField)updatedField).DefaultValue = ((UrlField)field).DefaultValue;
                    if (((UrlField)field).MaxLength != null)
                        ((UrlField)updatedField).MaxLength = ((UrlField)field).MaxLength;
                    if (((UrlField)field).OpenTargetInNewWindow != null)
                        ((UrlField)updatedField).OpenTargetInNewWindow = ((UrlField)field).OpenTargetInNewWindow;
                }

                if (field.Label != null)
                    updatedField.Label = field.Label;
                else if (field.PlaceholderText != null)
                    updatedField.PlaceholderText = field.PlaceholderText;
                else if (field.Description != null)
                    updatedField.Description = field.Description;
                else if (field.HelpText != null)
                    updatedField.HelpText = field.HelpText;
                else if (field.Required != null)
                    updatedField.Required = field.Required;
                else if (field.Unique != null)
                    updatedField.Unique = field.Unique;
                else if (field.Searchable != null)
                    updatedField.Searchable = field.Searchable;
                else if (field.Auditable != null)
                    updatedField.Auditable = field.Auditable;
                else if (field.System != null)
                    updatedField.System = field.System;

                //Field fieldForDelete = entity.Fields.FirstOrDefault(f => f.Id == updatedField.Id);
                //if (fieldForDelete.Id == updatedField.Id)
                //    entity.Fields.Remove(fieldForDelete);

                //entity.Fields.Add(updatedField);


                response.Object = updatedField;
                response.Errors = ValidateField(entity, updatedField, true);

                if (response.Errors.Count > 0)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The field was not updated. Validation error occurred!";
                    return response;
                }

                IStorageEntity updatedEntity = EntityRepository.Convert(entity);
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

                Entity entity = new Entity(storageEntity);

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

                IStorageEntity updatedEntity = EntityRepository.Convert(entity);
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

                FieldList fieldList = new FieldList(storageEntity.Fields);
                fieldList.Offset = fieldList.Fields != null && fieldList.Fields.Count > 0 ? fieldList.Fields.Last().Id.Value : Guid.Empty;

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

                Field field = Field.Convert(storageField);
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

        #region <-- RecordsList methods -->

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

                Entity entity = new Entity(storageEntity);

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

                IStorageEntity updatedEntity = EntityRepository.Convert(entity);
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

                Entity entity = new Entity(storageEntity);

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

                IStorageEntity updatedEntity = EntityRepository.Convert(entity);
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

                Entity entity = new Entity(storageEntity);

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

                IStorageEntity updatedEntity = EntityRepository.Convert(entity);
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

                Entity entity = new Entity(storageEntity);

                RecordsListCollection recordsListCollection = new RecordsListCollection();
                recordsListCollection.Views = entity.RecordsLists;
                recordsListCollection.Offset = recordsListCollection.Views != null && recordsListCollection.Views.Count > 0 ? recordsListCollection.Views.Last().Id.Value : Guid.Empty;

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

                Entity entity = new Entity(storageEntity);

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

        #region <-- RecordView methods -->

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

                Entity entity = new Entity(storageEntity);

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

                IStorageEntity updatedEntity = EntityRepository.Convert(entity);
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

                Entity entity = new Entity(storageEntity);

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

                IStorageEntity updatedEntity = EntityRepository.Convert(entity);
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

                Entity entity = new Entity(storageEntity);

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

                IStorageEntity updatedEntity = EntityRepository.Convert(entity);
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

                Entity entity = new Entity(storageEntity);

                RecordViewCollection recordViewList = new RecordViewCollection();
                recordViewList.Forms = entity.RecordViewLists;
                recordViewList.Offset = recordViewList.Forms != null && recordViewList.Forms.Count > 0 ? recordViewList.Forms.Last().Id.Value : Guid.Empty;

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

                Entity entity = new Entity(storageEntity);

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

        #region <-- Help methods -->

        private List<Field> CreateEntityDefaultFields(Entity entity)
        {
            List<Field> fields = new List<Field>();

            PrimaryKeyField primaryKeyField = new PrimaryKeyField();

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

            fields.Add(primaryKeyField);

            LookupRelationField createdBy = new LookupRelationField();

            createdBy.Id = Guid.NewGuid();
            createdBy.Name = "created_by";
            createdBy.Label = "Created By";
            createdBy.PlaceholderText = "";
            createdBy.Description = "";
            createdBy.HelpText = "";
            createdBy.Required = true;
            createdBy.Unique = false;
            createdBy.Searchable = false;
            createdBy.Auditable = false;
            createdBy.System = true;

            createdBy.RelatedEntityId = Guid.Empty; //User entity id must be set here

            fields.Add(createdBy);

            LookupRelationField lastModifiedBy = new LookupRelationField();

            lastModifiedBy.Id = Guid.NewGuid();
            lastModifiedBy.Name = "last_modified_by";
            lastModifiedBy.Label = "Last Modified By";
            lastModifiedBy.PlaceholderText = "";
            lastModifiedBy.Description = "";
            lastModifiedBy.HelpText = "";
            lastModifiedBy.Required = true;
            lastModifiedBy.Unique = false;
            lastModifiedBy.Searchable = false;
            lastModifiedBy.Auditable = false;
            lastModifiedBy.System = true;

            lastModifiedBy.RelatedEntityId = Guid.Empty; //User entity id must be set here

            fields.Add(lastModifiedBy);

            DateTimeField createdOn = new DateTimeField();

            createdOn.Id = Guid.NewGuid();
            createdOn.Name = "created_on";
            createdOn.Label = "Created On";
            createdOn.PlaceholderText = "";
            createdOn.Description = "";
            createdOn.HelpText = "";
            createdOn.Required = true;
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
            modifiedOn.Name = "modified_on";
            modifiedOn.Label = "Modified On";
            modifiedOn.PlaceholderText = "";
            modifiedOn.Description = "";
            modifiedOn.HelpText = "";
            modifiedOn.Required = true;
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