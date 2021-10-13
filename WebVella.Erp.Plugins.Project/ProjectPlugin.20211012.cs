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
		private static void Patch20211012(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{

#region << ***Update entity*** Entity name: role >>
{
	var updateObject = new InputEntity();
	updateObject.Id = new Guid("c4541fee-fbb6-4661-929e-1724adec285a");
	updateObject.Name = "role";
	updateObject.Label = "Role";
	updateObject.LabelPlural = "Roles";
	updateObject.System = true;
	updateObject.IconName = "fa fa-key";
	updateObject.Color = "#f44336";
		updateObject.RecordScreenIdField = null;
	updateObject.RecordPermissions = new RecordPermissions();
	updateObject.RecordPermissions.CanRead = new List<Guid>();
	updateObject.RecordPermissions.CanCreate = new List<Guid>();
	updateObject.RecordPermissions.CanUpdate = new List<Guid>();
	updateObject.RecordPermissions.CanDelete = new List<Guid>();
	updateObject.RecordPermissions.CanRead.Add(new Guid("987148b1-afa8-4b33-8616-55861e5fd065"));
	updateObject.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
	updateObject.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
	updateObject.RecordPermissions.CanCreate.Add(new Guid("987148b1-afa8-4b33-8616-55861e5fd065"));
	updateObject.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
	updateObject.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
	updateObject.RecordPermissions.CanDelete.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
	var updateEntityResult = entMan.UpdateEntity(updateObject);
	if (!updateEntityResult.Success)
	{
		throw new Exception("System error 10060. Entity update with name : role. Message:" + updateEntityResult.Message);
	}
}
#endregion

#region << ***Update entity*** Entity name: user >>
{
	var updateObject = new InputEntity();
	updateObject.Id = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
	updateObject.Name = "user";
	updateObject.Label = "User";
	updateObject.LabelPlural = "Users";
	updateObject.System = true;
	updateObject.IconName = "fa fa-user";
	updateObject.Color = "#f44336";
		updateObject.RecordScreenIdField = null;
	updateObject.RecordPermissions = new RecordPermissions();
	updateObject.RecordPermissions.CanRead = new List<Guid>();
	updateObject.RecordPermissions.CanCreate = new List<Guid>();
	updateObject.RecordPermissions.CanUpdate = new List<Guid>();
	updateObject.RecordPermissions.CanDelete = new List<Guid>();
	updateObject.RecordPermissions.CanRead.Add(new Guid("987148b1-afa8-4b33-8616-55861e5fd065"));
	updateObject.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
	updateObject.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
	updateObject.RecordPermissions.CanCreate.Add(new Guid("987148b1-afa8-4b33-8616-55861e5fd065"));
	updateObject.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
	updateObject.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
	updateObject.RecordPermissions.CanDelete.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
	var updateEntityResult = entMan.UpdateEntity(updateObject);
	if (!updateEntityResult.Success)
	{
		throw new Exception("System error 10060. Entity update with name : user. Message:" + updateEntityResult.Message);
	}
}
#endregion

#region << ***Update entity*** Entity name: user_file >>
{
	var updateObject = new InputEntity();
	updateObject.Id = new Guid("5c666c54-9e76-4327-ac7a-55851037810c");
	updateObject.Name = "user_file";
	updateObject.Label = "User File";
	updateObject.LabelPlural = "User Files";
	updateObject.System = true;
	updateObject.IconName = "fa fa-file";
	updateObject.Color = "#f44336";
		updateObject.RecordScreenIdField = null;
	updateObject.RecordPermissions = new RecordPermissions();
	updateObject.RecordPermissions.CanRead = new List<Guid>();
	updateObject.RecordPermissions.CanCreate = new List<Guid>();
	updateObject.RecordPermissions.CanUpdate = new List<Guid>();
	updateObject.RecordPermissions.CanDelete = new List<Guid>();
	updateObject.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
	updateObject.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
	updateObject.RecordPermissions.CanCreate.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
	updateObject.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
	updateObject.RecordPermissions.CanUpdate.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
	updateObject.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
	updateObject.RecordPermissions.CanDelete.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
	updateObject.RecordPermissions.CanDelete.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
	var updateEntityResult = entMan.UpdateEntity(updateObject);
	if (!updateEntityResult.Success)
	{
		throw new Exception("System error 10060. Entity update with name : user_file. Message:" + updateEntityResult.Message);
	}
}
#endregion

#region << ***Create field***  Entity: account Field Name: logo >>
{
	InputImageField imageField = new InputImageField();
	imageField.Id = new Guid("ff2be918-4132-4eac-a7d7-576facc52355");
	imageField.Name = "logo";
	imageField.Label = "Logo";
	imageField.PlaceholderText = null;
	imageField.Description = null;
	imageField.HelpText = null;
	imageField.Required = false;
	imageField.Unique = false;
	imageField.Searchable = false;
	imageField.Auditable =  false;
	imageField.System = true;
	imageField.DefaultValue ="";
	imageField.EnableSecurity = false;
	imageField.Permissions = new FieldPermissions();
	imageField.Permissions.CanRead = new List<Guid>();
	imageField.Permissions.CanUpdate = new List<Guid>();
	//READ
	//UPDATE
	{
		var response = entMan.CreateField(new Guid("2e22b50f-e444-4b62-a171-076e51246939"), imageField, false);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: account Field: logo Message:" + response.Message);
	}
}
#endregion

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

#region << ***Update sitemap area*** Sitemap area name: objects >>
{
	var id = new Guid("d3237d8c-c074-46d7-82c2-1385cbfff35a");
	var appId = new Guid("56a8548a-19d0-497f-8e5b-242abfdc4082");
	var name = "objects";
	var label = "Objects";
	var description = @"Schema and Layout management";
	var iconClass = "fa fa-pencil-ruler";
	var color = "#2196F3";
	var weight = 1;
	var showGroupNames = false;
	var access = new List<Guid>();
	var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();
	var descriptionTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

	new WebVella.Erp.Web.Services.AppService().UpdateArea(id,appId,name,label,labelTranslations,description,descriptionTranslations,iconClass,color,weight,showGroupNames,access,WebVella.Erp.Database.DbContext.Current.Transaction);
}
#endregion

#region << ***Update sitemap area*** Sitemap area name: access >>
{
	var id = new Guid("c5c4cefc-1402-4a8b-9867-7f2a059b745d");
	var appId = new Guid("56a8548a-19d0-497f-8e5b-242abfdc4082");
	var name = "access";
	var label = "Access";
	var description = @"Manage users and roles";
	var iconClass = "fa fa-key";
	var color = "#673AB7";
	var weight = 2;
	var showGroupNames = false;
	var access = new List<Guid>();
	var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();
	var descriptionTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

	new WebVella.Erp.Web.Services.AppService().UpdateArea(id,appId,name,label,labelTranslations,description,descriptionTranslations,iconClass,color,weight,showGroupNames,access,WebVella.Erp.Database.DbContext.Current.Transaction);
}
#endregion

#region << ***Update sitemap area*** Sitemap area name: server >>
{
	var id = new Guid("fee72214-f1c4-4ed5-8bda-35698dc11528");
	var appId = new Guid("56a8548a-19d0-497f-8e5b-242abfdc4082");
	var name = "server";
	var label = "Server";
	var description = @"Background jobs and maintenance";
	var iconClass = "fa fa-database";
	var color = "#F44336";
	var weight = 3;
	var showGroupNames = false;
	var access = new List<Guid>();
	var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();
	var descriptionTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

	new WebVella.Erp.Web.Services.AppService().UpdateArea(id,appId,name,label,labelTranslations,description,descriptionTranslations,iconClass,color,weight,showGroupNames,access,WebVella.Erp.Database.DbContext.Current.Transaction);
}
#endregion

#region << ***Update sitemap node*** Sitemap node name: page >>
{
	var id = new Guid("5b132ac0-703e-4342-a13d-c7ff93d07a4f");
	Guid? parentId = null;
	var areaId = new Guid("d3237d8c-c074-46d7-82c2-1385cbfff35a");
	Guid? entityId = null;
	var name = "page";
	var label = "Pages";
	var url = "/sdk/objects/page/l";
	var iconClass = "fa fa-file";
	var weight = 1;
	var type = ((int)3);
	var access = new List<Guid>();
	var entityListPages = new List<Guid>();
	var entityCreatePages = new List<Guid>();
	var entityDetailsPages = new List<Guid>();
	var entityManagePages = new List<Guid>();
	var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

	new WebVella.Erp.Web.Services.AppService().UpdateAreaNode(id,areaId,name,label,labelTranslations,iconClass,url,type,entityId,weight,access,entityListPages,entityCreatePages,entityDetailsPages,entityManagePages,WebVella.Erp.Database.DbContext.Current.Transaction,parentId);
}
#endregion

#region << ***Update sitemap node*** Sitemap node name: data_source >>
{
	var id = new Guid("9b30bf96-67d9-4d20-bf07-e6ef1c44d553");
	Guid? parentId = null;
	var areaId = new Guid("d3237d8c-c074-46d7-82c2-1385cbfff35a");
	Guid? entityId = null;
	var name = "data_source";
	var label = "Data sources";
	var url = "/sdk/objects/data_source/l/list";
	var iconClass = "fa fa-cloud-download-alt";
	var weight = 2;
	var type = ((int)3);
	var access = new List<Guid>();
	var entityListPages = new List<Guid>();
	var entityCreatePages = new List<Guid>();
	var entityDetailsPages = new List<Guid>();
	var entityManagePages = new List<Guid>();
	var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

	new WebVella.Erp.Web.Services.AppService().UpdateAreaNode(id,areaId,name,label,labelTranslations,iconClass,url,type,entityId,weight,access,entityListPages,entityCreatePages,entityDetailsPages,entityManagePages,WebVella.Erp.Database.DbContext.Current.Transaction,parentId);
}
#endregion

#region << ***Update sitemap node*** Sitemap node name: application >>
{
	var id = new Guid("02d75ea5-8fc6-4f95-9933-0eed6b36ca49");
	Guid? parentId = null;
	var areaId = new Guid("d3237d8c-c074-46d7-82c2-1385cbfff35a");
	Guid? entityId = null;
	var name = "application";
	var label = "Applications";
	var url = "/sdk/objects/application/l/list";
	var iconClass = "fa fa-th";
	var weight = 3;
	var type = ((int)3);
	var access = new List<Guid>();
	var entityListPages = new List<Guid>();
	var entityCreatePages = new List<Guid>();
	var entityDetailsPages = new List<Guid>();
	var entityManagePages = new List<Guid>();
	var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

	new WebVella.Erp.Web.Services.AppService().UpdateAreaNode(id,areaId,name,label,labelTranslations,iconClass,url,type,entityId,weight,access,entityListPages,entityCreatePages,entityDetailsPages,entityManagePages,WebVella.Erp.Database.DbContext.Current.Transaction,parentId);
}
#endregion

#region << ***Update sitemap node*** Sitemap node name: entity >>
{
	var id = new Guid("dfa7ec55-b55b-404f-b251-889f1d81df29");
	Guid? parentId = null;
	var areaId = new Guid("d3237d8c-c074-46d7-82c2-1385cbfff35a");
	Guid? entityId = null;
	var name = "entity";
	var label = "Entities";
	var url = "/sdk/objects/entity/l";
	var iconClass = "fa fa-database";
	var weight = 4;
	var type = ((int)3);
	var access = new List<Guid>();
	var entityListPages = new List<Guid>();
	var entityCreatePages = new List<Guid>();
	var entityDetailsPages = new List<Guid>();
	var entityManagePages = new List<Guid>();
	var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

	new WebVella.Erp.Web.Services.AppService().UpdateAreaNode(id,areaId,name,label,labelTranslations,iconClass,url,type,entityId,weight,access,entityListPages,entityCreatePages,entityDetailsPages,entityManagePages,WebVella.Erp.Database.DbContext.Current.Transaction,parentId);
}
#endregion

#region << ***Update sitemap node*** Sitemap node name: user >>
{
	var id = new Guid("ff578868-817e-433d-988f-bb8d4e9baa0d");
	Guid? parentId = null;
	var areaId = new Guid("c5c4cefc-1402-4a8b-9867-7f2a059b745d");
	Guid? entityId = null;
	var name = "user";
	var label = "Users";
	var url = "/sdk/access/user/l/list";
	var iconClass = "fa fa-user";
	var weight = 1;
	var type = ((int)3);
	var access = new List<Guid>();
	var entityListPages = new List<Guid>();
	var entityCreatePages = new List<Guid>();
	var entityDetailsPages = new List<Guid>();
	var entityManagePages = new List<Guid>();
	var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

	new WebVella.Erp.Web.Services.AppService().UpdateAreaNode(id,areaId,name,label,labelTranslations,iconClass,url,type,entityId,weight,access,entityListPages,entityCreatePages,entityDetailsPages,entityManagePages,WebVella.Erp.Database.DbContext.Current.Transaction,parentId);
}
#endregion

#region << ***Update sitemap node*** Sitemap node name: job >>
{
	var id = new Guid("396ec481-3b2e-461c-b514-743fb3252003");
	Guid? parentId = null;
	var areaId = new Guid("fee72214-f1c4-4ed5-8bda-35698dc11528");
	Guid? entityId = null;
	var name = "job";
	var label = "Background jobs";
	var url = "/sdk/server/job/l/plan";
	var iconClass = "fa fa-cogs";
	var weight = 1;
	var type = ((int)3);
	var access = new List<Guid>();
	var entityListPages = new List<Guid>();
	var entityCreatePages = new List<Guid>();
	var entityDetailsPages = new List<Guid>();
	var entityManagePages = new List<Guid>();
	var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

	new WebVella.Erp.Web.Services.AppService().UpdateAreaNode(id,areaId,name,label,labelTranslations,iconClass,url,type,entityId,weight,access,entityListPages,entityCreatePages,entityDetailsPages,entityManagePages,WebVella.Erp.Database.DbContext.Current.Transaction,parentId);
}
#endregion

#region << ***Update sitemap node*** Sitemap node name: log >>
{
	var id = new Guid("78a29ac8-d2aa-4379-b990-08f7f164a895");
	Guid? parentId = null;
	var areaId = new Guid("fee72214-f1c4-4ed5-8bda-35698dc11528");
	Guid? entityId = null;
	var name = "log";
	var label = "Logs";
	var url = "/sdk/server/log/l/list";
	var iconClass = "fas fa-sticky-note";
	var weight = 2;
	var type = ((int)3);
	var access = new List<Guid>();
	var entityListPages = new List<Guid>();
	var entityCreatePages = new List<Guid>();
	var entityDetailsPages = new List<Guid>();
	var entityManagePages = new List<Guid>();
	var labelTranslations = new List<WebVella.Erp.Web.Models.TranslationResource>();

	new WebVella.Erp.Web.Services.AppService().UpdateAreaNode(id,areaId,name,label,labelTranslations,iconClass,url,type,entityId,weight,access,entityListPages,entityCreatePages,entityDetailsPages,entityManagePages,WebVella.Erp.Database.DbContext.Current.Transaction,parentId);
}
#endregion

#region << ***Update page body node*** Page: dashboard ID: 63daa5c0-ed7f-432e-bfbb-746b94207146 >>
{
	var id = new Guid("63daa5c0-ed7f-432e-bfbb-746b94207146");
	Guid? parentId = new Guid("a584a5ed-96a2-4a28-95e8-23266bc36926");
	Guid? nodeId = null;
	Guid pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
	var componentName = "WebVella.Erp.Web.Components.PcSection";
	var containerId = "column2";
	var options = @"{
  ""is_visible"": """",
  ""title"": ""My Overdue Tasks"",
  ""title_tag"": ""span"",
  ""is_card"": ""true"",
  ""is_collapsable"": ""false"",
  ""is_collapsed"": ""false"",
  ""class"": ""card-sm mb-3 "",
  ""body_class"": """",
  ""label_mode"": ""1"",
  ""field_mode"": ""1""
}";
	var weight = 1;

	new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id,parentId,pageId,nodeId,weight,componentName,containerId,options,WebVella.Erp.Database.DbContext.Current.Transaction);
}
#endregion

