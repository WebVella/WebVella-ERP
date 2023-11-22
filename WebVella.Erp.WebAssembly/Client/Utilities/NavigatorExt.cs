

using Microsoft.Extensions.Primitives;
using System.Collections;

namespace WebVella.Erp.WebAssembly.Utilities
{
	public class NavigatorExt
	{
		public static async Task ApplyChangeToUrlQuery(NavigationManager navigator, Dictionary<string, object> replaceDict, bool forceLoad = false)
		{
			var currentUrl = navigator.Uri;
			var uri = new System.Uri(currentUrl);
			var queryDictionary = System.Web.HttpUtility.ParseQueryString(uri.Query);

			var newQueryDictionary = new Dictionary<string, string>();
			foreach (string key in queryDictionary.Keys)
			{
				if (!replaceDict.Keys.Contains(key))
				{
					var queryValue = queryDictionary[key];
					if (!string.IsNullOrWhiteSpace(queryValue))
						newQueryDictionary[key] = queryValue;
				}
			}

			foreach (string key in replaceDict.Keys)
			{
				var queryValue = replaceDict[key];
				if (queryValue is null)
					continue;

				if (IsList(queryValue))
				{
					var asIList = (IList)queryValue;
					if (asIList.Count > 0)
					{
						var firstElement = asIList[0];
						if (firstElement is string)
						{
							var encodedList = new List<string>();
							foreach (var value in asIList)
							{
								encodedList.Add(ProcessQueryValueForUrl((string)value));
							}
							if (encodedList.Count > 0)
							{
								newQueryDictionary[key] = String.Join(",", encodedList);
							}
						}
						else if (firstElement is int)
						{
							var encodedList = new List<string>();
							foreach (var value in asIList)
							{
								encodedList.Add(((int)value).ToString());
							}
							if (encodedList.Count > 0)
							{
								newQueryDictionary[key] = String.Join(",", encodedList);
							}
						}
						else if (firstElement is Enum)
						{
							var encodedList = new List<string>();
							foreach (var value in asIList)
							{
								encodedList.Add(((int)value).ToString());
							}
							if (encodedList.Count > 0)
							{
								newQueryDictionary[key] = String.Join(",", encodedList);
							}
						}
					}
				}
				else if (queryValue is int)
				{
					var value = (int)queryValue;
					newQueryDictionary[key] = ((int)value).ToString();
				}
				else if (queryValue is Enum)
				{
					var value = (int)queryValue;
					newQueryDictionary[key] = ((int)value).ToString();
				}
				else if (queryValue is string)
				{
					var value = (string)queryValue;
					if (!string.IsNullOrWhiteSpace(value))
						newQueryDictionary[key] = ProcessQueryValueForUrl(value);
				}
				else if (queryValue is Guid?)
				{
					var value = (Guid?)queryValue;
					if (value is not null)
						newQueryDictionary[key] = value.ToString();
				}
				else if (queryValue is bool?)
				{
					var value = (bool?)queryValue;
					if (value is not null)
						newQueryDictionary[key] = value.ToString();
				}
				else
					throw new Exception("Query type not supported by utility method");
			}

			var queryStringList = new List<string>();
			foreach (var key in newQueryDictionary.Keys)
			{
				queryStringList.Add($"{key}={newQueryDictionary[key]}");
			}
			var urlQueryString = "";
			if (queryStringList.Count > 0)
				urlQueryString = "?" + string.Join("&", queryStringList);
			navigator.NavigateTo(uri.LocalPath + urlQueryString, forceLoad);
			if (!forceLoad)
				await Task.Delay(1);

		}

		public static Dictionary<string, string> ParseQueryString(string queryString)
		{
			var nvc = HttpUtility.ParseQueryString(queryString);
			return nvc.AllKeys.ToDictionary(k => k, k => nvc[k]);
		}

		public static string GetStringFromQuery(NavigationManager navigator, string paramName, string defaultValue = null)
		{
			var urlAbsolute = navigator.ToAbsoluteUri(navigator.Uri);
			var queryDict = ParseQueryString(urlAbsolute.Query);
			if (queryDict.ContainsKey(paramName))
				return ProcessQueryValueFromUrl(queryDict[paramName]) ?? defaultValue;
			return defaultValue;
		}

		public static List<string> GetStringListFromQuery(NavigationManager navigator, string paramName, List<string> defaultValue = null)
		{
			//We use comma separated before encoding
			var paramValue = GetStringFromQuery(navigator, paramName, null);
			if (String.IsNullOrWhiteSpace(paramValue))
				return defaultValue;

			return paramValue.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
		}

		public static Guid? GetGuidFromQuery(NavigationManager navigator, string paramName, Guid? defaultValue = null)
		{
			if (Guid.TryParse(NavigatorExt.GetStringFromQuery(navigator, paramName), out Guid outGuid))
			{
				return outGuid;
			}
			return defaultValue;
		}

