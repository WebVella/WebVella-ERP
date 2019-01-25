using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Eql;
using WebVella.Erp.Hooks;
using WebVella.Erp.Plugins.Project.Services;

namespace WebVella.Erp.Plugins.Project.Hooks.Api
{
	[HookAttachment("comment")]
	public class CommentHook : IErpPreCreateRecordHook
	{

		public void OnPreCreateRecord(string entityName, EntityRecord record, List<ErrorModel> errors)
		{
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
				if (!String.IsNullOrWhiteSpace((string)record["body"])){
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
					body:body,relatedRecords: relatedRecords, scope: scope, type: "comment");
			}

		}

	}
}
