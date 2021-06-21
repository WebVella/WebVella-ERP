using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;
using WebVella.TagHelpers.Models;

namespace WebVella.Erp.Web.Components
{
	public abstract class PcFieldBase : PageComponent
	{

		public class PcFieldBaseOptions
		{
			[JsonProperty(PropertyName = "is_visible")]
			public string IsVisible { get; set; } = "";

			//Label
			[JsonProperty(PropertyName = "label_mode")]
			public WvLabelRenderMode LabelMode { get; set; } = WvLabelRenderMode.Undefined;

			[JsonProperty(PropertyName = "label_text")]
			public string LabelText { get; set; } = "";

			//Field
			[JsonProperty(PropertyName = "name")]
			public string Name { get; set; } = "field";

			[JsonProperty(PropertyName = "mode")]
			public WvFieldRenderMode Mode { get; set; } = WvFieldRenderMode.Undefined;

			[JsonProperty(PropertyName = "value")]
			public string Value { get; set; } = "";

			[JsonProperty(PropertyName = "connected_entity_id")]
			public Guid? ConnectedEntityId { get; set; } = null;

			[JsonProperty(PropertyName = "connected_record_id_ds")]
			public string ConnectedRecordIdDs { get; set; } = null;

			[JsonProperty(PropertyName = "access_override_ds")]
			public string AccessOverrideDs { get; set; } = "";

			[JsonProperty(PropertyName = "required_override_ds")]
			public string RequiredOverrideDs { get; set; } = ""; // bool? -> null - apply as in meta, true, false

			[JsonProperty(PropertyName = "ajax_api_url_ds")]
			public string AjaxApiUrlDs { get; set; } = null;

			[JsonProperty(PropertyName = "class")]
			public string Class { get; set; } = "";

			//Field specific options
			[JsonProperty(PropertyName = "template")]
			public string Template { get; set; } = "";

			[JsonProperty(PropertyName = "currency_code")]
			public string CurrencyCode { get; set; } = "USD";

			[JsonProperty(PropertyName = "maxlength")]
			public int? MaxLength { get; set; } = null;

			[JsonProperty(PropertyName = "min")]
			public decimal? Min { get; set; } = null;

			[JsonProperty(PropertyName = "max")]
			public decimal? Max { get; set; } = null;

			[JsonProperty(PropertyName = "step")]
			public decimal? Step { get; set; } = null;

			[JsonProperty(PropertyName = "decimal_digits")]
			public int DecimalDigits { get; set; } = 2;

			[JsonProperty(PropertyName = "label_help_text")]
			public string LabelHelpText { get; set; } = "";

			[JsonProperty(PropertyName = "description")]
			public string Description { get; set; } = "";

		}

		public class PcFieldBaseModel
		{

			//Label
			[JsonProperty(PropertyName = "label_warning_text")]
			public string LabelWarningText { get; set; } = "";

			[JsonProperty(PropertyName = "label_error_text")]
			public string LabelErrorText { get; set; } = "";

			[JsonProperty(PropertyName = "label_help_text")]
			public string LabelHelpText { get; set; } = "";


			//Field
			[JsonProperty(PropertyName = "field_id")]
			public Guid? FieldId { get; set; } = null;

			[JsonProperty(PropertyName = "class")]
			public string Class { get; set; } = "";

			[JsonProperty(PropertyName = "value")]
			public object Value { get; set; } = null;

			[JsonProperty(PropertyName = "default_value")]
			public object DefaultValue { get; set; } = null;

			[JsonProperty(PropertyName = "access")]
			public WvFieldAccess Access { get; set; } = WvFieldAccess.Full;

			[JsonProperty(PropertyName = "init_errors")]
			public List<string> InitErrors { get; set; } = new List<string>();

			[JsonProperty(PropertyName = "validation_errors")]
			public List<ValidationError> ValidationErrors { get; set; } = new List<ValidationError>();

			[JsonProperty(PropertyName = "record_id")]
			public Guid? RecordId { get; set; } = null;

			[JsonProperty(PropertyName = "entity_name")]
			public string EntityName { get; set; } = "";

			[JsonProperty(PropertyName = "api_url")]
			public string ApiUrl { get; set; } = "";

