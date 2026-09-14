using System.Collections.Generic;
using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal class EntityToDbEntityStrategy : IMapStrategy<Entity, DbEntity>
{
	public DbEntity Map(Entity source)
	{
		if (source == null)
			return null;

		var dest = new DbEntity();
		dest.Id = source.Id;
		dest.Name = source.Name;
		dest.Label = source.Label;
		dest.LabelPlural = source.LabelPlural;
		dest.System = source.System;
		dest.IconName = source.IconName;
		dest.Color = source.Color;
		dest.RecordScreenIdField = source.RecordScreenIdField;
		dest.RecordPermissions = source.RecordPermissions == null ? new DbRecordPermissions() : new DbRecordPermissions
		{
			CanRead = [.. source.RecordPermissions.CanRead ?? []],
			CanCreate = [.. source.RecordPermissions.CanCreate ?? []],
			CanUpdate = [.. source.RecordPermissions.CanUpdate ?? []],
			CanDelete = [.. source.RecordPermissions.CanDelete ?? []]
		};
		dest.Fields = new List<DbBaseField>();
		if (source.Fields != null)
		{
			foreach (var field in source.Fields)
			{
				DbBaseField dbField = field switch
				{
					AutoNumberField f => StrategyRegistry.GetStrategy<AutoNumberField, DbAutoNumberField>().Map(f),
					CheckboxField f => StrategyRegistry.GetStrategy<CheckboxField, DbCheckboxField>().Map(f),
					CurrencyField f => StrategyRegistry.GetStrategy<CurrencyField, DbCurrencyField>().Map(f),
					DateField f => StrategyRegistry.GetStrategy<DateField, DbDateField>().Map(f),
					DateTimeField f => StrategyRegistry.GetStrategy<DateTimeField, DbDateTimeField>().Map(f),
					EmailField f => StrategyRegistry.GetStrategy<EmailField, DbEmailField>().Map(f),
					FileField f => StrategyRegistry.GetStrategy<FileField, DbFileField>().Map(f),
					GeographyField f => StrategyRegistry.GetStrategy<GeographyField, DbGeographyField>().Map(f),
					GuidField f => StrategyRegistry.GetStrategy<GuidField, DbGuidField>().Map(f),
					HtmlField f => StrategyRegistry.GetStrategy<HtmlField, DbHtmlField>().Map(f),
					ImageField f => StrategyRegistry.GetStrategy<ImageField, DbImageField>().Map(f),
					MultiLineTextField f => StrategyRegistry.GetStrategy<MultiLineTextField, DbMultiLineTextField>().Map(f),
					MultiSelectField f => StrategyRegistry.GetStrategy<MultiSelectField, DbMultiSelectField>().Map(f),
					NumberField f => StrategyRegistry.GetStrategy<NumberField, DbNumberField>().Map(f),
					PasswordField f => StrategyRegistry.GetStrategy<PasswordField, DbPasswordField>().Map(f),
					PercentField f => StrategyRegistry.GetStrategy<PercentField, DbPercentField>().Map(f),
					PhoneField f => StrategyRegistry.GetStrategy<PhoneField, DbPhoneField>().Map(f),
					SelectField f => StrategyRegistry.GetStrategy<SelectField, DbSelectField>().Map(f),
					TextField f => StrategyRegistry.GetStrategy<TextField, DbTextField>().Map(f),
					UrlField f => StrategyRegistry.GetStrategy<UrlField, DbUrlField>().Map(f),
					_ => null
				};
				if (dbField != null)
					dest.Fields.Add(dbField);
			}
		}
		return dest;
	}
}
