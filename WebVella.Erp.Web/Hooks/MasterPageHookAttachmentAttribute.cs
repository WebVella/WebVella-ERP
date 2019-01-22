using System;

namespace WebVella.Erp.Web.Hooks
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class RenderHookAttachmentAttribute : Attribute
	{
		public string PlaceholderName { get; private set; } = string.Empty;

		public int Priority { get; private set; } = 10;

		public RenderHookAttachmentAttribute(string placeholderName, int priority = 10)
		{
			if (string.IsNullOrWhiteSpace(placeholderName))
				throw new Exception("MasterPage hook attachment is missing the placeholder name.");

			PlaceholderName = placeholderName;
			Priority = priority;
		}
	}
}
