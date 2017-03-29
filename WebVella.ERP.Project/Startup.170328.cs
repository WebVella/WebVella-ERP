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
		private static void Patch170328(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan, bool createSampleRecords = false)
		{

#region << ***Update view***  Entity: wv_milestone View Name: quick_view >>
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

#region << ***Update view***  Entity: wv_milestone View Name: general >>
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

#region << ***Update list***  Entity: wv_milestone List Name: lookup >>
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

#region << ***Update list***  Entity: wv_milestone List Name: project_milestones >>
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

#region << ***Update view***  Entity: wv_project_activity View Name: quick_view >>
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

#region << ***Update view***  Entity: wv_project_activity View Name: general >>
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

#region << ***Update list***  Entity: wv_project_activity List Name: general >>
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

#region << ***Update list***  Entity: wv_project_activity List Name: lookup >>
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

#region << ***Update view***  Entity: wv_timelog View Name: quick_view >>
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

#region << ***Update view***  Entity: wv_timelog View Name: general >>
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

#region << ***Update list***  Entity: wv_timelog List Name: general >>
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

#region << ***Update list***  Entity: wv_timelog List Name: lookup >>
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

#region << ***Update list***  Entity: wv_timelog List Name: bug_timelogs >>
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

#region << ***Update list***  Entity: wv_timelog List Name: task_timelogs >>
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

#region << ***Update view***  Entity: plugin_data View Name: quick_view >>
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

#region << ***Update view***  Entity: plugin_data View Name: general >>
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

#region << ***Update list***  Entity: plugin_data List Name: general >>
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

#region << ***Update list***  Entity: plugin_data List Name: lookup >>
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

#region << ***Update field***  Entity: wv_sprint Field Name: start_date >>
{
	var currentEntity = entMan.ReadEntity(new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd")).Object;
	InputDateField dateField = new InputDateField();
	dateField.Id =  currentEntity.Fields.SingleOrDefault(x => x.Name == "start_date").Id;
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
	dateField.DefaultValue = DateTime.Parse("11/30/2015 10:00:00 PM");
	dateField.Format = "yyyy-MMM-dd";
	dateField.UseCurrentTimeAsDefaultValue = false;
	dateField.EnableSecurity = false;
	dateField.Permissions = new FieldPermissions();
	dateField.Permissions.CanRead = new List<Guid>();
	dateField.Permissions.CanUpdate = new List<Guid>();
	//READ
	//UPDATE
	{
		var response = entMan.UpdateField(new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd"), dateField);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_sprint Field: start_date Message:" + response.Message);
	}
}
#endregion

#region << ***Update view***  Entity: wv_sprint View Name: general >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "general").Id;
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
					#region << field from Relation: name >>
					{
						var viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = new Guid("c4541fee-fbb6-4661-929e-1724adec285a");
						viewItemFromRelation.EntityName = "role";
						viewItemFromRelation.Type = "fieldFromRelation";
						viewItemFromRelation.FieldId = new Guid("36f91ebd-5a02-4032-8498-b7f716f6a349");
						viewItemFromRelation.FieldName = "name";
						viewItemFromRelation.FieldLabel = "Allowed roles";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = "";
						viewItemFromRelation.FieldRequired = false;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = new Guid("e77b7a71-134e-41bf-a079-008e8931303f");
						viewItemFromRelation.RelationName = "role_n_n_wv_sprint";
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
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""{{'DELETE_CONFIRMATION_ALERT_MESSAGE' | translate}}"" 
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
		var response = entMan.UpdateRecordView(new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_sprint Updated view: general Message:" + response.Message);
	}
}
#endregion

#region << ***Update list***  Entity: wv_sprint List Name: general >>
{
	var createListEntity = entMan.ReadEntity(new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "general").Id;
	createListInput.Type =  "General";
	createListInput.Name = "general";
	createListInput.Label = "All Sprints";
	createListInput.Title = "Available sprints";
	createListInput.Weight = Decimal.Parse("1.0");
	createListInput.Default = true;
	createListInput.System = false;
	createListInput.CssClass = "";
	createListInput.IconName = "";
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

		#region << sort >>
		{
			var sort = new InputRecordListSort();
			sort.FieldName = "created_on";
			sort.SortType = "Descending";
			createListInput.Sorts.Add(sort);
		}
		#endregion

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_sprint Updated list: general Message:" + response.Message);
	}
}
#endregion

#region << ***Update field***  Entity: wv_task Field Name: created_on >>
{
	var currentEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
	InputDateTimeField datetimeField = new InputDateTimeField();
	datetimeField.Id =  currentEntity.Fields.SingleOrDefault(x => x.Name == "created_on").Id;
	datetimeField.Name = "created_on";
	datetimeField.Label = "Created On";
	datetimeField.PlaceholderText = "";
	datetimeField.Description = "";
	datetimeField.HelpText = "";
	datetimeField.Required = false;
	datetimeField.Unique = false;
	datetimeField.Searchable = false;
	datetimeField.Auditable = false;
	datetimeField.System = true;
	datetimeField.DefaultValue = null;
	datetimeField.Format = "dd MMM yyyy HH:mm";
	datetimeField.UseCurrentTimeAsDefaultValue = true;
	datetimeField.EnableSecurity = true;
	datetimeField.Permissions = new FieldPermissions();
	datetimeField.Permissions.CanRead = new List<Guid>();
	datetimeField.Permissions.CanUpdate = new List<Guid>();
	//READ
	datetimeField.Permissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
	datetimeField.Permissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
	//UPDATE
	datetimeField.Permissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
	{
		var response = entMan.UpdateField(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89"), datetimeField);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_task Field: created_on Message:" + response.Message);
	}
}
#endregion

#region << ***Update view***  Entity: wv_task View Name: quick_view >>
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

#region << ***Update list***  Entity: wv_task List Name: lookup >>
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

#region << ***Update list***  Entity: wv_task List Name: created_tasks >>
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
	createListInput.ColumnWidthsCSV = "100px,auto,120px,120px,120px,120px,120px";
	createListInput.PageSize = Int32.Parse("50");
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
		#region << field from Relation: username >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("263c0b21-88c1-4c2b-80b4-db7402b0d2e2");
			listItemFromRelation.FieldName = "username";
			listItemFromRelation.FieldLabel = "owner";
			listItemFromRelation.FieldPlaceholder = "";
			listItemFromRelation.FieldHelpText = "";
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = "";
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
		queryDictionary[new Guid("aa0ad7fb-43a2-49cf-9d29-eaefbbf21ba5")] = new InputRecordListQuery();
		queryDictionary[new Guid("aa0ad7fb-43a2-49cf-9d29-eaefbbf21ba5")].FieldName = null;
		queryDictionary[new Guid("aa0ad7fb-43a2-49cf-9d29-eaefbbf21ba5")].FieldValue =  null;
		queryDictionary[new Guid("aa0ad7fb-43a2-49cf-9d29-eaefbbf21ba5")].QueryType = "AND";
		queryDictionary[new Guid("aa0ad7fb-43a2-49cf-9d29-eaefbbf21ba5")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("048c567c-c195-468f-939d-6992d01e98b5")] = new InputRecordListQuery();
			queryDictionary[new Guid("048c567c-c195-468f-939d-6992d01e98b5")].FieldName = "created_by";
			queryDictionary[new Guid("048c567c-c195-468f-939d-6992d01e98b5")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("048c567c-c195-468f-939d-6992d01e98b5")].QueryType = "EQ";
			queryDictionary[new Guid("048c567c-c195-468f-939d-6992d01e98b5")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("048c567c-c195-468f-939d-6992d01e98b5"))) {queryDictionary[new Guid("048c567c-c195-468f-939d-6992d01e98b5")].SubQueries = subQueryDictionary[new Guid("048c567c-c195-468f-939d-6992d01e98b5")];}
			if(!subQueryDictionary.ContainsKey(new Guid("aa0ad7fb-43a2-49cf-9d29-eaefbbf21ba5"))) {subQueryDictionary[new Guid("aa0ad7fb-43a2-49cf-9d29-eaefbbf21ba5")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("aa0ad7fb-43a2-49cf-9d29-eaefbbf21ba5")].Add(queryDictionary[new Guid("048c567c-c195-468f-939d-6992d01e98b5")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("aa0ad7fb-43a2-49cf-9d29-eaefbbf21ba5"))) {queryDictionary[new Guid("aa0ad7fb-43a2-49cf-9d29-eaefbbf21ba5")].SubQueries = subQueryDictionary[new Guid("aa0ad7fb-43a2-49cf-9d29-eaefbbf21ba5")];}
		if(!subQueryDictionary.ContainsKey(new Guid("68bf91b4-f493-4a5a-956a-989d715332ff"))) {subQueryDictionary[new Guid("68bf91b4-f493-4a5a-956a-989d715332ff")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("68bf91b4-f493-4a5a-956a-989d715332ff")].Add(queryDictionary[new Guid("aa0ad7fb-43a2-49cf-9d29-eaefbbf21ba5")]);
		}
		{
		queryDictionary[new Guid("928a7b04-0b01-475d-9085-636cf685d6d2")] = new InputRecordListQuery();
		queryDictionary[new Guid("928a7b04-0b01-475d-9085-636cf685d6d2")].FieldName = null;
		queryDictionary[new Guid("928a7b04-0b01-475d-9085-636cf685d6d2")].FieldValue =  null;
		queryDictionary[new Guid("928a7b04-0b01-475d-9085-636cf685d6d2")].QueryType = "AND";
		queryDictionary[new Guid("928a7b04-0b01-475d-9085-636cf685d6d2")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("970473ef-ec32-461d-be04-67566a778e65")] = new InputRecordListQuery();
			queryDictionary[new Guid("970473ef-ec32-461d-be04-67566a778e65")].FieldName = "code";
			queryDictionary[new Guid("970473ef-ec32-461d-be04-67566a778e65")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("970473ef-ec32-461d-be04-67566a778e65")].QueryType = "CONTAINS";
			queryDictionary[new Guid("970473ef-ec32-461d-be04-67566a778e65")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("970473ef-ec32-461d-be04-67566a778e65"))) {queryDictionary[new Guid("970473ef-ec32-461d-be04-67566a778e65")].SubQueries = subQueryDictionary[new Guid("970473ef-ec32-461d-be04-67566a778e65")];}
			if(!subQueryDictionary.ContainsKey(new Guid("928a7b04-0b01-475d-9085-636cf685d6d2"))) {subQueryDictionary[new Guid("928a7b04-0b01-475d-9085-636cf685d6d2")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("928a7b04-0b01-475d-9085-636cf685d6d2")].Add(queryDictionary[new Guid("970473ef-ec32-461d-be04-67566a778e65")]);
			}
			{
			queryDictionary[new Guid("aeee1d9b-a9bb-4238-bc55-59cfa43d5439")] = new InputRecordListQuery();
			queryDictionary[new Guid("aeee1d9b-a9bb-4238-bc55-59cfa43d5439")].FieldName = "subject";
			queryDictionary[new Guid("aeee1d9b-a9bb-4238-bc55-59cfa43d5439")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("aeee1d9b-a9bb-4238-bc55-59cfa43d5439")].QueryType = "CONTAINS";
			queryDictionary[new Guid("aeee1d9b-a9bb-4238-bc55-59cfa43d5439")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("aeee1d9b-a9bb-4238-bc55-59cfa43d5439"))) {queryDictionary[new Guid("aeee1d9b-a9bb-4238-bc55-59cfa43d5439")].SubQueries = subQueryDictionary[new Guid("aeee1d9b-a9bb-4238-bc55-59cfa43d5439")];}
			if(!subQueryDictionary.ContainsKey(new Guid("928a7b04-0b01-475d-9085-636cf685d6d2"))) {subQueryDictionary[new Guid("928a7b04-0b01-475d-9085-636cf685d6d2")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("928a7b04-0b01-475d-9085-636cf685d6d2")].Add(queryDictionary[new Guid("aeee1d9b-a9bb-4238-bc55-59cfa43d5439")]);
			}
			{
			queryDictionary[new Guid("b2a7c292-215b-4aa7-bf62-a0829c814d75")] = new InputRecordListQuery();
			queryDictionary[new Guid("b2a7c292-215b-4aa7-bf62-a0829c814d75")].FieldName = "status";
			queryDictionary[new Guid("b2a7c292-215b-4aa7-bf62-a0829c814d75")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("b2a7c292-215b-4aa7-bf62-a0829c814d75")].QueryType = "EQ";
			queryDictionary[new Guid("b2a7c292-215b-4aa7-bf62-a0829c814d75")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("b2a7c292-215b-4aa7-bf62-a0829c814d75"))) {queryDictionary[new Guid("b2a7c292-215b-4aa7-bf62-a0829c814d75")].SubQueries = subQueryDictionary[new Guid("b2a7c292-215b-4aa7-bf62-a0829c814d75")];}
			if(!subQueryDictionary.ContainsKey(new Guid("928a7b04-0b01-475d-9085-636cf685d6d2"))) {subQueryDictionary[new Guid("928a7b04-0b01-475d-9085-636cf685d6d2")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("928a7b04-0b01-475d-9085-636cf685d6d2")].Add(queryDictionary[new Guid("b2a7c292-215b-4aa7-bf62-a0829c814d75")]);
			}
			{
			queryDictionary[new Guid("20f8fa43-5a78-45b0-b3dd-e8edf545e843")] = new InputRecordListQuery();
			queryDictionary[new Guid("20f8fa43-5a78-45b0-b3dd-e8edf545e843")].FieldName = "priority";
			queryDictionary[new Guid("20f8fa43-5a78-45b0-b3dd-e8edf545e843")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("20f8fa43-5a78-45b0-b3dd-e8edf545e843")].QueryType = "EQ";
			queryDictionary[new Guid("20f8fa43-5a78-45b0-b3dd-e8edf545e843")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("20f8fa43-5a78-45b0-b3dd-e8edf545e843"))) {queryDictionary[new Guid("20f8fa43-5a78-45b0-b3dd-e8edf545e843")].SubQueries = subQueryDictionary[new Guid("20f8fa43-5a78-45b0-b3dd-e8edf545e843")];}
			if(!subQueryDictionary.ContainsKey(new Guid("928a7b04-0b01-475d-9085-636cf685d6d2"))) {subQueryDictionary[new Guid("928a7b04-0b01-475d-9085-636cf685d6d2")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("928a7b04-0b01-475d-9085-636cf685d6d2")].Add(queryDictionary[new Guid("20f8fa43-5a78-45b0-b3dd-e8edf545e843")]);
			}
			{
			queryDictionary[new Guid("b5876927-d5d7-44d9-9001-f44f7af39f33")] = new InputRecordListQuery();
			queryDictionary[new Guid("b5876927-d5d7-44d9-9001-f44f7af39f33")].FieldName = "$user_1_n_task_owner.username";
			queryDictionary[new Guid("b5876927-d5d7-44d9-9001-f44f7af39f33")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"$user_1_n_task_owner.username\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("b5876927-d5d7-44d9-9001-f44f7af39f33")].QueryType = "CONTAINS";
			queryDictionary[new Guid("b5876927-d5d7-44d9-9001-f44f7af39f33")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("b5876927-d5d7-44d9-9001-f44f7af39f33"))) {queryDictionary[new Guid("b5876927-d5d7-44d9-9001-f44f7af39f33")].SubQueries = subQueryDictionary[new Guid("b5876927-d5d7-44d9-9001-f44f7af39f33")];}
			if(!subQueryDictionary.ContainsKey(new Guid("928a7b04-0b01-475d-9085-636cf685d6d2"))) {subQueryDictionary[new Guid("928a7b04-0b01-475d-9085-636cf685d6d2")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("928a7b04-0b01-475d-9085-636cf685d6d2")].Add(queryDictionary[new Guid("b5876927-d5d7-44d9-9001-f44f7af39f33")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("928a7b04-0b01-475d-9085-636cf685d6d2"))) {queryDictionary[new Guid("928a7b04-0b01-475d-9085-636cf685d6d2")].SubQueries = subQueryDictionary[new Guid("928a7b04-0b01-475d-9085-636cf685d6d2")];}
		if(!subQueryDictionary.ContainsKey(new Guid("68bf91b4-f493-4a5a-956a-989d715332ff"))) {subQueryDictionary[new Guid("68bf91b4-f493-4a5a-956a-989d715332ff")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("68bf91b4-f493-4a5a-956a-989d715332ff")].Add(queryDictionary[new Guid("928a7b04-0b01-475d-9085-636cf685d6d2")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("68bf91b4-f493-4a5a-956a-989d715332ff"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("68bf91b4-f493-4a5a-956a-989d715332ff")];}
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

#region << ***Update list***  Entity: wv_task List Name: all_tasks >>
{
	var createListEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "all_tasks").Id;
	createListInput.Type =  "General";
	createListInput.Name = "all_tasks";
	createListInput.Label = "All tasks";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("12.0");
	createListInput.Default = false;
	createListInput.System = true;
	createListInput.CssClass = "task-list";
	createListInput.IconName = "tasks";
	createListInput.VisibleColumnsCount = Int32.Parse("7");
	createListInput.ColumnWidthsCSV = "100px,auto,120px,120px,120px,120px,120px";
	createListInput.PageSize = Int32.Parse("50");
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
		#region << field from Relation: username >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("263c0b21-88c1-4c2b-80b4-db7402b0d2e2");
			listItemFromRelation.FieldName = "username";
			listItemFromRelation.FieldLabel = "owner";
			listItemFromRelation.FieldPlaceholder = "";
			listItemFromRelation.FieldHelpText = "";
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = "";
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
		queryDictionary[new Guid("e7b2b0f4-f1bf-4997-a189-6aa40427cfc5")] = new InputRecordListQuery();
		queryDictionary[new Guid("e7b2b0f4-f1bf-4997-a189-6aa40427cfc5")].FieldName = "code";
		queryDictionary[new Guid("e7b2b0f4-f1bf-4997-a189-6aa40427cfc5")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("e7b2b0f4-f1bf-4997-a189-6aa40427cfc5")].QueryType = "CONTAINS";
		queryDictionary[new Guid("e7b2b0f4-f1bf-4997-a189-6aa40427cfc5")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("e7b2b0f4-f1bf-4997-a189-6aa40427cfc5"))) {queryDictionary[new Guid("e7b2b0f4-f1bf-4997-a189-6aa40427cfc5")].SubQueries = subQueryDictionary[new Guid("e7b2b0f4-f1bf-4997-a189-6aa40427cfc5")];}
		if(!subQueryDictionary.ContainsKey(new Guid("8539e2bf-9529-4521-9d85-fe6fc48cbdc1"))) {subQueryDictionary[new Guid("8539e2bf-9529-4521-9d85-fe6fc48cbdc1")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("8539e2bf-9529-4521-9d85-fe6fc48cbdc1")].Add(queryDictionary[new Guid("e7b2b0f4-f1bf-4997-a189-6aa40427cfc5")]);
		}
		{
		queryDictionary[new Guid("57385ffa-1d0d-4ebf-b4eb-8cb8cca887e1")] = new InputRecordListQuery();
		queryDictionary[new Guid("57385ffa-1d0d-4ebf-b4eb-8cb8cca887e1")].FieldName = "subject";
		queryDictionary[new Guid("57385ffa-1d0d-4ebf-b4eb-8cb8cca887e1")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("57385ffa-1d0d-4ebf-b4eb-8cb8cca887e1")].QueryType = "CONTAINS";
		queryDictionary[new Guid("57385ffa-1d0d-4ebf-b4eb-8cb8cca887e1")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("57385ffa-1d0d-4ebf-b4eb-8cb8cca887e1"))) {queryDictionary[new Guid("57385ffa-1d0d-4ebf-b4eb-8cb8cca887e1")].SubQueries = subQueryDictionary[new Guid("57385ffa-1d0d-4ebf-b4eb-8cb8cca887e1")];}
		if(!subQueryDictionary.ContainsKey(new Guid("8539e2bf-9529-4521-9d85-fe6fc48cbdc1"))) {subQueryDictionary[new Guid("8539e2bf-9529-4521-9d85-fe6fc48cbdc1")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("8539e2bf-9529-4521-9d85-fe6fc48cbdc1")].Add(queryDictionary[new Guid("57385ffa-1d0d-4ebf-b4eb-8cb8cca887e1")]);
		}
		{
		queryDictionary[new Guid("00cf2934-5a89-46e4-a02a-e077410b0504")] = new InputRecordListQuery();
		queryDictionary[new Guid("00cf2934-5a89-46e4-a02a-e077410b0504")].FieldName = "status";
		queryDictionary[new Guid("00cf2934-5a89-46e4-a02a-e077410b0504")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("00cf2934-5a89-46e4-a02a-e077410b0504")].QueryType = "EQ";
		queryDictionary[new Guid("00cf2934-5a89-46e4-a02a-e077410b0504")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("00cf2934-5a89-46e4-a02a-e077410b0504"))) {queryDictionary[new Guid("00cf2934-5a89-46e4-a02a-e077410b0504")].SubQueries = subQueryDictionary[new Guid("00cf2934-5a89-46e4-a02a-e077410b0504")];}
		if(!subQueryDictionary.ContainsKey(new Guid("8539e2bf-9529-4521-9d85-fe6fc48cbdc1"))) {subQueryDictionary[new Guid("8539e2bf-9529-4521-9d85-fe6fc48cbdc1")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("8539e2bf-9529-4521-9d85-fe6fc48cbdc1")].Add(queryDictionary[new Guid("00cf2934-5a89-46e4-a02a-e077410b0504")]);
		}
		{
		queryDictionary[new Guid("6ebfd147-e3de-482d-88f2-4c1d11188f07")] = new InputRecordListQuery();
		queryDictionary[new Guid("6ebfd147-e3de-482d-88f2-4c1d11188f07")].FieldName = "priority";
		queryDictionary[new Guid("6ebfd147-e3de-482d-88f2-4c1d11188f07")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("6ebfd147-e3de-482d-88f2-4c1d11188f07")].QueryType = "EQ";
		queryDictionary[new Guid("6ebfd147-e3de-482d-88f2-4c1d11188f07")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("6ebfd147-e3de-482d-88f2-4c1d11188f07"))) {queryDictionary[new Guid("6ebfd147-e3de-482d-88f2-4c1d11188f07")].SubQueries = subQueryDictionary[new Guid("6ebfd147-e3de-482d-88f2-4c1d11188f07")];}
		if(!subQueryDictionary.ContainsKey(new Guid("8539e2bf-9529-4521-9d85-fe6fc48cbdc1"))) {subQueryDictionary[new Guid("8539e2bf-9529-4521-9d85-fe6fc48cbdc1")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("8539e2bf-9529-4521-9d85-fe6fc48cbdc1")].Add(queryDictionary[new Guid("6ebfd147-e3de-482d-88f2-4c1d11188f07")]);
		}
		{
		queryDictionary[new Guid("294a182e-cbe6-48d1-8a38-416e0a6a9fdf")] = new InputRecordListQuery();
		queryDictionary[new Guid("294a182e-cbe6-48d1-8a38-416e0a6a9fdf")].FieldName = "$user_1_n_task_owner.username";
		queryDictionary[new Guid("294a182e-cbe6-48d1-8a38-416e0a6a9fdf")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"$user_1_n_task_owner.username\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("294a182e-cbe6-48d1-8a38-416e0a6a9fdf")].QueryType = "CONTAINS";
		queryDictionary[new Guid("294a182e-cbe6-48d1-8a38-416e0a6a9fdf")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("294a182e-cbe6-48d1-8a38-416e0a6a9fdf"))) {queryDictionary[new Guid("294a182e-cbe6-48d1-8a38-416e0a6a9fdf")].SubQueries = subQueryDictionary[new Guid("294a182e-cbe6-48d1-8a38-416e0a6a9fdf")];}
		if(!subQueryDictionary.ContainsKey(new Guid("8539e2bf-9529-4521-9d85-fe6fc48cbdc1"))) {subQueryDictionary[new Guid("8539e2bf-9529-4521-9d85-fe6fc48cbdc1")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("8539e2bf-9529-4521-9d85-fe6fc48cbdc1")].Add(queryDictionary[new Guid("294a182e-cbe6-48d1-8a38-416e0a6a9fdf")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("8539e2bf-9529-4521-9d85-fe6fc48cbdc1"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("8539e2bf-9529-4521-9d85-fe6fc48cbdc1")];}
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

#region << ***Update list***  Entity: wv_task List Name: my_tasks >>
{
	var createListEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "my_tasks").Id;
	createListInput.Type =  "General";
	createListInput.Name = "my_tasks";
	createListInput.Label = "My open tasks";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("1.0");
	createListInput.Default = false;
	createListInput.System = true;
	createListInput.CssClass = "task-list";
	createListInput.IconName = "tasks";
	createListInput.VisibleColumnsCount = Int32.Parse("7");
	createListInput.ColumnWidthsCSV = "100px,auto,120px,120px,120px,120px";
	createListInput.PageSize = Int32.Parse("50");
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
		queryDictionary[new Guid("ecab02b9-f80e-45c1-bec3-91383c47e8a2")] = new InputRecordListQuery();
		queryDictionary[new Guid("ecab02b9-f80e-45c1-bec3-91383c47e8a2")].FieldName = null;
		queryDictionary[new Guid("ecab02b9-f80e-45c1-bec3-91383c47e8a2")].FieldValue =  null;
		queryDictionary[new Guid("ecab02b9-f80e-45c1-bec3-91383c47e8a2")].QueryType = "AND";
		queryDictionary[new Guid("ecab02b9-f80e-45c1-bec3-91383c47e8a2")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("c88eb0a1-afad-4c8b-8ccb-59701eb96677")] = new InputRecordListQuery();
			queryDictionary[new Guid("c88eb0a1-afad-4c8b-8ccb-59701eb96677")].FieldName = "owner_id";
			queryDictionary[new Guid("c88eb0a1-afad-4c8b-8ccb-59701eb96677")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("c88eb0a1-afad-4c8b-8ccb-59701eb96677")].QueryType = "EQ";
			queryDictionary[new Guid("c88eb0a1-afad-4c8b-8ccb-59701eb96677")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("c88eb0a1-afad-4c8b-8ccb-59701eb96677"))) {queryDictionary[new Guid("c88eb0a1-afad-4c8b-8ccb-59701eb96677")].SubQueries = subQueryDictionary[new Guid("c88eb0a1-afad-4c8b-8ccb-59701eb96677")];}
			if(!subQueryDictionary.ContainsKey(new Guid("ecab02b9-f80e-45c1-bec3-91383c47e8a2"))) {subQueryDictionary[new Guid("ecab02b9-f80e-45c1-bec3-91383c47e8a2")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("ecab02b9-f80e-45c1-bec3-91383c47e8a2")].Add(queryDictionary[new Guid("c88eb0a1-afad-4c8b-8ccb-59701eb96677")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("ecab02b9-f80e-45c1-bec3-91383c47e8a2"))) {queryDictionary[new Guid("ecab02b9-f80e-45c1-bec3-91383c47e8a2")].SubQueries = subQueryDictionary[new Guid("ecab02b9-f80e-45c1-bec3-91383c47e8a2")];}
		if(!subQueryDictionary.ContainsKey(new Guid("0a0741f1-f63e-441f-8679-33d5fcf38db6"))) {subQueryDictionary[new Guid("0a0741f1-f63e-441f-8679-33d5fcf38db6")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("0a0741f1-f63e-441f-8679-33d5fcf38db6")].Add(queryDictionary[new Guid("ecab02b9-f80e-45c1-bec3-91383c47e8a2")]);
		}
		{
		queryDictionary[new Guid("28219ac2-015c-4064-a774-67627264632e")] = new InputRecordListQuery();
		queryDictionary[new Guid("28219ac2-015c-4064-a774-67627264632e")].FieldName = null;
		queryDictionary[new Guid("28219ac2-015c-4064-a774-67627264632e")].FieldValue =  null;
		queryDictionary[new Guid("28219ac2-015c-4064-a774-67627264632e")].QueryType = "AND";
		queryDictionary[new Guid("28219ac2-015c-4064-a774-67627264632e")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("968c922e-9128-451a-8213-066224c16b2b")] = new InputRecordListQuery();
			queryDictionary[new Guid("968c922e-9128-451a-8213-066224c16b2b")].FieldName = "code";
			queryDictionary[new Guid("968c922e-9128-451a-8213-066224c16b2b")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("968c922e-9128-451a-8213-066224c16b2b")].QueryType = "CONTAINS";
			queryDictionary[new Guid("968c922e-9128-451a-8213-066224c16b2b")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("968c922e-9128-451a-8213-066224c16b2b"))) {queryDictionary[new Guid("968c922e-9128-451a-8213-066224c16b2b")].SubQueries = subQueryDictionary[new Guid("968c922e-9128-451a-8213-066224c16b2b")];}
			if(!subQueryDictionary.ContainsKey(new Guid("28219ac2-015c-4064-a774-67627264632e"))) {subQueryDictionary[new Guid("28219ac2-015c-4064-a774-67627264632e")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("28219ac2-015c-4064-a774-67627264632e")].Add(queryDictionary[new Guid("968c922e-9128-451a-8213-066224c16b2b")]);
			}
			{
			queryDictionary[new Guid("3e188145-ffa8-477e-8d9c-9f7c53552d40")] = new InputRecordListQuery();
			queryDictionary[new Guid("3e188145-ffa8-477e-8d9c-9f7c53552d40")].FieldName = "subject";
			queryDictionary[new Guid("3e188145-ffa8-477e-8d9c-9f7c53552d40")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("3e188145-ffa8-477e-8d9c-9f7c53552d40")].QueryType = "CONTAINS";
			queryDictionary[new Guid("3e188145-ffa8-477e-8d9c-9f7c53552d40")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("3e188145-ffa8-477e-8d9c-9f7c53552d40"))) {queryDictionary[new Guid("3e188145-ffa8-477e-8d9c-9f7c53552d40")].SubQueries = subQueryDictionary[new Guid("3e188145-ffa8-477e-8d9c-9f7c53552d40")];}
			if(!subQueryDictionary.ContainsKey(new Guid("28219ac2-015c-4064-a774-67627264632e"))) {subQueryDictionary[new Guid("28219ac2-015c-4064-a774-67627264632e")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("28219ac2-015c-4064-a774-67627264632e")].Add(queryDictionary[new Guid("3e188145-ffa8-477e-8d9c-9f7c53552d40")]);
			}
			{
			queryDictionary[new Guid("0851ac9e-b1b3-4979-b466-02dc1d833d7e")] = new InputRecordListQuery();
			queryDictionary[new Guid("0851ac9e-b1b3-4979-b466-02dc1d833d7e")].FieldName = "status";
			queryDictionary[new Guid("0851ac9e-b1b3-4979-b466-02dc1d833d7e")].FieldValue =  "completed";
			queryDictionary[new Guid("0851ac9e-b1b3-4979-b466-02dc1d833d7e")].QueryType = "NOT";
			queryDictionary[new Guid("0851ac9e-b1b3-4979-b466-02dc1d833d7e")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("0851ac9e-b1b3-4979-b466-02dc1d833d7e"))) {queryDictionary[new Guid("0851ac9e-b1b3-4979-b466-02dc1d833d7e")].SubQueries = subQueryDictionary[new Guid("0851ac9e-b1b3-4979-b466-02dc1d833d7e")];}
			if(!subQueryDictionary.ContainsKey(new Guid("28219ac2-015c-4064-a774-67627264632e"))) {subQueryDictionary[new Guid("28219ac2-015c-4064-a774-67627264632e")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("28219ac2-015c-4064-a774-67627264632e")].Add(queryDictionary[new Guid("0851ac9e-b1b3-4979-b466-02dc1d833d7e")]);
			}
			{
			queryDictionary[new Guid("4f21d8ee-6f97-4550-b19f-a0036edee23f")] = new InputRecordListQuery();
			queryDictionary[new Guid("4f21d8ee-6f97-4550-b19f-a0036edee23f")].FieldName = "priority";
			queryDictionary[new Guid("4f21d8ee-6f97-4550-b19f-a0036edee23f")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("4f21d8ee-6f97-4550-b19f-a0036edee23f")].QueryType = "EQ";
			queryDictionary[new Guid("4f21d8ee-6f97-4550-b19f-a0036edee23f")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("4f21d8ee-6f97-4550-b19f-a0036edee23f"))) {queryDictionary[new Guid("4f21d8ee-6f97-4550-b19f-a0036edee23f")].SubQueries = subQueryDictionary[new Guid("4f21d8ee-6f97-4550-b19f-a0036edee23f")];}
			if(!subQueryDictionary.ContainsKey(new Guid("28219ac2-015c-4064-a774-67627264632e"))) {subQueryDictionary[new Guid("28219ac2-015c-4064-a774-67627264632e")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("28219ac2-015c-4064-a774-67627264632e")].Add(queryDictionary[new Guid("4f21d8ee-6f97-4550-b19f-a0036edee23f")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("28219ac2-015c-4064-a774-67627264632e"))) {queryDictionary[new Guid("28219ac2-015c-4064-a774-67627264632e")].SubQueries = subQueryDictionary[new Guid("28219ac2-015c-4064-a774-67627264632e")];}
		if(!subQueryDictionary.ContainsKey(new Guid("0a0741f1-f63e-441f-8679-33d5fcf38db6"))) {subQueryDictionary[new Guid("0a0741f1-f63e-441f-8679-33d5fcf38db6")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("0a0741f1-f63e-441f-8679-33d5fcf38db6")].Add(queryDictionary[new Guid("28219ac2-015c-4064-a774-67627264632e")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("0a0741f1-f63e-441f-8679-33d5fcf38db6"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("0a0741f1-f63e-441f-8679-33d5fcf38db6")];}
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

#region << ***Update list***  Entity: wv_task List Name: owned_tasks >>
{
	var createListEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "owned_tasks").Id;
	createListInput.Type =  "General";
	createListInput.Name = "owned_tasks";
	createListInput.Label = "All my tasks";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("2.0");
	createListInput.Default = false;
	createListInput.System = true;
	createListInput.CssClass = "task-list";
	createListInput.IconName = "tasks";
	createListInput.VisibleColumnsCount = Int32.Parse("7");
	createListInput.ColumnWidthsCSV = "100px,auto,120px,120px,120px,120px";
	createListInput.PageSize = Int32.Parse("50");
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
		queryDictionary[new Guid("490a2662-85e2-47f4-a0a5-71d705e65b6f")] = new InputRecordListQuery();
		queryDictionary[new Guid("490a2662-85e2-47f4-a0a5-71d705e65b6f")].FieldName = null;
		queryDictionary[new Guid("490a2662-85e2-47f4-a0a5-71d705e65b6f")].FieldValue =  null;
		queryDictionary[new Guid("490a2662-85e2-47f4-a0a5-71d705e65b6f")].QueryType = "OR";
		queryDictionary[new Guid("490a2662-85e2-47f4-a0a5-71d705e65b6f")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("41caed6c-bf8e-4c55-ada3-97c63fe8cbab")] = new InputRecordListQuery();
			queryDictionary[new Guid("41caed6c-bf8e-4c55-ada3-97c63fe8cbab")].FieldName = "owner_id";
			queryDictionary[new Guid("41caed6c-bf8e-4c55-ada3-97c63fe8cbab")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("41caed6c-bf8e-4c55-ada3-97c63fe8cbab")].QueryType = "EQ";
			queryDictionary[new Guid("41caed6c-bf8e-4c55-ada3-97c63fe8cbab")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("41caed6c-bf8e-4c55-ada3-97c63fe8cbab"))) {queryDictionary[new Guid("41caed6c-bf8e-4c55-ada3-97c63fe8cbab")].SubQueries = subQueryDictionary[new Guid("41caed6c-bf8e-4c55-ada3-97c63fe8cbab")];}
			if(!subQueryDictionary.ContainsKey(new Guid("490a2662-85e2-47f4-a0a5-71d705e65b6f"))) {subQueryDictionary[new Guid("490a2662-85e2-47f4-a0a5-71d705e65b6f")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("490a2662-85e2-47f4-a0a5-71d705e65b6f")].Add(queryDictionary[new Guid("41caed6c-bf8e-4c55-ada3-97c63fe8cbab")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("490a2662-85e2-47f4-a0a5-71d705e65b6f"))) {queryDictionary[new Guid("490a2662-85e2-47f4-a0a5-71d705e65b6f")].SubQueries = subQueryDictionary[new Guid("490a2662-85e2-47f4-a0a5-71d705e65b6f")];}
		if(!subQueryDictionary.ContainsKey(new Guid("c8a7741f-823d-4d05-9f29-8572e1890a77"))) {subQueryDictionary[new Guid("c8a7741f-823d-4d05-9f29-8572e1890a77")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("c8a7741f-823d-4d05-9f29-8572e1890a77")].Add(queryDictionary[new Guid("490a2662-85e2-47f4-a0a5-71d705e65b6f")]);
		}
		{
		queryDictionary[new Guid("58a477d0-1982-4985-b700-00305c09cbe5")] = new InputRecordListQuery();
		queryDictionary[new Guid("58a477d0-1982-4985-b700-00305c09cbe5")].FieldName = null;
		queryDictionary[new Guid("58a477d0-1982-4985-b700-00305c09cbe5")].FieldValue =  null;
		queryDictionary[new Guid("58a477d0-1982-4985-b700-00305c09cbe5")].QueryType = "AND";
		queryDictionary[new Guid("58a477d0-1982-4985-b700-00305c09cbe5")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("1099c37d-3c99-4f9e-ace8-a0e9acfb02e6")] = new InputRecordListQuery();
			queryDictionary[new Guid("1099c37d-3c99-4f9e-ace8-a0e9acfb02e6")].FieldName = "code";
			queryDictionary[new Guid("1099c37d-3c99-4f9e-ace8-a0e9acfb02e6")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("1099c37d-3c99-4f9e-ace8-a0e9acfb02e6")].QueryType = "CONTAINS";
			queryDictionary[new Guid("1099c37d-3c99-4f9e-ace8-a0e9acfb02e6")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("1099c37d-3c99-4f9e-ace8-a0e9acfb02e6"))) {queryDictionary[new Guid("1099c37d-3c99-4f9e-ace8-a0e9acfb02e6")].SubQueries = subQueryDictionary[new Guid("1099c37d-3c99-4f9e-ace8-a0e9acfb02e6")];}
			if(!subQueryDictionary.ContainsKey(new Guid("58a477d0-1982-4985-b700-00305c09cbe5"))) {subQueryDictionary[new Guid("58a477d0-1982-4985-b700-00305c09cbe5")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("58a477d0-1982-4985-b700-00305c09cbe5")].Add(queryDictionary[new Guid("1099c37d-3c99-4f9e-ace8-a0e9acfb02e6")]);
			}
			{
			queryDictionary[new Guid("2c502cfd-f1f7-45e6-9ce5-b39d38eafbf8")] = new InputRecordListQuery();
			queryDictionary[new Guid("2c502cfd-f1f7-45e6-9ce5-b39d38eafbf8")].FieldName = "subject";
			queryDictionary[new Guid("2c502cfd-f1f7-45e6-9ce5-b39d38eafbf8")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("2c502cfd-f1f7-45e6-9ce5-b39d38eafbf8")].QueryType = "CONTAINS";
			queryDictionary[new Guid("2c502cfd-f1f7-45e6-9ce5-b39d38eafbf8")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("2c502cfd-f1f7-45e6-9ce5-b39d38eafbf8"))) {queryDictionary[new Guid("2c502cfd-f1f7-45e6-9ce5-b39d38eafbf8")].SubQueries = subQueryDictionary[new Guid("2c502cfd-f1f7-45e6-9ce5-b39d38eafbf8")];}
			if(!subQueryDictionary.ContainsKey(new Guid("58a477d0-1982-4985-b700-00305c09cbe5"))) {subQueryDictionary[new Guid("58a477d0-1982-4985-b700-00305c09cbe5")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("58a477d0-1982-4985-b700-00305c09cbe5")].Add(queryDictionary[new Guid("2c502cfd-f1f7-45e6-9ce5-b39d38eafbf8")]);
			}
			{
			queryDictionary[new Guid("1e876baf-4a06-43bf-bafc-9fa458e50187")] = new InputRecordListQuery();
			queryDictionary[new Guid("1e876baf-4a06-43bf-bafc-9fa458e50187")].FieldName = "status";
			queryDictionary[new Guid("1e876baf-4a06-43bf-bafc-9fa458e50187")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("1e876baf-4a06-43bf-bafc-9fa458e50187")].QueryType = "EQ";
			queryDictionary[new Guid("1e876baf-4a06-43bf-bafc-9fa458e50187")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("1e876baf-4a06-43bf-bafc-9fa458e50187"))) {queryDictionary[new Guid("1e876baf-4a06-43bf-bafc-9fa458e50187")].SubQueries = subQueryDictionary[new Guid("1e876baf-4a06-43bf-bafc-9fa458e50187")];}
			if(!subQueryDictionary.ContainsKey(new Guid("58a477d0-1982-4985-b700-00305c09cbe5"))) {subQueryDictionary[new Guid("58a477d0-1982-4985-b700-00305c09cbe5")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("58a477d0-1982-4985-b700-00305c09cbe5")].Add(queryDictionary[new Guid("1e876baf-4a06-43bf-bafc-9fa458e50187")]);
			}
			{
			queryDictionary[new Guid("72ee8560-9b8b-4d7c-bc82-468a667a8000")] = new InputRecordListQuery();
			queryDictionary[new Guid("72ee8560-9b8b-4d7c-bc82-468a667a8000")].FieldName = "priority";
			queryDictionary[new Guid("72ee8560-9b8b-4d7c-bc82-468a667a8000")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("72ee8560-9b8b-4d7c-bc82-468a667a8000")].QueryType = "EQ";
			queryDictionary[new Guid("72ee8560-9b8b-4d7c-bc82-468a667a8000")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("72ee8560-9b8b-4d7c-bc82-468a667a8000"))) {queryDictionary[new Guid("72ee8560-9b8b-4d7c-bc82-468a667a8000")].SubQueries = subQueryDictionary[new Guid("72ee8560-9b8b-4d7c-bc82-468a667a8000")];}
			if(!subQueryDictionary.ContainsKey(new Guid("58a477d0-1982-4985-b700-00305c09cbe5"))) {subQueryDictionary[new Guid("58a477d0-1982-4985-b700-00305c09cbe5")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("58a477d0-1982-4985-b700-00305c09cbe5")].Add(queryDictionary[new Guid("72ee8560-9b8b-4d7c-bc82-468a667a8000")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("58a477d0-1982-4985-b700-00305c09cbe5"))) {queryDictionary[new Guid("58a477d0-1982-4985-b700-00305c09cbe5")].SubQueries = subQueryDictionary[new Guid("58a477d0-1982-4985-b700-00305c09cbe5")];}
		if(!subQueryDictionary.ContainsKey(new Guid("c8a7741f-823d-4d05-9f29-8572e1890a77"))) {subQueryDictionary[new Guid("c8a7741f-823d-4d05-9f29-8572e1890a77")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("c8a7741f-823d-4d05-9f29-8572e1890a77")].Add(queryDictionary[new Guid("58a477d0-1982-4985-b700-00305c09cbe5")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("c8a7741f-823d-4d05-9f29-8572e1890a77"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("c8a7741f-823d-4d05-9f29-8572e1890a77")];}
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

#region << ***Update list***  Entity: wv_task List Name: project_tasks >>
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
	createListInput.VisibleColumnsCount = Int32.Parse("7");
	createListInput.ColumnWidthsCSV = "100px,auto,120px,120px,120px,120px,120px";
	createListInput.PageSize = Int32.Parse("50");
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
		#region << field from Relation: username >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("263c0b21-88c1-4c2b-80b4-db7402b0d2e2");
			listItemFromRelation.FieldName = "username";
			listItemFromRelation.FieldLabel = "owner";
			listItemFromRelation.FieldPlaceholder = "";
			listItemFromRelation.FieldHelpText = "";
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = "";
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
		queryDictionary[new Guid("43b39c37-fb11-4ef1-859c-f77d336e0ab3")] = new InputRecordListQuery();
		queryDictionary[new Guid("43b39c37-fb11-4ef1-859c-f77d336e0ab3")].FieldName = "subject";
		queryDictionary[new Guid("43b39c37-fb11-4ef1-859c-f77d336e0ab3")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("43b39c37-fb11-4ef1-859c-f77d336e0ab3")].QueryType = "CONTAINS";
		queryDictionary[new Guid("43b39c37-fb11-4ef1-859c-f77d336e0ab3")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("43b39c37-fb11-4ef1-859c-f77d336e0ab3"))) {queryDictionary[new Guid("43b39c37-fb11-4ef1-859c-f77d336e0ab3")].SubQueries = subQueryDictionary[new Guid("43b39c37-fb11-4ef1-859c-f77d336e0ab3")];}
		if(!subQueryDictionary.ContainsKey(new Guid("527206cf-b08e-4b6a-bca0-b1ab928dc43c"))) {subQueryDictionary[new Guid("527206cf-b08e-4b6a-bca0-b1ab928dc43c")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("527206cf-b08e-4b6a-bca0-b1ab928dc43c")].Add(queryDictionary[new Guid("43b39c37-fb11-4ef1-859c-f77d336e0ab3")]);
		}
		{
		queryDictionary[new Guid("b440b0e9-833e-4922-a966-f75ca21af72a")] = new InputRecordListQuery();
		queryDictionary[new Guid("b440b0e9-833e-4922-a966-f75ca21af72a")].FieldName = "status";
		queryDictionary[new Guid("b440b0e9-833e-4922-a966-f75ca21af72a")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("b440b0e9-833e-4922-a966-f75ca21af72a")].QueryType = "EQ";
		queryDictionary[new Guid("b440b0e9-833e-4922-a966-f75ca21af72a")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("b440b0e9-833e-4922-a966-f75ca21af72a"))) {queryDictionary[new Guid("b440b0e9-833e-4922-a966-f75ca21af72a")].SubQueries = subQueryDictionary[new Guid("b440b0e9-833e-4922-a966-f75ca21af72a")];}
		if(!subQueryDictionary.ContainsKey(new Guid("527206cf-b08e-4b6a-bca0-b1ab928dc43c"))) {subQueryDictionary[new Guid("527206cf-b08e-4b6a-bca0-b1ab928dc43c")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("527206cf-b08e-4b6a-bca0-b1ab928dc43c")].Add(queryDictionary[new Guid("b440b0e9-833e-4922-a966-f75ca21af72a")]);
		}
		{
		queryDictionary[new Guid("2499fd62-b829-44b4-9665-ed5e58754b77")] = new InputRecordListQuery();
		queryDictionary[new Guid("2499fd62-b829-44b4-9665-ed5e58754b77")].FieldName = "priority";
		queryDictionary[new Guid("2499fd62-b829-44b4-9665-ed5e58754b77")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("2499fd62-b829-44b4-9665-ed5e58754b77")].QueryType = "EQ";
		queryDictionary[new Guid("2499fd62-b829-44b4-9665-ed5e58754b77")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("2499fd62-b829-44b4-9665-ed5e58754b77"))) {queryDictionary[new Guid("2499fd62-b829-44b4-9665-ed5e58754b77")].SubQueries = subQueryDictionary[new Guid("2499fd62-b829-44b4-9665-ed5e58754b77")];}
		if(!subQueryDictionary.ContainsKey(new Guid("527206cf-b08e-4b6a-bca0-b1ab928dc43c"))) {subQueryDictionary[new Guid("527206cf-b08e-4b6a-bca0-b1ab928dc43c")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("527206cf-b08e-4b6a-bca0-b1ab928dc43c")].Add(queryDictionary[new Guid("2499fd62-b829-44b4-9665-ed5e58754b77")]);
		}
		{
		queryDictionary[new Guid("aa350e0a-dec0-454a-bc27-5f22ffb94cc1")] = new InputRecordListQuery();
		queryDictionary[new Guid("aa350e0a-dec0-454a-bc27-5f22ffb94cc1")].FieldName = "$user_1_n_task_owner.username";
		queryDictionary[new Guid("aa350e0a-dec0-454a-bc27-5f22ffb94cc1")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"$user_1_n_task_owner.username\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("aa350e0a-dec0-454a-bc27-5f22ffb94cc1")].QueryType = "CONTAINS";
		queryDictionary[new Guid("aa350e0a-dec0-454a-bc27-5f22ffb94cc1")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("aa350e0a-dec0-454a-bc27-5f22ffb94cc1"))) {queryDictionary[new Guid("aa350e0a-dec0-454a-bc27-5f22ffb94cc1")].SubQueries = subQueryDictionary[new Guid("aa350e0a-dec0-454a-bc27-5f22ffb94cc1")];}
		if(!subQueryDictionary.ContainsKey(new Guid("527206cf-b08e-4b6a-bca0-b1ab928dc43c"))) {subQueryDictionary[new Guid("527206cf-b08e-4b6a-bca0-b1ab928dc43c")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("527206cf-b08e-4b6a-bca0-b1ab928dc43c")].Add(queryDictionary[new Guid("aa350e0a-dec0-454a-bc27-5f22ffb94cc1")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("527206cf-b08e-4b6a-bca0-b1ab928dc43c"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("527206cf-b08e-4b6a-bca0-b1ab928dc43c")];}
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

#region << ***Update view***  Entity: wv_bug View Name: quick_view >>
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

#region << ***Update list***  Entity: wv_bug List Name: lookup >>
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

#region << ***Update list***  Entity: wv_bug List Name: admin >>
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

#region << ***Update list***  Entity: wv_bug List Name: my_bugs >>
{
	var createListEntity = entMan.ReadEntity(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "my_bugs").Id;
	createListInput.Type =  "General";
	createListInput.Name = "my_bugs";
	createListInput.Label = "My open bugs";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("1.0");
	createListInput.Default = false;
	createListInput.System = true;
	createListInput.CssClass = "bug-list";
	createListInput.IconName = "bug";
	createListInput.VisibleColumnsCount = Int32.Parse("7");
	createListInput.ColumnWidthsCSV = "100px,auto,160px,120px,120px,120px";
	createListInput.PageSize = Int32.Parse("50");
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
		queryDictionary[new Guid("5c818ee6-d295-42de-94c6-a40693e1b8e6")] = new InputRecordListQuery();
		queryDictionary[new Guid("5c818ee6-d295-42de-94c6-a40693e1b8e6")].FieldName = null;
		queryDictionary[new Guid("5c818ee6-d295-42de-94c6-a40693e1b8e6")].FieldValue =  null;
		queryDictionary[new Guid("5c818ee6-d295-42de-94c6-a40693e1b8e6")].QueryType = "AND";
		queryDictionary[new Guid("5c818ee6-d295-42de-94c6-a40693e1b8e6")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("b40d1c4b-adbd-4483-a58f-e15447377092")] = new InputRecordListQuery();
			queryDictionary[new Guid("b40d1c4b-adbd-4483-a58f-e15447377092")].FieldName = "owner_id";
			queryDictionary[new Guid("b40d1c4b-adbd-4483-a58f-e15447377092")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("b40d1c4b-adbd-4483-a58f-e15447377092")].QueryType = "EQ";
			queryDictionary[new Guid("b40d1c4b-adbd-4483-a58f-e15447377092")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("b40d1c4b-adbd-4483-a58f-e15447377092"))) {queryDictionary[new Guid("b40d1c4b-adbd-4483-a58f-e15447377092")].SubQueries = subQueryDictionary[new Guid("b40d1c4b-adbd-4483-a58f-e15447377092")];}
			if(!subQueryDictionary.ContainsKey(new Guid("5c818ee6-d295-42de-94c6-a40693e1b8e6"))) {subQueryDictionary[new Guid("5c818ee6-d295-42de-94c6-a40693e1b8e6")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("5c818ee6-d295-42de-94c6-a40693e1b8e6")].Add(queryDictionary[new Guid("b40d1c4b-adbd-4483-a58f-e15447377092")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("5c818ee6-d295-42de-94c6-a40693e1b8e6"))) {queryDictionary[new Guid("5c818ee6-d295-42de-94c6-a40693e1b8e6")].SubQueries = subQueryDictionary[new Guid("5c818ee6-d295-42de-94c6-a40693e1b8e6")];}
		if(!subQueryDictionary.ContainsKey(new Guid("6c31f5fa-c4e0-4f4e-a9b3-ec5fda1afcf6"))) {subQueryDictionary[new Guid("6c31f5fa-c4e0-4f4e-a9b3-ec5fda1afcf6")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("6c31f5fa-c4e0-4f4e-a9b3-ec5fda1afcf6")].Add(queryDictionary[new Guid("5c818ee6-d295-42de-94c6-a40693e1b8e6")]);
		}
		{
		queryDictionary[new Guid("6716eb74-b213-4900-9244-1b31533b07f4")] = new InputRecordListQuery();
		queryDictionary[new Guid("6716eb74-b213-4900-9244-1b31533b07f4")].FieldName = null;
		queryDictionary[new Guid("6716eb74-b213-4900-9244-1b31533b07f4")].FieldValue =  null;
		queryDictionary[new Guid("6716eb74-b213-4900-9244-1b31533b07f4")].QueryType = "AND";
		queryDictionary[new Guid("6716eb74-b213-4900-9244-1b31533b07f4")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("7d1b6e65-d203-42f3-837a-613e5886c32c")] = new InputRecordListQuery();
			queryDictionary[new Guid("7d1b6e65-d203-42f3-837a-613e5886c32c")].FieldName = "code";
			queryDictionary[new Guid("7d1b6e65-d203-42f3-837a-613e5886c32c")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("7d1b6e65-d203-42f3-837a-613e5886c32c")].QueryType = "CONTAINS";
			queryDictionary[new Guid("7d1b6e65-d203-42f3-837a-613e5886c32c")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("7d1b6e65-d203-42f3-837a-613e5886c32c"))) {queryDictionary[new Guid("7d1b6e65-d203-42f3-837a-613e5886c32c")].SubQueries = subQueryDictionary[new Guid("7d1b6e65-d203-42f3-837a-613e5886c32c")];}
			if(!subQueryDictionary.ContainsKey(new Guid("6716eb74-b213-4900-9244-1b31533b07f4"))) {subQueryDictionary[new Guid("6716eb74-b213-4900-9244-1b31533b07f4")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("6716eb74-b213-4900-9244-1b31533b07f4")].Add(queryDictionary[new Guid("7d1b6e65-d203-42f3-837a-613e5886c32c")]);
			}
			{
			queryDictionary[new Guid("26ef70da-9f7f-4ead-95c6-350c359898a3")] = new InputRecordListQuery();
			queryDictionary[new Guid("26ef70da-9f7f-4ead-95c6-350c359898a3")].FieldName = "subject";
			queryDictionary[new Guid("26ef70da-9f7f-4ead-95c6-350c359898a3")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("26ef70da-9f7f-4ead-95c6-350c359898a3")].QueryType = "FTS";
			queryDictionary[new Guid("26ef70da-9f7f-4ead-95c6-350c359898a3")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("26ef70da-9f7f-4ead-95c6-350c359898a3"))) {queryDictionary[new Guid("26ef70da-9f7f-4ead-95c6-350c359898a3")].SubQueries = subQueryDictionary[new Guid("26ef70da-9f7f-4ead-95c6-350c359898a3")];}
			if(!subQueryDictionary.ContainsKey(new Guid("6716eb74-b213-4900-9244-1b31533b07f4"))) {subQueryDictionary[new Guid("6716eb74-b213-4900-9244-1b31533b07f4")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("6716eb74-b213-4900-9244-1b31533b07f4")].Add(queryDictionary[new Guid("26ef70da-9f7f-4ead-95c6-350c359898a3")]);
			}
			{
			queryDictionary[new Guid("dd17e006-686c-4fcb-9068-0139ff962336")] = new InputRecordListQuery();
			queryDictionary[new Guid("dd17e006-686c-4fcb-9068-0139ff962336")].FieldName = "status";
			queryDictionary[new Guid("dd17e006-686c-4fcb-9068-0139ff962336")].FieldValue =  "closed";
			queryDictionary[new Guid("dd17e006-686c-4fcb-9068-0139ff962336")].QueryType = "NOT";
			queryDictionary[new Guid("dd17e006-686c-4fcb-9068-0139ff962336")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("dd17e006-686c-4fcb-9068-0139ff962336"))) {queryDictionary[new Guid("dd17e006-686c-4fcb-9068-0139ff962336")].SubQueries = subQueryDictionary[new Guid("dd17e006-686c-4fcb-9068-0139ff962336")];}
			if(!subQueryDictionary.ContainsKey(new Guid("6716eb74-b213-4900-9244-1b31533b07f4"))) {subQueryDictionary[new Guid("6716eb74-b213-4900-9244-1b31533b07f4")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("6716eb74-b213-4900-9244-1b31533b07f4")].Add(queryDictionary[new Guid("dd17e006-686c-4fcb-9068-0139ff962336")]);
			}
			{
			queryDictionary[new Guid("8860cbf5-7ad5-48ae-acbc-47a8a60b4f74")] = new InputRecordListQuery();
			queryDictionary[new Guid("8860cbf5-7ad5-48ae-acbc-47a8a60b4f74")].FieldName = "priority";
			queryDictionary[new Guid("8860cbf5-7ad5-48ae-acbc-47a8a60b4f74")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("8860cbf5-7ad5-48ae-acbc-47a8a60b4f74")].QueryType = "EQ";
			queryDictionary[new Guid("8860cbf5-7ad5-48ae-acbc-47a8a60b4f74")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("8860cbf5-7ad5-48ae-acbc-47a8a60b4f74"))) {queryDictionary[new Guid("8860cbf5-7ad5-48ae-acbc-47a8a60b4f74")].SubQueries = subQueryDictionary[new Guid("8860cbf5-7ad5-48ae-acbc-47a8a60b4f74")];}
			if(!subQueryDictionary.ContainsKey(new Guid("6716eb74-b213-4900-9244-1b31533b07f4"))) {subQueryDictionary[new Guid("6716eb74-b213-4900-9244-1b31533b07f4")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("6716eb74-b213-4900-9244-1b31533b07f4")].Add(queryDictionary[new Guid("8860cbf5-7ad5-48ae-acbc-47a8a60b4f74")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("6716eb74-b213-4900-9244-1b31533b07f4"))) {queryDictionary[new Guid("6716eb74-b213-4900-9244-1b31533b07f4")].SubQueries = subQueryDictionary[new Guid("6716eb74-b213-4900-9244-1b31533b07f4")];}
		if(!subQueryDictionary.ContainsKey(new Guid("6c31f5fa-c4e0-4f4e-a9b3-ec5fda1afcf6"))) {subQueryDictionary[new Guid("6c31f5fa-c4e0-4f4e-a9b3-ec5fda1afcf6")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("6c31f5fa-c4e0-4f4e-a9b3-ec5fda1afcf6")].Add(queryDictionary[new Guid("6716eb74-b213-4900-9244-1b31533b07f4")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("6c31f5fa-c4e0-4f4e-a9b3-ec5fda1afcf6"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("6c31f5fa-c4e0-4f4e-a9b3-ec5fda1afcf6")];}
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

#region << ***Update list***  Entity: wv_bug List Name: owned_bugs >>
{
	var createListEntity = entMan.ReadEntity(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "owned_bugs").Id;
	createListInput.Type =  "General";
	createListInput.Name = "owned_bugs";
	createListInput.Label = "All my bugs";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("2.0");
	createListInput.Default = false;
	createListInput.System = true;
	createListInput.CssClass = "bug-list";
	createListInput.IconName = "bug";
	createListInput.VisibleColumnsCount = Int32.Parse("7");
	createListInput.ColumnWidthsCSV = "100px,auto,120px,120px,120px,120px";
	createListInput.PageSize = Int32.Parse("50");
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
		queryDictionary[new Guid("fe71e0a0-9b1a-4aef-aa95-71905db35c68")] = new InputRecordListQuery();
		queryDictionary[new Guid("fe71e0a0-9b1a-4aef-aa95-71905db35c68")].FieldName = null;
		queryDictionary[new Guid("fe71e0a0-9b1a-4aef-aa95-71905db35c68")].FieldValue =  null;
		queryDictionary[new Guid("fe71e0a0-9b1a-4aef-aa95-71905db35c68")].QueryType = "OR";
		queryDictionary[new Guid("fe71e0a0-9b1a-4aef-aa95-71905db35c68")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("eba8b1ff-2bf1-45c3-b707-30785cd248dd")] = new InputRecordListQuery();
			queryDictionary[new Guid("eba8b1ff-2bf1-45c3-b707-30785cd248dd")].FieldName = "owner_id";
			queryDictionary[new Guid("eba8b1ff-2bf1-45c3-b707-30785cd248dd")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("eba8b1ff-2bf1-45c3-b707-30785cd248dd")].QueryType = "EQ";
			queryDictionary[new Guid("eba8b1ff-2bf1-45c3-b707-30785cd248dd")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("eba8b1ff-2bf1-45c3-b707-30785cd248dd"))) {queryDictionary[new Guid("eba8b1ff-2bf1-45c3-b707-30785cd248dd")].SubQueries = subQueryDictionary[new Guid("eba8b1ff-2bf1-45c3-b707-30785cd248dd")];}
			if(!subQueryDictionary.ContainsKey(new Guid("fe71e0a0-9b1a-4aef-aa95-71905db35c68"))) {subQueryDictionary[new Guid("fe71e0a0-9b1a-4aef-aa95-71905db35c68")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("fe71e0a0-9b1a-4aef-aa95-71905db35c68")].Add(queryDictionary[new Guid("eba8b1ff-2bf1-45c3-b707-30785cd248dd")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("fe71e0a0-9b1a-4aef-aa95-71905db35c68"))) {queryDictionary[new Guid("fe71e0a0-9b1a-4aef-aa95-71905db35c68")].SubQueries = subQueryDictionary[new Guid("fe71e0a0-9b1a-4aef-aa95-71905db35c68")];}
		if(!subQueryDictionary.ContainsKey(new Guid("5ddda8c2-eb38-4b6b-85a3-0ed61d38e5be"))) {subQueryDictionary[new Guid("5ddda8c2-eb38-4b6b-85a3-0ed61d38e5be")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("5ddda8c2-eb38-4b6b-85a3-0ed61d38e5be")].Add(queryDictionary[new Guid("fe71e0a0-9b1a-4aef-aa95-71905db35c68")]);
		}
		{
		queryDictionary[new Guid("b937cd98-c336-4ee0-be91-508862fab6f4")] = new InputRecordListQuery();
		queryDictionary[new Guid("b937cd98-c336-4ee0-be91-508862fab6f4")].FieldName = null;
		queryDictionary[new Guid("b937cd98-c336-4ee0-be91-508862fab6f4")].FieldValue =  null;
		queryDictionary[new Guid("b937cd98-c336-4ee0-be91-508862fab6f4")].QueryType = "AND";
		queryDictionary[new Guid("b937cd98-c336-4ee0-be91-508862fab6f4")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("4e72cf81-4812-4e12-a403-6838b4896022")] = new InputRecordListQuery();
			queryDictionary[new Guid("4e72cf81-4812-4e12-a403-6838b4896022")].FieldName = "code";
			queryDictionary[new Guid("4e72cf81-4812-4e12-a403-6838b4896022")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("4e72cf81-4812-4e12-a403-6838b4896022")].QueryType = "CONTAINS";
			queryDictionary[new Guid("4e72cf81-4812-4e12-a403-6838b4896022")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("4e72cf81-4812-4e12-a403-6838b4896022"))) {queryDictionary[new Guid("4e72cf81-4812-4e12-a403-6838b4896022")].SubQueries = subQueryDictionary[new Guid("4e72cf81-4812-4e12-a403-6838b4896022")];}
			if(!subQueryDictionary.ContainsKey(new Guid("b937cd98-c336-4ee0-be91-508862fab6f4"))) {subQueryDictionary[new Guid("b937cd98-c336-4ee0-be91-508862fab6f4")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("b937cd98-c336-4ee0-be91-508862fab6f4")].Add(queryDictionary[new Guid("4e72cf81-4812-4e12-a403-6838b4896022")]);
			}
			{
			queryDictionary[new Guid("fe7ffd4e-2315-40cb-8818-a1e0bf0699c4")] = new InputRecordListQuery();
			queryDictionary[new Guid("fe7ffd4e-2315-40cb-8818-a1e0bf0699c4")].FieldName = "subject";
			queryDictionary[new Guid("fe7ffd4e-2315-40cb-8818-a1e0bf0699c4")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("fe7ffd4e-2315-40cb-8818-a1e0bf0699c4")].QueryType = "FTS";
			queryDictionary[new Guid("fe7ffd4e-2315-40cb-8818-a1e0bf0699c4")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("fe7ffd4e-2315-40cb-8818-a1e0bf0699c4"))) {queryDictionary[new Guid("fe7ffd4e-2315-40cb-8818-a1e0bf0699c4")].SubQueries = subQueryDictionary[new Guid("fe7ffd4e-2315-40cb-8818-a1e0bf0699c4")];}
			if(!subQueryDictionary.ContainsKey(new Guid("b937cd98-c336-4ee0-be91-508862fab6f4"))) {subQueryDictionary[new Guid("b937cd98-c336-4ee0-be91-508862fab6f4")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("b937cd98-c336-4ee0-be91-508862fab6f4")].Add(queryDictionary[new Guid("fe7ffd4e-2315-40cb-8818-a1e0bf0699c4")]);
			}
			{
			queryDictionary[new Guid("68d3aebd-91d6-454d-abec-dba09bcba745")] = new InputRecordListQuery();
			queryDictionary[new Guid("68d3aebd-91d6-454d-abec-dba09bcba745")].FieldName = "status";
			queryDictionary[new Guid("68d3aebd-91d6-454d-abec-dba09bcba745")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("68d3aebd-91d6-454d-abec-dba09bcba745")].QueryType = "EQ";
			queryDictionary[new Guid("68d3aebd-91d6-454d-abec-dba09bcba745")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("68d3aebd-91d6-454d-abec-dba09bcba745"))) {queryDictionary[new Guid("68d3aebd-91d6-454d-abec-dba09bcba745")].SubQueries = subQueryDictionary[new Guid("68d3aebd-91d6-454d-abec-dba09bcba745")];}
			if(!subQueryDictionary.ContainsKey(new Guid("b937cd98-c336-4ee0-be91-508862fab6f4"))) {subQueryDictionary[new Guid("b937cd98-c336-4ee0-be91-508862fab6f4")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("b937cd98-c336-4ee0-be91-508862fab6f4")].Add(queryDictionary[new Guid("68d3aebd-91d6-454d-abec-dba09bcba745")]);
			}
			{
			queryDictionary[new Guid("418b7b68-b55b-47cd-9489-0dab203de018")] = new InputRecordListQuery();
			queryDictionary[new Guid("418b7b68-b55b-47cd-9489-0dab203de018")].FieldName = "priority";
			queryDictionary[new Guid("418b7b68-b55b-47cd-9489-0dab203de018")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("418b7b68-b55b-47cd-9489-0dab203de018")].QueryType = "EQ";
			queryDictionary[new Guid("418b7b68-b55b-47cd-9489-0dab203de018")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("418b7b68-b55b-47cd-9489-0dab203de018"))) {queryDictionary[new Guid("418b7b68-b55b-47cd-9489-0dab203de018")].SubQueries = subQueryDictionary[new Guid("418b7b68-b55b-47cd-9489-0dab203de018")];}
			if(!subQueryDictionary.ContainsKey(new Guid("b937cd98-c336-4ee0-be91-508862fab6f4"))) {subQueryDictionary[new Guid("b937cd98-c336-4ee0-be91-508862fab6f4")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("b937cd98-c336-4ee0-be91-508862fab6f4")].Add(queryDictionary[new Guid("418b7b68-b55b-47cd-9489-0dab203de018")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("b937cd98-c336-4ee0-be91-508862fab6f4"))) {queryDictionary[new Guid("b937cd98-c336-4ee0-be91-508862fab6f4")].SubQueries = subQueryDictionary[new Guid("b937cd98-c336-4ee0-be91-508862fab6f4")];}
		if(!subQueryDictionary.ContainsKey(new Guid("5ddda8c2-eb38-4b6b-85a3-0ed61d38e5be"))) {subQueryDictionary[new Guid("5ddda8c2-eb38-4b6b-85a3-0ed61d38e5be")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("5ddda8c2-eb38-4b6b-85a3-0ed61d38e5be")].Add(queryDictionary[new Guid("b937cd98-c336-4ee0-be91-508862fab6f4")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("5ddda8c2-eb38-4b6b-85a3-0ed61d38e5be"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("5ddda8c2-eb38-4b6b-85a3-0ed61d38e5be")];}
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

#region << ***Update list***  Entity: wv_bug List Name: created_bugs >>
{
	var createListEntity = entMan.ReadEntity(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "created_bugs").Id;
	createListInput.Type =  "General";
	createListInput.Name = "created_bugs";
	createListInput.Label = "Bugs reported by me";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("3.0");
	createListInput.Default = false;
	createListInput.System = true;
	createListInput.CssClass = "bug-list";
	createListInput.IconName = "bug";
	createListInput.VisibleColumnsCount = Int32.Parse("7");
	createListInput.ColumnWidthsCSV = "100px,auto,120px,120px,120px,120px,120px";
	createListInput.PageSize = Int32.Parse("50");
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
		#region << field from Relation: username >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("263c0b21-88c1-4c2b-80b4-db7402b0d2e2");
			listItemFromRelation.FieldName = "username";
			listItemFromRelation.FieldLabel = "User Name";
			listItemFromRelation.FieldPlaceholder = "";
			listItemFromRelation.FieldHelpText = "";
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = "";
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
		queryDictionary[new Guid("2535e02d-4694-4d04-b38a-16f21ef06f96")] = new InputRecordListQuery();
		queryDictionary[new Guid("2535e02d-4694-4d04-b38a-16f21ef06f96")].FieldName = null;
		queryDictionary[new Guid("2535e02d-4694-4d04-b38a-16f21ef06f96")].FieldValue =  null;
		queryDictionary[new Guid("2535e02d-4694-4d04-b38a-16f21ef06f96")].QueryType = "AND";
		queryDictionary[new Guid("2535e02d-4694-4d04-b38a-16f21ef06f96")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("fb6c7b27-9834-4158-a559-bd2546e0bcc9")] = new InputRecordListQuery();
			queryDictionary[new Guid("fb6c7b27-9834-4158-a559-bd2546e0bcc9")].FieldName = "created_by";
			queryDictionary[new Guid("fb6c7b27-9834-4158-a559-bd2546e0bcc9")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("fb6c7b27-9834-4158-a559-bd2546e0bcc9")].QueryType = "EQ";
			queryDictionary[new Guid("fb6c7b27-9834-4158-a559-bd2546e0bcc9")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("fb6c7b27-9834-4158-a559-bd2546e0bcc9"))) {queryDictionary[new Guid("fb6c7b27-9834-4158-a559-bd2546e0bcc9")].SubQueries = subQueryDictionary[new Guid("fb6c7b27-9834-4158-a559-bd2546e0bcc9")];}
			if(!subQueryDictionary.ContainsKey(new Guid("2535e02d-4694-4d04-b38a-16f21ef06f96"))) {subQueryDictionary[new Guid("2535e02d-4694-4d04-b38a-16f21ef06f96")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("2535e02d-4694-4d04-b38a-16f21ef06f96")].Add(queryDictionary[new Guid("fb6c7b27-9834-4158-a559-bd2546e0bcc9")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("2535e02d-4694-4d04-b38a-16f21ef06f96"))) {queryDictionary[new Guid("2535e02d-4694-4d04-b38a-16f21ef06f96")].SubQueries = subQueryDictionary[new Guid("2535e02d-4694-4d04-b38a-16f21ef06f96")];}
		if(!subQueryDictionary.ContainsKey(new Guid("98918e2e-30b1-444f-8d89-4e563c51a3a5"))) {subQueryDictionary[new Guid("98918e2e-30b1-444f-8d89-4e563c51a3a5")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("98918e2e-30b1-444f-8d89-4e563c51a3a5")].Add(queryDictionary[new Guid("2535e02d-4694-4d04-b38a-16f21ef06f96")]);
		}
		{
		queryDictionary[new Guid("940d7533-3b09-4bfc-bce3-b6241f93d782")] = new InputRecordListQuery();
		queryDictionary[new Guid("940d7533-3b09-4bfc-bce3-b6241f93d782")].FieldName = null;
		queryDictionary[new Guid("940d7533-3b09-4bfc-bce3-b6241f93d782")].FieldValue =  null;
		queryDictionary[new Guid("940d7533-3b09-4bfc-bce3-b6241f93d782")].QueryType = "AND";
		queryDictionary[new Guid("940d7533-3b09-4bfc-bce3-b6241f93d782")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("f5ce4857-ba6d-4e10-8607-02090adab6ef")] = new InputRecordListQuery();
			queryDictionary[new Guid("f5ce4857-ba6d-4e10-8607-02090adab6ef")].FieldName = "code";
			queryDictionary[new Guid("f5ce4857-ba6d-4e10-8607-02090adab6ef")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("f5ce4857-ba6d-4e10-8607-02090adab6ef")].QueryType = "CONTAINS";
			queryDictionary[new Guid("f5ce4857-ba6d-4e10-8607-02090adab6ef")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("f5ce4857-ba6d-4e10-8607-02090adab6ef"))) {queryDictionary[new Guid("f5ce4857-ba6d-4e10-8607-02090adab6ef")].SubQueries = subQueryDictionary[new Guid("f5ce4857-ba6d-4e10-8607-02090adab6ef")];}
			if(!subQueryDictionary.ContainsKey(new Guid("940d7533-3b09-4bfc-bce3-b6241f93d782"))) {subQueryDictionary[new Guid("940d7533-3b09-4bfc-bce3-b6241f93d782")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("940d7533-3b09-4bfc-bce3-b6241f93d782")].Add(queryDictionary[new Guid("f5ce4857-ba6d-4e10-8607-02090adab6ef")]);
			}
			{
			queryDictionary[new Guid("2f590ed1-62a1-4b57-96de-f734f4fb6efd")] = new InputRecordListQuery();
			queryDictionary[new Guid("2f590ed1-62a1-4b57-96de-f734f4fb6efd")].FieldName = "subject";
			queryDictionary[new Guid("2f590ed1-62a1-4b57-96de-f734f4fb6efd")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("2f590ed1-62a1-4b57-96de-f734f4fb6efd")].QueryType = "FTS";
			queryDictionary[new Guid("2f590ed1-62a1-4b57-96de-f734f4fb6efd")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("2f590ed1-62a1-4b57-96de-f734f4fb6efd"))) {queryDictionary[new Guid("2f590ed1-62a1-4b57-96de-f734f4fb6efd")].SubQueries = subQueryDictionary[new Guid("2f590ed1-62a1-4b57-96de-f734f4fb6efd")];}
			if(!subQueryDictionary.ContainsKey(new Guid("940d7533-3b09-4bfc-bce3-b6241f93d782"))) {subQueryDictionary[new Guid("940d7533-3b09-4bfc-bce3-b6241f93d782")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("940d7533-3b09-4bfc-bce3-b6241f93d782")].Add(queryDictionary[new Guid("2f590ed1-62a1-4b57-96de-f734f4fb6efd")]);
			}
			{
			queryDictionary[new Guid("d1e507a3-f0a1-486e-ba07-ad4817541033")] = new InputRecordListQuery();
			queryDictionary[new Guid("d1e507a3-f0a1-486e-ba07-ad4817541033")].FieldName = "status";
			queryDictionary[new Guid("d1e507a3-f0a1-486e-ba07-ad4817541033")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("d1e507a3-f0a1-486e-ba07-ad4817541033")].QueryType = "EQ";
			queryDictionary[new Guid("d1e507a3-f0a1-486e-ba07-ad4817541033")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("d1e507a3-f0a1-486e-ba07-ad4817541033"))) {queryDictionary[new Guid("d1e507a3-f0a1-486e-ba07-ad4817541033")].SubQueries = subQueryDictionary[new Guid("d1e507a3-f0a1-486e-ba07-ad4817541033")];}
			if(!subQueryDictionary.ContainsKey(new Guid("940d7533-3b09-4bfc-bce3-b6241f93d782"))) {subQueryDictionary[new Guid("940d7533-3b09-4bfc-bce3-b6241f93d782")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("940d7533-3b09-4bfc-bce3-b6241f93d782")].Add(queryDictionary[new Guid("d1e507a3-f0a1-486e-ba07-ad4817541033")]);
			}
			{
			queryDictionary[new Guid("cf5710fd-9283-42d4-985b-77e5ecacb7c4")] = new InputRecordListQuery();
			queryDictionary[new Guid("cf5710fd-9283-42d4-985b-77e5ecacb7c4")].FieldName = "priority";
			queryDictionary[new Guid("cf5710fd-9283-42d4-985b-77e5ecacb7c4")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("cf5710fd-9283-42d4-985b-77e5ecacb7c4")].QueryType = "EQ";
			queryDictionary[new Guid("cf5710fd-9283-42d4-985b-77e5ecacb7c4")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("cf5710fd-9283-42d4-985b-77e5ecacb7c4"))) {queryDictionary[new Guid("cf5710fd-9283-42d4-985b-77e5ecacb7c4")].SubQueries = subQueryDictionary[new Guid("cf5710fd-9283-42d4-985b-77e5ecacb7c4")];}
			if(!subQueryDictionary.ContainsKey(new Guid("940d7533-3b09-4bfc-bce3-b6241f93d782"))) {subQueryDictionary[new Guid("940d7533-3b09-4bfc-bce3-b6241f93d782")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("940d7533-3b09-4bfc-bce3-b6241f93d782")].Add(queryDictionary[new Guid("cf5710fd-9283-42d4-985b-77e5ecacb7c4")]);
			}
			{
			queryDictionary[new Guid("f7c1bbc5-ecfc-472b-9714-bfa900b06848")] = new InputRecordListQuery();
			queryDictionary[new Guid("f7c1bbc5-ecfc-472b-9714-bfa900b06848")].FieldName = "$user_1_n_bug_owner.username";
			queryDictionary[new Guid("f7c1bbc5-ecfc-472b-9714-bfa900b06848")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"$user_1_n_bug_owner.username\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("f7c1bbc5-ecfc-472b-9714-bfa900b06848")].QueryType = "CONTAINS";
			queryDictionary[new Guid("f7c1bbc5-ecfc-472b-9714-bfa900b06848")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("f7c1bbc5-ecfc-472b-9714-bfa900b06848"))) {queryDictionary[new Guid("f7c1bbc5-ecfc-472b-9714-bfa900b06848")].SubQueries = subQueryDictionary[new Guid("f7c1bbc5-ecfc-472b-9714-bfa900b06848")];}
			if(!subQueryDictionary.ContainsKey(new Guid("940d7533-3b09-4bfc-bce3-b6241f93d782"))) {subQueryDictionary[new Guid("940d7533-3b09-4bfc-bce3-b6241f93d782")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("940d7533-3b09-4bfc-bce3-b6241f93d782")].Add(queryDictionary[new Guid("f7c1bbc5-ecfc-472b-9714-bfa900b06848")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("940d7533-3b09-4bfc-bce3-b6241f93d782"))) {queryDictionary[new Guid("940d7533-3b09-4bfc-bce3-b6241f93d782")].SubQueries = subQueryDictionary[new Guid("940d7533-3b09-4bfc-bce3-b6241f93d782")];}
		if(!subQueryDictionary.ContainsKey(new Guid("98918e2e-30b1-444f-8d89-4e563c51a3a5"))) {subQueryDictionary[new Guid("98918e2e-30b1-444f-8d89-4e563c51a3a5")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("98918e2e-30b1-444f-8d89-4e563c51a3a5")].Add(queryDictionary[new Guid("940d7533-3b09-4bfc-bce3-b6241f93d782")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("98918e2e-30b1-444f-8d89-4e563c51a3a5"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("98918e2e-30b1-444f-8d89-4e563c51a3a5")];}
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

#region << ***Update list***  Entity: wv_bug List Name: all_bugs >>
{
	var createListEntity = entMan.ReadEntity(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "all_bugs").Id;
	createListInput.Type =  "General";
	createListInput.Name = "all_bugs";
	createListInput.Label = "All bugs";
	createListInput.Title = "";
	createListInput.Weight = Decimal.Parse("12.0");
	createListInput.Default = false;
	createListInput.System = true;
	createListInput.CssClass = "bug-list";
	createListInput.IconName = "bug";
	createListInput.VisibleColumnsCount = Int32.Parse("7");
	createListInput.ColumnWidthsCSV = "100px,auto,120px,120px,120px,120px,120px";
	createListInput.PageSize = Int32.Parse("50");
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
		#region << field from Relation: username >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("263c0b21-88c1-4c2b-80b4-db7402b0d2e2");
			listItemFromRelation.FieldName = "username";
			listItemFromRelation.FieldLabel = "User Name";
			listItemFromRelation.FieldPlaceholder = "";
			listItemFromRelation.FieldHelpText = "";
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = "";
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
		queryDictionary[new Guid("21c11ae6-ed83-4e6d-aec2-33fba6bc8968")] = new InputRecordListQuery();
		queryDictionary[new Guid("21c11ae6-ed83-4e6d-aec2-33fba6bc8968")].FieldName = "code";
		queryDictionary[new Guid("21c11ae6-ed83-4e6d-aec2-33fba6bc8968")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("21c11ae6-ed83-4e6d-aec2-33fba6bc8968")].QueryType = "CONTAINS";
		queryDictionary[new Guid("21c11ae6-ed83-4e6d-aec2-33fba6bc8968")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("21c11ae6-ed83-4e6d-aec2-33fba6bc8968"))) {queryDictionary[new Guid("21c11ae6-ed83-4e6d-aec2-33fba6bc8968")].SubQueries = subQueryDictionary[new Guid("21c11ae6-ed83-4e6d-aec2-33fba6bc8968")];}
		if(!subQueryDictionary.ContainsKey(new Guid("608757ed-f20b-499d-a730-8fd4aa2311db"))) {subQueryDictionary[new Guid("608757ed-f20b-499d-a730-8fd4aa2311db")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("608757ed-f20b-499d-a730-8fd4aa2311db")].Add(queryDictionary[new Guid("21c11ae6-ed83-4e6d-aec2-33fba6bc8968")]);
		}
		{
		queryDictionary[new Guid("f47f92d8-7e1d-428a-840c-fe906b131f94")] = new InputRecordListQuery();
		queryDictionary[new Guid("f47f92d8-7e1d-428a-840c-fe906b131f94")].FieldName = "subject";
		queryDictionary[new Guid("f47f92d8-7e1d-428a-840c-fe906b131f94")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("f47f92d8-7e1d-428a-840c-fe906b131f94")].QueryType = "FTS";
		queryDictionary[new Guid("f47f92d8-7e1d-428a-840c-fe906b131f94")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("f47f92d8-7e1d-428a-840c-fe906b131f94"))) {queryDictionary[new Guid("f47f92d8-7e1d-428a-840c-fe906b131f94")].SubQueries = subQueryDictionary[new Guid("f47f92d8-7e1d-428a-840c-fe906b131f94")];}
		if(!subQueryDictionary.ContainsKey(new Guid("608757ed-f20b-499d-a730-8fd4aa2311db"))) {subQueryDictionary[new Guid("608757ed-f20b-499d-a730-8fd4aa2311db")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("608757ed-f20b-499d-a730-8fd4aa2311db")].Add(queryDictionary[new Guid("f47f92d8-7e1d-428a-840c-fe906b131f94")]);
		}
		{
		queryDictionary[new Guid("9b29da60-cf7b-44cf-a0a4-d30fd3de9892")] = new InputRecordListQuery();
		queryDictionary[new Guid("9b29da60-cf7b-44cf-a0a4-d30fd3de9892")].FieldName = "status";
		queryDictionary[new Guid("9b29da60-cf7b-44cf-a0a4-d30fd3de9892")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("9b29da60-cf7b-44cf-a0a4-d30fd3de9892")].QueryType = "EQ";
		queryDictionary[new Guid("9b29da60-cf7b-44cf-a0a4-d30fd3de9892")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("9b29da60-cf7b-44cf-a0a4-d30fd3de9892"))) {queryDictionary[new Guid("9b29da60-cf7b-44cf-a0a4-d30fd3de9892")].SubQueries = subQueryDictionary[new Guid("9b29da60-cf7b-44cf-a0a4-d30fd3de9892")];}
		if(!subQueryDictionary.ContainsKey(new Guid("608757ed-f20b-499d-a730-8fd4aa2311db"))) {subQueryDictionary[new Guid("608757ed-f20b-499d-a730-8fd4aa2311db")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("608757ed-f20b-499d-a730-8fd4aa2311db")].Add(queryDictionary[new Guid("9b29da60-cf7b-44cf-a0a4-d30fd3de9892")]);
		}
		{
		queryDictionary[new Guid("d089162e-2f69-46ec-8d1a-44820c167637")] = new InputRecordListQuery();
		queryDictionary[new Guid("d089162e-2f69-46ec-8d1a-44820c167637")].FieldName = "priority";
		queryDictionary[new Guid("d089162e-2f69-46ec-8d1a-44820c167637")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("d089162e-2f69-46ec-8d1a-44820c167637")].QueryType = "EQ";
		queryDictionary[new Guid("d089162e-2f69-46ec-8d1a-44820c167637")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("d089162e-2f69-46ec-8d1a-44820c167637"))) {queryDictionary[new Guid("d089162e-2f69-46ec-8d1a-44820c167637")].SubQueries = subQueryDictionary[new Guid("d089162e-2f69-46ec-8d1a-44820c167637")];}
		if(!subQueryDictionary.ContainsKey(new Guid("608757ed-f20b-499d-a730-8fd4aa2311db"))) {subQueryDictionary[new Guid("608757ed-f20b-499d-a730-8fd4aa2311db")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("608757ed-f20b-499d-a730-8fd4aa2311db")].Add(queryDictionary[new Guid("d089162e-2f69-46ec-8d1a-44820c167637")]);
		}
		{
		queryDictionary[new Guid("8ca36299-f874-461a-bf4d-fdf44e2b11d6")] = new InputRecordListQuery();
		queryDictionary[new Guid("8ca36299-f874-461a-bf4d-fdf44e2b11d6")].FieldName = "$user_1_n_bug_owner.username";
		queryDictionary[new Guid("8ca36299-f874-461a-bf4d-fdf44e2b11d6")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"$user_1_n_bug_owner.username\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("8ca36299-f874-461a-bf4d-fdf44e2b11d6")].QueryType = "CONTAINS";
		queryDictionary[new Guid("8ca36299-f874-461a-bf4d-fdf44e2b11d6")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("8ca36299-f874-461a-bf4d-fdf44e2b11d6"))) {queryDictionary[new Guid("8ca36299-f874-461a-bf4d-fdf44e2b11d6")].SubQueries = subQueryDictionary[new Guid("8ca36299-f874-461a-bf4d-fdf44e2b11d6")];}
		if(!subQueryDictionary.ContainsKey(new Guid("608757ed-f20b-499d-a730-8fd4aa2311db"))) {subQueryDictionary[new Guid("608757ed-f20b-499d-a730-8fd4aa2311db")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("608757ed-f20b-499d-a730-8fd4aa2311db")].Add(queryDictionary[new Guid("8ca36299-f874-461a-bf4d-fdf44e2b11d6")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("608757ed-f20b-499d-a730-8fd4aa2311db"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("608757ed-f20b-499d-a730-8fd4aa2311db")];}
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
		var response = entMan.UpdateRecordList(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_bug Updated list: all_bugs Message:" + response.Message);
	}
}
#endregion

#region << ***Update list***  Entity: wv_bug List Name: project_bugs >>
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
	createListInput.VisibleColumnsCount = Int32.Parse("6");
	createListInput.ColumnWidthsCSV = "100px,auto,120px,120px,120px,120px";
	createListInput.PageSize = Int32.Parse("50");
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
		#region << field from Relation: username >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("263c0b21-88c1-4c2b-80b4-db7402b0d2e2");
			listItemFromRelation.FieldName = "username";
			listItemFromRelation.FieldLabel = "owner";
			listItemFromRelation.FieldPlaceholder = "";
			listItemFromRelation.FieldHelpText = "";
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = "";
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
		queryDictionary[new Guid("805c0215-dc56-4d40-ad50-9d774ebec7b8")] = new InputRecordListQuery();
		queryDictionary[new Guid("805c0215-dc56-4d40-ad50-9d774ebec7b8")].FieldName = "subject";
		queryDictionary[new Guid("805c0215-dc56-4d40-ad50-9d774ebec7b8")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("805c0215-dc56-4d40-ad50-9d774ebec7b8")].QueryType = "FTS";
		queryDictionary[new Guid("805c0215-dc56-4d40-ad50-9d774ebec7b8")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("805c0215-dc56-4d40-ad50-9d774ebec7b8"))) {queryDictionary[new Guid("805c0215-dc56-4d40-ad50-9d774ebec7b8")].SubQueries = subQueryDictionary[new Guid("805c0215-dc56-4d40-ad50-9d774ebec7b8")];}
		if(!subQueryDictionary.ContainsKey(new Guid("33082bf6-cd34-497c-9769-cb9912bdbcc1"))) {subQueryDictionary[new Guid("33082bf6-cd34-497c-9769-cb9912bdbcc1")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("33082bf6-cd34-497c-9769-cb9912bdbcc1")].Add(queryDictionary[new Guid("805c0215-dc56-4d40-ad50-9d774ebec7b8")]);
		}
		{
		queryDictionary[new Guid("da985886-b29d-49fc-b683-a5fdee32acad")] = new InputRecordListQuery();
		queryDictionary[new Guid("da985886-b29d-49fc-b683-a5fdee32acad")].FieldName = "status";
		queryDictionary[new Guid("da985886-b29d-49fc-b683-a5fdee32acad")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("da985886-b29d-49fc-b683-a5fdee32acad")].QueryType = "EQ";
		queryDictionary[new Guid("da985886-b29d-49fc-b683-a5fdee32acad")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("da985886-b29d-49fc-b683-a5fdee32acad"))) {queryDictionary[new Guid("da985886-b29d-49fc-b683-a5fdee32acad")].SubQueries = subQueryDictionary[new Guid("da985886-b29d-49fc-b683-a5fdee32acad")];}
		if(!subQueryDictionary.ContainsKey(new Guid("33082bf6-cd34-497c-9769-cb9912bdbcc1"))) {subQueryDictionary[new Guid("33082bf6-cd34-497c-9769-cb9912bdbcc1")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("33082bf6-cd34-497c-9769-cb9912bdbcc1")].Add(queryDictionary[new Guid("da985886-b29d-49fc-b683-a5fdee32acad")]);
		}
		{
		queryDictionary[new Guid("b2a4efc8-d887-44f9-a170-f21789e8eafd")] = new InputRecordListQuery();
		queryDictionary[new Guid("b2a4efc8-d887-44f9-a170-f21789e8eafd")].FieldName = "priority";
		queryDictionary[new Guid("b2a4efc8-d887-44f9-a170-f21789e8eafd")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("b2a4efc8-d887-44f9-a170-f21789e8eafd")].QueryType = "EQ";
		queryDictionary[new Guid("b2a4efc8-d887-44f9-a170-f21789e8eafd")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("b2a4efc8-d887-44f9-a170-f21789e8eafd"))) {queryDictionary[new Guid("b2a4efc8-d887-44f9-a170-f21789e8eafd")].SubQueries = subQueryDictionary[new Guid("b2a4efc8-d887-44f9-a170-f21789e8eafd")];}
		if(!subQueryDictionary.ContainsKey(new Guid("33082bf6-cd34-497c-9769-cb9912bdbcc1"))) {subQueryDictionary[new Guid("33082bf6-cd34-497c-9769-cb9912bdbcc1")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("33082bf6-cd34-497c-9769-cb9912bdbcc1")].Add(queryDictionary[new Guid("b2a4efc8-d887-44f9-a170-f21789e8eafd")]);
		}
		{
		queryDictionary[new Guid("b090c609-ce37-46af-a88b-a933df974646")] = new InputRecordListQuery();
		queryDictionary[new Guid("b090c609-ce37-46af-a88b-a933df974646")].FieldName = "$user_1_n_bug_owner.username";
		queryDictionary[new Guid("b090c609-ce37-46af-a88b-a933df974646")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"$user_1_n_bug_owner.username\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("b090c609-ce37-46af-a88b-a933df974646")].QueryType = "CONTAINS";
		queryDictionary[new Guid("b090c609-ce37-46af-a88b-a933df974646")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("b090c609-ce37-46af-a88b-a933df974646"))) {queryDictionary[new Guid("b090c609-ce37-46af-a88b-a933df974646")].SubQueries = subQueryDictionary[new Guid("b090c609-ce37-46af-a88b-a933df974646")];}
		if(!subQueryDictionary.ContainsKey(new Guid("33082bf6-cd34-497c-9769-cb9912bdbcc1"))) {subQueryDictionary[new Guid("33082bf6-cd34-497c-9769-cb9912bdbcc1")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("33082bf6-cd34-497c-9769-cb9912bdbcc1")].Add(queryDictionary[new Guid("b090c609-ce37-46af-a88b-a933df974646")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("33082bf6-cd34-497c-9769-cb9912bdbcc1"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("33082bf6-cd34-497c-9769-cb9912bdbcc1")];}
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

#region << ***Update view***  Entity: wv_project_attachment View Name: quick_view >>
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

#region << ***Update view***  Entity: wv_project_attachment View Name: general >>
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

#region << ***Update list***  Entity: wv_project_attachment List Name: lookup >>
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

#region << ***Update list***  Entity: wv_project_attachment List Name: task_attachments >>
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

#region << ***Update view***  Entity: wv_project_comment View Name: quick_view >>
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

#region << ***Update view***  Entity: wv_project_comment View Name: general >>
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

#region << ***Update list***  Entity: wv_project_comment List Name: general >>
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

#region << ***Update list***  Entity: wv_project_comment List Name: lookup >>
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

#region << ***Update view***  Entity: wv_customer View Name: quick_view >>
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

#region << ***Update view***  Entity: wv_customer View Name: general >>
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

#region << ***Update view***  Entity: wv_customer View Name: admin_details >>
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

#region << ***Update list***  Entity: wv_customer List Name: admin >>
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
	createListInput.Query = new InputRecordListQuery();
	var queryDictionary = new Dictionary<Guid,InputRecordListQuery>();
	var subQueryDictionary = new Dictionary<Guid,List<InputRecordListQuery>>();
	//Main query rule
	createListInput.Query.FieldName = null;
	createListInput.Query.FieldValue =  null;
	createListInput.Query.QueryType = "AND";
	createListInput.Query.SubQueries = new List<InputRecordListQuery>();
		{
		queryDictionary[new Guid("9f45f1e3-0410-47d3-b8cd-d1ee99fe5504")] = new InputRecordListQuery();
		queryDictionary[new Guid("9f45f1e3-0410-47d3-b8cd-d1ee99fe5504")].FieldName = "name";
		queryDictionary[new Guid("9f45f1e3-0410-47d3-b8cd-d1ee99fe5504")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"name\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("9f45f1e3-0410-47d3-b8cd-d1ee99fe5504")].QueryType = "CONTAINS";
		queryDictionary[new Guid("9f45f1e3-0410-47d3-b8cd-d1ee99fe5504")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("9f45f1e3-0410-47d3-b8cd-d1ee99fe5504"))) {queryDictionary[new Guid("9f45f1e3-0410-47d3-b8cd-d1ee99fe5504")].SubQueries = subQueryDictionary[new Guid("9f45f1e3-0410-47d3-b8cd-d1ee99fe5504")];}
		if(!subQueryDictionary.ContainsKey(new Guid("491f1dff-eaf9-44f4-8a28-d3faf3cb3402"))) {subQueryDictionary[new Guid("491f1dff-eaf9-44f4-8a28-d3faf3cb3402")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("491f1dff-eaf9-44f4-8a28-d3faf3cb3402")].Add(queryDictionary[new Guid("9f45f1e3-0410-47d3-b8cd-d1ee99fe5504")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("491f1dff-eaf9-44f4-8a28-d3faf3cb3402"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("491f1dff-eaf9-44f4-8a28-d3faf3cb3402")];}
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

		#region << sort >>
		{
			var sort = new InputRecordListSort();
			sort.FieldName = "{\"name\":\"url_sort\", \"option\": \"sortBy\", \"default\": null, \"settings\":{\"order\":\"sortOrder\"}}";
			sort.SortType = "Ascending";
			createListInput.Sorts.Add(sort);
		}
		#endregion

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("90bcdb47-2cde-4137-a412-0198348fecc0"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_customer Updated list: admin Message:" + response.Message);
	}
}
#endregion

#region << ***Update view***  Entity: wv_project View Name: dashboard >>
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
			viewItemFromRelation.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
			viewItemFromRelation.EntityName = "wv_task";
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
			viewItemFromRelation.EntityId = new Guid("d691b634-016c-46ef-8ba8-8c3328797497");
			viewItemFromRelation.EntityName = "wv_milestone";
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
			viewItemFromRelation.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
			viewItemFromRelation.EntityName = "wv_bug";
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

#region << ***Update view***  Entity: wv_project View Name: general >>
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

#region << ***Update view***  Entity: wv_project View Name: quick_view >>
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

#region << ***Update view***  Entity: wv_project View Name: admin_details >>
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
					viewRow.Id = new Guid("ca9aedde-ce06-4c57-8942-8a346d594535");
					viewRow.Weight = Decimal.Parse("1.0");
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("4");
					viewColumn.Items = new List<InputRecordViewItemBase>();

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
					#region << Column 2 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("8");
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
					viewRow.Id = new Guid("5d7ce054-f592-41e7-b8f1-424cfd6d38b7");
					viewRow.Weight = Decimal.Parse("2.0");
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("12");
					viewColumn.Items = new List<InputRecordViewItemBase>();

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
				#region << Row 3>>
				{
					var viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("22d11cfc-a763-472b-b509-7ddfe36476bb");
					viewRow.Weight = Decimal.Parse("3.0");
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

#region << ***Update list***  Entity: wv_project List Name: admin >>
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

#region << ***Update view***  Entity: user View Name: quick_view >>
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

#region << ***Update view***  Entity: user View Name: general >>
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

#region << ***Update list***  Entity: user List Name: general >>
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

#region << ***Update list***  Entity: user List Name: lookup >>
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
		queryDictionary[new Guid("4f7a92d1-cac3-4ac5-b69a-5b7b3ac6a218")] = new InputRecordListQuery();
		queryDictionary[new Guid("4f7a92d1-cac3-4ac5-b69a-5b7b3ac6a218")].FieldName = "username";
		queryDictionary[new Guid("4f7a92d1-cac3-4ac5-b69a-5b7b3ac6a218")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"username\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("4f7a92d1-cac3-4ac5-b69a-5b7b3ac6a218")].QueryType = "CONTAINS";
		queryDictionary[new Guid("4f7a92d1-cac3-4ac5-b69a-5b7b3ac6a218")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("4f7a92d1-cac3-4ac5-b69a-5b7b3ac6a218"))) {queryDictionary[new Guid("4f7a92d1-cac3-4ac5-b69a-5b7b3ac6a218")].SubQueries = subQueryDictionary[new Guid("4f7a92d1-cac3-4ac5-b69a-5b7b3ac6a218")];}
		if(!subQueryDictionary.ContainsKey(new Guid("aad5d00e-9270-41ce-89e9-848c8939a241"))) {subQueryDictionary[new Guid("aad5d00e-9270-41ce-89e9-848c8939a241")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("aad5d00e-9270-41ce-89e9-848c8939a241")].Add(queryDictionary[new Guid("4f7a92d1-cac3-4ac5-b69a-5b7b3ac6a218")]);
		}
		{
		queryDictionary[new Guid("f5823bc8-58b8-4289-bd60-72d0ae89d3fd")] = new InputRecordListQuery();
		queryDictionary[new Guid("f5823bc8-58b8-4289-bd60-72d0ae89d3fd")].FieldName = "email";
		queryDictionary[new Guid("f5823bc8-58b8-4289-bd60-72d0ae89d3fd")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"email\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("f5823bc8-58b8-4289-bd60-72d0ae89d3fd")].QueryType = "CONTAINS";
		queryDictionary[new Guid("f5823bc8-58b8-4289-bd60-72d0ae89d3fd")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("f5823bc8-58b8-4289-bd60-72d0ae89d3fd"))) {queryDictionary[new Guid("f5823bc8-58b8-4289-bd60-72d0ae89d3fd")].SubQueries = subQueryDictionary[new Guid("f5823bc8-58b8-4289-bd60-72d0ae89d3fd")];}
		if(!subQueryDictionary.ContainsKey(new Guid("aad5d00e-9270-41ce-89e9-848c8939a241"))) {subQueryDictionary[new Guid("aad5d00e-9270-41ce-89e9-848c8939a241")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("aad5d00e-9270-41ce-89e9-848c8939a241")].Add(queryDictionary[new Guid("f5823bc8-58b8-4289-bd60-72d0ae89d3fd")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("aad5d00e-9270-41ce-89e9-848c8939a241"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("aad5d00e-9270-41ce-89e9-848c8939a241")];}
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

#region << ***Update view***  Entity: role View Name: quick_view >>
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

#region << ***Update view***  Entity: role View Name: general >>
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

#region << ***Update list***  Entity: role List Name: general >>
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

#region << ***Update list***  Entity: role List Name: lookup >>
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

#region << ***Update view***  Entity: area View Name: general >>
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

#region << ***Update view***  Entity: area View Name: quick_view >>
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

#region << ***Update list***  Entity: area List Name: general >>
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

#region << ***Update list***  Entity: area List Name: lookup >>
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

#region << ***Delete relation*** Relation name: user_system_log_created_by >>
{
	{
		var response = relMan.Delete(new Guid("fd947482-89d9-4908-bdc2-7bf619204a87"));
		if (!response.Success)
			throw new Exception("System error 10060. Relation: user_system_log_created_by Delete. Message:" + response.Message);
	}
}
#endregion

#region << ***Delete relation*** Relation name: user_system_log_modified_by >>
{
	{
		var response = relMan.Delete(new Guid("9fb4fabb-8eca-4c80-b4dd-bcbccd94d356"));
		if (!response.Success)
			throw new Exception("System error 10060. Relation: user_system_log_modified_by Delete. Message:" + response.Message);
	}
}
#endregion




		}


    }
}
