using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.ERP.Api.Models
{
    public class RecordView
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "fields")]
        public IList<RecordViewField> Fields { get; set; }
    }

    public class RecordViewField
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? Id { get; set; }

        [JsonProperty(PropertyName = "entityId")]
        public Guid? EntityId { get; set; }

        [JsonProperty(PropertyName = "column")]
        public RecordViewColumns? Column { get; set; }

        [JsonProperty(PropertyName = "position")]
        public int? Position { get; set; }
    }

    public class RecordViewCollection
    {
        [JsonProperty(PropertyName = "forms")]
        public List<RecordView> Forms { get; set; }
    }

    public class RecordViewResponse : BaseResponseModel
    {
        [JsonProperty(PropertyName = "object")]
        public RecordView Object { get; set; }
    }

    public class RecordViewCollectionResponse : BaseResponseModel
    {
        [JsonProperty(PropertyName = "object")]
        public RecordViewCollection Object { get; set; }
    }
}