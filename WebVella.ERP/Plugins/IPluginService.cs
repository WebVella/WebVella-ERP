using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc.Infrastructure;
using System.Collections.Generic;
using WebVella.ERP.Plugins;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Plugins
{
    public interface IPluginService
	{
		List<Plugin> Plugins { get; }

		void Initialize(IHostingEnvironment hostingEnvironment);
    }
}