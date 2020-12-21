using System;
using System.Collections.Generic;
using WebVella.Erp.Api;
using WebVella.Erp.Database;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Plugins.SDK
{
	public partial class SdkPlugin : ErpPlugin
	{
		private static void Patch20200610(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{
		#region << ***Update app*** App name: sdk >>
		{
			var id = new Guid("56a8548a-19d0-497f-8e5b-242abfdc4082");
			var name = "sdk";
			var label = "Software Development Kit";
			var description = "SDK & Development Tools";
			var iconClass = "fa fa-cogs";
			var author = "<ul class='nav'><li class='nav-item'><a class='nav-link' href='/sdk/objects/page/l'>pages</a></li><li class='nav-item'><a class='nav-link'href='/sdk/objects/entity/l'>entities</a></li><li class='nav-item'><a class='nav-link'href='/sdk/objects/application/l/list'>apps</a></li><li class='nav-item'><a class='nav-link'href='/sdk/access/user/l/list'>users</a></li><li class='nav-item'><a class='nav-link'href='/sdk/server/log/l/list'>logs</a></li><li class='nav-item'><a class='nav-link'href='/sdk/server/job/l/list'>jobs</a></li></ul>";
			var color = "#dc3545";
			var weight = 1000;
			var access = new List<Guid>();
			access.Add( new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda") );

			new WebVella.Erp.Web.Services.AppService().UpdateApplication(id,name,label,description,iconClass,author,color,weight,access,WebVella.Erp.Database.DbContext.Current.Transaction);
		}
		#endregion

		}
	}
}
