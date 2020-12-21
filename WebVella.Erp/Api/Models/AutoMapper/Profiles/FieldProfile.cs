using AutoMapper;
using System;
using WebVella.Erp.Database;
using WebVella.Erp.Storage;

namespace WebVella.Erp.Api.Models.AutoMapper.Profiles
{
	public class FieldProfile : Profile
	{
		public FieldProfile()
		{
			CreateMap<Field, InputField>()
				.Include<AutoNumberField, InputAutoNumberField>()
				.Include<CheckboxField, InputCheckboxField>()
				.Include<CurrencyField, InputCurrencyField>()
				.Include<DateField, InputDateField>()
				.Include<DateTimeField, InputDateTimeField>()
				.Include<EmailField, InputEmailField>()
				.Include<FileField, InputFileField>()
				.Include<GeographyField, InputGeographyField>()
				.Include<GuidField, InputGuidField>()
				.Include<HtmlField, InputHtmlField>()
				.Include<ImageField, InputImageField>()
				.Include<MultiLineTextField, InputMultiLineTextField>()
				.Include<MultiSelectField, InputMultiSelectField>()
				.Include<NumberField, InputNumberField>()
				.Include<PasswordField, InputPasswordField>()
				.Include<PercentField, InputPercentField>()
				.Include<PhoneField, InputPhoneField>()
				.Include<SelectField, InputSelectField>()
				.Include<TextField, InputTextField>()
				.Include<UrlField, InputUrlField>();

            CreateMap<InputField, Field>()
                .Include<InputAutoNumberField, AutoNumberField>()
                .Include<InputCheckboxField, CheckboxField>()
                .Include<InputCurrencyField, CurrencyField>()
                .Include<InputDateField, DateField>()
                .Include<InputDateTimeField, DateTimeField>()
                .Include<InputEmailField, EmailField>()
                .Include<InputFileField, FileField>()
				.Include<InputGeographyField, GeographyField>()
				.Include<InputGuidField, GuidField>()
                .Include<InputHtmlField, HtmlField>()
                .Include<InputImageField, ImageField>()
                .Include<InputMultiLineTextField, MultiLineTextField>()
                .Include<InputMultiSelectField, MultiSelectField>()
                .Include<InputNumberField, NumberField>()
                .Include<InputPasswordField, PasswordField>()
                .Include<InputPercentField, PercentField>()
                .Include<InputPhoneField, PhoneField>()
                .Include<InputSelectField, SelectField>()
                .Include<InputTextField, TextField>()
                .Include<InputUrlField, UrlField>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => (y.Id.HasValue) ? y.Id.Value : Guid.Empty))
                .ForMember(x => x.System, opt => opt.MapFrom(y => (y.System.HasValue) ? y.System.Value : false))
                .ForMember(x => x.Required, opt => opt.MapFrom(y => (y.Required.HasValue) ? y.Required.Value : false))
                .ForMember(x => x.Unique, opt => opt.MapFrom(y => (y.Unique.HasValue) ? y.Unique.Value : false))
                .ForMember(x => x.Searchable, opt => opt.MapFrom(y => (y.Searchable.HasValue) ? y.Searchable.Value : false))
                .ForMember(x => x.Auditable, opt => opt.MapFrom(y => (y.Auditable.HasValue) ? y.Auditable.Value : false));

			CreateMap<Field, DbBaseField>()
				.Include<AutoNumberField, DbAutoNumberField>()
				.Include<CheckboxField, DbCheckboxField>()
				.Include<CurrencyField, DbCurrencyField>()
				.Include<DateField, DbDateField>()
				.Include<DateTimeField, DbDateTimeField>()
				.Include<EmailField, DbEmailField>()
				.Include<FileField, DbFileField>()
				.Include<GeographyField, DbGeographyField>()
				.Include<GuidField, DbGuidField>()
				.Include<HtmlField, DbHtmlField>()
				.Include<ImageField, DbImageField>()
				.Include<MultiLineTextField, DbMultiLineTextField>()
				.Include<MultiSelectField, DbMultiSelectField>()
				.Include<NumberField, DbNumberField>()
				.Include<PasswordField, DbPasswordField>()
				.Include<PercentField, DbPercentField>()
				.Include<PhoneField, DbPhoneField>()
				.Include<SelectField, DbSelectField>()
				.Include<TextField, DbTextField>()
				.Include<UrlField, DbUrlField>();
				

			CreateMap<DbBaseField, Field>()
				.Include<DbAutoNumberField, AutoNumberField>()
				.Include<DbCurrencyField, CurrencyField>()
				.Include<DbCheckboxField, CheckboxField>()
				.Include<DbDateField, DateField>()
				.Include<DbDateTimeField, DateTimeField>()
				.Include<DbEmailField, EmailField>()
				.Include<DbFileField, FileField>()
				.Include<DbGeographyField, GeographyField>()
				.Include<DbGuidField, GuidField>()
				.Include<DbHtmlField, HtmlField>()
				.Include<DbImageField, ImageField>()
				.Include<DbMultiLineTextField, MultiLineTextField>()
				.Include<DbMultiSelectField, MultiSelectField>()
				.Include<DbNumberField, NumberField>()
				.Include<DbPasswordField, PasswordField>()
				.Include<DbPercentField, PercentField>()
				.Include<DbPhoneField, PhoneField>()
				.Include<DbSelectField, SelectField>()
				.Include<DbTextField, TextField>()
				.Include<DbUrlField, UrlField>();

