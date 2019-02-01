using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebVella.Erp.Api;
using WebVella.Erp.Jobs;


namespace WebVella.Erp.Plugins.Crm
{
	public partial class CrmPlugin : ErpPlugin
	{
		[JsonProperty(PropertyName = "name")]
		public override string Name { get; protected set; } = "crm";

		public override void Initialize(IServiceProvider serviceProvider)
		{
			using (var ctx = SecurityContext.OpenSystemScope())
			{
				ProcessPatches();
			}
		}
	}
}
