using Newtonsoft.Json;
using System;

namespace WebVella.Erp.Database
{
    public class DbEntityRelationOptions
    {
		[JsonProperty(PropertyName = "relation_id")]
        public Guid? RelationId { get; set; }

		[JsonProperty(PropertyName = "relation_name")]
        public string RelationName { get; set; }

		[JsonProperty(PropertyName = "direction")]
        public string Direction { get; set; }
    }
}