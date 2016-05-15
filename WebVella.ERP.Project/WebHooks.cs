using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Projects;
using WebVella.ERP.WebHooks;

namespace WebVella.ERP.Project
{

	public class WebHooks
	{

		RecordManager recMan;
		EntityManager entMan;
		EntityRelationManager relMan;
		SecurityManager secMan;

		public WebHooks()
		{
			recMan = new RecordManager();
			secMan = new SecurityManager();
			entMan = new EntityManager();
			relMan = new EntityRelationManager();
		}

		#region << Task >>

		#region << Create >>

		//[WebHook("create_record_validation_errors", "wv_task")] //<<<< UNCOMMENT TO HOOK
		public dynamic TaskCreateValidateFilter(dynamic data)
		{
			var errors = (List<ErrorModel>)data.errors;
			var record = (EntityRecord)data.record;
			var recordId = (Guid)data.recordId;
			var newErrorsList = Utils.ValidateTask(errors, record, recordId);
			data.errors = newErrorsList;
			return data;
		}

		[WebHook("create_record_pre_save", "wv_task")]
		public dynamic TaskCreateRecordPreSave(dynamic data)
		{
			EntityRecord record = (EntityRecord)data.record;
			record["project_id"] = new Guid((string)record["$project_1_n_task.id"]);
			record.Properties.Remove("$project_1_n_task.id");

			#region << Get project owner and set as ticket owner >>
			EntityRecord projectObject = null;
			{
				var filterObj = EntityQuery.QueryEQ("id", (Guid)record["project_id"]);
				var query = new EntityQuery("wv_project", "*", filterObj, null, null, null);
				var result = recMan.Find(query);
				if (!result.Success)
				{

					throw new Exception("Error getting the project: " + result.Message);
				}
				else if (result.Object == null || result.Object.Data == null || !result.Object.Data.Any())
				{
					throw new Exception("Project not found");
				}
				projectObject = result.Object.Data[0];
				record["owner_id"] = (Guid)result.Object.Data[0]["owner_id"];
			}
			#endregion

			#region << Increase the project counter >>
			var patchObject = new EntityRecord();
			patchObject["id"] = (Guid)projectObject["id"];
			switch ((string)record["status"])
			{
				case "not started":
					patchObject["x_tasks_not_started"] = (decimal)projectObject["x_tasks_not_started"] + 1;
					break;
				case "in progress":
					patchObject["x_tasks_in_progress"] = (decimal)projectObject["x_tasks_in_progress"] + 1;
					break;
				case "completed":
					patchObject["x_tasks_completed"] = (decimal)projectObject["x_tasks_completed"] + 1;
					break;
			}
			var updateResponse = recMan.UpdateRecord("wv_project", patchObject);
			if (!updateResponse.Success)
			{
				throw new Exception(updateResponse.Message);
			}

			#endregion

			return data;
		}

		[WebHook("create_record", "wv_task")]
		public void TaskCreateRecordAction(dynamic data)
		{
			var record = (EntityRecord)data.record;
			var createResult = (QueryResponse)data.result;
			var createdRecord = createResult.Object.Data[0];
			Utils.CreateActivity(recMan, "created", "created a <i class='fa fa-fw fa-tasks go-purple'></i> task #" + createdRecord["number"] + " <a href='/#/areas/projects/wv_task/view-general/sb/general/" + createdRecord["id"] + "'>" + System.Net.WebUtility.HtmlEncode((string)createdRecord["subject"]) + "</a>", null, (Guid)createdRecord["project_id"], (Guid)createdRecord["id"], null);

			#region << Add the task owner and creator in the watch list>>

			#region << Add creator in watch list >>
			{
				var targetRelation = relMan.Read("user_n_n_task_watchers").Object;
				var createRelationNtoNResponse = recMan.CreateRelationManyToManyRecord(targetRelation.Id, (Guid)record["created_by"], (Guid)record["id"]);
				if (!createRelationNtoNResponse.Success)
				{
					throw new Exception("Could not create watch relation" + createRelationNtoNResponse.Message);
				}
			}
			#endregion

			#region << Add creator in watch list >>
			{
				if ((Guid)record["created_by"] != (Guid)record["owner_id"])
				{
					var targetRelation = relMan.Read("user_n_n_task_watchers").Object;
					var createRelationNtoNResponse = recMan.CreateRelationManyToManyRecord(targetRelation.Id, (Guid)record["owner_id"], (Guid)record["id"]);
					if (!createRelationNtoNResponse.Success)
					{
						throw new Exception("Could not create watch relation" + createRelationNtoNResponse.Message);
					}
				}
			}
			#endregion

			#endregion

		}

