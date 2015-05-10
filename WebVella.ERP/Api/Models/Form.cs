using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.ERP.Api.Models
{
    public class Form
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "fields")]
        public IList<FormField> Fields { get; set; }
    }

    public class FormField
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? Id { get; set; }

        [JsonProperty(PropertyName = "entityId")]
        public Guid? EntityId { get; set; }

        [JsonProperty(PropertyName = "column")]
        public FormColumns? Column { get; set; }

        [JsonProperty(PropertyName = "position")]
        public int? Position { get; set; }
    }

    public class FormList
    {
        [JsonProperty(PropertyName = "offset")]
        public Guid Offset { get; set; }

        [JsonProperty(PropertyName = "forms")]
        public List<Form> Forms { get; set; }
    }

    public class FormResponse : BaseResponseModel
    {
        [JsonProperty(PropertyName = "object")]
        public Form Object { get; set; }
    }

    public class FormListResponse : BaseResponseModel
    {
        [JsonProperty(PropertyName = "object")]
        public FormList Object { get; set; }
    }
}