using System.Collections.Generic;
using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class RecordPermissionsToDbRecordPermissionsStrategy : IMapStrategy<RecordPermissions, DbRecordPermissions>
{
	public DbRecordPermissions Map(RecordPermissions source)
	{
		if (source == null)
			return null;

		return new DbRecordPermissions
		{
			CanRead = new List<System.Guid>(source.CanRead ?? new List<System.Guid>()),
			CanCreate = new List<System.Guid>(source.CanCreate ?? new List<System.Guid>()),
			CanUpdate = new List<System.Guid>(source.CanUpdate ?? new List<System.Guid>()),
			CanDelete = new List<System.Guid>(source.CanDelete ?? new List<System.Guid>())
		};
	}
}
