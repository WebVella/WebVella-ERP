using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.Erp.Web.Models
{
    public class QueryFilterJson
    {

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; } = "";

        [JsonProperty(PropertyName = "option")]
        public string Option { get; set; } = "";

        [JsonProperty(PropertyName = "default")]
        public string Default { get; set; } = "";

        [JsonProperty(PropertyName = "settings")]
        public QueryFilterJsonSettings Settings { get; set; } = null;

    }

    public class QueryFilterJsonSettings
    {

        [JsonProperty(PropertyName = "year")]
        public int Year { get; set; }

        [JsonProperty(PropertyName = "month")]
        public int Month { get; set; }

        [JsonProperty(PropertyName = "day")]
        public int Day { get; set; }

        [JsonProperty(PropertyName = "hour")]
        public int Hour { get; set; }

        [JsonProperty(PropertyName = "minute")]
        public int Minute { get; set; }
    }
}
