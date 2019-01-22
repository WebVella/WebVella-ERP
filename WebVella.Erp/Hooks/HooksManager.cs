//using System;
//using System.Collections.Generic;
//using System.Linq;
//using WebVella.Erp.Api.Models;

//namespace WebVella.Erp.Hooks
//{
//	internal static class HooksManager
//	{
//		private static Dictionary<string, List<HookInfo>> recordHooks = new Dictionary<string, List<HookInfo>>();

//		public static void RegisterHooks(ErpService service)
//		{
//			foreach (ErpPlugin plugin in service.Plugins)
//			{
//				foreach (var hookType in plugin.GetHookTypes())
//				{
//					var attributes = hookType.GetCustomAttributes(typeof(ErpRecordHookAttribute), true);
//					if (attributes.Length == 1)
//					{
//						if (!hookType.IsSubclassOf(typeof(ErpRecordHook)))
//							throw new Exception($"'{hookType.FullName}' does not inherit ErpRecordHook.");

//						var attribute = attributes[0] as ErpRecordHookAttribute;
//						RecordHookInfo info = new RecordHookInfo();
//						info.EntityName = attribute.EntityName;
//						info.Priority = attribute.Priority;
//						info.HookInstance = (ErpRecordHook)Activator.CreateInstance(hookType);
//						info.HookInstance.EntityName = attribute.EntityName;

//						info.DeclarePreCreate = hookType.GetMethod("PreCreate").DeclaringType == hookType;
//						info.DeclarePostCreate = hookType.GetMethod("PostCreate").DeclaringType == hookType;
//						info.DeclarePreUpdate = hookType.GetMethod("PreUpdate").DeclaringType == hookType;
//						info.DeclarePostUpdate = hookType.GetMethod("PostUpdate").DeclaringType == hookType;
//						info.DeclarePreDelete = hookType.GetMethod("PreDelete").DeclaringType == hookType;
//						info.DeclarePostDelete = hookType.GetMethod("PostDelete").DeclaringType == hookType;

//						if (!recordHooks.ContainsKey(info.EntityName.ToLowerInvariant()))
//							recordHooks.Add(info.EntityName.ToLowerInvariant(), new List<HookInfo>());

//						recordHooks[info.EntityName.ToLowerInvariant()].Add(info);
//						recordHooks[info.EntityName.ToLowerInvariant()] =
//							recordHooks[info.EntityName.ToLowerInvariant()].OrderByDescending(x => x.Priority).ToList();
//						continue;
//					}
//				}
//			}
//		}

//		internal static bool ContainsAnyHooksForEntity(string entityName)
//		{
//			if (string.IsNullOrWhiteSpace(entityName))
//				throw new ArgumentException("entityName");

//			return recordHooks.ContainsKey(entityName.ToLowerInvariant());
//		}

//		internal static void ExecutePreCreateRecordHooks(string entityName, EntityRecord record, List<ErrorModel> errors)
//		{
//			if (string.IsNullOrWhiteSpace(entityName))
//				throw new ArgumentException("entityName");

//			if (errors == null)
//				throw new ArgumentNullException("errors");

//			List<HookInfo> hookInfos;
//			if (recordHooks.TryGetValue(entityName.ToLowerInvariant(), out hookInfos))
//			{
//				if (hookInfos != null && hookInfos.Count > 0)
//				{
//					foreach (var info in hookInfos)
//						((ErpRecordHook)info.HookInstance).PreCreate(record, errors);
//				}
//			}
//		}

//		internal static void ExecutePostCreateRecordHooks(string entityName, EntityRecord record)
//		{
//			if (string.IsNullOrWhiteSpace(entityName))
//				throw new ArgumentException("entityName");

//			List<HookInfo> hookInfos;
//			if (recordHooks.TryGetValue(entityName.ToLowerInvariant(), out hookInfos))
//			{
//				if (hookInfos != null && hookInfos.Count > 0)
//				{
//					foreach (var info in hookInfos)
//						((ErpRecordHook)info.HookInstance).PostCreate(record);
//				}
//			}
//		}

//		internal static void ExecutePreUpdateRecordHooks(string entityName, EntityRecord record, List<ErrorModel> errors)
//		{
//			if (string.IsNullOrWhiteSpace(entityName))
//				throw new ArgumentException("entityName");

//			if (errors == null)
//				throw new ArgumentNullException("errors");

//			List<HookInfo> hookInfos;
//			if (recordHooks.TryGetValue(entityName.ToLowerInvariant(), out hookInfos))
//			{
//				if (hookInfos != null && hookInfos.Count > 0)
//				{
//					foreach (var info in hookInfos)
//						((ErpRecordHook)info.HookInstance).PreUpdate(record, errors);
//				}
//			}
//		}

//		internal static void ExecutePostUpdateRecordHooks(string entityName, EntityRecord record)
//		{
//			if (string.IsNullOrWhiteSpace(entityName))
//				throw new ArgumentException("entityName");

//			List<HookInfo> hookInfos;
//			if (recordHooks.TryGetValue(entityName.ToLowerInvariant(), out hookInfos))
//			{
//				if (hookInfos != null && hookInfos.Count > 0)
//				{
//					foreach (var info in hookInfos)
//						((ErpRecordHook)info.HookInstance).PostUpdate(record);
//				}
//			}
//		}

//		internal static void ExecutePreDeleteRecordHooks(string entityName, Guid recordId, List<ErrorModel> errors)
//		{
//			if (string.IsNullOrWhiteSpace(entityName))
//				throw new ArgumentException("entityName");

//			if (errors == null)
//				throw new ArgumentNullException("errors");

//			List<HookInfo> hookInfos;
//			if (recordHooks.TryGetValue(entityName.ToLowerInvariant(), out hookInfos))
//			{
//				if (hookInfos != null && hookInfos.Count > 0)
//				{
//					foreach (var info in hookInfos)
//						((ErpRecordHook)info.HookInstance).PreDelete(recordId, errors);
//				}
//			}
//		}

//		internal static void ExecutePostDeleteRecordHooks(string entityName, Guid recordId)
//		{
//			if (string.IsNullOrWhiteSpace(entityName))
//				throw new ArgumentException("entityName");

//			List<HookInfo> hookInfos;
//			if (recordHooks.TryGetValue(entityName.ToLowerInvariant(), out hookInfos))
//			{
//				if (hookInfos != null && hookInfos.Count > 0)
//				{
//					foreach (var info in hookInfos)
//						((ErpRecordHook)info.HookInstance).PostDelete(recordId);
//				}
//			}
//		}
//	}
//}
