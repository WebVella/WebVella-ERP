using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Api.Models.Mapping;

internal class FieldToInputFieldStrategy : IMapStrategy<Field, InputField>
{
	public InputField Map(Field source)
	{
		if (source == null)
			return null;

		return source switch
		{
			AutoNumberField s => StrategyRegistry.GetStrategy<AutoNumberField, InputAutoNumberField>().Map(s),
			CheckboxField s => StrategyRegistry.GetStrategy<CheckboxField, InputCheckboxField>().Map(s),
			CurrencyField s => StrategyRegistry.GetStrategy<CurrencyField, InputCurrencyField>().Map(s),
			DateField s => StrategyRegistry.GetStrategy<DateField, InputDateField>().Map(s),
			DateTimeField s => StrategyRegistry.GetStrategy<DateTimeField, InputDateTimeField>().Map(s),
			EmailField s => StrategyRegistry.GetStrategy<EmailField, InputEmailField>().Map(s),
			FileField s => StrategyRegistry.GetStrategy<FileField, InputFileField>().Map(s),
			GeographyField s => StrategyRegistry.GetStrategy<GeographyField, InputGeographyField>().Map(s),
			GuidField s => StrategyRegistry.GetStrategy<GuidField, InputGuidField>().Map(s),
			HtmlField s => StrategyRegistry.GetStrategy<HtmlField, InputHtmlField>().Map(s),
			ImageField s => StrategyRegistry.GetStrategy<ImageField, InputImageField>().Map(s),
			MultiLineTextField s => StrategyRegistry.GetStrategy<MultiLineTextField, InputMultiLineTextField>().Map(s),
			MultiSelectField s => StrategyRegistry.GetStrategy<MultiSelectField, InputMultiSelectField>().Map(s),
			NumberField s => StrategyRegistry.GetStrategy<NumberField, InputNumberField>().Map(s),
			PasswordField s => StrategyRegistry.GetStrategy<PasswordField, InputPasswordField>().Map(s),
			PercentField s => StrategyRegistry.GetStrategy<PercentField, InputPercentField>().Map(s),
			PhoneField s => StrategyRegistry.GetStrategy<PhoneField, InputPhoneField>().Map(s),
			SelectField s => StrategyRegistry.GetStrategy<SelectField, InputSelectField>().Map(s),
			TextField s => StrategyRegistry.GetStrategy<TextField, InputTextField>().Map(s),
			UrlField s => StrategyRegistry.GetStrategy<UrlField, InputUrlField>().Map(s),
			_ => throw new System.InvalidOperationException($"No InputField mapping registered for Field type: {source.GetType().Name}")
		};
	}
}
