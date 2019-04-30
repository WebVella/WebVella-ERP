using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebVella.Erp.Api.Models
{
	public class EntityRecordList : List<EntityRecord>
	{
		[JsonProperty(PropertyName = "TotalCount")]
		public int TotalCount { get; set; } = 0;
	}
}
