using System.Collections.Generic;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Hooks
{
	[Hook("Provide hook for point in code before entity record search.")]
	public interface IErpPostSearchRecordHook
	{
		void OnPostSearchRecord(string entityName, List<EntityRecord> record);
	}
}
