using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Security
{
    public class Role
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

}