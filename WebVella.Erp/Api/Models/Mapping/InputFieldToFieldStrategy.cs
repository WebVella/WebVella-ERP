namespace WebVella.Erp.Api.Models.Mapping;

internal class InputFieldToFieldStrategy : IMapStrategy<InputField, Field>
{
	public Field Map(InputField source)
	{
		if (source == null)
			return null;

		return source switch
		{
			InputAutoNumberField s => StrategyRegistry.GetStrategy<InputAutoNumberField, AutoNumberField>().Map(s),
			InputCheckboxField s => StrategyRegistry.GetStrategy<InputCheckboxField, CheckboxField>().Map(s),
			InputCurrencyField s => StrategyRegistry.GetStrategy<InputCurrencyField, CurrencyField>().Map(s),
			InputDateField s => StrategyRegistry.GetStrategy<InputDateField, DateField>().Map(s),
			InputDateTimeField s => StrategyRegistry.GetStrategy<InputDateTimeField, DateTimeField>().Map(s),
			InputEmailField s => StrategyRegistry.GetStrategy<InputEmailField, EmailField>().Map(s),
			InputFileField s => StrategyRegistry.GetStrategy<InputFileField, FileField>().Map(s),
			InputGeographyField s => StrategyRegistry.GetStrategy<InputGeographyField, GeographyField>().Map(s),
			InputGuidField s => StrategyRegistry.GetStrategy<InputGuidField, GuidField>().Map(s),
			InputHtmlField s => StrategyRegistry.GetStrategy<InputHtmlField, HtmlField>().Map(s),
			InputImageField s => StrategyRegistry.GetStrategy<InputImageField, ImageField>().Map(s),
			InputMultiLineTextField s => StrategyRegistry.GetStrategy<InputMultiLineTextField, MultiLineTextField>().Map(s),
			InputMultiSelectField s => StrategyRegistry.GetStrategy<InputMultiSelectField, MultiSelectField>().Map(s),
			InputNumberField s => StrategyRegistry.GetStrategy<InputNumberField, NumberField>().Map(s),
			InputPasswordField s => StrategyRegistry.GetStrategy<InputPasswordField, PasswordField>().Map(s),
			InputPercentField s => StrategyRegistry.GetStrategy<InputPercentField, PercentField>().Map(s),
			InputPhoneField s => StrategyRegistry.GetStrategy<InputPhoneField, PhoneField>().Map(s),
			InputSelectField s => StrategyRegistry.GetStrategy<InputSelectField, SelectField>().Map(s),
			InputTextField s => StrategyRegistry.GetStrategy<InputTextField, TextField>().Map(s),
			InputUrlField s => StrategyRegistry.GetStrategy<InputUrlField, UrlField>().Map(s),
			_ => throw new System.InvalidOperationException($"No Field mapping registered for InputField type: {source.GetType().Name}")
		};
	}
}
