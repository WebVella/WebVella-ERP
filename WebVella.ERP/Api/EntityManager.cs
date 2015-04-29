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

        public List<ErrorModel> Validate(Entity entity, bool checkId = false)
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
                if (entity.Name.Length > 20)
                    errorList.Add(new ErrorModel("name", entity.Name, "The lenght of Name must be less than 20 characters!"));

                string pattern = @"^([A-Za-z][A-Za-z0-9\s/_/.'-]{1,20})$";

                Match match = Regex.Match(entity.Name, pattern);
                if (match.Success && match.Value == entity.Name.Trim())
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
                if (entity.Label.Length > 50)
                    errorList.Add(new ErrorModel("label", entity.Label, "The lenght of Label must be less than 50 characters!"));

                string pattern = @"^([A-Za-z][A-Za-z0-9\s_.-]{1,50})$";

                Match match = Regex.Match(entity.Label, pattern);
                if (match.Success && match.Value == entity.Label.Trim())
                    errorList.Add(new ErrorModel("label", entity.Label, "Label can only contains underscores, dashes, dots, spaces and alphanumeric characters.!"));
            }

            if (string.IsNullOrWhiteSpace(entity.PluralLabel))
                errorList.Add(new ErrorModel("pluralLabel", entity.PluralLabel, "Plural Label is required!"));
            else
            {
                if (entity.PluralLabel.Length > 50)
                    errorList.Add(new ErrorModel("pluralLabel", entity.PluralLabel, "The lenght of Plural Label must be less than 50 characters!"));

                string pattern = @"^([A-Za-z][A-Za-z0-9\s_.-]{1,50})$";

                Match match = Regex.Match(entity.PluralLabel, pattern);
                if (match.Success && match.Value == entity.PluralLabel.Trim())
                    errorList.Add(new ErrorModel("pluralLabel", entity.PluralLabel, "Plural Label can only contains underscores, dashes, dots, spaces and alphanumeric characters.!"));
            }

            if (!entity.System.HasValue)
                errorList.Add(new ErrorModel("system", null, "System is required!"));

            errorList.AddRange(ValidateFields(entity, checkId));

            errorList.AddRange(ValidateViews(entity, checkId));

            errorList.AddRange(ValidateForms(entity, checkId));

            return errorList;
        }

        public List<ErrorModel> ValidateFields(Entity entity, bool checkId = false)
        {
            List<ErrorModel> errorList = new List<ErrorModel>();

            IList<Field> fields = entity.Fields;

            if (fields.Count == 0)
            {
                errorList.Add(new ErrorModel("fields", null, "There should be at least one field!"));
                return errorList;
            }

            foreach (var field in fields)
            {
                if (field.Id == Guid.Empty)
                    errorList.Add(new ErrorModel("fields.id", null, "Id is required!"));

                if (checkId)
                {
                    int fieldSameIdCount = fields.Where(f => f.Id == field.Id).Count();

                    if (fieldSameIdCount > 1)
                        errorList.Add(new ErrorModel("fields.id", null, "There are multiple fields with same Id!"));

                    int fieldSameNameCount = fields.Where(f => f.Id == field.Id).Count();

                    if (fieldSameNameCount > 1)
                        errorList.Add(new ErrorModel("fields.name", null, "There are multiple fields with same Name!"));
                }

                if (string.IsNullOrWhiteSpace(field.Name))
                    errorList.Add(new ErrorModel("fields.name", field.Name, "Name is required!"));
                else
                {
                    if (field.Name.Length > 20)
                        errorList.Add(new ErrorModel("fields.name", field.Name, "The lenght of Name must be less than 20 characters!"));

                    string pattern = @"^([A-Za-z][A-Za-z0-9_]{1,20})$";

                    Match match = Regex.Match(field.Name, pattern);
                    if (match.Success && match.Value == field.Name.Trim())
                        errorList.Add(new ErrorModel("fields.name", field.Name, "Name can only contains underscores and alphanumeric characters. It must begin with a letter, not include spaces, not end with an underscore, and not contain two consecutive underscores.!"));

                }

                if (string.IsNullOrWhiteSpace(field.Label))
                    errorList.Add(new ErrorModel("fields.label", field.Label, "Label is required!"));
                else
                {
                    if (field.Label.Length > 20)
                        errorList.Add(new ErrorModel("fields.label", field.Label, "The lenght of Label must be less than 20 characters!"));

                    string pattern = @"^([A-Za-z][A-Za-z0-9\s_.-]{1,20})$";

                    Match match = Regex.Match(field.Label, pattern);
                    if (match.Success && match.Value == field.Label.Trim())
                        errorList.Add(new ErrorModel("fields.label", field.Label, "Label can only contains underscores, dashes, dots, spaces and alphanumeric characters.!"));
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
                    if (string.IsNullOrWhiteSpace(((FileField)field).DefaultValue))
                        errorList.Add(new ErrorModel("fields.defaultValue", null, "Default Value is required!"));
                }
                else if (field is FormulaField)
                {
                    if (!string.IsNullOrWhiteSpace(((FormulaField)field).FormulaText))
                    {
                        //TODO: parse formula text and check if it is valid
                    }
                    else
                        errorList.Add(new ErrorModel("fields.formulaText", null, "Formula Text is required!"));

                    if (!((FormulaField)field).DecimalPlaces.HasValue)
                        errorList.Add(new ErrorModel("fields.decimalPlaces", null, "Decimal Places is required!"));
                }
                else if (field is HtmlField)
                {
                    if ((((HtmlField)field).DefaultValue) == null)
                        errorList.Add(new ErrorModel("fields.defaultValue", null, "Default Value is required!"));
                }
                else if (field is ImageField)
                {
                    if (((ImageField)field).DefaultValue == null)
                        errorList.Add(new ErrorModel("fields.defaultValue", null, "Default Value is required!"));

                    if (string.IsNullOrWhiteSpace(((ImageField)field).TargetEntityType))
                        errorList.Add(new ErrorModel("fields.targetEntityType", null, "Target Entity Type is required!"));

                    if (string.IsNullOrWhiteSpace(((ImageField)field).RelationshipName))
                        errorList.Add(new ErrorModel("fields.relationshipName", null, "Relationship Name is required!"));
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
                    if (((PasswordField)field).DefaultValue == null)
                        errorList.Add(new ErrorModel("fields.defaultValue", null, "Default Value is required!"));

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
                else if (field is PrimaryKeyField)
                {
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
            }

            return errorList;
        }

        public List<ErrorModel> ValidateViews(Entity entity, bool checkId = false)
        {
            List<ErrorModel> errorList = new List<ErrorModel>();

            IList<View> views = entity.Views;

            foreach (var view in views)
            {
                if (!view.Id.HasValue || view.Id.Value == Guid.Empty)
                    errorList.Add(new ErrorModel("views.id", null, "Id is required!"));

                if (string.IsNullOrWhiteSpace(view.Name))
                    errorList.Add(new ErrorModel("views.name", view.Name, "Name is required!"));
                else
                {
                    if (view.Name.Length > 20)
                        errorList.Add(new ErrorModel("name", view.Name, "The lenght of Name must be less than 20 characters!"));

                    string pattern = @"^([A-Za-z][A-Za-z0-9\s/_/.'-]{1,20})$";

                    Match match = Regex.Match(view.Name, pattern);
                    if (match.Success && match.Value == view.Name.Trim())
                        errorList.Add(new ErrorModel("views.name", view.Name, "Name can only contains underscores and alphanumeric characters. It must begin with a letter, not include spaces, not end with an underscore, and not contain two consecutive underscores.!"));
                }

                if (string.IsNullOrWhiteSpace(view.Label))
                    errorList.Add(new ErrorModel("views.label", view.Label, "Label is required!"));
                else
                {
                    if (view.Label.Length > 50)
                        errorList.Add(new ErrorModel("views.label", view.Label, "The lenght of Label must be less than 50 characters!"));

                    string pattern = @"^([A-Za-z][A-Za-z0-9\s_.-]{1,50})$";

                    Match match = Regex.Match(view.Label, pattern);
                    if (match.Success && match.Value == view.Label.Trim())
                        errorList.Add(new ErrorModel("views.label", view.Label, "Label can only contains underscores, dashes, dots, spaces and alphanumeric characters.!"));
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
                                    errorList.Add(new ErrorModel("views.filters.fieldId", entity.Id.ToString(), "Filter with such Id does not exist!"));
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
                                    errorList.Add(new ErrorModel("views.fields.id", entity.Id.ToString(), "Field with such Id does not exist!"));
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
            }

            return errorList;
        }

        public List<ErrorModel> ValidateForms(Entity entity, bool checkId = false)
        {
            List<ErrorModel> errorList = new List<ErrorModel>();

            IList<Form> forms = entity.Forms;

            foreach (Form form in forms)
            {
                if (!form.Id.HasValue || form.Id.Value == Guid.Empty)
                    errorList.Add(new ErrorModel("forms.id", null, "Id is required!"));

                if (string.IsNullOrWhiteSpace(form.Name))
                    errorList.Add(new ErrorModel("forms.name", form.Name, "Name is required!"));
                else
                {
                    if (form.Name.Length > 20)
                        errorList.Add(new ErrorModel("forms.name", form.Name, "The lenght of Name must be less than 20 characters!"));

                    string pattern = @"^([A-Za-z][A-Za-z0-9\s/_/.'-]{1,20})$";

                    Match match = Regex.Match(form.Name, pattern);
                    if (match.Success && match.Value == form.Name.Trim())
                        errorList.Add(new ErrorModel("forms.name", form.Name, "Name can only contains underscores and alphanumeric characters. It must begin with a letter, not include spaces, not end with an underscore, and not contain two consecutive underscores.!"));
                }

                if (string.IsNullOrWhiteSpace(form.Label))
                    errorList.Add(new ErrorModel("forms.label", form.Label, "Label is required!"));
                else
                {
                    if (form.Label.Length > 50)
                        errorList.Add(new ErrorModel("forms.label", form.Label, "The lenght of Label must be less than 50 characters!"));

                    string pattern = @"^([A-Za-z][A-Za-z0-9\s_.-]{1,50})$";

                    Match match = Regex.Match(form.Label, pattern);
                    if (match.Success && match.Value == form.Label.Trim())
                        errorList.Add(new ErrorModel("forms.label", form.Label, "Label can only contains underscores, dashes, dots, spaces and alphanumeric characters.!"));
                }

                foreach (var field in form.Fields)
                {
                    if (field.Id.HasValue && field.Id.Value != Guid.Empty)
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
            }

            return errorList;
        }

        public EntityResponse Create(Entity entity)
        {
            EntityResponse response = new EntityResponse
            {
                Success = true,
                Message = "The entity was successfully created!",
            };

            try
            {
                response.Object = entity;
                response.Errors = Validate(entity, true);

                if (response.Errors.Count > 0)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The entity was not created. Validation error occurred!";
                    return response;
                }

                bool result = EntityRepository.Create((IStorageEntity)entity);
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

            IStorageEntity storageEntity = EntityRepository.Read(entity.Id.Value);
            response.Object = new Entity(storageEntity);
            response.Timestamp = DateTime.UtcNow;

            return response;
        }

        public EntityResponse Update(Entity entity)
        {
            EntityResponse response = new EntityResponse
            {
                Success = true,
                Message = "The entity was successfully updated!",
            };

            try
            {
                response.Object = entity;
                response.Errors = Validate(entity, true);

                if (response.Errors.Count > 0)
                {
                    response.Timestamp = DateTime.UtcNow;
                    response.Success = false;
                    response.Message = "The entity was not updated. Validation error occurred!";
                    return response;
                }

                bool result = EntityRepository.Update((IStorageEntity)entity);
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

            IStorageEntity storageEntity = EntityRepository.Read(entity.Id.Value);
            response.Object = new Entity(storageEntity);
            response.Timestamp = DateTime.UtcNow;

            return response;
        }

        public EntityResponse Delete(Guid id)
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

        public EntityListResponse Read()
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

        public EntityResponse Read(Guid id)
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
    }
}