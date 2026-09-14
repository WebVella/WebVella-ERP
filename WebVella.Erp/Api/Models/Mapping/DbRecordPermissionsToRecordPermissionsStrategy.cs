using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DbRecordPermissionsToRecordPermissionsStrategy : IMapStrategy<DbRecordPermissions, RecordPermissions>
{
	public RecordPermissions Map(DbRecordPermissions source)
	{
		if (source == null)
			return null;

		return new RecordPermissions
		{
			CanRead = [.. source.CanRead ?? []],
			CanCreate = [.. source.CanCreate ?? []],
			CanUpdate = [.. source.CanUpdate ?? []],
			CanDelete = [.. source.CanDelete ?? []]
		};
	}
}
