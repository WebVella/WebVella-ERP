using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Api.Models
{
    public class RecordsList
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "type")]
        public ViewTypes Type { get; set; }

        [JsonProperty(PropertyName = "filters")]
        public IList<RecordsListFilter> Filters { get; set; }

        [JsonProperty(PropertyName = "fields")]
        public IList<RecordsListField> Fields { get; set; }
    }

    public class RecordsListFilter
    {
        [JsonProperty(PropertyName = "entityId")]
        public Guid? EntityId { get; set; }

        [JsonProperty(PropertyName = "fieldId")]
        public Guid? FieldId { get; set; }

        [JsonProperty(PropertyName = "operator")]
        public FilterOperatorTypes Operator { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }

    public class RecordsListField
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? Id { get; set; }

        [JsonProperty(PropertyName = "entityId")]
        public Guid? EntityId { get; set; }

        [JsonProperty(PropertyName = "position")]
        public int? Position { get; set; }
    }

    public class RecordsListCollection
    {
        [JsonProperty(PropertyName = "offset")]
        public Guid Offset { get; set; }

        [JsonProperty(PropertyName = "views")]
        public List<RecordsList> Views { get; set; }
    }

    public class RecordsListResponse : BaseResponseModel
    {
        [JsonProperty(PropertyName = "object")]
        public RecordsList Object { get; set; }
    }

    public class RecordsListCollectionResponse : BaseResponseModel
    {
        [JsonProperty(PropertyName = "object")]
        public RecordsListCollection Object { get; set; }
    }
}