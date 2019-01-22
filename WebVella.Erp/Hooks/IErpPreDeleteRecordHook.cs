using System.Collections.Generic;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Hooks
{
	[Hook("Provide hook for point in code before entity record delete.")]
	public interface IErpPreDeleteRecordHook
	{
		void OnPreDeleteRecord(string entityName, EntityRecord record, List<ErrorModel> errors);
	}
}
