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
		private static void Patch161119(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan, bool createSampleRecords = false)
		{

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
		queryDictionary[new Guid("94a98e95-e733-42b5-ae50-bbb86c6ea916")] = new InputRecordListQuery();
		queryDictionary[new Guid("94a98e95-e733-42b5-ae50-bbb86c6ea916")].FieldName = "code";
		queryDictionary[new Guid("94a98e95-e733-42b5-ae50-bbb86c6ea916")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("94a98e95-e733-42b5-ae50-bbb86c6ea916")].QueryType = "CONTAINS";
		queryDictionary[new Guid("94a98e95-e733-42b5-ae50-bbb86c6ea916")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("94a98e95-e733-42b5-ae50-bbb86c6ea916"))) {queryDictionary[new Guid("94a98e95-e733-42b5-ae50-bbb86c6ea916")].SubQueries = subQueryDictionary[new Guid("94a98e95-e733-42b5-ae50-bbb86c6ea916")];}
		if(!subQueryDictionary.ContainsKey(new Guid("8f0b70e4-9840-4eae-8dbc-44ca2bad576e"))) {subQueryDictionary[new Guid("8f0b70e4-9840-4eae-8dbc-44ca2bad576e")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("8f0b70e4-9840-4eae-8dbc-44ca2bad576e")].Add(queryDictionary[new Guid("94a98e95-e733-42b5-ae50-bbb86c6ea916")]);
		}
		{
		queryDictionary[new Guid("8c30de35-11e2-4d66-9d72-a93f98ac960d")] = new InputRecordListQuery();
		queryDictionary[new Guid("8c30de35-11e2-4d66-9d72-a93f98ac960d")].FieldName = "subject";
		queryDictionary[new Guid("8c30de35-11e2-4d66-9d72-a93f98ac960d")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("8c30de35-11e2-4d66-9d72-a93f98ac960d")].QueryType = "FTS";
		queryDictionary[new Guid("8c30de35-11e2-4d66-9d72-a93f98ac960d")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("8c30de35-11e2-4d66-9d72-a93f98ac960d"))) {queryDictionary[new Guid("8c30de35-11e2-4d66-9d72-a93f98ac960d")].SubQueries = subQueryDictionary[new Guid("8c30de35-11e2-4d66-9d72-a93f98ac960d")];}
		if(!subQueryDictionary.ContainsKey(new Guid("8f0b70e4-9840-4eae-8dbc-44ca2bad576e"))) {subQueryDictionary[new Guid("8f0b70e4-9840-4eae-8dbc-44ca2bad576e")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("8f0b70e4-9840-4eae-8dbc-44ca2bad576e")].Add(queryDictionary[new Guid("8c30de35-11e2-4d66-9d72-a93f98ac960d")]);
		}
		{
		queryDictionary[new Guid("3e0f39f9-bcc4-4468-b226-b6cae50cd8c8")] = new InputRecordListQuery();
		queryDictionary[new Guid("3e0f39f9-bcc4-4468-b226-b6cae50cd8c8")].FieldName = "status";
		queryDictionary[new Guid("3e0f39f9-bcc4-4468-b226-b6cae50cd8c8")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("3e0f39f9-bcc4-4468-b226-b6cae50cd8c8")].QueryType = "EQ";
		queryDictionary[new Guid("3e0f39f9-bcc4-4468-b226-b6cae50cd8c8")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("3e0f39f9-bcc4-4468-b226-b6cae50cd8c8"))) {queryDictionary[new Guid("3e0f39f9-bcc4-4468-b226-b6cae50cd8c8")].SubQueries = subQueryDictionary[new Guid("3e0f39f9-bcc4-4468-b226-b6cae50cd8c8")];}
		if(!subQueryDictionary.ContainsKey(new Guid("8f0b70e4-9840-4eae-8dbc-44ca2bad576e"))) {subQueryDictionary[new Guid("8f0b70e4-9840-4eae-8dbc-44ca2bad576e")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("8f0b70e4-9840-4eae-8dbc-44ca2bad576e")].Add(queryDictionary[new Guid("3e0f39f9-bcc4-4468-b226-b6cae50cd8c8")]);
		}
		{
		queryDictionary[new Guid("5b33d6b0-daae-4f15-819e-c56f78fa2b6b")] = new InputRecordListQuery();
		queryDictionary[new Guid("5b33d6b0-daae-4f15-819e-c56f78fa2b6b")].FieldName = "priority";
		queryDictionary[new Guid("5b33d6b0-daae-4f15-819e-c56f78fa2b6b")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("5b33d6b0-daae-4f15-819e-c56f78fa2b6b")].QueryType = "EQ";
		queryDictionary[new Guid("5b33d6b0-daae-4f15-819e-c56f78fa2b6b")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("5b33d6b0-daae-4f15-819e-c56f78fa2b6b"))) {queryDictionary[new Guid("5b33d6b0-daae-4f15-819e-c56f78fa2b6b")].SubQueries = subQueryDictionary[new Guid("5b33d6b0-daae-4f15-819e-c56f78fa2b6b")];}
		if(!subQueryDictionary.ContainsKey(new Guid("8f0b70e4-9840-4eae-8dbc-44ca2bad576e"))) {subQueryDictionary[new Guid("8f0b70e4-9840-4eae-8dbc-44ca2bad576e")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("8f0b70e4-9840-4eae-8dbc-44ca2bad576e")].Add(queryDictionary[new Guid("5b33d6b0-daae-4f15-819e-c56f78fa2b6b")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("8f0b70e4-9840-4eae-8dbc-44ca2bad576e"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("8f0b70e4-9840-4eae-8dbc-44ca2bad576e")];}
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
		queryDictionary[new Guid("b1c039b9-10bc-45f2-89b2-8dafa5d418cc")] = new InputRecordListQuery();
		queryDictionary[new Guid("b1c039b9-10bc-45f2-89b2-8dafa5d418cc")].FieldName = null;
		queryDictionary[new Guid("b1c039b9-10bc-45f2-89b2-8dafa5d418cc")].FieldValue =  null;
		queryDictionary[new Guid("b1c039b9-10bc-45f2-89b2-8dafa5d418cc")].QueryType = "AND";
		queryDictionary[new Guid("b1c039b9-10bc-45f2-89b2-8dafa5d418cc")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("635a2498-7941-4ad7-92fc-8a4e5023f5fd")] = new InputRecordListQuery();
			queryDictionary[new Guid("635a2498-7941-4ad7-92fc-8a4e5023f5fd")].FieldName = "created_by";
			queryDictionary[new Guid("635a2498-7941-4ad7-92fc-8a4e5023f5fd")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("635a2498-7941-4ad7-92fc-8a4e5023f5fd")].QueryType = "EQ";
			queryDictionary[new Guid("635a2498-7941-4ad7-92fc-8a4e5023f5fd")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("635a2498-7941-4ad7-92fc-8a4e5023f5fd"))) {queryDictionary[new Guid("635a2498-7941-4ad7-92fc-8a4e5023f5fd")].SubQueries = subQueryDictionary[new Guid("635a2498-7941-4ad7-92fc-8a4e5023f5fd")];}
			if(!subQueryDictionary.ContainsKey(new Guid("b1c039b9-10bc-45f2-89b2-8dafa5d418cc"))) {subQueryDictionary[new Guid("b1c039b9-10bc-45f2-89b2-8dafa5d418cc")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("b1c039b9-10bc-45f2-89b2-8dafa5d418cc")].Add(queryDictionary[new Guid("635a2498-7941-4ad7-92fc-8a4e5023f5fd")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("b1c039b9-10bc-45f2-89b2-8dafa5d418cc"))) {queryDictionary[new Guid("b1c039b9-10bc-45f2-89b2-8dafa5d418cc")].SubQueries = subQueryDictionary[new Guid("b1c039b9-10bc-45f2-89b2-8dafa5d418cc")];}
		if(!subQueryDictionary.ContainsKey(new Guid("22704f2c-ab7a-4c0d-9395-ed7f3f9096e3"))) {subQueryDictionary[new Guid("22704f2c-ab7a-4c0d-9395-ed7f3f9096e3")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("22704f2c-ab7a-4c0d-9395-ed7f3f9096e3")].Add(queryDictionary[new Guid("b1c039b9-10bc-45f2-89b2-8dafa5d418cc")]);
		}
		{
		queryDictionary[new Guid("e20e88fd-871c-462c-8c31-9e320d43bead")] = new InputRecordListQuery();
		queryDictionary[new Guid("e20e88fd-871c-462c-8c31-9e320d43bead")].FieldName = null;
		queryDictionary[new Guid("e20e88fd-871c-462c-8c31-9e320d43bead")].FieldValue =  null;
		queryDictionary[new Guid("e20e88fd-871c-462c-8c31-9e320d43bead")].QueryType = "AND";
		queryDictionary[new Guid("e20e88fd-871c-462c-8c31-9e320d43bead")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("1777c0bb-d6c5-4f84-b206-c73362f8c27c")] = new InputRecordListQuery();
			queryDictionary[new Guid("1777c0bb-d6c5-4f84-b206-c73362f8c27c")].FieldName = "code";
			queryDictionary[new Guid("1777c0bb-d6c5-4f84-b206-c73362f8c27c")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("1777c0bb-d6c5-4f84-b206-c73362f8c27c")].QueryType = "CONTAINS";
			queryDictionary[new Guid("1777c0bb-d6c5-4f84-b206-c73362f8c27c")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("1777c0bb-d6c5-4f84-b206-c73362f8c27c"))) {queryDictionary[new Guid("1777c0bb-d6c5-4f84-b206-c73362f8c27c")].SubQueries = subQueryDictionary[new Guid("1777c0bb-d6c5-4f84-b206-c73362f8c27c")];}
			if(!subQueryDictionary.ContainsKey(new Guid("e20e88fd-871c-462c-8c31-9e320d43bead"))) {subQueryDictionary[new Guid("e20e88fd-871c-462c-8c31-9e320d43bead")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("e20e88fd-871c-462c-8c31-9e320d43bead")].Add(queryDictionary[new Guid("1777c0bb-d6c5-4f84-b206-c73362f8c27c")]);
			}
			{
			queryDictionary[new Guid("66d4ea84-e23d-401b-af18-cd54b12baea4")] = new InputRecordListQuery();
			queryDictionary[new Guid("66d4ea84-e23d-401b-af18-cd54b12baea4")].FieldName = "subject";
			queryDictionary[new Guid("66d4ea84-e23d-401b-af18-cd54b12baea4")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("66d4ea84-e23d-401b-af18-cd54b12baea4")].QueryType = "FTS";
			queryDictionary[new Guid("66d4ea84-e23d-401b-af18-cd54b12baea4")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("66d4ea84-e23d-401b-af18-cd54b12baea4"))) {queryDictionary[new Guid("66d4ea84-e23d-401b-af18-cd54b12baea4")].SubQueries = subQueryDictionary[new Guid("66d4ea84-e23d-401b-af18-cd54b12baea4")];}
			if(!subQueryDictionary.ContainsKey(new Guid("e20e88fd-871c-462c-8c31-9e320d43bead"))) {subQueryDictionary[new Guid("e20e88fd-871c-462c-8c31-9e320d43bead")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("e20e88fd-871c-462c-8c31-9e320d43bead")].Add(queryDictionary[new Guid("66d4ea84-e23d-401b-af18-cd54b12baea4")]);
			}
			{
			queryDictionary[new Guid("008be48c-0789-40d4-9289-84c2e292dc37")] = new InputRecordListQuery();
			queryDictionary[new Guid("008be48c-0789-40d4-9289-84c2e292dc37")].FieldName = "status";
			queryDictionary[new Guid("008be48c-0789-40d4-9289-84c2e292dc37")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("008be48c-0789-40d4-9289-84c2e292dc37")].QueryType = "EQ";
			queryDictionary[new Guid("008be48c-0789-40d4-9289-84c2e292dc37")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("008be48c-0789-40d4-9289-84c2e292dc37"))) {queryDictionary[new Guid("008be48c-0789-40d4-9289-84c2e292dc37")].SubQueries = subQueryDictionary[new Guid("008be48c-0789-40d4-9289-84c2e292dc37")];}
			if(!subQueryDictionary.ContainsKey(new Guid("e20e88fd-871c-462c-8c31-9e320d43bead"))) {subQueryDictionary[new Guid("e20e88fd-871c-462c-8c31-9e320d43bead")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("e20e88fd-871c-462c-8c31-9e320d43bead")].Add(queryDictionary[new Guid("008be48c-0789-40d4-9289-84c2e292dc37")]);
			}
			{
			queryDictionary[new Guid("237ab9cb-bca7-4045-b3fd-fc6047f4ce6c")] = new InputRecordListQuery();
			queryDictionary[new Guid("237ab9cb-bca7-4045-b3fd-fc6047f4ce6c")].FieldName = "priority";
			queryDictionary[new Guid("237ab9cb-bca7-4045-b3fd-fc6047f4ce6c")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("237ab9cb-bca7-4045-b3fd-fc6047f4ce6c")].QueryType = "EQ";
			queryDictionary[new Guid("237ab9cb-bca7-4045-b3fd-fc6047f4ce6c")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("237ab9cb-bca7-4045-b3fd-fc6047f4ce6c"))) {queryDictionary[new Guid("237ab9cb-bca7-4045-b3fd-fc6047f4ce6c")].SubQueries = subQueryDictionary[new Guid("237ab9cb-bca7-4045-b3fd-fc6047f4ce6c")];}
			if(!subQueryDictionary.ContainsKey(new Guid("e20e88fd-871c-462c-8c31-9e320d43bead"))) {subQueryDictionary[new Guid("e20e88fd-871c-462c-8c31-9e320d43bead")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("e20e88fd-871c-462c-8c31-9e320d43bead")].Add(queryDictionary[new Guid("237ab9cb-bca7-4045-b3fd-fc6047f4ce6c")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("e20e88fd-871c-462c-8c31-9e320d43bead"))) {queryDictionary[new Guid("e20e88fd-871c-462c-8c31-9e320d43bead")].SubQueries = subQueryDictionary[new Guid("e20e88fd-871c-462c-8c31-9e320d43bead")];}
		if(!subQueryDictionary.ContainsKey(new Guid("22704f2c-ab7a-4c0d-9395-ed7f3f9096e3"))) {subQueryDictionary[new Guid("22704f2c-ab7a-4c0d-9395-ed7f3f9096e3")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("22704f2c-ab7a-4c0d-9395-ed7f3f9096e3")].Add(queryDictionary[new Guid("e20e88fd-871c-462c-8c31-9e320d43bead")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("22704f2c-ab7a-4c0d-9395-ed7f3f9096e3"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("22704f2c-ab7a-4c0d-9395-ed7f3f9096e3")];}
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
		queryDictionary[new Guid("2135da42-35a1-471c-9832-dccf47dab06a")] = new InputRecordListQuery();
		queryDictionary[new Guid("2135da42-35a1-471c-9832-dccf47dab06a")].FieldName = null;
		queryDictionary[new Guid("2135da42-35a1-471c-9832-dccf47dab06a")].FieldValue =  null;
		queryDictionary[new Guid("2135da42-35a1-471c-9832-dccf47dab06a")].QueryType = "AND";
		queryDictionary[new Guid("2135da42-35a1-471c-9832-dccf47dab06a")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("d250e46d-52ab-4935-92d5-d302f31f3de1")] = new InputRecordListQuery();
			queryDictionary[new Guid("d250e46d-52ab-4935-92d5-d302f31f3de1")].FieldName = "owner_id";
			queryDictionary[new Guid("d250e46d-52ab-4935-92d5-d302f31f3de1")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("d250e46d-52ab-4935-92d5-d302f31f3de1")].QueryType = "EQ";
			queryDictionary[new Guid("d250e46d-52ab-4935-92d5-d302f31f3de1")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("d250e46d-52ab-4935-92d5-d302f31f3de1"))) {queryDictionary[new Guid("d250e46d-52ab-4935-92d5-d302f31f3de1")].SubQueries = subQueryDictionary[new Guid("d250e46d-52ab-4935-92d5-d302f31f3de1")];}
			if(!subQueryDictionary.ContainsKey(new Guid("2135da42-35a1-471c-9832-dccf47dab06a"))) {subQueryDictionary[new Guid("2135da42-35a1-471c-9832-dccf47dab06a")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("2135da42-35a1-471c-9832-dccf47dab06a")].Add(queryDictionary[new Guid("d250e46d-52ab-4935-92d5-d302f31f3de1")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("2135da42-35a1-471c-9832-dccf47dab06a"))) {queryDictionary[new Guid("2135da42-35a1-471c-9832-dccf47dab06a")].SubQueries = subQueryDictionary[new Guid("2135da42-35a1-471c-9832-dccf47dab06a")];}
		if(!subQueryDictionary.ContainsKey(new Guid("fc78bb3e-7abf-4da2-b0a4-47580a0a8c97"))) {subQueryDictionary[new Guid("fc78bb3e-7abf-4da2-b0a4-47580a0a8c97")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("fc78bb3e-7abf-4da2-b0a4-47580a0a8c97")].Add(queryDictionary[new Guid("2135da42-35a1-471c-9832-dccf47dab06a")]);
		}
		{
		queryDictionary[new Guid("d467529d-e790-49e7-8725-4664feacd076")] = new InputRecordListQuery();
		queryDictionary[new Guid("d467529d-e790-49e7-8725-4664feacd076")].FieldName = null;
		queryDictionary[new Guid("d467529d-e790-49e7-8725-4664feacd076")].FieldValue =  null;
		queryDictionary[new Guid("d467529d-e790-49e7-8725-4664feacd076")].QueryType = "AND";
		queryDictionary[new Guid("d467529d-e790-49e7-8725-4664feacd076")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("7977b08a-299a-41a2-884c-2b612b80582f")] = new InputRecordListQuery();
			queryDictionary[new Guid("7977b08a-299a-41a2-884c-2b612b80582f")].FieldName = "code";
			queryDictionary[new Guid("7977b08a-299a-41a2-884c-2b612b80582f")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("7977b08a-299a-41a2-884c-2b612b80582f")].QueryType = "CONTAINS";
			queryDictionary[new Guid("7977b08a-299a-41a2-884c-2b612b80582f")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("7977b08a-299a-41a2-884c-2b612b80582f"))) {queryDictionary[new Guid("7977b08a-299a-41a2-884c-2b612b80582f")].SubQueries = subQueryDictionary[new Guid("7977b08a-299a-41a2-884c-2b612b80582f")];}
			if(!subQueryDictionary.ContainsKey(new Guid("d467529d-e790-49e7-8725-4664feacd076"))) {subQueryDictionary[new Guid("d467529d-e790-49e7-8725-4664feacd076")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("d467529d-e790-49e7-8725-4664feacd076")].Add(queryDictionary[new Guid("7977b08a-299a-41a2-884c-2b612b80582f")]);
			}
			{
			queryDictionary[new Guid("1a6f497f-d65e-466c-bb8e-18ec01552650")] = new InputRecordListQuery();
			queryDictionary[new Guid("1a6f497f-d65e-466c-bb8e-18ec01552650")].FieldName = "subject";
			queryDictionary[new Guid("1a6f497f-d65e-466c-bb8e-18ec01552650")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("1a6f497f-d65e-466c-bb8e-18ec01552650")].QueryType = "FTS";
			queryDictionary[new Guid("1a6f497f-d65e-466c-bb8e-18ec01552650")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("1a6f497f-d65e-466c-bb8e-18ec01552650"))) {queryDictionary[new Guid("1a6f497f-d65e-466c-bb8e-18ec01552650")].SubQueries = subQueryDictionary[new Guid("1a6f497f-d65e-466c-bb8e-18ec01552650")];}
			if(!subQueryDictionary.ContainsKey(new Guid("d467529d-e790-49e7-8725-4664feacd076"))) {subQueryDictionary[new Guid("d467529d-e790-49e7-8725-4664feacd076")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("d467529d-e790-49e7-8725-4664feacd076")].Add(queryDictionary[new Guid("1a6f497f-d65e-466c-bb8e-18ec01552650")]);
			}
			{
			queryDictionary[new Guid("9e6bc071-7c7c-4062-88a4-61d5ee5b4139")] = new InputRecordListQuery();
			queryDictionary[new Guid("9e6bc071-7c7c-4062-88a4-61d5ee5b4139")].FieldName = "status";
			queryDictionary[new Guid("9e6bc071-7c7c-4062-88a4-61d5ee5b4139")].FieldValue =  "closed";
			queryDictionary[new Guid("9e6bc071-7c7c-4062-88a4-61d5ee5b4139")].QueryType = "NOT";
			queryDictionary[new Guid("9e6bc071-7c7c-4062-88a4-61d5ee5b4139")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("9e6bc071-7c7c-4062-88a4-61d5ee5b4139"))) {queryDictionary[new Guid("9e6bc071-7c7c-4062-88a4-61d5ee5b4139")].SubQueries = subQueryDictionary[new Guid("9e6bc071-7c7c-4062-88a4-61d5ee5b4139")];}
			if(!subQueryDictionary.ContainsKey(new Guid("d467529d-e790-49e7-8725-4664feacd076"))) {subQueryDictionary[new Guid("d467529d-e790-49e7-8725-4664feacd076")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("d467529d-e790-49e7-8725-4664feacd076")].Add(queryDictionary[new Guid("9e6bc071-7c7c-4062-88a4-61d5ee5b4139")]);
			}
			{
			queryDictionary[new Guid("77b41987-e6f8-43d6-a2db-32936c2bf621")] = new InputRecordListQuery();
			queryDictionary[new Guid("77b41987-e6f8-43d6-a2db-32936c2bf621")].FieldName = "priority";
			queryDictionary[new Guid("77b41987-e6f8-43d6-a2db-32936c2bf621")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("77b41987-e6f8-43d6-a2db-32936c2bf621")].QueryType = "EQ";
			queryDictionary[new Guid("77b41987-e6f8-43d6-a2db-32936c2bf621")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("77b41987-e6f8-43d6-a2db-32936c2bf621"))) {queryDictionary[new Guid("77b41987-e6f8-43d6-a2db-32936c2bf621")].SubQueries = subQueryDictionary[new Guid("77b41987-e6f8-43d6-a2db-32936c2bf621")];}
			if(!subQueryDictionary.ContainsKey(new Guid("d467529d-e790-49e7-8725-4664feacd076"))) {subQueryDictionary[new Guid("d467529d-e790-49e7-8725-4664feacd076")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("d467529d-e790-49e7-8725-4664feacd076")].Add(queryDictionary[new Guid("77b41987-e6f8-43d6-a2db-32936c2bf621")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("d467529d-e790-49e7-8725-4664feacd076"))) {queryDictionary[new Guid("d467529d-e790-49e7-8725-4664feacd076")].SubQueries = subQueryDictionary[new Guid("d467529d-e790-49e7-8725-4664feacd076")];}
		if(!subQueryDictionary.ContainsKey(new Guid("fc78bb3e-7abf-4da2-b0a4-47580a0a8c97"))) {subQueryDictionary[new Guid("fc78bb3e-7abf-4da2-b0a4-47580a0a8c97")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("fc78bb3e-7abf-4da2-b0a4-47580a0a8c97")].Add(queryDictionary[new Guid("d467529d-e790-49e7-8725-4664feacd076")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("fc78bb3e-7abf-4da2-b0a4-47580a0a8c97"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("fc78bb3e-7abf-4da2-b0a4-47580a0a8c97")];}
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
		queryDictionary[new Guid("3e2b84c0-9e10-4389-a04c-4a674f2d4296")] = new InputRecordListQuery();
		queryDictionary[new Guid("3e2b84c0-9e10-4389-a04c-4a674f2d4296")].FieldName = null;
		queryDictionary[new Guid("3e2b84c0-9e10-4389-a04c-4a674f2d4296")].FieldValue =  null;
		queryDictionary[new Guid("3e2b84c0-9e10-4389-a04c-4a674f2d4296")].QueryType = "OR";
		queryDictionary[new Guid("3e2b84c0-9e10-4389-a04c-4a674f2d4296")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("5d67c3b7-6b65-430f-8e20-c58b58beab4f")] = new InputRecordListQuery();
			queryDictionary[new Guid("5d67c3b7-6b65-430f-8e20-c58b58beab4f")].FieldName = "owner_id";
			queryDictionary[new Guid("5d67c3b7-6b65-430f-8e20-c58b58beab4f")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("5d67c3b7-6b65-430f-8e20-c58b58beab4f")].QueryType = "EQ";
			queryDictionary[new Guid("5d67c3b7-6b65-430f-8e20-c58b58beab4f")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("5d67c3b7-6b65-430f-8e20-c58b58beab4f"))) {queryDictionary[new Guid("5d67c3b7-6b65-430f-8e20-c58b58beab4f")].SubQueries = subQueryDictionary[new Guid("5d67c3b7-6b65-430f-8e20-c58b58beab4f")];}
			if(!subQueryDictionary.ContainsKey(new Guid("3e2b84c0-9e10-4389-a04c-4a674f2d4296"))) {subQueryDictionary[new Guid("3e2b84c0-9e10-4389-a04c-4a674f2d4296")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("3e2b84c0-9e10-4389-a04c-4a674f2d4296")].Add(queryDictionary[new Guid("5d67c3b7-6b65-430f-8e20-c58b58beab4f")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("3e2b84c0-9e10-4389-a04c-4a674f2d4296"))) {queryDictionary[new Guid("3e2b84c0-9e10-4389-a04c-4a674f2d4296")].SubQueries = subQueryDictionary[new Guid("3e2b84c0-9e10-4389-a04c-4a674f2d4296")];}
		if(!subQueryDictionary.ContainsKey(new Guid("9bd3e50f-95be-47cc-94b6-dbeab4297fcd"))) {subQueryDictionary[new Guid("9bd3e50f-95be-47cc-94b6-dbeab4297fcd")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("9bd3e50f-95be-47cc-94b6-dbeab4297fcd")].Add(queryDictionary[new Guid("3e2b84c0-9e10-4389-a04c-4a674f2d4296")]);
		}
		{
		queryDictionary[new Guid("5a293409-557d-45b6-917c-1fa39ae76d65")] = new InputRecordListQuery();
		queryDictionary[new Guid("5a293409-557d-45b6-917c-1fa39ae76d65")].FieldName = null;
		queryDictionary[new Guid("5a293409-557d-45b6-917c-1fa39ae76d65")].FieldValue =  null;
		queryDictionary[new Guid("5a293409-557d-45b6-917c-1fa39ae76d65")].QueryType = "AND";
		queryDictionary[new Guid("5a293409-557d-45b6-917c-1fa39ae76d65")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("216f488a-60a5-46b7-8c03-39c257f16552")] = new InputRecordListQuery();
			queryDictionary[new Guid("216f488a-60a5-46b7-8c03-39c257f16552")].FieldName = "code";
			queryDictionary[new Guid("216f488a-60a5-46b7-8c03-39c257f16552")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("216f488a-60a5-46b7-8c03-39c257f16552")].QueryType = "CONTAINS";
			queryDictionary[new Guid("216f488a-60a5-46b7-8c03-39c257f16552")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("216f488a-60a5-46b7-8c03-39c257f16552"))) {queryDictionary[new Guid("216f488a-60a5-46b7-8c03-39c257f16552")].SubQueries = subQueryDictionary[new Guid("216f488a-60a5-46b7-8c03-39c257f16552")];}
			if(!subQueryDictionary.ContainsKey(new Guid("5a293409-557d-45b6-917c-1fa39ae76d65"))) {subQueryDictionary[new Guid("5a293409-557d-45b6-917c-1fa39ae76d65")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("5a293409-557d-45b6-917c-1fa39ae76d65")].Add(queryDictionary[new Guid("216f488a-60a5-46b7-8c03-39c257f16552")]);
			}
			{
			queryDictionary[new Guid("7ef14c81-bbb0-47a7-ab05-7863828d61ac")] = new InputRecordListQuery();
			queryDictionary[new Guid("7ef14c81-bbb0-47a7-ab05-7863828d61ac")].FieldName = "subject";
			queryDictionary[new Guid("7ef14c81-bbb0-47a7-ab05-7863828d61ac")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("7ef14c81-bbb0-47a7-ab05-7863828d61ac")].QueryType = "FTS";
			queryDictionary[new Guid("7ef14c81-bbb0-47a7-ab05-7863828d61ac")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("7ef14c81-bbb0-47a7-ab05-7863828d61ac"))) {queryDictionary[new Guid("7ef14c81-bbb0-47a7-ab05-7863828d61ac")].SubQueries = subQueryDictionary[new Guid("7ef14c81-bbb0-47a7-ab05-7863828d61ac")];}
			if(!subQueryDictionary.ContainsKey(new Guid("5a293409-557d-45b6-917c-1fa39ae76d65"))) {subQueryDictionary[new Guid("5a293409-557d-45b6-917c-1fa39ae76d65")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("5a293409-557d-45b6-917c-1fa39ae76d65")].Add(queryDictionary[new Guid("7ef14c81-bbb0-47a7-ab05-7863828d61ac")]);
			}
			{
			queryDictionary[new Guid("9e5b5061-e9a3-4c7d-afce-a8a1c7e225f5")] = new InputRecordListQuery();
			queryDictionary[new Guid("9e5b5061-e9a3-4c7d-afce-a8a1c7e225f5")].FieldName = "status";
			queryDictionary[new Guid("9e5b5061-e9a3-4c7d-afce-a8a1c7e225f5")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("9e5b5061-e9a3-4c7d-afce-a8a1c7e225f5")].QueryType = "EQ";
			queryDictionary[new Guid("9e5b5061-e9a3-4c7d-afce-a8a1c7e225f5")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("9e5b5061-e9a3-4c7d-afce-a8a1c7e225f5"))) {queryDictionary[new Guid("9e5b5061-e9a3-4c7d-afce-a8a1c7e225f5")].SubQueries = subQueryDictionary[new Guid("9e5b5061-e9a3-4c7d-afce-a8a1c7e225f5")];}
			if(!subQueryDictionary.ContainsKey(new Guid("5a293409-557d-45b6-917c-1fa39ae76d65"))) {subQueryDictionary[new Guid("5a293409-557d-45b6-917c-1fa39ae76d65")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("5a293409-557d-45b6-917c-1fa39ae76d65")].Add(queryDictionary[new Guid("9e5b5061-e9a3-4c7d-afce-a8a1c7e225f5")]);
			}
			{
			queryDictionary[new Guid("f2518b0a-07e5-42d3-91b9-9ac4142799c3")] = new InputRecordListQuery();
			queryDictionary[new Guid("f2518b0a-07e5-42d3-91b9-9ac4142799c3")].FieldName = "priority";
			queryDictionary[new Guid("f2518b0a-07e5-42d3-91b9-9ac4142799c3")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("f2518b0a-07e5-42d3-91b9-9ac4142799c3")].QueryType = "EQ";
			queryDictionary[new Guid("f2518b0a-07e5-42d3-91b9-9ac4142799c3")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("f2518b0a-07e5-42d3-91b9-9ac4142799c3"))) {queryDictionary[new Guid("f2518b0a-07e5-42d3-91b9-9ac4142799c3")].SubQueries = subQueryDictionary[new Guid("f2518b0a-07e5-42d3-91b9-9ac4142799c3")];}
			if(!subQueryDictionary.ContainsKey(new Guid("5a293409-557d-45b6-917c-1fa39ae76d65"))) {subQueryDictionary[new Guid("5a293409-557d-45b6-917c-1fa39ae76d65")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("5a293409-557d-45b6-917c-1fa39ae76d65")].Add(queryDictionary[new Guid("f2518b0a-07e5-42d3-91b9-9ac4142799c3")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("5a293409-557d-45b6-917c-1fa39ae76d65"))) {queryDictionary[new Guid("5a293409-557d-45b6-917c-1fa39ae76d65")].SubQueries = subQueryDictionary[new Guid("5a293409-557d-45b6-917c-1fa39ae76d65")];}
		if(!subQueryDictionary.ContainsKey(new Guid("9bd3e50f-95be-47cc-94b6-dbeab4297fcd"))) {subQueryDictionary[new Guid("9bd3e50f-95be-47cc-94b6-dbeab4297fcd")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("9bd3e50f-95be-47cc-94b6-dbeab4297fcd")].Add(queryDictionary[new Guid("5a293409-557d-45b6-917c-1fa39ae76d65")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("9bd3e50f-95be-47cc-94b6-dbeab4297fcd"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("9bd3e50f-95be-47cc-94b6-dbeab4297fcd")];}
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
		queryDictionary[new Guid("fa62f088-879f-4224-b1de-047eb6396aaa")] = new InputRecordListQuery();
		queryDictionary[new Guid("fa62f088-879f-4224-b1de-047eb6396aaa")].FieldName = "subject";
		queryDictionary[new Guid("fa62f088-879f-4224-b1de-047eb6396aaa")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("fa62f088-879f-4224-b1de-047eb6396aaa")].QueryType = "FTS";
		queryDictionary[new Guid("fa62f088-879f-4224-b1de-047eb6396aaa")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("fa62f088-879f-4224-b1de-047eb6396aaa"))) {queryDictionary[new Guid("fa62f088-879f-4224-b1de-047eb6396aaa")].SubQueries = subQueryDictionary[new Guid("fa62f088-879f-4224-b1de-047eb6396aaa")];}
		if(!subQueryDictionary.ContainsKey(new Guid("9ee7a7cc-e751-4447-b4cb-2344b50d8d0a"))) {subQueryDictionary[new Guid("9ee7a7cc-e751-4447-b4cb-2344b50d8d0a")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("9ee7a7cc-e751-4447-b4cb-2344b50d8d0a")].Add(queryDictionary[new Guid("fa62f088-879f-4224-b1de-047eb6396aaa")]);
		}
		{
		queryDictionary[new Guid("c367a52d-c27a-4396-aaf4-58237489c2fd")] = new InputRecordListQuery();
		queryDictionary[new Guid("c367a52d-c27a-4396-aaf4-58237489c2fd")].FieldName = "status";
		queryDictionary[new Guid("c367a52d-c27a-4396-aaf4-58237489c2fd")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("c367a52d-c27a-4396-aaf4-58237489c2fd")].QueryType = "EQ";
		queryDictionary[new Guid("c367a52d-c27a-4396-aaf4-58237489c2fd")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("c367a52d-c27a-4396-aaf4-58237489c2fd"))) {queryDictionary[new Guid("c367a52d-c27a-4396-aaf4-58237489c2fd")].SubQueries = subQueryDictionary[new Guid("c367a52d-c27a-4396-aaf4-58237489c2fd")];}
		if(!subQueryDictionary.ContainsKey(new Guid("9ee7a7cc-e751-4447-b4cb-2344b50d8d0a"))) {subQueryDictionary[new Guid("9ee7a7cc-e751-4447-b4cb-2344b50d8d0a")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("9ee7a7cc-e751-4447-b4cb-2344b50d8d0a")].Add(queryDictionary[new Guid("c367a52d-c27a-4396-aaf4-58237489c2fd")]);
		}
		{
		queryDictionary[new Guid("a8451170-2484-4834-8e84-ffe60a6ece4b")] = new InputRecordListQuery();
		queryDictionary[new Guid("a8451170-2484-4834-8e84-ffe60a6ece4b")].FieldName = "priority";
		queryDictionary[new Guid("a8451170-2484-4834-8e84-ffe60a6ece4b")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("a8451170-2484-4834-8e84-ffe60a6ece4b")].QueryType = "EQ";
		queryDictionary[new Guid("a8451170-2484-4834-8e84-ffe60a6ece4b")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("a8451170-2484-4834-8e84-ffe60a6ece4b"))) {queryDictionary[new Guid("a8451170-2484-4834-8e84-ffe60a6ece4b")].SubQueries = subQueryDictionary[new Guid("a8451170-2484-4834-8e84-ffe60a6ece4b")];}
		if(!subQueryDictionary.ContainsKey(new Guid("9ee7a7cc-e751-4447-b4cb-2344b50d8d0a"))) {subQueryDictionary[new Guid("9ee7a7cc-e751-4447-b4cb-2344b50d8d0a")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("9ee7a7cc-e751-4447-b4cb-2344b50d8d0a")].Add(queryDictionary[new Guid("a8451170-2484-4834-8e84-ffe60a6ece4b")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("9ee7a7cc-e751-4447-b4cb-2344b50d8d0a"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("9ee7a7cc-e751-4447-b4cb-2344b50d8d0a")];}
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

#region << ***Update list***  Entity: wv_project List Name: lookup >>
{
	var createListEntity = entMan.ReadEntity(new Guid("7821ece9-42ce-470b-84d4-afc9eb35aa32")).Object;
	var createListInput = new InputRecordList();

	#region << details >>
	createListInput.Id = createListEntity.RecordLists.SingleOrDefault(x => x.Name == "lookup").Id;
	createListInput.Type =  "Lookup";
	createListInput.Name = "lookup";
	createListInput.Label = "Lookup";
	createListInput.Title = null;
	createListInput.Weight = Decimal.Parse("10.0");
	createListInput.Default = true;
	createListInput.System = true;
	createListInput.CssClass = null;
	createListInput.IconName = "list";
	createListInput.VisibleColumnsCount = Int32.Parse("5");
	createListInput.ColumnWidthsCSV = null;
	createListInput.PageSize = Int32.Parse("10");
	createListInput.DynamicHtmlTemplate = null;
	createListInput.DataSourceUrl = "/plugins/webvella-projects/api/project/list/my-projects";
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
	createListInput.Query = new InputRecordListQuery();
	var queryDictionary = new Dictionary<Guid,InputRecordListQuery>();
	var subQueryDictionary = new Dictionary<Guid,List<InputRecordListQuery>>();
	//Main query rule
	createListInput.Query.FieldName = null;
	createListInput.Query.FieldValue =  null;
	createListInput.Query.QueryType = "AND";
	createListInput.Query.SubQueries = new List<InputRecordListQuery>();
		{
		queryDictionary[new Guid("fb7b4ba9-3113-4537-bd9e-bb8cd7df81b6")] = new InputRecordListQuery();
		queryDictionary[new Guid("fb7b4ba9-3113-4537-bd9e-bb8cd7df81b6")].FieldName = "name";
		queryDictionary[new Guid("fb7b4ba9-3113-4537-bd9e-bb8cd7df81b6")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"name\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("fb7b4ba9-3113-4537-bd9e-bb8cd7df81b6")].QueryType = "FTS";
		queryDictionary[new Guid("fb7b4ba9-3113-4537-bd9e-bb8cd7df81b6")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("fb7b4ba9-3113-4537-bd9e-bb8cd7df81b6"))) {queryDictionary[new Guid("fb7b4ba9-3113-4537-bd9e-bb8cd7df81b6")].SubQueries = subQueryDictionary[new Guid("fb7b4ba9-3113-4537-bd9e-bb8cd7df81b6")];}
		if(!subQueryDictionary.ContainsKey(new Guid("724d9fd8-b2df-49fb-b1f8-57d130dc3c07"))) {subQueryDictionary[new Guid("724d9fd8-b2df-49fb-b1f8-57d130dc3c07")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("724d9fd8-b2df-49fb-b1f8-57d130dc3c07")].Add(queryDictionary[new Guid("fb7b4ba9-3113-4537-bd9e-bb8cd7df81b6")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("724d9fd8-b2df-49fb-b1f8-57d130dc3c07"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("724d9fd8-b2df-49fb-b1f8-57d130dc3c07")];}
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
			throw new Exception("System error 10060. Entity: wv_project Updated list: lookup Message:" + response.Message);
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
		queryDictionary[new Guid("d034ab78-a748-46d2-b399-109db671e6c1")] = new InputRecordListQuery();
		queryDictionary[new Guid("d034ab78-a748-46d2-b399-109db671e6c1")].FieldName = "owner_id";
		queryDictionary[new Guid("d034ab78-a748-46d2-b399-109db671e6c1")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("d034ab78-a748-46d2-b399-109db671e6c1")].QueryType = "EQ";
		queryDictionary[new Guid("d034ab78-a748-46d2-b399-109db671e6c1")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("d034ab78-a748-46d2-b399-109db671e6c1"))) {queryDictionary[new Guid("d034ab78-a748-46d2-b399-109db671e6c1")].SubQueries = subQueryDictionary[new Guid("d034ab78-a748-46d2-b399-109db671e6c1")];}
		if(!subQueryDictionary.ContainsKey(new Guid("b63bdc7b-1c68-46c6-bf20-49924c8ad944"))) {subQueryDictionary[new Guid("b63bdc7b-1c68-46c6-bf20-49924c8ad944")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("b63bdc7b-1c68-46c6-bf20-49924c8ad944")].Add(queryDictionary[new Guid("d034ab78-a748-46d2-b399-109db671e6c1")]);
		}
		{
		queryDictionary[new Guid("9597ca82-e160-4283-846a-408623e64371")] = new InputRecordListQuery();
		queryDictionary[new Guid("9597ca82-e160-4283-846a-408623e64371")].FieldName = "subject";
		queryDictionary[new Guid("9597ca82-e160-4283-846a-408623e64371")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("9597ca82-e160-4283-846a-408623e64371")].QueryType = "FTS";
		queryDictionary[new Guid("9597ca82-e160-4283-846a-408623e64371")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("9597ca82-e160-4283-846a-408623e64371"))) {queryDictionary[new Guid("9597ca82-e160-4283-846a-408623e64371")].SubQueries = subQueryDictionary[new Guid("9597ca82-e160-4283-846a-408623e64371")];}
		if(!subQueryDictionary.ContainsKey(new Guid("b63bdc7b-1c68-46c6-bf20-49924c8ad944"))) {subQueryDictionary[new Guid("b63bdc7b-1c68-46c6-bf20-49924c8ad944")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("b63bdc7b-1c68-46c6-bf20-49924c8ad944")].Add(queryDictionary[new Guid("9597ca82-e160-4283-846a-408623e64371")]);
		}
		{
		queryDictionary[new Guid("064c0296-8108-4a24-b247-69dbcb4ffcfa")] = new InputRecordListQuery();
		queryDictionary[new Guid("064c0296-8108-4a24-b247-69dbcb4ffcfa")].FieldName = "status";
		queryDictionary[new Guid("064c0296-8108-4a24-b247-69dbcb4ffcfa")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("064c0296-8108-4a24-b247-69dbcb4ffcfa")].QueryType = "EQ";
		queryDictionary[new Guid("064c0296-8108-4a24-b247-69dbcb4ffcfa")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("064c0296-8108-4a24-b247-69dbcb4ffcfa"))) {queryDictionary[new Guid("064c0296-8108-4a24-b247-69dbcb4ffcfa")].SubQueries = subQueryDictionary[new Guid("064c0296-8108-4a24-b247-69dbcb4ffcfa")];}
		if(!subQueryDictionary.ContainsKey(new Guid("b63bdc7b-1c68-46c6-bf20-49924c8ad944"))) {subQueryDictionary[new Guid("b63bdc7b-1c68-46c6-bf20-49924c8ad944")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("b63bdc7b-1c68-46c6-bf20-49924c8ad944")].Add(queryDictionary[new Guid("064c0296-8108-4a24-b247-69dbcb4ffcfa")]);
		}
		{
		queryDictionary[new Guid("932428f4-5b13-4ad5-92dc-bb6cc1b62e9b")] = new InputRecordListQuery();
		queryDictionary[new Guid("932428f4-5b13-4ad5-92dc-bb6cc1b62e9b")].FieldName = "priority";
		queryDictionary[new Guid("932428f4-5b13-4ad5-92dc-bb6cc1b62e9b")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("932428f4-5b13-4ad5-92dc-bb6cc1b62e9b")].QueryType = "EQ";
		queryDictionary[new Guid("932428f4-5b13-4ad5-92dc-bb6cc1b62e9b")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("932428f4-5b13-4ad5-92dc-bb6cc1b62e9b"))) {queryDictionary[new Guid("932428f4-5b13-4ad5-92dc-bb6cc1b62e9b")].SubQueries = subQueryDictionary[new Guid("932428f4-5b13-4ad5-92dc-bb6cc1b62e9b")];}
		if(!subQueryDictionary.ContainsKey(new Guid("b63bdc7b-1c68-46c6-bf20-49924c8ad944"))) {subQueryDictionary[new Guid("b63bdc7b-1c68-46c6-bf20-49924c8ad944")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("b63bdc7b-1c68-46c6-bf20-49924c8ad944")].Add(queryDictionary[new Guid("932428f4-5b13-4ad5-92dc-bb6cc1b62e9b")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("b63bdc7b-1c68-46c6-bf20-49924c8ad944"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("b63bdc7b-1c68-46c6-bf20-49924c8ad944")];}
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
		queryDictionary[new Guid("091fc8d1-cb41-4a82-8340-e6f9b3e4e0c5")] = new InputRecordListQuery();
		queryDictionary[new Guid("091fc8d1-cb41-4a82-8340-e6f9b3e4e0c5")].FieldName = "code";
		queryDictionary[new Guid("091fc8d1-cb41-4a82-8340-e6f9b3e4e0c5")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("091fc8d1-cb41-4a82-8340-e6f9b3e4e0c5")].QueryType = "CONTAINS";
		queryDictionary[new Guid("091fc8d1-cb41-4a82-8340-e6f9b3e4e0c5")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("091fc8d1-cb41-4a82-8340-e6f9b3e4e0c5"))) {queryDictionary[new Guid("091fc8d1-cb41-4a82-8340-e6f9b3e4e0c5")].SubQueries = subQueryDictionary[new Guid("091fc8d1-cb41-4a82-8340-e6f9b3e4e0c5")];}
		if(!subQueryDictionary.ContainsKey(new Guid("10293ae2-9834-405a-8ee6-41e2af4b25ac"))) {subQueryDictionary[new Guid("10293ae2-9834-405a-8ee6-41e2af4b25ac")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("10293ae2-9834-405a-8ee6-41e2af4b25ac")].Add(queryDictionary[new Guid("091fc8d1-cb41-4a82-8340-e6f9b3e4e0c5")]);
		}
		{
		queryDictionary[new Guid("05eabb05-bed9-4475-b0c5-574c3e42eb2c")] = new InputRecordListQuery();
		queryDictionary[new Guid("05eabb05-bed9-4475-b0c5-574c3e42eb2c")].FieldName = "subject";
		queryDictionary[new Guid("05eabb05-bed9-4475-b0c5-574c3e42eb2c")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("05eabb05-bed9-4475-b0c5-574c3e42eb2c")].QueryType = "FTS";
		queryDictionary[new Guid("05eabb05-bed9-4475-b0c5-574c3e42eb2c")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("05eabb05-bed9-4475-b0c5-574c3e42eb2c"))) {queryDictionary[new Guid("05eabb05-bed9-4475-b0c5-574c3e42eb2c")].SubQueries = subQueryDictionary[new Guid("05eabb05-bed9-4475-b0c5-574c3e42eb2c")];}
		if(!subQueryDictionary.ContainsKey(new Guid("10293ae2-9834-405a-8ee6-41e2af4b25ac"))) {subQueryDictionary[new Guid("10293ae2-9834-405a-8ee6-41e2af4b25ac")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("10293ae2-9834-405a-8ee6-41e2af4b25ac")].Add(queryDictionary[new Guid("05eabb05-bed9-4475-b0c5-574c3e42eb2c")]);
		}
		{
		queryDictionary[new Guid("6a20406a-7098-40a9-b4a5-ab33ee339972")] = new InputRecordListQuery();
		queryDictionary[new Guid("6a20406a-7098-40a9-b4a5-ab33ee339972")].FieldName = "status";
		queryDictionary[new Guid("6a20406a-7098-40a9-b4a5-ab33ee339972")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("6a20406a-7098-40a9-b4a5-ab33ee339972")].QueryType = "EQ";
		queryDictionary[new Guid("6a20406a-7098-40a9-b4a5-ab33ee339972")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("6a20406a-7098-40a9-b4a5-ab33ee339972"))) {queryDictionary[new Guid("6a20406a-7098-40a9-b4a5-ab33ee339972")].SubQueries = subQueryDictionary[new Guid("6a20406a-7098-40a9-b4a5-ab33ee339972")];}
		if(!subQueryDictionary.ContainsKey(new Guid("10293ae2-9834-405a-8ee6-41e2af4b25ac"))) {subQueryDictionary[new Guid("10293ae2-9834-405a-8ee6-41e2af4b25ac")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("10293ae2-9834-405a-8ee6-41e2af4b25ac")].Add(queryDictionary[new Guid("6a20406a-7098-40a9-b4a5-ab33ee339972")]);
		}
		{
		queryDictionary[new Guid("b44005ea-6b3c-4c32-9a78-058599a06e72")] = new InputRecordListQuery();
		queryDictionary[new Guid("b44005ea-6b3c-4c32-9a78-058599a06e72")].FieldName = "priority";
		queryDictionary[new Guid("b44005ea-6b3c-4c32-9a78-058599a06e72")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("b44005ea-6b3c-4c32-9a78-058599a06e72")].QueryType = "EQ";
		queryDictionary[new Guid("b44005ea-6b3c-4c32-9a78-058599a06e72")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("b44005ea-6b3c-4c32-9a78-058599a06e72"))) {queryDictionary[new Guid("b44005ea-6b3c-4c32-9a78-058599a06e72")].SubQueries = subQueryDictionary[new Guid("b44005ea-6b3c-4c32-9a78-058599a06e72")];}
		if(!subQueryDictionary.ContainsKey(new Guid("10293ae2-9834-405a-8ee6-41e2af4b25ac"))) {subQueryDictionary[new Guid("10293ae2-9834-405a-8ee6-41e2af4b25ac")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("10293ae2-9834-405a-8ee6-41e2af4b25ac")].Add(queryDictionary[new Guid("b44005ea-6b3c-4c32-9a78-058599a06e72")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("10293ae2-9834-405a-8ee6-41e2af4b25ac"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("10293ae2-9834-405a-8ee6-41e2af4b25ac")];}
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
		queryDictionary[new Guid("cafefa47-af39-413c-a35f-68e75275d554")] = new InputRecordListQuery();
		queryDictionary[new Guid("cafefa47-af39-413c-a35f-68e75275d554")].FieldName = null;
		queryDictionary[new Guid("cafefa47-af39-413c-a35f-68e75275d554")].FieldValue =  null;
		queryDictionary[new Guid("cafefa47-af39-413c-a35f-68e75275d554")].QueryType = "AND";
		queryDictionary[new Guid("cafefa47-af39-413c-a35f-68e75275d554")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("b2a68e71-40e7-45dd-a48e-c97f26a1ba84")] = new InputRecordListQuery();
			queryDictionary[new Guid("b2a68e71-40e7-45dd-a48e-c97f26a1ba84")].FieldName = "created_by";
			queryDictionary[new Guid("b2a68e71-40e7-45dd-a48e-c97f26a1ba84")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("b2a68e71-40e7-45dd-a48e-c97f26a1ba84")].QueryType = "EQ";
			queryDictionary[new Guid("b2a68e71-40e7-45dd-a48e-c97f26a1ba84")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("b2a68e71-40e7-45dd-a48e-c97f26a1ba84"))) {queryDictionary[new Guid("b2a68e71-40e7-45dd-a48e-c97f26a1ba84")].SubQueries = subQueryDictionary[new Guid("b2a68e71-40e7-45dd-a48e-c97f26a1ba84")];}
			if(!subQueryDictionary.ContainsKey(new Guid("cafefa47-af39-413c-a35f-68e75275d554"))) {subQueryDictionary[new Guid("cafefa47-af39-413c-a35f-68e75275d554")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("cafefa47-af39-413c-a35f-68e75275d554")].Add(queryDictionary[new Guid("b2a68e71-40e7-45dd-a48e-c97f26a1ba84")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("cafefa47-af39-413c-a35f-68e75275d554"))) {queryDictionary[new Guid("cafefa47-af39-413c-a35f-68e75275d554")].SubQueries = subQueryDictionary[new Guid("cafefa47-af39-413c-a35f-68e75275d554")];}
		if(!subQueryDictionary.ContainsKey(new Guid("e8a539e0-d299-4d6e-97c7-82b87383e9b7"))) {subQueryDictionary[new Guid("e8a539e0-d299-4d6e-97c7-82b87383e9b7")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("e8a539e0-d299-4d6e-97c7-82b87383e9b7")].Add(queryDictionary[new Guid("cafefa47-af39-413c-a35f-68e75275d554")]);
		}
		{
		queryDictionary[new Guid("f659362f-18f3-45ab-917e-214bbb0dc8dc")] = new InputRecordListQuery();
		queryDictionary[new Guid("f659362f-18f3-45ab-917e-214bbb0dc8dc")].FieldName = null;
		queryDictionary[new Guid("f659362f-18f3-45ab-917e-214bbb0dc8dc")].FieldValue =  null;
		queryDictionary[new Guid("f659362f-18f3-45ab-917e-214bbb0dc8dc")].QueryType = "AND";
		queryDictionary[new Guid("f659362f-18f3-45ab-917e-214bbb0dc8dc")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("b4d3d93a-c557-46f8-add8-a3b6bcc8e3f9")] = new InputRecordListQuery();
			queryDictionary[new Guid("b4d3d93a-c557-46f8-add8-a3b6bcc8e3f9")].FieldName = "code";
			queryDictionary[new Guid("b4d3d93a-c557-46f8-add8-a3b6bcc8e3f9")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("b4d3d93a-c557-46f8-add8-a3b6bcc8e3f9")].QueryType = "CONTAINS";
			queryDictionary[new Guid("b4d3d93a-c557-46f8-add8-a3b6bcc8e3f9")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("b4d3d93a-c557-46f8-add8-a3b6bcc8e3f9"))) {queryDictionary[new Guid("b4d3d93a-c557-46f8-add8-a3b6bcc8e3f9")].SubQueries = subQueryDictionary[new Guid("b4d3d93a-c557-46f8-add8-a3b6bcc8e3f9")];}
			if(!subQueryDictionary.ContainsKey(new Guid("f659362f-18f3-45ab-917e-214bbb0dc8dc"))) {subQueryDictionary[new Guid("f659362f-18f3-45ab-917e-214bbb0dc8dc")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("f659362f-18f3-45ab-917e-214bbb0dc8dc")].Add(queryDictionary[new Guid("b4d3d93a-c557-46f8-add8-a3b6bcc8e3f9")]);
			}
			{
			queryDictionary[new Guid("34b80953-2743-4aa3-8ea8-f8d0441f9766")] = new InputRecordListQuery();
			queryDictionary[new Guid("34b80953-2743-4aa3-8ea8-f8d0441f9766")].FieldName = "subject";
			queryDictionary[new Guid("34b80953-2743-4aa3-8ea8-f8d0441f9766")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("34b80953-2743-4aa3-8ea8-f8d0441f9766")].QueryType = "FTS";
			queryDictionary[new Guid("34b80953-2743-4aa3-8ea8-f8d0441f9766")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("34b80953-2743-4aa3-8ea8-f8d0441f9766"))) {queryDictionary[new Guid("34b80953-2743-4aa3-8ea8-f8d0441f9766")].SubQueries = subQueryDictionary[new Guid("34b80953-2743-4aa3-8ea8-f8d0441f9766")];}
			if(!subQueryDictionary.ContainsKey(new Guid("f659362f-18f3-45ab-917e-214bbb0dc8dc"))) {subQueryDictionary[new Guid("f659362f-18f3-45ab-917e-214bbb0dc8dc")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("f659362f-18f3-45ab-917e-214bbb0dc8dc")].Add(queryDictionary[new Guid("34b80953-2743-4aa3-8ea8-f8d0441f9766")]);
			}
			{
			queryDictionary[new Guid("75460ab7-5b22-4380-af08-aec89bbe6b8d")] = new InputRecordListQuery();
			queryDictionary[new Guid("75460ab7-5b22-4380-af08-aec89bbe6b8d")].FieldName = "status";
			queryDictionary[new Guid("75460ab7-5b22-4380-af08-aec89bbe6b8d")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("75460ab7-5b22-4380-af08-aec89bbe6b8d")].QueryType = "EQ";
			queryDictionary[new Guid("75460ab7-5b22-4380-af08-aec89bbe6b8d")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("75460ab7-5b22-4380-af08-aec89bbe6b8d"))) {queryDictionary[new Guid("75460ab7-5b22-4380-af08-aec89bbe6b8d")].SubQueries = subQueryDictionary[new Guid("75460ab7-5b22-4380-af08-aec89bbe6b8d")];}
			if(!subQueryDictionary.ContainsKey(new Guid("f659362f-18f3-45ab-917e-214bbb0dc8dc"))) {subQueryDictionary[new Guid("f659362f-18f3-45ab-917e-214bbb0dc8dc")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("f659362f-18f3-45ab-917e-214bbb0dc8dc")].Add(queryDictionary[new Guid("75460ab7-5b22-4380-af08-aec89bbe6b8d")]);
			}
			{
			queryDictionary[new Guid("59aabf36-bc11-483d-9472-e28d9502b1da")] = new InputRecordListQuery();
			queryDictionary[new Guid("59aabf36-bc11-483d-9472-e28d9502b1da")].FieldName = "priority";
			queryDictionary[new Guid("59aabf36-bc11-483d-9472-e28d9502b1da")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("59aabf36-bc11-483d-9472-e28d9502b1da")].QueryType = "EQ";
			queryDictionary[new Guid("59aabf36-bc11-483d-9472-e28d9502b1da")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("59aabf36-bc11-483d-9472-e28d9502b1da"))) {queryDictionary[new Guid("59aabf36-bc11-483d-9472-e28d9502b1da")].SubQueries = subQueryDictionary[new Guid("59aabf36-bc11-483d-9472-e28d9502b1da")];}
			if(!subQueryDictionary.ContainsKey(new Guid("f659362f-18f3-45ab-917e-214bbb0dc8dc"))) {subQueryDictionary[new Guid("f659362f-18f3-45ab-917e-214bbb0dc8dc")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("f659362f-18f3-45ab-917e-214bbb0dc8dc")].Add(queryDictionary[new Guid("59aabf36-bc11-483d-9472-e28d9502b1da")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("f659362f-18f3-45ab-917e-214bbb0dc8dc"))) {queryDictionary[new Guid("f659362f-18f3-45ab-917e-214bbb0dc8dc")].SubQueries = subQueryDictionary[new Guid("f659362f-18f3-45ab-917e-214bbb0dc8dc")];}
		if(!subQueryDictionary.ContainsKey(new Guid("e8a539e0-d299-4d6e-97c7-82b87383e9b7"))) {subQueryDictionary[new Guid("e8a539e0-d299-4d6e-97c7-82b87383e9b7")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("e8a539e0-d299-4d6e-97c7-82b87383e9b7")].Add(queryDictionary[new Guid("f659362f-18f3-45ab-917e-214bbb0dc8dc")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("e8a539e0-d299-4d6e-97c7-82b87383e9b7"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("e8a539e0-d299-4d6e-97c7-82b87383e9b7")];}
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
		queryDictionary[new Guid("1dd79432-5d40-4ba6-b19a-dde0eb09148f")] = new InputRecordListQuery();
		queryDictionary[new Guid("1dd79432-5d40-4ba6-b19a-dde0eb09148f")].FieldName = null;
		queryDictionary[new Guid("1dd79432-5d40-4ba6-b19a-dde0eb09148f")].FieldValue =  null;
		queryDictionary[new Guid("1dd79432-5d40-4ba6-b19a-dde0eb09148f")].QueryType = "AND";
		queryDictionary[new Guid("1dd79432-5d40-4ba6-b19a-dde0eb09148f")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("baef5cbf-2a3a-41df-af69-90acad4bc155")] = new InputRecordListQuery();
			queryDictionary[new Guid("baef5cbf-2a3a-41df-af69-90acad4bc155")].FieldName = "owner_id";
			queryDictionary[new Guid("baef5cbf-2a3a-41df-af69-90acad4bc155")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("baef5cbf-2a3a-41df-af69-90acad4bc155")].QueryType = "EQ";
			queryDictionary[new Guid("baef5cbf-2a3a-41df-af69-90acad4bc155")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("baef5cbf-2a3a-41df-af69-90acad4bc155"))) {queryDictionary[new Guid("baef5cbf-2a3a-41df-af69-90acad4bc155")].SubQueries = subQueryDictionary[new Guid("baef5cbf-2a3a-41df-af69-90acad4bc155")];}
			if(!subQueryDictionary.ContainsKey(new Guid("1dd79432-5d40-4ba6-b19a-dde0eb09148f"))) {subQueryDictionary[new Guid("1dd79432-5d40-4ba6-b19a-dde0eb09148f")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("1dd79432-5d40-4ba6-b19a-dde0eb09148f")].Add(queryDictionary[new Guid("baef5cbf-2a3a-41df-af69-90acad4bc155")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("1dd79432-5d40-4ba6-b19a-dde0eb09148f"))) {queryDictionary[new Guid("1dd79432-5d40-4ba6-b19a-dde0eb09148f")].SubQueries = subQueryDictionary[new Guid("1dd79432-5d40-4ba6-b19a-dde0eb09148f")];}
		if(!subQueryDictionary.ContainsKey(new Guid("b4e1b8f4-c6c3-4daa-bba6-93c1d3d5b74f"))) {subQueryDictionary[new Guid("b4e1b8f4-c6c3-4daa-bba6-93c1d3d5b74f")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("b4e1b8f4-c6c3-4daa-bba6-93c1d3d5b74f")].Add(queryDictionary[new Guid("1dd79432-5d40-4ba6-b19a-dde0eb09148f")]);
		}
		{
		queryDictionary[new Guid("d846ba2c-7536-4b62-be30-45dac64f2070")] = new InputRecordListQuery();
		queryDictionary[new Guid("d846ba2c-7536-4b62-be30-45dac64f2070")].FieldName = null;
		queryDictionary[new Guid("d846ba2c-7536-4b62-be30-45dac64f2070")].FieldValue =  null;
		queryDictionary[new Guid("d846ba2c-7536-4b62-be30-45dac64f2070")].QueryType = "AND";
		queryDictionary[new Guid("d846ba2c-7536-4b62-be30-45dac64f2070")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("ad590664-756c-481a-887d-691bf980528e")] = new InputRecordListQuery();
			queryDictionary[new Guid("ad590664-756c-481a-887d-691bf980528e")].FieldName = "code";
			queryDictionary[new Guid("ad590664-756c-481a-887d-691bf980528e")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("ad590664-756c-481a-887d-691bf980528e")].QueryType = "CONTAINS";
			queryDictionary[new Guid("ad590664-756c-481a-887d-691bf980528e")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("ad590664-756c-481a-887d-691bf980528e"))) {queryDictionary[new Guid("ad590664-756c-481a-887d-691bf980528e")].SubQueries = subQueryDictionary[new Guid("ad590664-756c-481a-887d-691bf980528e")];}
			if(!subQueryDictionary.ContainsKey(new Guid("d846ba2c-7536-4b62-be30-45dac64f2070"))) {subQueryDictionary[new Guid("d846ba2c-7536-4b62-be30-45dac64f2070")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("d846ba2c-7536-4b62-be30-45dac64f2070")].Add(queryDictionary[new Guid("ad590664-756c-481a-887d-691bf980528e")]);
			}
			{
			queryDictionary[new Guid("88a03b6b-dec9-4da0-9894-49e48c857088")] = new InputRecordListQuery();
			queryDictionary[new Guid("88a03b6b-dec9-4da0-9894-49e48c857088")].FieldName = "subject";
			queryDictionary[new Guid("88a03b6b-dec9-4da0-9894-49e48c857088")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("88a03b6b-dec9-4da0-9894-49e48c857088")].QueryType = "FTS";
			queryDictionary[new Guid("88a03b6b-dec9-4da0-9894-49e48c857088")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("88a03b6b-dec9-4da0-9894-49e48c857088"))) {queryDictionary[new Guid("88a03b6b-dec9-4da0-9894-49e48c857088")].SubQueries = subQueryDictionary[new Guid("88a03b6b-dec9-4da0-9894-49e48c857088")];}
			if(!subQueryDictionary.ContainsKey(new Guid("d846ba2c-7536-4b62-be30-45dac64f2070"))) {subQueryDictionary[new Guid("d846ba2c-7536-4b62-be30-45dac64f2070")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("d846ba2c-7536-4b62-be30-45dac64f2070")].Add(queryDictionary[new Guid("88a03b6b-dec9-4da0-9894-49e48c857088")]);
			}
			{
			queryDictionary[new Guid("05ca31ed-31e4-47cd-9a2f-2ec6b9b12748")] = new InputRecordListQuery();
			queryDictionary[new Guid("05ca31ed-31e4-47cd-9a2f-2ec6b9b12748")].FieldName = "status";
			queryDictionary[new Guid("05ca31ed-31e4-47cd-9a2f-2ec6b9b12748")].FieldValue =  "completed";
			queryDictionary[new Guid("05ca31ed-31e4-47cd-9a2f-2ec6b9b12748")].QueryType = "NOT";
			queryDictionary[new Guid("05ca31ed-31e4-47cd-9a2f-2ec6b9b12748")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("05ca31ed-31e4-47cd-9a2f-2ec6b9b12748"))) {queryDictionary[new Guid("05ca31ed-31e4-47cd-9a2f-2ec6b9b12748")].SubQueries = subQueryDictionary[new Guid("05ca31ed-31e4-47cd-9a2f-2ec6b9b12748")];}
			if(!subQueryDictionary.ContainsKey(new Guid("d846ba2c-7536-4b62-be30-45dac64f2070"))) {subQueryDictionary[new Guid("d846ba2c-7536-4b62-be30-45dac64f2070")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("d846ba2c-7536-4b62-be30-45dac64f2070")].Add(queryDictionary[new Guid("05ca31ed-31e4-47cd-9a2f-2ec6b9b12748")]);
			}
			{
			queryDictionary[new Guid("4e99394c-f09b-4e95-b85a-4354a8e579b8")] = new InputRecordListQuery();
			queryDictionary[new Guid("4e99394c-f09b-4e95-b85a-4354a8e579b8")].FieldName = "priority";
			queryDictionary[new Guid("4e99394c-f09b-4e95-b85a-4354a8e579b8")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("4e99394c-f09b-4e95-b85a-4354a8e579b8")].QueryType = "EQ";
			queryDictionary[new Guid("4e99394c-f09b-4e95-b85a-4354a8e579b8")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("4e99394c-f09b-4e95-b85a-4354a8e579b8"))) {queryDictionary[new Guid("4e99394c-f09b-4e95-b85a-4354a8e579b8")].SubQueries = subQueryDictionary[new Guid("4e99394c-f09b-4e95-b85a-4354a8e579b8")];}
			if(!subQueryDictionary.ContainsKey(new Guid("d846ba2c-7536-4b62-be30-45dac64f2070"))) {subQueryDictionary[new Guid("d846ba2c-7536-4b62-be30-45dac64f2070")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("d846ba2c-7536-4b62-be30-45dac64f2070")].Add(queryDictionary[new Guid("4e99394c-f09b-4e95-b85a-4354a8e579b8")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("d846ba2c-7536-4b62-be30-45dac64f2070"))) {queryDictionary[new Guid("d846ba2c-7536-4b62-be30-45dac64f2070")].SubQueries = subQueryDictionary[new Guid("d846ba2c-7536-4b62-be30-45dac64f2070")];}
		if(!subQueryDictionary.ContainsKey(new Guid("b4e1b8f4-c6c3-4daa-bba6-93c1d3d5b74f"))) {subQueryDictionary[new Guid("b4e1b8f4-c6c3-4daa-bba6-93c1d3d5b74f")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("b4e1b8f4-c6c3-4daa-bba6-93c1d3d5b74f")].Add(queryDictionary[new Guid("d846ba2c-7536-4b62-be30-45dac64f2070")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("b4e1b8f4-c6c3-4daa-bba6-93c1d3d5b74f"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("b4e1b8f4-c6c3-4daa-bba6-93c1d3d5b74f")];}
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
		queryDictionary[new Guid("46dab097-00a2-4c3e-bb4f-aa56b7ab9e00")] = new InputRecordListQuery();
		queryDictionary[new Guid("46dab097-00a2-4c3e-bb4f-aa56b7ab9e00")].FieldName = null;
		queryDictionary[new Guid("46dab097-00a2-4c3e-bb4f-aa56b7ab9e00")].FieldValue =  null;
		queryDictionary[new Guid("46dab097-00a2-4c3e-bb4f-aa56b7ab9e00")].QueryType = "OR";
		queryDictionary[new Guid("46dab097-00a2-4c3e-bb4f-aa56b7ab9e00")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("96fbac24-7d26-40d5-ac7b-c339bc9505ca")] = new InputRecordListQuery();
			queryDictionary[new Guid("96fbac24-7d26-40d5-ac7b-c339bc9505ca")].FieldName = "owner_id";
			queryDictionary[new Guid("96fbac24-7d26-40d5-ac7b-c339bc9505ca")].FieldValue =  "{\"name\":\"current_user\", \"option\": \"id\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("96fbac24-7d26-40d5-ac7b-c339bc9505ca")].QueryType = "EQ";
			queryDictionary[new Guid("96fbac24-7d26-40d5-ac7b-c339bc9505ca")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("96fbac24-7d26-40d5-ac7b-c339bc9505ca"))) {queryDictionary[new Guid("96fbac24-7d26-40d5-ac7b-c339bc9505ca")].SubQueries = subQueryDictionary[new Guid("96fbac24-7d26-40d5-ac7b-c339bc9505ca")];}
			if(!subQueryDictionary.ContainsKey(new Guid("46dab097-00a2-4c3e-bb4f-aa56b7ab9e00"))) {subQueryDictionary[new Guid("46dab097-00a2-4c3e-bb4f-aa56b7ab9e00")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("46dab097-00a2-4c3e-bb4f-aa56b7ab9e00")].Add(queryDictionary[new Guid("96fbac24-7d26-40d5-ac7b-c339bc9505ca")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("46dab097-00a2-4c3e-bb4f-aa56b7ab9e00"))) {queryDictionary[new Guid("46dab097-00a2-4c3e-bb4f-aa56b7ab9e00")].SubQueries = subQueryDictionary[new Guid("46dab097-00a2-4c3e-bb4f-aa56b7ab9e00")];}
		if(!subQueryDictionary.ContainsKey(new Guid("67acbea4-7019-40b4-a81f-ffeea352e419"))) {subQueryDictionary[new Guid("67acbea4-7019-40b4-a81f-ffeea352e419")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("67acbea4-7019-40b4-a81f-ffeea352e419")].Add(queryDictionary[new Guid("46dab097-00a2-4c3e-bb4f-aa56b7ab9e00")]);
		}
		{
		queryDictionary[new Guid("2b13394b-010a-4d09-b236-6bef2e595c0c")] = new InputRecordListQuery();
		queryDictionary[new Guid("2b13394b-010a-4d09-b236-6bef2e595c0c")].FieldName = null;
		queryDictionary[new Guid("2b13394b-010a-4d09-b236-6bef2e595c0c")].FieldValue =  null;
		queryDictionary[new Guid("2b13394b-010a-4d09-b236-6bef2e595c0c")].QueryType = "AND";
		queryDictionary[new Guid("2b13394b-010a-4d09-b236-6bef2e595c0c")].SubQueries = new List<InputRecordListQuery>();
			{
			queryDictionary[new Guid("01a69994-d445-4903-a49f-200d181ad294")] = new InputRecordListQuery();
			queryDictionary[new Guid("01a69994-d445-4903-a49f-200d181ad294")].FieldName = "code";
			queryDictionary[new Guid("01a69994-d445-4903-a49f-200d181ad294")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"code\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("01a69994-d445-4903-a49f-200d181ad294")].QueryType = "CONTAINS";
			queryDictionary[new Guid("01a69994-d445-4903-a49f-200d181ad294")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("01a69994-d445-4903-a49f-200d181ad294"))) {queryDictionary[new Guid("01a69994-d445-4903-a49f-200d181ad294")].SubQueries = subQueryDictionary[new Guid("01a69994-d445-4903-a49f-200d181ad294")];}
			if(!subQueryDictionary.ContainsKey(new Guid("2b13394b-010a-4d09-b236-6bef2e595c0c"))) {subQueryDictionary[new Guid("2b13394b-010a-4d09-b236-6bef2e595c0c")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("2b13394b-010a-4d09-b236-6bef2e595c0c")].Add(queryDictionary[new Guid("01a69994-d445-4903-a49f-200d181ad294")]);
			}
			{
			queryDictionary[new Guid("a9a2af85-4685-47c4-865b-eddf16c14ba8")] = new InputRecordListQuery();
			queryDictionary[new Guid("a9a2af85-4685-47c4-865b-eddf16c14ba8")].FieldName = "subject";
			queryDictionary[new Guid("a9a2af85-4685-47c4-865b-eddf16c14ba8")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("a9a2af85-4685-47c4-865b-eddf16c14ba8")].QueryType = "FTS";
			queryDictionary[new Guid("a9a2af85-4685-47c4-865b-eddf16c14ba8")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("a9a2af85-4685-47c4-865b-eddf16c14ba8"))) {queryDictionary[new Guid("a9a2af85-4685-47c4-865b-eddf16c14ba8")].SubQueries = subQueryDictionary[new Guid("a9a2af85-4685-47c4-865b-eddf16c14ba8")];}
			if(!subQueryDictionary.ContainsKey(new Guid("2b13394b-010a-4d09-b236-6bef2e595c0c"))) {subQueryDictionary[new Guid("2b13394b-010a-4d09-b236-6bef2e595c0c")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("2b13394b-010a-4d09-b236-6bef2e595c0c")].Add(queryDictionary[new Guid("a9a2af85-4685-47c4-865b-eddf16c14ba8")]);
			}
			{
			queryDictionary[new Guid("245ad21c-be5d-4d7a-a290-9eae2e75887e")] = new InputRecordListQuery();
			queryDictionary[new Guid("245ad21c-be5d-4d7a-a290-9eae2e75887e")].FieldName = "status";
			queryDictionary[new Guid("245ad21c-be5d-4d7a-a290-9eae2e75887e")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("245ad21c-be5d-4d7a-a290-9eae2e75887e")].QueryType = "EQ";
			queryDictionary[new Guid("245ad21c-be5d-4d7a-a290-9eae2e75887e")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("245ad21c-be5d-4d7a-a290-9eae2e75887e"))) {queryDictionary[new Guid("245ad21c-be5d-4d7a-a290-9eae2e75887e")].SubQueries = subQueryDictionary[new Guid("245ad21c-be5d-4d7a-a290-9eae2e75887e")];}
			if(!subQueryDictionary.ContainsKey(new Guid("2b13394b-010a-4d09-b236-6bef2e595c0c"))) {subQueryDictionary[new Guid("2b13394b-010a-4d09-b236-6bef2e595c0c")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("2b13394b-010a-4d09-b236-6bef2e595c0c")].Add(queryDictionary[new Guid("245ad21c-be5d-4d7a-a290-9eae2e75887e")]);
			}
			{
			queryDictionary[new Guid("3fe94aa2-0147-42e6-b0fd-252c9a68cf10")] = new InputRecordListQuery();
			queryDictionary[new Guid("3fe94aa2-0147-42e6-b0fd-252c9a68cf10")].FieldName = "priority";
			queryDictionary[new Guid("3fe94aa2-0147-42e6-b0fd-252c9a68cf10")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
			queryDictionary[new Guid("3fe94aa2-0147-42e6-b0fd-252c9a68cf10")].QueryType = "EQ";
			queryDictionary[new Guid("3fe94aa2-0147-42e6-b0fd-252c9a68cf10")].SubQueries = new List<InputRecordListQuery>();
			if(subQueryDictionary.ContainsKey(new Guid("3fe94aa2-0147-42e6-b0fd-252c9a68cf10"))) {queryDictionary[new Guid("3fe94aa2-0147-42e6-b0fd-252c9a68cf10")].SubQueries = subQueryDictionary[new Guid("3fe94aa2-0147-42e6-b0fd-252c9a68cf10")];}
			if(!subQueryDictionary.ContainsKey(new Guid("2b13394b-010a-4d09-b236-6bef2e595c0c"))) {subQueryDictionary[new Guid("2b13394b-010a-4d09-b236-6bef2e595c0c")] = new List<InputRecordListQuery>();}
			subQueryDictionary[new Guid("2b13394b-010a-4d09-b236-6bef2e595c0c")].Add(queryDictionary[new Guid("3fe94aa2-0147-42e6-b0fd-252c9a68cf10")]);
			}
		if(subQueryDictionary.ContainsKey(new Guid("2b13394b-010a-4d09-b236-6bef2e595c0c"))) {queryDictionary[new Guid("2b13394b-010a-4d09-b236-6bef2e595c0c")].SubQueries = subQueryDictionary[new Guid("2b13394b-010a-4d09-b236-6bef2e595c0c")];}
		if(!subQueryDictionary.ContainsKey(new Guid("67acbea4-7019-40b4-a81f-ffeea352e419"))) {subQueryDictionary[new Guid("67acbea4-7019-40b4-a81f-ffeea352e419")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("67acbea4-7019-40b4-a81f-ffeea352e419")].Add(queryDictionary[new Guid("2b13394b-010a-4d09-b236-6bef2e595c0c")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("67acbea4-7019-40b4-a81f-ffeea352e419"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("67acbea4-7019-40b4-a81f-ffeea352e419")];}
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
		queryDictionary[new Guid("1b4cd0a2-6a60-4b3f-9828-5b174abaa9d1")] = new InputRecordListQuery();
		queryDictionary[new Guid("1b4cd0a2-6a60-4b3f-9828-5b174abaa9d1")].FieldName = "subject";
		queryDictionary[new Guid("1b4cd0a2-6a60-4b3f-9828-5b174abaa9d1")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"subject\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("1b4cd0a2-6a60-4b3f-9828-5b174abaa9d1")].QueryType = "FTS";
		queryDictionary[new Guid("1b4cd0a2-6a60-4b3f-9828-5b174abaa9d1")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("1b4cd0a2-6a60-4b3f-9828-5b174abaa9d1"))) {queryDictionary[new Guid("1b4cd0a2-6a60-4b3f-9828-5b174abaa9d1")].SubQueries = subQueryDictionary[new Guid("1b4cd0a2-6a60-4b3f-9828-5b174abaa9d1")];}
		if(!subQueryDictionary.ContainsKey(new Guid("590f73f3-7a43-444a-8657-d64e93cdf546"))) {subQueryDictionary[new Guid("590f73f3-7a43-444a-8657-d64e93cdf546")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("590f73f3-7a43-444a-8657-d64e93cdf546")].Add(queryDictionary[new Guid("1b4cd0a2-6a60-4b3f-9828-5b174abaa9d1")]);
		}
		{
		queryDictionary[new Guid("494e5943-b2f1-4656-854d-aa37aa7482dc")] = new InputRecordListQuery();
		queryDictionary[new Guid("494e5943-b2f1-4656-854d-aa37aa7482dc")].FieldName = "status";
		queryDictionary[new Guid("494e5943-b2f1-4656-854d-aa37aa7482dc")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"status\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("494e5943-b2f1-4656-854d-aa37aa7482dc")].QueryType = "EQ";
		queryDictionary[new Guid("494e5943-b2f1-4656-854d-aa37aa7482dc")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("494e5943-b2f1-4656-854d-aa37aa7482dc"))) {queryDictionary[new Guid("494e5943-b2f1-4656-854d-aa37aa7482dc")].SubQueries = subQueryDictionary[new Guid("494e5943-b2f1-4656-854d-aa37aa7482dc")];}
		if(!subQueryDictionary.ContainsKey(new Guid("590f73f3-7a43-444a-8657-d64e93cdf546"))) {subQueryDictionary[new Guid("590f73f3-7a43-444a-8657-d64e93cdf546")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("590f73f3-7a43-444a-8657-d64e93cdf546")].Add(queryDictionary[new Guid("494e5943-b2f1-4656-854d-aa37aa7482dc")]);
		}
		{
		queryDictionary[new Guid("711d4838-ba27-4f85-8f93-ae65fd42defa")] = new InputRecordListQuery();
		queryDictionary[new Guid("711d4838-ba27-4f85-8f93-ae65fd42defa")].FieldName = "priority";
		queryDictionary[new Guid("711d4838-ba27-4f85-8f93-ae65fd42defa")].FieldValue =  "{\"name\":\"url_query\", \"option\": \"priority\", \"default\": null, \"settings\":{}}";
		queryDictionary[new Guid("711d4838-ba27-4f85-8f93-ae65fd42defa")].QueryType = "EQ";
		queryDictionary[new Guid("711d4838-ba27-4f85-8f93-ae65fd42defa")].SubQueries = new List<InputRecordListQuery>();
		if(subQueryDictionary.ContainsKey(new Guid("711d4838-ba27-4f85-8f93-ae65fd42defa"))) {queryDictionary[new Guid("711d4838-ba27-4f85-8f93-ae65fd42defa")].SubQueries = subQueryDictionary[new Guid("711d4838-ba27-4f85-8f93-ae65fd42defa")];}
		if(!subQueryDictionary.ContainsKey(new Guid("590f73f3-7a43-444a-8657-d64e93cdf546"))) {subQueryDictionary[new Guid("590f73f3-7a43-444a-8657-d64e93cdf546")] = new List<InputRecordListQuery>();}
		subQueryDictionary[new Guid("590f73f3-7a43-444a-8657-d64e93cdf546")].Add(queryDictionary[new Guid("711d4838-ba27-4f85-8f93-ae65fd42defa")]);
		}
	if(subQueryDictionary.ContainsKey(new Guid("590f73f3-7a43-444a-8657-d64e93cdf546"))) {createListInput.Query.SubQueries = subQueryDictionary[new Guid("590f73f3-7a43-444a-8657-d64e93cdf546")];}
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



		}


    }
}
