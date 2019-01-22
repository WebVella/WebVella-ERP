using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebVella.Erp.Api;

namespace WebVella.Erp.Plugins.Next
{
	public partial class NextPlugin : ErpPlugin
	{
		[JsonProperty(PropertyName = "name")]
		public override string Name { get; protected set; } = "next";

		public override void Initialize(IServiceProvider serviceProvider)
		{
			using (var ctx = SecurityContext.OpenSystemScope())
			{
				ProcessPatches();
			}
		}

		public override IEnumerable<Type> GetJobTypes()
		{
			List<Type> jobTypes = new List<Type>();
			return jobTypes;
		}
	}
}