            CreateMap<AutoNumberField, InputAutoNumberField>();
			CreateMap<InputAutoNumberField, AutoNumberField>();
			CreateMap<AutoNumberField, DbAutoNumberField>();
			CreateMap<DbAutoNumberField, AutoNumberField>();

			CreateMap<CheckboxField, InputCheckboxField>();
			CreateMap<InputCheckboxField, CheckboxField>();
			CreateMap<CheckboxField, DbCheckboxField>();
			CreateMap<DbCheckboxField, CheckboxField>();

			CreateMap<CurrencyField, InputCurrencyField>();
			CreateMap<InputCurrencyField, CurrencyField>();
			CreateMap<CurrencyField, DbCurrencyField>();
			CreateMap<DbCurrencyField, CurrencyField>();

			CreateMap<DateField, InputDateField>();
			CreateMap<InputDateField, DateField>();
			CreateMap<DateField, DbDateField>();
			CreateMap<DbDateField, DateField>();

			CreateMap<DateTimeField, InputDateTimeField>();
			CreateMap<InputDateTimeField, DateTimeField>();
			CreateMap<DateTimeField, DbDateTimeField>();
			CreateMap<DbDateTimeField, DateTimeField>();

			CreateMap<EmailField, InputEmailField>();
			CreateMap<InputEmailField, EmailField>();
			CreateMap<EmailField, DbEmailField>();
			CreateMap<DbEmailField, EmailField>();

			CreateMap<FileField, InputFileField>();
			CreateMap<InputFileField, FileField>();
			CreateMap<FileField, DbFileField>();
			CreateMap<DbFileField, FileField>();

			CreateMap<GeographyField, InputGeographyField>();
			CreateMap<InputGeographyField, GeographyField>();
			CreateMap<GeographyField, DbGeographyField>();
			CreateMap<DbGeographyField, GeographyField>();

			CreateMap<GuidField, InputGuidField>();
			CreateMap<InputGuidField, GuidField>();
			CreateMap<GuidField, DbGuidField>();
			CreateMap<DbGuidField, GuidField>();

			CreateMap<HtmlField, InputHtmlField>();
			CreateMap<InputHtmlField, HtmlField>();
			CreateMap<HtmlField, DbHtmlField>();
			CreateMap<DbHtmlField, HtmlField>();

			CreateMap<ImageField, InputImageField>();
			CreateMap<InputImageField, ImageField>();
			CreateMap<ImageField, DbImageField>();
			CreateMap<DbImageField, ImageField>();

			CreateMap<MultiLineTextField, InputMultiLineTextField>();
			CreateMap<InputMultiLineTextField, MultiLineTextField>();
			CreateMap<MultiLineTextField, DbMultiLineTextField>();
			CreateMap<DbMultiLineTextField, MultiLineTextField>();

			CreateMap<MultiSelectField, InputMultiSelectField>();
			CreateMap<InputMultiSelectField, MultiSelectField>();
			CreateMap<MultiSelectField, DbMultiSelectField>();
			CreateMap<DbMultiSelectField, MultiSelectField>();

			CreateMap<NumberField, InputNumberField>();
			CreateMap<InputNumberField, NumberField>();
			CreateMap<NumberField, DbNumberField>();
			CreateMap<DbNumberField, NumberField>();

			CreateMap<PasswordField, InputPasswordField>();
			CreateMap<InputPasswordField, PasswordField>();
			CreateMap<PasswordField, DbPasswordField>();
			CreateMap<DbPasswordField, PasswordField>();

			CreateMap<PercentField, InputPercentField>();
			CreateMap<InputPercentField, PercentField>();
			CreateMap<PercentField, DbPercentField>();
			CreateMap<DbPercentField, PercentField>();

			CreateMap<PhoneField, InputPhoneField>();
			CreateMap<InputPhoneField, PhoneField>();
			CreateMap<PhoneField, DbPhoneField>();
			CreateMap<DbPhoneField, PhoneField>();

			CreateMap<SelectField, InputSelectField>();
			CreateMap<InputSelectField, SelectField>();
			CreateMap<SelectField, DbSelectField>();
			CreateMap<DbSelectField, SelectField>();

			CreateMap<TextField, InputTextField>();
			CreateMap<InputTextField, TextField>();
			CreateMap<TextField, DbTextField>();
			CreateMap<DbTextField, TextField>();

			CreateMap<UrlField, InputUrlField>();
			CreateMap<InputUrlField, UrlField>();
			CreateMap<UrlField, DbUrlField>();
			CreateMap<DbUrlField, UrlField>();

			CreateMap<SelectOption, DbSelectOption>();
			CreateMap<DbSelectOption, SelectOption>();

			CreateMap<CurrencyType, DbCurrencyType>();
			CreateMap<DbCurrencyType, CurrencyType>();

			
		}
	}
}
