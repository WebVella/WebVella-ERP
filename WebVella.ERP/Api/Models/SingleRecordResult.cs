using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.ERP.Api.Models
{
    public class SingleRecordResult
    {
        [JsonProperty(PropertyName = "fieldsMeta")]
        public List<Field> FieldsMeta { get; set; }

        [JsonProperty(PropertyName = "data")]
        public EntityRecord Data { get; set; }
    }
}
