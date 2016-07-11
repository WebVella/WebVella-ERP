using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Api.Models.AutoMapper;
using WebVella.ERP.Utilities;

namespace WebVella.ERP.Project
{
    public partial class Startup
    {
		private static void Patch160627(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan, bool createSampleRecords = false)
		{


#region << Update area: projects >>
{
	var patchObject = new EntityRecord();
	patchObject["id"] = new Guid("205877a1-242c-41bf-a080-49ea01d4f519");
	patchObject["attachments"] = "[{\"name\":null,\"label\":\"My Dashboard\",\"labelPlural\":null,\"iconName\":\"tachometer\",\"weight\":1,\"url\":\"/#/areas/projects/wv_project/dashboard\",\"view\":null,\"create\":null,\"list\":null},{\"name\":\"wv_task\",\"label\":\"Task\",\"labelPlural\":\"Tasks\",\"iconName\":\"tasks\",\"weight\":4,\"url\":null,\"view\":{\"name\":\"general\",\"label\":\"[{code}] {subject}\"},\"create\":{\"name\":\"create\",\"label\":\"Create\"},\"list\":{\"name\":\"my_tasks\",\"label\":\"My Owned Active Tasks\"}},{\"name\":\"wv_bug\",\"label\":\"Bug\",\"labelPlural\":\"Bugs\",\"iconName\":\"bug\",\"weight\":5,\"url\":null,\"view\":{\"name\":\"general\",\"label\":\"[{code}] {subject}\"},\"create\":{\"name\":\"create\",\"label\":\"Create\"},\"list\":{\"name\":\"my_bugs\",\"label\":\"My Owned Open Bugs\"}},{\"name\":\"wv_project\",\"label\":\"Project\",\"labelPlural\":\"Projects\",\"iconName\":\"product-hunt\",\"weight\":22,\"url\":null,\"view\":{\"name\":\"dashboard\",\"label\":\"Dashboard\"},\"create\":{\"name\":\"create\",\"label\":\"Create\"},\"list\":{\"name\":\"my_projects\",\"label\":\"My Projects\"}},{\"name\":null,\"label\":\"My Sprints\",\"url\":\"/#/areas/projects/wv_project/sprints\",\"labelPlural\":null,\"iconName\":\"fast-forward\",\"weight\":\"50\",\"view\":null,\"list\":null,\"create\":null}]";
	var updateAreaResult = recMan.UpdateRecord("area", patchObject);
	if (!updateAreaResult.Success)
	{
		throw new Exception("System error 10060. Area update with name : projects. Message:" + updateAreaResult.Message);
	}
}
#endregion

#region << Update area: project_admin >>
{
	var patchObject = new EntityRecord();
	patchObject["id"] = new Guid("5b131255-46fc-459d-bbb5-923a4bdfc006");
	patchObject["attachments"] = "[{\"name\":\"wv_task\",\"label\":\"Task\",\"labelPlural\":\"Tasks\",\"iconName\":\"tasks\",\"weight\":4,\"url\":null,\"view\":{\"name\":\"general\",\"label\":\"[{code}] {subject}\"},\"create\":{\"name\":\"create\",\"label\":\"Create\"},\"list\":{\"name\":\"admin\",\"label\":\"All tasks\"}},{\"name\":\"wv_bug\",\"label\":\"Bug\",\"labelPlural\":\"Bugs\",\"iconName\":\"bug\",\"weight\":5,\"url\":null,\"view\":{\"name\":\"general\",\"label\":\"[{code}] {subject}\"},\"create\":{\"name\":\"create\",\"label\":\"Create\"},\"list\":{\"name\":\"admin\",\"label\":\"All Bugs\"}},{\"name\":\"wv_project\",\"label\":\"Project\",\"labelPlural\":\"Projects\",\"iconName\":\"product-hunt\",\"weight\":22,\"url\":null,\"view\":{\"name\":\"admin_details\",\"label\":\"Project details\"},\"create\":{\"name\":\"admin_create\",\"label\":\"Project create\"},\"list\":{\"name\":\"admin\",\"label\":\"All Projects\"}},{\"name\":\"wv_sprint\",\"label\":\"Sprint\",\"url\":null,\"labelPlural\":\"Sprints\",\"iconName\":\"fast-forward\",\"weight\":100,\"view\":{\"name\":\"general\",\"label\":\"General\"},\"create\":{\"name\":\"create\",\"label\":\"create\"},\"list\":{\"name\":\"general\",\"label\":\"All Sprints\"}}]";
	var updateAreaResult = recMan.UpdateRecord("area", patchObject);
	if (!updateAreaResult.Success)
	{
		throw new Exception("System error 10060. Area update with name : project_admin. Message:" + updateAreaResult.Message);
	}
}
#endregion

#region << Update  Entity: user View: quick_view >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "quick_view").Id;
	createViewInput.Type = "Quick_View";
	createViewInput.Name = "quick_view";
	createViewInput.Label = "Quick view";
	createViewInput.Title = "";
	createViewInput.Default = true;
	createViewInput.System = false;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "file-text-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("10.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""::ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
		ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
	<i class=""fa fa-trash go-red""></i> Delete Record
</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: user Updated view: quick_view Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: user View: general >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "general").Id;
	createViewInput.Type = "General";
	createViewInput.Name = "general";
	createViewInput.Label = "General";
	createViewInput.Title = "";
	createViewInput.Default = true;
	createViewInput.System = false;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "file-text-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("10.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""::ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
		ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
	<i class=""fa fa-trash go-red""></i> Delete Record
</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: user Updated view: general Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: user name: general >>
{
	var createListEntity = entMan.ReadEntity(new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "general").Id;
	createListInput.Type =  "General";
	createListInput.Name = "general";
	createListInput.Label = "General";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("10.0");
	createListInput.Default = true;
	createListInput.System = false;
	createListInput.CssClass = null;
	createListInput.IconName = "list";
	createListInput.VisibleColumnsCount = Int32.Parse("5");
	createListInput.ColumnWidthsCSV = null;
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
    ng-href=""{{::ngCtrl.getRecordCreateUrl()}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_import_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_import_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("10.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openImportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-upload""></i> Import CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_export_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_export_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("11.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openExportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-download""></i> Export CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record)}}"">
    <i class=""fa fa-fw fa-eye""></i>
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = null;
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: user Updated list: general Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: user name: lookup >>
{
	var createListEntity = entMan.ReadEntity(new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "lookup").Id;
	createListInput.Type =  "Lookup";
	createListInput.Name = "lookup";
	createListInput.Label = "Lookup";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("10.0");
	createListInput.Default = true;
	createListInput.System = false;
	createListInput.CssClass = null;
	createListInput.IconName = "list";
	createListInput.VisibleColumnsCount = Int32.Parse("5");
	createListInput.ColumnWidthsCSV = null;
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
    ng-href=""{{::ngCtrl.getRecordCreateUrl()}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_import_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_import_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("10.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openImportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-upload""></i> Import CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_export_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_export_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("11.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openExportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-download""></i> Export CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record)}}"">
    <i class=""fa fa-fw fa-eye""></i>
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

		#region << username >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listField.EntityName = "user";
			listField.FieldId = new Guid("263c0b21-88c1-4c2b-80b4-db7402b0d2e2");
			listField.FieldName = "username";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << email >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listField.EntityName = "user";
			listField.FieldId = new Guid("9fc75c8f-ce80-4a64-81d7-e2befa5e4815");
			listField.FieldName = "email";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = new InputRecordListQuery();
	var queryDictionary = new Dictionary<Guid,InputRecordListQuery>();
	var subQueryDictionary = new Dictionary<Guid,List<InputRecordListQuery>>();
	//Main query rule
	createListInput.Query.FieldName = null;
	createListInput.Query.FieldValue =  null;
	createListInput.Query.QueryType = "AND";
	createListInput.Query.SubQueries = new List<InputRecordListQuery>();
		{
		queryDictionary[new Guid("f86e5d20-4cbb-4446-9e5f-a0f72cd4a449")] = new InputRecordListQuery();
		queryDictionary[new Guid("f86e5d20-4cbb-4446-9e5f-a0f72cd4a449")].FieldName = "username";
		queryDictionary[new Guid("f86e5d20-4cbb-4446-9e5f-a0f72cd4a449")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"username\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("f86e5d20-4cbb-4446-9e5f-a0f72cd4a449")].QueryType = "CONTAINS";
		queryDictionary[new Guid("f86e5d20-4cbb-4446-9e5f-a0f72cd4a449")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("f86e5d20-4cbb-4446-9e5f-a0f72cd4a449"))) {queryDictionary[new Guid("f86e5d20-4cbb-4446-9e5f-a0f72cd4a449")].SubQueries = subQueryDictionary[new Guid("f86e5d20-4cbb-4446-9e5f-a0f72cd4a449")];}
		if(!subQueryDictionary.ContainsKey(new Guid("b9db7d41-c244-48bb-9412-28a6c107275a"))) {subQueryDictionary[new Guid("b9db7d41-c244-48bb-9412-28a6c107275a")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("b9db7d41-c244-48bb-9412-28a6c107275a")].Add(queryDictionary[new Guid("f86e5d20-4cbb-4446-9e5f-a0f72cd4a449")]);
		}
		{
		queryDictionary[new Guid("565e8178-f947-4bac-95a4-6c85f795722c")] = new InputRecordListQuery();
		queryDictionary[new Guid("565e8178-f947-4bac-95a4-6c85f795722c")].FieldName = "email";
		queryDictionary[new Guid("565e8178-f947-4bac-95a4-6c85f795722c")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"email\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("565e8178-f947-4bac-95a4-6c85f795722c")].QueryType = "CONTAINS";
		queryDictionary[new Guid("565e8178-f947-4bac-95a4-6c85f795722c")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("565e8178-f947-4bac-95a4-6c85f795722c"))) {queryDictionary[new Guid("565e8178-f947-4bac-95a4-6c85f795722c")].SubQueries = subQueryDictionary[new Guid("565e8178-f947-4bac-95a4-6c85f795722c")];}
		if(!subQueryDictionary.ContainsKey(new Guid("b9db7d41-c244-48bb-9412-28a6c107275a"))) {subQueryDictionary[new Guid("b9db7d41-c244-48bb-9412-28a6c107275a")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("b9db7d41-c244-48bb-9412-28a6c107275a")].Add(queryDictionary[new Guid("565e8178-f947-4bac-95a4-6c85f795722c")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("b9db7d41-c244-48bb-9412-28a6c107275a"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("b9db7d41-c244-48bb-9412-28a6c107275a")];}
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

		#region << sort >>
		{
			var sort = new InputRecordListSort();
			sort.FieldName = "username";
			sort.SortType = "Ascending";
			createListInput.Sorts.Add(sort);
		}
		#endregion

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: user Updated list: lookup Message:" + response.Message);
	}
}
#endregion

#region << Create entity: wv_sprint >>
{
	#region << entity >>
	{
		var entity = new InputEntity();
		var systemFieldIdDictionary = new Dictionary<string,Guid>();
		systemFieldIdDictionary["id"] = new Guid("180c7aed-f254-4ccc-b5ad-ddf49c902b1b");
		systemFieldIdDictionary["created_on"] = new Guid("e018e872-6f74-4c65-8c3a-c72cc9d6504f");
		systemFieldIdDictionary["created_by"] = new Guid("d3bd11d4-1366-45bc-806e-033be85fec2f");
		systemFieldIdDictionary["last_modified_on"] = new Guid("a76c3b07-1e00-4943-a4d1-06753c292dad");
		systemFieldIdDictionary["last_modified_by"] = new Guid("080c2d61-fdd2-4e5c-bfe2-5d2734013758");
		systemFieldIdDictionary["user_wv_sprint_created_by"] = new Guid("8c015ea1-1226-4768-95a8-98574443af6c");
		systemFieldIdDictionary["user_wv_sprint_modified_by"] = new Guid("2b63de6e-32a5-4380-ae8b-491f5a72ebc0");
		entity.Id = new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd");
		entity.Name = "wv_sprint";
		entity.Label = "Sprint";
		entity.LabelPlural = "Sprints";
		entity.System = true;
		entity.IconName = "fast-forward";
		entity.Weight = (decimal)100.0;
		entity.RecordPermissions = new RecordPermissions();
		entity.RecordPermissions.CanCreate = new List<Guid>();
		entity.RecordPermissions.CanRead = new List<Guid>();
		entity.RecordPermissions.CanUpdate = new List<Guid>();
		entity.RecordPermissions.CanDelete = new List<Guid>();
		//Create
		entity.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
		entity.RecordPermissions.CanCreate.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
		//READ
		entity.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
		entity.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
		//UPDATE
		entity.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
		entity.RecordPermissions.CanUpdate.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
		//DELETE
		{
			var response = entMan.CreateEntity(entity, false, false,systemFieldIdDictionary);
			if (!response.Success)
				throw new Exception("System error 10050. Entity: wv_sprint creation Message: " + response.Message);
		}
	}
	#endregion
}
#endregion

#region << Create  Entity: wv_sprint field: name >>
{
	InputTextField textboxField = new InputTextField();
	textboxField.Id = new Guid("a313b778-392d-4214-8c1a-e2a591b8a9dd");
	textboxField.Name = "name";
	textboxField.Label = "Name";
	textboxField.PlaceholderText = "";
	textboxField.Description = "";
	textboxField.HelpText = "";
	textboxField.Required = true;
	textboxField.Unique = false;
	textboxField.Searchable = true;
	textboxField.Auditable = false;
	textboxField.System = true;
	textboxField.DefaultValue = "Sprint name";
	textboxField.MaxLength = null;
	textboxField.EnableSecurity = false;
	textboxField.Permissions = new FieldPermissions();
	textboxField.Permissions.CanRead = new List<Guid>();
	textboxField.Permissions.CanUpdate = new List<Guid>();
	//READ
	//UPDATE
	{
		var response = entMan.CreateField(new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd"), textboxField, false);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_sprint Field: name Message:" + response.Message);
	}
}
#endregion

#region << Create  Entity: wv_sprint field: end_date >>
{
	InputDateField dateField = new InputDateField();
	dateField.Id =  new Guid("63bd86c4-adac-4de1-a24a-5cb4a3a0a73f");
	dateField.Name = "end_date";
	dateField.Label = "End date";
	dateField.PlaceholderText = "";
	dateField.Description = "";
	dateField.HelpText = "";
	dateField.Required = true;
	dateField.Unique = false;
	dateField.Searchable = true;
	dateField.Auditable = false;
	dateField.System = true;
	dateField.DefaultValue = null;
	dateField.Format = "yyyy-MMM-dd";
	dateField.UseCurrentTimeAsDefaultValue = true;
	dateField.EnableSecurity = false;
	dateField.Permissions = new FieldPermissions();
	dateField.Permissions.CanRead = new List<Guid>();
	dateField.Permissions.CanUpdate = new List<Guid>();
	//READ
	//UPDATE
	{
		var response = entMan.CreateField(new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd"), dateField, false);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_sprint Field: end_date Message:" + response.Message);
	}
}
#endregion

#region << Create  Entity: wv_sprint field: start_date >>
{
	InputDateField dateField = new InputDateField();
	dateField.Id =  new Guid("0d2f3748-5ae8-40ab-bba2-fe5a6f249568");
	dateField.Name = "start_date";
	dateField.Label = "Start date";
	dateField.PlaceholderText = "";
	dateField.Description = "";
	dateField.HelpText = "";
	dateField.Required = true;
	dateField.Unique = false;
	dateField.Searchable = true;
	dateField.Auditable = false;
	dateField.System = true;
	dateField.DefaultValue = null;
	dateField.Format = "yyyy-MMM-dd";
	dateField.UseCurrentTimeAsDefaultValue = true;
	dateField.EnableSecurity = false;
	dateField.Permissions = new FieldPermissions();
	dateField.Permissions.CanRead = new List<Guid>();
	dateField.Permissions.CanUpdate = new List<Guid>();
	//READ
	//UPDATE
	{
		var response = entMan.CreateField(new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd"), dateField, false);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_sprint Field: start_date Message:" + response.Message);
	}
}
#endregion

#region << View  Entity: wv_sprint name: general >>
{
	var createViewEntity = entMan.ReadEntity(new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = new Guid("fdf4260b-c4aa-4197-82e3-9ddeadc04161");
	createViewInput.Type = "General";
	createViewInput.Name = "general";
	createViewInput.Label = "General";
	createViewInput.Title = "{name}";
	createViewInput.Default = true;
	createViewInput.System = false;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "file-text-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("1.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			#region << Section: section >>
			{
			var viewSection = new InputRecordViewSection();
			viewSection.Id = new Guid("f1aa9f94-1c67-4a38-bab6-6c3e9a2d9834");
			viewSection.Name = "section";
			viewSection.Label = "Section name";
			viewSection.ShowLabel = false;
			viewSection.CssClass = "";
			viewSection.Collapsed = false;
			viewSection.TabOrder = "left-right";
			viewSection.Weight = Decimal.Parse("1.0");
			viewSection.Rows = new List<InputRecordViewRow>();

				#region << Row 1>>
				{
					var viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("9a1a6e16-9dbc-4690-9e29-52627f099e33");
					viewRow.Weight = Decimal.Parse("1.0");
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("12");
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << name >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd");
						viewItem.EntityName = "wv_sprint";
						viewItem.FieldId = new Guid("a313b778-392d-4214-8c1a-e2a591b8a9dd");
						viewItem.FieldName = "name";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					//Save column
					viewRow.Columns.Add(viewColumn);
					}
					#endregion
				//Save row
				viewSection.Rows.Add(viewRow);
				}
				#endregion
				#region << Row 2>>
				{
					var viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("5477e48f-1925-4881-98f9-bbc2c0e961fb");
					viewRow.Weight = Decimal.Parse("2.0");
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("6");
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << start_date >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd");
						viewItem.EntityName = "wv_sprint";
						viewItem.FieldId = new Guid("0d2f3748-5ae8-40ab-bba2-fe5a6f249568");
						viewItem.FieldName = "start_date";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					//Save column
					viewRow.Columns.Add(viewColumn);
					}
					#endregion
					#region << Column 2 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("6");
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << end_date >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd");
						viewItem.EntityName = "wv_sprint";
						viewItem.FieldId = new Guid("63bd86c4-adac-4de1-a24a-5cb4a3a0a73f");
						viewItem.FieldName = "end_date";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					//Save column
					viewRow.Columns.Add(viewColumn);
					}
					#endregion
				//Save row
				viewSection.Rows.Add(viewRow);
				}
				#endregion
			//Save section
			viewRegion.Sections.Add(viewSection);
			}
			#endregion
			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""{{'::DELETE_CONFIRMATION_ALERT_MESSAGE' | translate}}"" 
		 ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')""> 
	 <i class=""fa fa-trash go-red""></i> Delete Record 
 </a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.CreateRecordView(new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_sprint Updated view: general Message:" + response.Message);
	}
}
#endregion

#region << View  Entity: wv_sprint name: create >>
{
	var createViewEntity = entMan.ReadEntity(new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = new Guid("c5c3d6fb-f630-4257-bf84-d25af7fa6a50");
	createViewInput.Type = "Create";
	createViewInput.Name = "create";
	createViewInput.Label = "create";
	createViewInput.Title = "Create sprint";
	createViewInput.Default = true;
	createViewInput.System = false;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "file-text-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("1.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			#region << Section: section >>
			{
			var viewSection = new InputRecordViewSection();
			viewSection.Id = new Guid("f1aa9f94-1c67-4a38-bab6-6c3e9a2d9834");
			viewSection.Name = "section";
			viewSection.Label = "Section name";
			viewSection.ShowLabel = false;
			viewSection.CssClass = "";
			viewSection.Collapsed = false;
			viewSection.TabOrder = "left-right";
			viewSection.Weight = Decimal.Parse("1.0");
			viewSection.Rows = new List<InputRecordViewRow>();

				#region << Row 1>>
				{
					var viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("9a1a6e16-9dbc-4690-9e29-52627f099e33");
					viewRow.Weight = Decimal.Parse("1.0");
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("12");
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << name >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd");
						viewItem.EntityName = "wv_sprint";
						viewItem.FieldId = new Guid("a313b778-392d-4214-8c1a-e2a591b8a9dd");
						viewItem.FieldName = "name";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					//Save column
					viewRow.Columns.Add(viewColumn);
					}
					#endregion
				//Save row
				viewSection.Rows.Add(viewRow);
				}
				#endregion
				#region << Row 2>>
				{
					var viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("5477e48f-1925-4881-98f9-bbc2c0e961fb");
					viewRow.Weight = Decimal.Parse("2.0");
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("6");
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << start_date >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd");
						viewItem.EntityName = "wv_sprint";
						viewItem.FieldId = new Guid("0d2f3748-5ae8-40ab-bba2-fe5a6f249568");
						viewItem.FieldName = "start_date";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					//Save column
					viewRow.Columns.Add(viewColumn);
					}
					#endregion
					#region << Column 2 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("6");
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << end_date >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd");
						viewItem.EntityName = "wv_sprint";
						viewItem.FieldId = new Guid("63bd86c4-adac-4de1-a24a-5cb4a3a0a73f");
						viewItem.FieldName = "end_date";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					//Save column
					viewRow.Columns.Add(viewColumn);
					}
					#endregion
				//Save row
				viewSection.Rows.Add(viewRow);
				}
				#endregion
			//Save section
			viewRegion.Sections.Add(viewSection);
			}
			#endregion
			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_create_and_list >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_and_list";
			actionItem.Menu = "create-bottom";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-primary"" ng-click='ngCtrl.create(""default"")' ng-if=""::ngCtrl.createViewRegion != null"">Create</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_create_cancel >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_cancel";
			actionItem.Menu = "create-bottom";
			actionItem.Weight = Decimal.Parse("3.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-click=""ngCtrl.cancel()"">Cancel</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.CreateRecordView(new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_sprint Updated view: create Message:" + response.Message);
	}
}
#endregion

