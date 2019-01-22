using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.Erp.Api.Models
{
	public class UserFile
	{
		[JsonProperty(PropertyName = "id")]
		public Guid Id { get; set; }

		[JsonProperty(PropertyName = "alt")]
		public string Alt { get; set; }
		
		[JsonProperty(PropertyName = "caption")]
		public string Caption { get; set; }

		[JsonProperty(PropertyName = "created_on")]
		public DateTime CreatedOn { get; set; }

		[JsonProperty(PropertyName = "customer_id")]
		public Guid? CustomerId { get; set; }

		[JsonProperty(PropertyName = "height")]
		public decimal Height { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "path")]
		public string Path { get; set; }

		[JsonProperty(PropertyName = "size")]
		public decimal Size { get; set; }

		[JsonProperty(PropertyName = "type")]
		public string Type { get; set; }

		[JsonProperty(PropertyName = "width")]
		public decimal Width { get; set; }


		public static string GetQueryColumns()
		{
			var columns = "*";

			return columns;
		}

	}
}
