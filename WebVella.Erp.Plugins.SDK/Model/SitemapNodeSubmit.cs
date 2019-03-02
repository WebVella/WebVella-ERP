using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Plugins.SDK.Model
{
	public class SitemapNodeSubmit : SitemapNode
	{
		[JsonProperty("pages")]
		public List<Guid> Pages { get; set; } = new List<Guid>();

    }
}
