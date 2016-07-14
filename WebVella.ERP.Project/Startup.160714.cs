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
		private static void Patch160714(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan, bool createSampleRecords = false)
		{

#region << ***Update list***  Entity: pt_role List Name: lookup >>
{
	var createListEntity = entMan.ReadEntity(new Guid("cc6cddbc-23c0-4f12-a1fe-a2241e75908b")).Object;
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

		#region << action item: wv_create_record >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_record";
			actionItem.Menu = "page-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" ng-href=""{{::ngCtrl.getRecordCreateUrl()}}"">Add New</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_import_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_import_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("10.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openImportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')""><i class=""fa fa-fw fa-upload""></i> Import CSV</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_export_records >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_export_records";
			actionItem.Menu = "page-title-dropdown";
			actionItem.Weight = Decimal.Parse("11.0");
			actionItem.Template = @"<a ng-click=""ngCtrl.openExportModal()"" class=""ng-hide"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')""><i class=""fa fa-fw fa-download""></i> Export CSV</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_record_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_record_details";
			actionItem.Menu = "record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record)}}""><i class=""fa fa-fw fa-eye""></i></a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_recursive_list_add_existing >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_recursive_list_add_existing";
			actionItem.Menu = "recursive-list-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" class=""btn btn-outline btn-sm"" ng-if=""::canAddExisting"" ng-click=""addExistingItem()""><i class=""fa fa-download""></i> Add existing</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_recursive_list_add_new >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_recursive_list_add_new";
			actionItem.Menu = "recursive-list-title";
			actionItem.Weight = Decimal.Parse("2.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" class=""btn btn-outline btn-sm"" ng-if=""::canCreate"" ng-click=""manageRelatedRecordItem(null)""><i class=""fa fa-plus""></i> Create & Add</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_recursive_list_view >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_recursive_list_view";
			actionItem.Menu = "recursive-list-record-row";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" title=""quick view this record"" class=""btn btn-sm btn-outline"" ng-click=""viewRelatedRecordItem(record)""><i class=""fa fa-eye""></i></a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_recursive_list_edit >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_recursive_list_edit";
			actionItem.Menu = "recursive-list-record-row";
			actionItem.Weight = Decimal.Parse("2.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" title=""quick edit this record"" class=""btn btn-sm btn-outline"" ng-click=""manageRelatedRecordItem(record)"" ng-if=""::canUpdate""><i class=""fa fa-pencil""></i></a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_recursive_list_unrelate >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_recursive_list_unrelate";
			actionItem.Menu = "recursive-list-record-row";
			actionItem.Weight = Decimal.Parse("3.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" title=""Detach records relation"" class=""btn btn-sm btn-outline"" confirmed-click=""instantDetachRecord(record)"" ng-confirm-click=""Are you sure that you need this relation broken?"" ng-if=""::canRemove""><i class=""fa fa-times go-red""></i></a>";
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
			listField.EntityId = new Guid("cc6cddbc-23c0-4f12-a1fe-a2241e75908b");
			listField.EntityName = "pt_role";
			listField.FieldId = new Guid("c09d05e7-da9c-4ac5-950f-d392ca73bf30");
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
		var response = entMan.UpdateRecordList(new Guid("cc6cddbc-23c0-4f12-a1fe-a2241e75908b"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: pt_role Updated list: lookup Message:" + response.Message);
	}
}
#endregion

#region << ***Update view***  Entity: account View Name: general >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "general").Id;
	createViewInput.Type = "General";
	createViewInput.Name = "general";
	createViewInput.Label = "Details";
	createViewInput.Title = "User accounts";
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
			viewSection.Id = new Guid("7d066133-4efb-4296-a04e-4653b3064bd5");
			viewSection.Name = "section";
			viewSection.Label = "Details";
			viewSection.ShowLabel = true;
			viewSection.CssClass = "";
			viewSection.Collapsed = false;
			viewSection.TabOrder = "left-right";
			viewSection.Weight = Decimal.Parse("1.0");
			viewSection.Rows = new List<InputRecordViewRow>();

				#region << Row 1>>
				{
					var viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("7b2a1dca-1e5c-439a-8b6d-20df1b00426d");
					viewRow.Weight = Decimal.Parse("1.0");
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("6");
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << first_name >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140");
						viewItem.EntityName = "account";
						viewItem.FieldId = new Guid("c1b45f0e-6326-4294-a68a-59997dea5014");
						viewItem.FieldName = "first_name";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					#region << email >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140");
						viewItem.EntityName = "account";
						viewItem.FieldId = new Guid("7d1eef99-78c6-4d4d-b77b-9ad774218c28");
						viewItem.FieldName = "email";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					#region << title >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140");
						viewItem.EntityName = "account";
						viewItem.FieldId = new Guid("cc7db989-118b-40e8-9904-b004897a7253");
						viewItem.FieldName = "title";
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

					#region << last_name >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140");
						viewItem.EntityName = "account";
						viewItem.FieldId = new Guid("2dcf9997-62ed-4cb9-9dec-81ffb4c27ca2");
						viewItem.FieldName = "last_name";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					#region << image >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140");
						viewItem.EntityName = "account";
						viewItem.FieldId = new Guid("a56d9d7c-aeaa-4e78-842e-a27fccb99160");
						viewItem.FieldName = "image";
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
			#region << Section: job >>
			{
			var viewSection = new InputRecordViewSection();
			viewSection.Id = new Guid("8c9300e0-5ea6-468b-b726-6bfb9f24c29c");
			viewSection.Name = "job";
			viewSection.Label = "Job info";
			viewSection.ShowLabel = true;
			viewSection.CssClass = "";
			viewSection.Collapsed = false;
			viewSection.TabOrder = "left-right";
			viewSection.Weight = Decimal.Parse("2.0");
			viewSection.Rows = new List<InputRecordViewRow>();

				#region << Row 1>>
				{
					var viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("ad44fc0a-df76-411c-a273-2d72dd7428c7");
					viewRow.Weight = Decimal.Parse("1.0");
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("6");
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << job_title >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140");
						viewItem.EntityName = "account";
						viewItem.FieldId = new Guid("5872bbbb-5fb5-454f-a6a2-59ac02e6597f");
						viewItem.FieldName = "job_title";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					#region << department >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140");
						viewItem.EntityName = "account";
						viewItem.FieldId = new Guid("fe357e00-7d2a-4f7b-911e-81d368c5698d");
						viewItem.FieldName = "department";
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
						viewItemFromRelation.EntityId = new Guid("1bd880af-40d7-4557-b398-51353d730a54");
						viewItemFromRelation.EntityName = "account_role";
						viewItemFromRelation.Type = "fieldFromRelation";
						viewItemFromRelation.FieldId = new Guid("5fc20b39-a65a-49f1-9e9f-ee4132307479");
						viewItemFromRelation.FieldName = "name";
						viewItemFromRelation.FieldLabel = "Customer role";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = "";
						viewItemFromRelation.FieldRequired = false;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = new Guid("1f43ee16-01be-429c-a591-7d00ebeeebf3");
						viewItemFromRelation.RelationName = "account_role_account";
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion
					#region << View from relation: institution_name >>
					{
						var viewItemFromRelation = new InputRecordViewRelationViewItem();
						viewItemFromRelation.EntityId = new Guid("a74449eb-33c4-4782-84b8-f2707209f6a4");
						viewItemFromRelation.EntityName = "customer";
						viewItemFromRelation.ViewId = new Guid("6a55fc6b-de7f-49b7-b541-e30878e1b5f5");
						viewItemFromRelation.ViewName = "institution_name";
						viewItemFromRelation.FieldLabel = "Customer institution";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = "";
						viewItemFromRelation.FieldRequired = false;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = new Guid("d77261fa-4ac3-4f73-9ce4-6185803fe604");
						viewItemFromRelation.RelationName = "customer_account_customer";
						viewItemFromRelation.Type = "viewFromRelation";
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
			#region << Section: authentication >>
			{
			var viewSection = new InputRecordViewSection();
			viewSection.Id = new Guid("b2074682-02a7-4f48-9319-aa0e55096fdb");
			viewSection.Name = "authentication";
			viewSection.Label = "Authentication info";
			viewSection.ShowLabel = true;
			viewSection.CssClass = "";
			viewSection.Collapsed = false;
			viewSection.TabOrder = "left-right";
			viewSection.Weight = Decimal.Parse("3.0");
			viewSection.Rows = new List<InputRecordViewRow>();

				#region << Row 1>>
				{
					var viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("9fb14ead-1e99-4c6e-9f8e-1b54f7654d67");
					viewRow.Weight = Decimal.Parse("1.0");
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("6");
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << username >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140");
						viewItem.EntityName = "account";
						viewItem.FieldId = new Guid("48135a61-ee9a-4667-9941-b826c6fd56f2");
						viewItem.FieldName = "username";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					#region << password >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140");
						viewItem.EntityName = "account";
						viewItem.FieldId = new Guid("6781f6ad-ea42-4d33-9577-be989f019578");
						viewItem.FieldName = "password";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					#region << created_on >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140");
						viewItem.EntityName = "account";
						viewItem.FieldId = new Guid("aae6a160-568c-4db8-b6ea-20189717fef5");
						viewItem.FieldName = "created_on";
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

					#region << enabled >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140");
						viewItem.EntityName = "account";
						viewItem.FieldId = new Guid("e2265eda-dc45-405e-99b7-767f4201a306");
						viewItem.FieldName = "enabled";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					#region << verified >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140");
						viewItem.EntityName = "account";
						viewItem.FieldId = new Guid("8f273c5c-d032-4621-ae2e-beb7d7f08434");
						viewItem.FieldName = "verified";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					#region << last_logged_in >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140");
						viewItem.EntityName = "account";
						viewItem.FieldId = new Guid("f019a591-69b4-490c-a2df-8f65bad97423");
						viewItem.FieldName = "last_logged_in";
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
			actionItem.Template = @"<a href=""javascript:void(0)"" confirmed-click=""ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""{{'::DELETE_CONFIRMATION_ALERT_MESSAGE' | translate}}"" 
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
		var response = entMan.UpdateRecordView(new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: account Updated view: general Message:" + response.Message);
	}
}
#endregion

#region << ***Update list***  Entity: account List Name: lookup >>
{
	var createListEntity = entMan.ReadEntity(new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140")).Object;
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
	createListInput.VisibleColumnsCount = Int32.Parse("7");
	createListInput.ColumnWidthsCSV = "auto,160px,160px";
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

		#region << action item: wv_recursive_list_unrelate >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_recursive_list_unrelate";
			actionItem.Menu = "recursive-list-record-row";
			actionItem.Weight = Decimal.Parse("3.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" title=""Detach records relation"" class=""btn btn-sm btn-outline"" confirmed-click=""instantDetachRecord(record)"" ng-confirm-click=""Are you sure that you need this relation broken?"" ng-if=""::canRemove""><i class=""fa fa-times go-red""></i></a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_recursive_list_add_existing >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_recursive_list_add_existing";
			actionItem.Menu = "recursive-list-title";
			actionItem.Weight = Decimal.Parse("1.0");
			actionItem.Template = @"<a href=""javascript:void(0)"" class=""btn btn-outline btn-sm"" ng-if=""::canAddExisting"" ng-click=""addExistingItem()""><i class=""fa fa-download""></i> Add existing</a>";
			createListInput.ActionItems.Add(actionItem);
		}
		#endregion

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

		#region << email >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140");
			listField.EntityName = "account";
			listField.FieldId = new Guid("7d1eef99-78c6-4d4d-b77b-9ad774218c28");
			listField.FieldName = "email";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << first_name >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140");
			listField.EntityName = "account";
			listField.FieldId = new Guid("c1b45f0e-6326-4294-a68a-59997dea5014");
			listField.FieldName = "first_name";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << last_name >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140");
			listField.EntityName = "account";
			listField.FieldId = new Guid("2dcf9997-62ed-4cb9-9dec-81ffb4c27ca2");
			listField.FieldName = "last_name";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << created_on >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140");
			listField.EntityName = "account";
			listField.FieldId = new Guid("aae6a160-568c-4db8-b6ea-20189717fef5");
			listField.FieldName = "created_on";
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
		queryDictionary[new Guid("d6b0f570-8285-4545-b43b-81ff081fa412")] = new InputRecordListQuery();
		queryDictionary[new Guid("d6b0f570-8285-4545-b43b-81ff081fa412")].FieldName = "email";
		queryDictionary[new Guid("d6b0f570-8285-4545-b43b-81ff081fa412")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"email\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("d6b0f570-8285-4545-b43b-81ff081fa412")].QueryType = "CONTAINS";
		queryDictionary[new Guid("d6b0f570-8285-4545-b43b-81ff081fa412")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("d6b0f570-8285-4545-b43b-81ff081fa412"))) {queryDictionary[new Guid("d6b0f570-8285-4545-b43b-81ff081fa412")].SubQueries = subQueryDictionary[new Guid("d6b0f570-8285-4545-b43b-81ff081fa412")];}
		if(!subQueryDictionary.ContainsKey(new Guid("3e6a7903-d953-49d5-abed-968d88bafc7d"))) {subQueryDictionary[new Guid("3e6a7903-d953-49d5-abed-968d88bafc7d")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("3e6a7903-d953-49d5-abed-968d88bafc7d")].Add(queryDictionary[new Guid("d6b0f570-8285-4545-b43b-81ff081fa412")]);
		}
		{
		queryDictionary[new Guid("629abc1a-725b-4451-9a68-2dea4c15c5de")] = new InputRecordListQuery();
		queryDictionary[new Guid("629abc1a-725b-4451-9a68-2dea4c15c5de")].FieldName = "first_name";
		queryDictionary[new Guid("629abc1a-725b-4451-9a68-2dea4c15c5de")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"first_name\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("629abc1a-725b-4451-9a68-2dea4c15c5de")].QueryType = "CONTAINS";
		queryDictionary[new Guid("629abc1a-725b-4451-9a68-2dea4c15c5de")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("629abc1a-725b-4451-9a68-2dea4c15c5de"))) {queryDictionary[new Guid("629abc1a-725b-4451-9a68-2dea4c15c5de")].SubQueries = subQueryDictionary[new Guid("629abc1a-725b-4451-9a68-2dea4c15c5de")];}
		if(!subQueryDictionary.ContainsKey(new Guid("3e6a7903-d953-49d5-abed-968d88bafc7d"))) {subQueryDictionary[new Guid("3e6a7903-d953-49d5-abed-968d88bafc7d")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("3e6a7903-d953-49d5-abed-968d88bafc7d")].Add(queryDictionary[new Guid("629abc1a-725b-4451-9a68-2dea4c15c5de")]);
		}
		{
		queryDictionary[new Guid("d8378fd6-e083-4073-9cb1-82f360e6d39c")] = new InputRecordListQuery();
		queryDictionary[new Guid("d8378fd6-e083-4073-9cb1-82f360e6d39c")].FieldName = "last_name";
		queryDictionary[new Guid("d8378fd6-e083-4073-9cb1-82f360e6d39c")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"last_name\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("d8378fd6-e083-4073-9cb1-82f360e6d39c")].QueryType = "CONTAINS";
		queryDictionary[new Guid("d8378fd6-e083-4073-9cb1-82f360e6d39c")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("d8378fd6-e083-4073-9cb1-82f360e6d39c"))) {queryDictionary[new Guid("d8378fd6-e083-4073-9cb1-82f360e6d39c")].SubQueries = subQueryDictionary[new Guid("d8378fd6-e083-4073-9cb1-82f360e6d39c")];}
		if(!subQueryDictionary.ContainsKey(new Guid("3e6a7903-d953-49d5-abed-968d88bafc7d"))) {subQueryDictionary[new Guid("3e6a7903-d953-49d5-abed-968d88bafc7d")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("3e6a7903-d953-49d5-abed-968d88bafc7d")].Add(queryDictionary[new Guid("d8378fd6-e083-4073-9cb1-82f360e6d39c")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("3e6a7903-d953-49d5-abed-968d88bafc7d"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("3e6a7903-d953-49d5-abed-968d88bafc7d")];}
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

		#region << sort >>
		{
			var sort = new InputRecordListSort();
			sort.FieldName = "{\"name\":\"url_sort\", \"option\": \"sortBy\", \"default\": \"email\", \"settings\":{\"order\":\"sortOrder\"}}";
			sort.SortType = "Ascending";
			createListInput.Sorts.Add(sort);
		}
		#endregion

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: account Updated list: lookup Message:" + response.Message);
	}
}
#endregion

#region << ***Update list***  Entity: account List Name: general >>
{
	var createListEntity = entMan.ReadEntity(new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "general").Id;
	createListInput.Type =  "General";
	createListInput.Name = "general";
	createListInput.Label = "User accounts";
	createListInput.Title = "User accounts";
	createListInput.Weight = Decimal.Parse("1.0");
	createListInput.Default = true;
	createListInput.System = false;
	createListInput.CssClass = "";
	createListInput.IconName = "user";
	createListInput.VisibleColumnsCount = Int32.Parse("7");
	createListInput.ColumnWidthsCSV = "auto,160px,160px,80px,80px,160px,160px";
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

	}
	#endregion

	#region << Columns >>
	{
	createListInput.Columns = new List<InputRecordListItemBase>();

		#region << email >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140");
			listField.EntityName = "account";
			listField.FieldId = new Guid("7d1eef99-78c6-4d4d-b77b-9ad774218c28");
			listField.FieldName = "email";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << first_name >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140");
			listField.EntityName = "account";
			listField.FieldId = new Guid("c1b45f0e-6326-4294-a68a-59997dea5014");
			listField.FieldName = "first_name";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << last_name >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140");
			listField.EntityName = "account";
			listField.FieldId = new Guid("2dcf9997-62ed-4cb9-9dec-81ffb4c27ca2");
			listField.FieldName = "last_name";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << enabled >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140");
			listField.EntityName = "account";
			listField.FieldId = new Guid("e2265eda-dc45-405e-99b7-767f4201a306");
			listField.FieldName = "enabled";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << verified >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140");
			listField.EntityName = "account";
			listField.FieldId = new Guid("8f273c5c-d032-4621-ae2e-beb7d7f08434");
			listField.FieldName = "verified";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << last_logged_in >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140");
			listField.EntityName = "account";
			listField.FieldId = new Guid("f019a591-69b4-490c-a2df-8f65bad97423");
			listField.FieldName = "last_logged_in";
			listField.Type = "field";
			createListInput.Columns.Add(listField);
		}
		#endregion
		#region << created_on >>
		{
			var listField = new InputRecordListFieldItem();
			listField.EntityId = new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140");
			listField.EntityName = "account";
			listField.FieldId = new Guid("aae6a160-568c-4db8-b6ea-20189717fef5");
			listField.FieldName = "created_on";
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
		queryDictionary[new Guid("3e8d20b3-f23d-4a64-96ee-9885a6d47c08")] = new InputRecordListQuery();
		queryDictionary[new Guid("3e8d20b3-f23d-4a64-96ee-9885a6d47c08")].FieldName = "email";
		queryDictionary[new Guid("3e8d20b3-f23d-4a64-96ee-9885a6d47c08")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"email\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("3e8d20b3-f23d-4a64-96ee-9885a6d47c08")].QueryType = "CONTAINS";
		queryDictionary[new Guid("3e8d20b3-f23d-4a64-96ee-9885a6d47c08")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("3e8d20b3-f23d-4a64-96ee-9885a6d47c08"))) {queryDictionary[new Guid("3e8d20b3-f23d-4a64-96ee-9885a6d47c08")].SubQueries = subQueryDictionary[new Guid("3e8d20b3-f23d-4a64-96ee-9885a6d47c08")];}
		if(!subQueryDictionary.ContainsKey(new Guid("a2ed258c-e964-4d92-a4ce-4c0a54215b22"))) {subQueryDictionary[new Guid("a2ed258c-e964-4d92-a4ce-4c0a54215b22")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("a2ed258c-e964-4d92-a4ce-4c0a54215b22")].Add(queryDictionary[new Guid("3e8d20b3-f23d-4a64-96ee-9885a6d47c08")]);
		}
		{
		queryDictionary[new Guid("2c8a4b7d-e3ba-4789-9a8c-e329820b4bdb")] = new InputRecordListQuery();
		queryDictionary[new Guid("2c8a4b7d-e3ba-4789-9a8c-e329820b4bdb")].FieldName = "first_name";
		queryDictionary[new Guid("2c8a4b7d-e3ba-4789-9a8c-e329820b4bdb")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"first_name\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("2c8a4b7d-e3ba-4789-9a8c-e329820b4bdb")].QueryType = "CONTAINS";
		queryDictionary[new Guid("2c8a4b7d-e3ba-4789-9a8c-e329820b4bdb")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("2c8a4b7d-e3ba-4789-9a8c-e329820b4bdb"))) {queryDictionary[new Guid("2c8a4b7d-e3ba-4789-9a8c-e329820b4bdb")].SubQueries = subQueryDictionary[new Guid("2c8a4b7d-e3ba-4789-9a8c-e329820b4bdb")];}
		if(!subQueryDictionary.ContainsKey(new Guid("a2ed258c-e964-4d92-a4ce-4c0a54215b22"))) {subQueryDictionary[new Guid("a2ed258c-e964-4d92-a4ce-4c0a54215b22")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("a2ed258c-e964-4d92-a4ce-4c0a54215b22")].Add(queryDictionary[new Guid("2c8a4b7d-e3ba-4789-9a8c-e329820b4bdb")]);
		}
		{
		queryDictionary[new Guid("fc3212fc-3b40-460b-9107-18c2749c640c")] = new InputRecordListQuery();
		queryDictionary[new Guid("fc3212fc-3b40-460b-9107-18c2749c640c")].FieldName = "last_name";
		queryDictionary[new Guid("fc3212fc-3b40-460b-9107-18c2749c640c")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"last_name\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("fc3212fc-3b40-460b-9107-18c2749c640c")].QueryType = "CONTAINS";
		queryDictionary[new Guid("fc3212fc-3b40-460b-9107-18c2749c640c")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("fc3212fc-3b40-460b-9107-18c2749c640c"))) {queryDictionary[new Guid("fc3212fc-3b40-460b-9107-18c2749c640c")].SubQueries = subQueryDictionary[new Guid("fc3212fc-3b40-460b-9107-18c2749c640c")];}
		if(!subQueryDictionary.ContainsKey(new Guid("a2ed258c-e964-4d92-a4ce-4c0a54215b22"))) {subQueryDictionary[new Guid("a2ed258c-e964-4d92-a4ce-4c0a54215b22")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("a2ed258c-e964-4d92-a4ce-4c0a54215b22")].Add(queryDictionary[new Guid("fc3212fc-3b40-460b-9107-18c2749c640c")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("a2ed258c-e964-4d92-a4ce-4c0a54215b22"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("a2ed258c-e964-4d92-a4ce-4c0a54215b22")];}
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

		#region << sort >>
		{
			var sort = new InputRecordListSort();
			sort.FieldName = "{\"name\":\"url_sort\", \"option\": \"sortBy\", \"default\": \"email\", \"settings\":{\"order\":\"sortOrder\"}}";
			sort.SortType = "Ascending";
			createListInput.Sorts.Add(sort);
		}
		#endregion

	}
	#endregion

	{
		var response = entMan.UpdateRecordList(new Guid("fdc7893d-6e01-4526-8bc3-2b9c418d8140"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: account Updated list: general Message:" + response.Message);
	}
}
#endregion

#region << View  Entity: customer name: quick_create >>
{
	var createViewEntity = entMan.ReadEntity(new Guid("a74449eb-33c4-4782-84b8-f2707209f6a4")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = new Guid("02aff3c5-a068-4dba-95c0-a0b57e96b3fb");
	createViewInput.Type = "Quick_Create";
	createViewInput.Name = "quick_create";
	createViewInput.Label = "quick_create";
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
			viewRegion.Weight = Decimal.Parse("1.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			#region << Section: section >>
			{
			var viewSection = new InputRecordViewSection();
			viewSection.Id = new Guid("7d0540c5-f6eb-492f-8006-013ebbdd8a60");
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
					viewRow.Id = new Guid("76ef8d4a-5324-4a97-a0a4-0f08ddfc0787");
					viewRow.Weight = Decimal.Parse("1.0");
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("12");
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << field from Relation: label >>
					{
						var viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = new Guid("a9800a74-fc0b-447e-85cf-7ad44d793a19");
						viewItemFromRelation.EntityName = "institution";
						viewItemFromRelation.Type = "fieldFromRelation";
						viewItemFromRelation.FieldId = new Guid("2c34d6b5-0a5c-46e0-9b41-aef3eb960fd3");
						viewItemFromRelation.FieldName = "label";
						viewItemFromRelation.FieldLabel = "Institution";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = "";
						viewItemFromRelation.FieldRequired = false;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = new Guid("e434d6e3-b3b3-411d-98a9-05f8baf298ad");
						viewItemFromRelation.RelationName = "institution_customer";
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion
					#region << field from Relation: name >>
					{
						var viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = new Guid("8aa18a62-9bb0-46b3-af21-f7607d0b4a2f");
						viewItemFromRelation.EntityName = "membership_plan";
						viewItemFromRelation.Type = "fieldFromRelation";
						viewItemFromRelation.FieldId = new Guid("53b3c6cd-ae8e-4c8c-af59-e46b6f755f11");
						viewItemFromRelation.FieldName = "name";
						viewItemFromRelation.FieldLabel = "Membership plan";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = "";
						viewItemFromRelation.FieldRequired = false;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = new Guid("55603da1-6e8b-4ad9-b00e-f7f622df8e90");
						viewItemFromRelation.RelationName = "membership_plan_customer";
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
			actionItem.Template = @"<a class=""btn btn-primary"" ng-click='ngCtrl.create(""default"")' ng-if=""ngCtrl.createViewRegion != null"">Create</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_create_and_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_and_details";
			actionItem.Menu = "create-bottom";
			actionItem.Weight = Decimal.Parse("2.0");
			actionItem.Template = @"<a class=""btn btn-default  btn-outline"" ng-click='ngCtrl.create(""details"")' ng-if=""ngCtrl.createViewRegion != null"">Create & Details</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_create_cancel >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_cancel";
			actionItem.Menu = "create-bottom";
			actionItem.Weight = Decimal.Parse("3.0");
			actionItem.Template = @"<a class=""btn btn-default  btn-outline"" ng-click=""ngCtrl.cancel()"">Cancel</a>";
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
		var response = entMan.CreateRecordView(new Guid("a74449eb-33c4-4782-84b8-f2707209f6a4"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: customer Updated view: quick_create Message:" + response.Message);
	}
}
#endregion

#region << View  Entity: customer name: quick_view >>
{
	var createViewEntity = entMan.ReadEntity(new Guid("a74449eb-33c4-4782-84b8-f2707209f6a4")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = new Guid("cd6dcfab-93c3-4db7-bdae-c0a8e66ceb37");
	createViewInput.Type = "Quick_View";
	createViewInput.Name = "quick_view";
	createViewInput.Label = "quick_view";
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
			viewRegion.Weight = Decimal.Parse("1.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			#region << Section: section >>
			{
			var viewSection = new InputRecordViewSection();
			viewSection.Id = new Guid("7d0540c5-f6eb-492f-8006-013ebbdd8a60");
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
					viewRow.Id = new Guid("76ef8d4a-5324-4a97-a0a4-0f08ddfc0787");
					viewRow.Weight = Decimal.Parse("1.0");
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("12");
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << field from Relation: label >>
					{
						var viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = new Guid("a9800a74-fc0b-447e-85cf-7ad44d793a19");
						viewItemFromRelation.EntityName = "institution";
						viewItemFromRelation.Type = "fieldFromRelation";
						viewItemFromRelation.FieldId = new Guid("2c34d6b5-0a5c-46e0-9b41-aef3eb960fd3");
						viewItemFromRelation.FieldName = "label";
						viewItemFromRelation.FieldLabel = "Institution";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = "";
						viewItemFromRelation.FieldRequired = false;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = new Guid("e434d6e3-b3b3-411d-98a9-05f8baf298ad");
						viewItemFromRelation.RelationName = "institution_customer";
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion
					#region << field from Relation: name >>
					{
						var viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = new Guid("8aa18a62-9bb0-46b3-af21-f7607d0b4a2f");
						viewItemFromRelation.EntityName = "membership_plan";
						viewItemFromRelation.Type = "fieldFromRelation";
						viewItemFromRelation.FieldId = new Guid("53b3c6cd-ae8e-4c8c-af59-e46b6f755f11");
						viewItemFromRelation.FieldName = "name";
						viewItemFromRelation.FieldLabel = "Membership plan";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = "";
						viewItemFromRelation.FieldRequired = false;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = new Guid("55603da1-6e8b-4ad9-b00e-f7f622df8e90");
						viewItemFromRelation.RelationName = "membership_plan_customer";
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
			actionItem.Template = @"<a class=""btn btn-primary"" ng-click='ngCtrl.create(""default"")' ng-if=""ngCtrl.createViewRegion != null"">Create</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_create_and_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_and_details";
			actionItem.Menu = "create-bottom";
			actionItem.Weight = Decimal.Parse("2.0");
			actionItem.Template = @"<a class=""btn btn-default  btn-outline"" ng-click='ngCtrl.create(""details"")' ng-if=""ngCtrl.createViewRegion != null"">Create & Details</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_create_cancel >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_cancel";
			actionItem.Menu = "create-bottom";
			actionItem.Weight = Decimal.Parse("3.0");
			actionItem.Template = @"<a class=""btn btn-default  btn-outline"" ng-click=""ngCtrl.cancel()"">Cancel</a>";
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
		var response = entMan.CreateRecordView(new Guid("a74449eb-33c4-4782-84b8-f2707209f6a4"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: customer Updated view: quick_view Message:" + response.Message);
	}
}
#endregion

#region << View  Entity: customer name: institution_name >>
{
	var createViewEntity = entMan.ReadEntity(new Guid("a74449eb-33c4-4782-84b8-f2707209f6a4")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = new Guid("6a55fc6b-de7f-49b7-b541-e30878e1b5f5");
	createViewInput.Type = "Hidden";
	createViewInput.Name = "institution_name";
	createViewInput.Label = "Institution name";
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
			viewRegion.Weight = Decimal.Parse("1.0");
			viewRegion.CssClass = "";
			viewRegion.Sections = new List<InputRecordViewSection>();

			#region << Section: section >>
			{
			var viewSection = new InputRecordViewSection();
			viewSection.Id = new Guid("157d5233-783d-4f62-85dc-62dcfd3e73c1");
			viewSection.Name = "section";
			viewSection.Label = "Section name";
			viewSection.ShowLabel = false;
			viewSection.CssClass = "no-labels";
			viewSection.Collapsed = false;
			viewSection.TabOrder = "left-right";
			viewSection.Weight = Decimal.Parse("1.0");
			viewSection.Rows = new List<InputRecordViewRow>();

				#region << Row 1>>
				{
					var viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("b535f43f-81a7-4e38-a276-e77040fecb2b");
					viewRow.Weight = Decimal.Parse("1.0");
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("12");
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << field from Relation: label >>
					{
						var viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = new Guid("a9800a74-fc0b-447e-85cf-7ad44d793a19");
						viewItemFromRelation.EntityName = "institution";
						viewItemFromRelation.Type = "fieldFromRelation";
						viewItemFromRelation.FieldId = new Guid("2c34d6b5-0a5c-46e0-9b41-aef3eb960fd3");
						viewItemFromRelation.FieldName = "label";
						viewItemFromRelation.FieldLabel = "Customer Institution";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = "";
						viewItemFromRelation.FieldRequired = false;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = new Guid("e434d6e3-b3b3-411d-98a9-05f8baf298ad");
						viewItemFromRelation.RelationName = "institution_customer";
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
			actionItem.Template = @"<a class=""btn btn-primary"" ng-click='ngCtrl.create(""default"")' ng-if=""ngCtrl.createViewRegion != null"">Create</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_create_and_details >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_and_details";
			actionItem.Menu = "create-bottom";
			actionItem.Weight = Decimal.Parse("2.0");
			actionItem.Template = @"<a class=""btn btn-default  btn-outline"" ng-click='ngCtrl.create(""details"")' ng-if=""ngCtrl.createViewRegion != null"">Create & Details</a>";
			createViewInput.ActionItems.Add(actionItem);
		}
		#endregion

		#region << action item: wv_create_cancel >>
		{
			var actionItem = new ActionItem();
			actionItem.Name = "wv_create_cancel";
			actionItem.Menu = "create-bottom";
			actionItem.Weight = Decimal.Parse("3.0");
			actionItem.Template = @"<a class=""btn btn-default  btn-outline"" ng-click=""ngCtrl.cancel()"">Cancel</a>";
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
		var response = entMan.CreateRecordView(new Guid("a74449eb-33c4-4782-84b8-f2707209f6a4"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: customer Updated view: institution_name Message:" + response.Message);
	}
}
#endregion

#region << List  Entity: customer name: lookup >>
{
	var createListEntity = entMan.ReadEntity(new Guid("a74449eb-33c4-4782-84b8-f2707209f6a4")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = new Guid("d50298d3-2354-4597-86a3-86bc94fce16d");
	createListInput.Type =  "Lookup";
	createListInput.Name = "lookup";
	createListInput.Label = "lookup";
	createListInput.Title = "Customers";
	createListInput.Weight = Decimal.Parse("1.0");
	createListInput.Default = true;
	createListInput.System = false;
	createListInput.CssClass = "";
	createListInput.IconName = string.IsNullOrEmpty("") ? string.Empty : "";
	createListInput.VisibleColumnsCount = Int32.Parse("7");
	createListInput.ColumnWidthsCSV = "auto,auto,140px";
	createListInput.PageSize = Int32.Parse("500");
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

		#region << field from Relation: label >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("a9800a74-fc0b-447e-85cf-7ad44d793a19");
			listItemFromRelation.EntityName = "institution";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("2c34d6b5-0a5c-46e0-9b41-aef3eb960fd3");
			listItemFromRelation.FieldName = "label";
			listItemFromRelation.FieldLabel = "Customer name";
			listItemFromRelation.FieldPlaceholder = "";
			listItemFromRelation.FieldHelpText = "";
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = "";
			listItemFromRelation.RelationId = new Guid("e434d6e3-b3b3-411d-98a9-05f8baf298ad");
			listItemFromRelation.RelationName = "institution_customer";
			createListInput.Columns.Add(listItemFromRelation);
		}
		#endregion
		#region << field from Relation: domain >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("a9800a74-fc0b-447e-85cf-7ad44d793a19");
			listItemFromRelation.EntityName = "institution";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("59f488c5-8246-472b-893e-7cf36ff75d02");
			listItemFromRelation.FieldName = "domain";
			listItemFromRelation.FieldLabel = "Domain";
			listItemFromRelation.FieldPlaceholder = "";
			listItemFromRelation.FieldHelpText = "";
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = "";
			listItemFromRelation.RelationId = new Guid("e434d6e3-b3b3-411d-98a9-05f8baf298ad");
			listItemFromRelation.RelationName = "institution_customer";
			createListInput.Columns.Add(listItemFromRelation);
		}
		#endregion
		#region << field from Relation: name >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("8aa18a62-9bb0-46b3-af21-f7607d0b4a2f");
			listItemFromRelation.EntityName = "membership_plan";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("53b3c6cd-ae8e-4c8c-af59-e46b6f755f11");
			listItemFromRelation.FieldName = "name";
			listItemFromRelation.FieldLabel = "Membership plan";
			listItemFromRelation.FieldPlaceholder = "";
			listItemFromRelation.FieldHelpText = "";
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = "";
			listItemFromRelation.RelationId = new Guid("55603da1-6e8b-4ad9-b00e-f7f622df8e90");
			listItemFromRelation.RelationName = "membership_plan_customer";
			createListInput.Columns.Add(listItemFromRelation);
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
		queryDictionary[new Guid("e1ce4085-5224-4c80-a0c1-33fbebe8e678")] = new InputRecordListQuery();
		queryDictionary[new Guid("e1ce4085-5224-4c80-a0c1-33fbebe8e678")].FieldName = "id";
		queryDictionary[new Guid("e1ce4085-5224-4c80-a0c1-33fbebe8e678")].FieldValue =  "3d203e15-f97f-41f3-bbe6-78350900e8e3";
		queryDictionary[new Guid("e1ce4085-5224-4c80-a0c1-33fbebe8e678")].QueryType = "NOT";
		queryDictionary[new Guid("e1ce4085-5224-4c80-a0c1-33fbebe8e678")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("e1ce4085-5224-4c80-a0c1-33fbebe8e678"))) {queryDictionary[new Guid("e1ce4085-5224-4c80-a0c1-33fbebe8e678")].SubQueries = subQueryDictionary[new Guid("e1ce4085-5224-4c80-a0c1-33fbebe8e678")];}
		if(!subQueryDictionary.ContainsKey(new Guid("b92c1221-779f-4d59-ba61-26c182212a02"))) {subQueryDictionary[new Guid("b92c1221-779f-4d59-ba61-26c182212a02")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("b92c1221-779f-4d59-ba61-26c182212a02")].Add(queryDictionary[new Guid("e1ce4085-5224-4c80-a0c1-33fbebe8e678")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("b92c1221-779f-4d59-ba61-26c182212a02"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("b92c1221-779f-4d59-ba61-26c182212a02")];}
	}
	#endregion

	#region << Sorts >>
	{
	createListInput.Sorts = new List<InputRecordListSort>();

	}
	#endregion

	{
		var response = entMan.CreateRecordList(new Guid("a74449eb-33c4-4782-84b8-f2707209f6a4"), createListInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: customer Created list: lookup Message:" + response.Message);
	}
}
#endregion




		}


    }
}
