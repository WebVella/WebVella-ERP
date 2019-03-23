using System;
using System.Collections.Generic;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Hooks
{
	internal static class RecordHookManager
	{
		public static bool ContainsAnyHooksForEntity(string entityName)
		{
			return
				HookManager.GetHookedInstances<IErpPreCreateRecordHook>(entityName).Count > 0 ||
				HookManager.GetHookedInstances<IErpPreUpdateRecordHook>(entityName).Count > 0 ||
				HookManager.GetHookedInstances<IErpPreDeleteRecordHook>(entityName).Count > 0 ||
				HookManager.GetHookedInstances<IErpPostCreateRecordHook>(entityName).Count > 0 ||
				HookManager.GetHookedInstances<IErpPostUpdateRecordHook>(entityName).Count > 0 ||
				HookManager.GetHookedInstances<IErpPostDeleteRecordHook>(entityName).Count > 0;
		}

		public static bool ContainsAnyHooksForRelation(string relationName)
		{
			return
				HookManager.GetHookedInstances<IErpPreCreateManyToManyRelationHook>(relationName).Count > 0 ||
				HookManager.GetHookedInstances<IErpPostCreateManyToManyRelationHook>(relationName).Count > 0 ||
				HookManager.GetHookedInstances<IErpPreDeleteManyToManyRelationHook>(relationName).Count > 0 ||
				HookManager.GetHookedInstances<IErpPostDeleteManyToManyRelationHook>(relationName).Count > 0;
		}

		internal static void ExecutePreCreateRecordHooks(string entityName, EntityRecord record, List<ErrorModel> errors)
		{
			if (string.IsNullOrWhiteSpace(entityName))
				throw new ArgumentException("entityName");

			if (errors == null)
				throw new ArgumentNullException("errors");

			List<IErpPreCreateRecordHook> hookedInstances = HookManager.GetHookedInstances<IErpPreCreateRecordHook>(entityName);
			foreach (var inst in hookedInstances)
				inst.OnPreCreateRecord(entityName, record, errors);
		}

		internal static void ExecutePostCreateRecordHooks(string entityName, EntityRecord record)
		{
			if (string.IsNullOrWhiteSpace(entityName))
				throw new ArgumentException("entityName");

			List<IErpPostCreateRecordHook> hookedInstances = HookManager.GetHookedInstances<IErpPostCreateRecordHook>(entityName);
			foreach (var inst in hookedInstances)
				inst.OnPostCreateRecord(entityName, record);
		}

		internal static void ExecutePreUpdateRecordHooks(string entityName, EntityRecord record, List<ErrorModel> errors)
		{
			if (string.IsNullOrWhiteSpace(entityName))
				throw new ArgumentException("entityName");

			if (errors == null)
				throw new ArgumentNullException("errors");

			List<IErpPreUpdateRecordHook> hookedInstances = HookManager.GetHookedInstances<IErpPreUpdateRecordHook>(entityName);
			foreach (var inst in hookedInstances)
				inst.OnPreUpdateRecord(entityName, record, errors);
		}

		internal static void ExecutePostUpdateRecordHooks(string entityName, EntityRecord record)
		{
			if (string.IsNullOrWhiteSpace(entityName))
				throw new ArgumentException("entityName");

			List<IErpPostUpdateRecordHook> hookedInstances = HookManager.GetHookedInstances<IErpPostUpdateRecordHook>(entityName);
			foreach (var inst in hookedInstances)
				inst.OnPostUpdateRecord(entityName, record);
		}

		internal static void ExecutePreDeleteRecordHooks(string entityName, EntityRecord record, List<ErrorModel> errors)
		{
			if (string.IsNullOrWhiteSpace(entityName))
				throw new ArgumentException("entityName");

			if (errors == null)
				throw new ArgumentNullException("errors");

			List<IErpPreDeleteRecordHook> hookedInstances = HookManager.GetHookedInstances<IErpPreDeleteRecordHook>(entityName);
			foreach (var inst in hookedInstances)
				inst.OnPreDeleteRecord(entityName, record, errors);
		}

		internal static void ExecutePostDeleteRecordHooks(string entityName, EntityRecord record)
		{
			if (string.IsNullOrWhiteSpace(entityName))
				throw new ArgumentException("entityName");

			List<IErpPostDeleteRecordHook> hookedInstances = HookManager.GetHookedInstances<IErpPostDeleteRecordHook>(entityName);
			foreach (var inst in hookedInstances)
				inst.OnPostDeleteRecord(entityName, record);
		}

		internal static void ExecutePreCreateManyToManyRelationHook(string relationName, Guid originId, Guid targetId, List<ErrorModel> errors)
		{
			if (string.IsNullOrWhiteSpace(relationName))
				throw new ArgumentException("relationName");

			if (errors == null)
				throw new ArgumentNullException("errors");

			List<IErpPreCreateManyToManyRelationHook> hookedInstances = HookManager.GetHookedInstances<IErpPreCreateManyToManyRelationHook>(relationName);
			foreach (var inst in hookedInstances)
				inst.OnPreCreate(relationName, originId, targetId, errors);
		}

		internal static void ExecutePostCreateManyToManyRelationHook(string relationName, Guid originId, Guid targetId )
		{
			if (string.IsNullOrWhiteSpace(relationName))
				throw new ArgumentException("relationName");

			List<IErpPostCreateManyToManyRelationHook> hookedInstances = HookManager.GetHookedInstances<IErpPostCreateManyToManyRelationHook>(relationName);
			foreach (var inst in hookedInstances)
				inst.OnPostCreate(relationName, originId, targetId);
		}

		internal static void ExecutePreDeleteManyToManyRelationHook(string relationName, Guid? originId, Guid? targetId, List<ErrorModel> errors)
		{
			if (string.IsNullOrWhiteSpace(relationName))
				throw new ArgumentException("relationName");

			if (errors == null)
				throw new ArgumentNullException("errors");

			List<IErpPreDeleteManyToManyRelationHook> hookedInstances = HookManager.GetHookedInstances<IErpPreDeleteManyToManyRelationHook>(relationName);
			foreach (var inst in hookedInstances)
				inst.OnPreDelete(relationName, originId, targetId, errors);
		}

		internal static void ExecutePostDeleteManyToManyRelationHook(string relationName, Guid? originId, Guid? targetId)
		{
			if (string.IsNullOrWhiteSpace(relationName))
				throw new ArgumentException("relationName");

			List<IErpPostDeleteManyToManyRelationHook> hookedInstances = HookManager.GetHookedInstances<IErpPostDeleteManyToManyRelationHook>(relationName);
			foreach (var inst in hookedInstances)
				inst.OnPostDelete(relationName, originId, targetId);
		}
	}
}
