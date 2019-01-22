using System.Collections.Generic;

namespace WebVella.Erp.Api.Models
{
	public class EntityRecordList : List<EntityRecord>
	{
		public int TotalCount { get; set; } = 0;
	}
}
