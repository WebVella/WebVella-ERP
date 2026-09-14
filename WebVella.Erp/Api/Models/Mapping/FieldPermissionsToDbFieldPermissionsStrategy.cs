using System.Collections.Generic;
using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class FieldPermissionsToDbFieldPermissionsStrategy : IMapStrategy<FieldPermissions, DbFieldPermissions>
{
	public DbFieldPermissions Map(FieldPermissions source)
	{
		if (source == null)
			return null;

		return new DbFieldPermissions
		{
			CanRead = [.. source.CanRead ?? []],
			CanUpdate = [.. source.CanUpdate ?? []]
		};
	}
}
