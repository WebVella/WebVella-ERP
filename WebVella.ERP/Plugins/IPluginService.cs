using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;

namespace WebVella.ERP.Plugins
{
    public interface IPluginService
	{
		List<Plugin> Plugins { get; }
		void Initialize(IServiceProvider serviceProvider);
	}
}