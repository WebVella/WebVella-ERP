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
		private static void Patch160430(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan, bool createSampleRecords = false)
		{

			#region << Create Project admin area >>
			//The areas are the main object for navigation for the user. You can attach entities and URLs later to them
			{
				var area = new EntityRecord();
				area["id"] = PROJECT_ADMIN_AREA_ID;
				area["name"] = "project_admin";
				area["label"] = "Project Admin";
				area["icon_name"] = "wrench";
				area["color"] = "pink";
				area["folder"] = "Admin";
				area["weight"] = 101;
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

			#region << Create Project Workplace area >>
			//The areas are the main object for navigation for the user. You can attach entities and URLs later to them
			{
				var area = new EntityRecord();
				area["id"] = PROJECT_WORKPLACE_AREA_ID;
				area["name"] = "projects";
				area["label"] = "Projects";
				area["icon_name"] = "product-hunt";
				area["color"] = "indigo";
				area["folder"] = "Projects";
				area["weight"] = 5;
				var areaRoles = new List<Guid>();
				areaRoles.Add(SystemIds.AdministratorRoleId);
				areaRoles.Add(SystemIds.RegularRoleId);
				area["roles"] = JsonConvert.SerializeObject(areaRoles);
				var createAreaResult = recMan.CreateRecord("area", area);
				if (!createAreaResult.Success)
				{
					throw new Exception("System error 10060. Area create with name : project_admin. Message:" + createAreaResult.Message);
				}
			}
			#endregion

			#region << Create Project create task area >>
			{
				var area = new EntityRecord();
				area["id"] = CREATE_TASK_WORKPLACE_AREA_ID;
				area["name"] = "create_task";
				area["label"] = "Create task";
				area["icon_name"] = "tasks";
				area["color"] = "teal";
				area["folder"] = "Projects";
				area["weight"] = 1;
				var areaRoles = new List<Guid>();
				areaRoles.Add(SystemIds.AdministratorRoleId);
				areaRoles.Add(SystemIds.RegularRoleId);
				area["roles"] = JsonConvert.SerializeObject(areaRoles);
				var createAreaResult = recMan.CreateRecord("area", area);
				if (!createAreaResult.Success)
				{
					throw new Exception("System error 10060. Area create with name : project_admin. Message:" + createAreaResult.Message);
				}
			}
			#endregion

			#region << area add subscription: Create task >>
			{
				var updatedAreaId = CREATE_TASK_WORKPLACE_AREA_ID;
				var updateAreaResult = Helpers.UpsertUrlAsAreaSubscription(entMan, recMan, updatedAreaId, "/#/areas/projects/wv_task/view-create/create?returnUrl=%2Fareas%2Fprojects%2Fwv_task%2Flist-general%2Fmy_tasks", "Create task", 1, "tasks");
				if (!updateAreaResult.Success)
				{
					throw new Exception("System error 10060. Area update with id : " + updatedAreaId + " Message:" + updateAreaResult.Message);
				}
			}
			#endregion

			#region << Create Project submit bug area >>
			{
				var area = new EntityRecord();
				area["id"] = REPORT_BUG_WORKPLACE_AREA_ID;
				area["name"] = "report_bug";
				area["label"] = "Report bug";
				area["icon_name"] = "bug";
				area["color"] = "deep-orange";
				area["folder"] = "Projects";
				area["weight"] = 2;
				var areaRoles = new List<Guid>();
				areaRoles.Add(SystemIds.AdministratorRoleId);
				areaRoles.Add(SystemIds.RegularRoleId);
				area["roles"] = JsonConvert.SerializeObject(areaRoles);
				var createAreaResult = recMan.CreateRecord("area", area);
				if (!createAreaResult.Success)
				{
					throw new Exception("System error 10060. Area create with name : project_admin. Message:" + createAreaResult.Message);
				}
			}
			#endregion

			#region << area add subscription: Create task >>
			{
				var updatedAreaId = REPORT_BUG_WORKPLACE_AREA_ID;
				var updateAreaResult = Helpers.UpsertUrlAsAreaSubscription(entMan, recMan, updatedAreaId, "/#/areas/projects/wv_bug/view-create/create?returnUrl=%2Fareas%2Fprojects%2Fwv_bug%2Flist-general%2Fmy_bugs", "Report bug", 1, "bug");
				if (!updateAreaResult.Success)
				{
					throw new Exception("System error 10060. Area update with id : " + updatedAreaId + " Message:" + updateAreaResult.Message);
				}
			}
			#endregion

			#region << wv_project >>
			{
				#region << entity >>
				{
					InputEntity entity = new InputEntity();
					entity.Id = PROJECT_ENTITY_ID;
					entity.Name = PROJECT_ENTITY_NAME;
					entity.Label = "Project";
					entity.LabelPlural = "Projects";
					entity.System = true;
					entity.IconName = "product-hunt";
					entity.Weight = 22;
					entity.RecordPermissions = new RecordPermissions();
					entity.RecordPermissions.CanCreate = new List<Guid>();
					entity.RecordPermissions.CanRead = new List<Guid>();
					entity.RecordPermissions.CanUpdate = new List<Guid>();
					entity.RecordPermissions.CanDelete = new List<Guid>();
					//Create
					entity.RecordPermissions.CanCreate.Add(SystemIds.AdministratorRoleId);
					//READ
					entity.RecordPermissions.CanRead.Add(SystemIds.AdministratorRoleId);
					entity.RecordPermissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					entity.RecordPermissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					//System fields and relations Ids should be hardcoded for the compare/change code generation to work later on correctly
					var systemItemIdDictionary = new Dictionary<string, Guid>();
					systemItemIdDictionary["id"] = new Guid("979dc238-c8f2-46c9-90e7-6ab65f9652da");
					systemItemIdDictionary["created_on"] = new Guid("64c613a5-354b-4cc3-8ba7-2ece7be562f0");
					systemItemIdDictionary["created_by"] = new Guid("5125750c-f82c-4320-905a-804d7c32d3fd");
					systemItemIdDictionary["last_modified_on"] = new Guid("58cad35e-39b6-4ce7-9e2b-610cd466141e");
					systemItemIdDictionary["last_modified_by"] = new Guid("292cd102-7915-4a3b-85a7-5ffdc5afe4e2");
					systemItemIdDictionary["user_wv_project_created_by"] = new Guid("330c72a0-fcce-4c63-979d-8cfad63fa6f5");
					systemItemIdDictionary["user_wv_project_modified_by"] = new Guid("d99cfe44-68a8-4e66-9e92-1a484fb73bd8");
					{
						var response = entMan.CreateEntity(entity, false, false, systemItemIdDictionary);
						if (!response.Success)
							throw new Exception("System error 10050. Entity: " + PROJECT_ENTITY_NAME + " Field: entity creation" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << name >>
				{
					InputTextField textboxField = new InputTextField();
					textboxField.Id = new Guid("d13427b7-e518-4305-b2cc-bc814a299b55");
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
						var response = entMan.CreateField(PROJECT_ENTITY_ID, textboxField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + PROJECT_ENTITY_NAME + " Field: name" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << code >>
				{
					InputTextField textboxField = new InputTextField();
					textboxField.Id = new Guid("d9c6a939-e2e3-4617-900e-e056f0638fa8");
					textboxField.Name = "code";
					textboxField.Label = "Code";
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
						var response = entMan.CreateField(PROJECT_ENTITY_ID, textboxField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + PROJECT_ENTITY_NAME + " Field: code" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << description >>
				{
					InputHtmlField htmlField = new InputHtmlField();
					htmlField.Id = new Guid("d5e2c42c-c0b8-4f03-92e0-e91bede1e7b3");
					htmlField.Name = "description";
					htmlField.Label = "Description";
					htmlField.PlaceholderText = "";
					htmlField.Description = "";
					htmlField.HelpText = "";
					htmlField.Required = false;
					htmlField.Unique = false;
					htmlField.Searchable = false;
					htmlField.Auditable = false;
					htmlField.System = true;
					htmlField.DefaultValue = string.Empty;
					htmlField.EnableSecurity = true;
					htmlField.Permissions = new FieldPermissions();
					htmlField.Permissions.CanRead = new List<Guid>();
					htmlField.Permissions.CanUpdate = new List<Guid>();
					htmlField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					htmlField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					htmlField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					{
						var response = entMan.CreateField(PROJECT_ENTITY_ID, htmlField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + PROJECT_ENTITY_NAME + " Field: description" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << owner_id >>
				{
					InputGuidField guidField = new InputGuidField();
					guidField.Id = new Guid("2e8589e0-3966-447d-8d21-6fd9a1250d60");
					guidField.Name = "owner_id";
					guidField.Label = "Project owner";
					guidField.PlaceholderText = "";
					guidField.Description = "";
					guidField.HelpText = "";
					guidField.Required = true;
					guidField.Unique = false;
					guidField.Searchable = false;
					guidField.Auditable = false;
					guidField.System = true;
					guidField.DefaultValue = Guid.Empty;
					guidField.GenerateNewId = false;
					guidField.EnableSecurity = true;
					guidField.Permissions = new FieldPermissions();
					guidField.Permissions.CanRead = new List<Guid>();
					guidField.Permissions.CanUpdate = new List<Guid>();
					//READ
					guidField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					guidField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);

					{
						var response = entMan.CreateField(PROJECT_ENTITY_ID, guidField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + PROJECT_ENTITY_NAME + " Field: owner_id" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << start_date >>
				{
					InputDateField dateField = new InputDateField();
					dateField.Id = new Guid("417ce7d7-a472-499b-8e70-43a1cb54723d");
					dateField.Name = "start_date";
					dateField.Label = "Start date";
					dateField.PlaceholderText = "";
					dateField.Description = "";
					dateField.HelpText = "";
					dateField.Required = false;
					dateField.Unique = false;
					dateField.Searchable = false;
					dateField.Auditable = false;
					dateField.System = true;
					dateField.DefaultValue = null;
					dateField.Format = "dd MMM yyyy";
					dateField.UseCurrentTimeAsDefaultValue = true;
					dateField.EnableSecurity = true;
					dateField.Permissions = new FieldPermissions();
					dateField.Permissions.CanRead = new List<Guid>();
					dateField.Permissions.CanUpdate = new List<Guid>();
					//READ
					dateField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					dateField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					dateField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					{
						var response = entMan.CreateField(PROJECT_ENTITY_ID, dateField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + PROJECT_ENTITY_NAME + " Field: start_date" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << end_date >>
				{
					InputDateField dateField = new InputDateField();
					dateField.Id = new Guid("57198778-636d-47ec-b33e-edfc5705cc05");
					dateField.Name = "end_date";
					dateField.Label = "End date";
					dateField.PlaceholderText = "";
					dateField.Description = "";
					dateField.HelpText = "";
					dateField.Required = false;
					dateField.Unique = false;
					dateField.Searchable = false;
					dateField.Auditable = false;
					dateField.System = true;
					dateField.DefaultValue = null;
					dateField.Format = "dd MMM yyyy";
					dateField.UseCurrentTimeAsDefaultValue = true;
					dateField.EnableSecurity = true;
					dateField.Permissions = new FieldPermissions();
					dateField.Permissions.CanRead = new List<Guid>();
					dateField.Permissions.CanUpdate = new List<Guid>();
					//READ
					dateField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					dateField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					dateField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					{
						var response = entMan.CreateField(PROJECT_ENTITY_ID, dateField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + PROJECT_ENTITY_NAME + " Field: end_date" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << priority >>
				{
					InputSelectField dropdownField = new InputSelectField();
					dropdownField.Id = new Guid("277feafe-5727-4a33-9024-153015dd06d0");
					dropdownField.Name = "priority";
					dropdownField.Label = "Priority";
					dropdownField.PlaceholderText = "";
					dropdownField.Description = "";
					dropdownField.HelpText = "";
					dropdownField.Required = true;
					dropdownField.Unique = false;
					dropdownField.Searchable = false;
					dropdownField.Auditable = false;
					dropdownField.System = true;
					dropdownField.DefaultValue = "medium";
					dropdownField.Options = new List<SelectFieldOption>
										{
											new SelectFieldOption(){ Key = "low", Value = "low" },
											new SelectFieldOption(){ Key = "medium", Value = "medium" },
											new SelectFieldOption(){ Key = "hight", Value = "hight" }
										};
					dropdownField.EnableSecurity = true;
					dropdownField.Permissions = new FieldPermissions();
					dropdownField.Permissions.CanRead = new List<Guid>();
					dropdownField.Permissions.CanUpdate = new List<Guid>();
					//READ
					dropdownField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					dropdownField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					dropdownField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);

					{
						var response = entMan.CreateField(PROJECT_ENTITY_ID, dropdownField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + PROJECT_ENTITY_NAME + " Field: priority" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << status >>
				{
					InputSelectField dropdownField = new InputSelectField();
					dropdownField.Id = new Guid("ba5698ba-fa81-4215-a5e1-17c368f504e2");
					dropdownField.Name = "status";
					dropdownField.Label = "Status";
					dropdownField.PlaceholderText = "";
					dropdownField.Description = "";
					dropdownField.HelpText = "";
					dropdownField.Required = true;
					dropdownField.Unique = false;
					dropdownField.Searchable = false;
					dropdownField.Auditable = false;
					dropdownField.System = true;
					dropdownField.DefaultValue = "draft";
					dropdownField.Options = new List<SelectFieldOption>
								{
									new SelectFieldOption(){ Key = "draft", Value = "draft" },
									new SelectFieldOption(){ Key = "in review", Value = "in review" },
									new SelectFieldOption(){ Key = "published", Value = "published" },
									new SelectFieldOption(){ Key = "archived", Value = "archived" }
								};
					dropdownField.EnableSecurity = true;
					dropdownField.Permissions = new FieldPermissions();
					dropdownField.Permissions.CanRead = new List<Guid>();
					dropdownField.Permissions.CanUpdate = new List<Guid>();
					//READ
					dropdownField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					dropdownField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					dropdownField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);

					{
						var response = entMan.CreateField(PROJECT_ENTITY_ID, dropdownField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + PROJECT_ENTITY_NAME + " Field: status" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << billable_hour_price >>
				{
					InputCurrencyField currencyField = new InputCurrencyField();
					currencyField.Id = new Guid("7179f4ab-0376-4ded-a334-a21ff451538e");
					currencyField.Name = "billable_hour_price";
					currencyField.Label = "billable_hour_price";
					currencyField.PlaceholderText = "";
					currencyField.Description = "";
					currencyField.HelpText = "";
					currencyField.Required = true;
					currencyField.Unique = false;
					currencyField.Searchable = false;
					currencyField.Auditable = false;
					currencyField.System = true;
					currencyField.DefaultValue = 0;
					currencyField.MinValue = 0;
					currencyField.Currency = Helpers.GetCurrencyTypeObject("USD");
					currencyField.EnableSecurity = true;
					currencyField.Permissions.CanRead = new List<Guid>();
					currencyField.Permissions.CanUpdate = new List<Guid>();
					//READ
					currencyField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					//UPDATE
					currencyField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);

					{
						var response = entMan.CreateField(PROJECT_ENTITY_ID, currencyField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + PROJECT_ENTITY_NAME + " Field: billable_hour_price" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << customer_id >>
				{
					InputGuidField guidField = new InputGuidField();
					guidField.Id = new Guid("6aec7d63-56f3-4f1d-b2df-5fb62e30ab74");
					guidField.Name = "customer_id";
					guidField.Label = "Customer";
					guidField.PlaceholderText = "";
					guidField.Description = "";
					guidField.HelpText = "";
					guidField.Required = true;
					guidField.Unique = false;
					guidField.Searchable = false;
					guidField.Auditable = false;
					guidField.System = true;
					guidField.DefaultValue = Guid.Empty;
					guidField.GenerateNewId = false;
					guidField.EnableSecurity = true;
					guidField.Permissions = new FieldPermissions();
					guidField.Permissions.CanRead = new List<Guid>();
					guidField.Permissions.CanUpdate = new List<Guid>();
					//READ
					guidField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					guidField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);

					{
						var response = entMan.CreateField(PROJECT_ENTITY_ID, guidField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + PROJECT_ENTITY_NAME + " Field: customer_id" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << x_milestones_opened >>
				{
					InputNumberField numberField = new InputNumberField();
					numberField.Id = new Guid("83a2c903-454f-480d-9709-9903ad7a4046");
					numberField.Name = "x_milestones_opened";
					numberField.Label = "x_milestones_opened";
					numberField.PlaceholderText = "";
					numberField.Description = "";
					numberField.HelpText = "";
					numberField.Required = true;
					numberField.Unique = false;
					numberField.Searchable = false;
					numberField.Auditable = false;
					numberField.System = true;
					numberField.DefaultValue = 0;
					numberField.MinValue = null;
					numberField.MaxValue = null;
					numberField.DecimalPlaces = 0;
					numberField.EnableSecurity = true;
					numberField.Permissions = new FieldPermissions();
					numberField.Permissions.CanRead = new List<Guid>();
					numberField.Permissions.CanUpdate = new List<Guid>();
					//READ
					numberField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					numberField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(PROJECT_ENTITY_ID, numberField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + PROJECT_ENTITY_NAME + " Field: x_milestones_opened" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << x_milestones_completed >>
				{
					InputNumberField numberField = new InputNumberField();
					numberField.Id = new Guid("635c2bb3-d141-4eb2-8dce-2d76d9bf0fc3");
					numberField.Name = "x_milestones_completed";
					numberField.Label = "x_milestones_completed";
					numberField.PlaceholderText = "";
					numberField.Description = "";
					numberField.HelpText = "";
					numberField.Required = true;
					numberField.Unique = false;
					numberField.Searchable = false;
					numberField.Auditable = false;
					numberField.System = true;
					numberField.DefaultValue = 0;
					numberField.MinValue = null;
					numberField.MaxValue = null;
					numberField.DecimalPlaces = 0;
					numberField.EnableSecurity = true;
					numberField.Permissions = new FieldPermissions();
					numberField.Permissions.CanRead = new List<Guid>();
					numberField.Permissions.CanUpdate = new List<Guid>();
					//READ
					numberField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					numberField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(PROJECT_ENTITY_ID, numberField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + PROJECT_ENTITY_NAME + " Field: x_milestones_completed" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << x_tasks_not_started >>
				{
					InputNumberField numberField = new InputNumberField();
					numberField.Id = new Guid("589ad094-3d99-4f85-a35f-6e02c2b2073c");
					numberField.Name = "x_tasks_not_started";
					numberField.Label = "x_tasks_not_started";
					numberField.PlaceholderText = "";
					numberField.Description = "";
					numberField.HelpText = "";
					numberField.Required = true;
					numberField.Unique = false;
					numberField.Searchable = false;
					numberField.Auditable = false;
					numberField.System = true;
					numberField.DefaultValue = 0;
					numberField.MinValue = null;
					numberField.MaxValue = null;
					numberField.DecimalPlaces = 0;
					numberField.EnableSecurity = true;
					numberField.Permissions = new FieldPermissions();
					numberField.Permissions.CanRead = new List<Guid>();
					numberField.Permissions.CanUpdate = new List<Guid>();
					//READ
					numberField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					numberField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(PROJECT_ENTITY_ID, numberField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + PROJECT_ENTITY_NAME + " Field: x_tasks_not_started" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << x_tasks_in_progress >>
				{
					InputNumberField numberField = new InputNumberField();
					numberField.Id = new Guid("cbc72ef7-b0ac-4952-af75-df65e8a560ea");
					numberField.Name = "x_tasks_in_progress";
					numberField.Label = "x_tasks_in_progress";
					numberField.PlaceholderText = "";
					numberField.Description = "";
					numberField.HelpText = "";
					numberField.Required = true;
					numberField.Unique = false;
					numberField.Searchable = false;
					numberField.Auditable = false;
					numberField.System = true;
					numberField.DefaultValue = 0;
					numberField.MinValue = null;
					numberField.MaxValue = null;
					numberField.DecimalPlaces = 0;
					numberField.EnableSecurity = true;
					numberField.Permissions = new FieldPermissions();
					numberField.Permissions.CanRead = new List<Guid>();
					numberField.Permissions.CanUpdate = new List<Guid>();
					//READ
					numberField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					numberField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(PROJECT_ENTITY_ID, numberField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + PROJECT_ENTITY_NAME + " Field: x_tasks_in_progress" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << x_tasks_completed >>
				{
					InputNumberField numberField = new InputNumberField();
					numberField.Id = new Guid("abd6b885-d095-4dc4-ad26-d47bd565abcd");
					numberField.Name = "x_tasks_completed";
					numberField.Label = "x_tasks_completed";
					numberField.PlaceholderText = "";
					numberField.Description = "";
					numberField.HelpText = "";
					numberField.Required = true;
					numberField.Unique = false;
					numberField.Searchable = false;
					numberField.Auditable = false;
					numberField.System = true;
					numberField.DefaultValue = 0;
					numberField.MinValue = null;
					numberField.MaxValue = null;
					numberField.DecimalPlaces = 0;
					numberField.EnableSecurity = true;
					numberField.Permissions = new FieldPermissions();
					numberField.Permissions.CanRead = new List<Guid>();
					numberField.Permissions.CanUpdate = new List<Guid>();
					//READ
					numberField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					numberField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(PROJECT_ENTITY_ID, numberField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + PROJECT_ENTITY_NAME + " Field: x_tasks_completed" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << x_bugs_opened >>
				{
					InputNumberField numberField = new InputNumberField();
					numberField.Id = new Guid("45222a14-313c-450f-8988-f386895753ba");
					numberField.Name = "x_bugs_opened";
					numberField.Label = "x_bugs_opened";
					numberField.PlaceholderText = "";
					numberField.Description = "";
					numberField.HelpText = "";
					numberField.Required = true;
					numberField.Unique = false;
					numberField.Searchable = false;
					numberField.Auditable = false;
					numberField.System = true;
					numberField.DefaultValue = 0;
					numberField.MinValue = null;
					numberField.MaxValue = null;
					numberField.DecimalPlaces = 0;
					numberField.EnableSecurity = true;
					numberField.Permissions = new FieldPermissions();
					numberField.Permissions.CanRead = new List<Guid>();
					numberField.Permissions.CanUpdate = new List<Guid>();
					//READ
					numberField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					numberField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(PROJECT_ENTITY_ID, numberField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + PROJECT_ENTITY_NAME + " Field: x_bugs_opened" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << x_bugs_reopened >>
				{
					InputNumberField numberField = new InputNumberField();
					numberField.Id = new Guid("9eee5691-1837-4187-8d65-3b6629c51bf7");
					numberField.Name = "x_bugs_reopened";
					numberField.Label = "x_bugs_reopened";
					numberField.PlaceholderText = "";
					numberField.Description = "";
					numberField.HelpText = "";
					numberField.Required = true;
					numberField.Unique = false;
					numberField.Searchable = false;
					numberField.Auditable = false;
					numberField.System = true;
					numberField.DefaultValue = 0;
					numberField.MinValue = null;
					numberField.MaxValue = null;
					numberField.DecimalPlaces = 0;
					numberField.EnableSecurity = true;
					numberField.Permissions = new FieldPermissions();
					numberField.Permissions.CanRead = new List<Guid>();
					numberField.Permissions.CanUpdate = new List<Guid>();
					//READ
					numberField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					numberField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(PROJECT_ENTITY_ID, numberField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + PROJECT_ENTITY_NAME + " Field: x_bugs_reopened" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << x_bugs_closed >>
				{
					InputNumberField numberField = new InputNumberField();
					numberField.Id = new Guid("41507552-2b75-43d5-98b7-63552e9fa420");
					numberField.Name = "x_bugs_closed";
					numberField.Label = "x_bugs_closed";
					numberField.PlaceholderText = "";
					numberField.Description = "";
					numberField.HelpText = "";
					numberField.Required = true;
					numberField.Unique = false;
					numberField.Searchable = false;
					numberField.Auditable = false;
					numberField.System = true;
					numberField.DefaultValue = 0;
					numberField.MinValue = null;
					numberField.MaxValue = null;
					numberField.DecimalPlaces = 0;
					numberField.EnableSecurity = true;
					numberField.Permissions = new FieldPermissions();
					numberField.Permissions.CanRead = new List<Guid>();
					numberField.Permissions.CanUpdate = new List<Guid>();
					//READ
					numberField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					numberField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(PROJECT_ENTITY_ID, numberField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + PROJECT_ENTITY_NAME + " Field: x_bugs_closed" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << user_1_n_project_owner Relation >>
				{
					var originEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
					var targetEntity = entMan.ReadEntity(PROJECT_ENTITY_ID).Object;
					EntityRelation oneToNRelation = new EntityRelation();
					oneToNRelation.Id = PROJECT_RELATION_USER_1_N_PROJECT_OWNER_ID;
					oneToNRelation.Name = "user_1_n_project_owner";
					oneToNRelation.Label = "user_1_n_project_owner";
					oneToNRelation.System = true;
					oneToNRelation.RelationType = EntityRelationType.OneToMany;
					oneToNRelation.OriginEntityId = originEntity.Id;
					oneToNRelation.OriginFieldId = originEntity.Fields.Single(x => x.Name == "id").Id;
					oneToNRelation.TargetEntityId = targetEntity.Id;
					oneToNRelation.TargetFieldId = targetEntity.Fields.Single(x => x.Name == "owner_id").Id;
					{
						var result = relMan.Create(oneToNRelation);
						if (!result.Success)
							throw new Exception("CREATE user_1_n_project_owner RELATION:" + result.Message);
					}
				}
				#endregion

				#region << customer_1_n_project Relation >>
				{
					var originEntity = entMan.ReadEntity(CUSTOMER_ENTITY_ID).Object;
					var targetEntity = entMan.ReadEntity(PROJECT_ENTITY_ID).Object;
					EntityRelation oneToNRelation = new EntityRelation();
					oneToNRelation.Id = PROJECT_RELATION_CUSTOMER_1_N_PROJECT_ID;
					oneToNRelation.Name = "customer_1_n_project";
					oneToNRelation.Label = "customer_1_n_project";
					oneToNRelation.System = true;
					oneToNRelation.RelationType = EntityRelationType.OneToMany;
					oneToNRelation.OriginEntityId = originEntity.Id;
					oneToNRelation.OriginFieldId = originEntity.Fields.Single(x => x.Name == "id").Id;
					oneToNRelation.TargetEntityId = targetEntity.Id;
					oneToNRelation.TargetFieldId = targetEntity.Fields.Single(x => x.Name == "customer_id").Id;
					{
						var result = relMan.Create(oneToNRelation);
						if (!result.Success)
							throw new Exception("CREATE customer_1_n_project RELATION:" + result.Message);
					}
				}
				#endregion

				#region << role_n_n_project_team Relation >>
				//Relation for the team user roles for this projects
				{
					var originEntity = entMan.ReadEntity(SystemIds.RoleEntityId).Object;
					var targetEntity = entMan.ReadEntity(PROJECT_ENTITY_ID).Object;
					EntityRelation NToNRelation = new EntityRelation();
					NToNRelation.Id = PROJECT_RELATION_ROLE_N_N_PROJECT_TEAM_ID;
					NToNRelation.Name = "role_n_n_project_team";
					NToNRelation.Label = "role_n_n_project_team";
					NToNRelation.System = true;
					NToNRelation.RelationType = EntityRelationType.ManyToMany;
					NToNRelation.OriginEntityId = originEntity.Id;
					NToNRelation.OriginFieldId = originEntity.Fields.Single(x => x.Name == "id").Id;
					NToNRelation.TargetEntityId = targetEntity.Id;
					NToNRelation.TargetFieldId = targetEntity.Fields.Single(x => x.Name == "id").Id;
					{
						var result = relMan.Create(NToNRelation);
						if (!result.Success)
							throw new Exception("CREATE role_n_n_project_team RELATION:" + result.Message);
					}
				}
				#endregion

				#region << role_n_n_project_customer Relation >>
				//Relation for the customer user roles for this projects
				{
					var originEntity = entMan.ReadEntity(SystemIds.RoleEntityId).Object;
					var targetEntity = entMan.ReadEntity(PROJECT_ENTITY_ID).Object;
					EntityRelation NToNRelation = new EntityRelation();
					NToNRelation.Id = new Guid("e6d75feb-3c8f-410b-9ff4-54ef8489dc2f");
					NToNRelation.Name = "role_n_n_project_customer";
					NToNRelation.Label = "role_n_n_project_customer";
					NToNRelation.System = true;
					NToNRelation.RelationType = EntityRelationType.ManyToMany;
					NToNRelation.OriginEntityId = originEntity.Id;
					NToNRelation.OriginFieldId = originEntity.Fields.Single(x => x.Name == "id").Id;
					NToNRelation.TargetEntityId = targetEntity.Id;
					NToNRelation.TargetFieldId = targetEntity.Fields.Single(x => x.Name == "id").Id;
					{
						var result = relMan.Create(NToNRelation);
						if (!result.Success)
							throw new Exception("CREATE role_n_n_project_customer RELATION:" + result.Message);
					}
				}
				#endregion

				#region << View name: admin_details >>
				{
					var createViewEntity = entMan.ReadEntity(PROJECT_ENTITY_ID).Object;
					var createViewInput = new InputRecordView();
					var viewRegion = new InputRecordViewRegion();
					var viewSection = new InputRecordViewSection();
					var viewRow = new InputRecordViewRow();
					var viewColumn = new InputRecordViewColumn();
					var viewItem = new InputRecordViewFieldItem();
					var viewItemFromRelation = new InputRecordViewRelationFieldItem();

					#region << details >>
					createViewInput.Id = new Guid("120dee5b-f3ed-4256-9346-da01d787a49c");
					createViewInput.Type = "hidden";
					createViewInput.Name = "admin_details";
					createViewInput.Label = "Project details";
					createViewInput.Title = "";
					createViewInput.Default = false;
					createViewInput.System = true;
					createViewInput.Weight = 10;
					createViewInput.CssClass = "";
					createViewInput.IconName = "product-hunt";
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
					viewSection.Id = new Guid("6f8fdcc2-8a8f-4cf6-bf26-47bf1c6f0438");
					viewSection.Name = "details";
					viewSection.Label = "Details";
					viewSection.ShowLabel = false;
					viewSection.CssClass = "";
					viewSection.Collapsed = false;
					viewSection.TabOrder = "left-right";
					viewSection.Weight = 1;
					viewSection.Rows = new List<InputRecordViewRow>();

					#region << Row 1 Column>>
					viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("5d7ce054-f592-41e7-b8f1-424cfd6d38b7");
					viewRow.Weight = 1;
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = 12;
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << name >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = PROJECT_ENTITY_ID;
						viewItem.EntityName = PROJECT_ENTITY_NAME;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "name").Id;
						viewItem.FieldName = "name";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion

					#region << description >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = PROJECT_ENTITY_ID;
						viewItem.EntityName = PROJECT_ENTITY_NAME;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "description").Id;
						viewItem.FieldName = "description";
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

					#region << Row 2 Columns>>
					viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("22d11cfc-a763-472b-b509-7ddfe36476bb");
					viewRow.Weight = 1;
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column Left >>
					viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = 6;
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << $user_1_n_project_owner > username >>
					{
						var targetEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
						viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = targetEntity.Id;
						viewItemFromRelation.EntityName = targetEntity.Name;
						viewItemFromRelation.Type = "field";
						viewItemFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "username").Id;
						viewItemFromRelation.FieldName = "username";
						viewItemFromRelation.FieldLabel = "Owner / Project manager";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldRequired = true;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = PROJECT_RELATION_USER_1_N_PROJECT_OWNER_ID;
						viewItemFromRelation.RelationName = "user_1_n_project_owner";
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion

					#region << $role_n_n_project_team > name >>
					{
						var targetEntity = entMan.ReadEntity(SystemIds.RoleEntityId).Object;
						viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = targetEntity.Id;
						viewItemFromRelation.EntityName = targetEntity.Name;
						viewItemFromRelation.Type = "field";
						viewItemFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "name").Id;
						viewItemFromRelation.FieldName = "name";
						viewItemFromRelation.FieldLabel = "Project team roles";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldRequired = true;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = PROJECT_RELATION_ROLE_N_N_PROJECT_TEAM_ID;
						viewItemFromRelation.RelationName = "role_n_n_project_team";
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion

					#region << start_date >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = PROJECT_ENTITY_ID;
						viewItem.EntityName = PROJECT_ENTITY_NAME;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "start_date").Id;
						viewItem.FieldName = "start_date";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion

					#region << billable_hour_price >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = PROJECT_ENTITY_ID;
						viewItem.EntityName = PROJECT_ENTITY_NAME;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "billable_hour_price").Id;
						viewItem.FieldName = "billable_hour_price";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion

					//Save column
					viewRow.Columns.Add(viewColumn);
					#endregion

					#region << Column right >>
					viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = 6;
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << $customer_1_n_project > name >>
					{
						var targetEntity = entMan.ReadEntity(CUSTOMER_ENTITY_ID).Object;
						viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = targetEntity.Id;
						viewItemFromRelation.EntityName = targetEntity.Name;
						viewItemFromRelation.Type = "field";
						viewItemFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "name").Id;
						viewItemFromRelation.FieldName = "name";
						viewItemFromRelation.FieldLabel = "Customer";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldRequired = true;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = PROJECT_RELATION_CUSTOMER_1_N_PROJECT_ID;
						viewItemFromRelation.RelationName = "customer_1_n_project";
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion

					#region << $role_n_n_project_customer > name >>
					{
						var targetEntity = entMan.ReadEntity(SystemIds.RoleEntityId).Object;
						viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = targetEntity.Id;
						viewItemFromRelation.EntityName = targetEntity.Name;
						viewItemFromRelation.Type = "field";
						viewItemFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "name").Id;
						viewItemFromRelation.FieldName = "name";
						viewItemFromRelation.FieldLabel = "Project customer roles";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldRequired = true;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = PROJECT_RELATION_ROLE_N_N_PROJECT_CUSTOMER_ID;
						viewItemFromRelation.RelationName = "role_n_n_project_customer";
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion

					#region << end_date >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = PROJECT_ENTITY_ID;
						viewItem.EntityName = PROJECT_ENTITY_NAME;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "end_date").Id;
						viewItem.FieldName = "end_date";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion

					#region << code >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = PROJECT_ENTITY_ID;
						viewItem.EntityName = PROJECT_ENTITY_NAME;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "code").Id;
						viewItem.FieldName = "code";
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
						var response = entMan.CreateRecordView(PROJECT_ENTITY_ID, createViewInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + PROJECT_ENTITY_NAME + " Updated view: admin_details" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << View name: admin_create >>
				{
					var createViewEntity = entMan.ReadEntity(PROJECT_ENTITY_ID).Object;
					var createViewInput = new InputRecordView();
					var viewRegion = new InputRecordViewRegion();
					var viewSection = new InputRecordViewSection();
					var viewRow = new InputRecordViewRow();
					var viewColumn = new InputRecordViewColumn();
					var viewItem = new InputRecordViewFieldItem();
					var viewItemFromRelation = new InputRecordViewRelationFieldItem();

					#region << details >>
					createViewInput.Id = new Guid("a6e121f5-0990-4576-9e39-59777e0ecb01");
					createViewInput.Type = "hidden";
					createViewInput.Title = "";
					createViewInput.Name = "admin_create";
					createViewInput.Label = "Project create";
					createViewInput.Default = false;
					createViewInput.System = true;
					createViewInput.Weight = 10;
					createViewInput.CssClass = null;
					createViewInput.IconName = "product-hunt";
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
					viewSection.Id = new Guid("88077697-7a36-4a5a-b021-3d2a8638dade");
					viewSection.Name = "details";
					viewSection.Label = "Details";
					viewSection.ShowLabel = false;
					viewSection.CssClass = "";
					viewSection.Collapsed = false;
					viewSection.TabOrder = "left-right";
					viewSection.Weight = 1;
					viewSection.Rows = new List<InputRecordViewRow>();

					#region << Row 1 Column>>
					viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("cc65d58f-0cfd-4f3d-b9c6-3c8faff6c8b4");
					viewRow.Weight = 1;
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = 12;
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << name >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = PROJECT_ENTITY_ID;
						viewItem.EntityName = PROJECT_ENTITY_NAME;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "name").Id;
						viewItem.FieldName = "name";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion

					#region << description >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = PROJECT_ENTITY_ID;
						viewItem.EntityName = PROJECT_ENTITY_NAME;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "description").Id;
						viewItem.FieldName = "description";
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

					#region << Row 2 Columns>>
					viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("d88b3da7-d501-48ba-b61d-2eb3485bce21");
					viewRow.Weight = 1;
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column Left >>
					viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = 6;
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << $user_1_n_project_owner > username >>
					{
						var targetEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
						viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = targetEntity.Id;
						viewItemFromRelation.EntityName = targetEntity.Name;
						viewItemFromRelation.Type = "field";
						viewItemFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "username").Id;
						viewItemFromRelation.FieldName = "username";
						viewItemFromRelation.FieldLabel = "Owner / Project manager";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldRequired = true;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = PROJECT_RELATION_USER_1_N_PROJECT_OWNER_ID;
						viewItemFromRelation.RelationName = "user_1_n_project_owner";
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion

					#region << $role_n_n_project_team > name >>
					{
						var targetEntity = entMan.ReadEntity(SystemIds.RoleEntityId).Object;
						viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = targetEntity.Id;
						viewItemFromRelation.EntityName = targetEntity.Name;
						viewItemFromRelation.Type = "field";
						viewItemFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "name").Id;
						viewItemFromRelation.FieldName = "name";
						viewItemFromRelation.FieldLabel = "Project team roles";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldRequired = true;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = PROJECT_RELATION_ROLE_N_N_PROJECT_TEAM_ID;
						viewItemFromRelation.RelationName = "role_n_n_project_team";
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion

					#region << start_date >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = PROJECT_ENTITY_ID;
						viewItem.EntityName = PROJECT_ENTITY_NAME;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "start_date").Id;
						viewItem.FieldName = "start_date";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion

					#region << billable_hour_price >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = PROJECT_ENTITY_ID;
						viewItem.EntityName = PROJECT_ENTITY_NAME;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "billable_hour_price").Id;
						viewItem.FieldName = "billable_hour_price";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion

					//Save column
					viewRow.Columns.Add(viewColumn);
					#endregion

					#region << Column right >>
					viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = 6;
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << $customer_1_n_project > name >>
					{
						var targetEntity = entMan.ReadEntity(CUSTOMER_ENTITY_ID).Object;
						viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = targetEntity.Id;
						viewItemFromRelation.EntityName = targetEntity.Name;
						viewItemFromRelation.Type = "field";
						viewItemFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "name").Id;
						viewItemFromRelation.FieldName = "name";
						viewItemFromRelation.FieldLabel = "Customer";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldRequired = true;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = PROJECT_RELATION_CUSTOMER_1_N_PROJECT_ID;
						viewItemFromRelation.RelationName = "customer_1_n_project";
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion

					#region << $role_n_n_project_customer > name >>
					{
						var targetEntity = entMan.ReadEntity(SystemIds.RoleEntityId).Object;
						viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = targetEntity.Id;
						viewItemFromRelation.EntityName = targetEntity.Name;
						viewItemFromRelation.Type = "field";
						viewItemFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "name").Id;
						viewItemFromRelation.FieldName = "name";
						viewItemFromRelation.FieldLabel = "Project customer roles";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldRequired = true;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = PROJECT_RELATION_ROLE_N_N_PROJECT_CUSTOMER_ID;
						viewItemFromRelation.RelationName = "role_n_n_project_customer";
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion

					#region << end_date >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = PROJECT_ENTITY_ID;
						viewItem.EntityName = PROJECT_ENTITY_NAME;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "end_date").Id;
						viewItem.FieldName = "end_date";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion

					#region << code >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = PROJECT_ENTITY_ID;
						viewItem.EntityName = PROJECT_ENTITY_NAME;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "code").Id;
						viewItem.FieldName = "code";
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
						var response = entMan.CreateRecordView(PROJECT_ENTITY_ID, createViewInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + PROJECT_ENTITY_NAME + " Updated view: admin_create" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << View name: dashboard >>
				{
					var createViewEntity = entMan.ReadEntity(PROJECT_ENTITY_ID).Object;
					var createViewInput = new InputRecordView();
					var viewRegion = new InputRecordViewRegion();
					var viewSection = new InputRecordViewSection();
					var viewRow = new InputRecordViewRow();
					var viewColumn = new InputRecordViewColumn();
					var viewItem = new InputRecordViewFieldItem();
					var viewItemFromRelation = new InputRecordViewRelationFieldItem();

					#region << details >>
					createViewInput.Id = new Guid("68402d13-d9e7-4fb3-9394-40ba598f3f1a");
					createViewInput.Type = "general";
					createViewInput.Name = "dashboard";
					createViewInput.Label = "[{code}] {name}";
					createViewInput.Title = "";
					createViewInput.Default = false;
					createViewInput.System = true;
					createViewInput.Weight = 10;
					createViewInput.CssClass = null;
					createViewInput.IconName = "tachometer";
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
						var response = entMan.CreateRecordView(PROJECT_ENTITY_ID, createViewInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + PROJECT_ENTITY_NAME + " Updated view: admin_details" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << List name: admin >>
				{
					var createListEntity = entMan.ReadEntity(PROJECT_ENTITY_ID).Object;
					var createListInput = new InputRecordList();
					var listItem = new InputRecordListFieldItem();
					var listItemFromRelation = new InputRecordListRelationFieldItem();
					var listSort = new InputRecordListSort();
					var listQuery = new InputRecordListQuery();

					#region << details >>
					createListInput.Id = new Guid("3eff91d7-152a-496c-913c-152819a42930");
					createListInput.Type = "hidden";
					createListInput.Name = "admin";
					createListInput.Label = "All Projects";
					createListInput.Weight = 1;
					createListInput.Default = false;
					createListInput.System = true;
					createListInput.CssClass = null;
					createListInput.IconName = "product-hunt";
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
					#region << field_name >>
					{
						var fieldName = "name";
						listItem = new InputRecordListFieldItem();
						listItem.EntityId = PROJECT_ENTITY_ID;
						listItem.EntityName = PROJECT_ENTITY_NAME;
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
					createListInput.Sorts = new List<InputRecordListSort>();
					listSort = new InputRecordListSort();
					#endregion
					{
						var response = entMan.CreateRecordList(PROJECT_ENTITY_ID, createListInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + PROJECT_ENTITY_NAME + " Updated List: admin" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << create project my projects list>>
				{
					var createListEntity = entMan.ReadEntity(PROJECT_ENTITY_ID).Object;
					var createListInput = new InputRecordList();
					var listItem = new InputRecordListFieldItem();
					var listSort = new InputRecordListSort();
					var listQuery = new InputRecordListQuery();

					//General list details
					createListInput.Id = new Guid("86f56bb1-9cf4-4168-8e0e-3e8c4d8dd0e6");
					createListInput.Name = "my_projects";
					createListInput.Label = "My Projects";
					createListInput.Default = true;
					createListInput.System = true;
					createListInput.Type = "general";
					createListInput.IconName = "product-hunt";
					createListInput.PageSize = 10;
					createListInput.Weight = 10;
					createListInput.VisibleColumnsCount = 5;
					createListInput.ServiceCode = null;
					createListInput.DataSourceUrl = "/plugins/webvella-projects/api/project/list/my-projects";
					createListInput.DynamicHtmlTemplate = "/plugins/webvella-projects/templates/my-projects.html";
					createListInput.ActionItems = new List<ActionItem>();
					createListInput.Columns = new List<InputRecordListItemBase>();
					//Fields

					#region << field_name >>
					listItem = new InputRecordListFieldItem();
					listItem.EntityId = PROJECT_ENTITY_ID;
					listItem.EntityName = PROJECT_ENTITY_NAME;
					listItem.FieldId = createListEntity.Fields.Single(x => x.Name == "name").Id;
					listItem.FieldName = "name";
					listItem.Type = "field";
					createListInput.Columns.Add(listItem);
					#endregion

					{
						var response = entMan.CreateRecordList(PROJECT_ENTITY_ID, createListInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + PROJECT_ENTITY_NAME + " Updated List: general" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << area add subscription: Project Admin -> Project >>
				{
					var updatedAreaId = PROJECT_ADMIN_AREA_ID;
					var updateAreaResult = Helpers.UpsertEntityAsAreaSubscription(entMan, recMan, updatedAreaId, PROJECT_ENTITY_NAME, "admin_details", "admin_create", "admin");
					if (!updateAreaResult.Success)
					{
						throw new Exception("System error 10060. Area update with id : " + updatedAreaId + " Message:" + updateAreaResult.Message);
					}
				}
				#endregion

				#region << create project lookup list >>
				{
					var createListEntity = entMan.ReadEntity(PROJECT_ENTITY_ID).Object;
					var createListInput = new InputRecordList();
					var listItem = new InputRecordListFieldItem();
					var listSort = new InputRecordListSort();
					var listQuery = new InputRecordListQuery();
					//General list details
					createListInput.Id = new Guid("623c9a45-9472-418c-a293-e508d0b91117");
					createListInput.Name = "lookup";
					createListInput.Label = "Lookup";
					createListInput.Default = true;
					createListInput.System = true;
					createListInput.Type = "lookup";
					createListInput.IconName = "list";
					createListInput.PageSize = 10;
					createListInput.Weight = 10;
					createListInput.VisibleColumnsCount = 5;
					createListInput.ServiceCode = null;
					createListInput.DynamicHtmlTemplate = null;
					createListInput.DataSourceUrl = "/plugins/webvella-projects/api/project/list/my-projects";
					createListInput.ActionItems = new List<ActionItem>();
					createListInput.Columns = new List<InputRecordListItemBase>();
					//Fields
					#region << field_name >>
					listItem = new InputRecordListFieldItem();
					listItem.EntityId = PROJECT_ENTITY_ID;
					listItem.EntityName = PROJECT_ENTITY_NAME;
					listItem.FieldId = createListEntity.Fields.Single(x => x.Name == "name").Id;
					listItem.FieldName = "name";
					listItem.Type = "field";
					createListInput.Columns.Add(listItem);
					#endregion

					#region << Queries >>
					{
						if (createListInput.Query == null)
						{
							createListInput.Query = new InputRecordListQuery();
						}
						createListInput.Query.FieldName = null;
						createListInput.Query.FieldValue = null;
						createListInput.Query.QueryType = "AND"; //AND,OR,EQ,NOT,LT,LTE,GT,GTE,CONTAINS,STARTSWITH
						createListInput.Query.SubQueries = new List<InputRecordListQuery>();
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "name";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""name"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "CONTAINS";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						createListInput.Query.SubQueries.Add(subQuery);
					}
					#endregion
					{
						var response = entMan.CreateRecordList(PROJECT_ENTITY_ID, createListInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + PROJECT_ENTITY_NAME + " Updated List: lookup" + " Message:" + response.Message);
					}
				}
				#endregion

			}
			#endregion

			#region << wv_milestone >>
			{
				#region << entity >>
				{
					InputEntity entity = new InputEntity();
					entity.Id = MILESTONE_ENTITY_ID;
					entity.Name = MILESTONE_ENTITY_NAME;
					entity.Label = "Milestone";
					entity.LabelPlural = "Milestones";
					entity.System = true;
					entity.IconName = "map-signs";
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
					systemItemIdDictionary["id"] = new Guid("28c8351b-991d-420f-9a42-57201572571c");
					systemItemIdDictionary["created_on"] = new Guid("7aa2003e-92d7-4073-ae59-6b9f02d87516");
					systemItemIdDictionary["created_by"] = new Guid("b31ee492-b9ff-47b7-a79d-938935742e40");
					systemItemIdDictionary["last_modified_on"] = new Guid("54c925f5-4f2a-4c55-883f-9ac450d807fe");
					systemItemIdDictionary["last_modified_by"] = new Guid("f78a6530-7bc7-429d-9c38-27370fce19c0");
					systemItemIdDictionary["user_wv_milestone_created_by"] = new Guid("7a6b7dca-ae20-4af5-a888-b3b6e2adf1aa");
					systemItemIdDictionary["user_wv_milestone_modified_by"] = new Guid("1f0aa3d9-f50a-496c-bc86-b3e06a7ad8d9");
					{
						var response = entMan.CreateEntity(entity, false, false, systemItemIdDictionary);
						if (!response.Success)
							throw new Exception("System error 10050. Entity: " + MILESTONE_ENTITY_NAME + " Field: entity creation" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << name >>
				{
					InputTextField textboxField = new InputTextField();
					textboxField.Id = new Guid("94cc3894-110a-4bb7-8c75-3e887cc83217");
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
						var response = entMan.CreateField(MILESTONE_ENTITY_ID, textboxField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + MILESTONE_ENTITY_NAME + " Field: name" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << start_date >>
				{
					InputDateField dateField = new InputDateField();
					dateField.Id = new Guid("9502a7e4-816c-433c-9f1e-6b1e2dffad62");
					dateField.Name = "start_date";
					dateField.Label = "Start date";
					dateField.PlaceholderText = "";
					dateField.Description = "";
					dateField.HelpText = "";
					dateField.Required = false;
					dateField.Unique = false;
					dateField.Searchable = false;
					dateField.Auditable = false;
					dateField.System = true;
					dateField.DefaultValue = null;
					dateField.Format = "dd MMM yyyy";
					dateField.UseCurrentTimeAsDefaultValue = true;
					dateField.EnableSecurity = true;
					dateField.Permissions = new FieldPermissions();
					dateField.Permissions.CanRead = new List<Guid>();
					dateField.Permissions.CanUpdate = new List<Guid>();
					//READ
					dateField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					dateField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					dateField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					{
						var response = entMan.CreateField(MILESTONE_ENTITY_ID, dateField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + MILESTONE_ENTITY_NAME + " Field: start_date" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << end_date >>
				{
					InputDateField dateField = new InputDateField();
					dateField.Id = new Guid("1252a300-c871-4d79-8242-f036705cc86d");
					dateField.Name = "end_date";
					dateField.Label = "End date";
					dateField.PlaceholderText = "";
					dateField.Description = "";
					dateField.HelpText = "";
					dateField.Required = false;
					dateField.Unique = false;
					dateField.Searchable = false;
					dateField.Auditable = false;
					dateField.System = true;
					dateField.DefaultValue = null;
					dateField.Format = "dd MMM yyyy";
					dateField.UseCurrentTimeAsDefaultValue = true;
					dateField.EnableSecurity = true;
					dateField.Permissions = new FieldPermissions();
					dateField.Permissions.CanRead = new List<Guid>();
					dateField.Permissions.CanUpdate = new List<Guid>();
					//READ
					dateField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					dateField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					dateField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					{
						var response = entMan.CreateField(MILESTONE_ENTITY_ID, dateField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + MILESTONE_ENTITY_NAME + " Field: end_date" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << status >>
				{
					InputSelectField dropdownField = new InputSelectField();
					dropdownField.Id = new Guid("63eed358-9b33-4d2c-b2cd-b533413df227");
					dropdownField.Name = "status";
					dropdownField.Label = "Status";
					dropdownField.PlaceholderText = "";
					dropdownField.Description = "";
					dropdownField.HelpText = "";
					dropdownField.Required = true;
					dropdownField.Unique = false;
					dropdownField.Searchable = false;
					dropdownField.Auditable = false;
					dropdownField.System = true;
					dropdownField.DefaultValue = "opened";
					dropdownField.Options = new List<SelectFieldOption>
								{
									new SelectFieldOption(){ Key = "opened", Value = "opened" },
									new SelectFieldOption(){ Key = "completed", Value = "completed" }
								};
					dropdownField.EnableSecurity = true;
					dropdownField.Permissions = new FieldPermissions();
					dropdownField.Permissions.CanRead = new List<Guid>();
					dropdownField.Permissions.CanUpdate = new List<Guid>();
					//READ
					dropdownField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					dropdownField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					dropdownField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					dropdownField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(MILESTONE_ENTITY_ID, dropdownField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + MILESTONE_ENTITY_NAME + " Field: status" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << project_id >>
				{
					InputGuidField guidField = new InputGuidField();
					guidField.Id = new Guid("f1540e81-be80-4fed-b0c6-1d538b1dd17a");
					guidField.Name = "project_id";
					guidField.Label = "Project";
					guidField.PlaceholderText = "";
					guidField.Description = "";
					guidField.HelpText = "";
					guidField.Required = true;
					guidField.Unique = false;
					guidField.Searchable = false;
					guidField.Auditable = false;
					guidField.System = true;
					guidField.DefaultValue = Guid.Empty;
					guidField.GenerateNewId = false;
					guidField.EnableSecurity = true;
					guidField.Permissions = new FieldPermissions();
					guidField.Permissions.CanRead = new List<Guid>();
					guidField.Permissions.CanUpdate = new List<Guid>();
					//READ
					guidField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					guidField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);

					{
						var response = entMan.CreateField(MILESTONE_ENTITY_ID, guidField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + MILESTONE_ENTITY_NAME + " Field: project_id" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << x_tasks_not_started >>
				{
					InputNumberField numberField = new InputNumberField();
					numberField.Id = new Guid("aacfb1b6-8318-4c88-ad9d-7fa0ad799537");
					numberField.Name = "x_tasks_not_started";
					numberField.Label = "x_tasks_not_started";
					numberField.PlaceholderText = "";
					numberField.Description = "";
					numberField.HelpText = "";
					numberField.Required = true;
					numberField.Unique = false;
					numberField.Searchable = false;
					numberField.Auditable = false;
					numberField.System = true;
					numberField.DefaultValue = 0;
					numberField.MinValue = null;
					numberField.MaxValue = null;
					numberField.DecimalPlaces = 0;
					numberField.EnableSecurity = true;
					numberField.Permissions = new FieldPermissions();
					numberField.Permissions.CanRead = new List<Guid>();
					numberField.Permissions.CanUpdate = new List<Guid>();
					//READ
					numberField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					numberField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(MILESTONE_ENTITY_ID, numberField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + MILESTONE_ENTITY_NAME + " Field: x_tasks_not_started" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << x_tasks_in_progress >>
				{
					InputNumberField numberField = new InputNumberField();
					numberField.Id = new Guid("07fb158a-2b3d-421f-a7f0-c296b5ce76c9");
					numberField.Name = "x_tasks_in_progress";
					numberField.Label = "x_tasks_in_progress";
					numberField.PlaceholderText = "";
					numberField.Description = "";
					numberField.HelpText = "";
					numberField.Required = true;
					numberField.Unique = false;
					numberField.Searchable = false;
					numberField.Auditable = false;
					numberField.System = true;
					numberField.DefaultValue = 0;
					numberField.MinValue = null;
					numberField.MaxValue = null;
					numberField.DecimalPlaces = 0;
					numberField.EnableSecurity = true;
					numberField.Permissions = new FieldPermissions();
					numberField.Permissions.CanRead = new List<Guid>();
					numberField.Permissions.CanUpdate = new List<Guid>();
					//READ
					numberField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					numberField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(MILESTONE_ENTITY_ID, numberField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + MILESTONE_ENTITY_NAME + " Field: x_tasks_in_progress" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << x_tasks_completed >>
				{
					InputNumberField numberField = new InputNumberField();
					numberField.Id = new Guid("407db47b-23be-4ce1-ad97-e0c63d8e5377");
					numberField.Name = "x_tasks_completed";
					numberField.Label = "x_tasks_completed";
					numberField.PlaceholderText = "";
					numberField.Description = "";
					numberField.HelpText = "";
					numberField.Required = true;
					numberField.Unique = false;
					numberField.Searchable = false;
					numberField.Auditable = false;
					numberField.System = true;
					numberField.DefaultValue = 0;
					numberField.MinValue = null;
					numberField.MaxValue = null;
					numberField.DecimalPlaces = 0;
					numberField.EnableSecurity = true;
					numberField.Permissions = new FieldPermissions();
					numberField.Permissions.CanRead = new List<Guid>();
					numberField.Permissions.CanUpdate = new List<Guid>();
					//READ
					numberField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					numberField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(MILESTONE_ENTITY_ID, numberField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + MILESTONE_ENTITY_NAME + " Field: x_tasks_completed" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << x_bugs_opened >>
				{
					InputNumberField numberField = new InputNumberField();
					numberField.Id = new Guid("014b2a7c-475d-4c0f-b992-0f6ddf3a5454");
					numberField.Name = "x_bugs_opened";
					numberField.Label = "x_bugs_opened";
					numberField.PlaceholderText = "";
					numberField.Description = "";
					numberField.HelpText = "";
					numberField.Required = true;
					numberField.Unique = false;
					numberField.Searchable = false;
					numberField.Auditable = false;
					numberField.System = true;
					numberField.DefaultValue = 0;
					numberField.MinValue = null;
					numberField.MaxValue = null;
					numberField.DecimalPlaces = 0;
					numberField.EnableSecurity = true;
					numberField.Permissions = new FieldPermissions();
					numberField.Permissions.CanRead = new List<Guid>();
					numberField.Permissions.CanUpdate = new List<Guid>();
					//READ
					numberField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					numberField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(MILESTONE_ENTITY_ID, numberField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + MILESTONE_ENTITY_NAME + " Field: x_bugs_opened" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << x_bugs_reopened >>
				{
					InputNumberField numberField = new InputNumberField();
					numberField.Id = new Guid("3432d7eb-690a-4076-be24-e583fa1e83c3");
					numberField.Name = "x_bugs_reopened";
					numberField.Label = "x_bugs_reopened";
					numberField.PlaceholderText = "";
					numberField.Description = "";
					numberField.HelpText = "";
					numberField.Required = true;
					numberField.Unique = false;
					numberField.Searchable = false;
					numberField.Auditable = false;
					numberField.System = true;
					numberField.DefaultValue = 0;
					numberField.MinValue = null;
					numberField.MaxValue = null;
					numberField.DecimalPlaces = 0;
					numberField.EnableSecurity = true;
					numberField.Permissions = new FieldPermissions();
					numberField.Permissions.CanRead = new List<Guid>();
					numberField.Permissions.CanUpdate = new List<Guid>();
					//READ
					numberField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					numberField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(MILESTONE_ENTITY_ID, numberField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + MILESTONE_ENTITY_NAME + " Field: x_bugs_reopened" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << x_bugs_closed >>
				{
					InputNumberField numberField = new InputNumberField();
					numberField.Id = new Guid("09bb35c9-ba7f-455f-8205-11a91fd7a90d");
					numberField.Name = "x_bugs_closed";
					numberField.Label = "x_bugs_closed";
					numberField.PlaceholderText = "";
					numberField.Description = "";
					numberField.HelpText = "";
					numberField.Required = true;
					numberField.Unique = false;
					numberField.Searchable = false;
					numberField.Auditable = false;
					numberField.System = true;
					numberField.DefaultValue = 0;
					numberField.MinValue = null;
					numberField.MaxValue = null;
					numberField.DecimalPlaces = 0;
					numberField.EnableSecurity = true;
					numberField.Permissions = new FieldPermissions();
					numberField.Permissions.CanRead = new List<Guid>();
					numberField.Permissions.CanUpdate = new List<Guid>();
					//READ
					numberField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					numberField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(MILESTONE_ENTITY_ID, numberField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + MILESTONE_ENTITY_NAME + " Field: x_bugs_closed" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << project_1_n_milestone Relation >>
				{
					var originEntity = entMan.ReadEntity(PROJECT_ENTITY_ID).Object;
					var targetEntity = entMan.ReadEntity(MILESTONE_ENTITY_ID).Object;
					EntityRelation oneToNRelation = new EntityRelation();
					oneToNRelation.Id = new Guid("0c446f98-eec2-40c1-9d66-8a3c2a2498e9");
					oneToNRelation.Name = "project_1_n_milestone";
					oneToNRelation.Label = "project_1_n_milestone";
					oneToNRelation.System = true;
					oneToNRelation.RelationType = EntityRelationType.OneToMany;
					oneToNRelation.OriginEntityId = originEntity.Id;
					oneToNRelation.OriginFieldId = originEntity.Fields.Single(x => x.Name == "id").Id;
					oneToNRelation.TargetEntityId = targetEntity.Id;
					oneToNRelation.TargetFieldId = targetEntity.Fields.Single(x => x.Name == "project_id").Id;
					{
						var result = relMan.Create(oneToNRelation);
						if (!result.Success)
							throw new Exception("CREATE project_1_n_milestone RELATION:" + result.Message);
					}
				}
				#endregion

				#region << create general list >>
				{
					var createListEntity = entMan.ReadEntity(MILESTONE_ENTITY_ID).Object;
					var createListInput = new InputRecordList();
					var listItem = new InputRecordListFieldItem();
					var listSort = new InputRecordListSort();
					var listQuery = new InputRecordListQuery();
					//General list details
					createListInput.Id = new Guid("92b40989-c3a2-4a06-869a-789fba54e733");
					createListInput.Name = "project_milestones";
					createListInput.Label = "Milestones";
					createListInput.Default = true;
					createListInput.System = true;
					createListInput.Type = "general";
					createListInput.IconName = "map-signs";
					createListInput.PageSize = 10;
					createListInput.Weight = 10;
					createListInput.VisibleColumnsCount = 5;
					createListInput.ServiceCode = null;
					createListInput.DynamicHtmlTemplate = "/plugins/webvella-projects/templates/project-milestones.html";
					createListInput.DataSourceUrl = "/plugins/webvella-projects/api/project/milestones-list";

					//Action items
					var newActionItemList = new List<ActionItem>();
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_create_record";
						actionItem.Menu = "page-title";
						actionItem.Template = "<a class=\"btn btn-default btn-outline hidden-xs\" ng-show=\"::ngCtrl.userHasRecordPermissions('canCreate')\" \n    ng-href=\"{{::ngCtrl.getRecordCreateUrl()}}\">Add New</a>";
						actionItem.Weight = 1;
						newActionItemList.Add(actionItem);
					}
					createListInput.ActionItems = newActionItemList;
					//Fields

					{
						var response = entMan.CreateRecordList(MILESTONE_ENTITY_ID, createListInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + MILESTONE_ENTITY_NAME + " Updated List: general" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << create create view >>
				{
					var createViewEntity = entMan.ReadEntity(MILESTONE_ENTITY_ID).Object;
					var createViewInput = new InputRecordView();
					var viewSection = new InputRecordViewSection();
					var viewRow = new InputRecordViewRow();
					var viewColumn = new InputRecordViewColumn();
					var viewItem = new InputRecordViewFieldItem();
					var viewItemFromRelation = new InputRecordViewRelationFieldItem();
					//General view fields

					#region << Details >>
					createViewInput.Id = new Guid("95160359-aa0a-419f-92f4-4102fc692411");
					createViewInput.Name = "create";
					createViewInput.Label = "Create";
					createViewInput.Title = "Create new milestone";
					createViewInput.CssClass = "";
					createViewInput.Default = true;
					createViewInput.System = true;
					createViewInput.Type = "create";
					createViewInput.Weight = 10;
					createViewInput.IconName = "file-text-o";
					createViewInput.ServiceCode = null;
					createViewInput.DynamicHtmlTemplate = null;
					createViewInput.DataSourceUrl = null;
					#endregion

					#region << Get the header Region >>
					var headerRegion = new InputRecordViewRegion();
					headerRegion.Name = "header";
					headerRegion.Label = "header";
					headerRegion.Sections = new List<InputRecordViewSection>();
					#endregion

					#region << Section >>
					viewSection = new InputRecordViewSection();
					viewSection.Id = new Guid("dd96bfb9-0dbc-4a6b-b7c6-92fb54b1844a");
					viewSection.Name = "details";
					viewSection.Label = "Details";
					viewSection.ShowLabel = false;
					viewSection.CssClass = "";
					viewSection.Collapsed = false;
					viewSection.TabOrder = "left-right";
					viewSection.Weight = 1;
					viewSection.Rows = new List<InputRecordViewRow>();

					#region << Row 1 >>
					viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("7f59d113-b469-4579-b780-961ead238e46");
					viewRow.Weight = 1;
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = 6;
					viewColumn.Items = new List<InputRecordViewItemBase>();


					#region << name >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = createViewEntity.Id;
						viewItem.EntityName = createViewEntity.Name;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "name").Id;
						viewItem.FieldName = "name";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion

					#region << start_date >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = createViewEntity.Id;
						viewItem.EntityName = createViewEntity.Name;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "start_date").Id;
						viewItem.FieldName = "start_date";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion

					#region << project name >>
					{
						var targetEntity = entMan.ReadEntity(PROJECT_ENTITY_ID).Object;
						var targetRelation = relMan.Read("project_1_n_milestone").Object;
						viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = targetEntity.Id;
						viewItemFromRelation.EntityName = targetEntity.Name;
						viewItemFromRelation.Type = "fieldFromRelation";
						viewItemFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "name").Id;
						viewItemFromRelation.FieldName = "name";
						viewItemFromRelation.FieldLabel = "Project";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldRequired = true;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = targetRelation.Id;
						viewItemFromRelation.RelationName = targetRelation.Name;
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion

					//Save column
					viewRow.Columns.Add(viewColumn);
					#endregion

					#region << Column 1 >>
					viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = 6;
					viewColumn.Items = new List<InputRecordViewItemBase>();


					#region << status >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = createViewEntity.Id;
						viewItem.EntityName = createViewEntity.Name;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "status").Id;
						viewItem.FieldName = "status";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion

					#region << end_date >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = createViewEntity.Id;
						viewItem.EntityName = createViewEntity.Name;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "end_date").Id;
						viewItem.FieldName = "end_date";
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
					headerRegion.Sections.Add(viewSection);

					#endregion

					createViewInput.Regions = new List<InputRecordViewRegion>();
					createViewInput.Regions.Add(headerRegion);

					#region << Sidebar >>
					createViewInput.Sidebar = new InputRecordViewSidebar();
					createViewInput.Sidebar.CssClass = "";
					createViewInput.Sidebar.Render = true;
					var sidebarItem = new InputRecordViewSidebarItemBase();
					
					#endregion								

					var newActionItemList = new List<ActionItem>();
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_back_button";
						actionItem.Menu = "sidebar-top";
						actionItem.Template = "<a class=\"back clearfix\" href=\"javascript:void(0)\" ng-click=\"sidebarData.goBack()\"><i class=\"fa fa-fw fa-arrow-left\"></i> <span class=\"text\">Back</span></a>";
						actionItem.Weight = 1;
						newActionItemList.Add(actionItem);
					}
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_create_and_list";
						actionItem.Menu = "create-bottom";
						actionItem.Template = "<a class=\"btn btn-primary\" ng-click='ngCtrl.create(\"default\")' ng-if=\"::ngCtrl.createViewRegion != null\">Create</a>";
						actionItem.Weight = 1;
						newActionItemList.Add(actionItem);
					}
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_create_and_details";
						actionItem.Menu = "create-bottom";
						actionItem.Template = "<a class=\"btn btn-default btn-outline\" ng-click='ngCtrl.create(\"details\")' ng-if=\"::ngCtrl.createViewRegion != null\">Create & Details</a>";
						actionItem.Weight = 2;
						newActionItemList.Add(actionItem);
					}
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_create_cancel";
						actionItem.Menu = "create-bottom";
						actionItem.Template = "<a class=\"btn btn-default btn-outline\" ng-click=\"ngCtrl.cancel()\">Cancel</a>";
						actionItem.Weight = 3;
						newActionItemList.Add(actionItem);
					}

					createViewInput.ActionItems = newActionItemList;

					{
						var response = entMan.CreateRecordView(MILESTONE_ENTITY_ID, createViewInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + MILESTONE_ENTITY_NAME + " Updated view: create" + " Message:" + response.Message);
					}
				}
				#endregion

			}
			#endregion

			#region << wv_task >>
			{
				#region << entity >>
				{
					InputEntity entity = new InputEntity();
					entity.Id = TASK_ENTITY_ID;
					entity.Name = TASK_ENTITY_NAME;
					entity.Label = "Task";
					entity.LabelPlural = "Tasks";
					entity.System = true;
					entity.IconName = "tasks";
					entity.Weight = 4;
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
					systemItemIdDictionary["id"] = new Guid("916b8d1e-d6b6-45a9-9060-b0717b2b8b73");
					systemItemIdDictionary["created_on"] = new Guid("6a83a91e-7eb8-4fb4-ab5f-5750cb4015d3");
					systemItemIdDictionary["created_by"] = new Guid("b6ae3dc9-0c79-478c-89c9-3502f9da319f");
					systemItemIdDictionary["last_modified_on"] = new Guid("55dcf9a2-79f7-42cc-8b3f-3cfea8f6dee3");
					systemItemIdDictionary["last_modified_by"] = new Guid("39fc45d5-c472-44ed-b3f3-4687fb4a7501");
					systemItemIdDictionary["user_wv_task_created_by"] = new Guid("0affc050-2c24-4ae3-bb5e-b08139661d83");
					systemItemIdDictionary["user_wv_task_modified_by"] = new Guid("5d4730b0-82b7-401e-8eb3-1e4250aa82a1");

					{
						var response = entMan.CreateEntity(entity, false, false, systemItemIdDictionary);
						if (!response.Success)
							throw new Exception("System error 10050. Entity: " + TASK_ENTITY_NAME + " Field: entity creation" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << number >>
				{
					InputAutoNumberField autonumberField = new InputAutoNumberField();
					autonumberField.Id = new Guid("1c7e71ce-125d-4afd-aa22-1c0a564bcb7b");
					autonumberField.Name = "number";
					autonumberField.Label = "Number";
					autonumberField.PlaceholderText = "";
					autonumberField.Description = "";
					autonumberField.HelpText = "";
					autonumberField.Required = true;
					autonumberField.Unique = true;
					autonumberField.Searchable = true;
					autonumberField.Auditable = false;
					autonumberField.System = true;
					autonumberField.DefaultValue = 0;
					autonumberField.DisplayFormat = "{0}";
					autonumberField.StartingNumber = 1;
					autonumberField.EnableSecurity = true;
					autonumberField.Permissions = new FieldPermissions();
					autonumberField.Permissions.CanRead = new List<Guid>();
					autonumberField.Permissions.CanUpdate = new List<Guid>();
					//READ
					autonumberField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					autonumberField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(TASK_ENTITY_ID, autonumberField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TASK_ENTITY_NAME + " Field: number" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << code >>
				{
					InputTextField textboxField = new InputTextField();
					textboxField.Id = new Guid("04f31ad8-4583-4237-8d54-f82c3f44b918");
					textboxField.Name = "code";
					textboxField.Label = "Code";
					textboxField.PlaceholderText = "";
					textboxField.Description = "";
					textboxField.HelpText = "";
					textboxField.Required = false;
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
					textboxField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(TASK_ENTITY_ID, textboxField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TASK_ENTITY_NAME + " Field: subject" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << subject >>
				{
					InputTextField textboxField = new InputTextField();
					textboxField.Id = new Guid("7843bfbd-30c1-4438-af48-ffe56b7f294a");
					textboxField.Name = "subject";
					textboxField.Label = "Subject";
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
					textboxField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(TASK_ENTITY_ID, textboxField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TASK_ENTITY_NAME + " Field: subject" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << owner_id >>
				{
					InputGuidField guidField = new InputGuidField();
					guidField.Id = new Guid("d9cfe549-5ae2-45ee-b7ee-1a14a84b0166");
					guidField.Name = "owner_id";
					guidField.Label = "Owned by";
					guidField.PlaceholderText = "";
					guidField.Description = "";
					guidField.HelpText = "";
					guidField.Required = true;
					guidField.Unique = false;
					guidField.Searchable = false;
					guidField.Auditable = false;
					guidField.System = true;
					guidField.DefaultValue = Guid.Empty;
					guidField.GenerateNewId = false;
					guidField.EnableSecurity = true;
					guidField.Permissions = new FieldPermissions();
					guidField.Permissions.CanRead = new List<Guid>();
					guidField.Permissions.CanUpdate = new List<Guid>();
					//READ
					guidField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					guidField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(TASK_ENTITY_ID, guidField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TASK_ENTITY_NAME + " Field: owner_id" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << milestone_id >>
				{
					InputGuidField guidField = new InputGuidField();
					guidField.Id = new Guid("6bb67bc2-a09c-4785-b316-c8795696fc17");
					guidField.Name = "milestone_id";
					guidField.Label = "Release milestone";
					guidField.PlaceholderText = "";
					guidField.Description = "";
					guidField.HelpText = "";
					guidField.Required = false;
					guidField.Unique = false;
					guidField.Searchable = false;
					guidField.Auditable = false;
					guidField.System = true;
					guidField.DefaultValue = Guid.Empty;
					guidField.GenerateNewId = false;
					guidField.EnableSecurity = true;
					guidField.Permissions = new FieldPermissions();
					guidField.Permissions.CanRead = new List<Guid>();
					guidField.Permissions.CanUpdate = new List<Guid>();
					//READ
					guidField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					guidField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(TASK_ENTITY_ID, guidField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TASK_ENTITY_NAME + " Field: milestone_id" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << description >>
				{
					InputHtmlField htmlField = new InputHtmlField();
					htmlField.Id = new Guid("a00eb247-918a-46ba-9869-8d1168ea8f45");
					htmlField.Name = "description";
					htmlField.Label = "Description";
					htmlField.PlaceholderText = "";
					htmlField.Description = "";
					htmlField.HelpText = "";
					htmlField.Required = false;
					htmlField.Unique = false;
					htmlField.Searchable = false;
					htmlField.Auditable = false;
					htmlField.System = true;
					htmlField.DefaultValue = string.Empty;
					htmlField.EnableSecurity = true;
					htmlField.Permissions = new FieldPermissions();
					htmlField.Permissions.CanRead = new List<Guid>();
					htmlField.Permissions.CanUpdate = new List<Guid>();

					htmlField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					htmlField.Permissions.CanRead.Add(SystemIds.RegularRoleId);

					htmlField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					htmlField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(TASK_ENTITY_ID, htmlField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TASK_ENTITY_NAME + " Field: description" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << parent_id >>
				{
					InputGuidField guidField = new InputGuidField();
					guidField.Id = new Guid("98f6cdf5-634c-494b-9e71-3aa7ee779328");
					guidField.Name = "parent_id";
					guidField.Label = "Parent task";
					guidField.PlaceholderText = "";
					guidField.Description = "";
					guidField.HelpText = "";
					guidField.Required = false;
					guidField.Unique = false;
					guidField.Searchable = false;
					guidField.Auditable = false;
					guidField.System = true;
					guidField.DefaultValue = Guid.Empty;
					guidField.GenerateNewId = false;
					guidField.EnableSecurity = true;
					guidField.Permissions = new FieldPermissions();
					guidField.Permissions.CanRead = new List<Guid>();
					guidField.Permissions.CanUpdate = new List<Guid>();
					//READ
					guidField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					guidField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(TASK_ENTITY_ID, guidField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TASK_ENTITY_NAME + " Field: parent_id" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << project_id >>
				{
					InputGuidField guidField = new InputGuidField();
					guidField.Id = new Guid("68e796c8-cc99-43b1-a285-2c6bc29f52f2");
					guidField.Name = "project_id";
					guidField.Label = "Project";
					guidField.PlaceholderText = "";
					guidField.Description = "";
					guidField.HelpText = "";
					guidField.Required = true;
					guidField.Unique = false;
					guidField.Searchable = false;
					guidField.Auditable = false;
					guidField.System = true;
					guidField.DefaultValue = Guid.Empty;
					guidField.GenerateNewId = false;
					guidField.EnableSecurity = true;
					guidField.Permissions = new FieldPermissions();
					guidField.Permissions.CanRead = new List<Guid>();
					guidField.Permissions.CanUpdate = new List<Guid>();
					//READ
					guidField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					guidField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					{
						var response = entMan.CreateField(TASK_ENTITY_ID, guidField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TASK_ENTITY_NAME + " Field: project_id" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << start_date >>
				{
					InputDateField dateField = new InputDateField();
					dateField.Id = new Guid("5bf852bf-5e6c-4791-bc8b-f1366440c04b");
					dateField.Name = "start_date";
					dateField.Label = "Start date";
					dateField.PlaceholderText = "";
					dateField.Description = "";
					dateField.HelpText = "";
					dateField.Required = false;
					dateField.Unique = false;
					dateField.Searchable = true;
					dateField.Auditable = false;
					dateField.System = true;
					dateField.DefaultValue = null;
					dateField.Format = "dd MMM yyyy";
					dateField.UseCurrentTimeAsDefaultValue = true;
					dateField.EnableSecurity = true;
					dateField.Permissions = new FieldPermissions();
					dateField.Permissions.CanRead = new List<Guid>();
					dateField.Permissions.CanUpdate = new List<Guid>();
					//READ
					dateField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					dateField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					dateField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					dateField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(TASK_ENTITY_ID, dateField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TASK_ENTITY_NAME + " Field: start_date" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << end_date >>
				{
					InputDateField dateField = new InputDateField();
					dateField.Id = new Guid("c6dc9db2-0081-4a91-b0e2-78f9c1c45a51");
					dateField.Name = "end_date";
					dateField.Label = "End date";
					dateField.PlaceholderText = "";
					dateField.Description = "";
					dateField.HelpText = "";
					dateField.Required = false;
					dateField.Unique = false;
					dateField.Searchable = true;
					dateField.Auditable = false;
					dateField.System = true;
					dateField.DefaultValue = null;
					dateField.Format = "dd MMM yyyy";
					dateField.UseCurrentTimeAsDefaultValue = true;
					dateField.EnableSecurity = true;
					dateField.Permissions = new FieldPermissions();
					dateField.Permissions.CanRead = new List<Guid>();
					dateField.Permissions.CanUpdate = new List<Guid>();
					//READ
					dateField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					dateField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					dateField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					dateField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(TASK_ENTITY_ID, dateField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TASK_ENTITY_NAME + " Field: end_date" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << priority >>
				{
					InputSelectField dropdownField = new InputSelectField();
					dropdownField.Id = new Guid("04ede478-99ec-4f7f-97af-0df3e89409b1");
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
											new SelectFieldOption(){ Key = "low", Value = "low" },
											new SelectFieldOption(){ Key = "medium", Value = "medium" },
											new SelectFieldOption(){ Key = "hight", Value = "hight" }
										};
					dropdownField.EnableSecurity = true;
					dropdownField.Permissions = new FieldPermissions();
					dropdownField.Permissions.CanRead = new List<Guid>();
					dropdownField.Permissions.CanUpdate = new List<Guid>();
					//READ
					dropdownField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					dropdownField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					dropdownField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					dropdownField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(TASK_ENTITY_ID, dropdownField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TASK_ENTITY_NAME + " Field: priority" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << status >>
				{
					InputSelectField dropdownField = new InputSelectField();
					dropdownField.Id = new Guid("0702e611-6fe5-42e4-9bad-d549cba9cbb1");
					dropdownField.Name = "status";
					dropdownField.Label = "Status";
					dropdownField.PlaceholderText = "";
					dropdownField.Description = "";
					dropdownField.HelpText = "";
					dropdownField.Required = true;
					dropdownField.Unique = false;
					dropdownField.Searchable = true;
					dropdownField.Auditable = false;
					dropdownField.System = true;
					dropdownField.DefaultValue = "not started";
					dropdownField.Options = new List<SelectFieldOption>
								{
									new SelectFieldOption(){ Key = "not started", Value = "not started" },
									new SelectFieldOption(){ Key = "in progress", Value = "in progress" },
									new SelectFieldOption(){ Key = "completed", Value = "completed" }
								};
					dropdownField.EnableSecurity = true;
					dropdownField.Permissions = new FieldPermissions();
					dropdownField.Permissions.CanRead = new List<Guid>();
					dropdownField.Permissions.CanUpdate = new List<Guid>();
					//READ
					dropdownField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					dropdownField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					dropdownField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					dropdownField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(TASK_ENTITY_ID, dropdownField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TASK_ENTITY_NAME + " Field: status" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << x_billable_hours >>
				{
					InputNumberField numberField = new InputNumberField();
					numberField.Id = new Guid("d79a25e4-3eeb-4a5b-84e8-294b0c146c4d");
					numberField.Name = "x_billable_hours";
					numberField.Label = "Billable hours";
					numberField.PlaceholderText = "";
					numberField.Description = "";
					numberField.HelpText = "";
					numberField.Required = false;
					numberField.Unique = false;
					numberField.Searchable = false;
					numberField.Auditable = false;
					numberField.System = true;
					numberField.DefaultValue = 0;
					numberField.MinValue = null;
					numberField.MaxValue = null;
					numberField.DecimalPlaces = 0;
					numberField.EnableSecurity = true;
					numberField.Permissions = new FieldPermissions();
					numberField.Permissions.CanRead = new List<Guid>();
					numberField.Permissions.CanUpdate = new List<Guid>();
					//READ
					numberField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					numberField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(TASK_ENTITY_ID, numberField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TASK_ENTITY_NAME + " Field: x_billable_hours" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << x_nonbillable_hours >>
				{
					InputNumberField numberField = new InputNumberField();
					numberField.Id = new Guid("a4196b7b-1de7-4106-b393-79eb3d1b4b79");
					numberField.Name = "x_nonbillable_hours";
					numberField.Label = "Non-Billable hours";
					numberField.PlaceholderText = "";
					numberField.Description = "";
					numberField.HelpText = "";
					numberField.Required = false;
					numberField.Unique = false;
					numberField.Searchable = false;
					numberField.Auditable = false;
					numberField.System = true;
					numberField.DefaultValue = 0;
					numberField.MinValue = null;
					numberField.MaxValue = null;
					numberField.DecimalPlaces = 0;
					numberField.EnableSecurity = true;
					numberField.Permissions = new FieldPermissions();
					numberField.Permissions.CanRead = new List<Guid>();
					numberField.Permissions.CanUpdate = new List<Guid>();
					//READ
					numberField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					numberField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(TASK_ENTITY_ID, numberField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TASK_ENTITY_NAME + " Field: x_nonbillable_hours" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << user_1_n_task_owner Relation >>
				{
					var originEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
					var targetEntity = entMan.ReadEntity(TASK_ENTITY_ID).Object;
					EntityRelation oneToNRelation = new EntityRelation();
					oneToNRelation.Id = new Guid("7ce76c81-e604-401e-907f-23de982b930e");
					oneToNRelation.Name = "user_1_n_task_owner";
					oneToNRelation.Label = "user_1_n_task_owner";
					oneToNRelation.System = true;
					oneToNRelation.RelationType = EntityRelationType.OneToMany;
					oneToNRelation.OriginEntityId = originEntity.Id;
					oneToNRelation.OriginFieldId = originEntity.Fields.Single(x => x.Name == "id").Id;
					oneToNRelation.TargetEntityId = targetEntity.Id;
					oneToNRelation.TargetFieldId = targetEntity.Fields.Single(x => x.Name == "owner_id").Id;
					{
						var result = relMan.Create(oneToNRelation);
						if (!result.Success)
							throw new Exception("CREATE user_1_n_task_owner RELATION:" + result.Message);
					}
				}
				#endregion

				#region << user_n_n_task_watchers Relation >>
				{
					var originEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
					var targetEntity = entMan.ReadEntity(TASK_ENTITY_ID).Object;
					EntityRelation NToNRelation = new EntityRelation();
					NToNRelation.Id = new Guid("de7e1578-8f8f-4454-a954-0fb62d3bf425");
					NToNRelation.Name = "user_n_n_task_watchers";
					NToNRelation.Label = "user_n_n_task_watchers";
					NToNRelation.System = true;
					NToNRelation.RelationType = EntityRelationType.ManyToMany;
					NToNRelation.OriginEntityId = originEntity.Id;
					NToNRelation.OriginFieldId = originEntity.Fields.Single(x => x.Name == "id").Id;
					NToNRelation.TargetEntityId = targetEntity.Id;
					NToNRelation.TargetFieldId = targetEntity.Fields.Single(x => x.Name == "id").Id;
					{
						var result = relMan.Create(NToNRelation);
						if (!result.Success)
							throw new Exception("CREATE user_n_n_task_watchers RELATION:" + result.Message);
					}
				}
				#endregion

				#region << task_1_n_task_parent Relation >>
				{
					var originEntity = entMan.ReadEntity(TASK_ENTITY_ID).Object;
					var targetEntity = entMan.ReadEntity(TASK_ENTITY_ID).Object;
					EntityRelation oneToNRelation = new EntityRelation();
					oneToNRelation.Id = new Guid("fd42ca83-9c08-4e7d-ba97-782208f44b18");
					oneToNRelation.Name = "task_1_n_task_parent";
					oneToNRelation.Label = "task_1_n_task_parent";
					oneToNRelation.System = true;
					oneToNRelation.RelationType = EntityRelationType.OneToMany;
					oneToNRelation.OriginEntityId = originEntity.Id;
					oneToNRelation.OriginFieldId = originEntity.Fields.Single(x => x.Name == "id").Id;
					oneToNRelation.TargetEntityId = targetEntity.Id;
					oneToNRelation.TargetFieldId = targetEntity.Fields.Single(x => x.Name == "parent_id").Id;
					{
						var result = relMan.Create(oneToNRelation);
						if (!result.Success)
							throw new Exception("CREATE task_1_n_task_parent RELATION:" + result.Message);
					}
				}
				#endregion

				#region << project_1_n_task Relation >>
				{
					var originEntity = entMan.ReadEntity(PROJECT_ENTITY_ID).Object;
					var targetEntity = entMan.ReadEntity(TASK_ENTITY_ID).Object;
					EntityRelation oneToNRelation = new EntityRelation();
					oneToNRelation.Id = new Guid("1f860b8c-7fa1-40fa-874f-19c2b5309817");
					oneToNRelation.Name = "project_1_n_task";
					oneToNRelation.Label = "project_1_n_task";
					oneToNRelation.System = true;
					oneToNRelation.RelationType = EntityRelationType.OneToMany;
					oneToNRelation.OriginEntityId = originEntity.Id;
					oneToNRelation.OriginFieldId = originEntity.Fields.Single(x => x.Name == "id").Id;
					oneToNRelation.TargetEntityId = targetEntity.Id;
					oneToNRelation.TargetFieldId = targetEntity.Fields.Single(x => x.Name == "project_id").Id;
					{
						var result = relMan.Create(oneToNRelation);
						if (!result.Success)
							throw new Exception("CREATE project_1_n_task RELATION:" + result.Message);
					}
				}
				#endregion

				#region << milestone_1_n_task Relation >>
				{
					var originEntity = entMan.ReadEntity(MILESTONE_ENTITY_ID).Object;
					var targetEntity = entMan.ReadEntity(TASK_ENTITY_ID).Object;
					EntityRelation oneToNRelation = new EntityRelation();
					oneToNRelation.Id = new Guid("3b600a1c-066e-42e2-a678-0de4f0f8a9e1");
					oneToNRelation.Name = "milestone_1_n_task";
					oneToNRelation.Label = "milestone_1_n_task";
					oneToNRelation.System = true;
					oneToNRelation.RelationType = EntityRelationType.OneToMany;
					oneToNRelation.OriginEntityId = originEntity.Id;
					oneToNRelation.OriginFieldId = originEntity.Fields.Single(x => x.Name == "id").Id;
					oneToNRelation.TargetEntityId = targetEntity.Id;
					oneToNRelation.TargetFieldId = targetEntity.Fields.Single(x => x.Name == "milestone_id").Id;
					{
						var result = relMan.Create(oneToNRelation);
						if (!result.Success)
							throw new Exception("CREATE milestone_1_n_task RELATION:" + result.Message);
					}
				}
				#endregion

				#region << create general list >>
				{
					var createListEntity = entMan.ReadEntity(TASK_ENTITY_ID).Object;
					var createListInput = new InputRecordList();
					var listField = new InputRecordListFieldItem();
					var listFieldFromRelation = new InputRecordListRelationFieldItem();
					var listSort = new InputRecordListSort();
					var listQuery = new InputRecordListQuery();

					//General list details
					createListInput.Id = new Guid("44f8ed83-b7e8-4223-b02e-b5e35ed4bcc1");
					createListInput.Name = "project_tasks";
					createListInput.Label = "Project Tasks";
					createListInput.Default = true;
					createListInput.System = true;
					createListInput.IconName = "tasks";
					createListInput.Type = "hidden";
					createListInput.PageSize = 10;
					createListInput.Weight = 10;
					createListInput.ColumnWidthsCSV = "auto,30px,120px,120px,120px,120px";
					createListInput.CssClass = "task-list";
					createListInput.VisibleColumnsCount = 6;
					createListInput.ServiceCode = null;
					createListInput.DynamicHtmlTemplate = null;
					createListInput.DataSourceUrl = null;
					createListInput.Columns = new List<InputRecordListItemBase>();

					//Fields
					#region << subject >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "subject").Id;
						listField.FieldName = "subject";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << owner - image >>
					{
						var targetEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
						listFieldFromRelation = new InputRecordListRelationFieldItem();
						listFieldFromRelation.EntityId = targetEntity.Id;
						listFieldFromRelation.EntityName = targetEntity.Name;
						listFieldFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "image").Id;
						listFieldFromRelation.FieldName = "image";
						listFieldFromRelation.FieldLabel = "Owner";
						listFieldFromRelation.RelationId = new Guid("7ce76c81-e604-401e-907f-23de982b930e");
						listFieldFromRelation.RelationName = "user_1_n_task_owner";
						listFieldFromRelation.Type = "fieldFromRelation";
						createListInput.Columns.Add(listFieldFromRelation);
					}
					#endregion

					#region << start_date >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "start_date").Id;
						listField.FieldName = "start_date";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << end_date >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "end_date").Id;
						listField.FieldName = "end_date";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << status >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "status").Id;
						listField.FieldName = "status";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << priority >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "priority").Id;
						listField.FieldName = "priority";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << query main>>
					{
						listQuery = new InputRecordListQuery();
						listQuery.FieldName = null;
						listQuery.FieldValue = null;
						listQuery.QueryType = "AND";
						listQuery.SubQueries = new List<InputRecordListQuery>();

						#region << subject >>
						{
							var subQuery = new InputRecordListQuery();
							subQuery.FieldName = "subject";
							subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""subject"", ""default"": null, ""settings"":{}}";
							subQuery.QueryType = "CONTAINS";
							subQuery.SubQueries = new List<InputRecordListQuery>();
							listQuery.SubQueries.Add(subQuery);
						}
						#endregion

						#region << status >>
						{
							var subQuery = new InputRecordListQuery();
							subQuery.FieldName = "status";
							subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""status"", ""default"": null, ""settings"":{}}";
							subQuery.QueryType = "EQ";
							subQuery.SubQueries = new List<InputRecordListQuery>();
							listQuery.SubQueries.Add(subQuery);
						}
						#endregion

						#region << priority >>
						{
							var subQuery = new InputRecordListQuery();
							subQuery.FieldName = "priority";
							subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""priority"", ""default"": null, ""settings"":{}}";
							subQuery.QueryType = "EQ";
							subQuery.SubQueries = new List<InputRecordListQuery>();
							listQuery.SubQueries.Add(subQuery);
						}
						#endregion

						createListInput.Query = listQuery;
					}
					#endregion

					#region << Sort >>
					createListInput.Sorts = new List<InputRecordListSort>();
					{
						listSort = new InputRecordListSort();
						listSort.FieldName = @"{""name"":""url_sort"", ""option"": ""sortBy"", ""default"": ""end_date"", ""settings"":{""order"":""sortOrder""}}";
						listSort.SortType = "ascending";
						createListInput.Sorts.Add(listSort);
					}
					#endregion

					var newActionItemList = new List<ActionItem>();
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_create_record";
						actionItem.Menu = "page-title";
						actionItem.Template = "<a class=\"btn btn-default btn-outline hidden-xs\" ng-show=\"::ngCtrl.userHasRecordPermissions('canCreate')\" \n    ng-href=\"{{::ngCtrl.getRecordCreateUrl()}}\">Add New</a>";
						actionItem.Weight = 1;
						newActionItemList.Add(actionItem);
					}
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_import_records";
						actionItem.Menu = "page-title-dropdown";
						actionItem.Template = "<a ng-click=\"ngCtrl.openImportModal()\" class=\"ng-hide\" ng-show=\"::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')\">\n\t<i class=\"fa fa-fw fa-upload\"></i> Import CSV\n</a>";
						actionItem.Weight = 10;
						newActionItemList.Add(actionItem);
					}
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_export_records";
						actionItem.Menu = "page-title-dropdown";
						actionItem.Template = "<a ng-click=\"ngCtrl.openExportModal()\" class=\"ng-hide\" ng-show=\"::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')\">\n\t<i class=\"fa fa-fw fa-download\"></i> Export CSV\n</a>";
						actionItem.Weight = 11;
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
						var response = entMan.CreateRecordList(TASK_ENTITY_ID, createListInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TASK_ENTITY_NAME + " Updated List: general" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << List name: my_tasks >>
				{
					var createListEntity = entMan.ReadEntity(TASK_ENTITY_ID).Object;
					var createListInput = new InputRecordList();
					var listField = new InputRecordListFieldItem();
					var listFieldFromRelation = new InputRecordListRelationFieldItem();
					var listSort = new InputRecordListSort();
					var listQuery = new InputRecordListQuery();

					#region << details >>
					createListInput.Id = new Guid("42da1595-bf3f-49b1-a784-1218b07d668d");
					createListInput.Type = "general";
					createListInput.Name = "my_tasks";
					createListInput.Label = "My Owned Active Tasks";
					createListInput.Weight = 1;
					createListInput.Default = false;
					createListInput.System = true;
					createListInput.CssClass = "task-list";
					createListInput.IconName = "tasks";
					createListInput.VisibleColumnsCount = 7;
					createListInput.ColumnWidthsCSV = "100px,auto,30px,120px,120px,120px,120px";
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

					#region << code >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "code").Id;
						listField.FieldName = "code";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << subject >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "subject").Id;
						listField.FieldName = "subject";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << owner - image >>
					{
						var targetEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
						listFieldFromRelation = new InputRecordListRelationFieldItem();
						listFieldFromRelation.EntityId = targetEntity.Id;
						listFieldFromRelation.EntityName = targetEntity.Name;
						listFieldFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "image").Id;
						listFieldFromRelation.FieldName = "image";
						listFieldFromRelation.FieldLabel = "Owner";
						listFieldFromRelation.RelationId = new Guid("7ce76c81-e604-401e-907f-23de982b930e");
						listFieldFromRelation.RelationName = "user_1_n_task_owner";
						listFieldFromRelation.Type = "fieldFromRelation";
						createListInput.Columns.Add(listFieldFromRelation);
					}
					#endregion

					#region << start_date >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "start_date").Id;
						listField.FieldName = "start_date";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << end_date >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "end_date").Id;
						listField.FieldName = "end_date";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << status >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "status").Id;
						listField.FieldName = "status";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << priority >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "priority").Id;
						listField.FieldName = "priority";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#endregion

					#region << relation options >>
					createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
					#endregion

					#region << query >>
					createListInput.Query = new InputRecordListQuery();
					//Main query rule
					createListInput.Query.FieldName = null;
					createListInput.Query.FieldValue = null;
					createListInput.Query.QueryType = "AND";
					createListInput.Query.SubQueries = new List<InputRecordListQuery>();

					#region << Section 1 >>
					listQuery = new InputRecordListQuery();
					listQuery.FieldName = null;
					listQuery.FieldValue = null;
					listQuery.QueryType = "AND";
					listQuery.SubQueries = new List<InputRecordListQuery>();

					#region << owner_id >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "owner_id";
						subQuery.FieldValue = @"{""name"":""current_user"", ""option"": ""id"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "EQ";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion

					createListInput.Query.SubQueries.Add(listQuery);
					#endregion

					#region << Section 2 >>
					listQuery = new InputRecordListQuery();
					listQuery.FieldName = null;
					listQuery.FieldValue = null;
					listQuery.QueryType = "AND";
					listQuery.SubQueries = new List<InputRecordListQuery>();

					#region << code >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "code";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""code"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "CONTAINS";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion

					#region << subject >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "subject";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""subject"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "CONTAINS";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion

					#region << status >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "status";
						subQuery.FieldValue = "completed";
						subQuery.QueryType = "NOT";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion


					#region << priority >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "priority";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""priority"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "EQ";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion

					createListInput.Query.SubQueries.Add(listQuery);
					#endregion


					#endregion

					#region << Sort >>
					createListInput.Sorts = new List<InputRecordListSort>();
					listSort = new InputRecordListSort();
					listSort.FieldName = @"{""name"":""url_sort"", ""option"": ""sortBy"", ""default"": ""end_date"", ""settings"":{""order"":""sortOrder""}}";
					listSort.SortType = "descending";
					createListInput.Sorts.Add(listSort);
					#endregion
					{
						var response = entMan.CreateRecordList(TASK_ENTITY_ID, createListInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TASK_ENTITY_NAME + " Updated List: my_tickets" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << List name: created_tasks >>
				{
					var createListEntity = entMan.ReadEntity(TASK_ENTITY_ID).Object;
					var createListInput = new InputRecordList();
					var listField = new InputRecordListFieldItem();
					var listFieldFromRelation = new InputRecordListRelationFieldItem();
					var listSort = new InputRecordListSort();
					var listQuery = new InputRecordListQuery();

					#region << details >>
					createListInput.Id = new Guid("dcdc058e-2684-4b80-b011-25ccc3dab184");
					createListInput.Type = "general";
					createListInput.Name = "created_tasks";
					createListInput.Label = "Tasks created by me";
					createListInput.Weight = 3;
					createListInput.Default = false;
					createListInput.System = true;
					createListInput.CssClass = "task-list";
					createListInput.IconName = "tasks";
					createListInput.VisibleColumnsCount = 7;
					createListInput.ColumnWidthsCSV = "100px,auto,30px,120px,120px,120px,120px";
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

					#region << code >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "code").Id;
						listField.FieldName = "code";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << subject >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "subject").Id;
						listField.FieldName = "subject";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << owner - image >>
					{
						var targetEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
						listFieldFromRelation = new InputRecordListRelationFieldItem();
						listFieldFromRelation.EntityId = targetEntity.Id;
						listFieldFromRelation.EntityName = targetEntity.Name;
						listFieldFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "image").Id;
						listFieldFromRelation.FieldName = "image";
						listFieldFromRelation.FieldLabel = "Owner";
						listFieldFromRelation.RelationId = new Guid("7ce76c81-e604-401e-907f-23de982b930e");
						listFieldFromRelation.RelationName = "user_1_n_task_owner";
						listFieldFromRelation.Type = "fieldFromRelation";
						createListInput.Columns.Add(listFieldFromRelation);
					}
					#endregion

					#region << start_date >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "start_date").Id;
						listField.FieldName = "start_date";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << end_date >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "end_date").Id;
						listField.FieldName = "end_date";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << status >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "status").Id;
						listField.FieldName = "status";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << priority >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "priority").Id;
						listField.FieldName = "priority";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#endregion

					#region << relation options >>
					createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
					#endregion

					#region << query >>
					createListInput.Query = new InputRecordListQuery();
					//Main query rule
					createListInput.Query.FieldName = null;
					createListInput.Query.FieldValue = null;
					createListInput.Query.QueryType = "AND";
					createListInput.Query.SubQueries = new List<InputRecordListQuery>();

					#region << Section 1 >>
					listQuery = new InputRecordListQuery();
					listQuery.FieldName = null;
					listQuery.FieldValue = null;
					listQuery.QueryType = "AND";
					listQuery.SubQueries = new List<InputRecordListQuery>();

					#region << created_by >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "created_by";
						subQuery.FieldValue = @"{""name"":""current_user"", ""option"": ""id"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "EQ";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion

					createListInput.Query.SubQueries.Add(listQuery);
					#endregion

					#region << Section 2 >>
					listQuery = new InputRecordListQuery();
					listQuery.FieldName = null;
					listQuery.FieldValue = null;
					listQuery.QueryType = "AND";
					listQuery.SubQueries = new List<InputRecordListQuery>();

					#region << code >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "code";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""code"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "CONTAINS";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion

					#region << subject >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "subject";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""subject"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "CONTAINS";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion

					#region << status >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "status";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""status"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "EQ";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion


					#region << priority >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "priority";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""priority"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "EQ";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion

					createListInput.Query.SubQueries.Add(listQuery);
					#endregion

					#endregion

					#region << Sort >>
					createListInput.Sorts = new List<InputRecordListSort>();
					listSort = new InputRecordListSort();
					listSort.FieldName = @"{""name"":""url_sort"", ""option"": ""sortBy"", ""default"": ""end_date"", ""settings"":{""order"":""sortOrder""}}";
					listSort.SortType = "descending";
					createListInput.Sorts.Add(listSort);
					#endregion
					{
						var response = entMan.CreateRecordList(TASK_ENTITY_ID, createListInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TASK_ENTITY_NAME + " Updated List: my_tickets" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << List name: owned_tasks >>
				{
					var createListEntity = entMan.ReadEntity(TASK_ENTITY_ID).Object;
					var createListInput = new InputRecordList();
					var listField = new InputRecordListFieldItem();
					var listFieldFromRelation = new InputRecordListRelationFieldItem();
					var listSort = new InputRecordListSort();
					var listQuery = new InputRecordListQuery();

					#region << details >>
					createListInput.Id = new Guid("930b8c52-1cda-4419-9cf4-7a41af95d776");
					createListInput.Type = "general";
					createListInput.Name = "owned_tasks";
					createListInput.Label = "Tasks owned by me";
					createListInput.Weight = 2;
					createListInput.Default = false;
					createListInput.System = true;
					createListInput.CssClass = "task-list";
					createListInput.IconName = "tasks";
					createListInput.VisibleColumnsCount = 7;
					createListInput.ColumnWidthsCSV = "100px,auto,30px,120px,120px,120px,120px";
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

					#region << code >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "code").Id;
						listField.FieldName = "code";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << subject >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "subject").Id;
						listField.FieldName = "subject";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << owner - image >>
					{
						var targetEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
						listFieldFromRelation = new InputRecordListRelationFieldItem();
						listFieldFromRelation.EntityId = targetEntity.Id;
						listFieldFromRelation.EntityName = targetEntity.Name;
						listFieldFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "image").Id;
						listFieldFromRelation.FieldName = "image";
						listFieldFromRelation.FieldLabel = "Owner";
						listFieldFromRelation.RelationId = new Guid("7ce76c81-e604-401e-907f-23de982b930e");
						listFieldFromRelation.RelationName = "user_1_n_task_owner";
						listFieldFromRelation.Type = "fieldFromRelation";
						createListInput.Columns.Add(listFieldFromRelation);
					}
					#endregion

					#region << start_date >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "start_date").Id;
						listField.FieldName = "start_date";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << end_date >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "end_date").Id;
						listField.FieldName = "end_date";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << status >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "status").Id;
						listField.FieldName = "status";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << priority >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "priority").Id;
						listField.FieldName = "priority";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#endregion

					#region << relation options >>
					createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
					#endregion

					#region << query >>
					createListInput.Query = new InputRecordListQuery();
					//Main query rule
					createListInput.Query.FieldName = null;
					createListInput.Query.FieldValue = null;
					createListInput.Query.QueryType = "AND";
					createListInput.Query.SubQueries = new List<InputRecordListQuery>();

					#region << Section 1 >>
					listQuery = new InputRecordListQuery();
					listQuery.FieldName = null;
					listQuery.FieldValue = null;
					listQuery.QueryType = "OR";
					listQuery.SubQueries = new List<InputRecordListQuery>();

					#region << owner_id >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "owner_id";
						subQuery.FieldValue = @"{""name"":""current_user"", ""option"": ""id"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "EQ";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion

					createListInput.Query.SubQueries.Add(listQuery);
					#endregion

					#region << Section 2 >>
					listQuery = new InputRecordListQuery();
					listQuery.FieldName = null;
					listQuery.FieldValue = null;
					listQuery.QueryType = "AND";
					listQuery.SubQueries = new List<InputRecordListQuery>();

					#region << number >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "code";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""number"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "CONTAINS";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion

					#region << subject >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "subject";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""subject"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "CONTAINS";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion

					#region << status >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "status";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""status"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "EQ";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion


					#region << priority >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "priority";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""priority"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "EQ";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion

					createListInput.Query.SubQueries.Add(listQuery);
					#endregion

					#endregion

					#region << Sort >>
					createListInput.Sorts = new List<InputRecordListSort>();
					listSort = new InputRecordListSort();
					listSort.FieldName = @"{""name"":""url_sort"", ""option"": ""sortBy"", ""default"": ""end_date"", ""settings"":{""order"":""sortOrder""}}";
					listSort.SortType = "descending";
					createListInput.Sorts.Add(listSort);
					#endregion
					{
						var response = entMan.CreateRecordList(TASK_ENTITY_ID, createListInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TASK_ENTITY_NAME + " Updated List: my_tickets" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << List name: all_tasks >>
				{
					var createListEntity = entMan.ReadEntity(TASK_ENTITY_ID).Object;
					var createListInput = new InputRecordList();
					var listField = new InputRecordListFieldItem();
					var listFieldFromRelation = new InputRecordListRelationFieldItem();
					var listSort = new InputRecordListSort();
					var listQuery = new InputRecordListQuery();

					#region << details >>
					createListInput.Id = new Guid("6b6ac308-bb21-46a3-b83c-0559348b2f46");
					createListInput.Type = "general";
					createListInput.Name = "all_tasks";
					createListInput.Label = "All Tasks";
					createListInput.Weight = 12;
					createListInput.Default = false;
					createListInput.System = true;
					createListInput.CssClass = "task-list";
					createListInput.IconName = "tasks";
					createListInput.VisibleColumnsCount = 7;
					createListInput.ColumnWidthsCSV = "100px,auto,30px,120px,120px,120px,120px";
					createListInput.PageSize = 10;
					createListInput.DynamicHtmlTemplate = null;
					createListInput.DataSourceUrl = @"/plugins/webvella-projects/api/task/list/all";
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

					#region << code >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "code").Id;
						listField.FieldName = "code";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << subject >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "subject").Id;
						listField.FieldName = "subject";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << owner - image >>
					{
						var targetEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
						listFieldFromRelation = new InputRecordListRelationFieldItem();
						listFieldFromRelation.EntityId = targetEntity.Id;
						listFieldFromRelation.EntityName = targetEntity.Name;
						listFieldFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "image").Id;
						listFieldFromRelation.FieldName = "image";
						listFieldFromRelation.FieldLabel = "Owner";
						listFieldFromRelation.RelationId = new Guid("7ce76c81-e604-401e-907f-23de982b930e");
						listFieldFromRelation.RelationName = "user_1_n_task_owner";
						listFieldFromRelation.Type = "fieldFromRelation";
						createListInput.Columns.Add(listFieldFromRelation);
					}
					#endregion

					#region << start_date >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "start_date").Id;
						listField.FieldName = "start_date";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << end_date >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "end_date").Id;
						listField.FieldName = "end_date";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << status >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "status").Id;
						listField.FieldName = "status";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << priority >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "priority").Id;
						listField.FieldName = "priority";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#endregion


					#region << query >>
					createListInput.Query = new InputRecordListQuery();
					//Main query rule
					createListInput.Query.FieldName = null;
					createListInput.Query.FieldValue = null;
					createListInput.Query.QueryType = "AND";
					createListInput.Query.SubQueries = new List<InputRecordListQuery>();

					#region << code >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "code";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""code"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "CONTAINS";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						createListInput.Query.SubQueries.Add(subQuery);
					}
					#endregion

					#region << subject >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "subject";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""subject"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "CONTAINS";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						createListInput.Query.SubQueries.Add(subQuery);
					}
					#endregion

					#region << status >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "status";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""status"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "EQ";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						createListInput.Query.SubQueries.Add(subQuery);
					}
					#endregion

					#region << priority >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "priority";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""priority"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "EQ";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						createListInput.Query.SubQueries.Add(subQuery);
					}
					#endregion

					#endregion

					#region << Sort >>
					createListInput.Sorts = new List<InputRecordListSort>();
					listSort = new InputRecordListSort();
					listSort.FieldName = @"{""name"":""url_sort"", ""option"": ""sortBy"", ""default"": ""end_date"", ""settings"":{""order"":""sortOrder""}}";
					listSort.SortType = "descending";
					createListInput.Sorts.Add(listSort);
					#endregion

					{
						var response = entMan.CreateRecordList(TASK_ENTITY_ID, createListInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TASK_ENTITY_NAME + " Updated List: all_tickets" + " Message:" + response.Message);
					}

				}
				#endregion

				#region << List name: admin >>
				{
					var createListEntity = entMan.ReadEntity(TASK_ENTITY_ID).Object;
					var createListInput = new InputRecordList();
					var listField = new InputRecordListFieldItem();
					var listFieldFromRelation = new InputRecordListRelationFieldItem();
					var listSort = new InputRecordListSort();
					var listQuery = new InputRecordListQuery();

					#region << details >>
					createListInput.Id = new Guid("10dce98d-843f-48ca-94be-da33a910375e");
					createListInput.Type = "hidden";
					createListInput.Name = "admin";
					createListInput.Label = "All tasks";
					createListInput.Weight = 2;
					createListInput.Default = false;
					createListInput.System = true;
					createListInput.CssClass = "task-list";
					createListInput.IconName = "tasks";
					createListInput.VisibleColumnsCount = 7;
					createListInput.ColumnWidthsCSV = "80px,auto,30px,120px,120px,120px,120px";
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

					#region << code >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "code").Id;
						listField.FieldName = "code";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << subject >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "subject").Id;
						listField.FieldName = "subject";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << owner - image >>
					{
						var targetEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
						listFieldFromRelation = new InputRecordListRelationFieldItem();
						listFieldFromRelation.EntityId = targetEntity.Id;
						listFieldFromRelation.EntityName = targetEntity.Name;
						listFieldFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "image").Id;
						listFieldFromRelation.FieldName = "image";
						listFieldFromRelation.FieldLabel = "Owner";
						listFieldFromRelation.RelationId = new Guid("7ce76c81-e604-401e-907f-23de982b930e");
						listFieldFromRelation.RelationName = "user_1_n_task_owner";
						listFieldFromRelation.Type = "fieldFromRelation";
						createListInput.Columns.Add(listFieldFromRelation);
					}
					#endregion

					#region << start_date >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "start_date").Id;
						listField.FieldName = "start_date";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << end_date >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "end_date").Id;
						listField.FieldName = "end_date";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << status >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "status").Id;
						listField.FieldName = "status";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << priority >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "priority").Id;
						listField.FieldName = "priority";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#endregion

					#region << relation options >>
					createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
					#endregion

					#region << query >>
					createListInput.Query = new InputRecordListQuery();
					listQuery = new InputRecordListQuery();
					listQuery.FieldName = null;
					listQuery.FieldValue = null;
					listQuery.QueryType = "AND";
					listQuery.SubQueries = new List<InputRecordListQuery>();

					#region << code >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "owner_id";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""code"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "EQ";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion
					#region << subject >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "subject";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""subject"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "EQ";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion

					#region << status >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "status";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""status"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "EQ";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion

					#region << priority >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "priority";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""priority"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "EQ";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion

					createListInput.Query = listQuery;
					#endregion

					#region << Sort >>
					createListInput.Sorts = new List<InputRecordListSort>();
					listSort = new InputRecordListSort();
					listSort.FieldName = @"{""name"":""url_sort"", ""option"": ""sortBy"", ""default"": ""end_date"", ""settings"":{""order"":""sortOrder""}}";
					listSort.SortType = "descending";
					createListInput.Sorts.Add(listSort);
					#endregion
					{
						var response = entMan.CreateRecordList(TASK_ENTITY_ID, createListInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TASK_ENTITY_NAME + " Updated List: admin" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << create create >>
				{
					var createViewEntity = entMan.ReadEntity(TASK_ENTITY_ID).Object;
					var createViewInput = new InputRecordView();
					var viewSection = new InputRecordViewSection();
					var viewRow = new InputRecordViewRow();
					var viewColumn = new InputRecordViewColumn();
					var viewItem = new InputRecordViewFieldItem();
					var viewItemFromRelation = new InputRecordViewRelationFieldItem();
					//General view fields

					#region << Details >>
					createViewInput.Id = new Guid("b879c8f8-1738-4f33-94aa-89064f227ed9");
					createViewInput.Name = "create";
					createViewInput.Label = "Create";
					createViewInput.Title = "";
					createViewInput.Default = true;
					createViewInput.System = true;
					createViewInput.Type = "create";
					createViewInput.Weight = 10;
					createViewInput.IconName = "file-text-o";
					createViewInput.ServiceCode = null;
					createViewInput.DynamicHtmlTemplate = null;
					createViewInput.DataSourceUrl = null;
					#endregion

					#region << Get the header Region >>
					var headerRegion = new InputRecordViewRegion();
					headerRegion.Name = "header";
					headerRegion.Label = "header";
					headerRegion.Sections = new List<InputRecordViewSection>();
					#endregion

					#region << Section >>
					viewSection = new InputRecordViewSection();
					viewSection.Id = new Guid("8b628f5d-16b3-49d0-a433-a910ea208b39");
					viewSection.Name = "details";
					viewSection.Label = "Details";
					viewSection.ShowLabel = false;
					viewSection.CssClass = "";
					viewSection.Collapsed = false;
					viewSection.TabOrder = "left-right";
					viewSection.Weight = 1;
					viewSection.Rows = new List<InputRecordViewRow>();

					#region << Row 1 >>
					viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("818f516c-f6c2-4073-8574-75c13a72aee4");
					viewRow.Weight = 1;
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = 12;
					viewColumn.Items = new List<InputRecordViewItemBase>();


					#region << subject >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = createViewEntity.Id;
						viewItem.EntityName = createViewEntity.Name;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "subject").Id;
						viewItem.FieldName = "subject";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion

					#region << project name >>
					{
						var targetEntity = entMan.ReadEntity(PROJECT_ENTITY_ID).Object;
						viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = targetEntity.Id;
						viewItemFromRelation.EntityName = targetEntity.Name;
						viewItemFromRelation.Type = "fieldFromRelation";
						viewItemFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "name").Id;
						viewItemFromRelation.FieldName = "name";
						viewItemFromRelation.FieldLabel = "Project";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldRequired = true;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = new Guid("1f860b8c-7fa1-40fa-874f-19c2b5309817");
						viewItemFromRelation.RelationName = "project_1_n_task";
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion


					#region << description >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = createViewEntity.Id;
						viewItem.EntityName = createViewEntity.Name;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "description").Id;
						viewItem.FieldName = "description";
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

					#region << Row 2 >>
					viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("2144c60b-4974-44e2-86ef-5ceec72d04f8");
					viewRow.Weight = 2;
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = 6;
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << status >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = createViewEntity.Id;
						viewItem.EntityName = createViewEntity.Name;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "status").Id;
						viewItem.FieldName = "status";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion

					#region << start_date >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = createViewEntity.Id;
						viewItem.EntityName = createViewEntity.Name;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "start_date").Id;
						viewItem.FieldName = "start_date";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion

					//Save column
					viewRow.Columns.Add(viewColumn);
					#endregion

					#region << Column 2 >>
					viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = 6;
					viewColumn.Items = new List<InputRecordViewItemBase>();


					#region << priority >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = createViewEntity.Id;
						viewItem.EntityName = createViewEntity.Name;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "priority").Id;
						viewItem.FieldName = "priority";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion

					#region << end_date >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = createViewEntity.Id;
						viewItem.EntityName = createViewEntity.Name;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "end_date").Id;
						viewItem.FieldName = "end_date";
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
					headerRegion.Sections.Add(viewSection);

					#endregion
					createViewInput.Regions = new List<InputRecordViewRegion>();
					createViewInput.Regions.Add(headerRegion);

					#region << Sidebar >>
					var sidebarItem = new InputRecordViewSidebarItemBase();
					#endregion`								

					var newActionItemList = new List<ActionItem>();
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_back_button";
						actionItem.Menu = "sidebar-top";
						actionItem.Template = "<a class=\"back clearfix\" href=\"javascript:void(0)\" ng-click=\"sidebarData.goBack()\"><i class=\"fa fa-fw fa-arrow-left\"></i> <span class=\"text\">Back</span></a>";
						actionItem.Weight = 1;
						newActionItemList.Add(actionItem);
					}
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_create_and_list";
						actionItem.Menu = "create-bottom";
						actionItem.Template = "<a class=\"btn btn-primary\" ng-click='ngCtrl.create(\"default\")' ng-if=\"::ngCtrl.createViewRegion != null\">Create</a>";
						actionItem.Weight = 1;
						newActionItemList.Add(actionItem);
					}
					{
						var actionItem = new ActionItem();
						actionItem.Name = "";
						actionItem.Menu = "create-bottom";
						actionItem.Template = "<a class=\"btn btn-default btn-outline\" ng-click='ngCtrl.create(\"details\")' ng-if=\"::ngCtrl.createViewRegion != null\">Create & Details</a>";
						actionItem.Weight = 2;
						newActionItemList.Add(actionItem);
					}
					{
						var actionItem = new ActionItem();
						actionItem.Name = "";
						actionItem.Menu = "create-bottom";
						actionItem.Template = "<a class=\"btn btn-default btn-outline\" ng-click=\"ngCtrl.cancel()\">Cancel</a>";
						actionItem.Weight = 3;
						newActionItemList.Add(actionItem);
					}
					createViewInput.ActionItems = newActionItemList;

					{
						var response = entMan.CreateRecordView(TASK_ENTITY_ID, createViewInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TASK_ENTITY_NAME + " Updated view: create" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << View name: project_milestone >>
				{
					var createViewEntity = entMan.ReadEntity(TASK_ENTITY_ID).Object;
					var createViewInput = new InputRecordView();
					var viewRegion = new InputRecordViewRegion();
					var viewSection = new InputRecordViewSection();
					var viewRow = new InputRecordViewRow();
					var viewColumn = new InputRecordViewColumn();
					var viewItem = new InputRecordViewFieldItem();
					var viewItemFromRelation = new InputRecordViewRelationFieldItem();

					#region << details >>
					createViewInput.Id = new Guid("820b6771-3100-4393-982b-3813d79f4df2");
					createViewInput.Type = "hidden";
					createViewInput.Name = "project_milestone";
					createViewInput.Label = "Project & Milestone";
					createViewInput.Title = "";
					createViewInput.Default = false;
					createViewInput.System = true;
					createViewInput.Weight = 10;
					createViewInput.CssClass = null;
					createViewInput.IconName = "code";
					createViewInput.DynamicHtmlTemplate = "/plugins/webvella-projects/templates/task-project-milestone-selection.html";
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
					viewSection.Id = new Guid("e886ea83-cb6f-408e-9fa9-a53cd249b714");
					viewSection.Name = "details";
					viewSection.Label = "Details";
					viewSection.ShowLabel = false;
					viewSection.CssClass = "";
					viewSection.Collapsed = false;
					viewSection.TabOrder = "left-right";
					viewSection.Weight = 1;
					viewSection.Rows = new List<InputRecordViewRow>();

					#region << Row Column>>
					viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("20be17ac-e915-4ac3-87a3-ab1ef534975f");
					viewRow.Weight = 1;
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = 12;
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << milestone_1_n_task>name from Relation >>
					{
						var targetEntity = entMan.ReadEntity(MILESTONE_ENTITY_ID).Object;
						var targetRelation = relMan.Read("milestone_1_n_task").Object;
						viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = targetEntity.Id;
						viewItemFromRelation.EntityName = targetEntity.Name;
						viewItemFromRelation.Type = "fieldFromRelation";
						viewItemFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "name").Id;
						viewItemFromRelation.FieldName = "name";
						viewItemFromRelation.FieldLabel = "Milestone";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldRequired = true;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = targetRelation.Id;
						viewItemFromRelation.RelationName = targetRelation.Name;
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion

					#region <<  project_1_n_task>name from Relation >>
					{
						var targetEntity = entMan.ReadEntity(PROJECT_ENTITY_ID).Object;
						var targetRelation = relMan.Read("project_1_n_task").Object;
						viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = targetEntity.Id;
						viewItemFromRelation.EntityName = targetEntity.Name;
						viewItemFromRelation.Type = "fieldFromRelation";
						viewItemFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "name").Id;
						viewItemFromRelation.FieldName = "name";
						viewItemFromRelation.FieldLabel = "Milestone";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldRequired = true;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = targetRelation.Id;
						viewItemFromRelation.RelationName = targetRelation.Name;
						viewColumn.Items.Add(viewItemFromRelation);
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

					createViewInput.RelationOptions = new List<EntityRelationOptionsItem>();
					createViewInput.ActionItems = new List<ActionItem>();
					#region << Sidebar >>
					createViewInput.Sidebar = new InputRecordViewSidebar();
					createViewInput.Sidebar.CssClass = "";
					createViewInput.Sidebar.Render = true;

					#endregion

					{
						var response = entMan.CreateRecordView(createViewEntity.Id, createViewInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + createViewEntity.Name + " Updated view: project_milestone" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << create general >>
				{
					var createViewEntity = entMan.ReadEntity(TASK_ENTITY_ID).Object;
					var createViewInput = new InputRecordView();
					var viewSection = new InputRecordViewSection();
					var viewRow = new InputRecordViewRow();
					var viewColumn = new InputRecordViewColumn();
					var viewItem = new InputRecordViewFieldItem();
					var viewItemFromRelation = new InputRecordViewRelationFieldItem();
					var viewItemView = new InputRecordViewViewItem();
					//General view fields

					#region << Details >>
					createViewInput.Id = new Guid("f3d3a025-ffd5-4eba-86d3-42bde882f597");
					createViewInput.Name = "general";
					createViewInput.Label = "[{code}] {subject}";
					createViewInput.Title = "";
					createViewInput.Type = "general";
					createViewInput.Default = true;
					createViewInput.System = true;
					createViewInput.IconName = "tasks";
					createViewInput.ServiceCode = "";
					#endregion

					#region << Get the header Region >>
					var headerRegion = new InputRecordViewRegion();
					headerRegion.Name = "header";
					headerRegion.Label = "header";
					headerRegion.Sections = new List<InputRecordViewSection>();
					#endregion

					#region << Section >>
					viewSection = new InputRecordViewSection();
					viewSection.Id = new Guid("0289b876-b6be-4d5f-915b-22dc0428bc25");
					viewSection.Name = "details";
					viewSection.Label = "Details";
					viewSection.ShowLabel = false;
					viewSection.CssClass = "";
					viewSection.Collapsed = false;
					viewSection.TabOrder = "left-right";
					viewSection.Weight = 1;
					viewSection.Rows = new List<InputRecordViewRow>();

					#region << Row 1 >>
					viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("cbf260ae-07e3-4e66-be57-beb7a36779bf");
					viewRow.Weight = 1;
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = 8;
					viewColumn.Items = new List<InputRecordViewItemBase>();


					#region << subject >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = createViewEntity.Id;
						viewItem.EntityName = createViewEntity.Name;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "subject").Id;
						viewItem.FieldName = "subject";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion

					#region << project_milestone name >>
					{
						var targetEntity = entMan.ReadEntity(TASK_ENTITY_ID).Object;
						viewItemView = new InputRecordViewViewItem();
						viewItemView.EntityId = targetEntity.Id;
						viewItemView.EntityName = targetEntity.Name;
						viewItemView.Type = "view";
						viewItemView.ViewId = targetEntity.RecordViews.Single(x => x.Name == "project_milestone").Id;
						viewColumn.Items.Add(viewItemView);
					}
					#endregion

					#region << description >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = createViewEntity.Id;
						viewItem.EntityName = createViewEntity.Name;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "description").Id;
						viewItem.FieldName = "description";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion


					//Save column
					viewRow.Columns.Add(viewColumn);
					#endregion

					#region << Column 2 >>
					viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = 4;
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << code >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = createViewEntity.Id;
						viewItem.EntityName = createViewEntity.Name;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "code").Id;
						viewItem.FieldName = "code";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion

					#region << status >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = createViewEntity.Id;
						viewItem.EntityName = createViewEntity.Name;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "status").Id;
						viewItem.FieldName = "status";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion

					#region << priority >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = createViewEntity.Id;
						viewItem.EntityName = createViewEntity.Name;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "priority").Id;
						viewItem.FieldName = "priority";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion

					#region << owner >>
					{
						var targetEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
						viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = targetEntity.Id;
						viewItemFromRelation.EntityName = targetEntity.Name;
						viewItemFromRelation.Type = "fieldFromRelation";
						viewItemFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "username").Id;
						viewItemFromRelation.FieldName = "username";
						viewItemFromRelation.FieldLabel = "Owner";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldRequired = true;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = new Guid("7ce76c81-e604-401e-907f-23de982b930e");
						viewItemFromRelation.RelationName = "user_1_n_task_owner";
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion

					#region << start_date >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = createViewEntity.Id;
						viewItem.EntityName = createViewEntity.Name;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "start_date").Id;
						viewItem.FieldName = "start_date";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion

					#region << end_date >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = createViewEntity.Id;
						viewItem.EntityName = createViewEntity.Name;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "end_date").Id;
						viewItem.FieldName = "end_date";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion

					#region << watchers >>
					{
						var targetEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
						var targetRelation = relMan.Read("user_n_n_task_watchers").Object;
						viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = targetEntity.Id;
						viewItemFromRelation.EntityName = targetEntity.Name;
						viewItemFromRelation.Type = "fieldFromRelation";
						viewItemFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "username").Id;
						viewItemFromRelation.FieldName = "username";
						viewItemFromRelation.FieldLabel = "Watchers";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldRequired = true;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = targetRelation.Id;
						viewItemFromRelation.RelationName = targetRelation.Name;
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion

					//Save column
					viewRow.Columns.Add(viewColumn);
					#endregion

					//Save row
					viewSection.Rows.Add(viewRow);
					#endregion

					//Save section
					headerRegion.Sections.Add(viewSection);

					#endregion

					createViewInput.Regions = new List<InputRecordViewRegion>();
					createViewInput.Regions.Add(headerRegion);

					#region << Sidebar >>
					var sidebarItem = new InputRecordViewSidebarItemBase();
					createViewInput.Sidebar = new InputRecordViewSidebar();
					createViewInput.Sidebar.CssClass = "";
					createViewInput.Sidebar.Render = true;

					#endregion

					#region << Action items edit >>
					var newActionItemList = new List<ActionItem>();
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_record_delete";
						actionItem.Menu = "page-title-dropdown";
						actionItem.Template = "<a href=\"javascript:void(0)\" confirmed-click=\"::ngCtrl.deleteRecord(ngCtrl)\" ng-confirm-click=\"Are you sure?\"\n\t\tng-if=\"::ngCtrl.userHasRecordPermissions('canDelete')\">\n\t<i class=\"fa fa-trash go-red\"></i> Delete Record\n</a>";
						actionItem.Weight = 1;
						newActionItemList.Add(actionItem);
					}
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_back_button";
						actionItem.Menu = "sidebar-top";
						actionItem.Template = "<a class=\"back clearfix\" href=\"javascript:void(0)\" ng-click=\"sidebarData.goBack()\"><i class=\"fa fa-fw fa-arrow-left\"></i> <span class=\"text\">Back</span></a>";
						actionItem.Weight = 1;
						newActionItemList.Add(actionItem);
					}
					createViewInput.ActionItems = newActionItemList;
					#endregion

					{
						var response = entMan.CreateRecordView(TASK_ENTITY_ID, createViewInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TASK_ENTITY_NAME + " Updated view: create" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << area add subscription: Project Workplace -> My tasks >>
				{
					var updatedAreaId = PROJECT_WORKPLACE_AREA_ID;
					var updateAreaResult = Helpers.UpsertEntityAsAreaSubscription(entMan, recMan, updatedAreaId, TASK_ENTITY_NAME, "general", "create", "my_tasks");
					if (!updateAreaResult.Success)
					{
						throw new Exception("System error 10060. Area update with id : " + updatedAreaId + " Message:" + updateAreaResult.Message);
					}
				}
				#endregion

				#region << area add subscription: Project Admin -> Bugs >>
				{
					var updatedAreaId = PROJECT_ADMIN_AREA_ID;
					var updateAreaResult = Helpers.UpsertEntityAsAreaSubscription(entMan, recMan, updatedAreaId, TASK_ENTITY_NAME, "general", "create", "admin");
					if (!updateAreaResult.Success)
					{
						throw new Exception("System error 10060. Area update with id : " + updatedAreaId + " Message:" + updateAreaResult.Message);
					}
				}
				#endregion
			}
			#endregion

			#region << wv_bug >>
			{
				#region << entity >>
				{
					InputEntity entity = new InputEntity();
					entity.Id = BUG_ENTITY_ID;
					entity.Name = BUG_ENTITY_NAME;
					entity.Label = "Bug";
					entity.LabelPlural = "Bugs";
					entity.System = true;
					entity.IconName = "bug";
					entity.Weight = 5;
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
					systemItemIdDictionary["id"] = new Guid("85e92948-ca90-4983-854d-c1fbad0d7e8c");
					systemItemIdDictionary["created_on"] = new Guid("781cee71-1632-4bf9-83b1-ff122d29eb2a");
					systemItemIdDictionary["created_by"] = new Guid("72b3a792-7ee8-420c-a48e-86c5beda474b");
					systemItemIdDictionary["last_modified_on"] = new Guid("ce3c4668-be18-4217-898e-704d126dc8a2");
					systemItemIdDictionary["last_modified_by"] = new Guid("18fdd103-06ee-449b-9140-098ea171670a");
					systemItemIdDictionary["user_wv_bug_created_by"] = new Guid("cdc5c5f9-30dc-4e3b-ac0f-4137e54c6c7f");
					systemItemIdDictionary["user_wv_bug_modified_by"] = new Guid("a09464e2-0383-435a-a1e7-52c6d2a2743c");
					{
						var response = entMan.CreateEntity(entity, false, false, systemItemIdDictionary);
						if (!response.Success)
							throw new Exception("System error 10050. Entity: " + BUG_ENTITY_NAME + " Field: entity creation" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << number >>
				{
					InputAutoNumberField autonumberField = new InputAutoNumberField();
					autonumberField.Id = new Guid("01a14364-7e42-42ed-b6aa-92525b7b36fb");
					autonumberField.Name = "number";
					autonumberField.Label = "Number";
					autonumberField.PlaceholderText = "";
					autonumberField.Description = "";
					autonumberField.HelpText = "";
					autonumberField.Required = true;
					autonumberField.Unique = true;
					autonumberField.Searchable = true;
					autonumberField.Auditable = false;
					autonumberField.System = true;
					autonumberField.DefaultValue = 0;
					autonumberField.DisplayFormat = "{0}";
					autonumberField.StartingNumber = 1;
					autonumberField.EnableSecurity = true;
					autonumberField.Permissions = new FieldPermissions();
					autonumberField.Permissions.CanRead = new List<Guid>();
					autonumberField.Permissions.CanUpdate = new List<Guid>();
					//READ
					autonumberField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					autonumberField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(BUG_ENTITY_ID, autonumberField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + BUG_ENTITY_NAME + " Field: number" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << code >>
				{
					InputTextField textboxField = new InputTextField();
					textboxField.Id = new Guid("6f2030e9-edd7-42ac-bb2a-2766b76c3da1");
					textboxField.Name = "code";
					textboxField.Label = "Code";
					textboxField.PlaceholderText = "";
					textboxField.Description = "";
					textboxField.HelpText = "";
					textboxField.Required = false;
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
					textboxField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(BUG_ENTITY_ID, textboxField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + BUG_ENTITY_NAME + " Field: code" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << subject >>
				{
					InputTextField textboxField = new InputTextField();
					textboxField.Id = new Guid("335a4500-130e-4739-b774-2f53f33ea22a");
					textboxField.Name = "subject";
					textboxField.Label = "Subject";
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
					textboxField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(BUG_ENTITY_ID, textboxField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + BUG_ENTITY_NAME + " Field: subject" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << description >>
				{
					InputHtmlField htmlField = new InputHtmlField();
					htmlField.Id = new Guid("4afe9621-39ee-40b9-a3ef-cb9b98131a6a");
					htmlField.Name = "description";
					htmlField.Label = "Description";
					htmlField.PlaceholderText = "";
					htmlField.Description = "";
					htmlField.HelpText = "";
					htmlField.Required = false;
					htmlField.Unique = false;
					htmlField.Searchable = false;
					htmlField.Auditable = false;
					htmlField.System = true;
					htmlField.DefaultValue = string.Empty;
					htmlField.EnableSecurity = true;
					htmlField.Permissions = new FieldPermissions();
					htmlField.Permissions.CanRead = new List<Guid>();
					htmlField.Permissions.CanUpdate = new List<Guid>();

					htmlField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					htmlField.Permissions.CanRead.Add(SystemIds.RegularRoleId);

					htmlField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					htmlField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(BUG_ENTITY_ID, htmlField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + BUG_ENTITY_NAME + " Field: description" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << priority >>
				{
					InputSelectField dropdownField = new InputSelectField();
					dropdownField.Id = new Guid("e506dfc7-6d49-4d00-9f61-8befd56c1a6e");
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
									new SelectFieldOption(){ Key = "low", Value = "low" },
									new SelectFieldOption(){ Key = "medium", Value = "medium" },
									new SelectFieldOption(){ Key = "hight", Value = "hight" }
								};
					dropdownField.EnableSecurity = true;
					dropdownField.Permissions = new FieldPermissions();
					dropdownField.Permissions.CanRead = new List<Guid>();
					dropdownField.Permissions.CanUpdate = new List<Guid>();
					//READ
					dropdownField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					dropdownField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					dropdownField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					dropdownField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(BUG_ENTITY_ID, dropdownField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + BUG_ENTITY_NAME + " Field: priority" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << owner_id >>
				{
					InputGuidField guidField = new InputGuidField();
					guidField.Id = new Guid("0ed595db-1abd-4a74-b160-db879c33cba9");
					guidField.Name = "owner_id";
					guidField.Label = "Owned by";
					guidField.PlaceholderText = "";
					guidField.Description = "";
					guidField.HelpText = "";
					guidField.Required = true;
					guidField.Unique = false;
					guidField.Searchable = false;
					guidField.Auditable = false;
					guidField.System = true;
					guidField.DefaultValue = Guid.Empty;
					guidField.GenerateNewId = false;
					guidField.EnableSecurity = true;
					guidField.Permissions = new FieldPermissions();
					guidField.Permissions.CanRead = new List<Guid>();
					guidField.Permissions.CanUpdate = new List<Guid>();
					//READ
					guidField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					guidField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(BUG_ENTITY_ID, guidField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + BUG_ENTITY_NAME + " Field: owner_id" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << project_id >>
				{
					InputGuidField guidField = new InputGuidField();
					guidField.Id = new Guid("dd345ce0-476f-4e74-a0b7-5f5c8ee480d6");
					guidField.Name = "project_id";
					guidField.Label = "Project";
					guidField.PlaceholderText = "";
					guidField.Description = "";
					guidField.HelpText = "";
					guidField.Required = true;
					guidField.Unique = false;
					guidField.Searchable = false;
					guidField.Auditable = false;
					guidField.System = true;
					guidField.DefaultValue = Guid.Empty;
					guidField.GenerateNewId = false;
					guidField.EnableSecurity = true;
					guidField.Permissions = new FieldPermissions();
					guidField.Permissions.CanRead = new List<Guid>();
					guidField.Permissions.CanUpdate = new List<Guid>();
					//READ
					guidField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					guidField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(BUG_ENTITY_ID, guidField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + BUG_ENTITY_NAME + " Field: project_id" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << milestone_id >>
				{
					InputGuidField guidField = new InputGuidField();
					guidField.Id = new Guid("bd817922-e269-44cf-bc84-64911b5abbdd");
					guidField.Name = "milestone_id";
					guidField.Label = "Release milestone";
					guidField.PlaceholderText = "";
					guidField.Description = "";
					guidField.HelpText = "";
					guidField.Required = false;
					guidField.Unique = false;
					guidField.Searchable = false;
					guidField.Auditable = false;
					guidField.System = true;
					guidField.DefaultValue = Guid.Empty;
					guidField.GenerateNewId = false;
					guidField.EnableSecurity = true;
					guidField.Permissions = new FieldPermissions();
					guidField.Permissions.CanRead = new List<Guid>();
					guidField.Permissions.CanUpdate = new List<Guid>();
					//READ
					guidField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					guidField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(BUG_ENTITY_ID, guidField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + BUG_ENTITY_NAME + " Field: milestone_id" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << status >>
				{
					InputSelectField dropdownField = new InputSelectField();
					dropdownField.Id = new Guid("48be1e6e-df47-46f0-b4e1-6e9e1cbaf71c");
					dropdownField.Name = "status";
					dropdownField.Label = "Status";
					dropdownField.PlaceholderText = "";
					dropdownField.Description = "";
					dropdownField.HelpText = "";
					dropdownField.Required = true;
					dropdownField.Unique = false;
					dropdownField.Searchable = true;
					dropdownField.Auditable = false;
					dropdownField.System = true;
					dropdownField.DefaultValue = "opened";
					dropdownField.Options = new List<SelectFieldOption>
								{
									new SelectFieldOption(){ Key = "opened", Value = "opened" },
									new SelectFieldOption(){ Key = "closed", Value = "closed" },
									new SelectFieldOption(){ Key = "reopened", Value = "reopened" }
								};
					dropdownField.EnableSecurity = true;
					dropdownField.Permissions = new FieldPermissions();
					dropdownField.Permissions.CanRead = new List<Guid>();
					dropdownField.Permissions.CanUpdate = new List<Guid>();
					//READ
					dropdownField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					dropdownField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					dropdownField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					dropdownField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(BUG_ENTITY_ID, dropdownField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + BUG_ENTITY_NAME + " Field: status" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << x_billable_hours >>
				{
					InputNumberField numberField = new InputNumberField();
					numberField.Id = new Guid("20fa3394-39f8-4ad0-8743-3f75afed8f8f");
					numberField.Name = "x_billable_hours";
					numberField.Label = "Billable hours";
					numberField.PlaceholderText = "";
					numberField.Description = "";
					numberField.HelpText = "";
					numberField.Required = false;
					numberField.Unique = false;
					numberField.Searchable = false;
					numberField.Auditable = false;
					numberField.System = true;
					numberField.DefaultValue = 0;
					numberField.MinValue = null;
					numberField.MaxValue = null;
					numberField.DecimalPlaces = 0;
					numberField.EnableSecurity = true;
					numberField.Permissions = new FieldPermissions();
					numberField.Permissions.CanRead = new List<Guid>();
					numberField.Permissions.CanUpdate = new List<Guid>();
					//READ
					numberField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					numberField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(BUG_ENTITY_ID, numberField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + BUG_ENTITY_NAME + " Field: x_billable_hours" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << x_nonbillable_hours >>
				{
					InputNumberField numberField = new InputNumberField();
					numberField.Id = new Guid("08c8cea9-79e1-4908-8611-abfa4c087209");
					numberField.Name = "x_nonbillable_hours";
					numberField.Label = "Non-Billable hours";
					numberField.PlaceholderText = "";
					numberField.Description = "";
					numberField.HelpText = "";
					numberField.Required = false;
					numberField.Unique = false;
					numberField.Searchable = false;
					numberField.Auditable = false;
					numberField.System = true;
					numberField.DefaultValue = 0;
					numberField.MinValue = null;
					numberField.MaxValue = null;
					numberField.DecimalPlaces = 0;
					numberField.EnableSecurity = true;
					numberField.Permissions = new FieldPermissions();
					numberField.Permissions.CanRead = new List<Guid>();
					numberField.Permissions.CanUpdate = new List<Guid>();
					//READ
					numberField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					numberField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(BUG_ENTITY_ID, numberField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + BUG_ENTITY_NAME + " Field: x_nonbillable_hours" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << user_1_n_bug_owner Relation >>
				{
					var originEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
					var targetEntity = entMan.ReadEntity(BUG_ENTITY_ID).Object;
					EntityRelation oneToNRelation = new EntityRelation();
					oneToNRelation.Id = new Guid("cddc10b6-30ff-4a86-96e4-645b3ea59fd9");
					oneToNRelation.Name = "user_1_n_bug_owner";
					oneToNRelation.Label = "user_1_n_bug_owner";
					oneToNRelation.System = true;
					oneToNRelation.RelationType = EntityRelationType.OneToMany;
					oneToNRelation.OriginEntityId = originEntity.Id;
					oneToNRelation.OriginFieldId = originEntity.Fields.Single(x => x.Name == "id").Id;
					oneToNRelation.TargetEntityId = targetEntity.Id;
					oneToNRelation.TargetFieldId = targetEntity.Fields.Single(x => x.Name == "owner_id").Id;
					{
						var result = relMan.Create(oneToNRelation);
						if (!result.Success)
							throw new Exception("CREATE user_1_n_bug_owner RELATION:" + result.Message);
					}
				}
				#endregion

				#region << user_n_n_bug_watchers Relation >>
				{
					var originEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
					var targetEntity = entMan.ReadEntity(BUG_ENTITY_ID).Object;
					EntityRelation NToNRelation = new EntityRelation();
					NToNRelation.Id = new Guid("b71d0c52-1626-48da-91bc-e10999ba79b8");
					NToNRelation.Name = "user_n_n_bug_watchers";
					NToNRelation.Label = "user_n_n_bug_watchers";
					NToNRelation.System = true;
					NToNRelation.RelationType = EntityRelationType.ManyToMany;
					NToNRelation.OriginEntityId = originEntity.Id;
					NToNRelation.OriginFieldId = originEntity.Fields.Single(x => x.Name == "id").Id;
					NToNRelation.TargetEntityId = targetEntity.Id;
					NToNRelation.TargetFieldId = targetEntity.Fields.Single(x => x.Name == "id").Id;
					{
						var result = relMan.Create(NToNRelation);
						if (!result.Success)
							throw new Exception("CREATE user_n_n_bug_watchers RELATION:" + result.Message);
					}
				}
				#endregion

				#region << task_n_n_bug Relation >>
				{
					var originEntity = entMan.ReadEntity(TASK_ENTITY_ID).Object;
					var targetEntity = entMan.ReadEntity(BUG_ENTITY_ID).Object;
					EntityRelation NToNRelation = new EntityRelation();
					NToNRelation.Id = new Guid("7103355b-bf03-40e1-8446-f6aeecfaa74c");
					NToNRelation.Name = "task_n_n_bug";
					NToNRelation.Label = "task_n_n_bug";
					NToNRelation.System = true;
					NToNRelation.RelationType = EntityRelationType.ManyToMany;
					NToNRelation.OriginEntityId = originEntity.Id;
					NToNRelation.OriginFieldId = originEntity.Fields.Single(x => x.Name == "id").Id;
					NToNRelation.TargetEntityId = targetEntity.Id;
					NToNRelation.TargetFieldId = targetEntity.Fields.Single(x => x.Name == "id").Id;
					{
						var result = relMan.Create(NToNRelation);
						if (!result.Success)
							throw new Exception("CREATE task_n_n_bug RELATION:" + result.Message);
					}
				}
				#endregion

				#region << project_1_n_bug Relation >>
				{
					var originEntity = entMan.ReadEntity(PROJECT_ENTITY_ID).Object;
					var targetEntity = entMan.ReadEntity(BUG_ENTITY_ID).Object;
					EntityRelation oneToNRelation = new EntityRelation();
					oneToNRelation.Id = new Guid("d94f100c-024c-47e7-af32-d67a49be2b6c");
					oneToNRelation.Name = "project_1_n_bug";
					oneToNRelation.Label = "project_1_n_bug";
					oneToNRelation.System = true;
					oneToNRelation.RelationType = EntityRelationType.OneToMany;
					oneToNRelation.OriginEntityId = originEntity.Id;
					oneToNRelation.OriginFieldId = originEntity.Fields.Single(x => x.Name == "id").Id;
					oneToNRelation.TargetEntityId = targetEntity.Id;
					oneToNRelation.TargetFieldId = targetEntity.Fields.Single(x => x.Name == "project_id").Id;
					{
						var result = relMan.Create(oneToNRelation);
						if (!result.Success)
							throw new Exception("CREATE project_1_n_bug RELATION:" + result.Message);
					}
				}
				#endregion

				#region << create project_bugs list >>
				{
					var createListEntity = entMan.ReadEntity(BUG_ENTITY_ID).Object;
					var createListInput = new InputRecordList();
					var listField = new InputRecordListFieldItem();
					var listFieldFromRelation = new InputRecordListRelationFieldItem();
					var listSort = new InputRecordListSort();
					var listQuery = new InputRecordListQuery();

					//General list details
					createListInput.Id = new Guid("3b2ebe34-1d02-448a-9616-5b62538fe2c7");
					createListInput.Name = "project_bugs";
					createListInput.Label = "Bugs";
					createListInput.Weight = 10;
					createListInput.Default = true;
					createListInput.VisibleColumnsCount = 5;
					createListInput.IconName = "bug";
					createListInput.ColumnWidthsCSV = "auto,30px,120px,120px,120px";
					createListInput.CssClass = "bug-list";
					createListInput.Type = "hidden";

					//Fields
					createListInput.Columns = new List<InputRecordListItemBase>();
					#region << subject >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "subject").Id;
						listField.FieldName = "subject";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << owner - image >>
					{
						var targetEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
						listFieldFromRelation = new InputRecordListRelationFieldItem();
						listFieldFromRelation.EntityId = targetEntity.Id;
						listFieldFromRelation.EntityName = targetEntity.Name;
						listFieldFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "image").Id;
						listFieldFromRelation.FieldName = "image";
						listFieldFromRelation.FieldLabel = "Owner";
						listFieldFromRelation.RelationId = new Guid("cddc10b6-30ff-4a86-96e4-645b3ea59fd9");
						listFieldFromRelation.RelationName = "user_1_n_bug_owner";
						listFieldFromRelation.Type = "fieldFromRelation";
						createListInput.Columns.Add(listFieldFromRelation);
					}
					#endregion

					#region << created_on >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "created_on").Id;
						listField.FieldName = "created_on";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << status >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "status").Id;
						listField.FieldName = "status";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << priority >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "priority").Id;
						listField.FieldName = "priority";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << query main>>
					{
						listQuery = new InputRecordListQuery();
						listQuery.FieldName = null;
						listQuery.FieldValue = null;
						listQuery.QueryType = "AND";
						listQuery.SubQueries = new List<InputRecordListQuery>();

						#region << subject >>
						{
							var subQuery = new InputRecordListQuery();
							subQuery.FieldName = "subject";
							subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""subject"", ""default"": null, ""settings"":{}}";
							subQuery.QueryType = "CONTAINS";
							subQuery.SubQueries = new List<InputRecordListQuery>();
							listQuery.SubQueries.Add(subQuery);
						}
						#endregion
						#region << subject >>
						{
							var subQuery = new InputRecordListQuery();
							subQuery.FieldName = "status";
							subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""status"", ""default"": null, ""settings"":{}}";
							subQuery.QueryType = "EQ";
							subQuery.SubQueries = new List<InputRecordListQuery>();
							listQuery.SubQueries.Add(subQuery);
						}
						#endregion
						#region << priority >>
						{
							var subQuery = new InputRecordListQuery();
							subQuery.FieldName = "priority";
							subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""priority"", ""default"": null, ""settings"":{}}";
							subQuery.QueryType = "EQ";
							subQuery.SubQueries = new List<InputRecordListQuery>();
							listQuery.SubQueries.Add(subQuery);
						}
						#endregion

						createListInput.Query = listQuery;
					}
					#endregion

					#region << Sort >>
					createListInput.Sorts = new List<InputRecordListSort>();
					{
						listSort = new InputRecordListSort();
						listSort.FieldName = @"{""name"":""url_sort"", ""option"": ""sortBy"", ""default"": ""created_on"", ""settings"":{""order"":""sortOrder""}}";
						listSort.SortType = "descending";
						createListInput.Sorts.Add(listSort);
					}
					#endregion

					var newActionItemList = new List<ActionItem>();
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_create_record";
						actionItem.Menu = "page-title";
						actionItem.Template = "<a class=\"btn btn-default btn-outline hidden-xs\" ng-show=\"::ngCtrl.userHasRecordPermissions('canCreate')\" \n    ng-href=\"{{::ngCtrl.getRecordCreateUrl()}}\">Add New</a>";
						actionItem.Weight = 1;
						newActionItemList.Add(actionItem);
					}
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_import_records";
						actionItem.Menu = "page-title-dropdown";
						actionItem.Template = "<a ng-click=\"ngCtrl.openImportModal()\" class=\"ng-hide\" ng-show=\"::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')\">\n\t<i class=\"fa fa-fw fa-upload\"></i> Import CSV\n</a>";
						actionItem.Weight = 10;
						newActionItemList.Add(actionItem);
					}
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_export_records";
						actionItem.Menu = "page-title-dropdown";
						actionItem.Template = "<a ng-click=\"ngCtrl.openExportModal()\" class=\"ng-hide\" ng-show=\"::ngCtrl.userHasRecordPermissions('canCreate,canUpdate')\">\n\t<i class=\"fa fa-fw fa-download\"></i> Export CSV\n</a>";
						actionItem.Weight = 11;
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
						var response = entMan.CreateRecordList(BUG_ENTITY_ID, createListInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + BUG_ENTITY_NAME + " Updated List: general" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << List name: my_bugs >>
				{
					var createListEntity = entMan.ReadEntity(BUG_ENTITY_ID).Object;
					var createListInput = new InputRecordList();
					var listField = new InputRecordListFieldItem();
					var listFieldFromRelation = new InputRecordListRelationFieldItem();
					var listSort = new InputRecordListSort();
					var listQuery = new InputRecordListQuery();

					#region << details >>
					createListInput.Id = new Guid("2ac91f01-0ee5-420e-8981-4f57eaea255e");
					createListInput.Type = "general";
					createListInput.Name = "my_bugs";
					createListInput.Label = "My Owned Open Bugs";
					createListInput.Weight = 1;
					createListInput.Default = false;
					createListInput.System = true;
					createListInput.CssClass = "bug-list";
					createListInput.IconName = "bug";
					createListInput.VisibleColumnsCount = 7;
					createListInput.ColumnWidthsCSV = "100px,auto,30px,160px,120px,120px,120px";
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

					#region << code >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "code").Id;
						listField.FieldName = "code";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << subject >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "subject").Id;
						listField.FieldName = "subject";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << owner - image >>
					{
						var targetEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
						var targetRelation = relMan.Read("user_1_n_bug_owner").Object;
						listFieldFromRelation = new InputRecordListRelationFieldItem();
						listFieldFromRelation.EntityId = targetEntity.Id;
						listFieldFromRelation.EntityName = targetEntity.Name;
						listFieldFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "image").Id;
						listFieldFromRelation.FieldName = "image";
						listFieldFromRelation.FieldLabel = "Owner";
						listFieldFromRelation.RelationId = targetRelation.Id;
						listFieldFromRelation.RelationName = targetRelation.Name;
						listFieldFromRelation.Type = "fieldFromRelation";
						createListInput.Columns.Add(listFieldFromRelation);
					}
					#endregion

					#region << created_on >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "created_on").Id;
						listField.FieldName = "created_on";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << status >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "status").Id;
						listField.FieldName = "status";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << priority >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "priority").Id;
						listField.FieldName = "priority";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#endregion

					#region << relation options >>
					createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
					#endregion

					#region << query >>
					createListInput.Query = new InputRecordListQuery();
					//Main query rule
					createListInput.Query.FieldName = null;
					createListInput.Query.FieldValue = null;
					createListInput.Query.QueryType = "AND";
					createListInput.Query.SubQueries = new List<InputRecordListQuery>();

					#region << Section 1 >>
					listQuery = new InputRecordListQuery();
					listQuery.FieldName = null;
					listQuery.FieldValue = null;
					listQuery.QueryType = "AND";
					listQuery.SubQueries = new List<InputRecordListQuery>();

					#region << owner_id >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "owner_id";
						subQuery.FieldValue = @"{""name"":""current_user"", ""option"": ""id"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "EQ";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion

					createListInput.Query.SubQueries.Add(listQuery);
					#endregion

					#region << Section 2 >>
					listQuery = new InputRecordListQuery();
					listQuery.FieldName = null;
					listQuery.FieldValue = null;
					listQuery.QueryType = "AND";
					listQuery.SubQueries = new List<InputRecordListQuery>();

					#region << code >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "code";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""code"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "CONTAINS";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion

					#region << subject >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "subject";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""subject"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "CONTAINS";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion

					#region << status >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "status";
						subQuery.FieldValue = "closed";
						subQuery.QueryType = "NOT";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion


					#region << priority >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "priority";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""priority"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "EQ";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion

					createListInput.Query.SubQueries.Add(listQuery);
					#endregion

					#endregion

					#region << Sort >>
					createListInput.Sorts = new List<InputRecordListSort>();
					listSort = new InputRecordListSort();
					listSort.FieldName = @"{""name"":""url_sort"", ""option"": ""sortBy"", ""default"": ""created_on"", ""settings"":{""order"":""sortOrder""}}";
					listSort.SortType = "ascending";
					createListInput.Sorts.Add(listSort);
					#endregion
					{
						var response = entMan.CreateRecordList(BUG_ENTITY_ID, createListInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + BUG_ENTITY_NAME + " Updated List: my_tickets" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << List name: created_bugs >>
				{
					var createListEntity = entMan.ReadEntity(BUG_ENTITY_ID).Object;
					var createListInput = new InputRecordList();
					var listField = new InputRecordListFieldItem();
					var listFieldFromRelation = new InputRecordListRelationFieldItem();
					var listSort = new InputRecordListSort();
					var listQuery = new InputRecordListQuery();

					#region << details >>
					createListInput.Id = new Guid("42356233-9a6b-4fcb-ae2b-42ca29d84fe8");
					createListInput.Type = "general";
					createListInput.Name = "created_bugs";
					createListInput.Label = "Bugs created by me";
					createListInput.Weight = 3;
					createListInput.Default = false;
					createListInput.System = true;
					createListInput.CssClass = "bug-list";
					createListInput.IconName = "bug";
					createListInput.VisibleColumnsCount = 7;
					createListInput.ColumnWidthsCSV = "100px,auto,30px,120px,120px,120px,120px";
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

					#region << code >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "code").Id;
						listField.FieldName = "code";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << subject >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "subject").Id;
						listField.FieldName = "subject";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << owner - image >>
					{
						var targetEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
						var targetRelation = relMan.Read("user_1_n_bug_owner").Object;
						listFieldFromRelation = new InputRecordListRelationFieldItem();
						listFieldFromRelation.EntityId = targetEntity.Id;
						listFieldFromRelation.EntityName = targetEntity.Name;
						listFieldFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "image").Id;
						listFieldFromRelation.FieldName = "image";
						listFieldFromRelation.FieldLabel = "Owner";
						listFieldFromRelation.RelationId = targetRelation.Id;
						listFieldFromRelation.RelationName = targetRelation.Name;
						listFieldFromRelation.Type = "fieldFromRelation";
						createListInput.Columns.Add(listFieldFromRelation);
					}
					#endregion

					#region << status >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "status").Id;
						listField.FieldName = "status";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << priority >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "priority").Id;
						listField.FieldName = "priority";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#endregion

					#region << relation options >>
					createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
					#endregion

					#region << query >>
					createListInput.Query = new InputRecordListQuery();
					//Main query rule
					createListInput.Query.FieldName = null;
					createListInput.Query.FieldValue = null;
					createListInput.Query.QueryType = "AND";
					createListInput.Query.SubQueries = new List<InputRecordListQuery>();

					#region << Section 1 >>
					listQuery = new InputRecordListQuery();
					listQuery.FieldName = null;
					listQuery.FieldValue = null;
					listQuery.QueryType = "AND";
					listQuery.SubQueries = new List<InputRecordListQuery>();

					#region << created_by >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "created_by";
						subQuery.FieldValue = @"{""name"":""current_user"", ""option"": ""id"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "EQ";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion

					createListInput.Query.SubQueries.Add(listQuery);
					#endregion

					#region << Section 2 >>
					listQuery = new InputRecordListQuery();
					listQuery.FieldName = null;
					listQuery.FieldValue = null;
					listQuery.QueryType = "AND";
					listQuery.SubQueries = new List<InputRecordListQuery>();

					#region << code >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "code";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""code"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "CONTAINS";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion

					#region << subject >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "subject";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""subject"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "CONTAINS";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion

					#region << status >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "status";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""status"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "EQ";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion


					#region << priority >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "priority";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""priority"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "EQ";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion

					createListInput.Query.SubQueries.Add(listQuery);
					#endregion

					#endregion

					#region << Sort >>
					createListInput.Sorts = new List<InputRecordListSort>();
					listSort = new InputRecordListSort();
					listSort.FieldName = @"{""name"":""url_sort"", ""option"": ""sortBy"", ""default"": ""created_on"", ""settings"":{""order"":""sortOrder""}}";
					listSort.SortType = "ascending";
					createListInput.Sorts.Add(listSort);
					#endregion
					{
						var response = entMan.CreateRecordList(BUG_ENTITY_ID, createListInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + BUG_ENTITY_NAME + " Updated List: bug_created" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << List name: owned_bugs >>
				{
					var createListEntity = entMan.ReadEntity(BUG_ENTITY_ID).Object;
					var createListInput = new InputRecordList();
					var listField = new InputRecordListFieldItem();
					var listFieldFromRelation = new InputRecordListRelationFieldItem();
					var listSort = new InputRecordListSort();
					var listQuery = new InputRecordListQuery();

					#region << details >>
					createListInput.Id = new Guid("c3f5b3f5-9d15-4b5a-a2cc-70e0d85efaf7");
					createListInput.Type = "general";
					createListInput.Name = "owned_bugs";
					createListInput.Label = "Bugs owned by me";
					createListInput.Weight = 2;
					createListInput.Default = false;
					createListInput.System = true;
					createListInput.CssClass = "bug-list";
					createListInput.IconName = "bug";
					createListInput.VisibleColumnsCount = 7;
					createListInput.ColumnWidthsCSV = "100px,auto,30px,120px,120px,120px,120px";
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

					#region << code >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "code").Id;
						listField.FieldName = "code";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << subject >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "subject").Id;
						listField.FieldName = "subject";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << owner - image >>
					{
						var targetEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
						var targetRelation = relMan.Read("user_1_n_bug_owner").Object;
						listFieldFromRelation = new InputRecordListRelationFieldItem();
						listFieldFromRelation.EntityId = targetEntity.Id;
						listFieldFromRelation.EntityName = targetEntity.Name;
						listFieldFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "image").Id;
						listFieldFromRelation.FieldName = "image";
						listFieldFromRelation.FieldLabel = "Owner";
						listFieldFromRelation.RelationId = targetRelation.Id;
						listFieldFromRelation.RelationName = targetRelation.Name;
						listFieldFromRelation.Type = "fieldFromRelation";
						createListInput.Columns.Add(listFieldFromRelation);
					}
					#endregion

					#region << status >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "status").Id;
						listField.FieldName = "status";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << priority >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "priority").Id;
						listField.FieldName = "priority";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#endregion

					#region << relation options >>
					createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
					#endregion

					#region << query >>
					createListInput.Query = new InputRecordListQuery();
					//Main query rule
					createListInput.Query.FieldName = null;
					createListInput.Query.FieldValue = null;
					createListInput.Query.QueryType = "AND";
					createListInput.Query.SubQueries = new List<InputRecordListQuery>();

					#region << Section 1 >>
					listQuery = new InputRecordListQuery();
					listQuery.FieldName = null;
					listQuery.FieldValue = null;
					listQuery.QueryType = "OR";
					listQuery.SubQueries = new List<InputRecordListQuery>();

					#region << owner_id >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "owner_id";
						subQuery.FieldValue = @"{""name"":""current_user"", ""option"": ""id"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "EQ";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion

					createListInput.Query.SubQueries.Add(listQuery);
					#endregion

					#region << Section 2 >>
					listQuery = new InputRecordListQuery();
					listQuery.FieldName = null;
					listQuery.FieldValue = null;
					listQuery.QueryType = "AND";
					listQuery.SubQueries = new List<InputRecordListQuery>();

					#region << code >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "code";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""code"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "CONTAINS";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion

					#region << subject >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "subject";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""subject"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "CONTAINS";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion

					#region << status >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "status";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""status"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "EQ";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion


					#region << priority >>
					{
						var subQuery = new InputRecordListQuery();
						subQuery.FieldName = "priority";
						subQuery.FieldValue = @"{""name"":""url_query"", ""option"": ""priority"", ""default"": null, ""settings"":{}}";
						subQuery.QueryType = "EQ";
						subQuery.SubQueries = new List<InputRecordListQuery>();
						listQuery.SubQueries.Add(subQuery);
					}
					#endregion

					createListInput.Query.SubQueries.Add(listQuery);
					#endregion

					#endregion

					#region << Sort >>
					createListInput.Sorts = new List<InputRecordListSort>();
					listSort = new InputRecordListSort();
					listSort.FieldName = @"{""name"":""url_sort"", ""option"": ""sortBy"", ""default"": ""created_on"", ""settings"":{""order"":""sortOrder""}}";
					listSort.SortType = "ascending";
					createListInput.Sorts.Add(listSort);
					#endregion
					{
						var response = entMan.CreateRecordList(BUG_ENTITY_ID, createListInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + BUG_ENTITY_NAME + " Updated List: owned_bug" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << List name: all_bugs >>
				{
					var createListEntity = entMan.ReadEntity(BUG_ENTITY_ID).Object;
					var createListInput = new InputRecordList();
					var listField = new InputRecordListFieldItem();
					var listFieldFromRelation = new InputRecordListRelationFieldItem();
					var listSort = new InputRecordListSort();
					var listQuery = new InputRecordListQuery();

					#region << details >>
					createListInput.Id = new Guid("97658e2a-69fb-48f5-b84c-236e1bd0103b");
					createListInput.Type = "general";
					createListInput.Name = "all_bugs";
					createListInput.Label = "All Bugs";
					createListInput.Weight = 12;
					createListInput.Default = false;
					createListInput.System = true;
					createListInput.CssClass = "bug-list";
					createListInput.IconName = "bug";
					createListInput.VisibleColumnsCount = 7;
					createListInput.ColumnWidthsCSV = "100px,auto,30px,120px,120px,120px,120px";
					createListInput.PageSize = 10;
					createListInput.DynamicHtmlTemplate = null;
					createListInput.DataSourceUrl = @"/plugins/webvella-projects/api/bug/list/all";
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

					#region << code >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "code").Id;
						listField.FieldName = "code";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << subject >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "subject").Id;
						listField.FieldName = "subject";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << owner - image >>
					{
						var targetEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
						var targetRelation = relMan.Read("user_1_n_bug_owner").Object;
						listFieldFromRelation = new InputRecordListRelationFieldItem();
						listFieldFromRelation.EntityId = targetEntity.Id;
						listFieldFromRelation.EntityName = targetEntity.Name;
						listFieldFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "image").Id;
						listFieldFromRelation.FieldName = "image";
						listFieldFromRelation.FieldLabel = "Owner";
						listFieldFromRelation.RelationId = targetRelation.Id;
						listFieldFromRelation.RelationName = targetRelation.Name;
						listFieldFromRelation.Type = "fieldFromRelation";
						createListInput.Columns.Add(listFieldFromRelation);
					}
					#endregion

					#region << status >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "status").Id;
						listField.FieldName = "status";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << priority >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "priority").Id;
						listField.FieldName = "priority";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#endregion


					#region << query >>
					createListInput.Query = null;
					#endregion

					#region << Sort >>
					createListInput.Sorts = new List<InputRecordListSort>();
					#endregion

					{
						var response = entMan.CreateRecordList(BUG_ENTITY_ID, createListInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + BUG_ENTITY_NAME + " Updated List: all_bugs" + " Message:" + response.Message);
					}

				}
				#endregion

				#region << List name: admin >>
				{
					var createListEntity = entMan.ReadEntity(BUG_ENTITY_ID).Object;
					var createListInput = new InputRecordList();
					var listField = new InputRecordListFieldItem();
					var listFieldFromRelation = new InputRecordListRelationFieldItem();
					var listSort = new InputRecordListSort();
					var listQuery = new InputRecordListQuery();

					#region << details >>
					createListInput.Id = new Guid("dd4eb5d6-0ada-4151-90af-675e2cc831e5");
					createListInput.Type = "hidden";
					createListInput.Name = "admin";
					createListInput.Label = "All Bugs";
					createListInput.Weight = 3;
					createListInput.Default = false;
					createListInput.System = true;
					createListInput.CssClass = "bug-list";
					createListInput.IconName = "bug";
					createListInput.VisibleColumnsCount = 7;
					createListInput.ColumnWidthsCSV = "auto,30px,120px,120px,120px,120px";
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

					#region << subject >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "subject").Id;
						listField.FieldName = "subject";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << owner - image >>
					{
						var targetEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
						listFieldFromRelation = new InputRecordListRelationFieldItem();
						listFieldFromRelation.EntityId = targetEntity.Id;
						listFieldFromRelation.EntityName = targetEntity.Name;
						listFieldFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "image").Id;
						listFieldFromRelation.FieldName = "image";
						listFieldFromRelation.FieldLabel = "Owner";
						listFieldFromRelation.RelationId = new Guid("cddc10b6-30ff-4a86-96e4-645b3ea59fd9");
						listFieldFromRelation.RelationName = "user_1_n_bug_owner";
						listFieldFromRelation.Type = "fieldFromRelation";
						createListInput.Columns.Add(listFieldFromRelation);
					}
					#endregion

					#region << created_on >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "created_on").Id;
						listField.FieldName = "created_on";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << status >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "status").Id;
						listField.FieldName = "status";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << priority >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "priority").Id;
						listField.FieldName = "priority";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#endregion

					#region << relation options >>
					createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
					#endregion

					#region << Sort >>
					createListInput.Sorts = new List<InputRecordListSort>();
					listSort = new InputRecordListSort();
					listSort.FieldName = @"{""name"":""url_sort"", ""option"": ""sortBy"", ""default"": ""created_on"", ""settings"":{""order"":""sortOrder""}}";
					listSort.SortType = "descending";
					createListInput.Sorts.Add(listSort);
					#endregion
					{
						var response = entMan.CreateRecordList(BUG_ENTITY_ID, createListInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + BUG_ENTITY_NAME + " Updated List: my_tickets" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << create create >>
				{
					var createViewEntity = entMan.ReadEntity(BUG_ENTITY_ID).Object;
					var createViewInput = new InputRecordView();
					var viewSection = new InputRecordViewSection();
					var viewRow = new InputRecordViewRow();
					var viewColumn = new InputRecordViewColumn();
					var viewItem = new InputRecordViewFieldItem();
					var viewItemFromRelation = new InputRecordViewRelationFieldItem();
					//General view fields

					#region << Details >>
					createViewInput.Id = new Guid("b369195d-39b2-4b69-abd3-5182477c7783");
					createViewInput.Name = "create";
					createViewInput.Label = "Create";
					createViewInput.Title = "";
					createViewInput.Default = true;
					createViewInput.System = true;
					createViewInput.Type = "create";
					createViewInput.Weight = 10;
					createViewInput.IconName = "file-text-o";
					createViewInput.ServiceCode = null;
					createViewInput.DynamicHtmlTemplate = null;
					createViewInput.DataSourceUrl = null;
					#endregion

					#region << Get the header Region >>
					var headerRegion = new InputRecordViewRegion();
					headerRegion.Name = "header";
					headerRegion.Label = "header";
					headerRegion.Sections = new List<InputRecordViewSection>();
					#endregion

					#region << Section >>
					viewSection = new InputRecordViewSection();
					viewSection.Id = new Guid("6781b136-b982-4934-a98c-5f736bd1a771");
					viewSection.Name = "details";
					viewSection.Label = "Details";
					viewSection.ShowLabel = false;
					viewSection.CssClass = "";
					viewSection.Collapsed = false;
					viewSection.TabOrder = "left-right";
					viewSection.Weight = 1;
					viewSection.Rows = new List<InputRecordViewRow>();

					#region << Row 1 >>
					viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("c0e3757b-6817-4eda-8cd1-e95b603af049");
					viewRow.Weight = 1;
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = 12;
					viewColumn.Items = new List<InputRecordViewItemBase>();


					#region << subject >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = createViewEntity.Id;
						viewItem.EntityName = createViewEntity.Name;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "subject").Id;
						viewItem.FieldName = "subject";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion

					#region << project name >>
					{
						var targetEntity = entMan.ReadEntity(PROJECT_ENTITY_ID).Object;
						var targetRelation = relMan.Read("project_1_n_bug").Object;
						viewItemFromRelation = new InputRecordViewRelationFieldItem();
						viewItemFromRelation.EntityId = targetEntity.Id;
						viewItemFromRelation.EntityName = targetEntity.Name;
						viewItemFromRelation.Type = "fieldFromRelation";
						viewItemFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "name").Id;
						viewItemFromRelation.FieldName = "name";
						viewItemFromRelation.FieldLabel = "Project";
						viewItemFromRelation.FieldPlaceholder = "";
						viewItemFromRelation.FieldRequired = true;
						viewItemFromRelation.FieldLookupList = "lookup";
						viewItemFromRelation.RelationId = targetRelation.Id;
						viewItemFromRelation.RelationName = targetRelation.Name;
						viewColumn.Items.Add(viewItemFromRelation);
					}
					#endregion


					#region << description >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = createViewEntity.Id;
						viewItem.EntityName = createViewEntity.Name;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "description").Id;
						viewItem.FieldName = "description";
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

					#region << Row 2 >>
					viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("61b3052c-339d-4ad2-8a5f-59f215f07358");
					viewRow.Weight = 2;
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = 6;
					viewColumn.Items = new List<InputRecordViewItemBase>();

					#region << status >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = createViewEntity.Id;
						viewItem.EntityName = createViewEntity.Name;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "status").Id;
						viewItem.FieldName = "status";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion

					//Save column
					viewRow.Columns.Add(viewColumn);
					#endregion

					#region << Column 2 >>
					viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = 6;
					viewColumn.Items = new List<InputRecordViewItemBase>();


					#region << priority >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = createViewEntity.Id;
						viewItem.EntityName = createViewEntity.Name;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "priority").Id;
						viewItem.FieldName = "priority";
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
					headerRegion.Sections.Add(viewSection);

					#endregion

					createViewInput.Regions = new List<InputRecordViewRegion>();
					createViewInput.Regions.Add(headerRegion);

					#region << Sidebar >>
					var sidebarItem = new InputRecordViewSidebarItemBase();
					createViewInput.Sidebar = new InputRecordViewSidebar();
					createViewInput.Sidebar.CssClass = "";
					createViewInput.Sidebar.Render = true;

					#endregion

					var newActionItemList = new List<ActionItem>();
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_back_button";
						actionItem.Menu = "sidebar-top";
						actionItem.Template = "<a class=\"back clearfix\" href=\"javascript:void(0)\" ng-click=\"sidebarData.goBack()\"><i class=\"fa fa-fw fa-arrow-left\"></i> <span class=\"text\">Back</span></a>";
						actionItem.Weight = 1;
						newActionItemList.Add(actionItem);
					}
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_create_and_list";
						actionItem.Menu = "create-bottom";
						actionItem.Template = "<a class=\"btn btn-primary\" ng-click='ngCtrl.create(\"default\")' ng-if=\"::ngCtrl.createViewRegion != null\">Create</a>";
						actionItem.Weight = 1;
						newActionItemList.Add(actionItem);
					}
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_create_and_details";
						actionItem.Menu = "create-bottom";
						actionItem.Template = "<a class=\"btn btn-default btn-outline\" ng-click='ngCtrl.create(\"details\")' ng-if=\"::ngCtrl.createViewRegion != null\">Create & Details</a>";
						actionItem.Weight = 2;
						newActionItemList.Add(actionItem);
					}
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_create_cancel";
						actionItem.Menu = "create-bottom";
						actionItem.Template = "<a class=\"btn btn-default btn-outline\" ng-click=\"ngCtrl.cancel()\">Cancel</a>";
						actionItem.Weight = 3;
						newActionItemList.Add(actionItem);
					}
					createViewInput.ActionItems = newActionItemList;

					{
						var response = entMan.CreateRecordView(BUG_ENTITY_ID, createViewInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + BUG_ENTITY_NAME + " Updated view: create" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << create general >>
				{
					var createViewEntity = entMan.ReadEntity(BUG_ENTITY_ID).Object;
					var createViewInput = new InputRecordView();
					var viewSection = new InputRecordViewSection();
					var viewRow = new InputRecordViewRow();
					var viewColumn = new InputRecordViewColumn();
					var viewItem = new InputRecordViewFieldItem();
					var viewItemFromRelation = new InputRecordViewRelationFieldItem();
					//General view fields

					#region << Details >>
					createViewInput.Id = new Guid("cc8ca474-29d3-4593-864a-166ad9e9a98e");
					createViewInput.Name = "general";
					createViewInput.Label = "General";
					createViewInput.Title = "";
					createViewInput.Default = true;
					createViewInput.System = true;
					createViewInput.Type = "general";
					createViewInput.Weight = 10;
					createViewInput.IconName = "file-text-o";
					createViewInput.ServiceCode = null;
					createViewInput.DynamicHtmlTemplate = null;
					createViewInput.DataSourceUrl = null;
					#endregion

					createViewInput.Regions = new List<InputRecordViewRegion>();

					#region << Sidebar >>
					var sidebarItem = new InputRecordViewSidebarItemBase();
					createViewInput.Sidebar = new InputRecordViewSidebar();
					createViewInput.Sidebar.CssClass = "";
					createViewInput.Sidebar.Render = true;

					#endregion

					var newActionItemList = new List<ActionItem>();
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_back_button";
						actionItem.Menu = "sidebar-top";
						actionItem.Template = "<a class=\"back clearfix\" href=\"javascript:void(0)\" ng-click=\"sidebarData.goBack()\"><i class=\"fa fa-fw fa-arrow-left\"></i> <span class=\"text\">Back</span></a>";
						actionItem.Weight = 1;
						newActionItemList.Add(actionItem);
					}
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_create_and_list";
						actionItem.Menu = "create-bottom";
						actionItem.Template = "<a class=\"btn btn-primary\" ng-click='ngCtrl.create(\"default\")' ng-if=\"::ngCtrl.createViewRegion != null\">Create</a>";
						actionItem.Weight = 1;
						newActionItemList.Add(actionItem);
					}
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_create_and_details";
						actionItem.Menu = "create-bottom";
						actionItem.Template = "<a class=\"btn btn-default btn-outline\" ng-click='ngCtrl.create(\"details\")' ng-if=\"::ngCtrl.createViewRegion != null\">Create & Details</a>";
						actionItem.Weight = 2;
						newActionItemList.Add(actionItem);
					}
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_create_cancel";
						actionItem.Menu = "create-bottom";
						actionItem.Template = "<a class=\"btn btn-default btn-outline\" ng-click=\"ngCtrl.cancel()\">Cancel</a>";
						actionItem.Weight = 3;
						newActionItemList.Add(actionItem);
					}
					createViewInput.ActionItems = newActionItemList;

					{
						var response = entMan.CreateRecordView(BUG_ENTITY_ID, createViewInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + BUG_ENTITY_NAME + " Updated view: create" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << area add subscription: Project Admin -> Bugs >>
				{
					var updatedAreaId = PROJECT_ADMIN_AREA_ID;
					var updateAreaResult = Helpers.UpsertEntityAsAreaSubscription(entMan, recMan, updatedAreaId, BUG_ENTITY_NAME, "general", "create", "admin");
					if (!updateAreaResult.Success)
					{
						throw new Exception("System error 10060. Area update with id : " + updatedAreaId + " Message:" + updateAreaResult.Message);
					}
				}
				#endregion

				#region << area add subscription: Project Workplace -> My bugs >>
				{
					var updatedAreaId = PROJECT_WORKPLACE_AREA_ID;
					var updateAreaResult = Helpers.UpsertEntityAsAreaSubscription(entMan, recMan, updatedAreaId, BUG_ENTITY_NAME, "general", "create", "my_bugs");
					if (!updateAreaResult.Success)
					{
						throw new Exception("System error 10060. Area update with id : " + updatedAreaId + " Message:" + updateAreaResult.Message);
					}
				}
				#endregion
			}
			#endregion

			#region << wv_timelog >>
			{
				#region << entity >>
				{
					InputEntity entity = new InputEntity();
					entity.Id = TIMELOG_ENTITY_ID;
					entity.Name = TIMELOG_ENTITY_NAME;
					entity.Label = "Time log";
					entity.LabelPlural = "Time logs";
					entity.System = true;
					entity.IconName = "clock-o";
					entity.Weight = 9;
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
					systemItemIdDictionary["id"] = new Guid("41a9a85b-894d-40d1-956a-dba950a253c7");
					systemItemIdDictionary["created_on"] = new Guid("ca1e843f-7f97-49db-bb7f-4b3a2b8f98ef");
					systemItemIdDictionary["created_by"] = new Guid("1cc75ad8-dd5b-4cdb-8896-32521e0a7a2e");
					systemItemIdDictionary["last_modified_on"] = new Guid("e91995d3-754e-418f-9a2d-aa2a319c568b");
					systemItemIdDictionary["last_modified_by"] = new Guid("19b6101a-737e-4dec-96fe-3994b3fb8b65");
					systemItemIdDictionary["user_wv_timelog_created_by"] = new Guid("393d1da8-0051-4807-8e89-1de933850888");
					systemItemIdDictionary["user_wv_timelog_modified_by"] = new Guid("2c728372-8dc4-4d18-8897-4716fce3d404");
					{
						var response = entMan.CreateEntity(entity, false, false, systemItemIdDictionary);
						if (!response.Success)
							throw new Exception("System error 10050. Entity: " + TIMELOG_ENTITY_NAME + " Field: entity creation" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << billable >> 
				{
					InputCheckboxField checkboxField = new InputCheckboxField();
					checkboxField.Id = new Guid("1f4b0729-4e31-4722-a8ce-3bf90c471dad");
					checkboxField.Name = "billable";
					checkboxField.Label = "Billable";
					checkboxField.PlaceholderText = "";
					checkboxField.Description = "";
					checkboxField.HelpText = "";
					checkboxField.Required = true;
					checkboxField.Unique = false;
					checkboxField.Searchable = false;
					checkboxField.Auditable = false;
					checkboxField.System = true;
					checkboxField.DefaultValue = true;
					checkboxField.EnableSecurity = true;
					checkboxField.Permissions = new FieldPermissions();
					checkboxField.Permissions.CanRead = new List<Guid>();
					checkboxField.Permissions.CanUpdate = new List<Guid>();
					//READ
					checkboxField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					checkboxField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					checkboxField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					checkboxField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(TIMELOG_ENTITY_ID, checkboxField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TIMELOG_ENTITY_NAME + " Field: field_name" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << hours >>
				{
					InputNumberField numberField = new InputNumberField();
					numberField.Id = new Guid("41caeb03-7430-4eb8-b830-c9df8bf2dc7f");
					numberField.Name = "hours";
					numberField.Label = "Hours";
					numberField.PlaceholderText = "";
					numberField.Description = "";
					numberField.HelpText = "";
					numberField.Required = true;
					numberField.Unique = false;
					numberField.Searchable = false;
					numberField.Auditable = false;
					numberField.System = true;
					numberField.DefaultValue = 0;
					numberField.MinValue = 0;
					numberField.MaxValue = null;
					numberField.DecimalPlaces = 2;
					numberField.EnableSecurity = true;
					numberField.Permissions = new FieldPermissions();
					numberField.Permissions.CanRead = new List<Guid>();
					numberField.Permissions.CanUpdate = new List<Guid>();
					//READ
					numberField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					numberField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					numberField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(TIMELOG_ENTITY_ID, numberField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TIMELOG_ENTITY_NAME + " Field: hours" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << description >>
				{
					InputMultiLineTextField textareaField = new InputMultiLineTextField();
					textareaField.Id = new Guid("1a1b646e-93df-4035-ace0-d844f62bad63");
					textareaField.Name = "description";
					textareaField.Label = "Description";
					textareaField.PlaceholderText = "";
					textareaField.Description = "";
					textareaField.HelpText = "";
					textareaField.Required = false;
					textareaField.Unique = false;
					textareaField.Searchable = false;
					textareaField.Auditable = false;
					textareaField.System = true;
					textareaField.DefaultValue = " ";
					textareaField.VisibleLineNumber = null;
					textareaField.EnableSecurity = true;
					textareaField.Permissions = new FieldPermissions();
					textareaField.Permissions.CanRead = new List<Guid>();
					textareaField.Permissions.CanUpdate = new List<Guid>();

					textareaField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					textareaField.Permissions.CanRead.Add(SystemIds.RegularRoleId);

					textareaField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					textareaField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);

					{
						var response = entMan.CreateField(TIMELOG_ENTITY_ID, textareaField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TIMELOG_ENTITY_NAME + " Field: description" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << log_date >>
				{
					InputDateField dateField = new InputDateField();
					dateField.Id = new Guid("29a32ad7-7b1c-4ea0-a06b-57b30be9b107");
					dateField.Name = "log_date";
					dateField.Label = "Log date";
					dateField.PlaceholderText = "";
					dateField.Description = "";
					dateField.HelpText = "";
					dateField.Required = true;
					dateField.Unique = false;
					dateField.Searchable = false;
					dateField.Auditable = false;
					dateField.System = true;
					dateField.DefaultValue = null;
					dateField.Format = "dd MMM yyyy";
					dateField.UseCurrentTimeAsDefaultValue = true;
					dateField.EnableSecurity = true;
					dateField.Permissions = new FieldPermissions();
					dateField.Permissions.CanRead = new List<Guid>();
					dateField.Permissions.CanUpdate = new List<Guid>();
					//READ
					dateField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					dateField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					dateField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					dateField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(TIMELOG_ENTITY_ID, dateField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TIMELOG_ENTITY_NAME + " Field: log_date" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << task_id >>
				{
					InputGuidField guidField = new InputGuidField();
					guidField.Id = new Guid("d256d5f7-4af7-4d62-b265-e4509319d700");
					guidField.Name = "task_id";
					guidField.Label = "Parent task";
					guidField.PlaceholderText = "";
					guidField.Description = "";
					guidField.HelpText = "";
					guidField.Required = false;
					guidField.Unique = false;
					guidField.Searchable = false;
					guidField.Auditable = false;
					guidField.System = true;
					guidField.DefaultValue = Guid.Empty;
					guidField.GenerateNewId = false;
					guidField.EnableSecurity = true;
					guidField.Permissions = new FieldPermissions();
					guidField.Permissions.CanRead = new List<Guid>();
					guidField.Permissions.CanUpdate = new List<Guid>();
					//READ
					guidField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					guidField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(TIMELOG_ENTITY_ID, guidField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TIMELOG_ENTITY_NAME + " Field: task_id" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << bug_id >>
				{
					InputGuidField guidField = new InputGuidField();
					guidField.Id = new Guid("f1b9a8f3-ab8d-4de0-8503-df3165ad7969");
					guidField.Name = "bug_id";
					guidField.Label = "Parent bug";
					guidField.PlaceholderText = "";
					guidField.Description = "";
					guidField.HelpText = "";
					guidField.Required = false;
					guidField.Unique = false;
					guidField.Searchable = false;
					guidField.Auditable = false;
					guidField.System = true;
					guidField.DefaultValue = Guid.Empty;
					guidField.GenerateNewId = false;
					guidField.EnableSecurity = true;
					guidField.Permissions = new FieldPermissions();
					guidField.Permissions.CanRead = new List<Guid>();
					guidField.Permissions.CanUpdate = new List<Guid>();
					//READ
					guidField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					guidField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(TIMELOG_ENTITY_ID, guidField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TIMELOG_ENTITY_NAME + " Field: bug_id" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << task_1_n_time_log Relation >>
				{
					var originEntity = entMan.ReadEntity(TASK_ENTITY_ID).Object;
					var targetEntity = entMan.ReadEntity(TIMELOG_ENTITY_ID).Object;
					EntityRelation oneToNRelation = new EntityRelation();
					oneToNRelation.Id = new Guid("61f1cd54-bcd6-4061-9c96-7934e01f0857");
					oneToNRelation.Name = "task_1_n_time_log";
					oneToNRelation.Label = "task_1_n_time_log";
					oneToNRelation.System = true;
					oneToNRelation.RelationType = EntityRelationType.OneToMany;
					oneToNRelation.OriginEntityId = originEntity.Id;
					oneToNRelation.OriginFieldId = originEntity.Fields.Single(x => x.Name == "id").Id;
					oneToNRelation.TargetEntityId = targetEntity.Id;
					oneToNRelation.TargetFieldId = targetEntity.Fields.Single(x => x.Name == "task_id").Id;
					{
						var result = relMan.Create(oneToNRelation);
						if (!result.Success)
							throw new Exception("CREATE task_1_n_time_log RELATION:" + result.Message);
					}
				}
				#endregion

				#region << bug_1_n_time_log Relation >>
				{
					var originEntity = entMan.ReadEntity(BUG_ENTITY_ID).Object;
					var targetEntity = entMan.ReadEntity(TIMELOG_ENTITY_ID).Object;
					EntityRelation oneToNRelation = new EntityRelation();
					oneToNRelation.Id = new Guid("97909e49-50d4-4534-aa7b-61c523b55d87");
					oneToNRelation.Name = "bug_1_n_time_log";
					oneToNRelation.Label = "bug_1_n_time_log";
					oneToNRelation.System = true;
					oneToNRelation.RelationType = EntityRelationType.OneToMany;
					oneToNRelation.OriginEntityId = originEntity.Id;
					oneToNRelation.OriginFieldId = originEntity.Fields.Single(x => x.Name == "id").Id;
					oneToNRelation.TargetEntityId = targetEntity.Id;
					oneToNRelation.TargetFieldId = targetEntity.Fields.Single(x => x.Name == "bug_id").Id;
					{
						var result = relMan.Create(oneToNRelation);
						if (!result.Success)
							throw new Exception("CREATE bug_1_n_time_log RELATION:" + result.Message);
					}
				}
				#endregion

				#region << List name: task_timelogs >>
				{
					var createListEntity = entMan.ReadEntity(TIMELOG_ENTITY_ID).Object;
					var createListInput = new InputRecordList();
					var listField = new InputRecordListFieldItem();
					var listFieldFromRelation = new InputRecordListRelationFieldItem();
					var listSort = new InputRecordListSort();
					var listQuery = new InputRecordListQuery();

					#region << details >>
					createListInput.Id = new Guid("c105b3f8-e140-4150-a587-a31cf600d99b");
					createListInput.Type = "hidden";
					createListInput.Name = "task_timelogs";
					createListInput.Label = "Time logs";
					createListInput.Weight = 25;
					createListInput.Default = false;
					createListInput.System = true;
					createListInput.CssClass = null;
					createListInput.IconName = "clock-o";
					createListInput.VisibleColumnsCount = 5;
					createListInput.ColumnWidthsCSV = "160px,160px,80px,auto";
					createListInput.PageSize = 10;
					createListInput.DynamicHtmlTemplate = null;
					createListInput.DataSourceUrl = null;
					createListInput.ServiceCode = null;
					#endregion

					#region << action items >>
					createListInput.ActionItems = new List<ActionItem>();

					#endregion

					#region << Columns >>
					createListInput.Columns = new List<InputRecordListItemBase>();

					#region << log_date >>
					{
						var fieldName = "log_date";
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == fieldName).Id;
						listField.FieldName = fieldName;
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << user_wv_timelog_created_by > username  >>
					{
						var relatedEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
						var relation = relMan.Read("user_wv_timelog_created_by").Object;
						listFieldFromRelation = new InputRecordListRelationFieldItem();
						listFieldFromRelation.EntityId = relatedEntity.Id;
						listFieldFromRelation.EntityName = relatedEntity.Name;
						listFieldFromRelation.FieldId = relatedEntity.Fields.Single(x => x.Name == "username").Id;
						listFieldFromRelation.FieldName = "username";
						listFieldFromRelation.FieldLabel = "username";
						listFieldFromRelation.Type = "field";
						listFieldFromRelation.RelationId = relation.Id;
						listFieldFromRelation.RelationName = relation.Name;
						createListInput.Columns.Add(listFieldFromRelation);
					}
					#endregion

					#region << hours >>
					{
						var fieldName = "hours";
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == fieldName).Id;
						listField.FieldName = fieldName;
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << description >>
					{
						var fieldName = "description";
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == fieldName).Id;
						listField.FieldName = fieldName;
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#endregion

					#region << relation options >>
					createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
					#endregion

					#region << action items >>
					createListInput.ActionItems = new List<ActionItem>();
					var actionItem = new ActionItem();
					{
						actionItem = new ActionItem();
						actionItem.Name = "wv_create_record";
						actionItem.Menu = "page-title";
						actionItem.Weight = 1;
						actionItem.Template = "" +
				@"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
											ng-href=""{{ngCtrl.getRecordCreateUrl(ngCtrl)}}"">Add New</a>";
						createListInput.ActionItems.Add(actionItem);
					}
					#endregion


					#region << Sort >>
					createListInput.Sorts = new List<InputRecordListSort>();
					listSort = new InputRecordListSort();
					listSort.FieldName = "log_date";
					listSort.SortType = "ascending";
					createListInput.Sorts.Add(listSort);
					#endregion
					{
						var response = entMan.CreateRecordList(TIMELOG_ENTITY_ID, createListInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TIMELOG_ENTITY_NAME + " Updated List: task_timelogs" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << List name: bug_timelogs >>
				{
					var createListEntity = entMan.ReadEntity(TIMELOG_ENTITY_ID).Object;
					var createListInput = new InputRecordList();
					var listField = new InputRecordListFieldItem();
					var listFieldFromRelation = new InputRecordListRelationFieldItem();
					var listSort = new InputRecordListSort();
					var listQuery = new InputRecordListQuery();

					#region << details >>
					createListInput.Id = new Guid("f9a12626-08db-4fd2-a443-b521162be2b5");
					createListInput.Type = "hidden";
					createListInput.Name = "bug_timelogs";
					createListInput.Label = "Time logs";
					createListInput.Weight = 25;
					createListInput.Default = false;
					createListInput.System = true;
					createListInput.CssClass = null;
					createListInput.IconName = "clock-o";
					createListInput.VisibleColumnsCount = 5;
					createListInput.ColumnWidthsCSV = "160px,160px,80px,auto";
					createListInput.PageSize = 10;
					createListInput.DynamicHtmlTemplate = null;
					createListInput.DataSourceUrl = null;
					createListInput.ServiceCode = null;
					#endregion

					#region << action items >>
					createListInput.ActionItems = new List<ActionItem>();

					#endregion

					#region << Columns >>
					createListInput.Columns = new List<InputRecordListItemBase>();

					#region << created_on >>
					{
						var fieldName = "log_date";
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == fieldName).Id;
						listField.FieldName = fieldName;
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << user_wv_timelog_created_by > username  >>
					{
						var relatedEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
						var relation = relMan.Read("user_wv_timelog_created_by").Object;
						listFieldFromRelation = new InputRecordListRelationFieldItem();
						listFieldFromRelation.EntityId = relatedEntity.Id;
						listFieldFromRelation.EntityName = relatedEntity.Name;
						listFieldFromRelation.FieldId = relatedEntity.Fields.Single(x => x.Name == "username").Id;
						listFieldFromRelation.FieldName = "username";
						listFieldFromRelation.FieldLabel = "username";
						listFieldFromRelation.Type = "field";
						listFieldFromRelation.RelationId = relation.Id;
						listFieldFromRelation.RelationName = relation.Name;
						createListInput.Columns.Add(listFieldFromRelation);
					}
					#endregion

					#region << hours >>
					{
						var fieldName = "hours";
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == fieldName).Id;
						listField.FieldName = fieldName;
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << description >>
					{
						var fieldName = "description";
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == fieldName).Id;
						listField.FieldName = fieldName;
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#endregion

					#region << relation options >>
					createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
					#endregion

					#region << action items >>
					createListInput.ActionItems = new List<ActionItem>();
					var actionItem = new ActionItem();
					{
						actionItem = new ActionItem();
						actionItem.Name = "wv_create_record";
						actionItem.Menu = "page-title";
						actionItem.Weight = 1;
						actionItem.Template = "" +
				@"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
											ng-href=""{{ngCtrl.getRecordCreateUrl(ngCtrl)}}"">Add New</a>";
						createListInput.ActionItems.Add(actionItem);
					}
					#endregion


					#region << Sort >>
					createListInput.Sorts = new List<InputRecordListSort>();
					listSort = new InputRecordListSort();
					listSort.FieldName = "log_date";
					listSort.SortType = "ascending";
					createListInput.Sorts.Add(listSort);
					#endregion
					{
						var response = entMan.CreateRecordList(TIMELOG_ENTITY_ID, createListInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TIMELOG_ENTITY_NAME + " Updated List: task_timelogs" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << create create >>
				{
					var createViewEntity = entMan.ReadEntity(TIMELOG_ENTITY_ID).Object;
					var createViewInput = new InputRecordView();
					var viewSection = new InputRecordViewSection();
					var viewRow = new InputRecordViewRow();
					var viewColumn = new InputRecordViewColumn();
					var viewItem = new InputRecordViewFieldItem();
					var viewItemFromRelation = new InputRecordViewRelationFieldItem();
					//General view fields

					#region << Details >>
					createViewInput.Id = new Guid("e33d960c-4867-48f2-bfc2-4f532aa2513d");
					createViewInput.Name = "create";
					createViewInput.Label = "Create";
					createViewInput.Title = "";
					createViewInput.Default = true;
					createViewInput.System = true;
					createViewInput.Type = "create";
					createViewInput.Weight = 10;
					createViewInput.IconName = "file-text-o";
					createViewInput.ServiceCode = null;
					createViewInput.DynamicHtmlTemplate = null;
					createViewInput.DataSourceUrl = null;
					#endregion

					#region << Get the header Region >>
					var headerRegion = new InputRecordViewRegion();
					headerRegion.Name = "header";
					headerRegion.Label = "header";
					headerRegion.Sections = new List<InputRecordViewSection>();
					#endregion

					#region << Section >>
					viewSection = new InputRecordViewSection();
					viewSection.Id = new Guid("559e8b4a-2ae5-4be7-8a87-805faeafeac4");
					viewSection.Name = "details";
					viewSection.Label = "Details";
					viewSection.ShowLabel = false;
					viewSection.CssClass = "";
					viewSection.Collapsed = false;
					viewSection.TabOrder = "left-right";
					viewSection.Weight = 1;
					viewSection.Rows = new List<InputRecordViewRow>();

					#region << Row 1 >>
					viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("6c769b09-045b-4826-a791-83812f6ded1b");
					viewRow.Weight = 1;
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = 6;
					viewColumn.Items = new List<InputRecordViewItemBase>();


					#region << hours >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = createViewEntity.Id;
						viewItem.EntityName = createViewEntity.Name;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "hours").Id;
						viewItem.FieldName = "hours";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion

					#region << hours >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = createViewEntity.Id;
						viewItem.EntityName = createViewEntity.Name;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "log_date").Id;
						viewItem.FieldName = "log_date";
						viewItem.Type = "field";
						viewColumn.Items.Add(viewItem);
					}
					#endregion
					//Save column
					viewRow.Columns.Add(viewColumn);
					#endregion

					#region << Column 2 >>
					viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = 6;
					viewColumn.Items = new List<InputRecordViewItemBase>();


					#region << subject >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = createViewEntity.Id;
						viewItem.EntityName = createViewEntity.Name;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "billable").Id;
						viewItem.FieldName = "billable";
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

					#region << Row 2 >>
					viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("57e71de9-8cb4-4343-bcec-5a0152f08dbd");
					viewRow.Weight = 2;
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = 12;
					viewColumn.Items = new List<InputRecordViewItemBase>();


					#region << description >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = createViewEntity.Id;
						viewItem.EntityName = createViewEntity.Name;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "description").Id;
						viewItem.FieldName = "description";
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
					headerRegion.Sections.Add(viewSection);

					#endregion

					createViewInput.Regions = new List<InputRecordViewRegion>();
					createViewInput.Regions.Add(headerRegion);

					#region << Sidebar >>
					var sidebarItem = new InputRecordViewSidebarItemBase();
					createViewInput.Sidebar = new InputRecordViewSidebar();
					createViewInput.Sidebar.CssClass = "";
					createViewInput.Sidebar.Render = true;

					#endregion

					#region << Action items>
					var newActionItemList = new List<ActionItem>();
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_create_and_list";
						actionItem.Menu = "create-bottom";
						actionItem.Template = "<a class=\"btn btn-primary\" ng-click='ngCtrl.create(\"default\")' ng-if=\"::ngCtrl.createViewRegion != null\">Create</a>";
						actionItem.Weight = 1;
						newActionItemList.Add(actionItem);
					}
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_create_cancel";
						actionItem.Menu = "create-bottom";
						actionItem.Template = "<a class=\"btn btn-default btn-outline\" ng-click=\"ngCtrl.cancel()\">Cancel</a>";
						actionItem.Weight = 2;
						newActionItemList.Add(actionItem);
					}
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_back_button";
						actionItem.Menu = "sidebar-top";
						actionItem.Template = "<a class=\"back clearfix\" href=\"javascript:void(0)\" ng-click=\"sidebarData.goBack()\"><i class=\"fa fa-fw fa-arrow-left\"></i> <span class=\"text\">Back</span></a>";
						actionItem.Weight = 1;
						newActionItemList.Add(actionItem);
					}

					createViewInput.ActionItems = newActionItemList;
					#endregion

					{
						var response = entMan.CreateRecordView(TIMELOG_ENTITY_ID, createViewInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + TIMELOG_ENTITY_NAME + " Updated view: create" + " Message:" + response.Message);
					}
				}
				#endregion
			}
			#endregion

			#region << wv_project_attachment >>
			{
				#region << entity >>
				{
					InputEntity entity = new InputEntity();
					entity.Id = ATTACHMENT_ENTITY_ID;
					entity.Name = ATTACHMENT_ENTITY_NAME;
					entity.Label = "Attachment";
					entity.LabelPlural = "Attachments";
					entity.System = true;
					entity.IconName = "paperclip";
					entity.Weight = 10;
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
					//DELETE
					entity.RecordPermissions.CanDelete.Add(SystemIds.AdministratorRoleId);
					entity.RecordPermissions.CanDelete.Add(SystemIds.RegularRoleId);
					//System fields and relations Ids should be hardcoded for the compare/change code generation to work later on correctly
					var systemItemIdDictionary = new Dictionary<string, Guid>();
					systemItemIdDictionary["id"] = new Guid("a965c8b6-83fb-43a0-a156-e8b58e4e51a5");
					systemItemIdDictionary["created_on"] = new Guid("381de04c-fad1-46b6-aa11-59bf7822a9a5");
					systemItemIdDictionary["created_by"] = new Guid("c8216ce8-5661-4a47-a03f-9cce93e943e1");
					systemItemIdDictionary["last_modified_on"] = new Guid("7bb58634-82ce-4b29-9a99-9ec1e8ea03dc");
					systemItemIdDictionary["last_modified_by"] = new Guid("c754a57c-4807-4cc8-9c63-6bcef8d09a7f");
					systemItemIdDictionary["user_wv_project_attachment_created_by"] = new Guid("97fe4c22-b090-4d8d-b9df-39d3e04a5865");
					systemItemIdDictionary["user_wv_project_attachment_modified_by"] = new Guid("b3f1f810-dd2d-4e9a-85d1-8e44746fbbdd");
					{
						var response = entMan.CreateEntity(entity, false, false, systemItemIdDictionary);
						if (!response.Success)
							throw new Exception("System error 10050. Entity: " + ATTACHMENT_ENTITY_NAME + " Field: entity creation" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << file >>
				{
					InputFileField fileField = new InputFileField();
					fileField.Id = new Guid("6d639a8c-e220-4d9f-86f0-de6ba03030b8");
					fileField.Name = "file";
					fileField.Label = "File";
					fileField.PlaceholderText = "";
					fileField.Description = "";
					fileField.HelpText = "";
					fileField.Required = true;
					fileField.Unique = false;
					fileField.Searchable = false;
					fileField.Auditable = false;
					fileField.System = true;
					fileField.DefaultValue = string.Empty;
					fileField.EnableSecurity = true;
					fileField.Permissions = new FieldPermissions();
					fileField.Permissions.CanRead = new List<Guid>();
					fileField.Permissions.CanUpdate = new List<Guid>();
					//READ
					fileField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					fileField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					fileField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					fileField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(ATTACHMENT_ENTITY_ID, fileField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + ATTACHMENT_ENTITY_NAME + " Field: file" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << task_id >>
				{
					InputGuidField guidField = new InputGuidField();
					guidField.Id = new Guid("841f6741-a7a6-4f52-9b45-31c4bf2e96ae");
					guidField.Name = "task_id";
					guidField.Label = "Parent task";
					guidField.PlaceholderText = "";
					guidField.Description = "";
					guidField.HelpText = "";
					guidField.Required = false;
					guidField.Unique = false;
					guidField.Searchable = false;
					guidField.Auditable = false;
					guidField.System = true;
					guidField.DefaultValue = Guid.Empty;
					guidField.GenerateNewId = false;
					guidField.EnableSecurity = true;
					guidField.Permissions = new FieldPermissions();
					guidField.Permissions.CanRead = new List<Guid>();
					guidField.Permissions.CanUpdate = new List<Guid>();
					//READ
					guidField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					guidField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(ATTACHMENT_ENTITY_ID, guidField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + ATTACHMENT_ENTITY_NAME + " Field: task_id" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << bug_id >>
				{
					InputGuidField guidField = new InputGuidField();
					guidField.Id = new Guid("8a389111-cee3-4da0-986f-bcdc5f179924");
					guidField.Name = "bug_id";
					guidField.Label = "Parent bug";
					guidField.PlaceholderText = "";
					guidField.Description = "";
					guidField.HelpText = "";
					guidField.Required = false;
					guidField.Unique = false;
					guidField.Searchable = false;
					guidField.Auditable = false;
					guidField.System = true;
					guidField.DefaultValue = Guid.Empty;
					guidField.GenerateNewId = false;
					guidField.EnableSecurity = true;
					guidField.Permissions = new FieldPermissions();
					guidField.Permissions.CanRead = new List<Guid>();
					guidField.Permissions.CanUpdate = new List<Guid>();
					//READ
					guidField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					guidField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(ATTACHMENT_ENTITY_ID, guidField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + ATTACHMENT_ENTITY_NAME + " Field: bug_id" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << project_id >>
				{
					InputGuidField guidField = new InputGuidField();
					guidField.Id = new Guid("151b03a4-156e-4500-a6ff-4431f67e500f");
					guidField.Name = "project_id";
					guidField.Label = "Project";
					guidField.PlaceholderText = "";
					guidField.Description = "";
					guidField.HelpText = "";
					guidField.Required = false;
					guidField.Unique = false;
					guidField.Searchable = false;
					guidField.Auditable = false;
					guidField.System = true;
					guidField.DefaultValue = Guid.Empty;
					guidField.GenerateNewId = false;
					guidField.EnableSecurity = true;
					guidField.Permissions = new FieldPermissions();
					guidField.Permissions.CanRead = new List<Guid>();
					guidField.Permissions.CanUpdate = new List<Guid>();
					//READ
					guidField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					guidField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(ATTACHMENT_ENTITY_ID, guidField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + ATTACHMENT_ENTITY_NAME + " Field: project_id" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << task_1_n_attachment Relation >>
				{
					var originEntity = entMan.ReadEntity(TASK_ENTITY_ID).Object;
					var targetEntity = entMan.ReadEntity(ATTACHMENT_ENTITY_ID).Object;
					EntityRelation oneToNRelation = new EntityRelation();
					oneToNRelation.Id = new Guid("f79f76e2-06b1-463a-9675-63845814bf22");
					oneToNRelation.Name = "task_1_n_attachment";
					oneToNRelation.Label = "task_1_n_attachment";
					oneToNRelation.System = true;
					oneToNRelation.RelationType = EntityRelationType.OneToMany;
					oneToNRelation.OriginEntityId = originEntity.Id;
					oneToNRelation.OriginFieldId = originEntity.Fields.Single(x => x.Name == "id").Id;
					oneToNRelation.TargetEntityId = targetEntity.Id;
					oneToNRelation.TargetFieldId = targetEntity.Fields.Single(x => x.Name == "task_id").Id;
					{
						var result = relMan.Create(oneToNRelation);
						if (!result.Success)
							throw new Exception("CREATE task_1_n_attachment RELATION:" + result.Message);
					}
				}
				#endregion

				#region << bug_1_n_attachment Relation >>
				{
					var originEntity = entMan.ReadEntity(BUG_ENTITY_ID).Object;
					var targetEntity = entMan.ReadEntity(ATTACHMENT_ENTITY_ID).Object;
					EntityRelation oneToNRelation = new EntityRelation();
					oneToNRelation.Id = new Guid("a4f60f87-66a9-4541-a2ef-29e00f2b418b");
					oneToNRelation.Name = "bug_1_n_attachment";
					oneToNRelation.Label = "bug_1_n_attachment";
					oneToNRelation.System = true;
					oneToNRelation.RelationType = EntityRelationType.OneToMany;
					oneToNRelation.OriginEntityId = originEntity.Id;
					oneToNRelation.OriginFieldId = originEntity.Fields.Single(x => x.Name == "id").Id;
					oneToNRelation.TargetEntityId = targetEntity.Id;
					oneToNRelation.TargetFieldId = targetEntity.Fields.Single(x => x.Name == "bug_id").Id;
					{
						var result = relMan.Create(oneToNRelation);
						if (!result.Success)
							throw new Exception("CREATE bug_1_n_attachment RELATION:" + result.Message);
					}
				}
				#endregion

				#region << project_1_n_attachment Relation >>
				{
					var originEntity = entMan.ReadEntity(PROJECT_ENTITY_ID).Object;
					var targetEntity = entMan.ReadEntity(ATTACHMENT_ENTITY_ID).Object;
					EntityRelation oneToNRelation = new EntityRelation();
					oneToNRelation.Id = new Guid("d20a4341-76d5-4537-91a0-c264824ff76c");
					oneToNRelation.Name = "project_1_n_attachment";
					oneToNRelation.Label = "project_1_n_attachment";
					oneToNRelation.System = true;
					oneToNRelation.RelationType = EntityRelationType.OneToMany;
					oneToNRelation.OriginEntityId = originEntity.Id;
					oneToNRelation.OriginFieldId = originEntity.Fields.Single(x => x.Name == "id").Id;
					oneToNRelation.TargetEntityId = targetEntity.Id;
					oneToNRelation.TargetFieldId = targetEntity.Fields.Single(x => x.Name == "project_id").Id;
					{
						var result = relMan.Create(oneToNRelation);
						if (!result.Success)
							throw new Exception("CREATE project_1_n_attachment RELATION:" + result.Message);
					}
				}
				#endregion

				#region << general list -> bug_attachments >>
				{
					var createListEntity = entMan.ReadEntity(ATTACHMENT_ENTITY_ID).Object;
					var createListInput = new InputRecordList();
					var listField = new InputRecordListFieldItem();
					var listFieldFromRelation = new InputRecordListRelationFieldItem();
					var listSort = new InputRecordListSort();
					var listQuery = new InputRecordListQuery();

					//General list details
					createListInput.Id = new Guid("2b83e4e3-6878-4b5b-9391-6e59429c0b5e");
					createListInput.Name = "bug_attachments";
					createListInput.Label = "Attachments";
					createListInput.IconName = "paperclip";
					createListInput.Type = "general";
					createListInput.Weight = 10;
					createListInput.Default = false;
					createListInput.System = true;
					createListInput.CssClass = null;
					createListInput.IconName = "paperclip";
					createListInput.VisibleColumnsCount = 5;
					createListInput.ColumnWidthsCSV = "160px,160px,auto";
					createListInput.PageSize = 10;
					createListInput.DynamicHtmlTemplate = null;
					createListInput.DataSourceUrl = null;
					createListInput.ServiceCode = null;
					createListInput.Columns = new List<InputRecordListItemBase>();
					#region << created_on >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "created_on").Id;
						listField.FieldName = "created_on";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << created_by_username >>
					{
						var relatedEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
						var relation = relMan.Read("user_wv_project_attachment_created_by").Object;
						listFieldFromRelation = new InputRecordListRelationFieldItem();
						listFieldFromRelation.EntityId = relatedEntity.Id;
						listFieldFromRelation.EntityName = relatedEntity.Name;
						listFieldFromRelation.FieldId = relatedEntity.Fields.Single(x => x.Name == "username").Id;
						listFieldFromRelation.FieldName = "username";
						listFieldFromRelation.Type = "field";
						listFieldFromRelation.RelationId = relation.Id;
						listFieldFromRelation.RelationName = relation.Name;
						createListInput.Columns.Add(listFieldFromRelation);
					}
					#endregion

					#region << file >>
					{
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == "file").Id;
						listField.FieldName = "file";
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					//Query
					#region << query descr >>
					{
					}
					#endregion

					//Action items
					var newActionItemList = new List<ActionItem>();
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_create_record";
						actionItem.Menu = "page-title";
						actionItem.Template = "<a class=\"btn btn-default btn-outline hidden-xs\" ng-show=\"::ngCtrl.userHasRecordPermissions('canCreate')\" \n    ng-href=\"{{::ngCtrl.getRecordCreateUrl()}}\">Add New</a>";
						actionItem.Weight = 1;
						newActionItemList.Add(actionItem);
					}
					createListInput.ActionItems = newActionItemList;

					//Sort
					#region << Sort >>
					createListInput.Sorts = new List<InputRecordListSort>();
					{
						listSort = new InputRecordListSort();
						listSort.FieldName = "created_on";
						listSort.SortType = "ascending";
						createListInput.Sorts.Add(listSort);
					}
					#endregion
					{
						var response = entMan.CreateRecordList(ATTACHMENT_ENTITY_ID, createListInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + ATTACHMENT_ENTITY_NAME + " Updated List: general" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << List name: task_attachments >>
				{
					var createListEntity = entMan.ReadEntity(ATTACHMENT_ENTITY_ID).Object;
					var createListInput = new InputRecordList();
					var listField = new InputRecordListFieldItem();
					var listFieldFromRelation = new InputRecordListRelationFieldItem();
					var listSort = new InputRecordListSort();
					var listQuery = new InputRecordListQuery();

					#region << details >>
					createListInput.Id = new Guid("6fc374ac-ba6b-4009-ade4-988304071f29");
					createListInput.Type = "hidden";
					createListInput.Name = "task_attachments";
					createListInput.Label = "Attachments";
					createListInput.Weight = 10;
					createListInput.Default = false;
					createListInput.System = true;
					createListInput.CssClass = null;
					createListInput.IconName = "paperclip";
					createListInput.VisibleColumnsCount = 5;
					createListInput.ColumnWidthsCSV = "160px,160px,auto";
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
						actionItem.Name = "wv_create_record";
						actionItem.Menu = "page-title";
						actionItem.Weight = 1;
						actionItem.Template = "" +
				@"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
										ng-href=""{{ngCtrl.getRecordCreateUrl(ngCtrl)}}"">Add New</a>";
						createListInput.ActionItems.Add(actionItem);
					}

					#endregion

					#region << Columns >>
					createListInput.Columns = new List<InputRecordListItemBase>();

					#region << created_on >>
					{
						var fieldName = "created_on";
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == fieldName).Id;
						listField.FieldName = fieldName;
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					//Field from relation
					#region << user_wv_project_attachment_created_by > username  >>
					{
						var relatedEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
						var relation = relMan.Read("user_wv_project_attachment_created_by").Object;
						listFieldFromRelation = new InputRecordListRelationFieldItem();
						listFieldFromRelation.EntityId = relatedEntity.Id;
						listFieldFromRelation.EntityName = relatedEntity.Name;
						listFieldFromRelation.FieldId = relatedEntity.Fields.Single(x => x.Name == "username").Id;
						listFieldFromRelation.FieldName = "username";
						listFieldFromRelation.Type = "field";
						listFieldFromRelation.RelationId = relation.Id;
						listFieldFromRelation.RelationName = relation.Name;
						createListInput.Columns.Add(listFieldFromRelation);
					}
					#endregion

					#region << file >>
					{
						var fieldName = "file";
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == fieldName).Id;
						listField.FieldName = fieldName;
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#endregion

					#region << relation options >>
					createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
					#endregion

					#region << Sort >>
					createListInput.Sorts = new List<InputRecordListSort>();
					listSort = new InputRecordListSort();
					listSort.FieldName = "created_on";
					listSort.SortType = "ascending";
					createListInput.Sorts.Add(listSort);
					#endregion
					{
						var response = entMan.CreateRecordList(ATTACHMENT_ENTITY_ID, createListInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + ATTACHMENT_ENTITY_NAME + " Updated List: task_attachment" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << create create >>
				{
					var createViewEntity = entMan.ReadEntity(ATTACHMENT_ENTITY_ID).Object;
					var createViewInput = new InputRecordView();
					var viewSection = new InputRecordViewSection();
					var viewRow = new InputRecordViewRow();
					var viewColumn = new InputRecordViewColumn();
					var viewItem = new InputRecordViewFieldItem();
					var viewItemFromRelation = new InputRecordViewRelationFieldItem();
					//General view fields

					#region << Details >>
					createViewInput.Id = new Guid("4a049b10-ac21-4402-9984-3b0b8cf015be");
					createViewInput.Name = "create";
					createViewInput.Label = "Create";
					createViewInput.Title = "";
					createViewInput.Default = true;
					createViewInput.System = true;
					createViewInput.Type = "create";
					createViewInput.Weight = 10;
					createViewInput.IconName = "file-text-o";
					createViewInput.ServiceCode = null;
					createViewInput.DynamicHtmlTemplate = null;
					createViewInput.DataSourceUrl = null;
					#endregion

					#region << Get the header Region >>
					var headerRegion = new InputRecordViewRegion();
					headerRegion.Name = "header";
					headerRegion.Label = "header";
					headerRegion.Sections = new List<InputRecordViewSection>();
					#endregion

					#region << Section >>
					viewSection = new InputRecordViewSection();
					viewSection.Id = new Guid("8ea140f9-b6fc-4b61-bc50-38b4e92f9eae");
					viewSection.Name = "details";
					viewSection.Label = "Details";
					viewSection.ShowLabel = false;
					viewSection.CssClass = "";
					viewSection.Collapsed = false;
					viewSection.TabOrder = "left-right";
					viewSection.Weight = 1;
					viewSection.Rows = new List<InputRecordViewRow>();

					#region << Row 1 >>
					viewRow = new InputRecordViewRow();
					viewRow.Id = new Guid("84482250-9a74-43d2-ae20-c883ee82a865");
					viewRow.Weight = 1;
					viewRow.Columns = new List<InputRecordViewColumn>();

					#region << Column 1 >>
					viewColumn = new InputRecordViewColumn();
					viewColumn.GridColCount = 12;
					viewColumn.Items = new List<InputRecordViewItemBase>();


					#region << subject >>
					{
						viewItem = new InputRecordViewFieldItem();
						viewItem.EntityId = createViewEntity.Id;
						viewItem.EntityName = createViewEntity.Name;
						viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "file").Id;
						viewItem.FieldName = "file";
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
					headerRegion.Sections.Add(viewSection);

					#endregion

					createViewInput.Regions = new List<InputRecordViewRegion>();
					createViewInput.Regions.Add(headerRegion);

					#region << Sidebar >>
					var sidebarItem = new InputRecordViewSidebarItemBase();
					createViewInput.Sidebar = new InputRecordViewSidebar();
					createViewInput.Sidebar.CssClass = "";
					createViewInput.Sidebar.Render = true;

					#endregion

					var newActionItemList = new List<ActionItem>();
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_back_button";
						actionItem.Menu = "sidebar-top";
						actionItem.Template = "<a class=\"back clearfix\" href=\"javascript:void(0)\" ng-click=\"sidebarData.goBack()\"><i class=\"fa fa-fw fa-arrow-left\"></i> <span class=\"text\">Back</span></a>";
						actionItem.Weight = 1;
						newActionItemList.Add(actionItem);
					}
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_create_and_list";
						actionItem.Menu = "create-bottom";
						actionItem.Template = "<a class=\"btn btn-primary\" ng-click='ngCtrl.create(\"default\")' ng-if=\"::ngCtrl.createViewRegion != null\">Create</a>";
						actionItem.Weight = 1;
						newActionItemList.Add(actionItem);
					}
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_create_and_details";
						actionItem.Menu = "create-bottom";
						actionItem.Template = "<a class=\"btn btn-default btn-outline\" ng-click='ngCtrl.create(\"details\")' ng-if=\"::ngCtrl.createViewRegion != null\">Create & Details</a>";
						actionItem.Weight = 2;
						newActionItemList.Add(actionItem);
					}
					{
						var actionItem = new ActionItem();
						actionItem.Name = "wv_create_cancel";
						actionItem.Menu = "create-bottom";
						actionItem.Template = "<a class=\"btn btn-default btn-outline\" ng-click=\"ngCtrl.cancel()\">Cancel</a>";
						actionItem.Weight = 3;
						newActionItemList.Add(actionItem);
					}
					createViewInput.ActionItems = newActionItemList;

					{
						var response = entMan.CreateRecordView(ATTACHMENT_ENTITY_ID, createViewInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + ATTACHMENT_ENTITY_NAME + " Updated view: create" + " Message:" + response.Message);
					}
				}
				#endregion

			}
			#endregion

			#region << wv_project_activity >>
			{
				#region << entity >>
				{
					InputEntity entity = new InputEntity();
					entity.Id = ACTIVITY_ENTITY_ID;
					entity.Name = ACTIVITY_ENTITY_NAME;
					entity.Label = "Activity";
					entity.LabelPlural = "Activities";
					entity.System = true;
					entity.IconName = "comments-o";
					entity.Weight = 7;
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
					systemItemIdDictionary["id"] = new Guid("0aac0f03-4153-47ed-b3a5-95371f46bb1e");
					systemItemIdDictionary["created_on"] = new Guid("f4d890fd-c3ed-4ea6-9b91-4bd55fe688c7");
					systemItemIdDictionary["created_by"] = new Guid("1605ec95-ffec-4c66-ba6e-b1e457936306");
					systemItemIdDictionary["last_modified_on"] = new Guid("540dfdfb-02aa-47d8-8312-0a887ab092e3");
					systemItemIdDictionary["last_modified_by"] = new Guid("80e7f499-cddd-4498-946a-e3972ee98a15");
					systemItemIdDictionary["user_wv_project_activity_created_by"] = new Guid("2fd1e3a2-feea-4b2b-a609-d3a5d6694cbb");
					systemItemIdDictionary["user_wv_project_activity_modified_by"] = new Guid("bc6314f8-fdf1-4b24-b5f5-574b04f45366");
					{
						var response = entMan.CreateEntity(entity, false, false, systemItemIdDictionary);
						if (!response.Success)
							throw new Exception("System error 10050. Entity: " + ACTIVITY_ENTITY_NAME + " Field: entity creation" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << label >>
				{
					InputSelectField dropdownField = new InputSelectField();
					dropdownField.Id = new Guid("fe4ee5da-8c32-4ecd-8773-04752b413cb0");
					dropdownField.Name = "label";
					dropdownField.Label = "Label";
					dropdownField.PlaceholderText = "";
					dropdownField.Description = "";
					dropdownField.HelpText = "";
					dropdownField.Required = false;
					dropdownField.Unique = false;
					dropdownField.Searchable = false;
					dropdownField.Auditable = false;
					dropdownField.System = true;
					dropdownField.DefaultValue = "added";
					dropdownField.Options = new List<SelectFieldOption>
								{
									new SelectFieldOption(){ Key = "added", Value = "added" },
									new SelectFieldOption(){ Key = "created", Value = "created" },
									new SelectFieldOption(){ Key = "updated", Value = "updated" },
									new SelectFieldOption(){ Key = "deleted", Value = "deleted" },
									new SelectFieldOption(){ Key = "timelog", Value = "timelog" },
									new SelectFieldOption(){ Key = "commented", Value = "commented" }
								};
					dropdownField.EnableSecurity = true;
					dropdownField.Permissions = new FieldPermissions();
					dropdownField.Permissions.CanRead = new List<Guid>();
					dropdownField.Permissions.CanUpdate = new List<Guid>();
					//READ
					dropdownField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					dropdownField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					dropdownField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					dropdownField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(ACTIVITY_ENTITY_ID, dropdownField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + ACTIVITY_ENTITY_NAME + " Field: label" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << subject >>
				{
					InputTextField textboxField = new InputTextField();
					textboxField.Id = new Guid("8f8b4cb9-aaed-4183-b863-b14faa2496ea");
					textboxField.Name = "subject";
					textboxField.Label = "Subject";
					textboxField.PlaceholderText = "";
					textboxField.Description = "";
					textboxField.HelpText = "";
					textboxField.Required = true;
					textboxField.Unique = false;
					textboxField.Searchable = false;
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
					textboxField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(ACTIVITY_ENTITY_ID, textboxField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + ACTIVITY_ENTITY_NAME + " Field: subject" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << description >>
				{
					InputMultiLineTextField textareaField = new InputMultiLineTextField();
					textareaField.Id = new Guid("6f29022e-c323-42fa-a4cb-76518787ba07");
					textareaField.Name = "description";
					textareaField.Label = "Description";
					textareaField.PlaceholderText = "";
					textareaField.Description = "";
					textareaField.HelpText = "";
					textareaField.Required = false;
					textareaField.Unique = false;
					textareaField.Searchable = false;
					textareaField.Auditable = false;
					textareaField.System = true;
					textareaField.DefaultValue = string.Empty;
					textareaField.VisibleLineNumber = 4;
					textareaField.EnableSecurity = true;
					textareaField.Permissions = new FieldPermissions();
					textareaField.Permissions.CanRead = new List<Guid>();
					textareaField.Permissions.CanUpdate = new List<Guid>();

					textareaField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					textareaField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);

					{
						var response = entMan.CreateField(ACTIVITY_ENTITY_ID, textareaField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + ACTIVITY_ENTITY_NAME + " Field: description" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << project_id >>
				{
					InputGuidField guidField = new InputGuidField();
					guidField.Id = new Guid("c67b14eb-e23c-43d1-8fd2-869618390b16");
					guidField.Name = "project_id";
					guidField.Label = "Parent Project";
					guidField.PlaceholderText = "";
					guidField.Description = "";
					guidField.HelpText = "";
					guidField.Required = true;
					guidField.Unique = false;
					guidField.Searchable = false;
					guidField.Auditable = false;
					guidField.System = true;
					guidField.DefaultValue = Guid.Empty;
					guidField.GenerateNewId = false;
					guidField.EnableSecurity = true;
					guidField.Permissions = new FieldPermissions();
					guidField.Permissions.CanRead = new List<Guid>();
					guidField.Permissions.CanUpdate = new List<Guid>();
					//READ
					guidField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					guidField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(ACTIVITY_ENTITY_ID, guidField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + ACTIVITY_ENTITY_NAME + " Field: project_id" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << task_id >>
				{
					InputGuidField guidField = new InputGuidField();
					guidField.Id = new Guid("2f919a7c-0167-44bb-a6dc-5a942cc98442");
					guidField.Name = "task_id";
					guidField.Label = "Parent task";
					guidField.PlaceholderText = "";
					guidField.Description = "";
					guidField.HelpText = "";
					guidField.Required = false;
					guidField.Unique = false;
					guidField.Searchable = false;
					guidField.Auditable = false;
					guidField.System = true;
					guidField.DefaultValue = Guid.Empty;
					guidField.GenerateNewId = false;
					guidField.EnableSecurity = true;
					guidField.Permissions = new FieldPermissions();
					guidField.Permissions.CanRead = new List<Guid>();
					guidField.Permissions.CanUpdate = new List<Guid>();
					//READ
					guidField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					guidField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(ACTIVITY_ENTITY_ID, guidField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + ACTIVITY_ENTITY_NAME + " Field: task_id" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << bug_id >>
				{
					InputGuidField guidField = new InputGuidField();
					guidField.Id = new Guid("a490f567-a404-4300-be98-e8dde9d3a47e");
					guidField.Name = "bug_id";
					guidField.Label = "Parent bug";
					guidField.PlaceholderText = "";
					guidField.Description = "";
					guidField.HelpText = "";
					guidField.Required = false;
					guidField.Unique = false;
					guidField.Searchable = false;
					guidField.Auditable = false;
					guidField.System = true;
					guidField.DefaultValue = Guid.Empty;
					guidField.GenerateNewId = false;
					guidField.EnableSecurity = true;
					guidField.Permissions = new FieldPermissions();
					guidField.Permissions.CanRead = new List<Guid>();
					guidField.Permissions.CanUpdate = new List<Guid>();
					//READ
					guidField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					guidField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(ACTIVITY_ENTITY_ID, guidField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + ACTIVITY_ENTITY_NAME + " Field: bug_id" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << project_1_n_activity Relation >>
				{
					var originEntity = entMan.ReadEntity(PROJECT_ENTITY_ID).Object;
					var targetEntity = entMan.ReadEntity(ACTIVITY_ENTITY_ID).Object;
					EntityRelation oneToNRelation = new EntityRelation();
					oneToNRelation.Id = new Guid("f0894d81-1924-48a8-b8ee-213c90a5f524");
					oneToNRelation.Name = "project_1_n_activity";
					oneToNRelation.Label = "project_1_n_activity";
					oneToNRelation.System = true;
					oneToNRelation.RelationType = EntityRelationType.OneToMany;
					oneToNRelation.OriginEntityId = originEntity.Id;
					oneToNRelation.OriginFieldId = originEntity.Fields.Single(x => x.Name == "id").Id;
					oneToNRelation.TargetEntityId = targetEntity.Id;
					oneToNRelation.TargetFieldId = targetEntity.Fields.Single(x => x.Name == "project_id").Id;
					{
						var result = relMan.Create(oneToNRelation);
						if (!result.Success)
							throw new Exception("CREATE project_1_n_activity RELATION:" + result.Message);
					}
				}
				#endregion

				#region << task_1_n_activity Relation >>
				{
					var originEntity = entMan.ReadEntity(TASK_ENTITY_ID).Object;
					var targetEntity = entMan.ReadEntity(ACTIVITY_ENTITY_ID).Object;
					EntityRelation oneToNRelation = new EntityRelation();
					oneToNRelation.Id = new Guid("8f294277-fd60-496e-bff7-9391fffcda41");
					oneToNRelation.Name = "task_1_n_activity";
					oneToNRelation.Label = "task_1_n_activity";
					oneToNRelation.System = true;
					oneToNRelation.RelationType = EntityRelationType.OneToMany;
					oneToNRelation.OriginEntityId = originEntity.Id;
					oneToNRelation.OriginFieldId = originEntity.Fields.Single(x => x.Name == "id").Id;
					oneToNRelation.TargetEntityId = targetEntity.Id;
					oneToNRelation.TargetFieldId = targetEntity.Fields.Single(x => x.Name == "task_id").Id;
					{
						var result = relMan.Create(oneToNRelation);
						if (!result.Success)
							throw new Exception("CREATE task_1_n_activity RELATION:" + result.Message);
					}
				}
				#endregion

				#region << bug_1_n_activity Relation >>
				{
					var originEntity = entMan.ReadEntity(BUG_ENTITY_ID).Object;
					var targetEntity = entMan.ReadEntity(ACTIVITY_ENTITY_ID).Object;
					EntityRelation oneToNRelation = new EntityRelation();
					oneToNRelation.Id = new Guid("b96189f7-a880-4da4-b9a9-2274a9745d2d");
					oneToNRelation.Name = "bug_1_n_activity";
					oneToNRelation.Label = "bug_1_n_activity";
					oneToNRelation.System = true;
					oneToNRelation.RelationType = EntityRelationType.OneToMany;
					oneToNRelation.OriginEntityId = originEntity.Id;
					oneToNRelation.OriginFieldId = originEntity.Fields.Single(x => x.Name == "id").Id;
					oneToNRelation.TargetEntityId = targetEntity.Id;
					oneToNRelation.TargetFieldId = targetEntity.Fields.Single(x => x.Name == "bug_id").Id;
					{
						var result = relMan.Create(oneToNRelation);
						if (!result.Success)
							throw new Exception("CREATE bug_1_n_activity RELATION:" + result.Message);
					}
				}
				#endregion

				#region << List name: task_activities >>
				{
					var createListEntity = entMan.ReadEntity(ACTIVITY_ENTITY_ID).Object;
					var createListInput = new InputRecordList();
					var listField = new InputRecordListFieldItem();
					var listFieldFromRelation = new InputRecordListRelationFieldItem();
					var listSort = new InputRecordListSort();
					var listQuery = new InputRecordListQuery();

					#region << details >>
					createListInput.Id = new Guid("121ce540-7838-4459-8357-d0d0ad2b65a4");
					createListInput.Type = "hidden";
					createListInput.Name = "task_activities";
					createListInput.Label = "Activities";
					createListInput.Weight = 30;
					createListInput.Default = false;
					createListInput.System = true;
					createListInput.CssClass = null;
					createListInput.IconName = "history";
					createListInput.VisibleColumnsCount = 5;
					createListInput.ColumnWidthsCSV = null;
					createListInput.PageSize = 10;
					createListInput.DynamicHtmlTemplate = null;
					createListInput.DataSourceUrl = null;
					createListInput.ServiceCode = null;
					#endregion

					#region << action items >>
					createListInput.ActionItems = new List<ActionItem>();

					#endregion

					#region << Columns >>
					createListInput.Columns = new List<InputRecordListItemBase>();

					#region << created_on >>
					{
						var fieldName = "created_on";
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == fieldName).Id;
						listField.FieldName = fieldName;
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << user_wv_project_activity_created_by > username  >>
					{
						var relatedEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
						var relation = relMan.Read("user_wv_project_activity_created_by").Object;
						listFieldFromRelation = new InputRecordListRelationFieldItem();
						listFieldFromRelation.EntityId = relatedEntity.Id;
						listFieldFromRelation.EntityName = relatedEntity.Name;
						listFieldFromRelation.FieldId = relatedEntity.Fields.Single(x => x.Name == "username").Id;
						listFieldFromRelation.FieldName = "username";
						listFieldFromRelation.Type = "field";
						listFieldFromRelation.RelationId = relation.Id;
						listFieldFromRelation.RelationName = relation.Name;
						createListInput.Columns.Add(listFieldFromRelation);
					}
					#endregion

					#region << subject >>
					{
						var fieldName = "subject";
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == fieldName).Id;
						listField.FieldName = fieldName;
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << label >>
					{
						var fieldName = "label";
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == fieldName).Id;
						listField.FieldName = fieldName;
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << user_wv_project_activity_created_by > image  >>
					{
						var relatedEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
						var relation = relMan.Read("user_wv_project_activity_created_by").Object;
						listFieldFromRelation = new InputRecordListRelationFieldItem();
						listFieldFromRelation.EntityId = relatedEntity.Id;
						listFieldFromRelation.EntityName = relatedEntity.Name;
						listFieldFromRelation.FieldId = relatedEntity.Fields.Single(x => x.Name == "image").Id;
						listFieldFromRelation.FieldName = "image";
						listFieldFromRelation.Type = "field";
						listFieldFromRelation.RelationId = relation.Id;
						listFieldFromRelation.RelationName = relation.Name;
						createListInput.Columns.Add(listFieldFromRelation);
					}
					#endregion

					#region << created_by >>
					{
						var fieldName = "created_by";
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == fieldName).Id;
						listField.FieldName = fieldName;
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#endregion

					#region << relation options >>
					createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
					#endregion

					#region << Sort >>
					createListInput.Sorts = new List<InputRecordListSort>();
					listSort = new InputRecordListSort();
					listSort.FieldName = "created_on";
					listSort.SortType = "ascending";
					createListInput.Sorts.Add(listSort);
					#endregion
					{
						var response = entMan.CreateRecordList(ACTIVITY_ENTITY_ID, createListInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + ACTIVITY_ENTITY_NAME + " Updated List: task_activities" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << List name: bug_activities >>
				{
					var createListEntity = entMan.ReadEntity(ACTIVITY_ENTITY_ID).Object;
					var createListInput = new InputRecordList();
					var listField = new InputRecordListFieldItem();
					var listFieldFromRelation = new InputRecordListRelationFieldItem();
					var listSort = new InputRecordListSort();
					var listQuery = new InputRecordListQuery();

					#region << details >>
					createListInput.Id = new Guid("57c3062c-df6e-488a-a432-dd927b0dd013");
					createListInput.Type = "hidden";
					createListInput.Name = "bug_activities";
					createListInput.Label = "Activities";
					createListInput.Weight = 30;
					createListInput.Default = false;
					createListInput.System = true;
					createListInput.CssClass = null;
					createListInput.IconName = "history";
					createListInput.VisibleColumnsCount = 5;
					createListInput.ColumnWidthsCSV = null;
					createListInput.PageSize = 10;
					createListInput.DynamicHtmlTemplate = null;
					createListInput.DataSourceUrl = null;
					createListInput.ServiceCode = null;
					#endregion

					#region << action items >>
					createListInput.ActionItems = new List<ActionItem>();

					#endregion

					#region << Columns >>
					createListInput.Columns = new List<InputRecordListItemBase>();

					#region << created_on >>
					{
						var fieldName = "created_on";
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == fieldName).Id;
						listField.FieldName = fieldName;
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << user_wv_project_activity_created_by > username  >>
					{
						var relatedEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
						var relation = relMan.Read("user_wv_project_activity_created_by").Object;
						listFieldFromRelation = new InputRecordListRelationFieldItem();
						listFieldFromRelation.EntityId = relatedEntity.Id;
						listFieldFromRelation.EntityName = relatedEntity.Name;
						listFieldFromRelation.FieldId = relatedEntity.Fields.Single(x => x.Name == "username").Id;
						listFieldFromRelation.FieldName = "username";
						listFieldFromRelation.Type = "field";
						listFieldFromRelation.RelationId = relation.Id;
						listFieldFromRelation.RelationName = relation.Name;
						createListInput.Columns.Add(listFieldFromRelation);
					}
					#endregion

					#region << subject >>
					{
						var fieldName = "subject";
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == fieldName).Id;
						listField.FieldName = fieldName;
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << label >>
					{
						var fieldName = "label";
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == fieldName).Id;
						listField.FieldName = fieldName;
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << user_wv_project_activity_created_by > image  >>
					{
						var relatedEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
						var relation = relMan.Read("user_wv_project_activity_created_by").Object;
						listFieldFromRelation = new InputRecordListRelationFieldItem();
						listFieldFromRelation.EntityId = relatedEntity.Id;
						listFieldFromRelation.EntityName = relatedEntity.Name;
						listFieldFromRelation.FieldId = relatedEntity.Fields.Single(x => x.Name == "image").Id;
						listFieldFromRelation.FieldName = "image";
						listFieldFromRelation.Type = "field";
						listFieldFromRelation.RelationId = relation.Id;
						listFieldFromRelation.RelationName = relation.Name;
						createListInput.Columns.Add(listFieldFromRelation);
					}
					#endregion

					#region << created_by >>
					{
						var fieldName = "created_by";
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == fieldName).Id;
						listField.FieldName = fieldName;
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#endregion

					#region << relation options >>
					createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
					#endregion

					#region << Sort >>
					createListInput.Sorts = new List<InputRecordListSort>();
					listSort = new InputRecordListSort();
					listSort.FieldName = "created_on";
					listSort.SortType = "ascending";
					createListInput.Sorts.Add(listSort);
					#endregion
					{
						var response = entMan.CreateRecordList(ACTIVITY_ENTITY_ID, createListInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + ACTIVITY_ENTITY_NAME + " Updated List: task_activities" + " Message:" + response.Message);
					}
				}
				#endregion

			}
			#endregion

			#region << wv_project_comment >>
			{
				#region << entity >>
				{
					InputEntity entity = new InputEntity();
					entity.Id = COMMENT_ENTITY_ID;
					entity.Name = COMMENT_ENTITY_NAME;
					entity.Label = "Comment";
					entity.LabelPlural = "Comments";
					entity.System = true;
					entity.IconName = "comment-o";
					entity.Weight = 17;
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
					systemItemIdDictionary["id"] = new Guid("4b10e607-b681-4650-a189-aab7c590aa62");
					systemItemIdDictionary["created_on"] = new Guid("c205c60f-598a-4db7-bd41-a7fd2ae3abd0");
					systemItemIdDictionary["created_by"] = new Guid("46208807-7bc8-4f54-8618-45134189e763");
					systemItemIdDictionary["last_modified_on"] = new Guid("5be05bc8-f6ec-4e6d-82d4-4ea3c48cc60e");
					systemItemIdDictionary["last_modified_by"] = new Guid("32e2a310-6613-4571-9fec-e592e1c4f333");
					systemItemIdDictionary["user_wv_project_comment_created_by"] = new Guid("2f3635a3-298e-475e-90f4-7d512da6cf95");
					systemItemIdDictionary["user_wv_project_comment_modified_by"] = new Guid("98ae433b-8b2e-49c5-b10e-8615d92dfab5");
					{
						var response = entMan.CreateEntity(entity, false, false, systemItemIdDictionary);
						if (!response.Success)
							throw new Exception("System error 10050. Entity: " + COMMENT_ENTITY_NAME + " Field: entity creation" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << content >>
				{
					InputHtmlField htmlField = new InputHtmlField();
					htmlField.Id = new Guid("23afb07b-438f-4e31-9372-c850a5789cc6");
					htmlField.Name = "content";
					htmlField.Label = "Content";
					htmlField.PlaceholderText = "";
					htmlField.Description = "";
					htmlField.HelpText = "";
					htmlField.Required = true;
					htmlField.Unique = false;
					htmlField.Searchable = false;
					htmlField.Auditable = false;
					htmlField.System = true;
					htmlField.DefaultValue = string.Empty;
					htmlField.EnableSecurity = true;
					htmlField.Permissions = new FieldPermissions();
					htmlField.Permissions.CanRead = new List<Guid>();
					htmlField.Permissions.CanUpdate = new List<Guid>();

					htmlField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					htmlField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					htmlField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					htmlField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(COMMENT_ENTITY_ID, htmlField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + COMMENT_ENTITY_ID + " Field: field_name" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << task_id >>
				{
					InputGuidField guidField = new InputGuidField();
					guidField.Id = new Guid("40068814-738f-4725-8aef-4722bc37ee7d");
					guidField.Name = "task_id";
					guidField.Label = "Parent task";
					guidField.PlaceholderText = "";
					guidField.Description = "";
					guidField.HelpText = "";
					guidField.Required = false;
					guidField.Unique = false;
					guidField.Searchable = false;
					guidField.Auditable = false;
					guidField.System = true;
					guidField.DefaultValue = Guid.Empty;
					guidField.GenerateNewId = false;
					guidField.EnableSecurity = true;
					guidField.Permissions = new FieldPermissions();
					guidField.Permissions.CanRead = new List<Guid>();
					guidField.Permissions.CanUpdate = new List<Guid>();
					//READ
					guidField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					guidField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(COMMENT_ENTITY_ID, guidField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + COMMENT_ENTITY_NAME + " Field: task_id" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << bug_id >>
				{
					InputGuidField guidField = new InputGuidField();
					guidField.Id = new Guid("6ac276d0-bb0c-4f8f-b30d-ea373cb64b73");
					guidField.Name = "bug_id";
					guidField.Label = "Parent bug";
					guidField.PlaceholderText = "";
					guidField.Description = "";
					guidField.HelpText = "";
					guidField.Required = false;
					guidField.Unique = false;
					guidField.Searchable = false;
					guidField.Auditable = false;
					guidField.System = true;
					guidField.DefaultValue = Guid.Empty;
					guidField.GenerateNewId = false;
					guidField.EnableSecurity = true;
					guidField.Permissions = new FieldPermissions();
					guidField.Permissions.CanRead = new List<Guid>();
					guidField.Permissions.CanUpdate = new List<Guid>();
					//READ
					guidField.Permissions.CanRead.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanRead.Add(SystemIds.RegularRoleId);
					//UPDATE
					guidField.Permissions.CanUpdate.Add(SystemIds.AdministratorRoleId);
					guidField.Permissions.CanUpdate.Add(SystemIds.RegularRoleId);
					{
						var response = entMan.CreateField(COMMENT_ENTITY_ID, guidField, false);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + COMMENT_ENTITY_NAME + " Field: bug_id" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << task_1_n_comment Relation >>
				{
					var originEntity = entMan.ReadEntity(TASK_ENTITY_ID).Object;
					var targetEntity = entMan.ReadEntity(COMMENT_ENTITY_ID).Object;
					EntityRelation oneToNRelation = new EntityRelation();
					oneToNRelation.Id = new Guid("884b9480-dc1c-468a-98f0-2d5f10084622");
					oneToNRelation.Name = "task_1_n_comment";
					oneToNRelation.Label = "task_1_n_comment";
					oneToNRelation.System = true;
					oneToNRelation.RelationType = EntityRelationType.OneToMany;
					oneToNRelation.OriginEntityId = originEntity.Id;
					oneToNRelation.OriginFieldId = originEntity.Fields.Single(x => x.Name == "id").Id;
					oneToNRelation.TargetEntityId = targetEntity.Id;
					oneToNRelation.TargetFieldId = targetEntity.Fields.Single(x => x.Name == "task_id").Id;
					{
						var result = relMan.Create(oneToNRelation);
						if (!result.Success)
							throw new Exception("CREATE task_1_n_comment RELATION:" + result.Message);
					}
				}
				#endregion

				#region << bug_1_n_comment Relation >>
				{
					var originEntity = entMan.ReadEntity(BUG_ENTITY_ID).Object;
					var targetEntity = entMan.ReadEntity(COMMENT_ENTITY_ID).Object;
					EntityRelation oneToNRelation = new EntityRelation();
					oneToNRelation.Id = new Guid("5af026bd-d046-42ba-b6a0-e9090727348f");
					oneToNRelation.Name = "bug_1_n_comment";
					oneToNRelation.Label = "bug_1_n_comment";
					oneToNRelation.System = true;
					oneToNRelation.RelationType = EntityRelationType.OneToMany;
					oneToNRelation.OriginEntityId = originEntity.Id;
					oneToNRelation.OriginFieldId = originEntity.Fields.Single(x => x.Name == "id").Id;
					oneToNRelation.TargetEntityId = targetEntity.Id;
					oneToNRelation.TargetFieldId = targetEntity.Fields.Single(x => x.Name == "bug_id").Id;
					{
						var result = relMan.Create(oneToNRelation);
						if (!result.Success)
							throw new Exception("CREATE bug_1_n_comment RELATION:" + result.Message);
					}
				}
				#endregion

				#region << List name: task_comments >>
				{
					var createListEntity = entMan.ReadEntity(COMMENT_ENTITY_ID).Object;
					var createListInput = new InputRecordList();
					var listField = new InputRecordListFieldItem();
					var listFieldFromRelation = new InputRecordListRelationFieldItem();
					var listSort = new InputRecordListSort();
					var listQuery = new InputRecordListQuery();

					#region << details >>
					createListInput.Id = new Guid("b8a7a81d-9176-47e6-90c5-3cabc2a4ceff");
					createListInput.Type = "hidden";
					createListInput.Name = "task_comments";
					createListInput.Label = "Comments";
					createListInput.Weight = 10;
					createListInput.Default = false;
					createListInput.System = true;
					createListInput.CssClass = null;
					createListInput.IconName = "comments-o";
					createListInput.VisibleColumnsCount = 5;
					createListInput.ColumnWidthsCSV = null;
					createListInput.PageSize = 0;
					createListInput.DynamicHtmlTemplate = @"/plugins/webvella-projects/templates/task-comments.html";
					createListInput.DataSourceUrl = null;
					createListInput.ServiceCode = @"/plugins/webvella-projects/providers/task-comments.service.js";
					#endregion

					#region << action items >>
					createListInput.ActionItems = new List<ActionItem>();
					var actionItem = new ActionItem();
					{
						actionItem = new ActionItem();
						actionItem.Name = "wv_create_record";
						actionItem.Menu = "page-title";
						actionItem.Weight = 1;
						actionItem.Template = "" +
					@"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
											ng-click=""ngCtrl.actionService.manageComment(null,ngCtrl)"">Add Comment</a>";
						createListInput.ActionItems.Add(actionItem);
					}

					#endregion

					#region << Columns >>
					createListInput.Columns = new List<InputRecordListItemBase>();

					#region << created_on >>
					{
						var fieldName = "created_on";
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == fieldName).Id;
						listField.FieldName = fieldName;
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << user_wv_project_comment_created_by > username  >>
					{
						var relatedEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
						var relation = relMan.Read("user_wv_project_comment_created_by").Object;
						listFieldFromRelation = new InputRecordListRelationFieldItem();
						listFieldFromRelation.EntityId = relatedEntity.Id;
						listFieldFromRelation.EntityName = relatedEntity.Name;
						listFieldFromRelation.FieldId = relatedEntity.Fields.Single(x => x.Name == "username").Id;
						listFieldFromRelation.FieldName = "username";
						listFieldFromRelation.Type = "field";
						listFieldFromRelation.RelationId = relation.Id;
						listFieldFromRelation.RelationName = relation.Name;
						createListInput.Columns.Add(listFieldFromRelation);
					}
					#endregion

					#region << file >>
					{
						var fieldName = "content";
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == fieldName).Id;
						listField.FieldName = fieldName;
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << user_wv_project_comment_created_by > image  >>
					{
						var relatedEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
						var relation = relMan.Read("user_wv_project_comment_created_by").Object;
						listFieldFromRelation = new InputRecordListRelationFieldItem();
						listFieldFromRelation.EntityId = relatedEntity.Id;
						listFieldFromRelation.EntityName = relatedEntity.Name;
						listFieldFromRelation.FieldId = relatedEntity.Fields.Single(x => x.Name == "image").Id;
						listFieldFromRelation.FieldName = "image";
						listFieldFromRelation.Type = "field";
						listFieldFromRelation.RelationId = relation.Id;
						listFieldFromRelation.RelationName = relation.Name;
						createListInput.Columns.Add(listFieldFromRelation);
					}
					#endregion

					#region << created_by >>
					{
						var fieldName = "created_by";
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == fieldName).Id;
						listField.FieldName = fieldName;
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#endregion

					#region << relation options >>
					createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
					#endregion

					#region << Sort >>
					createListInput.Sorts = new List<InputRecordListSort>();
					listSort = new InputRecordListSort();
					listSort.FieldName = "created_on";
					listSort.SortType = "ascending";
					createListInput.Sorts.Add(listSort);
					#endregion
					{
						var response = entMan.CreateRecordList(COMMENT_ENTITY_ID, createListInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + COMMENT_ENTITY_NAME + " Updated List: task_comments" + " Message:" + response.Message);
					}
				}
				#endregion

				#region << List name: bug_comments >>
				{
					var createListEntity = entMan.ReadEntity(COMMENT_ENTITY_ID).Object;
					var createListInput = new InputRecordList();
					var listField = new InputRecordListFieldItem();
					var listFieldFromRelation = new InputRecordListRelationFieldItem();
					var listSort = new InputRecordListSort();
					var listQuery = new InputRecordListQuery();

					#region << details >>
					createListInput.Id = new Guid("b143b82f-b79f-47c1-87e7-ecba6f6f2a32");
					createListInput.Type = "hidden";
					createListInput.Name = "bug_comments";
					createListInput.Label = "Comments";
					createListInput.Weight = 10;
					createListInput.Default = false;
					createListInput.System = true;
					createListInput.CssClass = null;
					createListInput.IconName = "comments-o";
					createListInput.VisibleColumnsCount = 5;
					createListInput.ColumnWidthsCSV = null;
					createListInput.PageSize = 0;
					createListInput.DynamicHtmlTemplate = @"/plugins/webvella-projects/templates/bug-comments.html";
					createListInput.DataSourceUrl = null;
					createListInput.ServiceCode = @"/plugins/webvella-projects/providers/bug-comments.service.js";
					#endregion

					#region << action items >>
					createListInput.ActionItems = new List<ActionItem>();
					var actionItem = new ActionItem();
					{
						actionItem = new ActionItem();
						actionItem.Name = "wv_create_record";
						actionItem.Menu = "page-title";
						actionItem.Weight = 1;
						actionItem.Template = "" +
					@"<a class=""btn btn-default btn-outline hidden-xs"" ng-show=""::ngCtrl.userHasRecordPermissions('canCreate')"" 
											ng-click=""ngCtrl.actionService.manageComment(null,ngCtrl)"">Add Comment</a>";
						createListInput.ActionItems.Add(actionItem);
					}

					#endregion

					#region << Columns >>
					createListInput.Columns = new List<InputRecordListItemBase>();

					#region << created_on >>
					{
						var fieldName = "created_on";
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == fieldName).Id;
						listField.FieldName = fieldName;
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << user_wv_project_comment_created_by > username  >>
					{
						var relatedEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
						var relation = relMan.Read("user_wv_project_comment_created_by").Object;
						listFieldFromRelation = new InputRecordListRelationFieldItem();
						listFieldFromRelation.EntityId = relatedEntity.Id;
						listFieldFromRelation.EntityName = relatedEntity.Name;
						listFieldFromRelation.FieldId = relatedEntity.Fields.Single(x => x.Name == "username").Id;
						listFieldFromRelation.FieldName = "username";
						listFieldFromRelation.Type = "field";
						listFieldFromRelation.RelationId = relation.Id;
						listFieldFromRelation.RelationName = relation.Name;
						createListInput.Columns.Add(listFieldFromRelation);
					}
					#endregion

					#region << file >>
					{
						var fieldName = "content";
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == fieldName).Id;
						listField.FieldName = fieldName;
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#region << user_wv_project_comment_created_by > image  >>
					{
						var relatedEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
						var relation = relMan.Read("user_wv_project_comment_created_by").Object;
						listFieldFromRelation = new InputRecordListRelationFieldItem();
						listFieldFromRelation.EntityId = relatedEntity.Id;
						listFieldFromRelation.EntityName = relatedEntity.Name;
						listFieldFromRelation.FieldId = relatedEntity.Fields.Single(x => x.Name == "image").Id;
						listFieldFromRelation.FieldName = "image";
						listFieldFromRelation.Type = "field";
						listFieldFromRelation.RelationId = relation.Id;
						listFieldFromRelation.RelationName = relation.Name;
						createListInput.Columns.Add(listFieldFromRelation);
					}
					#endregion

					#region << created_by >>
					{
						var fieldName = "created_by";
						listField = new InputRecordListFieldItem();
						listField.EntityId = createListEntity.Id;
						listField.EntityName = createListEntity.Name;
						listField.FieldId = createListEntity.Fields.Single(x => x.Name == fieldName).Id;
						listField.FieldName = fieldName;
						listField.Type = "field";
						createListInput.Columns.Add(listField);
					}
					#endregion

					#endregion

					#region << relation options >>
					createListInput.RelationOptions = new List<EntityRelationOptionsItem>();
					#endregion

					#region << Sort >>
					createListInput.Sorts = new List<InputRecordListSort>();
					listSort = new InputRecordListSort();
					listSort.FieldName = "created_on";
					listSort.SortType = "ascending";
					createListInput.Sorts.Add(listSort);
					#endregion
					{
						var response = entMan.CreateRecordList(COMMENT_ENTITY_ID, createListInput);
						if (!response.Success)
							throw new Exception("System error 10060. Entity: " + COMMENT_ENTITY_NAME + " Updated List: task_comments" + " Message:" + response.Message);
					}
				}
				#endregion

			}
			#endregion

			#region << create project general view >>
			{
				var createViewEntity = entMan.ReadEntity(PROJECT_ENTITY_ID).Object;
				var createViewInput = new InputRecordView();
				var viewSection = new InputRecordViewSection();
				var viewRow = new InputRecordViewRow();
				var viewColumn = new InputRecordViewColumn();
				var viewItem = new InputRecordViewFieldItem();
				var viewItemFromRelation = new InputRecordViewRelationFieldItem();
				//General view fields

				#region << Details >>
				createViewInput.Id = new Guid("211f028b-4e8e-418f-9c0e-78109f0839fc");
				createViewInput.Type = "hidden";
				createViewInput.Label = "{name}";
				createViewInput.Title = "";
				createViewInput.Weight = 10;
				createViewInput.CssClass = "";
				createViewInput.IconName = "file-text-o";
				createViewInput.Name = "general";
				createViewInput.Default = true;
				createViewInput.System = true;
				#endregion

				#region << Get the header Region >>
				var headerRegion = new InputRecordViewRegion();
				headerRegion.Name = "header";
				headerRegion.Label = "header";
				headerRegion.Sections = new List<InputRecordViewSection>();
				#endregion

				#region << Section >>
				viewSection = new InputRecordViewSection();
				viewSection.Id = new Guid("7848deef-6176-45ec-a12c-bb760793e9ef");
				viewSection.Name = "details";
				viewSection.Label = "Details";
				viewSection.ShowLabel = false;
				viewSection.CssClass = "";
				viewSection.Collapsed = false;
				viewSection.TabOrder = "left-right";
				viewSection.Weight = 1;
				viewSection.Rows = new List<InputRecordViewRow>();

				#region << Row 1 Column>>
				viewRow = new InputRecordViewRow();
				viewRow.Id = new Guid("3a099857-dd26-4723-b0ee-f62ae47d7d93");
				viewRow.Weight = 1;
				viewRow.Columns = new List<InputRecordViewColumn>();

				#region << Column 1 >>
				viewColumn = new InputRecordViewColumn();
				viewColumn.GridColCount = 12;
				viewColumn.Items = new List<InputRecordViewItemBase>();

				#region << name >>
				{
					viewItem = new InputRecordViewFieldItem();
					viewItem.EntityId = PROJECT_ENTITY_ID;
					viewItem.EntityName = PROJECT_ENTITY_NAME;
					viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "name").Id;
					viewItem.FieldName = "name";
					viewItem.Type = "field";
					viewColumn.Items.Add(viewItem);
				}
				#endregion

				#region << description >>
				{
					viewItem = new InputRecordViewFieldItem();
					viewItem.EntityId = PROJECT_ENTITY_ID;
					viewItem.EntityName = PROJECT_ENTITY_NAME;
					viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "description").Id;
					viewItem.FieldName = "description";
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

				#region << Row 2 Columns>>
				viewRow = new InputRecordViewRow();
				viewRow.Id = new Guid("b57588f0-9ca2-40b8-b516-3dd6d9efa0b5");
				viewRow.Weight = 1;
				viewRow.Columns = new List<InputRecordViewColumn>();

				#region << Column Left >>
				viewColumn = new InputRecordViewColumn();
				viewColumn.GridColCount = 6;
				viewColumn.Items = new List<InputRecordViewItemBase>();

				#region << $user_1_n_project_owner > username >>
				{
					var targetEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
					viewItemFromRelation = new InputRecordViewRelationFieldItem();
					viewItemFromRelation.EntityId = targetEntity.Id;
					viewItemFromRelation.EntityName = targetEntity.Name;
					viewItemFromRelation.Type = "field";
					viewItemFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "username").Id;
					viewItemFromRelation.FieldName = "username";
					viewItemFromRelation.FieldLabel = "Owner / Project manager";
					viewItemFromRelation.FieldPlaceholder = "";
					viewItemFromRelation.FieldRequired = true;
					viewItemFromRelation.FieldLookupList = "lookup";
					viewItemFromRelation.RelationId = PROJECT_RELATION_USER_1_N_PROJECT_OWNER_ID;
					viewItemFromRelation.RelationName = "user_1_n_project_owner";
					viewColumn.Items.Add(viewItemFromRelation);
				}
				#endregion

				#region << $role_n_n_project_team > name >>
				{
					var targetEntity = entMan.ReadEntity(SystemIds.RoleEntityId).Object;
					viewItemFromRelation = new InputRecordViewRelationFieldItem();
					viewItemFromRelation.EntityId = targetEntity.Id;
					viewItemFromRelation.EntityName = targetEntity.Name;
					viewItemFromRelation.Type = "field";
					viewItemFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "name").Id;
					viewItemFromRelation.FieldName = "name";
					viewItemFromRelation.FieldLabel = "Project team roles";
					viewItemFromRelation.FieldPlaceholder = "";
					viewItemFromRelation.FieldRequired = true;
					viewItemFromRelation.FieldLookupList = "lookup";
					viewItemFromRelation.RelationId = PROJECT_RELATION_ROLE_N_N_PROJECT_TEAM_ID;
					viewItemFromRelation.RelationName = "role_n_n_project_team";
					viewColumn.Items.Add(viewItemFromRelation);
				}
				#endregion

				#region << start_date >>
				{
					viewItem = new InputRecordViewFieldItem();
					viewItem.EntityId = PROJECT_ENTITY_ID;
					viewItem.EntityName = PROJECT_ENTITY_NAME;
					viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "start_date").Id;
					viewItem.FieldName = "start_date";
					viewItem.Type = "field";
					viewColumn.Items.Add(viewItem);
				}
				#endregion

				//Save column
				viewRow.Columns.Add(viewColumn);
				#endregion

				#region << Column right >>
				viewColumn = new InputRecordViewColumn();
				viewColumn.GridColCount = 6;
				viewColumn.Items = new List<InputRecordViewItemBase>();

				#region << $customer_1_n_project > name >>
				{
					var targetEntity = entMan.ReadEntity(CUSTOMER_ENTITY_ID).Object;
					viewItemFromRelation = new InputRecordViewRelationFieldItem();
					viewItemFromRelation.EntityId = targetEntity.Id;
					viewItemFromRelation.EntityName = targetEntity.Name;
					viewItemFromRelation.Type = "field";
					viewItemFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "name").Id;
					viewItemFromRelation.FieldName = "name";
					viewItemFromRelation.FieldLabel = "Customer";
					viewItemFromRelation.FieldPlaceholder = "";
					viewItemFromRelation.FieldRequired = true;
					viewItemFromRelation.FieldLookupList = "lookup";
					viewItemFromRelation.RelationId = PROJECT_RELATION_CUSTOMER_1_N_PROJECT_ID;
					viewItemFromRelation.RelationName = "customer_1_n_project";
					viewColumn.Items.Add(viewItemFromRelation);
				}
				#endregion

				#region << $role_n_n_project_customer > name >>
				{
					var targetEntity = entMan.ReadEntity(SystemIds.RoleEntityId).Object;
					viewItemFromRelation = new InputRecordViewRelationFieldItem();
					viewItemFromRelation.EntityId = targetEntity.Id;
					viewItemFromRelation.EntityName = targetEntity.Name;
					viewItemFromRelation.Type = "field";
					viewItemFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "name").Id;
					viewItemFromRelation.FieldName = "name";
					viewItemFromRelation.FieldLabel = "Project customer roles";
					viewItemFromRelation.FieldPlaceholder = "";
					viewItemFromRelation.FieldRequired = true;
					viewItemFromRelation.FieldLookupList = "lookup";
					viewItemFromRelation.RelationId = PROJECT_RELATION_ROLE_N_N_PROJECT_CUSTOMER_ID;
					viewItemFromRelation.RelationName = "role_n_n_project_customer";
					viewColumn.Items.Add(viewItemFromRelation);
				}
				#endregion

				#region << end_date >>
				{
					viewItem = new InputRecordViewFieldItem();
					viewItem.EntityId = PROJECT_ENTITY_ID;
					viewItem.EntityName = PROJECT_ENTITY_NAME;
					viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "end_date").Id;
					viewItem.FieldName = "end_date";
					viewItem.Type = "field";
					viewColumn.Items.Add(viewItem);
				}
				#endregion

				#region << code >>
				{
					viewItem = new InputRecordViewFieldItem();
					viewItem.EntityId = PROJECT_ENTITY_ID;
					viewItem.EntityName = PROJECT_ENTITY_NAME;
					viewItem.FieldId = createViewEntity.Fields.Single(x => x.Name == "code").Id;
					viewItem.FieldName = "code";
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
				headerRegion.Sections.Add(viewSection);

				#endregion

				createViewInput.Regions = new List<InputRecordViewRegion>();
				createViewInput.Regions.Add(headerRegion);

				#region << Sidebar >>
				var sidebarListFromRelationItem = new InputRecordViewSidebarRelationListItem();
				createViewInput.Sidebar = new InputRecordViewSidebar();
				createViewInput.Sidebar.CssClass = "";
				createViewInput.Sidebar.Render = true;

				#endregion
				{
					var response = entMan.CreateRecordView(PROJECT_ENTITY_ID, createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: " + PROJECT_ENTITY_NAME + " Updated view: general" + " Message:" + response.Message);
				}
			}
			#endregion

			#region << create project create view >>
			{
				var createViewEntity = entMan.ReadEntity(PROJECT_ENTITY_ID).Object;
				var createViewInput = new InputRecordView();
				var viewSection = new InputRecordViewSection();
				var viewRow = new InputRecordViewRow();
				var viewColumn = new InputRecordViewColumn();
				var viewItem = new InputRecordViewFieldItem();
				var viewItemFromRelation = new InputRecordViewRelationFieldItem();
				//General view fields

				#region << Details >>
				createViewInput.Id = new Guid("43688004-69df-4c2f-9075-a3d4612f4b69");
				createViewInput.Type = "create";
				createViewInput.Label = "Create";
				createViewInput.Weight = 1;
				createViewInput.CssClass = "";
				createViewInput.IconName = "file-text-o";
				createViewInput.Title = "";
				createViewInput.Name = "create";
				createViewInput.Default = true;
				createViewInput.System = true;
				#endregion

				#region << Get the header Region >>
				var headerRegion = new InputRecordViewRegion();
				headerRegion.Name = "header";
				headerRegion.Label = "header";
				headerRegion.Sections = new List<InputRecordViewSection>();
				#endregion

				createViewInput.Regions = new List<InputRecordViewRegion>();
				createViewInput.Regions.Add(headerRegion);

				#region << Sidebar >>
				var sidebarListFromRelationItem = new InputRecordViewSidebarRelationListItem();
				createViewInput.Sidebar = new InputRecordViewSidebar();
				createViewInput.Sidebar.CssClass = "";
				createViewInput.Sidebar.Render = true;

				var newActionItemList = new List<ActionItem>();
				{
					var actionItem = new ActionItem();
					actionItem.Name = "wv_back_button";
					actionItem.Menu = "sidebar-top";
					actionItem.Template = "<a class=\"back clearfix\" href=\"javascript:void(0)\" ng-click=\"sidebarData.goBack()\"><i class=\"fa fa-fw fa-arrow-left\"></i> <span class=\"text\">Back</span></a>";
					actionItem.Weight = 1;
					newActionItemList.Add(actionItem);
				}
				{
					var actionItem = new ActionItem();
					actionItem.Name = "wv_create_and_list";
					actionItem.Menu = "create-bottom";
					actionItem.Template = "<a class=\"btn btn-primary\" ng-click='ngCtrl.create(\"default\")' ng-if=\"::ngCtrl.createViewRegion != null\">Create</a>";
					actionItem.Weight = 1;
					newActionItemList.Add(actionItem);
				}
				{
					var actionItem = new ActionItem();
					actionItem.Name = "wv_create_and_details";
					actionItem.Menu = "create-bottom";
					actionItem.Template = "<a class=\"btn btn-default btn-outline\" ng-click='ngCtrl.create(\"details\")' ng-if=\"::ngCtrl.createViewRegion != null\">Create & Details</a>";
					actionItem.Weight = 2;
					newActionItemList.Add(actionItem);
				}
				{
					var actionItem = new ActionItem();
					actionItem.Name = "wv_create_cancel";
					actionItem.Menu = "create-bottom";
					actionItem.Template = "<a class=\"btn btn-default btn-outline\" ng-click=\"ngCtrl.cancel()\">Cancel</a>";
					actionItem.Weight = 3;
					newActionItemList.Add(actionItem);
				}

				createViewInput.ActionItems = newActionItemList;
				#endregion
				{
					var response = entMan.CreateRecordView(PROJECT_ENTITY_ID, createViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: " + PROJECT_ENTITY_NAME + " Updated view: create" + " Message:" + response.Message);
				}
			}
			#endregion

			#region << update project dashboard view >>
			{
				var updateViewEntity = entMan.ReadEntity(PROJECT_ENTITY_ID).Object;
				var updateView = updateViewEntity.RecordViews.Single(x => x.Name == "dashboard");
				var updateViewInput = new InputRecordView();
				var viewSection = new InputRecordViewSection();
				var viewRow = new InputRecordViewRow();
				var viewColumn = new InputRecordViewColumn();
				var viewItem = new InputRecordViewFieldItem();
				var viewItemFromRelation = new InputRecordViewRelationFieldItem();

				//Convert recordList to recordListInput
				updateViewInput = updateView.MapTo<InputRecordView>();
				updateViewInput.DynamicHtmlTemplate = "/plugins/webvella-projects/templates/project-dashboard.html";

				#region << Get the header Region >>
				var headerRegion = new InputRecordViewRegion();
				foreach (var region in updateViewInput.Regions)
				{
					if (region.Name == "header")
					{
						headerRegion = region;
					}
				}
				headerRegion.Sections = new List<InputRecordViewSection>();
				#endregion

				#region << Sidebar >>
				var sidebarListFromRelationItem = new InputRecordViewSidebarRelationListItem();
				var sidebarViewItem = new InputRecordViewSidebarViewItem();
				updateViewInput.Sidebar.CssClass = "";
				updateViewInput.Sidebar.Render = true;
				if(updateViewInput.Sidebar.Items == null) {
					updateViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();
				}
				#region << Tasks >>
				{
					var targetEntity = entMan.ReadEntity(TASK_ENTITY_ID).Object;
					var targetGeneralList = targetEntity.RecordLists.Single(x => x.Name == "project_tasks");
					var targetRelation = relMan.Read(new Guid("1f860b8c-7fa1-40fa-874f-19c2b5309817")).Object;
					sidebarListFromRelationItem = new InputRecordViewSidebarRelationListItem();
					sidebarListFromRelationItem.EntityId = targetEntity.Id;
					sidebarListFromRelationItem.EntityName = targetEntity.Name;
					sidebarListFromRelationItem.FieldHelpText = "";
					sidebarListFromRelationItem.FieldLabel = "Tasks";
					sidebarListFromRelationItem.FieldLookupList = "lookup";
					sidebarListFromRelationItem.FieldManageView = "general";
					sidebarListFromRelationItem.FieldPlaceholder = "";
					sidebarListFromRelationItem.FieldRequired = false;
					sidebarListFromRelationItem.ListId = targetGeneralList.Id;
					sidebarListFromRelationItem.ListName = targetGeneralList.Name;
					sidebarListFromRelationItem.RelationId = targetRelation.Id;
					sidebarListFromRelationItem.RelationName = targetRelation.Name;
					sidebarListFromRelationItem.Type = "listFromRelation";
					updateViewInput.Sidebar.Items.Add(sidebarListFromRelationItem);
				}
				#endregion

				#region << Milestone list >>
				{
					var milestoneEntity = entMan.ReadEntity(MILESTONE_ENTITY_ID).Object;
					var milestoneGeneralList = milestoneEntity.RecordLists.Single(x => x.Name == "project_milestones");
					var projectMilestoneRelation = relMan.Read(new Guid("0c446f98-eec2-40c1-9d66-8a3c2a2498e9")).Object;
					sidebarListFromRelationItem = new InputRecordViewSidebarRelationListItem();
					sidebarListFromRelationItem.EntityId = MILESTONE_ENTITY_ID;
					sidebarListFromRelationItem.EntityName = MILESTONE_ENTITY_NAME;
					sidebarListFromRelationItem.FieldHelpText = "";
					sidebarListFromRelationItem.FieldLabel = "Milestones";
					sidebarListFromRelationItem.FieldLookupList = "lookup";
					sidebarListFromRelationItem.FieldManageView = "general";
					sidebarListFromRelationItem.FieldPlaceholder = "";
					sidebarListFromRelationItem.FieldRequired = false;
					sidebarListFromRelationItem.ListId = milestoneGeneralList.Id;
					sidebarListFromRelationItem.ListName = milestoneGeneralList.Name;
					sidebarListFromRelationItem.RelationId = projectMilestoneRelation.Id;
					sidebarListFromRelationItem.RelationName = projectMilestoneRelation.Name;
					sidebarListFromRelationItem.Type = "listFromRelation";
					updateViewInput.Sidebar.Items.Add(sidebarListFromRelationItem);
				}
				#endregion

				#region << Bugs >>
				{
					var targetEntity = entMan.ReadEntity(BUG_ENTITY_ID).Object;
					var targetGeneralList = targetEntity.RecordLists.Single(x => x.Name == "project_bugs");
					var targetRelation = relMan.Read(new Guid("d94f100c-024c-47e7-af32-d67a49be2b6c")).Object;
					sidebarListFromRelationItem = new InputRecordViewSidebarRelationListItem();
					sidebarListFromRelationItem.EntityId = targetEntity.Id;
					sidebarListFromRelationItem.EntityName = targetEntity.Name;
					sidebarListFromRelationItem.FieldHelpText = "";
					sidebarListFromRelationItem.FieldLabel = "Bugs";
					sidebarListFromRelationItem.FieldLookupList = "lookup";
					sidebarListFromRelationItem.FieldManageView = "general";
					sidebarListFromRelationItem.FieldPlaceholder = "";
					sidebarListFromRelationItem.FieldRequired = false;
					sidebarListFromRelationItem.ListId = targetGeneralList.Id;
					sidebarListFromRelationItem.ListName = targetGeneralList.Name;
					sidebarListFromRelationItem.RelationId = targetRelation.Id;
					sidebarListFromRelationItem.RelationName = targetRelation.Name;
					sidebarListFromRelationItem.Type = "listFromRelation";
					updateViewInput.Sidebar.Items.Add(sidebarListFromRelationItem);
				}
				#endregion

				#region << Project details >>
				{
					var targetView = updateViewEntity.RecordViews.Single(x => x.Name == "general");
					sidebarViewItem = new InputRecordViewSidebarViewItem();
					sidebarViewItem.EntityId = updateViewEntity.Id;
					sidebarViewItem.EntityName = updateViewEntity.Name;
					sidebarViewItem.Type = "view";
					sidebarViewItem.ViewId = targetView.Id;
					sidebarViewItem.ViewName = targetView.Name;
					updateViewInput.Sidebar.Items.Add(sidebarViewItem);
				}
				#endregion

				#endregion
				{
					var response = entMan.UpdateRecordView(PROJECT_ENTITY_ID, updateViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: " + PROJECT_ENTITY_NAME + " Updated view: dashboard" + " Message:" + response.Message);
				}
			}
			#endregion

			#region << area add subscription: Project Workplace -> Project >>
			{
				var updatedAreaId = PROJECT_WORKPLACE_AREA_ID;
				var updateAreaResult = Helpers.UpsertEntityAsAreaSubscription(entMan, recMan, updatedAreaId, PROJECT_ENTITY_NAME, "dashboard", "create", "my_projects");
				if (!updateAreaResult.Success)
				{
					throw new Exception("System error 10060. Area update with id : " + updatedAreaId + " Message:" + updateAreaResult.Message);
				}
			}
			#endregion

			#region << area add subscription: URL Dashboard -> Project >>
			{
				var updatedAreaId = PROJECT_WORKPLACE_AREA_ID;
				var updateAreaResult = Helpers.UpsertUrlAsAreaSubscription(entMan, recMan, updatedAreaId, "/#/areas/projects/wv_project/dashboard", "My Dashboard", 1, "tachometer");
				if (!updateAreaResult.Success)
				{
					throw new Exception("System error 10060. Area update with id : " + updatedAreaId + " Message:" + updateAreaResult.Message);
				}
			}
			#endregion

			#region << update task general view >>
			{
				var updateViewEntity = entMan.ReadEntity(TASK_ENTITY_ID).Object;
				var updateView = updateViewEntity.RecordViews.Single(x => x.Name == "general");
				var updateViewInput = new InputRecordView();
				var viewSection = new InputRecordViewSection();
				var viewRow = new InputRecordViewRow();
				var viewColumn = new InputRecordViewColumn();
				var viewItem = new InputRecordViewFieldItem();
				var viewItemFromRelation = new InputRecordViewRelationFieldItem();
				//General view fields

				//Convert recordList to recordListInput
				updateViewInput = updateView.MapTo<InputRecordView>();

				#region << Get the header Region >>
				var headerRegion = new InputRecordViewRegion();
				foreach (var region in updateViewInput.Regions)
				{
					if (region.Name == "header")
					{
						headerRegion = region;
					}
				}
				#endregion

				#region << Sidebar >>
				var sidebarListFromRelationItem = new InputRecordViewSidebarRelationListItem();
				var sidebarViewItem = new InputRecordViewSidebarViewItem();
				updateViewInput.Sidebar.CssClass = "";
				updateViewInput.Sidebar.Render = true;
				if(updateViewInput.Sidebar.Items == null) {
					updateViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();
				}
				#region << Attachments >>
				{
					var targetEntity = entMan.ReadEntity(ATTACHMENT_ENTITY_ID).Object;
					var targetGeneralList = targetEntity.RecordLists.Single(x => x.Name == "task_attachments");
					var targetRelation = relMan.Read(new Guid("f79f76e2-06b1-463a-9675-63845814bf22")).Object;
					sidebarListFromRelationItem = new InputRecordViewSidebarRelationListItem();
					sidebarListFromRelationItem.EntityId = targetEntity.Id;
					sidebarListFromRelationItem.EntityName = targetEntity.Name;
					sidebarListFromRelationItem.FieldHelpText = "";
					sidebarListFromRelationItem.FieldLabel = "Attachments";
					sidebarListFromRelationItem.FieldLookupList = "lookup";
					sidebarListFromRelationItem.FieldManageView = "general";
					sidebarListFromRelationItem.FieldPlaceholder = "";
					sidebarListFromRelationItem.FieldRequired = false;
					sidebarListFromRelationItem.ListId = targetGeneralList.Id;
					sidebarListFromRelationItem.ListName = targetGeneralList.Name;
					sidebarListFromRelationItem.RelationId = targetRelation.Id;
					sidebarListFromRelationItem.RelationName = targetRelation.Name;
					sidebarListFromRelationItem.Type = "listFromRelation";
					updateViewInput.Sidebar = new InputRecordViewSidebar();
					updateViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();
					updateViewInput.Sidebar.Items.Add(sidebarListFromRelationItem);
				}
				#endregion

				#region << Comments >>
				{
					var targetEntity = entMan.ReadEntity(COMMENT_ENTITY_ID).Object;
					var targetGeneralList = targetEntity.RecordLists.Single(x => x.Name == "task_comments");
					var targetRelation = relMan.Read(new Guid("884b9480-dc1c-468a-98f0-2d5f10084622")).Object;
					sidebarListFromRelationItem = new InputRecordViewSidebarRelationListItem();
					sidebarListFromRelationItem.EntityId = targetEntity.Id;
					sidebarListFromRelationItem.EntityName = targetEntity.Name;
					sidebarListFromRelationItem.FieldHelpText = "";
					sidebarListFromRelationItem.FieldLabel = "Comments";
					sidebarListFromRelationItem.FieldLookupList = "lookup";
					sidebarListFromRelationItem.FieldManageView = "general";
					sidebarListFromRelationItem.FieldPlaceholder = "";
					sidebarListFromRelationItem.FieldRequired = false;
					sidebarListFromRelationItem.ListId = targetGeneralList.Id;
					sidebarListFromRelationItem.ListName = targetGeneralList.Name;
					sidebarListFromRelationItem.RelationId = targetRelation.Id;
					sidebarListFromRelationItem.RelationName = targetRelation.Name;
					sidebarListFromRelationItem.Type = "listFromRelation";
					updateViewInput.Sidebar.Items.Add(sidebarListFromRelationItem);
				}
				#endregion

				#region << Time logs >>
				{
					var targetEntity = entMan.ReadEntity(TIMELOG_ENTITY_ID).Object;
					var targetGeneralList = targetEntity.RecordLists.Single(x => x.Name == "task_timelogs");
					var targetRelation = relMan.Read(new Guid("61f1cd54-bcd6-4061-9c96-7934e01f0857")).Object;
					sidebarListFromRelationItem = new InputRecordViewSidebarRelationListItem();
					sidebarListFromRelationItem.EntityId = targetEntity.Id;
					sidebarListFromRelationItem.EntityName = targetEntity.Name;
					sidebarListFromRelationItem.FieldHelpText = "";
					sidebarListFromRelationItem.FieldLabel = "Time logs";
					sidebarListFromRelationItem.FieldLookupList = "lookup";
					sidebarListFromRelationItem.FieldManageView = "general";
					sidebarListFromRelationItem.FieldPlaceholder = "";
					sidebarListFromRelationItem.FieldRequired = false;
					sidebarListFromRelationItem.ListId = targetGeneralList.Id;
					sidebarListFromRelationItem.ListName = targetGeneralList.Name;
					sidebarListFromRelationItem.RelationId = targetRelation.Id;
					sidebarListFromRelationItem.RelationName = targetRelation.Name;
					sidebarListFromRelationItem.Type = "listFromRelation";
					updateViewInput.Sidebar.Items.Add(sidebarListFromRelationItem);
				}
				#endregion

				#region << Activities >>
				{
					var targetEntity = entMan.ReadEntity(ACTIVITY_ENTITY_ID).Object;
					var targetGeneralList = targetEntity.RecordLists.Single(x => x.Name == "task_activities");
					var targetRelation = relMan.Read(new Guid("8f294277-fd60-496e-bff7-9391fffcda41")).Object;
					sidebarListFromRelationItem = new InputRecordViewSidebarRelationListItem();
					sidebarListFromRelationItem.EntityId = targetEntity.Id;
					sidebarListFromRelationItem.EntityName = targetEntity.Name;
					sidebarListFromRelationItem.FieldHelpText = "";
					sidebarListFromRelationItem.FieldLabel = "Activities";
					sidebarListFromRelationItem.FieldLookupList = "lookup";
					sidebarListFromRelationItem.FieldManageView = "general";
					sidebarListFromRelationItem.FieldPlaceholder = "";
					sidebarListFromRelationItem.FieldRequired = false;
					sidebarListFromRelationItem.ListId = targetGeneralList.Id;
					sidebarListFromRelationItem.ListName = targetGeneralList.Name;
					sidebarListFromRelationItem.RelationId = targetRelation.Id;
					sidebarListFromRelationItem.RelationName = targetRelation.Name;
					sidebarListFromRelationItem.Type = "listFromRelation";
					updateViewInput.Sidebar.Items.Add(sidebarListFromRelationItem);
				}
				#endregion

				#endregion
				{
					var response = entMan.UpdateRecordView(TASK_ENTITY_ID, updateViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: " + TASK_ENTITY_NAME + " Updated view: general" + " Message:" + response.Message);
				}
			}
			#endregion

			#region << update bug general view >>
			{
				var updateViewEntity = entMan.ReadEntity(BUG_ENTITY_ID).Object;
				var updateView = updateViewEntity.RecordViews.Single(x => x.Name == "general");
				var updateViewInput = new InputRecordView();
				var viewSection = new InputRecordViewSection();
				var viewRow = new InputRecordViewRow();
				var viewColumn = new InputRecordViewColumn();
				var viewItem = new InputRecordViewFieldItem();
				var viewItemView = new InputRecordViewViewItem();
				var viewItemFromRelation = new InputRecordViewRelationFieldItem();
				//General view fields

				//Convert recordList to recordListInput
				updateViewInput = updateView.MapTo<InputRecordView>();

				#region << Details >>
				updateViewInput.Label = "[{code}] {subject}";
				updateViewInput.IconName = "bug";
				updateViewInput.ServiceCode = "";
				#endregion

				#region << Get the header Region >>
				var headerRegion = new InputRecordViewRegion();
				foreach (var region in updateViewInput.Regions)
				{
					if (region.Name == "header")
					{
						headerRegion = region;
					}
				}
				headerRegion.Sections = new List<InputRecordViewSection>();
				#endregion

				#region << Section >>
				viewSection = new InputRecordViewSection();
				viewSection.Id = new Guid("b3679dee-d30d-46d7-b5ac-300ed8f1e922");
				viewSection.Name = "details";
				viewSection.Label = "Details";
				viewSection.ShowLabel = false;
				viewSection.CssClass = "";
				viewSection.Collapsed = false;
				viewSection.TabOrder = "left-right";
				viewSection.Weight = 1;
				viewSection.Rows = new List<InputRecordViewRow>();

				#region << Row 1 >>
				viewRow = new InputRecordViewRow();
				viewRow.Id = new Guid("f9099d26-96ad-4fe2-9c81-db7a8f5daa47");
				viewRow.Weight = 1;
				viewRow.Columns = new List<InputRecordViewColumn>();

				#region << Column 1 >>
				viewColumn = new InputRecordViewColumn();
				viewColumn.GridColCount = 8;
				viewColumn.Items = new List<InputRecordViewItemBase>();


				#region << subject >>
				{
					viewItem = new InputRecordViewFieldItem();
					viewItem.EntityId = updateViewEntity.Id;
					viewItem.EntityName = updateViewEntity.Name;
					viewItem.FieldId = updateViewEntity.Fields.Single(x => x.Name == "subject").Id;
					viewItem.FieldName = "subject";
					viewItem.Type = "field";
					viewColumn.Items.Add(viewItem);
				}
				#endregion

				#region << project name from Relation >>
				{
					var targetEntity = entMan.ReadEntity(PROJECT_ENTITY_ID).Object;
					var targetRelation = relMan.Read("project_1_n_bug").Object;
					viewItemFromRelation = new InputRecordViewRelationFieldItem();
					viewItemFromRelation.EntityId = targetEntity.Id;
					viewItemFromRelation.EntityName = targetEntity.Name;
					viewItemFromRelation.Type = "fieldFromRelation";
					viewItemFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "name").Id;
					viewItemFromRelation.FieldName = "name";
					viewItemFromRelation.FieldLabel = "Project";
					viewItemFromRelation.FieldPlaceholder = "";
					viewItemFromRelation.FieldRequired = true;
					viewItemFromRelation.FieldLookupList = "lookup";
					viewItemFromRelation.RelationId = targetRelation.Id;
					viewItemFromRelation.RelationName = targetRelation.Name;
					viewColumn.Items.Add(viewItemFromRelation);
				}
				#endregion

				#region << description >>
				{
					viewItem = new InputRecordViewFieldItem();
					viewItem.EntityId = updateViewEntity.Id;
					viewItem.EntityName = updateViewEntity.Name;
					viewItem.FieldId = updateViewEntity.Fields.Single(x => x.Name == "description").Id;
					viewItem.FieldName = "description";
					viewItem.Type = "field";
					viewColumn.Items.Add(viewItem);
				}
				#endregion

				//Save column
				viewRow.Columns.Add(viewColumn);
				#endregion

				#region << Column 2 >>
				viewColumn = new InputRecordViewColumn();
				viewColumn.GridColCount = 4;
				viewColumn.Items = new List<InputRecordViewItemBase>();

				#region << code >>
				{
					viewItem = new InputRecordViewFieldItem();
					viewItem.EntityId = updateViewEntity.Id;
					viewItem.EntityName = updateViewEntity.Name;
					viewItem.FieldId = updateViewEntity.Fields.Single(x => x.Name == "code").Id;
					viewItem.FieldName = "code";
					viewItem.Type = "field";
					viewColumn.Items.Add(viewItem);
				}
				#endregion

				#region << status >>
				{
					viewItem = new InputRecordViewFieldItem();
					viewItem.EntityId = updateViewEntity.Id;
					viewItem.EntityName = updateViewEntity.Name;
					viewItem.FieldId = updateViewEntity.Fields.Single(x => x.Name == "status").Id;
					viewItem.FieldName = "status";
					viewItem.Type = "field";
					viewColumn.Items.Add(viewItem);
				}
				#endregion

				#region << priority >>
				{
					viewItem = new InputRecordViewFieldItem();
					viewItem.EntityId = updateViewEntity.Id;
					viewItem.EntityName = updateViewEntity.Name;
					viewItem.FieldId = updateViewEntity.Fields.Single(x => x.Name == "priority").Id;
					viewItem.FieldName = "priority";
					viewItem.Type = "field";
					viewColumn.Items.Add(viewItem);
				}
				#endregion

				#region << owner >>
				{
					var targetEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
					var targetRelation = relMan.Read("user_1_n_bug_owner").Object;
					viewItemFromRelation = new InputRecordViewRelationFieldItem();
					viewItemFromRelation.EntityId = targetEntity.Id;
					viewItemFromRelation.EntityName = targetEntity.Name;
					viewItemFromRelation.Type = "fieldFromRelation";
					viewItemFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "username").Id;
					viewItemFromRelation.FieldName = "username";
					viewItemFromRelation.FieldLabel = "Owner";
					viewItemFromRelation.FieldPlaceholder = "";
					viewItemFromRelation.FieldRequired = true;
					viewItemFromRelation.FieldLookupList = "lookup";
					viewItemFromRelation.RelationId = targetRelation.Id;
					viewItemFromRelation.RelationName = targetRelation.Name;
					viewColumn.Items.Add(viewItemFromRelation);
				}
				#endregion


				#region << watchers >>
				{
					var targetEntity = entMan.ReadEntity(SystemIds.UserEntityId).Object;
					var targetRelation = relMan.Read("user_n_n_bug_watchers").Object;
					viewItemFromRelation = new InputRecordViewRelationFieldItem();
					viewItemFromRelation.EntityId = targetEntity.Id;
					viewItemFromRelation.EntityName = targetEntity.Name;
					viewItemFromRelation.Type = "fieldFromRelation";
					viewItemFromRelation.FieldId = targetEntity.Fields.Single(x => x.Name == "username").Id;
					viewItemFromRelation.FieldName = "username";
					viewItemFromRelation.FieldLabel = "Watchers";
					viewItemFromRelation.FieldPlaceholder = "";
					viewItemFromRelation.FieldRequired = true;
					viewItemFromRelation.FieldLookupList = "lookup";
					viewItemFromRelation.RelationId = targetRelation.Id;
					viewItemFromRelation.RelationName = targetRelation.Name;
					viewColumn.Items.Add(viewItemFromRelation);
				}
				#endregion
				//Save column
				viewRow.Columns.Add(viewColumn);
				#endregion

				//Save row
				viewSection.Rows.Add(viewRow);
				#endregion

				//Save section
				headerRegion.Sections.Add(viewSection);

				#endregion

				#region << Sidebar >>
				var sidebarListFromRelationItem = new InputRecordViewSidebarRelationListItem();
				var sidebarViewItem = new InputRecordViewSidebarViewItem();
				updateViewInput.Sidebar = new InputRecordViewSidebar();
				updateViewInput.Sidebar.CssClass = "";
				updateViewInput.Sidebar.Render = true;
				if(updateViewInput.Sidebar.Items == null) {
					updateViewInput.Sidebar.Items = new List<InputRecordViewSidebarItemBase>();
				}

				#region << Attachments >>
				{
					var targetEntity = entMan.ReadEntity(ATTACHMENT_ENTITY_ID).Object;
					var targetGeneralList = targetEntity.RecordLists.Single(x => x.Name == "bug_attachments");
					var targetRelation = relMan.Read("bug_1_n_attachment").Object;
					sidebarListFromRelationItem = new InputRecordViewSidebarRelationListItem();
					sidebarListFromRelationItem.EntityId = targetEntity.Id;
					sidebarListFromRelationItem.EntityName = targetEntity.Name;
					sidebarListFromRelationItem.FieldHelpText = "";
					sidebarListFromRelationItem.FieldLabel = "Attachments";
					sidebarListFromRelationItem.FieldLookupList = "lookup";
					sidebarListFromRelationItem.FieldManageView = "general";
					sidebarListFromRelationItem.FieldPlaceholder = "";
					sidebarListFromRelationItem.FieldRequired = false;
					sidebarListFromRelationItem.ListId = targetGeneralList.Id;
					sidebarListFromRelationItem.ListName = targetGeneralList.Name;
					sidebarListFromRelationItem.RelationId = targetRelation.Id;
					sidebarListFromRelationItem.RelationName = targetRelation.Name;
					sidebarListFromRelationItem.Type = "listFromRelation";
					updateViewInput.Sidebar.Items.Add(sidebarListFromRelationItem);
				}
				#endregion

				#region << Comments >>
				{
					var targetEntity = entMan.ReadEntity(COMMENT_ENTITY_ID).Object;
					var targetGeneralList = targetEntity.RecordLists.Single(x => x.Name == "bug_comments");
					var targetRelation = relMan.Read("bug_1_n_comment").Object;
					sidebarListFromRelationItem = new InputRecordViewSidebarRelationListItem();
					sidebarListFromRelationItem.EntityId = targetEntity.Id;
					sidebarListFromRelationItem.EntityName = targetEntity.Name;
					sidebarListFromRelationItem.FieldHelpText = "";
					sidebarListFromRelationItem.FieldLabel = "Comments";
					sidebarListFromRelationItem.FieldLookupList = "lookup";
					sidebarListFromRelationItem.FieldManageView = "general";
					sidebarListFromRelationItem.FieldPlaceholder = "";
					sidebarListFromRelationItem.FieldRequired = false;
					sidebarListFromRelationItem.ListId = targetGeneralList.Id;
					sidebarListFromRelationItem.ListName = targetGeneralList.Name;
					sidebarListFromRelationItem.RelationId = targetRelation.Id;
					sidebarListFromRelationItem.RelationName = targetRelation.Name;
					sidebarListFromRelationItem.Type = "listFromRelation";
					updateViewInput.Sidebar.Items.Add(sidebarListFromRelationItem);
				}
				#endregion

				#region << Time logs >>
				{
					var targetEntity = entMan.ReadEntity(TIMELOG_ENTITY_ID).Object;
					var targetGeneralList = targetEntity.RecordLists.Single(x => x.Name == "bug_timelogs");
					var targetRelation = relMan.Read("bug_1_n_time_log").Object;
					sidebarListFromRelationItem = new InputRecordViewSidebarRelationListItem();
					sidebarListFromRelationItem.EntityId = targetEntity.Id;
					sidebarListFromRelationItem.EntityName = targetEntity.Name;
					sidebarListFromRelationItem.FieldHelpText = "";
					sidebarListFromRelationItem.FieldLabel = "Time logs";
					sidebarListFromRelationItem.FieldLookupList = "lookup";
					sidebarListFromRelationItem.FieldManageView = "general";
					sidebarListFromRelationItem.FieldPlaceholder = "";
					sidebarListFromRelationItem.FieldRequired = false;
					sidebarListFromRelationItem.ListId = targetGeneralList.Id;
					sidebarListFromRelationItem.ListName = targetGeneralList.Name;
					sidebarListFromRelationItem.RelationId = targetRelation.Id;
					sidebarListFromRelationItem.RelationName = targetRelation.Name;
					sidebarListFromRelationItem.Type = "listFromRelation";
					updateViewInput.Sidebar.Items.Add(sidebarListFromRelationItem);
				}
				#endregion

				#region << Activities >>
				{
					var targetEntity = entMan.ReadEntity(ACTIVITY_ENTITY_ID).Object;
					var targetGeneralList = targetEntity.RecordLists.Single(x => x.Name == "bug_activities");
					var targetRelation = relMan.Read("bug_1_n_activity").Object;
					sidebarListFromRelationItem = new InputRecordViewSidebarRelationListItem();
					sidebarListFromRelationItem.EntityId = targetEntity.Id;
					sidebarListFromRelationItem.EntityName = targetEntity.Name;
					sidebarListFromRelationItem.FieldHelpText = "";
					sidebarListFromRelationItem.FieldLabel = "Activities";
					sidebarListFromRelationItem.FieldLookupList = "lookup";
					sidebarListFromRelationItem.FieldManageView = "general";
					sidebarListFromRelationItem.FieldPlaceholder = "";
					sidebarListFromRelationItem.FieldRequired = false;
					sidebarListFromRelationItem.ListId = targetGeneralList.Id;
					sidebarListFromRelationItem.ListName = targetGeneralList.Name;
					sidebarListFromRelationItem.RelationId = targetRelation.Id;
					sidebarListFromRelationItem.RelationName = targetRelation.Name;
					sidebarListFromRelationItem.Type = "listFromRelation";
					updateViewInput.Sidebar.Items.Add(sidebarListFromRelationItem);
				}
				#endregion

				#endregion
				{
					var response = entMan.UpdateRecordView(BUG_ENTITY_ID, updateViewInput);
					if (!response.Success)
						throw new Exception("System error 10060. Entity: " + BUG_ENTITY_NAME + " Updated view: general" + " Message:" + response.Message);
				}
			}
			#endregion

			if (createSampleRecords)
			{
				#region << Create Project Team Role >>
				{
					var sampleRecord = new EntityRecord();
					sampleRecord["id"] = new Guid("083a5c46-7dbe-4ff9-b19f-44603671ccb2");
					sampleRecord["name"] = "project_team";
					sampleRecord["description"] = "Project team role for the Project application";
					var createSampleRecordResult = recMan.CreateRecord(SystemIds.RoleEntityId, sampleRecord);
					if (!createSampleRecordResult.Success)
					{
						throw new Exception("System error 10060. Create sample role record. Message:" + createSampleRecordResult.Message);
					}
				}
				#endregion

				#region << Create Sample Project Manager User >>
				{
					var sampleRecord = new EntityRecord();
					sampleRecord["id"] = new Guid("b646c5d4-acc8-4404-af77-6786b81bee05");
					sampleRecord["first_name"] = "Project";
					sampleRecord["last_name"] = "Manager";
					sampleRecord["username"] = "project_manager";
					sampleRecord["email"] = "manager@project.com";
					sampleRecord["password"] = "sample123";
					sampleRecord["enabled"] = true;
					sampleRecord["verified"] = true;
					sampleRecord["image"] = "/plugins/webvella-core/assets/avatar-red.png";
					var createSampleRecordResult = recMan.CreateRecord(SystemIds.UserEntityId, sampleRecord);
					if (!createSampleRecordResult.Success)
					{
						throw new Exception("System error 10060. Create sample customer record. Message:" + createSampleRecordResult.Message);
					}
				}
				#endregion

				#region << Create Sample Project Team User >>
				{
					var sampleRecord = new EntityRecord();
					sampleRecord["id"] = new Guid("6c820f13-f7d0-429e-a04a-c0ec3ba6ade0");
					sampleRecord["first_name"] = "Project";
					sampleRecord["last_name"] = "Team";
					sampleRecord["username"] = "project_team";
					sampleRecord["email"] = "team@project.com";
					sampleRecord["password"] = "sample123";
					sampleRecord["enabled"] = true;
					sampleRecord["verified"] = true;
					sampleRecord["image"] = "/plugins/webvella-core/assets/avatar-green.png";
					var createSampleRecordResult = recMan.CreateRecord(SystemIds.UserEntityId, sampleRecord);
					if (!createSampleRecordResult.Success)
					{
						throw new Exception("System error 10060. Create sample customer record. Message:" + createSampleRecordResult.Message);
					}
				}
				#endregion

				#region << Create relation between project manager user and role >>
				{
					var createRelationNtoNResponse = recMan.CreateRelationManyToManyRecord(new Guid("0c4b119e-1d7b-4b40-8d2c-9e447cc656ab"), new Guid("083a5c46-7dbe-4ff9-b19f-44603671ccb2"), new Guid("b646c5d4-acc8-4404-af77-6786b81bee05"));
					if (!createRelationNtoNResponse.Success)
					{
						throw new Exception("Could not create item image relation" + createRelationNtoNResponse.Message);
					}
				}
				#endregion

				#region << Create relation between project manager user and regular role  >>
				{
					var createRelationNtoNResponse = recMan.CreateRelationManyToManyRecord(new Guid("0c4b119e-1d7b-4b40-8d2c-9e447cc656ab"), new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"), new Guid("b646c5d4-acc8-4404-af77-6786b81bee05"));
					if (!createRelationNtoNResponse.Success)
					{
						throw new Exception("Could not create item image relation" + createRelationNtoNResponse.Message);
					}
				}
				#endregion

				#region << Create relation between project team user and role >>
				{
					var createRelationNtoNResponse = recMan.CreateRelationManyToManyRecord(new Guid("0c4b119e-1d7b-4b40-8d2c-9e447cc656ab"), new Guid("083a5c46-7dbe-4ff9-b19f-44603671ccb2"), new Guid("6c820f13-f7d0-429e-a04a-c0ec3ba6ade0"));
					if (!createRelationNtoNResponse.Success)
					{
						throw new Exception("Could not create item image relation" + createRelationNtoNResponse.Message);
					}
				}
				#endregion

				#region << Create relation between project manager user and regular role  >>
				{
					var createRelationNtoNResponse = recMan.CreateRelationManyToManyRecord(new Guid("0c4b119e-1d7b-4b40-8d2c-9e447cc656ab"), new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"), new Guid("6c820f13-f7d0-429e-a04a-c0ec3ba6ade0"));
					if (!createRelationNtoNResponse.Success)
					{
						throw new Exception("Could not create item image relation" + createRelationNtoNResponse.Message);
					}
				}
				#endregion

				#region << Create relation between the system admin and project team role, so he can see the first project >>
				{
					var createRelationNtoNResponse = recMan.CreateRelationManyToManyRecord(new Guid("0c4b119e-1d7b-4b40-8d2c-9e447cc656ab"), new Guid("083a5c46-7dbe-4ff9-b19f-44603671ccb2"), SystemIds.FirstUserId);
					if (!createRelationNtoNResponse.Success)
					{
						throw new Exception("Could not create item image relation" + createRelationNtoNResponse.Message);
					}
				}
				#endregion

				#region << Create Sample Project >>
				{
					var sampleRecord = new EntityRecord();
					sampleRecord["id"] = new Guid("a0141850-b13c-44b4-bb1b-4e0dde4850f4");
					sampleRecord["name"] = "Corporate website development";
					sampleRecord["description"] = "All activities for developing a sample application";
					sampleRecord["code"] = "SMPL";
					sampleRecord["billable_hour_price"] = 100;
					sampleRecord["owner_id"] = new Guid("b646c5d4-acc8-4404-af77-6786b81bee05");
					sampleRecord["customer_id"] = CUSTOMER_RECORD_ID;
					var customerRoles = new List<Guid>();
					customerRoles.Add(CUSTOMER_ROLE_ID);
					sampleRecord["$$role_n_n_project_customer.id"] = customerRoles;
					var projectTeamRoles = new List<Guid>();
					projectTeamRoles.Add(new Guid("083a5c46-7dbe-4ff9-b19f-44603671ccb2"));
					sampleRecord["$$role_n_n_project_team.id"] = projectTeamRoles;
					sampleRecord["priority"] = "medium";
					sampleRecord["status"] = "in review";
					sampleRecord["start_date"] = DateTime.UtcNow.AddDays(3);
					sampleRecord["end_date"] = DateTime.UtcNow.AddDays(90);
					var createSampleRecordResult = recMan.CreateRecord(PROJECT_ENTITY_NAME, sampleRecord);
					if (!createSampleRecordResult.Success)
					{
						throw new Exception("System error 10060. Create sample project. Message:" + createSampleRecordResult.Message);
					}
				}
				#endregion

				#region << Create Sample Milestone >>
				{
					var sampleRecord = new EntityRecord();
					sampleRecord["id"] = new Guid("42b881fc-d93f-46cf-b39d-391cd42fd2f6");
					sampleRecord["name"] = "Specification documents development";
					sampleRecord["start_date"] = DateTime.UtcNow.AddDays(3);
					sampleRecord["status"] = "opened";
					sampleRecord["end_date"] = DateTime.UtcNow.AddDays(90);
					sampleRecord["project_id"] = new Guid("a0141850-b13c-44b4-bb1b-4e0dde4850f4");
					var createSampleRecordResult = recMan.CreateRecord(MILESTONE_ENTITY_ID, sampleRecord);
					if (!createSampleRecordResult.Success)
					{
						throw new Exception("System error 10060. Create sample milestone. Message:" + createSampleRecordResult.Message);
					}

				}
				#endregion

				#region << Create Sample Task >>
				{
					var sampleRecord = new EntityRecord();
					sampleRecord["id"] = new Guid("70515386-7612-480d-99e6-76b97ca4190a");
					sampleRecord["subject"] = "Corporate identity specification";
					sampleRecord["owner_id"] = new Guid("b646c5d4-acc8-4404-af77-6786b81bee05");
					sampleRecord["milestone_id"] = new Guid("42b881fc-d93f-46cf-b39d-391cd42fd2f6");
					sampleRecord["description"] = "This is a sample task describing how to generate a corporate identity document";
					sampleRecord["parent_id"] = null;
					sampleRecord["code"] = "SMPL-T1";
					sampleRecord["start_date"] = DateTime.UtcNow.AddDays(3);
					sampleRecord["end_date"] = DateTime.UtcNow.AddDays(90);
					sampleRecord["priority"] = "medium";
					sampleRecord["status"] = "not started";
					sampleRecord["x_billable_hours"] = 0;
					sampleRecord["x_nonbillable_hours"] = 0;
					sampleRecord["project_id"] = new Guid("a0141850-b13c-44b4-bb1b-4e0dde4850f4");
					var createSampleRecordResult = recMan.CreateRecord(TASK_ENTITY_ID, sampleRecord);
					if (!createSampleRecordResult.Success)
					{
						throw new Exception("System error 10060. Create sample task. Message:" + createSampleRecordResult.Message);
					}
				}
				#endregion

				#region << Add watchers to the task >>
				//Creator
				{
					var targetRelation = relMan.Read("user_n_n_task_watchers").Object;
					var createRelationNtoNResponse = recMan.CreateRelationManyToManyRecord(targetRelation.Id, SystemIds.FirstUserId, new Guid("70515386-7612-480d-99e6-76b97ca4190a"));
					if (!createRelationNtoNResponse.Success)
					{
						throw new Exception("Could not create watch relation" + createRelationNtoNResponse.Message);
					}
				}
				// Project manager
				{
					var targetRelation = relMan.Read("user_n_n_task_watchers").Object;
					var createRelationNtoNResponse = recMan.CreateRelationManyToManyRecord(targetRelation.Id, new Guid("b646c5d4-acc8-4404-af77-6786b81bee05"), new Guid("70515386-7612-480d-99e6-76b97ca4190a"));
					if (!createRelationNtoNResponse.Success)
					{
						throw new Exception("Could not create watch relation" + createRelationNtoNResponse.Message);
					}
				}
				#endregion

				#region << Create Sample BUG >>
				{
					var sampleRecord = new EntityRecord();
					sampleRecord["id"] = new Guid("18934091-b4e4-4db4-8beb-678cd29b6916");
					sampleRecord["subject"] = "Sample bug subject";
					sampleRecord["owner_id"] = new Guid("b646c5d4-acc8-4404-af77-6786b81bee05");
					sampleRecord["milestone_id"] = new Guid("42b881fc-d93f-46cf-b39d-391cd42fd2f6");
					sampleRecord["description"] = "This is a sample bug about the project";
					sampleRecord["priority"] = "medium";
					sampleRecord["code"] = "SMPL-B1";
					sampleRecord["status"] = "opened";
					sampleRecord["x_billable_hours"] = 0;
					sampleRecord["x_nonbillable_hours"] = 0;
					sampleRecord["project_id"] = new Guid("a0141850-b13c-44b4-bb1b-4e0dde4850f4");
					var createSampleRecordResult = recMan.CreateRecord(BUG_ENTITY_ID, sampleRecord);
					if (!createSampleRecordResult.Success)
					{
						throw new Exception("System error 10060. Create sample bug. Message:" + createSampleRecordResult.Message);
					}
				}
				#endregion

				#region << Add watchers to the bug >>
				//Creator
				{
					var targetRelation = relMan.Read("user_n_n_bug_watchers").Object;
					var createRelationNtoNResponse = recMan.CreateRelationManyToManyRecord(targetRelation.Id, SystemIds.FirstUserId, new Guid("18934091-b4e4-4db4-8beb-678cd29b6916"));
					if (!createRelationNtoNResponse.Success)
					{
						throw new Exception("Could not create watch relation" + createRelationNtoNResponse.Message);
					}
				}
				// Project manager
				{
					var targetRelation = relMan.Read("user_n_n_bug_watchers").Object;
					var createRelationNtoNResponse = recMan.CreateRelationManyToManyRecord(targetRelation.Id, new Guid("b646c5d4-acc8-4404-af77-6786b81bee05"), new Guid("18934091-b4e4-4db4-8beb-678cd29b6916"));
					if (!createRelationNtoNResponse.Success)
					{
						throw new Exception("Could not create watch relation" + createRelationNtoNResponse.Message);
					}
				}
				#endregion

				#region << Update the project >>
				{
					var filterObj = EntityQuery.QueryEQ("id", new Guid("a0141850-b13c-44b4-bb1b-4e0dde4850f4"));
					var resultQuery = new EntityQuery(PROJECT_ENTITY_NAME, "*", filterObj, null, null, null, null);
					var updateResult = recMan.Find(resultQuery);
					if (!updateResult.Success)
					{
						throw new Exception("Failed to update the project");
					}
					var updateRecord = updateResult.Object.Data[0];
					updateRecord["x_milestones_opened"] = (decimal)updateRecord["x_milestones_opened"] + 1;
					updateRecord["x_tasks_not_started"] = (decimal)updateRecord["x_tasks_not_started"] + 1;
					updateRecord["x_bugs_opened"] = (decimal)updateRecord["x_bugs_opened"] + 1;
					var createSampleRecordResult = recMan.UpdateRecord(PROJECT_ENTITY_ID, updateRecord);
					if (!createSampleRecordResult.Success)
					{
						throw new Exception("System error 10060.Update sample project. Message:" + createSampleRecordResult.Message);
					}
				}
				#endregion

				#region << Update the milestone >>
				{
					var filterObj = EntityQuery.QueryEQ("id", new Guid("42b881fc-d93f-46cf-b39d-391cd42fd2f6"));
					var resultQuery = new EntityQuery(MILESTONE_ENTITY_NAME, "*", filterObj, null, null, null, null);
					var updateResult = recMan.Find(resultQuery);
					if (!updateResult.Success)
					{
						throw new Exception("Failed to update the project");
					}
					var updateRecord = updateResult.Object.Data[0];
					updateRecord["x_tasks_not_started"] = (decimal)updateRecord["x_tasks_not_started"] + 1;
					updateRecord["x_bugs_opened"] = (decimal)updateRecord["x_bugs_opened"] + 1;
					var createSampleRecordResult = recMan.UpdateRecord(MILESTONE_ENTITY_NAME, updateRecord);
					if (!createSampleRecordResult.Success)
					{
						throw new Exception("System error 10060. Update sample milestone. Message:" + createSampleRecordResult.Message);
					}
				}
				#endregion
			}

		}

	}
}
