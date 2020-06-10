using System;
using System.Collections.Generic;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Plugins.Mail
{
	public partial class MailPlugin : ErpPlugin
	{
		private static void Patch20200610(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{
			#region << ***Update app*** App name: mail >>
			{
				var id = new Guid("9d3b5497-e136-43b7-ad87-857e615a54c9");
				var name = "mail";
				var label = "Mail";
				var description = "Provides services for sending emails.";
				var iconClass = "far fa-envelope";
				var author = "WebVella";
				var color = "#8bc34a";
				var weight = 100;
				var access = new List<Guid>();
				access.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));

				new WebVella.Erp.Web.Services.AppService().UpdateApplication(id, name, label, description, iconClass, author, color, weight, access, WebVella.Erp.Database.DbContext.Current.Transaction);
			}
			#endregion

		}
	}
}
