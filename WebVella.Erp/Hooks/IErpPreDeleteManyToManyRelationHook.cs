using System;
using System.Collections.Generic;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Hooks
{
	[Hook("Provide hook for point in code before delete NN relation record.")]
	public interface IErpPreDeleteManyToManyRelationHook
	{
		void OnPreDelete(string relationName, Guid? originId, Guid? targetId, List<ErrorModel> errors);
	}
}
