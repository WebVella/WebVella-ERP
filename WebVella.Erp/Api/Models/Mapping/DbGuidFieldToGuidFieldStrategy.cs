using System;
using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DbGuidFieldToGuidFieldStrategy : IMapStrategy<DbGuidField, GuidField>
{
	public GuidField Map(DbGuidField source)
	{
		if (source == null)
			return null;

		var dest = new GuidField();
		FieldMappingHelpers.CopyDbBaseFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.GenerateNewId = source.GenerateNewId;
		return dest;
	}
}
