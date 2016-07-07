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
		private static void Patch160707(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan, bool createSampleRecords = false)
		{

#region << Create relation: role_n_n_wv_sprint >>
{
	var relation = new EntityRelation();
	var originEntity = entMan.ReadEntity(new Guid("c4541fee-fbb6-4661-929e-1724adec285a")).Object;
	var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
	var targetEntity = entMan.ReadEntity(new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd")).Object;
	var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "id");
	relation.Id = new Guid("e77b7a71-134e-41bf-a079-008e8931303f");
	relation.Name =  "role_n_n_wv_sprint";
	relation.Label = "role_n_n_wv_sprint";
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
			throw new Exception("System error 10060. Relation: role_n_n_wv_sprint Create. Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_project View: admin_create >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "admin_create").Id;
	createViewInput.Type = "Hidden";
	createViewInput.Name = "admin_create";
	createViewInput.Label = "Project create";
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
			viewSection.Id = new Guid("88077697-7a36-4a5a-b021-3d2a8638dade");
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
					viewRow.Id = new Guid("27fb37e4-774a-4b07-a678-a435dcd6be55");
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
					viewRow.Id = new Guid("cc65d58f-0cfd-4f3d-b9c6-3c8faff6c8b4");
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
					viewRow.Id = new Guid("d88b3da7-d501-48ba-b61d-2eb3485bce21");
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
			actionItem.Template = @"<a class=""btn btn-default btn-outline"" ng-click='ngCtrl.create(""details"")' ng-if=""ngCtrl.createViewRegion != null"">Create & Details</a>";
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
	createViewInput.Sidebar.Render = true;
	createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_project Updated view: admin_create Message:" + response.Message);
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
	createListInput.Query = new InputRecordListQuery();
	var queryDictionary = new Dictionary<Guid,InputRecordListQuery>();
	var subQueryDictionary = new Dictionary<Guid,List<InputRecordListQuery>>();
	//Main query rule
	createListInput.Query.FieldName = null;
	createListInput.Query.FieldValue =  null;
	createListInput.Query.QueryType = "AND";
	createListInput.Query.SubQueries = new List<InputRecordListQuery>();
		{
		queryDictionary[new Guid("0137bb5c-b9e6-42eb-a3d3-4b954203205b")] = new InputRecordListQuery();
		queryDictionary[new Guid("0137bb5c-b9e6-42eb-a3d3-4b954203205b")].FieldName = "code";
		queryDictionary[new Guid("0137bb5c-b9e6-42eb-a3d3-4b954203205b")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("0137bb5c-b9e6-42eb-a3d3-4b954203205b")].QueryType = "CONTAINS";
		queryDictionary[new Guid("0137bb5c-b9e6-42eb-a3d3-4b954203205b")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("0137bb5c-b9e6-42eb-a3d3-4b954203205b"))) {queryDictionary[new Guid("0137bb5c-b9e6-42eb-a3d3-4b954203205b")].SubQueries = subQueryDictionary[new Guid("0137bb5c-b9e6-42eb-a3d3-4b954203205b")];}
		if(!subQueryDictionary.ContainsKey(new Guid("48180ab7-1e06-48e9-8aeb-7625e166a945"))) {subQueryDictionary[new Guid("48180ab7-1e06-48e9-8aeb-7625e166a945")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("48180ab7-1e06-48e9-8aeb-7625e166a945")].Add(queryDictionary[new Guid("0137bb5c-b9e6-42eb-a3d3-4b954203205b")]);
		}
		{
		queryDictionary[new Guid("5172023d-a3cb-4945-af23-ff539f40b479")] = new InputRecordListQuery();
		queryDictionary[new Guid("5172023d-a3cb-4945-af23-ff539f40b479")].FieldName = "subject";
		queryDictionary[new Guid("5172023d-a3cb-4945-af23-ff539f40b479")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("5172023d-a3cb-4945-af23-ff539f40b479")].QueryType = "CONTAINS";
		queryDictionary[new Guid("5172023d-a3cb-4945-af23-ff539f40b479")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("5172023d-a3cb-4945-af23-ff539f40b479"))) {queryDictionary[new Guid("5172023d-a3cb-4945-af23-ff539f40b479")].SubQueries = subQueryDictionary[new Guid("5172023d-a3cb-4945-af23-ff539f40b479")];}
		if(!subQueryDictionary.ContainsKey(new Guid("48180ab7-1e06-48e9-8aeb-7625e166a945"))) {subQueryDictionary[new Guid("48180ab7-1e06-48e9-8aeb-7625e166a945")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("48180ab7-1e06-48e9-8aeb-7625e166a945")].Add(queryDictionary[new Guid("5172023d-a3cb-4945-af23-ff539f40b479")]);
		}
		{
		queryDictionary[new Guid("50fd915f-aadf-4f29-9e68-5c60b7ed2f39")] = new InputRecordListQuery();
		queryDictionary[new Guid("50fd915f-aadf-4f29-9e68-5c60b7ed2f39")].FieldName = "status";
		queryDictionary[new Guid("50fd915f-aadf-4f29-9e68-5c60b7ed2f39")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("50fd915f-aadf-4f29-9e68-5c60b7ed2f39")].QueryType = "EQ";
		queryDictionary[new Guid("50fd915f-aadf-4f29-9e68-5c60b7ed2f39")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("50fd915f-aadf-4f29-9e68-5c60b7ed2f39"))) {queryDictionary[new Guid("50fd915f-aadf-4f29-9e68-5c60b7ed2f39")].SubQueries = subQueryDictionary[new Guid("50fd915f-aadf-4f29-9e68-5c60b7ed2f39")];}
		if(!subQueryDictionary.ContainsKey(new Guid("48180ab7-1e06-48e9-8aeb-7625e166a945"))) {subQueryDictionary[new Guid("48180ab7-1e06-48e9-8aeb-7625e166a945")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("48180ab7-1e06-48e9-8aeb-7625e166a945")].Add(queryDictionary[new Guid("50fd915f-aadf-4f29-9e68-5c60b7ed2f39")]);
		}
		{
		queryDictionary[new Guid("98093a42-d6cb-48e1-a2d2-d93f1bb5591e")] = new InputRecordListQuery();
		queryDictionary[new Guid("98093a42-d6cb-48e1-a2d2-d93f1bb5591e")].FieldName = "priority";
		queryDictionary[new Guid("98093a42-d6cb-48e1-a2d2-d93f1bb5591e")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("98093a42-d6cb-48e1-a2d2-d93f1bb5591e")].QueryType = "EQ";
		queryDictionary[new Guid("98093a42-d6cb-48e1-a2d2-d93f1bb5591e")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("98093a42-d6cb-48e1-a2d2-d93f1bb5591e"))) {queryDictionary[new Guid("98093a42-d6cb-48e1-a2d2-d93f1bb5591e")].SubQueries = subQueryDictionary[new Guid("98093a42-d6cb-48e1-a2d2-d93f1bb5591e")];}
		if(!subQueryDictionary.ContainsKey(new Guid("48180ab7-1e06-48e9-8aeb-7625e166a945"))) {subQueryDictionary[new Guid("48180ab7-1e06-48e9-8aeb-7625e166a945")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("48180ab7-1e06-48e9-8aeb-7625e166a945")].Add(queryDictionary[new Guid("98093a42-d6cb-48e1-a2d2-d93f1bb5591e")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("48180ab7-1e06-48e9-8aeb-7625e166a945"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("48180ab7-1e06-48e9-8aeb-7625e166a945")];}
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

#region << Update  Entity: wv_sprint field: start_date >>
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

#region << Update  Entity: wv_sprint View: general >>
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
		var response = entMan.UpdateRecordView(new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_sprint Updated view: general Message:" + response.Message);
	}
}
#endregion

#region << Update  Entity: wv_sprint View: create >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = updateViewEntity.RecordViews.SingleOrDefault(x => x.Name == "create").Id;
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
		var response = entMan.UpdateRecordView(new Guid("a8de737a-9610-424d-9cf1-1b86e6cd17cd"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_sprint Updated view: create Message:" + response.Message);
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
		queryDictionary[new Guid("8a8380ab-b6eb-4d55-bf94-a7be70e8d37e")] = new InputRecordListQuery();
		queryDictionary[new Guid("8a8380ab-b6eb-4d55-bf94-a7be70e8d37e")].FieldName = "name";
		queryDictionary[new Guid("8a8380ab-b6eb-4d55-bf94-a7be70e8d37e")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"name\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("8a8380ab-b6eb-4d55-bf94-a7be70e8d37e")].QueryType = "CONTAINS";
		queryDictionary[new Guid("8a8380ab-b6eb-4d55-bf94-a7be70e8d37e")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("8a8380ab-b6eb-4d55-bf94-a7be70e8d37e"))) {queryDictionary[new Guid("8a8380ab-b6eb-4d55-bf94-a7be70e8d37e")].SubQueries = subQueryDictionary[new Guid("8a8380ab-b6eb-4d55-bf94-a7be70e8d37e")];}
		if(!subQueryDictionary.ContainsKey(new Guid("da2c6f8f-75ad-4ba5-847b-85f7ceecebd3"))) {subQueryDictionary[new Guid("da2c6f8f-75ad-4ba5-847b-85f7ceecebd3")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("da2c6f8f-75ad-4ba5-847b-85f7ceecebd3")].Add(queryDictionary[new Guid("8a8380ab-b6eb-4d55-bf94-a7be70e8d37e")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("da2c6f8f-75ad-4ba5-847b-85f7ceecebd3"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("da2c6f8f-75ad-4ba5-847b-85f7ceecebd3")];}
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


		}


    }
}
