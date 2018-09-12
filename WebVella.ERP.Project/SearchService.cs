using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Database;
using WebVella.ERP.Jobs;
namespace WebVella.ERP.Project
{
    public class SearchService
    {

		[JobAttribute("4E02D675-D02A-4211-837E-46C99A4CDE07", "SearchIndex", AllowSingleInstance = true)]
		public static void SearchIndex(JobContext context)
		{
			using (var dbCtx = DbContext.CreateContext(Settings.ConnectionString))
			{
				using (SecurityContext.OpenSystemScope())
				{
					var allIndexRecords = new List<EntityRecord>();
					var indexDictionary = new Dictionary<Guid,EntityRecord>();
					var allTasksRecords = new List<EntityRecord>();
					var allBugsRecords = new List<EntityRecord>();
					var indexIdsForDeletion = new List<Guid>();
					var searchRecordForCreation = new List<EntityRecord>();
					var recMan = new RecordManager();

					#region << Get allIndexRecords >>
					{
						var fields = "*";
						var result = recMan.Find(new EntityQuery("search",fields));
						if (result.Success)
						{
							allIndexRecords = result.Object.Data;
						}
						else { 
							throw new Exception(result.Message);
						}
					}
					#endregion

					#region << Get allTasksRecords >>
					{
						var fields = "*,$task_1_n_attachment.comment_content,$task_1_n_attachment.file,$task_1_n_comment.content";
						var result = recMan.Find(new EntityQuery("wv_task",fields));
						if (result.Success)
						{
							allTasksRecords = result.Object.Data;
						}
						else { 
							throw new Exception(result.Message);
						}
					}
					#endregion

					#region << Get allBugsRecords >>
					{
						var fields = "*,$bug_1_n_attachment.comment_content,$bug_1_n_attachment.file,$bug_1_n_comment.content";
						var result = recMan.Find(new EntityQuery("wv_bug",fields));
						if (result.Success)
						{
							allBugsRecords = result.Object.Data;
						}
						else { 
							throw new Exception(result.Message);
						}
					}
					#endregion

					#region << allIndexRecords >>

					foreach (var record in allIndexRecords)
					{
						var recordId = (Guid)record["item_id"];
						indexDictionary[recordId] = record;
					}

					#endregion

					#region << Process Tasks >>
					foreach (var record in allTasksRecords)
					{
						var recordId = (Guid)record["id"];
						var indexDate = (DateTime?)record["indexed_on"];
						EntityRecord indexedRecord = null;
						if(indexDictionary.ContainsKey(recordId)) {
							indexedRecord = indexDictionary[recordId];
						}

						//new record
						if(indexedRecord == null) {
							//new record will be created down						
						}
						else if(indexedRecord != null && indexDate == null) {
							//This is a possible problem. Best way is to recreate the the index
							indexIdsForDeletion.Add((Guid)indexedRecord["id"]);			
						}
						//updated record
						else if(indexedRecord != null && (DateTime)indexedRecord["created_on"] < indexDate) {
							//Mark the old index for deletion
							indexIdsForDeletion.Add((Guid)indexedRecord["id"]);
						}
						// index has the most recent data
						else { 
							//mark as processed and do nothing
							indexDictionary.Remove(recordId);
							continue;
						}

						var newIndexRecord = new EntityRecord();
						newIndexRecord["id"] =  Guid.NewGuid();
						newIndexRecord["item_id"] = recordId;
						newIndexRecord["entity"] = "task";
						newIndexRecord["index"] = "";
						newIndexRecord["project_id"] = (Guid)record["project_id"];
						newIndexRecord["title"] = "["+(string)record["code"]+"] " + (string)record["subject"];
						newIndexRecord["snippet"] = Utils.TextLength(Utils.RemoveHtml((string)record["description"]),"short");
						newIndexRecord["url"] = "/#/areas/projects/wv_task/view-general/sb/general/" + recordId;

						#region << Index >>
						var searchIndex = "";
						//Code
						searchIndex += " " + Utils.RemoveHtml((string)record["code"]) + " ";
						//Subject
						searchIndex += " " + Utils.RemoveHtml((string)record["subject"]) + " ";
						//Description
						searchIndex += " " + Utils.RemoveHtml((string)record["description"]) + " ";
						// Attachments
						var attachments = (List<EntityRecord>)record["$task_1_n_attachment"];
						foreach (var attachment in attachments)
						{
							//Comment Content
							searchIndex += " " + Utils.RemoveHtml((string)attachment["comment_content"]) + " ";
							//File name
							searchIndex += " " + Utils.RemoveHtml((string)attachment["file"]) + " ";
						}


						//Comments
						var comments = (List<EntityRecord>)record["$task_1_n_comment"];
						foreach (var comment in comments)
						{
							//Comment Content
							searchIndex += " " + Utils.RemoveHtml((string)comment["content"]) + " ";
						}

						newIndexRecord["index"] = searchIndex.ToLowerInvariant();
						#endregion

						searchRecordForCreation.Add(newIndexRecord);
					}
					#endregion

					#region << Process Bugs >>
					foreach (var record in allBugsRecords)
					{
						var recordId = (Guid)record["id"];
						var indexDate = (DateTime?)record["indexed_on"];
						EntityRecord indexedRecord = null;
						if(indexDictionary.ContainsKey(recordId)) {
							indexedRecord = indexDictionary[recordId];
						}

						//new record
						if(indexedRecord == null) {
							//new record will be created down						
						}
						else if(indexedRecord != null && indexDate == null) {
							//This is a possible problem. Best way is to recreate the the index
							indexIdsForDeletion.Add((Guid)indexedRecord["id"]);			
						}
						//updated record
						else if(indexedRecord != null && (DateTime)indexedRecord["created_on"] < indexDate) {
							//Mark the old index for deletion
							indexIdsForDeletion.Add((Guid)indexedRecord["id"]);
						}
						// index has the most recent data
						else { 
							//mark as processed and do nothing
							indexDictionary.Remove(recordId);
							continue;
						}

						var newIndexRecord = new EntityRecord();
						newIndexRecord["id"] = Guid.NewGuid();
						newIndexRecord["item_id"] = recordId;
						newIndexRecord["entity"] = "bug";
						newIndexRecord["index"] = "";
						newIndexRecord["project_id"] = (Guid)record["project_id"];
						newIndexRecord["title"] = "["+(string)record["code"]+"] " + (string)record["subject"];
						newIndexRecord["snippet"] = Utils.TextLength(Utils.RemoveHtml((string)record["description"]),"short");
						newIndexRecord["url"] = "/#/areas/projects/wv_bug/view-general/sb/general/" + recordId;

						#region << Index >>
						var searchIndex = "";
						//Code
						searchIndex += " " + Utils.RemoveHtml((string)record["code"]) + " ";
						//Subject
						searchIndex += " " + Utils.RemoveHtml((string)record["subject"]) + " ";
						//Description
						searchIndex += " " + Utils.RemoveHtml((string)record["description"]) + " ";
						// Attachments
						var attachments = (List<EntityRecord>)record["$bug_1_n_attachment"];
						foreach (var attachment in attachments)
						{
							//Comment Content
							searchIndex += " " + Utils.RemoveHtml((string)attachment["comment_content"]) + " ";
							//File name
							searchIndex += " " + Utils.RemoveHtml((string)attachment["file"]) + " ";
						}


						//Comments
						var comments = (List<EntityRecord>)record["$bug_1_n_comment"];
						foreach (var comment in comments)
						{
							//Comment Content
							searchIndex += " " + Utils.RemoveHtml((string)comment["content"]) + " ";
						}

						newIndexRecord["index"] = searchIndex.ToLowerInvariant();
						#endregion

						searchRecordForCreation.Add(newIndexRecord);
					}
					#endregion

					#region << Delete not needed records >>
					foreach (var recordId in indexIdsForDeletion)
					{
						var result = recMan.DeleteRecord("search",recordId);
						if(!result.Success) {
							throw new Exception(result.Message);
						}
					}

					#endregion

					#region << Add new records >>
					foreach (var record in searchRecordForCreation)
					{
						var result = recMan.CreateRecord("search",record,true);
						if(!result.Success) {
							throw new Exception(result.Message);
						}
					}

					#endregion

				}
			}
		}


