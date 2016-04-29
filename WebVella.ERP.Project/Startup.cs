using System.Diagnostics;
using WebVella.ERP.Plugins;

namespace WebVella.ERP.Project
{
	[PluginStartup]
    public class Startup
    {
		public void Start()
		{
			Debug.WriteLine("WebVella Sample Plugin start called");
		}
    }
}
