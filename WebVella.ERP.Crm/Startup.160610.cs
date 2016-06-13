using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Api.Models.AutoMapper;
using WebVella.ERP.Utilities;

namespace WebVella.ERP.Crm
{
    public partial class Startup
    {

		private static void Patch160610(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan, bool createSampleRecords = false) 
		{
		
			#region << View  Enity: wv_customer name: create >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("90bcdb47-2cde-4137-a412-0198348fecc0")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("334e9197-7d4c-48b8-8372-184e843a9090");
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
					var response = entMan.CreateRecordView(new Guid("90bcdb47-2cde-4137-a412-0198348fecc0"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_customer Updated view: create Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: wv_customer name: quick_create >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("90bcdb47-2cde-4137-a412-0198348fecc0")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("cf6b1f16-74ee-4e31-8a16-7847551b73a5");
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
					var response = entMan.CreateRecordView(new Guid("90bcdb47-2cde-4137-a412-0198348fecc0"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_customer Updated view: quick_create Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: wv_customer name: quick_view >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("90bcdb47-2cde-4137-a412-0198348fecc0")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("e21ed831-9997-427f-8306-cebcc13091d5");
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
					var response = entMan.CreateRecordView(new Guid("90bcdb47-2cde-4137-a412-0198348fecc0"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_customer Updated view: quick_view Message:" + response.Message);
				}
			}
			#endregion

			#region << View  Enity: wv_customer name: general >>
			{
				var createViewEntity = entMan.ReadEntity(new Guid("90bcdb47-2cde-4137-a412-0198348fecc0")).Object;
				var createViewInput = new InputRecordView();

				#region << details >>
				createViewInput.Id = new Guid("df2a03e6-e101-40b6-bd8c-2bd0f6858523");
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
					var response = entMan.CreateRecordView(new Guid("90bcdb47-2cde-4137-a412-0198348fecc0"), createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_customer Updated view: general Message:" + response.Message);
				}
			}
			#endregion

			#region << Update  Enity: wv_customer View: admin_details >>
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

			#region << List  Enity: wv_customer name: general >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("90bcdb47-2cde-4137-a412-0198348fecc0")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = new Guid("96825471-a154-4bc8-a04d-e16c7029c026");
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
					var response = entMan.CreateRecordList(new Guid("90bcdb47-2cde-4137-a412-0198348fecc0"), createListInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_customer Created list: general Message:" + response.Message);
				}
			}
			#endregion

			#region << Update  Enity: wv_customer name: admin >>
			{
				var createListEntity = entMan.ReadEntity(new Guid("90bcdb47-2cde-4137-a412-0198348fecc0")).Object;
				var createListInput = new InputRecordList();

				#region << details >>
				createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "admin").Id;
				createListInput.Type = "Hidden";
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



		}

    }
}
