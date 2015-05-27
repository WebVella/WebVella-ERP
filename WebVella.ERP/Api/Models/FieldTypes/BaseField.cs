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

		public static InputField ConvertField(Field field)
		{
			InputField inputField = null;

			if (field is AutoNumberField)
			{
				inputField = new InputAutoNumberField();
				((InputAutoNumberField)inputField).DefaultValue = ((AutoNumberField)field).DefaultValue;
				((InputAutoNumberField)inputField).DisplayFormat = ((AutoNumberField)field).DisplayFormat;
				((InputAutoNumberField)inputField).StartingNumber = ((AutoNumberField)field).StartingNumber;
			}
			else if (field is CheckboxField)
			{
				inputField = new InputCheckboxField();
				((InputCheckboxField)inputField).DefaultValue = ((CheckboxField)field).DefaultValue.Value;
			}
			else if (field is CurrencyField)
			{
				inputField = new InputCurrencyField();
				((InputCurrencyField)inputField).DefaultValue = ((CurrencyField)field).DefaultValue;
				((InputCurrencyField)inputField).MinValue = ((CurrencyField)field).MinValue;
				((InputCurrencyField)inputField).MaxValue = ((CurrencyField)field).MaxValue;
				((InputCurrencyField)inputField).Currency = ((CurrencyField)field).Currency;
			}
			else if (field is DateField)
			{
				inputField = new InputDateField();
				((InputDateField)inputField).DefaultValue = ((DateField)field).DefaultValue;
				((InputDateField)inputField).Format = ((DateField)field).Format;
				((InputDateField)inputField).UseCurrentTimeAsDefaultValue = ((DateField)field).UseCurrentTimeAsDefaultValue.Value;
			}
			else if (field is DateTimeField)
			{
				inputField = new InputDateTimeField();
				((InputDateTimeField)inputField).DefaultValue = ((DateTimeField)field).DefaultValue;
				((InputDateTimeField)inputField).Format = ((DateTimeField)field).Format;
				((InputDateTimeField)inputField).UseCurrentTimeAsDefaultValue = ((DateTimeField)field).UseCurrentTimeAsDefaultValue.Value;
			}
			else if (field is EmailField)
			{
				inputField = new InputEmailField();
				((InputEmailField)inputField).DefaultValue = ((EmailField)field).DefaultValue;
				((InputEmailField)inputField).MaxLength = ((EmailField)field).MaxLength;
			}
			else if (field is FileField)
			{
				inputField = new InputFileField();
				((InputFileField)inputField).DefaultValue = ((FileField)field).DefaultValue;
			}
			//else if (field is FormulaField)
			//{
			//    inputField = new MongoFormulaField();
			//    ((MongoFormulaField)inputField).ReturnType = ((FormulaField)field).ReturnType;
			//    ((MongoFormulaField)inputField).FormulaText = ((FormulaField)field).FormulaText;
			//    ((MongoFormulaField)inputField).DecimalPlaces = ((FormulaField)field).DecimalPlaces;
			//}
			else if (field is HtmlField)
			{
				inputField = new InputHtmlField();
				((InputHtmlField)inputField).DefaultValue = ((HtmlField)field).DefaultValue;
			}
			else if (field is ImageField)
			{
				inputField = new InputImageField();
				((InputImageField)inputField).DefaultValue = ((ImageField)field).DefaultValue;
			}
			else if (field is MultiLineTextField)
			{
				inputField = new InputMultiLineTextField();
				((InputMultiLineTextField)inputField).DefaultValue = ((MultiLineTextField)field).DefaultValue;
				((InputMultiLineTextField)inputField).MaxLength = ((MultiLineTextField)field).MaxLength;
				((InputMultiLineTextField)inputField).VisibleLineNumber = ((MultiLineTextField)field).VisibleLineNumber;
			}
			else if (field is MultiSelectField)
			{
				inputField = new InputMultiSelectField();
				((InputMultiSelectField)inputField).DefaultValue = ((MultiSelectField)field).DefaultValue;
				((InputMultiSelectField)inputField).Options = ((MultiSelectField)field).Options;
			}
			else if (field is NumberField)
			{
				inputField = new InputNumberField();
				((InputNumberField)inputField).DefaultValue = ((NumberField)field).DefaultValue;
				((InputNumberField)inputField).MinValue = ((NumberField)field).MinValue;
				((InputNumberField)inputField).MaxValue = ((NumberField)field).MaxValue;
				((InputNumberField)inputField).DecimalPlaces = ((NumberField)field).DecimalPlaces.Value;
			}
			else if (field is PasswordField)
			{
				inputField = new InputPasswordField();
				((InputPasswordField)inputField).MaxLength = ((PasswordField)field).MaxLength;
				((InputPasswordField)inputField).MinLength = ((PasswordField)field).MinLength;
				((InputPasswordField)inputField).Encrypted = ((PasswordField)field).Encrypted ?? true;
			}
			else if (field is PercentField)
			{
				inputField = new InputPercentField();
				((InputPercentField)inputField).DefaultValue = ((PercentField)field).DefaultValue;
				((InputPercentField)inputField).MinValue = ((PercentField)field).MinValue;
				((InputPercentField)inputField).MaxValue = ((PercentField)field).MaxValue;
				((InputPercentField)inputField).DecimalPlaces = ((PercentField)field).DecimalPlaces.Value;
			}
			else if (field is PhoneField)
			{
				inputField = new InputPhoneField();
				((InputPhoneField)inputField).DefaultValue = ((PhoneField)field).DefaultValue;
				((InputPhoneField)inputField).Format = ((PhoneField)field).Format;
				((InputPhoneField)inputField).MaxLength = ((PhoneField)field).MaxLength;
			}
			else if (field is GuidField)
			{
				inputField = new InputGuidField();
				((InputGuidField)inputField).DefaultValue = ((GuidField)field).DefaultValue;
				((InputGuidField)inputField).GenerateNewId = ((GuidField)field).GenerateNewId;
			}
			else if (field is SelectField)
			{
				inputField = new InputSelectField();
				((InputSelectField)inputField).DefaultValue = ((SelectField)field).DefaultValue;
				((InputSelectField)inputField).Options = ((SelectField)field).Options;

			}
			else if (field is TextField)
			{
				inputField = new InputTextField();
				((InputTextField)inputField).DefaultValue = ((TextField)field).DefaultValue;
				((InputTextField)inputField).MaxLength = ((TextField)field).MaxLength;
			}
			else if (field is UrlField)
			{
				inputField = new InputUrlField();
				((InputUrlField)inputField).DefaultValue = ((UrlField)field).DefaultValue;
				((InputUrlField)inputField).MaxLength = ((UrlField)field).MaxLength;
				((InputUrlField)inputField).OpenTargetInNewWindow = ((UrlField)field).OpenTargetInNewWindow.Value;
			}

			inputField.Id = field.Id;
			inputField.Name = field.Name;
			inputField.Label = field.Label;
			inputField.PlaceholderText = field.PlaceholderText;
			inputField.Description = field.Description;
			inputField.HelpText = field.HelpText;
			inputField.Required = field.Required;
			inputField.Unique = field.Unique;
			inputField.Searchable = field.Searchable;
			inputField.Auditable = field.Auditable;
			inputField.System = field.System;

			return inputField;
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

		public static Field ConvertField(InputField inputField)
		{
			Field field = null;

			if (inputField is InputAutoNumberField)
			{
				field = new AutoNumberField();
				((AutoNumberField)field).DefaultValue = ((InputAutoNumberField)inputField).DefaultValue;
				((AutoNumberField)field).DisplayFormat = ((InputAutoNumberField)inputField).DisplayFormat;
				((AutoNumberField)field).StartingNumber = ((InputAutoNumberField)inputField).StartingNumber;
			}
			else if (inputField is InputCheckboxField)
			{
				field = new CheckboxField();
				((CheckboxField)field).DefaultValue = ((InputCheckboxField)inputField).DefaultValue;
			}
			else if (inputField is InputCurrencyField)
			{
				field = new CurrencyField();
				((CurrencyField)field).DefaultValue = ((InputCurrencyField)inputField).DefaultValue;
				((CurrencyField)field).MinValue = ((InputCurrencyField)inputField).MinValue;
				((CurrencyField)field).MaxValue = ((InputCurrencyField)inputField).MaxValue;
				((CurrencyField)field).Currency = ((InputCurrencyField)inputField).Currency;
			}
			else if (inputField is InputDateField)
			{
				field = new DateField();
				((DateField)field).DefaultValue = ((InputDateField)inputField).DefaultValue;
				((DateField)field).Format = ((InputDateField)inputField).Format;
				((DateField)field).UseCurrentTimeAsDefaultValue = ((InputDateField)inputField).UseCurrentTimeAsDefaultValue;
			}
			else if (inputField is InputDateTimeField)
			{
				field = new DateTimeField();
				((DateTimeField)field).DefaultValue = ((InputDateTimeField)inputField).DefaultValue;
				((DateTimeField)field).Format = ((InputDateTimeField)inputField).Format;
				((DateTimeField)field).UseCurrentTimeAsDefaultValue = ((InputDateTimeField)inputField).UseCurrentTimeAsDefaultValue;
			}
			else if (inputField is InputEmailField)
			{
				field = new EmailField();
				((EmailField)field).DefaultValue = ((InputEmailField)inputField).DefaultValue;
				((EmailField)field).MaxLength = ((InputEmailField)inputField).MaxLength;
			}
			else if (inputField is InputFileField)
			{
				field = new FileField();
				((FileField)field).DefaultValue = ((InputFileField)inputField).DefaultValue;
			}
			//else if (inputField is InputFormulaField)
			//{
			//    field = new FormulaField();
			//    ((FormulaField)field).ReturnType = ((InputFormulaField)inputField).ReturnType;
			//    ((FormulaField)field).FormulaText = ((InputFormulaField)inputField).FormulaText;
			//    ((FormulaField)field).DecimalPlaces = ((InputFormulaField)inputField).DecimalPlaces;
			//}
			else if (inputField is InputHtmlField)
			{
				field = new HtmlField();
				((HtmlField)field).DefaultValue = ((InputHtmlField)inputField).DefaultValue;
			}
			else if (inputField is InputImageField)
			{
				field = new ImageField();
				((ImageField)field).DefaultValue = ((InputImageField)inputField).DefaultValue;
			}
			else if (inputField is InputImageField)
			{
				field = new ImageField();
				((ImageField)field).DefaultValue = ((InputImageField)inputField).DefaultValue;
			}
			else if (inputField is InputMultiLineTextField)
			{
				field = new MultiLineTextField();
				((MultiLineTextField)field).DefaultValue = ((InputMultiLineTextField)inputField).DefaultValue;
				((MultiLineTextField)field).MaxLength = ((InputMultiLineTextField)inputField).MaxLength;
				((MultiLineTextField)field).VisibleLineNumber = ((InputMultiLineTextField)inputField).VisibleLineNumber;
			}
			else if (inputField is InputMultiSelectField)
			{
				field = new MultiSelectField();
				((MultiSelectField)field).DefaultValue = ((InputMultiSelectField)inputField).DefaultValue;
				((MultiSelectField)field).Options = ((InputMultiSelectField)inputField).Options;
			}
			else if (inputField is InputNumberField)
			{
				field = new NumberField();
				((NumberField)field).DefaultValue = ((InputNumberField)inputField).DefaultValue;
				((NumberField)field).MinValue = ((InputNumberField)inputField).MinValue;
				((NumberField)field).MaxValue = ((InputNumberField)inputField).MaxValue;
				((NumberField)field).DecimalPlaces = ((InputNumberField)inputField).DecimalPlaces;
			}
			else if (inputField is InputPasswordField)
			{
				field = new PasswordField();
				((PasswordField)field).Encrypted = ((InputPasswordField)inputField).Encrypted;
				((PasswordField)field).MaxLength = ((InputPasswordField)inputField).MaxLength;
				((PasswordField)field).MinLength = ((InputPasswordField)inputField).MinLength;
			}
			else if (inputField is InputPercentField)
			{
				field = new PercentField();
				((PercentField)field).DefaultValue = ((InputPercentField)inputField).DefaultValue;
				((PercentField)field).MinValue = ((InputPercentField)inputField).MinValue;
				((PercentField)field).MaxValue = ((InputPercentField)inputField).MaxValue;
				((PercentField)field).DecimalPlaces = ((InputPercentField)inputField).DecimalPlaces;
			}
			else if (inputField is InputPhoneField)
			{
				field = new PhoneField();
				((PhoneField)field).DefaultValue = ((InputPhoneField)inputField).DefaultValue;
				((PhoneField)field).Format = ((InputPhoneField)inputField).Format;
				((PhoneField)field).MaxLength = ((InputPhoneField)inputField).MaxLength;
			}
			else if (inputField is InputGuidField)
			{
				field = new GuidField();
				((GuidField)field).DefaultValue = ((InputGuidField)inputField).DefaultValue;
				((GuidField)field).GenerateNewId = ((InputGuidField)inputField).GenerateNewId;
			}
			else if (inputField is InputSelectField)
			{
				field = new SelectField();
				((SelectField)field).DefaultValue = ((InputSelectField)inputField).DefaultValue;
				((SelectField)field).Options = ((InputSelectField)inputField).Options;
			}
			else if (inputField is InputTextField)
			{
				field = new TextField();
				((TextField)field).DefaultValue = ((InputTextField)inputField).DefaultValue;
				((TextField)field).MaxLength = ((InputTextField)inputField).MaxLength;
			}
			else if (inputField is InputUrlField)
			{
				field = new UrlField();
				((UrlField)field).DefaultValue = ((InputUrlField)inputField).DefaultValue;
				((UrlField)field).MaxLength = ((InputUrlField)inputField).MaxLength;
				((UrlField)field).OpenTargetInNewWindow = ((InputUrlField)inputField).OpenTargetInNewWindow;
			}

			field.Id = inputField.Id.HasValue ? inputField.Id.Value : Guid.Empty;
			field.Name = inputField.Name;
			field.Label = inputField.Label;
			field.PlaceholderText = inputField.PlaceholderText;
			field.Description = inputField.Description;
			field.HelpText = inputField.HelpText;
			field.Required = inputField.Required.HasValue && inputField.Required.Value;
			field.Unique = inputField.Unique.HasValue && inputField.Unique.Value;
			field.Searchable = inputField.Searchable.HasValue && inputField.Searchable.Value;
			field.Auditable = inputField.Auditable.HasValue && inputField.Auditable.Value;
			field.System = inputField.System.HasValue && inputField.System.Value;

			return field;
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