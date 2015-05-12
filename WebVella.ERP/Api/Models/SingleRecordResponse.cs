using Newtonsoft.Json;

namespace WebVella.ERP.Api.Models
{
    public class SingleRecordResponse : BaseResponseModel
    {
        [JsonProperty(PropertyName = "object")]
        public SingleRecordResult Object { get; set; }
    }
}
