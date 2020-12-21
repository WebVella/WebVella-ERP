using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp
{
    public abstract class InputField
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

        [JsonProperty(PropertyName = "permissions")]
        public FieldPermissions Permissions { get; set; }

        [JsonProperty(PropertyName = "enableSecurity")]
        public bool EnableSecurity { get; set; }

        public InputField()
        {
            Permissions = new FieldPermissions();
        }

        public InputField(InputField field)
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
            Permissions = field.Permissions;
            EnableSecurity = field.EnableSecurity;
        }

        public static InputField ConvertField(JObject inputField)
        {
            InputField field = null;

            var fieldTypeProp = inputField.Properties().SingleOrDefault(k => k.Name.ToLower() == "fieldtype");
            if (fieldTypeProp == null)
                return field;

            FieldType fieldType = (FieldType)Enum.ToObject(typeof(FieldType), fieldTypeProp.Value.ToObject<int>());

            switch (fieldType)
            {
                case FieldType.AutoNumberField:
                    field = inputField.ToObject<InputAutoNumberField>();
                    break;
                case FieldType.CheckboxField:
                    field = inputField.ToObject<InputCheckboxField>();
                    break;
                case FieldType.CurrencyField:
                    field = inputField.ToObject<InputCurrencyField>();
                    break;
                case FieldType.DateField:
                    field = inputField.ToObject<InputDateField>();
                    break;
                case FieldType.DateTimeField:
                    field = inputField.ToObject<InputDateTimeField>();
                    break;
                case FieldType.EmailField:
                    field = inputField.ToObject<InputEmailField>();
                    break;
                case FieldType.FileField:
                    field = inputField.ToObject<InputFileField>();
                    break;
                case FieldType.HtmlField:
                    field = inputField.ToObject<InputHtmlField>();
                    break;
                case FieldType.ImageField:
                    field = inputField.ToObject<InputImageField>();
                    break;
                case FieldType.MultiLineTextField:
                    field = inputField.ToObject<InputMultiLineTextField>();
                    break;
				case FieldType.GeographyField:
					field = inputField.ToObject<InputGeographyField>();
					break;
				case FieldType.MultiSelectField:
                    field = inputField.ToObject<InputMultiSelectField>();
                    break;
                case FieldType.NumberField:
                    field = inputField.ToObject<InputNumberField>();
                    break;
                case FieldType.PasswordField:
                    field = inputField.ToObject<InputPasswordField>();
                    break;
                case FieldType.PercentField:
                    field = inputField.ToObject<InputPercentField>();
                    break;
                case FieldType.PhoneField:
                    field = inputField.ToObject<InputPhoneField>();
                    break;
                case FieldType.GuidField:
                    field = inputField.ToObject<InputGuidField>();
                    break;
                case FieldType.SelectField:
                    field = inputField.ToObject<InputSelectField>();
                    break;
                case FieldType.TextField:
                    field = inputField.ToObject<InputTextField>();
                    break;
                case FieldType.UrlField:
                    field = inputField.ToObject<InputUrlField>();
                    break;
				default:
					throw new Exception("Invalid field type.");
            }

            return field;
        }

        public static Type GetFieldType(FieldType fieldType)
        {
            Type type = typeof(InputGuidField);

            switch (fieldType)
            {
                case FieldType.AutoNumberField:
                    type = typeof(InputAutoNumberField);
                    break;
                case FieldType.CheckboxField:
                    type = typeof(InputCheckboxField);
                    break;
                case FieldType.CurrencyField:
                    type = typeof(InputCurrencyField);
                    break;
                case FieldType.DateField:
                    type = typeof(InputDateField);
                    break;
                case FieldType.DateTimeField:
                    type = typeof(InputDateTimeField);
                    break;
                case FieldType.EmailField:
                    type = typeof(InputEmailField);
                    break;
                case FieldType.FileField:
                    type = typeof(InputFileField);
                    break;
                case FieldType.HtmlField:
                    type = typeof(InputHtmlField);
                    break;
                case FieldType.ImageField:
                    type = typeof(InputImageField);
                    break;
                case FieldType.MultiLineTextField:
                    type = typeof(InputMultiLineTextField);
                    break;
				case FieldType.GeographyField:
					type = typeof(GeographyField);
					break;
				case FieldType.MultiSelectField:
                    type = typeof(InputMultiSelectField);
                    break;
                case FieldType.NumberField:
                    type = typeof(InputNumberField);
                    break;
                case FieldType.PasswordField:
                    type = typeof(InputPasswordField);
                    break;
                case FieldType.PercentField:
                    type = typeof(InputPercentField);
                    break;
                case FieldType.PhoneField:
                    type = typeof(InputPhoneField);
                    break;
                case FieldType.GuidField:
                    type = typeof(InputGuidField);
                    break;
                case FieldType.SelectField:
                    type = typeof(InputSelectField);
                    break;
                case FieldType.TextField:
                    type = typeof(InputTextField);
                    break;
                case FieldType.UrlField:
                    type = typeof(InputUrlField);
                    break;
				default:
					throw new Exception("Invalid field type.");
			}

            return type;
        }
    }

	[Serializable]
	public abstract class Field
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

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
        public bool Required { get; set; }

        [JsonProperty(PropertyName = "unique")]
        public bool Unique { get; set; }

        [JsonProperty(PropertyName = "searchable")]
        public bool Searchable { get; set; }

        [JsonProperty(PropertyName = "auditable")]
        public bool Auditable { get; set; }

        [JsonProperty(PropertyName = "system")]
        public bool System { get; set; }

        [JsonProperty(PropertyName = "permissions")]
        public FieldPermissions Permissions { get; set; }

        [JsonProperty(PropertyName = "enableSecurity")]
        public bool EnableSecurity { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

        public Field()
        {
            Required = false;
            Unique = false;
            Searchable = false;
            Auditable = false;
            System = false;
            Permissions = null;
            EnableSecurity = false;
            Permissions = new FieldPermissions();
        }

        public Field(Field field) : this()
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
            Permissions = field.Permissions;
            EnableSecurity = field.EnableSecurity;
            EntityName = field.EntityName;
        }

        public object GetFieldDefaultValue()
        {
            if (this is AutoNumberField)
                return ((AutoNumberField)this).DefaultValue;
            else if (this is CheckboxField)
                return ((CheckboxField)this).DefaultValue;
            else if (this is CurrencyField)
                return ((CurrencyField)this).DefaultValue;
            else if (this is DateField)
            {
                if (((DateField)this).UseCurrentTimeAsDefaultValue.Value)
                    return DateTime.Now.Date;
                else
                    return ((DateField)this).DefaultValue;
            }
            else if (this is DateTimeField)
            {
                if (((DateTimeField)this).UseCurrentTimeAsDefaultValue.Value)
                    return DateTime.Now;
                else
                    return ((DateTimeField)this).DefaultValue;
            }
            else if (this is EmailField)
                return ((EmailField)this).DefaultValue;
            else if (this is FileField)
                //TODO convert file path to url path
                return ((FileField)this).DefaultValue;
            else if (this is ImageField)
                //TODO convert file path to url path
                return ((ImageField)this).DefaultValue;
			else if (this is GeographyField)
				return ((GeographyField)this).DefaultValue;
			else if (this is HtmlField)
                return ((HtmlField)this).DefaultValue;
            else if (this is MultiLineTextField)
                return ((MultiLineTextField)this).DefaultValue;
            else if (this is MultiSelectField)
                return ((MultiSelectField)this).DefaultValue;
            else if (this is NumberField)
                return ((NumberField)this).DefaultValue;
            else if (this is PasswordField)
                return null;
            else if (this is PercentField)
                return ((PercentField)this).DefaultValue;
            else if (this is PhoneField)
                return ((PhoneField)this).DefaultValue;
            else if (this is GuidField)
            {
				if (Name == "id")
					return null; //throw new Exception("Cannot use default value for id this.");

                var guidField = (this as GuidField);
                if (guidField.GenerateNewId != null && guidField.GenerateNewId.Value)
                    return Guid.NewGuid();

                return guidField.DefaultValue;
            }
            else if (this is SelectField)
                return ((SelectField)this).DefaultValue;
            else if (this is TextField)
                return ((TextField)this).DefaultValue;
            else if (this is UrlField)
                return ((UrlField)this).DefaultValue;

            return null;
        }

        public FieldType GetFieldType()
        {
            if (this is AutoNumberField)
                return FieldType.AutoNumberField;
            else if (this is CheckboxField)
                return FieldType.CheckboxField;
            else if (this is CurrencyField)
                return FieldType.CurrencyField;
            else if (this is DateField)
                return FieldType.DateField;
            else if (this is DateTimeField)
                return FieldType.DateTimeField;
            else if (this is EmailField)
                return FieldType.EmailField;
            else if (this is FileField)
                return FieldType.FileField;
            else if (this is ImageField)
                return FieldType.ImageField;
            else if (this is HtmlField)
                return FieldType.HtmlField;
            else if (this is MultiLineTextField)
                return FieldType.MultiLineTextField;
			else if (this is GeographyField)
				return FieldType.GeographyField;
			else if (this is MultiSelectField)
                return FieldType.MultiSelectField;
            else if (this is NumberField)
                return FieldType.NumberField;
            else if (this is PasswordField)
                return FieldType.PasswordField;
            else if (this is PercentField)
                return FieldType.PercentField;
            else if (this is PhoneField)
                return FieldType.PhoneField;
            else if (this is GuidField)
                return FieldType.GuidField;
            else if (this is SelectField)
                return FieldType.SelectField;
            else if (this is TextField)
                return FieldType.TextField;
            else if (this is UrlField)
                return FieldType.UrlField;

            return FieldType.GuidField;
        }
    }

	[Serializable]
	public class FieldList
    {
        [JsonProperty(PropertyName = "fields")]
        public List<Field> Fields { get; set; }

        public FieldList()
        {
            Fields = new List<Field>();
        }
    }

	[Serializable]
	public class FieldResponse : BaseResponseModel
    {
        [JsonProperty(PropertyName = "object")]
        public Field Object { get; set; }
    }

	[Serializable]
	public class FieldListResponse : BaseResponseModel
    {
        [JsonProperty(PropertyName = "object")]
        public FieldList Object { get; set; }
    }

	[Serializable]
	public class FieldPermissions
    {
        [JsonProperty(PropertyName = "canRead")]
        public List<Guid> CanRead { get; set; }

        [JsonProperty(PropertyName = "canUpdate")]
        public List<Guid> CanUpdate { get; set; }

        public FieldPermissions()
        {
            CanRead = new List<Guid>();
            CanUpdate = new List<Guid>();
        }
    }
}