			[JsonProperty(PropertyName = "access_denied_message")]
			public string AccessDeniedMessage { get; set; } = "access denied";

			[JsonProperty(PropertyName = "empty_value_message")]
			public string EmptyValueMessage { get; set; } = "no data";

			[JsonProperty(PropertyName = "required")]
			public bool Required { get; set; } = false;

			[JsonProperty(PropertyName = "placeholder")]
			public string Placeholder { get; set; } = "";

			[JsonProperty(PropertyName = "description")]
			public string Description { get; set; } = "";

			[JsonProperty(PropertyName = "locale")]
			public string Locale { get; set; } = "";

			public CultureInfo Culture { get; set; } = new CultureInfo("en-US");

			public List<WvSelectOption> LabelRenderModeOptions { get; set; } = new List<WvSelectOption>();

			public List<WvSelectOption> FieldRenderModeOptions { get; set; } = new List<WvSelectOption>();

			public List<SelectOption> EntitySelectOptions { get; set; } = new List<SelectOption>();

		}

		public class PcFieldSelectModel : PcFieldBaseModel
		{
			[JsonProperty(PropertyName = "options")]
			public List<SelectOption> Options { get; set; } = new List<SelectOption>();

			public static PcFieldSelectModel CopyFromBaseModel(PcFieldBaseModel input, List<SelectOption> options)
			{
				return new PcFieldSelectModel
				{
					Access = input.Access,
					AccessDeniedMessage = input.AccessDeniedMessage,
					ApiUrl = input.ApiUrl,
					Class = input.Class,
					Culture = input.Culture,
					Description = input.Description,
					EmptyValueMessage = input.EmptyValueMessage,
					EntityName = input.EntityName,
					FieldId = input.FieldId,
					FieldRenderModeOptions = input.FieldRenderModeOptions,
					LabelRenderModeOptions = input.LabelRenderModeOptions,
					EntitySelectOptions = input.EntitySelectOptions,
					InitErrors = input.InitErrors,
					LabelErrorText = input.LabelErrorText,
					LabelHelpText = input.LabelHelpText,
					LabelWarningText = input.LabelWarningText,
					Locale = input.Locale,
					Placeholder = input.Placeholder,
					RecordId = input.RecordId,
					Required = input.Required,
					ValidationErrors = input.ValidationErrors,
					Value = input.Value,
					DefaultValue = input.DefaultValue,
					Options = options
				};
			}
		}

		public class PcFieldCheckboxGridModel : PcFieldBaseModel
		{
			[JsonProperty(PropertyName = "rows")]
			public List<SelectOption> Rows { get; set; } = new List<SelectOption>();

			[JsonProperty(PropertyName = "columns")]
			public List<SelectOption> Columns { get; set; } = new List<SelectOption>();

			public static PcFieldCheckboxGridModel CopyFromBaseModel(PcFieldBaseModel input, List<SelectOption> rows, List<SelectOption> columns)
			{
				return new PcFieldCheckboxGridModel
				{
					Access = input.Access,
					AccessDeniedMessage = input.AccessDeniedMessage,
					ApiUrl = input.ApiUrl,
					Class = input.Class,
					Culture = input.Culture,
					Description = input.Description,
					EmptyValueMessage = input.EmptyValueMessage,
					EntityName = input.EntityName,
					FieldId = input.FieldId,
					FieldRenderModeOptions = input.FieldRenderModeOptions,
					LabelRenderModeOptions = input.LabelRenderModeOptions,
					EntitySelectOptions = input.EntitySelectOptions,
					InitErrors = input.InitErrors,
					LabelErrorText = input.LabelErrorText,
					LabelHelpText = input.LabelHelpText,
					LabelWarningText = input.LabelWarningText,
					Locale = input.Locale,
					Placeholder = input.Placeholder,
					RecordId = input.RecordId,
					Required = input.Required,
					ValidationErrors = input.ValidationErrors,
					Value = input.Value,
					DefaultValue = input.DefaultValue,
					Rows = rows,
					Columns = columns
				};
			}
		}

		public class PcFieldCheckboxListModel : PcFieldBaseModel
		{
			[JsonProperty(PropertyName = "options")]
			public List<SelectOption> Options { get; set; } = new List<SelectOption>();

