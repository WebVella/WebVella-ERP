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
		private static void Patch20180912(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan, bool createSampleRecords = false)
		{
#region << ***Create field***  Entity: wv_bug Field Name: indexed_on >>
{
	InputDateTimeField datetimeField = new InputDateTimeField();
	datetimeField.Id =  new Guid("3fa55ffc-eb9a-4fd1-8570-03e861ae043f");
	datetimeField.Name = "indexed_on";
	datetimeField.Label = "Indexed On";
	datetimeField.PlaceholderText = "";
	datetimeField.Description = "";
	datetimeField.HelpText = "";
	datetimeField.Required = false;
	datetimeField.Unique = false;
	datetimeField.Searchable = true;
	datetimeField.Auditable = false;
	datetimeField.System = true;
	datetimeField.DefaultValue = null;
	datetimeField.Format = "yyyy-MMM-dd HH:mm";
	datetimeField.UseCurrentTimeAsDefaultValue = false;
	datetimeField.EnableSecurity = false;
	datetimeField.Permissions = new FieldPermissions();
	datetimeField.Permissions.CanRead = new List<Guid>();
	datetimeField.Permissions.CanUpdate = new List<Guid>();
	//READ
	//UPDATE
	{
		var response = entMan.CreateField(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c"), datetimeField, false);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_bug Field: indexed_on Message:" + response.Message);
	}
}
#endregion

#region << ***Create field***  Entity: wv_task Field Name: indexed_on >>
{
	InputDateTimeField datetimeField = new InputDateTimeField();
	datetimeField.Id =  new Guid("2a28b090-433d-432b-9944-b00f6a5dbd3c");
	datetimeField.Name = "indexed_on";
	datetimeField.Label = "Indexed On";
	datetimeField.PlaceholderText = "";
	datetimeField.Description = "";
	datetimeField.HelpText = "";
	datetimeField.Required = false;
	datetimeField.Unique = false;
	datetimeField.Searchable = true;
	datetimeField.Auditable = false;
	datetimeField.System = true;
	datetimeField.DefaultValue = null;
	datetimeField.Format = "yyyy-MMM-dd HH:mm";
	datetimeField.UseCurrentTimeAsDefaultValue = false;
	datetimeField.EnableSecurity = false;
	datetimeField.Permissions = new FieldPermissions();
	datetimeField.Permissions.CanRead = new List<Guid>();
	datetimeField.Permissions.CanUpdate = new List<Guid>();
	//READ
	//UPDATE
	{
		var response = entMan.CreateField(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89"), datetimeField, false);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_task Field: indexed_on Message:" + response.Message);
	}
}
#endregion

#region << ***Create field***  Entity: wv_project_activity Field Name: user_id >>
{
	InputGuidField guidField = new InputGuidField();
	guidField.Id = new Guid("6819bbbc-65f8-4664-aecd-a8997d130cdb");
	guidField.Name = "user_id";
	guidField.Label = "User Id";
	guidField.PlaceholderText = "";
	guidField.Description = "";
	guidField.HelpText = "";
	guidField.Required = false;
	guidField.Unique = false;
	guidField.Searchable = true;
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
		var response = entMan.CreateField(new Guid("145a489b-4dfc-4639-b473-2dedccb93ce0"), guidField, false);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_project_activity Field: user_id Message:" + response.Message);
	}
}
#endregion

#region << ***Create entity*** Entity name: search >>
{
	#region << entity >>
	{
		var entity = new InputEntity();
		var systemFieldIdDictionary = new Dictionary<string,Guid>();
		systemFieldIdDictionary["id"] = new Guid("a28b6c82-5ca3-4b2d-95f9-967501b0c619");
		systemFieldIdDictionary["created_on"] = new Guid("234f44fa-512a-445e-a693-284e23ada9d8");
		systemFieldIdDictionary["created_by"] = new Guid("b669d361-b1cc-4d89-87b7-c8ef661a82cd");
		systemFieldIdDictionary["last_modified_on"] = new Guid("df59fde6-9367-4acd-b0a3-1880dba2cfea");
		systemFieldIdDictionary["last_modified_by"] = new Guid("4ba95b2e-01f0-4b9b-b1be-6e6ed73351c7");
		systemFieldIdDictionary["user_search_created_by"] = new Guid("f7f07ebd-1d26-4fe4-95f3-114d2322ce74");
		systemFieldIdDictionary["user_search_modified_by"] = new Guid("277fbd26-dce9-4942-af50-9ccd58d810f1");
		entity.Id = new Guid("171659b7-79a3-457e-844b-6c954b59420f");
		entity.Name = "search";
		entity.Label = "Search";
		entity.LabelPlural = "Search";
		entity.System = true;
		entity.IconName = "search";
		entity.Weight = (decimal)10.0;
		entity.RecordPermissions = new RecordPermissions();
		entity.RecordPermissions.CanCreate = new List<Guid>();
		entity.RecordPermissions.CanRead = new List<Guid>();
		entity.RecordPermissions.CanUpdate = new List<Guid>();
		entity.RecordPermissions.CanDelete = new List<Guid>();
		//Create
		entity.RecordPermissions.CanCreate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
		entity.RecordPermissions.CanCreate.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
		//READ
		entity.RecordPermissions.CanRead.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
		entity.RecordPermissions.CanRead.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
		//UPDATE
		entity.RecordPermissions.CanUpdate.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
		entity.RecordPermissions.CanUpdate.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
		//DELETE
		entity.RecordPermissions.CanDelete.Add(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"));
		entity.RecordPermissions.CanDelete.Add(new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));
		{
			var response = entMan.CreateEntity(entity, false, false,systemFieldIdDictionary);
			if (!response.Success)
				throw new Exception("System error 10050. Entity: search creation Message: " + response.Message);
		}
	}
	#endregion
}
#endregion

