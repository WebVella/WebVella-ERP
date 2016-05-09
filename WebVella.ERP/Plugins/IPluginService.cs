using Microsoft.AspNet.Hosting;
using System.Collections.Generic;

namespace WebVella.ERP.Plugins
{
    public interface IPluginService
	{
		List<Plugin> Plugins { get; }
		void Initialize(IHostingEnvironment hostingEnvironment);
	}
}