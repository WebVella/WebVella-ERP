using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Web.TagHelpers;

namespace WebVella.Erp.Web.Models
{
	public class WvFieldBaseConfig
    {
		public WvFieldBaseConfig() {
			CanAddValues = null;
			ApiUrl = "";
		}

		[JsonProperty(PropertyName = "can_add_values")]
		public bool? CanAddValues { get; set; }

		[JsonProperty(PropertyName = "api_url")]
		public string ApiUrl { get; set; }
	}

	public enum HtmlUploadMode
	{
		[SelectOption(Label = "None")]
		None = 1,
		[SelectOption(Label = "Site Repository")]
		SiteRepository = 2
	}

	public enum HtmlToolbarMode
	{
		[SelectOption(Label = "Basic")]
		Basic = 1,
		[SelectOption(Label = "Standard")]
		Standard = 2,
		[SelectOption(Label = "Full")]
		Full = 3
	}

	public enum ImageResizeMode
	{
		[SelectOption(Label = "Pad")]
		Pad = 1,
		[SelectOption(Label = "BoxPad")]
		BoxPad = 2,
		[SelectOption(Label = "Crop")]
		Crop = 3,
		[SelectOption(Label = "Min")]
		Min = 4,
		[SelectOption(Label = "Max")]
		Max = 5,
		[SelectOption(Label = "Stretch")]
		Stretch = 6
	}

	public class WvFieldAutonumberConfig : WvFieldBaseConfig
	{
		public WvFieldAutonumberConfig()
		{
			var baseConfig = new WvFieldBaseConfig();
			CanAddValues = baseConfig.CanAddValues;
		}

		public WvFieldAutonumberConfig(WvFieldBaseConfig baseConfig)
		{
			CanAddValues = baseConfig.CanAddValues;
			ApiUrl = baseConfig.ApiUrl;
		}
	}

	public class WvFieldCheckboxConfig : WvFieldBaseConfig
	{
		public WvFieldCheckboxConfig()
		{
			var baseConfig = new WvFieldBaseConfig();
			CanAddValues = baseConfig.CanAddValues;
			TrueLabel = "selected";
			FalseLabel = "not selected";
		}

		public WvFieldCheckboxConfig(WvFieldBaseConfig baseConfig)
		{
			CanAddValues = baseConfig.CanAddValues;
			TrueLabel = "selected";
			FalseLabel = "not selected";
			ApiUrl = baseConfig.ApiUrl;
		}

		[JsonProperty(PropertyName = "true_label")]
		public string TrueLabel { get; set; }

		[JsonProperty(PropertyName = "false_label")]
		public string FalseLabel { get; set; }
	}

	public class WvFieldCurrencyConfig : WvFieldBaseConfig
	{
		public WvFieldCurrencyConfig()
		{
			var baseConfig = new WvFieldBaseConfig();
			CanAddValues = baseConfig.CanAddValues;
		}

		public WvFieldCurrencyConfig(WvFieldBaseConfig baseConfig)
		{
			CanAddValues = baseConfig.CanAddValues;
			ApiUrl = baseConfig.ApiUrl;
		}
	}

	public class WvFieldDateConfig : WvFieldBaseConfig
	{
		public WvFieldDateConfig()
		{
			var baseConfig = new WvFieldBaseConfig();
			CanAddValues = baseConfig.CanAddValues;
		}

		public WvFieldDateConfig(WvFieldBaseConfig baseConfig)
		{
			CanAddValues = baseConfig.CanAddValues;
			ApiUrl = baseConfig.ApiUrl;
		}
	}

	public class WvFieldDateTimeConfig : WvFieldBaseConfig
	{
		public WvFieldDateTimeConfig()
		{
			var baseConfig = new WvFieldBaseConfig();
			CanAddValues = baseConfig.CanAddValues;
		}

		public WvFieldDateTimeConfig(WvFieldBaseConfig baseConfig)
		{
			CanAddValues = baseConfig.CanAddValues;
			ApiUrl = baseConfig.ApiUrl;
		}
	}

	public class WvFieldEmailConfig : WvFieldBaseConfig
	{
		public WvFieldEmailConfig()
		{
			var baseConfig = new WvFieldBaseConfig();
			CanAddValues = baseConfig.CanAddValues;
		}

		public WvFieldEmailConfig(WvFieldBaseConfig baseConfig)
		{
			CanAddValues = baseConfig.CanAddValues;
			ApiUrl = baseConfig.ApiUrl;
		}
	}

