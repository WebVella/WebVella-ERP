using System.Collections.Generic;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Eql;

namespace WebVella.Erp.Hooks
{
	[Hook("Provide hook for point in code before entity search create.")]
	public interface IErpPreSearchRecordHook
	{
		void OnPreSearchRecord(string entityName, EqlSelectNode tree, List<EqlError> errors);
	}
}
