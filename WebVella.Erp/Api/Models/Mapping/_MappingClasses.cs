using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace WebVella.Erp.Api.Models.Mapping;

public static class ErpMappingBootstrapper
{
	public static void Initialize()
	{
		//base classes
		StrategyRegistry.Register(new FieldToInputFieldStrategy());
		StrategyRegistry.Register(new InputFieldToFieldStrategy());
		StrategyRegistry.Register(new FieldToDbBaseFieldStrategy());
		StrategyRegistry.Register(new DbBaseFieldToFieldStrategy());

		StrategyRegistry.Register(new EntityRecordToErpUserStrategy());
		StrategyRegistry.Register(new ErpUserToEntityRecordStrategy());

		StrategyRegistry.Register(new EntityRecordToErpRoleStrategy());

		StrategyRegistry.Register(new CurrencyToCurrencyTypeStrategy());
		StrategyRegistry.Register(new CurrencyToSelectOptionStrategy());
		StrategyRegistry.Register(new CurrencyTypeToDbCurrencyTypeStrategy());
		StrategyRegistry.Register(new DbCurrencyTypeToCurrencyTypeStrategy());

		StrategyRegistry.Register(new SelectOptionToDbSelectOptionStrategy());
		StrategyRegistry.Register(new DbSelectOptionToSelectOptionStrategy());

		StrategyRegistry.Register(new EntityToInputEntityStrategy());
		StrategyRegistry.Register(new InputEntityToEntityStrategy());
		StrategyRegistry.Register(new EntityToDbEntityStrategy());
		StrategyRegistry.Register(new DbEntityToEntityStrategy());


		StrategyRegistry.Register(new EntityRelationToDbEntityRelationStrategy());
		StrategyRegistry.Register(new DbEntityRelationToEntityRelationStrategy());
		StrategyRegistry.Register(new EntityRelationOptionsItemToDbEntityRelationOptionsStrategy());
		StrategyRegistry.Register(new DbEntityRelationOptionsToEntityRelationOptionsItemStrategy());

		StrategyRegistry.Register(new FieldPermissionsToDbFieldPermissionsStrategy());
		StrategyRegistry.Register(new DbFieldPermissionsToFieldPermissionsStrategy());
		StrategyRegistry.Register(new RecordPermissionsToDbRecordPermissionsStrategy());
		StrategyRegistry.Register(new DbRecordPermissionsToRecordPermissionsStrategy());

		StrategyRegistry.Register(new ErrorModelToValidationErrorStrategy());


		StrategyRegistry.Register(new DataRowToDatabaseNNRelationRecordStrategy());
		StrategyRegistry.Register(new DataRowToDatabaseDataSourceStrategy());
		StrategyRegistry.Register(new DataRowToSearchResultStrategy());
		StrategyRegistry.Register(new DataRowToJobStrategy());
		StrategyRegistry.Register(new DataRowToSchedulePlanStrategy());

		StrategyRegistry.Register(new SchedulePlanToOutputSchedulePlanStrategy());

		StrategyRegistry.Register(new EntityRecordToUserFileStrategy());

		StrategyRegistry.Register(new AutoNumberFieldToInputAutoNumberFieldStrategy());
		StrategyRegistry.Register(new InputAutoNumberFieldToAutoNumberFieldStrategy());
		StrategyRegistry.Register(new AutoNumberFieldToDbAutoNumberFieldStrategy());
		StrategyRegistry.Register(new DbAutoNumberFieldToAutoNumberFieldStrategy());

		StrategyRegistry.Register(new CheckboxFieldToInputCheckboxFieldStrategy());
		StrategyRegistry.Register(new InputCheckboxFieldToCheckboxFieldStrategy());
		StrategyRegistry.Register(new CheckboxFieldToDbCheckboxFieldStrategy());
		StrategyRegistry.Register(new DbCheckboxFieldToCheckboxFieldStrategy());

		StrategyRegistry.Register(new CurrencyFieldToInputCurrencyFieldStrategy());
		StrategyRegistry.Register(new InputCurrencyFieldToCurrencyFieldStrategy());
		StrategyRegistry.Register(new CurrencyFieldToDbCurrencyFieldStrategy());
		StrategyRegistry.Register(new DbCurrencyFieldToCurrencyFieldStrategy());

		StrategyRegistry.Register(new DateFieldToInputDateFieldStrategy());
		StrategyRegistry.Register(new InputDateFieldToDateFieldStrategy());
		StrategyRegistry.Register(new DateFieldToDbDateFieldStrategy());
		StrategyRegistry.Register(new DbDateFieldToDateFieldStrategy());

		StrategyRegistry.Register(new DateTimeFieldToInputDateTimeFieldStrategy());
		StrategyRegistry.Register(new InputDateTimeFieldToDateTimeFieldStrategy());
		StrategyRegistry.Register(new DateTimeFieldToDbDateTimeFieldStrategy());
		StrategyRegistry.Register(new DbDateTimeFieldToDateTimeFieldStrategy());

		StrategyRegistry.Register(new EmailFieldToInputEmailFieldStrategy());
		StrategyRegistry.Register(new InputEmailFieldToEmailFieldStrategy());
		StrategyRegistry.Register(new EmailFieldToDbEmailFieldStrategy());
		StrategyRegistry.Register(new DbEmailFieldToEmailFieldStrategy());

		StrategyRegistry.Register(new FileFieldToInputFileFieldStrategy());
		StrategyRegistry.Register(new InputFileFieldToFileFieldStrategy());
		StrategyRegistry.Register(new FileFieldToDbFileFieldStrategy());
		StrategyRegistry.Register(new DbFileFieldToFileFieldStrategy());

		StrategyRegistry.Register(new GeographyFieldToInputGeographyFieldStrategy());
		StrategyRegistry.Register(new InputGeographyFieldToGeographyFieldStrategy());
		StrategyRegistry.Register(new GeographyFieldToDbGeographyFieldStrategy());
		StrategyRegistry.Register(new DbGeographyFieldToGeographyFieldStrategy());

		StrategyRegistry.Register(new GuidFieldToInputGuidFieldStrategy());
		StrategyRegistry.Register(new InputGuidFieldToGuidFieldStrategy());
		StrategyRegistry.Register(new GuidFieldToDbGuidFieldStrategy());
		StrategyRegistry.Register(new DbGuidFieldToGuidFieldStrategy());

		StrategyRegistry.Register(new HtmlFieldToInputHtmlFieldStrategy());
		StrategyRegistry.Register(new InputHtmlFieldToHtmlFieldStrategy());
		StrategyRegistry.Register(new HtmlFieldToDbHtmlFieldStrategy());
		StrategyRegistry.Register(new DbHtmlFieldToHtmlFieldStrategy());

		StrategyRegistry.Register(new ImageFieldToInputImageFieldStrategy());
		StrategyRegistry.Register(new InputImageFieldToImageFieldStrategy());
		StrategyRegistry.Register(new ImageFieldToDbImageFieldStrategy());
		StrategyRegistry.Register(new DbImageFieldToImageFieldStrategy());


		StrategyRegistry.Register(new MultiLineTextFieldToInputMultiLineTextFieldStrategy());
		StrategyRegistry.Register(new InputMultiLineTextFieldToMultiLineTextFieldStrategy());
		StrategyRegistry.Register(new MultiLineTextFieldToDbMultiLineTextFieldStrategy());
		StrategyRegistry.Register(new DbMultiLineTextFieldToMultiLineTextFieldStrategy());

		StrategyRegistry.Register(new MultiSelectFieldToInputMultiSelectFieldStrategy());
		StrategyRegistry.Register(new InputMultiSelectFieldToMultiSelectFieldStrategy());
		StrategyRegistry.Register(new MultiSelectFieldToDbMultiSelectFieldStrategy());
		StrategyRegistry.Register(new DbMultiSelectFieldToMultiSelectFieldStrategy());

		StrategyRegistry.Register(new NumberFieldToInputNumberFieldStrategy());
		StrategyRegistry.Register(new InputNumberFieldToNumberFieldStrategy());
		StrategyRegistry.Register(new NumberFieldToDbNumberFieldStrategy());
		StrategyRegistry.Register(new DbNumberFieldToNumberFieldStrategy());

		StrategyRegistry.Register(new PasswordFieldToInputPasswordFieldStrategy());
		StrategyRegistry.Register(new InputPasswordFieldToPasswordFieldStrategy());
		StrategyRegistry.Register(new PasswordFieldToDbPasswordFieldStrategy());
		StrategyRegistry.Register(new DbPasswordFieldToPasswordFieldStrategy());

		StrategyRegistry.Register(new PercentFieldToInputPercentFieldStrategy());
		StrategyRegistry.Register(new InputPercentFieldToPercentFieldStrategy());
		StrategyRegistry.Register(new PercentFieldToDbPercentFieldStrategy());
		StrategyRegistry.Register(new DbPercentFieldToPercentFieldStrategy());

		StrategyRegistry.Register(new PhoneFieldToInputPhoneFieldStrategy());
		StrategyRegistry.Register(new InputPhoneFieldToPhoneFieldStrategy());
		StrategyRegistry.Register(new PhoneFieldToDbPhoneFieldStrategy());
		StrategyRegistry.Register(new DbPhoneFieldToPhoneFieldStrategy());

		StrategyRegistry.Register(new SelectFieldToInputSelectFieldStrategy());
		StrategyRegistry.Register(new InputSelectFieldToSelectFieldStrategy());
		StrategyRegistry.Register(new SelectFieldToDbSelectFieldStrategy());
		StrategyRegistry.Register(new DbSelectFieldToSelectFieldStrategy());

		StrategyRegistry.Register(new TextFieldToInputTextFieldStrategy());
		StrategyRegistry.Register(new InputTextFieldToTextFieldStrategy());
		StrategyRegistry.Register(new TextFieldToDbTextFieldStrategy());
		StrategyRegistry.Register(new DbTextFieldToTextFieldStrategy());

		StrategyRegistry.Register(new UrlFieldToInputUrlFieldStrategy());
		StrategyRegistry.Register(new InputUrlFieldToUrlFieldStrategy());
		StrategyRegistry.Register(new UrlFieldToDbUrlFieldStrategy());
		StrategyRegistry.Register(new DbUrlFieldToUrlFieldStrategy());
	}
}