	public class WvFieldFileConfig : WvFieldBaseConfig
	{

		public WvFieldFileConfig()
		{
			var baseConfig = new WvFieldBaseConfig();
			CanAddValues = baseConfig.CanAddValues;
			Accept = "";
		}

		public WvFieldFileConfig(WvFieldBaseConfig baseConfig)
		{
			CanAddValues = baseConfig.CanAddValues;
			Accept = "";
			ApiUrl = baseConfig.ApiUrl;
		}

		[JsonProperty(PropertyName = "accept")]
		public string Accept { get; set; }
	}

	public class WvFieldHtmlConfig : WvFieldBaseConfig
	{
		public WvFieldHtmlConfig()
		{
			var baseConfig = new WvFieldBaseConfig();
			CanAddValues = baseConfig.CanAddValues;
			UploadMode = HtmlUploadMode.None;
			ToolbarMode = HtmlToolbarMode.Basic;
		}

		public WvFieldHtmlConfig(WvFieldBaseConfig baseConfig)
		{
			CanAddValues = baseConfig.CanAddValues;
			UploadMode = HtmlUploadMode.None;
			ToolbarMode = HtmlToolbarMode.Basic;
			ApiUrl = baseConfig.ApiUrl;
		}

		[JsonProperty(PropertyName = "upload_mode")]
		public HtmlUploadMode UploadMode { get; set; }

		[JsonProperty(PropertyName = "toolbar_mode")]
		public HtmlToolbarMode ToolbarMode { get; set; }
	}

	public class WvFieldImageConfig : WvFieldBaseConfig
	{
		public WvFieldImageConfig()
		{
			var baseConfig = new WvFieldBaseConfig();
			CanAddValues = baseConfig.CanAddValues;
			Accept = "";
			Width = null;
			Height = null;
			ResizeAction = null;
		}

		public WvFieldImageConfig(WvFieldBaseConfig baseConfig)
		{
			CanAddValues = baseConfig.CanAddValues;
			Accept = "";
			Width = null;
			Height = null;
			ResizeAction = null;
			ApiUrl = baseConfig.ApiUrl;
		}

		[JsonProperty(PropertyName = "accept")]
		public string Accept { get; set; }

		[JsonProperty(PropertyName = "width")]
		public int? Width { get; set; }

		[JsonProperty(PropertyName = "height")]
		public int? Height { get; set; }

		[JsonProperty(PropertyName = "resize_action")]
		public ImageResizeMode? ResizeAction { get; set; }
	}

	public class WvFieldTextareaConfig : WvFieldBaseConfig
	{
		public WvFieldTextareaConfig()
		{
			var baseConfig = new WvFieldBaseConfig();
			CanAddValues = baseConfig.CanAddValues;
		}

		public WvFieldTextareaConfig(WvFieldBaseConfig baseConfig)
		{
			CanAddValues = baseConfig.CanAddValues;
			ApiUrl = baseConfig.ApiUrl;
		}
	}

	public class WvFieldMultiSelectConfig : WvFieldBaseConfig
	{
		public WvFieldMultiSelectConfig()
		{
			var baseConfig = new WvFieldBaseConfig();
			CanAddValues = baseConfig.CanAddValues;
		}

		public WvFieldMultiSelectConfig(WvFieldBaseConfig baseConfig)
		{
			CanAddValues = baseConfig.CanAddValues;
			ApiUrl = baseConfig.ApiUrl;
		}

		[JsonProperty(PropertyName = "ajax_datasource")]
		public SelectOptionsAjaxDatasource AjaxDatasource { get; set; } = null;

	}

	public class WvFieldNumberConfig : WvFieldBaseConfig
	{
		public WvFieldNumberConfig()
		{
			var baseConfig = new WvFieldBaseConfig();
			CanAddValues = baseConfig.CanAddValues;
		}

		public WvFieldNumberConfig(WvFieldBaseConfig baseConfig)
		{
			CanAddValues = baseConfig.CanAddValues;
			ApiUrl = baseConfig.ApiUrl;
		}
	}

	public class WvFieldPasswordConfig : WvFieldBaseConfig
	{
		public WvFieldPasswordConfig()
		{
			var baseConfig = new WvFieldBaseConfig();
			CanAddValues = baseConfig.CanAddValues;
		}

		public WvFieldPasswordConfig(WvFieldBaseConfig baseConfig)
		{
			CanAddValues = baseConfig.CanAddValues;
			ApiUrl = baseConfig.ApiUrl;
		}
	}

