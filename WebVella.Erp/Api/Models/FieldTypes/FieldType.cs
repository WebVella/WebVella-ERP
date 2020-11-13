using System.ComponentModel;

namespace WebVella.Erp.Api.Models
{
	public enum FieldType
    {
		[SelectOption(Label = "autonumber")]
        AutoNumberField = 1,
		[SelectOption(Label = "checkbox")]
		CheckboxField = 2,
		[SelectOption(Label = "currency")]
		CurrencyField = 3,
		[SelectOption(Label = "date")]
		DateField = 4,
		[SelectOption(Label = "datetime")]
		DateTimeField = 5,
		[SelectOption(Label = "email")]
		EmailField = 6,
		[SelectOption(Label = "file")]
		FileField = 7,
		[SelectOption(Label = "html")]
		HtmlField = 8,
		[SelectOption(Label = "image")]
		ImageField = 9,
		[SelectOption(Label = "multilinetext")]
		MultiLineTextField = 10,
		[SelectOption(Label = "multiselect")]
		MultiSelectField = 11,
		[SelectOption(Label = "number")]
		NumberField = 12,
		[SelectOption(Label = "password")]
		PasswordField = 13,
		[SelectOption(Label = "percent")]
		PercentField = 14,
		[SelectOption(Label = "phone")]
		PhoneField = 15,
		[SelectOption(Label = "guid")]
		GuidField = 16,
		[SelectOption(Label = "select")]
		SelectField = 17,
		[SelectOption(Label = "text")]
		TextField = 18,
		[SelectOption(Label = "url")]
		UrlField = 19,
		[SelectOption(Label = "relation")]
		RelationField = 20,
		[SelectOption(Label = "geography")]
		GeographyField = 21
	}
}
