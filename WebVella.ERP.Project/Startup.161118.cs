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
		private static void Patch161118(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan, bool createSampleRecords = false)
		{


#region << ***Create field***  Entity: wv_project_attachment Field Name: comment_content >>
{
	InputMultiLineTextField textareaField = new InputMultiLineTextField();
	textareaField.Id = new Guid("501a2446-be58-4a04-a374-b74d041c8368");
	textareaField.Name = "comment_content";
	textareaField.Label = "Comment content";
	textareaField.PlaceholderText = "";
	textareaField.Description = "dummy holder for the comment content";
	textareaField.HelpText = "";
	textareaField.Required = false;
	textareaField.Unique = false;
	textareaField.Searchable = false;
	textareaField.Auditable = false;
	textareaField.System = true;
	textareaField.DefaultValue = null;
	textareaField.MaxLength = null;
	textareaField.VisibleLineNumber = null;
	textareaField.EnableSecurity = false;
	textareaField.Permissions = new FieldPermissions();
	textareaField.Permissions.CanRead = new List<Guid>();
	textareaField.Permissions.CanUpdate = new List<Guid>();
	//READ
	//UPDATE
	{
		var response = entMan.CreateField(new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73"), textareaField, false);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_project_attachment Field: comment_content Message:" + response.Message);
	}
}
#endregion

#region << ***Create field***  Entity: wv_project_comment Field Name: attachment_id >>
{
	InputGuidField guidField = new InputGuidField();
	guidField.Id = new Guid("6fe9c5d7-e42b-49fd-9b0e-734f04052c21");
	guidField.Name = "attachment_id";
	guidField.Label = "Attachment Id";
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
	guidField.EnableSecurity = false;
	guidField.Permissions = new FieldPermissions();
	guidField.Permissions.CanRead = new List<Guid>();
	guidField.Permissions.CanUpdate = new List<Guid>();
	//READ
	//UPDATE
	{
		var response = entMan.CreateField(new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99"), guidField, false);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_project_comment Field: attachment_id Message:" + response.Message);
	}
}
#endregion

#region << ***Create relation*** Relation name: attachment_1_n_comment >>
{
	var relation = new EntityRelation();
	var originEntity = entMan.ReadEntity(new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73")).Object;
	var originField = originEntity.Fields.SingleOrDefault(x => x.Name == "id");
	var targetEntity = entMan.ReadEntity(new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99")).Object;
	var targetField = targetEntity.Fields.SingleOrDefault(x => x.Name == "attachment_id");
	relation.Id = new Guid("3e3a95f0-65fd-4e37-aa17-8879b1c207b8");
	relation.Name =  "attachment_1_n_comment";
	relation.Label = "attachment_1_1_comment";
	relation.Description = "";
	relation.System =  true;
	relation.RelationType = EntityRelationType.OneToMany;
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
			throw new Exception("System error 10060. Relation: attachment_1_n_comment Create. Message:" + response.Message);
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
					#region << estimation >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("65acced0-1650-4ff0-bbff-9937c382cd89");
						viewItem.EntityName = "wv_task";
						viewItem.FieldId = new Guid("848e2a24-8d58-451b-9cf8-9ba1254a169a");
						viewItem.FieldName = "estimation";
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
		queryDictionary[new Guid("48565f64-5435-45ae-886d-8b728afade16")] = new InputRecordListQuery();
		queryDictionary[new Guid("48565f64-5435-45ae-886d-8b728afade16")].FieldName = null;
		queryDictionary[new Guid("48565f64-5435-45ae-886d-8b728afade16")].FieldValue =  null;
		queryDictionary[new Guid("48565f64-5435-45ae-886d-8b728afade16")].QueryType = "OR";
		queryDictionary[new Guid("48565f64-5435-45ae-886d-8b728afade16")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("9b620f94-8034-4d2a-bf9a-8c5bc52c62f1")] = new InputRecordListQuery();
			queryDictionary[new Guid("9b620f94-8034-4d2a-bf9a-8c5bc52c62f1")].FieldName = "owner_id";
			queryDictionary[new Guid("9b620f94-8034-4d2a-bf9a-8c5bc52c62f1")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("9b620f94-8034-4d2a-bf9a-8c5bc52c62f1")].QueryType = "EQ";
			queryDictionary[new Guid("9b620f94-8034-4d2a-bf9a-8c5bc52c62f1")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("9b620f94-8034-4d2a-bf9a-8c5bc52c62f1"))) {queryDictionary[new Guid("9b620f94-8034-4d2a-bf9a-8c5bc52c62f1")].SubQueries = subQueryDictionary[new Guid("9b620f94-8034-4d2a-bf9a-8c5bc52c62f1")];}
			if(!subQueryDictionary.ContainsKey(new Guid("48565f64-5435-45ae-886d-8b728afade16"))) {subQueryDictionary[new Guid("48565f64-5435-45ae-886d-8b728afade16")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("48565f64-5435-45ae-886d-8b728afade16")].Add(queryDictionary[new Guid("9b620f94-8034-4d2a-bf9a-8c5bc52c62f1")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("48565f64-5435-45ae-886d-8b728afade16"))) {queryDictionary[new Guid("48565f64-5435-45ae-886d-8b728afade16")].SubQueries = subQueryDictionary[new Guid("48565f64-5435-45ae-886d-8b728afade16")];}
		if(!subQueryDictionary.ContainsKey(new Guid("f6c53335-d065-49b4-a34d-3fa0e8c96f70"))) {subQueryDictionary[new Guid("f6c53335-d065-49b4-a34d-3fa0e8c96f70")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("f6c53335-d065-49b4-a34d-3fa0e8c96f70")].Add(queryDictionary[new Guid("48565f64-5435-45ae-886d-8b728afade16")]);
		}
		{
		queryDictionary[new Guid("8a9092cf-3f40-4946-a3ff-b416b0eb9661")] = new InputRecordListQuery();
		queryDictionary[new Guid("8a9092cf-3f40-4946-a3ff-b416b0eb9661")].FieldName = null;
		queryDictionary[new Guid("8a9092cf-3f40-4946-a3ff-b416b0eb9661")].FieldValue =  null;
		queryDictionary[new Guid("8a9092cf-3f40-4946-a3ff-b416b0eb9661")].QueryType = "AND";
		queryDictionary[new Guid("8a9092cf-3f40-4946-a3ff-b416b0eb9661")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("36170b6c-70c0-4a4d-83de-ecd8c9033cde")] = new InputRecordListQuery();
			queryDictionary[new Guid("36170b6c-70c0-4a4d-83de-ecd8c9033cde")].FieldName = "code";
			queryDictionary[new Guid("36170b6c-70c0-4a4d-83de-ecd8c9033cde")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("36170b6c-70c0-4a4d-83de-ecd8c9033cde")].QueryType = "CONTAINS";
			queryDictionary[new Guid("36170b6c-70c0-4a4d-83de-ecd8c9033cde")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("36170b6c-70c0-4a4d-83de-ecd8c9033cde"))) {queryDictionary[new Guid("36170b6c-70c0-4a4d-83de-ecd8c9033cde")].SubQueries = subQueryDictionary[new Guid("36170b6c-70c0-4a4d-83de-ecd8c9033cde")];}
			if(!subQueryDictionary.ContainsKey(new Guid("8a9092cf-3f40-4946-a3ff-b416b0eb9661"))) {subQueryDictionary[new Guid("8a9092cf-3f40-4946-a3ff-b416b0eb9661")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("8a9092cf-3f40-4946-a3ff-b416b0eb9661")].Add(queryDictionary[new Guid("36170b6c-70c0-4a4d-83de-ecd8c9033cde")]);
			}
			{
			queryDictionary[new Guid("6e6c4e0a-67e7-4ac9-8222-6e190304cf9d")] = new InputRecordListQuery();
			queryDictionary[new Guid("6e6c4e0a-67e7-4ac9-8222-6e190304cf9d")].FieldName = "subject";
			queryDictionary[new Guid("6e6c4e0a-67e7-4ac9-8222-6e190304cf9d")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("6e6c4e0a-67e7-4ac9-8222-6e190304cf9d")].QueryType = "CONTAINS";
			queryDictionary[new Guid("6e6c4e0a-67e7-4ac9-8222-6e190304cf9d")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("6e6c4e0a-67e7-4ac9-8222-6e190304cf9d"))) {queryDictionary[new Guid("6e6c4e0a-67e7-4ac9-8222-6e190304cf9d")].SubQueries = subQueryDictionary[new Guid("6e6c4e0a-67e7-4ac9-8222-6e190304cf9d")];}
			if(!subQueryDictionary.ContainsKey(new Guid("8a9092cf-3f40-4946-a3ff-b416b0eb9661"))) {subQueryDictionary[new Guid("8a9092cf-3f40-4946-a3ff-b416b0eb9661")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("8a9092cf-3f40-4946-a3ff-b416b0eb9661")].Add(queryDictionary[new Guid("6e6c4e0a-67e7-4ac9-8222-6e190304cf9d")]);
			}
			{
			queryDictionary[new Guid("9d3b9a5c-f588-4254-9513-45118c29a945")] = new InputRecordListQuery();
			queryDictionary[new Guid("9d3b9a5c-f588-4254-9513-45118c29a945")].FieldName = "status";
			queryDictionary[new Guid("9d3b9a5c-f588-4254-9513-45118c29a945")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("9d3b9a5c-f588-4254-9513-45118c29a945")].QueryType = "EQ";
			queryDictionary[new Guid("9d3b9a5c-f588-4254-9513-45118c29a945")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("9d3b9a5c-f588-4254-9513-45118c29a945"))) {queryDictionary[new Guid("9d3b9a5c-f588-4254-9513-45118c29a945")].SubQueries = subQueryDictionary[new Guid("9d3b9a5c-f588-4254-9513-45118c29a945")];}
			if(!subQueryDictionary.ContainsKey(new Guid("8a9092cf-3f40-4946-a3ff-b416b0eb9661"))) {subQueryDictionary[new Guid("8a9092cf-3f40-4946-a3ff-b416b0eb9661")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("8a9092cf-3f40-4946-a3ff-b416b0eb9661")].Add(queryDictionary[new Guid("9d3b9a5c-f588-4254-9513-45118c29a945")]);
			}
			{
			queryDictionary[new Guid("749e0e33-dac5-4ca9-84b0-63fea143fcdd")] = new InputRecordListQuery();
			queryDictionary[new Guid("749e0e33-dac5-4ca9-84b0-63fea143fcdd")].FieldName = "priority";
			queryDictionary[new Guid("749e0e33-dac5-4ca9-84b0-63fea143fcdd")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("749e0e33-dac5-4ca9-84b0-63fea143fcdd")].QueryType = "EQ";
			queryDictionary[new Guid("749e0e33-dac5-4ca9-84b0-63fea143fcdd")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("749e0e33-dac5-4ca9-84b0-63fea143fcdd"))) {queryDictionary[new Guid("749e0e33-dac5-4ca9-84b0-63fea143fcdd")].SubQueries = subQueryDictionary[new Guid("749e0e33-dac5-4ca9-84b0-63fea143fcdd")];}
			if(!subQueryDictionary.ContainsKey(new Guid("8a9092cf-3f40-4946-a3ff-b416b0eb9661"))) {subQueryDictionary[new Guid("8a9092cf-3f40-4946-a3ff-b416b0eb9661")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("8a9092cf-3f40-4946-a3ff-b416b0eb9661")].Add(queryDictionary[new Guid("749e0e33-dac5-4ca9-84b0-63fea143fcdd")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("8a9092cf-3f40-4946-a3ff-b416b0eb9661"))) {queryDictionary[new Guid("8a9092cf-3f40-4946-a3ff-b416b0eb9661")].SubQueries = subQueryDictionary[new Guid("8a9092cf-3f40-4946-a3ff-b416b0eb9661")];}
		if(!subQueryDictionary.ContainsKey(new Guid("f6c53335-d065-49b4-a34d-3fa0e8c96f70"))) {subQueryDictionary[new Guid("f6c53335-d065-49b4-a34d-3fa0e8c96f70")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("f6c53335-d065-49b4-a34d-3fa0e8c96f70")].Add(queryDictionary[new Guid("8a9092cf-3f40-4946-a3ff-b416b0eb9661")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("f6c53335-d065-49b4-a34d-3fa0e8c96f70"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("f6c53335-d065-49b4-a34d-3fa0e8c96f70")];}
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
		queryDictionary[new Guid("6fe764f1-6fa8-4d5f-b9c1-eaa7ae30afd2")] = new InputRecordListQuery();
		queryDictionary[new Guid("6fe764f1-6fa8-4d5f-b9c1-eaa7ae30afd2")].FieldName = "code";
		queryDictionary[new Guid("6fe764f1-6fa8-4d5f-b9c1-eaa7ae30afd2")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("6fe764f1-6fa8-4d5f-b9c1-eaa7ae30afd2")].QueryType = "CONTAINS";
		queryDictionary[new Guid("6fe764f1-6fa8-4d5f-b9c1-eaa7ae30afd2")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("6fe764f1-6fa8-4d5f-b9c1-eaa7ae30afd2"))) {queryDictionary[new Guid("6fe764f1-6fa8-4d5f-b9c1-eaa7ae30afd2")].SubQueries = subQueryDictionary[new Guid("6fe764f1-6fa8-4d5f-b9c1-eaa7ae30afd2")];}
		if(!subQueryDictionary.ContainsKey(new Guid("0a2eaa73-a1a0-4db6-a9d9-e3991f892788"))) {subQueryDictionary[new Guid("0a2eaa73-a1a0-4db6-a9d9-e3991f892788")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("0a2eaa73-a1a0-4db6-a9d9-e3991f892788")].Add(queryDictionary[new Guid("6fe764f1-6fa8-4d5f-b9c1-eaa7ae30afd2")]);
		}
		{
		queryDictionary[new Guid("c94157e1-f24b-4c73-8860-3d784ca8af4a")] = new InputRecordListQuery();
		queryDictionary[new Guid("c94157e1-f24b-4c73-8860-3d784ca8af4a")].FieldName = "subject";
		queryDictionary[new Guid("c94157e1-f24b-4c73-8860-3d784ca8af4a")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("c94157e1-f24b-4c73-8860-3d784ca8af4a")].QueryType = "CONTAINS";
		queryDictionary[new Guid("c94157e1-f24b-4c73-8860-3d784ca8af4a")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("c94157e1-f24b-4c73-8860-3d784ca8af4a"))) {queryDictionary[new Guid("c94157e1-f24b-4c73-8860-3d784ca8af4a")].SubQueries = subQueryDictionary[new Guid("c94157e1-f24b-4c73-8860-3d784ca8af4a")];}
		if(!subQueryDictionary.ContainsKey(new Guid("0a2eaa73-a1a0-4db6-a9d9-e3991f892788"))) {subQueryDictionary[new Guid("0a2eaa73-a1a0-4db6-a9d9-e3991f892788")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("0a2eaa73-a1a0-4db6-a9d9-e3991f892788")].Add(queryDictionary[new Guid("c94157e1-f24b-4c73-8860-3d784ca8af4a")]);
		}
		{
		queryDictionary[new Guid("b2b510e9-1ca5-430e-a3c7-c705fcf2c29a")] = new InputRecordListQuery();
		queryDictionary[new Guid("b2b510e9-1ca5-430e-a3c7-c705fcf2c29a")].FieldName = "status";
		queryDictionary[new Guid("b2b510e9-1ca5-430e-a3c7-c705fcf2c29a")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("b2b510e9-1ca5-430e-a3c7-c705fcf2c29a")].QueryType = "EQ";
		queryDictionary[new Guid("b2b510e9-1ca5-430e-a3c7-c705fcf2c29a")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("b2b510e9-1ca5-430e-a3c7-c705fcf2c29a"))) {queryDictionary[new Guid("b2b510e9-1ca5-430e-a3c7-c705fcf2c29a")].SubQueries = subQueryDictionary[new Guid("b2b510e9-1ca5-430e-a3c7-c705fcf2c29a")];}
		if(!subQueryDictionary.ContainsKey(new Guid("0a2eaa73-a1a0-4db6-a9d9-e3991f892788"))) {subQueryDictionary[new Guid("0a2eaa73-a1a0-4db6-a9d9-e3991f892788")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("0a2eaa73-a1a0-4db6-a9d9-e3991f892788")].Add(queryDictionary[new Guid("b2b510e9-1ca5-430e-a3c7-c705fcf2c29a")]);
		}
		{
		queryDictionary[new Guid("e6064637-0c82-4b12-b59c-056582c23c96")] = new InputRecordListQuery();
		queryDictionary[new Guid("e6064637-0c82-4b12-b59c-056582c23c96")].FieldName = "priority";
		queryDictionary[new Guid("e6064637-0c82-4b12-b59c-056582c23c96")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("e6064637-0c82-4b12-b59c-056582c23c96")].QueryType = "EQ";
		queryDictionary[new Guid("e6064637-0c82-4b12-b59c-056582c23c96")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("e6064637-0c82-4b12-b59c-056582c23c96"))) {queryDictionary[new Guid("e6064637-0c82-4b12-b59c-056582c23c96")].SubQueries = subQueryDictionary[new Guid("e6064637-0c82-4b12-b59c-056582c23c96")];}
		if(!subQueryDictionary.ContainsKey(new Guid("0a2eaa73-a1a0-4db6-a9d9-e3991f892788"))) {subQueryDictionary[new Guid("0a2eaa73-a1a0-4db6-a9d9-e3991f892788")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("0a2eaa73-a1a0-4db6-a9d9-e3991f892788")].Add(queryDictionary[new Guid("e6064637-0c82-4b12-b59c-056582c23c96")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("0a2eaa73-a1a0-4db6-a9d9-e3991f892788"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("0a2eaa73-a1a0-4db6-a9d9-e3991f892788")];}
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
		queryDictionary[new Guid("b5dd6bac-f8d3-4469-adfc-def75503876a")] = new InputRecordListQuery();
		queryDictionary[new Guid("b5dd6bac-f8d3-4469-adfc-def75503876a")].FieldName = null;
		queryDictionary[new Guid("b5dd6bac-f8d3-4469-adfc-def75503876a")].FieldValue =  null;
		queryDictionary[new Guid("b5dd6bac-f8d3-4469-adfc-def75503876a")].QueryType = "AND";
		queryDictionary[new Guid("b5dd6bac-f8d3-4469-adfc-def75503876a")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("0a4711c1-d07e-4854-9c43-bd85f9580ec4")] = new InputRecordListQuery();
			queryDictionary[new Guid("0a4711c1-d07e-4854-9c43-bd85f9580ec4")].FieldName = "owner_id";
			queryDictionary[new Guid("0a4711c1-d07e-4854-9c43-bd85f9580ec4")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("0a4711c1-d07e-4854-9c43-bd85f9580ec4")].QueryType = "EQ";
			queryDictionary[new Guid("0a4711c1-d07e-4854-9c43-bd85f9580ec4")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("0a4711c1-d07e-4854-9c43-bd85f9580ec4"))) {queryDictionary[new Guid("0a4711c1-d07e-4854-9c43-bd85f9580ec4")].SubQueries = subQueryDictionary[new Guid("0a4711c1-d07e-4854-9c43-bd85f9580ec4")];}
			if(!subQueryDictionary.ContainsKey(new Guid("b5dd6bac-f8d3-4469-adfc-def75503876a"))) {subQueryDictionary[new Guid("b5dd6bac-f8d3-4469-adfc-def75503876a")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("b5dd6bac-f8d3-4469-adfc-def75503876a")].Add(queryDictionary[new Guid("0a4711c1-d07e-4854-9c43-bd85f9580ec4")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("b5dd6bac-f8d3-4469-adfc-def75503876a"))) {queryDictionary[new Guid("b5dd6bac-f8d3-4469-adfc-def75503876a")].SubQueries = subQueryDictionary[new Guid("b5dd6bac-f8d3-4469-adfc-def75503876a")];}
		if(!subQueryDictionary.ContainsKey(new Guid("31d84153-03f2-4143-8415-caa6781e3239"))) {subQueryDictionary[new Guid("31d84153-03f2-4143-8415-caa6781e3239")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("31d84153-03f2-4143-8415-caa6781e3239")].Add(queryDictionary[new Guid("b5dd6bac-f8d3-4469-adfc-def75503876a")]);
		}
		{
		queryDictionary[new Guid("7783e2d6-ce68-4a45-b3f7-b8578fb9def9")] = new InputRecordListQuery();
		queryDictionary[new Guid("7783e2d6-ce68-4a45-b3f7-b8578fb9def9")].FieldName = null;
		queryDictionary[new Guid("7783e2d6-ce68-4a45-b3f7-b8578fb9def9")].FieldValue =  null;
		queryDictionary[new Guid("7783e2d6-ce68-4a45-b3f7-b8578fb9def9")].QueryType = "AND";
		queryDictionary[new Guid("7783e2d6-ce68-4a45-b3f7-b8578fb9def9")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("ff6a9665-c9b5-4770-9821-00d661b49f74")] = new InputRecordListQuery();
			queryDictionary[new Guid("ff6a9665-c9b5-4770-9821-00d661b49f74")].FieldName = "code";
			queryDictionary[new Guid("ff6a9665-c9b5-4770-9821-00d661b49f74")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("ff6a9665-c9b5-4770-9821-00d661b49f74")].QueryType = "CONTAINS";
			queryDictionary[new Guid("ff6a9665-c9b5-4770-9821-00d661b49f74")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("ff6a9665-c9b5-4770-9821-00d661b49f74"))) {queryDictionary[new Guid("ff6a9665-c9b5-4770-9821-00d661b49f74")].SubQueries = subQueryDictionary[new Guid("ff6a9665-c9b5-4770-9821-00d661b49f74")];}
			if(!subQueryDictionary.ContainsKey(new Guid("7783e2d6-ce68-4a45-b3f7-b8578fb9def9"))) {subQueryDictionary[new Guid("7783e2d6-ce68-4a45-b3f7-b8578fb9def9")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("7783e2d6-ce68-4a45-b3f7-b8578fb9def9")].Add(queryDictionary[new Guid("ff6a9665-c9b5-4770-9821-00d661b49f74")]);
			}
			{
			queryDictionary[new Guid("018f1a66-adc0-477d-9b74-175ef63911a7")] = new InputRecordListQuery();
			queryDictionary[new Guid("018f1a66-adc0-477d-9b74-175ef63911a7")].FieldName = "subject";
			queryDictionary[new Guid("018f1a66-adc0-477d-9b74-175ef63911a7")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("018f1a66-adc0-477d-9b74-175ef63911a7")].QueryType = "CONTAINS";
			queryDictionary[new Guid("018f1a66-adc0-477d-9b74-175ef63911a7")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("018f1a66-adc0-477d-9b74-175ef63911a7"))) {queryDictionary[new Guid("018f1a66-adc0-477d-9b74-175ef63911a7")].SubQueries = subQueryDictionary[new Guid("018f1a66-adc0-477d-9b74-175ef63911a7")];}
			if(!subQueryDictionary.ContainsKey(new Guid("7783e2d6-ce68-4a45-b3f7-b8578fb9def9"))) {subQueryDictionary[new Guid("7783e2d6-ce68-4a45-b3f7-b8578fb9def9")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("7783e2d6-ce68-4a45-b3f7-b8578fb9def9")].Add(queryDictionary[new Guid("018f1a66-adc0-477d-9b74-175ef63911a7")]);
			}
			{
			queryDictionary[new Guid("828d543e-85a6-4802-9079-c6e02bd72fad")] = new InputRecordListQuery();
			queryDictionary[new Guid("828d543e-85a6-4802-9079-c6e02bd72fad")].FieldName = "status";
			queryDictionary[new Guid("828d543e-85a6-4802-9079-c6e02bd72fad")].FieldValue =  "completed";
			queryDictionary[new Guid("828d543e-85a6-4802-9079-c6e02bd72fad")].QueryType = "NOT";
			queryDictionary[new Guid("828d543e-85a6-4802-9079-c6e02bd72fad")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("828d543e-85a6-4802-9079-c6e02bd72fad"))) {queryDictionary[new Guid("828d543e-85a6-4802-9079-c6e02bd72fad")].SubQueries = subQueryDictionary[new Guid("828d543e-85a6-4802-9079-c6e02bd72fad")];}
			if(!subQueryDictionary.ContainsKey(new Guid("7783e2d6-ce68-4a45-b3f7-b8578fb9def9"))) {subQueryDictionary[new Guid("7783e2d6-ce68-4a45-b3f7-b8578fb9def9")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("7783e2d6-ce68-4a45-b3f7-b8578fb9def9")].Add(queryDictionary[new Guid("828d543e-85a6-4802-9079-c6e02bd72fad")]);
			}
			{
			queryDictionary[new Guid("964067d6-d3da-46c5-9dcd-f6ae93c9a9b3")] = new InputRecordListQuery();
			queryDictionary[new Guid("964067d6-d3da-46c5-9dcd-f6ae93c9a9b3")].FieldName = "priority";
			queryDictionary[new Guid("964067d6-d3da-46c5-9dcd-f6ae93c9a9b3")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("964067d6-d3da-46c5-9dcd-f6ae93c9a9b3")].QueryType = "EQ";
			queryDictionary[new Guid("964067d6-d3da-46c5-9dcd-f6ae93c9a9b3")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("964067d6-d3da-46c5-9dcd-f6ae93c9a9b3"))) {queryDictionary[new Guid("964067d6-d3da-46c5-9dcd-f6ae93c9a9b3")].SubQueries = subQueryDictionary[new Guid("964067d6-d3da-46c5-9dcd-f6ae93c9a9b3")];}
			if(!subQueryDictionary.ContainsKey(new Guid("7783e2d6-ce68-4a45-b3f7-b8578fb9def9"))) {subQueryDictionary[new Guid("7783e2d6-ce68-4a45-b3f7-b8578fb9def9")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("7783e2d6-ce68-4a45-b3f7-b8578fb9def9")].Add(queryDictionary[new Guid("964067d6-d3da-46c5-9dcd-f6ae93c9a9b3")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("7783e2d6-ce68-4a45-b3f7-b8578fb9def9"))) {queryDictionary[new Guid("7783e2d6-ce68-4a45-b3f7-b8578fb9def9")].SubQueries = subQueryDictionary[new Guid("7783e2d6-ce68-4a45-b3f7-b8578fb9def9")];}
		if(!subQueryDictionary.ContainsKey(new Guid("31d84153-03f2-4143-8415-caa6781e3239"))) {subQueryDictionary[new Guid("31d84153-03f2-4143-8415-caa6781e3239")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("31d84153-03f2-4143-8415-caa6781e3239")].Add(queryDictionary[new Guid("7783e2d6-ce68-4a45-b3f7-b8578fb9def9")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("31d84153-03f2-4143-8415-caa6781e3239"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("31d84153-03f2-4143-8415-caa6781e3239")];}
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
		queryDictionary[new Guid("84821934-3f44-40bf-8119-571ab388d883")] = new InputRecordListQuery();
		queryDictionary[new Guid("84821934-3f44-40bf-8119-571ab388d883")].FieldName = null;
		queryDictionary[new Guid("84821934-3f44-40bf-8119-571ab388d883")].FieldValue =  null;
		queryDictionary[new Guid("84821934-3f44-40bf-8119-571ab388d883")].QueryType = "OR";
		queryDictionary[new Guid("84821934-3f44-40bf-8119-571ab388d883")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("5fa4fa1b-5502-4b43-9592-b6c9b0578f44")] = new InputRecordListQuery();
			queryDictionary[new Guid("5fa4fa1b-5502-4b43-9592-b6c9b0578f44")].FieldName = "owner_id";
			queryDictionary[new Guid("5fa4fa1b-5502-4b43-9592-b6c9b0578f44")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("5fa4fa1b-5502-4b43-9592-b6c9b0578f44")].QueryType = "EQ";
			queryDictionary[new Guid("5fa4fa1b-5502-4b43-9592-b6c9b0578f44")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("5fa4fa1b-5502-4b43-9592-b6c9b0578f44"))) {queryDictionary[new Guid("5fa4fa1b-5502-4b43-9592-b6c9b0578f44")].SubQueries = subQueryDictionary[new Guid("5fa4fa1b-5502-4b43-9592-b6c9b0578f44")];}
			if(!subQueryDictionary.ContainsKey(new Guid("84821934-3f44-40bf-8119-571ab388d883"))) {subQueryDictionary[new Guid("84821934-3f44-40bf-8119-571ab388d883")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("84821934-3f44-40bf-8119-571ab388d883")].Add(queryDictionary[new Guid("5fa4fa1b-5502-4b43-9592-b6c9b0578f44")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("84821934-3f44-40bf-8119-571ab388d883"))) {queryDictionary[new Guid("84821934-3f44-40bf-8119-571ab388d883")].SubQueries = subQueryDictionary[new Guid("84821934-3f44-40bf-8119-571ab388d883")];}
		if(!subQueryDictionary.ContainsKey(new Guid("c1df5842-d05d-476d-afc2-7eec51625eb8"))) {subQueryDictionary[new Guid("c1df5842-d05d-476d-afc2-7eec51625eb8")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("c1df5842-d05d-476d-afc2-7eec51625eb8")].Add(queryDictionary[new Guid("84821934-3f44-40bf-8119-571ab388d883")]);
		}
		{
		queryDictionary[new Guid("1cc2451c-7166-400c-b10f-b8c0fe4ca79d")] = new InputRecordListQuery();
		queryDictionary[new Guid("1cc2451c-7166-400c-b10f-b8c0fe4ca79d")].FieldName = null;
		queryDictionary[new Guid("1cc2451c-7166-400c-b10f-b8c0fe4ca79d")].FieldValue =  null;
		queryDictionary[new Guid("1cc2451c-7166-400c-b10f-b8c0fe4ca79d")].QueryType = "AND";
		queryDictionary[new Guid("1cc2451c-7166-400c-b10f-b8c0fe4ca79d")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("1473eaa1-e5de-4f52-84fc-3c62ee8c05de")] = new InputRecordListQuery();
			queryDictionary[new Guid("1473eaa1-e5de-4f52-84fc-3c62ee8c05de")].FieldName = "code";
			queryDictionary[new Guid("1473eaa1-e5de-4f52-84fc-3c62ee8c05de")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("1473eaa1-e5de-4f52-84fc-3c62ee8c05de")].QueryType = "CONTAINS";
			queryDictionary[new Guid("1473eaa1-e5de-4f52-84fc-3c62ee8c05de")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("1473eaa1-e5de-4f52-84fc-3c62ee8c05de"))) {queryDictionary[new Guid("1473eaa1-e5de-4f52-84fc-3c62ee8c05de")].SubQueries = subQueryDictionary[new Guid("1473eaa1-e5de-4f52-84fc-3c62ee8c05de")];}
			if(!subQueryDictionary.ContainsKey(new Guid("1cc2451c-7166-400c-b10f-b8c0fe4ca79d"))) {subQueryDictionary[new Guid("1cc2451c-7166-400c-b10f-b8c0fe4ca79d")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("1cc2451c-7166-400c-b10f-b8c0fe4ca79d")].Add(queryDictionary[new Guid("1473eaa1-e5de-4f52-84fc-3c62ee8c05de")]);
			}
			{
			queryDictionary[new Guid("4d3f5d7d-75f9-4931-8d75-19f08845537a")] = new InputRecordListQuery();
			queryDictionary[new Guid("4d3f5d7d-75f9-4931-8d75-19f08845537a")].FieldName = "subject";
			queryDictionary[new Guid("4d3f5d7d-75f9-4931-8d75-19f08845537a")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("4d3f5d7d-75f9-4931-8d75-19f08845537a")].QueryType = "CONTAINS";
			queryDictionary[new Guid("4d3f5d7d-75f9-4931-8d75-19f08845537a")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("4d3f5d7d-75f9-4931-8d75-19f08845537a"))) {queryDictionary[new Guid("4d3f5d7d-75f9-4931-8d75-19f08845537a")].SubQueries = subQueryDictionary[new Guid("4d3f5d7d-75f9-4931-8d75-19f08845537a")];}
			if(!subQueryDictionary.ContainsKey(new Guid("1cc2451c-7166-400c-b10f-b8c0fe4ca79d"))) {subQueryDictionary[new Guid("1cc2451c-7166-400c-b10f-b8c0fe4ca79d")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("1cc2451c-7166-400c-b10f-b8c0fe4ca79d")].Add(queryDictionary[new Guid("4d3f5d7d-75f9-4931-8d75-19f08845537a")]);
			}
			{
			queryDictionary[new Guid("bff4e3f5-b510-4e8a-8379-4b556e0c0b28")] = new InputRecordListQuery();
			queryDictionary[new Guid("bff4e3f5-b510-4e8a-8379-4b556e0c0b28")].FieldName = "status";
			queryDictionary[new Guid("bff4e3f5-b510-4e8a-8379-4b556e0c0b28")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("bff4e3f5-b510-4e8a-8379-4b556e0c0b28")].QueryType = "EQ";
			queryDictionary[new Guid("bff4e3f5-b510-4e8a-8379-4b556e0c0b28")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("bff4e3f5-b510-4e8a-8379-4b556e0c0b28"))) {queryDictionary[new Guid("bff4e3f5-b510-4e8a-8379-4b556e0c0b28")].SubQueries = subQueryDictionary[new Guid("bff4e3f5-b510-4e8a-8379-4b556e0c0b28")];}
			if(!subQueryDictionary.ContainsKey(new Guid("1cc2451c-7166-400c-b10f-b8c0fe4ca79d"))) {subQueryDictionary[new Guid("1cc2451c-7166-400c-b10f-b8c0fe4ca79d")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("1cc2451c-7166-400c-b10f-b8c0fe4ca79d")].Add(queryDictionary[new Guid("bff4e3f5-b510-4e8a-8379-4b556e0c0b28")]);
			}
			{
			queryDictionary[new Guid("bdd957e9-e411-4ca4-bf53-d1d5017864c8")] = new InputRecordListQuery();
			queryDictionary[new Guid("bdd957e9-e411-4ca4-bf53-d1d5017864c8")].FieldName = "priority";
			queryDictionary[new Guid("bdd957e9-e411-4ca4-bf53-d1d5017864c8")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("bdd957e9-e411-4ca4-bf53-d1d5017864c8")].QueryType = "EQ";
			queryDictionary[new Guid("bdd957e9-e411-4ca4-bf53-d1d5017864c8")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("bdd957e9-e411-4ca4-bf53-d1d5017864c8"))) {queryDictionary[new Guid("bdd957e9-e411-4ca4-bf53-d1d5017864c8")].SubQueries = subQueryDictionary[new Guid("bdd957e9-e411-4ca4-bf53-d1d5017864c8")];}
			if(!subQueryDictionary.ContainsKey(new Guid("1cc2451c-7166-400c-b10f-b8c0fe4ca79d"))) {subQueryDictionary[new Guid("1cc2451c-7166-400c-b10f-b8c0fe4ca79d")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("1cc2451c-7166-400c-b10f-b8c0fe4ca79d")].Add(queryDictionary[new Guid("bdd957e9-e411-4ca4-bf53-d1d5017864c8")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("1cc2451c-7166-400c-b10f-b8c0fe4ca79d"))) {queryDictionary[new Guid("1cc2451c-7166-400c-b10f-b8c0fe4ca79d")].SubQueries = subQueryDictionary[new Guid("1cc2451c-7166-400c-b10f-b8c0fe4ca79d")];}
		if(!subQueryDictionary.ContainsKey(new Guid("c1df5842-d05d-476d-afc2-7eec51625eb8"))) {subQueryDictionary[new Guid("c1df5842-d05d-476d-afc2-7eec51625eb8")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("c1df5842-d05d-476d-afc2-7eec51625eb8")].Add(queryDictionary[new Guid("1cc2451c-7166-400c-b10f-b8c0fe4ca79d")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("c1df5842-d05d-476d-afc2-7eec51625eb8"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("c1df5842-d05d-476d-afc2-7eec51625eb8")];}
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
		queryDictionary[new Guid("9abf63d3-f6bf-403b-8643-c578bf4a42ae")] = new InputRecordListQuery();
		queryDictionary[new Guid("9abf63d3-f6bf-403b-8643-c578bf4a42ae")].FieldName = "code";
		queryDictionary[new Guid("9abf63d3-f6bf-403b-8643-c578bf4a42ae")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("9abf63d3-f6bf-403b-8643-c578bf4a42ae")].QueryType = "CONTAINS";
		queryDictionary[new Guid("9abf63d3-f6bf-403b-8643-c578bf4a42ae")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("9abf63d3-f6bf-403b-8643-c578bf4a42ae"))) {queryDictionary[new Guid("9abf63d3-f6bf-403b-8643-c578bf4a42ae")].SubQueries = subQueryDictionary[new Guid("9abf63d3-f6bf-403b-8643-c578bf4a42ae")];}
		if(!subQueryDictionary.ContainsKey(new Guid("51193fdd-67a4-468c-a035-376b28b7a12c"))) {subQueryDictionary[new Guid("51193fdd-67a4-468c-a035-376b28b7a12c")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("51193fdd-67a4-468c-a035-376b28b7a12c")].Add(queryDictionary[new Guid("9abf63d3-f6bf-403b-8643-c578bf4a42ae")]);
		}
		{
		queryDictionary[new Guid("fe75650a-daf2-4953-98f1-46efb390fb4e")] = new InputRecordListQuery();
		queryDictionary[new Guid("fe75650a-daf2-4953-98f1-46efb390fb4e")].FieldName = "subject";
		queryDictionary[new Guid("fe75650a-daf2-4953-98f1-46efb390fb4e")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("fe75650a-daf2-4953-98f1-46efb390fb4e")].QueryType = "CONTAINS";
		queryDictionary[new Guid("fe75650a-daf2-4953-98f1-46efb390fb4e")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("fe75650a-daf2-4953-98f1-46efb390fb4e"))) {queryDictionary[new Guid("fe75650a-daf2-4953-98f1-46efb390fb4e")].SubQueries = subQueryDictionary[new Guid("fe75650a-daf2-4953-98f1-46efb390fb4e")];}
		if(!subQueryDictionary.ContainsKey(new Guid("51193fdd-67a4-468c-a035-376b28b7a12c"))) {subQueryDictionary[new Guid("51193fdd-67a4-468c-a035-376b28b7a12c")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("51193fdd-67a4-468c-a035-376b28b7a12c")].Add(queryDictionary[new Guid("fe75650a-daf2-4953-98f1-46efb390fb4e")]);
		}
		{
		queryDictionary[new Guid("50714c97-efb4-4b61-9d0f-043c853147a8")] = new InputRecordListQuery();
		queryDictionary[new Guid("50714c97-efb4-4b61-9d0f-043c853147a8")].FieldName = "status";
		queryDictionary[new Guid("50714c97-efb4-4b61-9d0f-043c853147a8")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("50714c97-efb4-4b61-9d0f-043c853147a8")].QueryType = "EQ";
		queryDictionary[new Guid("50714c97-efb4-4b61-9d0f-043c853147a8")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("50714c97-efb4-4b61-9d0f-043c853147a8"))) {queryDictionary[new Guid("50714c97-efb4-4b61-9d0f-043c853147a8")].SubQueries = subQueryDictionary[new Guid("50714c97-efb4-4b61-9d0f-043c853147a8")];}
		if(!subQueryDictionary.ContainsKey(new Guid("51193fdd-67a4-468c-a035-376b28b7a12c"))) {subQueryDictionary[new Guid("51193fdd-67a4-468c-a035-376b28b7a12c")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("51193fdd-67a4-468c-a035-376b28b7a12c")].Add(queryDictionary[new Guid("50714c97-efb4-4b61-9d0f-043c853147a8")]);
		}
		{
		queryDictionary[new Guid("cd27ee9c-e047-4fac-95fc-f80bb1320dfe")] = new InputRecordListQuery();
		queryDictionary[new Guid("cd27ee9c-e047-4fac-95fc-f80bb1320dfe")].FieldName = "priority";
		queryDictionary[new Guid("cd27ee9c-e047-4fac-95fc-f80bb1320dfe")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("cd27ee9c-e047-4fac-95fc-f80bb1320dfe")].QueryType = "EQ";
		queryDictionary[new Guid("cd27ee9c-e047-4fac-95fc-f80bb1320dfe")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("cd27ee9c-e047-4fac-95fc-f80bb1320dfe"))) {queryDictionary[new Guid("cd27ee9c-e047-4fac-95fc-f80bb1320dfe")].SubQueries = subQueryDictionary[new Guid("cd27ee9c-e047-4fac-95fc-f80bb1320dfe")];}
		if(!subQueryDictionary.ContainsKey(new Guid("51193fdd-67a4-468c-a035-376b28b7a12c"))) {subQueryDictionary[new Guid("51193fdd-67a4-468c-a035-376b28b7a12c")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("51193fdd-67a4-468c-a035-376b28b7a12c")].Add(queryDictionary[new Guid("cd27ee9c-e047-4fac-95fc-f80bb1320dfe")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("51193fdd-67a4-468c-a035-376b28b7a12c"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("51193fdd-67a4-468c-a035-376b28b7a12c")];}
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
		queryDictionary[new Guid("31226d31-41d8-4dd3-80c9-75761ecce887")] = new InputRecordListQuery();
		queryDictionary[new Guid("31226d31-41d8-4dd3-80c9-75761ecce887")].FieldName = null;
		queryDictionary[new Guid("31226d31-41d8-4dd3-80c9-75761ecce887")].FieldValue =  null;
		queryDictionary[new Guid("31226d31-41d8-4dd3-80c9-75761ecce887")].QueryType = "AND";
		queryDictionary[new Guid("31226d31-41d8-4dd3-80c9-75761ecce887")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("97700251-796a-47d2-a206-e46b6ef3d349")] = new InputRecordListQuery();
			queryDictionary[new Guid("97700251-796a-47d2-a206-e46b6ef3d349")].FieldName = "created_by";
			queryDictionary[new Guid("97700251-796a-47d2-a206-e46b6ef3d349")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("97700251-796a-47d2-a206-e46b6ef3d349")].QueryType = "EQ";
			queryDictionary[new Guid("97700251-796a-47d2-a206-e46b6ef3d349")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("97700251-796a-47d2-a206-e46b6ef3d349"))) {queryDictionary[new Guid("97700251-796a-47d2-a206-e46b6ef3d349")].SubQueries = subQueryDictionary[new Guid("97700251-796a-47d2-a206-e46b6ef3d349")];}
			if(!subQueryDictionary.ContainsKey(new Guid("31226d31-41d8-4dd3-80c9-75761ecce887"))) {subQueryDictionary[new Guid("31226d31-41d8-4dd3-80c9-75761ecce887")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("31226d31-41d8-4dd3-80c9-75761ecce887")].Add(queryDictionary[new Guid("97700251-796a-47d2-a206-e46b6ef3d349")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("31226d31-41d8-4dd3-80c9-75761ecce887"))) {queryDictionary[new Guid("31226d31-41d8-4dd3-80c9-75761ecce887")].SubQueries = subQueryDictionary[new Guid("31226d31-41d8-4dd3-80c9-75761ecce887")];}
		if(!subQueryDictionary.ContainsKey(new Guid("bfd7ed7d-712a-4094-bca4-031cc7255593"))) {subQueryDictionary[new Guid("bfd7ed7d-712a-4094-bca4-031cc7255593")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("bfd7ed7d-712a-4094-bca4-031cc7255593")].Add(queryDictionary[new Guid("31226d31-41d8-4dd3-80c9-75761ecce887")]);
		}
		{
		queryDictionary[new Guid("cc9ea546-68ee-4bc4-aba1-55a6363167e1")] = new InputRecordListQuery();
		queryDictionary[new Guid("cc9ea546-68ee-4bc4-aba1-55a6363167e1")].FieldName = null;
		queryDictionary[new Guid("cc9ea546-68ee-4bc4-aba1-55a6363167e1")].FieldValue =  null;
		queryDictionary[new Guid("cc9ea546-68ee-4bc4-aba1-55a6363167e1")].QueryType = "AND";
		queryDictionary[new Guid("cc9ea546-68ee-4bc4-aba1-55a6363167e1")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("b90df058-7f56-41a9-be55-8b1bdbac4c67")] = new InputRecordListQuery();
			queryDictionary[new Guid("b90df058-7f56-41a9-be55-8b1bdbac4c67")].FieldName = "code";
			queryDictionary[new Guid("b90df058-7f56-41a9-be55-8b1bdbac4c67")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("b90df058-7f56-41a9-be55-8b1bdbac4c67")].QueryType = "CONTAINS";
			queryDictionary[new Guid("b90df058-7f56-41a9-be55-8b1bdbac4c67")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("b90df058-7f56-41a9-be55-8b1bdbac4c67"))) {queryDictionary[new Guid("b90df058-7f56-41a9-be55-8b1bdbac4c67")].SubQueries = subQueryDictionary[new Guid("b90df058-7f56-41a9-be55-8b1bdbac4c67")];}
			if(!subQueryDictionary.ContainsKey(new Guid("cc9ea546-68ee-4bc4-aba1-55a6363167e1"))) {subQueryDictionary[new Guid("cc9ea546-68ee-4bc4-aba1-55a6363167e1")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("cc9ea546-68ee-4bc4-aba1-55a6363167e1")].Add(queryDictionary[new Guid("b90df058-7f56-41a9-be55-8b1bdbac4c67")]);
			}
			{
			queryDictionary[new Guid("20284e63-82ab-4df6-928c-240326fe4ee9")] = new InputRecordListQuery();
			queryDictionary[new Guid("20284e63-82ab-4df6-928c-240326fe4ee9")].FieldName = "subject";
			queryDictionary[new Guid("20284e63-82ab-4df6-928c-240326fe4ee9")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("20284e63-82ab-4df6-928c-240326fe4ee9")].QueryType = "CONTAINS";
			queryDictionary[new Guid("20284e63-82ab-4df6-928c-240326fe4ee9")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("20284e63-82ab-4df6-928c-240326fe4ee9"))) {queryDictionary[new Guid("20284e63-82ab-4df6-928c-240326fe4ee9")].SubQueries = subQueryDictionary[new Guid("20284e63-82ab-4df6-928c-240326fe4ee9")];}
			if(!subQueryDictionary.ContainsKey(new Guid("cc9ea546-68ee-4bc4-aba1-55a6363167e1"))) {subQueryDictionary[new Guid("cc9ea546-68ee-4bc4-aba1-55a6363167e1")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("cc9ea546-68ee-4bc4-aba1-55a6363167e1")].Add(queryDictionary[new Guid("20284e63-82ab-4df6-928c-240326fe4ee9")]);
			}
			{
			queryDictionary[new Guid("b148634f-eac2-4bbb-aae5-21c9e07eec3d")] = new InputRecordListQuery();
			queryDictionary[new Guid("b148634f-eac2-4bbb-aae5-21c9e07eec3d")].FieldName = "status";
			queryDictionary[new Guid("b148634f-eac2-4bbb-aae5-21c9e07eec3d")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("b148634f-eac2-4bbb-aae5-21c9e07eec3d")].QueryType = "EQ";
			queryDictionary[new Guid("b148634f-eac2-4bbb-aae5-21c9e07eec3d")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("b148634f-eac2-4bbb-aae5-21c9e07eec3d"))) {queryDictionary[new Guid("b148634f-eac2-4bbb-aae5-21c9e07eec3d")].SubQueries = subQueryDictionary[new Guid("b148634f-eac2-4bbb-aae5-21c9e07eec3d")];}
			if(!subQueryDictionary.ContainsKey(new Guid("cc9ea546-68ee-4bc4-aba1-55a6363167e1"))) {subQueryDictionary[new Guid("cc9ea546-68ee-4bc4-aba1-55a6363167e1")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("cc9ea546-68ee-4bc4-aba1-55a6363167e1")].Add(queryDictionary[new Guid("b148634f-eac2-4bbb-aae5-21c9e07eec3d")]);
			}
			{
			queryDictionary[new Guid("e1616e58-d01d-4a19-8397-c88dc200b684")] = new InputRecordListQuery();
			queryDictionary[new Guid("e1616e58-d01d-4a19-8397-c88dc200b684")].FieldName = "priority";
			queryDictionary[new Guid("e1616e58-d01d-4a19-8397-c88dc200b684")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("e1616e58-d01d-4a19-8397-c88dc200b684")].QueryType = "EQ";
			queryDictionary[new Guid("e1616e58-d01d-4a19-8397-c88dc200b684")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("e1616e58-d01d-4a19-8397-c88dc200b684"))) {queryDictionary[new Guid("e1616e58-d01d-4a19-8397-c88dc200b684")].SubQueries = subQueryDictionary[new Guid("e1616e58-d01d-4a19-8397-c88dc200b684")];}
			if(!subQueryDictionary.ContainsKey(new Guid("cc9ea546-68ee-4bc4-aba1-55a6363167e1"))) {subQueryDictionary[new Guid("cc9ea546-68ee-4bc4-aba1-55a6363167e1")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("cc9ea546-68ee-4bc4-aba1-55a6363167e1")].Add(queryDictionary[new Guid("e1616e58-d01d-4a19-8397-c88dc200b684")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("cc9ea546-68ee-4bc4-aba1-55a6363167e1"))) {queryDictionary[new Guid("cc9ea546-68ee-4bc4-aba1-55a6363167e1")].SubQueries = subQueryDictionary[new Guid("cc9ea546-68ee-4bc4-aba1-55a6363167e1")];}
		if(!subQueryDictionary.ContainsKey(new Guid("bfd7ed7d-712a-4094-bca4-031cc7255593"))) {subQueryDictionary[new Guid("bfd7ed7d-712a-4094-bca4-031cc7255593")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("bfd7ed7d-712a-4094-bca4-031cc7255593")].Add(queryDictionary[new Guid("cc9ea546-68ee-4bc4-aba1-55a6363167e1")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("bfd7ed7d-712a-4094-bca4-031cc7255593"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("bfd7ed7d-712a-4094-bca4-031cc7255593")];}
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
		queryDictionary[new Guid("2e59f846-9a45-4ed3-9f7e-db3a27f8ff1e")] = new InputRecordListQuery();
		queryDictionary[new Guid("2e59f846-9a45-4ed3-9f7e-db3a27f8ff1e")].FieldName = null;
		queryDictionary[new Guid("2e59f846-9a45-4ed3-9f7e-db3a27f8ff1e")].FieldValue =  null;
		queryDictionary[new Guid("2e59f846-9a45-4ed3-9f7e-db3a27f8ff1e")].QueryType = "AND";
		queryDictionary[new Guid("2e59f846-9a45-4ed3-9f7e-db3a27f8ff1e")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("9cc0b5ec-4ee2-421b-9a87-50ce6df30c18")] = new InputRecordListQuery();
			queryDictionary[new Guid("9cc0b5ec-4ee2-421b-9a87-50ce6df30c18")].FieldName = "owner_id";
			queryDictionary[new Guid("9cc0b5ec-4ee2-421b-9a87-50ce6df30c18")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("9cc0b5ec-4ee2-421b-9a87-50ce6df30c18")].QueryType = "EQ";
			queryDictionary[new Guid("9cc0b5ec-4ee2-421b-9a87-50ce6df30c18")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("9cc0b5ec-4ee2-421b-9a87-50ce6df30c18"))) {queryDictionary[new Guid("9cc0b5ec-4ee2-421b-9a87-50ce6df30c18")].SubQueries = subQueryDictionary[new Guid("9cc0b5ec-4ee2-421b-9a87-50ce6df30c18")];}
			if(!subQueryDictionary.ContainsKey(new Guid("2e59f846-9a45-4ed3-9f7e-db3a27f8ff1e"))) {subQueryDictionary[new Guid("2e59f846-9a45-4ed3-9f7e-db3a27f8ff1e")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("2e59f846-9a45-4ed3-9f7e-db3a27f8ff1e")].Add(queryDictionary[new Guid("9cc0b5ec-4ee2-421b-9a87-50ce6df30c18")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("2e59f846-9a45-4ed3-9f7e-db3a27f8ff1e"))) {queryDictionary[new Guid("2e59f846-9a45-4ed3-9f7e-db3a27f8ff1e")].SubQueries = subQueryDictionary[new Guid("2e59f846-9a45-4ed3-9f7e-db3a27f8ff1e")];}
		if(!subQueryDictionary.ContainsKey(new Guid("71a9c1be-ae22-474f-844e-76a407a63bc8"))) {subQueryDictionary[new Guid("71a9c1be-ae22-474f-844e-76a407a63bc8")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("71a9c1be-ae22-474f-844e-76a407a63bc8")].Add(queryDictionary[new Guid("2e59f846-9a45-4ed3-9f7e-db3a27f8ff1e")]);
		}
		{
		queryDictionary[new Guid("6afa7322-1627-49b2-8a05-2ec2f33a52f4")] = new InputRecordListQuery();
		queryDictionary[new Guid("6afa7322-1627-49b2-8a05-2ec2f33a52f4")].FieldName = null;
		queryDictionary[new Guid("6afa7322-1627-49b2-8a05-2ec2f33a52f4")].FieldValue =  null;
		queryDictionary[new Guid("6afa7322-1627-49b2-8a05-2ec2f33a52f4")].QueryType = "AND";
		queryDictionary[new Guid("6afa7322-1627-49b2-8a05-2ec2f33a52f4")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("1d828034-4ad5-4bf8-a9ff-8df7981245e7")] = new InputRecordListQuery();
			queryDictionary[new Guid("1d828034-4ad5-4bf8-a9ff-8df7981245e7")].FieldName = "code";
			queryDictionary[new Guid("1d828034-4ad5-4bf8-a9ff-8df7981245e7")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("1d828034-4ad5-4bf8-a9ff-8df7981245e7")].QueryType = "CONTAINS";
			queryDictionary[new Guid("1d828034-4ad5-4bf8-a9ff-8df7981245e7")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("1d828034-4ad5-4bf8-a9ff-8df7981245e7"))) {queryDictionary[new Guid("1d828034-4ad5-4bf8-a9ff-8df7981245e7")].SubQueries = subQueryDictionary[new Guid("1d828034-4ad5-4bf8-a9ff-8df7981245e7")];}
			if(!subQueryDictionary.ContainsKey(new Guid("6afa7322-1627-49b2-8a05-2ec2f33a52f4"))) {subQueryDictionary[new Guid("6afa7322-1627-49b2-8a05-2ec2f33a52f4")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("6afa7322-1627-49b2-8a05-2ec2f33a52f4")].Add(queryDictionary[new Guid("1d828034-4ad5-4bf8-a9ff-8df7981245e7")]);
			}
			{
			queryDictionary[new Guid("66b2532f-db39-4369-a150-0d148c0a2b57")] = new InputRecordListQuery();
			queryDictionary[new Guid("66b2532f-db39-4369-a150-0d148c0a2b57")].FieldName = "subject";
			queryDictionary[new Guid("66b2532f-db39-4369-a150-0d148c0a2b57")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("66b2532f-db39-4369-a150-0d148c0a2b57")].QueryType = "CONTAINS";
			queryDictionary[new Guid("66b2532f-db39-4369-a150-0d148c0a2b57")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("66b2532f-db39-4369-a150-0d148c0a2b57"))) {queryDictionary[new Guid("66b2532f-db39-4369-a150-0d148c0a2b57")].SubQueries = subQueryDictionary[new Guid("66b2532f-db39-4369-a150-0d148c0a2b57")];}
			if(!subQueryDictionary.ContainsKey(new Guid("6afa7322-1627-49b2-8a05-2ec2f33a52f4"))) {subQueryDictionary[new Guid("6afa7322-1627-49b2-8a05-2ec2f33a52f4")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("6afa7322-1627-49b2-8a05-2ec2f33a52f4")].Add(queryDictionary[new Guid("66b2532f-db39-4369-a150-0d148c0a2b57")]);
			}
			{
			queryDictionary[new Guid("b5565449-7547-4803-b7b9-eac1325f825a")] = new InputRecordListQuery();
			queryDictionary[new Guid("b5565449-7547-4803-b7b9-eac1325f825a")].FieldName = "status";
			queryDictionary[new Guid("b5565449-7547-4803-b7b9-eac1325f825a")].FieldValue =  "closed";
			queryDictionary[new Guid("b5565449-7547-4803-b7b9-eac1325f825a")].QueryType = "NOT";
			queryDictionary[new Guid("b5565449-7547-4803-b7b9-eac1325f825a")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("b5565449-7547-4803-b7b9-eac1325f825a"))) {queryDictionary[new Guid("b5565449-7547-4803-b7b9-eac1325f825a")].SubQueries = subQueryDictionary[new Guid("b5565449-7547-4803-b7b9-eac1325f825a")];}
			if(!subQueryDictionary.ContainsKey(new Guid("6afa7322-1627-49b2-8a05-2ec2f33a52f4"))) {subQueryDictionary[new Guid("6afa7322-1627-49b2-8a05-2ec2f33a52f4")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("6afa7322-1627-49b2-8a05-2ec2f33a52f4")].Add(queryDictionary[new Guid("b5565449-7547-4803-b7b9-eac1325f825a")]);
			}
			{
			queryDictionary[new Guid("391c37a4-3613-47ff-beb6-139c76a59271")] = new InputRecordListQuery();
			queryDictionary[new Guid("391c37a4-3613-47ff-beb6-139c76a59271")].FieldName = "priority";
			queryDictionary[new Guid("391c37a4-3613-47ff-beb6-139c76a59271")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("391c37a4-3613-47ff-beb6-139c76a59271")].QueryType = "EQ";
			queryDictionary[new Guid("391c37a4-3613-47ff-beb6-139c76a59271")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("391c37a4-3613-47ff-beb6-139c76a59271"))) {queryDictionary[new Guid("391c37a4-3613-47ff-beb6-139c76a59271")].SubQueries = subQueryDictionary[new Guid("391c37a4-3613-47ff-beb6-139c76a59271")];}
			if(!subQueryDictionary.ContainsKey(new Guid("6afa7322-1627-49b2-8a05-2ec2f33a52f4"))) {subQueryDictionary[new Guid("6afa7322-1627-49b2-8a05-2ec2f33a52f4")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("6afa7322-1627-49b2-8a05-2ec2f33a52f4")].Add(queryDictionary[new Guid("391c37a4-3613-47ff-beb6-139c76a59271")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("6afa7322-1627-49b2-8a05-2ec2f33a52f4"))) {queryDictionary[new Guid("6afa7322-1627-49b2-8a05-2ec2f33a52f4")].SubQueries = subQueryDictionary[new Guid("6afa7322-1627-49b2-8a05-2ec2f33a52f4")];}
		if(!subQueryDictionary.ContainsKey(new Guid("71a9c1be-ae22-474f-844e-76a407a63bc8"))) {subQueryDictionary[new Guid("71a9c1be-ae22-474f-844e-76a407a63bc8")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("71a9c1be-ae22-474f-844e-76a407a63bc8")].Add(queryDictionary[new Guid("6afa7322-1627-49b2-8a05-2ec2f33a52f4")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("71a9c1be-ae22-474f-844e-76a407a63bc8"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("71a9c1be-ae22-474f-844e-76a407a63bc8")];}
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

#region << ***Update view***  Entity: wv_project_attachment View Name: create >>
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
					#region << comment_content >>
					{
						var viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73");
						viewItem.EntityName = "wv_project_attachment";
						viewItem.FieldId = new Guid("501a2446-be58-4a04-a374-b74d041c8368");
						viewItem.FieldName = "comment_content";
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
		var response = entMan.UpdateRecordView(new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_project_attachment Updated view: create Message:" + response.Message);
	}
}
#endregion

#region << ***Update list***  Entity: wv_project_comment List Name: task_comments >>
{
	var createListEntity = entMan.ReadEntity(new Guid("7a57d17e-98f0-4356-baf0-9a8798da0b99")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "task_comments").Id;
	createListInput.Type =  "Hidden";
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
	createListInput.ServiceCode = "";
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
		#region << field from Relation: file >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73");
			listItemFromRelation.EntityName = "wv_project_attachment";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("6d639a8c-e220-4d9f-86f0-de6ba03030b8");
			listItemFromRelation.FieldName = "file";
			listItemFromRelation.FieldLabel = "File";
			listItemFromRelation.FieldPlaceholder = "";
			listItemFromRelation.FieldHelpText = "";
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = "";
			listItemFromRelation.RelationId = new Guid("3e3a95f0-65fd-4e37-aa17-8879b1c207b8");
			listItemFromRelation.RelationName = "attachment_1_n_comment";
			createListInput.Columns.Add(listItemFromRelation);
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

#region << ***Update list***  Entity: wv_project_comment List Name: bug_comments >>
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
		#region << field from Relation: file >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("f3dedc72-556a-4088-8278-bb5e8a8aad73");
			listItemFromRelation.EntityName = "wv_project_attachment";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("6d639a8c-e220-4d9f-86f0-de6ba03030b8");
			listItemFromRelation.FieldName = "file";
			listItemFromRelation.FieldLabel = "File";
			listItemFromRelation.FieldPlaceholder = "";
			listItemFromRelation.FieldHelpText = "";
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = "";
			listItemFromRelation.RelationId = new Guid("3e3a95f0-65fd-4e37-aa17-8879b1c207b8");
			listItemFromRelation.RelationName = "attachment_1_n_comment";
			createListInput.Columns.Add(listItemFromRelation);
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

#region << ***Update list***  Entity: wv_customer List Name: general >>
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
	createListInput.Query = new InputRecordListQuery();
	var queryDictionary = new Dictionary<Guid,InputRecordListQuery>();
	var subQueryDictionary = new Dictionary<Guid,List<InputRecordListQuery>>();
	//Main query rule
	createListInput.Query.FieldName = null;
	createListInput.Query.FieldValue =  null;
	createListInput.Query.QueryType = "AND";
	createListInput.Query.SubQueries = new List<InputRecordListQuery>();
		{
		queryDictionary[new Guid("148cc175-f9f2-40e0-b3fb-7f6f7aa7c4d3")] = new InputRecordListQuery();
		queryDictionary[new Guid("148cc175-f9f2-40e0-b3fb-7f6f7aa7c4d3")].FieldName = "id";
		queryDictionary[new Guid("148cc175-f9f2-40e0-b3fb-7f6f7aa7c4d3")].FieldValue =  "";
		queryDictionary[new Guid("148cc175-f9f2-40e0-b3fb-7f6f7aa7c4d3")].QueryType = "EQ";
		queryDictionary[new Guid("148cc175-f9f2-40e0-b3fb-7f6f7aa7c4d3")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("148cc175-f9f2-40e0-b3fb-7f6f7aa7c4d3"))) {queryDictionary[new Guid("148cc175-f9f2-40e0-b3fb-7f6f7aa7c4d3")].SubQueries = subQueryDictionary[new Guid("148cc175-f9f2-40e0-b3fb-7f6f7aa7c4d3")];}
		if(!subQueryDictionary.ContainsKey(new Guid("dec43d8b-a5f7-4091-abc1-ecd0b3c9466a"))) {subQueryDictionary[new Guid("dec43d8b-a5f7-4091-abc1-ecd0b3c9466a")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("dec43d8b-a5f7-4091-abc1-ecd0b3c9466a")].Add(queryDictionary[new Guid("148cc175-f9f2-40e0-b3fb-7f6f7aa7c4d3")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("dec43d8b-a5f7-4091-abc1-ecd0b3c9466a"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("dec43d8b-a5f7-4091-abc1-ecd0b3c9466a")];}
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


		}


    }
}
