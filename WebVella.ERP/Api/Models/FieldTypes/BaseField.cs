using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Storage;
using WebVella.ERP.Utilities.Dynamic;

namespace WebVella.ERP
{
    public class InputField : Expando
    {
    }

    public abstract class Field
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "placeholderText")]
        public string PlaceholderText { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "helpText")]
        public string HelpText { get; set; }

        [JsonProperty(PropertyName = "required")]
        public bool? Required { get; set; }

        [JsonProperty(PropertyName = "unique")]
        public bool? Unique { get; set; }

        [JsonProperty(PropertyName = "searchable")]
        public bool? Searchable { get; set; }

        [JsonProperty(PropertyName = "auditable")]
        public bool? Auditable { get; set; }

        [JsonProperty(PropertyName = "system")]
        public bool? System { get; set; }

        public Field()
        {

        }

        public Field(Field field)
        {
            Id = field.Id;
            Name = field.Name;
            Label = field.Label;
            PlaceholderText = field.PlaceholderText;
            Description = field.Description;
            HelpText = field.HelpText;
            Required = field.Required;
            Unique = field.Unique;
            Searchable = field.Searchable;
            Auditable = field.Auditable;
            System = field.System;
        }

        public Field(InputField field)
        {
            Id = (Guid)field["id"];
            Name = (string)field["name"];
            Label = (string)field["label"];
            PlaceholderText = (string)field["placeholderText"];
            Description = (string)field["description"];
            HelpText = (string)field["helpText"];
            Required = (bool?)field["required"];
            Unique = (bool?)field["unique"];
            Searchable = (bool?)field["searchable"];
            Auditable = (bool?)field["auditable"];
            System = (bool?)field["system"];
        }

        public static Field Convert(IStorageField storageField)
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
            else if (storageField is IStorageLookupRelationField)
            {
                field = new LookupRelationField();
                ((LookupRelationField)field).RelatedEntityId = ((IStorageLookupRelationField)storageField).RelatedEntityId;
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
                ((MultiSelectField)field).Options = ((IStorageMultiSelectField)storageField).Options;
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
                ((PasswordField)field).MaxLength = ((IStoragePasswordField)storageField).MaxLength;
                ((PasswordField)field).MaskType = ((IStoragePasswordField)storageField).MaskType;
                ((PasswordField)field).MaskCharacter = ((IStoragePasswordField)storageField).MaskCharacter;
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
            else if (storageField is IStoragePrimaryKeyField)
            {
                field = new PrimaryKeyField();
                ((PrimaryKeyField)field).DefaultValue = ((IStoragePrimaryKeyField)storageField).DefaultValue;
            }
            else if (storageField is IStorageSelectField)
            {
                field = new SelectField();
                ((SelectField)field).DefaultValue = ((IStorageSelectField)storageField).DefaultValue;
                ((SelectField)field).Options = ((IStorageSelectField)storageField).Options;
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

        public static Field Convert(InputField inputField)
        {
            Field field = null;

            FieldType fieldType = (FieldType)inputField["fieldType"];
            switch (fieldType)
            {
                case FieldType.AutoNumberField:
                    field = new AutoNumberField(inputField);
                    break;
                case FieldType.CheckboxField:
                    field = new CheckboxField(inputField);
                    break;
                case FieldType.CurrencyField:
                    field = new CurrencyField(inputField);
                    break;
                case FieldType.DateField:
                    field = new DateField(inputField);
                    break;
                case FieldType.DateTimeField:
                    field = new DateTimeField(inputField);
                    break;
                case FieldType.EmailField:
                    field = new EmailField(inputField);
                    break;
                case FieldType.FileField:
                    field = new FileField(inputField);
                    break;
                case FieldType.HtmlField:
                    field = new HtmlField(inputField);
                    break;
                case FieldType.ImageField:
                    field = new ImageField(inputField);
                    break;
                case FieldType.LookupRelationField:
                    field = new LookupRelationField(inputField);
                    break;
                case FieldType.MultiLineTextField:
                    field = new MultiLineTextField(inputField);
                    break;
                case FieldType.MultiSelectField:
                    field = new MultiSelectField(inputField);
                    break;
                case FieldType.NumberField:
                    field = new NumberField(inputField);
                    break;
                case FieldType.PasswordField:
                    field = new PasswordField(inputField);
                    break;
                case FieldType.PercentField:
                    field = new PercentField(inputField);
                    break;
                case FieldType.PhoneField:
                    field = new PhoneField(inputField);
                    break;
                case FieldType.PrimaryKeyField:
                    field = new PrimaryKeyField(inputField);
                    break;
                case FieldType.SelectField:
                    field = new SelectField(inputField);
                    break;
                case FieldType.TextField:
                    field = new TextField(inputField);
                    break;
                case FieldType.UrlField:
                    field = new UrlField(inputField);
                    break;
            }

            return field;
        }
    }

    public class FieldList
    {
        [JsonProperty(PropertyName = "offset")]
        public Guid Offset { get; set; }

        [JsonProperty(PropertyName = "fields")]
        public List<Field> Fields { get; set; }

        public FieldList()
        {
            Fields = new List<Field>();
        }

        public FieldList(List<IStorageField> fields)
        {
            Fields = new List<Field>();

            foreach (IStorageField storageField in fields)
            {
                Fields.Add(Field.Convert(storageField));
            }
        }
    }

    public class FieldResponse : BaseResponseModel
    {
        [JsonProperty(PropertyName = "object")]
        public Field Object { get; set; }
    }

    public class FieldListResponse : BaseResponseModel
    {
        [JsonProperty(PropertyName = "object")]
        public FieldList Object { get; set; }
    }
}