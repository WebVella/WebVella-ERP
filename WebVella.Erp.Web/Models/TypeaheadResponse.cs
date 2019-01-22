using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.Erp.Web.Models
{
    public class TypeaheadResponse
    {
        [JsonProperty(PropertyName = "results")]
        public List<TypeaheadResponseRow> Results { get; set; } = new List<TypeaheadResponseRow>();

        [JsonProperty(PropertyName = "pagination")]
        public TypeaheadResponsePagination Pagination { get; set; } = new TypeaheadResponsePagination();
    }

    public class TypeaheadResponseRow
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "iconName")]
        public string IconName { get; set; } = "database";

        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; } = "teal";

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; } = "";

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; } = "";

        [JsonProperty(PropertyName = "fieldName")]
        public string FieldName { get; set; } = "";
    }
    public class TypeaheadResponsePagination {
        [JsonProperty(PropertyName = "more")]
        public bool More { get; set; } = false;
    }

}
