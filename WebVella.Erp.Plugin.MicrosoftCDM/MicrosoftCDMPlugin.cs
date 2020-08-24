using System;
using Newtonsoft.Json;
using WebVella.Erp;
using WebVella.Erp.Api;

namespace WebVella.Erp.Plugin.MicrosoftCDM
{
	public partial class MicrosoftCDMPlugin : ErpPlugin
	{

		[JsonProperty(PropertyName = "name")]
		public override string Name { get; protected set; } = "MicrosoftCDMPlugin";

		public override void Initialize(IServiceProvider serviceProvider)
		{
			using (var ctx = SecurityContext.OpenSystemScope())
			{
				ProcessPatches();
			}
		}

	}
}
