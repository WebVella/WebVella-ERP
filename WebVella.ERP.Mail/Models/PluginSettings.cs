using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.ERP.Mail.Models
{
    public class PluginSettings
    {
		[JsonProperty(PropertyName = "version")]
		public int Version { get; set; }
    }
}