			public static PcFieldCheckboxListModel CopyFromBaseModel(PcFieldBaseModel input, List<SelectOption> options)
			{
				return new PcFieldCheckboxListModel
				{
					Access = input.Access,
					AccessDeniedMessage = input.AccessDeniedMessage,
					ApiUrl = input.ApiUrl,
					Class = input.Class,
					Culture = input.Culture,
					Description = input.Description,
					EmptyValueMessage = input.EmptyValueMessage,
					EntityName = input.EntityName,
					FieldId = input.FieldId,
					FieldRenderModeOptions = input.FieldRenderModeOptions,
					LabelRenderModeOptions = input.LabelRenderModeOptions,
					EntitySelectOptions = input.EntitySelectOptions,
					InitErrors = input.InitErrors,
					LabelErrorText = input.LabelErrorText,
					LabelHelpText = input.LabelHelpText,
					LabelWarningText = input.LabelWarningText,
					Locale = input.Locale,
					Placeholder = input.Placeholder,
					RecordId = input.RecordId,
					Required = input.Required,
					ValidationErrors = input.ValidationErrors,
					Value = input.Value,
					DefaultValue = input.DefaultValue,
					Options = options
				};
			}
		}

		public class PcFieldMultiSelectModel : PcFieldBaseModel
		{
			[JsonProperty(PropertyName = "options")]
			public List<SelectOption> Options { get; set; } = new List<SelectOption>();

			public static PcFieldMultiSelectModel CopyFromBaseModel(PcFieldBaseModel input, List<SelectOption> options)
			{
				return new PcFieldMultiSelectModel
				{
					Access = input.Access,
					AccessDeniedMessage = input.AccessDeniedMessage,
					ApiUrl = input.ApiUrl,
					Class = input.Class,
					Culture = input.Culture,
					Description = input.Description,
					EmptyValueMessage = input.EmptyValueMessage,
					EntityName = input.EntityName,
					FieldId = input.FieldId,
					FieldRenderModeOptions = input.FieldRenderModeOptions,
					LabelRenderModeOptions = input.LabelRenderModeOptions,
					EntitySelectOptions = input.EntitySelectOptions,
					InitErrors = input.InitErrors,
					LabelErrorText = input.LabelErrorText,
					LabelHelpText = input.LabelHelpText,
					LabelWarningText = input.LabelWarningText,
					Locale = input.Locale,
					Placeholder = input.Placeholder,
					RecordId = input.RecordId,
					Required = input.Required,
					ValidationErrors = input.ValidationErrors,
					Value = input.Value,
					DefaultValue = input.DefaultValue,
					Options = options
				};
			}
		}

		public class PcFieldRadioListModel : PcFieldBaseModel
		{
			[JsonProperty(PropertyName = "options")]
			public List<SelectOption> Options { get; set; } = new List<SelectOption>();

			public static PcFieldRadioListModel CopyFromBaseModel(PcFieldBaseModel input, List<SelectOption> options)
			{
				return new PcFieldRadioListModel
				{
					Access = input.Access,
					AccessDeniedMessage = input.AccessDeniedMessage,
					ApiUrl = input.ApiUrl,
					Class = input.Class,
					Culture = input.Culture,
					Description = input.Description,
					EmptyValueMessage = input.EmptyValueMessage,
					EntityName = input.EntityName,
					FieldId = input.FieldId,
					FieldRenderModeOptions = input.FieldRenderModeOptions,
					LabelRenderModeOptions = input.LabelRenderModeOptions,
					EntitySelectOptions = input.EntitySelectOptions,
					InitErrors = input.InitErrors,
					LabelErrorText = input.LabelErrorText,
					LabelHelpText = input.LabelHelpText,
					LabelWarningText = input.LabelWarningText,
					Locale = input.Locale,
					Placeholder = input.Placeholder,
					RecordId = input.RecordId,
					Required = input.Required,
					ValidationErrors = input.ValidationErrors,
					Value = input.Value,
					DefaultValue = input.DefaultValue,
					Options = options
				};
			}
		}