#region << ***Update page body node*** Page: dashboard ID: ae930e6f-38b5-4c48-a17f-63b0bdf7dab6 >>
{
	var id = new Guid("ae930e6f-38b5-4c48-a17f-63b0bdf7dab6");
	Guid? parentId = new Guid("a584a5ed-96a2-4a28-95e8-23266bc36926");
	Guid? nodeId = null;
	Guid pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
	var componentName = "WebVella.Erp.Web.Components.PcSection";
	var containerId = "column1";
	var options = @"{
  ""is_visible"": """",
  ""title"": ""All Users' Timesheet"",
  ""title_tag"": ""span"",
  ""is_card"": ""true"",
  ""is_collapsable"": ""false"",
  ""is_collapsed"": ""false"",
  ""class"": ""card-sm mb-3"",
  ""body_class"": ""pt-3 pb-3"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1""
}";
	var weight = 3;

	new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id,parentId,pageId,nodeId,weight,componentName,containerId,options,WebVella.Erp.Database.DbContext.Current.Transaction);
}
#endregion

#region << ***Update page body node*** Page: dashboard ID: 47303562-04a3-4935-b228-aaa61527f963 >>
{
	var id = new Guid("47303562-04a3-4935-b228-aaa61527f963");
	Guid? parentId = new Guid("151e265c-d3d3-4340-92fc-0cace2ca45f9");
	Guid? nodeId = null;
	Guid pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
	var componentName = "WebVella.Erp.Web.Components.PcSection";
	var containerId = "column1";
	var options = @"{
  ""is_visible"": """",
  ""title"": ""Tasks"",
  ""title_tag"": ""span"",
  ""is_card"": ""true"",
  ""is_collapsable"": ""false"",
  ""is_collapsed"": ""false"",
  ""class"": ""card-sm h-100"",
  ""body_class"": ""p-3 align-center-col"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1""
}";
	var weight = 1;

	new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id,parentId,pageId,nodeId,weight,componentName,containerId,options,WebVella.Erp.Database.DbContext.Current.Transaction);
}
#endregion

