using System.Collections.Generic;
using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DbFieldPermissionsToFieldPermissionsStrategy : IMapStrategy<DbFieldPermissions, FieldPermissions>
{
	public FieldPermissions Map(DbFieldPermissions source)
	{
		if (source == null)
			return null;

		return new FieldPermissions
		{
			CanRead = [.. source.CanRead ?? []],
			CanUpdate = [.. source.CanUpdate ?? []]
		};
	}
}
