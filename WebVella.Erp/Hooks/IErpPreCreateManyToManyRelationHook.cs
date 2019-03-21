using System;
using System.Collections.Generic;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Hooks
{
	[Hook("Provide hook for point in code before NN relation between 2 entity record is created.")]
	public interface IErpPreCreateManyToManyRelationHook
	{
		void OnPreCreate(string relationName, Guid originId, Guid targetId, List<ErrorModel> errors);
	}
}