		public PcFieldBaseOptions InitPcFieldBaseOptions(PageComponentContext context)
		{
			var options = new PcFieldBaseOptions();

			//Check if it is defined in form group
			if (context.Items.ContainsKey(typeof(WvLabelRenderMode)))
			{
				options.LabelMode = (WvLabelRenderMode)context.Items[typeof(WvLabelRenderMode)];
			}
			else
			{
				options.LabelMode = WvLabelRenderMode.Stacked;
			}


			//Check if it is defined in form group
			if (context.Items.ContainsKey(typeof(WvFieldRenderMode)))
			{
				options.Mode = (WvFieldRenderMode)context.Items[typeof(WvFieldRenderMode)];
			}
			else
			{
				options.Mode = WvFieldRenderMode.Form;
			}

			var baseOptions = JsonConvert.DeserializeObject<PcFieldBaseOptions>(context.Options.ToString());

			Entity mappedEntity = null;
			var entity = context.DataModel.GetProperty("Entity");
			if (baseOptions.ConnectedEntityId != null)
			{
				mappedEntity = new EntityManager().ReadEntity(baseOptions.ConnectedEntityId.Value).Object;
			}
			else if (baseOptions.ConnectedEntityId == null && entity is Entity)
			{
				mappedEntity = (Entity)entity;
			}

			if (mappedEntity != null)
			{
				var fieldName = baseOptions.Name;

				EntityRelation relation = null;
				if (fieldName.StartsWith("$")) {
					//Field with relation is set. Mapped entity should be changed
					var fieldNameArray = fieldName.Replace("$", "").Split(".", StringSplitOptions.RemoveEmptyEntries);
					if (fieldNameArray.Length == 2) {
						var relationName = fieldNameArray[0];
						fieldName = fieldNameArray[1];
						relation = new EntityRelationManager().Read(relationName).Object;
						if (relation != null) {
							if (relation.OriginEntityId == mappedEntity.Id)
								mappedEntity = new EntityManager().ReadEntity(relation.TargetEntityId).Object;
							else if (relation.TargetEntityId == mappedEntity.Id)
								mappedEntity = new EntityManager().ReadEntity(relation.OriginEntityId).Object;
						}

					}

				}

				var entityField = mappedEntity.Fields.FirstOrDefault(x => x.Name == fieldName);

				//for many to many relation field is always ID and that is not correct
				//so hide this field meta as field is not found
				if (relation != null && relation.RelationType == EntityRelationType.ManyToMany)
					entityField = null;

				if (entityField != null)
				{
					options.LabelHelpText = entityField.HelpText;
					options.Description = entityField.Description;
					switch (entityField.GetFieldType())
					{
						case FieldType.AutoNumberField:
							{
								var fieldMeta = (AutoNumberField)entityField;
								options.Template = fieldMeta.DisplayFormat;
							}
							break;
						case FieldType.CurrencyField:
							{
								var fieldMeta = (CurrencyField)entityField;
								options.Min = fieldMeta.MinValue;
								options.Min = fieldMeta.MinValue;
								options.CurrencyCode = fieldMeta.Currency.Code;
							}
							break;
						case FieldType.EmailField:
							{
								var fieldMeta = (EmailField)entityField;
								options.MaxLength = fieldMeta.MaxLength;
							}
							break;
						case FieldType.NumberField:
							{
								var fieldMeta = (NumberField)entityField;
								options.Min = fieldMeta.MinValue;
								options.Min = fieldMeta.MinValue;
								if (fieldMeta.DecimalPlaces != null)
								{
									if (int.TryParse(fieldMeta.DecimalPlaces.ToString(), out int outInt))
									{
										options.DecimalDigits = outInt;
									}
								}
							}
							break;
						case FieldType.PasswordField:
							{
								var fieldMeta = (PasswordField)entityField;
								options.Min = (decimal?)fieldMeta.MinLength;
								options.Max = (decimal?)fieldMeta.MaxLength;
							}
							break;
						case FieldType.PercentField:
							{
								var fieldMeta = (PercentField)entityField;
								options.Min = fieldMeta.MinValue;
								options.Min = fieldMeta.MinValue;
								if (fieldMeta.DecimalPlaces != null)
								{
									if (int.TryParse(fieldMeta.DecimalPlaces.ToString(), out int outInt))
									{
										options.DecimalDigits = outInt;
									}
								}
							}
							break;
						case FieldType.PhoneField:
							{
								var fieldMeta = (PhoneField)entityField;
								options.MaxLength = fieldMeta.MaxLength;
							}
							break;
						case FieldType.TextField:
							{
								var fieldMeta = (TextField)entityField;
								options.MaxLength = fieldMeta.MaxLength;
							}
							break;
						default:
							break;
					}
				}
			}

			return options;
		}

