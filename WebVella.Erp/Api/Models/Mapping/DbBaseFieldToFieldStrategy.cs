using WebVella.Erp.Api.Models;
using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DbBaseFieldToFieldStrategy : IMapStrategy<DbBaseField, Field>
{
	public Field Map(DbBaseField source)
	{
		if (source == null)
			return null;

		return source switch
		{
			DbAutoNumberField s => StrategyRegistry.GetStrategy<DbAutoNumberField, AutoNumberField>().Map(s),
			DbCheckboxField s => StrategyRegistry.GetStrategy<DbCheckboxField, CheckboxField>().Map(s),
			DbCurrencyField s => StrategyRegistry.GetStrategy<DbCurrencyField, CurrencyField>().Map(s),
			DbDateField s => StrategyRegistry.GetStrategy<DbDateField, DateField>().Map(s),
			DbDateTimeField s => StrategyRegistry.GetStrategy<DbDateTimeField, DateTimeField>().Map(s),
			DbEmailField s => StrategyRegistry.GetStrategy<DbEmailField, EmailField>().Map(s),
			DbFileField s => StrategyRegistry.GetStrategy<DbFileField, FileField>().Map(s),
			DbGeographyField s => StrategyRegistry.GetStrategy<DbGeographyField, GeographyField>().Map(s),
			DbGuidField s => StrategyRegistry.GetStrategy<DbGuidField, GuidField>().Map(s),
			DbHtmlField s => StrategyRegistry.GetStrategy<DbHtmlField, HtmlField>().Map(s),
			DbImageField s => StrategyRegistry.GetStrategy<DbImageField, ImageField>().Map(s),
			DbMultiLineTextField s => StrategyRegistry.GetStrategy<DbMultiLineTextField, MultiLineTextField>().Map(s),
			DbMultiSelectField s => StrategyRegistry.GetStrategy<DbMultiSelectField, MultiSelectField>().Map(s),
			DbNumberField s => StrategyRegistry.GetStrategy<DbNumberField, NumberField>().Map(s),
			DbPasswordField s => StrategyRegistry.GetStrategy<DbPasswordField, PasswordField>().Map(s),
			DbPercentField s => StrategyRegistry.GetStrategy<DbPercentField, PercentField>().Map(s),
			DbPhoneField s => StrategyRegistry.GetStrategy<DbPhoneField, PhoneField>().Map(s),
			DbSelectField s => StrategyRegistry.GetStrategy<DbSelectField, SelectField>().Map(s),
			DbTextField s => StrategyRegistry.GetStrategy<DbTextField, TextField>().Map(s),
			DbUrlField s => StrategyRegistry.GetStrategy<DbUrlField, UrlField>().Map(s),
			_ => throw new System.InvalidOperationException($"No Field mapping registered for DbBaseField type: {source.GetType().Name}")
		};
	}
}
