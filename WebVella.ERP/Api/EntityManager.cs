using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Storage;

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

        public List<ErrorModel> ValidateEntity(InputEntity entity, bool checkId = false)
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
                if (entity.Name.Length <= 3)
                    errorList.Add(new ErrorModel("name", entity.Name, "The length of Name must be greater than 3 characters!"));

                if (entity.Name.Length > 50)
                    errorList.Add(new ErrorModel("name", entity.Name, "The length of Name must be less than 50 characters!"));

                string pattern = @"[a-zA-Z][a-zA-Z0-9_]*";

                Match match = Regex.Match(entity.Name, pattern);
                if (!match.Success || match.Value != entity.Name.Trim())
                    errorList.Add(new ErrorModel("name", entity.Name, "Name can only contains underscores and alphanumeric characters. It must begin with a letter, not include spaces, not end with an underscore, and not contain two consecutive underscores.!"));

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

                string pattern = @"[A-Za-z][A-Za-z0-9\s_.-]*";

                Match match = Regex.Match(entity.Label, pattern);
                if (!match.Success || match.Value != entity.Label.Trim())
                    errorList.Add(new ErrorModel("label", entity.Label, "Label can only contains underscores, dashes, dots, spaces and alphanumeric characters.!"));

            }

            if (string.IsNullOrWhiteSpace(entity.PluralLabel))
                errorList.Add(new ErrorModel("pluralLabel", entity.PluralLabel, "Plural Label is required!"));
            else
            {
                //TODO check if we need this validation

                if (entity.PluralLabel.Length > 50)
                    errorList.Add(new ErrorModel("pluralLabel", entity.PluralLabel, "The length of Plural Label must be less than 50 characters!"));

                string pattern = @"[A-Za-z][A-Za-z0-9\s_.-]*";

                Match match = Regex.Match(entity.PluralLabel, pattern);
                if (!match.Success || match.Value != entity.PluralLabel.Trim())
                    errorList.Add(new ErrorModel("pluralLabel", entity.PluralLabel, "Plural Label can only contains underscores, dashes, dots, spaces and alphanumeric characters.!"));

            }

            if (!entity.System.HasValue)
                errorList.Add(new ErrorModel("system", null, "System is required!"));

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
                if (field.Name.Length > 30)
                    errorList.Add(new ErrorModel("fields.name", field.Name, "The length of Name must be less than 30 characters!"));

                string pattern = @"[a-zA-Z][a-zA-Z0-9_]*";

                Match match = Regex.Match(field.Name, pattern);
                if (!match.Success || match.Value != field.Name.Trim())
                    errorList.Add(new ErrorModel("fields.name", field.Name, "Name can only contains underscores and alphanumeric characters. It must begin with a letter, not include spaces, not end with an underscore, and not contain two consecutive underscores.!"));

            }

            if (string.IsNullOrWhiteSpace(field.Label))
                errorList.Add(new ErrorModel("fields.label", field.Label, "Label is required!"));
            else
            {
                //TODO check if we need this validation
                /*if (field.Label.Length > 30)
                    errorList.Add(new ErrorModel("fields.label", field.Label, "The length of Label must be less than 30 characters!"));

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
                if (!((DateField)field).DefaultValue.HasValue)
                    errorList.Add(new ErrorModel("fields.defaultValue", null, "Default Value is required!"));

                //TODO:parse format and check if it is valid
            }
            else if (field is DateTimeField)
            {
                if (!((DateTimeField)field).DefaultValue.HasValue)
                    errorList.Add(new ErrorModel("fields.defaultValue", null, "Default Value is required!"));

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
            else if (field is MasterDetailsRelationshipField)
            {
                if (!((MasterDetailsRelationshipField)field).RelatedEntityId.HasValue && ((MasterDetailsRelationshipField)field).RelatedEntityId.Value == Guid.Empty)
                    errorList.Add(new ErrorModel("fields.masterDetailsRelationshipField", null, "Master Details Relationship Field is required!"));
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

        public List<ErrorModel> ValidateViews(Guid entityId, List<View> views, bool checkId = false)
        {
            List<ErrorModel> errorList = new List<ErrorModel>();

            IStorageEntity storageEntity = EntityRepository.Read(entityId);
            Entity entity = new Entity(storageEntity);

            foreach (var view in views)
            {
                errorList.AddRange(ValidateView(entity, view, checkId));
            }

            return errorList;
        }

        public List<ErrorModel> ValidateView(Entity entity, View view, bool checkId = false)
        {
            List<ErrorModel> errorList = new List<ErrorModel>();

            if (!view.Id.HasValue || view.Id.Value == Guid.Empty)
                errorList.Add(new ErrorModel("views.id", null, "Id is required!"));

            if (string.IsNullOrWhiteSpace(view.Name))
                errorList.Add(new ErrorModel("views.name", view.Name, "Name is required!"));
            else
            {
                if (view.Name.Length > 30)
                    errorList.Add(new ErrorModel("name", view.Name, "The length of Name must be less than 30 characters!"));

                string pattern = @"[a-zA-Z][a-zA-Z0-9_]*";

                Match match = Regex.Match(view.Name, pattern);
                if (!match.Success || match.Value != view.Name.Trim())
                    errorList.Add(new ErrorModel("views.name", view.Name, "Name can only contains underscores and alphanumeric characters. It must begin with a letter, not include spaces, not end with an underscore, and not contain two consecutive underscores.!"));
            }

            if (string.IsNullOrWhiteSpace(view.Label))
                errorList.Add(new ErrorModel("views.label", view.Label, "Label is required!"));
            else
            {
                //TODO check if we need this validation
                /*
                if (view.Label.Length > 50)
                    errorList.Add(new ErrorModel("views.label", view.Label, "The length of Label must be less than 50 characters!"));

                string pattern = @"[A-Za-z][A-Za-z0-9\s_.-]*";

                Match match = Regex.Match(view.Label, pattern);
                if (!match.Success || match.Value != view.Label.Trim())
                    errorList.Add(new ErrorModel("views.label", view.Label, "Label can only contains underscores, dashes, dots, spaces and alphanumeric characters.!"));
                */
            }

            if (view.Filters != null && view.Filters.Count > 0)
            {

                foreach (var filter in view.Filters)
                {
                    if (!filter.FieldId.HasValue || filter.FieldId.Value == Guid.Empty)
                        errorList.Add(new ErrorModel("views.filters.fieldId", null, "FieldId is required!"));

                    if (filter.EntityId.HasValue && filter.EntityId.Value != Guid.Empty)
                    {
                        IStorageEntity verifiedEntity = EntityRepository.Read(filter.EntityId.Value);

                        if (verifiedEntity != null || filter.EntityId == entity.Id)
                        {
                            Entity currentEntity = verifiedEntity != null ? new Entity(verifiedEntity) : entity;

                            if (currentEntity.Fields.Where(f => f.Id == filter.FieldId).Count() == 0)
                                errorList.Add(new ErrorModel("views.filters.fieldId", filter.FieldId.ToString(), "Filter with such Id does not exist!"));
                        }
                        else
                            errorList.Add(new ErrorModel("views.filters.entityId", filter.EntityId.ToString(), "Entity with such Id does not exist!"));
                    }
                    else
                        errorList.Add(new ErrorModel("views.filters.entityId", null, "EntityId is required!"));

                    if (string.IsNullOrWhiteSpace(filter.Value))
                        errorList.Add(new ErrorModel("views.filters.value", null, "Value is required!"));
                }
            }

            if (view.Fields != null && view.Fields.Count > 0)
            {
                foreach (var field in view.Fields)
                {
                    if (!field.Id.HasValue || field.Id.Value == Guid.Empty)
                        errorList.Add(new ErrorModel("views.fields.id", null, "Id is required!"));

                    if (field.EntityId.HasValue && field.EntityId.Value != Guid.Empty)
                    {
                        IStorageEntity verifiedEntity = EntityRepository.Read(field.EntityId.Value);

                        if (verifiedEntity != null || field.EntityId == entity.Id)
                        {
                            Entity currentEntity = verifiedEntity != null ? new Entity(verifiedEntity) : entity;

                            if (currentEntity.Fields.Where(f => f.Id == field.Id).Count() == 0)
                                errorList.Add(new ErrorModel("views.fields.id", field.Id.ToString(), "Field with such Id does not exist!"));
                        }
                        else
                            errorList.Add(new ErrorModel("views.fields.entityId", field.EntityId.ToString(), "Entity with such Id does not exist!"));
                    }
                    else
                        errorList.Add(new ErrorModel("views.fields.entityId", null, "EntityId is required!"));

                    if (!field.Position.HasValue)
                        errorList.Add(new ErrorModel("views.fields.position", null, "Position is required!"));
                }
            }
            else
                errorList.Add(new ErrorModel("views.fields", view.Fields.ToString(), "Fields cannot be null or empty. It must contain at least field!"));

            return errorList;
        }

        public List<ErrorModel> ValidateForms(Guid entityId, List<Form> forms, bool checkId = false)
        {
            List<ErrorModel> errorList = new List<ErrorModel>();

            IStorageEntity storageEntity = EntityRepository.Read(entityId);
            Entity entity = new Entity(storageEntity);

            foreach (var form in forms)
            {
                errorList.AddRange(ValidateForm(entity, form, checkId));
            }

            return errorList;
        }

        public List<ErrorModel> ValidateForm(Entity entity, Form form, bool checkId = false)
        {
            List<ErrorModel> errorList = new List<ErrorModel>();

            if (!form.Id.HasValue || form.Id.Value == Guid.Empty)
                errorList.Add(new ErrorModel("forms.id", null, "Id is required!"));

            if (string.IsNullOrWhiteSpace(form.Name))
                errorList.Add(new ErrorModel("forms.name", form.Name, "Name is required!"));
            else
            {
                if (form.Name.Length > 30)
                    errorList.Add(new ErrorModel("forms.name", form.Name, "The length of Name must be less than 30 characters!"));

                string pattern = @"[a-zA-Z][a-zA-Z0-9_]*";

                Match match = Regex.Match(form.Name, pattern);
                if (!match.Success || match.Value != form.Name.Trim())
                    errorList.Add(new ErrorModel("forms.name", form.Name, "Name can only contains underscores and alphanumeric characters. It must begin with a letter, not include spaces, not end with an underscore, and not contain two consecutive underscores.!"));
            }

            if (string.IsNullOrWhiteSpace(form.Label))
                errorList.Add(new ErrorModel("forms.label", form.Label, "Label is required!"));
            else
            {
                //TODO check if we need this validation
                /*
                if (form.Label.Length > 50)
                    errorList.Add(new ErrorModel("forms.label", form.Label, "The length of Label must be less than 50 characters!"));

                string pattern = @"[A-Za-z][A-Za-z0-9\s_.-]*";

                Match match = Regex.Match(form.Label, pattern);
                if (!match.Success || match.Value != form.Label.Trim())
                    errorList.Add(new ErrorModel("forms.label", form.Label, "Label can only contains underscores, dashes, dots, spaces and alphanumeric characters.!"));
               */
            }

            foreach (var field in form.Fields)
            {
                if (!field.Id.HasValue && field.Id.Value == Guid.Empty)
                    errorList.Add(new ErrorModel("forms.fields.id", null, "Id is required!"));

                if (field.EntityId.HasValue && field.EntityId.Value != Guid.Empty)
                {
                    IStorageEntity verifiedEntity = EntityRepository.Read(field.EntityId.Value);

                    if (verifiedEntity != null || field.EntityId == entity.Id)
                    {
                        Entity currentEntity = verifiedEntity != null ? new Entity(verifiedEntity) : entity;

                        if (currentEntity.Fields.Where(f => f.Id == field.Id).Count() == 0)
                            errorList.Add(new ErrorModel("forms.fields.id", entity.Id.ToString(), "Field with such Id does not exist!"));
                    }
                    else
                        errorList.Add(new ErrorModel("forms.fields.entityId", field.EntityId.ToString(), "Entity with such Id does not exist!"));
                }
                else
                    errorList.Add(new ErrorModel("forms.fields.entityId", null, "EntityId is required!"));

                if (!field.Position.HasValue)
                    errorList.Add(new ErrorModel("forms.fields.position", null, "Position is required!"));

                if (!field.Column.HasValue)
                    errorList.Add(new ErrorModel("forms.column.position", null, "Column is required!"));
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
                if (inputEntity.Id == null)
                    inputEntity.Id = Guid.NewGuid();

                response.Errors = ValidateEntity(inputEntity, false);

                if (response.Errors.Count > 0)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The entity was not created. Validation error occurred!";
                    return response;
                }

                entity.Id = inputEntity.Id;
                entity.Fields = CreateEntityDefaultFields(entity);
                entity.Views = CreateEntityDefaultViews(entity);
                entity.Forms = CreateEntityDefaultForms(entity);

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
            catch (Exception)
            {
                response.Success = false;
                response.Message = "The entity was not created. An internal error occurred!";
                response.Object = entity;
                response.Timestamp = DateTime.UtcNow;
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
                response.Object = new Entity(inputEntity);
                response.Errors = ValidateEntity(inputEntity, true);

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

                bool result = EntityRepository.Update(storageEntity);

                if (!result)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The entity was not updated! An internal error occurred!";
                    return response;
                }

            }
            catch (Exception)
            {
                response.Success = false;
                response.Message = "The entity was not updated. An internal error occurred!";
                response.Object = entity;
                response.Timestamp = DateTime.UtcNow;
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
            catch (Exception)
            {
                response.Timestamp = DateTime.UtcNow;
                response.Success = false;
                response.Message = "The entity was not deleted. An internal error occurred!";
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
            catch (Exception)
            {
                response.Timestamp = DateTime.UtcNow;
                response.Success = false;
                response.Message = "An internal error occurred!";
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
                response.Message = "An internal error occurred!";
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
            catch (Exception)
            {
                response.Timestamp = DateTime.UtcNow;
                response.Success = false;
                response.Message = "An internal error occurred!";
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
                response.Message = "The field was not created. An internal error occurred!";
                response.Object = field;
                response.Timestamp = DateTime.UtcNow;
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
            catch (Exception)
            {
                response.Success = false;
                response.Message = "The field was not updated. An internal error occurred!";
                response.Object = field;
                response.Timestamp = DateTime.UtcNow;
                return response;
            }

            response.Object = field;
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
            catch (Exception)
            {
                response.Timestamp = DateTime.UtcNow;
                response.Success = false;
                response.Message = "The field was not deleted. An internal error occurred!";
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
            catch (Exception)
            {
                response.Timestamp = DateTime.UtcNow;
                response.Success = false;
                response.Message = "An internal error occurred!";
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
            catch (Exception)
            {
                response.Timestamp = DateTime.UtcNow;
                response.Success = false;
                response.Message = "An internal error occurred!";
                return response;
            }

            response.Timestamp = DateTime.Now;

            return response;
        }

        #endregion

        #region <-- View methods -->

        public ViewResponse CreateView(Guid entityId, View view)
        {
            ViewResponse response = new ViewResponse
            {
                Success = true,
                Message = "The view was successfully created!",
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

                response.Object = view;
                response.Errors = ValidateView(entity, view, false);

                if (response.Errors.Count > 0)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The view was not created. Validation error occurred!";
                    return response;
                }

                entity.Views.Add(view);

                IStorageEntity updatedEntity = EntityRepository.Convert(entity);
                bool result = EntityRepository.Update(updatedEntity);
                if (!result)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The view was not created! An internal error occurred!";
                    return response;
                }

            }
            catch (Exception)
            {
                response.Success = false;
                response.Message = "The view was not created. An internal error occurred!";
                response.Object = view;
                response.Timestamp = DateTime.UtcNow;
                return response;
            }

            response.Object = view;
            response.Timestamp = DateTime.UtcNow;

            return response;
        }

        public ViewResponse UpdateView(Guid entityId, View view)
        {
            ViewResponse response = new ViewResponse
            {
                Success = true,
                Message = "The view was successfully updated!",
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

                response.Object = view;
                response.Errors = ValidateView(entity, view, true);

                if (response.Errors.Count > 0)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The view was not updated. Validation error occurred!";
                    return response;
                }

                View viewForDelete = entity.Views.FirstOrDefault(v => v.Id == view.Id);
                if (viewForDelete.Id == view.Id)
                    entity.Views.Remove(viewForDelete);

                entity.Views.Add(view);

                IStorageEntity updatedEntity = EntityRepository.Convert(entity);
                bool result = EntityRepository.Update(updatedEntity);
                if (!result)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The view was not updated! An internal error occurred!";
                    return response;
                }

            }
            catch (Exception)
            {
                response.Success = false;
                response.Message = "The view was not updated. An internal error occurred!";
                response.Object = view;
                response.Timestamp = DateTime.UtcNow;
                return response;
            }

            response.Object = view;
            response.Timestamp = DateTime.UtcNow;

            return response;
        }

        public ViewResponse DeleteView(Guid entityId, Guid id)
        {
            ViewResponse response = new ViewResponse
            {
                Success = true,
                Message = "The view was successfully deleted!",
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

                View view = entity.Views.FirstOrDefault(v => v.Id == id);

                if (view == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The view was not deleted. Validation error occurred!";
                    response.Errors.Add(new ErrorModel("id", id.ToString(), "View with such Id does not exist!"));
                    return response;
                }

                entity.Views.Remove(view);

                IStorageEntity updatedEntity = EntityRepository.Convert(entity);
                bool result = EntityRepository.Update(updatedEntity);
                if (!result)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The view was not updated! An internal error occurred!";
                    return response;
                }
            }
            catch (Exception)
            {
                response.Timestamp = DateTime.UtcNow;
                response.Success = false;
                response.Message = "The view was not deleted. An internal error occurred!";
                return response;
            }

            response.Timestamp = DateTime.UtcNow;
            return response;
        }

        public ViewListResponse ReadViews(Guid entityId)
        {
            ViewListResponse response = new ViewListResponse
            {
                Success = true,
                Message = "The views were successfully returned!",
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

                ViewList viewList = new ViewList();
                viewList.Views = entity.Views;
                viewList.Offset = viewList.Views != null && viewList.Views.Count > 0 ? viewList.Views.Last().Id.Value : Guid.Empty;

                response.Object = viewList;
            }
            catch (Exception)
            {
                response.Timestamp = DateTime.UtcNow;
                response.Success = false;
                response.Message = "An internal error occurred!";
                return response;
            }

            response.Timestamp = DateTime.Now;

            return response;
        }

        public ViewResponse ReadView(Guid entityId, Guid id)
        {
            ViewResponse response = new ViewResponse
            {
                Success = true,
                Message = "The view was successfully returned!",
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

                View view = entity.Views.FirstOrDefault(v => v.Id == id);

                if (view == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Validation error occurred!";
                    response.Errors.Add(new ErrorModel("id", id.ToString(), "View with such Id does not exist!"));
                    return response;
                }

                response.Object = view;
            }
            catch (Exception)
            {
                response.Timestamp = DateTime.UtcNow;
                response.Success = false;
                response.Message = "An internal error occurred!";
                return response;
            }

            response.Timestamp = DateTime.Now;

            return response;
        }

        #endregion

        #region <-- Form methods -->

        public FormResponse CreateForm(Guid entityId, Form form)
        {
            FormResponse response = new FormResponse
            {
                Success = true,
                Message = "The form was successfully created!",
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

                response.Object = form;
                response.Errors = ValidateForm(entity, form, false);

                if (response.Errors.Count > 0)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The form was not created. Validation error occurred!";
                    return response;
                }

                entity.Forms.Add(form);

                IStorageEntity updatedEntity = EntityRepository.Convert(entity);
                bool result = EntityRepository.Update(updatedEntity);
                if (!result)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The form was not created! An internal error occurred!";
                    return response;
                }

            }
            catch (Exception)
            {
                response.Success = false;
                response.Message = "The form was not created. An internal error occurred!";
                response.Object = form;
                response.Timestamp = DateTime.UtcNow;
                return response;
            }

            response.Object = form;
            response.Timestamp = DateTime.UtcNow;

            return response;
        }

        public FormResponse UpdateForm(Guid entityId, Form form)
        {
            FormResponse response = new FormResponse
            {
                Success = true,
                Message = "The form was successfully updated!",
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

                response.Object = form;
                response.Errors = ValidateForm(entity, form, true);

                if (response.Errors.Count > 0)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The form was not updated. Validation error occurred!";
                    return response;
                }

                Form formForDelete = entity.Forms.FirstOrDefault(f => f.Id == form.Id);
                if (formForDelete.Id == form.Id)
                    entity.Forms.Remove(formForDelete);

                entity.Forms.Add(form);

                IStorageEntity updatedEntity = EntityRepository.Convert(entity);
                bool result = EntityRepository.Update(updatedEntity);
                if (!result)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The form was not updated! An internal error occurred!";
                    return response;
                }

            }
            catch (Exception)
            {
                response.Success = false;
                response.Message = "The form was not updated. An internal error occurred!";
                response.Object = form;
                response.Timestamp = DateTime.UtcNow;
                return response;
            }

            response.Object = form;
            response.Timestamp = DateTime.UtcNow;

            return response;
        }

        public FormResponse DeleteForm(Guid entityId, Guid id)
        {
            FormResponse response = new FormResponse
            {
                Success = true,
                Message = "The form was successfully deleted!",
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

                Form form = entity.Forms.FirstOrDefault(f => f.Id == id);

                if (form == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The form was not deleted. Validation error occurred!";
                    response.Errors.Add(new ErrorModel("id", id.ToString(), "Form with such Id does not exist!"));
                    return response;
                }

                entity.Forms.Remove(form);

                IStorageEntity updatedEntity = EntityRepository.Convert(entity);
                bool result = EntityRepository.Update(updatedEntity);
                if (!result)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The form was not updated! An internal error occurred!";
                    return response;
                }
            }
            catch (Exception)
            {
                response.Timestamp = DateTime.UtcNow;
                response.Success = false;
                response.Message = "The form was not deleted. An internal error occurred!";
                return response;
            }

            response.Timestamp = DateTime.UtcNow;
            return response;
        }

        public FormListResponse ReadForms(Guid entityId)
        {
            FormListResponse response = new FormListResponse
            {
                Success = true,
                Message = "The forms were successfully returned!",
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

                FormList formList = new FormList();
                formList.Forms = entity.Forms;
                formList.Offset = formList.Forms != null && formList.Forms.Count > 0 ? formList.Forms.Last().Id.Value : Guid.Empty;

                response.Object = formList;
            }
            catch (Exception)
            {
                response.Timestamp = DateTime.UtcNow;
                response.Success = false;
                response.Message = "An internal error occurred!";
                return response;
            }

            response.Timestamp = DateTime.Now;

            return response;
        }

        public FormResponse ReadForm(Guid entityId, Guid id)
        {
            FormResponse response = new FormResponse
            {
                Success = true,
                Message = "The form was successfully returned!",
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

                Form form = entity.Forms.FirstOrDefault(f => f.Id == id);

                if (form == null)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "Validation error occurred!";
                    response.Errors.Add(new ErrorModel("id", id.ToString(), "Form with such Id does not exist!"));
                    return response;
                }

                response.Object = form;
            }
            catch (Exception)
            {
                response.Timestamp = DateTime.UtcNow;
                response.Success = false;
                response.Message = "An internal error occurred!";
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
            primaryKeyField.Searchable = true;
            primaryKeyField.Auditable = false;
            primaryKeyField.System = true;
            primaryKeyField.DefaultValue = Guid.Empty;

            fields.Add(primaryKeyField);

            LookupRelationField createdBy = new LookupRelationField();

            createdBy.Id = Guid.NewGuid();
            createdBy.Name = "createdBy";
            createdBy.Label = "Created By";
            createdBy.PlaceholderText = "";
            createdBy.Description = "";
            createdBy.HelpText = "";
            createdBy.Required = true;
            createdBy.Unique = false;
            createdBy.Searchable = true;
            createdBy.Auditable = false;
            createdBy.System = true;

            createdBy.RelatedEntityId = Guid.Empty; //User entity id must be set here

            fields.Add(createdBy);

            LookupRelationField lastModifiedBy = new LookupRelationField();

            lastModifiedBy.Id = Guid.NewGuid();
            lastModifiedBy.Name = "lastModifiedBy";
            lastModifiedBy.Label = "Last Modified By";
            lastModifiedBy.PlaceholderText = "";
            lastModifiedBy.Description = "";
            lastModifiedBy.HelpText = "";
            lastModifiedBy.Required = true;
            lastModifiedBy.Unique = false;
            lastModifiedBy.Searchable = true;
            lastModifiedBy.Auditable = false;
            lastModifiedBy.System = true;

            lastModifiedBy.RelatedEntityId = Guid.Empty; //User entity id must be set here

            fields.Add(lastModifiedBy);

            DateTimeField createdOn = new DateTimeField();

            createdOn.Id = Guid.NewGuid();
            createdOn.Name = "createdOn";
            createdOn.Label = "CreatedOn";
            createdOn.PlaceholderText = "";
            createdOn.Description = "";
            createdOn.HelpText = "";
            createdOn.Required = true;
            createdOn.Unique = false;
            createdOn.Searchable = true;
            createdOn.Auditable = false;
            createdOn.System = true;
            createdOn.DefaultValue = DateTime.MinValue;

            createdOn.Format = "MM/dd/YYYY";

            fields.Add(createdOn);

            return fields;
        }

        private List<View> CreateEntityDefaultViews(Entity entity)
        {
            List<View> views = new List<View>();


            return views;
        }

        private List<Form> CreateEntityDefaultForms(Entity entity)
        {
            List<Form> forms = new List<Form>();


            return forms;
        }

        #endregion
    }
}