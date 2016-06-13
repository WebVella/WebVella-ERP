using System;

namespace WebVella.ERP.WebHooks
{
	public class WebHookAttribute : Attribute
    {
		public string Name { get; set; }
		public string EntityName { get; set; }
		public int Priority { get; set; }

		public WebHookAttribute(string name, string entityName, int priority = 10)
		{
			Name = name;
			EntityName = entityName;
			Priority = priority;
		}
    }
}
