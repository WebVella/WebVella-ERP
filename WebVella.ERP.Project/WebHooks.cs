using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
			using (SecurityContext.OpenSystemScope())
			{
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
			}
			return data;
		}

		[WebHook("create_record", "wv_task")]
		public void TaskCreateRecordAction(dynamic data)
		{
			var record = (EntityRecord)data.record;
			var createResult = (QueryResponse)data.result;
			var controller = (Controller)data.controller;
			var createdRecord = createResult.Object.Data[0];
			var patchObject = new EntityRecord();
			using (SecurityContext.OpenSystemScope())
			{
				Utils.CreateActivity(recMan, "created", "created a <i class='fa fa-fw fa-tasks go-purple'></i> task #" + createdRecord["number"] + " <a href='/#/areas/projects/wv_task/view-general/sb/general/" + createdRecord["id"] + "'>" + System.Net.WebUtility.HtmlEncode((string)createdRecord["subject"]) + "</a>", null, (Guid)createdRecord["project_id"], (Guid)createdRecord["id"], null);

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

				#region << Add owner in watch list >>
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

				#region << Generate the code field value >>
				{
					var filterObj = EntityQuery.QueryEQ("id", (Guid)createdRecord["project_id"]);
					var query = new EntityQuery("wv_project", "*", filterObj, null, null, null);
					var result = recMan.Find(query);
					if (result.Success && result.Object.Data.Any())
					{
						patchObject = new EntityRecord();
						patchObject["id"] = (Guid)createdRecord["id"];
						patchObject["code"] = result.Object.Data[0]["code"] + "-T" + createdRecord["number"];
						var patchResult = recMan.UpdateRecord("wv_task", patchObject);
						if (!patchResult.Success)
						{
							throw new Exception(patchResult.Message);
						}
					}
				}
				#endregion
			}
			var creatorUsername = "";
			#region << Get username of the creator>>
			{
				EntityQuery query = new EntityQuery("user", "username", EntityQuery.QueryEQ("id", (Guid)createdRecord["created_by"]), null, null, null);
				QueryResponse result = recMan.Find(query);
				if (!result.Success)
				{
					throw new Exception("Cannot get the username of the commentator");
				}
				creatorUsername = (string)result.Object.Data[0]["username"];
			}
			#endregion

			#region << Sent email notification>>
			{
				//At this moment only if the project manager is different than the item creator should get an email
				if ((Guid)createdRecord["created_by"] != (Guid)createdRecord["owner_id"])
				{
					var emailService = new EmailService();
					var emailSubjectParameters = new Dictionary<string, string>();
					emailSubjectParameters["code"] = (string)patchObject["code"];
					emailSubjectParameters["subject"] = (string)createdRecord["subject"];

					var subject = Regex.Replace(EmailTemplates.NewBugOrTaskNotificationSubject, @"\{(.+?)\}", m => emailSubjectParameters[m.Groups[1].Value]);

					var emailContentParameters = new Dictionary<string, string>();
					emailContentParameters["baseUrl"] = controller.HttpContext.Request.Scheme + "://" + controller.HttpContext.Request.Host.Value;
					emailContentParameters["subject"] = subject;
					emailContentParameters["type"] = "task";
					emailContentParameters["creator"] = creatorUsername;
					emailContentParameters["taskOrBugUrl"] = emailContentParameters["baseUrl"] + "/#/areas/projects/wv_task/view-general/sb/general/" + createdRecord["id"];
					emailContentParameters["taskOrBugDescription"] = (string)createdRecord["description"];

					var content = Regex.Replace(EmailTemplates.NewBugOrTaskNotificationContent, @"\{(.+?)\}", m => emailContentParameters[m.Groups[1].Value]);

					var resepients = new List<string>();

					#region << Get pm email>>
					{
						EntityQuery query = new EntityQuery("user", "email", EntityQuery.QueryEQ("id", (Guid)createdRecord["owner_id"]), null, null, null);
						QueryResponse result = recMan.Find(query);
						if (!result.Success)
						{
							throw new Exception("Cannot get the username of the commentator");
						}
						emailService.SendEmail((string)result.Object.Data[0]["email"], subject, content);
					}
					#endregion
				}
			}
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
			using (SecurityContext.OpenSystemScope())
			{
				var updateResponse = recMan.UpdateRecord("wv_project", patchObject);
				if (!updateResponse.Success)
				{
					throw new Exception(updateResponse.Message);
				}
			}
			#endregion

			return data;
		}

		[WebHook("create_record", "wv_bug")]
		public void BugCreateRecordAction(dynamic data)
		{
			var record = (EntityRecord)data.record;
			var createResult = (QueryResponse)data.result;
			var controller = (Controller)data.controller;
			var createdRecord = createResult.Object.Data[0];
			var patchObject = new EntityRecord();

			using (SecurityContext.OpenSystemScope())
			{
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

				#region << Generate the code field value >>
				{
					var filterObj = EntityQuery.QueryEQ("id", (Guid)createdRecord["project_id"]);
					var query = new EntityQuery("wv_project", "*", filterObj, null, null, null);
					var result = recMan.Find(query);
					if (result.Success && result.Object.Data.Any())
					{
						patchObject = new EntityRecord();
						patchObject["id"] = (Guid)createdRecord["id"];
						patchObject["code"] = result.Object.Data[0]["code"] + "-B" + createdRecord["number"];
						var patchResult = recMan.UpdateRecord("wv_bug", patchObject);
						if (!patchResult.Success)
						{
							throw new Exception(patchResult.Message);
						}
					}
				}
				#endregion
			}
			var creatorUsername = "";
			#region << Get username of the creator>>
			{
				EntityQuery query = new EntityQuery("user", "username", EntityQuery.QueryEQ("id", (Guid)createdRecord["created_by"]), null, null, null);
				QueryResponse result = recMan.Find(query);
				if (!result.Success)
				{
					throw new Exception("Cannot get the username of the commentator");
				}
				creatorUsername = (string)result.Object.Data[0]["username"];
			}
			#endregion

			#region << Sent email notification>>
			{
				if ((Guid)createdRecord["created_by"] != (Guid)createdRecord["owner_id"])
				{
					//At this moment only if the project manager is different than the item creator should get an email
					var emailService = new EmailService();
					var emailSubjectParameters = new Dictionary<string, string>();
					emailSubjectParameters["code"] = (string)patchObject["code"];
					emailSubjectParameters["subject"] = (string)createdRecord["subject"];

					var subject = Regex.Replace(EmailTemplates.NewCommentNotificationSubject, @"\{(.+?)\}", m => emailSubjectParameters[m.Groups[1].Value]);

					var emailContentParameters = new Dictionary<string, string>();
					emailContentParameters["baseUrl"] = controller.HttpContext.Request.Scheme + "://" + controller.HttpContext.Request.Host.Value;
					emailContentParameters["subject"] = subject;
					emailContentParameters["type"] = "bug";
					emailContentParameters["creator"] = creatorUsername;
					emailContentParameters["taskOrBugUrl"] = emailContentParameters["baseUrl"] + "/#/areas/projects/wv_bug/view-general/sb/general/" + createdRecord["id"];
					emailContentParameters["taskOrBugDescription"] = (string)createdRecord["description"];

					var content = Regex.Replace(EmailTemplates.NewCommentNotificationContent, @"\{(.+?)\}", m => emailContentParameters[m.Groups[1].Value]);

					var resepients = new List<string>();

					#region << Get pm email>>
					{
						EntityQuery query = new EntityQuery("user", "email", EntityQuery.QueryEQ("id", (Guid)createdRecord["owner_id"]), null, null, null);
						QueryResponse result = recMan.Find(query);
						if (!result.Success)
						{
							throw new Exception("Cannot get the username of the commentator");
						}
						emailService.SendEmail((string)result.Object.Data[0]["email"], subject, content);
					}
					#endregion
				}
			}
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
			using (SecurityContext.OpenSystemScope())
			{
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
		}

		#endregion

		#region << Comment >>

		[WebHook("create_record", "wv_project_comment")]
		public void CommentCreateRecordAction(dynamic data)
		{
			var record = (EntityRecord)data.record;
			var createResult = (QueryResponse)data.result;
			var controller = (Controller)data.controller;
			var createdRecord = createResult.Object.Data[0];
			var task = new EntityRecord();
			var bug = new EntityRecord();
			var recepients = new List<string>();
			using (SecurityContext.OpenSystemScope())
			{
				#region << Init >>
				if (createdRecord["task_id"] != null)
				{
					var filterObj = EntityQuery.QueryEQ("id", (Guid)createdRecord["task_id"]);
					var query = new EntityQuery("wv_task", "id,code,subject,description,project_id,$$user_n_n_task_watchers.id,$$user_n_n_task_watchers.email,$$project_1_n_task.code", filterObj, null, null, null);
					var result = recMan.Find(query);
					if (result.Success)
					{
						task = result.Object.Data[0];
						Utils.CreateActivity(recMan, "commented", "created a <i class='fa fa-fw fa-comment-o go-blue'></i> comment for task [" + task["code"] + "] <a href='/#/areas/projects/wv_task/view-general/sb/general/" + task["id"] + "'>" + System.Net.WebUtility.HtmlEncode((string)task["subject"]) + "</a>", null, (Guid)task["project_id"], (Guid)task["id"], null);
					}
					else
					{
						throw new Exception(result.Message);
					}
				}
				else if (createdRecord["bug_id"] != null)
				{
					var filterObj = EntityQuery.QueryEQ("id", (Guid)createdRecord["bug_id"]);
					var query = new EntityQuery("wv_bug", "id,code,subject,description,project_id,$$user_n_n_bug_watchers.id,$$user_n_n_bug_watchers.email,$$project_1_n_bug.code", filterObj, null, null, null);
					var result = recMan.Find(query);
					if (result.Success)
					{
						bug = result.Object.Data[0];
						Utils.CreateActivity(recMan, "commented", "created a <i class='fa fa-fw fa-comment-o go-blue'></i> comment for bug [" + bug["code"] + "] <a href='/#/areas/projects/wv_bug/view-general/sb/general/" + bug["id"] + "'>" + System.Net.WebUtility.HtmlEncode((string)bug["subject"]) + "</a>", null, (Guid)bug["project_id"], null, (Guid)bug["id"]);
					}
					else
					{
						throw new Exception(result.Message);
					}
				}
				#endregion
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
			}
			var commentatorUsername = "";
			#region << Get username of the commentator>>
			{
				EntityQuery query = new EntityQuery("user", "username", EntityQuery.QueryEQ("id", (Guid)createdRecord["created_by"]), null, null, null);
				QueryResponse result = recMan.Find(query);
				if (!result.Success)
				{
					throw new Exception("Cannot get the username of the commentator");
				}
				commentatorUsername = (string)result.Object.Data[0]["username"];
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
						var emailSubjectParameters = new Dictionary<string, string>();
						emailSubjectParameters["code"] = (string)task["code"];
						emailSubjectParameters["subject"] = (string)task["subject"];

						var subject = Regex.Replace(EmailTemplates.NewCommentNotificationSubject, @"\{(.+?)\}", m => emailSubjectParameters[m.Groups[1].Value]);

						var emailContentParameters = new Dictionary<string, string>();
						emailContentParameters["baseUrl"] = controller.HttpContext.Request.Scheme + "://" + controller.HttpContext.Request.Host.Value;
						emailContentParameters["subject"] = subject;
						emailContentParameters["type"] = "task";
						emailContentParameters["creator"] = commentatorUsername;
						emailContentParameters["taskOrBugUrl"] = emailContentParameters["baseUrl"] + "/#/areas/projects/wv_task/view-general/sb/general/" + createdRecord["task_id"];
						emailContentParameters["commentContent"] = (string)createdRecord["content"];
						emailContentParameters["taskOrBugDescription"] = (string)task["description"];

						var content = Regex.Replace(EmailTemplates.NewCommentNotificationContent, @"\{(.+?)\}", m => emailContentParameters[m.Groups[1].Value]);

						emailService.SendEmail(recepients.ToArray(), subject, content);
					}
					else if (createdRecord["bug_id"] != null)
					{
						var emailSubjectParameters = new Dictionary<string, string>();
						emailSubjectParameters["code"] = (string)bug["code"];
						emailSubjectParameters["subject"] = (string)bug["subject"];

						var subject = Regex.Replace(EmailTemplates.NewCommentNotificationSubject, @"\{(.+?)\}", m => emailSubjectParameters[m.Groups[1].Value]);

						var emailContentParameters = new Dictionary<string, string>();
						emailContentParameters["baseUrl"] = controller.HttpContext.Request.Scheme + "://" + controller.HttpContext.Request.Host.Value;
						emailContentParameters["subject"] = subject;
						emailContentParameters["type"] = "bug";
						emailContentParameters["creator"] = commentatorUsername;
						emailContentParameters["taskOrBugUrl"] = emailContentParameters["baseUrl"] + "/#/areas/projects/wv_bug/view-general/sb/general/" + createdRecord["bug_id"];
						emailContentParameters["commentContent"] = (string)createdRecord["content"];
						emailContentParameters["taskOrBugDescription"] = (string)bug["description"];

						var content = Regex.Replace(EmailTemplates.NewCommentNotificationContent, @"\{(.+?)\}", m => emailContentParameters[m.Groups[1].Value]);

						emailService.SendEmail(recepients.ToArray(), subject, content);
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

		#region << Relation >>
		[WebHook("manage_relation_pre_save", "wv_task")]
		public dynamic TaskManageRelationPreSave(dynamic data)
		{
			return Utils.ManageRelationWithProject(data, recMan, "bug");
		}

		[WebHook("manage_relation_pre_save", "wv_bug")]
		public dynamic BugManageRelationPreSave(dynamic data)
		{
			return Utils.ManageRelationWithProject(data, recMan, "bug");
		}

		#endregion

	}

}
