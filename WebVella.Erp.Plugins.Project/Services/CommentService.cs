using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Eql;
using WebVella.Erp.Exceptions;


//TODO develop service
namespace WebVella.Erp.Plugins.Project.Services
{
	public class CommentService : BaseService
	{
		public void Create(Guid? id = null, Guid? createdBy = null, DateTime? createdOn = null, string body = "", Guid? parentId = null, 
			List<string> scope = null, List<Guid> relatedRecords = null)
		{
			#region << Init >>
			if (id == null)
				id = Guid.NewGuid();

			if (createdBy == null)
				createdBy = SystemIds.SystemUserId;

			if (createdOn == null)
				createdOn = DateTime.UtcNow;
			#endregion

			try
			{
				var record = new EntityRecord();
				record["id"] = id;
				record["created_by"] = createdBy;
				record["created_on"] = createdOn;
				record["body"] = body;
				record["parent_id"] = parentId;
				record["l_scope"] = JsonConvert.SerializeObject(scope);
				record["l_related_records"] = JsonConvert.SerializeObject(relatedRecords);

				var response = RecMan.CreateRecord("comment", record);
				if (!response.Success)
				{
					throw new ValidationException(response.Message);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void Delete(Guid recordId) {
			//Validate - only authors can start to delete their posts and comments. Moderation will be later added if needed
			{
				var eqlCommand = "SELECT id,created_by FROM comment WHERE id = @commentId";
				var eqlParams = new List<EqlParameter>() { new EqlParameter("commentId", recordId) };
				var eqlResult = new EqlCommand(eqlCommand, eqlParams).Execute();
				if (!eqlResult.Any())
					throw new Exception("RecordId not found");
				if ((Guid)eqlResult[0]["created_by"] != SecurityContext.CurrentUser.Id)
					throw new Exception("Only the author can delete its comment");
			}

			var commentIdListForDeletion = new List<Guid>();
			//Add requested
			commentIdListForDeletion.Add(recordId);

			//Find and add all the child comments
			//TODO currently only on level if comment nesting is implemented. If it is increased this method should be changed
			{
				var eqlCommand = "SELECT id FROM comment WHERE parent_id = @commentId";
				var eqlParams = new List<EqlParameter>() { new EqlParameter("commentId", recordId) };
				var eqlResult = new EqlCommand(eqlCommand, eqlParams).Execute();
				foreach (var childComment in eqlResult) {
					commentIdListForDeletion.Add((Guid)childComment["id"]);
				}
			}

			//Create transaction 

			//Trigger delete
			foreach (var commentId in commentIdListForDeletion)
			{
	
				//Remove case relations
				//TODO


				var deleteResponse = new RecordManager().DeleteRecord("comment", commentId);
				if (!deleteResponse.Success)
				{
					throw new Exception(deleteResponse.Message);
				}
			}
						


		}

		public void PreCreateApiHookLogic(string entityName, EntityRecord record, List<ErrorModel> errors) {
			var isProjectTimeLog = false;
			var relatedTaskRecords = new EntityRecordList();
			//Get timelog
			if (record["l_scope"] != null && ((string)record["l_scope"]).Contains("projects"))
			{
				isProjectTimeLog = true;
			}

			if (isProjectTimeLog)
			{
				//Get related tasks from related records field
				if (record["l_related_records"] != null && (string)record["l_related_records"] != "")
				{
					try
					{
						var relatedRecordGuid = JsonConvert.DeserializeObject<List<Guid>>((string)record["l_related_records"]);
						var taskEqlCommand = "SELECT *,$project_nn_task.id from task WHERE ";
						var filterStringList = new List<string>();
						var taskEqlParams = new List<EqlParameter>();
						var index = 1;
						foreach (var taskGuid in relatedRecordGuid)
						{
							var paramName = "taskId" + index;
							filterStringList.Add($" id = @{paramName} ");
							taskEqlParams.Add(new EqlParameter(paramName, taskGuid));
							index++;
						}
						taskEqlCommand += String.Join(" OR ", filterStringList);
						relatedTaskRecords = new EqlCommand(taskEqlCommand, taskEqlParams).Execute();
					}
					catch
					{
						//Do nothing
					}
				}
				if (!relatedTaskRecords.Any())
					throw new Exception("Hook exception: This comment is a project comment but does not have an existing taskId related");

				var taskRecord = relatedTaskRecords[0]; //Currently should be related only to 1 task in projects

				//Get Project Id
				Guid? projectId = null;
				if (((List<EntityRecord>)taskRecord["$project_nn_task"]).Any())
				{
					var projectRecord = ((List<EntityRecord>)taskRecord["$project_nn_task"]).First();
					if (projectRecord != null)
					{
						projectId = (Guid)projectRecord["id"];
					}
				}


				//Add activity log
				var subject = $"commented on <a href=\"/projects/tasks/tasks/r/{taskRecord["id"]}/details\">[{taskRecord["key"]}] {taskRecord["subject"]}</a>";
				var relatedRecords = new List<string>() { taskRecord["id"].ToString(), record["id"].ToString() };
				if (projectId != null)
				{
					relatedRecords.Add(projectId.ToString());
				}
				var body = "";
				if (!String.IsNullOrWhiteSpace((string)record["body"]))
				{
					HtmlDocument doc = new HtmlDocument();
					doc.LoadHtml((string)record["body"]);
					var root = doc.DocumentNode;
					var sb = new StringBuilder();
					foreach (var node in root.DescendantsAndSelf())
					{
						if (!node.HasChildNodes)
						{
							string text = node.InnerText;
							if (!string.IsNullOrEmpty(text))
								sb.AppendLine(text.Trim());
						}
					}
					var commentText = sb.ToString();
					if (commentText.Length > 150)
					{
						body = sb.ToString().Substring(0, 150);
						if (commentText.Length > 150)
						{
							body += "...";
						}
					}
				}
				var scope = new List<string>() { "projects" };
				new FeedItemService().Create(id: Guid.NewGuid(), createdBy: SecurityContext.CurrentUser.Id, subject: subject,
					body: body, relatedRecords: relatedRecords, scope: scope, type: "comment");
			}

		}
	}
}