#region << ***Update page body node*** Page: dashboard ID: be907fa3-0971-45b5-9dcf-fabbb277fe54 >>
{
	var id = new Guid("be907fa3-0971-45b5-9dcf-fabbb277fe54");
	Guid? parentId = new Guid("151e265c-d3d3-4340-92fc-0cace2ca45f9");
	Guid? nodeId = null;
	Guid pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
	var componentName = "WebVella.Erp.Web.Components.PcSection";
	var containerId = "column2";
	var options = @"{
  ""is_visible"": """",
  ""title"": ""Priority"",
  ""title_tag"": ""span"",
  ""is_card"": ""true"",
  ""is_collapsable"": ""false"",
  ""is_collapsed"": ""false"",
  ""class"": ""card-sm h-100"",
  ""body_class"": ""p-3 align-center-col"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1""
}";
	var weight = 1;

	new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id,parentId,pageId,nodeId,weight,componentName,containerId,options,WebVella.Erp.Database.DbContext.Current.Transaction);
}
#endregion

#region << ***Update page body node*** Page: dashboard ID: e49cf2f9-82b0-4988-aa29-427e8d9501d9 >>
{
	var id = new Guid("e49cf2f9-82b0-4988-aa29-427e8d9501d9");
	Guid? parentId = new Guid("a584a5ed-96a2-4a28-95e8-23266bc36926");
	Guid? nodeId = null;
	Guid pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
	var componentName = "WebVella.Erp.Web.Components.PcSection";
	var containerId = "column1";
	var options = @"{
  ""is_visible"": """",
  ""title"": ""My Timesheet"",
  ""title_tag"": ""span"",
  ""is_card"": ""true"",
  ""is_collapsable"": ""false"",
  ""is_collapsed"": ""false"",
  ""class"": ""card-sm mb-3"",
  ""body_class"": ""pt-3 pb-3"",
  ""label_mode"": ""1"",
  ""field_mode"": ""1""
}";
	var weight = 2;

	new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id,parentId,pageId,nodeId,weight,componentName,containerId,options,WebVella.Erp.Database.DbContext.Current.Transaction);
}
#endregion

#region << ***Update page body node*** Page: dashboard ID: 8e533c53-0bf5-4082-ae06-f47f1bd9b3b5 >>
{
	var id = new Guid("8e533c53-0bf5-4082-ae06-f47f1bd9b3b5");
	Guid? parentId = new Guid("a584a5ed-96a2-4a28-95e8-23266bc36926");
	Guid? nodeId = null;
	Guid pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
	var componentName = "WebVella.Erp.Web.Components.PcSection";
	var containerId = "column2";
	var options = @"{
  ""is_visible"": """",
  ""title"": ""My 10 Upcoming Tasks "",
  ""title_tag"": ""span"",
  ""is_card"": ""true"",
  ""is_collapsable"": ""false"",
  ""is_collapsed"": ""false"",
  ""class"": ""card-sm mb-3"",
  ""body_class"": """",
  ""label_mode"": ""1"",
  ""field_mode"": ""1""
}";
	var weight = 3;

	new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id,parentId,pageId,nodeId,weight,componentName,containerId,options,WebVella.Erp.Database.DbContext.Current.Transaction);
}
#endregion

#region << ***Update page body node*** Page: dashboard ID: 6ef7bbd7-b96c-45d4-97e1-b8e43f489ed5 >>
{
	var id = new Guid("6ef7bbd7-b96c-45d4-97e1-b8e43f489ed5");
	Guid? parentId = new Guid("a584a5ed-96a2-4a28-95e8-23266bc36926");
	Guid? nodeId = null;
	Guid pageId = new Guid("33f2cd33-cf38-4247-9097-75f895d1ef7a");
	var componentName = "WebVella.Erp.Web.Components.PcSection";
	var containerId = "column2";
	var options = @"{
  ""is_visible"": """",
  ""title"": ""My Tasks Due Today"",
  ""title_tag"": ""span"",
  ""is_card"": ""true"",
  ""is_collapsable"": ""false"",
  ""is_collapsed"": ""false"",
  ""class"": ""card-sm mb-3"",
  ""body_class"": """",
  ""label_mode"": ""1"",
  ""field_mode"": ""1""
}";
	var weight = 2;

	new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id,parentId,pageId,nodeId,weight,componentName,containerId,options,WebVella.Erp.Database.DbContext.Current.Transaction);
}
#endregion

