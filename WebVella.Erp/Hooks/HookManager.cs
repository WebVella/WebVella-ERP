using System;
using System.Collections.Generic;
using System.Linq;

namespace WebVella.Erp.Hooks
{
	public static class HookManager
	{
		private static Dictionary<Type, List<HookInfo>> hooksDict = new Dictionary<Type, List<HookInfo>>();

		public static void RegisterHooks(ErpService service)
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies()
							.Where(a => !(a.FullName.ToLowerInvariant().StartsWith("microsoft.")
								|| a.FullName.ToLowerInvariant().StartsWith("system.")));
			foreach (var assembly in assemblies)
			{
				foreach (Type type in assembly.GetTypes())
				{
					var attachAttributes = type.GetCustomAttributes(typeof(HookAttachmentAttribute), true);
					HookAttachmentAttribute attachAttribute = null;
					if (attachAttributes.Length == 1 && type.IsClass)
						attachAttribute = (HookAttachmentAttribute)attachAttributes[0];

					if (attachAttribute != null)
					{
						foreach (var typeInterface in type.GetInterfaces())
						{
							var	hookAttributes = typeInterface.GetCustomAttributes(typeof(HookAttribute), true);
							HookAttribute hookAttribute = null;
							if (hookAttributes.Length == 1 && type.IsClass)
								hookAttribute = (HookAttribute)hookAttributes[0];

							if (hookAttribute == null)
								continue;

							if (!hooksDict.ContainsKey(typeInterface))
								hooksDict[typeInterface] = new List<HookInfo>();

							HookInfo hookInfo = new HookInfo();
							hookInfo.Type = type;
							hookInfo.AttachAttribute = attachAttribute;
							hookInfo.HookAttribute = hookAttribute;
							hookInfo.Instance = Activator.CreateInstance(type);
							hooksDict[typeInterface].Add(hookInfo);
						}
					}
				}
			}
		}

		public static List<T> GetHookedInstances<T>(string key = "")
		{
			var result = new List<T>();
			if (hooksDict.ContainsKey(typeof(T)))
			{
				List<HookInfo> hookObjs;
				if (hooksDict.TryGetValue(typeof(T), out hookObjs))
				{
					foreach (var obj in hookObjs.Where(x => x.AttachAttribute.Key == key).OrderByDescending(x => x.AttachAttribute.Priority))
						result.Add((T)obj.Instance);
				}
			}

			return result;
		}
	}
}
