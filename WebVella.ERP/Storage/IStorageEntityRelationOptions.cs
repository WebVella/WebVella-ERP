using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.ERP.Storage
{
    public interface IStorageEntityRelationOptions 
    {
        [JsonProperty(PropertyName = "relationId")]
        Guid? RelationId { get; set; }

        [JsonProperty(PropertyName = "relationName")]
        string RelationName { get; set; }

        [JsonProperty(PropertyName = "direction")]
        string Direction { get; set; }
    }
}
