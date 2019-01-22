//namespace WebVella.Erp.Hooks
//{
//	internal class HookInfo
//	{
//		public string EntityName { get; set; }
//		public int Priority { get; set; }
//		public ErpBaseHook HookInstance { get; set; }
//	}

//	internal class RecordHookInfo : HookInfo
//	{
//		public bool DeclarePreCreate { get; set; }
//		public bool DeclarePostCreate { get; set; }
//		public bool DeclarePreUpdate { get; set; }
//		public bool DeclarePostUpdate { get; set; }
//		public bool DeclarePreDelete { get; set; }
//		public bool DeclarePostDelete { get; set; }
//	}
//}

using System;

namespace WebVella.Erp.Hooks
{
	public class HookInfo
	{
		public HookAttachmentAttribute AttachAttribute { get; set; }

		public HookAttribute HookAttribute { get; set; }

		public Type Type { get; set; }

		public object Instance { get; set; }

	}
}