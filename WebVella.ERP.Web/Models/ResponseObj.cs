using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.ERP.Web.Models
{
	public class ResponseObj
	{
		public ResponseObj()
		{
			Success = false;
			Message = String.Empty;
			Timestamp = DateTime.Now;
			Errors = new List<ResponseError>();
			Object = null;
		}

		[JsonProperty(PropertyName = "success")]
		public bool Success { get; set; }

		[JsonProperty(PropertyName = "message")]
		public string Message { get; set; }

		[JsonProperty(PropertyName = "timestamp")]
		public DateTime Timestamp { get; set; }

		[JsonProperty(PropertyName = "errors")]
		public List<ResponseError> Errors { get; set; }

		[JsonProperty(PropertyName = "object")]
		public dynamic Object { get; set; }

	}

	public class ResponseError
	{
		public ResponseError()
		{
			Key = String.Empty;
			Value = String.Empty;
			Message = String.Empty;
		}

		[JsonProperty(PropertyName = "key")]
		public string Key { get; set; }

		[JsonProperty(PropertyName = "value")]
		public string Value { get; set; }

		[JsonProperty(PropertyName = "message")]
		public string Message { get; set; }

	}

	public class AccessWarning
	{
		public AccessWarning()
		{
			Key = String.Empty;
			Value = String.Empty;
			Message = String.Empty;
		}

		[JsonProperty(PropertyName = "key")]
		public String Key { get; set; }
		[JsonProperty(PropertyName = "value")]
		public String Value { get; set; }
		[JsonProperty(PropertyName = "Message")]
		public String Message { get; set; }
	}
}