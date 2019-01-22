using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Web.Models;

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
						var soAttributes = memInfo[0].GetCustomAttributes(typeof(SelectOptionAttribute), false);
						if (soAttributes.Length > 0)
						{
							// we're only getting the first description we find
							// others will be ignored
							label = ((SelectOptionAttribute)soAttributes[0]).Label;
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
	}
}
