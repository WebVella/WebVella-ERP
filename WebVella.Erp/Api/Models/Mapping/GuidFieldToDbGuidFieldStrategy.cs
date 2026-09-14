using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class GuidFieldToDbGuidFieldStrategy : IMapStrategy<GuidField, DbGuidField>
{
	public DbGuidField Map(GuidField source)
	{
		if (source == null)
			return null;

		var dest = new DbGuidField();
		FieldMappingHelpers.CopyFieldToDbBaseField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.GenerateNewId = source.GenerateNewId;
		return dest;
	}
}
