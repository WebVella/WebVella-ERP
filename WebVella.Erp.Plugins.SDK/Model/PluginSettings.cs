using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Plugins.SDK.Model
{
	public class PluginSettings
	{
		[JsonProperty(PropertyName = "version")]
		public int Version { get; set; }
	}
}