		#endregion

		#region << Update >>

		//[WebHook("update_record_validation_errors", "wv_task")] //<<<< UNCOMMENT TO HOOK
		public dynamic TaskUpdateValidateFilter(dynamic data)
		{
			var errors = (List<ErrorModel>)data.errors;
			var record = (EntityRecord)data.record;
			var recordId = (Guid)data.recordId;
			var newErrorsList = Utils.ValidateTask(errors, record, recordId);
			data.errors = newErrorsList;
			return data;
		}

		[WebHook("update_record_pre_save", "wv_task")]
		public dynamic TaskUpdateRecordPreSave(dynamic data)
		{
			data = Utils.UpdateTask(data, recMan);
			return data;
		}

		#endregion

		#region << Patch >>

		//[WebHook("patch_record_validation_errors", "wv_task")] //<<<< UNCOMMENT TO HOOK
		public dynamic TaskPatchValidateFilter(dynamic data)
		{
			var errors = (List<ErrorModel>)data.errors;
			var record = (EntityRecord)data.record;
			var recordId = (Guid)data.recordId;
			var newErrorsList = Utils.ValidateTask(errors, record, recordId);
			data.errors = newErrorsList;
			return data;
		}

		[WebHook("patch_record_pre_save", "wv_task")]
		public dynamic TaskPatchRecordPreSave(dynamic data)
		{
			data = Utils.UpdateTask(data, recMan);
			return data;
		}

		#endregion


		#endregion

		#region << Bug >>

		#region << Create >>
		[WebHook("create_record_pre_save", "wv_bug")]
		public dynamic BugCreateRecordPreSave(dynamic data)
		{
			EntityRecord record = (EntityRecord)data.record;
			record["project_id"] = new Guid((string)record["$project_1_n_bug.id"]);
			record.Properties.Remove("$project_1_n_bug.id");

			#region << Get project owner and set as ticket owner >>
			EntityRecord projectObject = null;
			{
				var filterObj = EntityQuery.QueryEQ("id", (Guid)record["project_id"]);
				var query = new EntityQuery("wv_project", "*", filterObj, null, null, null);
				var result = recMan.Find(query);
				if (!result.Success)
				{

					throw new Exception("Error getting the project: " + result.Message);
				}
				else if (result.Object == null || result.Object.Data == null || !result.Object.Data.Any())
				{
					throw new Exception("Project not found");
				}
				projectObject = result.Object.Data[0];
				record["owner_id"] = (Guid)result.Object.Data[0]["owner_id"];
			}
			#endregion

			#region << Increase the project counter >>
			var patchObject = new EntityRecord();
			patchObject["id"] = (Guid)projectObject["id"];
			switch ((string)record["status"])
			{
				case "opened":
					patchObject["x_bugs_opened"] = (decimal)projectObject["x_bugs_opened"] + 1;
					break;
				case "closed":
					patchObject["x_bugs_closed"] = (decimal)projectObject["x_bugs_closed"] + 1;
					break;
				case "completed":
					patchObject["x_bugs_reopened"] = (decimal)projectObject["x_bugs_reopened"] + 1;
					break;
			}
			var updateResponse = recMan.UpdateRecord("wv_project", patchObject);
			if (!updateResponse.Success)
			{
				throw new Exception(updateResponse.Message);
			}

			#endregion

			return data;
		}

		[WebHook("create_record", "wv_bug")]
		public void BugCreateRecordAction(dynamic data)
		{
			var record = (EntityRecord)data.record;
			var createResult = (QueryResponse)data.result;
			var createdRecord = createResult.Object.Data[0];
			Utils.CreateActivity(recMan, "created", "created a <i class='fa fa-fw fa-bug go-red'></i> bug #" + createdRecord["number"] + " <a href='/#/areas/projects/wv_bug/view-general/sb/general/" + createdRecord["id"] + "'>" + System.Net.WebUtility.HtmlEncode((string)createdRecord["subject"]) + "</a>", null, (Guid)createdRecord["project_id"], null, (Guid)createdRecord["id"]);

			#region << Add the task owner and creator in the watch list>>

			#region << Add creator in watch list >>
			{
				var targetRelation = relMan.Read("user_n_n_bug_watchers").Object;
				var createRelationNtoNResponse = recMan.CreateRelationManyToManyRecord(targetRelation.Id, (Guid)record["created_by"], (Guid)record["id"]);
				if (!createRelationNtoNResponse.Success)
				{
					throw new Exception("Could not create watch relation" + createRelationNtoNResponse.Message);
				}
			}
			#endregion

			#region << Add creator in watch list >>
			{
				if ((Guid)record["created_by"] != (Guid)record["owner_id"])
				{
					var targetRelation = relMan.Read("user_n_n_bug_watchers").Object;
					var createRelationNtoNResponse = recMan.CreateRelationManyToManyRecord(targetRelation.Id, (Guid)record["owner_id"], (Guid)record["id"]);
					if (!createRelationNtoNResponse.Success)
					{
						throw new Exception("Could not create watch relation" + createRelationNtoNResponse.Message);
					}
				}
			}
			#endregion

			#endregion
		}
		#endregion


