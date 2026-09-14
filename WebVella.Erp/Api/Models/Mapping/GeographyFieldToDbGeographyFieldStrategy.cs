using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class GeographyFieldToDbGeographyFieldStrategy : IMapStrategy<GeographyField, DbGeographyField>
{

	public DbGeographyField Map(GeographyField source)
	{
		if (source == null)
			return null;

		var dest = new DbGeographyField();
		FieldMappingHelpers.CopyFieldToDbBaseField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MaxLength = source.MaxLength;
		dest.VisibleLineNumber = source.VisibleLineNumber;

		dest.Format = source.Format.HasValue
			? (DbGeographyFieldFormat)(int)source.Format.Value
			: (DbGeographyFieldFormat?)null;

		dest.SRID = source.SRID;
		return dest;
	}
}
