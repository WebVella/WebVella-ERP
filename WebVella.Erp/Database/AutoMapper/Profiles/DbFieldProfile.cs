//using AutoMapper;
//using WebVella.Erp.Database;
//using System;
//using WebVella.Erp;
//using WebVella.Erp.Api;
//using WebVella.Erp.Api.Models;
//using WebVella.Erp.Storage;

//namespace WebVella.Erp.Database.AutoMapper.Profiles
//{
//	public class DbFieldProfile : Profile
//	{
//		IErpService service;

//		public DbFieldProfile(IErpService service)
//		{
//			this.service = service;
//		}

//		protected override void Configure()
//		{
//			Mapper.CreateMap<AutoNumberField, DbAutoNumberField>();
//			Mapper.CreateMap<DbAutoNumberField, AutoNumberField>();
//			Mapper.CreateMap<DbAutoNumberField, IStorageAutoNumberField>().ConstructUsing(x => CreateEmptyAutoNumberFieldObject(x));
//			Mapper.CreateMap<IStorageAutoNumberField, DbAutoNumberField>();

//			Mapper.CreateMap<CheckboxField, DbCheckboxField>();
//			Mapper.CreateMap<DbCheckboxField, CheckboxField>();
//			Mapper.CreateMap<DbCheckboxField, IStorageCheckboxField>().ConstructUsing(x => CreateEmptyCheckboxFieldObject(x));
//			Mapper.CreateMap<IStorageCheckboxField, DbCheckboxField>();

//			Mapper.CreateMap<CurrencyField, DbCurrencyField>();
//			Mapper.CreateMap<DbCurrencyField, CurrencyField>();
//			Mapper.CreateMap<DbCurrencyField, IStorageCurrencyField>().ConstructUsing(x => CreateEmptyCurrencyFieldObject(x));
//			Mapper.CreateMap<IStorageCurrencyField, DbCurrencyField>();

//			Mapper.CreateMap<DateField, DbDateField>();
//			Mapper.CreateMap<DbDateField, DateField>();
//			Mapper.CreateMap<DbDateField, IStorageDateField>().ConstructUsing(x => CreateEmptyDateFieldObject(x));
//			Mapper.CreateMap<IStorageDateField, DbDateField>();

//			Mapper.CreateMap<DateTimeField, DbDateTimeField>();
//			Mapper.CreateMap<DbDateTimeField, DateTimeField>();
//			Mapper.CreateMap<DbDateTimeField, IStorageDateTimeField>().ConstructUsing(x => CreateEmptyDateTimeFieldObject(x));
//			Mapper.CreateMap<IStorageDateTimeField, DbDateTimeField>();

//			Mapper.CreateMap<EmailField, DbEmailField>();
//			Mapper.CreateMap<DbEmailField, EmailField>();
//			Mapper.CreateMap<DbEmailField, IStorageEmailField>().ConstructUsing(x => CreateEmptyEmailFieldObject(x));
//			Mapper.CreateMap<IStorageEmailField, DbEmailField>();

//			Mapper.CreateMap<FileField, DbFileField>();
//			Mapper.CreateMap<DbFileField, FileField>();
//			Mapper.CreateMap<DbFileField, IStorageFileField>().ConstructUsing(x => CreateEmptyFileFieldObject(x));
//			Mapper.CreateMap<IStorageFileField, DbFileField>();

//			Mapper.CreateMap<GuidField, DbGuidField>();
//			Mapper.CreateMap<DbGuidField, GuidField>();
//			Mapper.CreateMap<DbGuidField, IStorageGuidField>().ConstructUsing(x => CreateEmptyGuidFieldObject(x));
//			Mapper.CreateMap<IStorageGuidField, DbGuidField>();

//			Mapper.CreateMap<HtmlField, DbHtmlField>();
//			Mapper.CreateMap<DbHtmlField, HtmlField>();
//			Mapper.CreateMap<DbHtmlField, IStorageHtmlField>().ConstructUsing(x => CreateEmptyHtmlFieldObject(x));
//			Mapper.CreateMap<IStorageHtmlField, DbHtmlField>();

