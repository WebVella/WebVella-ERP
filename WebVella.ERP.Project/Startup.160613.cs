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
		private static void Patch160613(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan, bool createSampleRecords = false)
		{

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

			#region << Update  Enity: wv_bug View: general >>
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

			#region << Create  Enity: wv_task field: estimation >>
			{
				InputNumberField numberField = new InputNumberField();
				numberField.Id = new Guid("848e2a24-8d58-451b-9cf8-9ba1254a169a");
				numberField.Name = "estimation";
				numberField.Label = "Estimation";
				numberField.PlaceholderText = "";
				numberField.Description = "";
				numberField.HelpText = "";
				numberField.Required = true;
				numberField.Unique = false;
				numberField.Searchable = false;
				numberField.Auditable = false;
				numberField.System = false;
				numberField.DefaultValue = Decimal.Parse("0.0");
				numberField.MinValue = null;
				numberField.MaxValue = null;
				numberField.DecimalPlaces = byte.Parse("2");
				numberField.EnableSecurity = false;
				numberField.Permissions = new FieldPermissions();
				numberField.Permissions.CanRead = new List<Guid>();
				numberField.Permissions.CanUpdate = new List<Guid>();
				//READ
				//UPDATE
				{
					var response = entMan.CreateField(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89"), numberField, false);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: wv_task Field: estimation Message:" + response.Message);
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

			#region << Update  Enity: wv_task View: general >>
			{
				var updateViewEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
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




		}


	}
}
