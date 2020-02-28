using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Web.Models;
using WebVella.TagHelpers.Models;

namespace WebVella.Erp.Web.Utils
{
	public static class ModelExtensions
	{
		public static string GetLabel<T>(this T e) where T : IConvertible
		{
			string label = "";

			if (e is Enum)
			{
				Type type = e.GetType();
				Array values = Enum.GetValues(type);

				foreach (int val in values)
				{
					if (val == e.ToInt32(CultureInfo.InvariantCulture))
					{
						var memInfo = type.GetMember(type.GetEnumName(val));
						var soAttributes = memInfo[0].GetCustomAttributes(typeof(Api.Models.SelectOptionAttribute), false);
						if (soAttributes.Length > 0)
						{
							// we're only getting the first description we find
							// others will be ignored
							label = ((Api.Models.SelectOptionAttribute)soAttributes[0]).Label;
						}

						break;
					}
				}
			}

			return label;
		}

		public static List<SelectOption> GetEnumAsSelectOptions<T>()
		{
			var selectOptions = new List<SelectOption>();
			var values = Enum.GetValues(typeof(T));
			var type = typeof(T);
			foreach (var val in values)
			{
				var memInfo = type.GetMember(type.GetEnumName(val));
				var enumAuxAttributes = memInfo[0].GetCustomAttributes(typeof(SelectOptionAttribute), false);
				var label = "";
				var iconClass = "";
				var color = "";
				if (enumAuxAttributes.Length > 0)
				{
					// we're only getting the first description we find
					// others will be ignored
					var enumAux = (SelectOptionAttribute)enumAuxAttributes[0];
					label = enumAux.Label;
					iconClass = enumAux.IconClass;
					color = enumAux.Color;
				}

				selectOptions.Add(new SelectOption() { Value = ((int)val).ToString(), Label = label, Color = color, IconClass = iconClass });
			}
			return selectOptions;
		}
	
		public static List<KeyValuePair<string, string>> ToErrorList(this ValidationException validation, List<string> includeFields = null, List<string> excludeFields = null)
		{
			if(validation == null)
				return null;

			var result = new List<KeyValuePair<string, string>>();
			if(includeFields == null)
				includeFields = new List<string>();

			if(excludeFields == null)
				excludeFields = new List<string>();

			foreach (var valError in validation.Errors)
			{
				var isIncluded = false;
				
				if(includeFields.Count == 0)
					isIncluded = true;
				else if(includeFields.Contains(valError.PropertyName))
					isIncluded = true;
				if (excludeFields.Contains(valError.PropertyName))
					isIncluded = false;

				if (isIncluded)
					result.Add(new KeyValuePair<string, string>(valError.PropertyName, valError.Message));
			}

			return result;
		}		

		public static List<KeyValuePair<string, string>> ToKeyValuePair(this List<ValidationError> errors)
		{
			if(errors == null)
				return null;

			return errors.Select(x=> new KeyValuePair<string, string>(x.PropertyName,x.Message)).ToList();
		}	

		public static List<WvSelectOption> ToWvSelectOption(this List<SelectOption> originOptions)
		{
			if(originOptions == null)
				return null;

			return originOptions.Select(x=> new WvSelectOption{Color = x.Color,IconClass = x.IconClass,Label = x.Label, Value = x.Value}).ToList();
		}	

		public static WvSelectOptionsAjaxDatasource ToWvSelectOptionsAjaxDatasource(this SelectOptionsAjaxDatasource origin){
			if(origin == null)
				return null;

			var result = new WvSelectOptionsAjaxDatasource{
				DatasourceName = origin.DatasourceName,
				InitOptions = origin.InitOptions.ToWvSelectOption(),
				UseSelectApi = origin.UseSelectApi,
				Label = origin.Label,
				PageSize = origin.PageSize,
				Value = origin.Value
			};
			return result;
		}
	}
}
