using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WebVella.ERP;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Api.Models.AutoMapper;
using WebVella.ERP.Crm.Models;
using WebVella.ERP.Database;
using WebVella.ERP.Plugins;
using WebVella.ERP.Utilities;

namespace WebVella.Crm.Project
{
	[PluginStartup]
	public class Startup
	{
		//System elements	
		// Check the SystemIds for lot's of helpful constants you may need, e.g. SystemIds.UserEntityId

		//Code snippets
		//Check out the CodeSnippets.txt file in WebVella.ERP.Web > Docs folder for code pieces on how to create or update some elements		

		private bool createSampleRecords = false;
		//Constants
		private static Guid WEBVELLA_CRM_PLUGIN_ID = new Guid("7b44d010-a856-449f-b415-507efe869ccd");
		private static string WEBVELLA_CRM_PLUGIN_NAME = "webvella-crm";
		private static Guid CUSTOMER_ENTITY_ID = new Guid("90bcdb47-2cde-4137-a412-0198348fecc0");
		private static string CUSTOMER_ENTITY_NAME = "wv_customer";
		private static Guid CRM_ADMIN_AREA_ID = new Guid("0e3a02c4-64df-4b61-bae5-8b3cc847f129");

		public void Start()
		{

			var entMan = new EntityManager();
			var relMan = new EntityRelationManager();
			var recMan = new RecordManager();
			var storeSystemSettings = DbContext.Current.SettingsRepository.Read();
			var systemSettings = new SystemSettings(storeSystemSettings);

			using (SecurityContext.OpenSystemScope())
			{

				//Create transaction
				using (var connection = DbContext.Current.CreateConnection())
				{
					try
					{
						connection.BeginTransaction();
						//Here we need to initialize or update the environment based on the plugin requirements.
						//The default place for the plugin data is the "plugin_data" entity -> the "data" text field, which is used to store stringified JSON
						//containing the plugin settings or version

						#region << 1.Get the current ERP database version and checks for other plugin dependencies >>

						if (systemSettings.Version > 0)
						{
							//Do something if database version is not what you expect
						}

						//This plugin needs the webvella-crm plugin to be installed, so we will check this here
						var installedPlugins = new PluginService().Plugins;
						var corePluginFound = false;
						foreach (var plugin in installedPlugins)
						{
							if (plugin.Name == "webvella-core")
							{
								corePluginFound = true;
								break;
							}
						}

						if (!corePluginFound)
							throw new Exception("'webvella-core' plugin is required for the 'webvella-crm' to operate");

						#endregion

						#region << 2.Get the current plugin settings from the database >>
						var currentPluginSettings = new PluginSettings();
						QueryObject pluginDataQueryObject = EntityQuery.QueryEQ("name", WEBVELLA_CRM_PLUGIN_NAME);
						var pluginDataQuery = new EntityQuery("plugin_data", "*", pluginDataQueryObject);
						var pluginDataQueryResponse = recMan.Find(pluginDataQuery);
						if (!pluginDataQueryResponse.Success)
							throw new Exception("plugin 'webvella-project' failed to get its settings due to: " + pluginDataQueryResponse.Message);

						if (pluginDataQueryResponse.Object == null || !pluginDataQueryResponse.Object.Data.Any() || pluginDataQueryResponse.Object.Data[0]["data"] == DBNull.Value)
						{
							//plugin was not installed
							currentPluginSettings.Version = 20160429;
							{
								string json = JsonConvert.SerializeObject(currentPluginSettings);
								var settingsEntityRecord = new EntityRecord();
								settingsEntityRecord["id"] = WEBVELLA_CRM_PLUGIN_ID;
								settingsEntityRecord["name"] = WEBVELLA_CRM_PLUGIN_NAME;
								settingsEntityRecord["data"] = json;
								var settingsSaveReponse = recMan.CreateRecord("plugin_data", settingsEntityRecord);
								if (!settingsSaveReponse.Success)
									throw new Exception("plugin 'webvella-project' failed to save its settings in the database due to: " + pluginDataQueryResponse.Message);
							}
						}
						else
						{
							string json = (string)((List<EntityRecord>)pluginDataQueryResponse.Object.Data)[0]["data"];
							currentPluginSettings = JsonConvert.DeserializeObject<PluginSettings>(json);
						}
						#endregion

						#region << 3. Run methods based on the current installed version of the plugin >>
						if (currentPluginSettings.Version < 20160430)
						{
							currentPluginSettings.Version = 20160430;

							#region << Create CRM admin area >>
							//The areas are the main object for navigation for the user. You can attach entities and URLs later to them
							{
								var area = new EntityRecord();
								area["id"] = CRM_ADMIN_AREA_ID;
								area["name"] = "crm_admin";
								area["label"] = "CRM Admin";
								area["icon_name"] = "users";
								area["color"] = "pink";
								area["folder"] = "Admin";
								area["weight"] = 100;
								var areaRoles = new List<Guid>();
								areaRoles.Add(SystemIds.AdministratorRoleId);
								area["roles"] = JsonConvert.SerializeObject(areaRoles);
								var createAreaResult = recMan.CreateRecord("area", area);
								if (!createAreaResult.Success)
								{
									throw new Exception("System error 10060. Area create with name : project_admin. Message:" + createAreaResult.Message);
								}
							}
							#endregion

							#region << wv_customer >>
							{
								#region << entity >>
								{
									InputEntity entity = new InputEntity();
									entity.Id = CUSTOMER_ENTITY_ID;
									entity.Name = CUSTOMER_ENTITY_NAME;
									entity.Label = "Customer";
									entity.LabelPlural = "Customers";
									entity.System = true;
									entity.IconName = "building-o";
									entity.Weight = 2;
									entity.RecordPermissions = new RecordPermissions();
									entity.RecordPermissions.CanCreate = new List<Guid>();
									entity.RecordPermissions.CanRead = new List<Guid>();
									entity.RecordPermissions.CanUpdate = new List<Guid>();
									entity.RecordPermissions.CanDelete = new List<Guid>();
									//Create
									entity.RecordPermissions.CanCreate.Add(SystemIds.AdministratorRoleId);
									entity.RecordPermissions.CanCreate.Add(SystemIds.RegularRoleId);
									//READ
									entity.RecordPermissions.CanRead.Add(SystemIds.AdministratorRoleId);
									entity.RecordPermissions.CanRead.Add(SystemIds.RegularRoleId);
									//UPDATE
									entity.RecordPermissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
									entity.RecordPermissions.CanUpdate.Add(SystemIds.RegularRoleId);
									//System fields and relations Ids should be hardcoded for the compare/change code generation to work later on correctly
									var systemItemIdDictionary = new Dictionary<string, Guid>();
									systemItemIdDictionary["id"] = new Guid("e8564a56-9917-4c45-a264-4c080f2d2b31");
									systemItemIdDictionary["created_on"] = new Guid("1ca9c660-0042-47b3-91c7-e0f2b817633c");
									systemItemIdDictionary["created_by"] = new Guid("12c29d6a-b5e5-4293-9e8a-91475d10dac9");
									systemItemIdDictionary["last_modified_on"] = new Guid("b6a172aa-b50a-403f-a178-f923a0d7576f");
									systemItemIdDictionary["last_modified_by"] = new Guid("6aab2af3-6909-4f8d-8c54-6a2403d6a25a");
									systemItemIdDictionary["user_wv_customer_created_by"] = new Guid("2d2045f4-0553-4ffd-bd12-cbef17aea14a");
									systemItemIdDictionary["user_wv_customer_modified_by"] = new Guid("2ba6c8c5-2550-4f26-8611-f397c529a61c");
									{
										var response = entMan.CreateEntity(entity, false, false, systemItemIdDictionary);
										if (!response.Success)
											throw new Exception("System error 10050. Entity: " + CUSTOMER_ENTITY_NAME + " Field: entity creation" + " Message:" + response.Message);
									}
								}
								#endregion

								#region << name >>
								{
									InputTextField textboxField = new InputTextField();
									textboxField.Id = new Guid("7fb95d0f-ab59-421d-974d-ab357e28a1f9");
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
									textboxField.DefaultValue = string.Empty;
									textboxField.MaxLength = null;
									textboxField.EnableSecurity = true;
									textboxField.Permissions = new FieldPermissions();
									textboxField.Permissions.CanRead = new List<Guid>();
									textboxField.Permissions.CanUpdate = new List<Guid>();
									//READ
									textboxField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
									textboxField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
									//UPDATE
									textboxField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
									{
										var response = entMan.CreateField(CUSTOMER_ENTITY_ID, textboxField, false);
										if (!response.Success)
											throw new Exception("System error 10060. Entity: " + CUSTOMER_ENTITY_NAME + " Field: name" + " Message:" + response.Message);
									}
								}
								#endregion

							}
							#endregion

							#region << View name: admin_details >>
							{
								var createViewEntity = entMan.ReadEntity(CUSTOMER_ENTITY_ID).Object;
								var createViewInput = new InputRecordView();
								var viewRegion = new InputRecordViewRegion();
								var viewSection = new InputRecordViewSection();
								var viewRow = new InputRecordViewRow();
								var viewColumn = new InputRecordViewColumn();
								var viewItem = new InputRecordViewFieldItem();

								#region << details >>
								createViewInput.Id = new Guid("3a0e1319-5357-49ec-9e85-8d9be2363fcf");
								createViewInput.Type = "hidden";
								createViewInput.Name = "admin_details";
								createViewInput.Label = "Details";
								createViewInput.Default = false;
								createViewInput.System = false;
								createViewInput.Weight = 15;
								createViewInput.CssClass = null;
								createViewInput.IconName = "building-o";
								createViewInput.DynamicHtmlTemplate = null;
								createViewInput.DataSourceUrl = null;
								createViewInput.ServiceCode = null;
								createViewInput.Regions = new List<InputRecordViewRegion>();
								#endregion

								#region << Header Region >>
								viewRegion = new InputRecordViewRegion();
								viewRegion.Name = "header";
								viewRegion.Label = "Header";
								viewRegion.Render = true;
								viewRegion.Weight = 1;
								viewRegion.CssClass = "";
								viewRegion.Sections = new List<InputRecordViewSection>();

								#region << Section >>
								viewSection = new InputRecordViewSection();
								viewSection.Id = Guid.NewGuid();
								viewSection.Name = "details";
								viewSection.Label = "Details";
								viewSection.ShowLabel = false;
								viewSection.CssClass = "";
								viewSection.Collapsed = false;
								viewSection.TabOrder = "left-right";
								viewSection.Weight = 1;
								viewSection.Rows = new List<InputRecordViewRow>();

								#region << Row >>
								viewRow = new InputRecordViewRow();
								viewRow.Id = Guid.NewGuid();
								viewRow.Weight = 1;
								viewRow.Columns = new List<InputRecordViewColumn>();

								#region << Column 1 >>
								viewColumn = new InputRecordViewColumn();
								viewColumn.GridColCount = 12;
								viewColumn.Items = new List<InputRecordViewItemBase>();


								#region << name >>
								{
									viewItem = new InputRecordViewFieldItem();
									viewItem.EntityId = CUSTOMER_ENTITY_ID;
									viewItem.EntityName = CUSTOMER_ENTITY_NAME;
									viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "name").Id;
									viewItem.FieldName = "name";
									viewItem.Type = "field";
									viewColumn.Items.Add(viewItem);
								}
								#endregion


								//Save column
								viewRow.Columns.Add(viewColumn);
								#endregion

								//Save row
								viewSection.Rows.Add(viewRow);
								#endregion

								//Save section
								viewRegion.Sections.Add(viewSection);
								#endregion

								//Save region
								createViewInput.Regions.Add(viewRegion);
								#endregion

								#region << relation options >>
								createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
								#endregion

								#region << Sidebar >>
								createViewInput.Sidebar = new InputRecordViewSidebar();
								createViewInput.Sidebar.CssClass = "";
								createViewInput.Sidebar.Render = true;
								createViewInput.Sidebar.Render = true;
								createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();
								#endregion

								#region << action items >>
								createViewInput.ActionItems = new List<ActionItem>();
								var actionItem = new ActionItem();
								{
									actionItem = new ActionItem();
									actionItem.Name = "wv_record_delete";
									actionItem.Menu = "page-title-dropdown";
									actionItem.Weight = 1;
									actionItem.Template = "" +
							@"<a href=""javascript:void(0)"" confirmed-click=""ngCtrl.deleteRecord(ngCtrl)"" ng-confirm-click=""Are you sure?""
										ng-if=""::ngCtrl.userHasRecordPermissions('canDelete')"">
									<i class=""fa fa-trash go-red""></i> Delete Record
								</a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								{
									actionItem = new ActionItem();
									actionItem.Name = "wv_back_button";
									actionItem.Menu = "sidebar-top";
									actionItem.Weight = 1;
									actionItem.Template = "" +
							@"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion

								{
									var response = entMan.CreateRecordView(CUSTOMER_ENTITY_ID, createViewInput);
									if (!response.Success)
										throw new Exception("System error 10060. Entity: " + CUSTOMER_ENTITY_NAME + " Updated view: admin_details" + " Message:" + response.Message);
								}
							}
							#endregion

							#region << View name: admin_create >>
							{
								var createViewEntity = entMan.ReadEntity(CUSTOMER_ENTITY_ID).Object;
								var createViewInput = new InputRecordView();
								var viewRegion = new InputRecordViewRegion();
								var viewSection = new InputRecordViewSection();
								var viewRow = new InputRecordViewRow();
								var viewColumn = new InputRecordViewColumn();
								var viewItem = new InputRecordViewFieldItem();

								#region << details >>
								createViewInput.Id = new Guid("93043954-ae70-41a3-b4b7-665531a23a76");
								createViewInput.Type = "create";
								createViewInput.Name = "admin_create";
								createViewInput.Label = "Create customer";
								createViewInput.Default = false;
								createViewInput.System = false;
								createViewInput.Weight = 25;
								createViewInput.CssClass = null;
								createViewInput.IconName = "building-o";
								createViewInput.DynamicHtmlTemplate = null;
								createViewInput.DataSourceUrl = null;
								createViewInput.ServiceCode = null;
								createViewInput.Regions = new List<InputRecordViewRegion>();
								#endregion

								#region << Header Region >>
								viewRegion = new InputRecordViewRegion();
								viewRegion.Name = "header";
								viewRegion.Label = "Header";
								viewRegion.Render = true;
								viewRegion.Weight = 1;
								viewRegion.CssClass = "";
								viewRegion.Sections = new List<InputRecordViewSection>();

								#region << Section >>
								viewSection = new InputRecordViewSection();
								viewSection.Id = Guid.NewGuid();
								viewSection.Name = "details";
								viewSection.Label = "Details";
								viewSection.ShowLabel = false;
								viewSection.CssClass = "";
								viewSection.Collapsed = false;
								viewSection.TabOrder = "left-right";
								viewSection.Weight = 1;
								viewSection.Rows = new List<InputRecordViewRow>();

								#region << Row >>
								viewRow = new InputRecordViewRow();
								viewRow.Id = Guid.NewGuid();
								viewRow.Weight = 1;
								viewRow.Columns = new List<InputRecordViewColumn>();

								#region << Column 1 >>
								viewColumn = new InputRecordViewColumn();
								viewColumn.GridColCount = 12;
								viewColumn.Items = new List<InputRecordViewItemBase>();


								#region << name >>
								{
									viewItem = new InputRecordViewFieldItem();
									viewItem.EntityId = CUSTOMER_ENTITY_ID;
									viewItem.EntityName = CUSTOMER_ENTITY_NAME;
									viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "name").Id;
									viewItem.FieldName = "name";
									viewItem.Type = "field";
									viewColumn.Items.Add(viewItem);
								}
								#endregion


								//Save column
								viewRow.Columns.Add(viewColumn);
								#endregion

								//Save row
								viewSection.Rows.Add(viewRow);
								#endregion

								//Save section
								viewRegion.Sections.Add(viewSection);
								#endregion

								//Save region
								createViewInput.Regions.Add(viewRegion);
								#endregion

								#region << relation options >>
								createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
								#endregion

								#region << Sidebar >>
								createViewInput.Sidebar = new InputRecordViewSidebar();
								createViewInput.Sidebar.CssClass = "";
								createViewInput.Sidebar.Render = true;
								createViewInput.Sidebar.Render = true;
								createViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();
								#endregion

								#region << action items >>
								createViewInput.ActionItems = new List<ActionItem>();
								var actionItem = new ActionItem();

								{
									actionItem = new ActionItem();
									actionItem.Name = "wv_create_and_list";
									actionItem.Menu = "create-bottom";
									actionItem.Weight = 1;
									actionItem.Template = "" +
							@"<a class=""btn btn-primary"" ng-click='ngCtrl.create(""default"")' ng-if=""ngCtrl.createViewRegion != null"">Create</a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								{
									actionItem = new ActionItem();
									actionItem.Name = "wv_create_and_details";
									actionItem.Menu = "create-bottom";
									actionItem.Weight = 2;
									actionItem.Template = "" +
							@"<a class=""btn btn-default btn-outline"" ng-click='ngCtrl.create(""details"")' ng-if=""ngCtrl.createViewRegion != null"">Create & Details</a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								{
									actionItem = new ActionItem();
									actionItem.Name = "wv_create_cancel";
									actionItem.Menu = "create-bottom";
									actionItem.Weight = 3;
									actionItem.Template = "" +
							@"<a class=""btn btn-default btn-outline"" ng-click=""ngCtrl.cancel()"">Cancel</a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								{
									actionItem = new ActionItem();
									actionItem.Name = "wv_back_button";
									actionItem.Menu = "sidebar-top";
									actionItem.Weight = 1;
									actionItem.Template = "" +
							@"<a class=""back clearfix"" href=""javascript:void(0)"" ng-click=""sidebarData.goBack()""><i class=""fa fa-fw fa-arrow-left""></i> <span class=""text"">Back</span></a>";
									createViewInput.ActionItems.Add(actionItem);
								}
								#endregion
								{
									var response = entMan.CreateRecordView(CUSTOMER_ENTITY_ID, createViewInput);
									if (!response.Success)
										throw new Exception("System error 10060. Entity: " + CUSTOMER_ENTITY_NAME + " Updated view: admin_create" + " Message:" + response.Message);
								}
							}
							#endregion

							#region << List name: admin >>
							{
								var createListEntity = entMan.ReadEntity(CUSTOMER_ENTITY_ID).Object;
								var createListInput = new InputRecordList();
								var listItem = new InputRecordListFieldItem();
								var listSort = new InputRecordListSort();
								var listQuery = new InputRecordListQuery();

								#region << details >>
								createListInput.Id = new Guid("ff15f200-8e68-4683-8576-4c8244405ca9");
								createListInput.Type = "hidden";
								createListInput.Name = "admin";
								createListInput.Label = "Customers";
								createListInput.Weight = 11;
								createListInput.Default = false;
								createListInput.System = true;
								createListInput.CssClass = null;
								createListInput.IconName = "building-o";
								createListInput.VisibleColumnsCount = 7;
								createListInput.ColumnWidthsCSV = null;
								createListInput.PageSize = 10;
								createListInput.DynamicHtmlTemplate = null;
								createListInput.DataSourceUrl = null;
								createListInput.ServiceCode = null;
								#endregion

								#region << action items >>
								createListInput.ActionItems = new List<ActionItem>();
								var actionItem = new ActionItem();
								{
									actionItem = new ActionItem();
									actionItem.Name = "wv_record_details";
									actionItem.Menu = "record-row";
									actionItem.Weight = 1;
									actionItem.Template = "" +
							@"<a class=""btn btn-default btn-outline"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record, ngCtrl)}}"">
							<i class=""fa fa-fw fa-eye""></i>
							</a>";
									createListInput.ActionItems.Add(actionItem);
								}
								{
									actionItem = new ActionItem();
									actionItem.Name = "wv_create_record";
									actionItem.Menu = "page-title";
									actionItem.Weight = 1;
									actionItem.Template = "" +
@"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
    ng-href=""{{::ngCtrl.getRecordCreateUrl(ngCtrl)}}"">Add New</a>";
									createListInput.ActionItems.Add(actionItem);
								}
								#endregion

								#region << Columns >>
								createListInput.Columns = new List<InputRecordListItemBase>();
								#region << name >>
								{
									var fieldName = "name";
									listItem = new InputRecordListFieldItem();
									listItem.EntityId = CUSTOMER_ENTITY_ID;
									listItem.EntityName = CUSTOMER_ENTITY_NAME;
									listItem.FieldId = createListEntity.Fields.Single(x => x.Name == fieldName).Id;
									listItem.FieldName = fieldName;
									listItem.Type = "field";
									createListInput.Columns.Add(listItem);
								}
								#endregion

								#endregion

								#region << relation options >>
								createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
								#endregion

								#region << query >>
								listQuery = new InputRecordListQuery();
								#endregion

								#region << Sort >>
								listSort = new InputRecordListSort();
								#endregion
								{
									var response = entMan.CreateRecordList(CUSTOMER_ENTITY_ID, createListInput);
									if (!response.Success)
										throw new Exception("System error 10060. Entity: " + CUSTOMER_ENTITY_NAME + " Updated List: admin" + " Message:" + response.Message);
								}
							}
							#endregion

							#region << area add subscription: CRM Admin -> Customer >>
							{
								var updatedAreaId = CRM_ADMIN_AREA_ID;
								var updateAreaResult = Helpers.UpsertEntityAsAreaSubscription(entMan, recMan, updatedAreaId, CUSTOMER_ENTITY_NAME, "admin_details", "admin_create", "admin");
								if (!updateAreaResult.Success)
								{
									throw new Exception("System error 10060. Area update with id : " + updatedAreaId + " Message:" + updateAreaResult.Message);
								}
							}
							#endregion

							#region << customer lookup list >>
							{
								var createListEntity = entMan.ReadEntity(CUSTOMER_ENTITY_ID).Object;
								var createListInput = new InputRecordList();
								var listItem = new InputRecordListFieldItem();
								var listSort = new InputRecordListSort();
								var listQuery = new InputRecordListQuery();

								//General list details
								createListInput.Id = new Guid("2287d4dc-0e9e-4c00-a1d4-cc2b8bf0f315");	
								createListInput.Name = "lookup";
								createListInput.Label = "Lookup";
								createListInput.Type = "lookup";
								createListInput.Default = true;
								createListInput.System = true;
								createListInput.Weight = 10;
								createListInput.CssClass = null;
								createListInput.IconName = "list";
								createListInput.Columns = new List<InputRecordListItemBase>();
								//Fields
								#region << name >>
								listItem = new InputRecordListFieldItem();
								listItem.EntityId = CUSTOMER_ENTITY_ID;
								listItem.EntityName = "customer";
								listItem.FieldId = createListEntity.Fields.Single(x => x.Name == "name").Id;
								listItem.FieldName = "name";
								listItem.Type = "field";
								createListInput.Columns.Add(listItem);
								#endregion

								//Query
								#region << query descr >>
								listQuery = new InputRecordListQuery();
								#endregion


								//Sort
								#region << Sort >>
								createListInput.Sorts = new List<InputRecordListSort>();
								listSort = new InputRecordListSort();
								listSort.FieldName = "name";
								listSort.SortType = "ascending";
								createListInput.Sorts.Add(listSort);
								#endregion

								var newActionItemList = new List<ActionItem>();
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_create_record";
									actionItem.Menu = "page-title";
									actionItem.Template = "<a class=\"btn btn-default btn-outline hidden-xs\" ng-show=\"::ngCtrl.userHasRecordPermissions('canCreate')\" \r\n    ng-href=\"{{::ngCtrl.getRecordCreateUrl()}}\">Add New</a>";
									actionItem.Weight = 1;
									newActionItemList.Add(actionItem);
								}
								{
									var actionItem = new ActionItem();
									actionItem.Name = "wv_record_details";
									actionItem.Menu = "record-row";
									actionItem.Template = "<a class=\"btn btn-default btn-outline\" ng-href=\"{{::ngCtrl.getRecordDetailsUrl(record)}}\">\n    <i class=\"fa fa-fw fa-eye\"></i>\n</a>";
									actionItem.Weight = 1;
									newActionItemList.Add(actionItem);
								}

								createListInput.ActionItems = newActionItemList;

								{
									var responseObject = entMan.CreateRecordList(CUSTOMER_ENTITY_ID, createListInput);
									if (!responseObject.Success)
										throw new Exception("System error 10060. Entity: " + "user" + " Updated List: list_name" + " Message:" + responseObject.Message);
								}
							}
							#endregion

							if (createSampleRecords)
							{
								#region << Create Sample Customer >>
								{
									var sampleRecord = new EntityRecord();
									sampleRecord["id"] = new Guid("fb06213f-7632-495b-bb8d-ed5ff07dc515");
									sampleRecord["name"] = "Buckley Miller & Wright";
									var createSampleRecordResult = recMan.CreateRecord(CUSTOMER_ENTITY_NAME, sampleRecord);
									if (!createSampleRecordResult.Success)
									{
										throw new Exception("System error 10060. Create sample record. Message:" + createSampleRecordResult.Message);
									}
								}
								#endregion

								#region << Create Sample Customer User >>
								{
									var sampleRecord = new EntityRecord();
									sampleRecord["id"] = new Guid("307fe376-a1c6-495e-a7c0-2a78797565f2");
									sampleRecord["first_name"] = "Sample";
									sampleRecord["last_name"] = "Customer";
									sampleRecord["username"] = "crm_customer";
									sampleRecord["email"] = "customer@sample.com";
									sampleRecord["password"] = "sample123";
									sampleRecord["enabled"] = true;
									sampleRecord["verified"] = true;
									sampleRecord["image"] = "/plugins/webvella-core/assets/avatar-deep-purple.png";
									var createSampleRecordResult = recMan.CreateRecord(SystemIds.UserEntityId, sampleRecord);
									if (!createSampleRecordResult.Success)
									{
										throw new Exception("System error 10060. Create sample customer record. Message:" + createSampleRecordResult.Message);
									}
								}
								#endregion

								#region << Create Sample User Role>>
								{
									var sampleRecord = new EntityRecord();
									sampleRecord["id"] = new Guid("27745245-09bd-4adb-8831-3870bcae46fe");
									sampleRecord["name"] = "crm_customer";
									sampleRecord["description"] = "Sample Customer role for CRM application";
									var createSampleRecordResult = recMan.CreateRecord(SystemIds.RoleEntityId, sampleRecord);
									if (!createSampleRecordResult.Success)
									{
										throw new Exception("System error 10060. Create sample role record. Message:" + createSampleRecordResult.Message);
									}
								}
								#endregion

								#region << Create relation between sample customer and role >>
								{
									var createRelationNtoNResponse = recMan.CreateRelationManyToManyRecord(new Guid("0c4b119e-1d7b-4b40-8d2c-9e447cc656ab"), new Guid("27745245-09bd-4adb-8831-3870bcae46fe"), new Guid("307fe376-a1c6-495e-a7c0-2a78797565f2"));
									if (!createRelationNtoNResponse.Success)
									{
										throw new Exception("Could not create item image relation" + createRelationNtoNResponse.Message);
									}
								}
								#endregion

								#region << Create relation between sample customer and regular role >>
								{
									var createRelationNtoNResponse = recMan.CreateRelationManyToManyRecord(new Guid("0c4b119e-1d7b-4b40-8d2c-9e447cc656ab"), new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"), new Guid("307fe376-a1c6-495e-a7c0-2a78797565f2"));
									if (!createRelationNtoNResponse.Success)
									{
										throw new Exception("Could not create item image relation" + createRelationNtoNResponse.Message);
									}
								}
								#endregion

							}
						}
						#endregion

						#region << 4. Save needed changes to the plugin setting data >>
						{
							string json = JsonConvert.SerializeObject(currentPluginSettings);
							var settingsEntityRecord = new EntityRecord();
							settingsEntityRecord["id"] = WEBVELLA_CRM_PLUGIN_ID;
							settingsEntityRecord["name"] = WEBVELLA_CRM_PLUGIN_NAME;
							settingsEntityRecord["data"] = json;
							var settingsUpdateReponse = recMan.UpdateRecord("plugin_data", settingsEntityRecord);
							if (!settingsUpdateReponse.Success)
								throw new Exception("plugin 'webvella-project' failed to update its settings in the database due to: " + pluginDataQueryResponse.Message);
						}
						#endregion

						connection.CommitTransaction();
					}
					catch (Exception ex)
					{
						connection.RollbackTransaction();
						throw ex;
					}
				}
			}
		}
	}
}
