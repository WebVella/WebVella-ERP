using Newtonsoft.Json;
using System;
using WebVella.Erp.Api;
using WebVella.Erp.Plugins.Mail.Api;

namespace WebVella.Erp.Plugins.Mail
{
	public partial class MailPlugin : ErpPlugin
	{
		[JsonProperty(PropertyName = "name")]
		public override string Name { get; protected set; } = "mail";

		public override void Initialize(IServiceProvider serviceProvider)
		{
			using (var ctx = SecurityContext.OpenSystemScope())
			{
				ProcessPatches();
			}

			//ServiceManager sm = new ServiceManager();
			//var smtpService = sm.GetSmtpService();
			//smtpService.SendEmail("rumen webvella", "rumen@webvella.com", "testing smtp service", "text body", "<html><body><h1>html body</h1></body></html>");
		}
	}
}