		public static bool? GetBooleanFromQuery(NavigationManager navigator, string paramName, bool? defaultValue = null)
		{
			if (Boolean.TryParse(NavigatorExt.GetStringFromQuery(navigator, paramName), out bool outBool))
			{
				return outBool;
			}
			return defaultValue;
		}
		public static DateTime? GetDateFromQuery(NavigationManager navigator, string paramName, DateTime? defaultValue = null)
		{
			var urlValue = NavigatorExt.GetStringFromQuery(navigator, paramName, null);

			if (urlValue == "null")
			{
				return null;
			}
			else if (!String.IsNullOrWhiteSpace(urlValue))
			{
				return DateTimeUtils.FromUrlString(urlValue);
			}
			return defaultValue;
		}

		public static DateOnly? GetDateOnlyFromQuery(NavigationManager navigator, string paramName, DateOnly? defaultValue = null)
		{
			var urlValue = NavigatorExt.GetStringFromQuery(navigator, paramName, null);

			if (urlValue == "null")
			{
				return null;
			}
			else if (!String.IsNullOrWhiteSpace(urlValue))
			{
				return DateOnlyUtils.FromUrlString(urlValue);
			}
			return defaultValue;
		}

		public static int? GetIntFromQuery(NavigationManager navigator, string paramName, int? defaultValue = null)
		{
			if (int.TryParse(NavigatorExt.GetStringFromQuery(navigator, paramName), out int outInt))
			{
				return outInt;
			}
			return defaultValue;
		}

		public static TEnum? GetEnumFromQuery<TEnum>(NavigationManager navigator, string paramName, TEnum? defaultValue = null) where TEnum : struct
		{
			var stringValue = NavigatorExt.GetStringFromQuery(navigator, paramName);

			if (Enum.TryParse<TEnum>(stringValue, out TEnum outInt))
			{
				return outInt;
			}
			return defaultValue;
		}

		public static List<int> GetListIntFromQuery(NavigationManager navigator, string paramName, List<int> defaultValue = null)
		{

			var stringValues = NavigatorExt.GetStringListFromQuery(navigator, paramName, null);
			if (stringValues is null)
				return defaultValue;

			var result = new List<int>();
			foreach (var stringValue in stringValues)
			{
				if (int.TryParse(stringValue, out int outInt))
				{
					result.Add(outInt);
				}
			}
			if (result.Count == 0)
				return defaultValue;

			return result;
		}

		public static List<TEnum> GetListEnumFromQuery<TEnum>(NavigationManager navigator, string paramName, List<TEnum> defaultValue = null) where TEnum : struct
		{

			var stringValues = NavigatorExt.GetStringListFromQuery(navigator, paramName, null);
			if (stringValues is null)
				return defaultValue;

			var result = new List<TEnum>();
			foreach (var stringValue in stringValues)
			{
				if (Enum.TryParse<TEnum>(stringValue, out TEnum outInt))
				{
					result.Add(outInt);
				}
			}
			if (result.Count == 0)
				return defaultValue;

			return result;
		}

		public static string ProcessQueryValueFromUrl(string queryValue)
		{
			if (String.IsNullOrWhiteSpace(queryValue))
				return null;

			return UrlUndoReplaceSpecialCharacters(HttpUtility.UrlDecode(queryValue));

		}
		public static string ProcessQueryValueForUrl(string queryValue)
		{
			var processedValue = queryValue?.Trim();
			if (String.IsNullOrWhiteSpace(processedValue))
				return null;

			return HttpUtility.UrlEncode(UrlReplaceSpecialCharacters(processedValue));

		}
		public static string UrlReplaceSpecialCharacters(string inputValue)
		{
			return inputValue.Replace("/", "~");
		}
		public static string UrlUndoReplaceSpecialCharacters(string inputValue)
		{
			return inputValue.Replace("~", "/");
		}
		public static string GetLocalUrl(NavigationManager navigator)
		{
			var uri = new Uri(navigator.Uri);
			var localUrl = uri.LocalPath;
			if (!String.IsNullOrWhiteSpace(uri.Query))
				localUrl = localUrl + uri.Query;

			return localUrl;
		}

		public static void ReloadCurrentUrl(NavigationManager navigator)
		{
			navigator.NavigateTo(navigator.Uri, true);
		}



		private static bool IsList(object o)
		{
			if (o == null) return false;
			return o is IList &&
				   o.GetType().IsGenericType &&
				   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
		}

		private static bool IsDictionary(object o)
		{
			if (o == null) return false;
			return o is IDictionary &&
				   o.GetType().IsGenericType &&
				   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
		}

		public static void NotFound(NavigationManager navigator)
		{
			throw new Exception("Not found");
		}

	}

}