		#region << Update >>
		[WebHook("update_record_pre_save", "wv_bug")]
		public dynamic BugUpdateRecordPreSave(dynamic data)
		{
			data = Utils.UpdateBug(data, recMan);
			return data;
		}
		#endregion

		#region << Patch >>
		[WebHook("patch_record_pre_save", "wv_bug")]
		public dynamic BugPatchRecordPreSave(dynamic data)
		{
			data = Utils.UpdateBug(data, recMan);
			return data;
		}
		#endregion

		#endregion

		#region << Time log >>

		[WebHook("create_record", "wv_timelog")]
		public void TimelogCreateRecordAction(dynamic data)
		{
			var record = (EntityRecord)data.record;
			var createResult = (QueryResponse)data.result;
			var createdRecord = createResult.Object.Data[0];
			var billableString = "not billable";
			if ((bool)createdRecord["billable"])
			{
				billableString = "billable";
			}
			if (createdRecord["task_id"] != null)
			{
				var filterObj = EntityQuery.QueryEQ("id", (Guid)createdRecord["task_id"]);
				var query = new EntityQuery("wv_task", "*", filterObj, null, null, null);
				var result = recMan.Find(query);
				if (result.Success)
				{
					var task = result.Object.Data[0];
					Utils.CreateActivity(recMan, "timelog", "created a <i class='fa fa-fw fa-clock-o go-blue'></i> time log of <b>" + ((decimal)createdRecord["hours"]).ToString("N2") + " " + billableString + "</b> hours for task #" + task["number"] + " <a href='/#/areas/projects/wv_task/view-general/sb/general/" + task["id"] + "'>" + System.Net.WebUtility.HtmlEncode((string)task["subject"]) + "</a>", null, (Guid)task["project_id"], (Guid)task["id"], null);
				}
			}
			else if (createdRecord["bug_id"] != null)
			{
				var filterObj = EntityQuery.QueryEQ("id", (Guid)createdRecord["bug_id"]);
				var query = new EntityQuery("wv_bug", "*", filterObj, null, null, null);
				var result = recMan.Find(query);
				if (result.Success)
				{
					var bug = result.Object.Data[0];
					Utils.CreateActivity(recMan, "timelog", "created a <i class='fa fa-fw fa-clock-o go-blue'></i> time log of <b>" + ((decimal)createdRecord["hours"]).ToString("N2") + " " + billableString + "</b> hours  for bug #" + bug["number"] + " <a href='/#/areas/projects/wv_bug/view-general/sb/general/" + bug["id"] + "'>" + System.Net.WebUtility.HtmlEncode((string)bug["subject"]) + "</a>", null, (Guid)bug["project_id"], null, (Guid)bug["id"]);
				}
			}
		}

		#endregion

		#region << Comment >>