//			Mapper.CreateMap<ImageField, DbImageField>();
//			Mapper.CreateMap<DbImageField, ImageField>();
//			Mapper.CreateMap<DbImageField, IStorageImageField>().ConstructUsing(x => CreateEmptyImageFieldObject(x));
//			Mapper.CreateMap<IStorageImageField, DbImageField>();

//			Mapper.CreateMap<MultiLineTextField, DbMultiLineTextField>();
//			Mapper.CreateMap<DbMultiLineTextField, MultiLineTextField>();
//			Mapper.CreateMap<DbMultiLineTextField, IStorageMultiLineTextField>().ConstructUsing(x => CreateEmptyMultiLineTextFieldObject(x));
//			Mapper.CreateMap<IStorageMultiLineTextField, DbMultiLineTextField>();

//			Mapper.CreateMap<MultiSelectField, DbMultiSelectField>();
//			Mapper.CreateMap<DbMultiSelectField, MultiSelectField>();
//			Mapper.CreateMap<DbMultiSelectField, IStorageMultiSelectField>().ConstructUsing(x => CreateEmptyMultiSelectFieldObject(x));
//			Mapper.CreateMap<IStorageMultiSelectField, DbMultiSelectField>();

//			Mapper.CreateMap<NumberField, DbNumberField>();
//			Mapper.CreateMap<DbNumberField, NumberField>();
//			Mapper.CreateMap<DbNumberField, IStorageNumberField>().ConstructUsing(x => CreateEmptyNumberFieldObject(x));
//			Mapper.CreateMap<IStorageNumberField, DbNumberField>();

//			Mapper.CreateMap<PasswordField, DbPasswordField>();
//			Mapper.CreateMap<DbPasswordField, PasswordField>();
//			Mapper.CreateMap<DbPasswordField, IStoragePasswordField>().ConstructUsing(x => CreateEmptyPasswordFieldObject(x));
//			Mapper.CreateMap<IStoragePasswordField, DbPasswordField>();

//			Mapper.CreateMap<PercentField, DbPercentField>();
//			Mapper.CreateMap<DbPercentField, PercentField>();
//			Mapper.CreateMap<DbPercentField, IStoragePercentField>().ConstructUsing(x => CreateEmptyPercentFieldObject(x));
//			Mapper.CreateMap<IStoragePercentField, DbPercentField>();

//			Mapper.CreateMap<PhoneField, DbPhoneField>();
//			Mapper.CreateMap<DbPhoneField, PhoneField>();
//			Mapper.CreateMap<DbPhoneField, IStoragePhoneField>().ConstructUsing(x => CreateEmptyPhoneFieldObject(x));
//			Mapper.CreateMap<IStoragePhoneField, DbPhoneField>();

//			Mapper.CreateMap<SelectField, DbSelectField>();
//			Mapper.CreateMap<DbSelectField, SelectField>();
//			Mapper.CreateMap<DbSelectField, IStorageSelectField>().ConstructUsing(x => CreateEmptySelectFieldObject(x));
//			Mapper.CreateMap<IStorageSelectField, DbSelectField>();

//			Mapper.CreateMap<TextField, DbTextField>();
//			Mapper.CreateMap<DbTextField, TextField>();
//			Mapper.CreateMap<DbTextField, IStorageTextField>().ConstructUsing(x => CreateEmptyTextFieldObject(x));
//			Mapper.CreateMap<IStorageTextField, DbTextField>();

//			Mapper.CreateMap<UrlField, DbUrlField>();
//			Mapper.CreateMap<DbUrlField, UrlField>();
//			Mapper.CreateMap<DbUrlField, IStorageUrlField>().ConstructUsing(x => CreateEmptyUrlFieldObject(x));
//			Mapper.CreateMap<IStorageUrlField, DbUrlField>();

