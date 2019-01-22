using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Web.Services
{
	public class WebSettingsService : BaseService
	{
		WebSettings coresettings = new WebSettings();

		public WebSettings Get()
		{
			return coresettings; 
		}
	}
}
