using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebVella.Erp.Api.Models
{
    public class QueryResult
    {
        [JsonProperty(PropertyName = "fieldsMeta")]
        public List<Field> FieldsMeta { get; set; } 

        [JsonProperty(PropertyName = "data")]
        public List<EntityRecord> Data { get; set; }
    }
}
