using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Plugins.Project
{
	public partial class ProjectPlugin : ErpPlugin
	{
		private static void Patch20211013(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{
#region << ***Update page body node*** Page: track-time ID: 9946104a-a6ec-4a0b-b996-7bc630c16287 >>
{
	var id = new Guid("9946104a-a6ec-4a0b-b996-7bc630c16287");
	Guid? parentId = new Guid("6694f852-c49e-4dd2-a4dc-dd2f6faaf4b4");
	Guid? nodeId = null;
	Guid pageId = new Guid("e9c8f7ef-4714-40e9-90cd-3814d89603b1");
	var componentName = "WebVella.Erp.Web.Components.PcButton";
	var containerId = "footer";
	var options = @"{
  ""type"": ""0"",
  ""text"": ""Cancel"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": """",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""icon_right"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""is_visible"": """",
  ""onclick"": ""$('#wv-6694f852-c49e-4dd2-a4dc-dd2f6faaf4b4').modal(\""hide\"")"",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
	var weight = 2;

	new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id,parentId,pageId,nodeId,weight,componentName,containerId,options,WebVella.Erp.Database.DbContext.Current.Transaction);
}
#endregion



		}
	}
}
