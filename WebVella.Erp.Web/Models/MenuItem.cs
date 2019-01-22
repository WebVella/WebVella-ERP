using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public class MenuItem
	{
		[JsonProperty("content")]
		public string Content { get; set; } = "";//What to render

		[JsonProperty("is_html")]
		public bool IsHtml { get; set; } = true;

		[JsonProperty("render_wrapper")]
		public bool RenderWrapper { get; set; } = true;

		[JsonProperty("nodes")]
		public List<MenuItem> Nodes { get; set; } = new List<MenuItem>();

		[JsonProperty("is_dropdown_right")]
		public bool isDropdownRight { get; set; } = false;

	}
}
