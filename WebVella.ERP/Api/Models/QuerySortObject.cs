using Newtonsoft.Json;

namespace WebVella.ERP.Api.Models
{
    public class QuerySortObject
    {
        [JsonProperty(PropertyName = "fieldName")]
        public string FieldName { get; private set; }

        [JsonProperty(PropertyName = "sortType")]
        public QuerySortType SortType { get; private set; }

        public QuerySortObject( string fieldName, QuerySortType sortType )
        {
            FieldName = fieldName;
            SortType = sortType;
        }
    }

   
}