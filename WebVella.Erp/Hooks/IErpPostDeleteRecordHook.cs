using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Hooks
{
	[Hook("Provide hook for point in code after entity record is deleted.")]
	public interface IErpPostDeleteRecordHook 
	{
		void OnPostDeleteRecord(string entityName, EntityRecord record);
	}
}
