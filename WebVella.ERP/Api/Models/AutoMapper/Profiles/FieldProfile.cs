using AutoMapper;
using System;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
{
	public class FieldProfile : Profile
	{
		IErpService service;

		public FieldProfile(IErpService service)
		{
			this.service = service;
		}

		protected override void Configure()
		{
			Mapper.CreateMap<AutoNumberField, IStorageAutoNumberField>().ConstructUsing(x => CreateEmptyAutoNumberFieldObject(x));
			Mapper.CreateMap<IStorageAutoNumberField, AutoNumberField>();
			Mapper.CreateMap<AutoNumberField, InputAutoNumberField>();
			Mapper.CreateMap<InputAutoNumberField, AutoNumberField>();

			Mapper.CreateMap<CheckboxField, IStorageCheckboxField>().ConstructUsing(x => CreateEmptyCheckboxFieldObject(x));
			Mapper.CreateMap<IStorageCheckboxField, CheckboxField>();
			Mapper.CreateMap<CheckboxField, InputCheckboxField>();
			Mapper.CreateMap<InputCheckboxField, CheckboxField>();

			Mapper.CreateMap<CurrencyField, IStorageCurrencyField>().ConstructUsing(x => CreateEmptyCurrencyFieldObject(x));
			Mapper.CreateMap<IStorageCurrencyField, CurrencyField>();
			Mapper.CreateMap<CurrencyField, InputCurrencyField>();
			Mapper.CreateMap<InputCurrencyField, CurrencyField>();

			Mapper.CreateMap<DateField, IStorageDateField>().ConstructUsing(x => CreateEmptyDateFieldObject(x));
			Mapper.CreateMap<IStorageDateField, DateField>();
			Mapper.CreateMap<DateField, InputDateField>();
			Mapper.CreateMap<InputDateField, DateField>();

			Mapper.CreateMap<DateTimeField, IStorageDateTimeField>().ConstructUsing(x => CreateEmptyDateTimeFieldObject(x));
			Mapper.CreateMap<IStorageDateTimeField, DateTimeField>();
			Mapper.CreateMap<DateTimeField, InputDateTimeField>();
			Mapper.CreateMap<InputDateTimeField, DateTimeField>();

			Mapper.CreateMap<EmailField, IStorageEmailField>().ConstructUsing(x => CreateEmptyEmailFieldObject(x));
			Mapper.CreateMap<IStorageEmailField, EmailField>();
			Mapper.CreateMap<EmailField, InputEmailField>();
			Mapper.CreateMap<InputEmailField, EmailField>();

			Mapper.CreateMap<FileField, IStorageFileField>().ConstructUsing(x => CreateEmptyFileFieldObject(x));
			Mapper.CreateMap<IStorageFileField, FileField>();
			Mapper.CreateMap<FileField, InputFileField>();
			Mapper.CreateMap<InputFileField, FileField>();

			Mapper.CreateMap<GuidField, IStorageGuidField>().ConstructUsing(x => CreateEmptyGuidFieldObject(x));
			Mapper.CreateMap<IStorageGuidField, GuidField>();
			Mapper.CreateMap<GuidField, InputGuidField>();
			Mapper.CreateMap<InputGuidField, GuidField>();

			Mapper.CreateMap<HtmlField, IStorageHtmlField>().ConstructUsing(x => CreateEmptyHtmlFieldObject(x));
			Mapper.CreateMap<IStorageHtmlField, HtmlField>();
			Mapper.CreateMap<HtmlField, InputHtmlField>();
			Mapper.CreateMap<InputHtmlField, HtmlField>();

			Mapper.CreateMap<ImageField, IStorageImageField>().ConstructUsing(x => CreateEmptyImageFieldObject(x));
			Mapper.CreateMap<IStorageImageField, ImageField>();
			Mapper.CreateMap<ImageField, InputImageField>();
			Mapper.CreateMap<InputImageField, ImageField>();

			Mapper.CreateMap<MultiLineTextField, IStorageMultiLineTextField>().ConstructUsing(x => CreateEmptyMultiLineTextFieldObject(x));
			Mapper.CreateMap<IStorageMultiLineTextField, MultiLineTextField>();
			Mapper.CreateMap<MultiLineTextField, InputMultiLineTextField>();
			Mapper.CreateMap<InputMultiLineTextField, MultiLineTextField>();

			Mapper.CreateMap<MultiSelectField, IStorageMultiSelectField>().ConstructUsing(x => CreateEmptyMultiSelectFieldObject(x));
			Mapper.CreateMap<IStorageMultiSelectField, MultiSelectField>();
			Mapper.CreateMap<MultiSelectField, InputMultiSelectField>();
			Mapper.CreateMap<InputMultiSelectField, MultiSelectField>();

			Mapper.CreateMap<NumberField, IStorageNumberField>().ConstructUsing(x => CreateEmptyNumberFieldObject(x));
			Mapper.CreateMap<IStorageNumberField, NumberField>();
			Mapper.CreateMap<NumberField, InputNumberField>();
			Mapper.CreateMap<InputNumberField, NumberField>();

			Mapper.CreateMap<PasswordField, IStoragePasswordField>().ConstructUsing(x => CreateEmptyPasswordFieldObject(x));
			Mapper.CreateMap<IStoragePasswordField, PasswordField>();
			Mapper.CreateMap<PasswordField, InputPasswordField>();
			Mapper.CreateMap<InputPasswordField, PasswordField>();

			Mapper.CreateMap<PercentField, IStoragePercentField>().ConstructUsing(x => CreateEmptyPercentFieldObject(x));
			Mapper.CreateMap<IStoragePercentField, PercentField>();
			Mapper.CreateMap<PercentField, InputPercentField>();
			Mapper.CreateMap<InputPercentField, PercentField>();

			Mapper.CreateMap<PhoneField, IStoragePhoneField>().ConstructUsing(x => CreateEmptyPhoneFieldObject(x));
			Mapper.CreateMap<IStoragePhoneField, PhoneField>();
			Mapper.CreateMap<PhoneField, InputPhoneField>();
			Mapper.CreateMap<InputPhoneField, PhoneField>();

			Mapper.CreateMap<SelectField, IStorageSelectField>().ConstructUsing(x => CreateEmptySelectFieldObject(x));
			Mapper.CreateMap<IStorageSelectField, SelectField>();
			Mapper.CreateMap<SelectField, InputSelectField>();
			Mapper.CreateMap<InputSelectField, SelectField>();

			Mapper.CreateMap<TextField, IStorageTextField>().ConstructUsing(x => CreateEmptyTextFieldObject(x));
			Mapper.CreateMap<IStorageTextField, TextField>();
			Mapper.CreateMap<TextField, InputTextField>();
			Mapper.CreateMap<InputTextField, TextField>();

			Mapper.CreateMap<UrlField, IStorageUrlField>().ConstructUsing(x => CreateEmptyUrlFieldObject(x));
			Mapper.CreateMap<IStorageUrlField, UrlField>();
			Mapper.CreateMap<UrlField, InputUrlField>();
			Mapper.CreateMap<InputUrlField, UrlField>();

			Mapper.CreateMap<SelectFieldOption, IStorageSelectFieldOption>().ConstructUsing(x => CreateEmptySelectFieldOptionObject(x));
			Mapper.CreateMap<IStorageSelectFieldOption, SelectFieldOption>();

			Mapper.CreateMap<MultiSelectFieldOption, IStorageMultiSelectFieldOption>().ConstructUsing(x => CreateEmptyMultiSelectFieldOptionObject(x));
			Mapper.CreateMap<IStorageMultiSelectFieldOption, MultiSelectFieldOption>();

			Mapper.CreateMap<Field, IStorageField>().ConstructUsing(x => CreateEmptyFieldObject(x))
				.Include<AutoNumberField, IStorageAutoNumberField>()
				.Include<CheckboxField, IStorageCheckboxField>()
				.Include<CurrencyField, IStorageCurrencyField>()
				.Include<DateField, IStorageDateField>()
				.Include<DateTimeField, IStorageDateTimeField>()
				.Include<EmailField, IStorageEmailField>()
				.Include<FileField, IStorageFileField>()
				.Include<GuidField, IStorageGuidField>()
				.Include<HtmlField, IStorageHtmlField>()
				.Include<ImageField, IStorageImageField>()
				.Include<MultiSelectField, IStorageMultiSelectField>()
				.Include<NumberField, IStorageNumberField>()
				.Include<PasswordField, IStoragePasswordField>()
				.Include<PercentField, IStoragePercentField>()
				.Include<PhoneField, IStoragePhoneField>()
				.Include<SelectField, IStorageSelectField>()
				.Include<TextField, IStorageTextField>()
				.Include<UrlField, IStorageUrlField>();
			Mapper.CreateMap<IStorageField, Field>()
				.Include<IStorageAutoNumberField, AutoNumberField>()
				.Include<IStorageCheckboxField, CheckboxField>()
				.Include<IStorageCurrencyField, CurrencyField>()
				.Include<IStorageDateField, DateField>()
				.Include<IStorageDateTimeField, DateTimeField>()
				.Include<IStorageEmailField, EmailField>()
				.Include<IStorageFileField, FileField>()
				.Include<IStorageGuidField, GuidField>()
				.Include<IStorageHtmlField, HtmlField>()
				.Include<IStorageImageField, ImageField>()
				.Include<IStorageMultiSelectField, MultiSelectField>()
				.Include<IStorageNumberField, NumberField>()
				.Include<IStoragePasswordField, PasswordField>()
				.Include<IStoragePercentField, PercentField>()
				.Include<IStoragePhoneField, PhoneField>()
				.Include<IStorageSelectField, SelectField>()
				.Include<IStorageTextField, TextField>()
				.Include<IStorageUrlField, UrlField>();

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
				.Include<UrlField, InputUrlField>();
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
				.ForMember(x => x.Id, opt => opt.MapFrom(y => (y.Id.HasValue) ? y.Id.Value : Guid.Empty))
				.ForMember(x => x.System, opt => opt.MapFrom(y => (y.System.HasValue) ? y.System.Value : false))
				.ForMember(x => x.Required, opt => opt.MapFrom(y => (y.Required.HasValue) ? y.Required.Value : false))
				.ForMember(x => x.Unique, opt => opt.MapFrom(y => (y.Unique.HasValue) ? y.Unique.Value : false))
				.ForMember(x => x.Searchable, opt => opt.MapFrom(y => (y.Searchable.HasValue) ? y.Searchable.Value : false))
				.ForMember(x => x.Auditable, opt => opt.MapFrom(y => (y.Auditable.HasValue) ? y.Auditable.Value : false));
		}

		protected IStorageField CreateEmptyFieldObject(Field field)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
		}

		protected IStorageAutoNumberField CreateEmptyAutoNumberFieldObject(AutoNumberField field)
		{
			var storageService = service.StorageService;
			return (IStorageAutoNumberField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
		}

		protected IStorageCheckboxField CreateEmptyCheckboxFieldObject(CheckboxField field)
		{
			var storageService = service.StorageService;
			return (IStorageCheckboxField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
		}
		protected IStorageCurrencyField CreateEmptyCurrencyFieldObject(CurrencyField field)
		{
			var storageService = service.StorageService;
			return (IStorageCurrencyField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
		}

		protected IStorageDateField CreateEmptyDateFieldObject(DateField field)
		{
			var storageService = service.StorageService;
			return (IStorageDateField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
		}

		protected IStorageDateTimeField CreateEmptyDateTimeFieldObject(DateTimeField field)
		{
			var storageService = service.StorageService;
			return (IStorageDateTimeField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
		}

		protected IStorageEmailField CreateEmptyEmailFieldObject(EmailField field)
		{
			var storageService = service.StorageService;
			return (IStorageEmailField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
		}

		protected IStorageFileField CreateEmptyFileFieldObject(FileField field)
		{
			var storageService = service.StorageService;
			return (IStorageFileField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
		}

		protected IStorageGuidField CreateEmptyGuidFieldObject(GuidField field)
		{
			var storageService = service.StorageService;
			return (IStorageGuidField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
		}

		protected IStorageHtmlField CreateEmptyHtmlFieldObject(HtmlField field)
		{
			var storageService = service.StorageService;
			return (IStorageHtmlField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
		}

		protected IStorageImageField CreateEmptyImageFieldObject(ImageField field)
		{
			var storageService = service.StorageService;
			return (IStorageImageField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
		}

		protected IStorageMultiLineTextField CreateEmptyMultiLineTextFieldObject(MultiLineTextField field)
		{
			var storageService = service.StorageService;
			return (IStorageMultiLineTextField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
		}

		protected IStorageMultiSelectField CreateEmptyMultiSelectFieldObject(MultiSelectField field)
		{
			var storageService = service.StorageService;
			return (IStorageMultiSelectField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
		}

		protected IStorageNumberField CreateEmptyNumberFieldObject(NumberField field)
		{
			var storageService = service.StorageService;
			return (IStorageNumberField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
		}

		protected IStoragePasswordField CreateEmptyPasswordFieldObject(PasswordField field)
		{
			var storageService = service.StorageService;
			return (IStoragePasswordField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
		}

		protected IStoragePercentField CreateEmptyPercentFieldObject(PercentField field)
		{
			var storageService = service.StorageService;
			return (IStoragePercentField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
		}

		protected IStoragePhoneField CreateEmptyPhoneFieldObject(PhoneField field)
		{
			var storageService = service.StorageService;
			return (IStoragePhoneField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
		}

		protected IStorageSelectField CreateEmptySelectFieldObject(SelectField field)
		{
			var storageService = service.StorageService;
			return (IStorageSelectField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
		}

		protected IStorageTextField CreateEmptyTextFieldObject(TextField field)
		{
			var storageService = service.StorageService;
			return (IStorageTextField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
		}

		protected IStorageUrlField CreateEmptyUrlFieldObject(UrlField field)
		{
			var storageService = service.StorageService;
			return (IStorageUrlField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
		}

		protected IStorageSelectFieldOption CreateEmptySelectFieldOptionObject(SelectFieldOption field)
		{
			var storageService = service.StorageService;
			return (IStorageSelectFieldOption)storageService.GetObjectFactory().CreateEmptySelectFieldOptionObject();
		}

		protected IStorageMultiSelectFieldOption CreateEmptyMultiSelectFieldOptionObject(MultiSelectFieldOption field)
		{
			var storageService = service.StorageService;
			return (IStorageMultiSelectFieldOption)storageService.GetObjectFactory().CreateEmptyMultiSelectFieldOptionObject();
		}

	}
}
