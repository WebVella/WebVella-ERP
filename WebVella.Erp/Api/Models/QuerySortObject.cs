using Newtonsoft.Json;
using System;

namespace WebVella.Erp.Api.Models
{
	[Serializable]
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