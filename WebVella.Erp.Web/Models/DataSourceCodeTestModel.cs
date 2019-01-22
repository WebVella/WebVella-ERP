using Newtonsoft.Json;
using System.Collections.Generic;
using WebVella.Erp.Api.Models;
namespace WebVella.Erp.Web.Models
{
	public class DataSourceCodeTestModel
	{
		[JsonProperty(PropertyName = "csCode")]
		public string CsCode { get; set; }
	}
}
