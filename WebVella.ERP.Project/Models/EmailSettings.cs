using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.ERP.Projects.Models
{
    public class EmailSettings
    {
        [JsonProperty(PropertyName = "enabled")]
		public bool Enabled { get; set; }
		[JsonProperty(PropertyName = "server")]
		public string Server { get; set; }
        [JsonProperty(PropertyName = "enable_ssl")]
		public bool EnableSsl { get; set; }
        [JsonProperty(PropertyName = "port")]
		public int Port { get; set; }
		[JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }
		[JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
		[JsonProperty(PropertyName = "display_name")]
        public string DisplayName { get; set; }
    }
}
