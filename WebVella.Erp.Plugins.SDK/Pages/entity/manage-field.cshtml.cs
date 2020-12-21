using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.AutoMapper;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Plugins.SDK.Utils;
using WebVella.Erp.Utilities;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Plugins.SDK.Pages.ErpEntity
{
	public class ManageFieldModel : BaseErpPageModel
	{
		public ManageFieldModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public Entity ErpEntity { get; set; }

		public Field Field { get; set; }

		public EntityRecord FieldCard { get; set; } = null;

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
		public decimal? StartingNumber { get; set; } = 1;

		[BindProperty]
		public string DisplayFormat { get; set; } = "";

		[BindProperty]
		public string CurrencyCode { get; set; } = "USD";

        [BindProperty]
        public string GeographyFormat { get; set; } = "1";

        [BindProperty]
        public string SRID { get; set; }

        public List<SelectOption> CurrencyOptions { get; set; } = new List<SelectOption>();

        public List<SelectOption> GeographyFormats { get; set; } = new List<SelectOption>();

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
		public string SelectOptions { get; set; } = null;

		[BindProperty]
		public List<SelectOption> MultiSelectFieldOptions { get; set; } = new List<SelectOption>();

		[BindProperty]
		public List<string> DefaultAreaValues { get; set; } = new List<string>();

		[BindProperty]
		public List<SelectOption> SelectFieldOptions { get; set; } = new List<SelectOption>();

		[BindProperty]
		public string FieldPermissions { get; set; } = "[]";

		public List<SelectOption> RoleOptions { get; set; } = new List<SelectOption>();

		public List<SelectOption> PermissionOptions { get; set; } = new List<SelectOption>();

		public List<string> HeaderActions { get; private set; } = new List<string>();

		public List<string> HeaderToolbar { get; private set; } = new List<string>();

		public void InitPage()
		{
			var entMan = new EntityManager();
			ErpEntity = entMan.ReadEntity(ParentRecordId ?? Guid.Empty).Object;
			if (ErpEntity == null) return;

			Field = ErpEntity.Fields.FirstOrDefault(x => x.Id == RecordId);
			if (Field == null) return;

			#region << Init Field Values >>
			if (PageContext.HttpContext.Request.Method == "GET")
			{
				switch (Field.GetFieldType())
				{
					case FieldType.AutoNumberField:
						{
							var typedInput = (AutoNumberField)Field;
							Name = typedInput.Name;
							Label = typedInput.Label;
							Required = typedInput.Required;
							Description = typedInput.Description;
							Unique = typedInput.Unique;
							HelpText = typedInput.HelpText;
							System = typedInput.System;
							PlaceholderText = typedInput.PlaceholderText;
							Searchable = typedInput.Searchable;
							DefaultValue = (typedInput.DefaultValue != null ? typedInput.DefaultValue.ToString() : "");
							StartingNumber = typedInput.StartingNumber;
							DisplayFormat = typedInput.DisplayFormat;
							EnableSecurity = typedInput.EnableSecurity;
						}
						break;
					case FieldType.CheckboxField:
						{
							var typedInput = (CheckboxField)Field;
							Name = typedInput.Name;
							Label = typedInput.Label;
							Required = typedInput.Required;
							Description = typedInput.Description;
							Unique = typedInput.Unique;
							HelpText = typedInput.HelpText;
							System = typedInput.System;
							PlaceholderText = typedInput.PlaceholderText;
							Searchable = typedInput.Searchable;
							DefaultValue = (typedInput.DefaultValue != null ? typedInput.DefaultValue.ToString().ToLowerInvariant() : "");
							EnableSecurity = typedInput.EnableSecurity;
						}
						break;
					case FieldType.CurrencyField:
						{
							var typedInput = (CurrencyField)Field;
							Name = typedInput.Name;
							Label = typedInput.Label;
							Required = typedInput.Required;
							Description = typedInput.Description;
							Unique = typedInput.Unique;
							HelpText = typedInput.HelpText;
							System = typedInput.System;
							PlaceholderText = typedInput.PlaceholderText;
							Searchable = typedInput.Searchable;
							DefaultValue = (typedInput.DefaultValue != null ? typedInput.DefaultValue.ToString() : "");
							EnableSecurity = typedInput.EnableSecurity;
							MinValue = typedInput.MinValue;
							MaxValue = typedInput.MaxValue;
							CurrencyCode = typedInput.Currency.Code;
						}
						break;
					case FieldType.DateField:
						{
							var typedInput = (DateField)Field;
							Name = typedInput.Name;
							Label = typedInput.Label;
							Required = typedInput.Required;
							Description = typedInput.Description;
							Unique = typedInput.Unique;
							HelpText = typedInput.HelpText;
							System = typedInput.System;
							PlaceholderText = typedInput.PlaceholderText;
							Searchable = typedInput.Searchable;
                            DefaultValue = typedInput.UseCurrentTimeAsDefaultValue.HasValue && typedInput.UseCurrentTimeAsDefaultValue.Value ? null : (typedInput.DefaultValue != null ? typedInput.DefaultValue.ToString() : "");
							EnableSecurity = typedInput.EnableSecurity;
							Format = typedInput.Format;
							UseCurrentTimeAsDefaultValue = typedInput.UseCurrentTimeAsDefaultValue ?? false;
						}
						break;
					case FieldType.DateTimeField:
						{
							var typedInput = (DateTimeField)Field;
							Name = typedInput.Name;
							Label = typedInput.Label;
							Required = typedInput.Required;
							Description = typedInput.Description;
							Unique = typedInput.Unique;
							HelpText = typedInput.HelpText;
							System = typedInput.System;
							PlaceholderText = typedInput.PlaceholderText;
							Searchable = typedInput.Searchable;
							DefaultValue = typedInput.UseCurrentTimeAsDefaultValue.HasValue && typedInput.UseCurrentTimeAsDefaultValue.Value ? null : (typedInput.DefaultValue != null ? typedInput.DefaultValue.ToString() : "");
							EnableSecurity = typedInput.EnableSecurity;
							Format = typedInput.Format;
							UseCurrentTimeAsDefaultValue = typedInput.UseCurrentTimeAsDefaultValue ?? false;
						}
						break;
					case FieldType.EmailField:
						{
							var typedInput = (EmailField)Field;
							Name = typedInput.Name;
							Label = typedInput.Label;
							Required = typedInput.Required;
							Description = typedInput.Description;
							Unique = typedInput.Unique;
							HelpText = typedInput.HelpText;
							System = typedInput.System;
							PlaceholderText = typedInput.PlaceholderText;
							Searchable = typedInput.Searchable;
							DefaultValue = (typedInput.DefaultValue != null ? typedInput.DefaultValue.ToString() : "NULL");
							EnableSecurity = typedInput.EnableSecurity;
							MaxLength = typedInput.MaxLength;
						}
						break;
					case FieldType.FileField:
						{
							var typedInput = (FileField)Field;
							Name = typedInput.Name;
							Label = typedInput.Label;
							Required = typedInput.Required;
							Description = typedInput.Description;
							Unique = typedInput.Unique;
							HelpText = typedInput.HelpText;
							System = typedInput.System;
							PlaceholderText = typedInput.PlaceholderText;
							Searchable = typedInput.Searchable;
							DefaultValue = (typedInput.DefaultValue != null ? typedInput.DefaultValue.ToString() : "NULL");
							EnableSecurity = typedInput.EnableSecurity;
						}
						break;
					case FieldType.HtmlField:
						{
							var typedInput = (HtmlField)Field;
							Name = typedInput.Name;
							Label = typedInput.Label;
							Required = typedInput.Required;
							Description = typedInput.Description;
							Unique = typedInput.Unique;
							HelpText = typedInput.HelpText;
							System = typedInput.System;
							PlaceholderText = typedInput.PlaceholderText;
							Searchable = typedInput.Searchable;
							DefaultValue = (typedInput.DefaultValue != null ? typedInput.DefaultValue.ToString() : "NULL");
							EnableSecurity = typedInput.EnableSecurity;
						}
						break;
					case FieldType.ImageField:
						{
							var typedInput = (ImageField)Field;
							Name = typedInput.Name;
							Label = typedInput.Label;
							Required = typedInput.Required;
							Description = typedInput.Description;
							Unique = typedInput.Unique;
							HelpText = typedInput.HelpText;
							System = typedInput.System;
							PlaceholderText = typedInput.PlaceholderText;
							Searchable = typedInput.Searchable;
							DefaultValue = (typedInput.DefaultValue != null ? typedInput.DefaultValue.ToString() : "NULL");
							EnableSecurity = typedInput.EnableSecurity;
						}
						break;
					case FieldType.MultiLineTextField:
						{
							var typedInput = (MultiLineTextField)Field;
							Name = typedInput.Name;
							Label = typedInput.Label;
							Required = typedInput.Required;
							Description = typedInput.Description;
							Unique = typedInput.Unique;
							HelpText = typedInput.HelpText;
							System = typedInput.System;
							PlaceholderText = typedInput.PlaceholderText;
							Searchable = typedInput.Searchable;
							DefaultValue = (typedInput.DefaultValue != null ? typedInput.DefaultValue.ToString() : "NULL");
							EnableSecurity = typedInput.EnableSecurity;
							MaxLength = typedInput.MaxLength;
						}
						break;
                    case FieldType.GeographyField:
                        {
                            var typedInput = (GeographyField)Field;
                            SRID = typedInput.SRID.ToString();
                            GeographyFormat = typedInput.Format.ToString();
                            Name = typedInput.Name;
                            Label = typedInput.Label;
                            Required = typedInput.Required;
                            Description = typedInput.Description;
                            Unique = typedInput.Unique;
                            HelpText = typedInput.HelpText;
                            System = typedInput.System;
                            PlaceholderText = typedInput.PlaceholderText;
                            Searchable = typedInput.Searchable;
                            DefaultValue = (typedInput.DefaultValue != null ? typedInput.DefaultValue.ToString() : "NULL");
                            EnableSecurity = typedInput.EnableSecurity;
                            MaxLength = typedInput.MaxLength;
                        }
                        break;
                    case FieldType.MultiSelectField:
						{
							var typedInput = (MultiSelectField)Field;
							Name = typedInput.Name;
							Label = typedInput.Label;
							Required = typedInput.Required;
							Description = typedInput.Description;
							Unique = typedInput.Unique;
							HelpText = typedInput.HelpText;
							System = typedInput.System;
							PlaceholderText = typedInput.PlaceholderText;
							Searchable = typedInput.Searchable;
							EnableSecurity = typedInput.EnableSecurity;
							MultiSelectFieldOptions = typedInput.Options;

							SelectOptions = "";
							DefaultValue = "";
							foreach (var option in typedInput.Options)
							{
								if (String.IsNullOrWhiteSpace(option.IconClass))
								{
									SelectOptions += $"{option.Value},{option.Label}" + Environment.NewLine;
								}
								else
								{
									if (String.IsNullOrWhiteSpace(option.Color))
									{
										SelectOptions += $"{option.Value},{option.Label},{option.IconClass}" + Environment.NewLine;
									}
									else
									{
										SelectOptions += $"{option.Value},{option.Label},{option.IconClass},{option.Color}" + Environment.NewLine;
									}
								}
							}
							foreach (var option in typedInput.DefaultValue)
							{
								DefaultValue += $"{option}" + Environment.NewLine;
							}
						}
						break;
					case FieldType.NumberField:
						{
							var typedInput = (NumberField)Field;
							Name = typedInput.Name;
							Label = typedInput.Label;
							Required = typedInput.Required;
							Description = typedInput.Description;
							Unique = typedInput.Unique;
							HelpText = typedInput.HelpText;
							System = typedInput.System;
							PlaceholderText = typedInput.PlaceholderText;
							Searchable = typedInput.Searchable;
							DefaultValue = (typedInput.DefaultValue != null ? typedInput.DefaultValue.ToString() : "");
							EnableSecurity = typedInput.EnableSecurity;
							MinValue = typedInput.MinValue;
							MaxValue = typedInput.MaxValue;
							DecimalPlaces = typedInput.DecimalPlaces ?? 2;
						}
						break;
					case FieldType.PasswordField:
						{
							var typedInput = (PasswordField)Field;
							Name = typedInput.Name;
							Label = typedInput.Label;
							Required = typedInput.Required;
							Description = typedInput.Description;
							Unique = typedInput.Unique;
							HelpText = typedInput.HelpText;
							System = typedInput.System;
							PlaceholderText = typedInput.PlaceholderText;
							Searchable = typedInput.Searchable;
							MinLength = typedInput.MinLength;
							Encrypted = typedInput.Encrypted ?? false;
							EnableSecurity = typedInput.EnableSecurity;
							MaxLength = typedInput.MaxLength;
						}
						break;
					case FieldType.PercentField:
						{
							var typedInput = (PercentField)Field;
							Name = typedInput.Name;
							Label = typedInput.Label;
							Required = typedInput.Required;
							Description = typedInput.Description;
							Unique = typedInput.Unique;
							HelpText = typedInput.HelpText;
							System = typedInput.System;
							PlaceholderText = typedInput.PlaceholderText;
							Searchable = typedInput.Searchable;
							DefaultValue = (typedInput.DefaultValue != null ? typedInput.DefaultValue.ToString() : "");
							EnableSecurity = typedInput.EnableSecurity;
							MinValue = typedInput.MinValue;
							MaxValue = typedInput.MaxValue;
							DecimalPlaces = typedInput.DecimalPlaces ?? 2;
						}
						break;
					case FieldType.PhoneField:
						{
							var typedInput = (PhoneField)Field;
							Name = typedInput.Name;
							Label = typedInput.Label;
							Required = typedInput.Required;
							Description = typedInput.Description;
							Unique = typedInput.Unique;
							HelpText = typedInput.HelpText;
							System = typedInput.System;
							PlaceholderText = typedInput.PlaceholderText;
							Searchable = typedInput.Searchable;
							DefaultValue = (typedInput.DefaultValue != null ? typedInput.DefaultValue.ToString() : "NULL");
							EnableSecurity = typedInput.EnableSecurity;
							MaxLength = typedInput.MaxLength;
						}
						break;
					case FieldType.GuidField:
						{
							var typedInput = (GuidField)Field;
							Name = typedInput.Name;
							Label = typedInput.Label;
							Required = typedInput.Required;
							Description = typedInput.Description;
							Unique = typedInput.Unique;
							HelpText = typedInput.HelpText;
							System = typedInput.System;
							PlaceholderText = typedInput.PlaceholderText;
							Searchable = typedInput.Searchable;
							DefaultValue = (typedInput.DefaultValue != null ? typedInput.DefaultValue.ToString() : "");
							EnableSecurity = typedInput.EnableSecurity;
							GenerateNewId = typedInput.GenerateNewId ?? false;
						}
						break;
					case FieldType.SelectField:
						{
							var typedInput = (SelectField)Field;
							Name = typedInput.Name;
							Label = typedInput.Label;
							Required = typedInput.Required;
							Description = typedInput.Description;
							Unique = typedInput.Unique;
							HelpText = typedInput.HelpText;
							System = typedInput.System;
							PlaceholderText = typedInput.PlaceholderText;
							Searchable = typedInput.Searchable;
							DefaultValue = (typedInput.DefaultValue != null ? typedInput.DefaultValue.ToString() : "");
							EnableSecurity = typedInput.EnableSecurity;
							SelectFieldOptions = typedInput.Options;

							SelectOptions = "";
							foreach (var option in typedInput.Options)
							{
								if (String.IsNullOrWhiteSpace(option.IconClass))
								{
									SelectOptions += $"{option.Value},{option.Label}" + Environment.NewLine;
								}
								else
								{
									if (String.IsNullOrWhiteSpace(option.Color))
									{
										SelectOptions += $"{option.Value},{option.Label},{option.IconClass}" + Environment.NewLine;
									}
									else
									{
										SelectOptions += $"{option.Value},{option.Label},{option.IconClass},{option.Color}" + Environment.NewLine;
									}
								}
							}
						}
						break;
					case FieldType.TextField:
						{
							var typedInput = (TextField)Field;
							Name = typedInput.Name;
							Label = typedInput.Label;
							Required = typedInput.Required;
							Description = typedInput.Description;
							Unique = typedInput.Unique;
							HelpText = typedInput.HelpText;
							System = typedInput.System;
							PlaceholderText = typedInput.PlaceholderText;
							Searchable = typedInput.Searchable;
							DefaultValue = (typedInput.DefaultValue != null ? typedInput.DefaultValue.ToString() : "NULL");
							EnableSecurity = typedInput.EnableSecurity;
							MaxLength = typedInput.MaxLength;
						}
						break;
					case FieldType.UrlField:
						{
							var typedInput = (UrlField)Field;
							Name = typedInput.Name;
							Label = typedInput.Label;
							Required = typedInput.Required;
							Description = typedInput.Description;
							Unique = typedInput.Unique;
							HelpText = typedInput.HelpText;
							System = typedInput.System;
							PlaceholderText = typedInput.PlaceholderText;
							Searchable = typedInput.Searchable;
							DefaultValue = (typedInput.DefaultValue != null ? typedInput.DefaultValue.ToString() : "NULL");
							EnableSecurity = typedInput.EnableSecurity;
							MaxLength = typedInput.MaxLength;
							OpenTargetInNewWindow = typedInput.OpenTargetInNewWindow ?? false;
						}
						break;
					default:
						throw new Exception("Field Type not recognized");
				}
			}

			#endregion

			var allCards = AdminPageUtils.GetFieldCards();

			FieldCard = allCards.First(x => (string)x["type"] == ((int)Field.GetFieldType()).ToString());

			#region << Field Type init >>
			switch (Field.GetFieldType())
			{
				case FieldType.CurrencyField:
					CurrencyOptions = Helpers.GetAllCurrency().MapTo<SelectOption>();
					break;
				case FieldType.DateTimeField:
					Format = "yyyy-MMM-dd HH:mm";
					break;
                case FieldType.GeographyField:
                    foreach (int format in Enum.GetValues(typeof(GeographyFieldFormat)))
                    {
                        string value = format.ToString();
                        string name = ((GeographyFieldFormat)format).ToString();
                        GeographyFormats.Add(new SelectOption(value, name));
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
				if (Field.Permissions.CanRead.Contains(role.Id))
				{
					keyValuesObj.Values.Add("read");
				}
				if (Field.Permissions.CanUpdate.Contains(role.Id))
				{
					keyValuesObj.Values.Add("update");
				}
				valueGrid.Add(keyValuesObj);
			}
			if (HttpContext.Request.Method == "GET")
			{
				FieldPermissions = JsonConvert.SerializeObject(valueGrid);
			}
			#endregion

			#region << Actions >>
			HeaderActions.AddRange(new List<string>() {

				PageUtils.GetActionTemplate(PageUtilsActionType.SubmitForm, label: "Save Field",formId:"ManageRecord"),
				PageUtils.GetActionTemplate(PageUtilsActionType.Cancel, returnUrl: ReturnUrl)
			});

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
				ReturnUrl = $"/sdk/objects/entity/r/{RecordId}/rl/fields/c/select";
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

			InitPage();

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
				var fieldId = Field.Id;
				var response = new FieldResponse();
				InputField input = null;
				switch (Field.GetFieldType())
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
								Id = fieldId,
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
							bool? defaultDecimal = null;
							if (Boolean.TryParse(DefaultValue, out bool result))
							{
								defaultDecimal = result;
							}
							input = new InputCheckboxField()
							{
								Id = fieldId,
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
								Id = fieldId,
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
							DateTime? defaultAsValue = null;
							if (DateTime.TryParse(DefaultValue, out DateTime result))
							{
								defaultAsValue = result;
							}
							input = new InputDateField()
							{
								Id = fieldId,
								Name = Name,
								Label = Label,
								Required = Required,
								Description = Description,
								Unique = Unique,
								HelpText = HelpText,
								System = System,
								PlaceholderText = PlaceholderText,
								Searchable = Searchable,
								DefaultValue = defaultAsValue,
								EnableSecurity = EnableSecurity,
								Format = Format,
								UseCurrentTimeAsDefaultValue = UseCurrentTimeAsDefaultValue
							};
						}
						break;
					case FieldType.DateTimeField:
						{
							DateTime? defaultAsValue = null;
							if (DateTime.TryParse(DefaultValue, out DateTime result))
							{
								defaultAsValue = result;
							}
							input = new InputDateTimeField()
							{
								Id = fieldId,
								Name = Name,
								Label = Label,
								Required = Required,
								Description = Description,
								Unique = Unique,
								HelpText = HelpText,
								System = System,
								PlaceholderText = PlaceholderText,
								Searchable = Searchable,
								DefaultValue = defaultAsValue,
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
								Id = fieldId,
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
								Id = fieldId,
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
								Id = fieldId,
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
								Id = fieldId,
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
								Id = fieldId,
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

                            input = new InputGeographyField()
                            {
                                Id = fieldId,
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
                    case FieldType.MultiSelectField:
						{
							var selectOptions = SelectOptions.Split(Environment.NewLine);
							var defaultOptions = DefaultValue.Split(Environment.NewLine);
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
								Id = fieldId,
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
								Id = fieldId,
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
								Id = fieldId,
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
								Id = fieldId,
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
							if (DefaultValue != null && DefaultValue.ToLowerInvariant() != "null")
							{
								defaultValue = DefaultValue;
							}

							input = new InputPhoneField()
							{
								Id = fieldId,
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
								Id = fieldId,
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
									if (optionArray.Length > 2 && !String.IsNullOrWhiteSpace(optionArray[2])) { 
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
								Id = fieldId,
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
								Id = fieldId,
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
								Id = fieldId,
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

				response = entMan.UpdateField(ErpEntity.Id, input);
				if (!response.Success)
				{
					var exception = new ValidationException(response.Message);
					exception.Errors = response.Errors.MapTo<ValidationError>();
					throw exception;
				}
				return Redirect($"/sdk/objects/entity/r/{ErpEntity.Id}/rl/fields/r/{Field.Id}");
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

			BeforeRender();
			return Page();
		}
	}
}