		public static List<Guid> ProjectsUserCanAccess(ErpUser user) {
			var result = new List<Guid>();
			QueryResponse queryResponse = new RecordManager().Find(new EntityQuery("wv_project","id,$user_1_n_project_owner.id,$role_n_n_project_team.id,$role_n_n_project_customer.id"));
			if (!queryResponse.Success)
			{
				throw new Exception(queryResponse.Message);
			}

			foreach (var record in queryResponse.Object.Data)
			{
				var userIsPM = false;
				var userIsStaff = false;
				var userIsCustomer = false;
				foreach (var userRole in user.Roles)
				{
					if (!userIsPM)
					{
						userIsPM = ((List<EntityRecord>)record["$user_1_n_project_owner"]).Any(z => (Guid)z["id"] == user.Id);
					}
					if (!userIsStaff)
					{
						userIsStaff = ((List<EntityRecord>)record["$role_n_n_project_team"]).Any(z => (Guid)z["id"] == userRole.Id);
					}
					if (!userIsCustomer)
					{
						userIsCustomer = ((List<EntityRecord>)record["$role_n_n_project_customer"]).Any(z => (Guid)z["id"] == userRole.Id);
					}
				}

				if (userIsPM || userIsStaff || userIsCustomer)
				{
					result.Add((Guid)record["id"]);
				}

			}
			return result;
		}

    }
}
