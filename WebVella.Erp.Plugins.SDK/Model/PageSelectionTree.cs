using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Plugins.SDK.Model
{
	public class PageSelectionTree
	{
		[JsonProperty(PropertyName = "all_entities")]
		public List<SelectOption> AllEntities { get; set; } = new List<SelectOption>();

		[JsonProperty(PropertyName = "all_types")]
		public List<SelectOption> AllTypes { get; set; } = new List<SelectOption>();

		[JsonProperty(PropertyName = "all_apps")]
		public List<SelectOption> AllApps { get; set; } = new List<SelectOption>();

		[JsonProperty(PropertyName = "app_selection_tree")]
		public List<AppSelectionTree> AppSelectionTree { get; set; } = new List<AppSelectionTree>();
	}
}
