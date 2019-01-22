using System;

namespace WebVella.Erp.Hooks
{
	[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
	public class HookAttribute : Attribute
    {
		public string Description { get; private set; } = string.Empty;

		public HookAttribute(string description = "")
		{
			Description = description;
		}
	}
}