//			Mapper.CreateMap<TreeSelectField, DbTreeSelectField>();
//			Mapper.CreateMap<DbTreeSelectField, TreeSelectField>();
//			Mapper.CreateMap<DbTreeSelectField, IStorageTreeSelectField>().ConstructUsing(x => CreateEmptyTreeSelectFieldObject(x));
//			Mapper.CreateMap<IStorageTreeSelectField, DbTreeSelectField>();

//			Mapper.CreateMap<SelectFieldOption, DbSelectFieldOption>();
//			Mapper.CreateMap<DbSelectFieldOption, SelectFieldOption>();
//			Mapper.CreateMap<DbSelectFieldOption, IStorageSelectFieldOption>().ConstructUsing(x => CreateEmptySelectFieldOptionObject(x));
//			Mapper.CreateMap<IStorageSelectFieldOption, DbSelectFieldOption>();

//			Mapper.CreateMap<MultiSelectFieldOption, DbMultiSelectFieldOption>();
//			Mapper.CreateMap<DbMultiSelectFieldOption, MultiSelectFieldOption>();
//			Mapper.CreateMap<DbMultiSelectFieldOption, IStorageMultiSelectFieldOption>().ConstructUsing(x => CreateEmptyMultiSelectFieldOptionObject(x));
//			Mapper.CreateMap<IStorageMultiSelectFieldOption, DbMultiSelectFieldOption>();

//			Mapper.CreateMap<CurrencyType, DbCurrencyType>();
//			Mapper.CreateMap<DbCurrencyType, CurrencyType>();
//			Mapper.CreateMap<DbCurrencyType, IStorageCurrencyType>().ConstructUsing(x => CreateEmptyCurrencyTypeObject(x));
//			Mapper.CreateMap<IStorageCurrencyType, DbCurrencyType>();

//			Mapper.CreateMap<Field, DbBaseField>()
//				.Include<AutoNumberField, DbAutoNumberField>()
//				.Include<CheckboxField, DbCheckboxField>()
//				.Include<CurrencyField, DbCurrencyField>()
//				.Include<DateField, DbDateField>()
//				.Include<DateTimeField, DbDateTimeField>()
//				.Include<EmailField, DbEmailField>()
//				.Include<FileField, DbFileField>()
//				.Include<GuidField, DbGuidField>()
//				.Include<HtmlField, DbHtmlField>()
//				.Include<ImageField, DbImageField>()
//				.Include<MultiSelectField, DbMultiSelectField>()
//				.Include<NumberField, DbNumberField>()
//				.Include<PasswordField, DbPasswordField>()
//				.Include<PercentField, DbPercentField>()
//				.Include<PhoneField, DbPhoneField>()
//				.Include<SelectField, DbSelectField>()
//				.Include<TextField, DbTextField>()
//				.Include<UrlField, DbUrlField>()
//				.Include<TreeSelectField, DbTreeSelectField>();
//			Mapper.CreateMap<DbBaseField, Field>()
//				.Include<DbAutoNumberField, AutoNumberField>()
//				.Include<DbCurrencyField, CurrencyField>()
//				.Include<DbCheckboxField, CheckboxField>()
//				.Include<DbDateField, DateField>()
//				.Include<DbDateTimeField, DateTimeField>()
//				.Include<DbEmailField, EmailField>()
//				.Include<DbFileField, FileField>()
//				.Include<DbGuidField, GuidField>()
//				.Include<DbHtmlField, HtmlField>()
//				.Include<DbImageField, ImageField>()
//				.Include<DbMultiSelectField, MultiSelectField>()
//				.Include<DbNumberField, NumberField>()
//				.Include<DbPasswordField, PasswordField>()
//				.Include<DbPercentField, PercentField>()
//				.Include<DbPhoneField, PhoneField>()
//				.Include<DbSelectField, SelectField>()
//				.Include<DbTextField, TextField>()
//				.Include<DbUrlField, UrlField>()
//				.Include<DbTreeSelectField, TreeSelectField>();

