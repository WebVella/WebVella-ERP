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
		private static void Patch170119(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan, bool createSampleRecords = false)
		{

#region << ***Update area***  Area name: create_task >>
{
	var patchObject = new EntityRecord();
	patchObject["id"] = new Guid("aacf2d40-9b03-43d5-aba8-8c8cb3a0133e");
	patchObject["attachments"] = "[{\"name\":null,\"label\":\"Create task\",\"labelPlural\":null,\"iconName\":\"tasks\",\"weight\":1,\"url\":\"/#/areas/projects/wv_task/view-create/create?returnUrl=%2Fareas%2Fprojects%2Fwv_project%2Fdashboard\",\"view\":null,\"create\":null,\"list\":null}]";
	var updateAreaResult = recMan.UpdateRecord("area", patchObject);
	if (!updateAreaResult.Success)
	{
		throw new Exception("System error 10060. Area update with name : create_task. Message:" + updateAreaResult.Message);
	}
}
#endregion

#region << ***Update area***  Area name: report_bug >>
{
	var patchObject = new EntityRecord();
	patchObject["id"] = new Guid("c57fc703-4546-4bfa-9448-29151e21d6ae");
	patchObject["attachments"] = "[{\"name\":null,\"label\":\"Report bug\",\"labelPlural\":null,\"iconName\":\"bug\",\"weight\":1,\"url\":\"/#/areas/projects/wv_bug/view-create/create?returnUrl=%2Fareas%2Fprojects%2Fwv_project%2Fdashboard\",\"view\":null,\"create\":null,\"list\":null}]";
	var updateAreaResult = recMan.UpdateRecord("area", patchObject);
	if (!updateAreaResult.Success)
	{
		throw new Exception("System error 10060. Area update with name : report_bug. Message:" + updateAreaResult.Message);
	}
}
#endregion

#region << ***Update area***  Area name: projects >>
{
	var patchObject = new EntityRecord();
	patchObject["id"] = new Guid("205877a1-242c-41bf-a080-49ea01d4f519");
	patchObject["attachments"] = "[{\"name\":null,\"label\":\"My Dashboard\",\"labelPlural\":null,\"iconName\":\"tachometer\",\"weight\":1,\"url\":\"/#/areas/projects/wv_project/dashboard\",\"view\":null,\"create\":null,\"list\":null},{\"name\":\"wv_task\",\"label\":\"Task\",\"labelPlural\":\"Tasks\",\"iconName\":\"tasks\",\"weight\":4,\"url\":null,\"view\":{\"name\":\"general\",\"label\":\"Details\"},\"create\":{\"name\":\"create\",\"label\":\"Create\"},\"list\":{\"name\":\"my_tasks\",\"label\":\"My open tasks\"}},{\"name\":\"wv_bug\",\"label\":\"Bug\",\"labelPlural\":\"Bugs\",\"iconName\":\"bug\",\"weight\":5,\"url\":null,\"view\":{\"name\":\"general\",\"label\":\"Details\"},\"create\":{\"name\":\"create\",\"label\":\"Create\"},\"list\":{\"name\":\"my_bugs\",\"label\":\"My open bugs\"}},{\"name\":\"wv_project\",\"label\":\"Project\",\"labelPlural\":\"Projects\",\"iconName\":\"product-hunt\",\"weight\":22,\"url\":null,\"view\":{\"name\":\"dashboard\",\"label\":\"Dashboard\"},\"create\":{\"name\":\"create\",\"label\":\"Create\"},\"list\":{\"name\":\"my_projects\",\"label\":\"My Projects\"}},{\"name\":null,\"label\":\"My Sprints\",\"url\":\"/#/areas/projects/wv_project/sprints\",\"labelPlural\":null,\"iconName\":\"fast-forward\",\"weight\":\"50\",\"view\":null,\"list\":null,\"create\":null}]";
	var updateAreaResult = recMan.UpdateRecord("area", patchObject);
	if (!updateAreaResult.Success)
	{
		throw new Exception("System error 10060. Area update with name : projects. Message:" + updateAreaResult.Message);
	}
}
#endregion

#region << ***Update area***  Area name: project_admin >>
{
	var patchObject = new EntityRecord();
	patchObject["id"] = new Guid("5b131255-46fc-459d-bbb5-923a4bdfc006");
	patchObject["attachments"] = "[{\"name\":\"wv_project\",\"label\":\"Project\",\"labelPlural\":\"Projects\",\"iconName\":\"product-hunt\",\"weight\":22,\"url\":null,\"view\":{\"name\":\"admin_details\",\"label\":\"Project details\"},\"create\":{\"name\":\"admin_create\",\"label\":\"Project create\"},\"list\":{\"name\":\"admin\",\"label\":\"All Projects\"}},{\"name\":\"wv_sprint\",\"label\":\"Sprint\",\"url\":null,\"labelPlural\":\"Sprints\",\"iconName\":\"fast-forward\",\"weight\":100,\"view\":{\"name\":\"general\",\"label\":\"General\"},\"create\":{\"name\":\"create\",\"label\":\"create\"},\"list\":{\"name\":\"general\",\"label\":\"All Sprints\"}}]";
	var updateAreaResult = recMan.UpdateRecord("area", patchObject);
	if (!updateAreaResult.Success)
	{
		throw new Exception("System error 10060. Area update with name : project_admin. Message:" + updateAreaResult.Message);
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
	{
		var response = entMan.UpdateField(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89"), datetimeField);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_task Field: created_on Message:" + response.Message);
	}
}
#endregion

#region << ***Update field***  Entity: wv_task Field Name: created_by >>
{
	var currentEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
	InputGuidField guidField = new InputGuidField();
	guidField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "created_by").Id;
	guidField.Name = "created_by";
	guidField.Label = "Created By";
	guidField.PlaceholderText = "";
	guidField.Description = "";
	guidField.HelpText = "";
	guidField.Required = false;
	guidField.Unique = false;
	guidField.Searchable = false;
	guidField.Auditable = false;
	guidField.System = true;
	guidField.DefaultValue = null;
	guidField.GenerateNewId = false;
	guidField.EnableSecurity = true;
	guidField.Permissions = new FieldPermissions();
	guidField.Permissions.CanRead = new List<Guid>();
	guidField.Permissions.CanUpdate = new List<Guid>();
	//READ
	guidField.Permissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
	guidField.Permissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
	//UPDATE
	{
		var response = entMan.UpdateField(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89"), guidField);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_task Field: created_by Message:" + response.Message);
	}
}
#endregion

#region << ***Update view***  Entity: wv_task View Name: create >>
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
					viewRow.Id = new Guid("4bb8fccc-30d4-4aee-92c0-6c6ea7b4fb9d");
					viewRow.Weight = Decimal.Parse("1.0");
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("6");
					viewColumn.Items = new List<InputRecordViewItemBase>();

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
					//Save column
					viewRow.Columns.Add(viewColumn);
					}
					#endregion
					#region << Column 2 >>
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
						viewItemFromRelation.FieldLabel = "Owner (blank for default)";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = "";
						viewItemFromRelation.FieldRequired = false;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = new Guid("7ce76c81-e604-401e-907f-23de982b930e");
						viewItemFromRelation.RelationName = "user_1_n_task_owner";
						viewColumn.Items.Add(viewItemFromRelation);
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
					viewRow.Id = new Guid("818f516c-f6c2-4073-8574-75c13a72aee4");
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

