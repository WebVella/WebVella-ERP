using System.Collections.Generic;
using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class DbEntityToEntityStrategy : IMapStrategy<DbEntity, Entity>
{
	public Entity Map(DbEntity source)
	{
		if (source == null)
			return null;

		var dest = new Entity();
		dest.Id = source.Id;
		dest.Name = source.Name;
		dest.Label = source.Label;
		dest.LabelPlural = source.LabelPlural;
		dest.System = source.System;
		dest.IconName = source.IconName;
		dest.Color = source.Color;
		dest.RecordScreenIdField = source.RecordScreenIdField;
		dest.RecordPermissions = source.RecordPermissions == null ? new RecordPermissions() : new RecordPermissions
		{
			CanRead = [.. source.RecordPermissions.CanRead ?? []],
			CanCreate = [.. source.RecordPermissions.CanCreate ?? []],
			CanUpdate = [.. source.RecordPermissions.CanUpdate ?? []],
			CanDelete = [.. source.RecordPermissions.CanDelete ?? []]
		};
		dest.Fields = new List<Field>();
		if (source.Fields != null)
		{
			foreach (var dbField in source.Fields)
			{
				Field field = dbField switch
				{
					DbAutoNumberField f => StrategyRegistry.GetStrategy<DbAutoNumberField, AutoNumberField>().Map(f),
					DbCheckboxField f => StrategyRegistry.GetStrategy<DbCheckboxField, CheckboxField>().Map(f),
					DbCurrencyField f => StrategyRegistry.GetStrategy<DbCurrencyField, CurrencyField>().Map(f),
					DbDateField f => StrategyRegistry.GetStrategy<DbDateField, DateField>().Map(f),
					DbDateTimeField f => StrategyRegistry.GetStrategy<DbDateTimeField, DateTimeField>().Map(f),
					DbEmailField f => StrategyRegistry.GetStrategy<DbEmailField, EmailField>().Map(f),
					DbFileField f => StrategyRegistry.GetStrategy<DbFileField, FileField>().Map(f),
					DbGeographyField f => StrategyRegistry.GetStrategy<DbGeographyField, GeographyField>().Map(f),
					DbGuidField f => StrategyRegistry.GetStrategy<DbGuidField, GuidField>().Map(f),
					DbHtmlField f => StrategyRegistry.GetStrategy<DbHtmlField, HtmlField>().Map(f),
					DbImageField f => StrategyRegistry.GetStrategy<DbImageField, ImageField>().Map(f),
					DbMultiLineTextField f => StrategyRegistry.GetStrategy<DbMultiLineTextField, MultiLineTextField>().Map(f),
					DbMultiSelectField f => StrategyRegistry.GetStrategy<DbMultiSelectField, MultiSelectField>().Map(f),
					DbNumberField f => StrategyRegistry.GetStrategy<DbNumberField, NumberField>().Map(f),
					DbPasswordField f => StrategyRegistry.GetStrategy<DbPasswordField, PasswordField>().Map(f),
					DbPercentField f => StrategyRegistry.GetStrategy<DbPercentField, PercentField>().Map(f),
					DbPhoneField f => StrategyRegistry.GetStrategy<DbPhoneField, PhoneField>().Map(f),
					DbSelectField f => StrategyRegistry.GetStrategy<DbSelectField, SelectField>().Map(f),
					DbTextField f => StrategyRegistry.GetStrategy<DbTextField, TextField>().Map(f),
					DbUrlField f => StrategyRegistry.GetStrategy<DbUrlField, UrlField>().Map(f),
					_ => null
				};
				if (field != null)
					dest.Fields.Add(field);
			}
		}
		return dest;
	}
}
