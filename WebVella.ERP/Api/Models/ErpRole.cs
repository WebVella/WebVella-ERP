using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api.Models
{
    public class ErpRole
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

} 