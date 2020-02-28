using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.Erp.Web.Models
{
	public class MenuItem
	{
		[JsonProperty("id")]
		public Guid Id { get; set; }

		[JsonProperty("parent_id")]
		public Guid? ParentId { get; set; } = null;

		[JsonProperty("content")]
		public string Content { get; set; } = "";//What to render

		[JsonProperty("class")]
		public string Class { get; set; } = "";//specific class to attach - like active

		[JsonProperty("is_html")]
		public bool IsHtml { get; set; } = true;

		[JsonProperty("render_wrapper")]
		public bool RenderWrapper { get; set; } = true;

		[JsonProperty("nodes")]
		public List<MenuItem> Nodes { get; set; } = new List<MenuItem>();

		[JsonProperty("is_dropdown_right")]
		public bool isDropdownRight { get; set; } = false;

		[JsonProperty("sort_order")]
		public int SortOrder { get; set; } = 10;

	}
}
