using System;

namespace WebVella.Erp.Hooks
{
	[Hook("Provide hook for point in code after relation NN record is created.")]
	public interface IErpPostCreateManyToManyRelationHook
	{
		void OnPostCreate(string relationName, Guid originId, Guid targetId );
	}
}
