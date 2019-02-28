using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Web.Utils
{
	public static class DataUtils
	{
		public static void ValidateValueToFieldType(FieldType? fieldType, dynamic InValue, out dynamic OutValue, out List<string> errorList) {
			OutValue = null;
			errorList = new List<string>();
			if(InValue != null && InValue is Enum){
				InValue = ((int)InValue).ToString();
			}

			switch (fieldType)
			{
				case FieldType.AutoNumberField:
					{
						if (InValue == null || InValue.ToString() == "")
						{
							OutValue = null;
						}
						else if (InValue is decimal)
						{
							OutValue = (decimal)InValue;
						}
						else if (Decimal.TryParse(InValue.ToString(), out decimal result))
						{
							OutValue = result;
						}
						else
						{
							errorList.Add("Value should be a decimal");
						}
					}
					break;
				case FieldType.CheckboxField:
					{
						if (InValue == null || InValue.ToString() == "")
						{
							OutValue = null;
						}
						else if (InValue is bool)
						{
							OutValue = (bool)InValue;
						}
						else if (Boolean.TryParse(InValue.ToString(), out bool result))
						{
							OutValue = result;
						}
						else
						{
							errorList.Add("Value should be a boolean");
						}
					}
					break;
				case FieldType.CurrencyField:
				case FieldType.NumberField:
				case FieldType.PercentField:
					{
						if (InValue == null || InValue.ToString() == "")
						{
							OutValue = null;
						}
						else if (InValue is decimal)
						{
							OutValue = (decimal)InValue;
						}
						else if (Decimal.TryParse(InValue.ToString(), out decimal result))
						{
							OutValue = result;
						}
						else
						{
							errorList.Add("Value should be a decimal");
						}
					}
					break;
				case FieldType.DateField:
				case FieldType.DateTimeField:
					{
						if (InValue == null || InValue.ToString() == "")
						{
							OutValue = null;
						}
						else if (InValue is DateTime)
						{
							OutValue = (DateTime)InValue;
						}
						else if (DateTime.TryParse(InValue.ToString(), out DateTime result))
						{
							OutValue = result;
						}
						else
						{
							errorList.Add("Value should be a DateTime");
						}
					}
					break;
				case FieldType.EmailField:
					{
						if (InValue == null || InValue.ToString() == "")
						{
							OutValue = "";
						}
						else
						{
							var emailRgx = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
							if (!String.IsNullOrWhiteSpace(InValue) && !emailRgx.IsMatch(InValue.ToString()))
							{
								errorList.Add("Value is not a valid email!");
							}
							OutValue = InValue.ToString();
						}
					}
					break;
				case FieldType.GuidField:
					{
						if (InValue == null || InValue.ToString() == "")
						{
							OutValue = null;
						}
						else if (InValue is Guid)
						{
							OutValue = (Guid)InValue;
						}
						else if (Guid.TryParse(InValue.ToString(), out Guid result))
						{
							OutValue = result;
						}
						else
						{
							errorList.Add("Value should be a Guid");
						}
					}
					break;
				case FieldType.HtmlField:
					{
						if (InValue == null || InValue.ToString() == "")
						{
							OutValue = "";
						}
						else
						{
							InValue = InValue.ToString();
							//Check if Html value is valid
							HtmlDocument doc = new HtmlDocument();
							doc.LoadHtml(InValue);
							doc.OptionFixNestedTags = true;
							doc.OptionAutoCloseOnEnd = true;
							if (doc.ParseErrors != null && doc.ParseErrors.Count() > 0)
							{
								foreach (var error in doc.ParseErrors)
								{
									errorList.Add($"Invalid html on line {error.Line}. {error.Reason}");
								}
							}
							else
							{
								OutValue = doc.DocumentNode.OuterHtml;
							}
						}
					}
					break;
				case FieldType.MultiSelectField:
					{
						if (InValue == null || InValue.ToString() == "")
						{
							OutValue = new List<string>();
						}
						else if (InValue is List<string>)
						{
							OutValue = (List<string>)InValue;
						}
						else
						{
							var newList = new List<string>();
							newList.Add(InValue.ToString());
							OutValue = newList;
						}
					}
					break;
				case FieldType.FileField:
				case FieldType.ImageField:
				case FieldType.MultiLineTextField:
				case FieldType.PasswordField:
				case FieldType.PhoneField:
				case FieldType.SelectField:
				case FieldType.TextField:
				case FieldType.UrlField:
					{
						if (InValue == null || InValue.ToString() == "")
						{
							OutValue = "";
						}
						else
						{
							OutValue = InValue.ToString();
						}
					}
					break;
				default:
					{
						OutValue = InValue;
					}
					break;
			}
		}

		public static List<FilterType> GetFilterTypesForFieldType(FieldType fieldType) {
			var result = new List<FilterType>();

			switch (fieldType)
			{
				case FieldType.CheckboxField:
					{
						result.Add(FilterType.EQ);
					}
					break;
				case FieldType.AutoNumberField:
				case FieldType.CurrencyField:
				case FieldType.NumberField:
				case FieldType.PercentField:
					{
						result.Add(FilterType.EQ);
						result.Add(FilterType.NOT);
						result.Add(FilterType.LT);
						result.Add(FilterType.LTE);
						result.Add(FilterType.GT);
						result.Add(FilterType.GTE);
						result.Add(FilterType.BETWEEN);
						result.Add(FilterType.NOTBETWEEN);
					}
					break;
				case FieldType.DateField:
				case FieldType.DateTimeField:
					{
						result.Add(FilterType.EQ);
						result.Add(FilterType.NOT);
						result.Add(FilterType.LT);
						result.Add(FilterType.LTE);
						result.Add(FilterType.GT);
						result.Add(FilterType.GTE);
						result.Add(FilterType.BETWEEN);
						result.Add(FilterType.NOTBETWEEN);
					}
					break;
				case FieldType.GuidField:
					{
						result.Add(FilterType.EQ);
					}
					break;
				case FieldType.MultiSelectField:
					{
						result.Add(FilterType.CONTAINS);
					}
					break;
				default:
					{
						result.Add(FilterType.STARTSWITH);
						result.Add(FilterType.CONTAINS);
						result.Add(FilterType.EQ);
						result.Add(FilterType.NOT);
						result.Add(FilterType.REGEX);
						result.Add(FilterType.FTS);
					}
					break;
			}
			return result;
		}
	}
}