		public dynamic InitPcFieldBaseModel(PageComponentContext context, PcFieldBaseOptions options, out string label, string targetModel = "PcFieldBaseModel")
		{
			label = "";
			var model = new PcFieldBaseModel();

			if (context.Items.ContainsKey(typeof(ValidationException)))
			{
				model.ValidationErrors = ((ValidationException)context.Items[typeof(ValidationException)]).Errors;
			}

			model.LabelRenderModeOptions = WebVella.TagHelpers.Utilities.ModelExtensions.GetEnumAsSelectOptions<WvLabelRenderMode>();

			model.FieldRenderModeOptions = WebVella.TagHelpers.Utilities.ModelExtensions.GetEnumAsSelectOptions<WvFieldRenderMode>();

			if (context.Mode == ComponentMode.Options)
				model.EntitySelectOptions = new MetaService().GetEntitiesAsSelectOptions();

			var recordId = context.DataModel.GetProperty("RecordId");
			if (recordId != null && recordId is Guid)
			{
				model.RecordId = (Guid)recordId;
			}
			//Check for RecordId override
			if(!String.IsNullOrWhiteSpace(options.ConnectedRecordIdDs)){
				var dsRecordId = context.DataModel.GetPropertyValueByDataSource(options.ConnectedRecordIdDs) as Guid?;
				if(dsRecordId == null && Guid.TryParse(options.ConnectedRecordIdDs, out Guid outGuid)){
					model.RecordId = outGuid;
				}
				else{
					model.RecordId = dsRecordId.Value;
				}
			}


			var entity = context.DataModel.GetProperty("Entity");
			if (entity != null && entity is Entity)
			{
				model.EntityName = ((Entity)entity).Name;
				if (!String.IsNullOrWhiteSpace(model.EntityName) && model.RecordId != null)
					model.ApiUrl = $"/api/v3/en_US/record/{model.EntityName}/{model.RecordId}/";
			}

			Entity mappedEntity = null;
			if (options.ConnectedEntityId != null)
			{
				mappedEntity = new EntityManager().ReadEntity(options.ConnectedEntityId.Value).Object;
			}
			else if (options.ConnectedEntityId == null && entity is Entity)
			{
				mappedEntity = (Entity)entity;
			}

			if (mappedEntity != null)
			{
				//Override the entity settings
				model.EntityName = mappedEntity.Name;	
				var fieldName = options.Name;

				if (fieldName.StartsWith("$"))
				{
					//Field with relation is set. Mapped entity should be changed
					var fieldNameArray = fieldName.Replace("$", "").Split(".", StringSplitOptions.RemoveEmptyEntries);
					if (fieldNameArray.Length == 2)
					{
						var relationName = fieldNameArray[0];
						fieldName = fieldNameArray[1];
						var relation = new EntityRelationManager().Read(relationName).Object;
						if (relation != null)
						{
							if (relation.OriginEntityId == mappedEntity.Id)
								mappedEntity = new EntityManager().ReadEntity(relation.TargetEntityId).Object;
							else if (relation.TargetEntityId == mappedEntity.Id)
								mappedEntity = new EntityManager().ReadEntity(relation.OriginEntityId).Object;
						}

					}

				}

				var entityField = mappedEntity.Fields.FirstOrDefault(x => x.Name == fieldName);
				if (entityField != null)
				{
					//Connection success set local options if needed
					if (String.IsNullOrWhiteSpace(model.Placeholder))
						model.Placeholder = entityField.PlaceholderText;

					if (String.IsNullOrWhiteSpace(model.Description))
						model.Description = entityField.Description;

					if (String.IsNullOrWhiteSpace(model.LabelHelpText))
						model.LabelHelpText = entityField.HelpText;

					if (String.IsNullOrWhiteSpace(label))
						label = entityField.Label;

					model.Required = entityField.Required;

					if (entityField.EnableSecurity)
					{

						var currentUser = context.DataModel.GetProperty("CurrentUser");
						if (currentUser != null && currentUser is ErpUser)
						{
							var canRead = false;
							var canUpdate = false;
							var user = (ErpUser)currentUser;
							foreach (var role in user.Roles)
							{
								if (entityField.Permissions.CanRead.Any(x => x == role.Id))
									canRead = true;
								if (entityField.Permissions.CanUpdate.Any(x => x == role.Id))
									canUpdate = true;
							}
							if (canUpdate)
								model.Access = WvFieldAccess.Full;
							else if (canRead)
								model.Access = WvFieldAccess.ReadOnly;
							else
								model.Access = WvFieldAccess.Forbidden;

						}
					}

					//Specific model properties
					var fieldOptions = new List<SelectOption>();
					switch (entityField.GetFieldType())
					{
						case FieldType.SelectField:
							var selectField = ((SelectField)entityField);
							model.DefaultValue = selectField.DefaultValue;
							fieldOptions = selectField.Options;
							break;
						case FieldType.MultiSelectField:
							var multiselectField = ((MultiSelectField)entityField);
							model.DefaultValue = multiselectField.DefaultValue;
							fieldOptions = multiselectField.Options;
							break;
						default:
							break;
					}

					if (!String.IsNullOrWhiteSpace(model.EntityName) && model.RecordId != null)
						model.ApiUrl = $"/api/v3/en_US/record/{model.EntityName}/{model.RecordId}/";

					if(!String.IsNullOrWhiteSpace(options.AjaxApiUrlDs)){
						var urlString = context.DataModel.GetPropertyValueByDataSource(options.AjaxApiUrlDs) as string;
						if(!String.IsNullOrWhiteSpace(urlString)){
							model.ApiUrl = String.Format(urlString,model.EntityName,model.RecordId);
						}
					}					
					
					switch (targetModel)
					{
						case "PcFieldSelectModel":
							return PcFieldSelectModel.CopyFromBaseModel(model, fieldOptions);
						case "PcFieldRadioListModel":
							return PcFieldRadioListModel.CopyFromBaseModel(model, fieldOptions);
						case "PcFieldCheckboxListModel":
							return PcFieldCheckboxListModel.CopyFromBaseModel(model, fieldOptions);
						case "PcFieldMultiSelectModel":
							return PcFieldMultiSelectModel.CopyFromBaseModel(model, fieldOptions);
						case "PcFieldCheckboxGridModel":
							return PcFieldCheckboxGridModel.CopyFromBaseModel(model, new List<SelectOption>(), new List<SelectOption>());
						default:
							return model;
					}
				}


				

			}

			if (!String.IsNullOrWhiteSpace(model.EntityName) && model.RecordId != null)
				model.ApiUrl = $"/api/v3/en_US/record/{model.EntityName}/{model.RecordId}/";

			if(!String.IsNullOrWhiteSpace(options.AjaxApiUrlDs)){
				var urlString = context.DataModel.GetPropertyValueByDataSource(options.AjaxApiUrlDs) as string;
				if(!String.IsNullOrWhiteSpace(urlString)){
					model.ApiUrl = String.Format(urlString,model.EntityName,model.RecordId);
				}
			}

			switch (targetModel)
			{
				case "PcFieldSelectModel":
					return PcFieldSelectModel.CopyFromBaseModel(model, new List<SelectOption>());
				case "PcFieldRadioListModel":
					return PcFieldRadioListModel.CopyFromBaseModel(model, new List<SelectOption>());
				case "PcFieldCheckboxListModel":
					return PcFieldCheckboxListModel.CopyFromBaseModel(model, new List<SelectOption>());
				case "PcFieldMultiSelectModel":
					return PcFieldMultiSelectModel.CopyFromBaseModel(model, new List<SelectOption>());
				case "PcFieldCheckboxGridModel":
					return PcFieldCheckboxGridModel.CopyFromBaseModel(model, new List<SelectOption>(), new List<SelectOption>());
				default:
					return model;
			}

		}

	}
}