#region << ***Update page body node*** Page: details ID: 754bf941-df31-4b13-ba32-eb3c7a8c8922 >>
{
	var id = new Guid("754bf941-df31-4b13-ba32-eb3c7a8c8922");
	Guid? parentId = new Guid("e15e2d00-e704-4212-a7d2-ee125dd687a6");
	Guid? nodeId = null;
	Guid pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
	var componentName = "WebVella.Erp.Web.Components.PcFieldText";
	var containerId = "column1";
	var options = @"{
  ""is_visible"": """",
  ""label_mode"": ""0"",
  ""label_text"": ""Subject 123"",
  ""link"": """",
  ""mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.subject\"",\""default\"":\""\""}"",
  ""name"": ""subject"",
  ""class"": """",
  ""maxlength"": 0,
  ""placeholder"": """",
  ""connected_entity_id"": """",
  ""connected_record_id_ds"": """",
  ""access_override_ds"": """",
  ""required_override_ds"": """",
  ""ajax_api_url_ds"": """",
  ""description"": """",
  ""label_help_text"": """"
}";
	var weight = 1;

	new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id,parentId,pageId,nodeId,weight,componentName,containerId,options,WebVella.Erp.Database.DbContext.Current.Transaction);
}
#endregion

#region << ***Update page body node*** Page: details ID: 151d5da3-161a-44c0-97fa-84c76c9d3b60 >>
{
	var id = new Guid("151d5da3-161a-44c0-97fa-84c76c9d3b60");
	Guid? parentId = new Guid("651e5fb2-56df-4c46-86b3-19a641dc942d");
	Guid? nodeId = null;
	Guid pageId = new Guid("3a40b8e6-0a87-4eee-9b6b-6c665ebee28c");
	var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
	var containerId = "body";
	var options = @"{
  ""is_visible"": """",
  ""label_mode"": ""0"",
  ""label_text"": """",
  ""mode"": ""3"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""Record.start_time\"",\""default\"":\""\""}"",
  ""name"": ""start_time"",
  ""class"": """",
  ""show_icon"": ""false"",
  ""connected_entity_id"": """",
  ""connected_record_id_ds"": """",
  ""access_override_ds"": """",
  ""required_override_ds"": """",
  ""ajax_api_url_ds"": """",
  ""description"": """",
  ""label_help_text"": """"
}";
	var weight = 1;

	new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id,parentId,pageId,nodeId,weight,componentName,containerId,options,WebVella.Erp.Database.DbContext.Current.Transaction);
}
#endregion

#region << ***Update page body node*** Page: open ID: e1b676c0-e128-46a2-b2cc-51a5b3ec2816 >>
{
	var id = new Guid("e1b676c0-e128-46a2-b2cc-51a5b3ec2816");
	Guid? parentId = new Guid("250115da-cea5-46f3-a77a-d2f7704c650d");
	Guid? nodeId = null;
	Guid pageId = new Guid("273dd749-3804-48c8-8306-078f1e7f3b3f");
	var componentName = "WebVella.Erp.Web.Components.PcButton";
	var containerId = "actions";
	var options = @"{
  ""type"": ""0"",
  ""text"": ""Search"",
  ""color"": ""0"",
  ""size"": ""3"",
  ""class"": """",
  ""id"": """",
  ""icon_class"": ""fa fa-search"",
  ""is_block"": ""false"",
  ""is_outline"": ""false"",
  ""is_active"": ""false"",
  ""is_disabled"": ""false"",
  ""onclick"": ""ErpEvent.DISPATCH('WebVella.Erp.Web.Components.PcDrawer','open')"",
  ""href"": """",
  ""new_tab"": ""false"",
  ""form"": """"
}";
	var weight = 1;

	new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id,parentId,pageId,nodeId,weight,componentName,containerId,options,WebVella.Erp.Database.DbContext.Current.Transaction);
}
#endregion

#region << ***Update page body node*** Page: all ID: ad9c357f-e620-4ed1-9593-d76c97019677 >>
{
	var id = new Guid("ad9c357f-e620-4ed1-9593-d76c97019677");
	Guid? parentId = new Guid("8b4b07e4-b994-4fdc-95d4-1e7b33dea6dc");
	Guid? nodeId = null;
	Guid pageId = new Guid("6d3fe557-59dd-4a2e-b710-f3f326ae172b");
	var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
	var containerId = "column7";
	var options = @"{
  ""is_visible"": """",
  ""label_mode"": ""3"",
  ""label_text"": """",
  ""mode"": ""4"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.created_on\"",\""default\"":\""\""}"",
  ""name"": ""created_on"",
  ""class"": """",
  ""show_icon"": ""false"",
  ""connected_entity_id"": """",
  ""connected_record_id_ds"": """",
  ""access_override_ds"": """",
  ""required_override_ds"": """",
  ""ajax_api_url_ds"": """"
}";
	var weight = 1;

	new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id,parentId,pageId,nodeId,weight,componentName,containerId,options,WebVella.Erp.Database.DbContext.Current.Transaction);
}
#endregion

#region << ***Update page body node*** Page: open ID: bd05d5ef-0ab4-48b0-a40e-5959875d071b >>
{
	var id = new Guid("bd05d5ef-0ab4-48b0-a40e-5959875d071b");
	Guid? parentId = new Guid("a4719fbd-b3d0-4f81-b302-96f5620e17cc");
	Guid? nodeId = null;
	Guid pageId = new Guid("273dd749-3804-48c8-8306-078f1e7f3b3f");
	var componentName = "WebVella.Erp.Web.Components.PcFieldDate";
	var containerId = "column7";
	var options = @"{
  ""is_visible"": """",
  ""label_mode"": ""3"",
  ""label_text"": """",
  ""mode"": ""4"",
  ""value"": ""{\""type\"":\""0\"",\""string\"":\""RowRecord.created_on\"",\""default\"":\""\""}"",
  ""name"": ""created_on"",
  ""class"": """",
  ""show_icon"": ""false"",
  ""connected_entity_id"": """",
  ""connected_record_id_ds"": """",
  ""access_override_ds"": """",
  ""required_override_ds"": """",
  ""ajax_api_url_ds"": """"
}";
	var weight = 1;

	new WebVella.Erp.Web.Services.PageService().UpdatePageBodyNode(id,parentId,pageId,nodeId,weight,componentName,containerId,options,WebVella.Erp.Database.DbContext.Current.Transaction);
}
#endregion

#region << ***Update data source*** Name: WvProjectAllAccounts >>
{
	var id = new Guid("61d21547-b353-48b8-8b75-b727680da79e");
	var name = @"WvProjectAllAccounts";
	var description = @"Lists all accounts in the system";
	var eqlText = @"SELECT id,name 
FROM account
where name CONTAINS @name
ORDER BY @sortBy ASC
PAGE @page
PAGESIZE @pageSize";
	var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT 
	 rec_account.""id"" AS ""id"",
	 rec_account.""name"" AS ""name"",
	 COUNT(*) OVER() AS ___total_count___
FROM rec_account
WHERE  ( rec_account.""name""  ILIKE  CONCAT ( '%' , @name , '%' ) )
ORDER BY rec_account.""name"" ASC
LIMIT 10
OFFSET 0
) X
";
	var parametersJson = @"[{""name"":""name"",""type"":""text"",""value"":""null"",""ignore_parse_errors"":false},{""name"":""sortBy"",""type"":""text"",""value"":""name"",""ignore_parse_errors"":false},{""name"":""page"",""type"":""int"",""value"":""1"",""ignore_parse_errors"":false},{""name"":""pageSize"",""type"":""int"",""value"":""10"",""ignore_parse_errors"":false}]";
	var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]";
	var weight = 10;
	var entityName =  @"account";

	new WebVella.Erp.Database.DbDataSourceRepository().Update(id,name,description,weight,eqlText,sqlText,parametersJson,fieldsJson,entityName);
}
#endregion

#region << ***Update data source*** Name: WvProjectAllTasks >>
{
	var id = new Guid("5a6e9d56-63bc-43b1-b95e-24838db9f435");
	var name = @"WvProjectAllTasks";
	var description = @"All tasks selection";
	var eqlText = @"SELECT *,$project_nn_task.abbr,$user_1n_task.username,$task_status_1n_task.label,$task_type_1n_task.label,$task_type_1n_task.icon_class,$task_type_1n_task.color,$user_1n_task_creator.username
FROM task
WHERE x_search CONTAINS @searchQuery
ORDER BY @sortBy @sortOrder
PAGE @page
PAGESIZE @pageSize
";
	var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_task.""id"" AS ""id"",
	 rec_task.""subject"" AS ""subject"",
	 rec_task.""body"" AS ""body"",
	 rec_task.""created_on"" AS ""created_on"",
	 rec_task.""created_by"" AS ""created_by"",
	 rec_task.""completed_on"" AS ""completed_on"",
	 rec_task.""number"" AS ""number"",
	 rec_task.""parent_id"" AS ""parent_id"",
	 rec_task.""status_id"" AS ""status_id"",
	 rec_task.""key"" AS ""key"",
	 rec_task.""estimated_minutes"" AS ""estimated_minutes"",
	 rec_task.""x_billable_minutes"" AS ""x_billable_minutes"",
	 rec_task.""x_nonbillable_minutes"" AS ""x_nonbillable_minutes"",
	 rec_task.""priority"" AS ""priority"",
	 rec_task.""timelog_started_on"" AS ""timelog_started_on"",
	 rec_task.""owner_id"" AS ""owner_id"",
	 rec_task.""type_id"" AS ""type_id"",
	 rec_task.""start_time"" AS ""start_time"",
	 rec_task.""end_time"" AS ""end_time"",
	 rec_task.""recurrence_id"" AS ""recurrence_id"",
	 rec_task.""reserve_time"" AS ""reserve_time"",
	 rec_task.""recurrence_template"" AS ""recurrence_template"",
	 rec_task.""l_scope"" AS ""l_scope"",
	 rec_task.""x_search"" AS ""x_search"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $project_nn_task
	(SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
	 SELECT 
		 project_nn_task.""id"" AS ""id"",
		 project_nn_task.""abbr"" AS ""abbr""
	 FROM rec_project project_nn_task
	 LEFT JOIN  rel_project_nn_task project_nn_task_target ON project_nn_task_target.target_id = rec_task.id
	 WHERE project_nn_task.id = project_nn_task_target.origin_id )d  )::jsonb AS ""$project_nn_task"",
	-------< $project_nn_task
	------->: $user_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_task.""id"" AS ""id"",
		 user_1n_task.""username"" AS ""username"" 
	 FROM rec_user user_1n_task
	 WHERE user_1n_task.id = rec_task.owner_id ) d )::jsonb AS ""$user_1n_task"",
	-------< $user_1n_task
	------->: $task_status_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 task_status_1n_task.""id"" AS ""id"",
		 task_status_1n_task.""label"" AS ""label"" 
	 FROM rec_task_status task_status_1n_task
	 WHERE task_status_1n_task.id = rec_task.status_id ) d )::jsonb AS ""$task_status_1n_task"",
	-------< $task_status_1n_task
	------->: $task_type_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 task_type_1n_task.""id"" AS ""id"",
		 task_type_1n_task.""label"" AS ""label"",
		 task_type_1n_task.""icon_class"" AS ""icon_class"",
		 task_type_1n_task.""color"" AS ""color"" 
	 FROM rec_task_type task_type_1n_task
	 WHERE task_type_1n_task.id = rec_task.type_id ) d )::jsonb AS ""$task_type_1n_task"",
	-------< $task_type_1n_task
	------->: $user_1n_task_creator
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_task_creator.""id"" AS ""id"",
		 user_1n_task_creator.""username"" AS ""username"" 
	 FROM rec_user user_1n_task_creator
	 WHERE user_1n_task_creator.id = rec_task.created_by ) d )::jsonb AS ""$user_1n_task_creator""	
	-------< $user_1n_task_creator

FROM rec_task
WHERE  ( rec_task.""x_search""  ILIKE  @searchQuery ) 
ORDER BY rec_task.""end_time"" ASC
LIMIT 10
OFFSET 0
) X
";
	var parametersJson = @"[{""name"":""sortBy"",""type"":""text"",""value"":""end_time"",""ignore_parse_errors"":false},{""name"":""sortOrder"",""type"":""text"",""value"":""asc"",""ignore_parse_errors"":false},{""name"":""page"",""type"":""int"",""value"":""1"",""ignore_parse_errors"":false},{""name"":""pageSize"",""type"":""int"",""value"":""10"",""ignore_parse_errors"":false},{""name"":""searchQuery"",""type"":""text"",""value"":""string.empty"",""ignore_parse_errors"":false}]";
	var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":8,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""completed_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""number"",""type"":1,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""status_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""key"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""estimated_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_billable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_nonbillable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""priority"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""timelog_started_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""owner_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""type_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""start_time"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""end_time"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""recurrence_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""reserve_time"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""recurrence_template"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_search"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$project_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""abbr"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$user_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_status_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_type_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""icon_class"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""color"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$user_1n_task_creator"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
	var weight = 10;
	var entityName =  @"task";

	new WebVella.Erp.Database.DbDataSourceRepository().Update(id,name,description,weight,eqlText,sqlText,parametersJson,fieldsJson,entityName);
}
#endregion

#region << ***Update data source*** Name: WvProjectAllProjects >>
{
	var id = new Guid("96218f33-42f1-4ff1-926c-b1765e1f8c6e");
	var name = @"WvProjectAllProjects";
	var description = @"all project records";
	var eqlText = @"SELECT id,abbr,name,$user_1n_project_owner.username
FROM project
WHERE name CONTAINS @filterName AND (@onlyActive = false OR start_date < @now AND end_date > @now)
ORDER BY @sortBy @sortOrder
PAGE @page
PAGESIZE @pageSize
";
	var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT 
	 rec_project.""id"" AS ""id"",
	 rec_project.""abbr"" AS ""abbr"",
	 rec_project.""name"" AS ""name"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $user_1n_project_owner
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM (
	 SELECT 
		 user_1n_project_owner.""id"" AS ""id"",
		 user_1n_project_owner.""username"" AS ""username""
	 FROM rec_user user_1n_project_owner
	 WHERE user_1n_project_owner.id = rec_project.owner_id ) d )::jsonb AS ""$user_1n_project_owner""	
	-------< $user_1n_project_owner

FROM rec_project
WHERE  (  ( rec_project.""name""  ILIKE  CONCAT ( '%' , @filterName , '%' ) ) AND  (  ( @onlyActive = FALSE )  OR  (  ( rec_project.""start_date"" < @now )  AND  ( rec_project.""end_date"" > @now )  )  )  ) 
ORDER BY rec_project.""name"" ASC
LIMIT 10
OFFSET 0
) X
";
	var parametersJson = @"[{""name"":""sortBy"",""type"":""text"",""value"":""name"",""ignore_parse_errors"":false},{""name"":""sortOrder"",""type"":""text"",""value"":""asc"",""ignore_parse_errors"":false},{""name"":""page"",""type"":""int"",""value"":""1"",""ignore_parse_errors"":false},{""name"":""pageSize"",""type"":""int"",""value"":""10"",""ignore_parse_errors"":false},{""name"":""filterName"",""type"":""text"",""value"":""null"",""ignore_parse_errors"":false},{""name"":""now"",""type"":""date"",""value"":""utc_now"",""ignore_parse_errors"":false},{""name"":""onlyActive"",""type"":""bool"",""value"":""false"",""ignore_parse_errors"":false}]";
	var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""abbr"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$user_1n_project_owner"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
	var weight = 10;
	var entityName =  @"project";

	new WebVella.Erp.Database.DbDataSourceRepository().Update(id,name,description,weight,eqlText,sqlText,parametersJson,fieldsJson,entityName);
}
#endregion

#region << ***Update data source*** Name: WvProjectCommentsForRecordId >>
{
	var id = new Guid("a588e096-358d-4426-adf6-5db693f32322");
	var name = @"WvProjectCommentsForRecordId";
	var description = @"Get all comments for a record";
	var eqlText = @"SELECT *,$user_1n_comment.image,$user_1n_comment.username
FROM comment
WHERE l_related_records CONTAINS @recordId 
ORDER BY @sortBy @sortOrder
PAGE @page
PAGESIZE @pageSize";
	var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_comment.""id"" AS ""id"",
	 rec_comment.""body"" AS ""body"",
	 rec_comment.""created_by"" AS ""created_by"",
	 rec_comment.""parent_id"" AS ""parent_id"",
	 rec_comment.""created_on"" AS ""created_on"",
	 rec_comment.""l_related_records"" AS ""l_related_records"",
	 rec_comment.""l_scope"" AS ""l_scope"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $user_1n_comment
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_comment.""id"" AS ""id"",
		 user_1n_comment.""image"" AS ""image"",
		 user_1n_comment.""username"" AS ""username"" 
	 FROM rec_user user_1n_comment
	 WHERE user_1n_comment.id = rec_comment.created_by ) d )::jsonb AS ""$user_1n_comment""	
	-------< $user_1n_comment

FROM rec_comment
WHERE  ( rec_comment.""l_related_records""  ILIKE  @recordId ) 
ORDER BY rec_comment.""created_on"" DESC
LIMIT 10
OFFSET 0
) X
";
	var parametersJson = @"[{""name"":""sortBy"",""type"":""text"",""value"":""created_on"",""ignore_parse_errors"":false},{""name"":""sortOrder"",""type"":""text"",""value"":""desc"",""ignore_parse_errors"":false},{""name"":""page"",""type"":""int"",""value"":""1"",""ignore_parse_errors"":false},{""name"":""pageSize"",""type"":""int"",""value"":""10"",""ignore_parse_errors"":false},{""name"":""recordId"",""type"":""text"",""value"":""string.empty"",""ignore_parse_errors"":false}]";
	var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_related_records"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$user_1n_comment"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""image"",""type"":9,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
	var weight = 10;
	var entityName =  @"comment";

	new WebVella.Erp.Database.DbDataSourceRepository().Update(id,name,description,weight,eqlText,sqlText,parametersJson,fieldsJson,entityName);
}
#endregion

#region << ***Update data source*** Name: WvProjectAllProjectTasks >>
{
	var id = new Guid("c2284f3d-2ddc-4bad-9d1b-f6e44d502bdd");
	var name = @"WvProjectAllProjectTasks";
	var description = @"All tasks in a project";
	var eqlText = @"SELECT *,$project_nn_task.abbr,$user_1n_task.username,$task_status_1n_task.label,$task_type_1n_task.label,$task_type_1n_task.icon_class,$task_type_1n_task.color,$user_1n_task_creator.username
FROM task
WHERE x_search CONTAINS @searchQuery AND $project_nn_task.id = @projectId
ORDER BY @sortBy @sortOrder
PAGE @page
PAGESIZE @pageSize";
	var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_task.""id"" AS ""id"",
	 rec_task.""subject"" AS ""subject"",
	 rec_task.""body"" AS ""body"",
	 rec_task.""created_on"" AS ""created_on"",
	 rec_task.""created_by"" AS ""created_by"",
	 rec_task.""completed_on"" AS ""completed_on"",
	 rec_task.""number"" AS ""number"",
	 rec_task.""parent_id"" AS ""parent_id"",
	 rec_task.""status_id"" AS ""status_id"",
	 rec_task.""key"" AS ""key"",
	 rec_task.""estimated_minutes"" AS ""estimated_minutes"",
	 rec_task.""x_billable_minutes"" AS ""x_billable_minutes"",
	 rec_task.""x_nonbillable_minutes"" AS ""x_nonbillable_minutes"",
	 rec_task.""priority"" AS ""priority"",
	 rec_task.""timelog_started_on"" AS ""timelog_started_on"",
	 rec_task.""owner_id"" AS ""owner_id"",
	 rec_task.""type_id"" AS ""type_id"",
	 rec_task.""start_time"" AS ""start_time"",
	 rec_task.""end_time"" AS ""end_time"",
	 rec_task.""recurrence_id"" AS ""recurrence_id"",
	 rec_task.""reserve_time"" AS ""reserve_time"",
	 rec_task.""recurrence_template"" AS ""recurrence_template"",
	 rec_task.""l_scope"" AS ""l_scope"",
	 rec_task.""x_search"" AS ""x_search"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $project_nn_task
	(SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
	 SELECT 
		 project_nn_task.""id"" AS ""id"",
		 project_nn_task.""abbr"" AS ""abbr""
	 FROM rec_project project_nn_task
	 LEFT JOIN  rel_project_nn_task project_nn_task_target ON project_nn_task_target.target_id = rec_task.id
	 WHERE project_nn_task.id = project_nn_task_target.origin_id )d  )::jsonb AS ""$project_nn_task"",
	-------< $project_nn_task
	------->: $user_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_task.""id"" AS ""id"",
		 user_1n_task.""username"" AS ""username"" 
	 FROM rec_user user_1n_task
	 WHERE user_1n_task.id = rec_task.owner_id ) d )::jsonb AS ""$user_1n_task"",
	-------< $user_1n_task
	------->: $task_status_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 task_status_1n_task.""id"" AS ""id"",
		 task_status_1n_task.""label"" AS ""label"" 
	 FROM rec_task_status task_status_1n_task
	 WHERE task_status_1n_task.id = rec_task.status_id ) d )::jsonb AS ""$task_status_1n_task"",
	-------< $task_status_1n_task
	------->: $task_type_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 task_type_1n_task.""id"" AS ""id"",
		 task_type_1n_task.""label"" AS ""label"",
		 task_type_1n_task.""icon_class"" AS ""icon_class"",
		 task_type_1n_task.""color"" AS ""color"" 
	 FROM rec_task_type task_type_1n_task
	 WHERE task_type_1n_task.id = rec_task.type_id ) d )::jsonb AS ""$task_type_1n_task"",
	-------< $task_type_1n_task
	------->: $user_1n_task_creator
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_task_creator.""id"" AS ""id"",
		 user_1n_task_creator.""username"" AS ""username"" 
	 FROM rec_user user_1n_task_creator
	 WHERE user_1n_task_creator.id = rec_task.created_by ) d )::jsonb AS ""$user_1n_task_creator""	
	-------< $user_1n_task_creator

FROM rec_task
LEFT OUTER JOIN  rel_project_nn_task project_nn_task_target ON project_nn_task_target.target_id = rec_task.id
LEFT OUTER JOIN  rec_project project_nn_task_tar_org ON project_nn_task_target.origin_id = project_nn_task_tar_org.id
WHERE  (  ( rec_task.""x_search""  ILIKE  @searchQuery )  AND  ( project_nn_task_tar_org.""id"" = @projectId )  ) 
ORDER BY rec_task.""end_time"" ASC
LIMIT 10
OFFSET 0
) X
";
	var parametersJson = @"[{""name"":""sortBy"",""type"":""text"",""value"":""end_time"",""ignore_parse_errors"":false},{""name"":""sortOrder"",""type"":""text"",""value"":""asc"",""ignore_parse_errors"":false},{""name"":""page"",""type"":""int"",""value"":""1"",""ignore_parse_errors"":false},{""name"":""pageSize"",""type"":""int"",""value"":""10"",""ignore_parse_errors"":false},{""name"":""searchQuery"",""type"":""text"",""value"":""string.empty"",""ignore_parse_errors"":false},{""name"":""projectId"",""type"":""guid"",""value"":""guid.empty"",""ignore_parse_errors"":false}]";
	var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":8,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""completed_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""number"",""type"":1,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""status_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""key"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""estimated_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_billable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_nonbillable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""priority"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""timelog_started_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""owner_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""type_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""start_time"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""end_time"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""recurrence_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""reserve_time"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""recurrence_template"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_search"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$project_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""abbr"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$user_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_status_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_type_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""icon_class"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""color"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$user_1n_task_creator"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
	var weight = 10;
	var entityName =  @"task";

	new WebVella.Erp.Database.DbDataSourceRepository().Update(id,name,description,weight,eqlText,sqlText,parametersJson,fieldsJson,entityName);
}
#endregion

#region << ***Update data source*** Name: WvProjectFeedItemsForRecordId >>
{
	var id = new Guid("74e5a414-6deb-4af6-8e29-567f718ca430");
	var name = @"WvProjectFeedItemsForRecordId";
	var description = @"Get all feed items for a record";
	var eqlText = @"SELECT *,$user_1n_feed_item.image,$user_1n_feed_item.username
FROM feed_item
WHERE l_related_records CONTAINS @recordId AND type CONTAINS @type
ORDER BY @sortBy @sortOrder
PAGE @page
PAGESIZE @pageSize
";
	var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_feed_item.""id"" AS ""id"",
	 rec_feed_item.""created_by"" AS ""created_by"",
	 rec_feed_item.""created_on"" AS ""created_on"",
	 rec_feed_item.""subject"" AS ""subject"",
	 rec_feed_item.""body"" AS ""body"",
	 rec_feed_item.""type"" AS ""type"",
	 rec_feed_item.""l_related_records"" AS ""l_related_records"",
	 rec_feed_item.""l_scope"" AS ""l_scope"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $user_1n_feed_item
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_feed_item.""id"" AS ""id"",
		 user_1n_feed_item.""image"" AS ""image"",
		 user_1n_feed_item.""username"" AS ""username"" 
	 FROM rec_user user_1n_feed_item
	 WHERE user_1n_feed_item.id = rec_feed_item.created_by ) d )::jsonb AS ""$user_1n_feed_item""	
	-------< $user_1n_feed_item

FROM rec_feed_item
WHERE  (  ( rec_feed_item.""l_related_records""  ILIKE  @recordId )  AND  ( rec_feed_item.""type""  ILIKE  @type )  ) 
ORDER BY rec_feed_item.""created_on"" DESC
LIMIT 10
OFFSET 0
) X
";
	var parametersJson = @"[{""name"":""sortBy"",""type"":""text"",""value"":""created_on"",""ignore_parse_errors"":false},{""name"":""sortOrder"",""type"":""text"",""value"":""desc"",""ignore_parse_errors"":false},{""name"":""page"",""type"":""int"",""value"":""1"",""ignore_parse_errors"":false},{""name"":""pageSize"",""type"":""int"",""value"":""10"",""ignore_parse_errors"":false},{""name"":""recordId"",""type"":""text"",""value"":""string.empty"",""ignore_parse_errors"":false},{""name"":""type"",""type"":""text"",""value"":""string.empty"",""ignore_parse_errors"":false}]";
	var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""type"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_related_records"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$user_1n_feed_item"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""image"",""type"":9,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
	var weight = 10;
	var entityName =  @"feed_item";

	new WebVella.Erp.Database.DbDataSourceRepository().Update(id,name,description,weight,eqlText,sqlText,parametersJson,fieldsJson,entityName);
}
#endregion

#region << ***Update data source*** Name: WvProjectNoOwnerTasks >>
{
	var id = new Guid("40c0bcc6-2e3e-4b68-ae6a-27f1f472f069");
	var name = @"WvProjectNoOwnerTasks";
	var description = @"all tasks without an owner";
	var eqlText = @"SELECT *,$project_nn_task.abbr,$user_1n_task.username,$task_status_1n_task.label,$task_type_1n_task.label,$task_type_1n_task.icon_class,$task_type_1n_task.color,$user_1n_task_creator.username
FROM task
WHERE owner_id = NULL AND x_search CONTAINS @searchQuery AND status_id <> 'b1cc69e5-ce09-40e0-8785-b6452b257bdf'
ORDER BY @sortBy @sortOrder
PAGE @page
PAGESIZE @pageSize
";
	var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_task.""id"" AS ""id"",
	 rec_task.""subject"" AS ""subject"",
	 rec_task.""body"" AS ""body"",
	 rec_task.""created_on"" AS ""created_on"",
	 rec_task.""created_by"" AS ""created_by"",
	 rec_task.""completed_on"" AS ""completed_on"",
	 rec_task.""number"" AS ""number"",
	 rec_task.""parent_id"" AS ""parent_id"",
	 rec_task.""status_id"" AS ""status_id"",
	 rec_task.""key"" AS ""key"",
	 rec_task.""estimated_minutes"" AS ""estimated_minutes"",
	 rec_task.""x_billable_minutes"" AS ""x_billable_minutes"",
	 rec_task.""x_nonbillable_minutes"" AS ""x_nonbillable_minutes"",
	 rec_task.""priority"" AS ""priority"",
	 rec_task.""timelog_started_on"" AS ""timelog_started_on"",
	 rec_task.""owner_id"" AS ""owner_id"",
	 rec_task.""type_id"" AS ""type_id"",
	 rec_task.""start_time"" AS ""start_time"",
	 rec_task.""end_time"" AS ""end_time"",
	 rec_task.""recurrence_id"" AS ""recurrence_id"",
	 rec_task.""reserve_time"" AS ""reserve_time"",
	 rec_task.""recurrence_template"" AS ""recurrence_template"",
	 rec_task.""l_scope"" AS ""l_scope"",
	 rec_task.""x_search"" AS ""x_search"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $project_nn_task
	(SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
	 SELECT 
		 project_nn_task.""id"" AS ""id"",
		 project_nn_task.""abbr"" AS ""abbr""
	 FROM rec_project project_nn_task
	 LEFT JOIN  rel_project_nn_task project_nn_task_target ON project_nn_task_target.target_id = rec_task.id
	 WHERE project_nn_task.id = project_nn_task_target.origin_id )d  )::jsonb AS ""$project_nn_task"",
	-------< $project_nn_task
	------->: $user_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_task.""id"" AS ""id"",
		 user_1n_task.""username"" AS ""username"" 
	 FROM rec_user user_1n_task
	 WHERE user_1n_task.id = rec_task.owner_id ) d )::jsonb AS ""$user_1n_task"",
	-------< $user_1n_task
	------->: $task_status_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 task_status_1n_task.""id"" AS ""id"",
		 task_status_1n_task.""label"" AS ""label"" 
	 FROM rec_task_status task_status_1n_task
	 WHERE task_status_1n_task.id = rec_task.status_id ) d )::jsonb AS ""$task_status_1n_task"",
	-------< $task_status_1n_task
	------->: $task_type_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 task_type_1n_task.""id"" AS ""id"",
		 task_type_1n_task.""label"" AS ""label"",
		 task_type_1n_task.""icon_class"" AS ""icon_class"",
		 task_type_1n_task.""color"" AS ""color"" 
	 FROM rec_task_type task_type_1n_task
	 WHERE task_type_1n_task.id = rec_task.type_id ) d )::jsonb AS ""$task_type_1n_task"",
	-------< $task_type_1n_task
	------->: $user_1n_task_creator
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_task_creator.""id"" AS ""id"",
		 user_1n_task_creator.""username"" AS ""username"" 
	 FROM rec_user user_1n_task_creator
	 WHERE user_1n_task_creator.id = rec_task.created_by ) d )::jsonb AS ""$user_1n_task_creator""	
	-------< $user_1n_task_creator

FROM rec_task
WHERE  (  (  ( rec_task.""owner_id"" IS NULL )  AND  ( rec_task.""x_search""  ILIKE  @searchQuery )  )  AND  ( rec_task.""status_id"" <> 'b1cc69e5-ce09-40e0-8785-b6452b257bdf' )  ) 
ORDER BY rec_task.""end_time"" ASC
LIMIT 10
OFFSET 0
) X
";
	var parametersJson = @"[{""name"":""sortBy"",""type"":""text"",""value"":""end_time"",""ignore_parse_errors"":false},{""name"":""sortOrder"",""type"":""text"",""value"":""asc"",""ignore_parse_errors"":false},{""name"":""page"",""type"":""int"",""value"":""1"",""ignore_parse_errors"":false},{""name"":""pageSize"",""type"":""int"",""value"":""10"",""ignore_parse_errors"":false},{""name"":""searchQuery"",""type"":""text"",""value"":""string.empty"",""ignore_parse_errors"":false}]";
	var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":8,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""completed_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""number"",""type"":1,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""status_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""key"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""estimated_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_billable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_nonbillable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""priority"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""timelog_started_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""owner_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""type_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""start_time"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""end_time"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""recurrence_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""reserve_time"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""recurrence_template"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_search"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$project_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""abbr"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$user_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_status_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_type_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""icon_class"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""color"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$user_1n_task_creator"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
	var weight = 10;
	var entityName =  @"task";

	new WebVella.Erp.Database.DbDataSourceRepository().Update(id,name,description,weight,eqlText,sqlText,parametersJson,fieldsJson,entityName);
}
#endregion

#region << ***Update data source*** Name: WvProjectAllOpenTasks >>
{
	var id = new Guid("9c2337ac-b505-4ce4-b1ff-ffde2e37b312");
	var name = @"WvProjectAllOpenTasks";
	var description = @"All open tasks selection";
	var eqlText = @"SELECT *,$project_nn_task.abbr,$user_1n_task.username,$task_status_1n_task.label,$task_type_1n_task.label,$task_type_1n_task.icon_class,$task_type_1n_task.color,$user_1n_task_creator.username
FROM task
WHERE status_id <> 'b1cc69e5-ce09-40e0-8785-b6452b257bdf' AND x_search CONTAINS @searchQuery
ORDER BY @sortBy @sortOrder
PAGE @page
PAGESIZE @pageSize";
	var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_task.""id"" AS ""id"",
	 rec_task.""subject"" AS ""subject"",
	 rec_task.""body"" AS ""body"",
	 rec_task.""created_on"" AS ""created_on"",
	 rec_task.""created_by"" AS ""created_by"",
	 rec_task.""completed_on"" AS ""completed_on"",
	 rec_task.""number"" AS ""number"",
	 rec_task.""parent_id"" AS ""parent_id"",
	 rec_task.""status_id"" AS ""status_id"",
	 rec_task.""key"" AS ""key"",
	 rec_task.""estimated_minutes"" AS ""estimated_minutes"",
	 rec_task.""x_billable_minutes"" AS ""x_billable_minutes"",
	 rec_task.""x_nonbillable_minutes"" AS ""x_nonbillable_minutes"",
	 rec_task.""priority"" AS ""priority"",
	 rec_task.""timelog_started_on"" AS ""timelog_started_on"",
	 rec_task.""owner_id"" AS ""owner_id"",
	 rec_task.""type_id"" AS ""type_id"",
	 rec_task.""start_time"" AS ""start_time"",
	 rec_task.""end_time"" AS ""end_time"",
	 rec_task.""recurrence_id"" AS ""recurrence_id"",
	 rec_task.""reserve_time"" AS ""reserve_time"",
	 rec_task.""recurrence_template"" AS ""recurrence_template"",
	 rec_task.""l_scope"" AS ""l_scope"",
	 rec_task.""x_search"" AS ""x_search"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $project_nn_task
	(SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
	 SELECT 
		 project_nn_task.""id"" AS ""id"",
		 project_nn_task.""abbr"" AS ""abbr""
	 FROM rec_project project_nn_task
	 LEFT JOIN  rel_project_nn_task project_nn_task_target ON project_nn_task_target.target_id = rec_task.id
	 WHERE project_nn_task.id = project_nn_task_target.origin_id )d  )::jsonb AS ""$project_nn_task"",
	-------< $project_nn_task
	------->: $user_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_task.""id"" AS ""id"",
		 user_1n_task.""username"" AS ""username"" 
	 FROM rec_user user_1n_task
	 WHERE user_1n_task.id = rec_task.owner_id ) d )::jsonb AS ""$user_1n_task"",
	-------< $user_1n_task
	------->: $task_status_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 task_status_1n_task.""id"" AS ""id"",
		 task_status_1n_task.""label"" AS ""label"" 
	 FROM rec_task_status task_status_1n_task
	 WHERE task_status_1n_task.id = rec_task.status_id ) d )::jsonb AS ""$task_status_1n_task"",
	-------< $task_status_1n_task
	------->: $task_type_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 task_type_1n_task.""id"" AS ""id"",
		 task_type_1n_task.""label"" AS ""label"",
		 task_type_1n_task.""icon_class"" AS ""icon_class"",
		 task_type_1n_task.""color"" AS ""color"" 
	 FROM rec_task_type task_type_1n_task
	 WHERE task_type_1n_task.id = rec_task.type_id ) d )::jsonb AS ""$task_type_1n_task"",
	-------< $task_type_1n_task
	------->: $user_1n_task_creator
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_task_creator.""id"" AS ""id"",
		 user_1n_task_creator.""username"" AS ""username"" 
	 FROM rec_user user_1n_task_creator
	 WHERE user_1n_task_creator.id = rec_task.created_by ) d )::jsonb AS ""$user_1n_task_creator""	
	-------< $user_1n_task_creator

FROM rec_task
WHERE  (  ( rec_task.""status_id"" <> 'b1cc69e5-ce09-40e0-8785-b6452b257bdf' )  AND  ( rec_task.""x_search""  ILIKE  @searchQuery )  ) 
ORDER BY rec_task.""end_time"" ASC
LIMIT 10
OFFSET 0
) X
";
	var parametersJson = @"[{""name"":""sortBy"",""type"":""text"",""value"":""end_time"",""ignore_parse_errors"":false},{""name"":""sortOrder"",""type"":""text"",""value"":""asc"",""ignore_parse_errors"":false},{""name"":""page"",""type"":""int"",""value"":""1"",""ignore_parse_errors"":false},{""name"":""pageSize"",""type"":""int"",""value"":""10"",""ignore_parse_errors"":false},{""name"":""searchQuery"",""type"":""text"",""value"":""string.empty"",""ignore_parse_errors"":false}]";
	var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":8,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""completed_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""number"",""type"":1,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""status_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""key"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""estimated_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_billable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_nonbillable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""priority"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""timelog_started_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""owner_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""type_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""start_time"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""end_time"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""recurrence_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""reserve_time"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""recurrence_template"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_search"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$project_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""abbr"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$user_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_status_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_type_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""icon_class"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""color"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$user_1n_task_creator"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
	var weight = 10;
	var entityName =  @"task";

	new WebVella.Erp.Database.DbDataSourceRepository().Update(id,name,description,weight,eqlText,sqlText,parametersJson,fieldsJson,entityName);
}
#endregion

#region << ***Update data source*** Name: WvProjectOpenTasks >>
{
	var id = new Guid("46aab266-e2a8-4b67-9155-39ec1cf3bccb");
	var name = @"WvProjectOpenTasks";
	var description = @"All open tasks for a project";
	var eqlText = @"SELECT *,$milestone_nn_task.name,$task_status_1n_task.label,$task_type_1n_task.label
FROM task
WHERE $project_nn_task.id = @projectId
ORDER BY @sortBy @sortOrder
PAGE @page
PAGESIZE @pageSize
";
	var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_task.""id"" AS ""id"",
	 rec_task.""subject"" AS ""subject"",
	 rec_task.""body"" AS ""body"",
	 rec_task.""created_on"" AS ""created_on"",
	 rec_task.""created_by"" AS ""created_by"",
	 rec_task.""completed_on"" AS ""completed_on"",
	 rec_task.""number"" AS ""number"",
	 rec_task.""parent_id"" AS ""parent_id"",
	 rec_task.""status_id"" AS ""status_id"",
	 rec_task.""key"" AS ""key"",
	 rec_task.""estimated_minutes"" AS ""estimated_minutes"",
	 rec_task.""x_billable_minutes"" AS ""x_billable_minutes"",
	 rec_task.""x_nonbillable_minutes"" AS ""x_nonbillable_minutes"",
	 rec_task.""priority"" AS ""priority"",
	 rec_task.""timelog_started_on"" AS ""timelog_started_on"",
	 rec_task.""owner_id"" AS ""owner_id"",
	 rec_task.""type_id"" AS ""type_id"",
	 rec_task.""start_time"" AS ""start_time"",
	 rec_task.""end_time"" AS ""end_time"",
	 rec_task.""recurrence_id"" AS ""recurrence_id"",
	 rec_task.""reserve_time"" AS ""reserve_time"",
	 rec_task.""recurrence_template"" AS ""recurrence_template"",
	 rec_task.""l_scope"" AS ""l_scope"",
	 rec_task.""x_search"" AS ""x_search"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $milestone_nn_task
	(SELECT  COALESCE(  array_to_json(array_agg( row_to_json(d))), '[]') FROM ( 
	 SELECT 
		 milestone_nn_task.""id"" AS ""id"",
		 milestone_nn_task.""name"" AS ""name""
	 FROM rec_milestone milestone_nn_task
	 LEFT JOIN  rel_milestone_nn_task milestone_nn_task_target ON milestone_nn_task_target.target_id = rec_task.id
	 WHERE milestone_nn_task.id = milestone_nn_task_target.origin_id )d  )::jsonb AS ""$milestone_nn_task"",
	-------< $milestone_nn_task
	------->: $task_status_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 task_status_1n_task.""id"" AS ""id"",
		 task_status_1n_task.""label"" AS ""label"" 
	 FROM rec_task_status task_status_1n_task
	 WHERE task_status_1n_task.id = rec_task.status_id ) d )::jsonb AS ""$task_status_1n_task"",
	-------< $task_status_1n_task
	------->: $task_type_1n_task
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 task_type_1n_task.""id"" AS ""id"",
		 task_type_1n_task.""label"" AS ""label"" 
	 FROM rec_task_type task_type_1n_task
	 WHERE task_type_1n_task.id = rec_task.type_id ) d )::jsonb AS ""$task_type_1n_task""	
	-------< $task_type_1n_task

FROM rec_task
LEFT OUTER JOIN  rel_project_nn_task project_nn_task_target ON project_nn_task_target.target_id = rec_task.id
LEFT OUTER JOIN  rec_project project_nn_task_tar_org ON project_nn_task_target.origin_id = project_nn_task_tar_org.id
WHERE  ( project_nn_task_tar_org.""id"" = @projectId ) 
ORDER BY rec_task.""id"" ASC
LIMIT 10
OFFSET 0
) X
";
	var parametersJson = @"[{""name"":""projectId"",""type"":""guid"",""value"":""00000000-0000-0000-0000-000000000000"",""ignore_parse_errors"":false},{""name"":""sortBy"",""type"":""text"",""value"":""id"",""ignore_parse_errors"":false},{""name"":""sortOrder"",""type"":""text"",""value"":""asc"",""ignore_parse_errors"":false},{""name"":""page"",""type"":""int"",""value"":""1"",""ignore_parse_errors"":false},{""name"":""pageSize"",""type"":""int"",""value"":""10"",""ignore_parse_errors"":false}]";
	var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":8,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""completed_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""number"",""type"":1,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""parent_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""status_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""key"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""estimated_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_billable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_nonbillable_minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""priority"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""timelog_started_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""owner_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""type_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""start_time"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""end_time"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""recurrence_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""reserve_time"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""recurrence_template"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_search"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$milestone_nn_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""name"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_status_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]},{""name"":""$task_type_1n_task"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""label"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
	var weight = 10;
	var entityName =  @"task";

	new WebVella.Erp.Database.DbDataSourceRepository().Update(id,name,description,weight,eqlText,sqlText,parametersJson,fieldsJson,entityName);
}
#endregion

#region << ***Update data source*** Name: WvProjectTimeLogsForRecordId >>
{
	var id = new Guid("e66b8374-82ea-4305-8456-085b3a1f1f2d");
	var name = @"WvProjectTimeLogsForRecordId";
	var description = @"Get all time logs for a record";
	var eqlText = @"SELECT *,$user_1n_timelog.image,$user_1n_timelog.username
FROM timelog
WHERE l_related_records CONTAINS @recordId 
ORDER BY @sortBy @sortOrder
PAGE @page
PAGESIZE @pageSize";
	var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT DISTINCT 
	 rec_timelog.""id"" AS ""id"",
	 rec_timelog.""body"" AS ""body"",
	 rec_timelog.""created_by"" AS ""created_by"",
	 rec_timelog.""created_on"" AS ""created_on"",
	 rec_timelog.""is_billable"" AS ""is_billable"",
	 rec_timelog.""logged_on"" AS ""logged_on"",
	 rec_timelog.""minutes"" AS ""minutes"",
	 rec_timelog.""l_scope"" AS ""l_scope"",
	 rec_timelog.""l_related_records"" AS ""l_related_records"",
	 COUNT(*) OVER() AS ___total_count___,
	------->: $user_1n_timelog
	(SELECT  COALESCE( array_to_json( array_agg( row_to_json(d) )), '[]') FROM ( 
	 SELECT 
		 user_1n_timelog.""id"" AS ""id"",
		 user_1n_timelog.""image"" AS ""image"",
		 user_1n_timelog.""username"" AS ""username"" 
	 FROM rec_user user_1n_timelog
	 WHERE user_1n_timelog.id = rec_timelog.created_by ) d )::jsonb AS ""$user_1n_timelog""	
	-------< $user_1n_timelog

FROM rec_timelog
WHERE  ( rec_timelog.""l_related_records""  ILIKE  @recordId ) 
ORDER BY rec_timelog.""created_on"" DESC
LIMIT 10
OFFSET 0
) X
";
	var parametersJson = @"[{""name"":""sortBy"",""type"":""text"",""value"":""created_on"",""ignore_parse_errors"":false},{""name"":""sortOrder"",""type"":""text"",""value"":""desc"",""ignore_parse_errors"":false},{""name"":""page"",""type"":""int"",""value"":""1"",""ignore_parse_errors"":false},{""name"":""pageSize"",""type"":""int"",""value"":""10"",""ignore_parse_errors"":false},{""name"":""recordId"",""type"":""text"",""value"":""string.empty"",""ignore_parse_errors"":false}]";
	var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""body"",""type"":10,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_by"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""is_billable"",""type"":2,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""logged_on"",""type"":4,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""minutes"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_scope"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""l_related_records"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""$user_1n_timelog"",""type"":20,""entity_name"":"""",""relation_name"":null,""children"":[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""image"",""type"":9,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""username"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]}]";
	var weight = 10;
	var entityName =  @"timelog";

	new WebVella.Erp.Database.DbDataSourceRepository().Update(id,name,description,weight,eqlText,sqlText,parametersJson,fieldsJson,entityName);
}
#endregion

#region << ***Update page data source*** Name: AllProjects >>
{
	var id = new Guid("c4bb6351-2fa9-4953-852f-62eb782e839c");
	var pageId = new Guid("68100014-1fd7-456c-9b26-27aa9f858287");
	var dataSourceId = new Guid("96218f33-42f1-4ff1-926c-b1765e1f8c6e");
	var name = @"AllProjects";
	var parameters = @"[{""name"":""onlyActive"",""type"":""bool"",""value"":""true"",""ignore_parse_errors"":false}]";

	new WebVella.Erp.Web.Services.PageService(ErpSettings.ConnectionString).UpdatePageDataSource(id, pageId, dataSourceId,name,parameters,WebVella.Erp.Database.DbContext.Current.Transaction );
}
#endregion




		}
	}
}
