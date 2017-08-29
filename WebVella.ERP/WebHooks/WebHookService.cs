using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using WebVella.ERP.Plugins;
using WebVella.ERP.Utilities.Dynamic;

namespace WebVella.ERP.WebHooks
{
	public class WebHookService : IWebHookService
	{
		private static Dictionary<string, Dictionary<string, List<WebHookInfo>>> hooks
			 = new Dictionary<string, Dictionary<string, List<WebHookInfo>>>();

		public void Initialize(IPluginService pluginService)
		{
			foreach (var plugin in pluginService.Plugins)
			{
				foreach (var assembly in plugin.Assemblies)
				{
					if (plugin.Assemblies.Any(x => x.FullName == assembly.FullName))
					{
						foreach (Type type in assembly.GetTypes())
						{
							try
							{
								var methods = from method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
											  where method.IsDefined(typeof(WebHookAttribute))
											  select method;

								foreach (MethodInfo method in methods)
								{
									var arrtibutes = Attribute.GetCustomAttributes(method, false).Where(a => a is WebHookAttribute);
									foreach (WebHookAttribute attr in arrtibutes)
									{
										WebHookInfo whi = new WebHookInfo();
										whi.Instance = new DynamicObjectCreater(type).CreateInstance();
										whi.Method = method;
										whi.EntityName = attr.EntityName.ToLowerInvariant();
										whi.Name = attr.Name.ToLowerInvariant();
										whi.Priority = attr.Priority;

										Dictionary<string, List<WebHookInfo>> entityHooks;
										if (hooks.TryGetValue(whi.EntityName, out entityHooks))
										{
											List<WebHookInfo> hooksList;
											if (entityHooks.TryGetValue(whi.Name, out hooksList))
												hooksList.Add(whi);
											else
											{
												hooksList = new List<WebHookInfo>();
												hooksList.Add(whi);
												entityHooks.Add(whi.Name, hooksList);
											}
										}
										else
										{
											entityHooks = new Dictionary<string, List<WebHookInfo>>();
											List<WebHookInfo> hooksList = new List<WebHookInfo>();
											hooksList.Add(whi);
											entityHooks.Add(whi.Name, hooksList);
											hooks.Add(whi.EntityName, entityHooks);
										}
									}
								}
							}
							catch (Exception ex)
							{
								throw new Exception("An exception is thrown while register web hooks for plugin : '" +
									assembly.FullName + ";" + type.Namespace + "." + type.Name + "'", ex);
							}
						}
					}
				}
			}
		}

		private struct WebHookInfo
		{
			public string Name { get; set; }
			public string EntityName { get; set; }
			public int Priority { get; set; }
			public Object Instance { get; set; }
			public MethodInfo Method { get; set; }
		}

		public dynamic ProcessFilters(string filterName, string entityName, dynamic args)
		{
			if (string.IsNullOrWhiteSpace(entityName))
				throw new ArgumentException("entityName");

			if (string.IsNullOrWhiteSpace(filterName))
				throw new ArgumentException("filterName");

			Dictionary<string, List<WebHookInfo>> entityHooks;
			if (hooks.TryGetValue(entityName.ToLowerInvariant(), out entityHooks))
			{
				List<WebHookInfo> hooksList;
				if (entityHooks.TryGetValue(filterName.ToLowerInvariant(), out hooksList))
				{
					if (hooksList != null && hooksList.Count > 0)
					{
						dynamic result = args;
						foreach (var whi in hooksList.OrderByDescending(x => x.Priority))
							try
							{
								result = whi.Method.Invoke(whi.Instance, new object[] { result });
							}
							catch (TargetInvocationException ex)
							{
								throw ex.InnerException;
							}
							catch (Exception ex)
							{
								throw ex;
							}
						return result;
					}
				}
			}

			return args;
		}

		public void ProcessActions(string filterName, string entityName, dynamic args)
		{
			if (string.IsNullOrWhiteSpace(entityName))
				throw new ArgumentException("entityName");

			if (string.IsNullOrWhiteSpace(filterName))
				throw new ArgumentException("filterName");

			Dictionary<string, List<WebHookInfo>> entityHooks;
			if (hooks.TryGetValue(entityName.ToLowerInvariant(), out entityHooks))
			{
				List<WebHookInfo> hooksList;
				if (entityHooks.TryGetValue(filterName.ToLowerInvariant(), out hooksList))
				{
					if (hooksList != null && hooksList.Count > 0)
					{
						foreach (var whi in hooksList.OrderByDescending(x => x.Priority))
							try
							{
								whi.Method.Invoke(whi.Instance, new object[] { args });
							}
							catch (TargetInvocationException ex)
							{
								throw ex.InnerException;
							}
							catch (Exception ex)
							{
								throw ex;
							}
					}
				}
			}
		}
	}
}