	public class WvFieldPercentConfig : WvFieldBaseConfig
	{
		public WvFieldPercentConfig()
		{
			var baseConfig = new WvFieldBaseConfig();
			CanAddValues = baseConfig.CanAddValues;
			DecimalDigits = null;
		}

		public WvFieldPercentConfig(WvFieldBaseConfig baseConfig)
		{
			CanAddValues = baseConfig.CanAddValues;
			ApiUrl = baseConfig.ApiUrl;
			DecimalDigits = null;
		}

		[JsonProperty(PropertyName = "decimal_digits")]
		public int? DecimalDigits { get; set; }
	}

	public class WvFieldPhoneConfig : WvFieldBaseConfig
	{
		public WvFieldPhoneConfig()
		{
			var baseConfig = new WvFieldBaseConfig();
			CanAddValues = baseConfig.CanAddValues;
		}

		public WvFieldPhoneConfig(WvFieldBaseConfig baseConfig)
		{
			CanAddValues = baseConfig.CanAddValues;
			ApiUrl = baseConfig.ApiUrl;
		}
	}

	public class WvFieldGuidConfig : WvFieldBaseConfig
	{
		public WvFieldGuidConfig()
		{
			var baseConfig = new WvFieldBaseConfig();
			CanAddValues = baseConfig.CanAddValues;
		}

		public WvFieldGuidConfig(WvFieldBaseConfig baseConfig)
		{
			CanAddValues = baseConfig.CanAddValues;
			ApiUrl = baseConfig.ApiUrl;
		}
	}

	public class WvFieldSelectConfig : WvFieldBaseConfig
	{
		public WvFieldSelectConfig()
		{
			var baseConfig = new WvFieldBaseConfig();
			CanAddValues = baseConfig.CanAddValues;
		}

		public WvFieldSelectConfig(WvFieldBaseConfig baseConfig)
		{
			CanAddValues = baseConfig.CanAddValues;
			ApiUrl = baseConfig.ApiUrl;
		}

		[JsonProperty(PropertyName = "is_invalid")]
		public bool IsInvalid { get; set; } = false;

		[JsonProperty(PropertyName = "ajax_datasource")]
		public SelectOptionsAjaxDatasource AjaxDatasource { get; set; } = null;

	}

	public class WvFieldTextConfig : WvFieldBaseConfig
	{
		public WvFieldTextConfig()
		{
			var baseConfig = new WvFieldBaseConfig();
			CanAddValues = baseConfig.CanAddValues;
		}

		public WvFieldTextConfig(WvFieldBaseConfig baseConfig)
		{
			CanAddValues = baseConfig.CanAddValues;
			ApiUrl = baseConfig.ApiUrl;
		}
	}

	public class WvFieldColorConfig : WvFieldBaseConfig
	{
		public WvFieldColorConfig()
		{
			var baseConfig = new WvFieldBaseConfig();
			CanAddValues = baseConfig.CanAddValues;
		}

		public WvFieldColorConfig(WvFieldBaseConfig baseConfig)
		{
			CanAddValues = baseConfig.CanAddValues;
			ApiUrl = baseConfig.ApiUrl;
		}
	}

	public class WvFieldCodeConfig : WvFieldBaseConfig
	{
		public WvFieldCodeConfig()
		{
			var baseConfig = new WvFieldBaseConfig();
			CanAddValues = baseConfig.CanAddValues;
			Language = "razor";
			Theme = "xcode";
			ReadOnly = false;
		}

		public WvFieldCodeConfig(WvFieldBaseConfig baseConfig)
		{
			CanAddValues = baseConfig.CanAddValues;
			ApiUrl = baseConfig.ApiUrl;
			Language = "razor";
			Theme = "xcode";
			ReadOnly = false;
		}

		[JsonProperty(PropertyName = "language")]
		public string Language { get; set; } = "razor";

		[JsonProperty(PropertyName = "theme")]
		public string Theme { get; set; } = "xcode";

		[JsonProperty(PropertyName = "read_only")]
		public bool ReadOnly { get; set; } = false;
	}

	public class WvFieldUrlConfig : WvFieldBaseConfig
	{
		public WvFieldUrlConfig()
		{
			var baseConfig = new WvFieldBaseConfig();
			CanAddValues = baseConfig.CanAddValues;
		}

		public WvFieldUrlConfig(WvFieldBaseConfig baseConfig)
		{
			CanAddValues = baseConfig.CanAddValues;
			ApiUrl = baseConfig.ApiUrl;
		}
	}


}
