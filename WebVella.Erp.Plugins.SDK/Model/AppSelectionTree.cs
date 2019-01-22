using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Plugins.SDK.Model
{
	public class AppSelectionTree
	{
		[JsonProperty(PropertyName = "app_id")]
		public Guid AppId { get; set; } = Guid.Empty;

		[JsonProperty(PropertyName = "all_areas")]
		public List<SelectOption> AllAreas { get; set; } = new List<SelectOption>();

		[JsonProperty(PropertyName = "area_selection_tree")]
		public List<AreaSelectionTree> AreaSelectionTree { get; set; } = new List<AreaSelectionTree>();

		[JsonProperty(PropertyName = "entities")]
		public List<SelectOption> Entities { get; set; } = new List<SelectOption>();
	}
}
