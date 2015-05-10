using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Api.Models
{
    public class View
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
        public IList<ViewFilter> Filters { get; set; }

        [JsonProperty(PropertyName = "fields")]
        public IList<ViewField> Fields { get; set; }
    }

    public class ViewFilter
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

    public class ViewField
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? Id { get; set; }

        [JsonProperty(PropertyName = "entityId")]
        public Guid? EntityId { get; set; }

        [JsonProperty(PropertyName = "position")]
        public int? Position { get; set; }
    }

    public class ViewList
    {
        [JsonProperty(PropertyName = "offset")]
        public Guid Offset { get; set; }

        [JsonProperty(PropertyName = "views")]
        public List<View> Views { get; set; }
    }

    public class ViewResponse : BaseResponseModel
    {
        [JsonProperty(PropertyName = "object")]
        public View Object { get; set; }
    }

    public class ViewListResponse : BaseResponseModel
    {
        [JsonProperty(PropertyName = "object")]
        public ViewList Object { get; set; }
    }
}