using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Builders;
using WebVella.ERP.Api.Models;

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
            storageEntity.IconName = entity.IconName;
            storageEntity.Weight = entity.Weight.Value;
            storageEntity.RecordPermissions = new MongoRecordPermissions();
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
                    ((MongoDateField)storageField).DefaultValue = ((DateField)field).DefaultValue;
                    ((MongoDateField)storageField).Format = ((DateField)field).Format;
                    ((MongoDateField)storageField).UseCurrentTimeAsDefaultValue = ((DateField)field).UseCurrentTimeAsDefaultValue.Value;
                }
                else if (field is DateTimeField)
                {
                    storageField = new MongoDateTimeField();
                    ((MongoDateTimeField)storageField).DefaultValue = ((DateTimeField)field).DefaultValue;
                    ((MongoDateTimeField)storageField).Format = ((DateTimeField)field).Format;
                    ((MongoDateTimeField)storageField).UseCurrentTimeAsDefaultValue = ((DateTimeField)field).UseCurrentTimeAsDefaultValue.Value;
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
                    ((MongoMultiSelectField)storageField).Options = new List<IStorageMultiSelectFieldOption>();
                    foreach (var option in ((MultiSelectField)field).Options)
                    {
                        MongoMultiSelectFieldOption storeOption = new MongoMultiSelectFieldOption();
                        storeOption.Key = option.Key;
                        storeOption.Value = option.Value;

                        ((MongoMultiSelectField)storageField).Options.Add(storeOption);
                    }
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
                else if (field is GuidField)
                {
                    storageField = new MongoGuidField();
                    ((MongoGuidField)storageField).DefaultValue = ((GuidField)field).DefaultValue.Value;
                }
                else if (field is SelectField)
                {
                    storageField = new MongoSelectField();
                    ((MongoSelectField)storageField).DefaultValue = ((SelectField)field).DefaultValue;
                    ((MongoSelectField)storageField).Options = new List<IStorageSelectFieldOption>();
                    foreach (var option in ((SelectField)field).Options)
                    {
                        MongoSelectFieldOption storeOption = new MongoSelectFieldOption();
                        storeOption.Key = option.Key;
                        storeOption.Value = option.Value;

                        ((MongoSelectField)storageField).Options.Add(storeOption);
                    }

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

            storageEntity.RecordsLists = new List<IStorageRecordsList>();

            foreach (RecordsList recordsList in entity.RecordsLists)
            {
                MongoRecordsList storageRecordsList = new MongoRecordsList();
                storageRecordsList.Id = recordsList.Id.Value;
                storageRecordsList.Name = recordsList.Name;
                storageRecordsList.Label = recordsList.Label;
                storageRecordsList.Type = recordsList.Type;

                storageRecordsList.Filters = new List<IStorageRecordsListFilter>();

                foreach (RecordsListFilter filter in recordsList.Filters)
                {
                    MongoRecordsListFilter storageFilter = new MongoRecordsListFilter();

                    storageFilter.EntityId = filter.EntityId.Value;
                    storageFilter.FieldId = filter.FieldId.Value;
                    storageFilter.Operator = filter.Operator;
                    storageFilter.Value = filter.Value;

                    storageRecordsList.Filters.Add(storageFilter);
                }

                storageRecordsList.Fields = new List<IStorageRecordsListField>();

                foreach (RecordsListField field in recordsList.Fields)
                {
                    MongoRecordsListField storageField = new MongoRecordsListField();

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
                MongoRecordView storageRecordView = new MongoRecordView();
                storageRecordView.Id = recordView.Id.Value;
                storageRecordView.Name = recordView.Name;
                storageRecordView.Label = recordView.Label;

                storageRecordView.Fields = new List<IStorageRecordViewField>();

                foreach (RecordViewField field in recordView.Fields)
                {
                    MongoRecordViewField storageField = new MongoRecordViewField();

                    storageField.EntityId = field.EntityId.Value;
                    storageField.Id = field.Id.Value;
                    storageField.Position = field.Position.Value;

                    storageRecordView.Fields.Add(storageField);
                }

                storageEntity.RecordViewList.Add(storageRecordView);
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
                ((MongoDateField)storageField).DefaultValue = ((DateField)field).DefaultValue;
                ((MongoDateField)storageField).Format = ((DateField)field).Format;
                ((MongoDateField)storageField).UseCurrentTimeAsDefaultValue = ((DateField)field).UseCurrentTimeAsDefaultValue.Value;
            }
            else if (field is DateTimeField)
            {
                storageField = new MongoDateTimeField();
                ((MongoDateTimeField)storageField).DefaultValue = ((DateTimeField)field).DefaultValue;
                ((MongoDateTimeField)storageField).Format = ((DateTimeField)field).Format;
                ((MongoDateTimeField)storageField).UseCurrentTimeAsDefaultValue = ((DateTimeField)field).UseCurrentTimeAsDefaultValue.Value;
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
                ((MongoMultiSelectField)storageField).Options = new List<IStorageMultiSelectFieldOption>();
                foreach (var option in ((MultiSelectField)field).Options)
                {
                    MongoMultiSelectFieldOption storeOption = new MongoMultiSelectFieldOption();
                    storeOption.Key = option.Key;
                    storeOption.Value = option.Value;

                    ((MongoMultiSelectField)storageField).Options.Add(storeOption);
                }
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
            else if (field is GuidField)
            {
                storageField = new MongoGuidField();
                ((MongoGuidField)storageField).DefaultValue = ((GuidField)field).DefaultValue.Value;
            }
            else if (field is SelectField)
            {
                storageField = new MongoSelectField();
                ((MongoSelectField)storageField).DefaultValue = ((SelectField)field).DefaultValue;
                ((MongoSelectField)storageField).Options = new List<IStorageSelectFieldOption>();
                foreach (var option in ((SelectField)field).Options)
                {
                    MongoSelectFieldOption storeOption = new MongoSelectFieldOption();
                    storeOption.Key = option.Key;
                    storeOption.Value = option.Value;

                    ((MongoSelectField)storageField).Options.Add(storeOption);
                }
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