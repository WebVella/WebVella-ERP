namespace WebVella.Erp.Api.Models.Mapping;

internal class InputGeographyFieldToGeographyFieldStrategy : IMapStrategy<InputGeographyField, GeographyField>
{
	public GeographyField Map(InputGeographyField source)
	{
		if (source == null)
			return null;

		var dest = new GeographyField();
		FieldMappingHelpers.CopyInputFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MaxLength = source.MaxLength;
		dest.VisibleLineNumber = source.VisibleLineNumber;
		dest.Format = source.Format;
		dest.SRID = source.SRID;
		return dest;
	}
}