#region << ***Update view***  Entity: wv_task View Name: general >>
{
	var updateViewEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
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
					#region << List from relation: task_comments >>
					{
						var viewItemFromRelation = new InputRecordViewRelationListItem();
						viewItemFromRelation.EntityId = new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99");
						viewItemFromRelation.EntityName = "wv_project_comment";
						viewItemFromRelation.ListId = new Guid("b8a7a81d-9176-47e6-90c5-3cabc2a4ceff");
						viewItemFromRelation.ListName = "task_comments";
						viewItemFromRelation.FieldLabel = "Comments";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = "";
						viewItemFromRelation.FieldRequired = false;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = new Guid("884b9480-dc1c-468a-98f0-2d5f10084622");
						viewItemFromRelation.RelationName = "task_1_n_comment";
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
					#region << created_on >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
						viewItem.EntityName = "wv_task";
						viewItem.FieldId = new Guid("6a83a91e-7eb8-4fb4-ab5f-5750cb4015d3");
						viewItem.FieldName = "created_on";
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
						viewItemFromRelation.FieldLabel = "Created by";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = "";
						viewItemFromRelation.FieldRequired = false;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = new Guid("0affc050-2c24-4ae3-bb5e-b08139661d83");
						viewItemFromRelation.RelationName = "user_wv_task_created_by";
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
			#region << Section: hidden >>
			{
			var viewSection = new InputRecordViewSection();
			viewSection.Id = new Guid("23557322-9c2c-4824-b817-9d4d5bc0e83b");
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
					viewRow.Id = new Guid("19c54fd0-b594-4e96-a727-adac8518fd38");
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
						viewItem.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
						viewItem.EntityName = "wv_task";
						viewItem.FieldId = new Guid("04f31ad8-4583-4237-8d54-f82c3f44b918");
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
			viewItemFromRelation.EntityId = new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73");
			viewItemFromRelation.EntityName = "wv_project_attachment";
			viewItemFromRelation.ListId = new Guid("6fc374ac-ba6b-4009-ade4-988304071f29");
			viewItemFromRelation.ListName ="task_attachments";
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

		#region << list from relation: task_timelogs >>
		{
			var viewItemFromRelation = new InputRecordViewSidebarRelationListItem();
			viewItemFromRelation.EntityId = new Guid("e2db7515-721f-446e-8333-6149b1ba131b");
			viewItemFromRelation.EntityName = "wv_timelog";
			viewItemFromRelation.ListId = new Guid("c105b3f8-e140-4150-a587-a31cf600d99b");
			viewItemFromRelation.ListName ="task_timelogs";
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

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_task Updated view: general Message:" + response.Message);
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
	createListInput.ColumnWidthsCSV = "100px,auto,30px,120px,120px,120px,120px";
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
		queryDictionary[new Guid("8b8ccaf4-b106-460d-ad50-8ed9877dbbb6")] = new InputRecordListQuery();
		queryDictionary[new Guid("8b8ccaf4-b106-460d-ad50-8ed9877dbbb6")].FieldName = "code";
		queryDictionary[new Guid("8b8ccaf4-b106-460d-ad50-8ed9877dbbb6")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("8b8ccaf4-b106-460d-ad50-8ed9877dbbb6")].QueryType = "CONTAINS";
		queryDictionary[new Guid("8b8ccaf4-b106-460d-ad50-8ed9877dbbb6")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("8b8ccaf4-b106-460d-ad50-8ed9877dbbb6"))) {queryDictionary[new Guid("8b8ccaf4-b106-460d-ad50-8ed9877dbbb6")].SubQueries = subQueryDictionary[new Guid("8b8ccaf4-b106-460d-ad50-8ed9877dbbb6")];}
		if(!subQueryDictionary.ContainsKey(new Guid("ee97734d-e43f-48ec-8d90-29465e076f6e"))) {subQueryDictionary[new Guid("ee97734d-e43f-48ec-8d90-29465e076f6e")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("ee97734d-e43f-48ec-8d90-29465e076f6e")].Add(queryDictionary[new Guid("8b8ccaf4-b106-460d-ad50-8ed9877dbbb6")]);
		}
		{
		queryDictionary[new Guid("dd677a72-cd84-4d81-9246-6a7bca1ffea8")] = new InputRecordListQuery();
		queryDictionary[new Guid("dd677a72-cd84-4d81-9246-6a7bca1ffea8")].FieldName = "subject";
		queryDictionary[new Guid("dd677a72-cd84-4d81-9246-6a7bca1ffea8")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("dd677a72-cd84-4d81-9246-6a7bca1ffea8")].QueryType = "CONTAINS";
		queryDictionary[new Guid("dd677a72-cd84-4d81-9246-6a7bca1ffea8")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("dd677a72-cd84-4d81-9246-6a7bca1ffea8"))) {queryDictionary[new Guid("dd677a72-cd84-4d81-9246-6a7bca1ffea8")].SubQueries = subQueryDictionary[new Guid("dd677a72-cd84-4d81-9246-6a7bca1ffea8")];}
		if(!subQueryDictionary.ContainsKey(new Guid("ee97734d-e43f-48ec-8d90-29465e076f6e"))) {subQueryDictionary[new Guid("ee97734d-e43f-48ec-8d90-29465e076f6e")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("ee97734d-e43f-48ec-8d90-29465e076f6e")].Add(queryDictionary[new Guid("dd677a72-cd84-4d81-9246-6a7bca1ffea8")]);
		}
		{
		queryDictionary[new Guid("55067cb4-74e5-4277-8a7d-aefbf333d6dc")] = new InputRecordListQuery();
		queryDictionary[new Guid("55067cb4-74e5-4277-8a7d-aefbf333d6dc")].FieldName = "status";
		queryDictionary[new Guid("55067cb4-74e5-4277-8a7d-aefbf333d6dc")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("55067cb4-74e5-4277-8a7d-aefbf333d6dc")].QueryType = "EQ";
		queryDictionary[new Guid("55067cb4-74e5-4277-8a7d-aefbf333d6dc")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("55067cb4-74e5-4277-8a7d-aefbf333d6dc"))) {queryDictionary[new Guid("55067cb4-74e5-4277-8a7d-aefbf333d6dc")].SubQueries = subQueryDictionary[new Guid("55067cb4-74e5-4277-8a7d-aefbf333d6dc")];}
		if(!subQueryDictionary.ContainsKey(new Guid("ee97734d-e43f-48ec-8d90-29465e076f6e"))) {subQueryDictionary[new Guid("ee97734d-e43f-48ec-8d90-29465e076f6e")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("ee97734d-e43f-48ec-8d90-29465e076f6e")].Add(queryDictionary[new Guid("55067cb4-74e5-4277-8a7d-aefbf333d6dc")]);
		}
		{
		queryDictionary[new Guid("e08946fd-633f-4895-82db-dea31df348b6")] = new InputRecordListQuery();
		queryDictionary[new Guid("e08946fd-633f-4895-82db-dea31df348b6")].FieldName = "priority";
		queryDictionary[new Guid("e08946fd-633f-4895-82db-dea31df348b6")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("e08946fd-633f-4895-82db-dea31df348b6")].QueryType = "EQ";
		queryDictionary[new Guid("e08946fd-633f-4895-82db-dea31df348b6")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("e08946fd-633f-4895-82db-dea31df348b6"))) {queryDictionary[new Guid("e08946fd-633f-4895-82db-dea31df348b6")].SubQueries = subQueryDictionary[new Guid("e08946fd-633f-4895-82db-dea31df348b6")];}
		if(!subQueryDictionary.ContainsKey(new Guid("ee97734d-e43f-48ec-8d90-29465e076f6e"))) {subQueryDictionary[new Guid("ee97734d-e43f-48ec-8d90-29465e076f6e")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("ee97734d-e43f-48ec-8d90-29465e076f6e")].Add(queryDictionary[new Guid("e08946fd-633f-4895-82db-dea31df348b6")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("ee97734d-e43f-48ec-8d90-29465e076f6e"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("ee97734d-e43f-48ec-8d90-29465e076f6e")];}
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
	createListInput.ColumnWidthsCSV = "100px,auto,30px,120px,120px,120px,120px";
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
		queryDictionary[new Guid("52dc0c30-b972-4988-b31e-ff9631eaf99e")] = new InputRecordListQuery();
		queryDictionary[new Guid("52dc0c30-b972-4988-b31e-ff9631eaf99e")].FieldName = null;
		queryDictionary[new Guid("52dc0c30-b972-4988-b31e-ff9631eaf99e")].FieldValue =  null;
		queryDictionary[new Guid("52dc0c30-b972-4988-b31e-ff9631eaf99e")].QueryType = "AND";
		queryDictionary[new Guid("52dc0c30-b972-4988-b31e-ff9631eaf99e")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("707da7e9-6d53-4ee4-a386-067af27937fd")] = new InputRecordListQuery();
			queryDictionary[new Guid("707da7e9-6d53-4ee4-a386-067af27937fd")].FieldName = "created_by";
			queryDictionary[new Guid("707da7e9-6d53-4ee4-a386-067af27937fd")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("707da7e9-6d53-4ee4-a386-067af27937fd")].QueryType = "EQ";
			queryDictionary[new Guid("707da7e9-6d53-4ee4-a386-067af27937fd")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("707da7e9-6d53-4ee4-a386-067af27937fd"))) {queryDictionary[new Guid("707da7e9-6d53-4ee4-a386-067af27937fd")].SubQueries = subQueryDictionary[new Guid("707da7e9-6d53-4ee4-a386-067af27937fd")];}
			if(!subQueryDictionary.ContainsKey(new Guid("52dc0c30-b972-4988-b31e-ff9631eaf99e"))) {subQueryDictionary[new Guid("52dc0c30-b972-4988-b31e-ff9631eaf99e")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("52dc0c30-b972-4988-b31e-ff9631eaf99e")].Add(queryDictionary[new Guid("707da7e9-6d53-4ee4-a386-067af27937fd")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("52dc0c30-b972-4988-b31e-ff9631eaf99e"))) {queryDictionary[new Guid("52dc0c30-b972-4988-b31e-ff9631eaf99e")].SubQueries = subQueryDictionary[new Guid("52dc0c30-b972-4988-b31e-ff9631eaf99e")];}
		if(!subQueryDictionary.ContainsKey(new Guid("8024b52f-c24e-46af-85f6-60b649f82412"))) {subQueryDictionary[new Guid("8024b52f-c24e-46af-85f6-60b649f82412")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("8024b52f-c24e-46af-85f6-60b649f82412")].Add(queryDictionary[new Guid("52dc0c30-b972-4988-b31e-ff9631eaf99e")]);
		}
		{
		queryDictionary[new Guid("cd843bc0-b264-4718-965f-8ecbfaf8ce43")] = new InputRecordListQuery();
		queryDictionary[new Guid("cd843bc0-b264-4718-965f-8ecbfaf8ce43")].FieldName = null;
		queryDictionary[new Guid("cd843bc0-b264-4718-965f-8ecbfaf8ce43")].FieldValue =  null;
		queryDictionary[new Guid("cd843bc0-b264-4718-965f-8ecbfaf8ce43")].QueryType = "AND";
		queryDictionary[new Guid("cd843bc0-b264-4718-965f-8ecbfaf8ce43")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("94514a6c-fc79-4156-aff7-40a5dc760175")] = new InputRecordListQuery();
			queryDictionary[new Guid("94514a6c-fc79-4156-aff7-40a5dc760175")].FieldName = "code";
			queryDictionary[new Guid("94514a6c-fc79-4156-aff7-40a5dc760175")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("94514a6c-fc79-4156-aff7-40a5dc760175")].QueryType = "CONTAINS";
			queryDictionary[new Guid("94514a6c-fc79-4156-aff7-40a5dc760175")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("94514a6c-fc79-4156-aff7-40a5dc760175"))) {queryDictionary[new Guid("94514a6c-fc79-4156-aff7-40a5dc760175")].SubQueries = subQueryDictionary[new Guid("94514a6c-fc79-4156-aff7-40a5dc760175")];}
			if(!subQueryDictionary.ContainsKey(new Guid("cd843bc0-b264-4718-965f-8ecbfaf8ce43"))) {subQueryDictionary[new Guid("cd843bc0-b264-4718-965f-8ecbfaf8ce43")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("cd843bc0-b264-4718-965f-8ecbfaf8ce43")].Add(queryDictionary[new Guid("94514a6c-fc79-4156-aff7-40a5dc760175")]);
			}
			{
			queryDictionary[new Guid("998a7d39-e7d3-4649-93e2-f32f6cfae6dc")] = new InputRecordListQuery();
			queryDictionary[new Guid("998a7d39-e7d3-4649-93e2-f32f6cfae6dc")].FieldName = "subject";
			queryDictionary[new Guid("998a7d39-e7d3-4649-93e2-f32f6cfae6dc")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("998a7d39-e7d3-4649-93e2-f32f6cfae6dc")].QueryType = "CONTAINS";
			queryDictionary[new Guid("998a7d39-e7d3-4649-93e2-f32f6cfae6dc")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("998a7d39-e7d3-4649-93e2-f32f6cfae6dc"))) {queryDictionary[new Guid("998a7d39-e7d3-4649-93e2-f32f6cfae6dc")].SubQueries = subQueryDictionary[new Guid("998a7d39-e7d3-4649-93e2-f32f6cfae6dc")];}
			if(!subQueryDictionary.ContainsKey(new Guid("cd843bc0-b264-4718-965f-8ecbfaf8ce43"))) {subQueryDictionary[new Guid("cd843bc0-b264-4718-965f-8ecbfaf8ce43")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("cd843bc0-b264-4718-965f-8ecbfaf8ce43")].Add(queryDictionary[new Guid("998a7d39-e7d3-4649-93e2-f32f6cfae6dc")]);
			}
			{
			queryDictionary[new Guid("ba7709e0-1755-4504-8f68-5b5d40c19407")] = new InputRecordListQuery();
			queryDictionary[new Guid("ba7709e0-1755-4504-8f68-5b5d40c19407")].FieldName = "status";
			queryDictionary[new Guid("ba7709e0-1755-4504-8f68-5b5d40c19407")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("ba7709e0-1755-4504-8f68-5b5d40c19407")].QueryType = "EQ";
			queryDictionary[new Guid("ba7709e0-1755-4504-8f68-5b5d40c19407")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("ba7709e0-1755-4504-8f68-5b5d40c19407"))) {queryDictionary[new Guid("ba7709e0-1755-4504-8f68-5b5d40c19407")].SubQueries = subQueryDictionary[new Guid("ba7709e0-1755-4504-8f68-5b5d40c19407")];}
			if(!subQueryDictionary.ContainsKey(new Guid("cd843bc0-b264-4718-965f-8ecbfaf8ce43"))) {subQueryDictionary[new Guid("cd843bc0-b264-4718-965f-8ecbfaf8ce43")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("cd843bc0-b264-4718-965f-8ecbfaf8ce43")].Add(queryDictionary[new Guid("ba7709e0-1755-4504-8f68-5b5d40c19407")]);
			}
			{
			queryDictionary[new Guid("cccafa80-85d7-4cdc-b0c1-045702e06d08")] = new InputRecordListQuery();
			queryDictionary[new Guid("cccafa80-85d7-4cdc-b0c1-045702e06d08")].FieldName = "priority";
			queryDictionary[new Guid("cccafa80-85d7-4cdc-b0c1-045702e06d08")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("cccafa80-85d7-4cdc-b0c1-045702e06d08")].QueryType = "EQ";
			queryDictionary[new Guid("cccafa80-85d7-4cdc-b0c1-045702e06d08")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("cccafa80-85d7-4cdc-b0c1-045702e06d08"))) {queryDictionary[new Guid("cccafa80-85d7-4cdc-b0c1-045702e06d08")].SubQueries = subQueryDictionary[new Guid("cccafa80-85d7-4cdc-b0c1-045702e06d08")];}
			if(!subQueryDictionary.ContainsKey(new Guid("cd843bc0-b264-4718-965f-8ecbfaf8ce43"))) {subQueryDictionary[new Guid("cd843bc0-b264-4718-965f-8ecbfaf8ce43")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("cd843bc0-b264-4718-965f-8ecbfaf8ce43")].Add(queryDictionary[new Guid("cccafa80-85d7-4cdc-b0c1-045702e06d08")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("cd843bc0-b264-4718-965f-8ecbfaf8ce43"))) {queryDictionary[new Guid("cd843bc0-b264-4718-965f-8ecbfaf8ce43")].SubQueries = subQueryDictionary[new Guid("cd843bc0-b264-4718-965f-8ecbfaf8ce43")];}
		if(!subQueryDictionary.ContainsKey(new Guid("8024b52f-c24e-46af-85f6-60b649f82412"))) {subQueryDictionary[new Guid("8024b52f-c24e-46af-85f6-60b649f82412")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("8024b52f-c24e-46af-85f6-60b649f82412")].Add(queryDictionary[new Guid("cd843bc0-b264-4718-965f-8ecbfaf8ce43")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("8024b52f-c24e-46af-85f6-60b649f82412"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("8024b52f-c24e-46af-85f6-60b649f82412")];}
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
	createListInput.ColumnWidthsCSV = "100px,auto,30px,120px,120px,120px,120px";
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
		queryDictionary[new Guid("3b4e4055-18f7-4db6-9934-592a311452a8")] = new InputRecordListQuery();
		queryDictionary[new Guid("3b4e4055-18f7-4db6-9934-592a311452a8")].FieldName = null;
		queryDictionary[new Guid("3b4e4055-18f7-4db6-9934-592a311452a8")].FieldValue =  null;
		queryDictionary[new Guid("3b4e4055-18f7-4db6-9934-592a311452a8")].QueryType = "AND";
		queryDictionary[new Guid("3b4e4055-18f7-4db6-9934-592a311452a8")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("594551e9-efbe-432d-86fd-b730b18f38cd")] = new InputRecordListQuery();
			queryDictionary[new Guid("594551e9-efbe-432d-86fd-b730b18f38cd")].FieldName = "owner_id";
			queryDictionary[new Guid("594551e9-efbe-432d-86fd-b730b18f38cd")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("594551e9-efbe-432d-86fd-b730b18f38cd")].QueryType = "EQ";
			queryDictionary[new Guid("594551e9-efbe-432d-86fd-b730b18f38cd")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("594551e9-efbe-432d-86fd-b730b18f38cd"))) {queryDictionary[new Guid("594551e9-efbe-432d-86fd-b730b18f38cd")].SubQueries = subQueryDictionary[new Guid("594551e9-efbe-432d-86fd-b730b18f38cd")];}
			if(!subQueryDictionary.ContainsKey(new Guid("3b4e4055-18f7-4db6-9934-592a311452a8"))) {subQueryDictionary[new Guid("3b4e4055-18f7-4db6-9934-592a311452a8")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("3b4e4055-18f7-4db6-9934-592a311452a8")].Add(queryDictionary[new Guid("594551e9-efbe-432d-86fd-b730b18f38cd")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("3b4e4055-18f7-4db6-9934-592a311452a8"))) {queryDictionary[new Guid("3b4e4055-18f7-4db6-9934-592a311452a8")].SubQueries = subQueryDictionary[new Guid("3b4e4055-18f7-4db6-9934-592a311452a8")];}
		if(!subQueryDictionary.ContainsKey(new Guid("6cfedaf0-f198-4dc4-a298-2ae69871057d"))) {subQueryDictionary[new Guid("6cfedaf0-f198-4dc4-a298-2ae69871057d")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("6cfedaf0-f198-4dc4-a298-2ae69871057d")].Add(queryDictionary[new Guid("3b4e4055-18f7-4db6-9934-592a311452a8")]);
		}
		{
		queryDictionary[new Guid("00e04ca5-bdc8-4e55-9efa-da895e7ece2f")] = new InputRecordListQuery();
		queryDictionary[new Guid("00e04ca5-bdc8-4e55-9efa-da895e7ece2f")].FieldName = null;
		queryDictionary[new Guid("00e04ca5-bdc8-4e55-9efa-da895e7ece2f")].FieldValue =  null;
		queryDictionary[new Guid("00e04ca5-bdc8-4e55-9efa-da895e7ece2f")].QueryType = "AND";
		queryDictionary[new Guid("00e04ca5-bdc8-4e55-9efa-da895e7ece2f")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("3f0d6681-09b3-47c0-97c5-822dd7d98cac")] = new InputRecordListQuery();
			queryDictionary[new Guid("3f0d6681-09b3-47c0-97c5-822dd7d98cac")].FieldName = "code";
			queryDictionary[new Guid("3f0d6681-09b3-47c0-97c5-822dd7d98cac")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("3f0d6681-09b3-47c0-97c5-822dd7d98cac")].QueryType = "CONTAINS";
			queryDictionary[new Guid("3f0d6681-09b3-47c0-97c5-822dd7d98cac")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("3f0d6681-09b3-47c0-97c5-822dd7d98cac"))) {queryDictionary[new Guid("3f0d6681-09b3-47c0-97c5-822dd7d98cac")].SubQueries = subQueryDictionary[new Guid("3f0d6681-09b3-47c0-97c5-822dd7d98cac")];}
			if(!subQueryDictionary.ContainsKey(new Guid("00e04ca5-bdc8-4e55-9efa-da895e7ece2f"))) {subQueryDictionary[new Guid("00e04ca5-bdc8-4e55-9efa-da895e7ece2f")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("00e04ca5-bdc8-4e55-9efa-da895e7ece2f")].Add(queryDictionary[new Guid("3f0d6681-09b3-47c0-97c5-822dd7d98cac")]);
			}
			{
			queryDictionary[new Guid("6ad699be-93d0-4594-8a3f-188ccd6828e3")] = new InputRecordListQuery();
			queryDictionary[new Guid("6ad699be-93d0-4594-8a3f-188ccd6828e3")].FieldName = "subject";
			queryDictionary[new Guid("6ad699be-93d0-4594-8a3f-188ccd6828e3")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("6ad699be-93d0-4594-8a3f-188ccd6828e3")].QueryType = "CONTAINS";
			queryDictionary[new Guid("6ad699be-93d0-4594-8a3f-188ccd6828e3")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("6ad699be-93d0-4594-8a3f-188ccd6828e3"))) {queryDictionary[new Guid("6ad699be-93d0-4594-8a3f-188ccd6828e3")].SubQueries = subQueryDictionary[new Guid("6ad699be-93d0-4594-8a3f-188ccd6828e3")];}
			if(!subQueryDictionary.ContainsKey(new Guid("00e04ca5-bdc8-4e55-9efa-da895e7ece2f"))) {subQueryDictionary[new Guid("00e04ca5-bdc8-4e55-9efa-da895e7ece2f")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("00e04ca5-bdc8-4e55-9efa-da895e7ece2f")].Add(queryDictionary[new Guid("6ad699be-93d0-4594-8a3f-188ccd6828e3")]);
			}
			{
			queryDictionary[new Guid("5389c568-3557-4038-8107-1492536150ca")] = new InputRecordListQuery();
			queryDictionary[new Guid("5389c568-3557-4038-8107-1492536150ca")].FieldName = "status";
			queryDictionary[new Guid("5389c568-3557-4038-8107-1492536150ca")].FieldValue =  "completed";
			queryDictionary[new Guid("5389c568-3557-4038-8107-1492536150ca")].QueryType = "NOT";
			queryDictionary[new Guid("5389c568-3557-4038-8107-1492536150ca")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("5389c568-3557-4038-8107-1492536150ca"))) {queryDictionary[new Guid("5389c568-3557-4038-8107-1492536150ca")].SubQueries = subQueryDictionary[new Guid("5389c568-3557-4038-8107-1492536150ca")];}
			if(!subQueryDictionary.ContainsKey(new Guid("00e04ca5-bdc8-4e55-9efa-da895e7ece2f"))) {subQueryDictionary[new Guid("00e04ca5-bdc8-4e55-9efa-da895e7ece2f")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("00e04ca5-bdc8-4e55-9efa-da895e7ece2f")].Add(queryDictionary[new Guid("5389c568-3557-4038-8107-1492536150ca")]);
			}
			{
			queryDictionary[new Guid("a9b0b5a8-ab9a-47b4-8361-0ce9d66accb3")] = new InputRecordListQuery();
			queryDictionary[new Guid("a9b0b5a8-ab9a-47b4-8361-0ce9d66accb3")].FieldName = "priority";
			queryDictionary[new Guid("a9b0b5a8-ab9a-47b4-8361-0ce9d66accb3")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("a9b0b5a8-ab9a-47b4-8361-0ce9d66accb3")].QueryType = "EQ";
			queryDictionary[new Guid("a9b0b5a8-ab9a-47b4-8361-0ce9d66accb3")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("a9b0b5a8-ab9a-47b4-8361-0ce9d66accb3"))) {queryDictionary[new Guid("a9b0b5a8-ab9a-47b4-8361-0ce9d66accb3")].SubQueries = subQueryDictionary[new Guid("a9b0b5a8-ab9a-47b4-8361-0ce9d66accb3")];}
			if(!subQueryDictionary.ContainsKey(new Guid("00e04ca5-bdc8-4e55-9efa-da895e7ece2f"))) {subQueryDictionary[new Guid("00e04ca5-bdc8-4e55-9efa-da895e7ece2f")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("00e04ca5-bdc8-4e55-9efa-da895e7ece2f")].Add(queryDictionary[new Guid("a9b0b5a8-ab9a-47b4-8361-0ce9d66accb3")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("00e04ca5-bdc8-4e55-9efa-da895e7ece2f"))) {queryDictionary[new Guid("00e04ca5-bdc8-4e55-9efa-da895e7ece2f")].SubQueries = subQueryDictionary[new Guid("00e04ca5-bdc8-4e55-9efa-da895e7ece2f")];}
		if(!subQueryDictionary.ContainsKey(new Guid("6cfedaf0-f198-4dc4-a298-2ae69871057d"))) {subQueryDictionary[new Guid("6cfedaf0-f198-4dc4-a298-2ae69871057d")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("6cfedaf0-f198-4dc4-a298-2ae69871057d")].Add(queryDictionary[new Guid("00e04ca5-bdc8-4e55-9efa-da895e7ece2f")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("6cfedaf0-f198-4dc4-a298-2ae69871057d"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("6cfedaf0-f198-4dc4-a298-2ae69871057d")];}
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
	createListInput.ColumnWidthsCSV = "100px,auto,30px,120px,120px,120px,120px";
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
		queryDictionary[new Guid("67aa58cc-e2bc-4fe3-ad11-3fac6a144d51")] = new InputRecordListQuery();
		queryDictionary[new Guid("67aa58cc-e2bc-4fe3-ad11-3fac6a144d51")].FieldName = null;
		queryDictionary[new Guid("67aa58cc-e2bc-4fe3-ad11-3fac6a144d51")].FieldValue =  null;
		queryDictionary[new Guid("67aa58cc-e2bc-4fe3-ad11-3fac6a144d51")].QueryType = "OR";
		queryDictionary[new Guid("67aa58cc-e2bc-4fe3-ad11-3fac6a144d51")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("03cf7bf7-96d5-4524-a1c5-62d92bdeef87")] = new InputRecordListQuery();
			queryDictionary[new Guid("03cf7bf7-96d5-4524-a1c5-62d92bdeef87")].FieldName = "owner_id";
			queryDictionary[new Guid("03cf7bf7-96d5-4524-a1c5-62d92bdeef87")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("03cf7bf7-96d5-4524-a1c5-62d92bdeef87")].QueryType = "EQ";
			queryDictionary[new Guid("03cf7bf7-96d5-4524-a1c5-62d92bdeef87")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("03cf7bf7-96d5-4524-a1c5-62d92bdeef87"))) {queryDictionary[new Guid("03cf7bf7-96d5-4524-a1c5-62d92bdeef87")].SubQueries = subQueryDictionary[new Guid("03cf7bf7-96d5-4524-a1c5-62d92bdeef87")];}
			if(!subQueryDictionary.ContainsKey(new Guid("67aa58cc-e2bc-4fe3-ad11-3fac6a144d51"))) {subQueryDictionary[new Guid("67aa58cc-e2bc-4fe3-ad11-3fac6a144d51")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("67aa58cc-e2bc-4fe3-ad11-3fac6a144d51")].Add(queryDictionary[new Guid("03cf7bf7-96d5-4524-a1c5-62d92bdeef87")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("67aa58cc-e2bc-4fe3-ad11-3fac6a144d51"))) {queryDictionary[new Guid("67aa58cc-e2bc-4fe3-ad11-3fac6a144d51")].SubQueries = subQueryDictionary[new Guid("67aa58cc-e2bc-4fe3-ad11-3fac6a144d51")];}
		if(!subQueryDictionary.ContainsKey(new Guid("6daf0c7a-0cc5-4067-aaed-7d9ca1d54c9f"))) {subQueryDictionary[new Guid("6daf0c7a-0cc5-4067-aaed-7d9ca1d54c9f")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("6daf0c7a-0cc5-4067-aaed-7d9ca1d54c9f")].Add(queryDictionary[new Guid("67aa58cc-e2bc-4fe3-ad11-3fac6a144d51")]);
		}
		{
		queryDictionary[new Guid("6a9bf9d2-a338-426f-957e-7d58e146b685")] = new InputRecordListQuery();
		queryDictionary[new Guid("6a9bf9d2-a338-426f-957e-7d58e146b685")].FieldName = null;
		queryDictionary[new Guid("6a9bf9d2-a338-426f-957e-7d58e146b685")].FieldValue =  null;
		queryDictionary[new Guid("6a9bf9d2-a338-426f-957e-7d58e146b685")].QueryType = "AND";
		queryDictionary[new Guid("6a9bf9d2-a338-426f-957e-7d58e146b685")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("85de879d-f9c1-4e4e-a9aa-acded4d101b0")] = new InputRecordListQuery();
			queryDictionary[new Guid("85de879d-f9c1-4e4e-a9aa-acded4d101b0")].FieldName = "code";
			queryDictionary[new Guid("85de879d-f9c1-4e4e-a9aa-acded4d101b0")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("85de879d-f9c1-4e4e-a9aa-acded4d101b0")].QueryType = "CONTAINS";
			queryDictionary[new Guid("85de879d-f9c1-4e4e-a9aa-acded4d101b0")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("85de879d-f9c1-4e4e-a9aa-acded4d101b0"))) {queryDictionary[new Guid("85de879d-f9c1-4e4e-a9aa-acded4d101b0")].SubQueries = subQueryDictionary[new Guid("85de879d-f9c1-4e4e-a9aa-acded4d101b0")];}
			if(!subQueryDictionary.ContainsKey(new Guid("6a9bf9d2-a338-426f-957e-7d58e146b685"))) {subQueryDictionary[new Guid("6a9bf9d2-a338-426f-957e-7d58e146b685")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("6a9bf9d2-a338-426f-957e-7d58e146b685")].Add(queryDictionary[new Guid("85de879d-f9c1-4e4e-a9aa-acded4d101b0")]);
			}
			{
			queryDictionary[new Guid("65f6947e-3d2e-46b7-9d1c-399b7ffbffc4")] = new InputRecordListQuery();
			queryDictionary[new Guid("65f6947e-3d2e-46b7-9d1c-399b7ffbffc4")].FieldName = "subject";
			queryDictionary[new Guid("65f6947e-3d2e-46b7-9d1c-399b7ffbffc4")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("65f6947e-3d2e-46b7-9d1c-399b7ffbffc4")].QueryType = "CONTAINS";
			queryDictionary[new Guid("65f6947e-3d2e-46b7-9d1c-399b7ffbffc4")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("65f6947e-3d2e-46b7-9d1c-399b7ffbffc4"))) {queryDictionary[new Guid("65f6947e-3d2e-46b7-9d1c-399b7ffbffc4")].SubQueries = subQueryDictionary[new Guid("65f6947e-3d2e-46b7-9d1c-399b7ffbffc4")];}
			if(!subQueryDictionary.ContainsKey(new Guid("6a9bf9d2-a338-426f-957e-7d58e146b685"))) {subQueryDictionary[new Guid("6a9bf9d2-a338-426f-957e-7d58e146b685")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("6a9bf9d2-a338-426f-957e-7d58e146b685")].Add(queryDictionary[new Guid("65f6947e-3d2e-46b7-9d1c-399b7ffbffc4")]);
			}
			{
			queryDictionary[new Guid("bd120730-d565-4bd3-bb40-8ac4756a0f8e")] = new InputRecordListQuery();
			queryDictionary[new Guid("bd120730-d565-4bd3-bb40-8ac4756a0f8e")].FieldName = "status";
			queryDictionary[new Guid("bd120730-d565-4bd3-bb40-8ac4756a0f8e")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("bd120730-d565-4bd3-bb40-8ac4756a0f8e")].QueryType = "EQ";
			queryDictionary[new Guid("bd120730-d565-4bd3-bb40-8ac4756a0f8e")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("bd120730-d565-4bd3-bb40-8ac4756a0f8e"))) {queryDictionary[new Guid("bd120730-d565-4bd3-bb40-8ac4756a0f8e")].SubQueries = subQueryDictionary[new Guid("bd120730-d565-4bd3-bb40-8ac4756a0f8e")];}
			if(!subQueryDictionary.ContainsKey(new Guid("6a9bf9d2-a338-426f-957e-7d58e146b685"))) {subQueryDictionary[new Guid("6a9bf9d2-a338-426f-957e-7d58e146b685")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("6a9bf9d2-a338-426f-957e-7d58e146b685")].Add(queryDictionary[new Guid("bd120730-d565-4bd3-bb40-8ac4756a0f8e")]);
			}
			{
			queryDictionary[new Guid("d3309aa2-785e-403e-88c6-5f127ba16756")] = new InputRecordListQuery();
			queryDictionary[new Guid("d3309aa2-785e-403e-88c6-5f127ba16756")].FieldName = "priority";
			queryDictionary[new Guid("d3309aa2-785e-403e-88c6-5f127ba16756")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("d3309aa2-785e-403e-88c6-5f127ba16756")].QueryType = "EQ";
			queryDictionary[new Guid("d3309aa2-785e-403e-88c6-5f127ba16756")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("d3309aa2-785e-403e-88c6-5f127ba16756"))) {queryDictionary[new Guid("d3309aa2-785e-403e-88c6-5f127ba16756")].SubQueries = subQueryDictionary[new Guid("d3309aa2-785e-403e-88c6-5f127ba16756")];}
			if(!subQueryDictionary.ContainsKey(new Guid("6a9bf9d2-a338-426f-957e-7d58e146b685"))) {subQueryDictionary[new Guid("6a9bf9d2-a338-426f-957e-7d58e146b685")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("6a9bf9d2-a338-426f-957e-7d58e146b685")].Add(queryDictionary[new Guid("d3309aa2-785e-403e-88c6-5f127ba16756")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("6a9bf9d2-a338-426f-957e-7d58e146b685"))) {queryDictionary[new Guid("6a9bf9d2-a338-426f-957e-7d58e146b685")].SubQueries = subQueryDictionary[new Guid("6a9bf9d2-a338-426f-957e-7d58e146b685")];}
		if(!subQueryDictionary.ContainsKey(new Guid("6daf0c7a-0cc5-4067-aaed-7d9ca1d54c9f"))) {subQueryDictionary[new Guid("6daf0c7a-0cc5-4067-aaed-7d9ca1d54c9f")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("6daf0c7a-0cc5-4067-aaed-7d9ca1d54c9f")].Add(queryDictionary[new Guid("6a9bf9d2-a338-426f-957e-7d58e146b685")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("6daf0c7a-0cc5-4067-aaed-7d9ca1d54c9f"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("6daf0c7a-0cc5-4067-aaed-7d9ca1d54c9f")];}
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
	createListInput.VisibleColumnsCount = Int32.Parse("6");
	createListInput.ColumnWidthsCSV = "auto,30px,120px,120px,120px,120px";
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
		queryDictionary[new Guid("f8367643-1aa0-4057-93e8-b18a9acdadb5")] = new InputRecordListQuery();
		queryDictionary[new Guid("f8367643-1aa0-4057-93e8-b18a9acdadb5")].FieldName = "subject";
		queryDictionary[new Guid("f8367643-1aa0-4057-93e8-b18a9acdadb5")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("f8367643-1aa0-4057-93e8-b18a9acdadb5")].QueryType = "CONTAINS";
		queryDictionary[new Guid("f8367643-1aa0-4057-93e8-b18a9acdadb5")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("f8367643-1aa0-4057-93e8-b18a9acdadb5"))) {queryDictionary[new Guid("f8367643-1aa0-4057-93e8-b18a9acdadb5")].SubQueries = subQueryDictionary[new Guid("f8367643-1aa0-4057-93e8-b18a9acdadb5")];}
		if(!subQueryDictionary.ContainsKey(new Guid("c7ea4294-27d1-4689-ab67-39183a33af56"))) {subQueryDictionary[new Guid("c7ea4294-27d1-4689-ab67-39183a33af56")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("c7ea4294-27d1-4689-ab67-39183a33af56")].Add(queryDictionary[new Guid("f8367643-1aa0-4057-93e8-b18a9acdadb5")]);
		}
		{
		queryDictionary[new Guid("0b3b19f1-6c1e-495a-9706-e3e198bb8570")] = new InputRecordListQuery();
		queryDictionary[new Guid("0b3b19f1-6c1e-495a-9706-e3e198bb8570")].FieldName = "status";
		queryDictionary[new Guid("0b3b19f1-6c1e-495a-9706-e3e198bb8570")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("0b3b19f1-6c1e-495a-9706-e3e198bb8570")].QueryType = "EQ";
		queryDictionary[new Guid("0b3b19f1-6c1e-495a-9706-e3e198bb8570")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("0b3b19f1-6c1e-495a-9706-e3e198bb8570"))) {queryDictionary[new Guid("0b3b19f1-6c1e-495a-9706-e3e198bb8570")].SubQueries = subQueryDictionary[new Guid("0b3b19f1-6c1e-495a-9706-e3e198bb8570")];}
		if(!subQueryDictionary.ContainsKey(new Guid("c7ea4294-27d1-4689-ab67-39183a33af56"))) {subQueryDictionary[new Guid("c7ea4294-27d1-4689-ab67-39183a33af56")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("c7ea4294-27d1-4689-ab67-39183a33af56")].Add(queryDictionary[new Guid("0b3b19f1-6c1e-495a-9706-e3e198bb8570")]);
		}
		{
		queryDictionary[new Guid("dcb08357-1956-4b35-8052-318576cee11f")] = new InputRecordListQuery();
		queryDictionary[new Guid("dcb08357-1956-4b35-8052-318576cee11f")].FieldName = "priority";
		queryDictionary[new Guid("dcb08357-1956-4b35-8052-318576cee11f")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("dcb08357-1956-4b35-8052-318576cee11f")].QueryType = "EQ";
		queryDictionary[new Guid("dcb08357-1956-4b35-8052-318576cee11f")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("dcb08357-1956-4b35-8052-318576cee11f"))) {queryDictionary[new Guid("dcb08357-1956-4b35-8052-318576cee11f")].SubQueries = subQueryDictionary[new Guid("dcb08357-1956-4b35-8052-318576cee11f")];}
		if(!subQueryDictionary.ContainsKey(new Guid("c7ea4294-27d1-4689-ab67-39183a33af56"))) {subQueryDictionary[new Guid("c7ea4294-27d1-4689-ab67-39183a33af56")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("c7ea4294-27d1-4689-ab67-39183a33af56")].Add(queryDictionary[new Guid("dcb08357-1956-4b35-8052-318576cee11f")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("c7ea4294-27d1-4689-ab67-39183a33af56"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("c7ea4294-27d1-4689-ab67-39183a33af56")];}
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

#region << ***Update field***  Entity: wv_bug Field Name: created_by >>
{
	var currentEntity = entMan.ReadEntity(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c")).Object;
	InputGuidField guidField = new InputGuidField();
	guidField.Id = currentEntity.Fields.SingleOrDefault(x => x.Name == "created_by").Id;
	guidField.Name = "created_by";
	guidField.Label = "Created By";
	guidField.PlaceholderText = "";
	guidField.Description = "";
	guidField.HelpText = "";
	guidField.Required = false;
	guidField.Unique = false;
	guidField.Searchable = false;
	guidField.Auditable = false;
	guidField.System = true;
	guidField.DefaultValue = null;
	guidField.GenerateNewId = false;
	guidField.EnableSecurity = true;
	guidField.Permissions = new FieldPermissions();
	guidField.Permissions.CanRead = new List<Guid>();
	guidField.Permissions.CanUpdate = new List<Guid>();
	//READ
	guidField.Permissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
	guidField.Permissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
	//UPDATE
	{
		var response = entMan.UpdateField(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c"), guidField);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_bug Field: created_by Message:" + response.Message);
	}
}
#endregion

#region << ***Update field***  Entity: wv_bug Field Name: created_on >>
{
	var currentEntity = entMan.ReadEntity(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c")).Object;
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
	{
		var response = entMan.UpdateField(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c"), datetimeField);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_bug Field: created_on Message:" + response.Message);
	}
}
#endregion

#region << ***Update view***  Entity: wv_bug View Name: create >>
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
					viewRow.Id = new Guid("b60ee280-7a49-4e66-94b5-69894be40ea0");
					viewRow.Weight = Decimal.Parse("1.0");
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					{
					var viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = Int32.Parse("6");
					viewColumn.Items = new List<InputRecordViewItemBase>();

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
					//Save column
					viewRow.Columns.Add(viewColumn);
					}
					#endregion
					#region << Column 2 >>
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
						viewItemFromRelation.FieldLabel = "Owner (blank for default)";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = "";
						viewItemFromRelation.FieldRequired = false;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = new Guid("cddc10b6-30ff-4a86-96e4-645b3ea59fd9");
						viewItemFromRelation.RelationName = "user_1_n_bug_owner";
						viewColumn.Items.Add(viewItemFromRelation);
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
					viewRow.Id = new Guid("c0e3757b-6817-4eda-8cd1-e95b603af049");
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

#region << ***Update view***  Entity: wv_bug View Name: general >>
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
					#region << created_on >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c");
						viewItem.EntityName = "wv_bug";
						viewItem.FieldId = new Guid("781cee71-1632-4bf9-83b1-ff122d29eb2a");
						viewItem.FieldName = "created_on";
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
						viewItemFromRelation.FieldLabel = "Created by";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldHelpText = "";
						viewItemFromRelation.FieldRequired = false;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = new Guid("cdc5c5f9-30dc-4e3b-ac0f-4137e54c6c7f");
						viewItemFromRelation.RelationName = "user_wv_bug_created_by";
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
			viewItemFromRelation.EntityId = new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73");
			viewItemFromRelation.EntityName = "wv_project_attachment";
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
			viewItemFromRelation.EntityId = new Guid("e2db7515-721f-446e-8333-6149b1ba131b");
			viewItemFromRelation.EntityName = "wv_timelog";
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

	#endregion
	{
		var response = entMan.UpdateRecordView(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_bug Updated view: general Message:" + response.Message);
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
	createListInput.ColumnWidthsCSV = "100px,auto,30px,120px,120px,120px,120px";
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
		queryDictionary[new Guid("e034941a-f3ac-4c14-ac84-cd18f272fd2a")] = new InputRecordListQuery();
		queryDictionary[new Guid("e034941a-f3ac-4c14-ac84-cd18f272fd2a")].FieldName = "code";
		queryDictionary[new Guid("e034941a-f3ac-4c14-ac84-cd18f272fd2a")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("e034941a-f3ac-4c14-ac84-cd18f272fd2a")].QueryType = "CONTAINS";
		queryDictionary[new Guid("e034941a-f3ac-4c14-ac84-cd18f272fd2a")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("e034941a-f3ac-4c14-ac84-cd18f272fd2a"))) {queryDictionary[new Guid("e034941a-f3ac-4c14-ac84-cd18f272fd2a")].SubQueries = subQueryDictionary[new Guid("e034941a-f3ac-4c14-ac84-cd18f272fd2a")];}
		if(!subQueryDictionary.ContainsKey(new Guid("2da36171-8c1d-4dd6-a612-aeb1a73635f7"))) {subQueryDictionary[new Guid("2da36171-8c1d-4dd6-a612-aeb1a73635f7")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("2da36171-8c1d-4dd6-a612-aeb1a73635f7")].Add(queryDictionary[new Guid("e034941a-f3ac-4c14-ac84-cd18f272fd2a")]);
		}
		{
		queryDictionary[new Guid("8d58e8bb-ca8c-4712-bcb7-7717c5790627")] = new InputRecordListQuery();
		queryDictionary[new Guid("8d58e8bb-ca8c-4712-bcb7-7717c5790627")].FieldName = "subject";
		queryDictionary[new Guid("8d58e8bb-ca8c-4712-bcb7-7717c5790627")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("8d58e8bb-ca8c-4712-bcb7-7717c5790627")].QueryType = "FTS";
		queryDictionary[new Guid("8d58e8bb-ca8c-4712-bcb7-7717c5790627")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("8d58e8bb-ca8c-4712-bcb7-7717c5790627"))) {queryDictionary[new Guid("8d58e8bb-ca8c-4712-bcb7-7717c5790627")].SubQueries = subQueryDictionary[new Guid("8d58e8bb-ca8c-4712-bcb7-7717c5790627")];}
		if(!subQueryDictionary.ContainsKey(new Guid("2da36171-8c1d-4dd6-a612-aeb1a73635f7"))) {subQueryDictionary[new Guid("2da36171-8c1d-4dd6-a612-aeb1a73635f7")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("2da36171-8c1d-4dd6-a612-aeb1a73635f7")].Add(queryDictionary[new Guid("8d58e8bb-ca8c-4712-bcb7-7717c5790627")]);
		}
		{
		queryDictionary[new Guid("b6f27ac7-31ae-43fa-b762-e9e8805cddcc")] = new InputRecordListQuery();
		queryDictionary[new Guid("b6f27ac7-31ae-43fa-b762-e9e8805cddcc")].FieldName = "status";
		queryDictionary[new Guid("b6f27ac7-31ae-43fa-b762-e9e8805cddcc")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("b6f27ac7-31ae-43fa-b762-e9e8805cddcc")].QueryType = "EQ";
		queryDictionary[new Guid("b6f27ac7-31ae-43fa-b762-e9e8805cddcc")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("b6f27ac7-31ae-43fa-b762-e9e8805cddcc"))) {queryDictionary[new Guid("b6f27ac7-31ae-43fa-b762-e9e8805cddcc")].SubQueries = subQueryDictionary[new Guid("b6f27ac7-31ae-43fa-b762-e9e8805cddcc")];}
		if(!subQueryDictionary.ContainsKey(new Guid("2da36171-8c1d-4dd6-a612-aeb1a73635f7"))) {subQueryDictionary[new Guid("2da36171-8c1d-4dd6-a612-aeb1a73635f7")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("2da36171-8c1d-4dd6-a612-aeb1a73635f7")].Add(queryDictionary[new Guid("b6f27ac7-31ae-43fa-b762-e9e8805cddcc")]);
		}
		{
		queryDictionary[new Guid("21100241-6a1f-4bfb-bb01-44b2d864a866")] = new InputRecordListQuery();
		queryDictionary[new Guid("21100241-6a1f-4bfb-bb01-44b2d864a866")].FieldName = "priority";
		queryDictionary[new Guid("21100241-6a1f-4bfb-bb01-44b2d864a866")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("21100241-6a1f-4bfb-bb01-44b2d864a866")].QueryType = "EQ";
		queryDictionary[new Guid("21100241-6a1f-4bfb-bb01-44b2d864a866")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("21100241-6a1f-4bfb-bb01-44b2d864a866"))) {queryDictionary[new Guid("21100241-6a1f-4bfb-bb01-44b2d864a866")].SubQueries = subQueryDictionary[new Guid("21100241-6a1f-4bfb-bb01-44b2d864a866")];}
		if(!subQueryDictionary.ContainsKey(new Guid("2da36171-8c1d-4dd6-a612-aeb1a73635f7"))) {subQueryDictionary[new Guid("2da36171-8c1d-4dd6-a612-aeb1a73635f7")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("2da36171-8c1d-4dd6-a612-aeb1a73635f7")].Add(queryDictionary[new Guid("21100241-6a1f-4bfb-bb01-44b2d864a866")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("2da36171-8c1d-4dd6-a612-aeb1a73635f7"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("2da36171-8c1d-4dd6-a612-aeb1a73635f7")];}
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
	createListInput.ColumnWidthsCSV = "100px,auto,30px,120px,120px,120px,120px";
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
		queryDictionary[new Guid("e6cc5145-7265-403e-a91b-9325b5e88b84")] = new InputRecordListQuery();
		queryDictionary[new Guid("e6cc5145-7265-403e-a91b-9325b5e88b84")].FieldName = null;
		queryDictionary[new Guid("e6cc5145-7265-403e-a91b-9325b5e88b84")].FieldValue =  null;
		queryDictionary[new Guid("e6cc5145-7265-403e-a91b-9325b5e88b84")].QueryType = "AND";
		queryDictionary[new Guid("e6cc5145-7265-403e-a91b-9325b5e88b84")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("de1e9166-ed9a-43ce-bde2-d5460ceda46e")] = new InputRecordListQuery();
			queryDictionary[new Guid("de1e9166-ed9a-43ce-bde2-d5460ceda46e")].FieldName = "created_by";
			queryDictionary[new Guid("de1e9166-ed9a-43ce-bde2-d5460ceda46e")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("de1e9166-ed9a-43ce-bde2-d5460ceda46e")].QueryType = "EQ";
			queryDictionary[new Guid("de1e9166-ed9a-43ce-bde2-d5460ceda46e")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("de1e9166-ed9a-43ce-bde2-d5460ceda46e"))) {queryDictionary[new Guid("de1e9166-ed9a-43ce-bde2-d5460ceda46e")].SubQueries = subQueryDictionary[new Guid("de1e9166-ed9a-43ce-bde2-d5460ceda46e")];}
			if(!subQueryDictionary.ContainsKey(new Guid("e6cc5145-7265-403e-a91b-9325b5e88b84"))) {subQueryDictionary[new Guid("e6cc5145-7265-403e-a91b-9325b5e88b84")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("e6cc5145-7265-403e-a91b-9325b5e88b84")].Add(queryDictionary[new Guid("de1e9166-ed9a-43ce-bde2-d5460ceda46e")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("e6cc5145-7265-403e-a91b-9325b5e88b84"))) {queryDictionary[new Guid("e6cc5145-7265-403e-a91b-9325b5e88b84")].SubQueries = subQueryDictionary[new Guid("e6cc5145-7265-403e-a91b-9325b5e88b84")];}
		if(!subQueryDictionary.ContainsKey(new Guid("175522c8-dfbc-4778-856c-f05124de3bd4"))) {subQueryDictionary[new Guid("175522c8-dfbc-4778-856c-f05124de3bd4")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("175522c8-dfbc-4778-856c-f05124de3bd4")].Add(queryDictionary[new Guid("e6cc5145-7265-403e-a91b-9325b5e88b84")]);
		}
		{
		queryDictionary[new Guid("0a012adf-60fa-44d9-b4a4-1b33dc1f3a6e")] = new InputRecordListQuery();
		queryDictionary[new Guid("0a012adf-60fa-44d9-b4a4-1b33dc1f3a6e")].FieldName = null;
		queryDictionary[new Guid("0a012adf-60fa-44d9-b4a4-1b33dc1f3a6e")].FieldValue =  null;
		queryDictionary[new Guid("0a012adf-60fa-44d9-b4a4-1b33dc1f3a6e")].QueryType = "AND";
		queryDictionary[new Guid("0a012adf-60fa-44d9-b4a4-1b33dc1f3a6e")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("03aba577-6d57-4f83-8770-0c3b3ac1c096")] = new InputRecordListQuery();
			queryDictionary[new Guid("03aba577-6d57-4f83-8770-0c3b3ac1c096")].FieldName = "code";
			queryDictionary[new Guid("03aba577-6d57-4f83-8770-0c3b3ac1c096")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("03aba577-6d57-4f83-8770-0c3b3ac1c096")].QueryType = "CONTAINS";
			queryDictionary[new Guid("03aba577-6d57-4f83-8770-0c3b3ac1c096")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("03aba577-6d57-4f83-8770-0c3b3ac1c096"))) {queryDictionary[new Guid("03aba577-6d57-4f83-8770-0c3b3ac1c096")].SubQueries = subQueryDictionary[new Guid("03aba577-6d57-4f83-8770-0c3b3ac1c096")];}
			if(!subQueryDictionary.ContainsKey(new Guid("0a012adf-60fa-44d9-b4a4-1b33dc1f3a6e"))) {subQueryDictionary[new Guid("0a012adf-60fa-44d9-b4a4-1b33dc1f3a6e")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("0a012adf-60fa-44d9-b4a4-1b33dc1f3a6e")].Add(queryDictionary[new Guid("03aba577-6d57-4f83-8770-0c3b3ac1c096")]);
			}
			{
			queryDictionary[new Guid("d963e33e-2738-475b-b304-dee5e8ca2936")] = new InputRecordListQuery();
			queryDictionary[new Guid("d963e33e-2738-475b-b304-dee5e8ca2936")].FieldName = "subject";
			queryDictionary[new Guid("d963e33e-2738-475b-b304-dee5e8ca2936")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("d963e33e-2738-475b-b304-dee5e8ca2936")].QueryType = "FTS";
			queryDictionary[new Guid("d963e33e-2738-475b-b304-dee5e8ca2936")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("d963e33e-2738-475b-b304-dee5e8ca2936"))) {queryDictionary[new Guid("d963e33e-2738-475b-b304-dee5e8ca2936")].SubQueries = subQueryDictionary[new Guid("d963e33e-2738-475b-b304-dee5e8ca2936")];}
			if(!subQueryDictionary.ContainsKey(new Guid("0a012adf-60fa-44d9-b4a4-1b33dc1f3a6e"))) {subQueryDictionary[new Guid("0a012adf-60fa-44d9-b4a4-1b33dc1f3a6e")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("0a012adf-60fa-44d9-b4a4-1b33dc1f3a6e")].Add(queryDictionary[new Guid("d963e33e-2738-475b-b304-dee5e8ca2936")]);
			}
			{
			queryDictionary[new Guid("45715123-491f-4c58-9793-2f9baf00ae7d")] = new InputRecordListQuery();
			queryDictionary[new Guid("45715123-491f-4c58-9793-2f9baf00ae7d")].FieldName = "status";
			queryDictionary[new Guid("45715123-491f-4c58-9793-2f9baf00ae7d")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("45715123-491f-4c58-9793-2f9baf00ae7d")].QueryType = "EQ";
			queryDictionary[new Guid("45715123-491f-4c58-9793-2f9baf00ae7d")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("45715123-491f-4c58-9793-2f9baf00ae7d"))) {queryDictionary[new Guid("45715123-491f-4c58-9793-2f9baf00ae7d")].SubQueries = subQueryDictionary[new Guid("45715123-491f-4c58-9793-2f9baf00ae7d")];}
			if(!subQueryDictionary.ContainsKey(new Guid("0a012adf-60fa-44d9-b4a4-1b33dc1f3a6e"))) {subQueryDictionary[new Guid("0a012adf-60fa-44d9-b4a4-1b33dc1f3a6e")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("0a012adf-60fa-44d9-b4a4-1b33dc1f3a6e")].Add(queryDictionary[new Guid("45715123-491f-4c58-9793-2f9baf00ae7d")]);
			}
			{
			queryDictionary[new Guid("f4a120dc-6380-4b68-9b4c-f7a636f795f7")] = new InputRecordListQuery();
			queryDictionary[new Guid("f4a120dc-6380-4b68-9b4c-f7a636f795f7")].FieldName = "priority";
			queryDictionary[new Guid("f4a120dc-6380-4b68-9b4c-f7a636f795f7")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("f4a120dc-6380-4b68-9b4c-f7a636f795f7")].QueryType = "EQ";
			queryDictionary[new Guid("f4a120dc-6380-4b68-9b4c-f7a636f795f7")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("f4a120dc-6380-4b68-9b4c-f7a636f795f7"))) {queryDictionary[new Guid("f4a120dc-6380-4b68-9b4c-f7a636f795f7")].SubQueries = subQueryDictionary[new Guid("f4a120dc-6380-4b68-9b4c-f7a636f795f7")];}
			if(!subQueryDictionary.ContainsKey(new Guid("0a012adf-60fa-44d9-b4a4-1b33dc1f3a6e"))) {subQueryDictionary[new Guid("0a012adf-60fa-44d9-b4a4-1b33dc1f3a6e")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("0a012adf-60fa-44d9-b4a4-1b33dc1f3a6e")].Add(queryDictionary[new Guid("f4a120dc-6380-4b68-9b4c-f7a636f795f7")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("0a012adf-60fa-44d9-b4a4-1b33dc1f3a6e"))) {queryDictionary[new Guid("0a012adf-60fa-44d9-b4a4-1b33dc1f3a6e")].SubQueries = subQueryDictionary[new Guid("0a012adf-60fa-44d9-b4a4-1b33dc1f3a6e")];}
		if(!subQueryDictionary.ContainsKey(new Guid("175522c8-dfbc-4778-856c-f05124de3bd4"))) {subQueryDictionary[new Guid("175522c8-dfbc-4778-856c-f05124de3bd4")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("175522c8-dfbc-4778-856c-f05124de3bd4")].Add(queryDictionary[new Guid("0a012adf-60fa-44d9-b4a4-1b33dc1f3a6e")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("175522c8-dfbc-4778-856c-f05124de3bd4"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("175522c8-dfbc-4778-856c-f05124de3bd4")];}
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
	createListInput.ColumnWidthsCSV = "100px,auto,30px,160px,120px,120px,120px";
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
		queryDictionary[new Guid("5984f75f-f2ae-4bfd-b905-ae69fc21fd8b")] = new InputRecordListQuery();
		queryDictionary[new Guid("5984f75f-f2ae-4bfd-b905-ae69fc21fd8b")].FieldName = null;
		queryDictionary[new Guid("5984f75f-f2ae-4bfd-b905-ae69fc21fd8b")].FieldValue =  null;
		queryDictionary[new Guid("5984f75f-f2ae-4bfd-b905-ae69fc21fd8b")].QueryType = "AND";
		queryDictionary[new Guid("5984f75f-f2ae-4bfd-b905-ae69fc21fd8b")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("06f93140-007e-4b0d-a875-12805032b412")] = new InputRecordListQuery();
			queryDictionary[new Guid("06f93140-007e-4b0d-a875-12805032b412")].FieldName = "owner_id";
			queryDictionary[new Guid("06f93140-007e-4b0d-a875-12805032b412")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("06f93140-007e-4b0d-a875-12805032b412")].QueryType = "EQ";
			queryDictionary[new Guid("06f93140-007e-4b0d-a875-12805032b412")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("06f93140-007e-4b0d-a875-12805032b412"))) {queryDictionary[new Guid("06f93140-007e-4b0d-a875-12805032b412")].SubQueries = subQueryDictionary[new Guid("06f93140-007e-4b0d-a875-12805032b412")];}
			if(!subQueryDictionary.ContainsKey(new Guid("5984f75f-f2ae-4bfd-b905-ae69fc21fd8b"))) {subQueryDictionary[new Guid("5984f75f-f2ae-4bfd-b905-ae69fc21fd8b")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("5984f75f-f2ae-4bfd-b905-ae69fc21fd8b")].Add(queryDictionary[new Guid("06f93140-007e-4b0d-a875-12805032b412")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("5984f75f-f2ae-4bfd-b905-ae69fc21fd8b"))) {queryDictionary[new Guid("5984f75f-f2ae-4bfd-b905-ae69fc21fd8b")].SubQueries = subQueryDictionary[new Guid("5984f75f-f2ae-4bfd-b905-ae69fc21fd8b")];}
		if(!subQueryDictionary.ContainsKey(new Guid("57b80552-e41d-4ff2-adef-7278ef138f87"))) {subQueryDictionary[new Guid("57b80552-e41d-4ff2-adef-7278ef138f87")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("57b80552-e41d-4ff2-adef-7278ef138f87")].Add(queryDictionary[new Guid("5984f75f-f2ae-4bfd-b905-ae69fc21fd8b")]);
		}
		{
		queryDictionary[new Guid("56fff3a9-1d8a-4d32-81ae-bda3a3149c70")] = new InputRecordListQuery();
		queryDictionary[new Guid("56fff3a9-1d8a-4d32-81ae-bda3a3149c70")].FieldName = null;
		queryDictionary[new Guid("56fff3a9-1d8a-4d32-81ae-bda3a3149c70")].FieldValue =  null;
		queryDictionary[new Guid("56fff3a9-1d8a-4d32-81ae-bda3a3149c70")].QueryType = "AND";
		queryDictionary[new Guid("56fff3a9-1d8a-4d32-81ae-bda3a3149c70")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("06cac4e9-13d6-4acc-aa4a-a94343bba512")] = new InputRecordListQuery();
			queryDictionary[new Guid("06cac4e9-13d6-4acc-aa4a-a94343bba512")].FieldName = "code";
			queryDictionary[new Guid("06cac4e9-13d6-4acc-aa4a-a94343bba512")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("06cac4e9-13d6-4acc-aa4a-a94343bba512")].QueryType = "CONTAINS";
			queryDictionary[new Guid("06cac4e9-13d6-4acc-aa4a-a94343bba512")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("06cac4e9-13d6-4acc-aa4a-a94343bba512"))) {queryDictionary[new Guid("06cac4e9-13d6-4acc-aa4a-a94343bba512")].SubQueries = subQueryDictionary[new Guid("06cac4e9-13d6-4acc-aa4a-a94343bba512")];}
			if(!subQueryDictionary.ContainsKey(new Guid("56fff3a9-1d8a-4d32-81ae-bda3a3149c70"))) {subQueryDictionary[new Guid("56fff3a9-1d8a-4d32-81ae-bda3a3149c70")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("56fff3a9-1d8a-4d32-81ae-bda3a3149c70")].Add(queryDictionary[new Guid("06cac4e9-13d6-4acc-aa4a-a94343bba512")]);
			}
			{
			queryDictionary[new Guid("983f850a-848c-4520-ae64-97b204529e89")] = new InputRecordListQuery();
			queryDictionary[new Guid("983f850a-848c-4520-ae64-97b204529e89")].FieldName = "subject";
			queryDictionary[new Guid("983f850a-848c-4520-ae64-97b204529e89")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("983f850a-848c-4520-ae64-97b204529e89")].QueryType = "FTS";
			queryDictionary[new Guid("983f850a-848c-4520-ae64-97b204529e89")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("983f850a-848c-4520-ae64-97b204529e89"))) {queryDictionary[new Guid("983f850a-848c-4520-ae64-97b204529e89")].SubQueries = subQueryDictionary[new Guid("983f850a-848c-4520-ae64-97b204529e89")];}
			if(!subQueryDictionary.ContainsKey(new Guid("56fff3a9-1d8a-4d32-81ae-bda3a3149c70"))) {subQueryDictionary[new Guid("56fff3a9-1d8a-4d32-81ae-bda3a3149c70")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("56fff3a9-1d8a-4d32-81ae-bda3a3149c70")].Add(queryDictionary[new Guid("983f850a-848c-4520-ae64-97b204529e89")]);
			}
			{
			queryDictionary[new Guid("c404423b-1cb1-4929-9929-c21ee6c813a8")] = new InputRecordListQuery();
			queryDictionary[new Guid("c404423b-1cb1-4929-9929-c21ee6c813a8")].FieldName = "status";
			queryDictionary[new Guid("c404423b-1cb1-4929-9929-c21ee6c813a8")].FieldValue =  "closed";
			queryDictionary[new Guid("c404423b-1cb1-4929-9929-c21ee6c813a8")].QueryType = "NOT";
			queryDictionary[new Guid("c404423b-1cb1-4929-9929-c21ee6c813a8")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("c404423b-1cb1-4929-9929-c21ee6c813a8"))) {queryDictionary[new Guid("c404423b-1cb1-4929-9929-c21ee6c813a8")].SubQueries = subQueryDictionary[new Guid("c404423b-1cb1-4929-9929-c21ee6c813a8")];}
			if(!subQueryDictionary.ContainsKey(new Guid("56fff3a9-1d8a-4d32-81ae-bda3a3149c70"))) {subQueryDictionary[new Guid("56fff3a9-1d8a-4d32-81ae-bda3a3149c70")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("56fff3a9-1d8a-4d32-81ae-bda3a3149c70")].Add(queryDictionary[new Guid("c404423b-1cb1-4929-9929-c21ee6c813a8")]);
			}
			{
			queryDictionary[new Guid("53b7ea19-537b-4670-bff8-6b0324f67baf")] = new InputRecordListQuery();
			queryDictionary[new Guid("53b7ea19-537b-4670-bff8-6b0324f67baf")].FieldName = "priority";
			queryDictionary[new Guid("53b7ea19-537b-4670-bff8-6b0324f67baf")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("53b7ea19-537b-4670-bff8-6b0324f67baf")].QueryType = "EQ";
			queryDictionary[new Guid("53b7ea19-537b-4670-bff8-6b0324f67baf")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("53b7ea19-537b-4670-bff8-6b0324f67baf"))) {queryDictionary[new Guid("53b7ea19-537b-4670-bff8-6b0324f67baf")].SubQueries = subQueryDictionary[new Guid("53b7ea19-537b-4670-bff8-6b0324f67baf")];}
			if(!subQueryDictionary.ContainsKey(new Guid("56fff3a9-1d8a-4d32-81ae-bda3a3149c70"))) {subQueryDictionary[new Guid("56fff3a9-1d8a-4d32-81ae-bda3a3149c70")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("56fff3a9-1d8a-4d32-81ae-bda3a3149c70")].Add(queryDictionary[new Guid("53b7ea19-537b-4670-bff8-6b0324f67baf")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("56fff3a9-1d8a-4d32-81ae-bda3a3149c70"))) {queryDictionary[new Guid("56fff3a9-1d8a-4d32-81ae-bda3a3149c70")].SubQueries = subQueryDictionary[new Guid("56fff3a9-1d8a-4d32-81ae-bda3a3149c70")];}
		if(!subQueryDictionary.ContainsKey(new Guid("57b80552-e41d-4ff2-adef-7278ef138f87"))) {subQueryDictionary[new Guid("57b80552-e41d-4ff2-adef-7278ef138f87")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("57b80552-e41d-4ff2-adef-7278ef138f87")].Add(queryDictionary[new Guid("56fff3a9-1d8a-4d32-81ae-bda3a3149c70")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("57b80552-e41d-4ff2-adef-7278ef138f87"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("57b80552-e41d-4ff2-adef-7278ef138f87")];}
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
	createListInput.ColumnWidthsCSV = "100px,auto,30px,120px,120px,120px,120px";
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
		queryDictionary[new Guid("afa0931d-7b08-4357-9747-cada41ea7876")] = new InputRecordListQuery();
		queryDictionary[new Guid("afa0931d-7b08-4357-9747-cada41ea7876")].FieldName = null;
		queryDictionary[new Guid("afa0931d-7b08-4357-9747-cada41ea7876")].FieldValue =  null;
		queryDictionary[new Guid("afa0931d-7b08-4357-9747-cada41ea7876")].QueryType = "OR";
		queryDictionary[new Guid("afa0931d-7b08-4357-9747-cada41ea7876")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("85081fcd-cf93-478a-a218-2916b5b2ef0b")] = new InputRecordListQuery();
			queryDictionary[new Guid("85081fcd-cf93-478a-a218-2916b5b2ef0b")].FieldName = "owner_id";
			queryDictionary[new Guid("85081fcd-cf93-478a-a218-2916b5b2ef0b")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("85081fcd-cf93-478a-a218-2916b5b2ef0b")].QueryType = "EQ";
			queryDictionary[new Guid("85081fcd-cf93-478a-a218-2916b5b2ef0b")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("85081fcd-cf93-478a-a218-2916b5b2ef0b"))) {queryDictionary[new Guid("85081fcd-cf93-478a-a218-2916b5b2ef0b")].SubQueries = subQueryDictionary[new Guid("85081fcd-cf93-478a-a218-2916b5b2ef0b")];}
			if(!subQueryDictionary.ContainsKey(new Guid("afa0931d-7b08-4357-9747-cada41ea7876"))) {subQueryDictionary[new Guid("afa0931d-7b08-4357-9747-cada41ea7876")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("afa0931d-7b08-4357-9747-cada41ea7876")].Add(queryDictionary[new Guid("85081fcd-cf93-478a-a218-2916b5b2ef0b")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("afa0931d-7b08-4357-9747-cada41ea7876"))) {queryDictionary[new Guid("afa0931d-7b08-4357-9747-cada41ea7876")].SubQueries = subQueryDictionary[new Guid("afa0931d-7b08-4357-9747-cada41ea7876")];}
		if(!subQueryDictionary.ContainsKey(new Guid("9bf707e4-9fb0-414a-9ac6-037ef1a42f2c"))) {subQueryDictionary[new Guid("9bf707e4-9fb0-414a-9ac6-037ef1a42f2c")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("9bf707e4-9fb0-414a-9ac6-037ef1a42f2c")].Add(queryDictionary[new Guid("afa0931d-7b08-4357-9747-cada41ea7876")]);
		}
		{
		queryDictionary[new Guid("de54341e-44cc-4697-bb35-5b618b30c38c")] = new InputRecordListQuery();
		queryDictionary[new Guid("de54341e-44cc-4697-bb35-5b618b30c38c")].FieldName = null;
		queryDictionary[new Guid("de54341e-44cc-4697-bb35-5b618b30c38c")].FieldValue =  null;
		queryDictionary[new Guid("de54341e-44cc-4697-bb35-5b618b30c38c")].QueryType = "AND";
		queryDictionary[new Guid("de54341e-44cc-4697-bb35-5b618b30c38c")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("5600a866-06cc-4d91-8239-2de417f73905")] = new InputRecordListQuery();
			queryDictionary[new Guid("5600a866-06cc-4d91-8239-2de417f73905")].FieldName = "code";
			queryDictionary[new Guid("5600a866-06cc-4d91-8239-2de417f73905")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("5600a866-06cc-4d91-8239-2de417f73905")].QueryType = "CONTAINS";
			queryDictionary[new Guid("5600a866-06cc-4d91-8239-2de417f73905")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("5600a866-06cc-4d91-8239-2de417f73905"))) {queryDictionary[new Guid("5600a866-06cc-4d91-8239-2de417f73905")].SubQueries = subQueryDictionary[new Guid("5600a866-06cc-4d91-8239-2de417f73905")];}
			if(!subQueryDictionary.ContainsKey(new Guid("de54341e-44cc-4697-bb35-5b618b30c38c"))) {subQueryDictionary[new Guid("de54341e-44cc-4697-bb35-5b618b30c38c")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("de54341e-44cc-4697-bb35-5b618b30c38c")].Add(queryDictionary[new Guid("5600a866-06cc-4d91-8239-2de417f73905")]);
			}
			{
			queryDictionary[new Guid("7e036fb0-ac00-4c14-8358-62c47c85314a")] = new InputRecordListQuery();
			queryDictionary[new Guid("7e036fb0-ac00-4c14-8358-62c47c85314a")].FieldName = "subject";
			queryDictionary[new Guid("7e036fb0-ac00-4c14-8358-62c47c85314a")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("7e036fb0-ac00-4c14-8358-62c47c85314a")].QueryType = "FTS";
			queryDictionary[new Guid("7e036fb0-ac00-4c14-8358-62c47c85314a")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("7e036fb0-ac00-4c14-8358-62c47c85314a"))) {queryDictionary[new Guid("7e036fb0-ac00-4c14-8358-62c47c85314a")].SubQueries = subQueryDictionary[new Guid("7e036fb0-ac00-4c14-8358-62c47c85314a")];}
			if(!subQueryDictionary.ContainsKey(new Guid("de54341e-44cc-4697-bb35-5b618b30c38c"))) {subQueryDictionary[new Guid("de54341e-44cc-4697-bb35-5b618b30c38c")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("de54341e-44cc-4697-bb35-5b618b30c38c")].Add(queryDictionary[new Guid("7e036fb0-ac00-4c14-8358-62c47c85314a")]);
			}
			{
			queryDictionary[new Guid("aa53550e-24f1-4e90-bd44-f9c1f2cbc7c0")] = new InputRecordListQuery();
			queryDictionary[new Guid("aa53550e-24f1-4e90-bd44-f9c1f2cbc7c0")].FieldName = "status";
			queryDictionary[new Guid("aa53550e-24f1-4e90-bd44-f9c1f2cbc7c0")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("aa53550e-24f1-4e90-bd44-f9c1f2cbc7c0")].QueryType = "EQ";
			queryDictionary[new Guid("aa53550e-24f1-4e90-bd44-f9c1f2cbc7c0")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("aa53550e-24f1-4e90-bd44-f9c1f2cbc7c0"))) {queryDictionary[new Guid("aa53550e-24f1-4e90-bd44-f9c1f2cbc7c0")].SubQueries = subQueryDictionary[new Guid("aa53550e-24f1-4e90-bd44-f9c1f2cbc7c0")];}
			if(!subQueryDictionary.ContainsKey(new Guid("de54341e-44cc-4697-bb35-5b618b30c38c"))) {subQueryDictionary[new Guid("de54341e-44cc-4697-bb35-5b618b30c38c")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("de54341e-44cc-4697-bb35-5b618b30c38c")].Add(queryDictionary[new Guid("aa53550e-24f1-4e90-bd44-f9c1f2cbc7c0")]);
			}
			{
			queryDictionary[new Guid("94952a4b-afa5-4071-909e-6b1b954ac28d")] = new InputRecordListQuery();
			queryDictionary[new Guid("94952a4b-afa5-4071-909e-6b1b954ac28d")].FieldName = "priority";
			queryDictionary[new Guid("94952a4b-afa5-4071-909e-6b1b954ac28d")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("94952a4b-afa5-4071-909e-6b1b954ac28d")].QueryType = "EQ";
			queryDictionary[new Guid("94952a4b-afa5-4071-909e-6b1b954ac28d")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("94952a4b-afa5-4071-909e-6b1b954ac28d"))) {queryDictionary[new Guid("94952a4b-afa5-4071-909e-6b1b954ac28d")].SubQueries = subQueryDictionary[new Guid("94952a4b-afa5-4071-909e-6b1b954ac28d")];}
			if(!subQueryDictionary.ContainsKey(new Guid("de54341e-44cc-4697-bb35-5b618b30c38c"))) {subQueryDictionary[new Guid("de54341e-44cc-4697-bb35-5b618b30c38c")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("de54341e-44cc-4697-bb35-5b618b30c38c")].Add(queryDictionary[new Guid("94952a4b-afa5-4071-909e-6b1b954ac28d")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("de54341e-44cc-4697-bb35-5b618b30c38c"))) {queryDictionary[new Guid("de54341e-44cc-4697-bb35-5b618b30c38c")].SubQueries = subQueryDictionary[new Guid("de54341e-44cc-4697-bb35-5b618b30c38c")];}
		if(!subQueryDictionary.ContainsKey(new Guid("9bf707e4-9fb0-414a-9ac6-037ef1a42f2c"))) {subQueryDictionary[new Guid("9bf707e4-9fb0-414a-9ac6-037ef1a42f2c")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("9bf707e4-9fb0-414a-9ac6-037ef1a42f2c")].Add(queryDictionary[new Guid("de54341e-44cc-4697-bb35-5b618b30c38c")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("9bf707e4-9fb0-414a-9ac6-037ef1a42f2c"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("9bf707e4-9fb0-414a-9ac6-037ef1a42f2c")];}
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
	createListInput.VisibleColumnsCount = Int32.Parse("5");
	createListInput.ColumnWidthsCSV = "auto,30px,120px,120px,120px";
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
		queryDictionary[new Guid("15d9f87f-e11b-45fc-b725-0ff4b5904606")] = new InputRecordListQuery();
		queryDictionary[new Guid("15d9f87f-e11b-45fc-b725-0ff4b5904606")].FieldName = "subject";
		queryDictionary[new Guid("15d9f87f-e11b-45fc-b725-0ff4b5904606")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("15d9f87f-e11b-45fc-b725-0ff4b5904606")].QueryType = "FTS";
		queryDictionary[new Guid("15d9f87f-e11b-45fc-b725-0ff4b5904606")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("15d9f87f-e11b-45fc-b725-0ff4b5904606"))) {queryDictionary[new Guid("15d9f87f-e11b-45fc-b725-0ff4b5904606")].SubQueries = subQueryDictionary[new Guid("15d9f87f-e11b-45fc-b725-0ff4b5904606")];}
		if(!subQueryDictionary.ContainsKey(new Guid("92a4ea4c-c2c1-444f-bba5-c932c335da44"))) {subQueryDictionary[new Guid("92a4ea4c-c2c1-444f-bba5-c932c335da44")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("92a4ea4c-c2c1-444f-bba5-c932c335da44")].Add(queryDictionary[new Guid("15d9f87f-e11b-45fc-b725-0ff4b5904606")]);
		}
		{
		queryDictionary[new Guid("8027f349-113e-4e65-843b-d2db62d73b08")] = new InputRecordListQuery();
		queryDictionary[new Guid("8027f349-113e-4e65-843b-d2db62d73b08")].FieldName = "status";
		queryDictionary[new Guid("8027f349-113e-4e65-843b-d2db62d73b08")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("8027f349-113e-4e65-843b-d2db62d73b08")].QueryType = "EQ";
		queryDictionary[new Guid("8027f349-113e-4e65-843b-d2db62d73b08")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("8027f349-113e-4e65-843b-d2db62d73b08"))) {queryDictionary[new Guid("8027f349-113e-4e65-843b-d2db62d73b08")].SubQueries = subQueryDictionary[new Guid("8027f349-113e-4e65-843b-d2db62d73b08")];}
		if(!subQueryDictionary.ContainsKey(new Guid("92a4ea4c-c2c1-444f-bba5-c932c335da44"))) {subQueryDictionary[new Guid("92a4ea4c-c2c1-444f-bba5-c932c335da44")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("92a4ea4c-c2c1-444f-bba5-c932c335da44")].Add(queryDictionary[new Guid("8027f349-113e-4e65-843b-d2db62d73b08")]);
		}
		{
		queryDictionary[new Guid("d427d09c-581f-4444-babc-ca4d5bd15a54")] = new InputRecordListQuery();
		queryDictionary[new Guid("d427d09c-581f-4444-babc-ca4d5bd15a54")].FieldName = "priority";
		queryDictionary[new Guid("d427d09c-581f-4444-babc-ca4d5bd15a54")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("d427d09c-581f-4444-babc-ca4d5bd15a54")].QueryType = "EQ";
		queryDictionary[new Guid("d427d09c-581f-4444-babc-ca4d5bd15a54")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("d427d09c-581f-4444-babc-ca4d5bd15a54"))) {queryDictionary[new Guid("d427d09c-581f-4444-babc-ca4d5bd15a54")].SubQueries = subQueryDictionary[new Guid("d427d09c-581f-4444-babc-ca4d5bd15a54")];}
		if(!subQueryDictionary.ContainsKey(new Guid("92a4ea4c-c2c1-444f-bba5-c932c335da44"))) {subQueryDictionary[new Guid("92a4ea4c-c2c1-444f-bba5-c932c335da44")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("92a4ea4c-c2c1-444f-bba5-c932c335da44")].Add(queryDictionary[new Guid("d427d09c-581f-4444-babc-ca4d5bd15a54")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("92a4ea4c-c2c1-444f-bba5-c932c335da44"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("92a4ea4c-c2c1-444f-bba5-c932c335da44")];}
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




		}


    }
}