public interface IMapStrategy<TSource, TTarget>
{
	TTarget Map(TSource source);
}

public static class StrategyRegistry
{
	private static readonly ConcurrentDictionary<(Type Source, Type Target), object> _strategies = new();

	public static void Register<TSource, TTarget>(IMapStrategy<TSource, TTarget> strategy)
	{
		ArgumentNullException.ThrowIfNull(strategy);

		var key = (typeof(TSource), typeof(TTarget));
		_strategies.TryAdd(key, strategy);
	}

	public static IMapStrategy<TSource, TTarget> GetStrategy<TSource, TTarget>()
	{
		var sourceType = typeof(TSource);
		var targetType = typeof(TTarget);

		var result = FindStrategy(sourceType, targetType);
		if (result != null)
			return (IMapStrategy<TSource, TTarget>)result;

		throw new InvalidOperationException($"No mapping strategy is registered for: {sourceType.Name} -> {targetType.Name}." +
			$" Add call to StrategyRegistry.Register() in MappingBootstrapper.");
	}

	public static object GetStrategy(Type source, Type target)
	{
		var result = FindStrategy(source, target);
		if (result != null)
			return result;

		throw new InvalidOperationException($"No mapping strategy is registered for: {source.Name} -> {target.Name}." +
			$" Add call to StrategyRegistry.Register() in MappingBootstrapper.");
	}

