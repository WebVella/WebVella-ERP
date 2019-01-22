using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.Erp.Web.Models
{
    public class QuerySortJson
    {

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; } = "";

        [JsonProperty(PropertyName = "option")]
        public string Option { get; set; } = "";

        [JsonProperty(PropertyName = "default")]
        public string Default { get; set; } = "";

        [JsonProperty(PropertyName = "settings")]
        public QuerySortJsonSettings Settings { get; set; } = null;

    }

    public class QuerySortJsonSettings
    {

        [JsonProperty(PropertyName = "order")]
        public string Order { get; set; } = "";
    }
}
