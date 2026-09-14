using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class PhoneFieldToDbPhoneFieldStrategy : IMapStrategy<PhoneField, DbPhoneField>
{
	public DbPhoneField Map(PhoneField source)
	{
		if (source == null)
			return null;

		var dest = new DbPhoneField();
		FieldMappingHelpers.CopyFieldToDbBaseField(source, dest);
		dest.DefaultValue = source.DefaultValue;
		dest.Format = source.Format;
		dest.MaxLength = source.MaxLength;
		return dest;
	}
}
