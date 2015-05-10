using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.ERP.Api.Models
{
    public class Role : EntityRecord
    {
        [JsonProperty(PropertyName = "name")]
        string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        string Description { get; set; }
    }
}