#region << List  Entity: wv_sprint name: general >>
{
	var createListEntity = entMan.ReadEntity(new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = new Guid("0a6f3f3e-37e4-428e-a2c8-d381101de660");
	createListInput.Type =  "General";
	createListInput.Name = "general";
	createListInput.Label = "All Sprints";
	createListInput.Title = "Available sprints";
	createListInput.Weight = Decimal.Parse("1.0");
	createListInput.Default = true;
	createListInput.System = false;
	createListInput.CssClass = "";
	createListInput.IconName = string.IsNullOrEmpty("") ? string.Empty : "";
	createListInput.VisibleColumnsCount = Int32.Parse("7");
	createListInput.ColumnWidthsCSV = "auto,120px,120px";
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')""
 ng-href=""{{ngCtrl.getRecordCreateUrl(ngCtrl)}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_import_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_import_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("10.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openImportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-upload""></i> Import CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_export_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_export_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("11.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openExportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-download""></i> Export CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record, ngCtrl)}}"">
	<i class=""fa fa-fw fa-eye""></i>
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

		#region << name >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd");
			listField.EntityName = "wv_sprint";
			listField.FieldId = new Guid("a313b778-392d-4214-8c1a-e2a591b8a9dd");
			listField.FieldName = "name";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << start_date >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd");
			listField.EntityName = "wv_sprint";
			listField.FieldId = new Guid("0d2f3748-5ae8-40ab-bba2-fe5a6f249568");
			listField.FieldName = "start_date";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << end_date >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd");
			listField.EntityName = "wv_sprint";
			listField.FieldId = new Guid("63bd86c4-adac-4de1-a24a-5cb4a3a0a73f");
			listField.FieldName = "end_date";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = null;
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();
	var listSort = new InputRecordListSort();
	listSort = new InputRecordListSort();
	listSort.FieldName = "created_on";
	listSort.SortType = "descending";
	createListInput.Sorts.Add(listSort);

	}
	#endregion

	{
		var response = entMan.CreateRecordList(new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_sprint Created list: general Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: role View: quick_view >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("c4541fee-fbb6-4661-929e-1724adec285a")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "quick_view").Id;
	createViewInput.Type = "Quick_View";
	createViewInput.Name = "quick_view";
	createViewInput.Label = "Quick view";
	createViewInput.Title = "";
	createViewInput.Default = true;
	createViewInput.System = false;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "file-text-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("10.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""::ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
		ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
	<i class=""fa fa-trash go-red""></i> Delete Record
</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("c4541fee-fbb6-4661-929e-1724adec285a"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: role Updated view: quick_view Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: role View: general >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("c4541fee-fbb6-4661-929e-1724adec285a")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "general").Id;
	createViewInput.Type = "General";
	createViewInput.Name = "general";
	createViewInput.Label = "General";
	createViewInput.Title = "";
	createViewInput.Default = true;
	createViewInput.System = false;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "file-text-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("10.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""::ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
		ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
	<i class=""fa fa-trash go-red""></i> Delete Record
</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("c4541fee-fbb6-4661-929e-1724adec285a"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: role Updated view: general Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: role name: general >>
{
	var createListEntity = entMan.ReadEntity(new Guid("c4541fee-fbb6-4661-929e-1724adec285a")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "general").Id;
	createListInput.Type =  "General";
	createListInput.Name = "general";
	createListInput.Label = "General";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("10.0");
	createListInput.Default = true;
	createListInput.System = false;
	createListInput.CssClass = null;
	createListInput.IconName = "list";
	createListInput.VisibleColumnsCount = Int32.Parse("5");
	createListInput.ColumnWidthsCSV = null;
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
    ng-href=""{{::ngCtrl.getRecordCreateUrl()}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_import_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_import_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("10.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openImportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-upload""></i> Import CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_export_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_export_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("11.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openExportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-download""></i> Export CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record)}}"">
    <i class=""fa fa-fw fa-eye""></i>
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = null;
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("c4541fee-fbb6-4661-929e-1724adec285a"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: role Updated list: general Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: role name: lookup >>
{
	var createListEntity = entMan.ReadEntity(new Guid("c4541fee-fbb6-4661-929e-1724adec285a")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "lookup").Id;
	createListInput.Type =  "Lookup";
	createListInput.Name = "lookup";
	createListInput.Label = "Lookup";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("10.0");
	createListInput.Default = true;
	createListInput.System = false;
	createListInput.CssClass = null;
	createListInput.IconName = "list";
	createListInput.VisibleColumnsCount = Int32.Parse("5");
	createListInput.ColumnWidthsCSV = null;
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
    ng-href=""{{::ngCtrl.getRecordCreateUrl()}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_import_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_import_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("10.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openImportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-upload""></i> Import CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_export_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_export_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("11.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openExportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-download""></i> Export CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record)}}"">
    <i class=""fa fa-fw fa-eye""></i>
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

		#region << name >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("c4541fee-fbb6-4661-929e-1724adec285a");
			listField.EntityName = "role";
			listField.FieldId = new Guid("36f91ebd-5a02-4032-8498-b7f716f6a349");
			listField.FieldName = "name";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = null;
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

		#region << sort >>
		{
			var sort = new InputRecordListSort();
			sort.FieldName = "name";
			sort.SortType = "Ascending";
			createListInput.Sorts.Add(sort);
		}
		#endregion

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("c4541fee-fbb6-4661-929e-1724adec285a"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: role Updated list: lookup Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: area View: general >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("cb434298-8583-4a96-bdbb-97b2c1764192")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "general").Id;
	createViewInput.Type = "General";
	createViewInput.Name = "general";
	createViewInput.Label = "General";
	createViewInput.Title = "General";
	createViewInput.Default = true;
	createViewInput.System = false;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "file-text-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("10.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""::ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
		ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
	<i class=""fa fa-trash go-red""></i> Delete Record
</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("cb434298-8583-4a96-bdbb-97b2c1764192"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: area Updated view: general Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: area View: quick_view >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("cb434298-8583-4a96-bdbb-97b2c1764192")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "quick_view").Id;
	createViewInput.Type = "Quick_View";
	createViewInput.Name = "quick_view";
	createViewInput.Label = "Quick view";
	createViewInput.Title = "Quick view";
	createViewInput.Default = true;
	createViewInput.System = false;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "file-text-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("10.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""::ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
		ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
	<i class=""fa fa-trash go-red""></i> Delete Record
</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("cb434298-8583-4a96-bdbb-97b2c1764192"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: area Updated view: quick_view Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: area name: general >>
{
	var createListEntity = entMan.ReadEntity(new Guid("cb434298-8583-4a96-bdbb-97b2c1764192")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "general").Id;
	createListInput.Type =  "General";
	createListInput.Name = "general";
	createListInput.Label = "General";
	createListInput.Title = "General";
	createListInput.Weight = Decimal.Parse("10.0");
	createListInput.Default = true;
	createListInput.System = false;
	createListInput.CssClass = null;
	createListInput.IconName = "list";
	createListInput.VisibleColumnsCount = Int32.Parse("5");
	createListInput.ColumnWidthsCSV = null;
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
    ng-href=""{{::ngCtrl.getRecordCreateUrl()}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_import_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_import_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("10.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openImportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-upload""></i> Import CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_export_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_export_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("11.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openExportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-download""></i> Export CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record)}}"">
    <i class=""fa fa-fw fa-eye""></i>
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = null;
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("cb434298-8583-4a96-bdbb-97b2c1764192"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: area Updated list: general Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: area name: lookup >>
{
	var createListEntity = entMan.ReadEntity(new Guid("cb434298-8583-4a96-bdbb-97b2c1764192")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "lookup").Id;
	createListInput.Type =  "Lookup";
	createListInput.Name = "lookup";
	createListInput.Label = "Lookup";
	createListInput.Title = "Lookup";
	createListInput.Weight = Decimal.Parse("10.0");
	createListInput.Default = true;
	createListInput.System = false;
	createListInput.CssClass = null;
	createListInput.IconName = "list";
	createListInput.VisibleColumnsCount = Int32.Parse("5");
	createListInput.ColumnWidthsCSV = null;
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_import_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_import_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("10.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openImportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-upload""></i> Import CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_export_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_export_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("11.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openExportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-download""></i> Export CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record)}}"">
    <i class=""fa fa-fw fa-eye""></i>
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
ng-href=""{{::ngCtrl.getRecordCreateUrl()}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = null;
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("cb434298-8583-4a96-bdbb-97b2c1764192"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: area Updated list: lookup Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_customer View: quick_view >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("90bcdb47-2cde-4137-a412-0198348fecc0")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "quick_view").Id;
	createViewInput.Type = "Quick_View";
	createViewInput.Name = "quick_view";
	createViewInput.Label = "Quick view";
	createViewInput.Title = "";
	createViewInput.Default = true;
	createViewInput.System = false;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "file-text-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("10.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""::ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
		ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
	<i class=""fa fa-trash go-red""></i> Delete Record
</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("90bcdb47-2cde-4137-a412-0198348fecc0"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_customer Updated view: quick_view Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_customer View: general >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("90bcdb47-2cde-4137-a412-0198348fecc0")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "general").Id;
	createViewInput.Type = "General";
	createViewInput.Name = "general";
	createViewInput.Label = "General";
	createViewInput.Title = "";
	createViewInput.Default = true;
	createViewInput.System = false;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "file-text-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("10.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""::ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
		ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
	<i class=""fa fa-trash go-red""></i> Delete Record
</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("90bcdb47-2cde-4137-a412-0198348fecc0"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_customer Updated view: general Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_customer View: admin_details >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("90bcdb47-2cde-4137-a412-0198348fecc0")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "admin_details").Id;
	createViewInput.Type = "Hidden";
	createViewInput.Name = "admin_details";
	createViewInput.Label = "Details";
	createViewInput.Title = "";
	createViewInput.Default = false;
	createViewInput.System = true;
	createViewInput.Weight = Decimal.Parse("15.0");
	createViewInput.CssClass = null;
	createViewInput.IconName = "building-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("1.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			#region << Section: details >>
			{
			var viewSection = new InputRecordViewSection();
			viewSection.Id = new Guid("201882a6-c1f8-4f84-878d-fddfacdcb2d7");
			viewSection.Name = "details";
			viewSection.Label = "Details";
			viewSection.ShowLabel = false;
			viewSection.CssClass = "";
			viewSection.Collapsed = false;
			viewSection.TabOrder = "left-right";
			viewSection.Weight = Decimal.Parse("1.0");
			viewSection.Rows = new List<InputRecordViewRow>();

				#region << Row 1>>
				{
					var viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("ce09fdad-bb23-4f7f-b64b-190fec656711");
					viewRow.Weight = Decimal.Parse("1.0");
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("12");
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << name >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("90bcdb47-2cde-4137-a412-0198348fecc0");
						viewItem.EntityName = "wv_customer";
						viewItem.FieldId = new Guid("7fb95d0f-ab59-421d-974d-ab357e28a1f9");
						viewItem.FieldName = "name";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					//Save column
					viewRow.Columns.Add(viewColumn);
					}
					#endregion
				//Save row
				viewSection.Rows.Add(viewRow);
				}
				#endregion
			//Save section
			viewRegion.Sections.Add(viewSection);
			}
			#endregion
			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
										ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
									<i class=""fa fa-trash go-red""></i> Delete Record
								</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = true;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("90bcdb47-2cde-4137-a412-0198348fecc0"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_customer Updated view: admin_details Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_customer name: general >>
{
	var createListEntity = entMan.ReadEntity(new Guid("90bcdb47-2cde-4137-a412-0198348fecc0")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "general").Id;
	createListInput.Type =  "General";
	createListInput.Name = "general";
	createListInput.Label = "General";
	createListInput.Title = null;
	createListInput.Weight = Decimal.Parse("10.0");
	createListInput.Default = true;
	createListInput.System = false;
	createListInput.CssClass = null;
	createListInput.IconName = "list";
	createListInput.VisibleColumnsCount = Int32.Parse("5");
	createListInput.ColumnWidthsCSV = null;
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
    ng-href=""{{::ngCtrl.getRecordCreateUrl()}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_import_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_import_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("10.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openImportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-upload""></i> Import CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_export_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_export_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("11.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openExportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-download""></i> Export CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record)}}"">
    <i class=""fa fa-fw fa-eye""></i>
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = null;
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("90bcdb47-2cde-4137-a412-0198348fecc0"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_customer Updated list: general Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_customer name: admin >>
{
	var createListEntity = entMan.ReadEntity(new Guid("90bcdb47-2cde-4137-a412-0198348fecc0")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "admin").Id;
	createListInput.Type =  "Hidden";
	createListInput.Name = "admin";
	createListInput.Label = "Customers";
	createListInput.Title = null;
	createListInput.Weight = Decimal.Parse("11.0");
	createListInput.Default = false;
	createListInput.System = true;
	createListInput.CssClass = null;
	createListInput.IconName = "building-o";
	createListInput.VisibleColumnsCount = Int32.Parse("7");
	createListInput.ColumnWidthsCSV = null;
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record, ngCtrl)}}"">
							<i class=""fa fa-fw fa-eye""></i>
							</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
    ng-href=""{{::ngCtrl.getRecordCreateUrl(ngCtrl)}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

		#region << name >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("90bcdb47-2cde-4137-a412-0198348fecc0");
			listField.EntityName = "wv_customer";
			listField.FieldId = new Guid("7fb95d0f-ab59-421d-974d-ab357e28a1f9");
			listField.FieldName = "name";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = null;
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("90bcdb47-2cde-4137-a412-0198348fecc0"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_customer Updated list: admin Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_project View: admin_details >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "admin_details").Id;
	createViewInput.Type = "Hidden";
	createViewInput.Name = "admin_details";
	createViewInput.Label = "Project details";
	createViewInput.Title = "";
	createViewInput.Default = false;
	createViewInput.System = true;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = null;
	createViewInput.IconName = "product-hunt";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("1.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			#region << Section: details >>
			{
			var viewSection = new InputRecordViewSection();
			viewSection.Id = new Guid("6f8fdcc2-8a8f-4cf6-bf26-47bf1c6f0438");
			viewSection.Name = "details";
			viewSection.Label = "Details";
			viewSection.ShowLabel = false;
			viewSection.CssClass = "";
			viewSection.Collapsed = false;
			viewSection.TabOrder = "left-right";
			viewSection.Weight = Decimal.Parse("1.0");
			viewSection.Rows = new List<InputRecordViewRow>();

				#region << Row 1>>
				{
					var viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("5d7ce054-f592-41e7-b8f1-424cfd6d38b7");
					viewRow.Weight = Decimal.Parse("1.0");
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("12");
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << name >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32");
						viewItem.EntityName = "wv_project";
						viewItem.FieldId = new Guid("d13427b7-e518-4305-b2cc-bc814a299b55");
						viewItem.FieldName = "name";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					#region << description >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32");
						viewItem.EntityName = "wv_project";
						viewItem.FieldId = new Guid("d5e2c42c-c0b8-4f03-92e0-e91bede1e7b3");
						viewItem.FieldName = "description";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					//Save column
					viewRow.Columns.Add(viewColumn);
					}
					#endregion
				//Save row
				viewSection.Rows.Add(viewRow);
				}
				#endregion
				#region << Row 2>>
				{
					var viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("22d11cfc-a763-472b-b509-7ddfe36476bb");
					viewRow.Weight = Decimal.Parse("1.0");
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("6");
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << field from Relation: username >>
					{
						var viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
						viewItemFromRelation.EntityName = "user";
						viewItemFromRelation.Type = "fieldFromRelation";
						viewItemFromRelation.FieldId = new Guid("263c0b21-88c1-4c2b-80b4-db7402b0d2e2");
						viewItemFromRelation.FieldName = "username";
						viewItemFromRelation.FieldLabel = "Owner / Project manager";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = null;
						viewItemFromRelation.FieldRequired = true;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = new Guid("0cad07c3-73bd-4c1f-a5d6-552256f679a4");
						viewItemFromRelation.RelationName = "user_1_n_project_owner";
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion
					#region << field from Relation: name >>
					{
						var viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = new Guid("c4541fee-fbb6-4661-929e-1724adec285a");
						viewItemFromRelation.EntityName = "role";
						viewItemFromRelation.Type = "fieldFromRelation";
						viewItemFromRelation.FieldId = new Guid("36f91ebd-5a02-4032-8498-b7f716f6a349");
						viewItemFromRelation.FieldName = "name";
						viewItemFromRelation.FieldLabel = "Project team roles";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = null;
						viewItemFromRelation.FieldRequired = true;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = new Guid("4860a4b6-d07e-416f-b548-60491607e93f");
						viewItemFromRelation.RelationName = "role_n_n_project_team";
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion
					#region << start_date >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32");
						viewItem.EntityName = "wv_project";
						viewItem.FieldId = new Guid("417ce7d7-a472-499b-8e70-43a1cb54723d");
						viewItem.FieldName = "start_date";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					#region << billable_hour_price >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32");
						viewItem.EntityName = "wv_project";
						viewItem.FieldId = new Guid("7179f4ab-0376-4ded-a334-a21ff451538e");
						viewItem.FieldName = "billable_hour_price";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					//Save column
					viewRow.Columns.Add(viewColumn);
					}
					#endregion
					#region << Column 2 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("6");
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << field from Relation: name >>
					{
						var viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = new Guid("90bcdb47-2cde-4137-a412-0198348fecc0");
						viewItemFromRelation.EntityName = "wv_customer";
						viewItemFromRelation.Type = "fieldFromRelation";
						viewItemFromRelation.FieldId = new Guid("7fb95d0f-ab59-421d-974d-ab357e28a1f9");
						viewItemFromRelation.FieldName = "name";
						viewItemFromRelation.FieldLabel = "Customer";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = null;
						viewItemFromRelation.FieldRequired = true;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = new Guid("d7f1ec35-9f59-4d75-b8a2-554c7eaeab11");
						viewItemFromRelation.RelationName = "customer_1_n_project";
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion
					#region << field from Relation: name >>
					{
						var viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = new Guid("c4541fee-fbb6-4661-929e-1724adec285a");
						viewItemFromRelation.EntityName = "role";
						viewItemFromRelation.Type = "fieldFromRelation";
						viewItemFromRelation.FieldId = new Guid("36f91ebd-5a02-4032-8498-b7f716f6a349");
						viewItemFromRelation.FieldName = "name";
						viewItemFromRelation.FieldLabel = "Project customer roles";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = null;
						viewItemFromRelation.FieldRequired = true;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = new Guid("e6d75feb-3c8f-410b-9ff4-54ef8489dc2f");
						viewItemFromRelation.RelationName = "role_n_n_project_customer";
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion
					#region << end_date >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32");
						viewItem.EntityName = "wv_project";
						viewItem.FieldId = new Guid("57198778-636d-47ec-b33e-edfc5705cc05");
						viewItem.FieldName = "end_date";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					#region << code >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32");
						viewItem.EntityName = "wv_project";
						viewItem.FieldId = new Guid("d9c6a939-e2e3-4617-900e-e056f0638fa8");
						viewItem.FieldName = "code";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					//Save column
					viewRow.Columns.Add(viewColumn);
					}
					#endregion
				//Save row
				viewSection.Rows.Add(viewRow);
				}
				#endregion
			//Save section
			viewRegion.Sections.Add(viewSection);
			}
			#endregion
			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
										ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
									<i class=""fa fa-trash go-red""></i> Delete Record
								</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = true;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_project Updated view: admin_details Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_project View: dashboard >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "dashboard").Id;
	createViewInput.Type = "General";
	createViewInput.Name = "dashboard";
	createViewInput.Label = "Dashboard";
	createViewInput.Title = "[{code}] {name}";
	createViewInput.Default = false;
	createViewInput.System = true;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = null;
	createViewInput.IconName = "tachometer";
	createViewInput.DynamicHtmlTemplate = "/plugins/webvella-projects/templates/project-dashboard.html";
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("1.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
										ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
									<i class=""fa fa-trash go-red""></i> Delete Record
								</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = true;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

		#region << list from relation: project_tasks >>
		{
			var viewItemFromRelation = new InputRecordViewSidebarRelationListItem();
			viewItemFromRelation.EntityId = new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32");
			viewItemFromRelation.EntityName = "wv_project";
			viewItemFromRelation.ListId = new Guid("44f8ed83-b7e8-4223-b02e-b5e35ed4bcc1");
			viewItemFromRelation.ListName ="project_tasks";
						viewItemFromRelation.FieldLabel = "Tasks";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = "";
			viewItemFromRelation.FieldRequired = false;
			viewItemFromRelation.FieldManageView = "general";
			viewItemFromRelation.FieldLookupList = "lookup";
			viewItemFromRelation.RelationId = new Guid("1f860b8c-7fa1-40fa-874f-19c2b5309817");
			viewItemFromRelation.RelationName = "project_1_n_task";
			viewItemFromRelation.Type = "listFromRelation";
			createViewInput.Sidebar.Items.Add(viewItemFromRelation);
		}
		#endregion

		#region << list from relation: project_milestones >>
		{
			var viewItemFromRelation = new InputRecordViewSidebarRelationListItem();
			viewItemFromRelation.EntityId = new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32");
			viewItemFromRelation.EntityName = "wv_project";
			viewItemFromRelation.ListId = new Guid("92b40989-c3a2-4a06-869a-789fba54e733");
			viewItemFromRelation.ListName ="project_milestones";
						viewItemFromRelation.FieldLabel = "Milestones";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = "";
			viewItemFromRelation.FieldRequired = false;
			viewItemFromRelation.FieldManageView = "general";
			viewItemFromRelation.FieldLookupList = "lookup";
			viewItemFromRelation.RelationId = new Guid("0c446f98-eec2-40c1-9d66-8a3c2a2498e9");
			viewItemFromRelation.RelationName = "project_1_n_milestone";
			viewItemFromRelation.Type = "listFromRelation";
			createViewInput.Sidebar.Items.Add(viewItemFromRelation);
		}
		#endregion

		#region << list from relation: project_bugs >>
		{
			var viewItemFromRelation = new InputRecordViewSidebarRelationListItem();
			viewItemFromRelation.EntityId = new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32");
			viewItemFromRelation.EntityName = "wv_project";
			viewItemFromRelation.ListId = new Guid("3b2ebe34-1d02-448a-9616-5b62538fe2c7");
			viewItemFromRelation.ListName ="project_bugs";
						viewItemFromRelation.FieldLabel = "Bugs";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = "";
			viewItemFromRelation.FieldRequired = false;
			viewItemFromRelation.FieldManageView = "general";
			viewItemFromRelation.FieldLookupList = "lookup";
			viewItemFromRelation.RelationId = new Guid("d94f100c-024c-47e7-af32-d67a49be2b6c");
			viewItemFromRelation.RelationName = "project_1_n_bug";
			viewItemFromRelation.Type = "listFromRelation";
			createViewInput.Sidebar.Items.Add(viewItemFromRelation);
		}
		#endregion

		#region << View: general >>
		{
			var viewItem = new InputRecordViewSidebarViewItem();
			viewItem.EntityId = new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32");
			viewItem.EntityName = "wv_project";
			viewItem.ViewId = new Guid("211f028b-4e8e-418f-9c0e-78109f0839fc");
			viewItem.ViewName = "general";
			viewItem.Type = "view";
			createViewInput.Sidebar.Items.Add(viewItem);
		}
		#endregion

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_project Updated view: dashboard Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_project View: general >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "general").Id;
	createViewInput.Type = "Hidden";
	createViewInput.Name = "general";
	createViewInput.Label = "Details";
	createViewInput.Title = "{name}";
	createViewInput.Default = true;
	createViewInput.System = true;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "file-text-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("10.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			#region << Section: details >>
			{
			var viewSection = new InputRecordViewSection();
			viewSection.Id = new Guid("7848deef-6176-45ec-a12c-bb760793e9ef");
			viewSection.Name = "details";
			viewSection.Label = "Details";
			viewSection.ShowLabel = false;
			viewSection.CssClass = "";
			viewSection.Collapsed = false;
			viewSection.TabOrder = "left-right";
			viewSection.Weight = Decimal.Parse("1.0");
			viewSection.Rows = new List<InputRecordViewRow>();

				#region << Row 1>>
				{
					var viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("3a099857-dd26-4723-b0ee-f62ae47d7d93");
					viewRow.Weight = Decimal.Parse("1.0");
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("12");
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << name >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32");
						viewItem.EntityName = "wv_project";
						viewItem.FieldId = new Guid("d13427b7-e518-4305-b2cc-bc814a299b55");
						viewItem.FieldName = "name";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					#region << description >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32");
						viewItem.EntityName = "wv_project";
						viewItem.FieldId = new Guid("d5e2c42c-c0b8-4f03-92e0-e91bede1e7b3");
						viewItem.FieldName = "description";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					//Save column
					viewRow.Columns.Add(viewColumn);
					}
					#endregion
				//Save row
				viewSection.Rows.Add(viewRow);
				}
				#endregion
				#region << Row 2>>
				{
					var viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("b57588f0-9ca2-40b8-b516-3dd6d9efa0b5");
					viewRow.Weight = Decimal.Parse("1.0");
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("6");
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << field from Relation: username >>
					{
						var viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
						viewItemFromRelation.EntityName = "user";
						viewItemFromRelation.Type = "fieldFromRelation";
						viewItemFromRelation.FieldId = new Guid("263c0b21-88c1-4c2b-80b4-db7402b0d2e2");
						viewItemFromRelation.FieldName = "username";
						viewItemFromRelation.FieldLabel = "Owner / Project manager";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = null;
						viewItemFromRelation.FieldRequired = true;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = new Guid("0cad07c3-73bd-4c1f-a5d6-552256f679a4");
						viewItemFromRelation.RelationName = "user_1_n_project_owner";
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion
					#region << field from Relation: name >>
					{
						var viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = new Guid("c4541fee-fbb6-4661-929e-1724adec285a");
						viewItemFromRelation.EntityName = "role";
						viewItemFromRelation.Type = "fieldFromRelation";
						viewItemFromRelation.FieldId = new Guid("36f91ebd-5a02-4032-8498-b7f716f6a349");
						viewItemFromRelation.FieldName = "name";
						viewItemFromRelation.FieldLabel = "Project team roles";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = null;
						viewItemFromRelation.FieldRequired = true;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = new Guid("4860a4b6-d07e-416f-b548-60491607e93f");
						viewItemFromRelation.RelationName = "role_n_n_project_team";
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion
					#region << start_date >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32");
						viewItem.EntityName = "wv_project";
						viewItem.FieldId = new Guid("417ce7d7-a472-499b-8e70-43a1cb54723d");
						viewItem.FieldName = "start_date";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					//Save column
					viewRow.Columns.Add(viewColumn);
					}
					#endregion
					#region << Column 2 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("6");
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << field from Relation: name >>
					{
						var viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = new Guid("90bcdb47-2cde-4137-a412-0198348fecc0");
						viewItemFromRelation.EntityName = "wv_customer";
						viewItemFromRelation.Type = "fieldFromRelation";
						viewItemFromRelation.FieldId = new Guid("7fb95d0f-ab59-421d-974d-ab357e28a1f9");
						viewItemFromRelation.FieldName = "name";
						viewItemFromRelation.FieldLabel = "Customer";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = null;
						viewItemFromRelation.FieldRequired = true;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = new Guid("d7f1ec35-9f59-4d75-b8a2-554c7eaeab11");
						viewItemFromRelation.RelationName = "customer_1_n_project";
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion
					#region << field from Relation: name >>
					{
						var viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = new Guid("c4541fee-fbb6-4661-929e-1724adec285a");
						viewItemFromRelation.EntityName = "role";
						viewItemFromRelation.Type = "fieldFromRelation";
						viewItemFromRelation.FieldId = new Guid("36f91ebd-5a02-4032-8498-b7f716f6a349");
						viewItemFromRelation.FieldName = "name";
						viewItemFromRelation.FieldLabel = "Project customer roles";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = null;
						viewItemFromRelation.FieldRequired = true;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = new Guid("e6d75feb-3c8f-410b-9ff4-54ef8489dc2f");
						viewItemFromRelation.RelationName = "role_n_n_project_customer";
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion
					#region << end_date >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32");
						viewItem.EntityName = "wv_project";
						viewItem.FieldId = new Guid("57198778-636d-47ec-b33e-edfc5705cc05");
						viewItem.FieldName = "end_date";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					#region << code >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32");
						viewItem.EntityName = "wv_project";
						viewItem.FieldId = new Guid("d9c6a939-e2e3-4617-900e-e056f0638fa8");
						viewItem.FieldName = "code";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					//Save column
					viewRow.Columns.Add(viewColumn);
					}
					#endregion
				//Save row
				viewSection.Rows.Add(viewRow);
				}
				#endregion
			//Save section
			viewRegion.Sections.Add(viewSection);
			}
			#endregion
			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""::ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
		ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
	<i class=""fa fa-trash go-red""></i> Delete Record
</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_project Updated view: general Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_project View: quick_view >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "quick_view").Id;
	createViewInput.Type = "Quick_View";
	createViewInput.Name = "quick_view";
	createViewInput.Label = "Quick view";
	createViewInput.Title = "";
	createViewInput.Default = true;
	createViewInput.System = true;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "file-text-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("10.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""::ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
		ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
	<i class=""fa fa-trash go-red""></i> Delete Record
</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_project Updated view: quick_view Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_project name: admin >>
{
	var createListEntity = entMan.ReadEntity(new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "admin").Id;
	createListInput.Type =  "Hidden";
	createListInput.Name = "admin";
	createListInput.Label = "All Projects";
	createListInput.Title = null;
	createListInput.Weight = Decimal.Parse("1.0");
	createListInput.Default = false;
	createListInput.System = true;
	createListInput.CssClass = null;
	createListInput.IconName = "product-hunt";
	createListInput.VisibleColumnsCount = Int32.Parse("7");
	createListInput.ColumnWidthsCSV = null;
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record, ngCtrl)}}"">
					<i class=""fa fa-fw fa-eye""></i>
					</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
									ng-href=""{{::ngCtrl.getRecordCreateUrl(ngCtrl)}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

		#region << name >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32");
			listField.EntityName = "wv_project";
			listField.FieldId = new Guid("d13427b7-e518-4305-b2cc-bc814a299b55");
			listField.FieldName = "name";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = null;
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_project Updated list: admin Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_milestone View: quick_view >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("d691b634-016c-46ef-8ba8-8c3328797497")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "quick_view").Id;
	createViewInput.Type = "Quick_View";
	createViewInput.Name = "quick_view";
	createViewInput.Label = "Quick view";
	createViewInput.Title = "";
	createViewInput.Default = true;
	createViewInput.System = false;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "file-text-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("10.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""::ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
		ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
	<i class=""fa fa-trash go-red""></i> Delete Record
</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("d691b634-016c-46ef-8ba8-8c3328797497"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_milestone Updated view: quick_view Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_milestone View: general >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("d691b634-016c-46ef-8ba8-8c3328797497")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "general").Id;
	createViewInput.Type = "General";
	createViewInput.Name = "general";
	createViewInput.Label = "General";
	createViewInput.Title = "";
	createViewInput.Default = true;
	createViewInput.System = false;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "file-text-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("10.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""::ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
		ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
	<i class=""fa fa-trash go-red""></i> Delete Record
</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("d691b634-016c-46ef-8ba8-8c3328797497"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_milestone Updated view: general Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_milestone View: create >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("d691b634-016c-46ef-8ba8-8c3328797497")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "create").Id;
	createViewInput.Type = "Create";
	createViewInput.Name = "create";
	createViewInput.Label = "Create";
	createViewInput.Title = "";
	createViewInput.Default = true;
	createViewInput.System = false;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "file-text-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("10.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			#region << Section: details >>
			{
			var viewSection = new InputRecordViewSection();
			viewSection.Id = new Guid("dd96bfb9-0dbc-4a6b-b7c6-92fb54b1844a");
			viewSection.Name = "details";
			viewSection.Label = "Details";
			viewSection.ShowLabel = false;
			viewSection.CssClass = "";
			viewSection.Collapsed = false;
			viewSection.TabOrder = "left-right";
			viewSection.Weight = Decimal.Parse("1.0");
			viewSection.Rows = new List<InputRecordViewRow>();

				#region << Row 1>>
				{
					var viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("7f59d113-b469-4579-b780-961ead238e46");
					viewRow.Weight = Decimal.Parse("1.0");
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("6");
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << name >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("d691b634-016c-46ef-8ba8-8c3328797497");
						viewItem.EntityName = "wv_milestone";
						viewItem.FieldId = new Guid("94cc3894-110a-4bb7-8c75-3e887cc83217");
						viewItem.FieldName = "name";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					#region << start_date >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("d691b634-016c-46ef-8ba8-8c3328797497");
						viewItem.EntityName = "wv_milestone";
						viewItem.FieldId = new Guid("9502a7e4-816c-433c-9f1e-6b1e2dffad62");
						viewItem.FieldName = "start_date";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					#region << field from Relation: name >>
					{
						var viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32");
						viewItemFromRelation.EntityName = "wv_project";
						viewItemFromRelation.Type = "fieldFromRelation";
						viewItemFromRelation.FieldId = new Guid("d13427b7-e518-4305-b2cc-bc814a299b55");
						viewItemFromRelation.FieldName = "name";
						viewItemFromRelation.FieldLabel = "Project";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = null;
						viewItemFromRelation.FieldRequired = true;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = new Guid("0c446f98-eec2-40c1-9d66-8a3c2a2498e9");
						viewItemFromRelation.RelationName = "project_1_n_milestone";
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion
					//Save column
					viewRow.Columns.Add(viewColumn);
					}
					#endregion
					#region << Column 2 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("6");
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << status >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("d691b634-016c-46ef-8ba8-8c3328797497");
						viewItem.EntityName = "wv_milestone";
						viewItem.FieldId = new Guid("63eed358-9b33-4d2c-b2cd-b533413df227");
						viewItem.FieldName = "status";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					#region << end_date >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("d691b634-016c-46ef-8ba8-8c3328797497");
						viewItem.EntityName = "wv_milestone";
						viewItem.FieldId = new Guid("1252a300-c871-4d79-8242-f036705cc86d");
						viewItem.FieldName = "end_date";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					//Save column
					viewRow.Columns.Add(viewColumn);
					}
					#endregion
				//Save row
				viewSection.Rows.Add(viewRow);
				}
				#endregion
			//Save section
			viewRegion.Sections.Add(viewSection);
			}
			#endregion
			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_create_and_list >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_and_list";
			actionItem.Menu = "create-bottom";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-primary"" ng-click='ngCtrl.create(""default"")' ng-if=""::ngCtrl.createViewRegion != null"">Create</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_create_and_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_and_details";
			actionItem.Menu = "create-bottom";
			actionItem.Weight = Decimal.Parse("2.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-click='ngCtrl.create(""details"")' ng-if=""::ngCtrl.createViewRegion != null"">Create & Details</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_create_cancel >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_cancel";
			actionItem.Menu = "create-bottom";
			actionItem.Weight = Decimal.Parse("3.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-click=""ngCtrl.cancel()"">Cancel</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("d691b634-016c-46ef-8ba8-8c3328797497"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_milestone Updated view: create Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_milestone name: lookup >>
{
	var createListEntity = entMan.ReadEntity(new Guid("d691b634-016c-46ef-8ba8-8c3328797497")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "lookup").Id;
	createListInput.Type =  "Lookup";
	createListInput.Name = "lookup";
	createListInput.Label = "Lookup";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("10.0");
	createListInput.Default = true;
	createListInput.System = false;
	createListInput.CssClass = null;
	createListInput.IconName = "list";
	createListInput.VisibleColumnsCount = Int32.Parse("5");
	createListInput.ColumnWidthsCSV = null;
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
    ng-href=""{{::ngCtrl.getRecordCreateUrl()}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_import_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_import_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("10.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openImportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-upload""></i> Import CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_export_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_export_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("11.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openExportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-download""></i> Export CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record)}}"">
    <i class=""fa fa-fw fa-eye""></i>
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = null;
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("d691b634-016c-46ef-8ba8-8c3328797497"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_milestone Updated list: lookup Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_milestone name: project_milestones >>
{
	var createListEntity = entMan.ReadEntity(new Guid("d691b634-016c-46ef-8ba8-8c3328797497")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "project_milestones").Id;
	createListInput.Type =  "General";
	createListInput.Name = "project_milestones";
	createListInput.Label = "Milestones";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("10.0");
	createListInput.Default = true;
	createListInput.System = false;
	createListInput.CssClass = null;
	createListInput.IconName = "map-signs";
	createListInput.VisibleColumnsCount = Int32.Parse("5");
	createListInput.ColumnWidthsCSV = null;
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = "/plugins/webvella-projects/templates/project-milestones.html";
	createListInput.DataSourceUrl = "/plugins/webvella-projects/api/project/milestones-list";
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
    ng-href=""{{::ngCtrl.getRecordCreateUrl()}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = null;
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("d691b634-016c-46ef-8ba8-8c3328797497"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_milestone Updated list: project_milestones Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_project_activity View: quick_view >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "quick_view").Id;
	createViewInput.Type = "Quick_View";
	createViewInput.Name = "quick_view";
	createViewInput.Label = "Quick view";
	createViewInput.Title = "";
	createViewInput.Default = true;
	createViewInput.System = false;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "file-text-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("10.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""::ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
		ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
	<i class=""fa fa-trash go-red""></i> Delete Record
</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_project_activity Updated view: quick_view Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_project_activity View: general >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "general").Id;
	createViewInput.Type = "General";
	createViewInput.Name = "general";
	createViewInput.Label = "General";
	createViewInput.Title = "";
	createViewInput.Default = true;
	createViewInput.System = false;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "file-text-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("10.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""::ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
		ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
	<i class=""fa fa-trash go-red""></i> Delete Record
</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_project_activity Updated view: general Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_project_activity name: general >>
{
	var createListEntity = entMan.ReadEntity(new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "general").Id;
	createListInput.Type =  "General";
	createListInput.Name = "general";
	createListInput.Label = "General";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("10.0");
	createListInput.Default = true;
	createListInput.System = false;
	createListInput.CssClass = null;
	createListInput.IconName = "list";
	createListInput.VisibleColumnsCount = Int32.Parse("5");
	createListInput.ColumnWidthsCSV = null;
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
    ng-href=""{{::ngCtrl.getRecordCreateUrl()}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_import_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_import_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("10.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openImportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-upload""></i> Import CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_export_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_export_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("11.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openExportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-download""></i> Export CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record)}}"">
    <i class=""fa fa-fw fa-eye""></i>
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = null;
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_project_activity Updated list: general Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_project_activity name: lookup >>
{
	var createListEntity = entMan.ReadEntity(new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "lookup").Id;
	createListInput.Type =  "Lookup";
	createListInput.Name = "lookup";
	createListInput.Label = "Lookup";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("10.0");
	createListInput.Default = true;
	createListInput.System = false;
	createListInput.CssClass = null;
	createListInput.IconName = "list";
	createListInput.VisibleColumnsCount = Int32.Parse("5");
	createListInput.ColumnWidthsCSV = null;
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
    ng-href=""{{::ngCtrl.getRecordCreateUrl()}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_import_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_import_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("10.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openImportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-upload""></i> Import CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_export_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_export_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("11.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openExportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-download""></i> Export CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record)}}"">
    <i class=""fa fa-fw fa-eye""></i>
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = null;
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_project_activity Updated list: lookup Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_project_attachment View: quick_view >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "quick_view").Id;
	createViewInput.Type = "Quick_View";
	createViewInput.Name = "quick_view";
	createViewInput.Label = "Quick view";
	createViewInput.Title = "";
	createViewInput.Default = true;
	createViewInput.System = false;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "file-text-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("10.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""::ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
		ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
	<i class=""fa fa-trash go-red""></i> Delete Record
</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_project_attachment Updated view: quick_view Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_project_attachment View: general >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "general").Id;
	createViewInput.Type = "General";
	createViewInput.Name = "general";
	createViewInput.Label = "General";
	createViewInput.Title = "";
	createViewInput.Default = true;
	createViewInput.System = false;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "file-text-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("10.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""::ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
		ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
	<i class=""fa fa-trash go-red""></i> Delete Record
</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_project_attachment Updated view: general Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_project_attachment View: create >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "create").Id;
	createViewInput.Type = "Create";
	createViewInput.Name = "create";
	createViewInput.Label = "Create";
	createViewInput.Title = "";
	createViewInput.Default = true;
	createViewInput.System = false;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "file-text-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("10.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			#region << Section: details >>
			{
			var viewSection = new InputRecordViewSection();
			viewSection.Id = new Guid("8ea140f9-b6fc-4b61-bc50-38b4e92f9eae");
			viewSection.Name = "details";
			viewSection.Label = "Details";
			viewSection.ShowLabel = false;
			viewSection.CssClass = "";
			viewSection.Collapsed = false;
			viewSection.TabOrder = "left-right";
			viewSection.Weight = Decimal.Parse("1.0");
			viewSection.Rows = new List<InputRecordViewRow>();

				#region << Row 1>>
				{
					var viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("84482250-9a74-43d2-ae20-c883ee82a865");
					viewRow.Weight = Decimal.Parse("1.0");
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("12");
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << file >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73");
						viewItem.EntityName = "wv_project_attachment";
						viewItem.FieldId = new Guid("6d639a8c-e220-4d9f-86f0-de6ba03030b8");
						viewItem.FieldName = "file";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					//Save column
					viewRow.Columns.Add(viewColumn);
					}
					#endregion
				//Save row
				viewSection.Rows.Add(viewRow);
				}
				#endregion
			//Save section
			viewRegion.Sections.Add(viewSection);
			}
			#endregion
			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_create_and_list >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_and_list";
			actionItem.Menu = "create-bottom";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-primary"" ng-click='ngCtrl.create(""default"")' ng-if=""::ngCtrl.createViewRegion != null"">Create</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_create_and_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_and_details";
			actionItem.Menu = "create-bottom";
			actionItem.Weight = Decimal.Parse("2.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-click='ngCtrl.create(""details"")' ng-if=""::ngCtrl.createViewRegion != null"">Create & Details</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_create_cancel >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_cancel";
			actionItem.Menu = "create-bottom";
			actionItem.Weight = Decimal.Parse("3.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-click=""ngCtrl.cancel()"">Cancel</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_project_attachment Updated view: create Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_project_attachment name: lookup >>
{
	var createListEntity = entMan.ReadEntity(new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "lookup").Id;
	createListInput.Type =  "Lookup";
	createListInput.Name = "lookup";
	createListInput.Label = "Lookup";
	createListInput.Title = null;
	createListInput.Weight = Decimal.Parse("10.0");
	createListInput.Default = true;
	createListInput.System = false;
	createListInput.CssClass = null;
	createListInput.IconName = "list";
	createListInput.VisibleColumnsCount = Int32.Parse("5");
	createListInput.ColumnWidthsCSV = null;
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
    ng-href=""{{::ngCtrl.getRecordCreateUrl()}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_import_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_import_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("10.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openImportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-upload""></i> Import CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_export_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_export_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("11.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openExportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-download""></i> Export CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record)}}"">
    <i class=""fa fa-fw fa-eye""></i>
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = null;
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_project_attachment Updated list: lookup Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_project_attachment name: task_attachments >>
{
	var createListEntity = entMan.ReadEntity(new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "task_attachments").Id;
	createListInput.Type =  "Hidden";
	createListInput.Name = "task_attachments";
	createListInput.Label = "Attachments";
	createListInput.Title = null;
	createListInput.Weight = Decimal.Parse("10.0");
	createListInput.Default = false;
	createListInput.System = true;
	createListInput.CssClass = null;
	createListInput.IconName = "paperclip";
	createListInput.VisibleColumnsCount = Int32.Parse("5");
	createListInput.ColumnWidthsCSV = "160px,160px,auto";
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
										ng-href=""{{ngCtrl.getRecordCreateUrl(ngCtrl)}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

		#region << created_on >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73");
			listField.EntityName = "wv_project_attachment";
			listField.FieldId = new Guid("381de04c-fad1-46b6-aa11-59bf7822a9a5");
			listField.FieldName = "created_on";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << field from Relation: username >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("263c0b21-88c1-4c2b-80b4-db7402b0d2e2");
			listItemFromRelation.FieldName = "username";
			listItemFromRelation.FieldLabel = null;
			listItemFromRelation.FieldPlaceholder = null;
			listItemFromRelation.FieldHelpText = null;
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = null;
			listItemFromRelation.RelationId = new Guid("97fe4c22-b090-4d8d-b9df-39d3e04a5865");
			listItemFromRelation.RelationName = "user_wv_project_attachment_created_by";
			createListInput.Columns.Add(listItemFromRelation);
		}
		#endregion
		#region << file >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73");
			listField.EntityName = "wv_project_attachment";
			listField.FieldId = new Guid("6d639a8c-e220-4d9f-86f0-de6ba03030b8");
			listField.FieldName = "file";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = null;
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

		#region << sort >>
		{
			var sort = new InputRecordListSort();
			sort.FieldName = "created_on";
			sort.SortType = "Ascending";
			createListInput.Sorts.Add(sort);
		}
		#endregion

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_project_attachment Updated list: task_attachments Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_timelog View: quick_view >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("e2db7515-721f-446e-8333-6149b1ba131b")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "quick_view").Id;
	createViewInput.Type = "Quick_View";
	createViewInput.Name = "quick_view";
	createViewInput.Label = "Quick view";
	createViewInput.Title = "";
	createViewInput.Default = true;
	createViewInput.System = false;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "file-text-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("10.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""::ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
		ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
	<i class=""fa fa-trash go-red""></i> Delete Record
</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("e2db7515-721f-446e-8333-6149b1ba131b"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_timelog Updated view: quick_view Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_timelog View: general >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("e2db7515-721f-446e-8333-6149b1ba131b")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "general").Id;
	createViewInput.Type = "General";
	createViewInput.Name = "general";
	createViewInput.Label = "General";
	createViewInput.Title = "";
	createViewInput.Default = true;
	createViewInput.System = false;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "file-text-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("10.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""::ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
		ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
	<i class=""fa fa-trash go-red""></i> Delete Record
</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("e2db7515-721f-446e-8333-6149b1ba131b"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_timelog Updated view: general Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_timelog name: general >>
{
	var createListEntity = entMan.ReadEntity(new Guid("e2db7515-721f-446e-8333-6149b1ba131b")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "general").Id;
	createListInput.Type =  "General";
	createListInput.Name = "general";
	createListInput.Label = "General";
	createListInput.Title = null;
	createListInput.Weight = Decimal.Parse("10.0");
	createListInput.Default = true;
	createListInput.System = false;
	createListInput.CssClass = null;
	createListInput.IconName = "list";
	createListInput.VisibleColumnsCount = Int32.Parse("5");
	createListInput.ColumnWidthsCSV = null;
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
    ng-href=""{{::ngCtrl.getRecordCreateUrl()}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_import_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_import_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("10.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openImportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-upload""></i> Import CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_export_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_export_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("11.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openExportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-download""></i> Export CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record)}}"">
    <i class=""fa fa-fw fa-eye""></i>
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = null;
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("e2db7515-721f-446e-8333-6149b1ba131b"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_timelog Updated list: general Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_timelog name: lookup >>
{
	var createListEntity = entMan.ReadEntity(new Guid("e2db7515-721f-446e-8333-6149b1ba131b")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "lookup").Id;
	createListInput.Type =  "Lookup";
	createListInput.Name = "lookup";
	createListInput.Label = "Lookup";
	createListInput.Title = null;
	createListInput.Weight = Decimal.Parse("10.0");
	createListInput.Default = true;
	createListInput.System = false;
	createListInput.CssClass = null;
	createListInput.IconName = "list";
	createListInput.VisibleColumnsCount = Int32.Parse("5");
	createListInput.ColumnWidthsCSV = null;
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
    ng-href=""{{::ngCtrl.getRecordCreateUrl()}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_import_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_import_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("10.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openImportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-upload""></i> Import CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_export_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_export_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("11.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openExportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-download""></i> Export CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record)}}"">
    <i class=""fa fa-fw fa-eye""></i>
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = null;
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("e2db7515-721f-446e-8333-6149b1ba131b"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_timelog Updated list: lookup Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_timelog name: bug_timelogs >>
{
	var createListEntity = entMan.ReadEntity(new Guid("e2db7515-721f-446e-8333-6149b1ba131b")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "bug_timelogs").Id;
	createListInput.Type =  "Hidden";
	createListInput.Name = "bug_timelogs";
	createListInput.Label = "Time logs";
	createListInput.Title = null;
	createListInput.Weight = Decimal.Parse("25.0");
	createListInput.Default = false;
	createListInput.System = true;
	createListInput.CssClass = null;
	createListInput.IconName = "clock-o";
	createListInput.VisibleColumnsCount = Int32.Parse("5");
	createListInput.ColumnWidthsCSV = "160px,160px,80px,auto";
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
											ng-href=""{{ngCtrl.getRecordCreateUrl(ngCtrl)}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

		#region << log_date >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("e2db7515-721f-446e-8333-6149b1ba131b");
			listField.EntityName = "wv_timelog";
			listField.FieldId = new Guid("29a32ad7-7b1c-4ea0-a06b-57b30be9b107");
			listField.FieldName = "log_date";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << field from Relation: username >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("263c0b21-88c1-4c2b-80b4-db7402b0d2e2");
			listItemFromRelation.FieldName = "username";
			listItemFromRelation.FieldLabel = "username";
			listItemFromRelation.FieldPlaceholder = null;
			listItemFromRelation.FieldHelpText = null;
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = null;
			listItemFromRelation.RelationId = new Guid("393d1da8-0051-4807-8e89-1de933850888");
			listItemFromRelation.RelationName = "user_wv_timelog_created_by";
			createListInput.Columns.Add(listItemFromRelation);
		}
		#endregion
		#region << hours >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("e2db7515-721f-446e-8333-6149b1ba131b");
			listField.EntityName = "wv_timelog";
			listField.FieldId = new Guid("41caeb03-7430-4eb8-b830-c9df8bf2dc7f");
			listField.FieldName = "hours";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << description >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("e2db7515-721f-446e-8333-6149b1ba131b");
			listField.EntityName = "wv_timelog";
			listField.FieldId = new Guid("1a1b646e-93df-4035-ace0-d844f62bad63");
			listField.FieldName = "description";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = null;
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

		#region << sort >>
		{
			var sort = new InputRecordListSort();
			sort.FieldName = "log_date";
			sort.SortType = "Ascending";
			createListInput.Sorts.Add(sort);
		}
		#endregion

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("e2db7515-721f-446e-8333-6149b1ba131b"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_timelog Updated list: bug_timelogs Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_timelog name: task_timelogs >>
{
	var createListEntity = entMan.ReadEntity(new Guid("e2db7515-721f-446e-8333-6149b1ba131b")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "task_timelogs").Id;
	createListInput.Type =  "Hidden";
	createListInput.Name = "task_timelogs";
	createListInput.Label = "Time logs";
	createListInput.Title = null;
	createListInput.Weight = Decimal.Parse("25.0");
	createListInput.Default = false;
	createListInput.System = true;
	createListInput.CssClass = null;
	createListInput.IconName = "clock-o";
	createListInput.VisibleColumnsCount = Int32.Parse("5");
	createListInput.ColumnWidthsCSV = "160px,160px,80px,auto";
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
											ng-href=""{{ngCtrl.getRecordCreateUrl(ngCtrl)}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

		#region << log_date >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("e2db7515-721f-446e-8333-6149b1ba131b");
			listField.EntityName = "wv_timelog";
			listField.FieldId = new Guid("29a32ad7-7b1c-4ea0-a06b-57b30be9b107");
			listField.FieldName = "log_date";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << field from Relation: username >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("263c0b21-88c1-4c2b-80b4-db7402b0d2e2");
			listItemFromRelation.FieldName = "username";
			listItemFromRelation.FieldLabel = "username";
			listItemFromRelation.FieldPlaceholder = null;
			listItemFromRelation.FieldHelpText = null;
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = null;
			listItemFromRelation.RelationId = new Guid("393d1da8-0051-4807-8e89-1de933850888");
			listItemFromRelation.RelationName = "user_wv_timelog_created_by";
			createListInput.Columns.Add(listItemFromRelation);
		}
		#endregion
		#region << hours >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("e2db7515-721f-446e-8333-6149b1ba131b");
			listField.EntityName = "wv_timelog";
			listField.FieldId = new Guid("41caeb03-7430-4eb8-b830-c9df8bf2dc7f");
			listField.FieldName = "hours";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << description >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("e2db7515-721f-446e-8333-6149b1ba131b");
			listField.EntityName = "wv_timelog";
			listField.FieldId = new Guid("1a1b646e-93df-4035-ace0-d844f62bad63");
			listField.FieldName = "description";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = null;
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

		#region << sort >>
		{
			var sort = new InputRecordListSort();
			sort.FieldName = "log_date";
			sort.SortType = "Ascending";
			createListInput.Sorts.Add(sort);
		}
		#endregion

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("e2db7515-721f-446e-8333-6149b1ba131b"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_timelog Updated list: task_timelogs Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_task field: priority >>
{
	var currentEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
	InputSelectField dropdownField = new InputSelectField();
	dropdownField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "priority").Id;
	dropdownField.Name = "priority";
	dropdownField.Label = "Priority";
	dropdownField.PlaceholderText = "";
	dropdownField.Description = "";
	dropdownField.HelpText = "";
	dropdownField.Required = true;
	dropdownField.Unique = false;
	dropdownField.Searchable = true;
	dropdownField.Auditable = false;
	dropdownField.System = true;
	dropdownField.DefaultValue = "medium";
	dropdownField.Options = new List<SelectFieldOption>
	{
		new SelectFieldOption() { Key = "low", Value = "low"},
		new SelectFieldOption() { Key = "medium", Value = "medium"},
		new SelectFieldOption() { Key = "high", Value = "high"}
	};
	dropdownField.EnableSecurity = true;
	dropdownField.Permissions = new FieldPermissions();
	dropdownField.Permissions.CanRead = new List<Guid>();
	dropdownField.Permissions.CanUpdate = new List<Guid>();
	//READ
	dropdownField.Permissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
	dropdownField.Permissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
	//UPDATE
	dropdownField.Permissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
	dropdownField.Permissions.CanUpdate.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
	{
		var response = entMan.UpdateField(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89"), dropdownField);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_task Field: priority Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_task View: quick_view >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "quick_view").Id;
	createViewInput.Type = "Quick_View";
	createViewInput.Name = "quick_view";
	createViewInput.Label = "Quick view";
	createViewInput.Title = "";
	createViewInput.Default = true;
	createViewInput.System = false;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "file-text-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("10.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""::ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
		ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
	<i class=""fa fa-trash go-red""></i> Delete Record
</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_task Updated view: quick_view Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_task name: lookup >>
{
	var createListEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "lookup").Id;
	createListInput.Type =  "Lookup";
	createListInput.Name = "lookup";
	createListInput.Label = "Lookup";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("10.0");
	createListInput.Default = true;
	createListInput.System = false;
	createListInput.CssClass = null;
	createListInput.IconName = "list";
	createListInput.VisibleColumnsCount = Int32.Parse("5");
	createListInput.ColumnWidthsCSV = null;
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
    ng-href=""{{::ngCtrl.getRecordCreateUrl()}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_import_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_import_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("10.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openImportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-upload""></i> Import CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_export_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_export_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("11.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openExportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-download""></i> Export CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record)}}"">
    <i class=""fa fa-fw fa-eye""></i>
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = null;
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_task Updated list: lookup Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_task name: project_tasks >>
{
	var createListEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "project_tasks").Id;
	createListInput.Type =  "Hidden";
	createListInput.Name = "project_tasks";
	createListInput.Label = "Project Tasks";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("10.0");
	createListInput.Default = true;
	createListInput.System = false;
	createListInput.CssClass = "task-list";
	createListInput.IconName = "tasks";
	createListInput.VisibleColumnsCount = Int32.Parse("6");
	createListInput.ColumnWidthsCSV = "auto,30px,120px,120px,120px,120px";
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
    ng-href=""{{::ngCtrl.getRecordCreateUrl()}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_import_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_import_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("10.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openImportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-upload""></i> Import CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_export_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_export_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("11.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openExportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-download""></i> Export CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record)}}"">
    <i class=""fa fa-fw fa-eye""></i>
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

		#region << subject >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("7843bfbd-30c1-4438-af48-ffe56b7f294a");
			listField.FieldName = "subject";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << field from Relation: image >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("bf199b74-4448-4f58-93f5-6b86d888843b");
			listItemFromRelation.FieldName = "image";
			listItemFromRelation.FieldLabel = "Owner";
			listItemFromRelation.FieldPlaceholder = null;
			listItemFromRelation.FieldHelpText = null;
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = null;
			listItemFromRelation.RelationId = new Guid("7ce76c81-e604-401e-907f-23de982b930e");
			listItemFromRelation.RelationName = "user_1_n_task_owner";
			createListInput.Columns.Add(listItemFromRelation);
		}
		#endregion
		#region << start_date >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("5bf852bf-5e6c-4791-bc8b-f1366440c04b");
			listField.FieldName = "start_date";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << end_date >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("c6dc9db2-0081-4a91-b0e2-78f9c1c45a51");
			listField.FieldName = "end_date";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << status >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("0702e611-6fe5-42e4-9bad-d549cba9cbb1");
			listField.FieldName = "status";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << priority >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("04ede478-99ec-4f7f-97af-0df3e89409b1");
			listField.FieldName = "priority";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = new InputRecordListQuery();
	var queryDictionary = new Dictionary<Guid,InputRecordListQuery>();
	var subQueryDictionary = new Dictionary<Guid,List<InputRecordListQuery>>();
	//Main query rule
	createListInput.Query.FieldName = null;
	createListInput.Query.FieldValue =  null;
	createListInput.Query.QueryType = "AND";
	createListInput.Query.SubQueries = new List<InputRecordListQuery>();
		{
		queryDictionary[new Guid("f93c2154-20cf-4e46-a490-a5ac28201ca3")] = new InputRecordListQuery();
		queryDictionary[new Guid("f93c2154-20cf-4e46-a490-a5ac28201ca3")].FieldName = "subject";
		queryDictionary[new Guid("f93c2154-20cf-4e46-a490-a5ac28201ca3")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("f93c2154-20cf-4e46-a490-a5ac28201ca3")].QueryType = "CONTAINS";
		queryDictionary[new Guid("f93c2154-20cf-4e46-a490-a5ac28201ca3")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("f93c2154-20cf-4e46-a490-a5ac28201ca3"))) {queryDictionary[new Guid("f93c2154-20cf-4e46-a490-a5ac28201ca3")].SubQueries = subQueryDictionary[new Guid("f93c2154-20cf-4e46-a490-a5ac28201ca3")];}
		if(!subQueryDictionary.ContainsKey(new Guid("5860e9d1-e28d-40ba-bc6d-a650d9464df0"))) {subQueryDictionary[new Guid("5860e9d1-e28d-40ba-bc6d-a650d9464df0")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("5860e9d1-e28d-40ba-bc6d-a650d9464df0")].Add(queryDictionary[new Guid("f93c2154-20cf-4e46-a490-a5ac28201ca3")]);
		}
		{
		queryDictionary[new Guid("f25bb7f8-ab71-46ad-b249-95abb3d8ed18")] = new InputRecordListQuery();
		queryDictionary[new Guid("f25bb7f8-ab71-46ad-b249-95abb3d8ed18")].FieldName = "status";
		queryDictionary[new Guid("f25bb7f8-ab71-46ad-b249-95abb3d8ed18")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("f25bb7f8-ab71-46ad-b249-95abb3d8ed18")].QueryType = "EQ";
		queryDictionary[new Guid("f25bb7f8-ab71-46ad-b249-95abb3d8ed18")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("f25bb7f8-ab71-46ad-b249-95abb3d8ed18"))) {queryDictionary[new Guid("f25bb7f8-ab71-46ad-b249-95abb3d8ed18")].SubQueries = subQueryDictionary[new Guid("f25bb7f8-ab71-46ad-b249-95abb3d8ed18")];}
		if(!subQueryDictionary.ContainsKey(new Guid("5860e9d1-e28d-40ba-bc6d-a650d9464df0"))) {subQueryDictionary[new Guid("5860e9d1-e28d-40ba-bc6d-a650d9464df0")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("5860e9d1-e28d-40ba-bc6d-a650d9464df0")].Add(queryDictionary[new Guid("f25bb7f8-ab71-46ad-b249-95abb3d8ed18")]);
		}
		{
		queryDictionary[new Guid("21ea496d-b37c-464a-beb5-c646e25f83d7")] = new InputRecordListQuery();
		queryDictionary[new Guid("21ea496d-b37c-464a-beb5-c646e25f83d7")].FieldName = "priority";
		queryDictionary[new Guid("21ea496d-b37c-464a-beb5-c646e25f83d7")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("21ea496d-b37c-464a-beb5-c646e25f83d7")].QueryType = "EQ";
		queryDictionary[new Guid("21ea496d-b37c-464a-beb5-c646e25f83d7")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("21ea496d-b37c-464a-beb5-c646e25f83d7"))) {queryDictionary[new Guid("21ea496d-b37c-464a-beb5-c646e25f83d7")].SubQueries = subQueryDictionary[new Guid("21ea496d-b37c-464a-beb5-c646e25f83d7")];}
		if(!subQueryDictionary.ContainsKey(new Guid("5860e9d1-e28d-40ba-bc6d-a650d9464df0"))) {subQueryDictionary[new Guid("5860e9d1-e28d-40ba-bc6d-a650d9464df0")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("5860e9d1-e28d-40ba-bc6d-a650d9464df0")].Add(queryDictionary[new Guid("21ea496d-b37c-464a-beb5-c646e25f83d7")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("5860e9d1-e28d-40ba-bc6d-a650d9464df0"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("5860e9d1-e28d-40ba-bc6d-a650d9464df0")];}
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

		#region << sort >>
		{
			var sort = new InputRecordListSort();
			sort.FieldName = "{\"name\":\"url_sort\", \"option\": \"sortBy\", \"default\": \"end_date\", \"settings\":{\"order\":\"sortOrder\"}}";
			sort.SortType = "Ascending";
			createListInput.Sorts.Add(sort);
		}
		#endregion

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_task Updated list: project_tasks Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_task name: created_tasks >>
{
	var createListEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "created_tasks").Id;
	createListInput.Type =  "General";
	createListInput.Name = "created_tasks";
	createListInput.Label = "Tasks created by me";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("3.0");
	createListInput.Default = false;
	createListInput.System = true;
	createListInput.CssClass = "task-list";
	createListInput.IconName = "tasks";
	createListInput.VisibleColumnsCount = Int32.Parse("7");
	createListInput.ColumnWidthsCSV = "100px,auto,30px,120px,120px,120px,120px";
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record, ngCtrl)}}"">
					<i class=""fa fa-fw fa-eye""></i>
					</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
									ng-href=""{{::ngCtrl.getRecordCreateUrl(ngCtrl)}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

		#region << code >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("04f31ad8-4583-4237-8d54-f82c3f44b918");
			listField.FieldName = "code";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << subject >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("7843bfbd-30c1-4438-af48-ffe56b7f294a");
			listField.FieldName = "subject";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << field from Relation: image >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("bf199b74-4448-4f58-93f5-6b86d888843b");
			listItemFromRelation.FieldName = "image";
			listItemFromRelation.FieldLabel = "Owner";
			listItemFromRelation.FieldPlaceholder = null;
			listItemFromRelation.FieldHelpText = null;
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = null;
			listItemFromRelation.RelationId = new Guid("7ce76c81-e604-401e-907f-23de982b930e");
			listItemFromRelation.RelationName = "user_1_n_task_owner";
			createListInput.Columns.Add(listItemFromRelation);
		}
		#endregion
		#region << start_date >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("5bf852bf-5e6c-4791-bc8b-f1366440c04b");
			listField.FieldName = "start_date";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << end_date >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("c6dc9db2-0081-4a91-b0e2-78f9c1c45a51");
			listField.FieldName = "end_date";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << status >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("0702e611-6fe5-42e4-9bad-d549cba9cbb1");
			listField.FieldName = "status";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << priority >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("04ede478-99ec-4f7f-97af-0df3e89409b1");
			listField.FieldName = "priority";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = new InputRecordListQuery();
	var queryDictionary = new Dictionary<Guid,InputRecordListQuery>();
	var subQueryDictionary = new Dictionary<Guid,List<InputRecordListQuery>>();
	//Main query rule
	createListInput.Query.FieldName = null;
	createListInput.Query.FieldValue =  null;
	createListInput.Query.QueryType = "AND";
	createListInput.Query.SubQueries = new List<InputRecordListQuery>();
		{
		queryDictionary[new Guid("64cf12a6-ec5c-4746-8eca-50b5d6243bfb")] = new InputRecordListQuery();
		queryDictionary[new Guid("64cf12a6-ec5c-4746-8eca-50b5d6243bfb")].FieldName = null;
		queryDictionary[new Guid("64cf12a6-ec5c-4746-8eca-50b5d6243bfb")].FieldValue =  null;
		queryDictionary[new Guid("64cf12a6-ec5c-4746-8eca-50b5d6243bfb")].QueryType = "AND";
		queryDictionary[new Guid("64cf12a6-ec5c-4746-8eca-50b5d6243bfb")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("64b8b246-81c5-4eef-a7ee-d44224db0b8e")] = new InputRecordListQuery();
			queryDictionary[new Guid("64b8b246-81c5-4eef-a7ee-d44224db0b8e")].FieldName = "created_by";
			queryDictionary[new Guid("64b8b246-81c5-4eef-a7ee-d44224db0b8e")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("64b8b246-81c5-4eef-a7ee-d44224db0b8e")].QueryType = "EQ";
			queryDictionary[new Guid("64b8b246-81c5-4eef-a7ee-d44224db0b8e")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("64b8b246-81c5-4eef-a7ee-d44224db0b8e"))) {queryDictionary[new Guid("64b8b246-81c5-4eef-a7ee-d44224db0b8e")].SubQueries = subQueryDictionary[new Guid("64b8b246-81c5-4eef-a7ee-d44224db0b8e")];}
			if(!subQueryDictionary.ContainsKey(new Guid("64cf12a6-ec5c-4746-8eca-50b5d6243bfb"))) {subQueryDictionary[new Guid("64cf12a6-ec5c-4746-8eca-50b5d6243bfb")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("64cf12a6-ec5c-4746-8eca-50b5d6243bfb")].Add(queryDictionary[new Guid("64b8b246-81c5-4eef-a7ee-d44224db0b8e")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("64cf12a6-ec5c-4746-8eca-50b5d6243bfb"))) {queryDictionary[new Guid("64cf12a6-ec5c-4746-8eca-50b5d6243bfb")].SubQueries = subQueryDictionary[new Guid("64cf12a6-ec5c-4746-8eca-50b5d6243bfb")];}
		if(!subQueryDictionary.ContainsKey(new Guid("f1378494-e3ab-431a-a7c6-16e3fb82b32d"))) {subQueryDictionary[new Guid("f1378494-e3ab-431a-a7c6-16e3fb82b32d")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("f1378494-e3ab-431a-a7c6-16e3fb82b32d")].Add(queryDictionary[new Guid("64cf12a6-ec5c-4746-8eca-50b5d6243bfb")]);
		}
		{
		queryDictionary[new Guid("615a4831-bd91-412d-9eb0-8bb70ed80d59")] = new InputRecordListQuery();
		queryDictionary[new Guid("615a4831-bd91-412d-9eb0-8bb70ed80d59")].FieldName = null;
		queryDictionary[new Guid("615a4831-bd91-412d-9eb0-8bb70ed80d59")].FieldValue =  null;
		queryDictionary[new Guid("615a4831-bd91-412d-9eb0-8bb70ed80d59")].QueryType = "AND";
		queryDictionary[new Guid("615a4831-bd91-412d-9eb0-8bb70ed80d59")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("7ef98e73-6e69-4a97-a618-5e178a2a0f90")] = new InputRecordListQuery();
			queryDictionary[new Guid("7ef98e73-6e69-4a97-a618-5e178a2a0f90")].FieldName = "code";
			queryDictionary[new Guid("7ef98e73-6e69-4a97-a618-5e178a2a0f90")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("7ef98e73-6e69-4a97-a618-5e178a2a0f90")].QueryType = "CONTAINS";
			queryDictionary[new Guid("7ef98e73-6e69-4a97-a618-5e178a2a0f90")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("7ef98e73-6e69-4a97-a618-5e178a2a0f90"))) {queryDictionary[new Guid("7ef98e73-6e69-4a97-a618-5e178a2a0f90")].SubQueries = subQueryDictionary[new Guid("7ef98e73-6e69-4a97-a618-5e178a2a0f90")];}
			if(!subQueryDictionary.ContainsKey(new Guid("615a4831-bd91-412d-9eb0-8bb70ed80d59"))) {subQueryDictionary[new Guid("615a4831-bd91-412d-9eb0-8bb70ed80d59")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("615a4831-bd91-412d-9eb0-8bb70ed80d59")].Add(queryDictionary[new Guid("7ef98e73-6e69-4a97-a618-5e178a2a0f90")]);
			}
			{
			queryDictionary[new Guid("ecca91ff-60b0-4f3b-83b4-591aa5f7c24d")] = new InputRecordListQuery();
			queryDictionary[new Guid("ecca91ff-60b0-4f3b-83b4-591aa5f7c24d")].FieldName = "subject";
			queryDictionary[new Guid("ecca91ff-60b0-4f3b-83b4-591aa5f7c24d")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("ecca91ff-60b0-4f3b-83b4-591aa5f7c24d")].QueryType = "CONTAINS";
			queryDictionary[new Guid("ecca91ff-60b0-4f3b-83b4-591aa5f7c24d")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("ecca91ff-60b0-4f3b-83b4-591aa5f7c24d"))) {queryDictionary[new Guid("ecca91ff-60b0-4f3b-83b4-591aa5f7c24d")].SubQueries = subQueryDictionary[new Guid("ecca91ff-60b0-4f3b-83b4-591aa5f7c24d")];}
			if(!subQueryDictionary.ContainsKey(new Guid("615a4831-bd91-412d-9eb0-8bb70ed80d59"))) {subQueryDictionary[new Guid("615a4831-bd91-412d-9eb0-8bb70ed80d59")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("615a4831-bd91-412d-9eb0-8bb70ed80d59")].Add(queryDictionary[new Guid("ecca91ff-60b0-4f3b-83b4-591aa5f7c24d")]);
			}
			{
			queryDictionary[new Guid("c8bc3197-5f19-4767-8e3d-ccb37d6aa106")] = new InputRecordListQuery();
			queryDictionary[new Guid("c8bc3197-5f19-4767-8e3d-ccb37d6aa106")].FieldName = "status";
			queryDictionary[new Guid("c8bc3197-5f19-4767-8e3d-ccb37d6aa106")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("c8bc3197-5f19-4767-8e3d-ccb37d6aa106")].QueryType = "EQ";
			queryDictionary[new Guid("c8bc3197-5f19-4767-8e3d-ccb37d6aa106")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("c8bc3197-5f19-4767-8e3d-ccb37d6aa106"))) {queryDictionary[new Guid("c8bc3197-5f19-4767-8e3d-ccb37d6aa106")].SubQueries = subQueryDictionary[new Guid("c8bc3197-5f19-4767-8e3d-ccb37d6aa106")];}
			if(!subQueryDictionary.ContainsKey(new Guid("615a4831-bd91-412d-9eb0-8bb70ed80d59"))) {subQueryDictionary[new Guid("615a4831-bd91-412d-9eb0-8bb70ed80d59")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("615a4831-bd91-412d-9eb0-8bb70ed80d59")].Add(queryDictionary[new Guid("c8bc3197-5f19-4767-8e3d-ccb37d6aa106")]);
			}
			{
			queryDictionary[new Guid("c6ecc6a7-8932-4476-a664-863a30852dd5")] = new InputRecordListQuery();
			queryDictionary[new Guid("c6ecc6a7-8932-4476-a664-863a30852dd5")].FieldName = "priority";
			queryDictionary[new Guid("c6ecc6a7-8932-4476-a664-863a30852dd5")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("c6ecc6a7-8932-4476-a664-863a30852dd5")].QueryType = "EQ";
			queryDictionary[new Guid("c6ecc6a7-8932-4476-a664-863a30852dd5")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("c6ecc6a7-8932-4476-a664-863a30852dd5"))) {queryDictionary[new Guid("c6ecc6a7-8932-4476-a664-863a30852dd5")].SubQueries = subQueryDictionary[new Guid("c6ecc6a7-8932-4476-a664-863a30852dd5")];}
			if(!subQueryDictionary.ContainsKey(new Guid("615a4831-bd91-412d-9eb0-8bb70ed80d59"))) {subQueryDictionary[new Guid("615a4831-bd91-412d-9eb0-8bb70ed80d59")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("615a4831-bd91-412d-9eb0-8bb70ed80d59")].Add(queryDictionary[new Guid("c6ecc6a7-8932-4476-a664-863a30852dd5")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("615a4831-bd91-412d-9eb0-8bb70ed80d59"))) {queryDictionary[new Guid("615a4831-bd91-412d-9eb0-8bb70ed80d59")].SubQueries = subQueryDictionary[new Guid("615a4831-bd91-412d-9eb0-8bb70ed80d59")];}
		if(!subQueryDictionary.ContainsKey(new Guid("f1378494-e3ab-431a-a7c6-16e3fb82b32d"))) {subQueryDictionary[new Guid("f1378494-e3ab-431a-a7c6-16e3fb82b32d")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("f1378494-e3ab-431a-a7c6-16e3fb82b32d")].Add(queryDictionary[new Guid("615a4831-bd91-412d-9eb0-8bb70ed80d59")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("f1378494-e3ab-431a-a7c6-16e3fb82b32d"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("f1378494-e3ab-431a-a7c6-16e3fb82b32d")];}
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

		#region << sort >>
		{
			var sort = new InputRecordListSort();
			sort.FieldName = "{\"name\":\"url_sort\", \"option\": \"sortBy\", \"default\": \"end_date\", \"settings\":{\"order\":\"sortOrder\"}}";
			sort.SortType = "Descending";
			createListInput.Sorts.Add(sort);
		}
		#endregion

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_task Updated list: created_tasks Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_task name: owned_tasks >>
{
	var createListEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "owned_tasks").Id;
	createListInput.Type =  "General";
	createListInput.Name = "owned_tasks";
	createListInput.Label = "Tasks owned by me";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("2.0");
	createListInput.Default = false;
	createListInput.System = true;
	createListInput.CssClass = "task-list";
	createListInput.IconName = "tasks";
	createListInput.VisibleColumnsCount = Int32.Parse("7");
	createListInput.ColumnWidthsCSV = "100px,auto,30px,120px,120px,120px,120px";
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record, ngCtrl)}}"">
					<i class=""fa fa-fw fa-eye""></i>
					</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
									ng-href=""{{::ngCtrl.getRecordCreateUrl(ngCtrl)}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

		#region << code >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("04f31ad8-4583-4237-8d54-f82c3f44b918");
			listField.FieldName = "code";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << subject >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("7843bfbd-30c1-4438-af48-ffe56b7f294a");
			listField.FieldName = "subject";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << field from Relation: image >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("bf199b74-4448-4f58-93f5-6b86d888843b");
			listItemFromRelation.FieldName = "image";
			listItemFromRelation.FieldLabel = "Owner";
			listItemFromRelation.FieldPlaceholder = null;
			listItemFromRelation.FieldHelpText = null;
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = null;
			listItemFromRelation.RelationId = new Guid("7ce76c81-e604-401e-907f-23de982b930e");
			listItemFromRelation.RelationName = "user_1_n_task_owner";
			createListInput.Columns.Add(listItemFromRelation);
		}
		#endregion
		#region << start_date >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("5bf852bf-5e6c-4791-bc8b-f1366440c04b");
			listField.FieldName = "start_date";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << end_date >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("c6dc9db2-0081-4a91-b0e2-78f9c1c45a51");
			listField.FieldName = "end_date";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << status >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("0702e611-6fe5-42e4-9bad-d549cba9cbb1");
			listField.FieldName = "status";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << priority >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("04ede478-99ec-4f7f-97af-0df3e89409b1");
			listField.FieldName = "priority";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = new InputRecordListQuery();
	var queryDictionary = new Dictionary<Guid,InputRecordListQuery>();
	var subQueryDictionary = new Dictionary<Guid,List<InputRecordListQuery>>();
	//Main query rule
	createListInput.Query.FieldName = null;
	createListInput.Query.FieldValue =  null;
	createListInput.Query.QueryType = "AND";
	createListInput.Query.SubQueries = new List<InputRecordListQuery>();
		{
		queryDictionary[new Guid("5ecb637e-b725-4497-917a-ef2ac6139b1d")] = new InputRecordListQuery();
		queryDictionary[new Guid("5ecb637e-b725-4497-917a-ef2ac6139b1d")].FieldName = null;
		queryDictionary[new Guid("5ecb637e-b725-4497-917a-ef2ac6139b1d")].FieldValue =  null;
		queryDictionary[new Guid("5ecb637e-b725-4497-917a-ef2ac6139b1d")].QueryType = "OR";
		queryDictionary[new Guid("5ecb637e-b725-4497-917a-ef2ac6139b1d")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("d3acf50b-8854-4bbd-89d7-95400289fc74")] = new InputRecordListQuery();
			queryDictionary[new Guid("d3acf50b-8854-4bbd-89d7-95400289fc74")].FieldName = "owner_id";
			queryDictionary[new Guid("d3acf50b-8854-4bbd-89d7-95400289fc74")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("d3acf50b-8854-4bbd-89d7-95400289fc74")].QueryType = "EQ";
			queryDictionary[new Guid("d3acf50b-8854-4bbd-89d7-95400289fc74")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("d3acf50b-8854-4bbd-89d7-95400289fc74"))) {queryDictionary[new Guid("d3acf50b-8854-4bbd-89d7-95400289fc74")].SubQueries = subQueryDictionary[new Guid("d3acf50b-8854-4bbd-89d7-95400289fc74")];}
			if(!subQueryDictionary.ContainsKey(new Guid("5ecb637e-b725-4497-917a-ef2ac6139b1d"))) {subQueryDictionary[new Guid("5ecb637e-b725-4497-917a-ef2ac6139b1d")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("5ecb637e-b725-4497-917a-ef2ac6139b1d")].Add(queryDictionary[new Guid("d3acf50b-8854-4bbd-89d7-95400289fc74")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("5ecb637e-b725-4497-917a-ef2ac6139b1d"))) {queryDictionary[new Guid("5ecb637e-b725-4497-917a-ef2ac6139b1d")].SubQueries = subQueryDictionary[new Guid("5ecb637e-b725-4497-917a-ef2ac6139b1d")];}
		if(!subQueryDictionary.ContainsKey(new Guid("ee3db83c-fcb7-4b72-85ab-b264ab79710a"))) {subQueryDictionary[new Guid("ee3db83c-fcb7-4b72-85ab-b264ab79710a")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("ee3db83c-fcb7-4b72-85ab-b264ab79710a")].Add(queryDictionary[new Guid("5ecb637e-b725-4497-917a-ef2ac6139b1d")]);
		}
		{
		queryDictionary[new Guid("83def500-765d-4b83-93e5-b01ac122426b")] = new InputRecordListQuery();
		queryDictionary[new Guid("83def500-765d-4b83-93e5-b01ac122426b")].FieldName = null;
		queryDictionary[new Guid("83def500-765d-4b83-93e5-b01ac122426b")].FieldValue =  null;
		queryDictionary[new Guid("83def500-765d-4b83-93e5-b01ac122426b")].QueryType = "AND";
		queryDictionary[new Guid("83def500-765d-4b83-93e5-b01ac122426b")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("0746ad1a-5978-4f39-8343-3bd1f82d7ead")] = new InputRecordListQuery();
			queryDictionary[new Guid("0746ad1a-5978-4f39-8343-3bd1f82d7ead")].FieldName = "code";
			queryDictionary[new Guid("0746ad1a-5978-4f39-8343-3bd1f82d7ead")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"number\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("0746ad1a-5978-4f39-8343-3bd1f82d7ead")].QueryType = "CONTAINS";
			queryDictionary[new Guid("0746ad1a-5978-4f39-8343-3bd1f82d7ead")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("0746ad1a-5978-4f39-8343-3bd1f82d7ead"))) {queryDictionary[new Guid("0746ad1a-5978-4f39-8343-3bd1f82d7ead")].SubQueries = subQueryDictionary[new Guid("0746ad1a-5978-4f39-8343-3bd1f82d7ead")];}
			if(!subQueryDictionary.ContainsKey(new Guid("83def500-765d-4b83-93e5-b01ac122426b"))) {subQueryDictionary[new Guid("83def500-765d-4b83-93e5-b01ac122426b")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("83def500-765d-4b83-93e5-b01ac122426b")].Add(queryDictionary[new Guid("0746ad1a-5978-4f39-8343-3bd1f82d7ead")]);
			}
			{
			queryDictionary[new Guid("7defdbba-e95e-499b-8a17-4623c6daaa74")] = new InputRecordListQuery();
			queryDictionary[new Guid("7defdbba-e95e-499b-8a17-4623c6daaa74")].FieldName = "subject";
			queryDictionary[new Guid("7defdbba-e95e-499b-8a17-4623c6daaa74")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("7defdbba-e95e-499b-8a17-4623c6daaa74")].QueryType = "CONTAINS";
			queryDictionary[new Guid("7defdbba-e95e-499b-8a17-4623c6daaa74")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("7defdbba-e95e-499b-8a17-4623c6daaa74"))) {queryDictionary[new Guid("7defdbba-e95e-499b-8a17-4623c6daaa74")].SubQueries = subQueryDictionary[new Guid("7defdbba-e95e-499b-8a17-4623c6daaa74")];}
			if(!subQueryDictionary.ContainsKey(new Guid("83def500-765d-4b83-93e5-b01ac122426b"))) {subQueryDictionary[new Guid("83def500-765d-4b83-93e5-b01ac122426b")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("83def500-765d-4b83-93e5-b01ac122426b")].Add(queryDictionary[new Guid("7defdbba-e95e-499b-8a17-4623c6daaa74")]);
			}
			{
			queryDictionary[new Guid("997a6005-2ed3-4c3b-aa98-22d49de71ed3")] = new InputRecordListQuery();
			queryDictionary[new Guid("997a6005-2ed3-4c3b-aa98-22d49de71ed3")].FieldName = "status";
			queryDictionary[new Guid("997a6005-2ed3-4c3b-aa98-22d49de71ed3")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("997a6005-2ed3-4c3b-aa98-22d49de71ed3")].QueryType = "EQ";
			queryDictionary[new Guid("997a6005-2ed3-4c3b-aa98-22d49de71ed3")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("997a6005-2ed3-4c3b-aa98-22d49de71ed3"))) {queryDictionary[new Guid("997a6005-2ed3-4c3b-aa98-22d49de71ed3")].SubQueries = subQueryDictionary[new Guid("997a6005-2ed3-4c3b-aa98-22d49de71ed3")];}
			if(!subQueryDictionary.ContainsKey(new Guid("83def500-765d-4b83-93e5-b01ac122426b"))) {subQueryDictionary[new Guid("83def500-765d-4b83-93e5-b01ac122426b")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("83def500-765d-4b83-93e5-b01ac122426b")].Add(queryDictionary[new Guid("997a6005-2ed3-4c3b-aa98-22d49de71ed3")]);
			}
			{
			queryDictionary[new Guid("097200a2-4200-41bd-a10e-d4cad0051442")] = new InputRecordListQuery();
			queryDictionary[new Guid("097200a2-4200-41bd-a10e-d4cad0051442")].FieldName = "priority";
			queryDictionary[new Guid("097200a2-4200-41bd-a10e-d4cad0051442")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("097200a2-4200-41bd-a10e-d4cad0051442")].QueryType = "EQ";
			queryDictionary[new Guid("097200a2-4200-41bd-a10e-d4cad0051442")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("097200a2-4200-41bd-a10e-d4cad0051442"))) {queryDictionary[new Guid("097200a2-4200-41bd-a10e-d4cad0051442")].SubQueries = subQueryDictionary[new Guid("097200a2-4200-41bd-a10e-d4cad0051442")];}
			if(!subQueryDictionary.ContainsKey(new Guid("83def500-765d-4b83-93e5-b01ac122426b"))) {subQueryDictionary[new Guid("83def500-765d-4b83-93e5-b01ac122426b")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("83def500-765d-4b83-93e5-b01ac122426b")].Add(queryDictionary[new Guid("097200a2-4200-41bd-a10e-d4cad0051442")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("83def500-765d-4b83-93e5-b01ac122426b"))) {queryDictionary[new Guid("83def500-765d-4b83-93e5-b01ac122426b")].SubQueries = subQueryDictionary[new Guid("83def500-765d-4b83-93e5-b01ac122426b")];}
		if(!subQueryDictionary.ContainsKey(new Guid("ee3db83c-fcb7-4b72-85ab-b264ab79710a"))) {subQueryDictionary[new Guid("ee3db83c-fcb7-4b72-85ab-b264ab79710a")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("ee3db83c-fcb7-4b72-85ab-b264ab79710a")].Add(queryDictionary[new Guid("83def500-765d-4b83-93e5-b01ac122426b")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("ee3db83c-fcb7-4b72-85ab-b264ab79710a"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("ee3db83c-fcb7-4b72-85ab-b264ab79710a")];}
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

		#region << sort >>
		{
			var sort = new InputRecordListSort();
			sort.FieldName = "{\"name\":\"url_sort\", \"option\": \"sortBy\", \"default\": \"end_date\", \"settings\":{\"order\":\"sortOrder\"}}";
			sort.SortType = "Descending";
			createListInput.Sorts.Add(sort);
		}
		#endregion

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_task Updated list: owned_tasks Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_task name: all_tasks >>
{
	var createListEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "all_tasks").Id;
	createListInput.Type =  "General";
	createListInput.Name = "all_tasks";
	createListInput.Label = "All Tasks";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("12.0");
	createListInput.Default = false;
	createListInput.System = true;
	createListInput.CssClass = "task-list";
	createListInput.IconName = "tasks";
	createListInput.VisibleColumnsCount = Int32.Parse("7");
	createListInput.ColumnWidthsCSV = "100px,auto,30px,120px,120px,120px,120px";
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = "/plugins/webvella-projects/api/task/list/all";
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record, ngCtrl)}}"">
											<i class=""fa fa-fw fa-eye""></i>
											</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
									ng-href=""{{::ngCtrl.getRecordCreateUrl(ngCtrl)}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

		#region << code >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("04f31ad8-4583-4237-8d54-f82c3f44b918");
			listField.FieldName = "code";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << subject >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("7843bfbd-30c1-4438-af48-ffe56b7f294a");
			listField.FieldName = "subject";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << field from Relation: image >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("bf199b74-4448-4f58-93f5-6b86d888843b");
			listItemFromRelation.FieldName = "image";
			listItemFromRelation.FieldLabel = "Owner";
			listItemFromRelation.FieldPlaceholder = null;
			listItemFromRelation.FieldHelpText = null;
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = null;
			listItemFromRelation.RelationId = new Guid("7ce76c81-e604-401e-907f-23de982b930e");
			listItemFromRelation.RelationName = "user_1_n_task_owner";
			createListInput.Columns.Add(listItemFromRelation);
		}
		#endregion
		#region << start_date >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("5bf852bf-5e6c-4791-bc8b-f1366440c04b");
			listField.FieldName = "start_date";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << end_date >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("c6dc9db2-0081-4a91-b0e2-78f9c1c45a51");
			listField.FieldName = "end_date";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << status >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("0702e611-6fe5-42e4-9bad-d549cba9cbb1");
			listField.FieldName = "status";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << priority >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("04ede478-99ec-4f7f-97af-0df3e89409b1");
			listField.FieldName = "priority";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = new InputRecordListQuery();
	var queryDictionary = new Dictionary<Guid,InputRecordListQuery>();
	var subQueryDictionary = new Dictionary<Guid,List<InputRecordListQuery>>();
	//Main query rule
	createListInput.Query.FieldName = null;
	createListInput.Query.FieldValue =  null;
	createListInput.Query.QueryType = "AND";
	createListInput.Query.SubQueries = new List<InputRecordListQuery>();
		{
		queryDictionary[new Guid("5fd3e5cc-1aec-4a6f-9b79-2107fba82d62")] = new InputRecordListQuery();
		queryDictionary[new Guid("5fd3e5cc-1aec-4a6f-9b79-2107fba82d62")].FieldName = "code";
		queryDictionary[new Guid("5fd3e5cc-1aec-4a6f-9b79-2107fba82d62")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("5fd3e5cc-1aec-4a6f-9b79-2107fba82d62")].QueryType = "CONTAINS";
		queryDictionary[new Guid("5fd3e5cc-1aec-4a6f-9b79-2107fba82d62")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("5fd3e5cc-1aec-4a6f-9b79-2107fba82d62"))) {queryDictionary[new Guid("5fd3e5cc-1aec-4a6f-9b79-2107fba82d62")].SubQueries = subQueryDictionary[new Guid("5fd3e5cc-1aec-4a6f-9b79-2107fba82d62")];}
		if(!subQueryDictionary.ContainsKey(new Guid("123339a1-f79b-4c11-b8a4-f318679b9047"))) {subQueryDictionary[new Guid("123339a1-f79b-4c11-b8a4-f318679b9047")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("123339a1-f79b-4c11-b8a4-f318679b9047")].Add(queryDictionary[new Guid("5fd3e5cc-1aec-4a6f-9b79-2107fba82d62")]);
		}
		{
		queryDictionary[new Guid("0527b5fd-63d8-4435-b836-fb63fe5f571a")] = new InputRecordListQuery();
		queryDictionary[new Guid("0527b5fd-63d8-4435-b836-fb63fe5f571a")].FieldName = "subject";
		queryDictionary[new Guid("0527b5fd-63d8-4435-b836-fb63fe5f571a")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("0527b5fd-63d8-4435-b836-fb63fe5f571a")].QueryType = "CONTAINS";
		queryDictionary[new Guid("0527b5fd-63d8-4435-b836-fb63fe5f571a")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("0527b5fd-63d8-4435-b836-fb63fe5f571a"))) {queryDictionary[new Guid("0527b5fd-63d8-4435-b836-fb63fe5f571a")].SubQueries = subQueryDictionary[new Guid("0527b5fd-63d8-4435-b836-fb63fe5f571a")];}
		if(!subQueryDictionary.ContainsKey(new Guid("123339a1-f79b-4c11-b8a4-f318679b9047"))) {subQueryDictionary[new Guid("123339a1-f79b-4c11-b8a4-f318679b9047")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("123339a1-f79b-4c11-b8a4-f318679b9047")].Add(queryDictionary[new Guid("0527b5fd-63d8-4435-b836-fb63fe5f571a")]);
		}
		{
		queryDictionary[new Guid("e0c446da-a55a-41bb-acd5-10d7d156047d")] = new InputRecordListQuery();
		queryDictionary[new Guid("e0c446da-a55a-41bb-acd5-10d7d156047d")].FieldName = "status";
		queryDictionary[new Guid("e0c446da-a55a-41bb-acd5-10d7d156047d")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("e0c446da-a55a-41bb-acd5-10d7d156047d")].QueryType = "EQ";
		queryDictionary[new Guid("e0c446da-a55a-41bb-acd5-10d7d156047d")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("e0c446da-a55a-41bb-acd5-10d7d156047d"))) {queryDictionary[new Guid("e0c446da-a55a-41bb-acd5-10d7d156047d")].SubQueries = subQueryDictionary[new Guid("e0c446da-a55a-41bb-acd5-10d7d156047d")];}
		if(!subQueryDictionary.ContainsKey(new Guid("123339a1-f79b-4c11-b8a4-f318679b9047"))) {subQueryDictionary[new Guid("123339a1-f79b-4c11-b8a4-f318679b9047")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("123339a1-f79b-4c11-b8a4-f318679b9047")].Add(queryDictionary[new Guid("e0c446da-a55a-41bb-acd5-10d7d156047d")]);
		}
		{
		queryDictionary[new Guid("604c4d26-0072-4f95-b8ff-9fefe0e2e6be")] = new InputRecordListQuery();
		queryDictionary[new Guid("604c4d26-0072-4f95-b8ff-9fefe0e2e6be")].FieldName = "priority";
		queryDictionary[new Guid("604c4d26-0072-4f95-b8ff-9fefe0e2e6be")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("604c4d26-0072-4f95-b8ff-9fefe0e2e6be")].QueryType = "EQ";
		queryDictionary[new Guid("604c4d26-0072-4f95-b8ff-9fefe0e2e6be")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("604c4d26-0072-4f95-b8ff-9fefe0e2e6be"))) {queryDictionary[new Guid("604c4d26-0072-4f95-b8ff-9fefe0e2e6be")].SubQueries = subQueryDictionary[new Guid("604c4d26-0072-4f95-b8ff-9fefe0e2e6be")];}
		if(!subQueryDictionary.ContainsKey(new Guid("123339a1-f79b-4c11-b8a4-f318679b9047"))) {subQueryDictionary[new Guid("123339a1-f79b-4c11-b8a4-f318679b9047")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("123339a1-f79b-4c11-b8a4-f318679b9047")].Add(queryDictionary[new Guid("604c4d26-0072-4f95-b8ff-9fefe0e2e6be")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("123339a1-f79b-4c11-b8a4-f318679b9047"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("123339a1-f79b-4c11-b8a4-f318679b9047")];}
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

		#region << sort >>
		{
			var sort = new InputRecordListSort();
			sort.FieldName = "{\"name\":\"url_sort\", \"option\": \"sortBy\", \"default\": \"end_date\", \"settings\":{\"order\":\"sortOrder\"}}";
			sort.SortType = "Descending";
			createListInput.Sorts.Add(sort);
		}
		#endregion

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_task Updated list: all_tasks Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_task name: admin >>
{
	var createListEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "admin").Id;
	createListInput.Type =  "Hidden";
	createListInput.Name = "admin";
	createListInput.Label = "All tasks";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("2.0");
	createListInput.Default = false;
	createListInput.System = true;
	createListInput.CssClass = "task-list";
	createListInput.IconName = "tasks";
	createListInput.VisibleColumnsCount = Int32.Parse("7");
	createListInput.ColumnWidthsCSV = "80px,auto,30px,120px,120px,120px,120px";
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record, ngCtrl)}}"">
					<i class=""fa fa-fw fa-eye""></i>
					</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
									ng-href=""{{::ngCtrl.getRecordCreateUrl(ngCtrl)}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

		#region << code >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("04f31ad8-4583-4237-8d54-f82c3f44b918");
			listField.FieldName = "code";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << subject >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("7843bfbd-30c1-4438-af48-ffe56b7f294a");
			listField.FieldName = "subject";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << field from Relation: image >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("bf199b74-4448-4f58-93f5-6b86d888843b");
			listItemFromRelation.FieldName = "image";
			listItemFromRelation.FieldLabel = "Owner";
			listItemFromRelation.FieldPlaceholder = null;
			listItemFromRelation.FieldHelpText = null;
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = null;
			listItemFromRelation.RelationId = new Guid("7ce76c81-e604-401e-907f-23de982b930e");
			listItemFromRelation.RelationName = "user_1_n_task_owner";
			createListInput.Columns.Add(listItemFromRelation);
		}
		#endregion
		#region << start_date >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("5bf852bf-5e6c-4791-bc8b-f1366440c04b");
			listField.FieldName = "start_date";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << end_date >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("c6dc9db2-0081-4a91-b0e2-78f9c1c45a51");
			listField.FieldName = "end_date";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << status >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("0702e611-6fe5-42e4-9bad-d549cba9cbb1");
			listField.FieldName = "status";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << priority >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("04ede478-99ec-4f7f-97af-0df3e89409b1");
			listField.FieldName = "priority";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = new InputRecordListQuery();
	var queryDictionary = new Dictionary<Guid,InputRecordListQuery>();
	var subQueryDictionary = new Dictionary<Guid,List<InputRecordListQuery>>();
	//Main query rule
	createListInput.Query.FieldName = null;
	createListInput.Query.FieldValue =  null;
	createListInput.Query.QueryType = "AND";
	createListInput.Query.SubQueries = new List<InputRecordListQuery>();
		{
		queryDictionary[new Guid("7f547926-4346-4b9b-adad-c761e790e5ff")] = new InputRecordListQuery();
		queryDictionary[new Guid("7f547926-4346-4b9b-adad-c761e790e5ff")].FieldName = "owner_id";
		queryDictionary[new Guid("7f547926-4346-4b9b-adad-c761e790e5ff")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("7f547926-4346-4b9b-adad-c761e790e5ff")].QueryType = "EQ";
		queryDictionary[new Guid("7f547926-4346-4b9b-adad-c761e790e5ff")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("7f547926-4346-4b9b-adad-c761e790e5ff"))) {queryDictionary[new Guid("7f547926-4346-4b9b-adad-c761e790e5ff")].SubQueries = subQueryDictionary[new Guid("7f547926-4346-4b9b-adad-c761e790e5ff")];}
		if(!subQueryDictionary.ContainsKey(new Guid("218a2879-abb3-44ac-a753-6b9b59052f89"))) {subQueryDictionary[new Guid("218a2879-abb3-44ac-a753-6b9b59052f89")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("218a2879-abb3-44ac-a753-6b9b59052f89")].Add(queryDictionary[new Guid("7f547926-4346-4b9b-adad-c761e790e5ff")]);
		}
		{
		queryDictionary[new Guid("1ca0efc2-904b-4197-a94d-f58b1edf13f6")] = new InputRecordListQuery();
		queryDictionary[new Guid("1ca0efc2-904b-4197-a94d-f58b1edf13f6")].FieldName = "subject";
		queryDictionary[new Guid("1ca0efc2-904b-4197-a94d-f58b1edf13f6")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("1ca0efc2-904b-4197-a94d-f58b1edf13f6")].QueryType = "EQ";
		queryDictionary[new Guid("1ca0efc2-904b-4197-a94d-f58b1edf13f6")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("1ca0efc2-904b-4197-a94d-f58b1edf13f6"))) {queryDictionary[new Guid("1ca0efc2-904b-4197-a94d-f58b1edf13f6")].SubQueries = subQueryDictionary[new Guid("1ca0efc2-904b-4197-a94d-f58b1edf13f6")];}
		if(!subQueryDictionary.ContainsKey(new Guid("218a2879-abb3-44ac-a753-6b9b59052f89"))) {subQueryDictionary[new Guid("218a2879-abb3-44ac-a753-6b9b59052f89")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("218a2879-abb3-44ac-a753-6b9b59052f89")].Add(queryDictionary[new Guid("1ca0efc2-904b-4197-a94d-f58b1edf13f6")]);
		}
		{
		queryDictionary[new Guid("1b114123-a050-4e45-a4bc-dccc7e209e3f")] = new InputRecordListQuery();
		queryDictionary[new Guid("1b114123-a050-4e45-a4bc-dccc7e209e3f")].FieldName = "status";
		queryDictionary[new Guid("1b114123-a050-4e45-a4bc-dccc7e209e3f")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("1b114123-a050-4e45-a4bc-dccc7e209e3f")].QueryType = "EQ";
		queryDictionary[new Guid("1b114123-a050-4e45-a4bc-dccc7e209e3f")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("1b114123-a050-4e45-a4bc-dccc7e209e3f"))) {queryDictionary[new Guid("1b114123-a050-4e45-a4bc-dccc7e209e3f")].SubQueries = subQueryDictionary[new Guid("1b114123-a050-4e45-a4bc-dccc7e209e3f")];}
		if(!subQueryDictionary.ContainsKey(new Guid("218a2879-abb3-44ac-a753-6b9b59052f89"))) {subQueryDictionary[new Guid("218a2879-abb3-44ac-a753-6b9b59052f89")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("218a2879-abb3-44ac-a753-6b9b59052f89")].Add(queryDictionary[new Guid("1b114123-a050-4e45-a4bc-dccc7e209e3f")]);
		}
		{
		queryDictionary[new Guid("b3c62375-bc5f-49aa-95b6-681e52047ad6")] = new InputRecordListQuery();
		queryDictionary[new Guid("b3c62375-bc5f-49aa-95b6-681e52047ad6")].FieldName = "priority";
		queryDictionary[new Guid("b3c62375-bc5f-49aa-95b6-681e52047ad6")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("b3c62375-bc5f-49aa-95b6-681e52047ad6")].QueryType = "EQ";
		queryDictionary[new Guid("b3c62375-bc5f-49aa-95b6-681e52047ad6")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("b3c62375-bc5f-49aa-95b6-681e52047ad6"))) {queryDictionary[new Guid("b3c62375-bc5f-49aa-95b6-681e52047ad6")].SubQueries = subQueryDictionary[new Guid("b3c62375-bc5f-49aa-95b6-681e52047ad6")];}
		if(!subQueryDictionary.ContainsKey(new Guid("218a2879-abb3-44ac-a753-6b9b59052f89"))) {subQueryDictionary[new Guid("218a2879-abb3-44ac-a753-6b9b59052f89")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("218a2879-abb3-44ac-a753-6b9b59052f89")].Add(queryDictionary[new Guid("b3c62375-bc5f-49aa-95b6-681e52047ad6")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("218a2879-abb3-44ac-a753-6b9b59052f89"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("218a2879-abb3-44ac-a753-6b9b59052f89")];}
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

		#region << sort >>
		{
			var sort = new InputRecordListSort();
			sort.FieldName = "{\"name\":\"url_sort\", \"option\": \"sortBy\", \"default\": \"end_date\", \"settings\":{\"order\":\"sortOrder\"}}";
			sort.SortType = "Descending";
			createListInput.Sorts.Add(sort);
		}
		#endregion

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_task Updated list: admin Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_task name: my_tasks >>
{
	var createListEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "my_tasks").Id;
	createListInput.Type =  "General";
	createListInput.Name = "my_tasks";
	createListInput.Label = "My Owned Active Tasks";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("1.0");
	createListInput.Default = false;
	createListInput.System = true;
	createListInput.CssClass = "task-list";
	createListInput.IconName = "tasks";
	createListInput.VisibleColumnsCount = Int32.Parse("7");
	createListInput.ColumnWidthsCSV = "100px,auto,30px,120px,120px,120px,120px";
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record, ngCtrl)}}"">
					<i class=""fa fa-fw fa-eye""></i>
					</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
									ng-href=""{{::ngCtrl.getRecordCreateUrl(ngCtrl)}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

		#region << code >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("04f31ad8-4583-4237-8d54-f82c3f44b918");
			listField.FieldName = "code";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << subject >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("7843bfbd-30c1-4438-af48-ffe56b7f294a");
			listField.FieldName = "subject";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << field from Relation: image >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("bf199b74-4448-4f58-93f5-6b86d888843b");
			listItemFromRelation.FieldName = "image";
			listItemFromRelation.FieldLabel = "Owner";
			listItemFromRelation.FieldPlaceholder = null;
			listItemFromRelation.FieldHelpText = null;
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = null;
			listItemFromRelation.RelationId = new Guid("7ce76c81-e604-401e-907f-23de982b930e");
			listItemFromRelation.RelationName = "user_1_n_task_owner";
			createListInput.Columns.Add(listItemFromRelation);
		}
		#endregion
		#region << start_date >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("5bf852bf-5e6c-4791-bc8b-f1366440c04b");
			listField.FieldName = "start_date";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << end_date >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("c6dc9db2-0081-4a91-b0e2-78f9c1c45a51");
			listField.FieldName = "end_date";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << status >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("0702e611-6fe5-42e4-9bad-d549cba9cbb1");
			listField.FieldName = "status";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << priority >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			listField.EntityName = "wv_task";
			listField.FieldId = new Guid("04ede478-99ec-4f7f-97af-0df3e89409b1");
			listField.FieldName = "priority";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = new InputRecordListQuery();
	var queryDictionary = new Dictionary<Guid,InputRecordListQuery>();
	var subQueryDictionary = new Dictionary<Guid,List<InputRecordListQuery>>();
	//Main query rule
	createListInput.Query.FieldName = null;
	createListInput.Query.FieldValue =  null;
	createListInput.Query.QueryType = "AND";
	createListInput.Query.SubQueries = new List<InputRecordListQuery>();
		{
		queryDictionary[new Guid("805b33af-ef11-46ab-bb27-6b1f6de23257")] = new InputRecordListQuery();
		queryDictionary[new Guid("805b33af-ef11-46ab-bb27-6b1f6de23257")].FieldName = null;
		queryDictionary[new Guid("805b33af-ef11-46ab-bb27-6b1f6de23257")].FieldValue =  null;
		queryDictionary[new Guid("805b33af-ef11-46ab-bb27-6b1f6de23257")].QueryType = "AND";
		queryDictionary[new Guid("805b33af-ef11-46ab-bb27-6b1f6de23257")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("27248095-bf45-4214-8ee6-a0e5ab849b5e")] = new InputRecordListQuery();
			queryDictionary[new Guid("27248095-bf45-4214-8ee6-a0e5ab849b5e")].FieldName = "owner_id";
			queryDictionary[new Guid("27248095-bf45-4214-8ee6-a0e5ab849b5e")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("27248095-bf45-4214-8ee6-a0e5ab849b5e")].QueryType = "EQ";
			queryDictionary[new Guid("27248095-bf45-4214-8ee6-a0e5ab849b5e")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("27248095-bf45-4214-8ee6-a0e5ab849b5e"))) {queryDictionary[new Guid("27248095-bf45-4214-8ee6-a0e5ab849b5e")].SubQueries = subQueryDictionary[new Guid("27248095-bf45-4214-8ee6-a0e5ab849b5e")];}
			if(!subQueryDictionary.ContainsKey(new Guid("805b33af-ef11-46ab-bb27-6b1f6de23257"))) {subQueryDictionary[new Guid("805b33af-ef11-46ab-bb27-6b1f6de23257")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("805b33af-ef11-46ab-bb27-6b1f6de23257")].Add(queryDictionary[new Guid("27248095-bf45-4214-8ee6-a0e5ab849b5e")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("805b33af-ef11-46ab-bb27-6b1f6de23257"))) {queryDictionary[new Guid("805b33af-ef11-46ab-bb27-6b1f6de23257")].SubQueries = subQueryDictionary[new Guid("805b33af-ef11-46ab-bb27-6b1f6de23257")];}
		if(!subQueryDictionary.ContainsKey(new Guid("af7adbe9-9516-4f1c-8af1-8fabdfb8cfd9"))) {subQueryDictionary[new Guid("af7adbe9-9516-4f1c-8af1-8fabdfb8cfd9")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("af7adbe9-9516-4f1c-8af1-8fabdfb8cfd9")].Add(queryDictionary[new Guid("805b33af-ef11-46ab-bb27-6b1f6de23257")]);
		}
		{
		queryDictionary[new Guid("31ed38fa-9ba0-44b9-80c8-6f17393a8e6c")] = new InputRecordListQuery();
		queryDictionary[new Guid("31ed38fa-9ba0-44b9-80c8-6f17393a8e6c")].FieldName = null;
		queryDictionary[new Guid("31ed38fa-9ba0-44b9-80c8-6f17393a8e6c")].FieldValue =  null;
		queryDictionary[new Guid("31ed38fa-9ba0-44b9-80c8-6f17393a8e6c")].QueryType = "AND";
		queryDictionary[new Guid("31ed38fa-9ba0-44b9-80c8-6f17393a8e6c")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("d2b4ab7c-3906-4c95-85e9-37056b80c5fb")] = new InputRecordListQuery();
			queryDictionary[new Guid("d2b4ab7c-3906-4c95-85e9-37056b80c5fb")].FieldName = "code";
			queryDictionary[new Guid("d2b4ab7c-3906-4c95-85e9-37056b80c5fb")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("d2b4ab7c-3906-4c95-85e9-37056b80c5fb")].QueryType = "CONTAINS";
			queryDictionary[new Guid("d2b4ab7c-3906-4c95-85e9-37056b80c5fb")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("d2b4ab7c-3906-4c95-85e9-37056b80c5fb"))) {queryDictionary[new Guid("d2b4ab7c-3906-4c95-85e9-37056b80c5fb")].SubQueries = subQueryDictionary[new Guid("d2b4ab7c-3906-4c95-85e9-37056b80c5fb")];}
			if(!subQueryDictionary.ContainsKey(new Guid("31ed38fa-9ba0-44b9-80c8-6f17393a8e6c"))) {subQueryDictionary[new Guid("31ed38fa-9ba0-44b9-80c8-6f17393a8e6c")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("31ed38fa-9ba0-44b9-80c8-6f17393a8e6c")].Add(queryDictionary[new Guid("d2b4ab7c-3906-4c95-85e9-37056b80c5fb")]);
			}
			{
			queryDictionary[new Guid("68f18ec6-e3ab-48d1-bf6f-7d2f2ea81184")] = new InputRecordListQuery();
			queryDictionary[new Guid("68f18ec6-e3ab-48d1-bf6f-7d2f2ea81184")].FieldName = "subject";
			queryDictionary[new Guid("68f18ec6-e3ab-48d1-bf6f-7d2f2ea81184")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("68f18ec6-e3ab-48d1-bf6f-7d2f2ea81184")].QueryType = "CONTAINS";
			queryDictionary[new Guid("68f18ec6-e3ab-48d1-bf6f-7d2f2ea81184")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("68f18ec6-e3ab-48d1-bf6f-7d2f2ea81184"))) {queryDictionary[new Guid("68f18ec6-e3ab-48d1-bf6f-7d2f2ea81184")].SubQueries = subQueryDictionary[new Guid("68f18ec6-e3ab-48d1-bf6f-7d2f2ea81184")];}
			if(!subQueryDictionary.ContainsKey(new Guid("31ed38fa-9ba0-44b9-80c8-6f17393a8e6c"))) {subQueryDictionary[new Guid("31ed38fa-9ba0-44b9-80c8-6f17393a8e6c")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("31ed38fa-9ba0-44b9-80c8-6f17393a8e6c")].Add(queryDictionary[new Guid("68f18ec6-e3ab-48d1-bf6f-7d2f2ea81184")]);
			}
			{
			queryDictionary[new Guid("7710c4ee-f74b-495c-8aba-00683fec6c17")] = new InputRecordListQuery();
			queryDictionary[new Guid("7710c4ee-f74b-495c-8aba-00683fec6c17")].FieldName = "status";
			queryDictionary[new Guid("7710c4ee-f74b-495c-8aba-00683fec6c17")].FieldValue =  "completed";
			queryDictionary[new Guid("7710c4ee-f74b-495c-8aba-00683fec6c17")].QueryType = "NOT";
			queryDictionary[new Guid("7710c4ee-f74b-495c-8aba-00683fec6c17")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("7710c4ee-f74b-495c-8aba-00683fec6c17"))) {queryDictionary[new Guid("7710c4ee-f74b-495c-8aba-00683fec6c17")].SubQueries = subQueryDictionary[new Guid("7710c4ee-f74b-495c-8aba-00683fec6c17")];}
			if(!subQueryDictionary.ContainsKey(new Guid("31ed38fa-9ba0-44b9-80c8-6f17393a8e6c"))) {subQueryDictionary[new Guid("31ed38fa-9ba0-44b9-80c8-6f17393a8e6c")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("31ed38fa-9ba0-44b9-80c8-6f17393a8e6c")].Add(queryDictionary[new Guid("7710c4ee-f74b-495c-8aba-00683fec6c17")]);
			}
			{
			queryDictionary[new Guid("1c2c1bcb-d3a9-4344-9db8-2474e22e9419")] = new InputRecordListQuery();
			queryDictionary[new Guid("1c2c1bcb-d3a9-4344-9db8-2474e22e9419")].FieldName = "priority";
			queryDictionary[new Guid("1c2c1bcb-d3a9-4344-9db8-2474e22e9419")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("1c2c1bcb-d3a9-4344-9db8-2474e22e9419")].QueryType = "EQ";
			queryDictionary[new Guid("1c2c1bcb-d3a9-4344-9db8-2474e22e9419")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("1c2c1bcb-d3a9-4344-9db8-2474e22e9419"))) {queryDictionary[new Guid("1c2c1bcb-d3a9-4344-9db8-2474e22e9419")].SubQueries = subQueryDictionary[new Guid("1c2c1bcb-d3a9-4344-9db8-2474e22e9419")];}
			if(!subQueryDictionary.ContainsKey(new Guid("31ed38fa-9ba0-44b9-80c8-6f17393a8e6c"))) {subQueryDictionary[new Guid("31ed38fa-9ba0-44b9-80c8-6f17393a8e6c")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("31ed38fa-9ba0-44b9-80c8-6f17393a8e6c")].Add(queryDictionary[new Guid("1c2c1bcb-d3a9-4344-9db8-2474e22e9419")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("31ed38fa-9ba0-44b9-80c8-6f17393a8e6c"))) {queryDictionary[new Guid("31ed38fa-9ba0-44b9-80c8-6f17393a8e6c")].SubQueries = subQueryDictionary[new Guid("31ed38fa-9ba0-44b9-80c8-6f17393a8e6c")];}
		if(!subQueryDictionary.ContainsKey(new Guid("af7adbe9-9516-4f1c-8af1-8fabdfb8cfd9"))) {subQueryDictionary[new Guid("af7adbe9-9516-4f1c-8af1-8fabdfb8cfd9")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("af7adbe9-9516-4f1c-8af1-8fabdfb8cfd9")].Add(queryDictionary[new Guid("31ed38fa-9ba0-44b9-80c8-6f17393a8e6c")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("af7adbe9-9516-4f1c-8af1-8fabdfb8cfd9"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("af7adbe9-9516-4f1c-8af1-8fabdfb8cfd9")];}
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

		#region << sort >>
		{
			var sort = new InputRecordListSort();
			sort.FieldName = "{\"name\":\"url_sort\", \"option\": \"sortBy\", \"default\": \"end_date\", \"settings\":{\"order\":\"sortOrder\"}}";
			sort.SortType = "Descending";
			createListInput.Sorts.Add(sort);
		}
		#endregion

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_task Updated list: my_tasks Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_bug field: priority >>
{
	var currentEntity = entMan.ReadEntity(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c")).Object;
	InputSelectField dropdownField = new InputSelectField();
	dropdownField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "priority").Id;
	dropdownField.Name = "priority";
	dropdownField.Label = "Priority";
	dropdownField.PlaceholderText = "";
	dropdownField.Description = "";
	dropdownField.HelpText = "";
	dropdownField.Required = true;
	dropdownField.Unique = false;
	dropdownField.Searchable = true;
	dropdownField.Auditable = false;
	dropdownField.System = true;
	dropdownField.DefaultValue = "medium";
	dropdownField.Options = new List<SelectFieldOption>
	{
		new SelectFieldOption() { Key = "low", Value = "low"},
		new SelectFieldOption() { Key = "medium", Value = "medium"},
		new SelectFieldOption() { Key = "high", Value = "high"}
	};
	dropdownField.EnableSecurity = true;
	dropdownField.Permissions = new FieldPermissions();
	dropdownField.Permissions.CanRead = new List<Guid>();
	dropdownField.Permissions.CanUpdate = new List<Guid>();
	//READ
	dropdownField.Permissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
	dropdownField.Permissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
	//UPDATE
	dropdownField.Permissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
	dropdownField.Permissions.CanUpdate.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
	{
		var response = entMan.UpdateField(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c"), dropdownField);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_bug Field: priority Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_bug View: quick_view >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "quick_view").Id;
	createViewInput.Type = "Quick_View";
	createViewInput.Name = "quick_view";
	createViewInput.Label = "Quick view";
	createViewInput.Title = "";
	createViewInput.Default = true;
	createViewInput.System = false;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "file-text-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("10.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""::ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
		ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
	<i class=""fa fa-trash go-red""></i> Delete Record
</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_bug Updated view: quick_view Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_bug View: general >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "general").Id;
	createViewInput.Type = "General";
	createViewInput.Name = "general";
	createViewInput.Label = "[{code}] {subject}";
	createViewInput.Title = "";
	createViewInput.Default = true;
	createViewInput.System = false;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "bug";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = "";
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("10.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			#region << Section: details >>
			{
			var viewSection = new InputRecordViewSection();
			viewSection.Id = new Guid("b3679dee-d30d-46d7-b5ac-300ed8f1e922");
			viewSection.Name = "details";
			viewSection.Label = "Details";
			viewSection.ShowLabel = false;
			viewSection.CssClass = "";
			viewSection.Collapsed = false;
			viewSection.TabOrder = "left-right";
			viewSection.Weight = Decimal.Parse("1.0");
			viewSection.Rows = new List<InputRecordViewRow>();

				#region << Row 1>>
				{
					var viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("f9099d26-96ad-4fe2-9c81-db7a8f5daa47");
					viewRow.Weight = Decimal.Parse("1.0");
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("8");
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << subject >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
						viewItem.EntityName = "wv_bug";
						viewItem.FieldId = new Guid("335a4500-130e-4739-b774-2f53f33ea22a");
						viewItem.FieldName = "subject";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					#region << description >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
						viewItem.EntityName = "wv_bug";
						viewItem.FieldId = new Guid("4afe9621-39ee-40b9-a3ef-cb9b98131a6a");
						viewItem.FieldName = "description";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					#region << List from relation: bug_comments >>
					{
						var viewItemFromRelation = new InputRecordViewRelationListItem();
						viewItemFromRelation.EntityId = new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99");
						viewItemFromRelation.EntityName = "wv_project_comment";
						viewItemFromRelation.ListId = new Guid("b143b82f-b79f-47c1-87e7-ecba6f6f2a32");
						viewItemFromRelation.ListName = "bug_comments";
						viewItemFromRelation.FieldLabel = "Comments";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = "";
						viewItemFromRelation.FieldRequired = false;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = new Guid("5af026bd-d046-42ba-b6a0-e9090727348f");
						viewItemFromRelation.RelationName = "bug_1_n_comment";
						viewItemFromRelation.Type = "listFromRelation";
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion
					//Save column
					viewRow.Columns.Add(viewColumn);
					}
					#endregion
					#region << Column 2 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("4");
					viewColumn.Items = new List<InputRecordViewItemBase>();


					#region << status >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
						viewItem.EntityName = "wv_bug";
						viewItem.FieldId = new Guid("48be1e6e-df47-46f0-b4e1-6e9e1cbaf71c");
						viewItem.FieldName = "status";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					#region << priority >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
						viewItem.EntityName = "wv_bug";
						viewItem.FieldId = new Guid("e506dfc7-6d49-4d00-9f61-8befd56c1a6e");
						viewItem.FieldName = "priority";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					#region << field from Relation: name >>
					{
						var viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32");
						viewItemFromRelation.EntityName = "wv_project";
						viewItemFromRelation.Type = "fieldFromRelation";
						viewItemFromRelation.FieldId = new Guid("d13427b7-e518-4305-b2cc-bc814a299b55");
						viewItemFromRelation.FieldName = "name";
						viewItemFromRelation.FieldLabel = "Project";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = null;
						viewItemFromRelation.FieldRequired = true;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = new Guid("d94f100c-024c-47e7-af32-d67a49be2b6c");
						viewItemFromRelation.RelationName = "project_1_n_bug";
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion
					#region << field from Relation: username >>
					{
						var viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
						viewItemFromRelation.EntityName = "user";
						viewItemFromRelation.Type = "fieldFromRelation";
						viewItemFromRelation.FieldId = new Guid("263c0b21-88c1-4c2b-80b4-db7402b0d2e2");
						viewItemFromRelation.FieldName = "username";
						viewItemFromRelation.FieldLabel = "Owner";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = null;
						viewItemFromRelation.FieldRequired = true;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = new Guid("cddc10b6-30ff-4a86-96e4-645b3ea59fd9");
						viewItemFromRelation.RelationName = "user_1_n_bug_owner";
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion
					#region << field from Relation: username >>
					{
						var viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
						viewItemFromRelation.EntityName = "user";
						viewItemFromRelation.Type = "fieldFromRelation";
						viewItemFromRelation.FieldId = new Guid("263c0b21-88c1-4c2b-80b4-db7402b0d2e2");
						viewItemFromRelation.FieldName = "username";
						viewItemFromRelation.FieldLabel = "Watchers";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = null;
						viewItemFromRelation.FieldRequired = true;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = new Guid("b71d0c52-1626-48da-91bc-e10999ba79b8");
						viewItemFromRelation.RelationName = "user_n_n_bug_watchers";
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion
					//Save column
					viewRow.Columns.Add(viewColumn);
					}
					#endregion
				//Save row
				viewSection.Rows.Add(viewRow);
				}
				#endregion
			//Save section
			viewRegion.Sections.Add(viewSection);
			}
			#endregion
			#region << Section: hidden >>
			{
			var viewSection = new InputRecordViewSection();
			viewSection.Id = new Guid("b8bbe88a-04d0-46a9-85a4-0601c8f46847");
			viewSection.Name = "hidden";
			viewSection.Label = "Hidden";
			viewSection.ShowLabel = false;
			viewSection.CssClass = "ng-hide";
			viewSection.Collapsed = false;
			viewSection.TabOrder = "left-right";
			viewSection.Weight = Decimal.Parse("2.0");
			viewSection.Rows = new List<InputRecordViewRow>();

				#region << Row 1>>
				{
					var viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("ca91c7a6-070c-4960-9d82-9a21d75b6180");
					viewRow.Weight = Decimal.Parse("1.0");
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("12");
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << code >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
						viewItem.EntityName = "wv_bug";
						viewItem.FieldId = new Guid("6f2030e9-edd7-42ac-bb2a-2766b76c3da1");
						viewItem.FieldName = "code";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion

					//Save column
					viewRow.Columns.Add(viewColumn);
					}
					#endregion
				//Save row
				viewSection.Rows.Add(viewRow);
				}
				#endregion
			//Save section
			viewRegion.Sections.Add(viewSection);
			}
			#endregion
			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""::ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
		ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
	<i class=""fa fa-trash go-red""></i> Delete Record
</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

		#region << list from relation: bug_attachments >>
		{
			var viewItemFromRelation = new InputRecordViewSidebarRelationListItem();
			viewItemFromRelation.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			viewItemFromRelation.EntityName = "wv_bug";
			viewItemFromRelation.ListId = new Guid("2b83e4e3-6878-4b5b-9391-6e59429c0b5e");
			viewItemFromRelation.ListName ="bug_attachments";
						viewItemFromRelation.FieldLabel = "Attachments";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = "";
			viewItemFromRelation.FieldRequired = false;
			viewItemFromRelation.FieldManageView = "general";
			viewItemFromRelation.FieldLookupList = "lookup";
			viewItemFromRelation.RelationId = new Guid("a4f60f87-66a9-4541-a2ef-29e00f2b418b");
			viewItemFromRelation.RelationName = "bug_1_n_attachment";
			viewItemFromRelation.Type = "listFromRelation";
			createViewInput.Sidebar.Items.Add(viewItemFromRelation);
		}
		#endregion

		#region << list from relation: bug_timelogs >>
		{
			var viewItemFromRelation = new InputRecordViewSidebarRelationListItem();
			viewItemFromRelation.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			viewItemFromRelation.EntityName = "wv_bug";
			viewItemFromRelation.ListId = new Guid("f9a12626-08db-4fd2-a443-b521162be2b5");
			viewItemFromRelation.ListName ="bug_timelogs";
						viewItemFromRelation.FieldLabel = "Time logs";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = "";
			viewItemFromRelation.FieldRequired = false;
			viewItemFromRelation.FieldManageView = "general";
			viewItemFromRelation.FieldLookupList = "lookup";
			viewItemFromRelation.RelationId = new Guid("97909e49-50d4-4534-aa7b-61c523b55d87");
			viewItemFromRelation.RelationName = "bug_1_n_time_log";
			viewItemFromRelation.Type = "listFromRelation";
			createViewInput.Sidebar.Items.Add(viewItemFromRelation);
		}
		#endregion

		#region << list from relation: bug_activities >>
		{
			var viewItemFromRelation = new InputRecordViewSidebarRelationListItem();
			viewItemFromRelation.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			viewItemFromRelation.EntityName = "wv_bug";
			viewItemFromRelation.ListId = new Guid("57c3062c-df6e-488a-a432-dd927b0dd013");
			viewItemFromRelation.ListName ="bug_activities";
						viewItemFromRelation.FieldLabel = "Activities";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = "";
			viewItemFromRelation.FieldRequired = false;
			viewItemFromRelation.FieldManageView = "general";
			viewItemFromRelation.FieldLookupList = "lookup";
			viewItemFromRelation.RelationId = new Guid("b96189f7-a880-4da4-b9a9-2274a9745d2d");
			viewItemFromRelation.RelationName = "bug_1_n_activity";
			viewItemFromRelation.Type = "listFromRelation";
			createViewInput.Sidebar.Items.Add(viewItemFromRelation);
		}
		#endregion

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_bug Updated view: general Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_bug name: lookup >>
{
	var createListEntity = entMan.ReadEntity(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "lookup").Id;
	createListInput.Type =  "Lookup";
	createListInput.Name = "lookup";
	createListInput.Label = "Lookup";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("10.0");
	createListInput.Default = true;
	createListInput.System = false;
	createListInput.CssClass = null;
	createListInput.IconName = "list";
	createListInput.VisibleColumnsCount = Int32.Parse("5");
	createListInput.ColumnWidthsCSV = null;
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
    ng-href=""{{::ngCtrl.getRecordCreateUrl()}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_import_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_import_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("10.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openImportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-upload""></i> Import CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_export_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_export_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("11.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openExportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-download""></i> Export CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record)}}"">
    <i class=""fa fa-fw fa-eye""></i>
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = null;
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_bug Updated list: lookup Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_bug name: project_bugs >>
{
	var createListEntity = entMan.ReadEntity(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "project_bugs").Id;
	createListInput.Type =  "Hidden";
	createListInput.Name = "project_bugs";
	createListInput.Label = "Bugs";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("10.0");
	createListInput.Default = true;
	createListInput.System = false;
	createListInput.CssClass = "bug-list";
	createListInput.IconName = "bug";
	createListInput.VisibleColumnsCount = Int32.Parse("5");
	createListInput.ColumnWidthsCSV = "auto,30px,120px,120px,120px";
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
    ng-href=""{{::ngCtrl.getRecordCreateUrl()}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_import_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_import_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("10.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openImportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-upload""></i> Import CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_export_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_export_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("11.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openExportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-download""></i> Export CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record)}}"">
    <i class=""fa fa-fw fa-eye""></i>
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

		#region << subject >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			listField.EntityName = "wv_bug";
			listField.FieldId = new Guid("335a4500-130e-4739-b774-2f53f33ea22a");
			listField.FieldName = "subject";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << field from Relation: image >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("bf199b74-4448-4f58-93f5-6b86d888843b");
			listItemFromRelation.FieldName = "image";
			listItemFromRelation.FieldLabel = "Owner";
			listItemFromRelation.FieldPlaceholder = null;
			listItemFromRelation.FieldHelpText = null;
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = null;
			listItemFromRelation.RelationId = new Guid("cddc10b6-30ff-4a86-96e4-645b3ea59fd9");
			listItemFromRelation.RelationName = "user_1_n_bug_owner";
			createListInput.Columns.Add(listItemFromRelation);
		}
		#endregion
		#region << created_on >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			listField.EntityName = "wv_bug";
			listField.FieldId = new Guid("781cee71-1632-4bf9-83b1-ff122d29eb2a");
			listField.FieldName = "created_on";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << status >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			listField.EntityName = "wv_bug";
			listField.FieldId = new Guid("48be1e6e-df47-46f0-b4e1-6e9e1cbaf71c");
			listField.FieldName = "status";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << priority >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			listField.EntityName = "wv_bug";
			listField.FieldId = new Guid("e506dfc7-6d49-4d00-9f61-8befd56c1a6e");
			listField.FieldName = "priority";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = new InputRecordListQuery();
	var queryDictionary = new Dictionary<Guid,InputRecordListQuery>();
	var subQueryDictionary = new Dictionary<Guid,List<InputRecordListQuery>>();
	//Main query rule
	createListInput.Query.FieldName = null;
	createListInput.Query.FieldValue =  null;
	createListInput.Query.QueryType = "AND";
	createListInput.Query.SubQueries = new List<InputRecordListQuery>();
		{
		queryDictionary[new Guid("1ba01da3-8d02-40ab-b2fb-306a07c90062")] = new InputRecordListQuery();
		queryDictionary[new Guid("1ba01da3-8d02-40ab-b2fb-306a07c90062")].FieldName = "subject";
		queryDictionary[new Guid("1ba01da3-8d02-40ab-b2fb-306a07c90062")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("1ba01da3-8d02-40ab-b2fb-306a07c90062")].QueryType = "CONTAINS";
		queryDictionary[new Guid("1ba01da3-8d02-40ab-b2fb-306a07c90062")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("1ba01da3-8d02-40ab-b2fb-306a07c90062"))) {queryDictionary[new Guid("1ba01da3-8d02-40ab-b2fb-306a07c90062")].SubQueries = subQueryDictionary[new Guid("1ba01da3-8d02-40ab-b2fb-306a07c90062")];}
		if(!subQueryDictionary.ContainsKey(new Guid("24f5b8dc-0169-4b53-b8bc-cdf925a1e7d2"))) {subQueryDictionary[new Guid("24f5b8dc-0169-4b53-b8bc-cdf925a1e7d2")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("24f5b8dc-0169-4b53-b8bc-cdf925a1e7d2")].Add(queryDictionary[new Guid("1ba01da3-8d02-40ab-b2fb-306a07c90062")]);
		}
		{
		queryDictionary[new Guid("66e5b348-d558-4e28-9c8f-637aadfc8b75")] = new InputRecordListQuery();
		queryDictionary[new Guid("66e5b348-d558-4e28-9c8f-637aadfc8b75")].FieldName = "status";
		queryDictionary[new Guid("66e5b348-d558-4e28-9c8f-637aadfc8b75")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("66e5b348-d558-4e28-9c8f-637aadfc8b75")].QueryType = "EQ";
		queryDictionary[new Guid("66e5b348-d558-4e28-9c8f-637aadfc8b75")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("66e5b348-d558-4e28-9c8f-637aadfc8b75"))) {queryDictionary[new Guid("66e5b348-d558-4e28-9c8f-637aadfc8b75")].SubQueries = subQueryDictionary[new Guid("66e5b348-d558-4e28-9c8f-637aadfc8b75")];}
		if(!subQueryDictionary.ContainsKey(new Guid("24f5b8dc-0169-4b53-b8bc-cdf925a1e7d2"))) {subQueryDictionary[new Guid("24f5b8dc-0169-4b53-b8bc-cdf925a1e7d2")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("24f5b8dc-0169-4b53-b8bc-cdf925a1e7d2")].Add(queryDictionary[new Guid("66e5b348-d558-4e28-9c8f-637aadfc8b75")]);
		}
		{
		queryDictionary[new Guid("4fb3eb3e-b6ed-423e-9445-5c11008b2894")] = new InputRecordListQuery();
		queryDictionary[new Guid("4fb3eb3e-b6ed-423e-9445-5c11008b2894")].FieldName = "priority";
		queryDictionary[new Guid("4fb3eb3e-b6ed-423e-9445-5c11008b2894")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("4fb3eb3e-b6ed-423e-9445-5c11008b2894")].QueryType = "EQ";
		queryDictionary[new Guid("4fb3eb3e-b6ed-423e-9445-5c11008b2894")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("4fb3eb3e-b6ed-423e-9445-5c11008b2894"))) {queryDictionary[new Guid("4fb3eb3e-b6ed-423e-9445-5c11008b2894")].SubQueries = subQueryDictionary[new Guid("4fb3eb3e-b6ed-423e-9445-5c11008b2894")];}
		if(!subQueryDictionary.ContainsKey(new Guid("24f5b8dc-0169-4b53-b8bc-cdf925a1e7d2"))) {subQueryDictionary[new Guid("24f5b8dc-0169-4b53-b8bc-cdf925a1e7d2")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("24f5b8dc-0169-4b53-b8bc-cdf925a1e7d2")].Add(queryDictionary[new Guid("4fb3eb3e-b6ed-423e-9445-5c11008b2894")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("24f5b8dc-0169-4b53-b8bc-cdf925a1e7d2"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("24f5b8dc-0169-4b53-b8bc-cdf925a1e7d2")];}
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

		#region << sort >>
		{
			var sort = new InputRecordListSort();
			sort.FieldName = "{\"name\":\"url_sort\", \"option\": \"sortBy\", \"default\": \"created_on\", \"settings\":{\"order\":\"sortOrder\"}}";
			sort.SortType = "Descending";
			createListInput.Sorts.Add(sort);
		}
		#endregion

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_bug Updated list: project_bugs Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_bug name: created_bugs >>
{
	var createListEntity = entMan.ReadEntity(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "created_bugs").Id;
	createListInput.Type =  "General";
	createListInput.Name = "created_bugs";
	createListInput.Label = "Bugs created by me";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("3.0");
	createListInput.Default = false;
	createListInput.System = true;
	createListInput.CssClass = "bug-list";
	createListInput.IconName = "bug";
	createListInput.VisibleColumnsCount = Int32.Parse("7");
	createListInput.ColumnWidthsCSV = "100px,auto,30px,120px,120px,120px,120px";
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record, ngCtrl)}}"">
					<i class=""fa fa-fw fa-eye""></i>
					</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
									ng-href=""{{::ngCtrl.getRecordCreateUrl(ngCtrl)}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

		#region << code >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			listField.EntityName = "wv_bug";
			listField.FieldId = new Guid("6f2030e9-edd7-42ac-bb2a-2766b76c3da1");
			listField.FieldName = "code";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << subject >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			listField.EntityName = "wv_bug";
			listField.FieldId = new Guid("335a4500-130e-4739-b774-2f53f33ea22a");
			listField.FieldName = "subject";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << field from Relation: image >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("bf199b74-4448-4f58-93f5-6b86d888843b");
			listItemFromRelation.FieldName = "image";
			listItemFromRelation.FieldLabel = "Owner";
			listItemFromRelation.FieldPlaceholder = null;
			listItemFromRelation.FieldHelpText = null;
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = null;
			listItemFromRelation.RelationId = new Guid("cddc10b6-30ff-4a86-96e4-645b3ea59fd9");
			listItemFromRelation.RelationName = "user_1_n_bug_owner";
			createListInput.Columns.Add(listItemFromRelation);
		}
		#endregion
		#region << status >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			listField.EntityName = "wv_bug";
			listField.FieldId = new Guid("48be1e6e-df47-46f0-b4e1-6e9e1cbaf71c");
			listField.FieldName = "status";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << priority >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			listField.EntityName = "wv_bug";
			listField.FieldId = new Guid("e506dfc7-6d49-4d00-9f61-8befd56c1a6e");
			listField.FieldName = "priority";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = new InputRecordListQuery();
	var queryDictionary = new Dictionary<Guid,InputRecordListQuery>();
	var subQueryDictionary = new Dictionary<Guid,List<InputRecordListQuery>>();
	//Main query rule
	createListInput.Query.FieldName = null;
	createListInput.Query.FieldValue =  null;
	createListInput.Query.QueryType = "AND";
	createListInput.Query.SubQueries = new List<InputRecordListQuery>();
		{
		queryDictionary[new Guid("207d3650-8999-480b-9f5c-33d4acd73c56")] = new InputRecordListQuery();
		queryDictionary[new Guid("207d3650-8999-480b-9f5c-33d4acd73c56")].FieldName = null;
		queryDictionary[new Guid("207d3650-8999-480b-9f5c-33d4acd73c56")].FieldValue =  null;
		queryDictionary[new Guid("207d3650-8999-480b-9f5c-33d4acd73c56")].QueryType = "AND";
		queryDictionary[new Guid("207d3650-8999-480b-9f5c-33d4acd73c56")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("0c541998-4a94-4d13-8e09-b2ccdfff36bb")] = new InputRecordListQuery();
			queryDictionary[new Guid("0c541998-4a94-4d13-8e09-b2ccdfff36bb")].FieldName = "created_by";
			queryDictionary[new Guid("0c541998-4a94-4d13-8e09-b2ccdfff36bb")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("0c541998-4a94-4d13-8e09-b2ccdfff36bb")].QueryType = "EQ";
			queryDictionary[new Guid("0c541998-4a94-4d13-8e09-b2ccdfff36bb")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("0c541998-4a94-4d13-8e09-b2ccdfff36bb"))) {queryDictionary[new Guid("0c541998-4a94-4d13-8e09-b2ccdfff36bb")].SubQueries = subQueryDictionary[new Guid("0c541998-4a94-4d13-8e09-b2ccdfff36bb")];}
			if(!subQueryDictionary.ContainsKey(new Guid("207d3650-8999-480b-9f5c-33d4acd73c56"))) {subQueryDictionary[new Guid("207d3650-8999-480b-9f5c-33d4acd73c56")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("207d3650-8999-480b-9f5c-33d4acd73c56")].Add(queryDictionary[new Guid("0c541998-4a94-4d13-8e09-b2ccdfff36bb")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("207d3650-8999-480b-9f5c-33d4acd73c56"))) {queryDictionary[new Guid("207d3650-8999-480b-9f5c-33d4acd73c56")].SubQueries = subQueryDictionary[new Guid("207d3650-8999-480b-9f5c-33d4acd73c56")];}
		if(!subQueryDictionary.ContainsKey(new Guid("f1ce9115-6ff6-43e5-86a1-58de50318aaf"))) {subQueryDictionary[new Guid("f1ce9115-6ff6-43e5-86a1-58de50318aaf")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("f1ce9115-6ff6-43e5-86a1-58de50318aaf")].Add(queryDictionary[new Guid("207d3650-8999-480b-9f5c-33d4acd73c56")]);
		}
		{
		queryDictionary[new Guid("5197f754-3f12-4536-9953-62f829ef614a")] = new InputRecordListQuery();
		queryDictionary[new Guid("5197f754-3f12-4536-9953-62f829ef614a")].FieldName = null;
		queryDictionary[new Guid("5197f754-3f12-4536-9953-62f829ef614a")].FieldValue =  null;
		queryDictionary[new Guid("5197f754-3f12-4536-9953-62f829ef614a")].QueryType = "AND";
		queryDictionary[new Guid("5197f754-3f12-4536-9953-62f829ef614a")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("cceda48b-d874-4bbe-941f-e4eb0ccac495")] = new InputRecordListQuery();
			queryDictionary[new Guid("cceda48b-d874-4bbe-941f-e4eb0ccac495")].FieldName = "code";
			queryDictionary[new Guid("cceda48b-d874-4bbe-941f-e4eb0ccac495")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("cceda48b-d874-4bbe-941f-e4eb0ccac495")].QueryType = "CONTAINS";
			queryDictionary[new Guid("cceda48b-d874-4bbe-941f-e4eb0ccac495")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("cceda48b-d874-4bbe-941f-e4eb0ccac495"))) {queryDictionary[new Guid("cceda48b-d874-4bbe-941f-e4eb0ccac495")].SubQueries = subQueryDictionary[new Guid("cceda48b-d874-4bbe-941f-e4eb0ccac495")];}
			if(!subQueryDictionary.ContainsKey(new Guid("5197f754-3f12-4536-9953-62f829ef614a"))) {subQueryDictionary[new Guid("5197f754-3f12-4536-9953-62f829ef614a")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("5197f754-3f12-4536-9953-62f829ef614a")].Add(queryDictionary[new Guid("cceda48b-d874-4bbe-941f-e4eb0ccac495")]);
			}
			{
			queryDictionary[new Guid("b5af40f9-dd64-42a7-8da0-cded75680295")] = new InputRecordListQuery();
			queryDictionary[new Guid("b5af40f9-dd64-42a7-8da0-cded75680295")].FieldName = "subject";
			queryDictionary[new Guid("b5af40f9-dd64-42a7-8da0-cded75680295")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("b5af40f9-dd64-42a7-8da0-cded75680295")].QueryType = "CONTAINS";
			queryDictionary[new Guid("b5af40f9-dd64-42a7-8da0-cded75680295")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("b5af40f9-dd64-42a7-8da0-cded75680295"))) {queryDictionary[new Guid("b5af40f9-dd64-42a7-8da0-cded75680295")].SubQueries = subQueryDictionary[new Guid("b5af40f9-dd64-42a7-8da0-cded75680295")];}
			if(!subQueryDictionary.ContainsKey(new Guid("5197f754-3f12-4536-9953-62f829ef614a"))) {subQueryDictionary[new Guid("5197f754-3f12-4536-9953-62f829ef614a")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("5197f754-3f12-4536-9953-62f829ef614a")].Add(queryDictionary[new Guid("b5af40f9-dd64-42a7-8da0-cded75680295")]);
			}
			{
			queryDictionary[new Guid("171686a8-198a-4305-9fb8-5b23b840c421")] = new InputRecordListQuery();
			queryDictionary[new Guid("171686a8-198a-4305-9fb8-5b23b840c421")].FieldName = "status";
			queryDictionary[new Guid("171686a8-198a-4305-9fb8-5b23b840c421")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("171686a8-198a-4305-9fb8-5b23b840c421")].QueryType = "EQ";
			queryDictionary[new Guid("171686a8-198a-4305-9fb8-5b23b840c421")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("171686a8-198a-4305-9fb8-5b23b840c421"))) {queryDictionary[new Guid("171686a8-198a-4305-9fb8-5b23b840c421")].SubQueries = subQueryDictionary[new Guid("171686a8-198a-4305-9fb8-5b23b840c421")];}
			if(!subQueryDictionary.ContainsKey(new Guid("5197f754-3f12-4536-9953-62f829ef614a"))) {subQueryDictionary[new Guid("5197f754-3f12-4536-9953-62f829ef614a")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("5197f754-3f12-4536-9953-62f829ef614a")].Add(queryDictionary[new Guid("171686a8-198a-4305-9fb8-5b23b840c421")]);
			}
			{
			queryDictionary[new Guid("1ca6e6e9-b815-40f7-b456-14422d52fc7d")] = new InputRecordListQuery();
			queryDictionary[new Guid("1ca6e6e9-b815-40f7-b456-14422d52fc7d")].FieldName = "priority";
			queryDictionary[new Guid("1ca6e6e9-b815-40f7-b456-14422d52fc7d")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("1ca6e6e9-b815-40f7-b456-14422d52fc7d")].QueryType = "EQ";
			queryDictionary[new Guid("1ca6e6e9-b815-40f7-b456-14422d52fc7d")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("1ca6e6e9-b815-40f7-b456-14422d52fc7d"))) {queryDictionary[new Guid("1ca6e6e9-b815-40f7-b456-14422d52fc7d")].SubQueries = subQueryDictionary[new Guid("1ca6e6e9-b815-40f7-b456-14422d52fc7d")];}
			if(!subQueryDictionary.ContainsKey(new Guid("5197f754-3f12-4536-9953-62f829ef614a"))) {subQueryDictionary[new Guid("5197f754-3f12-4536-9953-62f829ef614a")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("5197f754-3f12-4536-9953-62f829ef614a")].Add(queryDictionary[new Guid("1ca6e6e9-b815-40f7-b456-14422d52fc7d")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("5197f754-3f12-4536-9953-62f829ef614a"))) {queryDictionary[new Guid("5197f754-3f12-4536-9953-62f829ef614a")].SubQueries = subQueryDictionary[new Guid("5197f754-3f12-4536-9953-62f829ef614a")];}
		if(!subQueryDictionary.ContainsKey(new Guid("f1ce9115-6ff6-43e5-86a1-58de50318aaf"))) {subQueryDictionary[new Guid("f1ce9115-6ff6-43e5-86a1-58de50318aaf")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("f1ce9115-6ff6-43e5-86a1-58de50318aaf")].Add(queryDictionary[new Guid("5197f754-3f12-4536-9953-62f829ef614a")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("f1ce9115-6ff6-43e5-86a1-58de50318aaf"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("f1ce9115-6ff6-43e5-86a1-58de50318aaf")];}
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

		#region << sort >>
		{
			var sort = new InputRecordListSort();
			sort.FieldName = "{\"name\":\"url_sort\", \"option\": \"sortBy\", \"default\": \"created_on\", \"settings\":{\"order\":\"sortOrder\"}}";
			sort.SortType = "Ascending";
			createListInput.Sorts.Add(sort);
		}
		#endregion

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_bug Updated list: created_bugs Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_bug name: owned_bugs >>
{
	var createListEntity = entMan.ReadEntity(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "owned_bugs").Id;
	createListInput.Type =  "General";
	createListInput.Name = "owned_bugs";
	createListInput.Label = "Bugs owned by me";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("2.0");
	createListInput.Default = false;
	createListInput.System = true;
	createListInput.CssClass = "bug-list";
	createListInput.IconName = "bug";
	createListInput.VisibleColumnsCount = Int32.Parse("7");
	createListInput.ColumnWidthsCSV = "100px,auto,30px,120px,120px,120px,120px";
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record, ngCtrl)}}"">
					<i class=""fa fa-fw fa-eye""></i>
					</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
									ng-href=""{{::ngCtrl.getRecordCreateUrl(ngCtrl)}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

		#region << code >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			listField.EntityName = "wv_bug";
			listField.FieldId = new Guid("6f2030e9-edd7-42ac-bb2a-2766b76c3da1");
			listField.FieldName = "code";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << subject >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			listField.EntityName = "wv_bug";
			listField.FieldId = new Guid("335a4500-130e-4739-b774-2f53f33ea22a");
			listField.FieldName = "subject";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << field from Relation: image >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("bf199b74-4448-4f58-93f5-6b86d888843b");
			listItemFromRelation.FieldName = "image";
			listItemFromRelation.FieldLabel = "Owner";
			listItemFromRelation.FieldPlaceholder = null;
			listItemFromRelation.FieldHelpText = null;
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = null;
			listItemFromRelation.RelationId = new Guid("cddc10b6-30ff-4a86-96e4-645b3ea59fd9");
			listItemFromRelation.RelationName = "user_1_n_bug_owner";
			createListInput.Columns.Add(listItemFromRelation);
		}
		#endregion
		#region << status >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			listField.EntityName = "wv_bug";
			listField.FieldId = new Guid("48be1e6e-df47-46f0-b4e1-6e9e1cbaf71c");
			listField.FieldName = "status";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << priority >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			listField.EntityName = "wv_bug";
			listField.FieldId = new Guid("e506dfc7-6d49-4d00-9f61-8befd56c1a6e");
			listField.FieldName = "priority";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = new InputRecordListQuery();
	var queryDictionary = new Dictionary<Guid,InputRecordListQuery>();
	var subQueryDictionary = new Dictionary<Guid,List<InputRecordListQuery>>();
	//Main query rule
	createListInput.Query.FieldName = null;
	createListInput.Query.FieldValue =  null;
	createListInput.Query.QueryType = "AND";
	createListInput.Query.SubQueries = new List<InputRecordListQuery>();
		{
		queryDictionary[new Guid("274701f4-53a9-43ca-9fb1-e4627172fbe7")] = new InputRecordListQuery();
		queryDictionary[new Guid("274701f4-53a9-43ca-9fb1-e4627172fbe7")].FieldName = null;
		queryDictionary[new Guid("274701f4-53a9-43ca-9fb1-e4627172fbe7")].FieldValue =  null;
		queryDictionary[new Guid("274701f4-53a9-43ca-9fb1-e4627172fbe7")].QueryType = "OR";
		queryDictionary[new Guid("274701f4-53a9-43ca-9fb1-e4627172fbe7")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("7fb3e9a4-b032-40c9-b5ec-8fc2d48988b7")] = new InputRecordListQuery();
			queryDictionary[new Guid("7fb3e9a4-b032-40c9-b5ec-8fc2d48988b7")].FieldName = "owner_id";
			queryDictionary[new Guid("7fb3e9a4-b032-40c9-b5ec-8fc2d48988b7")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("7fb3e9a4-b032-40c9-b5ec-8fc2d48988b7")].QueryType = "EQ";
			queryDictionary[new Guid("7fb3e9a4-b032-40c9-b5ec-8fc2d48988b7")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("7fb3e9a4-b032-40c9-b5ec-8fc2d48988b7"))) {queryDictionary[new Guid("7fb3e9a4-b032-40c9-b5ec-8fc2d48988b7")].SubQueries = subQueryDictionary[new Guid("7fb3e9a4-b032-40c9-b5ec-8fc2d48988b7")];}
			if(!subQueryDictionary.ContainsKey(new Guid("274701f4-53a9-43ca-9fb1-e4627172fbe7"))) {subQueryDictionary[new Guid("274701f4-53a9-43ca-9fb1-e4627172fbe7")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("274701f4-53a9-43ca-9fb1-e4627172fbe7")].Add(queryDictionary[new Guid("7fb3e9a4-b032-40c9-b5ec-8fc2d48988b7")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("274701f4-53a9-43ca-9fb1-e4627172fbe7"))) {queryDictionary[new Guid("274701f4-53a9-43ca-9fb1-e4627172fbe7")].SubQueries = subQueryDictionary[new Guid("274701f4-53a9-43ca-9fb1-e4627172fbe7")];}
		if(!subQueryDictionary.ContainsKey(new Guid("611142c7-30ab-48d7-b167-73aab641e088"))) {subQueryDictionary[new Guid("611142c7-30ab-48d7-b167-73aab641e088")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("611142c7-30ab-48d7-b167-73aab641e088")].Add(queryDictionary[new Guid("274701f4-53a9-43ca-9fb1-e4627172fbe7")]);
		}
		{
		queryDictionary[new Guid("061e671e-0060-42e0-9ab0-102ba85ec9c6")] = new InputRecordListQuery();
		queryDictionary[new Guid("061e671e-0060-42e0-9ab0-102ba85ec9c6")].FieldName = null;
		queryDictionary[new Guid("061e671e-0060-42e0-9ab0-102ba85ec9c6")].FieldValue =  null;
		queryDictionary[new Guid("061e671e-0060-42e0-9ab0-102ba85ec9c6")].QueryType = "AND";
		queryDictionary[new Guid("061e671e-0060-42e0-9ab0-102ba85ec9c6")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("bb88cafc-a18e-443a-a4a5-eea6863ea07f")] = new InputRecordListQuery();
			queryDictionary[new Guid("bb88cafc-a18e-443a-a4a5-eea6863ea07f")].FieldName = "code";
			queryDictionary[new Guid("bb88cafc-a18e-443a-a4a5-eea6863ea07f")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("bb88cafc-a18e-443a-a4a5-eea6863ea07f")].QueryType = "CONTAINS";
			queryDictionary[new Guid("bb88cafc-a18e-443a-a4a5-eea6863ea07f")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("bb88cafc-a18e-443a-a4a5-eea6863ea07f"))) {queryDictionary[new Guid("bb88cafc-a18e-443a-a4a5-eea6863ea07f")].SubQueries = subQueryDictionary[new Guid("bb88cafc-a18e-443a-a4a5-eea6863ea07f")];}
			if(!subQueryDictionary.ContainsKey(new Guid("061e671e-0060-42e0-9ab0-102ba85ec9c6"))) {subQueryDictionary[new Guid("061e671e-0060-42e0-9ab0-102ba85ec9c6")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("061e671e-0060-42e0-9ab0-102ba85ec9c6")].Add(queryDictionary[new Guid("bb88cafc-a18e-443a-a4a5-eea6863ea07f")]);
			}
			{
			queryDictionary[new Guid("6da87ce3-d8fd-4b24-b5b2-0f6291981630")] = new InputRecordListQuery();
			queryDictionary[new Guid("6da87ce3-d8fd-4b24-b5b2-0f6291981630")].FieldName = "subject";
			queryDictionary[new Guid("6da87ce3-d8fd-4b24-b5b2-0f6291981630")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("6da87ce3-d8fd-4b24-b5b2-0f6291981630")].QueryType = "CONTAINS";
			queryDictionary[new Guid("6da87ce3-d8fd-4b24-b5b2-0f6291981630")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("6da87ce3-d8fd-4b24-b5b2-0f6291981630"))) {queryDictionary[new Guid("6da87ce3-d8fd-4b24-b5b2-0f6291981630")].SubQueries = subQueryDictionary[new Guid("6da87ce3-d8fd-4b24-b5b2-0f6291981630")];}
			if(!subQueryDictionary.ContainsKey(new Guid("061e671e-0060-42e0-9ab0-102ba85ec9c6"))) {subQueryDictionary[new Guid("061e671e-0060-42e0-9ab0-102ba85ec9c6")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("061e671e-0060-42e0-9ab0-102ba85ec9c6")].Add(queryDictionary[new Guid("6da87ce3-d8fd-4b24-b5b2-0f6291981630")]);
			}
			{
			queryDictionary[new Guid("703b1415-d807-4fa2-b667-e5f27f002a55")] = new InputRecordListQuery();
			queryDictionary[new Guid("703b1415-d807-4fa2-b667-e5f27f002a55")].FieldName = "status";
			queryDictionary[new Guid("703b1415-d807-4fa2-b667-e5f27f002a55")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("703b1415-d807-4fa2-b667-e5f27f002a55")].QueryType = "EQ";
			queryDictionary[new Guid("703b1415-d807-4fa2-b667-e5f27f002a55")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("703b1415-d807-4fa2-b667-e5f27f002a55"))) {queryDictionary[new Guid("703b1415-d807-4fa2-b667-e5f27f002a55")].SubQueries = subQueryDictionary[new Guid("703b1415-d807-4fa2-b667-e5f27f002a55")];}
			if(!subQueryDictionary.ContainsKey(new Guid("061e671e-0060-42e0-9ab0-102ba85ec9c6"))) {subQueryDictionary[new Guid("061e671e-0060-42e0-9ab0-102ba85ec9c6")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("061e671e-0060-42e0-9ab0-102ba85ec9c6")].Add(queryDictionary[new Guid("703b1415-d807-4fa2-b667-e5f27f002a55")]);
			}
			{
			queryDictionary[new Guid("5adffff0-13ca-456e-bd66-3f658e3d5942")] = new InputRecordListQuery();
			queryDictionary[new Guid("5adffff0-13ca-456e-bd66-3f658e3d5942")].FieldName = "priority";
			queryDictionary[new Guid("5adffff0-13ca-456e-bd66-3f658e3d5942")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("5adffff0-13ca-456e-bd66-3f658e3d5942")].QueryType = "EQ";
			queryDictionary[new Guid("5adffff0-13ca-456e-bd66-3f658e3d5942")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("5adffff0-13ca-456e-bd66-3f658e3d5942"))) {queryDictionary[new Guid("5adffff0-13ca-456e-bd66-3f658e3d5942")].SubQueries = subQueryDictionary[new Guid("5adffff0-13ca-456e-bd66-3f658e3d5942")];}
			if(!subQueryDictionary.ContainsKey(new Guid("061e671e-0060-42e0-9ab0-102ba85ec9c6"))) {subQueryDictionary[new Guid("061e671e-0060-42e0-9ab0-102ba85ec9c6")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("061e671e-0060-42e0-9ab0-102ba85ec9c6")].Add(queryDictionary[new Guid("5adffff0-13ca-456e-bd66-3f658e3d5942")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("061e671e-0060-42e0-9ab0-102ba85ec9c6"))) {queryDictionary[new Guid("061e671e-0060-42e0-9ab0-102ba85ec9c6")].SubQueries = subQueryDictionary[new Guid("061e671e-0060-42e0-9ab0-102ba85ec9c6")];}
		if(!subQueryDictionary.ContainsKey(new Guid("611142c7-30ab-48d7-b167-73aab641e088"))) {subQueryDictionary[new Guid("611142c7-30ab-48d7-b167-73aab641e088")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("611142c7-30ab-48d7-b167-73aab641e088")].Add(queryDictionary[new Guid("061e671e-0060-42e0-9ab0-102ba85ec9c6")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("611142c7-30ab-48d7-b167-73aab641e088"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("611142c7-30ab-48d7-b167-73aab641e088")];}
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

		#region << sort >>
		{
			var sort = new InputRecordListSort();
			sort.FieldName = "{\"name\":\"url_sort\", \"option\": \"sortBy\", \"default\": \"created_on\", \"settings\":{\"order\":\"sortOrder\"}}";
			sort.SortType = "Ascending";
			createListInput.Sorts.Add(sort);
		}
		#endregion

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_bug Updated list: owned_bugs Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_bug name: all_bugs >>
{
	var createListEntity = entMan.ReadEntity(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "all_bugs").Id;
	createListInput.Type =  "General";
	createListInput.Name = "all_bugs";
	createListInput.Label = "All Bugs";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("12.0");
	createListInput.Default = false;
	createListInput.System = true;
	createListInput.CssClass = "bug-list";
	createListInput.IconName = "bug";
	createListInput.VisibleColumnsCount = Int32.Parse("7");
	createListInput.ColumnWidthsCSV = "100px,auto,30px,120px,120px,120px,120px";
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = "/plugins/webvella-projects/api/bug/list/all";
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record, ngCtrl)}}"">
											<i class=""fa fa-fw fa-eye""></i>
											</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
									ng-href=""{{::ngCtrl.getRecordCreateUrl(ngCtrl)}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

		#region << code >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			listField.EntityName = "wv_bug";
			listField.FieldId = new Guid("6f2030e9-edd7-42ac-bb2a-2766b76c3da1");
			listField.FieldName = "code";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << subject >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			listField.EntityName = "wv_bug";
			listField.FieldId = new Guid("335a4500-130e-4739-b774-2f53f33ea22a");
			listField.FieldName = "subject";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << field from Relation: image >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("bf199b74-4448-4f58-93f5-6b86d888843b");
			listItemFromRelation.FieldName = "image";
			listItemFromRelation.FieldLabel = "Owner";
			listItemFromRelation.FieldPlaceholder = null;
			listItemFromRelation.FieldHelpText = null;
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = null;
			listItemFromRelation.RelationId = new Guid("cddc10b6-30ff-4a86-96e4-645b3ea59fd9");
			listItemFromRelation.RelationName = "user_1_n_bug_owner";
			createListInput.Columns.Add(listItemFromRelation);
		}
		#endregion
		#region << status >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			listField.EntityName = "wv_bug";
			listField.FieldId = new Guid("48be1e6e-df47-46f0-b4e1-6e9e1cbaf71c");
			listField.FieldName = "status";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << priority >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			listField.EntityName = "wv_bug";
			listField.FieldId = new Guid("e506dfc7-6d49-4d00-9f61-8befd56c1a6e");
			listField.FieldName = "priority";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = null;
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_bug Updated list: all_bugs Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_bug name: admin >>
{
	var createListEntity = entMan.ReadEntity(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "admin").Id;
	createListInput.Type =  "Hidden";
	createListInput.Name = "admin";
	createListInput.Label = "All Bugs";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("3.0");
	createListInput.Default = false;
	createListInput.System = true;
	createListInput.CssClass = "bug-list";
	createListInput.IconName = "bug";
	createListInput.VisibleColumnsCount = Int32.Parse("7");
	createListInput.ColumnWidthsCSV = "auto,30px,120px,120px,120px,120px";
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record, ngCtrl)}}"">
					<i class=""fa fa-fw fa-eye""></i>
					</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
									ng-href=""{{::ngCtrl.getRecordCreateUrl(ngCtrl)}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

		#region << subject >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			listField.EntityName = "wv_bug";
			listField.FieldId = new Guid("335a4500-130e-4739-b774-2f53f33ea22a");
			listField.FieldName = "subject";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << field from Relation: image >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("bf199b74-4448-4f58-93f5-6b86d888843b");
			listItemFromRelation.FieldName = "image";
			listItemFromRelation.FieldLabel = "Owner";
			listItemFromRelation.FieldPlaceholder = null;
			listItemFromRelation.FieldHelpText = null;
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = null;
			listItemFromRelation.RelationId = new Guid("cddc10b6-30ff-4a86-96e4-645b3ea59fd9");
			listItemFromRelation.RelationName = "user_1_n_bug_owner";
			createListInput.Columns.Add(listItemFromRelation);
		}
		#endregion
		#region << created_on >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			listField.EntityName = "wv_bug";
			listField.FieldId = new Guid("781cee71-1632-4bf9-83b1-ff122d29eb2a");
			listField.FieldName = "created_on";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << status >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			listField.EntityName = "wv_bug";
			listField.FieldId = new Guid("48be1e6e-df47-46f0-b4e1-6e9e1cbaf71c");
			listField.FieldName = "status";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << priority >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			listField.EntityName = "wv_bug";
			listField.FieldId = new Guid("e506dfc7-6d49-4d00-9f61-8befd56c1a6e");
			listField.FieldName = "priority";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = null;
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

		#region << sort >>
		{
			var sort = new InputRecordListSort();
			sort.FieldName = "{\"name\":\"url_sort\", \"option\": \"sortBy\", \"default\": \"created_on\", \"settings\":{\"order\":\"sortOrder\"}}";
			sort.SortType = "Descending";
			createListInput.Sorts.Add(sort);
		}
		#endregion

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_bug Updated list: admin Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_bug name: my_bugs >>
{
	var createListEntity = entMan.ReadEntity(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "my_bugs").Id;
	createListInput.Type =  "General";
	createListInput.Name = "my_bugs";
	createListInput.Label = "My Owned Open Bugs";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("1.0");
	createListInput.Default = false;
	createListInput.System = true;
	createListInput.CssClass = "bug-list";
	createListInput.IconName = "bug";
	createListInput.VisibleColumnsCount = Int32.Parse("7");
	createListInput.ColumnWidthsCSV = "100px,auto,30px,160px,120px,120px,120px";
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record, ngCtrl)}}"">
					<i class=""fa fa-fw fa-eye""></i>
					</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
									ng-href=""{{::ngCtrl.getRecordCreateUrl(ngCtrl)}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

		#region << code >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			listField.EntityName = "wv_bug";
			listField.FieldId = new Guid("6f2030e9-edd7-42ac-bb2a-2766b76c3da1");
			listField.FieldName = "code";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << subject >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			listField.EntityName = "wv_bug";
			listField.FieldId = new Guid("335a4500-130e-4739-b774-2f53f33ea22a");
			listField.FieldName = "subject";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << field from Relation: image >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("bf199b74-4448-4f58-93f5-6b86d888843b");
			listItemFromRelation.FieldName = "image";
			listItemFromRelation.FieldLabel = "Owner";
			listItemFromRelation.FieldPlaceholder = null;
			listItemFromRelation.FieldHelpText = null;
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = null;
			listItemFromRelation.RelationId = new Guid("cddc10b6-30ff-4a86-96e4-645b3ea59fd9");
			listItemFromRelation.RelationName = "user_1_n_bug_owner";
			createListInput.Columns.Add(listItemFromRelation);
		}
		#endregion
		#region << created_on >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			listField.EntityName = "wv_bug";
			listField.FieldId = new Guid("781cee71-1632-4bf9-83b1-ff122d29eb2a");
			listField.FieldName = "created_on";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << status >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			listField.EntityName = "wv_bug";
			listField.FieldId = new Guid("48be1e6e-df47-46f0-b4e1-6e9e1cbaf71c");
			listField.FieldName = "status";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << priority >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			listField.EntityName = "wv_bug";
			listField.FieldId = new Guid("e506dfc7-6d49-4d00-9f61-8befd56c1a6e");
			listField.FieldName = "priority";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = new InputRecordListQuery();
	var queryDictionary = new Dictionary<Guid,InputRecordListQuery>();
	var subQueryDictionary = new Dictionary<Guid,List<InputRecordListQuery>>();
	//Main query rule
	createListInput.Query.FieldName = null;
	createListInput.Query.FieldValue =  null;
	createListInput.Query.QueryType = "AND";
	createListInput.Query.SubQueries = new List<InputRecordListQuery>();
		{
		queryDictionary[new Guid("2ec6b3fe-27b4-43d2-b901-0cbad5fe22b4")] = new InputRecordListQuery();
		queryDictionary[new Guid("2ec6b3fe-27b4-43d2-b901-0cbad5fe22b4")].FieldName = null;
		queryDictionary[new Guid("2ec6b3fe-27b4-43d2-b901-0cbad5fe22b4")].FieldValue =  null;
		queryDictionary[new Guid("2ec6b3fe-27b4-43d2-b901-0cbad5fe22b4")].QueryType = "AND";
		queryDictionary[new Guid("2ec6b3fe-27b4-43d2-b901-0cbad5fe22b4")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("d7f0f28d-cee4-41a2-91f5-76dc272daa22")] = new InputRecordListQuery();
			queryDictionary[new Guid("d7f0f28d-cee4-41a2-91f5-76dc272daa22")].FieldName = "owner_id";
			queryDictionary[new Guid("d7f0f28d-cee4-41a2-91f5-76dc272daa22")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("d7f0f28d-cee4-41a2-91f5-76dc272daa22")].QueryType = "EQ";
			queryDictionary[new Guid("d7f0f28d-cee4-41a2-91f5-76dc272daa22")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("d7f0f28d-cee4-41a2-91f5-76dc272daa22"))) {queryDictionary[new Guid("d7f0f28d-cee4-41a2-91f5-76dc272daa22")].SubQueries = subQueryDictionary[new Guid("d7f0f28d-cee4-41a2-91f5-76dc272daa22")];}
			if(!subQueryDictionary.ContainsKey(new Guid("2ec6b3fe-27b4-43d2-b901-0cbad5fe22b4"))) {subQueryDictionary[new Guid("2ec6b3fe-27b4-43d2-b901-0cbad5fe22b4")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("2ec6b3fe-27b4-43d2-b901-0cbad5fe22b4")].Add(queryDictionary[new Guid("d7f0f28d-cee4-41a2-91f5-76dc272daa22")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("2ec6b3fe-27b4-43d2-b901-0cbad5fe22b4"))) {queryDictionary[new Guid("2ec6b3fe-27b4-43d2-b901-0cbad5fe22b4")].SubQueries = subQueryDictionary[new Guid("2ec6b3fe-27b4-43d2-b901-0cbad5fe22b4")];}
		if(!subQueryDictionary.ContainsKey(new Guid("c1645016-0bbb-45d4-a5d7-700e7f9c9e7d"))) {subQueryDictionary[new Guid("c1645016-0bbb-45d4-a5d7-700e7f9c9e7d")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("c1645016-0bbb-45d4-a5d7-700e7f9c9e7d")].Add(queryDictionary[new Guid("2ec6b3fe-27b4-43d2-b901-0cbad5fe22b4")]);
		}
		{
		queryDictionary[new Guid("3bb6fe9e-40b7-48a6-a521-8b27438a4b42")] = new InputRecordListQuery();
		queryDictionary[new Guid("3bb6fe9e-40b7-48a6-a521-8b27438a4b42")].FieldName = null;
		queryDictionary[new Guid("3bb6fe9e-40b7-48a6-a521-8b27438a4b42")].FieldValue =  null;
		queryDictionary[new Guid("3bb6fe9e-40b7-48a6-a521-8b27438a4b42")].QueryType = "AND";
		queryDictionary[new Guid("3bb6fe9e-40b7-48a6-a521-8b27438a4b42")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("210b3444-2741-4056-8dc0-74596aacf1da")] = new InputRecordListQuery();
			queryDictionary[new Guid("210b3444-2741-4056-8dc0-74596aacf1da")].FieldName = "code";
			queryDictionary[new Guid("210b3444-2741-4056-8dc0-74596aacf1da")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("210b3444-2741-4056-8dc0-74596aacf1da")].QueryType = "CONTAINS";
			queryDictionary[new Guid("210b3444-2741-4056-8dc0-74596aacf1da")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("210b3444-2741-4056-8dc0-74596aacf1da"))) {queryDictionary[new Guid("210b3444-2741-4056-8dc0-74596aacf1da")].SubQueries = subQueryDictionary[new Guid("210b3444-2741-4056-8dc0-74596aacf1da")];}
			if(!subQueryDictionary.ContainsKey(new Guid("3bb6fe9e-40b7-48a6-a521-8b27438a4b42"))) {subQueryDictionary[new Guid("3bb6fe9e-40b7-48a6-a521-8b27438a4b42")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("3bb6fe9e-40b7-48a6-a521-8b27438a4b42")].Add(queryDictionary[new Guid("210b3444-2741-4056-8dc0-74596aacf1da")]);
			}
			{
			queryDictionary[new Guid("66703a29-8aab-49ee-b2c7-b817bfdbe8ca")] = new InputRecordListQuery();
			queryDictionary[new Guid("66703a29-8aab-49ee-b2c7-b817bfdbe8ca")].FieldName = "subject";
			queryDictionary[new Guid("66703a29-8aab-49ee-b2c7-b817bfdbe8ca")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("66703a29-8aab-49ee-b2c7-b817bfdbe8ca")].QueryType = "CONTAINS";
			queryDictionary[new Guid("66703a29-8aab-49ee-b2c7-b817bfdbe8ca")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("66703a29-8aab-49ee-b2c7-b817bfdbe8ca"))) {queryDictionary[new Guid("66703a29-8aab-49ee-b2c7-b817bfdbe8ca")].SubQueries = subQueryDictionary[new Guid("66703a29-8aab-49ee-b2c7-b817bfdbe8ca")];}
			if(!subQueryDictionary.ContainsKey(new Guid("3bb6fe9e-40b7-48a6-a521-8b27438a4b42"))) {subQueryDictionary[new Guid("3bb6fe9e-40b7-48a6-a521-8b27438a4b42")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("3bb6fe9e-40b7-48a6-a521-8b27438a4b42")].Add(queryDictionary[new Guid("66703a29-8aab-49ee-b2c7-b817bfdbe8ca")]);
			}
			{
			queryDictionary[new Guid("05de14e5-e1c9-4152-866d-6915c338f64a")] = new InputRecordListQuery();
			queryDictionary[new Guid("05de14e5-e1c9-4152-866d-6915c338f64a")].FieldName = "status";
			queryDictionary[new Guid("05de14e5-e1c9-4152-866d-6915c338f64a")].FieldValue =  "closed";
			queryDictionary[new Guid("05de14e5-e1c9-4152-866d-6915c338f64a")].QueryType = "NOT";
			queryDictionary[new Guid("05de14e5-e1c9-4152-866d-6915c338f64a")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("05de14e5-e1c9-4152-866d-6915c338f64a"))) {queryDictionary[new Guid("05de14e5-e1c9-4152-866d-6915c338f64a")].SubQueries = subQueryDictionary[new Guid("05de14e5-e1c9-4152-866d-6915c338f64a")];}
			if(!subQueryDictionary.ContainsKey(new Guid("3bb6fe9e-40b7-48a6-a521-8b27438a4b42"))) {subQueryDictionary[new Guid("3bb6fe9e-40b7-48a6-a521-8b27438a4b42")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("3bb6fe9e-40b7-48a6-a521-8b27438a4b42")].Add(queryDictionary[new Guid("05de14e5-e1c9-4152-866d-6915c338f64a")]);
			}
			{
			queryDictionary[new Guid("ba4ddd53-dd4f-4968-8236-3bc8a5bfa87d")] = new InputRecordListQuery();
			queryDictionary[new Guid("ba4ddd53-dd4f-4968-8236-3bc8a5bfa87d")].FieldName = "priority";
			queryDictionary[new Guid("ba4ddd53-dd4f-4968-8236-3bc8a5bfa87d")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("ba4ddd53-dd4f-4968-8236-3bc8a5bfa87d")].QueryType = "EQ";
			queryDictionary[new Guid("ba4ddd53-dd4f-4968-8236-3bc8a5bfa87d")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("ba4ddd53-dd4f-4968-8236-3bc8a5bfa87d"))) {queryDictionary[new Guid("ba4ddd53-dd4f-4968-8236-3bc8a5bfa87d")].SubQueries = subQueryDictionary[new Guid("ba4ddd53-dd4f-4968-8236-3bc8a5bfa87d")];}
			if(!subQueryDictionary.ContainsKey(new Guid("3bb6fe9e-40b7-48a6-a521-8b27438a4b42"))) {subQueryDictionary[new Guid("3bb6fe9e-40b7-48a6-a521-8b27438a4b42")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("3bb6fe9e-40b7-48a6-a521-8b27438a4b42")].Add(queryDictionary[new Guid("ba4ddd53-dd4f-4968-8236-3bc8a5bfa87d")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("3bb6fe9e-40b7-48a6-a521-8b27438a4b42"))) {queryDictionary[new Guid("3bb6fe9e-40b7-48a6-a521-8b27438a4b42")].SubQueries = subQueryDictionary[new Guid("3bb6fe9e-40b7-48a6-a521-8b27438a4b42")];}
		if(!subQueryDictionary.ContainsKey(new Guid("c1645016-0bbb-45d4-a5d7-700e7f9c9e7d"))) {subQueryDictionary[new Guid("c1645016-0bbb-45d4-a5d7-700e7f9c9e7d")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("c1645016-0bbb-45d4-a5d7-700e7f9c9e7d")].Add(queryDictionary[new Guid("3bb6fe9e-40b7-48a6-a521-8b27438a4b42")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("c1645016-0bbb-45d4-a5d7-700e7f9c9e7d"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("c1645016-0bbb-45d4-a5d7-700e7f9c9e7d")];}
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

		#region << sort >>
		{
			var sort = new InputRecordListSort();
			sort.FieldName = "{\"name\":\"url_sort\", \"option\": \"sortBy\", \"default\": \"created_on\", \"settings\":{\"order\":\"sortOrder\"}}";
			sort.SortType = "Ascending";
			createListInput.Sorts.Add(sort);
		}
		#endregion

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_bug Updated list: my_bugs Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: plugin_data View: quick_view >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("d928d031-c8b1-4359-be3e-39bceb58f268")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "quick_view").Id;
	createViewInput.Type = "Quick_View";
	createViewInput.Name = "quick_view";
	createViewInput.Label = "Quick view";
	createViewInput.Title = "";
	createViewInput.Default = true;
	createViewInput.System = false;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "file-text-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("10.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""::ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
		ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
	<i class=""fa fa-trash go-red""></i> Delete Record
</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("d928d031-c8b1-4359-be3e-39bceb58f268"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: plugin_data Updated view: quick_view Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: plugin_data View: general >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("d928d031-c8b1-4359-be3e-39bceb58f268")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "general").Id;
	createViewInput.Type = "General";
	createViewInput.Name = "general";
	createViewInput.Label = "General";
	createViewInput.Title = "General";
	createViewInput.Default = true;
	createViewInput.System = false;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "file-text-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("10.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""::ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
		ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
	<i class=""fa fa-trash go-red""></i> Delete Record
</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("d928d031-c8b1-4359-be3e-39bceb58f268"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: plugin_data Updated view: general Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: plugin_data name: general >>
{
	var createListEntity = entMan.ReadEntity(new Guid("d928d031-c8b1-4359-be3e-39bceb58f268")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "general").Id;
	createListInput.Type =  "General";
	createListInput.Name = "general";
	createListInput.Label = "General";
	createListInput.Title = null;
	createListInput.Weight = Decimal.Parse("10.0");
	createListInput.Default = true;
	createListInput.System = false;
	createListInput.CssClass = null;
	createListInput.IconName = "list";
	createListInput.VisibleColumnsCount = Int32.Parse("5");
	createListInput.ColumnWidthsCSV = null;
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
    ng-href=""{{::ngCtrl.getRecordCreateUrl()}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_import_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_import_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("10.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openImportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-upload""></i> Import CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_export_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_export_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("11.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openExportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-download""></i> Export CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record)}}"">
    <i class=""fa fa-fw fa-eye""></i>
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = null;
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("d928d031-c8b1-4359-be3e-39bceb58f268"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: plugin_data Updated list: general Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: plugin_data name: lookup >>
{
	var createListEntity = entMan.ReadEntity(new Guid("d928d031-c8b1-4359-be3e-39bceb58f268")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "lookup").Id;
	createListInput.Type =  "Lookup";
	createListInput.Name = "lookup";
	createListInput.Label = "Lookup";
	createListInput.Title = null;
	createListInput.Weight = Decimal.Parse("10.0");
	createListInput.Default = true;
	createListInput.System = false;
	createListInput.CssClass = null;
	createListInput.IconName = "list";
	createListInput.VisibleColumnsCount = Int32.Parse("5");
	createListInput.ColumnWidthsCSV = null;
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
    ng-href=""{{::ngCtrl.getRecordCreateUrl()}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_import_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_import_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("10.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openImportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-upload""></i> Import CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_export_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_export_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("11.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openExportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-download""></i> Export CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record)}}"">
    <i class=""fa fa-fw fa-eye""></i>
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = null;
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("d928d031-c8b1-4359-be3e-39bceb58f268"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: plugin_data Updated list: lookup Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_project_comment View: quick_view >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "quick_view").Id;
	createViewInput.Type = "Quick_View";
	createViewInput.Name = "quick_view";
	createViewInput.Label = "Quick view";
	createViewInput.Title = "";
	createViewInput.Default = true;
	createViewInput.System = false;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "file-text-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("10.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""::ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
		ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
	<i class=""fa fa-trash go-red""></i> Delete Record
</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_project_comment Updated view: quick_view Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_project_comment View: general >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "general").Id;
	createViewInput.Type = "General";
	createViewInput.Name = "general";
	createViewInput.Label = "General";
	createViewInput.Title = "";
	createViewInput.Default = true;
	createViewInput.System = false;
	createViewInput.Weight = Decimal.Parse("10.0");
	createViewInput.CssClass = "";
	createViewInput.IconName = "file-text-o";
	createViewInput.DynamicHtmlTemplate = null;
	createViewInput.DataSourceUrl = null;
	createViewInput.ServiceCode = null;
	#endregion

	#region << regions >>
	createViewInput.Regions = new List<InputRecordViewRegion>();

		#region << Region: header >>
		{
			var viewRegion = new InputRecordViewRegion();
			viewRegion.Name = "header";
			viewRegion.Label = "Header";
			viewRegion.Render = true;
			viewRegion.Weight = Decimal.Parse("10.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			//Save region
			createViewInput.Regions.Add(viewRegion);
		}
		#endregion

	#endregion

	#region << Relation options >>
	{
	createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createViewInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_record_delete >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_delete";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""::ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
		ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
	<i class=""fa fa-trash go-red""></i> Delete Record
</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_back_button >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_back_button";
			actionItem.Menu = "sidebar-top";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Sidebar >>
	createViewInput.Sidebar = new InputRecordViewSidebar();
	createViewInput.Sidebar.CssClass = "";
	createViewInput.Sidebar.Render = false;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_project_comment Updated view: general Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_project_comment name: general >>
{
	var createListEntity = entMan.ReadEntity(new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "general").Id;
	createListInput.Type =  "General";
	createListInput.Name = "general";
	createListInput.Label = "General";
	createListInput.Title = null;
	createListInput.Weight = Decimal.Parse("10.0");
	createListInput.Default = true;
	createListInput.System = false;
	createListInput.CssClass = null;
	createListInput.IconName = "list";
	createListInput.VisibleColumnsCount = Int32.Parse("5");
	createListInput.ColumnWidthsCSV = null;
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
    ng-href=""{{::ngCtrl.getRecordCreateUrl()}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_import_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_import_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("10.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openImportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-upload""></i> Import CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_export_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_export_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("11.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openExportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-download""></i> Export CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record)}}"">
    <i class=""fa fa-fw fa-eye""></i>
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = null;
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_project_comment Updated list: general Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_project_comment name: lookup >>
{
	var createListEntity = entMan.ReadEntity(new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "lookup").Id;
	createListInput.Type =  "Lookup";
	createListInput.Name = "lookup";
	createListInput.Label = "Lookup";
	createListInput.Title = null;
	createListInput.Weight = Decimal.Parse("10.0");
	createListInput.Default = true;
	createListInput.System = false;
	createListInput.CssClass = null;
	createListInput.IconName = "list";
	createListInput.VisibleColumnsCount = Int32.Parse("5");
	createListInput.ColumnWidthsCSV = null;
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = null;
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
    ng-href=""{{::ngCtrl.getRecordCreateUrl()}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_import_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_import_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("10.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openImportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-upload""></i> Import CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_export_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_export_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("11.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openExportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')"">
	<i class=""fa fa-fw fa-download""></i> Export CSV
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record)}}"">
    <i class=""fa fa-fw fa-eye""></i>
</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = null;
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_project_comment Updated list: lookup Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_project_comment name: bug_comments >>
{
	var createListEntity = entMan.ReadEntity(new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "bug_comments").Id;
	createListInput.Type =  "Hidden";
	createListInput.Name = "bug_comments";
	createListInput.Label = "Comments";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("10.0");
	createListInput.Default = false;
	createListInput.System = true;
	createListInput.CssClass = null;
	createListInput.IconName = "comments-o";
	createListInput.VisibleColumnsCount = Int32.Parse("5");
	createListInput.ColumnWidthsCSV = null;
	createListInput.PageSize = Int32.Parse("0");
	createListInput.DynamicHtmlTemplate = "/plugins/webvella-projects/templates/bug-comments.html";
	createListInput.DataSourceUrl = null;
	createListInput.ServiceCode = "/plugins/webvella-projects/providers/bug-comments.service.js";
	#endregion

	#region << Relation options >>
	{
	createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
	}
	#endregion

	#region << Action items >>
	{
	createListInput.ActionItems = new List<ActionItem>();

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
											ng-click=""ngCtrl.actionService.manageComment(null,ngCtrl)"">Add Comment</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

		#region << created_on >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99");
			listField.EntityName = "wv_project_comment";
			listField.FieldId = new Guid("c205c60f-598a-4db7-bd41-a7fd2ae3abd0");
			listField.FieldName = "created_on";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << field from Relation: username >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("263c0b21-88c1-4c2b-80b4-db7402b0d2e2");
			listItemFromRelation.FieldName = "username";
			listItemFromRelation.FieldLabel = null;
			listItemFromRelation.FieldPlaceholder = null;
			listItemFromRelation.FieldHelpText = null;
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = null;
			listItemFromRelation.RelationId = new Guid("2f3635a3-298e-475e-90f4-7d512da6cf95");
			listItemFromRelation.RelationName = "user_wv_project_comment_created_by";
			createListInput.Columns.Add(listItemFromRelation);
		}
		#endregion
		#region << content >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99");
			listField.EntityName = "wv_project_comment";
			listField.FieldId = new Guid("23afb07b-438f-4e31-9372-c850a5789cc6");
			listField.FieldName = "content";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << field from Relation: image >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("bf199b74-4448-4f58-93f5-6b86d888843b");
			listItemFromRelation.FieldName = "image";
			listItemFromRelation.FieldLabel = null;
			listItemFromRelation.FieldPlaceholder = null;
			listItemFromRelation.FieldHelpText = null;
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = null;
			listItemFromRelation.RelationId = new Guid("2f3635a3-298e-475e-90f4-7d512da6cf95");
			listItemFromRelation.RelationName = "user_wv_project_comment_created_by";
			createListInput.Columns.Add(listItemFromRelation);
		}
		#endregion
		#region << created_by >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99");
			listField.EntityName = "wv_project_comment";
			listField.FieldId = new Guid("46208807-7bc8-4f54-8618-45134189e763");
			listField.FieldName = "created_by";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
	}
	#endregion

	#region << Query >>
	{
	createListInput.Query = null;
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

		#region << sort >>
		{
			var sort = new InputRecordListSort();
			sort.FieldName = "created_on";
			sort.SortType = "Ascending";
			createListInput.Sorts.Add(sort);
		}
		#endregion

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_project_comment Updated list: bug_comments Message:" + response.Message);
	}
}
#endregion

#region << Create relation: wv_sprint_n_n_wv_task >>
{
	var relation = new EntityRelation();
	var originEntity = entMan.ReadEntity(new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd")).Object;
	var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
	var targetEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
	var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "id");
	relation.Id = new Guid("c8bbae9a-82cf-4716-9f61-38b0f743e6a2");
	relation.Name =  "wv_sprint_n_n_wv_task";
	relation.Label = "wv_sprint_n_n_wv_task";
	relation.Description = "";
	relation.System =  true;
	relation.RelationType = EntityRelationType.ManyToMany;
	relation.OriginEntityId = originEntity.Id;
	relation.OriginEntityName = originEntity.Name;
	relation.OriginFieldId = originField.Id;
	relation.OriginFieldName = originField.Name;
	relation.TargetEntityId = targetEntity.Id;
	relation.TargetEntityName = targetEntity.Name;
	relation.TargetFieldId = targetField.Id;
	relation.TargetFieldName = targetField.Name;
	{
		var response = relMan.Create(relation);
		if (!response.Success)
			throw new Exception("System error 10060. Relation: wv_sprint_n_n_wv_task Create. Message:" + response.Message);
	}
}
#endregion


		}


    }
}