		[WebHook("create_record", "wv_project_comment")]
		public void CommentCreateRecordAction(dynamic data)
		{
			var record = (EntityRecord)data.record;
			var createResult = (QueryResponse)data.result;
			var createdRecord = createResult.Object.Data[0];
			var task = new EntityRecord();
			var bug = new EntityRecord();
			if (createdRecord["task_id"] != null)
			{
				var filterObj = EntityQuery.QueryEQ("id", (Guid)createdRecord["task_id"]);
				var query = new EntityQuery("wv_task", "id,number,subject,project_id,$$user_n_n_task_watchers.id,$$user_n_n_task_watchers.email,$$project_1_n_task.code", filterObj, null, null, null);
				var result = recMan.Find(query);
				if (result.Success)
				{
					task = result.Object.Data[0];
					Utils.CreateActivity(recMan, "commented", "created a <i class='fa fa-fw fa-comment-o go-blue'></i> comment for task #" + task["number"] + " <a href='/#/areas/projects/wv_task/view-general/sb/general/" + task["id"] + "'>" + System.Net.WebUtility.HtmlEncode((string)task["subject"]) + "</a>", null, (Guid)task["project_id"], (Guid)task["id"], null);
				}
				else
				{
					throw new Exception(result.Message);
				}
			}
			else if (createdRecord["bug_id"] != null)
			{
				var filterObj = EntityQuery.QueryEQ("id", (Guid)createdRecord["bug_id"]);
				var query = new EntityQuery("wv_bug", "id,number,subject,project_id,$$user_n_n_bug_watchers.id,$$user_n_n_bug_watchers.email,$$project_1_n_bug.code", filterObj, null, null, null);
				var result = recMan.Find(query);
				if (result.Success)
				{
					bug = result.Object.Data[0];
					Utils.CreateActivity(recMan, "commented", "created a <i class='fa fa-fw fa-comment-o go-blue'></i> comment for bug #" + bug["number"] + " <a href='/#/areas/projects/wv_bug/view-general/sb/general/" + bug["id"] + "'>" + System.Net.WebUtility.HtmlEncode((string)bug["subject"]) + "</a>", null, (Guid)bug["project_id"], null, (Guid)bug["id"]);
				}
				else
				{
					throw new Exception(result.Message);
				}
			}

			var recepients = new List<string>();
			#region << Add the comment creator to the watch list if he is not there, Generate recipients list >>
			{
				if (createdRecord["task_id"] != null)
				{
					var isCommentatorInWatchList = false;
					#region << Check if is in watch list already >>
					foreach (var watcher in (List<EntityRecord>)task["$user_n_n_task_watchers"])
					{
						if ((Guid)watcher["id"] == (Guid)createdRecord["created_by"])
						{
							isCommentatorInWatchList = true;
						}
						else
						{
							recepients.Add((string)watcher["email"]);
						}
					}
					#endregion
					if (!isCommentatorInWatchList)
					{
						var targetRelation = relMan.Read("user_n_n_task_watchers").Object;
						var createRelationNtoNResponse = recMan.CreateRelationManyToManyRecord(targetRelation.Id, (Guid)record["created_by"], (Guid)record["id"]);
						if (!createRelationNtoNResponse.Success)
						{
							throw new Exception("Could not create watch relation" + createRelationNtoNResponse.Message);
						}
					}
				}
				else if (createdRecord["bug_id"] != null)
				{
					var isCommentatorInWatchList = false;
					#region << Check if is in watch list already >>
					foreach (var watcher in (List<EntityRecord>)bug["$user_n_n_bug_watchers"])
					{
						if ((Guid)watcher["id"] == (Guid)createdRecord["created_by"])
						{
							isCommentatorInWatchList = true;
						}
						else
						{
							recepients.Add((string)watcher["email"]);
						}
					}
					#endregion
					if (!isCommentatorInWatchList)
					{
						var targetRelation = relMan.Read("user_n_n_bug_watchers").Object;
						var createRelationNtoNResponse = recMan.CreateRelationManyToManyRecord(targetRelation.Id, (Guid)record["created_by"], (Guid)record["id"]);
						if (!createRelationNtoNResponse.Success)
						{
							throw new Exception("Could not create watch relation" + createRelationNtoNResponse.Message);
						}
					}
				}
			}
			#endregion

			#region << Generate notifications to watch list>>
			var emailService = new EmailService();
			if (recepients.Count > 0)
			{
				try
				{
					if (createdRecord["task_id"] != null)
					{
						var subject = "[Task] [New comment] (" + ((List<EntityRecord>)task["$project_1_n_task"])[0]["code"] + "-" + task["number"] + ") " + task["subject"];
						var content = "<div>Comment contents:</div><div>" + createdRecord["content"] + "</div>";
						emailService.SendEmail(recepients.ToArray(), subject, content);
					}
					else if (createdRecord["bug_id"] != null)
					{
						var subject = "[Bug] [New comment] (" + ((List<EntityRecord>)task["$project_1_n_bug"])[0]["code"] + "-" + task["number"] + ") " + task["subject"];
						var content = "<div>Comment contents:</div><div>" + createdRecord["content"] + "</div>";
						emailService.SendEmail(recepients.ToArray(), "New comment on bug", content);
					}
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}

			#endregion

		}

		#endregion
	}

}
