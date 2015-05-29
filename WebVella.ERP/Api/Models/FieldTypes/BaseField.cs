using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP
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

		public InputField()
		{
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
			}

			return field;
		}
			}

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

		public Field()
		{
			Required = false;
			Unique = false;
			Searchable = false;
			Auditable = false;
			System = false;
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

		internal object GetDefaultValue()
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
					return DateTime.UtcNow.Date;
				else
					return ((DateField)this).DefaultValue;
			}
			else if (this is DateTimeField)
			{
				if (((DateTimeField)this).UseCurrentTimeAsDefaultValue.Value)
					return DateTime.UtcNow;
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
					throw new Exception("Cannot use default value for id this.");

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
	}

	public class FieldList
	{
		[JsonProperty(PropertyName = "fields")]
		public List<Field> Fields { get; set; }

		public FieldList()
		{
			Fields = new List<Field>();
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