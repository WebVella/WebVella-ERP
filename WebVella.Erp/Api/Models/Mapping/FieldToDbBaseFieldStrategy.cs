using WebVella.Erp.Api.Models;
using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class FieldToDbBaseFieldStrategy : IMapStrategy<Field, DbBaseField>
{
	public DbBaseField Map(Field source)
	{
		if (source == null)
			return null;

		return source switch
		{
			AutoNumberField s => StrategyRegistry.GetStrategy<AutoNumberField, DbAutoNumberField>().Map(s),
			CheckboxField s => StrategyRegistry.GetStrategy<CheckboxField, DbCheckboxField>().Map(s),
			CurrencyField s => StrategyRegistry.GetStrategy<CurrencyField, DbCurrencyField>().Map(s),
			DateField s => StrategyRegistry.GetStrategy<DateField, DbDateField>().Map(s),
			DateTimeField s => StrategyRegistry.GetStrategy<DateTimeField, DbDateTimeField>().Map(s),
			EmailField s => StrategyRegistry.GetStrategy<EmailField, DbEmailField>().Map(s),
			FileField s => StrategyRegistry.GetStrategy<FileField, DbFileField>().Map(s),
			GeographyField s => StrategyRegistry.GetStrategy<GeographyField, DbGeographyField>().Map(s),
			GuidField s => StrategyRegistry.GetStrategy<GuidField, DbGuidField>().Map(s),
			HtmlField s => StrategyRegistry.GetStrategy<HtmlField, DbHtmlField>().Map(s),
			ImageField s => StrategyRegistry.GetStrategy<ImageField, DbImageField>().Map(s),
			MultiLineTextField s => StrategyRegistry.GetStrategy<MultiLineTextField, DbMultiLineTextField>().Map(s),
			MultiSelectField s => StrategyRegistry.GetStrategy<MultiSelectField, DbMultiSelectField>().Map(s),
			NumberField s => StrategyRegistry.GetStrategy<NumberField, DbNumberField>().Map(s),
			PasswordField s => StrategyRegistry.GetStrategy<PasswordField, DbPasswordField>().Map(s),
			PercentField s => StrategyRegistry.GetStrategy<PercentField, DbPercentField>().Map(s),
			PhoneField s => StrategyRegistry.GetStrategy<PhoneField, DbPhoneField>().Map(s),
			SelectField s => StrategyRegistry.GetStrategy<SelectField, DbSelectField>().Map(s),
			TextField s => StrategyRegistry.GetStrategy<TextField, DbTextField>().Map(s),
			UrlField s => StrategyRegistry.GetStrategy<UrlField, DbUrlField>().Map(s),
			_ => throw new System.InvalidOperationException($"No DbBaseField mapping registered for Field type: {source.GetType().Name}")
		};
	}
}
