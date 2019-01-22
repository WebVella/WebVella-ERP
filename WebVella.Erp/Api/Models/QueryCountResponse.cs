using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.Erp.Api.Models
{
    public class QueryCountResponse : BaseResponseModel
    { 
        [JsonProperty(PropertyName = "object")]
        public long Object { get; set; }
	}
}
