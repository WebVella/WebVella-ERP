using Newtonsoft.Json;
using System;

namespace WebVella.Erp.Web.Models
{
	public class WebSettings
	{
		[JsonProperty("theme_id")]
		public Guid ThemeId { get; set; } = Guid.Empty; 
	}
}
