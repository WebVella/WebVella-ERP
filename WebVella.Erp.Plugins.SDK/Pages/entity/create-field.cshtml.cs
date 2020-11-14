using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.AutoMapper;
using WebVella.Erp.Database;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Plugins.SDK.Utils;
using WebVella.Erp.Utilities;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Utils;
using WebVella.TagHelpers.Models;

namespace WebVella.Erp.Plugins.SDK.Pages.ErpEntity
{
    public class CreateFieldModel : BaseErpPageModel
    {
        public CreateFieldModel([FromServices] ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

        public Entity ErpEntity { get; set; }

        [BindProperty(SupportsGet = true)]
        public int FieldTypeId { get; set; } = 18;

        [BindProperty]
        public Guid? Id { get; set; } = null;

        [BindProperty]
        public string Name { get; set; } = "";

        [BindProperty]
        public string Label { get; set; } = "";

        [BindProperty]
        public bool Required { get; set; } = false;

        [BindProperty]
        public string Description { get; set; } = "";

        [BindProperty]
        public bool Unique { get; set; } = false;

        public WvFieldRenderMode UniqueRenderMode
        {
            get
            {
                if (Type == FieldType.GeographyField)
                {
                    return WvFieldRenderMode.Display; // can't make unique indexes on geography type
                }
                else
                {
                    return WvFieldRenderMode.Undefined;
                }
            }
        }

        [BindProperty]
        public string HelpText { get; set; } = "";

        [BindProperty]
        public bool System { get; set; } = false;

        [BindProperty]
        public string PlaceholderText { get; set; } = "";

        [BindProperty]
        public bool Searchable { get; set; } = false;

        [BindProperty]
        public bool EnableSecurity { get; set; } = false;

        [BindProperty]
        public string DefaultValue { get; set; } = null;

        [BindProperty]
        public string SelectOptions { get; set; } = null;

        [BindProperty]
        public decimal? StartingNumber { get; set; } = 1;

        [BindProperty]
        public string DisplayFormat { get; set; } = "";

        [BindProperty]
        public string CurrencyCode { get; set; } = "USD";

        [BindProperty]
        public string GeographyFormat { get; set; } = "1"; // matches .GeoJSON

        public List<SelectOption> CurrencyOptions { get; set; } = new List<SelectOption>();

        public List<SelectOption> GeographyFormats { get; set; } = new List<SelectOption>();

        [BindProperty]
        public int SRID { get; set; } = ErpSettings.DefaultSRID;

        [BindProperty]
        public decimal? MinValue { get; set; } = null;

        [BindProperty]
        public decimal? MaxValue { get; set; } = null;

        [BindProperty]
        public string Format { get; set; } = "yyyy-MMM-dd";

        [BindProperty]
        public bool UseCurrentTimeAsDefaultValue { get; set; } = false;

        [BindProperty]
        public int? MaxLength { get; set; } = null;

        [BindProperty]
        public byte DecimalPlaces { get; set; } = 2;

        [BindProperty]
        public int? MinLength { get; set; } = null;

        [BindProperty]
        public bool Encrypted { get; set; } = true;

        [BindProperty]
        public bool GenerateNewId { get; set; } = false;

        [BindProperty]
        public bool OpenTargetInNewWindow { get; set; } = false;


        [BindProperty]
        public string FieldPermissions { get; set; } = "[]";

        public List<SelectOption> RoleOptions { get; set; } = new List<SelectOption>();

        public List<SelectOption> PermissionOptions { get; set; } = new List<SelectOption>();


        public CurrencyType Currency { get; set; } = null;

        public EntityRecord FieldCard { get; set; } = null;

        public FieldType Type { get; set; } = FieldType.TextField;

        public List<string> HeaderActions { get; private set; } = new List<string>();

        public List<string> HeaderToolbar { get; private set; } = new List<string>();

        public bool IsPostgisInstalled { get; set; } = false;

        public void InitPage(bool isGet = true)
        {
            IsPostgisInstalled = DbRepository.IsPostgisInstalled();

            var entMan = new EntityManager();
            ErpEntity = entMan.ReadEntity(ParentRecordId ?? Guid.Empty).Object;

            var allCards = AdminPageUtils.GetFieldCards();

            if (FieldTypeId == 20) //RelationField
                throw new Exception("RelationField is unsupported field type");

            FieldCard = allCards.First(x => (string)x["type"] == FieldTypeId.ToString());

            if (Enum.TryParse<FieldType>(FieldTypeId.ToString(), out FieldType result))
            {
                Type = result;
            }

            if (isGet)
            {
                #region << Field Type init >>
                switch (Type)
                {
                    case FieldType.AutoNumberField:
                        DisplayFormat = "{0}";
                        break;
                    case FieldType.CurrencyField:
                        CurrencyOptions = Helpers.GetAllCurrency().MapTo<SelectOption>();
                        break;
                    case FieldType.DateTimeField:
                        Format = "yyyy-MMM-dd HH:mm";
                        break;
                    case FieldType.GeographyField:
                        {
                            foreach (int format in Enum.GetValues(typeof(GeographyFieldFormat)))
                            {
                                string value = format.ToString();
                                string name = ((GeographyFieldFormat)format).ToString();
                                GeographyFormats.Add(new SelectOption(value, name));
                            }
                        }
                        break;
                    default:
                        break;
                }

                #endregion

                #region << Init RecordPermissions >>
                var valueGrid = new List<KeyStringList>();
                PermissionOptions = new List<SelectOption>() {
                                new SelectOption("read","read"),
                                new SelectOption("update","update")
                            };

                var roles = AdminPageUtils.GetUserRoles(); //Special order is applied

                foreach (var role in roles)
                {
                    RoleOptions.Add(new SelectOption(role.Id.ToString(), role.Name));
                    var keyValuesObj = new KeyStringList()
                    {
                        Key = role.Id.ToString(),
                        Values = new List<string>()
                    };

                }

                #endregion
            }

            #region << Actions >>

            if (Type != FieldType.GeographyField)
            {
                HeaderActions.AddRange(new List<string>() {
                    PageUtils.GetActionTemplate(PageUtilsActionType.SubmitForm, label: "Create Field",formId:"CreateRecord", btnClass:"btn btn-green btn-sm", iconClass:"fa fa-plus"),
                    PageUtils.GetActionTemplate(PageUtilsActionType.Cancel, returnUrl: ReturnUrl)
            });
            }
            else if (Type == FieldType.GeographyField)
            {
                if (IsPostgisInstalled)
                    HeaderActions.Add(PageUtils.GetActionTemplate(PageUtilsActionType.SubmitForm, label: "Create Field", formId: "CreateRecord", btnClass: "btn btn-green btn-sm", iconClass: "fa fa-plus"));

                HeaderActions.Add(PageUtils.GetActionTemplate(PageUtilsActionType.Cancel, returnUrl: ReturnUrl));
            }
            #endregion

        }
        public IActionResult OnGet()
        {
            var initResult = Init();
            if (initResult != null)
                return initResult;

            InitPage();

            if (ErpEntity == null)
            {
                return NotFound();
            }

            if (String.IsNullOrWhiteSpace(ReturnUrl))
            {
                ReturnUrl = $"/sdk/objects/entity/r/{ParentRecordId}/rl/fields/c/select";
            }

            HeaderToolbar.AddRange(AdminPageUtils.GetEntityAdminSubNav(ErpEntity, "fields"));

            ErpRequestContext.PageContext = PageContext;

            BeforeRender();
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) throw new Exception("Antiforgery check failed.");

            var initResult = Init();
            if (initResult != null)
                return initResult;

            InitPage(false);

            if (ErpEntity == null)
            {
                return NotFound();
            }

            if (String.IsNullOrWhiteSpace(ReturnUrl))
            {
                ReturnUrl = $"/sdk/objects/entity/r/{RecordId}/rl/fields/c/select";
            }

            //empty html input is not posted, so we init it with string.empty
            if (DefaultValue == null)
                DefaultValue = string.Empty;

            var entMan = new EntityManager();
            try
            {
                var newFieldId = Guid.NewGuid();
                if (Id != null)
                    newFieldId = Id.Value;

                var response = new FieldResponse();
                InputField input = null;
                switch (Type)
                {
                    case FieldType.AutoNumberField:
                        {
                            decimal defaultDecimal = 1;
                            if (Decimal.TryParse(DefaultValue, out decimal result))
                            {
                                defaultDecimal = result;
                            }

                            input = new InputAutoNumberField()
                            {
                                Id = newFieldId,
                                Name = Name,
                                Label = Label,
                                Required = Required,
                                Description = Description,
                                Unique = Unique,
                                HelpText = HelpText,
                                System = System,
                                PlaceholderText = PlaceholderText,
                                Searchable = Searchable,
                                DefaultValue = defaultDecimal,
                                StartingNumber = StartingNumber,
                                DisplayFormat = DisplayFormat,
                                EnableSecurity = EnableSecurity
                            };
                        }
                        break;
                    case FieldType.CheckboxField:
                        {
                            bool? defaultValue = null;
                            if (Boolean.TryParse(DefaultValue, out bool result))
                            {
                                defaultValue = result;
                            }
                            input = new InputCheckboxField()
                            {
                                Id = newFieldId,
                                Name = Name,
                                Label = Label,
                                Required = Required,
                                Description = Description,
                                Unique = Unique,
                                HelpText = HelpText,
                                System = System,
                                PlaceholderText = PlaceholderText,
                                Searchable = Searchable,
                                DefaultValue = defaultValue,
                                EnableSecurity = EnableSecurity
                            };
                        }
                        break;
                    case FieldType.CurrencyField:
                        {
                            decimal? defaultDecimal = null;
                            if (Decimal.TryParse(DefaultValue, out decimal result))
                            {
                                defaultDecimal = result;
                            }
                            input = new InputCurrencyField()
                            {
                                Id = newFieldId,
                                Name = Name,
                                Label = Label,
                                Required = Required,
                                Description = Description,
                                Unique = Unique,
                                HelpText = HelpText,
                                System = System,
                                PlaceholderText = PlaceholderText,
                                Searchable = Searchable,
                                DefaultValue = defaultDecimal,
                                EnableSecurity = EnableSecurity,
                                MinValue = MinValue,
                                MaxValue = MaxValue,
                                Currency = Helpers.GetCurrencyType(CurrencyCode)
                            };
                        }
                        break;
                    case FieldType.DateField:
                        {
                            DateTime? defaultValue = null;
                            if (DateTime.TryParse(DefaultValue, out DateTime result))
                            {
                                defaultValue = result;
                            }
                            input = new InputDateField()
                            {
                                Id = newFieldId,
                                Name = Name,
                                Label = Label,
                                Required = Required,
                                Description = Description,
                                Unique = Unique,
                                HelpText = HelpText,
                                System = System,
                                PlaceholderText = PlaceholderText,
                                Searchable = Searchable,
                                DefaultValue = defaultValue,
                                EnableSecurity = EnableSecurity,
                                Format = Format,
                                UseCurrentTimeAsDefaultValue = UseCurrentTimeAsDefaultValue
                            };
                        }
                        break;
                    case FieldType.DateTimeField:
                        {
                            DateTime? defaultValue = null;
                            if (DateTime.TryParse(DefaultValue, out DateTime result))
                            {
                                defaultValue = result;
                            }
                            input = new InputDateTimeField()
                            {
                                Id = newFieldId,
                                Name = Name,
                                Label = Label,
                                Required = Required,
                                Description = Description,
                                Unique = Unique,
                                HelpText = HelpText,
                                System = System,
                                PlaceholderText = PlaceholderText,
                                Searchable = Searchable,
                                DefaultValue = defaultValue,
                                EnableSecurity = EnableSecurity,
                                Format = Format,
                                UseCurrentTimeAsDefaultValue = UseCurrentTimeAsDefaultValue
                            };
                        }
                        break;
                    case FieldType.EmailField:
                        {
                            string defaultValue = null;
                            if (DefaultValue.ToLowerInvariant() != "null")
                            {
                                defaultValue = DefaultValue;
                            }

                            input = new InputEmailField()
                            {
                                Id = newFieldId,
                                Name = Name,
                                Label = Label,
                                Required = Required,
                                Description = Description,
                                Unique = Unique,
                                HelpText = HelpText,
                                System = System,
                                PlaceholderText = PlaceholderText,
                                Searchable = Searchable,
                                DefaultValue = defaultValue,
                                EnableSecurity = EnableSecurity,
                                MaxLength = MaxLength
                            };
                        }
                        break;
                    case FieldType.FileField:
                        {
                            string defaultValue = null;
                            if (DefaultValue.ToLowerInvariant() != "null")
                            {
                                defaultValue = DefaultValue;
                            }

                            input = new InputFileField()
                            {
                                Id = newFieldId,
                                Name = Name,
                                Label = Label,
                                Required = Required,
                                Description = Description,
                                Unique = Unique,
                                HelpText = HelpText,
                                System = System,
                                PlaceholderText = PlaceholderText,
                                Searchable = Searchable,
                                DefaultValue = defaultValue,
                                EnableSecurity = EnableSecurity
                            };
                        }
                        break;
                    case FieldType.HtmlField:
                        {
                            string defaultValue = null;
                            if (DefaultValue.ToLowerInvariant() != "null")
                            {
                                defaultValue = DefaultValue;
                            }

                            input = new InputHtmlField()
                            {
                                Id = newFieldId,
                                Name = Name,
                                Label = Label,
                                Required = Required,
                                Description = Description,
                                Unique = Unique,
                                HelpText = HelpText,
                                System = System,
                                PlaceholderText = PlaceholderText,
                                Searchable = Searchable,
                                DefaultValue = defaultValue,
                                EnableSecurity = EnableSecurity
                            };
                        }
                        break;
                    case FieldType.ImageField:
                        {
                            string defaultValue = null;
                            if (DefaultValue.ToLowerInvariant() != "null")
                            {
                                defaultValue = DefaultValue;
                            }

                            input = new InputImageField()
                            {
                                Id = newFieldId,
                                Name = Name,
                                Label = Label,
                                Required = Required,
                                Description = Description,
                                Unique = Unique,
                                HelpText = HelpText,
                                System = System,
                                PlaceholderText = PlaceholderText,
                                Searchable = Searchable,
                                DefaultValue = defaultValue,
                                EnableSecurity = EnableSecurity
                            };
                        }
                        break;
                    case FieldType.MultiLineTextField:
                        {
                            string defaultValue = null;
                            if (DefaultValue.ToLowerInvariant() != "null")
                            {
                                defaultValue = DefaultValue;
                            }

                            input = new InputMultiLineTextField()
                            {
                                Id = newFieldId,
                                Name = Name,
                                Label = Label,
                                Required = Required,
                                Description = Description,
                                Unique = Unique,
                                HelpText = HelpText,
                                System = System,
                                PlaceholderText = PlaceholderText,
                                Searchable = Searchable,
                                DefaultValue = defaultValue,
                                EnableSecurity = EnableSecurity,
                                MaxLength = MaxLength
                            };
                        }
                        break;
                    case FieldType.GeographyField:
                        {
                            string defaultValue = null;
                            if (DefaultValue.ToLowerInvariant() != "null")
                            {
                                defaultValue = DefaultValue;
                            }
                            GeographyFieldFormat format = (GeographyFieldFormat)int.Parse(GeographyFormat);
                            input = new InputGeographyField()
                            {
                                Id = newFieldId,
                                Name = Name,
                                Label = Label,
                                Required = Required,
                                Description = Description,
                                Unique = Unique,
                                HelpText = HelpText,
                                System = System,
                                PlaceholderText = PlaceholderText,
                                Searchable = Searchable,
                                DefaultValue = defaultValue,
                                EnableSecurity = EnableSecurity,
                                MaxLength = MaxLength,
                                SRID = SRID,
                                Format = format
                            };
                        }
                        break;
                    case FieldType.MultiSelectField:
                        {
                            var selectOptions = (SelectOptions ?? string.Empty).Split(Environment.NewLine);
                            var defaultOptions = (DefaultValue ?? string.Empty).Split(Environment.NewLine);
                            var multiSelectOptions = new List<SelectOption>();
                            var defaultValues = new List<string>();

                            foreach (var option in selectOptions)
                            {
                                if (!String.IsNullOrWhiteSpace(option))
                                {
                                    var optionArray = option.Split(',');
                                    var key = "";
                                    var value = "";
                                    var iconClass = "";
                                    var color = "";
                                    if (optionArray.Length > 0 && !String.IsNullOrWhiteSpace(optionArray[0]))
                                    {
                                        key = optionArray[0].Trim().ToLowerInvariant();
                                    }
                                    if (optionArray.Length > 1 && !String.IsNullOrWhiteSpace(optionArray[1]))
                                    {
                                        value = optionArray[1].Trim();
                                    }
                                    else if (optionArray.Length > 0 && !String.IsNullOrWhiteSpace(optionArray[0]))
                                    {
                                        value = key;
                                    }
                                    if (optionArray.Length > 2 && !String.IsNullOrWhiteSpace(optionArray[2]))
                                    {
                                        iconClass = optionArray[2].Trim();
                                    }
                                    if (optionArray.Length > 3 && !String.IsNullOrWhiteSpace(optionArray[3]))
                                    {
                                        color = optionArray[3].Trim();
                                    }
                                    if (!String.IsNullOrWhiteSpace(key) && !String.IsNullOrWhiteSpace(value))
                                    {
                                        multiSelectOptions.Add(new SelectOption()
                                        {
                                            Value = key,
                                            Label = value,
                                            IconClass = iconClass,
                                            Color = color
                                        });
                                    }
                                }
                            }

                            foreach (var option in defaultOptions)
                            {
                                var fixedOption = option.Trim().ToLowerInvariant();
                                if (!String.IsNullOrWhiteSpace(option) && multiSelectOptions.Any(x => x.Value == fixedOption))
                                {
                                    defaultValues.Add(fixedOption);
                                }
                                else if (!String.IsNullOrWhiteSpace(option) && !multiSelectOptions.Any(x => x.Value == fixedOption))
                                {
                                    Validation.Errors.Add(new ValidationError("DefaultValue", "one or more of the default values are not found as select options"));
                                    throw Validation;
                                }
                            }

                            input = new InputMultiSelectField()
                            {
                                Id = newFieldId,
                                Name = Name,
                                Label = Label,
                                Required = Required,
                                Description = Description,
                                Unique = Unique,
                                HelpText = HelpText,
                                System = System,
                                PlaceholderText = PlaceholderText,
                                Searchable = Searchable,
                                EnableSecurity = EnableSecurity,
                                Options = multiSelectOptions,
                                DefaultValue = defaultValues
                            };
                        }
                        break;
                    case FieldType.NumberField:
                        {
                            decimal? defaultDecimal = null;
                            if (Decimal.TryParse(DefaultValue, out decimal result))
                            {
                                defaultDecimal = result;
                            }
                            input = new InputNumberField()
                            {
                                Id = newFieldId,
                                Name = Name,
                                Label = Label,
                                Required = Required,
                                Description = Description,
                                Unique = Unique,
                                HelpText = HelpText,
                                System = System,
                                PlaceholderText = PlaceholderText,
                                Searchable = Searchable,
                                DefaultValue = defaultDecimal,
                                EnableSecurity = EnableSecurity,
                                MinValue = MinValue,
                                MaxValue = MaxValue,
                                DecimalPlaces = DecimalPlaces
                            };
                        }
                        break;
                    case FieldType.PasswordField:
                        {
                            input = new InputPasswordField()
                            {
                                Id = newFieldId,
                                Name = Name,
                                Label = Label,
                                Required = Required,
                                Description = Description,
                                Unique = Unique,
                                HelpText = HelpText,
                                System = System,
                                PlaceholderText = PlaceholderText,
                                Searchable = Searchable,
                                MinLength = MinLength,
                                Encrypted = Encrypted,
                                EnableSecurity = EnableSecurity,
                                MaxLength = MaxLength
                            };
                        }
                        break;
                    case FieldType.PercentField:
                        {
                            decimal? defaultDecimal = null;
                            if (Decimal.TryParse(DefaultValue, out decimal result))
                            {
                                defaultDecimal = result;
                            }
                            input = new InputNumberField()
                            {
                                Id = newFieldId,
                                Name = Name,
                                Label = Label,
                                Required = Required,
                                Description = Description,
                                Unique = Unique,
                                HelpText = HelpText,
                                System = System,
                                PlaceholderText = PlaceholderText,
                                Searchable = Searchable,
                                DefaultValue = defaultDecimal,
                                EnableSecurity = EnableSecurity,
                                MinValue = MinValue,
                                MaxValue = MaxValue,
                                DecimalPlaces = DecimalPlaces
                            };
                        }
                        break;
                    case FieldType.PhoneField:
                        {
                            string defaultValue = null;
                            if (DefaultValue.ToLowerInvariant() != "null")
                            {
                                defaultValue = DefaultValue;
                            }

                            input = new InputPhoneField()
                            {
                                Id = newFieldId,
                                Name = Name,
                                Label = Label,
                                Required = Required,
                                Description = Description,
                                Unique = Unique,
                                HelpText = HelpText,
                                System = System,
                                PlaceholderText = PlaceholderText,
                                Searchable = Searchable,
                                DefaultValue = defaultValue,
                                EnableSecurity = EnableSecurity,
                                MaxLength = MaxLength
                            };
                        }
                        break;
                    case FieldType.GuidField:
                        {
                            Guid? defaultGuid = null;
                            if (Guid.TryParse(DefaultValue, out Guid result))
                            {
                                defaultGuid = result;
                            }
                            input = new InputGuidField()
                            {
                                Id = newFieldId,
                                Name = Name,
                                Label = Label,
                                Required = Required,
                                Description = Description,
                                Unique = Unique,
                                HelpText = HelpText,
                                System = System,
                                PlaceholderText = PlaceholderText,
                                Searchable = Searchable,
                                DefaultValue = defaultGuid,
                                EnableSecurity = EnableSecurity,
                                GenerateNewId = GenerateNewId
                            };
                        }
                        break;
                    case FieldType.SelectField:
                        {
                            var selectOptions = SelectOptions.Split(Environment.NewLine);
                            var modelSelectOptions = new List<SelectOption>();

                            foreach (var option in selectOptions)
                            {
                                if (!String.IsNullOrWhiteSpace(option))
                                {
                                    var optionArray = option.Split(',');
                                    var key = "";
                                    var value = "";
                                    var iconClass = "";
                                    var color = "";
                                    if (optionArray.Length > 0 && !String.IsNullOrWhiteSpace(optionArray[0]))
                                    {
                                        key = optionArray[0].Trim().ToLowerInvariant();
                                    }
                                    if (optionArray.Length > 1 && !String.IsNullOrWhiteSpace(optionArray[1]))
                                    {
                                        value = optionArray[1].Trim();
                                    }
                                    else if (optionArray.Length > 0 && !String.IsNullOrWhiteSpace(optionArray[0]))
                                    {
                                        value = key;
                                    }
                                    if (optionArray.Length > 2 && !String.IsNullOrWhiteSpace(optionArray[2]))
                                    {
                                        iconClass = optionArray[2].Trim();
                                    }
                                    if (optionArray.Length > 3 && !String.IsNullOrWhiteSpace(optionArray[3]))
                                    {
                                        color = optionArray[3].Trim();
                                    }
                                    if (!String.IsNullOrWhiteSpace(key) && !String.IsNullOrWhiteSpace(value))
                                    {
                                        modelSelectOptions.Add(new SelectOption()
                                        {
                                            Value = key,
                                            Label = value,
                                            IconClass = iconClass,
                                            Color = color
                                        });
                                    }
                                }
                            }

                            DefaultValue = DefaultValue.Trim().ToLowerInvariant();

                            if (!String.IsNullOrWhiteSpace(DefaultValue) && !modelSelectOptions.Any(x => x.Value == DefaultValue))
                            {
                                Validation.Errors.Add(new ValidationError("DefaultValue", "one or more of the default values are not found as select options"));
                                throw Validation;
                            }

                            input = new InputSelectField()
                            {
                                Id = newFieldId,
                                Name = Name,
                                Label = Label,
                                Required = Required,
                                Description = Description,
                                Unique = Unique,
                                HelpText = HelpText,
                                System = System,
                                PlaceholderText = PlaceholderText,
                                Searchable = Searchable,
                                DefaultValue = DefaultValue,
                                EnableSecurity = EnableSecurity,
                                Options = modelSelectOptions
                            };
                        }
                        break;
                    case FieldType.UrlField:
                        {
                            string defaultValue = null;
                            if (DefaultValue.ToLowerInvariant() != "null")
                            {
                                defaultValue = DefaultValue;
                            }

                            input = new InputUrlField()
                            {
                                Id = newFieldId,
                                Name = Name,
                                Label = Label,
                                Required = Required,
                                Description = Description,
                                Unique = Unique,
                                HelpText = HelpText,
                                System = System,
                                PlaceholderText = PlaceholderText,
                                Searchable = Searchable,
                                DefaultValue = defaultValue,
                                EnableSecurity = EnableSecurity,
                                MaxLength = MaxLength,
                                OpenTargetInNewWindow = OpenTargetInNewWindow
                            };
                        }
                        break;
                    case FieldType.TextField:
                        {
                            string defaultValue = null;
                            if (DefaultValue.ToLowerInvariant() != "null")
                            {
                                defaultValue = DefaultValue;
                            }

                            input = new InputTextField()
                            {
                                Id = newFieldId,
                                Name = Name,
                                Label = Label,
                                Required = Required,
                                Description = Description,
                                Unique = Unique,
                                HelpText = HelpText,
                                System = System,
                                PlaceholderText = PlaceholderText,
                                Searchable = Searchable,
                                DefaultValue = defaultValue,
                                EnableSecurity = EnableSecurity,
                                MaxLength = MaxLength
                            };
                        }
                        break;
                    default:
                        throw new Exception("Field Type not recognized");
                }

                var recordPermissionsKeyValues = JsonConvert.DeserializeObject<List<KeyStringList>>(FieldPermissions);
                input.Permissions = new FieldPermissions();
                input.Permissions.CanRead = new List<Guid>();
                input.Permissions.CanUpdate = new List<Guid>();


                foreach (var role in recordPermissionsKeyValues)
                {
                    var roleId = Guid.Empty;
                    if (Guid.TryParse(role.Key, out Guid result))
                    {
                        roleId = result;
                    }
                    if (roleId != Guid.Empty)
                    {
                        if (role.Values.Contains("read"))
                        {
                            input.Permissions.CanRead.Add(roleId);
                        }
                        if (role.Values.Contains("update"))
                        {
                            input.Permissions.CanUpdate.Add(roleId);
                        }
                    }
                }

                response = entMan.CreateField(ErpEntity.Id, input);
                if (!response.Success)
                {
                    var exception = new ValidationException(response.Message);
                    exception.Errors = response.Errors.MapTo<ValidationError>();
                    throw exception;
                }

                // because of https://github.com/aspnet/Mvc/issues/6711, i added TempDataExtensions int 
                // WebVella.Erp.Web.Utils and using Put and Get<> instead of 
                // TempData["ScreenMessage"] = new ScreenMessage() { Message = "Field created successfully" };
                TempData.Put("ScreenMessage", new ScreenMessage() { Message = "Field created successfully" });
                return Redirect($"/sdk/objects/entity/r/{ErpEntity.Id}/rl/fields/l");
            }
            catch (ValidationException ex)
            {
                Validation.Message = ex.Message;
                Validation.Errors = ex.Errors;
            }
            catch (Exception ex)
            {
                Validation.Message = ex.Message;
                Validation.Errors.Add(new ValidationError("", ex.Message, isSystem: true));
            }

            HeaderToolbar.AddRange(AdminPageUtils.GetEntityAdminSubNav(ErpEntity, "fields"));

            ErpRequestContext.PageContext = PageContext;

            BeforeRender();
            return Page();
        }
    }
}
