using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DbGeographyFieldToGeographyFieldStrategy : IMapStrategy<DbGeographyField, GeographyField>
{
	public GeographyField Map(DbGeographyField source)
	{
		if (source == null)
			return null;

		var dest = new GeographyField();
		FieldMappingHelpers.CopyDbBaseFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MaxLength = source.MaxLength;
		dest.VisibleLineNumber = source.VisibleLineNumber;

		dest.Format = source.Format.HasValue
			? (GeographyFieldFormat)(int)source.Format.Value
			: (GeographyFieldFormat?)null;

		dest.SRID = source.SRID;
		return dest;
	}
}
