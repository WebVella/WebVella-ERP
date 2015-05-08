using System;
using System.Collections.Generic;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Storage;

namespace WebVella.ERP
{
    public abstract class Field
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string Label { get; set; }

        public string PlaceholderText { get; set; }

        public string Description { get; set; }

        public string HelpText { get; set; }

        public bool? Required { get; set; }

        public bool? Unique { get; set; }

        public bool? Searchable { get; set; }

        public bool? Auditable { get; set; }

        public bool? System { get; set; }

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
            }
            else if (storageField is IStorageDateTimeField)
            {
                field = new DateTimeField();
                ((DateTimeField)field).DefaultValue = ((IStorageDateTimeField)storageField).DefaultValue;
                ((DateTimeField)field).Format = ((IStorageDateTimeField)storageField).Format;
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
            else if (storageField is IStorageMasterDetailsRelationshipField)
            {
                field = new MasterDetailsRelationshipField();
                ((MasterDetailsRelationshipField)field).RelatedEntityId = ((IStorageMasterDetailsRelationshipField)storageField).RelatedEntityId;
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
    }

    public class FieldList
    {
        public Guid Offset { get; set; }

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
        public Field Object { get; set; }
    }

    public class FieldListResponse : BaseResponseModel
    {
        public FieldList Object { get; set; }
    }
}