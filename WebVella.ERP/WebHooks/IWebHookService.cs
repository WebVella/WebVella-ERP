using WebVella.ERP.Plugins;

namespace WebVella.ERP.WebHooks
{
    public interface IWebHookService
	{
		void Initialize(IPluginService pluginService);
		dynamic ProcessFilters(string entityName, string filterName, dynamic args);
		void  ProcessActions(string entityName, string filterName, dynamic args);
	}
}