//			Mapper.CreateMap<DbBaseField, IStorageField>().ConstructUsing(x => CreateEmptyFieldObject(x))
//				.Include<DbAutoNumberField, IStorageAutoNumberField>()
//				.Include<DbCheckboxField, IStorageCheckboxField>()
//				.Include<DbCurrencyField, IStorageCurrencyField>()
//				.Include<DbDateField, IStorageDateField>()
//				.Include<DbDateTimeField, IStorageDateTimeField>()
//				.Include<DbEmailField, IStorageEmailField>()
//				.Include<DbFileField, IStorageFileField>()
//				.Include<DbGuidField, IStorageGuidField>()
//				.Include<DbHtmlField, IStorageHtmlField>()
//				.Include<DbImageField, IStorageImageField>()
//				.Include<DbMultiSelectField, IStorageMultiSelectField>()
//				.Include<DbNumberField, IStorageNumberField>()
//				.Include<DbPasswordField, IStoragePasswordField>()
//				.Include<DbPercentField, IStoragePercentField>()
//				.Include<DbPhoneField, IStoragePhoneField>()
//				.Include<DbSelectField, IStorageSelectField>()
//				.Include<DbTextField, IStorageTextField>()
//				.Include<DbUrlField, IStorageUrlField>()
//				.Include<DbTreeSelectField, IStorageTreeSelectField>();
//			Mapper.CreateMap<IStorageField, DbBaseField>()
//				.Include<IStorageAutoNumberField, DbAutoNumberField>()
//				.Include<IStorageCheckboxField, DbCheckboxField>()
//				.Include<IStorageCurrencyField, DbCurrencyField>()
//				.Include<IStorageDateField, DbDateField>()
//				.Include<IStorageDateTimeField, DbDateTimeField>()
//				.Include<IStorageEmailField, DbEmailField>()
//				.Include<IStorageFileField, DbFileField>()
//				.Include<IStorageGuidField, DbGuidField>()
//				.Include<IStorageHtmlField, DbHtmlField>()
//				.Include<IStorageImageField, DbImageField>()
//				.Include<IStorageMultiSelectField, DbMultiSelectField>()
//				.Include<IStorageNumberField, DbNumberField>()
//				.Include<IStoragePasswordField, DbPasswordField>()
//				.Include<IStoragePercentField, DbPercentField>()
//				.Include<IStoragePhoneField, DbPhoneField>()
//				.Include<IStorageSelectField, DbSelectField>()
//				.Include<IStorageTextField, DbTextField>()
//				.Include<IStorageUrlField, DbUrlField>()
//				.Include<IStorageTreeSelectField, DbTreeSelectField>();
//		}

//		protected IStorageField CreateEmptyFieldObject(DbBaseField field)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
//		}

//		protected IStorageAutoNumberField CreateEmptyAutoNumberFieldObject(DbAutoNumberField field)
//		{
//			var storageService = service.StorageService;
//			return (IStorageAutoNumberField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
//		}

//		protected IStorageCheckboxField CreateEmptyCheckboxFieldObject(DbCheckboxField field)
//		{
//			var storageService = service.StorageService;
//			return (IStorageCheckboxField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
//		}
//		protected IStorageCurrencyField CreateEmptyCurrencyFieldObject(DbCurrencyField field)
//		{
//			var storageService = service.StorageService;
//			return (IStorageCurrencyField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
//		}

//		protected IStorageDateField CreateEmptyDateFieldObject(DbDateField field)
//		{
//			var storageService = service.StorageService;
//			return (IStorageDateField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
//		}

//		protected IStorageDateTimeField CreateEmptyDateTimeFieldObject(DbDateTimeField field)
//		{
//			var storageService = service.StorageService;
//			return (IStorageDateTimeField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
//		}