#region << ***Create field***  Entity: search Field Name: item_id >>
{
	InputGuidField guidField = new InputGuidField();
	guidField.Id = new Guid("99379d7b-defe-4e62-b9ee-b32f79d9429e");
	guidField.Name = "item_id";
	guidField.Label = "Item ID";
	guidField.PlaceholderText = "";
	guidField.Description = "";
	guidField.HelpText = "";
	guidField.Required = true;
	guidField.Unique = true;
	guidField.Searchable = true;
	guidField.Auditable = false;
	guidField.System = true;
	guidField.DefaultValue = Guid.Parse("00000000-0000-0000-0000-000000000000");
	guidField.GenerateNewId = true;
	guidField.EnableSecurity = false;
	guidField.Permissions = new FieldPermissions();
	guidField.Permissions.CanRead = new List<Guid>();
	guidField.Permissions.CanUpdate = new List<Guid>();
	//READ
	//UPDATE
	{
		var response = entMan.CreateField(new Guid("171659b7-79a3-457e-844b-6c954b59420f"), guidField, false);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: search Field: item_id Message:" + response.Message);
	}
}
#endregion

#region << ***Create field***  Entity: search Field Name: entity >>
{
	InputTextField textboxField = new InputTextField();
	textboxField.Id = new Guid("c5ba7772-db1c-407f-841d-36ca15abbe69");
	textboxField.Name = "entity";
	textboxField.Label = "Entity";
	textboxField.PlaceholderText = "";
	textboxField.Description = "";
	textboxField.HelpText = "";
	textboxField.Required = true;
	textboxField.Unique = false;
	textboxField.Searchable = true;
	textboxField.Auditable = false;
	textboxField.System = true;
	textboxField.DefaultValue = "entity";
	textboxField.MaxLength = null;
	textboxField.EnableSecurity = false;
	textboxField.Permissions = new FieldPermissions();
	textboxField.Permissions.CanRead = new List<Guid>();
	textboxField.Permissions.CanUpdate = new List<Guid>();
	//READ
	//UPDATE
	{
		var response = entMan.CreateField(new Guid("171659b7-79a3-457e-844b-6c954b59420f"), textboxField, false);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: search Field: entity Message:" + response.Message);
	}
}
#endregion

#region << ***Create field***  Entity: search Field Name: index >>
{
	InputTextField textboxField = new InputTextField();
	textboxField.Id = new Guid("5ef4a0e0-3e47-4e22-8ae4-fb25207ed7cf");
	textboxField.Name = "index";
	textboxField.Label = "Index";
	textboxField.PlaceholderText = "";
	textboxField.Description = "";
	textboxField.HelpText = "";
	textboxField.Required = true;
	textboxField.Unique = false;
	textboxField.Searchable = true;
	textboxField.Auditable = false;
	textboxField.System = true;
	textboxField.DefaultValue = "ddddd";
	textboxField.MaxLength = null;
	textboxField.EnableSecurity = false;
	textboxField.Permissions = new FieldPermissions();
	textboxField.Permissions.CanRead = new List<Guid>();
	textboxField.Permissions.CanUpdate = new List<Guid>();
	//READ
	//UPDATE
	{
		var response = entMan.CreateField(new Guid("171659b7-79a3-457e-844b-6c954b59420f"), textboxField, false);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: search Field: index Message:" + response.Message);
	}
}
#endregion

#region << ***Create field***  Entity: search Field Name: snippet >>
{
	InputTextField textboxField = new InputTextField();
	textboxField.Id = new Guid("c43dc119-aebe-4114-820d-6d888cda115c");
	textboxField.Name = "snippet";
	textboxField.Label = "Snippet";
	textboxField.PlaceholderText = "";
	textboxField.Description = "";
	textboxField.HelpText = "";
	textboxField.Required = true;
	textboxField.Unique = false;
	textboxField.Searchable = false;
	textboxField.Auditable = false;
	textboxField.System = true;
	textboxField.DefaultValue = "search snippet";
	textboxField.MaxLength = null;
	textboxField.EnableSecurity = false;
	textboxField.Permissions = new FieldPermissions();
	textboxField.Permissions.CanRead = new List<Guid>();
	textboxField.Permissions.CanUpdate = new List<Guid>();
	//READ
	//UPDATE
	{
		var response = entMan.CreateField(new Guid("171659b7-79a3-457e-844b-6c954b59420f"), textboxField, false);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: search Field: snippet Message:" + response.Message);
	}
}
#endregion

