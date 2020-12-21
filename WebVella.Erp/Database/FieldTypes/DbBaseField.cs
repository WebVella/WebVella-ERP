using Newtonsoft.Json;
using System;
using WebVella.Erp.Api.Models;


namespace WebVella.Erp.Database
{
	public abstract class DbBaseField
	{
		[JsonProperty(PropertyName = "id")]
		public Guid Id { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "label")]
		public string Label { get; set; }

		[JsonProperty(PropertyName = "placeholder_text")]
		public string PlaceholderText { get; set; }

		[JsonProperty(PropertyName = "description")]
		public string Description { get; set; }

		[JsonProperty(PropertyName = "help_text")]
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
		public DbFieldPermissions Permissions { get; set; }

		[JsonProperty(PropertyName = "enable_security")]
		public bool EnableSecurity { get; set; }

		internal object GetDefaultValue()
		{
			if (this is DbAutoNumberField)
				return ((DbAutoNumberField)this).DefaultValue;
			else if (this is DbCheckboxField)
				return ((DbCheckboxField)this).DefaultValue;
			else if (this is DbCurrencyField)
				return ((DbCurrencyField)this).DefaultValue;
			else if (this is DbDateField)
			{
				if (((DbDateField)this).UseCurrentTimeAsDefaultValue)
					return null;
				else
					return ((DbDateField)this).DefaultValue;
			}
			else if (this is DbDateTimeField)
			{
				if (((DbDateTimeField)this).UseCurrentTimeAsDefaultValue)
					return null;
				else
					return ((DbDateTimeField)this).DefaultValue;
			}
			else if (this is DbEmailField)
				return ((DbEmailField)this).DefaultValue;
			else if (this is DbFileField)
				//TODO convert file path to url path
				return ((DbFileField)this).DefaultValue;
			else if (this is DbImageField)
				//TODO convert file path to url path
				return ((DbImageField)this).DefaultValue;
			else if (this is DbHtmlField)
				return ((DbHtmlField)this).DefaultValue;
			else if (this is DbMultiLineTextField)
				return ((DbMultiLineTextField)this).DefaultValue;
			else if (this is DbMultiSelectField)
				return ((DbMultiSelectField)this).DefaultValue;
			else if (this is DbNumberField)
				return ((DbNumberField)this).DefaultValue;
			else if (this is DbPasswordField)
				return null;
			else if (this is DbPercentField)
				return ((DbPercentField)this).DefaultValue;
			else if (this is DbPhoneField)
				return ((DbPhoneField)this).DefaultValue;
			else if (this is DbGuidField)
			{
				if (Name == "id")
					return null; //throw new Exception("Cannot use default value for id this.");

				var guidField = (this as DbGuidField);
				if (guidField.GenerateNewId != null && guidField.GenerateNewId.Value)
					return Guid.NewGuid();

				return guidField.DefaultValue;
			}
			else if (this is DbSelectField)
				return ((DbSelectField)this).DefaultValue;
			else if (this is DbTextField)
				return ((DbTextField)this).DefaultValue;
			else if (this is DbUrlField)
				return ((DbUrlField)this).DefaultValue;

			return null;
		}

		public FieldType GetFieldType()
		{
			if (this is DbAutoNumberField)
				return FieldType.AutoNumberField;
			else if (this is DbCheckboxField)
				return FieldType.CheckboxField;
			else if (this is DbCurrencyField)
				return FieldType.CurrencyField;
			else if (this is DbDateField)
				return FieldType.DateField;
			else if (this is DbDateTimeField)
				return FieldType.DateTimeField;
			else if (this is DbEmailField)
				return FieldType.EmailField;
			else if (this is DbFileField)
				return FieldType.FileField;
			else if (this is DbImageField)
				return FieldType.ImageField;
			else if (this is DbHtmlField)
				return FieldType.HtmlField;
			else if (this is DbMultiLineTextField)
				return FieldType.MultiLineTextField;
			else if (this is DbGeographyField)
				return FieldType.GeographyField;
			else if (this is DbMultiSelectField)
				return FieldType.MultiSelectField;
			else if (this is DbNumberField)
				return FieldType.NumberField;
			else if (this is DbPasswordField)
				return FieldType.PasswordField;
			else if (this is DbPercentField)
				return FieldType.PercentField;
			else if (this is DbPhoneField)
				return FieldType.PhoneField;
			else if (this is DbGuidField)
				return FieldType.GuidField;
			else if (this is DbSelectField)
				return FieldType.SelectField;
			else if (this is DbTextField)
				return FieldType.TextField;
			else if (this is DbUrlField)
				return FieldType.UrlField;
			else if (this is DbGuidField)
				return FieldType.GuidField;

			throw new Exception("Unknown field type.");
		}
	}
}
