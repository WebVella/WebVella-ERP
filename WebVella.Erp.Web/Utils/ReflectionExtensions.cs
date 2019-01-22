using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Text;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Web.Utils
{
	public static class ReflectionExtensions
	{
		public static Object GetPropValue(this Object obj, String propName)
		{
			if (obj == null) {
				return null;
			}
			string[] nameParts = propName.Split('.');
			if (nameParts.Length == 1)
			{
				var objType = obj.GetType();
				if (objType == null) return null;
				var property = objType.GetProperty(propName);
				if (property == null) return null;

				return property.GetValue(obj, null);
			}

			foreach (String part in nameParts)
			{
				if (obj == null) { return null; }

				Type type = obj.GetType();
				PropertyInfo info = null;
				if (obj is ViewDataDictionary)
				{
					var propertyValues = (ViewDataDictionary)obj;
					info = obj.GetType().GetProperty(part, typeof(PageModel));
				}
				else
				{
					info = type.GetProperty(part);
				}
				if (info == null) { return null; }
				obj = info.GetValue(obj, null);
			}
			return obj;
		}

		public static ModelNode GetPropertyTree(this Object obj, String propName, ModelNode result)
		{
			if (obj == null)
			{
				return new ModelNode()
				{
					PageDataSourceName = propName,
					DataType = "null"
				};
			}
			Type objType = obj.GetType();
			PropertyInfo[] properties = objType.GetProperties();
			var modelNode = new ModelNode()
			{
				PageDataSourceName = propName,
				DataType = objType.Name
			};
			
			foreach (var property in properties)
			{
				if (property.GetIndexParameters().Length == 0)
				{
					var node = property.GetValue(obj).GetPropertyTree(property.Name, result);
					if (node == null)
					{
						modelNode.Nodes.Add(new ModelNode()
						{
							PageDataSourceName = propName,
							DataType = "null"
						});
					}
					else
					{
						modelNode.Nodes.Add(node);

					}
				}
				else
				{
					modelNode.Nodes.Add(new ModelNode()
					{
						PageDataSourceName = property.Name,
						DataType = "Indexed"
					});
				}
			}
			return modelNode;
		}
	}
}
