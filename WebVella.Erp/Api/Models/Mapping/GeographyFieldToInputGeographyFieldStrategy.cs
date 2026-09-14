namespace WebVella.Erp.Api.Models.Mapping;

internal class GeographyFieldToInputGeographyFieldStrategy : IMapStrategy<GeographyField, InputGeographyField>
{
	public InputGeographyField Map(GeographyField source)
	{
		if (source == null)
			return null;

		var dest = new InputGeographyField();
		FieldMappingHelpers.CopyFieldToInputField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.MaxLength = source.MaxLength;
		dest.VisibleLineNumber = source.VisibleLineNumber;
		dest.Format = source.Format;
		dest.SRID = source.SRID;
		return dest;
	}
}