#region << ***Create field***  Entity: search Field Name: url >>
{
	InputUrlField urlField = new InputUrlField();
	urlField.Id = new Guid("e0324495-06f3-4c03-aea0-4f1ef6357ba4");
	urlField.Name = "url";
	urlField.Label = "Url";
	urlField.PlaceholderText = "";
	urlField.Description = "";
	urlField.HelpText = "";
	urlField.Required = true;
	urlField.Unique = false;
	urlField.Searchable = false;
	urlField.Auditable = false;
	urlField.System = true;
	urlField.DefaultValue = "/";
	urlField.MaxLength = null;
	urlField.OpenTargetInNewWindow = false;
	urlField.EnableSecurity = false;
	urlField.Permissions = new FieldPermissions();
	urlField.Permissions.CanRead = new List<Guid>();
	urlField.Permissions.CanUpdate = new List<Guid>();
	//READ
	//UPDATE
	{
		var response = entMan.CreateField(new Guid("171659b7-79a3-457e-844b-6c954b59420f"), urlField, false);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: search Field: url Message:" + response.Message);
	}
}
#endregion

#region << ***Create field***  Entity: search Field Name: title >>
{
	InputTextField textboxField = new InputTextField();
	textboxField.Id = new Guid("53370748-600d-446e-8995-6bb02785642a");
	textboxField.Name = "title";
	textboxField.Label = "Title";
	textboxField.PlaceholderText = "";
	textboxField.Description = "";
	textboxField.HelpText = "";
	textboxField.Required = true;
	textboxField.Unique = false;
	textboxField.Searchable = false;
	textboxField.Auditable = false;
	textboxField.System = true;
	textboxField.DefaultValue = "Search Title";
	textboxField.MaxLength = null;
	textboxField.EnableSecurity = false;
	textboxField.Permissions = new FieldPermissions();
	textboxField.Permissions.CanRead = new List<Guid>();
	textboxField.Permissions.CanUpdate = new List<Guid>();
	//READ
	//UPDATE
	{
		var response = entMan.CreateField(new Guid("171659b7-79a3-457e-844b-6c954b59420f"), textboxField, false);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: search Field: title Message:" + response.Message);
	}
}
#endregion

#region << ***Create field***  Entity: search Field Name: project_id >>
{
	InputGuidField guidField = new InputGuidField();
	guidField.Id = new Guid("5b24518d-4dcc-46de-b2d6-d7947dbcceee");
	guidField.Name = "project_id";
	guidField.Label = "Project ID";
	guidField.PlaceholderText = "";
	guidField.Description = "";
	guidField.HelpText = "";
	guidField.Required = true;
	guidField.Unique = false;
	guidField.Searchable = true;
	guidField.Auditable = false;
	guidField.System = true;
	guidField.DefaultValue = Guid.Parse("00000000-0000-0000-0000-000000000000");
	guidField.GenerateNewId = false;
	guidField.EnableSecurity = false;
	guidField.Permissions = new FieldPermissions();
	guidField.Permissions.CanRead = new List<Guid>();
	guidField.Permissions.CanUpdate = new List<Guid>();
	//READ
	//UPDATE
	{
		var response = entMan.CreateField(new Guid("171659b7-79a3-457e-844b-6c954b59420f"), guidField, false);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: search Field: project_id Message:" + response.Message);
	}
}
#endregion

#region << View  Entity: wv_project name: search >>
{
	var createViewEntity = entMan.ReadEntity(new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32")).Object;
	var createViewInput = new InputRecordView();

	#region << details >>
	createViewInput.Id = new Guid("2010d57a-4a01-48e6-9a75-53ae63af9110");
	createViewInput.Type = "General";
	createViewInput.Name = "search";
	createViewInput.Label = "search";
	createViewInput.Title = "[{code}] {name}";
	createViewInput.Default = false;
	createViewInput.System = true;
	createViewInput.Weight = Decimal.Parse("10");
	createViewInput.CssClass = null;
	createViewInput.IconName = "tachometer";
	createViewInput.DynamicHtmlTemplate = "/plugins/webvella-projects/templates/search.html";
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
			viewRegion.Weight = Decimal.Parse("1");
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
			actionItem.Weight = Decimal.Parse("1");
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
			actionItem.Weight = Decimal.Parse("1");
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
		var response = entMan.CreateRecordView(new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32"), createViewInput);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_project Updated view: search Message:" + response.Message);
	}
}
#endregion

		

		}


	}
}
