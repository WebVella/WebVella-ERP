using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.Erp.Api.Models
{
    public class MetaChangeResponseModel
    {
		public MetaChangeResponseModel() {
			Code = "";
			Changes = new List<MetaChangeModel>();
			Message = "";
			Success = true;
		}
	
		[JsonProperty(PropertyName = "code")]
		public string Code { get; set; }

		[JsonProperty(PropertyName = "changes")]
		public List<MetaChangeModel> Changes { get; set; }

		[JsonProperty(PropertyName = "success")]
		public bool Success { get; set; }

		[JsonProperty(PropertyName = "message")]
		public string Message { get; set; }

    }


    public class MetaChangeModel
    {
		[JsonProperty(PropertyName = "element")]
		public string Element { get; set; }

		[JsonProperty(PropertyName = "type")]
		public string Type { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "change_list")]
		public List<string> ChangeList { get; set; } = new List<string>();

    }

    public class UpdateCheckResponse
    {
		public UpdateCheckResponse() {
			HasUpdate = false;
			Code = string.Empty;
			ChangeList = new List<string>();
		}
		
		[JsonProperty(PropertyName = "hasUpdate")]
		public bool HasUpdate { get; set; }

		[JsonProperty(PropertyName = "code")]
		public string Code { get; set; }

		[JsonProperty(PropertyName = "change_list")]
		public List<string> ChangeList { get; set; }


    }
}
