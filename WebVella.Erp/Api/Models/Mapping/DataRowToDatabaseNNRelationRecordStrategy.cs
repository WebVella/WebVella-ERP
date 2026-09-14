using System;
using System.Data;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DataRowToDatabaseNNRelationRecordStrategy : IMapStrategy<DataRow, DatabaseNNRelationRecord>
{
	public DatabaseNNRelationRecord Map(DataRow source)
	{
		if (source == null)
			return null;

		var dest = new DatabaseNNRelationRecord();
		dest.OriginId = (Guid)source["origin_id"];
		dest.TargetId = (Guid)source["target_id"];
		return dest;
	}
}
