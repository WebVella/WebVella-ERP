using Newtonsoft.Json;
using System;

namespace WebVella.Erp.Api.Models
{
	[Serializable]
	public class ErpRole
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

		[JsonProperty(PropertyName = "description")]
		public string Description { get; set; }
	}

} 