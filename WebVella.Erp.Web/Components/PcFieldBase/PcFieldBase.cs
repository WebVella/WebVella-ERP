using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Utils;
using WebVella.Erp.Exceptions;
using System.Linq;

namespace WebVella.Erp.Web.Components
{
	public abstract class PcFieldBase : PageComponent
	{

		public class PcFieldBaseOptions
		{
			//Label
			[JsonProperty(PropertyName = "label_mode")]
			public LabelRenderMode LabelMode { get; set; } = LabelRenderMode.Undefined;

			[JsonProperty(PropertyName = "label_text")]
			public string LabelText { get; set; } = "";

			//Field
			[JsonProperty(PropertyName = "name")]
			public string Name { get; set; } = "field";

			[JsonProperty(PropertyName = "mode")]
			public FieldRenderMode Mode { get; set; } = FieldRenderMode.Undefined;

			[JsonProperty(PropertyName = "value")]
			public string Value { get; set; } = "";

			[JsonProperty(PropertyName = "try_connect_to_entity")]
			public bool TryConnectToEntity { get; set; } = false;

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
			public FieldAccess Access { get; set; } = FieldAccess.Full;

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

			public List<SelectOption> LabelRenderModeOptions { get; set; } = new List<SelectOption>();

			public List<SelectOption> FieldRenderModeOptions { get; set; } = new List<SelectOption>();
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

			public static PcFieldCheckboxListModel CopyFromBaseModel(PcFieldBaseModel input,List<SelectOption> options)
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
			if (context.Items.ContainsKey(typeof(LabelRenderMode)))
			{
				options.LabelMode = (LabelRenderMode)context.Items[typeof(LabelRenderMode)];
			}
			else
			{
				options.LabelMode = LabelRenderMode.Stacked;
			}


			//Check if it is defined in form group
			if (context.Items.ContainsKey(typeof(FieldRenderMode)))
			{
				options.Mode = (FieldRenderMode)context.Items[typeof(FieldRenderMode)];
			}
			else
			{
				options.Mode = FieldRenderMode.Form;
			}

			return options;
		}

		public dynamic InitPcFieldBaseModel(PageComponentContext context, PcFieldBaseOptions options,out string label, string targetModel = "PcFieldBaseModel") {
			label = "";
			var model = new PcFieldBaseModel();

			if (context.Items.ContainsKey(typeof(ValidationException)))
			{
				model.ValidationErrors = ((ValidationException)context.Items[typeof(ValidationException)]).Errors;
			}

			model.LabelRenderModeOptions = ModelExtensions.GetEnumAsSelectOptions<LabelRenderMode>();

			model.FieldRenderModeOptions = ModelExtensions.GetEnumAsSelectOptions<FieldRenderMode>();

			var recordId = context.DataModel.GetProperty("RecordId");
			if (recordId != null && recordId is Guid)
			{
				model.RecordId = (Guid)recordId;
			}

			var entity = context.DataModel.GetProperty("Entity");
			if (entity != null && entity is Entity)
			{
				model.EntityName = ((Entity)entity).Name;
				if (!String.IsNullOrWhiteSpace(model.EntityName) && model.RecordId != null)
					model.ApiUrl = $"/api/v3/en_US/record/{model.EntityName}/{model.RecordId}/";

				if (options.TryConnectToEntity)
				{
					var fieldName = options.Name;
					var entityField = ((Entity)entity).Fields.FirstOrDefault(x => x.Name == fieldName);
					if (entityField != null)
					{
						//Connection success override the local options
						//Init model
						model.Placeholder = entityField.PlaceholderText;
						model.Description = entityField.Description;
						model.LabelHelpText = entityField.HelpText;
						model.Required = entityField.Required;
						label = entityField.Label;
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
									model.Access = FieldAccess.Full;
								else if (canRead)
									model.Access = FieldAccess.ReadOnly;
								else
									model.Access = FieldAccess.Forbidden;

							}
						}

						//Specific model properties
						var fieldOptions = new List<SelectOption>();
						switch (entityField.GetFieldType()) {
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
