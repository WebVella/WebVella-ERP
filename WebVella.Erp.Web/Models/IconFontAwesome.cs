using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public class IconFontAwesome
	{
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; } = "";	
		
		[JsonProperty(PropertyName = "class")]
		public string Class { get; set; } = "";		

		[JsonProperty(PropertyName = "symbol")]
		public string Symbol { get; set; } = "";		
	}
}
