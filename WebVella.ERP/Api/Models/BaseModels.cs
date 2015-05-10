using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.ERP.Api.Models
{
    public class BaseResponseModel
    {
        [JsonProperty(PropertyName = "timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public List<ErrorModel> Errors { get; set; }

        public BaseResponseModel()
        {
            Errors = new List<ErrorModel>();
        }
    }

    public class AccessWarningModel
    {
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }

    public class ErrorModel
    {
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        public ErrorModel()
        {
        }

        public ErrorModel(string key, string value, string message)
        {
            Key = key;
            Value = value;
            Message = message;
        }
    }
}