using Newtonsoft.Json;
using System.Collections.Generic;
using WebVella.Erp.Api.Models;
namespace WebVella.Erp.Web.Models
{
	public class DataSourceTestModel
	{
		[JsonProperty(PropertyName = "action")]
		public string Action { get; set; }

		[JsonProperty(PropertyName = "eql")]
		public string Eql { get; set; }

		[JsonProperty(PropertyName = "parameters")]
		public string Parameters { get; set; }

		[JsonProperty(PropertyName = "param_list")]
		public List<DataSourceParameter> ParamList { get; set; }
	}
}
