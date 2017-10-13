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
		private static void Patch20170502(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan, bool createSampleRecords = false)
		{

#region << ***Create field***  Entity: wv_task Field Name: fts >>
{
	InputTextField textboxField = new InputTextField();
	textboxField.Id = new Guid("160c64ef-7d1a-48d6-9b47-2cae1bda515c");
	textboxField.Name = "fts";
	textboxField.Label = "FTS";
	textboxField.PlaceholderText = "";
	textboxField.Description = "";
	textboxField.HelpText = "";
	textboxField.Required = false;
	textboxField.Unique = false;
	textboxField.Searchable = false;
	textboxField.Auditable = false;
	textboxField.System = false;
	textboxField.DefaultValue = null;
	textboxField.MaxLength = null;
	textboxField.EnableSecurity = false;
	textboxField.Permissions = new FieldPermissions();
	textboxField.Permissions.CanRead = new List<Guid>();
	textboxField.Permissions.CanUpdate = new List<Guid>();
	//READ
	//UPDATE
	{
		var response = entMan.CreateField(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89"), textboxField, false);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_task Field: fts Message:" + response.Message);
	}
}
#endregion

#region << ***Create field***  Entity: wv_bug Field Name: fts >>
{
	InputTextField textboxField = new InputTextField();
	textboxField.Id = new Guid("409c4a24-fb5a-464e-ba43-c91675063a33");
	textboxField.Name = "fts";
	textboxField.Label = "FTS";
	textboxField.PlaceholderText = "";
	textboxField.Description = "";
	textboxField.HelpText = "";
	textboxField.Required = false;
	textboxField.Unique = false;
	textboxField.Searchable = false;
	textboxField.Auditable = false;
	textboxField.System = false;
	textboxField.DefaultValue = null;
	textboxField.MaxLength = null;
	textboxField.EnableSecurity = false;
	textboxField.Permissions = new FieldPermissions();
	textboxField.Permissions.CanRead = new List<Guid>();
	textboxField.Permissions.CanUpdate = new List<Guid>();
	//READ
	//UPDATE
	{
		var response = entMan.CreateField(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c"), textboxField, false);
		if (!response.Success)
			throw new Exception("System error 10060. Entity: wv_bug Field: fts Message:" + response.Message);
	}
}
#endregion

#region << ***Update list***  Entity: wv_project List Name: admin >>
{
	var createListEntity = entMan.ReadEntity(new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "admin").Id;
	createListInput.Type =  "Hidden";
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
			actionItem.Template = @"<a class=""btn btn-default btn-outline btn-sm"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record, ngCtrl)}}"">
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
	createListInput.ColumnWidthsCSV = "100px,auto,120px,120px,120px,120px";
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
			actionItem.Template = @"<a class=""btn btn-default btn-outline btn-sm"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record, ngCtrl)}}"">
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
		queryDictionary[new Guid("07d79609-0db1-4b33-bfa7-77ab7f1aee33")] = new InputRecordListQuery();
		queryDictionary[new Guid("07d79609-0db1-4b33-bfa7-77ab7f1aee33")].FieldName = null;
		queryDictionary[new Guid("07d79609-0db1-4b33-bfa7-77ab7f1aee33")].FieldValue =  null;
		queryDictionary[new Guid("07d79609-0db1-4b33-bfa7-77ab7f1aee33")].QueryType = "AND";
		queryDictionary[new Guid("07d79609-0db1-4b33-bfa7-77ab7f1aee33")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("fa78f61e-6393-4269-b5a9-b569ee994756")] = new InputRecordListQuery();
			queryDictionary[new Guid("fa78f61e-6393-4269-b5a9-b569ee994756")].FieldName = "owner_id";
			queryDictionary[new Guid("fa78f61e-6393-4269-b5a9-b569ee994756")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("fa78f61e-6393-4269-b5a9-b569ee994756")].QueryType = "EQ";
			queryDictionary[new Guid("fa78f61e-6393-4269-b5a9-b569ee994756")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("fa78f61e-6393-4269-b5a9-b569ee994756"))) {queryDictionary[new Guid("fa78f61e-6393-4269-b5a9-b569ee994756")].SubQueries = subQueryDictionary[new Guid("fa78f61e-6393-4269-b5a9-b569ee994756")];}
			if(!subQueryDictionary.ContainsKey(new Guid("07d79609-0db1-4b33-bfa7-77ab7f1aee33"))) {subQueryDictionary[new Guid("07d79609-0db1-4b33-bfa7-77ab7f1aee33")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("07d79609-0db1-4b33-bfa7-77ab7f1aee33")].Add(queryDictionary[new Guid("fa78f61e-6393-4269-b5a9-b569ee994756")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("07d79609-0db1-4b33-bfa7-77ab7f1aee33"))) {queryDictionary[new Guid("07d79609-0db1-4b33-bfa7-77ab7f1aee33")].SubQueries = subQueryDictionary[new Guid("07d79609-0db1-4b33-bfa7-77ab7f1aee33")];}
		if(!subQueryDictionary.ContainsKey(new Guid("e891c21a-219d-410e-97d2-12628fc10888"))) {subQueryDictionary[new Guid("e891c21a-219d-410e-97d2-12628fc10888")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("e891c21a-219d-410e-97d2-12628fc10888")].Add(queryDictionary[new Guid("07d79609-0db1-4b33-bfa7-77ab7f1aee33")]);
		}
		{
		queryDictionary[new Guid("8f5a367c-6229-49d1-abd6-be07f8fae403")] = new InputRecordListQuery();
		queryDictionary[new Guid("8f5a367c-6229-49d1-abd6-be07f8fae403")].FieldName = null;
		queryDictionary[new Guid("8f5a367c-6229-49d1-abd6-be07f8fae403")].FieldValue =  null;
		queryDictionary[new Guid("8f5a367c-6229-49d1-abd6-be07f8fae403")].QueryType = "AND";
		queryDictionary[new Guid("8f5a367c-6229-49d1-abd6-be07f8fae403")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("8fd4fc61-c4dd-435d-80d1-ba9374b75096")] = new InputRecordListQuery();
			queryDictionary[new Guid("8fd4fc61-c4dd-435d-80d1-ba9374b75096")].FieldName = "code";
			queryDictionary[new Guid("8fd4fc61-c4dd-435d-80d1-ba9374b75096")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("8fd4fc61-c4dd-435d-80d1-ba9374b75096")].QueryType = "CONTAINS";
			queryDictionary[new Guid("8fd4fc61-c4dd-435d-80d1-ba9374b75096")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("8fd4fc61-c4dd-435d-80d1-ba9374b75096"))) {queryDictionary[new Guid("8fd4fc61-c4dd-435d-80d1-ba9374b75096")].SubQueries = subQueryDictionary[new Guid("8fd4fc61-c4dd-435d-80d1-ba9374b75096")];}
			if(!subQueryDictionary.ContainsKey(new Guid("8f5a367c-6229-49d1-abd6-be07f8fae403"))) {subQueryDictionary[new Guid("8f5a367c-6229-49d1-abd6-be07f8fae403")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("8f5a367c-6229-49d1-abd6-be07f8fae403")].Add(queryDictionary[new Guid("8fd4fc61-c4dd-435d-80d1-ba9374b75096")]);
			}
			{
			queryDictionary[new Guid("69b5d234-dffe-426c-b131-51a7582aaf01")] = new InputRecordListQuery();
			queryDictionary[new Guid("69b5d234-dffe-426c-b131-51a7582aaf01")].FieldName = "subject";
			queryDictionary[new Guid("69b5d234-dffe-426c-b131-51a7582aaf01")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("69b5d234-dffe-426c-b131-51a7582aaf01")].QueryType = "CONTAINS";
			queryDictionary[new Guid("69b5d234-dffe-426c-b131-51a7582aaf01")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("69b5d234-dffe-426c-b131-51a7582aaf01"))) {queryDictionary[new Guid("69b5d234-dffe-426c-b131-51a7582aaf01")].SubQueries = subQueryDictionary[new Guid("69b5d234-dffe-426c-b131-51a7582aaf01")];}
			if(!subQueryDictionary.ContainsKey(new Guid("8f5a367c-6229-49d1-abd6-be07f8fae403"))) {subQueryDictionary[new Guid("8f5a367c-6229-49d1-abd6-be07f8fae403")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("8f5a367c-6229-49d1-abd6-be07f8fae403")].Add(queryDictionary[new Guid("69b5d234-dffe-426c-b131-51a7582aaf01")]);
			}
			{
			queryDictionary[new Guid("3e501b07-9fd0-4a2c-97e2-724a36a4a106")] = new InputRecordListQuery();
			queryDictionary[new Guid("3e501b07-9fd0-4a2c-97e2-724a36a4a106")].FieldName = "status";
			queryDictionary[new Guid("3e501b07-9fd0-4a2c-97e2-724a36a4a106")].FieldValue =  "completed";
			queryDictionary[new Guid("3e501b07-9fd0-4a2c-97e2-724a36a4a106")].QueryType = "NOT";
			queryDictionary[new Guid("3e501b07-9fd0-4a2c-97e2-724a36a4a106")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("3e501b07-9fd0-4a2c-97e2-724a36a4a106"))) {queryDictionary[new Guid("3e501b07-9fd0-4a2c-97e2-724a36a4a106")].SubQueries = subQueryDictionary[new Guid("3e501b07-9fd0-4a2c-97e2-724a36a4a106")];}
			if(!subQueryDictionary.ContainsKey(new Guid("8f5a367c-6229-49d1-abd6-be07f8fae403"))) {subQueryDictionary[new Guid("8f5a367c-6229-49d1-abd6-be07f8fae403")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("8f5a367c-6229-49d1-abd6-be07f8fae403")].Add(queryDictionary[new Guid("3e501b07-9fd0-4a2c-97e2-724a36a4a106")]);
			}
			{
			queryDictionary[new Guid("4623533a-0516-4ba1-81c9-62ac2b200035")] = new InputRecordListQuery();
			queryDictionary[new Guid("4623533a-0516-4ba1-81c9-62ac2b200035")].FieldName = "priority";
			queryDictionary[new Guid("4623533a-0516-4ba1-81c9-62ac2b200035")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("4623533a-0516-4ba1-81c9-62ac2b200035")].QueryType = "EQ";
			queryDictionary[new Guid("4623533a-0516-4ba1-81c9-62ac2b200035")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("4623533a-0516-4ba1-81c9-62ac2b200035"))) {queryDictionary[new Guid("4623533a-0516-4ba1-81c9-62ac2b200035")].SubQueries = subQueryDictionary[new Guid("4623533a-0516-4ba1-81c9-62ac2b200035")];}
			if(!subQueryDictionary.ContainsKey(new Guid("8f5a367c-6229-49d1-abd6-be07f8fae403"))) {subQueryDictionary[new Guid("8f5a367c-6229-49d1-abd6-be07f8fae403")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("8f5a367c-6229-49d1-abd6-be07f8fae403")].Add(queryDictionary[new Guid("4623533a-0516-4ba1-81c9-62ac2b200035")]);
			}
			{
			queryDictionary[new Guid("c7f96577-4dac-47e2-bc88-9ee193394c5b")] = new InputRecordListQuery();
			queryDictionary[new Guid("c7f96577-4dac-47e2-bc88-9ee193394c5b")].FieldName = "fts";
			queryDictionary[new Guid("c7f96577-4dac-47e2-bc88-9ee193394c5b")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"fts\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("c7f96577-4dac-47e2-bc88-9ee193394c5b")].QueryType = "FTS";
			queryDictionary[new Guid("c7f96577-4dac-47e2-bc88-9ee193394c5b")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("c7f96577-4dac-47e2-bc88-9ee193394c5b"))) {queryDictionary[new Guid("c7f96577-4dac-47e2-bc88-9ee193394c5b")].SubQueries = subQueryDictionary[new Guid("c7f96577-4dac-47e2-bc88-9ee193394c5b")];}
			if(!subQueryDictionary.ContainsKey(new Guid("8f5a367c-6229-49d1-abd6-be07f8fae403"))) {subQueryDictionary[new Guid("8f5a367c-6229-49d1-abd6-be07f8fae403")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("8f5a367c-6229-49d1-abd6-be07f8fae403")].Add(queryDictionary[new Guid("c7f96577-4dac-47e2-bc88-9ee193394c5b")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("8f5a367c-6229-49d1-abd6-be07f8fae403"))) {queryDictionary[new Guid("8f5a367c-6229-49d1-abd6-be07f8fae403")].SubQueries = subQueryDictionary[new Guid("8f5a367c-6229-49d1-abd6-be07f8fae403")];}
		if(!subQueryDictionary.ContainsKey(new Guid("e891c21a-219d-410e-97d2-12628fc10888"))) {subQueryDictionary[new Guid("e891c21a-219d-410e-97d2-12628fc10888")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("e891c21a-219d-410e-97d2-12628fc10888")].Add(queryDictionary[new Guid("8f5a367c-6229-49d1-abd6-be07f8fae403")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("e891c21a-219d-410e-97d2-12628fc10888"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("e891c21a-219d-410e-97d2-12628fc10888")];}
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

#region << ***Update list***  Entity: wv_task List Name: admin >>
{
	var createListEntity = entMan.ReadEntity(new Guid("65acced0-1650-4ff0-bbff-9937c382cd89")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "admin").Id;
	createListInput.Type =  "Hidden";
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
			actionItem.Template = @"<a class=""btn btn-default btn-outline btn-sm"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record, ngCtrl)}}"">
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
		queryDictionary[new Guid("5ffe152a-e226-415b-928b-91450f940f8c")] = new InputRecordListQuery();
		queryDictionary[new Guid("5ffe152a-e226-415b-928b-91450f940f8c")].FieldName = "owner_id";
		queryDictionary[new Guid("5ffe152a-e226-415b-928b-91450f940f8c")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("5ffe152a-e226-415b-928b-91450f940f8c")].QueryType = "EQ";
		queryDictionary[new Guid("5ffe152a-e226-415b-928b-91450f940f8c")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("5ffe152a-e226-415b-928b-91450f940f8c"))) {queryDictionary[new Guid("5ffe152a-e226-415b-928b-91450f940f8c")].SubQueries = subQueryDictionary[new Guid("5ffe152a-e226-415b-928b-91450f940f8c")];}
		if(!subQueryDictionary.ContainsKey(new Guid("edefa95a-02b0-41b6-90d9-a8f33855c7ba"))) {subQueryDictionary[new Guid("edefa95a-02b0-41b6-90d9-a8f33855c7ba")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("edefa95a-02b0-41b6-90d9-a8f33855c7ba")].Add(queryDictionary[new Guid("5ffe152a-e226-415b-928b-91450f940f8c")]);
		}
		{
		queryDictionary[new Guid("c2253e91-29cc-43b5-bbcd-50da55228f9c")] = new InputRecordListQuery();
		queryDictionary[new Guid("c2253e91-29cc-43b5-bbcd-50da55228f9c")].FieldName = "subject";
		queryDictionary[new Guid("c2253e91-29cc-43b5-bbcd-50da55228f9c")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("c2253e91-29cc-43b5-bbcd-50da55228f9c")].QueryType = "FTS";
		queryDictionary[new Guid("c2253e91-29cc-43b5-bbcd-50da55228f9c")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("c2253e91-29cc-43b5-bbcd-50da55228f9c"))) {queryDictionary[new Guid("c2253e91-29cc-43b5-bbcd-50da55228f9c")].SubQueries = subQueryDictionary[new Guid("c2253e91-29cc-43b5-bbcd-50da55228f9c")];}
		if(!subQueryDictionary.ContainsKey(new Guid("edefa95a-02b0-41b6-90d9-a8f33855c7ba"))) {subQueryDictionary[new Guid("edefa95a-02b0-41b6-90d9-a8f33855c7ba")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("edefa95a-02b0-41b6-90d9-a8f33855c7ba")].Add(queryDictionary[new Guid("c2253e91-29cc-43b5-bbcd-50da55228f9c")]);
		}
		{
		queryDictionary[new Guid("6469fb69-5d3a-495f-868f-a2d2bcdc9658")] = new InputRecordListQuery();
		queryDictionary[new Guid("6469fb69-5d3a-495f-868f-a2d2bcdc9658")].FieldName = "status";
		queryDictionary[new Guid("6469fb69-5d3a-495f-868f-a2d2bcdc9658")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("6469fb69-5d3a-495f-868f-a2d2bcdc9658")].QueryType = "EQ";
		queryDictionary[new Guid("6469fb69-5d3a-495f-868f-a2d2bcdc9658")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("6469fb69-5d3a-495f-868f-a2d2bcdc9658"))) {queryDictionary[new Guid("6469fb69-5d3a-495f-868f-a2d2bcdc9658")].SubQueries = subQueryDictionary[new Guid("6469fb69-5d3a-495f-868f-a2d2bcdc9658")];}
		if(!subQueryDictionary.ContainsKey(new Guid("edefa95a-02b0-41b6-90d9-a8f33855c7ba"))) {subQueryDictionary[new Guid("edefa95a-02b0-41b6-90d9-a8f33855c7ba")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("edefa95a-02b0-41b6-90d9-a8f33855c7ba")].Add(queryDictionary[new Guid("6469fb69-5d3a-495f-868f-a2d2bcdc9658")]);
		}
		{
		queryDictionary[new Guid("138835bd-77a5-41e0-931c-9ebba1083cf3")] = new InputRecordListQuery();
		queryDictionary[new Guid("138835bd-77a5-41e0-931c-9ebba1083cf3")].FieldName = "priority";
		queryDictionary[new Guid("138835bd-77a5-41e0-931c-9ebba1083cf3")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("138835bd-77a5-41e0-931c-9ebba1083cf3")].QueryType = "EQ";
		queryDictionary[new Guid("138835bd-77a5-41e0-931c-9ebba1083cf3")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("138835bd-77a5-41e0-931c-9ebba1083cf3"))) {queryDictionary[new Guid("138835bd-77a5-41e0-931c-9ebba1083cf3")].SubQueries = subQueryDictionary[new Guid("138835bd-77a5-41e0-931c-9ebba1083cf3")];}
		if(!subQueryDictionary.ContainsKey(new Guid("edefa95a-02b0-41b6-90d9-a8f33855c7ba"))) {subQueryDictionary[new Guid("edefa95a-02b0-41b6-90d9-a8f33855c7ba")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("edefa95a-02b0-41b6-90d9-a8f33855c7ba")].Add(queryDictionary[new Guid("138835bd-77a5-41e0-931c-9ebba1083cf3")]);
		}
		{
		queryDictionary[new Guid("37664ad9-47a1-4947-96cc-fbfc9494e089")] = new InputRecordListQuery();
		queryDictionary[new Guid("37664ad9-47a1-4947-96cc-fbfc9494e089")].FieldName = "fts";
		queryDictionary[new Guid("37664ad9-47a1-4947-96cc-fbfc9494e089")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"fts\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("37664ad9-47a1-4947-96cc-fbfc9494e089")].QueryType = "FTS";
		queryDictionary[new Guid("37664ad9-47a1-4947-96cc-fbfc9494e089")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("37664ad9-47a1-4947-96cc-fbfc9494e089"))) {queryDictionary[new Guid("37664ad9-47a1-4947-96cc-fbfc9494e089")].SubQueries = subQueryDictionary[new Guid("37664ad9-47a1-4947-96cc-fbfc9494e089")];}
		if(!subQueryDictionary.ContainsKey(new Guid("edefa95a-02b0-41b6-90d9-a8f33855c7ba"))) {subQueryDictionary[new Guid("edefa95a-02b0-41b6-90d9-a8f33855c7ba")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("edefa95a-02b0-41b6-90d9-a8f33855c7ba")].Add(queryDictionary[new Guid("37664ad9-47a1-4947-96cc-fbfc9494e089")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("edefa95a-02b0-41b6-90d9-a8f33855c7ba"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("edefa95a-02b0-41b6-90d9-a8f33855c7ba")];}
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
	createListInput.ColumnWidthsCSV = "100px,auto,120px,120px,120px,120px,120px";
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
			actionItem.Template = @"<a class=""btn btn-default btn-outline btn-sm"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record, ngCtrl)}}"">
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
		#region << field from Relation: username >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("263c0b21-88c1-4c2b-80b4-db7402b0d2e2");
			listItemFromRelation.FieldName = "username";
			listItemFromRelation.FieldLabel = "owner";
			listItemFromRelation.FieldPlaceholder = "";
			listItemFromRelation.FieldHelpText = "";
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = "";
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
		queryDictionary[new Guid("ae4c3154-d6b5-4b4e-9128-ce1ecd04afe8")] = new InputRecordListQuery();
		queryDictionary[new Guid("ae4c3154-d6b5-4b4e-9128-ce1ecd04afe8")].FieldName = "code";
		queryDictionary[new Guid("ae4c3154-d6b5-4b4e-9128-ce1ecd04afe8")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("ae4c3154-d6b5-4b4e-9128-ce1ecd04afe8")].QueryType = "CONTAINS";
		queryDictionary[new Guid("ae4c3154-d6b5-4b4e-9128-ce1ecd04afe8")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("ae4c3154-d6b5-4b4e-9128-ce1ecd04afe8"))) {queryDictionary[new Guid("ae4c3154-d6b5-4b4e-9128-ce1ecd04afe8")].SubQueries = subQueryDictionary[new Guid("ae4c3154-d6b5-4b4e-9128-ce1ecd04afe8")];}
		if(!subQueryDictionary.ContainsKey(new Guid("daa892c2-f26b-4b28-9b8f-32de2adbb6bb"))) {subQueryDictionary[new Guid("daa892c2-f26b-4b28-9b8f-32de2adbb6bb")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("daa892c2-f26b-4b28-9b8f-32de2adbb6bb")].Add(queryDictionary[new Guid("ae4c3154-d6b5-4b4e-9128-ce1ecd04afe8")]);
		}
		{
		queryDictionary[new Guid("5245e400-ea32-427a-b655-9336267820c0")] = new InputRecordListQuery();
		queryDictionary[new Guid("5245e400-ea32-427a-b655-9336267820c0")].FieldName = "subject";
		queryDictionary[new Guid("5245e400-ea32-427a-b655-9336267820c0")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("5245e400-ea32-427a-b655-9336267820c0")].QueryType = "CONTAINS";
		queryDictionary[new Guid("5245e400-ea32-427a-b655-9336267820c0")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("5245e400-ea32-427a-b655-9336267820c0"))) {queryDictionary[new Guid("5245e400-ea32-427a-b655-9336267820c0")].SubQueries = subQueryDictionary[new Guid("5245e400-ea32-427a-b655-9336267820c0")];}
		if(!subQueryDictionary.ContainsKey(new Guid("daa892c2-f26b-4b28-9b8f-32de2adbb6bb"))) {subQueryDictionary[new Guid("daa892c2-f26b-4b28-9b8f-32de2adbb6bb")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("daa892c2-f26b-4b28-9b8f-32de2adbb6bb")].Add(queryDictionary[new Guid("5245e400-ea32-427a-b655-9336267820c0")]);
		}
		{
		queryDictionary[new Guid("dbde71f0-2555-4d8e-bed4-b396103cdc3b")] = new InputRecordListQuery();
		queryDictionary[new Guid("dbde71f0-2555-4d8e-bed4-b396103cdc3b")].FieldName = "status";
		queryDictionary[new Guid("dbde71f0-2555-4d8e-bed4-b396103cdc3b")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("dbde71f0-2555-4d8e-bed4-b396103cdc3b")].QueryType = "EQ";
		queryDictionary[new Guid("dbde71f0-2555-4d8e-bed4-b396103cdc3b")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("dbde71f0-2555-4d8e-bed4-b396103cdc3b"))) {queryDictionary[new Guid("dbde71f0-2555-4d8e-bed4-b396103cdc3b")].SubQueries = subQueryDictionary[new Guid("dbde71f0-2555-4d8e-bed4-b396103cdc3b")];}
		if(!subQueryDictionary.ContainsKey(new Guid("daa892c2-f26b-4b28-9b8f-32de2adbb6bb"))) {subQueryDictionary[new Guid("daa892c2-f26b-4b28-9b8f-32de2adbb6bb")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("daa892c2-f26b-4b28-9b8f-32de2adbb6bb")].Add(queryDictionary[new Guid("dbde71f0-2555-4d8e-bed4-b396103cdc3b")]);
		}
		{
		queryDictionary[new Guid("2cabacec-4f1f-42c0-b99d-c0add8443e56")] = new InputRecordListQuery();
		queryDictionary[new Guid("2cabacec-4f1f-42c0-b99d-c0add8443e56")].FieldName = "priority";
		queryDictionary[new Guid("2cabacec-4f1f-42c0-b99d-c0add8443e56")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("2cabacec-4f1f-42c0-b99d-c0add8443e56")].QueryType = "EQ";
		queryDictionary[new Guid("2cabacec-4f1f-42c0-b99d-c0add8443e56")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("2cabacec-4f1f-42c0-b99d-c0add8443e56"))) {queryDictionary[new Guid("2cabacec-4f1f-42c0-b99d-c0add8443e56")].SubQueries = subQueryDictionary[new Guid("2cabacec-4f1f-42c0-b99d-c0add8443e56")];}
		if(!subQueryDictionary.ContainsKey(new Guid("daa892c2-f26b-4b28-9b8f-32de2adbb6bb"))) {subQueryDictionary[new Guid("daa892c2-f26b-4b28-9b8f-32de2adbb6bb")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("daa892c2-f26b-4b28-9b8f-32de2adbb6bb")].Add(queryDictionary[new Guid("2cabacec-4f1f-42c0-b99d-c0add8443e56")]);
		}
		{
		queryDictionary[new Guid("6931bb93-e25f-434d-bcf5-cd381accc8da")] = new InputRecordListQuery();
		queryDictionary[new Guid("6931bb93-e25f-434d-bcf5-cd381accc8da")].FieldName = "$user_1_n_task_owner.username";
		queryDictionary[new Guid("6931bb93-e25f-434d-bcf5-cd381accc8da")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"$user_1_n_task_owner.username\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("6931bb93-e25f-434d-bcf5-cd381accc8da")].QueryType = "CONTAINS";
		queryDictionary[new Guid("6931bb93-e25f-434d-bcf5-cd381accc8da")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("6931bb93-e25f-434d-bcf5-cd381accc8da"))) {queryDictionary[new Guid("6931bb93-e25f-434d-bcf5-cd381accc8da")].SubQueries = subQueryDictionary[new Guid("6931bb93-e25f-434d-bcf5-cd381accc8da")];}
		if(!subQueryDictionary.ContainsKey(new Guid("daa892c2-f26b-4b28-9b8f-32de2adbb6bb"))) {subQueryDictionary[new Guid("daa892c2-f26b-4b28-9b8f-32de2adbb6bb")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("daa892c2-f26b-4b28-9b8f-32de2adbb6bb")].Add(queryDictionary[new Guid("6931bb93-e25f-434d-bcf5-cd381accc8da")]);
		}
		{
		queryDictionary[new Guid("a4b6ecd4-27ea-4f7b-8578-084d754f4035")] = new InputRecordListQuery();
		queryDictionary[new Guid("a4b6ecd4-27ea-4f7b-8578-084d754f4035")].FieldName = "fts";
		queryDictionary[new Guid("a4b6ecd4-27ea-4f7b-8578-084d754f4035")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"fts\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("a4b6ecd4-27ea-4f7b-8578-084d754f4035")].QueryType = "FTS";
		queryDictionary[new Guid("a4b6ecd4-27ea-4f7b-8578-084d754f4035")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("a4b6ecd4-27ea-4f7b-8578-084d754f4035"))) {queryDictionary[new Guid("a4b6ecd4-27ea-4f7b-8578-084d754f4035")].SubQueries = subQueryDictionary[new Guid("a4b6ecd4-27ea-4f7b-8578-084d754f4035")];}
		if(!subQueryDictionary.ContainsKey(new Guid("daa892c2-f26b-4b28-9b8f-32de2adbb6bb"))) {subQueryDictionary[new Guid("daa892c2-f26b-4b28-9b8f-32de2adbb6bb")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("daa892c2-f26b-4b28-9b8f-32de2adbb6bb")].Add(queryDictionary[new Guid("a4b6ecd4-27ea-4f7b-8578-084d754f4035")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("daa892c2-f26b-4b28-9b8f-32de2adbb6bb"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("daa892c2-f26b-4b28-9b8f-32de2adbb6bb")];}
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
	createListInput.ColumnWidthsCSV = "100px,auto,120px,120px,120px,120px,120px";
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
			actionItem.Template = @"<a class=""btn btn-default btn-outline btn-sm"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record, ngCtrl)}}"">
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
		#region << field from Relation: username >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("263c0b21-88c1-4c2b-80b4-db7402b0d2e2");
			listItemFromRelation.FieldName = "username";
			listItemFromRelation.FieldLabel = "owner";
			listItemFromRelation.FieldPlaceholder = "";
			listItemFromRelation.FieldHelpText = "";
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = "";
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
		queryDictionary[new Guid("051aa0fa-07f7-4129-9431-be150c063d41")] = new InputRecordListQuery();
		queryDictionary[new Guid("051aa0fa-07f7-4129-9431-be150c063d41")].FieldName = null;
		queryDictionary[new Guid("051aa0fa-07f7-4129-9431-be150c063d41")].FieldValue =  null;
		queryDictionary[new Guid("051aa0fa-07f7-4129-9431-be150c063d41")].QueryType = "AND";
		queryDictionary[new Guid("051aa0fa-07f7-4129-9431-be150c063d41")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("dcd58291-495d-46a6-86ec-6e8cc15be9e2")] = new InputRecordListQuery();
			queryDictionary[new Guid("dcd58291-495d-46a6-86ec-6e8cc15be9e2")].FieldName = "created_by";
			queryDictionary[new Guid("dcd58291-495d-46a6-86ec-6e8cc15be9e2")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("dcd58291-495d-46a6-86ec-6e8cc15be9e2")].QueryType = "EQ";
			queryDictionary[new Guid("dcd58291-495d-46a6-86ec-6e8cc15be9e2")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("dcd58291-495d-46a6-86ec-6e8cc15be9e2"))) {queryDictionary[new Guid("dcd58291-495d-46a6-86ec-6e8cc15be9e2")].SubQueries = subQueryDictionary[new Guid("dcd58291-495d-46a6-86ec-6e8cc15be9e2")];}
			if(!subQueryDictionary.ContainsKey(new Guid("051aa0fa-07f7-4129-9431-be150c063d41"))) {subQueryDictionary[new Guid("051aa0fa-07f7-4129-9431-be150c063d41")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("051aa0fa-07f7-4129-9431-be150c063d41")].Add(queryDictionary[new Guid("dcd58291-495d-46a6-86ec-6e8cc15be9e2")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("051aa0fa-07f7-4129-9431-be150c063d41"))) {queryDictionary[new Guid("051aa0fa-07f7-4129-9431-be150c063d41")].SubQueries = subQueryDictionary[new Guid("051aa0fa-07f7-4129-9431-be150c063d41")];}
		if(!subQueryDictionary.ContainsKey(new Guid("619a0fd0-5886-42c9-8ba3-7a834a281736"))) {subQueryDictionary[new Guid("619a0fd0-5886-42c9-8ba3-7a834a281736")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("619a0fd0-5886-42c9-8ba3-7a834a281736")].Add(queryDictionary[new Guid("051aa0fa-07f7-4129-9431-be150c063d41")]);
		}
		{
		queryDictionary[new Guid("92f0fcd6-4ed2-4f92-b868-709c2c4735f9")] = new InputRecordListQuery();
		queryDictionary[new Guid("92f0fcd6-4ed2-4f92-b868-709c2c4735f9")].FieldName = null;
		queryDictionary[new Guid("92f0fcd6-4ed2-4f92-b868-709c2c4735f9")].FieldValue =  null;
		queryDictionary[new Guid("92f0fcd6-4ed2-4f92-b868-709c2c4735f9")].QueryType = "AND";
		queryDictionary[new Guid("92f0fcd6-4ed2-4f92-b868-709c2c4735f9")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("10dfcc42-64f4-4924-a589-b5b9319454ac")] = new InputRecordListQuery();
			queryDictionary[new Guid("10dfcc42-64f4-4924-a589-b5b9319454ac")].FieldName = "code";
			queryDictionary[new Guid("10dfcc42-64f4-4924-a589-b5b9319454ac")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("10dfcc42-64f4-4924-a589-b5b9319454ac")].QueryType = "CONTAINS";
			queryDictionary[new Guid("10dfcc42-64f4-4924-a589-b5b9319454ac")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("10dfcc42-64f4-4924-a589-b5b9319454ac"))) {queryDictionary[new Guid("10dfcc42-64f4-4924-a589-b5b9319454ac")].SubQueries = subQueryDictionary[new Guid("10dfcc42-64f4-4924-a589-b5b9319454ac")];}
			if(!subQueryDictionary.ContainsKey(new Guid("92f0fcd6-4ed2-4f92-b868-709c2c4735f9"))) {subQueryDictionary[new Guid("92f0fcd6-4ed2-4f92-b868-709c2c4735f9")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("92f0fcd6-4ed2-4f92-b868-709c2c4735f9")].Add(queryDictionary[new Guid("10dfcc42-64f4-4924-a589-b5b9319454ac")]);
			}
			{
			queryDictionary[new Guid("b6b0f627-5727-4fc5-aa93-999e004e0f34")] = new InputRecordListQuery();
			queryDictionary[new Guid("b6b0f627-5727-4fc5-aa93-999e004e0f34")].FieldName = "subject";
			queryDictionary[new Guid("b6b0f627-5727-4fc5-aa93-999e004e0f34")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("b6b0f627-5727-4fc5-aa93-999e004e0f34")].QueryType = "CONTAINS";
			queryDictionary[new Guid("b6b0f627-5727-4fc5-aa93-999e004e0f34")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("b6b0f627-5727-4fc5-aa93-999e004e0f34"))) {queryDictionary[new Guid("b6b0f627-5727-4fc5-aa93-999e004e0f34")].SubQueries = subQueryDictionary[new Guid("b6b0f627-5727-4fc5-aa93-999e004e0f34")];}
			if(!subQueryDictionary.ContainsKey(new Guid("92f0fcd6-4ed2-4f92-b868-709c2c4735f9"))) {subQueryDictionary[new Guid("92f0fcd6-4ed2-4f92-b868-709c2c4735f9")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("92f0fcd6-4ed2-4f92-b868-709c2c4735f9")].Add(queryDictionary[new Guid("b6b0f627-5727-4fc5-aa93-999e004e0f34")]);
			}
			{
			queryDictionary[new Guid("fd370069-cd72-4a47-9c63-16bbdb938de2")] = new InputRecordListQuery();
			queryDictionary[new Guid("fd370069-cd72-4a47-9c63-16bbdb938de2")].FieldName = "status";
			queryDictionary[new Guid("fd370069-cd72-4a47-9c63-16bbdb938de2")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("fd370069-cd72-4a47-9c63-16bbdb938de2")].QueryType = "EQ";
			queryDictionary[new Guid("fd370069-cd72-4a47-9c63-16bbdb938de2")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("fd370069-cd72-4a47-9c63-16bbdb938de2"))) {queryDictionary[new Guid("fd370069-cd72-4a47-9c63-16bbdb938de2")].SubQueries = subQueryDictionary[new Guid("fd370069-cd72-4a47-9c63-16bbdb938de2")];}
			if(!subQueryDictionary.ContainsKey(new Guid("92f0fcd6-4ed2-4f92-b868-709c2c4735f9"))) {subQueryDictionary[new Guid("92f0fcd6-4ed2-4f92-b868-709c2c4735f9")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("92f0fcd6-4ed2-4f92-b868-709c2c4735f9")].Add(queryDictionary[new Guid("fd370069-cd72-4a47-9c63-16bbdb938de2")]);
			}
			{
			queryDictionary[new Guid("75c39134-4b3d-4161-88c6-dfc2324ef143")] = new InputRecordListQuery();
			queryDictionary[new Guid("75c39134-4b3d-4161-88c6-dfc2324ef143")].FieldName = "priority";
			queryDictionary[new Guid("75c39134-4b3d-4161-88c6-dfc2324ef143")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("75c39134-4b3d-4161-88c6-dfc2324ef143")].QueryType = "EQ";
			queryDictionary[new Guid("75c39134-4b3d-4161-88c6-dfc2324ef143")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("75c39134-4b3d-4161-88c6-dfc2324ef143"))) {queryDictionary[new Guid("75c39134-4b3d-4161-88c6-dfc2324ef143")].SubQueries = subQueryDictionary[new Guid("75c39134-4b3d-4161-88c6-dfc2324ef143")];}
			if(!subQueryDictionary.ContainsKey(new Guid("92f0fcd6-4ed2-4f92-b868-709c2c4735f9"))) {subQueryDictionary[new Guid("92f0fcd6-4ed2-4f92-b868-709c2c4735f9")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("92f0fcd6-4ed2-4f92-b868-709c2c4735f9")].Add(queryDictionary[new Guid("75c39134-4b3d-4161-88c6-dfc2324ef143")]);
			}
			{
			queryDictionary[new Guid("49c597fd-7d98-4e32-a65e-a54fac16a2f6")] = new InputRecordListQuery();
			queryDictionary[new Guid("49c597fd-7d98-4e32-a65e-a54fac16a2f6")].FieldName = "$user_1_n_task_owner.username";
			queryDictionary[new Guid("49c597fd-7d98-4e32-a65e-a54fac16a2f6")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"$user_1_n_task_owner.username\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("49c597fd-7d98-4e32-a65e-a54fac16a2f6")].QueryType = "CONTAINS";
			queryDictionary[new Guid("49c597fd-7d98-4e32-a65e-a54fac16a2f6")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("49c597fd-7d98-4e32-a65e-a54fac16a2f6"))) {queryDictionary[new Guid("49c597fd-7d98-4e32-a65e-a54fac16a2f6")].SubQueries = subQueryDictionary[new Guid("49c597fd-7d98-4e32-a65e-a54fac16a2f6")];}
			if(!subQueryDictionary.ContainsKey(new Guid("92f0fcd6-4ed2-4f92-b868-709c2c4735f9"))) {subQueryDictionary[new Guid("92f0fcd6-4ed2-4f92-b868-709c2c4735f9")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("92f0fcd6-4ed2-4f92-b868-709c2c4735f9")].Add(queryDictionary[new Guid("49c597fd-7d98-4e32-a65e-a54fac16a2f6")]);
			}
			{
			queryDictionary[new Guid("60f663ac-a77a-4a83-8734-a77bb41ae37e")] = new InputRecordListQuery();
			queryDictionary[new Guid("60f663ac-a77a-4a83-8734-a77bb41ae37e")].FieldName = "fts";
			queryDictionary[new Guid("60f663ac-a77a-4a83-8734-a77bb41ae37e")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"fts\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("60f663ac-a77a-4a83-8734-a77bb41ae37e")].QueryType = "FTS";
			queryDictionary[new Guid("60f663ac-a77a-4a83-8734-a77bb41ae37e")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("60f663ac-a77a-4a83-8734-a77bb41ae37e"))) {queryDictionary[new Guid("60f663ac-a77a-4a83-8734-a77bb41ae37e")].SubQueries = subQueryDictionary[new Guid("60f663ac-a77a-4a83-8734-a77bb41ae37e")];}
			if(!subQueryDictionary.ContainsKey(new Guid("92f0fcd6-4ed2-4f92-b868-709c2c4735f9"))) {subQueryDictionary[new Guid("92f0fcd6-4ed2-4f92-b868-709c2c4735f9")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("92f0fcd6-4ed2-4f92-b868-709c2c4735f9")].Add(queryDictionary[new Guid("60f663ac-a77a-4a83-8734-a77bb41ae37e")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("92f0fcd6-4ed2-4f92-b868-709c2c4735f9"))) {queryDictionary[new Guid("92f0fcd6-4ed2-4f92-b868-709c2c4735f9")].SubQueries = subQueryDictionary[new Guid("92f0fcd6-4ed2-4f92-b868-709c2c4735f9")];}
		if(!subQueryDictionary.ContainsKey(new Guid("619a0fd0-5886-42c9-8ba3-7a834a281736"))) {subQueryDictionary[new Guid("619a0fd0-5886-42c9-8ba3-7a834a281736")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("619a0fd0-5886-42c9-8ba3-7a834a281736")].Add(queryDictionary[new Guid("92f0fcd6-4ed2-4f92-b868-709c2c4735f9")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("619a0fd0-5886-42c9-8ba3-7a834a281736"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("619a0fd0-5886-42c9-8ba3-7a834a281736")];}
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
	createListInput.ColumnWidthsCSV = "100px,auto,120px,120px,120px,120px";
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
			actionItem.Template = @"<a class=""btn btn-default btn-outline btn-sm"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record, ngCtrl)}}"">
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
		queryDictionary[new Guid("c6c3f0f4-1b5a-4c13-9001-02f5c7c38c43")] = new InputRecordListQuery();
		queryDictionary[new Guid("c6c3f0f4-1b5a-4c13-9001-02f5c7c38c43")].FieldName = null;
		queryDictionary[new Guid("c6c3f0f4-1b5a-4c13-9001-02f5c7c38c43")].FieldValue =  null;
		queryDictionary[new Guid("c6c3f0f4-1b5a-4c13-9001-02f5c7c38c43")].QueryType = "OR";
		queryDictionary[new Guid("c6c3f0f4-1b5a-4c13-9001-02f5c7c38c43")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("4a2f87f7-0e1f-4375-a137-cd3ef93c2655")] = new InputRecordListQuery();
			queryDictionary[new Guid("4a2f87f7-0e1f-4375-a137-cd3ef93c2655")].FieldName = "owner_id";
			queryDictionary[new Guid("4a2f87f7-0e1f-4375-a137-cd3ef93c2655")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("4a2f87f7-0e1f-4375-a137-cd3ef93c2655")].QueryType = "EQ";
			queryDictionary[new Guid("4a2f87f7-0e1f-4375-a137-cd3ef93c2655")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("4a2f87f7-0e1f-4375-a137-cd3ef93c2655"))) {queryDictionary[new Guid("4a2f87f7-0e1f-4375-a137-cd3ef93c2655")].SubQueries = subQueryDictionary[new Guid("4a2f87f7-0e1f-4375-a137-cd3ef93c2655")];}
			if(!subQueryDictionary.ContainsKey(new Guid("c6c3f0f4-1b5a-4c13-9001-02f5c7c38c43"))) {subQueryDictionary[new Guid("c6c3f0f4-1b5a-4c13-9001-02f5c7c38c43")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("c6c3f0f4-1b5a-4c13-9001-02f5c7c38c43")].Add(queryDictionary[new Guid("4a2f87f7-0e1f-4375-a137-cd3ef93c2655")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("c6c3f0f4-1b5a-4c13-9001-02f5c7c38c43"))) {queryDictionary[new Guid("c6c3f0f4-1b5a-4c13-9001-02f5c7c38c43")].SubQueries = subQueryDictionary[new Guid("c6c3f0f4-1b5a-4c13-9001-02f5c7c38c43")];}
		if(!subQueryDictionary.ContainsKey(new Guid("21514d68-8938-43fd-9873-5e8cf5903790"))) {subQueryDictionary[new Guid("21514d68-8938-43fd-9873-5e8cf5903790")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("21514d68-8938-43fd-9873-5e8cf5903790")].Add(queryDictionary[new Guid("c6c3f0f4-1b5a-4c13-9001-02f5c7c38c43")]);
		}
		{
		queryDictionary[new Guid("12520365-001d-47a1-8ca6-ea2bd9ffd122")] = new InputRecordListQuery();
		queryDictionary[new Guid("12520365-001d-47a1-8ca6-ea2bd9ffd122")].FieldName = null;
		queryDictionary[new Guid("12520365-001d-47a1-8ca6-ea2bd9ffd122")].FieldValue =  null;
		queryDictionary[new Guid("12520365-001d-47a1-8ca6-ea2bd9ffd122")].QueryType = "AND";
		queryDictionary[new Guid("12520365-001d-47a1-8ca6-ea2bd9ffd122")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("915571d9-7858-4fb3-8e41-b3495a93b5df")] = new InputRecordListQuery();
			queryDictionary[new Guid("915571d9-7858-4fb3-8e41-b3495a93b5df")].FieldName = "code";
			queryDictionary[new Guid("915571d9-7858-4fb3-8e41-b3495a93b5df")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("915571d9-7858-4fb3-8e41-b3495a93b5df")].QueryType = "CONTAINS";
			queryDictionary[new Guid("915571d9-7858-4fb3-8e41-b3495a93b5df")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("915571d9-7858-4fb3-8e41-b3495a93b5df"))) {queryDictionary[new Guid("915571d9-7858-4fb3-8e41-b3495a93b5df")].SubQueries = subQueryDictionary[new Guid("915571d9-7858-4fb3-8e41-b3495a93b5df")];}
			if(!subQueryDictionary.ContainsKey(new Guid("12520365-001d-47a1-8ca6-ea2bd9ffd122"))) {subQueryDictionary[new Guid("12520365-001d-47a1-8ca6-ea2bd9ffd122")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("12520365-001d-47a1-8ca6-ea2bd9ffd122")].Add(queryDictionary[new Guid("915571d9-7858-4fb3-8e41-b3495a93b5df")]);
			}
			{
			queryDictionary[new Guid("7697059a-7049-46ac-b084-955e94994555")] = new InputRecordListQuery();
			queryDictionary[new Guid("7697059a-7049-46ac-b084-955e94994555")].FieldName = "subject";
			queryDictionary[new Guid("7697059a-7049-46ac-b084-955e94994555")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("7697059a-7049-46ac-b084-955e94994555")].QueryType = "CONTAINS";
			queryDictionary[new Guid("7697059a-7049-46ac-b084-955e94994555")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("7697059a-7049-46ac-b084-955e94994555"))) {queryDictionary[new Guid("7697059a-7049-46ac-b084-955e94994555")].SubQueries = subQueryDictionary[new Guid("7697059a-7049-46ac-b084-955e94994555")];}
			if(!subQueryDictionary.ContainsKey(new Guid("12520365-001d-47a1-8ca6-ea2bd9ffd122"))) {subQueryDictionary[new Guid("12520365-001d-47a1-8ca6-ea2bd9ffd122")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("12520365-001d-47a1-8ca6-ea2bd9ffd122")].Add(queryDictionary[new Guid("7697059a-7049-46ac-b084-955e94994555")]);
			}
			{
			queryDictionary[new Guid("efed6d2f-c739-4d4a-9e76-c6b8c1bff297")] = new InputRecordListQuery();
			queryDictionary[new Guid("efed6d2f-c739-4d4a-9e76-c6b8c1bff297")].FieldName = "status";
			queryDictionary[new Guid("efed6d2f-c739-4d4a-9e76-c6b8c1bff297")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("efed6d2f-c739-4d4a-9e76-c6b8c1bff297")].QueryType = "EQ";
			queryDictionary[new Guid("efed6d2f-c739-4d4a-9e76-c6b8c1bff297")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("efed6d2f-c739-4d4a-9e76-c6b8c1bff297"))) {queryDictionary[new Guid("efed6d2f-c739-4d4a-9e76-c6b8c1bff297")].SubQueries = subQueryDictionary[new Guid("efed6d2f-c739-4d4a-9e76-c6b8c1bff297")];}
			if(!subQueryDictionary.ContainsKey(new Guid("12520365-001d-47a1-8ca6-ea2bd9ffd122"))) {subQueryDictionary[new Guid("12520365-001d-47a1-8ca6-ea2bd9ffd122")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("12520365-001d-47a1-8ca6-ea2bd9ffd122")].Add(queryDictionary[new Guid("efed6d2f-c739-4d4a-9e76-c6b8c1bff297")]);
			}
			{
			queryDictionary[new Guid("aed37ba3-4e14-4319-94d6-6c6109d8c252")] = new InputRecordListQuery();
			queryDictionary[new Guid("aed37ba3-4e14-4319-94d6-6c6109d8c252")].FieldName = "priority";
			queryDictionary[new Guid("aed37ba3-4e14-4319-94d6-6c6109d8c252")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("aed37ba3-4e14-4319-94d6-6c6109d8c252")].QueryType = "EQ";
			queryDictionary[new Guid("aed37ba3-4e14-4319-94d6-6c6109d8c252")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("aed37ba3-4e14-4319-94d6-6c6109d8c252"))) {queryDictionary[new Guid("aed37ba3-4e14-4319-94d6-6c6109d8c252")].SubQueries = subQueryDictionary[new Guid("aed37ba3-4e14-4319-94d6-6c6109d8c252")];}
			if(!subQueryDictionary.ContainsKey(new Guid("12520365-001d-47a1-8ca6-ea2bd9ffd122"))) {subQueryDictionary[new Guid("12520365-001d-47a1-8ca6-ea2bd9ffd122")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("12520365-001d-47a1-8ca6-ea2bd9ffd122")].Add(queryDictionary[new Guid("aed37ba3-4e14-4319-94d6-6c6109d8c252")]);
			}
			{
			queryDictionary[new Guid("ac323844-471b-4ed9-a692-605a78545b0d")] = new InputRecordListQuery();
			queryDictionary[new Guid("ac323844-471b-4ed9-a692-605a78545b0d")].FieldName = "fts";
			queryDictionary[new Guid("ac323844-471b-4ed9-a692-605a78545b0d")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"fts\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("ac323844-471b-4ed9-a692-605a78545b0d")].QueryType = "FTS";
			queryDictionary[new Guid("ac323844-471b-4ed9-a692-605a78545b0d")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("ac323844-471b-4ed9-a692-605a78545b0d"))) {queryDictionary[new Guid("ac323844-471b-4ed9-a692-605a78545b0d")].SubQueries = subQueryDictionary[new Guid("ac323844-471b-4ed9-a692-605a78545b0d")];}
			if(!subQueryDictionary.ContainsKey(new Guid("12520365-001d-47a1-8ca6-ea2bd9ffd122"))) {subQueryDictionary[new Guid("12520365-001d-47a1-8ca6-ea2bd9ffd122")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("12520365-001d-47a1-8ca6-ea2bd9ffd122")].Add(queryDictionary[new Guid("ac323844-471b-4ed9-a692-605a78545b0d")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("12520365-001d-47a1-8ca6-ea2bd9ffd122"))) {queryDictionary[new Guid("12520365-001d-47a1-8ca6-ea2bd9ffd122")].SubQueries = subQueryDictionary[new Guid("12520365-001d-47a1-8ca6-ea2bd9ffd122")];}
		if(!subQueryDictionary.ContainsKey(new Guid("21514d68-8938-43fd-9873-5e8cf5903790"))) {subQueryDictionary[new Guid("21514d68-8938-43fd-9873-5e8cf5903790")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("21514d68-8938-43fd-9873-5e8cf5903790")].Add(queryDictionary[new Guid("12520365-001d-47a1-8ca6-ea2bd9ffd122")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("21514d68-8938-43fd-9873-5e8cf5903790"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("21514d68-8938-43fd-9873-5e8cf5903790")];}
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
	createListInput.VisibleColumnsCount = Int32.Parse("7");
	createListInput.ColumnWidthsCSV = "100px,auto,120px,120px,120px,120px,120px";
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
			actionItem.Template = @"<a class=""btn btn-default btn-outline btn-sm"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record)}}"">
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
		#region << field from Relation: username >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("263c0b21-88c1-4c2b-80b4-db7402b0d2e2");
			listItemFromRelation.FieldName = "username";
			listItemFromRelation.FieldLabel = "owner";
			listItemFromRelation.FieldPlaceholder = "";
			listItemFromRelation.FieldHelpText = "";
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = "";
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
		queryDictionary[new Guid("913d82a9-dfbe-4862-b70d-ec4a7b1b4035")] = new InputRecordListQuery();
		queryDictionary[new Guid("913d82a9-dfbe-4862-b70d-ec4a7b1b4035")].FieldName = "subject";
		queryDictionary[new Guid("913d82a9-dfbe-4862-b70d-ec4a7b1b4035")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("913d82a9-dfbe-4862-b70d-ec4a7b1b4035")].QueryType = "CONTAINS";
		queryDictionary[new Guid("913d82a9-dfbe-4862-b70d-ec4a7b1b4035")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("913d82a9-dfbe-4862-b70d-ec4a7b1b4035"))) {queryDictionary[new Guid("913d82a9-dfbe-4862-b70d-ec4a7b1b4035")].SubQueries = subQueryDictionary[new Guid("913d82a9-dfbe-4862-b70d-ec4a7b1b4035")];}
		if(!subQueryDictionary.ContainsKey(new Guid("c4afc6a8-637a-4438-80a3-db8bb89163ed"))) {subQueryDictionary[new Guid("c4afc6a8-637a-4438-80a3-db8bb89163ed")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("c4afc6a8-637a-4438-80a3-db8bb89163ed")].Add(queryDictionary[new Guid("913d82a9-dfbe-4862-b70d-ec4a7b1b4035")]);
		}
		{
		queryDictionary[new Guid("44539f57-4b4a-4b07-888b-f28e813fd40f")] = new InputRecordListQuery();
		queryDictionary[new Guid("44539f57-4b4a-4b07-888b-f28e813fd40f")].FieldName = "status";
		queryDictionary[new Guid("44539f57-4b4a-4b07-888b-f28e813fd40f")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("44539f57-4b4a-4b07-888b-f28e813fd40f")].QueryType = "EQ";
		queryDictionary[new Guid("44539f57-4b4a-4b07-888b-f28e813fd40f")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("44539f57-4b4a-4b07-888b-f28e813fd40f"))) {queryDictionary[new Guid("44539f57-4b4a-4b07-888b-f28e813fd40f")].SubQueries = subQueryDictionary[new Guid("44539f57-4b4a-4b07-888b-f28e813fd40f")];}
		if(!subQueryDictionary.ContainsKey(new Guid("c4afc6a8-637a-4438-80a3-db8bb89163ed"))) {subQueryDictionary[new Guid("c4afc6a8-637a-4438-80a3-db8bb89163ed")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("c4afc6a8-637a-4438-80a3-db8bb89163ed")].Add(queryDictionary[new Guid("44539f57-4b4a-4b07-888b-f28e813fd40f")]);
		}
		{
		queryDictionary[new Guid("b57f0dc5-dbcf-4446-b0e9-2adec7a6ae10")] = new InputRecordListQuery();
		queryDictionary[new Guid("b57f0dc5-dbcf-4446-b0e9-2adec7a6ae10")].FieldName = "priority";
		queryDictionary[new Guid("b57f0dc5-dbcf-4446-b0e9-2adec7a6ae10")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("b57f0dc5-dbcf-4446-b0e9-2adec7a6ae10")].QueryType = "EQ";
		queryDictionary[new Guid("b57f0dc5-dbcf-4446-b0e9-2adec7a6ae10")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("b57f0dc5-dbcf-4446-b0e9-2adec7a6ae10"))) {queryDictionary[new Guid("b57f0dc5-dbcf-4446-b0e9-2adec7a6ae10")].SubQueries = subQueryDictionary[new Guid("b57f0dc5-dbcf-4446-b0e9-2adec7a6ae10")];}
		if(!subQueryDictionary.ContainsKey(new Guid("c4afc6a8-637a-4438-80a3-db8bb89163ed"))) {subQueryDictionary[new Guid("c4afc6a8-637a-4438-80a3-db8bb89163ed")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("c4afc6a8-637a-4438-80a3-db8bb89163ed")].Add(queryDictionary[new Guid("b57f0dc5-dbcf-4446-b0e9-2adec7a6ae10")]);
		}
		{
		queryDictionary[new Guid("1e38911c-aadc-4126-97c8-631011f6a6ad")] = new InputRecordListQuery();
		queryDictionary[new Guid("1e38911c-aadc-4126-97c8-631011f6a6ad")].FieldName = "$user_1_n_task_owner.username";
		queryDictionary[new Guid("1e38911c-aadc-4126-97c8-631011f6a6ad")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"$user_1_n_task_owner.username\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("1e38911c-aadc-4126-97c8-631011f6a6ad")].QueryType = "CONTAINS";
		queryDictionary[new Guid("1e38911c-aadc-4126-97c8-631011f6a6ad")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("1e38911c-aadc-4126-97c8-631011f6a6ad"))) {queryDictionary[new Guid("1e38911c-aadc-4126-97c8-631011f6a6ad")].SubQueries = subQueryDictionary[new Guid("1e38911c-aadc-4126-97c8-631011f6a6ad")];}
		if(!subQueryDictionary.ContainsKey(new Guid("c4afc6a8-637a-4438-80a3-db8bb89163ed"))) {subQueryDictionary[new Guid("c4afc6a8-637a-4438-80a3-db8bb89163ed")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("c4afc6a8-637a-4438-80a3-db8bb89163ed")].Add(queryDictionary[new Guid("1e38911c-aadc-4126-97c8-631011f6a6ad")]);
		}
		{
		queryDictionary[new Guid("65d973da-d094-4dd4-864a-933ba1994421")] = new InputRecordListQuery();
		queryDictionary[new Guid("65d973da-d094-4dd4-864a-933ba1994421")].FieldName = "fts";
		queryDictionary[new Guid("65d973da-d094-4dd4-864a-933ba1994421")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"fts\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("65d973da-d094-4dd4-864a-933ba1994421")].QueryType = "FTS";
		queryDictionary[new Guid("65d973da-d094-4dd4-864a-933ba1994421")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("65d973da-d094-4dd4-864a-933ba1994421"))) {queryDictionary[new Guid("65d973da-d094-4dd4-864a-933ba1994421")].SubQueries = subQueryDictionary[new Guid("65d973da-d094-4dd4-864a-933ba1994421")];}
		if(!subQueryDictionary.ContainsKey(new Guid("c4afc6a8-637a-4438-80a3-db8bb89163ed"))) {subQueryDictionary[new Guid("c4afc6a8-637a-4438-80a3-db8bb89163ed")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("c4afc6a8-637a-4438-80a3-db8bb89163ed")].Add(queryDictionary[new Guid("65d973da-d094-4dd4-864a-933ba1994421")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("c4afc6a8-637a-4438-80a3-db8bb89163ed"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("c4afc6a8-637a-4438-80a3-db8bb89163ed")];}
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

#region << ***Update list***  Entity: wv_bug List Name: admin >>
{
	var createListEntity = entMan.ReadEntity(new Guid("c11655fa-e4a3-4c2b-8f1e-0a6d87cfd20c")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "admin").Id;
	createListInput.Type =  "Hidden";
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
			actionItem.Template = @"<a class=""btn btn-default btn-outline btn-sm"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record, ngCtrl)}}"">
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
	createListInput.ColumnWidthsCSV = "100px,auto,120px,120px,120px,120px,120px";
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
			actionItem.Template = @"<a class=""btn btn-default btn-outline btn-sm"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record, ngCtrl)}}"">
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
		#region << field from Relation: username >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("263c0b21-88c1-4c2b-80b4-db7402b0d2e2");
			listItemFromRelation.FieldName = "username";
			listItemFromRelation.FieldLabel = "User Name";
			listItemFromRelation.FieldPlaceholder = "";
			listItemFromRelation.FieldHelpText = "";
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = "";
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
		queryDictionary[new Guid("60b340f3-cde4-42b6-9111-aa512b98031c")] = new InputRecordListQuery();
		queryDictionary[new Guid("60b340f3-cde4-42b6-9111-aa512b98031c")].FieldName = "code";
		queryDictionary[new Guid("60b340f3-cde4-42b6-9111-aa512b98031c")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("60b340f3-cde4-42b6-9111-aa512b98031c")].QueryType = "CONTAINS";
		queryDictionary[new Guid("60b340f3-cde4-42b6-9111-aa512b98031c")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("60b340f3-cde4-42b6-9111-aa512b98031c"))) {queryDictionary[new Guid("60b340f3-cde4-42b6-9111-aa512b98031c")].SubQueries = subQueryDictionary[new Guid("60b340f3-cde4-42b6-9111-aa512b98031c")];}
		if(!subQueryDictionary.ContainsKey(new Guid("bce8ef17-87cb-4103-a81f-f96d2ae3dfde"))) {subQueryDictionary[new Guid("bce8ef17-87cb-4103-a81f-f96d2ae3dfde")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("bce8ef17-87cb-4103-a81f-f96d2ae3dfde")].Add(queryDictionary[new Guid("60b340f3-cde4-42b6-9111-aa512b98031c")]);
		}
		{
		queryDictionary[new Guid("e16b13f2-0146-4c61-a398-0a5015f77556")] = new InputRecordListQuery();
		queryDictionary[new Guid("e16b13f2-0146-4c61-a398-0a5015f77556")].FieldName = "subject";
		queryDictionary[new Guid("e16b13f2-0146-4c61-a398-0a5015f77556")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("e16b13f2-0146-4c61-a398-0a5015f77556")].QueryType = "FTS";
		queryDictionary[new Guid("e16b13f2-0146-4c61-a398-0a5015f77556")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("e16b13f2-0146-4c61-a398-0a5015f77556"))) {queryDictionary[new Guid("e16b13f2-0146-4c61-a398-0a5015f77556")].SubQueries = subQueryDictionary[new Guid("e16b13f2-0146-4c61-a398-0a5015f77556")];}
		if(!subQueryDictionary.ContainsKey(new Guid("bce8ef17-87cb-4103-a81f-f96d2ae3dfde"))) {subQueryDictionary[new Guid("bce8ef17-87cb-4103-a81f-f96d2ae3dfde")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("bce8ef17-87cb-4103-a81f-f96d2ae3dfde")].Add(queryDictionary[new Guid("e16b13f2-0146-4c61-a398-0a5015f77556")]);
		}
		{
		queryDictionary[new Guid("303b94dc-78a6-418f-9305-2d468d1c64a4")] = new InputRecordListQuery();
		queryDictionary[new Guid("303b94dc-78a6-418f-9305-2d468d1c64a4")].FieldName = "status";
		queryDictionary[new Guid("303b94dc-78a6-418f-9305-2d468d1c64a4")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("303b94dc-78a6-418f-9305-2d468d1c64a4")].QueryType = "EQ";
		queryDictionary[new Guid("303b94dc-78a6-418f-9305-2d468d1c64a4")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("303b94dc-78a6-418f-9305-2d468d1c64a4"))) {queryDictionary[new Guid("303b94dc-78a6-418f-9305-2d468d1c64a4")].SubQueries = subQueryDictionary[new Guid("303b94dc-78a6-418f-9305-2d468d1c64a4")];}
		if(!subQueryDictionary.ContainsKey(new Guid("bce8ef17-87cb-4103-a81f-f96d2ae3dfde"))) {subQueryDictionary[new Guid("bce8ef17-87cb-4103-a81f-f96d2ae3dfde")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("bce8ef17-87cb-4103-a81f-f96d2ae3dfde")].Add(queryDictionary[new Guid("303b94dc-78a6-418f-9305-2d468d1c64a4")]);
		}
		{
		queryDictionary[new Guid("e09764f4-4364-47b2-9de3-5ce308ccc1e4")] = new InputRecordListQuery();
		queryDictionary[new Guid("e09764f4-4364-47b2-9de3-5ce308ccc1e4")].FieldName = "priority";
		queryDictionary[new Guid("e09764f4-4364-47b2-9de3-5ce308ccc1e4")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("e09764f4-4364-47b2-9de3-5ce308ccc1e4")].QueryType = "EQ";
		queryDictionary[new Guid("e09764f4-4364-47b2-9de3-5ce308ccc1e4")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("e09764f4-4364-47b2-9de3-5ce308ccc1e4"))) {queryDictionary[new Guid("e09764f4-4364-47b2-9de3-5ce308ccc1e4")].SubQueries = subQueryDictionary[new Guid("e09764f4-4364-47b2-9de3-5ce308ccc1e4")];}
		if(!subQueryDictionary.ContainsKey(new Guid("bce8ef17-87cb-4103-a81f-f96d2ae3dfde"))) {subQueryDictionary[new Guid("bce8ef17-87cb-4103-a81f-f96d2ae3dfde")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("bce8ef17-87cb-4103-a81f-f96d2ae3dfde")].Add(queryDictionary[new Guid("e09764f4-4364-47b2-9de3-5ce308ccc1e4")]);
		}
		{
		queryDictionary[new Guid("9312d55a-ac89-4579-a2a0-e401c1f14d07")] = new InputRecordListQuery();
		queryDictionary[new Guid("9312d55a-ac89-4579-a2a0-e401c1f14d07")].FieldName = "$user_1_n_bug_owner.username";
		queryDictionary[new Guid("9312d55a-ac89-4579-a2a0-e401c1f14d07")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"$user_1_n_bug_owner.username\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("9312d55a-ac89-4579-a2a0-e401c1f14d07")].QueryType = "CONTAINS";
		queryDictionary[new Guid("9312d55a-ac89-4579-a2a0-e401c1f14d07")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("9312d55a-ac89-4579-a2a0-e401c1f14d07"))) {queryDictionary[new Guid("9312d55a-ac89-4579-a2a0-e401c1f14d07")].SubQueries = subQueryDictionary[new Guid("9312d55a-ac89-4579-a2a0-e401c1f14d07")];}
		if(!subQueryDictionary.ContainsKey(new Guid("bce8ef17-87cb-4103-a81f-f96d2ae3dfde"))) {subQueryDictionary[new Guid("bce8ef17-87cb-4103-a81f-f96d2ae3dfde")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("bce8ef17-87cb-4103-a81f-f96d2ae3dfde")].Add(queryDictionary[new Guid("9312d55a-ac89-4579-a2a0-e401c1f14d07")]);
		}
		{
		queryDictionary[new Guid("19dfdf89-dcab-41ab-8a31-eb97d64503ae")] = new InputRecordListQuery();
		queryDictionary[new Guid("19dfdf89-dcab-41ab-8a31-eb97d64503ae")].FieldName = "fts";
		queryDictionary[new Guid("19dfdf89-dcab-41ab-8a31-eb97d64503ae")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"fts\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("19dfdf89-dcab-41ab-8a31-eb97d64503ae")].QueryType = "FTS";
		queryDictionary[new Guid("19dfdf89-dcab-41ab-8a31-eb97d64503ae")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("19dfdf89-dcab-41ab-8a31-eb97d64503ae"))) {queryDictionary[new Guid("19dfdf89-dcab-41ab-8a31-eb97d64503ae")].SubQueries = subQueryDictionary[new Guid("19dfdf89-dcab-41ab-8a31-eb97d64503ae")];}
		if(!subQueryDictionary.ContainsKey(new Guid("bce8ef17-87cb-4103-a81f-f96d2ae3dfde"))) {subQueryDictionary[new Guid("bce8ef17-87cb-4103-a81f-f96d2ae3dfde")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("bce8ef17-87cb-4103-a81f-f96d2ae3dfde")].Add(queryDictionary[new Guid("19dfdf89-dcab-41ab-8a31-eb97d64503ae")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("bce8ef17-87cb-4103-a81f-f96d2ae3dfde"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("bce8ef17-87cb-4103-a81f-f96d2ae3dfde")];}
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
	createListInput.ColumnWidthsCSV = "100px,auto,120px,120px,120px,120px,120px";
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
			actionItem.Template = @"<a class=""btn btn-default btn-outline btn-sm"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record, ngCtrl)}}"">
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
		#region << field from Relation: username >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("263c0b21-88c1-4c2b-80b4-db7402b0d2e2");
			listItemFromRelation.FieldName = "username";
			listItemFromRelation.FieldLabel = "User Name";
			listItemFromRelation.FieldPlaceholder = "";
			listItemFromRelation.FieldHelpText = "";
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = "";
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
		queryDictionary[new Guid("9c3def26-d3c5-4a4d-ba11-98c54a25795f")] = new InputRecordListQuery();
		queryDictionary[new Guid("9c3def26-d3c5-4a4d-ba11-98c54a25795f")].FieldName = null;
		queryDictionary[new Guid("9c3def26-d3c5-4a4d-ba11-98c54a25795f")].FieldValue =  null;
		queryDictionary[new Guid("9c3def26-d3c5-4a4d-ba11-98c54a25795f")].QueryType = "AND";
		queryDictionary[new Guid("9c3def26-d3c5-4a4d-ba11-98c54a25795f")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("ebf738df-f3fb-4c64-91b5-027329873d28")] = new InputRecordListQuery();
			queryDictionary[new Guid("ebf738df-f3fb-4c64-91b5-027329873d28")].FieldName = "created_by";
			queryDictionary[new Guid("ebf738df-f3fb-4c64-91b5-027329873d28")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("ebf738df-f3fb-4c64-91b5-027329873d28")].QueryType = "EQ";
			queryDictionary[new Guid("ebf738df-f3fb-4c64-91b5-027329873d28")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("ebf738df-f3fb-4c64-91b5-027329873d28"))) {queryDictionary[new Guid("ebf738df-f3fb-4c64-91b5-027329873d28")].SubQueries = subQueryDictionary[new Guid("ebf738df-f3fb-4c64-91b5-027329873d28")];}
			if(!subQueryDictionary.ContainsKey(new Guid("9c3def26-d3c5-4a4d-ba11-98c54a25795f"))) {subQueryDictionary[new Guid("9c3def26-d3c5-4a4d-ba11-98c54a25795f")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("9c3def26-d3c5-4a4d-ba11-98c54a25795f")].Add(queryDictionary[new Guid("ebf738df-f3fb-4c64-91b5-027329873d28")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("9c3def26-d3c5-4a4d-ba11-98c54a25795f"))) {queryDictionary[new Guid("9c3def26-d3c5-4a4d-ba11-98c54a25795f")].SubQueries = subQueryDictionary[new Guid("9c3def26-d3c5-4a4d-ba11-98c54a25795f")];}
		if(!subQueryDictionary.ContainsKey(new Guid("65e58de1-01bf-4006-862b-6bd1f7ad3203"))) {subQueryDictionary[new Guid("65e58de1-01bf-4006-862b-6bd1f7ad3203")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("65e58de1-01bf-4006-862b-6bd1f7ad3203")].Add(queryDictionary[new Guid("9c3def26-d3c5-4a4d-ba11-98c54a25795f")]);
		}
		{
		queryDictionary[new Guid("7b50f930-3940-44b4-80d5-a600b9864534")] = new InputRecordListQuery();
		queryDictionary[new Guid("7b50f930-3940-44b4-80d5-a600b9864534")].FieldName = null;
		queryDictionary[new Guid("7b50f930-3940-44b4-80d5-a600b9864534")].FieldValue =  null;
		queryDictionary[new Guid("7b50f930-3940-44b4-80d5-a600b9864534")].QueryType = "AND";
		queryDictionary[new Guid("7b50f930-3940-44b4-80d5-a600b9864534")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("9ca9653a-8d7e-4a3b-a7a0-30524a822a6c")] = new InputRecordListQuery();
			queryDictionary[new Guid("9ca9653a-8d7e-4a3b-a7a0-30524a822a6c")].FieldName = "code";
			queryDictionary[new Guid("9ca9653a-8d7e-4a3b-a7a0-30524a822a6c")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("9ca9653a-8d7e-4a3b-a7a0-30524a822a6c")].QueryType = "CONTAINS";
			queryDictionary[new Guid("9ca9653a-8d7e-4a3b-a7a0-30524a822a6c")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("9ca9653a-8d7e-4a3b-a7a0-30524a822a6c"))) {queryDictionary[new Guid("9ca9653a-8d7e-4a3b-a7a0-30524a822a6c")].SubQueries = subQueryDictionary[new Guid("9ca9653a-8d7e-4a3b-a7a0-30524a822a6c")];}
			if(!subQueryDictionary.ContainsKey(new Guid("7b50f930-3940-44b4-80d5-a600b9864534"))) {subQueryDictionary[new Guid("7b50f930-3940-44b4-80d5-a600b9864534")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("7b50f930-3940-44b4-80d5-a600b9864534")].Add(queryDictionary[new Guid("9ca9653a-8d7e-4a3b-a7a0-30524a822a6c")]);
			}
			{
			queryDictionary[new Guid("502b5747-7809-46b3-b66e-1dd479654aa1")] = new InputRecordListQuery();
			queryDictionary[new Guid("502b5747-7809-46b3-b66e-1dd479654aa1")].FieldName = "subject";
			queryDictionary[new Guid("502b5747-7809-46b3-b66e-1dd479654aa1")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("502b5747-7809-46b3-b66e-1dd479654aa1")].QueryType = "FTS";
			queryDictionary[new Guid("502b5747-7809-46b3-b66e-1dd479654aa1")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("502b5747-7809-46b3-b66e-1dd479654aa1"))) {queryDictionary[new Guid("502b5747-7809-46b3-b66e-1dd479654aa1")].SubQueries = subQueryDictionary[new Guid("502b5747-7809-46b3-b66e-1dd479654aa1")];}
			if(!subQueryDictionary.ContainsKey(new Guid("7b50f930-3940-44b4-80d5-a600b9864534"))) {subQueryDictionary[new Guid("7b50f930-3940-44b4-80d5-a600b9864534")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("7b50f930-3940-44b4-80d5-a600b9864534")].Add(queryDictionary[new Guid("502b5747-7809-46b3-b66e-1dd479654aa1")]);
			}
			{
			queryDictionary[new Guid("59c7d4d7-80c8-4296-aaf5-46470d2b3d19")] = new InputRecordListQuery();
			queryDictionary[new Guid("59c7d4d7-80c8-4296-aaf5-46470d2b3d19")].FieldName = "status";
			queryDictionary[new Guid("59c7d4d7-80c8-4296-aaf5-46470d2b3d19")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("59c7d4d7-80c8-4296-aaf5-46470d2b3d19")].QueryType = "EQ";
			queryDictionary[new Guid("59c7d4d7-80c8-4296-aaf5-46470d2b3d19")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("59c7d4d7-80c8-4296-aaf5-46470d2b3d19"))) {queryDictionary[new Guid("59c7d4d7-80c8-4296-aaf5-46470d2b3d19")].SubQueries = subQueryDictionary[new Guid("59c7d4d7-80c8-4296-aaf5-46470d2b3d19")];}
			if(!subQueryDictionary.ContainsKey(new Guid("7b50f930-3940-44b4-80d5-a600b9864534"))) {subQueryDictionary[new Guid("7b50f930-3940-44b4-80d5-a600b9864534")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("7b50f930-3940-44b4-80d5-a600b9864534")].Add(queryDictionary[new Guid("59c7d4d7-80c8-4296-aaf5-46470d2b3d19")]);
			}
			{
			queryDictionary[new Guid("d6c6bdc0-bf1e-4526-aac4-f92af254f3f8")] = new InputRecordListQuery();
			queryDictionary[new Guid("d6c6bdc0-bf1e-4526-aac4-f92af254f3f8")].FieldName = "priority";
			queryDictionary[new Guid("d6c6bdc0-bf1e-4526-aac4-f92af254f3f8")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("d6c6bdc0-bf1e-4526-aac4-f92af254f3f8")].QueryType = "EQ";
			queryDictionary[new Guid("d6c6bdc0-bf1e-4526-aac4-f92af254f3f8")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("d6c6bdc0-bf1e-4526-aac4-f92af254f3f8"))) {queryDictionary[new Guid("d6c6bdc0-bf1e-4526-aac4-f92af254f3f8")].SubQueries = subQueryDictionary[new Guid("d6c6bdc0-bf1e-4526-aac4-f92af254f3f8")];}
			if(!subQueryDictionary.ContainsKey(new Guid("7b50f930-3940-44b4-80d5-a600b9864534"))) {subQueryDictionary[new Guid("7b50f930-3940-44b4-80d5-a600b9864534")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("7b50f930-3940-44b4-80d5-a600b9864534")].Add(queryDictionary[new Guid("d6c6bdc0-bf1e-4526-aac4-f92af254f3f8")]);
			}
			{
			queryDictionary[new Guid("c4a1d528-9582-4695-9581-dd940db8c63a")] = new InputRecordListQuery();
			queryDictionary[new Guid("c4a1d528-9582-4695-9581-dd940db8c63a")].FieldName = "$user_1_n_bug_owner.username";
			queryDictionary[new Guid("c4a1d528-9582-4695-9581-dd940db8c63a")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"$user_1_n_bug_owner.username\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("c4a1d528-9582-4695-9581-dd940db8c63a")].QueryType = "CONTAINS";
			queryDictionary[new Guid("c4a1d528-9582-4695-9581-dd940db8c63a")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("c4a1d528-9582-4695-9581-dd940db8c63a"))) {queryDictionary[new Guid("c4a1d528-9582-4695-9581-dd940db8c63a")].SubQueries = subQueryDictionary[new Guid("c4a1d528-9582-4695-9581-dd940db8c63a")];}
			if(!subQueryDictionary.ContainsKey(new Guid("7b50f930-3940-44b4-80d5-a600b9864534"))) {subQueryDictionary[new Guid("7b50f930-3940-44b4-80d5-a600b9864534")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("7b50f930-3940-44b4-80d5-a600b9864534")].Add(queryDictionary[new Guid("c4a1d528-9582-4695-9581-dd940db8c63a")]);
			}
			{
			queryDictionary[new Guid("a60cc838-6f8d-47be-9c48-7a95d32b4ca5")] = new InputRecordListQuery();
			queryDictionary[new Guid("a60cc838-6f8d-47be-9c48-7a95d32b4ca5")].FieldName = "fts";
			queryDictionary[new Guid("a60cc838-6f8d-47be-9c48-7a95d32b4ca5")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"fts\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("a60cc838-6f8d-47be-9c48-7a95d32b4ca5")].QueryType = "FTS";
			queryDictionary[new Guid("a60cc838-6f8d-47be-9c48-7a95d32b4ca5")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("a60cc838-6f8d-47be-9c48-7a95d32b4ca5"))) {queryDictionary[new Guid("a60cc838-6f8d-47be-9c48-7a95d32b4ca5")].SubQueries = subQueryDictionary[new Guid("a60cc838-6f8d-47be-9c48-7a95d32b4ca5")];}
			if(!subQueryDictionary.ContainsKey(new Guid("7b50f930-3940-44b4-80d5-a600b9864534"))) {subQueryDictionary[new Guid("7b50f930-3940-44b4-80d5-a600b9864534")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("7b50f930-3940-44b4-80d5-a600b9864534")].Add(queryDictionary[new Guid("a60cc838-6f8d-47be-9c48-7a95d32b4ca5")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("7b50f930-3940-44b4-80d5-a600b9864534"))) {queryDictionary[new Guid("7b50f930-3940-44b4-80d5-a600b9864534")].SubQueries = subQueryDictionary[new Guid("7b50f930-3940-44b4-80d5-a600b9864534")];}
		if(!subQueryDictionary.ContainsKey(new Guid("65e58de1-01bf-4006-862b-6bd1f7ad3203"))) {subQueryDictionary[new Guid("65e58de1-01bf-4006-862b-6bd1f7ad3203")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("65e58de1-01bf-4006-862b-6bd1f7ad3203")].Add(queryDictionary[new Guid("7b50f930-3940-44b4-80d5-a600b9864534")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("65e58de1-01bf-4006-862b-6bd1f7ad3203"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("65e58de1-01bf-4006-862b-6bd1f7ad3203")];}
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
	createListInput.ColumnWidthsCSV = "100px,auto,160px,120px,120px,120px";
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
			actionItem.Template = @"<a class=""btn btn-default btn-outline btn-sm"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record, ngCtrl)}}"">
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
		queryDictionary[new Guid("0680f2e9-f208-4fc2-a656-e110735de5b1")] = new InputRecordListQuery();
		queryDictionary[new Guid("0680f2e9-f208-4fc2-a656-e110735de5b1")].FieldName = null;
		queryDictionary[new Guid("0680f2e9-f208-4fc2-a656-e110735de5b1")].FieldValue =  null;
		queryDictionary[new Guid("0680f2e9-f208-4fc2-a656-e110735de5b1")].QueryType = "AND";
		queryDictionary[new Guid("0680f2e9-f208-4fc2-a656-e110735de5b1")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("2bf25678-6d4b-4585-a9a8-464d2faf45a9")] = new InputRecordListQuery();
			queryDictionary[new Guid("2bf25678-6d4b-4585-a9a8-464d2faf45a9")].FieldName = "owner_id";
			queryDictionary[new Guid("2bf25678-6d4b-4585-a9a8-464d2faf45a9")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("2bf25678-6d4b-4585-a9a8-464d2faf45a9")].QueryType = "EQ";
			queryDictionary[new Guid("2bf25678-6d4b-4585-a9a8-464d2faf45a9")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("2bf25678-6d4b-4585-a9a8-464d2faf45a9"))) {queryDictionary[new Guid("2bf25678-6d4b-4585-a9a8-464d2faf45a9")].SubQueries = subQueryDictionary[new Guid("2bf25678-6d4b-4585-a9a8-464d2faf45a9")];}
			if(!subQueryDictionary.ContainsKey(new Guid("0680f2e9-f208-4fc2-a656-e110735de5b1"))) {subQueryDictionary[new Guid("0680f2e9-f208-4fc2-a656-e110735de5b1")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("0680f2e9-f208-4fc2-a656-e110735de5b1")].Add(queryDictionary[new Guid("2bf25678-6d4b-4585-a9a8-464d2faf45a9")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("0680f2e9-f208-4fc2-a656-e110735de5b1"))) {queryDictionary[new Guid("0680f2e9-f208-4fc2-a656-e110735de5b1")].SubQueries = subQueryDictionary[new Guid("0680f2e9-f208-4fc2-a656-e110735de5b1")];}
		if(!subQueryDictionary.ContainsKey(new Guid("72905a01-a258-4132-a205-2b5aa0a75b68"))) {subQueryDictionary[new Guid("72905a01-a258-4132-a205-2b5aa0a75b68")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("72905a01-a258-4132-a205-2b5aa0a75b68")].Add(queryDictionary[new Guid("0680f2e9-f208-4fc2-a656-e110735de5b1")]);
		}
		{
		queryDictionary[new Guid("995c86da-609f-412d-a771-41103bd71d08")] = new InputRecordListQuery();
		queryDictionary[new Guid("995c86da-609f-412d-a771-41103bd71d08")].FieldName = null;
		queryDictionary[new Guid("995c86da-609f-412d-a771-41103bd71d08")].FieldValue =  null;
		queryDictionary[new Guid("995c86da-609f-412d-a771-41103bd71d08")].QueryType = "AND";
		queryDictionary[new Guid("995c86da-609f-412d-a771-41103bd71d08")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("d91269ac-3834-4d32-b826-08aff8d38e91")] = new InputRecordListQuery();
			queryDictionary[new Guid("d91269ac-3834-4d32-b826-08aff8d38e91")].FieldName = "code";
			queryDictionary[new Guid("d91269ac-3834-4d32-b826-08aff8d38e91")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("d91269ac-3834-4d32-b826-08aff8d38e91")].QueryType = "CONTAINS";
			queryDictionary[new Guid("d91269ac-3834-4d32-b826-08aff8d38e91")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("d91269ac-3834-4d32-b826-08aff8d38e91"))) {queryDictionary[new Guid("d91269ac-3834-4d32-b826-08aff8d38e91")].SubQueries = subQueryDictionary[new Guid("d91269ac-3834-4d32-b826-08aff8d38e91")];}
			if(!subQueryDictionary.ContainsKey(new Guid("995c86da-609f-412d-a771-41103bd71d08"))) {subQueryDictionary[new Guid("995c86da-609f-412d-a771-41103bd71d08")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("995c86da-609f-412d-a771-41103bd71d08")].Add(queryDictionary[new Guid("d91269ac-3834-4d32-b826-08aff8d38e91")]);
			}
			{
			queryDictionary[new Guid("8dbb7ecf-5ddb-4f50-ba0d-0c8de7a2537f")] = new InputRecordListQuery();
			queryDictionary[new Guid("8dbb7ecf-5ddb-4f50-ba0d-0c8de7a2537f")].FieldName = "subject";
			queryDictionary[new Guid("8dbb7ecf-5ddb-4f50-ba0d-0c8de7a2537f")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("8dbb7ecf-5ddb-4f50-ba0d-0c8de7a2537f")].QueryType = "FTS";
			queryDictionary[new Guid("8dbb7ecf-5ddb-4f50-ba0d-0c8de7a2537f")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("8dbb7ecf-5ddb-4f50-ba0d-0c8de7a2537f"))) {queryDictionary[new Guid("8dbb7ecf-5ddb-4f50-ba0d-0c8de7a2537f")].SubQueries = subQueryDictionary[new Guid("8dbb7ecf-5ddb-4f50-ba0d-0c8de7a2537f")];}
			if(!subQueryDictionary.ContainsKey(new Guid("995c86da-609f-412d-a771-41103bd71d08"))) {subQueryDictionary[new Guid("995c86da-609f-412d-a771-41103bd71d08")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("995c86da-609f-412d-a771-41103bd71d08")].Add(queryDictionary[new Guid("8dbb7ecf-5ddb-4f50-ba0d-0c8de7a2537f")]);
			}
			{
			queryDictionary[new Guid("38c9cda3-298b-4802-aeb0-3db472b806ef")] = new InputRecordListQuery();
			queryDictionary[new Guid("38c9cda3-298b-4802-aeb0-3db472b806ef")].FieldName = "status";
			queryDictionary[new Guid("38c9cda3-298b-4802-aeb0-3db472b806ef")].FieldValue =  "closed";
			queryDictionary[new Guid("38c9cda3-298b-4802-aeb0-3db472b806ef")].QueryType = "NOT";
			queryDictionary[new Guid("38c9cda3-298b-4802-aeb0-3db472b806ef")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("38c9cda3-298b-4802-aeb0-3db472b806ef"))) {queryDictionary[new Guid("38c9cda3-298b-4802-aeb0-3db472b806ef")].SubQueries = subQueryDictionary[new Guid("38c9cda3-298b-4802-aeb0-3db472b806ef")];}
			if(!subQueryDictionary.ContainsKey(new Guid("995c86da-609f-412d-a771-41103bd71d08"))) {subQueryDictionary[new Guid("995c86da-609f-412d-a771-41103bd71d08")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("995c86da-609f-412d-a771-41103bd71d08")].Add(queryDictionary[new Guid("38c9cda3-298b-4802-aeb0-3db472b806ef")]);
			}
			{
			queryDictionary[new Guid("6ce09cfa-38d2-4c04-9f6c-75bd7220dcac")] = new InputRecordListQuery();
			queryDictionary[new Guid("6ce09cfa-38d2-4c04-9f6c-75bd7220dcac")].FieldName = "priority";
			queryDictionary[new Guid("6ce09cfa-38d2-4c04-9f6c-75bd7220dcac")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("6ce09cfa-38d2-4c04-9f6c-75bd7220dcac")].QueryType = "EQ";
			queryDictionary[new Guid("6ce09cfa-38d2-4c04-9f6c-75bd7220dcac")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("6ce09cfa-38d2-4c04-9f6c-75bd7220dcac"))) {queryDictionary[new Guid("6ce09cfa-38d2-4c04-9f6c-75bd7220dcac")].SubQueries = subQueryDictionary[new Guid("6ce09cfa-38d2-4c04-9f6c-75bd7220dcac")];}
			if(!subQueryDictionary.ContainsKey(new Guid("995c86da-609f-412d-a771-41103bd71d08"))) {subQueryDictionary[new Guid("995c86da-609f-412d-a771-41103bd71d08")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("995c86da-609f-412d-a771-41103bd71d08")].Add(queryDictionary[new Guid("6ce09cfa-38d2-4c04-9f6c-75bd7220dcac")]);
			}
			{
			queryDictionary[new Guid("861fc000-6e33-4d55-90f7-cff4fbd7adab")] = new InputRecordListQuery();
			queryDictionary[new Guid("861fc000-6e33-4d55-90f7-cff4fbd7adab")].FieldName = "fts";
			queryDictionary[new Guid("861fc000-6e33-4d55-90f7-cff4fbd7adab")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"fts\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("861fc000-6e33-4d55-90f7-cff4fbd7adab")].QueryType = "FTS";
			queryDictionary[new Guid("861fc000-6e33-4d55-90f7-cff4fbd7adab")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("861fc000-6e33-4d55-90f7-cff4fbd7adab"))) {queryDictionary[new Guid("861fc000-6e33-4d55-90f7-cff4fbd7adab")].SubQueries = subQueryDictionary[new Guid("861fc000-6e33-4d55-90f7-cff4fbd7adab")];}
			if(!subQueryDictionary.ContainsKey(new Guid("995c86da-609f-412d-a771-41103bd71d08"))) {subQueryDictionary[new Guid("995c86da-609f-412d-a771-41103bd71d08")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("995c86da-609f-412d-a771-41103bd71d08")].Add(queryDictionary[new Guid("861fc000-6e33-4d55-90f7-cff4fbd7adab")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("995c86da-609f-412d-a771-41103bd71d08"))) {queryDictionary[new Guid("995c86da-609f-412d-a771-41103bd71d08")].SubQueries = subQueryDictionary[new Guid("995c86da-609f-412d-a771-41103bd71d08")];}
		if(!subQueryDictionary.ContainsKey(new Guid("72905a01-a258-4132-a205-2b5aa0a75b68"))) {subQueryDictionary[new Guid("72905a01-a258-4132-a205-2b5aa0a75b68")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("72905a01-a258-4132-a205-2b5aa0a75b68")].Add(queryDictionary[new Guid("995c86da-609f-412d-a771-41103bd71d08")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("72905a01-a258-4132-a205-2b5aa0a75b68"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("72905a01-a258-4132-a205-2b5aa0a75b68")];}
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
	createListInput.ColumnWidthsCSV = "100px,auto,120px,120px,120px,120px";
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
			actionItem.Template = @"<a class=""btn btn-default btn-outline btn-sm"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record, ngCtrl)}}"">
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
		queryDictionary[new Guid("d1769d79-b7c9-4d0c-bdbb-1b3d1f0af655")] = new InputRecordListQuery();
		queryDictionary[new Guid("d1769d79-b7c9-4d0c-bdbb-1b3d1f0af655")].FieldName = null;
		queryDictionary[new Guid("d1769d79-b7c9-4d0c-bdbb-1b3d1f0af655")].FieldValue =  null;
		queryDictionary[new Guid("d1769d79-b7c9-4d0c-bdbb-1b3d1f0af655")].QueryType = "OR";
		queryDictionary[new Guid("d1769d79-b7c9-4d0c-bdbb-1b3d1f0af655")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("9c0b0385-8318-4b04-a0b8-20a84dedbcfe")] = new InputRecordListQuery();
			queryDictionary[new Guid("9c0b0385-8318-4b04-a0b8-20a84dedbcfe")].FieldName = "owner_id";
			queryDictionary[new Guid("9c0b0385-8318-4b04-a0b8-20a84dedbcfe")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("9c0b0385-8318-4b04-a0b8-20a84dedbcfe")].QueryType = "EQ";
			queryDictionary[new Guid("9c0b0385-8318-4b04-a0b8-20a84dedbcfe")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("9c0b0385-8318-4b04-a0b8-20a84dedbcfe"))) {queryDictionary[new Guid("9c0b0385-8318-4b04-a0b8-20a84dedbcfe")].SubQueries = subQueryDictionary[new Guid("9c0b0385-8318-4b04-a0b8-20a84dedbcfe")];}
			if(!subQueryDictionary.ContainsKey(new Guid("d1769d79-b7c9-4d0c-bdbb-1b3d1f0af655"))) {subQueryDictionary[new Guid("d1769d79-b7c9-4d0c-bdbb-1b3d1f0af655")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("d1769d79-b7c9-4d0c-bdbb-1b3d1f0af655")].Add(queryDictionary[new Guid("9c0b0385-8318-4b04-a0b8-20a84dedbcfe")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("d1769d79-b7c9-4d0c-bdbb-1b3d1f0af655"))) {queryDictionary[new Guid("d1769d79-b7c9-4d0c-bdbb-1b3d1f0af655")].SubQueries = subQueryDictionary[new Guid("d1769d79-b7c9-4d0c-bdbb-1b3d1f0af655")];}
		if(!subQueryDictionary.ContainsKey(new Guid("58778400-8a88-4ae4-93f9-7f0305ae1606"))) {subQueryDictionary[new Guid("58778400-8a88-4ae4-93f9-7f0305ae1606")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("58778400-8a88-4ae4-93f9-7f0305ae1606")].Add(queryDictionary[new Guid("d1769d79-b7c9-4d0c-bdbb-1b3d1f0af655")]);
		}
		{
		queryDictionary[new Guid("94641ff7-aa93-4a7d-b6ed-99286abbca98")] = new InputRecordListQuery();
		queryDictionary[new Guid("94641ff7-aa93-4a7d-b6ed-99286abbca98")].FieldName = null;
		queryDictionary[new Guid("94641ff7-aa93-4a7d-b6ed-99286abbca98")].FieldValue =  null;
		queryDictionary[new Guid("94641ff7-aa93-4a7d-b6ed-99286abbca98")].QueryType = "AND";
		queryDictionary[new Guid("94641ff7-aa93-4a7d-b6ed-99286abbca98")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("679ac5b8-6ce1-4698-83f7-55a12a4a1c8d")] = new InputRecordListQuery();
			queryDictionary[new Guid("679ac5b8-6ce1-4698-83f7-55a12a4a1c8d")].FieldName = "code";
			queryDictionary[new Guid("679ac5b8-6ce1-4698-83f7-55a12a4a1c8d")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("679ac5b8-6ce1-4698-83f7-55a12a4a1c8d")].QueryType = "CONTAINS";
			queryDictionary[new Guid("679ac5b8-6ce1-4698-83f7-55a12a4a1c8d")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("679ac5b8-6ce1-4698-83f7-55a12a4a1c8d"))) {queryDictionary[new Guid("679ac5b8-6ce1-4698-83f7-55a12a4a1c8d")].SubQueries = subQueryDictionary[new Guid("679ac5b8-6ce1-4698-83f7-55a12a4a1c8d")];}
			if(!subQueryDictionary.ContainsKey(new Guid("94641ff7-aa93-4a7d-b6ed-99286abbca98"))) {subQueryDictionary[new Guid("94641ff7-aa93-4a7d-b6ed-99286abbca98")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("94641ff7-aa93-4a7d-b6ed-99286abbca98")].Add(queryDictionary[new Guid("679ac5b8-6ce1-4698-83f7-55a12a4a1c8d")]);
			}
			{
			queryDictionary[new Guid("e135fca2-f0c0-4aa6-93f6-4b702882f791")] = new InputRecordListQuery();
			queryDictionary[new Guid("e135fca2-f0c0-4aa6-93f6-4b702882f791")].FieldName = "subject";
			queryDictionary[new Guid("e135fca2-f0c0-4aa6-93f6-4b702882f791")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("e135fca2-f0c0-4aa6-93f6-4b702882f791")].QueryType = "FTS";
			queryDictionary[new Guid("e135fca2-f0c0-4aa6-93f6-4b702882f791")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("e135fca2-f0c0-4aa6-93f6-4b702882f791"))) {queryDictionary[new Guid("e135fca2-f0c0-4aa6-93f6-4b702882f791")].SubQueries = subQueryDictionary[new Guid("e135fca2-f0c0-4aa6-93f6-4b702882f791")];}
			if(!subQueryDictionary.ContainsKey(new Guid("94641ff7-aa93-4a7d-b6ed-99286abbca98"))) {subQueryDictionary[new Guid("94641ff7-aa93-4a7d-b6ed-99286abbca98")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("94641ff7-aa93-4a7d-b6ed-99286abbca98")].Add(queryDictionary[new Guid("e135fca2-f0c0-4aa6-93f6-4b702882f791")]);
			}
			{
			queryDictionary[new Guid("4810a5b8-fca1-4503-81ad-fe8dfdbaf65d")] = new InputRecordListQuery();
			queryDictionary[new Guid("4810a5b8-fca1-4503-81ad-fe8dfdbaf65d")].FieldName = "status";
			queryDictionary[new Guid("4810a5b8-fca1-4503-81ad-fe8dfdbaf65d")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("4810a5b8-fca1-4503-81ad-fe8dfdbaf65d")].QueryType = "EQ";
			queryDictionary[new Guid("4810a5b8-fca1-4503-81ad-fe8dfdbaf65d")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("4810a5b8-fca1-4503-81ad-fe8dfdbaf65d"))) {queryDictionary[new Guid("4810a5b8-fca1-4503-81ad-fe8dfdbaf65d")].SubQueries = subQueryDictionary[new Guid("4810a5b8-fca1-4503-81ad-fe8dfdbaf65d")];}
			if(!subQueryDictionary.ContainsKey(new Guid("94641ff7-aa93-4a7d-b6ed-99286abbca98"))) {subQueryDictionary[new Guid("94641ff7-aa93-4a7d-b6ed-99286abbca98")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("94641ff7-aa93-4a7d-b6ed-99286abbca98")].Add(queryDictionary[new Guid("4810a5b8-fca1-4503-81ad-fe8dfdbaf65d")]);
			}
			{
			queryDictionary[new Guid("076940c3-e499-4466-82bf-9129d18c9e6c")] = new InputRecordListQuery();
			queryDictionary[new Guid("076940c3-e499-4466-82bf-9129d18c9e6c")].FieldName = "priority";
			queryDictionary[new Guid("076940c3-e499-4466-82bf-9129d18c9e6c")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("076940c3-e499-4466-82bf-9129d18c9e6c")].QueryType = "EQ";
			queryDictionary[new Guid("076940c3-e499-4466-82bf-9129d18c9e6c")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("076940c3-e499-4466-82bf-9129d18c9e6c"))) {queryDictionary[new Guid("076940c3-e499-4466-82bf-9129d18c9e6c")].SubQueries = subQueryDictionary[new Guid("076940c3-e499-4466-82bf-9129d18c9e6c")];}
			if(!subQueryDictionary.ContainsKey(new Guid("94641ff7-aa93-4a7d-b6ed-99286abbca98"))) {subQueryDictionary[new Guid("94641ff7-aa93-4a7d-b6ed-99286abbca98")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("94641ff7-aa93-4a7d-b6ed-99286abbca98")].Add(queryDictionary[new Guid("076940c3-e499-4466-82bf-9129d18c9e6c")]);
			}
			{
			queryDictionary[new Guid("0cb92042-a8b1-48d3-a6d2-05b4730b16f5")] = new InputRecordListQuery();
			queryDictionary[new Guid("0cb92042-a8b1-48d3-a6d2-05b4730b16f5")].FieldName = "fts";
			queryDictionary[new Guid("0cb92042-a8b1-48d3-a6d2-05b4730b16f5")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"fts\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("0cb92042-a8b1-48d3-a6d2-05b4730b16f5")].QueryType = "FTS";
			queryDictionary[new Guid("0cb92042-a8b1-48d3-a6d2-05b4730b16f5")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("0cb92042-a8b1-48d3-a6d2-05b4730b16f5"))) {queryDictionary[new Guid("0cb92042-a8b1-48d3-a6d2-05b4730b16f5")].SubQueries = subQueryDictionary[new Guid("0cb92042-a8b1-48d3-a6d2-05b4730b16f5")];}
			if(!subQueryDictionary.ContainsKey(new Guid("94641ff7-aa93-4a7d-b6ed-99286abbca98"))) {subQueryDictionary[new Guid("94641ff7-aa93-4a7d-b6ed-99286abbca98")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("94641ff7-aa93-4a7d-b6ed-99286abbca98")].Add(queryDictionary[new Guid("0cb92042-a8b1-48d3-a6d2-05b4730b16f5")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("94641ff7-aa93-4a7d-b6ed-99286abbca98"))) {queryDictionary[new Guid("94641ff7-aa93-4a7d-b6ed-99286abbca98")].SubQueries = subQueryDictionary[new Guid("94641ff7-aa93-4a7d-b6ed-99286abbca98")];}
		if(!subQueryDictionary.ContainsKey(new Guid("58778400-8a88-4ae4-93f9-7f0305ae1606"))) {subQueryDictionary[new Guid("58778400-8a88-4ae4-93f9-7f0305ae1606")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("58778400-8a88-4ae4-93f9-7f0305ae1606")].Add(queryDictionary[new Guid("94641ff7-aa93-4a7d-b6ed-99286abbca98")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("58778400-8a88-4ae4-93f9-7f0305ae1606"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("58778400-8a88-4ae4-93f9-7f0305ae1606")];}
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
	createListInput.VisibleColumnsCount = Int32.Parse("6");
	createListInput.ColumnWidthsCSV = "100px,auto,120px,120px,120px,120px";
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
			actionItem.Template = @"<a class=""btn btn-default btn-outline btn-sm"" ng-href=""{{::ngCtrl.getRecordDetailsUrl(record)}}"">
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
		#region << field from Relation: username >>
		{
			var listItemFromRelation = new InputRecordListRelationFieldItem();
			listItemFromRelation.EntityId = new Guid("b9cebc3b-6443-452a-8e34-b311a73dcc8b");
			listItemFromRelation.EntityName = "user";
			listItemFromRelation.Type = "fieldFromRelation";
			listItemFromRelation.FieldId = new Guid("263c0b21-88c1-4c2b-80b4-db7402b0d2e2");
			listItemFromRelation.FieldName = "username";
			listItemFromRelation.FieldLabel = "owner";
			listItemFromRelation.FieldPlaceholder = "";
			listItemFromRelation.FieldHelpText = "";
			listItemFromRelation.FieldRequired = false;
			listItemFromRelation.FieldLookupList = "";
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
		queryDictionary[new Guid("b3f010c9-1c5e-4779-88d8-203dc1739034")] = new InputRecordListQuery();
		queryDictionary[new Guid("b3f010c9-1c5e-4779-88d8-203dc1739034")].FieldName = "subject";
		queryDictionary[new Guid("b3f010c9-1c5e-4779-88d8-203dc1739034")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("b3f010c9-1c5e-4779-88d8-203dc1739034")].QueryType = "FTS";
		queryDictionary[new Guid("b3f010c9-1c5e-4779-88d8-203dc1739034")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("b3f010c9-1c5e-4779-88d8-203dc1739034"))) {queryDictionary[new Guid("b3f010c9-1c5e-4779-88d8-203dc1739034")].SubQueries = subQueryDictionary[new Guid("b3f010c9-1c5e-4779-88d8-203dc1739034")];}
		if(!subQueryDictionary.ContainsKey(new Guid("7bf231a5-f6e0-4136-af7c-3ffaf01b9a4c"))) {subQueryDictionary[new Guid("7bf231a5-f6e0-4136-af7c-3ffaf01b9a4c")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("7bf231a5-f6e0-4136-af7c-3ffaf01b9a4c")].Add(queryDictionary[new Guid("b3f010c9-1c5e-4779-88d8-203dc1739034")]);
		}
		{
		queryDictionary[new Guid("8eb085fb-896d-4f47-a8ac-4ac534328b6c")] = new InputRecordListQuery();
		queryDictionary[new Guid("8eb085fb-896d-4f47-a8ac-4ac534328b6c")].FieldName = "status";
		queryDictionary[new Guid("8eb085fb-896d-4f47-a8ac-4ac534328b6c")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("8eb085fb-896d-4f47-a8ac-4ac534328b6c")].QueryType = "EQ";
		queryDictionary[new Guid("8eb085fb-896d-4f47-a8ac-4ac534328b6c")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("8eb085fb-896d-4f47-a8ac-4ac534328b6c"))) {queryDictionary[new Guid("8eb085fb-896d-4f47-a8ac-4ac534328b6c")].SubQueries = subQueryDictionary[new Guid("8eb085fb-896d-4f47-a8ac-4ac534328b6c")];}
		if(!subQueryDictionary.ContainsKey(new Guid("7bf231a5-f6e0-4136-af7c-3ffaf01b9a4c"))) {subQueryDictionary[new Guid("7bf231a5-f6e0-4136-af7c-3ffaf01b9a4c")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("7bf231a5-f6e0-4136-af7c-3ffaf01b9a4c")].Add(queryDictionary[new Guid("8eb085fb-896d-4f47-a8ac-4ac534328b6c")]);
		}
		{
		queryDictionary[new Guid("469fb4c2-7d27-410b-bbec-d889a84ab1c1")] = new InputRecordListQuery();
		queryDictionary[new Guid("469fb4c2-7d27-410b-bbec-d889a84ab1c1")].FieldName = "priority";
		queryDictionary[new Guid("469fb4c2-7d27-410b-bbec-d889a84ab1c1")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("469fb4c2-7d27-410b-bbec-d889a84ab1c1")].QueryType = "EQ";
		queryDictionary[new Guid("469fb4c2-7d27-410b-bbec-d889a84ab1c1")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("469fb4c2-7d27-410b-bbec-d889a84ab1c1"))) {queryDictionary[new Guid("469fb4c2-7d27-410b-bbec-d889a84ab1c1")].SubQueries = subQueryDictionary[new Guid("469fb4c2-7d27-410b-bbec-d889a84ab1c1")];}
		if(!subQueryDictionary.ContainsKey(new Guid("7bf231a5-f6e0-4136-af7c-3ffaf01b9a4c"))) {subQueryDictionary[new Guid("7bf231a5-f6e0-4136-af7c-3ffaf01b9a4c")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("7bf231a5-f6e0-4136-af7c-3ffaf01b9a4c")].Add(queryDictionary[new Guid("469fb4c2-7d27-410b-bbec-d889a84ab1c1")]);
		}
		{
		queryDictionary[new Guid("624e7f3f-bd5e-4032-a8f5-74b51738a428")] = new InputRecordListQuery();
		queryDictionary[new Guid("624e7f3f-bd5e-4032-a8f5-74b51738a428")].FieldName = "$user_1_n_bug_owner.username";
		queryDictionary[new Guid("624e7f3f-bd5e-4032-a8f5-74b51738a428")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"$user_1_n_bug_owner.username\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("624e7f3f-bd5e-4032-a8f5-74b51738a428")].QueryType = "CONTAINS";
		queryDictionary[new Guid("624e7f3f-bd5e-4032-a8f5-74b51738a428")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("624e7f3f-bd5e-4032-a8f5-74b51738a428"))) {queryDictionary[new Guid("624e7f3f-bd5e-4032-a8f5-74b51738a428")].SubQueries = subQueryDictionary[new Guid("624e7f3f-bd5e-4032-a8f5-74b51738a428")];}
		if(!subQueryDictionary.ContainsKey(new Guid("7bf231a5-f6e0-4136-af7c-3ffaf01b9a4c"))) {subQueryDictionary[new Guid("7bf231a5-f6e0-4136-af7c-3ffaf01b9a4c")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("7bf231a5-f6e0-4136-af7c-3ffaf01b9a4c")].Add(queryDictionary[new Guid("624e7f3f-bd5e-4032-a8f5-74b51738a428")]);
		}
		{
		queryDictionary[new Guid("0be16c48-3728-4a9d-bbef-e7ab36b934f3")] = new InputRecordListQuery();
		queryDictionary[new Guid("0be16c48-3728-4a9d-bbef-e7ab36b934f3")].FieldName = "fts";
		queryDictionary[new Guid("0be16c48-3728-4a9d-bbef-e7ab36b934f3")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"fts\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("0be16c48-3728-4a9d-bbef-e7ab36b934f3")].QueryType = "FTS";
		queryDictionary[new Guid("0be16c48-3728-4a9d-bbef-e7ab36b934f3")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("0be16c48-3728-4a9d-bbef-e7ab36b934f3"))) {queryDictionary[new Guid("0be16c48-3728-4a9d-bbef-e7ab36b934f3")].SubQueries = subQueryDictionary[new Guid("0be16c48-3728-4a9d-bbef-e7ab36b934f3")];}
		if(!subQueryDictionary.ContainsKey(new Guid("7bf231a5-f6e0-4136-af7c-3ffaf01b9a4c"))) {subQueryDictionary[new Guid("7bf231a5-f6e0-4136-af7c-3ffaf01b9a4c")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("7bf231a5-f6e0-4136-af7c-3ffaf01b9a4c")].Add(queryDictionary[new Guid("0be16c48-3728-4a9d-bbef-e7ab36b934f3")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("7bf231a5-f6e0-4136-af7c-3ffaf01b9a4c"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("7bf231a5-f6e0-4136-af7c-3ffaf01b9a4c")];}
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
