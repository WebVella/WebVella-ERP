using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DbPhoneFieldToPhoneFieldStrategy : IMapStrategy<DbPhoneField, PhoneField>
{
	public PhoneField Map(DbPhoneField source)
	{
		if (source == null)
			return null;

		var dest = new PhoneField();
		FieldMappingHelpers.CopyDbBaseFieldToField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.Format = source.Format;
		dest.MaxLength = source.MaxLength;
		return dest;
	}
}