//		protected IStorageEmailField CreateEmptyEmailFieldObject(DbEmailField field)
//		{
//			var storageService = service.StorageService;
//			return (IStorageEmailField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
//		}

//		protected IStorageFileField CreateEmptyFileFieldObject(DbFileField field)
//		{
//			var storageService = service.StorageService;
//			return (IStorageFileField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
//		}

//		protected IStorageGuidField CreateEmptyGuidFieldObject(DbGuidField field)
//		{
//			var storageService = service.StorageService;
//			return (IStorageGuidField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
//		}

//		protected IStorageHtmlField CreateEmptyHtmlFieldObject(DbHtmlField field)
//		{
//			var storageService = service.StorageService;
//			return (IStorageHtmlField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
//		}

//		protected IStorageImageField CreateEmptyImageFieldObject(DbImageField field)
//		{
//			var storageService = service.StorageService;
//			return (IStorageImageField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
//		}

//		protected IStorageMultiLineTextField CreateEmptyMultiLineTextFieldObject(DbMultiLineTextField field)
//		{
//			var storageService = service.StorageService;
//			return (IStorageMultiLineTextField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
//		}

//		protected IStorageMultiSelectField CreateEmptyMultiSelectFieldObject(DbMultiSelectField field)
//		{
//			var storageService = service.StorageService;
//			return (IStorageMultiSelectField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
//		}

//		protected IStorageNumberField CreateEmptyNumberFieldObject(DbNumberField field)
//		{
//			var storageService = service.StorageService;
//			return (IStorageNumberField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
//		}

//		protected IStoragePasswordField CreateEmptyPasswordFieldObject(DbPasswordField field)
//		{
//			var storageService = service.StorageService;
//			return (IStoragePasswordField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
//		}

//		protected IStoragePercentField CreateEmptyPercentFieldObject(DbPercentField field)
//		{
//			var storageService = service.StorageService;
//			return (IStoragePercentField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
//		}

//		protected IStoragePhoneField CreateEmptyPhoneFieldObject(DbPhoneField field)
//		{
//			var storageService = service.StorageService;
//			return (IStoragePhoneField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
//		}

//		protected IStorageSelectField CreateEmptySelectFieldObject(DbSelectField field)
//		{
//			var storageService = service.StorageService;
//			return (IStorageSelectField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
//		}

//		protected IStorageTextField CreateEmptyTextFieldObject(DbTextField field)
//		{
//			var storageService = service.StorageService;
//			return (IStorageTextField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
//		}

//		protected IStorageUrlField CreateEmptyUrlFieldObject(DbUrlField field)
//		{
//			var storageService = service.StorageService;
//			return (IStorageUrlField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
//		}

//		protected IStorageTreeSelectField CreateEmptyTreeSelectFieldObject(DbTreeSelectField field)
//		{
//			var storageService = service.StorageService;
//			return (IStorageTreeSelectField)storageService.GetObjectFactory().CreateEmptyFieldObject(field.GetType());
//		}

//		protected IStorageSelectFieldOption CreateEmptySelectFieldOptionObject(DbSelectFieldOption field)
//		{
//			var storageService = service.StorageService;
//			return (IStorageSelectFieldOption)storageService.GetObjectFactory().CreateEmptySelectFieldOptionObject();
//		}

//		protected IStorageMultiSelectFieldOption CreateEmptyMultiSelectFieldOptionObject(DbMultiSelectFieldOption field)
//		{
//			var storageService = service.StorageService;
//			return (IStorageMultiSelectFieldOption)storageService.GetObjectFactory().CreateEmptyMultiSelectFieldOptionObject();
//		}

//		protected IStorageCurrencyType CreateEmptyCurrencyTypeObject(DbCurrencyType field)
//		{
//			var storageService = service.StorageService;
//			return (IStorageCurrencyType)storageService.GetObjectFactory().CreateEmptyCurrencyTypeObject();
//		}
//	}
//}