	private static object FindStrategy(Type source, Type target)
	{
		var sourceType = source;
		while (sourceType != null && sourceType != typeof(object))
		{
			var targetType = target;
			while (targetType != null && targetType != typeof(object))
			{
				if (_strategies.TryGetValue((sourceType, targetType), out var strategy))
					return strategy;
				targetType = targetType.BaseType;
			}
			sourceType = sourceType.BaseType;
		}
		return null;
	}

	public static void Clear()
	{
		_strategies.Clear();
		_mapMethodCache.Clear();
	}

	private static readonly ConcurrentDictionary<Type, MethodInfo> _mapMethodCache = new();

	internal static object InvokeMap(object strategy, object source)
	{
		var mapMethod = _mapMethodCache.GetOrAdd(strategy.GetType(), t => t.GetMethod("Map")!);
		return mapMethod.Invoke(strategy, [source]);
	}
}

public static class MappingExtensions
{
	public static TTarget MapTo<TTarget>(this object source)
	{
		if (source == null)
			return default;

		var strategy = StrategyRegistry.GetStrategy(source.GetType(), typeof(TTarget));
		return (TTarget)StrategyRegistry.InvokeMap(strategy, source);
	}

	public static TTarget MapTo<TTarget>(this EntityRecord source)
	{
		if (source == null)
			return default;

		return StrategyRegistry.GetStrategy<EntityRecord, TTarget>().Map(source);
	}

	public static List<TTarget> MapTo<TTarget>(this IEnumerable<object> source)
	{
		if (source == null)
			return new List<TTarget>();

		var result = new List<TTarget>();
		foreach (var item in source)
		{
			if (item == null) continue;
			var strategy = StrategyRegistry.GetStrategy(item.GetType(), typeof(TTarget));
			result.Add((TTarget)StrategyRegistry.InvokeMap(strategy, item));
		}
		return result;
	}

	public static List<TTarget> MapTo<TTarget>(this IEnumerable<EntityRecord> source)
	{
		if (source == null)
			return new List<TTarget>();

		var strategy = StrategyRegistry.GetStrategy<EntityRecord, TTarget>();
		var result = new List<TTarget>();
		foreach (var item in source)
		{
			if (item == null) continue;
			result.Add(strategy.Map(item));
		}
		return result;
	}

	public static List<TTarget> MapTo<TTarget>(this DataRowCollection source)
	{
		if (source == null)
			return new List<TTarget>();

		var strategy = StrategyRegistry.GetStrategy<DataRow, TTarget>();
		var result = new List<TTarget>();
		foreach (DataRow item in source)
		{
			if (item == null) continue;
			result.Add(item.MapTo<TTarget>());
		}
		return result;
	}

	public static TResult MapToSingleObject<TResult>(this IEnumerable self)
	{
		if (self == null)
			return default;

		var strategy = StrategyRegistry.GetStrategy(self.GetType(), typeof(TResult));
		return (TResult)StrategyRegistry.InvokeMap(strategy, self);
	}
}
