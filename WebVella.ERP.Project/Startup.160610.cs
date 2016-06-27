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
		private static void Patch160610(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan, bool createSampleRecords = false)
		{

			#region << Update area: project_admin >>
			{
				var patchObject = new EntityRecord();
				patchObject["id"] = new Guid("5b131255-46fc-459d-bbb5-923a4bdfc006");
				patchObject["attachments"] = "[{\"name\":\"wv_task\",\"label\":\"Task\",\"labelPlural\":\"Tasks\",\"iconName\":\"tasks\",\"weight\":4,\"url\":null,\"view\":{\"name\":\"general\",\"label\":\"[{code}] {subject}\"},\"create\":{\"name\":\"create\",\"label\":\"Create\"},\"list\":{\"name\":\"admin\",\"label\":\"All tasks\"}},{\"name\":\"wv_bug\",\"label\":\"Bug\",\"labelPlural\":\"Bugs\",\"iconName\":\"bug\",\"weight\":5,\"url\":null,\"view\":{\"name\":\"general\",\"label\":\"[{code}] {subject}\"},\"create\":{\"name\":\"create\",\"label\":\"Create\"},\"list\":{\"name\":\"admin\",\"label\":\"All Bugs\"}},{\"name\":\"wv_project\",\"label\":\"Project\",\"labelPlural\":\"Projects\",\"iconName\":\"product-hunt\",\"weight\":22,\"url\":null,\"view\":{\"name\":\"admin_details\",\"label\":\"Project details\"},\"create\":{\"name\":\"admin_create\",\"label\":\"Project create\"},\"list\":{\"name\":\"admin\",\"label\":\"All Projects\"}}]";
				var updateAreaResult = recMan.UpdateRecord("area", patchObject);
				if (!updateAreaResult.Success)
				{
					throw new Exception("System error 10060. Area update with name : project_admin. Message:" + updateAreaResult.Message);
				}
			}
			#endregion

			#region << Update area: projects >>
			{
				var patchObject = new EntityRecord();
				patchObject["id"] = new Guid("205877a1-242c-41bf-a080-49ea01d4f519");
				patchObject["attachments"] = "[{\"name\":null,\"label\":\"My Dashboard\",\"labelPlural\":null,\"iconName\":\"tachometer\",\"weight\":1,\"url\":\"/#/areas/projects/wv_project/dashboard\",\"view\":null,\"create\":null,\"list\":null},{\"name\":\"wv_task\",\"label\":\"Task\",\"labelPlural\":\"Tasks\",\"iconName\":\"tasks\",\"weight\":4,\"url\":null,\"view\":{\"name\":\"general\",\"label\":\"[{code}] {subject}\"},\"create\":{\"name\":\"create\",\"label\":\"Create\"},\"list\":{\"name\":\"my_tasks\",\"label\":\"My Owned Active Tasks\"}},{\"name\":\"wv_bug\",\"label\":\"Bug\",\"labelPlural\":\"Bugs\",\"iconName\":\"bug\",\"weight\":5,\"url\":null,\"view\":{\"name\":\"general\",\"label\":\"[{code}] {subject}\"},\"create\":{\"name\":\"create\",\"label\":\"Create\"},\"list\":{\"name\":\"my_bugs\",\"label\":\"My Owned Open Bugs\"}},{\"name\":\"wv_project\",\"label\":\"Project\",\"labelPlural\":\"Projects\",\"iconName\":\"product-hunt\",\"weight\":22,\"url\":null,\"view\":{\"name\":\"dashboard\",\"label\":\"[{code}] {name}\"},\"create\":{\"name\":\"create\",\"label\":\"Create\"},\"list\":{\"name\":\"my_projects\",\"label\":\"My Projects\"}}]";
				var updateAreaResult = recMan.UpdateRecord("area", patchObject);
				if (!updateAreaResult.Success)
				{
					throw new Exception("System error 10060. Area update with name : projects. Message:" + updateAreaResult.Message);
				}
			}
			#endregion

			#region << View  Enity: area name: create >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("cb434298-8583-4a96-bdbb-97b2c1764192")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("a81daf5f-46ed-4a22-8225-14fabd58be8d");
				createViewInput.Type = "Create";
				createViewInput.Name = "create";
				createViewInput.Label = "Create";
				createViewInput.Title = "Create";
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
					var response = entMan.CreateRecordView(new Guid("cb434298-8583-4a96-bdbb-97b2c1764192"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: area Updated view: create Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: area name: general >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("cb434298-8583-4a96-bdbb-97b2c1764192")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("284d2a04-9347-4769-a6f1-a6ee2cd211a7");
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
					var response = entMan.CreateRecordView(new Guid("cb434298-8583-4a96-bdbb-97b2c1764192"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: area Updated view: general Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: area name: quick_create >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("cb434298-8583-4a96-bdbb-97b2c1764192")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("0ce603bf-3f08-49c5-9e10-f260ad88becf");
				createViewInput.Type = "Quick_Create";
				createViewInput.Name = "quick_create";
				createViewInput.Label = "Quick create";
				createViewInput.Title = "Quick create";
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
					var response = entMan.CreateRecordView(new Guid("cb434298-8583-4a96-bdbb-97b2c1764192"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: area Updated view: quick_create Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: area name: quick_view >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("cb434298-8583-4a96-bdbb-97b2c1764192")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("ab14de6e-55be-49cf-b69d-68e588bda7a7");
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
					var response = entMan.CreateRecordView(new Guid("cb434298-8583-4a96-bdbb-97b2c1764192"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: area Updated view: quick_view Message:" + response.Message);
				}
			}
			#endregion

			#region << List  Enity: area name: general >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("cb434298-8583-4a96-bdbb-97b2c1764192")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = new Guid("e9a9c98a-bc0c-4b9e-901b-5bf9e3b4fb10");
				createListInput.Type = "General";
				createListInput.Name = "general";
				createListInput.Label = "General";
				createListInput.Title = "General";
				createListInput.Weight = Decimal.Parse("10.0");
				createListInput.Default = true;
				createListInput.System = false;
				createListInput.CssClass = null;
				createListInput.IconName = string.IsNullOrEmpty("list") ? string.Empty : "list";
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
					var response = entMan.CreateRecordList(new Guid("cb434298-8583-4a96-bdbb-97b2c1764192"), createListInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: area Created list: general Message:" + response.Message);
				}
			}
			#endregion

			#region << List  Enity: area name: lookup >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("cb434298-8583-4a96-bdbb-97b2c1764192")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = new Guid("28d11c7e-fb2f-47d3-a3c4-7343f5d63e2a");
				createListInput.Type = "Lookup";
				createListInput.Name = "lookup";
				createListInput.Label = "Lookup";
				createListInput.Title = "Lookup";
				createListInput.Weight = Decimal.Parse("10.0");
				createListInput.Default = true;
				createListInput.System = false;
				createListInput.CssClass = null;
				createListInput.IconName = string.IsNullOrEmpty("list") ? string.Empty : "list";
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
					var response = entMan.CreateRecordList(new Guid("cb434298-8583-4a96-bdbb-97b2c1764192"), createListInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: area Created list: lookup Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: wv_project_activity name: create >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("128ee874-085e-436c-bc37-52209190a354");
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
					var response = entMan.CreateRecordView(new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_project_activity Updated view: create Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: wv_project_activity name: quick_create >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("3505918b-7f44-4dc3-afd7-1360aee91841");
				createViewInput.Type = "Quick_Create";
				createViewInput.Name = "quick_create";
				createViewInput.Label = "Quick create";
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
					var response = entMan.CreateRecordView(new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_project_activity Updated view: quick_create Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: wv_project_activity name: quick_view >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("29a2cba8-4224-43d0-b5e3-0a2c33330f70");
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
					var response = entMan.CreateRecordView(new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_project_activity Updated view: quick_view Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: wv_project_activity name: general >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("e3289dae-b8a4-4054-8d3a-b9c9eaba894e");
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
					var response = entMan.CreateRecordView(new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_project_activity Updated view: general Message:" + response.Message);
				}
			}
			#endregion

			#region << Update  Enity: wv_project_activity name: task_activities >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "task_activities").Id;
				createListInput.Type = "Hidden";
				createListInput.Name = "task_activities";
				createListInput.Label = "Activities";
				createListInput.Title = "";
				createListInput.Weight = Decimal.Parse("30.0");
				createListInput.Default = false;
				createListInput.System = true;
				createListInput.CssClass = null;
				createListInput.IconName = "history";
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

				}
				#endregion

				#region << Columns >>
				{
					createListInput.Columns = new List<InputRecordListItemBase>();

					#region << created_on >>
					{
						var listField = new InputRecordListFieldItem();
						listField.EntityId = new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0");
						listField.EntityName = "wv_project_activity";
						listField.FieldId = new Guid("f4d890fd-c3ed-4ea6-9b91-4bd55fe688c7");
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
						listItemFromRelation.RelationId = new Guid("2fd1e3a2-feea-4b2b-a609-d3a5d6694cbb");
						listItemFromRelation.RelationName = "user_wv_project_activity_created_by";
						createListInput.Columns.Add(listItemFromRelation);
					}
					#endregion
					#region << subject >>
					{
						var listField = new InputRecordListFieldItem();
						listField.EntityId = new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0");
						listField.EntityName = "wv_project_activity";
						listField.FieldId = new Guid("8f8b4cb9-aaed-4183-b863-b14faa2496ea");
						listField.FieldName = "subject";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion
					#region << label >>
					{
						var listField = new InputRecordListFieldItem();
						listField.EntityId = new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0");
						listField.EntityName = "wv_project_activity";
						listField.FieldId = new Guid("fe4ee5da-8c32-4ecd-8773-04752b413cb0");
						listField.FieldName = "label";
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
						listItemFromRelation.RelationId = new Guid("2fd1e3a2-feea-4b2b-a609-d3a5d6694cbb");
						listItemFromRelation.RelationName = "user_wv_project_activity_created_by";
						createListInput.Columns.Add(listItemFromRelation);
					}
					#endregion
					#region << created_by >>
					{
						var listField = new InputRecordListFieldItem();
						listField.EntityId = new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0");
						listField.EntityName = "wv_project_activity";
						listField.FieldId = new Guid("1605ec95-ffec-4c66-ba6e-b1e457936306");
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
					var response = entMan.UpdateRecordList(new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0"), createListInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_project_activity Updated list: task_activities Message:" + response.Message);
				}
			}
			#endregion

			#region << Update  Enity: wv_project_activity name: bug_activities >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "bug_activities").Id;
				createListInput.Type = "Hidden";
				createListInput.Name = "bug_activities";
				createListInput.Label = "Activities";
				createListInput.Title = "";
				createListInput.Weight = Decimal.Parse("30.0");
				createListInput.Default = false;
				createListInput.System = true;
				createListInput.CssClass = null;
				createListInput.IconName = "history";
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

				}
				#endregion

				#region << Columns >>
				{
					createListInput.Columns = new List<InputRecordListItemBase>();

					#region << created_on >>
					{
						var listField = new InputRecordListFieldItem();
						listField.EntityId = new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0");
						listField.EntityName = "wv_project_activity";
						listField.FieldId = new Guid("f4d890fd-c3ed-4ea6-9b91-4bd55fe688c7");
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
						listItemFromRelation.RelationId = new Guid("2fd1e3a2-feea-4b2b-a609-d3a5d6694cbb");
						listItemFromRelation.RelationName = "user_wv_project_activity_created_by";
						createListInput.Columns.Add(listItemFromRelation);
					}
					#endregion
					#region << subject >>
					{
						var listField = new InputRecordListFieldItem();
						listField.EntityId = new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0");
						listField.EntityName = "wv_project_activity";
						listField.FieldId = new Guid("8f8b4cb9-aaed-4183-b863-b14faa2496ea");
						listField.FieldName = "subject";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion
					#region << label >>
					{
						var listField = new InputRecordListFieldItem();
						listField.EntityId = new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0");
						listField.EntityName = "wv_project_activity";
						listField.FieldId = new Guid("fe4ee5da-8c32-4ecd-8773-04752b413cb0");
						listField.FieldName = "label";
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
						listItemFromRelation.RelationId = new Guid("2fd1e3a2-feea-4b2b-a609-d3a5d6694cbb");
						listItemFromRelation.RelationName = "user_wv_project_activity_created_by";
						createListInput.Columns.Add(listItemFromRelation);
					}
					#endregion
					#region << created_by >>
					{
						var listField = new InputRecordListFieldItem();
						listField.EntityId = new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0");
						listField.EntityName = "wv_project_activity";
						listField.FieldId = new Guid("1605ec95-ffec-4c66-ba6e-b1e457936306");
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
					var response = entMan.UpdateRecordList(new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0"), createListInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_project_activity Updated list: bug_activities Message:" + response.Message);
				}
			}
			#endregion

			#region << List  Enity: wv_project_activity name: general >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = new Guid("f4a42de5-6605-429d-bf95-e3f9db80c6f9");
				createListInput.Type = "General";
				createListInput.Name = "general";
				createListInput.Label = "General";
				createListInput.Title = "";
				createListInput.Weight = Decimal.Parse("10.0");
				createListInput.Default = true;
				createListInput.System = false;
				createListInput.CssClass = null;
				createListInput.IconName = string.IsNullOrEmpty("list") ? string.Empty : "list";
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
					var response = entMan.CreateRecordList(new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0"), createListInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_project_activity Created list: general Message:" + response.Message);
				}
			}
			#endregion

			#region << List  Enity: wv_project_activity name: lookup >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = new Guid("f5d459d7-7c66-400a-a18a-3d65365b51de");
				createListInput.Type = "Lookup";
				createListInput.Name = "lookup";
				createListInput.Label = "Lookup";
				createListInput.Title = "";
				createListInput.Weight = Decimal.Parse("10.0");
				createListInput.Default = true;
				createListInput.System = false;
				createListInput.CssClass = null;
				createListInput.IconName = string.IsNullOrEmpty("list") ? string.Empty : "list";
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
					var response = entMan.CreateRecordList(new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0"), createListInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_project_activity Created list: lookup Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: wv_task name: quick_create >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("86640d72-a62f-4d9b-ab46-e86d7c6c1a28");
				createViewInput.Type = "Quick_Create";
				createViewInput.Name = "quick_create";
				createViewInput.Label = "Quick create";
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
					var response = entMan.CreateRecordView(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_task Updated view: quick_create Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: wv_task name: quick_view >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("a8a0b604-d117-48fc-8d3a-bb9be579190e");
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
					var response = entMan.CreateRecordView(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_task Updated view: quick_view Message:" + response.Message);
				}
			}
			#endregion

			#region << Update  Enity: wv_task View: create >>
			{
				var updateViewEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
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
						viewSection.Id = new Guid("8b628f5d-16b3-49d0-a433-a910ea208b39");
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
							viewRow.Id = new Guid("818f516c-f6c2-4073-8574-75c13a72aee4");
							viewRow.Weight = Decimal.Parse("1.0");
							viewRow.Columns = new List<InputRecordViewColumn>();

							#region << Column 1 >>
							{
								var viewColumn = new InputRecordViewColumn();
								viewColumn.GridColCount = Int32.Parse("12");
								viewColumn.Items = new List<InputRecordViewItemBase>();

								#region << subject >>
								{
									var viewItem = new InputRecordViewFieldItem();
									viewItem.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
									viewItem.EntityName = "wv_task";
									viewItem.FieldId = new Guid("7843bfbd-30c1-4438-af48-ffe56b7f294a");
									viewItem.FieldName = "subject";
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
									viewItemFromRelation.RelationId = new Guid("1f860b8c-7fa1-40fa-874f-19c2b5309817");
									viewItemFromRelation.RelationName = "project_1_n_task";
									viewColumn.Items.Add(viewItemFromRelation);
								}
								#endregion
								#region << description >>
								{
									var viewItem = new InputRecordViewFieldItem();
									viewItem.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
									viewItem.EntityName = "wv_task";
									viewItem.FieldId = new Guid("a00eb247-918a-46ba-9869-8d1168ea8f45");
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
							viewRow.Id = new Guid("2144c60b-4974-44e2-86ef-5ceec72d04f8");
							viewRow.Weight = Decimal.Parse("2.0");
							viewRow.Columns = new List<InputRecordViewColumn>();

							#region << Column 1 >>
							{
								var viewColumn = new InputRecordViewColumn();
								viewColumn.GridColCount = Int32.Parse("6");
								viewColumn.Items = new List<InputRecordViewItemBase>();

								#region << status >>
								{
									var viewItem = new InputRecordViewFieldItem();
									viewItem.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
									viewItem.EntityName = "wv_task";
									viewItem.FieldId = new Guid("0702e611-6fe5-42e4-9bad-d549cba9cbb1");
									viewItem.FieldName = "status";
									viewItem.Type = "field";
									viewColumn.Items.Add(viewItem);
								}
								#endregion
								#region << start_date >>
								{
									var viewItem = new InputRecordViewFieldItem();
									viewItem.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
									viewItem.EntityName = "wv_task";
									viewItem.FieldId = new Guid("5bf852bf-5e6c-4791-bc8b-f1366440c04b");
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

								#region << priority >>
								{
									var viewItem = new InputRecordViewFieldItem();
									viewItem.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
									viewItem.EntityName = "wv_task";
									viewItem.FieldId = new Guid("04ede478-99ec-4f7f-97af-0df3e89409b1");
									viewItem.FieldName = "priority";
									viewItem.Type = "field";
									viewColumn.Items.Add(viewItem);
								}
								#endregion
								#region << end_date >>
								{
									var viewItem = new InputRecordViewFieldItem();
									viewItem.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
									viewItem.EntityName = "wv_task";
									viewItem.FieldId = new Guid("c6dc9db2-0081-4a91-b0e2-78f9c1c45a51");
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
					var response = entMan.UpdateRecordView(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_task Updated view: create Message:" + response.Message);
				}
			}
			#endregion

			#region << Update  Enity: wv_task View: project_milestone >>
			{
				var updateViewEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "project_milestone").Id;
				createViewInput.Type = "Hidden";
				createViewInput.Name = "project_milestone";
				createViewInput.Label = "Project & Milestone";
				createViewInput.Title = "";
				createViewInput.Default = false;
				createViewInput.System = false;
				createViewInput.Weight = Decimal.Parse("10.0");
				createViewInput.CssClass = null;
				createViewInput.IconName = "code";
				createViewInput.DynamicHtmlTemplate = "/plugins/webvella-projects/templates/task-project-milestone-selection.html";
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
						viewSection.Id = new Guid("e886ea83-cb6f-408e-9fa9-a53cd249b714");
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
							viewRow.Id = new Guid("20be17ac-e915-4ac3-87a3-ab1ef534975f");
							viewRow.Weight = Decimal.Parse("1.0");
							viewRow.Columns = new List<InputRecordViewColumn>();

							#region << Column 1 >>
							{
								var viewColumn = new InputRecordViewColumn();
								viewColumn.GridColCount = Int32.Parse("12");
								viewColumn.Items = new List<InputRecordViewItemBase>();

								#region << field from Relation: name >>
								{
									var viewItemFromRelation = new InputRecordViewRelationFieldItem();
									viewItemFromRelation.EntityId = new Guid("d691b634-016c-46ef-8ba8-8c3328797497");
									viewItemFromRelation.EntityName = "wv_milestone";
									viewItemFromRelation.Type = "fieldFromRelation";
									viewItemFromRelation.FieldId = new Guid("94cc3894-110a-4bb7-8c75-3e887cc83217");
									viewItemFromRelation.FieldName = "name";
									viewItemFromRelation.FieldLabel = "Milestone";
									viewItemFromRelation.FieldPlaceholder = "";
									viewItemFromRelation.FieldHelpText = null;
									viewItemFromRelation.FieldRequired = true;
									viewItemFromRelation.FieldLookupList = "lookup";
									viewItemFromRelation.RelationId = new Guid("3b600a1c-066e-42e2-a678-0de4f0f8a9e1");
									viewItemFromRelation.RelationName = "milestone_1_n_task";
									viewColumn.Items.Add(viewItemFromRelation);
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
									viewItemFromRelation.FieldLabel = "Milestone";
									viewItemFromRelation.FieldPlaceholder = "";
									viewItemFromRelation.FieldHelpText = null;
									viewItemFromRelation.FieldRequired = true;
									viewItemFromRelation.FieldLookupList = "lookup";
									viewItemFromRelation.RelationId = new Guid("1f860b8c-7fa1-40fa-874f-19c2b5309817");
									viewItemFromRelation.RelationName = "project_1_n_task";
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

				}
				#endregion

				#region << Sidebar >>
				createViewInput.Sidebar = new InputRecordViewSidebar();
				createViewInput.Sidebar.CssClass = "";
				createViewInput.Sidebar.Render = true;
				createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

				#endregion
				{
					var response = entMan.UpdateRecordView(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_task Updated view: project_milestone Message:" + response.Message);
				}
			}
			#endregion

			#region << Update  Enity: wv_task View: general >>
			{
				var updateViewEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "general").Id;
				createViewInput.Type = "General";
				createViewInput.Name = "general";
				createViewInput.Label = "General";
				createViewInput.Title = "[{code}] {subject}";
				createViewInput.Default = true;
				createViewInput.System = false;
				createViewInput.Weight = Decimal.Parse("10.0");
				createViewInput.CssClass = "";
				createViewInput.IconName = "tasks";
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
						viewSection.Id = new Guid("0289b876-b6be-4d5f-915b-22dc0428bc25");
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
							viewRow.Id = new Guid("cbf260ae-07e3-4e66-be57-beb7a36779bf");
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
									viewItem.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
									viewItem.EntityName = "wv_task";
									viewItem.FieldId = new Guid("7843bfbd-30c1-4438-af48-ffe56b7f294a");
									viewItem.FieldName = "subject";
									viewItem.Type = "field";
									viewColumn.Items.Add(viewItem);
								}
								#endregion
								#region << View: project_milestone >>
								{
									var viewItem = new InputRecordViewViewItem();
									viewItem.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
									viewItem.EntityName = "wv_task";
									viewItem.ViewId = new Guid("820b6771-3100-4393-982b-3813d79f4df2");
									viewItem.ViewName = "project_milestone";
									viewItem.Type = "view";
									viewColumn.Items.Add(viewItem);
								}
								#endregion
								#region << description >>
								{
									var viewItem = new InputRecordViewFieldItem();
									viewItem.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
									viewItem.EntityName = "wv_task";
									viewItem.FieldId = new Guid("a00eb247-918a-46ba-9869-8d1168ea8f45");
									viewItem.FieldName = "description";
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
								viewColumn.GridColCount = Int32.Parse("4");
								viewColumn.Items = new List<InputRecordViewItemBase>();

								#region << code >>
								{
									var viewItem = new InputRecordViewFieldItem();
									viewItem.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
									viewItem.EntityName = "wv_task";
									viewItem.FieldId = new Guid("04f31ad8-4583-4237-8d54-f82c3f44b918");
									viewItem.FieldName = "code";
									viewItem.Type = "field";
									viewColumn.Items.Add(viewItem);
								}
								#endregion
								#region << status >>
								{
									var viewItem = new InputRecordViewFieldItem();
									viewItem.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
									viewItem.EntityName = "wv_task";
									viewItem.FieldId = new Guid("0702e611-6fe5-42e4-9bad-d549cba9cbb1");
									viewItem.FieldName = "status";
									viewItem.Type = "field";
									viewColumn.Items.Add(viewItem);
								}
								#endregion
								#region << priority >>
								{
									var viewItem = new InputRecordViewFieldItem();
									viewItem.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
									viewItem.EntityName = "wv_task";
									viewItem.FieldId = new Guid("04ede478-99ec-4f7f-97af-0df3e89409b1");
									viewItem.FieldName = "priority";
									viewItem.Type = "field";
									viewColumn.Items.Add(viewItem);
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
									viewItemFromRelation.RelationId = new Guid("7ce76c81-e604-401e-907f-23de982b930e");
									viewItemFromRelation.RelationName = "user_1_n_task_owner";
									viewColumn.Items.Add(viewItemFromRelation);
								}
								#endregion
								#region << start_date >>
								{
									var viewItem = new InputRecordViewFieldItem();
									viewItem.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
									viewItem.EntityName = "wv_task";
									viewItem.FieldId = new Guid("5bf852bf-5e6c-4791-bc8b-f1366440c04b");
									viewItem.FieldName = "start_date";
									viewItem.Type = "field";
									viewColumn.Items.Add(viewItem);
								}
								#endregion
								#region << end_date >>
								{
									var viewItem = new InputRecordViewFieldItem();
									viewItem.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
									viewItem.EntityName = "wv_task";
									viewItem.FieldId = new Guid("c6dc9db2-0081-4a91-b0e2-78f9c1c45a51");
									viewItem.FieldName = "end_date";
									viewItem.Type = "field";
									viewColumn.Items.Add(viewItem);
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
									viewItemFromRelation.RelationId = new Guid("de7e1578-8f8f-4454-a954-0fb62d3bf425");
									viewItemFromRelation.RelationName = "user_n_n_task_watchers";
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

				#region << list from relation: task_attachments >>
				{
					var viewItemFromRelation = new InputRecordViewSidebarRelationListItem();
					viewItemFromRelation.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
					viewItemFromRelation.EntityName = "wv_task";
					viewItemFromRelation.ListId = new Guid("6fc374ac-ba6b-4009-ade4-988304071f29");
					viewItemFromRelation.ListName = "task_attachments";
					viewItemFromRelation.FieldLabel = "Attachments";
					viewItemFromRelation.FieldPlaceholder = "";
					viewItemFromRelation.FieldHelpText = "";
					viewItemFromRelation.FieldRequired = false;
					viewItemFromRelation.FieldManageView = "general";
					viewItemFromRelation.FieldLookupList = "lookup";
					viewItemFromRelation.RelationId = new Guid("f79f76e2-06b1-463a-9675-63845814bf22");
					viewItemFromRelation.RelationName = "task_1_n_attachment";
					viewItemFromRelation.Type = "listFromRelation";
					createViewInput.Sidebar.Items.Add(viewItemFromRelation);
				}
				#endregion

				#region << list from relation: task_comments >>
				{
					var viewItemFromRelation = new InputRecordViewSidebarRelationListItem();
					viewItemFromRelation.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
					viewItemFromRelation.EntityName = "wv_task";
					viewItemFromRelation.ListId = new Guid("b8a7a81d-9176-47e6-90c5-3cabc2a4ceff");
					viewItemFromRelation.ListName = "task_comments";
					viewItemFromRelation.FieldLabel = "Comments";
					viewItemFromRelation.FieldPlaceholder = "";
					viewItemFromRelation.FieldHelpText = "";
					viewItemFromRelation.FieldRequired = false;
					viewItemFromRelation.FieldManageView = "general";
					viewItemFromRelation.FieldLookupList = "lookup";
					viewItemFromRelation.RelationId = new Guid("884b9480-dc1c-468a-98f0-2d5f10084622");
					viewItemFromRelation.RelationName = "task_1_n_comment";
					viewItemFromRelation.Type = "listFromRelation";
					createViewInput.Sidebar.Items.Add(viewItemFromRelation);
				}
				#endregion

				#region << list from relation: task_timelogs >>
				{
					var viewItemFromRelation = new InputRecordViewSidebarRelationListItem();
					viewItemFromRelation.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
					viewItemFromRelation.EntityName = "wv_task";
					viewItemFromRelation.ListId = new Guid("c105b3f8-e140-4150-a587-a31cf600d99b");
					viewItemFromRelation.ListName = "task_timelogs";
					viewItemFromRelation.FieldLabel = "Time logs";
					viewItemFromRelation.FieldPlaceholder = "";
					viewItemFromRelation.FieldHelpText = "";
					viewItemFromRelation.FieldRequired = false;
					viewItemFromRelation.FieldManageView = "general";
					viewItemFromRelation.FieldLookupList = "lookup";
					viewItemFromRelation.RelationId = new Guid("61f1cd54-bcd6-4061-9c96-7934e01f0857");
					viewItemFromRelation.RelationName = "task_1_n_time_log";
					viewItemFromRelation.Type = "listFromRelation";
					createViewInput.Sidebar.Items.Add(viewItemFromRelation);
				}
				#endregion

				#region << list from relation: task_activities >>
				{
					var viewItemFromRelation = new InputRecordViewSidebarRelationListItem();
					viewItemFromRelation.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
					viewItemFromRelation.EntityName = "wv_task";
					viewItemFromRelation.ListId = new Guid("121ce540-7838-4459-8357-d0d0ad2b65a4");
					viewItemFromRelation.ListName = "task_activities";
					viewItemFromRelation.FieldLabel = "Activities";
					viewItemFromRelation.FieldPlaceholder = "";
					viewItemFromRelation.FieldHelpText = "";
					viewItemFromRelation.FieldRequired = false;
					viewItemFromRelation.FieldManageView = "general";
					viewItemFromRelation.FieldLookupList = "lookup";
					viewItemFromRelation.RelationId = new Guid("8f294277-fd60-496e-bff7-9391fffcda41");
					viewItemFromRelation.RelationName = "task_1_n_activity";
					viewItemFromRelation.Type = "listFromRelation";
					createViewInput.Sidebar.Items.Add(viewItemFromRelation);
				}
				#endregion

				#endregion
				{
					var response = entMan.UpdateRecordView(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_task Updated view: general Message:" + response.Message);
				}
			}
			#endregion

			#region << List  Enity: wv_task name: lookup >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = new Guid("62da6ee5-8c88-4c9a-a75c-bc1a2eb5c733");
				createListInput.Type = "Lookup";
				createListInput.Name = "lookup";
				createListInput.Label = "Lookup";
				createListInput.Title = "";
				createListInput.Weight = Decimal.Parse("10.0");
				createListInput.Default = true;
				createListInput.System = false;
				createListInput.CssClass = null;
				createListInput.IconName = string.IsNullOrEmpty("list") ? string.Empty : "list";
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
					var response = entMan.CreateRecordList(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89"), createListInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_task Created list: lookup Message:" + response.Message);
				}
			}
			#endregion

			#region << Update  Enity: wv_task name: project_tasks >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "project_tasks").Id;
				createListInput.Type = "Hidden";
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
					var queryDictionary = new Dictionary<Guid, InputRecordListQuery>();
					var subQueryDictionary = new Dictionary<Guid, List<InputRecordListQuery>>();
					//Main query rule
					createListInput.Query.FieldName = null;
					createListInput.Query.FieldValue = null;
					createListInput.Query.QueryType = "AND";
					createListInput.Query.SubQueries = new List<InputRecordListQuery>();
					{
						queryDictionary[new Guid("e8fd344c-be58-45c7-9b87-8388c7b4cb6a")] = new InputRecordListQuery();
						queryDictionary[new Guid("e8fd344c-be58-45c7-9b87-8388c7b4cb6a")].FieldName = "subject";
						queryDictionary[new Guid("e8fd344c-be58-45c7-9b87-8388c7b4cb6a")].FieldValue = "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
						queryDictionary[new Guid("e8fd344c-be58-45c7-9b87-8388c7b4cb6a")].QueryType = "CONTAINS";
						queryDictionary[new Guid("e8fd344c-be58-45c7-9b87-8388c7b4cb6a")].SubQueries = new List<InputRecordListQuery>();
						if (subQueryDictionary.ContainsKey(new Guid("e8fd344c-be58-45c7-9b87-8388c7b4cb6a"))) { queryDictionary[new Guid("e8fd344c-be58-45c7-9b87-8388c7b4cb6a")].SubQueries = subQueryDictionary[new Guid("e8fd344c-be58-45c7-9b87-8388c7b4cb6a")]; }
						if (!subQueryDictionary.ContainsKey(new Guid("ddeca57d-5267-4d75-987c-fe23de29ec0b"))) { subQueryDictionary[new Guid("ddeca57d-5267-4d75-987c-fe23de29ec0b")] = new List<InputRecordListQuery>(); }
						subQueryDictionary[new Guid("ddeca57d-5267-4d75-987c-fe23de29ec0b")].Add(queryDictionary[new Guid("e8fd344c-be58-45c7-9b87-8388c7b4cb6a")]);
					}
					{
						queryDictionary[new Guid("bdcc6e9b-cb20-4d61-a944-e63b31110b11")] = new InputRecordListQuery();
						queryDictionary[new Guid("bdcc6e9b-cb20-4d61-a944-e63b31110b11")].FieldName = "status";
						queryDictionary[new Guid("bdcc6e9b-cb20-4d61-a944-e63b31110b11")].FieldValue = "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
						queryDictionary[new Guid("bdcc6e9b-cb20-4d61-a944-e63b31110b11")].QueryType = "EQ";
						queryDictionary[new Guid("bdcc6e9b-cb20-4d61-a944-e63b31110b11")].SubQueries = new List<InputRecordListQuery>();
						if (subQueryDictionary.ContainsKey(new Guid("bdcc6e9b-cb20-4d61-a944-e63b31110b11"))) { queryDictionary[new Guid("bdcc6e9b-cb20-4d61-a944-e63b31110b11")].SubQueries = subQueryDictionary[new Guid("bdcc6e9b-cb20-4d61-a944-e63b31110b11")]; }
						if (!subQueryDictionary.ContainsKey(new Guid("ddeca57d-5267-4d75-987c-fe23de29ec0b"))) { subQueryDictionary[new Guid("ddeca57d-5267-4d75-987c-fe23de29ec0b")] = new List<InputRecordListQuery>(); }
						subQueryDictionary[new Guid("ddeca57d-5267-4d75-987c-fe23de29ec0b")].Add(queryDictionary[new Guid("bdcc6e9b-cb20-4d61-a944-e63b31110b11")]);
					}
					{
						queryDictionary[new Guid("d89ceb3c-ea05-4f14-9b6d-8528c63868b2")] = new InputRecordListQuery();
						queryDictionary[new Guid("d89ceb3c-ea05-4f14-9b6d-8528c63868b2")].FieldName = "priority";
						queryDictionary[new Guid("d89ceb3c-ea05-4f14-9b6d-8528c63868b2")].FieldValue = "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
						queryDictionary[new Guid("d89ceb3c-ea05-4f14-9b6d-8528c63868b2")].QueryType = "EQ";
						queryDictionary[new Guid("d89ceb3c-ea05-4f14-9b6d-8528c63868b2")].SubQueries = new List<InputRecordListQuery>();
						if (subQueryDictionary.ContainsKey(new Guid("d89ceb3c-ea05-4f14-9b6d-8528c63868b2"))) { queryDictionary[new Guid("d89ceb3c-ea05-4f14-9b6d-8528c63868b2")].SubQueries = subQueryDictionary[new Guid("d89ceb3c-ea05-4f14-9b6d-8528c63868b2")]; }
						if (!subQueryDictionary.ContainsKey(new Guid("ddeca57d-5267-4d75-987c-fe23de29ec0b"))) { subQueryDictionary[new Guid("ddeca57d-5267-4d75-987c-fe23de29ec0b")] = new List<InputRecordListQuery>(); }
						subQueryDictionary[new Guid("ddeca57d-5267-4d75-987c-fe23de29ec0b")].Add(queryDictionary[new Guid("d89ceb3c-ea05-4f14-9b6d-8528c63868b2")]);
					}
					if (subQueryDictionary.ContainsKey(new Guid("ddeca57d-5267-4d75-987c-fe23de29ec0b"))) { createListInput.Query.SubQueries = subQueryDictionary[new Guid("ddeca57d-5267-4d75-987c-fe23de29ec0b")]; }
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

			#region << Update  Enity: wv_task name: created_tasks >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "created_tasks").Id;
				createListInput.Type = "General";
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
					var queryDictionary = new Dictionary<Guid, InputRecordListQuery>();
					var subQueryDictionary = new Dictionary<Guid, List<InputRecordListQuery>>();
					//Main query rule
					createListInput.Query.FieldName = null;
					createListInput.Query.FieldValue = null;
					createListInput.Query.QueryType = "AND";
					createListInput.Query.SubQueries = new List<InputRecordListQuery>();
					{
						queryDictionary[new Guid("5b755dcb-3318-46ca-8ae8-985760406942")] = new InputRecordListQuery();
						queryDictionary[new Guid("5b755dcb-3318-46ca-8ae8-985760406942")].FieldName = null;
						queryDictionary[new Guid("5b755dcb-3318-46ca-8ae8-985760406942")].FieldValue = null;
						queryDictionary[new Guid("5b755dcb-3318-46ca-8ae8-985760406942")].QueryType = "AND";
						queryDictionary[new Guid("5b755dcb-3318-46ca-8ae8-985760406942")].SubQueries = new List<InputRecordListQuery>();
						{
							queryDictionary[new Guid("e5c8c5b9-013e-4264-8f3d-50b7b1ea624a")] = new InputRecordListQuery();
							queryDictionary[new Guid("e5c8c5b9-013e-4264-8f3d-50b7b1ea624a")].FieldName = "created_by";
							queryDictionary[new Guid("e5c8c5b9-013e-4264-8f3d-50b7b1ea624a")].FieldValue = "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
							queryDictionary[new Guid("e5c8c5b9-013e-4264-8f3d-50b7b1ea624a")].QueryType = "EQ";
							queryDictionary[new Guid("e5c8c5b9-013e-4264-8f3d-50b7b1ea624a")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("e5c8c5b9-013e-4264-8f3d-50b7b1ea624a"))) { queryDictionary[new Guid("e5c8c5b9-013e-4264-8f3d-50b7b1ea624a")].SubQueries = subQueryDictionary[new Guid("e5c8c5b9-013e-4264-8f3d-50b7b1ea624a")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("5b755dcb-3318-46ca-8ae8-985760406942"))) { subQueryDictionary[new Guid("5b755dcb-3318-46ca-8ae8-985760406942")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("5b755dcb-3318-46ca-8ae8-985760406942")].Add(queryDictionary[new Guid("e5c8c5b9-013e-4264-8f3d-50b7b1ea624a")]);
						}
						if (subQueryDictionary.ContainsKey(new Guid("5b755dcb-3318-46ca-8ae8-985760406942"))) { queryDictionary[new Guid("5b755dcb-3318-46ca-8ae8-985760406942")].SubQueries = subQueryDictionary[new Guid("5b755dcb-3318-46ca-8ae8-985760406942")]; }
						if (!subQueryDictionary.ContainsKey(new Guid("3cf020bd-7145-40e7-be4f-0dd50950789e"))) { subQueryDictionary[new Guid("3cf020bd-7145-40e7-be4f-0dd50950789e")] = new List<InputRecordListQuery>(); }
						subQueryDictionary[new Guid("3cf020bd-7145-40e7-be4f-0dd50950789e")].Add(queryDictionary[new Guid("5b755dcb-3318-46ca-8ae8-985760406942")]);
					}
					{
						queryDictionary[new Guid("add28557-f311-45cd-be24-063e65d32782")] = new InputRecordListQuery();
						queryDictionary[new Guid("add28557-f311-45cd-be24-063e65d32782")].FieldName = null;
						queryDictionary[new Guid("add28557-f311-45cd-be24-063e65d32782")].FieldValue = null;
						queryDictionary[new Guid("add28557-f311-45cd-be24-063e65d32782")].QueryType = "AND";
						queryDictionary[new Guid("add28557-f311-45cd-be24-063e65d32782")].SubQueries = new List<InputRecordListQuery>();
						{
							queryDictionary[new Guid("58393565-3706-44ba-9553-1701dc8c545e")] = new InputRecordListQuery();
							queryDictionary[new Guid("58393565-3706-44ba-9553-1701dc8c545e")].FieldName = "code";
							queryDictionary[new Guid("58393565-3706-44ba-9553-1701dc8c545e")].FieldValue = "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
							queryDictionary[new Guid("58393565-3706-44ba-9553-1701dc8c545e")].QueryType = "CONTAINS";
							queryDictionary[new Guid("58393565-3706-44ba-9553-1701dc8c545e")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("58393565-3706-44ba-9553-1701dc8c545e"))) { queryDictionary[new Guid("58393565-3706-44ba-9553-1701dc8c545e")].SubQueries = subQueryDictionary[new Guid("58393565-3706-44ba-9553-1701dc8c545e")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("add28557-f311-45cd-be24-063e65d32782"))) { subQueryDictionary[new Guid("add28557-f311-45cd-be24-063e65d32782")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("add28557-f311-45cd-be24-063e65d32782")].Add(queryDictionary[new Guid("58393565-3706-44ba-9553-1701dc8c545e")]);
						}
						{
							queryDictionary[new Guid("931b9ad8-fda1-4080-a472-11013c80d790")] = new InputRecordListQuery();
							queryDictionary[new Guid("931b9ad8-fda1-4080-a472-11013c80d790")].FieldName = "subject";
							queryDictionary[new Guid("931b9ad8-fda1-4080-a472-11013c80d790")].FieldValue = "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
							queryDictionary[new Guid("931b9ad8-fda1-4080-a472-11013c80d790")].QueryType = "CONTAINS";
							queryDictionary[new Guid("931b9ad8-fda1-4080-a472-11013c80d790")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("931b9ad8-fda1-4080-a472-11013c80d790"))) { queryDictionary[new Guid("931b9ad8-fda1-4080-a472-11013c80d790")].SubQueries = subQueryDictionary[new Guid("931b9ad8-fda1-4080-a472-11013c80d790")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("add28557-f311-45cd-be24-063e65d32782"))) { subQueryDictionary[new Guid("add28557-f311-45cd-be24-063e65d32782")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("add28557-f311-45cd-be24-063e65d32782")].Add(queryDictionary[new Guid("931b9ad8-fda1-4080-a472-11013c80d790")]);
						}
						{
							queryDictionary[new Guid("09517e38-7099-4e51-8eb3-28a9956e310d")] = new InputRecordListQuery();
							queryDictionary[new Guid("09517e38-7099-4e51-8eb3-28a9956e310d")].FieldName = "status";
							queryDictionary[new Guid("09517e38-7099-4e51-8eb3-28a9956e310d")].FieldValue = "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
							queryDictionary[new Guid("09517e38-7099-4e51-8eb3-28a9956e310d")].QueryType = "EQ";
							queryDictionary[new Guid("09517e38-7099-4e51-8eb3-28a9956e310d")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("09517e38-7099-4e51-8eb3-28a9956e310d"))) { queryDictionary[new Guid("09517e38-7099-4e51-8eb3-28a9956e310d")].SubQueries = subQueryDictionary[new Guid("09517e38-7099-4e51-8eb3-28a9956e310d")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("add28557-f311-45cd-be24-063e65d32782"))) { subQueryDictionary[new Guid("add28557-f311-45cd-be24-063e65d32782")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("add28557-f311-45cd-be24-063e65d32782")].Add(queryDictionary[new Guid("09517e38-7099-4e51-8eb3-28a9956e310d")]);
						}
						{
							queryDictionary[new Guid("0a8f7332-0c4e-4820-9caa-84f9ebbfebd6")] = new InputRecordListQuery();
							queryDictionary[new Guid("0a8f7332-0c4e-4820-9caa-84f9ebbfebd6")].FieldName = "priority";
							queryDictionary[new Guid("0a8f7332-0c4e-4820-9caa-84f9ebbfebd6")].FieldValue = "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
							queryDictionary[new Guid("0a8f7332-0c4e-4820-9caa-84f9ebbfebd6")].QueryType = "EQ";
							queryDictionary[new Guid("0a8f7332-0c4e-4820-9caa-84f9ebbfebd6")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("0a8f7332-0c4e-4820-9caa-84f9ebbfebd6"))) { queryDictionary[new Guid("0a8f7332-0c4e-4820-9caa-84f9ebbfebd6")].SubQueries = subQueryDictionary[new Guid("0a8f7332-0c4e-4820-9caa-84f9ebbfebd6")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("add28557-f311-45cd-be24-063e65d32782"))) { subQueryDictionary[new Guid("add28557-f311-45cd-be24-063e65d32782")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("add28557-f311-45cd-be24-063e65d32782")].Add(queryDictionary[new Guid("0a8f7332-0c4e-4820-9caa-84f9ebbfebd6")]);
						}
						if (subQueryDictionary.ContainsKey(new Guid("add28557-f311-45cd-be24-063e65d32782"))) { queryDictionary[new Guid("add28557-f311-45cd-be24-063e65d32782")].SubQueries = subQueryDictionary[new Guid("add28557-f311-45cd-be24-063e65d32782")]; }
						if (!subQueryDictionary.ContainsKey(new Guid("3cf020bd-7145-40e7-be4f-0dd50950789e"))) { subQueryDictionary[new Guid("3cf020bd-7145-40e7-be4f-0dd50950789e")] = new List<InputRecordListQuery>(); }
						subQueryDictionary[new Guid("3cf020bd-7145-40e7-be4f-0dd50950789e")].Add(queryDictionary[new Guid("add28557-f311-45cd-be24-063e65d32782")]);
					}
					if (subQueryDictionary.ContainsKey(new Guid("3cf020bd-7145-40e7-be4f-0dd50950789e"))) { createListInput.Query.SubQueries = subQueryDictionary[new Guid("3cf020bd-7145-40e7-be4f-0dd50950789e")]; }
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

			#region << Update  Enity: wv_task name: owned_tasks >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "owned_tasks").Id;
				createListInput.Type = "General";
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
					var queryDictionary = new Dictionary<Guid, InputRecordListQuery>();
					var subQueryDictionary = new Dictionary<Guid, List<InputRecordListQuery>>();
					//Main query rule
					createListInput.Query.FieldName = null;
					createListInput.Query.FieldValue = null;
					createListInput.Query.QueryType = "AND";
					createListInput.Query.SubQueries = new List<InputRecordListQuery>();
					{
						queryDictionary[new Guid("1e474d1c-ea0d-4023-93ca-8c49c483b379")] = new InputRecordListQuery();
						queryDictionary[new Guid("1e474d1c-ea0d-4023-93ca-8c49c483b379")].FieldName = null;
						queryDictionary[new Guid("1e474d1c-ea0d-4023-93ca-8c49c483b379")].FieldValue = null;
						queryDictionary[new Guid("1e474d1c-ea0d-4023-93ca-8c49c483b379")].QueryType = "OR";
						queryDictionary[new Guid("1e474d1c-ea0d-4023-93ca-8c49c483b379")].SubQueries = new List<InputRecordListQuery>();
						{
							queryDictionary[new Guid("b3209fec-501e-422f-8797-f9539079ab5b")] = new InputRecordListQuery();
							queryDictionary[new Guid("b3209fec-501e-422f-8797-f9539079ab5b")].FieldName = "owner_id";
							queryDictionary[new Guid("b3209fec-501e-422f-8797-f9539079ab5b")].FieldValue = "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
							queryDictionary[new Guid("b3209fec-501e-422f-8797-f9539079ab5b")].QueryType = "EQ";
							queryDictionary[new Guid("b3209fec-501e-422f-8797-f9539079ab5b")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("b3209fec-501e-422f-8797-f9539079ab5b"))) { queryDictionary[new Guid("b3209fec-501e-422f-8797-f9539079ab5b")].SubQueries = subQueryDictionary[new Guid("b3209fec-501e-422f-8797-f9539079ab5b")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("1e474d1c-ea0d-4023-93ca-8c49c483b379"))) { subQueryDictionary[new Guid("1e474d1c-ea0d-4023-93ca-8c49c483b379")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("1e474d1c-ea0d-4023-93ca-8c49c483b379")].Add(queryDictionary[new Guid("b3209fec-501e-422f-8797-f9539079ab5b")]);
						}
						if (subQueryDictionary.ContainsKey(new Guid("1e474d1c-ea0d-4023-93ca-8c49c483b379"))) { queryDictionary[new Guid("1e474d1c-ea0d-4023-93ca-8c49c483b379")].SubQueries = subQueryDictionary[new Guid("1e474d1c-ea0d-4023-93ca-8c49c483b379")]; }
						if (!subQueryDictionary.ContainsKey(new Guid("1146eff7-0b32-41c4-b3cf-608a64f7f660"))) { subQueryDictionary[new Guid("1146eff7-0b32-41c4-b3cf-608a64f7f660")] = new List<InputRecordListQuery>(); }
						subQueryDictionary[new Guid("1146eff7-0b32-41c4-b3cf-608a64f7f660")].Add(queryDictionary[new Guid("1e474d1c-ea0d-4023-93ca-8c49c483b379")]);
					}
					{
						queryDictionary[new Guid("0ca0f816-fee2-4e2f-b333-9e7a3a501170")] = new InputRecordListQuery();
						queryDictionary[new Guid("0ca0f816-fee2-4e2f-b333-9e7a3a501170")].FieldName = null;
						queryDictionary[new Guid("0ca0f816-fee2-4e2f-b333-9e7a3a501170")].FieldValue = null;
						queryDictionary[new Guid("0ca0f816-fee2-4e2f-b333-9e7a3a501170")].QueryType = "AND";
						queryDictionary[new Guid("0ca0f816-fee2-4e2f-b333-9e7a3a501170")].SubQueries = new List<InputRecordListQuery>();
						{
							queryDictionary[new Guid("c3632319-0b00-4d00-b344-96d7efbac516")] = new InputRecordListQuery();
							queryDictionary[new Guid("c3632319-0b00-4d00-b344-96d7efbac516")].FieldName = "code";
							queryDictionary[new Guid("c3632319-0b00-4d00-b344-96d7efbac516")].FieldValue = "{\"name\":\"url_query\", \"option\": \"number\", \"default\": null, \"settings\":{}}";
							queryDictionary[new Guid("c3632319-0b00-4d00-b344-96d7efbac516")].QueryType = "CONTAINS";
							queryDictionary[new Guid("c3632319-0b00-4d00-b344-96d7efbac516")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("c3632319-0b00-4d00-b344-96d7efbac516"))) { queryDictionary[new Guid("c3632319-0b00-4d00-b344-96d7efbac516")].SubQueries = subQueryDictionary[new Guid("c3632319-0b00-4d00-b344-96d7efbac516")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("0ca0f816-fee2-4e2f-b333-9e7a3a501170"))) { subQueryDictionary[new Guid("0ca0f816-fee2-4e2f-b333-9e7a3a501170")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("0ca0f816-fee2-4e2f-b333-9e7a3a501170")].Add(queryDictionary[new Guid("c3632319-0b00-4d00-b344-96d7efbac516")]);
						}
						{
							queryDictionary[new Guid("307283e8-2ae6-40ef-8158-c105a4685e8d")] = new InputRecordListQuery();
							queryDictionary[new Guid("307283e8-2ae6-40ef-8158-c105a4685e8d")].FieldName = "subject";
							queryDictionary[new Guid("307283e8-2ae6-40ef-8158-c105a4685e8d")].FieldValue = "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
							queryDictionary[new Guid("307283e8-2ae6-40ef-8158-c105a4685e8d")].QueryType = "CONTAINS";
							queryDictionary[new Guid("307283e8-2ae6-40ef-8158-c105a4685e8d")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("307283e8-2ae6-40ef-8158-c105a4685e8d"))) { queryDictionary[new Guid("307283e8-2ae6-40ef-8158-c105a4685e8d")].SubQueries = subQueryDictionary[new Guid("307283e8-2ae6-40ef-8158-c105a4685e8d")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("0ca0f816-fee2-4e2f-b333-9e7a3a501170"))) { subQueryDictionary[new Guid("0ca0f816-fee2-4e2f-b333-9e7a3a501170")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("0ca0f816-fee2-4e2f-b333-9e7a3a501170")].Add(queryDictionary[new Guid("307283e8-2ae6-40ef-8158-c105a4685e8d")]);
						}
						{
							queryDictionary[new Guid("8f622b34-9026-4ffe-bf4c-d2224e7641c7")] = new InputRecordListQuery();
							queryDictionary[new Guid("8f622b34-9026-4ffe-bf4c-d2224e7641c7")].FieldName = "status";
							queryDictionary[new Guid("8f622b34-9026-4ffe-bf4c-d2224e7641c7")].FieldValue = "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
							queryDictionary[new Guid("8f622b34-9026-4ffe-bf4c-d2224e7641c7")].QueryType = "EQ";
							queryDictionary[new Guid("8f622b34-9026-4ffe-bf4c-d2224e7641c7")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("8f622b34-9026-4ffe-bf4c-d2224e7641c7"))) { queryDictionary[new Guid("8f622b34-9026-4ffe-bf4c-d2224e7641c7")].SubQueries = subQueryDictionary[new Guid("8f622b34-9026-4ffe-bf4c-d2224e7641c7")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("0ca0f816-fee2-4e2f-b333-9e7a3a501170"))) { subQueryDictionary[new Guid("0ca0f816-fee2-4e2f-b333-9e7a3a501170")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("0ca0f816-fee2-4e2f-b333-9e7a3a501170")].Add(queryDictionary[new Guid("8f622b34-9026-4ffe-bf4c-d2224e7641c7")]);
						}
						{
							queryDictionary[new Guid("40cb781a-e358-44e4-9d7d-33a1d54d8f30")] = new InputRecordListQuery();
							queryDictionary[new Guid("40cb781a-e358-44e4-9d7d-33a1d54d8f30")].FieldName = "priority";
							queryDictionary[new Guid("40cb781a-e358-44e4-9d7d-33a1d54d8f30")].FieldValue = "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
							queryDictionary[new Guid("40cb781a-e358-44e4-9d7d-33a1d54d8f30")].QueryType = "EQ";
							queryDictionary[new Guid("40cb781a-e358-44e4-9d7d-33a1d54d8f30")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("40cb781a-e358-44e4-9d7d-33a1d54d8f30"))) { queryDictionary[new Guid("40cb781a-e358-44e4-9d7d-33a1d54d8f30")].SubQueries = subQueryDictionary[new Guid("40cb781a-e358-44e4-9d7d-33a1d54d8f30")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("0ca0f816-fee2-4e2f-b333-9e7a3a501170"))) { subQueryDictionary[new Guid("0ca0f816-fee2-4e2f-b333-9e7a3a501170")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("0ca0f816-fee2-4e2f-b333-9e7a3a501170")].Add(queryDictionary[new Guid("40cb781a-e358-44e4-9d7d-33a1d54d8f30")]);
						}
						if (subQueryDictionary.ContainsKey(new Guid("0ca0f816-fee2-4e2f-b333-9e7a3a501170"))) { queryDictionary[new Guid("0ca0f816-fee2-4e2f-b333-9e7a3a501170")].SubQueries = subQueryDictionary[new Guid("0ca0f816-fee2-4e2f-b333-9e7a3a501170")]; }
						if (!subQueryDictionary.ContainsKey(new Guid("1146eff7-0b32-41c4-b3cf-608a64f7f660"))) { subQueryDictionary[new Guid("1146eff7-0b32-41c4-b3cf-608a64f7f660")] = new List<InputRecordListQuery>(); }
						subQueryDictionary[new Guid("1146eff7-0b32-41c4-b3cf-608a64f7f660")].Add(queryDictionary[new Guid("0ca0f816-fee2-4e2f-b333-9e7a3a501170")]);
					}
					if (subQueryDictionary.ContainsKey(new Guid("1146eff7-0b32-41c4-b3cf-608a64f7f660"))) { createListInput.Query.SubQueries = subQueryDictionary[new Guid("1146eff7-0b32-41c4-b3cf-608a64f7f660")]; }
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

			#region << Update  Enity: wv_task name: all_tasks >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "all_tasks").Id;
				createListInput.Type = "General";
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
					var queryDictionary = new Dictionary<Guid, InputRecordListQuery>();
					var subQueryDictionary = new Dictionary<Guid, List<InputRecordListQuery>>();
					//Main query rule
					createListInput.Query.FieldName = null;
					createListInput.Query.FieldValue = null;
					createListInput.Query.QueryType = "AND";
					createListInput.Query.SubQueries = new List<InputRecordListQuery>();
					{
						queryDictionary[new Guid("18aa08ba-eff8-4701-9be5-fb0ad7e087e9")] = new InputRecordListQuery();
						queryDictionary[new Guid("18aa08ba-eff8-4701-9be5-fb0ad7e087e9")].FieldName = "code";
						queryDictionary[new Guid("18aa08ba-eff8-4701-9be5-fb0ad7e087e9")].FieldValue = "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
						queryDictionary[new Guid("18aa08ba-eff8-4701-9be5-fb0ad7e087e9")].QueryType = "CONTAINS";
						queryDictionary[new Guid("18aa08ba-eff8-4701-9be5-fb0ad7e087e9")].SubQueries = new List<InputRecordListQuery>();
						if (subQueryDictionary.ContainsKey(new Guid("18aa08ba-eff8-4701-9be5-fb0ad7e087e9"))) { queryDictionary[new Guid("18aa08ba-eff8-4701-9be5-fb0ad7e087e9")].SubQueries = subQueryDictionary[new Guid("18aa08ba-eff8-4701-9be5-fb0ad7e087e9")]; }
						if (!subQueryDictionary.ContainsKey(new Guid("7a4e4ce2-ea93-49c9-9cbe-eab2d1f57c63"))) { subQueryDictionary[new Guid("7a4e4ce2-ea93-49c9-9cbe-eab2d1f57c63")] = new List<InputRecordListQuery>(); }
						subQueryDictionary[new Guid("7a4e4ce2-ea93-49c9-9cbe-eab2d1f57c63")].Add(queryDictionary[new Guid("18aa08ba-eff8-4701-9be5-fb0ad7e087e9")]);
					}
					{
						queryDictionary[new Guid("d7667ece-67d5-4c5e-9417-d7b448098ba0")] = new InputRecordListQuery();
						queryDictionary[new Guid("d7667ece-67d5-4c5e-9417-d7b448098ba0")].FieldName = "subject";
						queryDictionary[new Guid("d7667ece-67d5-4c5e-9417-d7b448098ba0")].FieldValue = "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
						queryDictionary[new Guid("d7667ece-67d5-4c5e-9417-d7b448098ba0")].QueryType = "CONTAINS";
						queryDictionary[new Guid("d7667ece-67d5-4c5e-9417-d7b448098ba0")].SubQueries = new List<InputRecordListQuery>();
						if (subQueryDictionary.ContainsKey(new Guid("d7667ece-67d5-4c5e-9417-d7b448098ba0"))) { queryDictionary[new Guid("d7667ece-67d5-4c5e-9417-d7b448098ba0")].SubQueries = subQueryDictionary[new Guid("d7667ece-67d5-4c5e-9417-d7b448098ba0")]; }
						if (!subQueryDictionary.ContainsKey(new Guid("7a4e4ce2-ea93-49c9-9cbe-eab2d1f57c63"))) { subQueryDictionary[new Guid("7a4e4ce2-ea93-49c9-9cbe-eab2d1f57c63")] = new List<InputRecordListQuery>(); }
						subQueryDictionary[new Guid("7a4e4ce2-ea93-49c9-9cbe-eab2d1f57c63")].Add(queryDictionary[new Guid("d7667ece-67d5-4c5e-9417-d7b448098ba0")]);
					}
					{
						queryDictionary[new Guid("420788a4-4b22-4fcf-8a31-fb65ed2a145e")] = new InputRecordListQuery();
						queryDictionary[new Guid("420788a4-4b22-4fcf-8a31-fb65ed2a145e")].FieldName = "status";
						queryDictionary[new Guid("420788a4-4b22-4fcf-8a31-fb65ed2a145e")].FieldValue = "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
						queryDictionary[new Guid("420788a4-4b22-4fcf-8a31-fb65ed2a145e")].QueryType = "EQ";
						queryDictionary[new Guid("420788a4-4b22-4fcf-8a31-fb65ed2a145e")].SubQueries = new List<InputRecordListQuery>();
						if (subQueryDictionary.ContainsKey(new Guid("420788a4-4b22-4fcf-8a31-fb65ed2a145e"))) { queryDictionary[new Guid("420788a4-4b22-4fcf-8a31-fb65ed2a145e")].SubQueries = subQueryDictionary[new Guid("420788a4-4b22-4fcf-8a31-fb65ed2a145e")]; }
						if (!subQueryDictionary.ContainsKey(new Guid("7a4e4ce2-ea93-49c9-9cbe-eab2d1f57c63"))) { subQueryDictionary[new Guid("7a4e4ce2-ea93-49c9-9cbe-eab2d1f57c63")] = new List<InputRecordListQuery>(); }
						subQueryDictionary[new Guid("7a4e4ce2-ea93-49c9-9cbe-eab2d1f57c63")].Add(queryDictionary[new Guid("420788a4-4b22-4fcf-8a31-fb65ed2a145e")]);
					}
					{
						queryDictionary[new Guid("1761300b-bb55-4821-9084-a5c3622ae59b")] = new InputRecordListQuery();
						queryDictionary[new Guid("1761300b-bb55-4821-9084-a5c3622ae59b")].FieldName = "priority";
						queryDictionary[new Guid("1761300b-bb55-4821-9084-a5c3622ae59b")].FieldValue = "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
						queryDictionary[new Guid("1761300b-bb55-4821-9084-a5c3622ae59b")].QueryType = "EQ";
						queryDictionary[new Guid("1761300b-bb55-4821-9084-a5c3622ae59b")].SubQueries = new List<InputRecordListQuery>();
						if (subQueryDictionary.ContainsKey(new Guid("1761300b-bb55-4821-9084-a5c3622ae59b"))) { queryDictionary[new Guid("1761300b-bb55-4821-9084-a5c3622ae59b")].SubQueries = subQueryDictionary[new Guid("1761300b-bb55-4821-9084-a5c3622ae59b")]; }
						if (!subQueryDictionary.ContainsKey(new Guid("7a4e4ce2-ea93-49c9-9cbe-eab2d1f57c63"))) { subQueryDictionary[new Guid("7a4e4ce2-ea93-49c9-9cbe-eab2d1f57c63")] = new List<InputRecordListQuery>(); }
						subQueryDictionary[new Guid("7a4e4ce2-ea93-49c9-9cbe-eab2d1f57c63")].Add(queryDictionary[new Guid("1761300b-bb55-4821-9084-a5c3622ae59b")]);
					}
					if (subQueryDictionary.ContainsKey(new Guid("7a4e4ce2-ea93-49c9-9cbe-eab2d1f57c63"))) { createListInput.Query.SubQueries = subQueryDictionary[new Guid("7a4e4ce2-ea93-49c9-9cbe-eab2d1f57c63")]; }
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

			#region << Update  Enity: wv_task name: admin >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "admin").Id;
				createListInput.Type = "Hidden";
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
					var queryDictionary = new Dictionary<Guid, InputRecordListQuery>();
					var subQueryDictionary = new Dictionary<Guid, List<InputRecordListQuery>>();
					//Main query rule
					createListInput.Query.FieldName = null;
					createListInput.Query.FieldValue = null;
					createListInput.Query.QueryType = "AND";
					createListInput.Query.SubQueries = new List<InputRecordListQuery>();
					{
						queryDictionary[new Guid("92e17b8c-82b7-4022-9976-6ab42b6be8b4")] = new InputRecordListQuery();
						queryDictionary[new Guid("92e17b8c-82b7-4022-9976-6ab42b6be8b4")].FieldName = "owner_id";
						queryDictionary[new Guid("92e17b8c-82b7-4022-9976-6ab42b6be8b4")].FieldValue = "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
						queryDictionary[new Guid("92e17b8c-82b7-4022-9976-6ab42b6be8b4")].QueryType = "EQ";
						queryDictionary[new Guid("92e17b8c-82b7-4022-9976-6ab42b6be8b4")].SubQueries = new List<InputRecordListQuery>();
						if (subQueryDictionary.ContainsKey(new Guid("92e17b8c-82b7-4022-9976-6ab42b6be8b4"))) { queryDictionary[new Guid("92e17b8c-82b7-4022-9976-6ab42b6be8b4")].SubQueries = subQueryDictionary[new Guid("92e17b8c-82b7-4022-9976-6ab42b6be8b4")]; }
						if (!subQueryDictionary.ContainsKey(new Guid("d97d66d8-be9f-45bb-a1d0-f3525653783c"))) { subQueryDictionary[new Guid("d97d66d8-be9f-45bb-a1d0-f3525653783c")] = new List<InputRecordListQuery>(); }
						subQueryDictionary[new Guid("d97d66d8-be9f-45bb-a1d0-f3525653783c")].Add(queryDictionary[new Guid("92e17b8c-82b7-4022-9976-6ab42b6be8b4")]);
					}
					{
						queryDictionary[new Guid("1b3a6dd1-9c71-43d3-86ff-dbfa3ed944d7")] = new InputRecordListQuery();
						queryDictionary[new Guid("1b3a6dd1-9c71-43d3-86ff-dbfa3ed944d7")].FieldName = "subject";
						queryDictionary[new Guid("1b3a6dd1-9c71-43d3-86ff-dbfa3ed944d7")].FieldValue = "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
						queryDictionary[new Guid("1b3a6dd1-9c71-43d3-86ff-dbfa3ed944d7")].QueryType = "EQ";
						queryDictionary[new Guid("1b3a6dd1-9c71-43d3-86ff-dbfa3ed944d7")].SubQueries = new List<InputRecordListQuery>();
						if (subQueryDictionary.ContainsKey(new Guid("1b3a6dd1-9c71-43d3-86ff-dbfa3ed944d7"))) { queryDictionary[new Guid("1b3a6dd1-9c71-43d3-86ff-dbfa3ed944d7")].SubQueries = subQueryDictionary[new Guid("1b3a6dd1-9c71-43d3-86ff-dbfa3ed944d7")]; }
						if (!subQueryDictionary.ContainsKey(new Guid("d97d66d8-be9f-45bb-a1d0-f3525653783c"))) { subQueryDictionary[new Guid("d97d66d8-be9f-45bb-a1d0-f3525653783c")] = new List<InputRecordListQuery>(); }
						subQueryDictionary[new Guid("d97d66d8-be9f-45bb-a1d0-f3525653783c")].Add(queryDictionary[new Guid("1b3a6dd1-9c71-43d3-86ff-dbfa3ed944d7")]);
					}
					{
						queryDictionary[new Guid("93a95605-b57a-4f56-91b4-96253aca3b75")] = new InputRecordListQuery();
						queryDictionary[new Guid("93a95605-b57a-4f56-91b4-96253aca3b75")].FieldName = "status";
						queryDictionary[new Guid("93a95605-b57a-4f56-91b4-96253aca3b75")].FieldValue = "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
						queryDictionary[new Guid("93a95605-b57a-4f56-91b4-96253aca3b75")].QueryType = "EQ";
						queryDictionary[new Guid("93a95605-b57a-4f56-91b4-96253aca3b75")].SubQueries = new List<InputRecordListQuery>();
						if (subQueryDictionary.ContainsKey(new Guid("93a95605-b57a-4f56-91b4-96253aca3b75"))) { queryDictionary[new Guid("93a95605-b57a-4f56-91b4-96253aca3b75")].SubQueries = subQueryDictionary[new Guid("93a95605-b57a-4f56-91b4-96253aca3b75")]; }
						if (!subQueryDictionary.ContainsKey(new Guid("d97d66d8-be9f-45bb-a1d0-f3525653783c"))) { subQueryDictionary[new Guid("d97d66d8-be9f-45bb-a1d0-f3525653783c")] = new List<InputRecordListQuery>(); }
						subQueryDictionary[new Guid("d97d66d8-be9f-45bb-a1d0-f3525653783c")].Add(queryDictionary[new Guid("93a95605-b57a-4f56-91b4-96253aca3b75")]);
					}
					{
						queryDictionary[new Guid("297cd5dd-022f-4e16-8417-258b8fa55828")] = new InputRecordListQuery();
						queryDictionary[new Guid("297cd5dd-022f-4e16-8417-258b8fa55828")].FieldName = "priority";
						queryDictionary[new Guid("297cd5dd-022f-4e16-8417-258b8fa55828")].FieldValue = "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
						queryDictionary[new Guid("297cd5dd-022f-4e16-8417-258b8fa55828")].QueryType = "EQ";
						queryDictionary[new Guid("297cd5dd-022f-4e16-8417-258b8fa55828")].SubQueries = new List<InputRecordListQuery>();
						if (subQueryDictionary.ContainsKey(new Guid("297cd5dd-022f-4e16-8417-258b8fa55828"))) { queryDictionary[new Guid("297cd5dd-022f-4e16-8417-258b8fa55828")].SubQueries = subQueryDictionary[new Guid("297cd5dd-022f-4e16-8417-258b8fa55828")]; }
						if (!subQueryDictionary.ContainsKey(new Guid("d97d66d8-be9f-45bb-a1d0-f3525653783c"))) { subQueryDictionary[new Guid("d97d66d8-be9f-45bb-a1d0-f3525653783c")] = new List<InputRecordListQuery>(); }
						subQueryDictionary[new Guid("d97d66d8-be9f-45bb-a1d0-f3525653783c")].Add(queryDictionary[new Guid("297cd5dd-022f-4e16-8417-258b8fa55828")]);
					}
					if (subQueryDictionary.ContainsKey(new Guid("d97d66d8-be9f-45bb-a1d0-f3525653783c"))) { createListInput.Query.SubQueries = subQueryDictionary[new Guid("d97d66d8-be9f-45bb-a1d0-f3525653783c")]; }
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

			#region << Update  Enity: wv_task name: my_tasks >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "my_tasks").Id;
				createListInput.Type = "General";
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
					var queryDictionary = new Dictionary<Guid, InputRecordListQuery>();
					var subQueryDictionary = new Dictionary<Guid, List<InputRecordListQuery>>();
					//Main query rule
					createListInput.Query.FieldName = null;
					createListInput.Query.FieldValue = null;
					createListInput.Query.QueryType = "AND";
					createListInput.Query.SubQueries = new List<InputRecordListQuery>();
					{
						queryDictionary[new Guid("4e644905-b455-4988-9fe9-9a963821ce33")] = new InputRecordListQuery();
						queryDictionary[new Guid("4e644905-b455-4988-9fe9-9a963821ce33")].FieldName = null;
						queryDictionary[new Guid("4e644905-b455-4988-9fe9-9a963821ce33")].FieldValue = null;
						queryDictionary[new Guid("4e644905-b455-4988-9fe9-9a963821ce33")].QueryType = "AND";
						queryDictionary[new Guid("4e644905-b455-4988-9fe9-9a963821ce33")].SubQueries = new List<InputRecordListQuery>();
						{
							queryDictionary[new Guid("5474b380-67df-4f34-91b9-ac92c8eaf510")] = new InputRecordListQuery();
							queryDictionary[new Guid("5474b380-67df-4f34-91b9-ac92c8eaf510")].FieldName = "owner_id";
							queryDictionary[new Guid("5474b380-67df-4f34-91b9-ac92c8eaf510")].FieldValue = "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
							queryDictionary[new Guid("5474b380-67df-4f34-91b9-ac92c8eaf510")].QueryType = "EQ";
							queryDictionary[new Guid("5474b380-67df-4f34-91b9-ac92c8eaf510")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("5474b380-67df-4f34-91b9-ac92c8eaf510"))) { queryDictionary[new Guid("5474b380-67df-4f34-91b9-ac92c8eaf510")].SubQueries = subQueryDictionary[new Guid("5474b380-67df-4f34-91b9-ac92c8eaf510")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("4e644905-b455-4988-9fe9-9a963821ce33"))) { subQueryDictionary[new Guid("4e644905-b455-4988-9fe9-9a963821ce33")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("4e644905-b455-4988-9fe9-9a963821ce33")].Add(queryDictionary[new Guid("5474b380-67df-4f34-91b9-ac92c8eaf510")]);
						}
						if (subQueryDictionary.ContainsKey(new Guid("4e644905-b455-4988-9fe9-9a963821ce33"))) { queryDictionary[new Guid("4e644905-b455-4988-9fe9-9a963821ce33")].SubQueries = subQueryDictionary[new Guid("4e644905-b455-4988-9fe9-9a963821ce33")]; }
						if (!subQueryDictionary.ContainsKey(new Guid("f2433ee5-768d-4d83-8b75-4deedd5ced92"))) { subQueryDictionary[new Guid("f2433ee5-768d-4d83-8b75-4deedd5ced92")] = new List<InputRecordListQuery>(); }
						subQueryDictionary[new Guid("f2433ee5-768d-4d83-8b75-4deedd5ced92")].Add(queryDictionary[new Guid("4e644905-b455-4988-9fe9-9a963821ce33")]);
					}
					{
						queryDictionary[new Guid("f2d779ad-7878-45f3-ac8a-a190e156b8fc")] = new InputRecordListQuery();
						queryDictionary[new Guid("f2d779ad-7878-45f3-ac8a-a190e156b8fc")].FieldName = null;
						queryDictionary[new Guid("f2d779ad-7878-45f3-ac8a-a190e156b8fc")].FieldValue = null;
						queryDictionary[new Guid("f2d779ad-7878-45f3-ac8a-a190e156b8fc")].QueryType = "AND";
						queryDictionary[new Guid("f2d779ad-7878-45f3-ac8a-a190e156b8fc")].SubQueries = new List<InputRecordListQuery>();
						{
							queryDictionary[new Guid("b917e36d-0042-4ed5-b785-6f757cb5599e")] = new InputRecordListQuery();
							queryDictionary[new Guid("b917e36d-0042-4ed5-b785-6f757cb5599e")].FieldName = "code";
							queryDictionary[new Guid("b917e36d-0042-4ed5-b785-6f757cb5599e")].FieldValue = "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
							queryDictionary[new Guid("b917e36d-0042-4ed5-b785-6f757cb5599e")].QueryType = "CONTAINS";
							queryDictionary[new Guid("b917e36d-0042-4ed5-b785-6f757cb5599e")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("b917e36d-0042-4ed5-b785-6f757cb5599e"))) { queryDictionary[new Guid("b917e36d-0042-4ed5-b785-6f757cb5599e")].SubQueries = subQueryDictionary[new Guid("b917e36d-0042-4ed5-b785-6f757cb5599e")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("f2d779ad-7878-45f3-ac8a-a190e156b8fc"))) { subQueryDictionary[new Guid("f2d779ad-7878-45f3-ac8a-a190e156b8fc")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("f2d779ad-7878-45f3-ac8a-a190e156b8fc")].Add(queryDictionary[new Guid("b917e36d-0042-4ed5-b785-6f757cb5599e")]);
						}
						{
							queryDictionary[new Guid("425d99a5-e8ba-4db1-914e-f83f998bcf7b")] = new InputRecordListQuery();
							queryDictionary[new Guid("425d99a5-e8ba-4db1-914e-f83f998bcf7b")].FieldName = "subject";
							queryDictionary[new Guid("425d99a5-e8ba-4db1-914e-f83f998bcf7b")].FieldValue = "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
							queryDictionary[new Guid("425d99a5-e8ba-4db1-914e-f83f998bcf7b")].QueryType = "CONTAINS";
							queryDictionary[new Guid("425d99a5-e8ba-4db1-914e-f83f998bcf7b")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("425d99a5-e8ba-4db1-914e-f83f998bcf7b"))) { queryDictionary[new Guid("425d99a5-e8ba-4db1-914e-f83f998bcf7b")].SubQueries = subQueryDictionary[new Guid("425d99a5-e8ba-4db1-914e-f83f998bcf7b")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("f2d779ad-7878-45f3-ac8a-a190e156b8fc"))) { subQueryDictionary[new Guid("f2d779ad-7878-45f3-ac8a-a190e156b8fc")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("f2d779ad-7878-45f3-ac8a-a190e156b8fc")].Add(queryDictionary[new Guid("425d99a5-e8ba-4db1-914e-f83f998bcf7b")]);
						}
						{
							queryDictionary[new Guid("0cbbad21-1ad2-4adb-822e-8dcb8eb26b34")] = new InputRecordListQuery();
							queryDictionary[new Guid("0cbbad21-1ad2-4adb-822e-8dcb8eb26b34")].FieldName = "status";
							queryDictionary[new Guid("0cbbad21-1ad2-4adb-822e-8dcb8eb26b34")].FieldValue = "completed";
							queryDictionary[new Guid("0cbbad21-1ad2-4adb-822e-8dcb8eb26b34")].QueryType = "NOT";
							queryDictionary[new Guid("0cbbad21-1ad2-4adb-822e-8dcb8eb26b34")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("0cbbad21-1ad2-4adb-822e-8dcb8eb26b34"))) { queryDictionary[new Guid("0cbbad21-1ad2-4adb-822e-8dcb8eb26b34")].SubQueries = subQueryDictionary[new Guid("0cbbad21-1ad2-4adb-822e-8dcb8eb26b34")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("f2d779ad-7878-45f3-ac8a-a190e156b8fc"))) { subQueryDictionary[new Guid("f2d779ad-7878-45f3-ac8a-a190e156b8fc")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("f2d779ad-7878-45f3-ac8a-a190e156b8fc")].Add(queryDictionary[new Guid("0cbbad21-1ad2-4adb-822e-8dcb8eb26b34")]);
						}
						{
							queryDictionary[new Guid("9f2c586e-bd67-4d2e-98c1-27e3458f6d35")] = new InputRecordListQuery();
							queryDictionary[new Guid("9f2c586e-bd67-4d2e-98c1-27e3458f6d35")].FieldName = "priority";
							queryDictionary[new Guid("9f2c586e-bd67-4d2e-98c1-27e3458f6d35")].FieldValue = "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
							queryDictionary[new Guid("9f2c586e-bd67-4d2e-98c1-27e3458f6d35")].QueryType = "EQ";
							queryDictionary[new Guid("9f2c586e-bd67-4d2e-98c1-27e3458f6d35")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("9f2c586e-bd67-4d2e-98c1-27e3458f6d35"))) { queryDictionary[new Guid("9f2c586e-bd67-4d2e-98c1-27e3458f6d35")].SubQueries = subQueryDictionary[new Guid("9f2c586e-bd67-4d2e-98c1-27e3458f6d35")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("f2d779ad-7878-45f3-ac8a-a190e156b8fc"))) { subQueryDictionary[new Guid("f2d779ad-7878-45f3-ac8a-a190e156b8fc")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("f2d779ad-7878-45f3-ac8a-a190e156b8fc")].Add(queryDictionary[new Guid("9f2c586e-bd67-4d2e-98c1-27e3458f6d35")]);
						}
						if (subQueryDictionary.ContainsKey(new Guid("f2d779ad-7878-45f3-ac8a-a190e156b8fc"))) { queryDictionary[new Guid("f2d779ad-7878-45f3-ac8a-a190e156b8fc")].SubQueries = subQueryDictionary[new Guid("f2d779ad-7878-45f3-ac8a-a190e156b8fc")]; }
						if (!subQueryDictionary.ContainsKey(new Guid("f2433ee5-768d-4d83-8b75-4deedd5ced92"))) { subQueryDictionary[new Guid("f2433ee5-768d-4d83-8b75-4deedd5ced92")] = new List<InputRecordListQuery>(); }
						subQueryDictionary[new Guid("f2433ee5-768d-4d83-8b75-4deedd5ced92")].Add(queryDictionary[new Guid("f2d779ad-7878-45f3-ac8a-a190e156b8fc")]);
					}
					if (subQueryDictionary.ContainsKey(new Guid("f2433ee5-768d-4d83-8b75-4deedd5ced92"))) { createListInput.Query.SubQueries = subQueryDictionary[new Guid("f2433ee5-768d-4d83-8b75-4deedd5ced92")]; }
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

			#region << Update  Enity: wv_project View: admin_details >>
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

			#region << Update  Enity: wv_project View: create >>
			{
				var updateViewEntity = entMan.ReadEntity(new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "create").Id;
				createViewInput.Type = "Create";
				createViewInput.Name = "create";
				createViewInput.Label = "Create";
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
					var response = entMan.UpdateRecordView(new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_project Updated view: create Message:" + response.Message);
				}
			}
			#endregion

			#region << Update  Enity: wv_project View: dashboard >>
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
					viewItemFromRelation.ListName = "project_tasks";
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
					viewItemFromRelation.ListName = "project_milestones";
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
					viewItemFromRelation.ListName = "project_bugs";
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

			#region << Update  Enity: wv_project View: general >>
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

			#region << View  Enity: wv_project name: quick_create >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("cd6d4f57-7940-47f8-8abd-ca3614a7aa2e");
				createViewInput.Type = "Quick_Create";
				createViewInput.Name = "quick_create";
				createViewInput.Label = "Quick create";
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
					var response = entMan.CreateRecordView(new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_project Updated view: quick_create Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: wv_project name: quick_view >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("8d568f84-3600-4155-8a24-8daf20a270ef");
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
					var response = entMan.CreateRecordView(new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_project Updated view: quick_view Message:" + response.Message);
				}
			}
			#endregion

			#region << Update  Enity: wv_project name: admin >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "admin").Id;
				createListInput.Type = "Hidden";
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

			#region << View  Enity: role name: create >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("c4541fee-fbb6-4661-929e-1724adec285a")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("0fe76c7a-7e7e-4f3f-ab17-8a1df951901e");
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
					var response = entMan.CreateRecordView(new Guid("c4541fee-fbb6-4661-929e-1724adec285a"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: role Updated view: create Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: role name: quick_create >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("c4541fee-fbb6-4661-929e-1724adec285a")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("2fe14937-8c2e-4d81-b5bf-9a39704f1e15");
				createViewInput.Type = "Quick_Create";
				createViewInput.Name = "quick_create";
				createViewInput.Label = "Quick create";
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
					var response = entMan.CreateRecordView(new Guid("c4541fee-fbb6-4661-929e-1724adec285a"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: role Updated view: quick_create Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: role name: quick_view >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("c4541fee-fbb6-4661-929e-1724adec285a")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("7f2778ac-a8eb-48e3-aeff-e27af018ac4f");
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
					var response = entMan.CreateRecordView(new Guid("c4541fee-fbb6-4661-929e-1724adec285a"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: role Updated view: quick_view Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: role name: general >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("c4541fee-fbb6-4661-929e-1724adec285a")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("d990148c-7a64-4a89-845d-2f09c6ef823a");
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
					var response = entMan.CreateRecordView(new Guid("c4541fee-fbb6-4661-929e-1724adec285a"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: role Updated view: general Message:" + response.Message);
				}
			}
			#endregion

			#region << List  Enity: role name: general >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("c4541fee-fbb6-4661-929e-1724adec285a")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = new Guid("9a3c30a6-ef9f-4e82-83c2-539d5be5f6a4");
				createListInput.Type = "General";
				createListInput.Name = "general";
				createListInput.Label = "General";
				createListInput.Title = "";
				createListInput.Weight = Decimal.Parse("10.0");
				createListInput.Default = true;
				createListInput.System = false;
				createListInput.CssClass = null;
				createListInput.IconName = string.IsNullOrEmpty("list") ? string.Empty : "list";
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
					var response = entMan.CreateRecordList(new Guid("c4541fee-fbb6-4661-929e-1724adec285a"), createListInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: role Created list: general Message:" + response.Message);
				}
			}
			#endregion

			#region << List  Enity: role name: lookup >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("c4541fee-fbb6-4661-929e-1724adec285a")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = new Guid("82bd4903-ef38-42e0-89bd-f4e84583d3c9");
				createListInput.Type = "Lookup";
				createListInput.Name = "lookup";
				createListInput.Label = "Lookup";
				createListInput.Title = "";
				createListInput.Weight = Decimal.Parse("10.0");
				createListInput.Default = true;
				createListInput.System = false;
				createListInput.CssClass = null;
				createListInput.IconName = string.IsNullOrEmpty("list") ? string.Empty : "list";
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
					var response = entMan.CreateRecordList(new Guid("c4541fee-fbb6-4661-929e-1724adec285a"), createListInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: role Created list: lookup Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: user name: create >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("3e0d1e18-41fe-4e32-bfdf-97c283b6d25a");
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
					var response = entMan.CreateRecordView(new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: user Updated view: create Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: user name: quick_create >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("15afe595-86b6-4d6d-9d61-087325fb5200");
				createViewInput.Type = "Quick_Create";
				createViewInput.Name = "quick_create";
				createViewInput.Label = "Quick create";
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
					var response = entMan.CreateRecordView(new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: user Updated view: quick_create Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: user name: quick_view >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("dfaa5588-9219-4717-9824-dafccb5a88fb");
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
					var response = entMan.CreateRecordView(new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: user Updated view: quick_view Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: user name: general >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("61dc420a-20d1-44a8-9ee8-76cbd9a35808");
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
					var response = entMan.CreateRecordView(new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: user Updated view: general Message:" + response.Message);
				}
			}
			#endregion

			#region << List  Enity: user name: general >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = new Guid("441a4578-2794-42d5-bb89-23d98abb6eaa");
				createListInput.Type = "General";
				createListInput.Name = "general";
				createListInput.Label = "General";
				createListInput.Title = "";
				createListInput.Weight = Decimal.Parse("10.0");
				createListInput.Default = true;
				createListInput.System = false;
				createListInput.CssClass = null;
				createListInput.IconName = string.IsNullOrEmpty("list") ? string.Empty : "list";
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
					var response = entMan.CreateRecordList(new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b"), createListInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: user Created list: general Message:" + response.Message);
				}
			}
			#endregion

			#region << List  Enity: user name: lookup >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = new Guid("6440ed7f-8543-4854-862b-520fccaf4d68");
				createListInput.Type = "Lookup";
				createListInput.Name = "lookup";
				createListInput.Label = "Lookup";
				createListInput.Title = "";
				createListInput.Weight = Decimal.Parse("10.0");
				createListInput.Default = true;
				createListInput.System = false;
				createListInput.CssClass = null;
				createListInput.IconName = string.IsNullOrEmpty("list") ? string.Empty : "list";
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
					var queryDictionary = new Dictionary<Guid, InputRecordListQuery>();
					var subQueryDictionary = new Dictionary<Guid, List<InputRecordListQuery>>();
					//Main query rule
					createListInput.Query.FieldName = null;
					createListInput.Query.FieldValue = null;
					createListInput.Query.QueryType = "AND";
					createListInput.Query.SubQueries = new List<InputRecordListQuery>();
					{
						queryDictionary[new Guid("395c11ee-3711-4c00-b9cd-1be5fd70a39a")] = new InputRecordListQuery();
						queryDictionary[new Guid("395c11ee-3711-4c00-b9cd-1be5fd70a39a")].FieldName = "username";
						queryDictionary[new Guid("395c11ee-3711-4c00-b9cd-1be5fd70a39a")].FieldValue = "{\"name\":\"url_query\", \"option\": \"username\", \"default\": null, \"settings\":{}}";
						queryDictionary[new Guid("395c11ee-3711-4c00-b9cd-1be5fd70a39a")].QueryType = "CONTAINS";
						queryDictionary[new Guid("395c11ee-3711-4c00-b9cd-1be5fd70a39a")].SubQueries = new List<InputRecordListQuery>();
						if (subQueryDictionary.ContainsKey(new Guid("395c11ee-3711-4c00-b9cd-1be5fd70a39a"))) { queryDictionary[new Guid("395c11ee-3711-4c00-b9cd-1be5fd70a39a")].SubQueries = subQueryDictionary[new Guid("395c11ee-3711-4c00-b9cd-1be5fd70a39a")]; }
						if (!subQueryDictionary.ContainsKey(new Guid("9bcd88e6-6a4a-498b-ac48-306bf6366b7e"))) { subQueryDictionary[new Guid("9bcd88e6-6a4a-498b-ac48-306bf6366b7e")] = new List<InputRecordListQuery>(); }
						subQueryDictionary[new Guid("9bcd88e6-6a4a-498b-ac48-306bf6366b7e")].Add(queryDictionary[new Guid("395c11ee-3711-4c00-b9cd-1be5fd70a39a")]);
					}
					{
						queryDictionary[new Guid("15b73ded-1319-4d99-a088-13c95738e8a6")] = new InputRecordListQuery();
						queryDictionary[new Guid("15b73ded-1319-4d99-a088-13c95738e8a6")].FieldName = "email";
						queryDictionary[new Guid("15b73ded-1319-4d99-a088-13c95738e8a6")].FieldValue = "{\"name\":\"url_query\", \"option\": \"email\", \"default\": null, \"settings\":{}}";
						queryDictionary[new Guid("15b73ded-1319-4d99-a088-13c95738e8a6")].QueryType = "CONTAINS";
						queryDictionary[new Guid("15b73ded-1319-4d99-a088-13c95738e8a6")].SubQueries = new List<InputRecordListQuery>();
						if (subQueryDictionary.ContainsKey(new Guid("15b73ded-1319-4d99-a088-13c95738e8a6"))) { queryDictionary[new Guid("15b73ded-1319-4d99-a088-13c95738e8a6")].SubQueries = subQueryDictionary[new Guid("15b73ded-1319-4d99-a088-13c95738e8a6")]; }
						if (!subQueryDictionary.ContainsKey(new Guid("9bcd88e6-6a4a-498b-ac48-306bf6366b7e"))) { subQueryDictionary[new Guid("9bcd88e6-6a4a-498b-ac48-306bf6366b7e")] = new List<InputRecordListQuery>(); }
						subQueryDictionary[new Guid("9bcd88e6-6a4a-498b-ac48-306bf6366b7e")].Add(queryDictionary[new Guid("15b73ded-1319-4d99-a088-13c95738e8a6")]);
					}
					if (subQueryDictionary.ContainsKey(new Guid("9bcd88e6-6a4a-498b-ac48-306bf6366b7e"))) { createListInput.Query.SubQueries = subQueryDictionary[new Guid("9bcd88e6-6a4a-498b-ac48-306bf6366b7e")]; }
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
					var response = entMan.CreateRecordList(new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b"), createListInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: user Created list: lookup Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: wv_milestone name: quick_create >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("d691b634-016c-46ef-8ba8-8c3328797497")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("ff807d26-7bc3-48f3-a29b-333fb1c3db1c");
				createViewInput.Type = "Quick_Create";
				createViewInput.Name = "quick_create";
				createViewInput.Label = "Quick create";
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
					var response = entMan.CreateRecordView(new Guid("d691b634-016c-46ef-8ba8-8c3328797497"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_milestone Updated view: quick_create Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: wv_milestone name: quick_view >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("d691b634-016c-46ef-8ba8-8c3328797497")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("25cb0a1b-8413-457b-b21c-31909b673c9d");
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
					var response = entMan.CreateRecordView(new Guid("d691b634-016c-46ef-8ba8-8c3328797497"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_milestone Updated view: quick_view Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: wv_milestone name: general >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("d691b634-016c-46ef-8ba8-8c3328797497")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("92bf4f9c-da49-4760-956a-992b48fbe3fd");
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
					var response = entMan.CreateRecordView(new Guid("d691b634-016c-46ef-8ba8-8c3328797497"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_milestone Updated view: general Message:" + response.Message);
				}
			}
			#endregion

			#region << Update  Enity: wv_milestone View: create >>
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

			#region << List  Enity: wv_milestone name: lookup >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("d691b634-016c-46ef-8ba8-8c3328797497")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = new Guid("ba94f43b-79ef-4242-a2df-f9801bcacb9a");
				createListInput.Type = "Lookup";
				createListInput.Name = "lookup";
				createListInput.Label = "Lookup";
				createListInput.Title = "";
				createListInput.Weight = Decimal.Parse("10.0");
				createListInput.Default = true;
				createListInput.System = false;
				createListInput.CssClass = null;
				createListInput.IconName = string.IsNullOrEmpty("list") ? string.Empty : "list";
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
					var response = entMan.CreateRecordList(new Guid("d691b634-016c-46ef-8ba8-8c3328797497"), createListInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_milestone Created list: lookup Message:" + response.Message);
				}
			}
			#endregion

			#region << Update  Enity: wv_milestone name: project_milestones >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("d691b634-016c-46ef-8ba8-8c3328797497")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "project_milestones").Id;
				createListInput.Type = "General";
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

			#region << View  Enity: wv_project_attachment name: quick_create >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("b373a9ab-9c2e-43bc-8ac9-5929e90380c5");
				createViewInput.Type = "Quick_Create";
				createViewInput.Name = "quick_create";
				createViewInput.Label = "Quick create";
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
					var response = entMan.CreateRecordView(new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_project_attachment Updated view: quick_create Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: wv_project_attachment name: quick_view >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("33a245f4-d9f3-402a-a368-87eac4153fe1");
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
					var response = entMan.CreateRecordView(new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_project_attachment Updated view: quick_view Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: wv_project_attachment name: general >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("39f4a027-62a9-462e-a66a-8f773886cc35");
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
					var response = entMan.CreateRecordView(new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_project_attachment Updated view: general Message:" + response.Message);
				}
			}
			#endregion

			#region << Update  Enity: wv_project_attachment View: create >>
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

			#region << List  Enity: wv_project_attachment name: lookup >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = new Guid("3a7b194e-ea07-4d4f-a09e-00475d089ada");
				createListInput.Type = "Lookup";
				createListInput.Name = "lookup";
				createListInput.Label = "Lookup";
				createListInput.Title = null;
				createListInput.Weight = Decimal.Parse("10.0");
				createListInput.Default = true;
				createListInput.System = false;
				createListInput.CssClass = null;
				createListInput.IconName = string.IsNullOrEmpty("list") ? string.Empty : "list";
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
					var response = entMan.CreateRecordList(new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73"), createListInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_project_attachment Created list: lookup Message:" + response.Message);
				}
			}
			#endregion

			#region << Update  Enity: wv_project_attachment name: task_attachments >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "task_attachments").Id;
				createListInput.Type = "Hidden";
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

			#region << Update  Enity: wv_timelog field: description >>
			{
				var currentEntity = entMan.ReadEntity(new Guid("e2db7515-721f-446e-8333-6149b1ba131b")).Object;
				InputMultiLineTextField textareaField = new InputMultiLineTextField();
				textareaField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "description").Id;
				textareaField.Name = "description";
				textareaField.Label = "Description";
				textareaField.PlaceholderText = string.IsNullOrEmpty("") ? string.Empty : "";
				textareaField.Description = string.IsNullOrEmpty("") ? string.Empty : "";
				textareaField.HelpText = string.IsNullOrEmpty("") ? string.Empty : "";
				textareaField.Required = false;
				textareaField.Unique = false;
				textareaField.Searchable = false;
				textareaField.Auditable = false;
				textareaField.System = true;
				textareaField.DefaultValue = string.IsNullOrEmpty("") ? string.Empty : "";
				textareaField.MaxLength = string.IsNullOrEmpty("") ? (int?)null : Int32.Parse("");
				textareaField.VisibleLineNumber = string.IsNullOrEmpty("") ? (int?)null : Int32.Parse("");
				textareaField.EnableSecurity = true;
				textareaField.Permissions = new FieldPermissions();
				textareaField.Permissions.CanRead = new List<Guid>();
				textareaField.Permissions.CanUpdate = new List<Guid>();
				//READ
				textareaField.Permissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
				textareaField.Permissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
				//UPDATE
				textareaField.Permissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
				textareaField.Permissions.CanUpdate.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
				{
					var response = entMan.UpdateField(new Guid("e2db7515-721f-446e-8333-6149b1ba131b"), textareaField);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_timelog Field: description Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: wv_timelog name: quick_create >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("e2db7515-721f-446e-8333-6149b1ba131b")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("a30d319d-42d0-44b7-b4ec-8c5883460e07");
				createViewInput.Type = "Quick_Create";
				createViewInput.Name = "quick_create";
				createViewInput.Label = "Quick create";
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
					var response = entMan.CreateRecordView(new Guid("e2db7515-721f-446e-8333-6149b1ba131b"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_timelog Updated view: quick_create Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: wv_timelog name: quick_view >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("e2db7515-721f-446e-8333-6149b1ba131b")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("b9cfc363-9e08-4c13-9b6d-e5352f958f31");
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
					var response = entMan.CreateRecordView(new Guid("e2db7515-721f-446e-8333-6149b1ba131b"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_timelog Updated view: quick_view Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: wv_timelog name: general >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("e2db7515-721f-446e-8333-6149b1ba131b")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("19dba00e-20d7-40a1-b784-c8852a076498");
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
					var response = entMan.CreateRecordView(new Guid("e2db7515-721f-446e-8333-6149b1ba131b"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_timelog Updated view: general Message:" + response.Message);
				}
			}
			#endregion

			#region << Update  Enity: wv_timelog View: create >>
			{
				var updateViewEntity = entMan.ReadEntity(new Guid("e2db7515-721f-446e-8333-6149b1ba131b")).Object;
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
						viewSection.Id = new Guid("559e8b4a-2ae5-4be7-8a87-805faeafeac4");
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
							viewRow.Id = new Guid("6c769b09-045b-4826-a791-83812f6ded1b");
							viewRow.Weight = Decimal.Parse("1.0");
							viewRow.Columns = new List<InputRecordViewColumn>();

							#region << Column 1 >>
							{
								var viewColumn = new InputRecordViewColumn();
								viewColumn.GridColCount = Int32.Parse("6");
								viewColumn.Items = new List<InputRecordViewItemBase>();

								#region << hours >>
								{
									var viewItem = new InputRecordViewFieldItem();
									viewItem.EntityId = new Guid("e2db7515-721f-446e-8333-6149b1ba131b");
									viewItem.EntityName = "wv_timelog";
									viewItem.FieldId = new Guid("41caeb03-7430-4eb8-b830-c9df8bf2dc7f");
									viewItem.FieldName = "hours";
									viewItem.Type = "field";
									viewColumn.Items.Add(viewItem);
								}
								#endregion
								#region << log_date >>
								{
									var viewItem = new InputRecordViewFieldItem();
									viewItem.EntityId = new Guid("e2db7515-721f-446e-8333-6149b1ba131b");
									viewItem.EntityName = "wv_timelog";
									viewItem.FieldId = new Guid("29a32ad7-7b1c-4ea0-a06b-57b30be9b107");
									viewItem.FieldName = "log_date";
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

								#region << billable >>
								{
									var viewItem = new InputRecordViewFieldItem();
									viewItem.EntityId = new Guid("e2db7515-721f-446e-8333-6149b1ba131b");
									viewItem.EntityName = "wv_timelog";
									viewItem.FieldId = new Guid("1f4b0729-4e31-4722-a8ce-3bf90c471dad");
									viewItem.FieldName = "billable";
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
							viewRow.Id = new Guid("57e71de9-8cb4-4343-bcec-5a0152f08dbd");
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
									viewItem.EntityId = new Guid("e2db7515-721f-446e-8333-6149b1ba131b");
									viewItem.EntityName = "wv_timelog";
									viewItem.FieldId = new Guid("1a1b646e-93df-4035-ace0-d844f62bad63");
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
						throw new Exception("System error 10060. Entity: wv_timelog Updated view: create Message:" + response.Message);
				}
			}
			#endregion

			#region << List  Enity: wv_timelog name: general >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("e2db7515-721f-446e-8333-6149b1ba131b")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = new Guid("5c6f5737-a0e2-4dc4-bb7d-746a2a6477b9");
				createListInput.Type = "General";
				createListInput.Name = "general";
				createListInput.Label = "General";
				createListInput.Title = null;
				createListInput.Weight = Decimal.Parse("10.0");
				createListInput.Default = true;
				createListInput.System = false;
				createListInput.CssClass = null;
				createListInput.IconName = string.IsNullOrEmpty("list") ? string.Empty : "list";
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
					var response = entMan.CreateRecordList(new Guid("e2db7515-721f-446e-8333-6149b1ba131b"), createListInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_timelog Created list: general Message:" + response.Message);
				}
			}
			#endregion

			#region << List  Enity: wv_timelog name: lookup >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("e2db7515-721f-446e-8333-6149b1ba131b")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = new Guid("e7a5cd83-84de-482f-bb2f-bdcb65ccb61a");
				createListInput.Type = "Lookup";
				createListInput.Name = "lookup";
				createListInput.Label = "Lookup";
				createListInput.Title = null;
				createListInput.Weight = Decimal.Parse("10.0");
				createListInput.Default = true;
				createListInput.System = false;
				createListInput.CssClass = null;
				createListInput.IconName = string.IsNullOrEmpty("list") ? string.Empty : "list";
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
					var response = entMan.CreateRecordList(new Guid("e2db7515-721f-446e-8333-6149b1ba131b"), createListInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_timelog Created list: lookup Message:" + response.Message);
				}
			}
			#endregion

			#region << Update  Enity: wv_timelog name: bug_timelogs >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("e2db7515-721f-446e-8333-6149b1ba131b")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "bug_timelogs").Id;
				createListInput.Type = "Hidden";
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

			#region << Update  Enity: wv_timelog name: task_timelogs >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("e2db7515-721f-446e-8333-6149b1ba131b")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "task_timelogs").Id;
				createListInput.Type = "Hidden";
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

			#region << View  Enity: wv_bug name: quick_create >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("a9951a8e-bb85-4581-85ea-1364a4d6ad66");
				createViewInput.Type = "Quick_Create";
				createViewInput.Name = "quick_create";
				createViewInput.Label = "Quick create";
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
					var response = entMan.CreateRecordView(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_bug Updated view: quick_create Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: wv_bug name: quick_view >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("9565f541-177e-469e-a3ca-d14d096520f3");
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
					var response = entMan.CreateRecordView(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_bug Updated view: quick_view Message:" + response.Message);
				}
			}
			#endregion

			#region << Update  Enity: wv_bug View: create >>
			{
				var updateViewEntity = entMan.ReadEntity(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c")).Object;
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
						viewSection.Id = new Guid("6781b136-b982-4934-a98c-5f736bd1a771");
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
							viewRow.Id = new Guid("c0e3757b-6817-4eda-8cd1-e95b603af049");
							viewRow.Weight = Decimal.Parse("1.0");
							viewRow.Columns = new List<InputRecordViewColumn>();

							#region << Column 1 >>
							{
								var viewColumn = new InputRecordViewColumn();
								viewColumn.GridColCount = Int32.Parse("12");
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
							viewRow.Id = new Guid("61b3052c-339d-4ad2-8a5f-59f215f07358");
							viewRow.Weight = Decimal.Parse("2.0");
							viewRow.Columns = new List<InputRecordViewColumn>();

							#region << Column 1 >>
							{
								var viewColumn = new InputRecordViewColumn();
								viewColumn.GridColCount = Int32.Parse("6");
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
								//Save column
								viewRow.Columns.Add(viewColumn);
							}
							#endregion
							#region << Column 2 >>
							{
								var viewColumn = new InputRecordViewColumn();
								viewColumn.GridColCount = Int32.Parse("6");
								viewColumn.Items = new List<InputRecordViewItemBase>();

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
					var response = entMan.UpdateRecordView(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_bug Updated view: create Message:" + response.Message);
				}
			}
			#endregion

			#region << Update  Enity: wv_bug View: general >>
			{
				var updateViewEntity = entMan.ReadEntity(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "general").Id;
				createViewInput.Type = "General";
				createViewInput.Name = "general";
				createViewInput.Label = "Details";
				createViewInput.Title = "[{code}] {subject}";
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
								//Save column
								viewRow.Columns.Add(viewColumn);
							}
							#endregion
							#region << Column 2 >>
							{
								var viewColumn = new InputRecordViewColumn();
								viewColumn.GridColCount = Int32.Parse("4");
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
					viewItemFromRelation.ListName = "bug_attachments";
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

				#region << list from relation: bug_comments >>
				{
					var viewItemFromRelation = new InputRecordViewSidebarRelationListItem();
					viewItemFromRelation.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
					viewItemFromRelation.EntityName = "wv_bug";
					viewItemFromRelation.ListId = new Guid("b143b82f-b79f-47c1-87e7-ecba6f6f2a32");
					viewItemFromRelation.ListName = "bug_comments";
					viewItemFromRelation.FieldLabel = "Comments";
					viewItemFromRelation.FieldPlaceholder = "";
					viewItemFromRelation.FieldHelpText = "";
					viewItemFromRelation.FieldRequired = false;
					viewItemFromRelation.FieldManageView = "general";
					viewItemFromRelation.FieldLookupList = "lookup";
					viewItemFromRelation.RelationId = new Guid("5af026bd-d046-42ba-b6a0-e9090727348f");
					viewItemFromRelation.RelationName = "bug_1_n_comment";
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
					viewItemFromRelation.ListName = "bug_timelogs";
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
					viewItemFromRelation.ListName = "bug_activities";
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

			#region << List  Enity: wv_bug name: lookup >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = new Guid("6e20244a-ee61-404c-af80-2cb2498b54bd");
				createListInput.Type = "Lookup";
				createListInput.Name = "lookup";
				createListInput.Label = "Lookup";
				createListInput.Title = "";
				createListInput.Weight = Decimal.Parse("10.0");
				createListInput.Default = true;
				createListInput.System = false;
				createListInput.CssClass = null;
				createListInput.IconName = string.IsNullOrEmpty("list") ? string.Empty : "list";
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
					var response = entMan.CreateRecordList(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c"), createListInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_bug Created list: lookup Message:" + response.Message);
				}
			}
			#endregion

			#region << Update  Enity: wv_bug name: project_bugs >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "project_bugs").Id;
				createListInput.Type = "Hidden";
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
					var queryDictionary = new Dictionary<Guid, InputRecordListQuery>();
					var subQueryDictionary = new Dictionary<Guid, List<InputRecordListQuery>>();
					//Main query rule
					createListInput.Query.FieldName = null;
					createListInput.Query.FieldValue = null;
					createListInput.Query.QueryType = "AND";
					createListInput.Query.SubQueries = new List<InputRecordListQuery>();
					{
						queryDictionary[new Guid("71080102-9187-4a00-a7cf-f4c06358b37c")] = new InputRecordListQuery();
						queryDictionary[new Guid("71080102-9187-4a00-a7cf-f4c06358b37c")].FieldName = "subject";
						queryDictionary[new Guid("71080102-9187-4a00-a7cf-f4c06358b37c")].FieldValue = "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
						queryDictionary[new Guid("71080102-9187-4a00-a7cf-f4c06358b37c")].QueryType = "CONTAINS";
						queryDictionary[new Guid("71080102-9187-4a00-a7cf-f4c06358b37c")].SubQueries = new List<InputRecordListQuery>();
						if (subQueryDictionary.ContainsKey(new Guid("71080102-9187-4a00-a7cf-f4c06358b37c"))) { queryDictionary[new Guid("71080102-9187-4a00-a7cf-f4c06358b37c")].SubQueries = subQueryDictionary[new Guid("71080102-9187-4a00-a7cf-f4c06358b37c")]; }
						if (!subQueryDictionary.ContainsKey(new Guid("12f2b249-f609-4e21-a9da-d894f6cbe88c"))) { subQueryDictionary[new Guid("12f2b249-f609-4e21-a9da-d894f6cbe88c")] = new List<InputRecordListQuery>(); }
						subQueryDictionary[new Guid("12f2b249-f609-4e21-a9da-d894f6cbe88c")].Add(queryDictionary[new Guid("71080102-9187-4a00-a7cf-f4c06358b37c")]);
					}
					{
						queryDictionary[new Guid("bad7e9e9-5314-4c49-ab85-59123ea11695")] = new InputRecordListQuery();
						queryDictionary[new Guid("bad7e9e9-5314-4c49-ab85-59123ea11695")].FieldName = "status";
						queryDictionary[new Guid("bad7e9e9-5314-4c49-ab85-59123ea11695")].FieldValue = "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
						queryDictionary[new Guid("bad7e9e9-5314-4c49-ab85-59123ea11695")].QueryType = "EQ";
						queryDictionary[new Guid("bad7e9e9-5314-4c49-ab85-59123ea11695")].SubQueries = new List<InputRecordListQuery>();
						if (subQueryDictionary.ContainsKey(new Guid("bad7e9e9-5314-4c49-ab85-59123ea11695"))) { queryDictionary[new Guid("bad7e9e9-5314-4c49-ab85-59123ea11695")].SubQueries = subQueryDictionary[new Guid("bad7e9e9-5314-4c49-ab85-59123ea11695")]; }
						if (!subQueryDictionary.ContainsKey(new Guid("12f2b249-f609-4e21-a9da-d894f6cbe88c"))) { subQueryDictionary[new Guid("12f2b249-f609-4e21-a9da-d894f6cbe88c")] = new List<InputRecordListQuery>(); }
						subQueryDictionary[new Guid("12f2b249-f609-4e21-a9da-d894f6cbe88c")].Add(queryDictionary[new Guid("bad7e9e9-5314-4c49-ab85-59123ea11695")]);
					}
					{
						queryDictionary[new Guid("66a31e85-464a-4bc5-8b5a-209945ce40ae")] = new InputRecordListQuery();
						queryDictionary[new Guid("66a31e85-464a-4bc5-8b5a-209945ce40ae")].FieldName = "priority";
						queryDictionary[new Guid("66a31e85-464a-4bc5-8b5a-209945ce40ae")].FieldValue = "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
						queryDictionary[new Guid("66a31e85-464a-4bc5-8b5a-209945ce40ae")].QueryType = "EQ";
						queryDictionary[new Guid("66a31e85-464a-4bc5-8b5a-209945ce40ae")].SubQueries = new List<InputRecordListQuery>();
						if (subQueryDictionary.ContainsKey(new Guid("66a31e85-464a-4bc5-8b5a-209945ce40ae"))) { queryDictionary[new Guid("66a31e85-464a-4bc5-8b5a-209945ce40ae")].SubQueries = subQueryDictionary[new Guid("66a31e85-464a-4bc5-8b5a-209945ce40ae")]; }
						if (!subQueryDictionary.ContainsKey(new Guid("12f2b249-f609-4e21-a9da-d894f6cbe88c"))) { subQueryDictionary[new Guid("12f2b249-f609-4e21-a9da-d894f6cbe88c")] = new List<InputRecordListQuery>(); }
						subQueryDictionary[new Guid("12f2b249-f609-4e21-a9da-d894f6cbe88c")].Add(queryDictionary[new Guid("66a31e85-464a-4bc5-8b5a-209945ce40ae")]);
					}
					if (subQueryDictionary.ContainsKey(new Guid("12f2b249-f609-4e21-a9da-d894f6cbe88c"))) { createListInput.Query.SubQueries = subQueryDictionary[new Guid("12f2b249-f609-4e21-a9da-d894f6cbe88c")]; }
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

			#region << Update  Enity: wv_bug name: created_bugs >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "created_bugs").Id;
				createListInput.Type = "General";
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
					var queryDictionary = new Dictionary<Guid, InputRecordListQuery>();
					var subQueryDictionary = new Dictionary<Guid, List<InputRecordListQuery>>();
					//Main query rule
					createListInput.Query.FieldName = null;
					createListInput.Query.FieldValue = null;
					createListInput.Query.QueryType = "AND";
					createListInput.Query.SubQueries = new List<InputRecordListQuery>();
					{
						queryDictionary[new Guid("2fedf88a-7309-4e97-a66b-c0682ba0b807")] = new InputRecordListQuery();
						queryDictionary[new Guid("2fedf88a-7309-4e97-a66b-c0682ba0b807")].FieldName = null;
						queryDictionary[new Guid("2fedf88a-7309-4e97-a66b-c0682ba0b807")].FieldValue = null;
						queryDictionary[new Guid("2fedf88a-7309-4e97-a66b-c0682ba0b807")].QueryType = "AND";
						queryDictionary[new Guid("2fedf88a-7309-4e97-a66b-c0682ba0b807")].SubQueries = new List<InputRecordListQuery>();
						{
							queryDictionary[new Guid("19f5bf29-fa48-4ec9-a04a-00c44b773caf")] = new InputRecordListQuery();
							queryDictionary[new Guid("19f5bf29-fa48-4ec9-a04a-00c44b773caf")].FieldName = "created_by";
							queryDictionary[new Guid("19f5bf29-fa48-4ec9-a04a-00c44b773caf")].FieldValue = "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
							queryDictionary[new Guid("19f5bf29-fa48-4ec9-a04a-00c44b773caf")].QueryType = "EQ";
							queryDictionary[new Guid("19f5bf29-fa48-4ec9-a04a-00c44b773caf")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("19f5bf29-fa48-4ec9-a04a-00c44b773caf"))) { queryDictionary[new Guid("19f5bf29-fa48-4ec9-a04a-00c44b773caf")].SubQueries = subQueryDictionary[new Guid("19f5bf29-fa48-4ec9-a04a-00c44b773caf")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("2fedf88a-7309-4e97-a66b-c0682ba0b807"))) { subQueryDictionary[new Guid("2fedf88a-7309-4e97-a66b-c0682ba0b807")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("2fedf88a-7309-4e97-a66b-c0682ba0b807")].Add(queryDictionary[new Guid("19f5bf29-fa48-4ec9-a04a-00c44b773caf")]);
						}
						if (subQueryDictionary.ContainsKey(new Guid("2fedf88a-7309-4e97-a66b-c0682ba0b807"))) { queryDictionary[new Guid("2fedf88a-7309-4e97-a66b-c0682ba0b807")].SubQueries = subQueryDictionary[new Guid("2fedf88a-7309-4e97-a66b-c0682ba0b807")]; }
						if (!subQueryDictionary.ContainsKey(new Guid("a6643b4d-654d-4e40-9b46-c3c7700fdb83"))) { subQueryDictionary[new Guid("a6643b4d-654d-4e40-9b46-c3c7700fdb83")] = new List<InputRecordListQuery>(); }
						subQueryDictionary[new Guid("a6643b4d-654d-4e40-9b46-c3c7700fdb83")].Add(queryDictionary[new Guid("2fedf88a-7309-4e97-a66b-c0682ba0b807")]);
					}
					{
						queryDictionary[new Guid("9fe42513-4c88-4222-bfb0-437086df9e6c")] = new InputRecordListQuery();
						queryDictionary[new Guid("9fe42513-4c88-4222-bfb0-437086df9e6c")].FieldName = null;
						queryDictionary[new Guid("9fe42513-4c88-4222-bfb0-437086df9e6c")].FieldValue = null;
						queryDictionary[new Guid("9fe42513-4c88-4222-bfb0-437086df9e6c")].QueryType = "AND";
						queryDictionary[new Guid("9fe42513-4c88-4222-bfb0-437086df9e6c")].SubQueries = new List<InputRecordListQuery>();
						{
							queryDictionary[new Guid("05ad3e43-7d5f-49e2-8906-4d0dace4e572")] = new InputRecordListQuery();
							queryDictionary[new Guid("05ad3e43-7d5f-49e2-8906-4d0dace4e572")].FieldName = "code";
							queryDictionary[new Guid("05ad3e43-7d5f-49e2-8906-4d0dace4e572")].FieldValue = "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
							queryDictionary[new Guid("05ad3e43-7d5f-49e2-8906-4d0dace4e572")].QueryType = "CONTAINS";
							queryDictionary[new Guid("05ad3e43-7d5f-49e2-8906-4d0dace4e572")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("05ad3e43-7d5f-49e2-8906-4d0dace4e572"))) { queryDictionary[new Guid("05ad3e43-7d5f-49e2-8906-4d0dace4e572")].SubQueries = subQueryDictionary[new Guid("05ad3e43-7d5f-49e2-8906-4d0dace4e572")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("9fe42513-4c88-4222-bfb0-437086df9e6c"))) { subQueryDictionary[new Guid("9fe42513-4c88-4222-bfb0-437086df9e6c")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("9fe42513-4c88-4222-bfb0-437086df9e6c")].Add(queryDictionary[new Guid("05ad3e43-7d5f-49e2-8906-4d0dace4e572")]);
						}
						{
							queryDictionary[new Guid("abea33cb-e34a-4dc0-8d48-a2fcdc35cff0")] = new InputRecordListQuery();
							queryDictionary[new Guid("abea33cb-e34a-4dc0-8d48-a2fcdc35cff0")].FieldName = "subject";
							queryDictionary[new Guid("abea33cb-e34a-4dc0-8d48-a2fcdc35cff0")].FieldValue = "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
							queryDictionary[new Guid("abea33cb-e34a-4dc0-8d48-a2fcdc35cff0")].QueryType = "CONTAINS";
							queryDictionary[new Guid("abea33cb-e34a-4dc0-8d48-a2fcdc35cff0")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("abea33cb-e34a-4dc0-8d48-a2fcdc35cff0"))) { queryDictionary[new Guid("abea33cb-e34a-4dc0-8d48-a2fcdc35cff0")].SubQueries = subQueryDictionary[new Guid("abea33cb-e34a-4dc0-8d48-a2fcdc35cff0")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("9fe42513-4c88-4222-bfb0-437086df9e6c"))) { subQueryDictionary[new Guid("9fe42513-4c88-4222-bfb0-437086df9e6c")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("9fe42513-4c88-4222-bfb0-437086df9e6c")].Add(queryDictionary[new Guid("abea33cb-e34a-4dc0-8d48-a2fcdc35cff0")]);
						}
						{
							queryDictionary[new Guid("0206e517-4e02-4a14-907d-3621a6ba013e")] = new InputRecordListQuery();
							queryDictionary[new Guid("0206e517-4e02-4a14-907d-3621a6ba013e")].FieldName = "status";
							queryDictionary[new Guid("0206e517-4e02-4a14-907d-3621a6ba013e")].FieldValue = "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
							queryDictionary[new Guid("0206e517-4e02-4a14-907d-3621a6ba013e")].QueryType = "EQ";
							queryDictionary[new Guid("0206e517-4e02-4a14-907d-3621a6ba013e")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("0206e517-4e02-4a14-907d-3621a6ba013e"))) { queryDictionary[new Guid("0206e517-4e02-4a14-907d-3621a6ba013e")].SubQueries = subQueryDictionary[new Guid("0206e517-4e02-4a14-907d-3621a6ba013e")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("9fe42513-4c88-4222-bfb0-437086df9e6c"))) { subQueryDictionary[new Guid("9fe42513-4c88-4222-bfb0-437086df9e6c")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("9fe42513-4c88-4222-bfb0-437086df9e6c")].Add(queryDictionary[new Guid("0206e517-4e02-4a14-907d-3621a6ba013e")]);
						}
						{
							queryDictionary[new Guid("93efb4c4-ae57-4660-ae82-6fd7a42f974a")] = new InputRecordListQuery();
							queryDictionary[new Guid("93efb4c4-ae57-4660-ae82-6fd7a42f974a")].FieldName = "priority";
							queryDictionary[new Guid("93efb4c4-ae57-4660-ae82-6fd7a42f974a")].FieldValue = "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
							queryDictionary[new Guid("93efb4c4-ae57-4660-ae82-6fd7a42f974a")].QueryType = "EQ";
							queryDictionary[new Guid("93efb4c4-ae57-4660-ae82-6fd7a42f974a")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("93efb4c4-ae57-4660-ae82-6fd7a42f974a"))) { queryDictionary[new Guid("93efb4c4-ae57-4660-ae82-6fd7a42f974a")].SubQueries = subQueryDictionary[new Guid("93efb4c4-ae57-4660-ae82-6fd7a42f974a")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("9fe42513-4c88-4222-bfb0-437086df9e6c"))) { subQueryDictionary[new Guid("9fe42513-4c88-4222-bfb0-437086df9e6c")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("9fe42513-4c88-4222-bfb0-437086df9e6c")].Add(queryDictionary[new Guid("93efb4c4-ae57-4660-ae82-6fd7a42f974a")]);
						}
						if (subQueryDictionary.ContainsKey(new Guid("9fe42513-4c88-4222-bfb0-437086df9e6c"))) { queryDictionary[new Guid("9fe42513-4c88-4222-bfb0-437086df9e6c")].SubQueries = subQueryDictionary[new Guid("9fe42513-4c88-4222-bfb0-437086df9e6c")]; }
						if (!subQueryDictionary.ContainsKey(new Guid("a6643b4d-654d-4e40-9b46-c3c7700fdb83"))) { subQueryDictionary[new Guid("a6643b4d-654d-4e40-9b46-c3c7700fdb83")] = new List<InputRecordListQuery>(); }
						subQueryDictionary[new Guid("a6643b4d-654d-4e40-9b46-c3c7700fdb83")].Add(queryDictionary[new Guid("9fe42513-4c88-4222-bfb0-437086df9e6c")]);
					}
					if (subQueryDictionary.ContainsKey(new Guid("a6643b4d-654d-4e40-9b46-c3c7700fdb83"))) { createListInput.Query.SubQueries = subQueryDictionary[new Guid("a6643b4d-654d-4e40-9b46-c3c7700fdb83")]; }
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

			#region << Update  Enity: wv_bug name: owned_bugs >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "owned_bugs").Id;
				createListInput.Type = "General";
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
					var queryDictionary = new Dictionary<Guid, InputRecordListQuery>();
					var subQueryDictionary = new Dictionary<Guid, List<InputRecordListQuery>>();
					//Main query rule
					createListInput.Query.FieldName = null;
					createListInput.Query.FieldValue = null;
					createListInput.Query.QueryType = "AND";
					createListInput.Query.SubQueries = new List<InputRecordListQuery>();
					{
						queryDictionary[new Guid("9b1812dd-1390-496f-8572-8d5470a6ef9f")] = new InputRecordListQuery();
						queryDictionary[new Guid("9b1812dd-1390-496f-8572-8d5470a6ef9f")].FieldName = null;
						queryDictionary[new Guid("9b1812dd-1390-496f-8572-8d5470a6ef9f")].FieldValue = null;
						queryDictionary[new Guid("9b1812dd-1390-496f-8572-8d5470a6ef9f")].QueryType = "OR";
						queryDictionary[new Guid("9b1812dd-1390-496f-8572-8d5470a6ef9f")].SubQueries = new List<InputRecordListQuery>();
						{
							queryDictionary[new Guid("dc4acb44-ebb0-481f-8b9e-2ba2cf97d9fc")] = new InputRecordListQuery();
							queryDictionary[new Guid("dc4acb44-ebb0-481f-8b9e-2ba2cf97d9fc")].FieldName = "owner_id";
							queryDictionary[new Guid("dc4acb44-ebb0-481f-8b9e-2ba2cf97d9fc")].FieldValue = "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
							queryDictionary[new Guid("dc4acb44-ebb0-481f-8b9e-2ba2cf97d9fc")].QueryType = "EQ";
							queryDictionary[new Guid("dc4acb44-ebb0-481f-8b9e-2ba2cf97d9fc")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("dc4acb44-ebb0-481f-8b9e-2ba2cf97d9fc"))) { queryDictionary[new Guid("dc4acb44-ebb0-481f-8b9e-2ba2cf97d9fc")].SubQueries = subQueryDictionary[new Guid("dc4acb44-ebb0-481f-8b9e-2ba2cf97d9fc")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("9b1812dd-1390-496f-8572-8d5470a6ef9f"))) { subQueryDictionary[new Guid("9b1812dd-1390-496f-8572-8d5470a6ef9f")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("9b1812dd-1390-496f-8572-8d5470a6ef9f")].Add(queryDictionary[new Guid("dc4acb44-ebb0-481f-8b9e-2ba2cf97d9fc")]);
						}
						if (subQueryDictionary.ContainsKey(new Guid("9b1812dd-1390-496f-8572-8d5470a6ef9f"))) { queryDictionary[new Guid("9b1812dd-1390-496f-8572-8d5470a6ef9f")].SubQueries = subQueryDictionary[new Guid("9b1812dd-1390-496f-8572-8d5470a6ef9f")]; }
						if (!subQueryDictionary.ContainsKey(new Guid("7935df8f-dea5-4f7c-b47b-1ca9145edd82"))) { subQueryDictionary[new Guid("7935df8f-dea5-4f7c-b47b-1ca9145edd82")] = new List<InputRecordListQuery>(); }
						subQueryDictionary[new Guid("7935df8f-dea5-4f7c-b47b-1ca9145edd82")].Add(queryDictionary[new Guid("9b1812dd-1390-496f-8572-8d5470a6ef9f")]);
					}
					{
						queryDictionary[new Guid("d1199ab1-4fe9-4b3f-8e48-628ede3f5937")] = new InputRecordListQuery();
						queryDictionary[new Guid("d1199ab1-4fe9-4b3f-8e48-628ede3f5937")].FieldName = null;
						queryDictionary[new Guid("d1199ab1-4fe9-4b3f-8e48-628ede3f5937")].FieldValue = null;
						queryDictionary[new Guid("d1199ab1-4fe9-4b3f-8e48-628ede3f5937")].QueryType = "AND";
						queryDictionary[new Guid("d1199ab1-4fe9-4b3f-8e48-628ede3f5937")].SubQueries = new List<InputRecordListQuery>();
						{
							queryDictionary[new Guid("af42c498-f722-4568-8cef-a361c53dc950")] = new InputRecordListQuery();
							queryDictionary[new Guid("af42c498-f722-4568-8cef-a361c53dc950")].FieldName = "code";
							queryDictionary[new Guid("af42c498-f722-4568-8cef-a361c53dc950")].FieldValue = "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
							queryDictionary[new Guid("af42c498-f722-4568-8cef-a361c53dc950")].QueryType = "CONTAINS";
							queryDictionary[new Guid("af42c498-f722-4568-8cef-a361c53dc950")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("af42c498-f722-4568-8cef-a361c53dc950"))) { queryDictionary[new Guid("af42c498-f722-4568-8cef-a361c53dc950")].SubQueries = subQueryDictionary[new Guid("af42c498-f722-4568-8cef-a361c53dc950")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("d1199ab1-4fe9-4b3f-8e48-628ede3f5937"))) { subQueryDictionary[new Guid("d1199ab1-4fe9-4b3f-8e48-628ede3f5937")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("d1199ab1-4fe9-4b3f-8e48-628ede3f5937")].Add(queryDictionary[new Guid("af42c498-f722-4568-8cef-a361c53dc950")]);
						}
						{
							queryDictionary[new Guid("10bb07ab-ce08-41da-afb3-a33daa18c1d5")] = new InputRecordListQuery();
							queryDictionary[new Guid("10bb07ab-ce08-41da-afb3-a33daa18c1d5")].FieldName = "subject";
							queryDictionary[new Guid("10bb07ab-ce08-41da-afb3-a33daa18c1d5")].FieldValue = "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
							queryDictionary[new Guid("10bb07ab-ce08-41da-afb3-a33daa18c1d5")].QueryType = "CONTAINS";
							queryDictionary[new Guid("10bb07ab-ce08-41da-afb3-a33daa18c1d5")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("10bb07ab-ce08-41da-afb3-a33daa18c1d5"))) { queryDictionary[new Guid("10bb07ab-ce08-41da-afb3-a33daa18c1d5")].SubQueries = subQueryDictionary[new Guid("10bb07ab-ce08-41da-afb3-a33daa18c1d5")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("d1199ab1-4fe9-4b3f-8e48-628ede3f5937"))) { subQueryDictionary[new Guid("d1199ab1-4fe9-4b3f-8e48-628ede3f5937")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("d1199ab1-4fe9-4b3f-8e48-628ede3f5937")].Add(queryDictionary[new Guid("10bb07ab-ce08-41da-afb3-a33daa18c1d5")]);
						}
						{
							queryDictionary[new Guid("083838f0-5418-4216-98c6-405c36a8914a")] = new InputRecordListQuery();
							queryDictionary[new Guid("083838f0-5418-4216-98c6-405c36a8914a")].FieldName = "status";
							queryDictionary[new Guid("083838f0-5418-4216-98c6-405c36a8914a")].FieldValue = "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
							queryDictionary[new Guid("083838f0-5418-4216-98c6-405c36a8914a")].QueryType = "EQ";
							queryDictionary[new Guid("083838f0-5418-4216-98c6-405c36a8914a")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("083838f0-5418-4216-98c6-405c36a8914a"))) { queryDictionary[new Guid("083838f0-5418-4216-98c6-405c36a8914a")].SubQueries = subQueryDictionary[new Guid("083838f0-5418-4216-98c6-405c36a8914a")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("d1199ab1-4fe9-4b3f-8e48-628ede3f5937"))) { subQueryDictionary[new Guid("d1199ab1-4fe9-4b3f-8e48-628ede3f5937")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("d1199ab1-4fe9-4b3f-8e48-628ede3f5937")].Add(queryDictionary[new Guid("083838f0-5418-4216-98c6-405c36a8914a")]);
						}
						{
							queryDictionary[new Guid("3abeac9b-6195-4c42-93ec-250036dd53a6")] = new InputRecordListQuery();
							queryDictionary[new Guid("3abeac9b-6195-4c42-93ec-250036dd53a6")].FieldName = "priority";
							queryDictionary[new Guid("3abeac9b-6195-4c42-93ec-250036dd53a6")].FieldValue = "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
							queryDictionary[new Guid("3abeac9b-6195-4c42-93ec-250036dd53a6")].QueryType = "EQ";
							queryDictionary[new Guid("3abeac9b-6195-4c42-93ec-250036dd53a6")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("3abeac9b-6195-4c42-93ec-250036dd53a6"))) { queryDictionary[new Guid("3abeac9b-6195-4c42-93ec-250036dd53a6")].SubQueries = subQueryDictionary[new Guid("3abeac9b-6195-4c42-93ec-250036dd53a6")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("d1199ab1-4fe9-4b3f-8e48-628ede3f5937"))) { subQueryDictionary[new Guid("d1199ab1-4fe9-4b3f-8e48-628ede3f5937")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("d1199ab1-4fe9-4b3f-8e48-628ede3f5937")].Add(queryDictionary[new Guid("3abeac9b-6195-4c42-93ec-250036dd53a6")]);
						}
						if (subQueryDictionary.ContainsKey(new Guid("d1199ab1-4fe9-4b3f-8e48-628ede3f5937"))) { queryDictionary[new Guid("d1199ab1-4fe9-4b3f-8e48-628ede3f5937")].SubQueries = subQueryDictionary[new Guid("d1199ab1-4fe9-4b3f-8e48-628ede3f5937")]; }
						if (!subQueryDictionary.ContainsKey(new Guid("7935df8f-dea5-4f7c-b47b-1ca9145edd82"))) { subQueryDictionary[new Guid("7935df8f-dea5-4f7c-b47b-1ca9145edd82")] = new List<InputRecordListQuery>(); }
						subQueryDictionary[new Guid("7935df8f-dea5-4f7c-b47b-1ca9145edd82")].Add(queryDictionary[new Guid("d1199ab1-4fe9-4b3f-8e48-628ede3f5937")]);
					}
					if (subQueryDictionary.ContainsKey(new Guid("7935df8f-dea5-4f7c-b47b-1ca9145edd82"))) { createListInput.Query.SubQueries = subQueryDictionary[new Guid("7935df8f-dea5-4f7c-b47b-1ca9145edd82")]; }
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

			#region << Update  Enity: wv_bug name: all_bugs >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "all_bugs").Id;
				createListInput.Type = "General";
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

			#region << Update  Enity: wv_bug name: admin >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "admin").Id;
				createListInput.Type = "Hidden";
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

			#region << Update  Enity: wv_bug name: my_bugs >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "my_bugs").Id;
				createListInput.Type = "General";
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
					var queryDictionary = new Dictionary<Guid, InputRecordListQuery>();
					var subQueryDictionary = new Dictionary<Guid, List<InputRecordListQuery>>();
					//Main query rule
					createListInput.Query.FieldName = null;
					createListInput.Query.FieldValue = null;
					createListInput.Query.QueryType = "AND";
					createListInput.Query.SubQueries = new List<InputRecordListQuery>();
					{
						queryDictionary[new Guid("fcb8b25e-81d1-4b22-89bf-e67d4000d1bf")] = new InputRecordListQuery();
						queryDictionary[new Guid("fcb8b25e-81d1-4b22-89bf-e67d4000d1bf")].FieldName = null;
						queryDictionary[new Guid("fcb8b25e-81d1-4b22-89bf-e67d4000d1bf")].FieldValue = null;
						queryDictionary[new Guid("fcb8b25e-81d1-4b22-89bf-e67d4000d1bf")].QueryType = "AND";
						queryDictionary[new Guid("fcb8b25e-81d1-4b22-89bf-e67d4000d1bf")].SubQueries = new List<InputRecordListQuery>();
						{
							queryDictionary[new Guid("de3253ce-9f62-4014-9c12-b22dac0344c2")] = new InputRecordListQuery();
							queryDictionary[new Guid("de3253ce-9f62-4014-9c12-b22dac0344c2")].FieldName = "owner_id";
							queryDictionary[new Guid("de3253ce-9f62-4014-9c12-b22dac0344c2")].FieldValue = "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
							queryDictionary[new Guid("de3253ce-9f62-4014-9c12-b22dac0344c2")].QueryType = "EQ";
							queryDictionary[new Guid("de3253ce-9f62-4014-9c12-b22dac0344c2")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("de3253ce-9f62-4014-9c12-b22dac0344c2"))) { queryDictionary[new Guid("de3253ce-9f62-4014-9c12-b22dac0344c2")].SubQueries = subQueryDictionary[new Guid("de3253ce-9f62-4014-9c12-b22dac0344c2")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("fcb8b25e-81d1-4b22-89bf-e67d4000d1bf"))) { subQueryDictionary[new Guid("fcb8b25e-81d1-4b22-89bf-e67d4000d1bf")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("fcb8b25e-81d1-4b22-89bf-e67d4000d1bf")].Add(queryDictionary[new Guid("de3253ce-9f62-4014-9c12-b22dac0344c2")]);
						}
						if (subQueryDictionary.ContainsKey(new Guid("fcb8b25e-81d1-4b22-89bf-e67d4000d1bf"))) { queryDictionary[new Guid("fcb8b25e-81d1-4b22-89bf-e67d4000d1bf")].SubQueries = subQueryDictionary[new Guid("fcb8b25e-81d1-4b22-89bf-e67d4000d1bf")]; }
						if (!subQueryDictionary.ContainsKey(new Guid("345848cd-2987-4d01-ae68-9e1d19544c6a"))) { subQueryDictionary[new Guid("345848cd-2987-4d01-ae68-9e1d19544c6a")] = new List<InputRecordListQuery>(); }
						subQueryDictionary[new Guid("345848cd-2987-4d01-ae68-9e1d19544c6a")].Add(queryDictionary[new Guid("fcb8b25e-81d1-4b22-89bf-e67d4000d1bf")]);
					}
					{
						queryDictionary[new Guid("b5c48786-0d53-4b3a-b136-421e6927973f")] = new InputRecordListQuery();
						queryDictionary[new Guid("b5c48786-0d53-4b3a-b136-421e6927973f")].FieldName = null;
						queryDictionary[new Guid("b5c48786-0d53-4b3a-b136-421e6927973f")].FieldValue = null;
						queryDictionary[new Guid("b5c48786-0d53-4b3a-b136-421e6927973f")].QueryType = "AND";
						queryDictionary[new Guid("b5c48786-0d53-4b3a-b136-421e6927973f")].SubQueries = new List<InputRecordListQuery>();
						{
							queryDictionary[new Guid("b037a01d-93b7-4a7e-bf17-12701a0bd653")] = new InputRecordListQuery();
							queryDictionary[new Guid("b037a01d-93b7-4a7e-bf17-12701a0bd653")].FieldName = "code";
							queryDictionary[new Guid("b037a01d-93b7-4a7e-bf17-12701a0bd653")].FieldValue = "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
							queryDictionary[new Guid("b037a01d-93b7-4a7e-bf17-12701a0bd653")].QueryType = "CONTAINS";
							queryDictionary[new Guid("b037a01d-93b7-4a7e-bf17-12701a0bd653")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("b037a01d-93b7-4a7e-bf17-12701a0bd653"))) { queryDictionary[new Guid("b037a01d-93b7-4a7e-bf17-12701a0bd653")].SubQueries = subQueryDictionary[new Guid("b037a01d-93b7-4a7e-bf17-12701a0bd653")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("b5c48786-0d53-4b3a-b136-421e6927973f"))) { subQueryDictionary[new Guid("b5c48786-0d53-4b3a-b136-421e6927973f")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("b5c48786-0d53-4b3a-b136-421e6927973f")].Add(queryDictionary[new Guid("b037a01d-93b7-4a7e-bf17-12701a0bd653")]);
						}
						{
							queryDictionary[new Guid("53094889-679d-4bd0-b391-e59bc90f5a2f")] = new InputRecordListQuery();
							queryDictionary[new Guid("53094889-679d-4bd0-b391-e59bc90f5a2f")].FieldName = "subject";
							queryDictionary[new Guid("53094889-679d-4bd0-b391-e59bc90f5a2f")].FieldValue = "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
							queryDictionary[new Guid("53094889-679d-4bd0-b391-e59bc90f5a2f")].QueryType = "CONTAINS";
							queryDictionary[new Guid("53094889-679d-4bd0-b391-e59bc90f5a2f")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("53094889-679d-4bd0-b391-e59bc90f5a2f"))) { queryDictionary[new Guid("53094889-679d-4bd0-b391-e59bc90f5a2f")].SubQueries = subQueryDictionary[new Guid("53094889-679d-4bd0-b391-e59bc90f5a2f")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("b5c48786-0d53-4b3a-b136-421e6927973f"))) { subQueryDictionary[new Guid("b5c48786-0d53-4b3a-b136-421e6927973f")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("b5c48786-0d53-4b3a-b136-421e6927973f")].Add(queryDictionary[new Guid("53094889-679d-4bd0-b391-e59bc90f5a2f")]);
						}
						{
							queryDictionary[new Guid("012845b5-27ee-4407-ad07-0531d681dd07")] = new InputRecordListQuery();
							queryDictionary[new Guid("012845b5-27ee-4407-ad07-0531d681dd07")].FieldName = "status";
							queryDictionary[new Guid("012845b5-27ee-4407-ad07-0531d681dd07")].FieldValue = "closed";
							queryDictionary[new Guid("012845b5-27ee-4407-ad07-0531d681dd07")].QueryType = "NOT";
							queryDictionary[new Guid("012845b5-27ee-4407-ad07-0531d681dd07")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("012845b5-27ee-4407-ad07-0531d681dd07"))) { queryDictionary[new Guid("012845b5-27ee-4407-ad07-0531d681dd07")].SubQueries = subQueryDictionary[new Guid("012845b5-27ee-4407-ad07-0531d681dd07")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("b5c48786-0d53-4b3a-b136-421e6927973f"))) { subQueryDictionary[new Guid("b5c48786-0d53-4b3a-b136-421e6927973f")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("b5c48786-0d53-4b3a-b136-421e6927973f")].Add(queryDictionary[new Guid("012845b5-27ee-4407-ad07-0531d681dd07")]);
						}
						{
							queryDictionary[new Guid("c332e15f-c5c3-4863-9925-12758f917b77")] = new InputRecordListQuery();
							queryDictionary[new Guid("c332e15f-c5c3-4863-9925-12758f917b77")].FieldName = "priority";
							queryDictionary[new Guid("c332e15f-c5c3-4863-9925-12758f917b77")].FieldValue = "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
							queryDictionary[new Guid("c332e15f-c5c3-4863-9925-12758f917b77")].QueryType = "EQ";
							queryDictionary[new Guid("c332e15f-c5c3-4863-9925-12758f917b77")].SubQueries = new List<InputRecordListQuery>();
							if (subQueryDictionary.ContainsKey(new Guid("c332e15f-c5c3-4863-9925-12758f917b77"))) { queryDictionary[new Guid("c332e15f-c5c3-4863-9925-12758f917b77")].SubQueries = subQueryDictionary[new Guid("c332e15f-c5c3-4863-9925-12758f917b77")]; }
							if (!subQueryDictionary.ContainsKey(new Guid("b5c48786-0d53-4b3a-b136-421e6927973f"))) { subQueryDictionary[new Guid("b5c48786-0d53-4b3a-b136-421e6927973f")] = new List<InputRecordListQuery>(); }
							subQueryDictionary[new Guid("b5c48786-0d53-4b3a-b136-421e6927973f")].Add(queryDictionary[new Guid("c332e15f-c5c3-4863-9925-12758f917b77")]);
						}
						if (subQueryDictionary.ContainsKey(new Guid("b5c48786-0d53-4b3a-b136-421e6927973f"))) { queryDictionary[new Guid("b5c48786-0d53-4b3a-b136-421e6927973f")].SubQueries = subQueryDictionary[new Guid("b5c48786-0d53-4b3a-b136-421e6927973f")]; }
						if (!subQueryDictionary.ContainsKey(new Guid("345848cd-2987-4d01-ae68-9e1d19544c6a"))) { subQueryDictionary[new Guid("345848cd-2987-4d01-ae68-9e1d19544c6a")] = new List<InputRecordListQuery>(); }
						subQueryDictionary[new Guid("345848cd-2987-4d01-ae68-9e1d19544c6a")].Add(queryDictionary[new Guid("b5c48786-0d53-4b3a-b136-421e6927973f")]);
					}
					if (subQueryDictionary.ContainsKey(new Guid("345848cd-2987-4d01-ae68-9e1d19544c6a"))) { createListInput.Query.SubQueries = subQueryDictionary[new Guid("345848cd-2987-4d01-ae68-9e1d19544c6a")]; }
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

			#region << View  Enity: plugin_data name: quick_create >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("d928d031-c8b1-4359-be3e-39bceb58f268")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("2a384965-60ec-4856-ae8c-a0396ea646ae");
				createViewInput.Type = "Quick_Create";
				createViewInput.Name = "quick_create";
				createViewInput.Label = "Quick create";
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
					var response = entMan.CreateRecordView(new Guid("d928d031-c8b1-4359-be3e-39bceb58f268"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: plugin_data Updated view: quick_create Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: plugin_data name: quick_view >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("d928d031-c8b1-4359-be3e-39bceb58f268")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("30921924-a411-4235-8d5a-0380a9c33a21");
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
					var response = entMan.CreateRecordView(new Guid("d928d031-c8b1-4359-be3e-39bceb58f268"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: plugin_data Updated view: quick_view Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: plugin_data name: create >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("d928d031-c8b1-4359-be3e-39bceb58f268")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("a9905dc8-9a3d-4f58-8d4f-597b6e9cec0c");
				createViewInput.Type = "Create";
				createViewInput.Name = "create";
				createViewInput.Label = "Create";
				createViewInput.Title = "Create";
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
					var response = entMan.CreateRecordView(new Guid("d928d031-c8b1-4359-be3e-39bceb58f268"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: plugin_data Updated view: create Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: plugin_data name: general >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("d928d031-c8b1-4359-be3e-39bceb58f268")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("253faf72-58ea-4152-9b22-3e29814a400b");
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
					var response = entMan.CreateRecordView(new Guid("d928d031-c8b1-4359-be3e-39bceb58f268"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: plugin_data Updated view: general Message:" + response.Message);
				}
			}
			#endregion

			#region << List  Enity: plugin_data name: general >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("d928d031-c8b1-4359-be3e-39bceb58f268")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = new Guid("7fa5aafd-b093-4088-abe0-ba2147772a36");
				createListInput.Type = "General";
				createListInput.Name = "general";
				createListInput.Label = "General";
				createListInput.Title = null;
				createListInput.Weight = Decimal.Parse("10.0");
				createListInput.Default = true;
				createListInput.System = false;
				createListInput.CssClass = null;
				createListInput.IconName = string.IsNullOrEmpty("list") ? string.Empty : "list";
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
					var response = entMan.CreateRecordList(new Guid("d928d031-c8b1-4359-be3e-39bceb58f268"), createListInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: plugin_data Created list: general Message:" + response.Message);
				}
			}
			#endregion

			#region << List  Enity: plugin_data name: lookup >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("d928d031-c8b1-4359-be3e-39bceb58f268")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = new Guid("b3001a35-6037-4491-b2b0-1340e05ff57f");
				createListInput.Type = "Lookup";
				createListInput.Name = "lookup";
				createListInput.Label = "Lookup";
				createListInput.Title = null;
				createListInput.Weight = Decimal.Parse("10.0");
				createListInput.Default = true;
				createListInput.System = false;
				createListInput.CssClass = null;
				createListInput.IconName = string.IsNullOrEmpty("list") ? string.Empty : "list";
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
					var response = entMan.CreateRecordList(new Guid("d928d031-c8b1-4359-be3e-39bceb58f268"), createListInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: plugin_data Created list: lookup Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: wv_project_comment name: create >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("c02e83e9-f601-489a-8d72-e8e66a47dfa8");
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
					var response = entMan.CreateRecordView(new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_project_comment Updated view: create Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: wv_project_comment name: quick_create >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("eea2f8c7-0dd1-41c1-8924-9b37e1abb9a9");
				createViewInput.Type = "Quick_Create";
				createViewInput.Name = "quick_create";
				createViewInput.Label = "Quick create";
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
					var response = entMan.CreateRecordView(new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_project_comment Updated view: quick_create Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: wv_project_comment name: quick_view >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("28e7ff58-b08f-4e36-88af-4f83da309d75");
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
					var response = entMan.CreateRecordView(new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_project_comment Updated view: quick_view Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: wv_project_comment name: general >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("769b359c-9444-4ca7-aa02-cfb80835492a");
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
					var response = entMan.CreateRecordView(new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_project_comment Updated view: general Message:" + response.Message);
				}
			}
			#endregion

			#region << List  Enity: wv_project_comment name: general >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = new Guid("5e1c1cef-5832-4a10-b986-2bb5c06ccc79");
				createListInput.Type = "General";
				createListInput.Name = "general";
				createListInput.Label = "General";
				createListInput.Title = null;
				createListInput.Weight = Decimal.Parse("10.0");
				createListInput.Default = true;
				createListInput.System = false;
				createListInput.CssClass = null;
				createListInput.IconName = string.IsNullOrEmpty("list") ? string.Empty : "list";
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
					var response = entMan.CreateRecordList(new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99"), createListInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_project_comment Created list: general Message:" + response.Message);
				}
			}
			#endregion

			#region << List  Enity: wv_project_comment name: lookup >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = new Guid("9916b331-3f45-485a-b431-23558e96175d");
				createListInput.Type = "Lookup";
				createListInput.Name = "lookup";
				createListInput.Label = "Lookup";
				createListInput.Title = null;
				createListInput.Weight = Decimal.Parse("10.0");
				createListInput.Default = true;
				createListInput.System = false;
				createListInput.CssClass = null;
				createListInput.IconName = string.IsNullOrEmpty("list") ? string.Empty : "list";
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
					var response = entMan.CreateRecordList(new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99"), createListInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_project_comment Created list: lookup Message:" + response.Message);
				}
			}
			#endregion

			#region << Update  Enity: wv_project_comment name: task_comments >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "task_comments").Id;
				createListInput.Type = "Hidden";
				createListInput.Name = "task_comments";
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
				createListInput.DynamicHtmlTemplate = "/plugins/webvella-projects/templates/task-comments.html";
				createListInput.DataSourceUrl = null;
				createListInput.ServiceCode = "/plugins/webvella-projects/providers/task-comments.service.js";
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
						throw new Exception("System error 10060. Entity: wv_project_comment Updated list: task_comments Message:" + response.Message);
				}
			}
			#endregion

			#region << Update  Enity: wv_project_comment name: bug_comments >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "bug_comments").Id;
				createListInput.Type = "Hidden";
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




		}

	}
}
