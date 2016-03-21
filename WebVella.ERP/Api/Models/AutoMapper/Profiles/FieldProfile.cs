using AutoMapper;
using System;
using WebVella.ERP.Database;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
{
	public class FieldProfile : Profile
	{
		protected override void Configure()
		{
			Mapper.CreateMap<AutoNumberField, InputAutoNumberField>();
			Mapper.CreateMap<InputAutoNumberField, AutoNumberField>();
			Mapper.CreateMap<AutoNumberField, DbAutoNumberField>();
			Mapper.CreateMap<DbAutoNumberField, AutoNumberField>();

			Mapper.CreateMap<CheckboxField, InputCheckboxField>();
			Mapper.CreateMap<InputCheckboxField, CheckboxField>();
			Mapper.CreateMap<CheckboxField, DbCheckboxField>();
			Mapper.CreateMap<DbCheckboxField, CheckboxField>();

			Mapper.CreateMap<CurrencyField, InputCurrencyField>();
			Mapper.CreateMap<InputCurrencyField, CurrencyField>();
			Mapper.CreateMap<CurrencyField, DbCurrencyField>();
			Mapper.CreateMap<DbCurrencyField, CurrencyField>();

			Mapper.CreateMap<DateField, InputDateField>();
			Mapper.CreateMap<InputDateField, DateField>();
			Mapper.CreateMap<DateField, DbDateField>();
			Mapper.CreateMap<DbDateField, DateField>();

			Mapper.CreateMap<DateTimeField, InputDateTimeField>();
			Mapper.CreateMap<InputDateTimeField, DateTimeField>();
			Mapper.CreateMap<DateTimeField, DbDateTimeField>();
			Mapper.CreateMap<DbDateTimeField, DateTimeField>();

			Mapper.CreateMap<EmailField, InputEmailField>();
			Mapper.CreateMap<InputEmailField, EmailField>();
			Mapper.CreateMap<EmailField, DbEmailField>();
			Mapper.CreateMap<DbEmailField, EmailField>();

			Mapper.CreateMap<FileField, InputFileField>();
			Mapper.CreateMap<InputFileField, FileField>();
			Mapper.CreateMap<FileField, DbFileField>();
			Mapper.CreateMap<DbFileField, FileField>();

			Mapper.CreateMap<GuidField, InputGuidField>();
			Mapper.CreateMap<InputGuidField, GuidField>();
			Mapper.CreateMap<GuidField, DbGuidField>();
			Mapper.CreateMap<DbGuidField, GuidField>();

			Mapper.CreateMap<HtmlField, InputHtmlField>();
			Mapper.CreateMap<InputHtmlField, HtmlField>();
			Mapper.CreateMap<HtmlField, DbHtmlField>();
			Mapper.CreateMap<DbHtmlField, HtmlField>();

			Mapper.CreateMap<ImageField, InputImageField>();
			Mapper.CreateMap<InputImageField, ImageField>();
			Mapper.CreateMap<ImageField, DbImageField>();
			Mapper.CreateMap<DbImageField, ImageField>();

			Mapper.CreateMap<MultiLineTextField, InputMultiLineTextField>();
			Mapper.CreateMap<InputMultiLineTextField, MultiLineTextField>();
			Mapper.CreateMap<MultiLineTextField, DbMultiLineTextField>();
			Mapper.CreateMap<DbMultiLineTextField, MultiLineTextField>();

			Mapper.CreateMap<MultiSelectField, InputMultiSelectField>();
			Mapper.CreateMap<InputMultiSelectField, MultiSelectField>();
			Mapper.CreateMap<MultiSelectField, DbMultiSelectField>();
			Mapper.CreateMap<DbMultiSelectField, MultiSelectField>();

			Mapper.CreateMap<NumberField, InputNumberField>();
			Mapper.CreateMap<InputNumberField, NumberField>();
			Mapper.CreateMap<NumberField, DbNumberField>();
			Mapper.CreateMap<DbNumberField, NumberField>();

			Mapper.CreateMap<PasswordField, InputPasswordField>();
			Mapper.CreateMap<InputPasswordField, PasswordField>();
			Mapper.CreateMap<PasswordField, DbPasswordField>();
			Mapper.CreateMap<DbPasswordField, PasswordField>();

			Mapper.CreateMap<PercentField, InputPercentField>();
			Mapper.CreateMap<InputPercentField, PercentField>();
			Mapper.CreateMap<PercentField, DbPercentField>();
			Mapper.CreateMap<DbPercentField, PercentField>();

			Mapper.CreateMap<PhoneField, InputPhoneField>();
			Mapper.CreateMap<InputPhoneField, PhoneField>();
			Mapper.CreateMap<PhoneField, DbPhoneField>();
			Mapper.CreateMap<DbPhoneField, PhoneField>();

			Mapper.CreateMap<SelectField, InputSelectField>();
			Mapper.CreateMap<InputSelectField, SelectField>();
			Mapper.CreateMap<SelectField, DbSelectField>();
			Mapper.CreateMap<DbSelectField, SelectField>();

			Mapper.CreateMap<TextField, InputTextField>();
			Mapper.CreateMap<InputTextField, TextField>();
			Mapper.CreateMap<TextField, DbTextField>();
			Mapper.CreateMap<DbTextField, TextField>();

			Mapper.CreateMap<UrlField, InputUrlField>();
			Mapper.CreateMap<InputUrlField, UrlField>();
			Mapper.CreateMap<UrlField, DbUrlField>();
			Mapper.CreateMap<DbUrlField, UrlField>();

			Mapper.CreateMap<TreeSelectField, InputTreeSelectField>();
			Mapper.CreateMap<InputTreeSelectField, TreeSelectField>();
			Mapper.CreateMap<TreeSelectField, DbTreeSelectField>();
			Mapper.CreateMap<DbTreeSelectField, TreeSelectField>();

			Mapper.CreateMap<SelectFieldOption, DbSelectFieldOption>();
			Mapper.CreateMap<DbSelectFieldOption, SelectFieldOption>();

			Mapper.CreateMap<MultiSelectFieldOption, DbMultiSelectFieldOption>();
			Mapper.CreateMap<DbMultiSelectFieldOption, MultiSelectFieldOption>();

			Mapper.CreateMap<CurrencyType, DbCurrencyType>();
			Mapper.CreateMap<DbCurrencyType, CurrencyType>();

			Mapper.CreateMap<Field, InputField>()
				.Include<AutoNumberField, InputAutoNumberField>()
				.Include<CheckboxField, InputCheckboxField>()
				.Include<CurrencyField, InputCurrencyField>()
				.Include<DateField, InputDateField>()
				.Include<DateTimeField, InputDateTimeField>()
				.Include<EmailField, InputEmailField>()
				.Include<FileField, InputFileField>()
				.Include<GuidField, InputGuidField>()
				.Include<HtmlField, InputHtmlField>()
				.Include<ImageField, InputImageField>()
				.Include<MultiSelectField, InputMultiSelectField>()
				.Include<NumberField, InputNumberField>()
				.Include<PasswordField, InputPasswordField>()
				.Include<PercentField, InputPercentField>()
				.Include<PhoneField, InputPhoneField>()
				.Include<SelectField, InputSelectField>()
				.Include<TextField, InputTextField>()
				.Include<UrlField, InputUrlField>()
				.Include<TreeSelectField, InputTreeSelectField>();
			Mapper.CreateMap<InputField, Field>()
				.Include<InputAutoNumberField, AutoNumberField>()
				.Include<InputCheckboxField, CheckboxField>()
				.Include<InputCurrencyField, CurrencyField>()
				.Include<InputDateField, DateField>()
				.Include<InputDateTimeField, DateTimeField>()
				.Include<InputEmailField, EmailField>()
				.Include<InputFileField, FileField>()
				.Include<InputGuidField, GuidField>()
				.Include<InputHtmlField, HtmlField>()
				.Include<InputImageField, ImageField>()
				.Include<InputMultiSelectField, MultiSelectField>()
				.Include<InputNumberField, NumberField>()
				.Include<InputPasswordField, PasswordField>()
				.Include<InputPercentField, PercentField>()
				.Include<InputPhoneField, PhoneField>()
				.Include<InputSelectField, SelectField>()
				.Include<InputTextField, TextField>()
				.Include<InputUrlField, UrlField>()
				.Include<InputTreeSelectField, TreeSelectField>()
				.ForMember(x => x.Id, opt => opt.MapFrom(y => (y.Id.HasValue) ? y.Id.Value : Guid.Empty))
				.ForMember(x => x.System, opt => opt.MapFrom(y => (y.System.HasValue) ? y.System.Value : false))
				.ForMember(x => x.Required, opt => opt.MapFrom(y => (y.Required.HasValue) ? y.Required.Value : false))
				.ForMember(x => x.Unique, opt => opt.MapFrom(y => (y.Unique.HasValue) ? y.Unique.Value : false))
				.ForMember(x => x.Searchable, opt => opt.MapFrom(y => (y.Searchable.HasValue) ? y.Searchable.Value : false))
				.ForMember(x => x.Auditable, opt => opt.MapFrom(y => (y.Auditable.HasValue) ? y.Auditable.Value : false));

			Mapper.CreateMap<Field, DbBaseField>()
				.Include<AutoNumberField, DbAutoNumberField>()
				.Include<CheckboxField, DbCheckboxField>()
				.Include<CurrencyField, DbCurrencyField>()
				.Include<DateField, DbDateField>()
				.Include<DateTimeField, DbDateTimeField>()
				.Include<EmailField, DbEmailField>()
				.Include<FileField, DbFileField>()
				.Include<GuidField, DbGuidField>()
				.Include<HtmlField, DbHtmlField>()
				.Include<ImageField, DbImageField>()
				.Include<MultiSelectField, DbMultiSelectField>()
				.Include<NumberField, DbNumberField>()
				.Include<PasswordField, DbPasswordField>()
				.Include<PercentField, DbPercentField>()
				.Include<PhoneField, DbPhoneField>()
				.Include<SelectField, DbSelectField>()
				.Include<TextField, DbTextField>()
				.Include<UrlField, DbUrlField>()
				.Include<TreeSelectField, DbTreeSelectField>();
			Mapper.CreateMap<DbBaseField, Field>()
				.Include<DbAutoNumberField, AutoNumberField>()
				.Include<DbCurrencyField, CurrencyField>()
				.Include<DbCheckboxField, CheckboxField>()
				.Include<DbDateField, DateField>()
				.Include<DbDateTimeField, DateTimeField>()
				.Include<DbEmailField, EmailField>()
				.Include<DbFileField, FileField>()
				.Include<DbGuidField, GuidField>()
				.Include<DbHtmlField, HtmlField>()
				.Include<DbImageField, ImageField>()
				.Include<DbMultiSelectField, MultiSelectField>()
				.Include<DbNumberField, NumberField>()
				.Include<DbPasswordField, PasswordField>()
				.Include<DbPercentField, PercentField>()
				.Include<DbPhoneField, PhoneField>()
				.Include<DbSelectField, SelectField>()
				.Include<DbTextField, TextField>()
				.Include<DbUrlField, UrlField>()
				.Include<DbTreeSelectField, TreeSelectField>();
		}
	}
}
