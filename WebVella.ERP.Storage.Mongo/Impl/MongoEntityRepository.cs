using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Builders;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Storage.Mongo.Impl;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoEntityRepository : IStorageEntityRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IStorageEntity Empty()
        {
            return new MongoEntity();
        }

        public IStorageEntity Convert(Entity entity)
        {
            MongoEntity storageEntity = new MongoEntity();

            storageEntity.Id = entity.Id.Value;
            storageEntity.Name = entity.Name;
            storageEntity.Label = entity.Label;
            storageEntity.PluralLabel = entity.PluralLabel;
            storageEntity.System = entity.System.Value;

            storageEntity.Fields = new List<IStorageField>();

            foreach (Field field in entity.Fields)
            {
                MongoBaseField storageField = null;
                if (field is AutoNumberField)
                {
                    storageField = new MongoAutoNumberField();
                    ((MongoAutoNumberField)storageField).DefaultValue = ((AutoNumberField)field).DefaultValue.Value;
                    ((MongoAutoNumberField)storageField).DisplayFormat = ((AutoNumberField)field).DisplayFormat;
                    ((MongoAutoNumberField)storageField).StartingNumber = ((AutoNumberField)field).StartingNumber.Value;
                }
                else if (field is CheckboxField)
                {
                    storageField = new MongoCheckboxField();
                    ((MongoCheckboxField)storageField).DefaultValue = ((CheckboxField)field).DefaultValue.Value;
                }
                else if (field is CurrencyField)
                {
                    storageField = new MongoCurrencyField();
                    ((MongoCurrencyField)storageField).DefaultValue = ((CurrencyField)field).DefaultValue.Value;
                    ((MongoCurrencyField)storageField).MinValue = ((CurrencyField)field).MinValue.Value;
                    ((MongoCurrencyField)storageField).MaxValue = ((CurrencyField)field).MaxValue.Value;
                    ((MongoCurrencyField)storageField).Currency = ((CurrencyField)field).Currency;
                }
                else if (field is DateField)
                {
                    storageField = new MongoDateField();
                    ((MongoDateField)storageField).DefaultValue = ((DateField)field).DefaultValue.Value;
                    ((MongoDateField)storageField).Format = ((DateField)field).Format;
                }
                else if (field is DateTimeField)
                {
                    storageField = new MongoDateTimeField();
                    ((MongoDateTimeField)storageField).DefaultValue = ((DateTimeField)field).DefaultValue.Value;
                    ((MongoDateTimeField)storageField).Format = ((DateTimeField)field).Format;
                }
                else if (field is EmailField)
                {
                    storageField = new MongoEmailField();
                    ((MongoEmailField)storageField).DefaultValue = ((EmailField)field).DefaultValue;
                    ((MongoEmailField)storageField).MaxLength = ((EmailField)field).MaxLength.Value;
                }
                else if (field is FileField)
                {
                    storageField = new MongoFileField();
                    ((MongoFileField)storageField).DefaultValue = ((FileField)field).DefaultValue;
                }
                //else if (field is FormulaField)
                //{
                //    storageField = new MongoFormulaField();
                //    ((MongoFormulaField)storageField).ReturnType = ((FormulaField)field).ReturnType;
                //    ((MongoFormulaField)storageField).FormulaText = ((FormulaField)field).FormulaText;
                //    ((MongoFormulaField)storageField).DecimalPlaces = ((FormulaField)field).DecimalPlaces.Value;
                //}
                else if (field is HtmlField)
                {
                    storageField = new MongoHtmlField();
                    ((MongoHtmlField)storageField).DefaultValue = ((HtmlField)field).DefaultValue;
                }
                else if (field is ImageField)
                {
                    storageField = new MongoImageField();
                    ((MongoImageField)storageField).DefaultValue = ((ImageField)field).DefaultValue;
                }
                else if (field is LookupRelationField)
                {
                    storageField = new MongoLookupRelationField();
                    ((MongoLookupRelationField)storageField).RelatedEntityId = ((LookupRelationField)field).RelatedEntityId.Value;
                }
                else if (field is MasterDetailsRelationshipField)
                {
                    storageField = new MongoMasterDetailsRelationshipField();
                    ((MongoMasterDetailsRelationshipField)storageField).RelatedEntityId = ((MasterDetailsRelationshipField)field).RelatedEntityId.Value;
                }
                else if (field is MultiLineTextField)
                {
                    storageField = new MongoMultiLineTextField();
                    ((MongoMultiLineTextField)storageField).DefaultValue = ((MultiLineTextField)field).DefaultValue;
                    ((MongoMultiLineTextField)storageField).MaxLength = ((MultiLineTextField)field).MaxLength.Value;
                    ((MongoMultiLineTextField)storageField).VisibleLineNumber = ((MultiLineTextField)field).VisibleLineNumber.Value;
                }
                else if (field is MultiSelectField)
                {
                    storageField = new MongoMultiSelectField();
                    ((MongoMultiSelectField)storageField).DefaultValue = ((MultiSelectField)field).DefaultValue;
                    ((MongoMultiSelectField)storageField).Options = ((MultiSelectField)field).Options;
                }
                else if (field is NumberField)
                {
                    storageField = new MongoNumberField();
                    ((MongoNumberField)storageField).DefaultValue = ((NumberField)field).DefaultValue.Value;
                    ((MongoNumberField)storageField).MinValue = ((NumberField)field).MinValue.Value;
                    ((MongoNumberField)storageField).MaxValue = ((NumberField)field).MaxValue.Value;
                    ((MongoNumberField)storageField).DecimalPlaces = ((NumberField)field).DecimalPlaces.Value;
                }
                else if (field is PasswordField)
                {
                    storageField = new MongoPasswordField();
                    ((MongoPasswordField)storageField).MaxLength = ((PasswordField)field).MaxLength.Value;
                    ((MongoPasswordField)storageField).MaskType = ((PasswordField)field).MaskType;
                    ((MongoPasswordField)storageField).MaskCharacter = ((PasswordField)field).MaskCharacter.Value;
                }
                else if (field is PercentField)
                {
                    storageField = new MongoPercentField();
                    ((MongoPercentField)storageField).DefaultValue = ((PercentField)field).DefaultValue.Value;
                    ((MongoPercentField)storageField).MinValue = ((PercentField)field).MinValue.Value;
                    ((MongoPercentField)storageField).MaxValue = ((PercentField)field).MaxValue.Value;
                    ((MongoPercentField)storageField).DecimalPlaces = ((PercentField)field).DecimalPlaces.Value;
                }
                else if (field is PhoneField)
                {
                    storageField = new MongoPhoneField();
                    ((MongoPhoneField)storageField).DefaultValue = ((PhoneField)field).DefaultValue;
                    ((MongoPhoneField)storageField).Format = ((PhoneField)field).Format;
                    ((MongoPhoneField)storageField).MaxLength = ((PhoneField)field).MaxLength.Value;
                }
                else if (field is PrimaryKeyField)
                {
                    storageField = new MongoPrimaryKeyField();
                    ((MongoPrimaryKeyField)storageField).DefaultValue = ((PrimaryKeyField)field).DefaultValue.Value;
                }
                else if (field is SelectField)
                {
                    storageField = new MongoSelectField();
                    ((MongoSelectField)storageField).DefaultValue = ((SelectField)field).DefaultValue;
                    ((MongoSelectField)storageField).Options = ((SelectField)field).Options;
                }
                else if (field is TextField)
                {
                    storageField = new MongoTextField();
                    ((MongoTextField)storageField).DefaultValue = ((TextField)field).DefaultValue;
                    ((MongoTextField)storageField).MaxLength = ((TextField)field).MaxLength.Value;
                }
                else if (field is UrlField)
                {
                    storageField = new MongoUrlField();
                    ((MongoUrlField)storageField).DefaultValue = ((UrlField)field).DefaultValue;
                    ((MongoUrlField)storageField).MaxLength = ((UrlField)field).MaxLength.Value;
                    ((MongoUrlField)storageField).OpenTargetInNewWindow = ((UrlField)field).OpenTargetInNewWindow.Value;
                }

                storageField.Id = field.Id.Value;
                storageField.Name = field.Name;
                storageField.Label = field.Label;
                storageField.PlaceholderText = field.PlaceholderText;
                storageField.Description = field.Description;
                storageField.HelpText = field.HelpText;
                storageField.Required = field.Required.Value;
                storageField.Unique = field.Unique.Value;
                storageField.Searchable = field.Searchable.Value;
                storageField.Auditable = field.Auditable.Value;
                storageField.System = field.System.Value;

                storageEntity.Fields.Add(storageField);
            }

            storageEntity.Views = new List<IStorageView>();

            foreach (View view in entity.Views)
            {
                MongoView storageView = new MongoView();
                storageView.Id = view.Id.Value;
                storageView.Name = view.Name;
                storageView.Label = view.Label;
                storageView.Type = view.Type;

                storageView.Filters = new List<IStorageViewFilter>();

                foreach (ViewFilter filter in view.Filters)
                {
                    MongoViewFilter storageFilter = new MongoViewFilter();

                    storageFilter.EntityId = filter.EntityId.Value;
                    storageFilter.FieldId = filter.FieldId.Value;
                    storageFilter.Operator = filter.Operator;
                    storageFilter.Value = filter.Value;

                    storageView.Filters.Add(storageFilter);
                }

                storageView.Fields = new List<IStorageViewField>();

                foreach (ViewField field in view.Fields)
                {
                    MongoViewField storageField = new MongoViewField();

                    storageField.EntityId = field.EntityId.Value;
                    storageField.Id = field.Id.Value;
                    storageField.Position = field.Position.Value;

                    storageView.Fields.Add(storageField);
                }

                storageEntity.Views.Add(storageView);
            }

            storageEntity.Forms = new List<IStorageForm>();

            foreach (Form form in entity.Forms)
            {
                MongoForm storageForm = new MongoForm();
                storageForm.Id = form.Id.Value;
                storageForm.Name = form.Name;
                storageForm.Label = form.Label;

                storageForm.Fields = new List<IStorageFormField>();

                foreach (FormField field in form.Fields)
                {
                    MongoFormField storageField = new MongoFormField();

                    storageField.EntityId = field.EntityId.Value;
                    storageField.Id = field.Id.Value;
                    storageField.Position = field.Position.Value;

                    storageForm.Fields.Add(storageField);
                }

                storageEntity.Forms.Add(storageForm);
            }

            return storageEntity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<IStorageEntity> Read()
        {
            return MongoStaticContext.Context.Entities.Get().ToList<IStorageEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IStorageEntity Read(Guid id)
        {
            return MongoStaticContext.Context.Entities.SingleOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IStorageEntity Read(string name)
        {
            return MongoStaticContext.Context.Entities.SingleOrDefault(x => x.Name.ToLowerInvariant() == name.ToLowerInvariant());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public bool Create(IStorageEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var mongoEntity = entity as MongoEntity;

            if (mongoEntity == null)
                throw new Exception("The specified entity is not mongo storage object.");

            return MongoStaticContext.Context.Entities.Create(mongoEntity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public bool Update(IStorageEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var mongoEntity = entity as MongoEntity;

            if (mongoEntity == null)
                throw new Exception("The specified entity is not mongo storage object.");

            return MongoStaticContext.Context.Entities.Update(mongoEntity);
        }

        /// <summary>
        /// Deletes entity document by identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(Guid id)
        {
            return MongoStaticContext.Context.Entities.Delete(Query.EQ("_id", id));
        }

        /// <summary>
        /// Saves entity document
        /// </summary>
        /// <param name="entity"></param>
        public bool Save(IStorageEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var mongoEntity = entity as MongoEntity;

            if (mongoEntity == null)
                throw new Exception("The specified entity is not mongo storage object.");

            return MongoStaticContext.Context.Entities.Save(mongoEntity);
        }


        public IStorageField ConvertField(Field field)
        {
            if (field == null)
                throw new ArgumentNullException("field");

            MongoBaseField storageField = null;
            if (field is AutoNumberField)
            {
                storageField = new MongoAutoNumberField();
                ((MongoAutoNumberField)storageField).DefaultValue = ((AutoNumberField)field).DefaultValue.Value;
                ((MongoAutoNumberField)storageField).DisplayFormat = ((AutoNumberField)field).DisplayFormat;
                ((MongoAutoNumberField)storageField).StartingNumber = ((AutoNumberField)field).StartingNumber.Value;
            }
            else if (field is CheckboxField)
            {
                storageField = new MongoCheckboxField();
                ((MongoCheckboxField)storageField).DefaultValue = ((CheckboxField)field).DefaultValue.Value;
            }
            else if (field is CurrencyField)
            {
                storageField = new MongoCurrencyField();
                ((MongoCurrencyField)storageField).DefaultValue = ((CurrencyField)field).DefaultValue.Value;
                ((MongoCurrencyField)storageField).MinValue = ((CurrencyField)field).MinValue.Value;
                ((MongoCurrencyField)storageField).MaxValue = ((CurrencyField)field).MaxValue.Value;
                ((MongoCurrencyField)storageField).Currency = ((CurrencyField)field).Currency;
            }
            else if (field is DateField)
            {
                storageField = new MongoDateField();
                ((MongoDateField)storageField).DefaultValue = ((DateField)field).DefaultValue.Value;
                ((MongoDateField)storageField).Format = ((DateField)field).Format;
            }
            else if (field is DateTimeField)
            {
                storageField = new MongoDateTimeField();
                ((MongoDateTimeField)storageField).DefaultValue = ((DateTimeField)field).DefaultValue.Value;
                ((MongoDateTimeField)storageField).Format = ((DateTimeField)field).Format;
            }
            else if (field is EmailField)
            {
                storageField = new MongoEmailField();
                ((MongoEmailField)storageField).DefaultValue = ((EmailField)field).DefaultValue;
                ((MongoEmailField)storageField).MaxLength = ((EmailField)field).MaxLength.Value;
            }
            else if (field is FileField)
            {
                storageField = new MongoFileField();
                ((MongoFileField)storageField).DefaultValue = ((FileField)field).DefaultValue;
            }
            //else if (field is FormulaField)
            //{
            //    storageField = new MongoFormulaField();
            //    ((MongoFormulaField)storageField).ReturnType = ((FormulaField)field).ReturnType;
            //    ((MongoFormulaField)storageField).FormulaText = ((FormulaField)field).FormulaText;
            //    ((MongoFormulaField)storageField).DecimalPlaces = ((FormulaField)field).DecimalPlaces.Value;
            //}
            else if (field is HtmlField)
            {
                storageField = new MongoHtmlField();
                ((MongoHtmlField)storageField).DefaultValue = ((HtmlField)field).DefaultValue;
            }
            else if (field is ImageField)
            {
                storageField = new MongoImageField();
                ((MongoImageField)storageField).DefaultValue = ((ImageField)field).DefaultValue;
            }
            else if (field is LookupRelationField)
            {
                storageField = new MongoLookupRelationField();
                ((MongoLookupRelationField)storageField).RelatedEntityId = ((LookupRelationField)field).RelatedEntityId.Value;
            }
            else if (field is MasterDetailsRelationshipField)
            {
                storageField = new MongoMasterDetailsRelationshipField();
                ((MongoMasterDetailsRelationshipField)storageField).RelatedEntityId = ((MasterDetailsRelationshipField)field).RelatedEntityId.Value;
            }
            else if (field is MultiLineTextField)
            {
                storageField = new MongoMultiLineTextField();
                ((MongoMultiLineTextField)storageField).DefaultValue = ((MultiLineTextField)field).DefaultValue;
                ((MongoMultiLineTextField)storageField).MaxLength = ((MultiLineTextField)field).MaxLength.Value;
                ((MongoMultiLineTextField)storageField).VisibleLineNumber = ((MultiLineTextField)field).VisibleLineNumber.Value;
            }
            else if (field is MultiSelectField)
            {
                storageField = new MongoMultiSelectField();
                ((MongoMultiSelectField)storageField).DefaultValue = ((MultiSelectField)field).DefaultValue;
                ((MongoMultiSelectField)storageField).Options = ((MultiSelectField)field).Options;
            }
            else if (field is NumberField)
            {
                storageField = new MongoNumberField();
                ((MongoNumberField)storageField).DefaultValue = ((NumberField)field).DefaultValue.Value;
                ((MongoNumberField)storageField).MinValue = ((NumberField)field).MinValue.Value;
                ((MongoNumberField)storageField).MaxValue = ((NumberField)field).MaxValue.Value;
                ((MongoNumberField)storageField).DecimalPlaces = ((NumberField)field).DecimalPlaces.Value;
            }
            else if (field is PasswordField)
            {
                storageField = new MongoPasswordField();
                ((MongoPasswordField)storageField).MaxLength = ((PasswordField)field).MaxLength.Value;
                ((MongoPasswordField)storageField).MaskType = ((PasswordField)field).MaskType;
                ((MongoPasswordField)storageField).MaskCharacter = ((PasswordField)field).MaskCharacter.Value;
            }
            else if (field is NumberField)
            {
                storageField = new MongoNumberField();
                ((MongoNumberField)storageField).DefaultValue = ((NumberField)field).DefaultValue.Value;
                ((MongoNumberField)storageField).MinValue = ((NumberField)field).MinValue.Value;
                ((MongoNumberField)storageField).MaxValue = ((NumberField)field).MaxValue.Value;
                ((MongoNumberField)storageField).DecimalPlaces = ((NumberField)field).DecimalPlaces.Value;
            }
            else if (field is PhoneField)
            {
                storageField = new MongoPhoneField();
                ((MongoPhoneField)storageField).DefaultValue = ((PhoneField)field).DefaultValue;
                ((MongoPhoneField)storageField).Format = ((PhoneField)field).Format;
                ((MongoPhoneField)storageField).MaxLength = ((PhoneField)field).MaxLength.Value;
            }
            else if (field is PrimaryKeyField)
            {
                storageField = new MongoPrimaryKeyField();
                ((MongoPrimaryKeyField)storageField).DefaultValue = ((PrimaryKeyField)field).DefaultValue.Value;
            }
            else if (field is SelectField)
            {
                storageField = new MongoSelectField();
                ((MongoSelectField)storageField).DefaultValue = ((SelectField)field).DefaultValue;
                ((MongoSelectField)storageField).Options = ((SelectField)field).Options;
            }
            else if (field is TextField)
            {
                storageField = new MongoTextField();
                ((MongoTextField)storageField).DefaultValue = ((TextField)field).DefaultValue;
                ((MongoTextField)storageField).MaxLength = ((TextField)field).MaxLength.Value;
            }
            else if (field is UrlField)
            {
                storageField = new MongoUrlField();
                ((MongoUrlField)storageField).DefaultValue = ((UrlField)field).DefaultValue;
                ((MongoUrlField)storageField).MaxLength = ((UrlField)field).MaxLength.Value;
                ((MongoUrlField)storageField).OpenTargetInNewWindow = ((UrlField)field).OpenTargetInNewWindow.Value;
            }

            storageField.Id = field.Id.Value;
            storageField.Name = field.Name;
            storageField.Label = field.Label;
            storageField.PlaceholderText = field.PlaceholderText;
            storageField.Description = field.Description;
            storageField.HelpText = field.HelpText;
            storageField.Required = field.Required.Value;
            storageField.Unique = field.Unique.Value;
            storageField.Searchable = field.Searchable.Value;
            storageField.Auditable = field.Auditable.Value;
            storageField.System = field.System.Value;

            return storageField;
        }
    }
}