using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebVella.Erp.Web.Models
{
	public class Sitemap
	{
		[JsonProperty("areas")]
		public List<SitemapArea> Areas { get; set; } = new List<SitemapArea>();
	}
}
