using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Project
{
	public class Utils
	{


		public static dynamic UpdateTask(dynamic data, RecordManager recMan)
		{
			#region << Task activities to be logged >>
			//status,milestone,project,priority,owner,start_date,end_date,attachment_add,attachment_delete,comment_add,comment_delete,timelog_add,timelog_update,task_created_task_deleted
			#endregion

			try
			{
				EntityRecord newTaskObject = null;
				#region << Init checks>>
				newTaskObject = (EntityRecord)data.record;
				var recordId = (Guid)data.recordId;

				if (!newTaskObject.Properties.ContainsKey("status") && !newTaskObject.Properties.ContainsKey("project_id") && !newTaskObject.Properties.ContainsKey("milestone_id"))
				{
					return data;
				}
				//Convert ids to guid
				if (newTaskObject.Properties.ContainsKey("project_id") && newTaskObject["project_id"] != null)
				{
					newTaskObject["project_id"] = new Guid(newTaskObject["project_id"].ToString());
				}
				else if (newTaskObject.Properties.ContainsKey("project_id"))
				{
					newTaskObject["project_id"] = null;
				}

				if (newTaskObject.Properties.ContainsKey("milestone_id") && newTaskObject["milestone_id"] != null)
				{
					newTaskObject["milestone_id"] = new Guid(newTaskObject["milestone_id"].ToString());
				}
				else if (newTaskObject.Properties.ContainsKey("milestone_id"))
				{
					newTaskObject["milestone_id"] = null;
				}
				#endregion

				EntityRecord oldTaskObject = null;
				#region << Get the old task object >>
				{
					var filterObj = EntityQuery.QueryEQ("id", recordId);
					var query = new EntityQuery("wv_task", "*", filterObj, null, null, null);
					var result = recMan.Find(query);
					if (!result.Success)
					{

						throw new Exception("Error getting the updated task: " + result.Message);
					}
					else if (result.Object == null || result.Object.Data == null || !result.Object.Data.Any())
					{
						throw new Exception("Task not found");
					}
					oldTaskObject = result.Object.Data[0];
					//As we will use status checks below a lot, we need to be sure the newTaskObject object (posted by the user) has it also
					if (!newTaskObject.Properties.ContainsKey("status"))
					{
						newTaskObject["status"] = (string)oldTaskObject["status"];
					}

				}
				#endregion

				var isProjectChanged = false;
				var isMilestoneChanged = false;
				var isStatusChanged = false;
				#region << Check what is changed >>
				var oldStatus = (string)oldTaskObject["status"];
				if (newTaskObject.Properties.ContainsKey("status") && oldStatus != (string)newTaskObject["status"])
				{
					isStatusChanged = true;
				}

				var oldProjectId = (Guid?)oldTaskObject["project_id"];
				Guid? newProjectId = null;
				if (newTaskObject.Properties.ContainsKey("project_id") && oldProjectId != (Guid?)newTaskObject["project_id"])
				{
					isProjectChanged = true;
					newProjectId = (Guid?)newTaskObject["project_id"];
				}

				var oldMilestoneId = (Guid?)oldTaskObject["milestone_id"];
				Guid? newMilestoneId = null;
				if (newTaskObject.Properties.ContainsKey("milestone_id") && oldMilestoneId != (Guid?)newTaskObject["milestone_id"])
				{
					isMilestoneChanged = true;
					newMilestoneId = (Guid?)newTaskObject["milestone_id"];
				}

				//If none of the properties of interest is changed, just return
				if (!isStatusChanged && !isProjectChanged && !isMilestoneChanged)
				{
					return data;
				}

				#endregion

				EntityRecord oldTaskProject = new EntityRecord();
				EntityRecord newTaskProject = new EntityRecord();
				#region << Get the task projects new and old >>
				{
					if (oldTaskObject["project_id"] != null || isProjectChanged)
					{
						var projectFiltersList = new List<QueryObject>();
						if (oldTaskObject["project_id"] != null)
						{
							var oldProjectFilterObj = EntityQuery.QueryEQ("id", (Guid)oldTaskObject["project_id"]);
							projectFiltersList.Add(oldProjectFilterObj);
						}
						if (isProjectChanged && newTaskObject["project_id"] != null)
						{
							var newProjectFilterObj = EntityQuery.QueryEQ("id", (Guid)newTaskObject["project_id"]);
							projectFiltersList.Add(newProjectFilterObj);
						}
						var sectionFilter = EntityQuery.QueryOR(projectFiltersList.ToArray());
						var query = new EntityQuery("wv_project", "*", sectionFilter, null, null, null);
						var result = recMan.Find(query);
						if (!result.Success)
						{

							throw new Exception("Error getting the updated project: " + result.Message);
						}
						else if (result.Object == null || result.Object.Data == null || !result.Object.Data.Any())
						{
							throw new Exception("Project not found");
						}
						else
						{
							foreach (var resultObject in result.Object.Data)
							{
								if (oldTaskObject["project_id"] != null && (Guid)resultObject["id"] == (Guid)oldTaskObject["project_id"])
								{
									oldTaskProject = resultObject;
								}
								if (isProjectChanged && newTaskObject["project_id"] != null && (Guid)resultObject["id"] == (Guid)newTaskObject["project_id"])
								{
									newTaskProject = resultObject;
								}
							}

						}
					}
				}
				#endregion


				EntityRecord oldTaskMilestone = new EntityRecord();
				EntityRecord newTaskMilestone = new EntityRecord();
				#region << Get the task milestones old and new >>
				{
					if (oldTaskObject["milestone_id"] != null || isMilestoneChanged)
					{
						var milestoneFiltersList = new List<QueryObject>();
						if (oldTaskObject["milestone_id"] != null)
						{
							var oldMilestoneFilter = EntityQuery.QueryEQ("id", (Guid)oldTaskObject["milestone_id"]);
							milestoneFiltersList.Add(oldMilestoneFilter);
						}
						if (isMilestoneChanged && newTaskObject["milestone_id"] != null)
						{
							var newMilestoneFilter = EntityQuery.QueryEQ("id", (Guid)newTaskObject["milestone_id"]);
							milestoneFiltersList.Add(newMilestoneFilter);
						}
						var sectionFilter = EntityQuery.QueryOR(milestoneFiltersList.ToArray());
						var query = new EntityQuery("wv_milestone", "*", sectionFilter, null, null, null);
						var result = recMan.Find(query);
						if (!result.Success)
						{

							throw new Exception("Error getting the updated milestone: " + result.Message);
						}
						else if (result.Object == null || result.Object.Data == null || !result.Object.Data.Any())
						{
							throw new Exception("Project not found");
						}
						else
						{
							foreach (var resultObject in result.Object.Data)
							{
								if (oldTaskObject["milestone_id"] != null && (Guid)resultObject["id"] == (Guid)oldTaskObject["milestone_id"])
								{
									oldTaskMilestone = resultObject;
								}
								if (isMilestoneChanged && newTaskObject["milestone_id"] != null && (Guid)resultObject["id"] == (Guid)newTaskObject["milestone_id"])
								{
									newTaskMilestone = resultObject;
								}
							}
						}
					}
				}
				#endregion


				EntityRecord oldProjectPatchObject = new EntityRecord();
				EntityRecord newProjectPatchObject = new EntityRecord();
				EntityRecord oldMilestonePatchObject = new EntityRecord();
				EntityRecord newMilestonePatchObject = new EntityRecord();
				var isOldProjectPatched = false;
				var isNewProjectPatched = false;
				var isOldMilestonePatched = false;
				var isNewMilestonePatched = false;
				//var isTaskPatched = false;
				#region << init patch objects >>
				if (oldTaskProject.Properties.Count > 0)
				{
					oldProjectPatchObject["id"] = (Guid)oldTaskProject["id"];
					oldProjectPatchObject["x_tasks_not_started"] = (decimal)oldTaskProject["x_tasks_not_started"];
					oldProjectPatchObject["x_tasks_in_progress"] = (decimal)oldTaskProject["x_tasks_in_progress"];
					oldProjectPatchObject["x_tasks_completed"] = (decimal)oldTaskProject["x_tasks_completed"];
				}
				if (newTaskProject.Properties.Count > 0)
				{
					newProjectPatchObject["id"] = (Guid)newTaskProject["id"];
					newProjectPatchObject["x_tasks_not_started"] = (decimal)newTaskProject["x_tasks_not_started"];
					newProjectPatchObject["x_tasks_in_progress"] = (decimal)newTaskProject["x_tasks_in_progress"];
					newProjectPatchObject["x_tasks_completed"] = (decimal)newTaskProject["x_tasks_completed"];
				}
				if (oldTaskMilestone.Properties.Count > 0)
				{
					oldMilestonePatchObject["id"] = (Guid)oldTaskMilestone["id"];
					oldMilestonePatchObject["x_tasks_not_started"] = (decimal)oldTaskMilestone["x_tasks_not_started"];
					oldMilestonePatchObject["x_tasks_in_progress"] = (decimal)oldTaskMilestone["x_tasks_in_progress"];
					oldMilestonePatchObject["x_tasks_completed"] = (decimal)oldTaskMilestone["x_tasks_completed"];
				}
				if (newTaskMilestone.Properties.Count > 0)
				{
					newMilestonePatchObject["id"] = (Guid)newTaskMilestone["id"];
					newMilestonePatchObject["x_tasks_not_started"] = (decimal)newTaskMilestone["x_tasks_not_started"];
					newMilestonePatchObject["x_tasks_in_progress"] = (decimal)newTaskMilestone["x_tasks_in_progress"];
					newMilestonePatchObject["x_tasks_completed"] = (decimal)newTaskMilestone["x_tasks_completed"];
				}
				#endregion

				#region << Case 1 - changes based on Project >>
				if (isProjectChanged)
				{
					//Remove one from the old project <> old status if the target is not null
					if (oldProjectPatchObject.Properties.Count > 0)
					{
						oldProjectPatchObject = UpdateProjectOrMilestoneCounter(oldProjectPatchObject, (string)oldTaskObject["status"], -1);
						isOldProjectPatched = true;
					}
					//Add one to the new project <> new status if the target is not null
					if (newProjectPatchObject.Properties.Count > 0)
					{
						newProjectPatchObject = UpdateProjectOrMilestoneCounter(newProjectPatchObject, (string)newTaskObject["status"], 1);
						isNewProjectPatched = true;
					}

					//Regenerate the task Code
					data.record["code"] = newTaskProject["code"] + "-T" + oldTaskObject["number"];

				}
				else
				{
					//No change is needed based on this case
				}
				#endregion

				#region << Case 2 - changes based on Milestone >>
				if (isMilestoneChanged)
				{
					//Remove one from the old milestone <> old status if the target is not null
					if (oldMilestonePatchObject.Properties.Count > 0)
					{
						oldMilestonePatchObject = UpdateProjectOrMilestoneCounter(oldMilestonePatchObject, (string)oldTaskObject["status"], -1);
						isOldMilestonePatched = true;
					}
					//Add one to the new milestone <> new status if the target is not null
					if (newMilestonePatchObject.Properties.Count > 0)
					{
						newMilestonePatchObject = UpdateProjectOrMilestoneCounter(newMilestonePatchObject, (string)newTaskObject["status"], 1);
						isNewMilestonePatched = true;
					}
				}
				else
				{
					//No change is needed based on this case
				}
				#endregion

				#region << Case 3 - changes based on Status >>
				{
					if (isStatusChanged)
					{
						if (isProjectChanged)
						{
							//the status change is already set in the new project object in case 1
						}
						else if (oldProjectPatchObject.Properties.Count > 0)
						{
							//Remove one from the old project old status
							oldProjectPatchObject = UpdateProjectOrMilestoneCounter(oldProjectPatchObject, (string)oldTaskObject["status"], -1);
							//Add one from the old project new status
							oldProjectPatchObject = UpdateProjectOrMilestoneCounter(oldProjectPatchObject, (string)newTaskObject["status"], 1);
							isOldProjectPatched = true;
						}
						if (isMilestoneChanged)
						{
							//the status change is already set in the new milestone object in case 2
						}
						else if (oldMilestonePatchObject.Properties.Count > 0)
						{
							//Remove one from the old milestone old status
							oldMilestonePatchObject = UpdateProjectOrMilestoneCounter(oldMilestonePatchObject, (string)oldTaskObject["status"], -1);
							//Add one from the old milestone new status	
							oldMilestonePatchObject = UpdateProjectOrMilestoneCounter(oldMilestonePatchObject, (string)newTaskObject["status"], 1);
							isOldMilestonePatched = true;
						}
					}
					else
					{
						//do nothing
					}
				}
				#endregion

				using (SecurityContext.OpenSystemScope())
				{
					#region << Update objects >>
					if (isOldProjectPatched)
					{
						var updateResponse = recMan.UpdateRecord("wv_project", oldProjectPatchObject);
						if (!updateResponse.Success)
						{
							throw new Exception("Old project update error: " + updateResponse.Message);
						}
					}

					if (isNewProjectPatched)
					{
						var updateResponse = recMan.UpdateRecord("wv_project", newProjectPatchObject);
						if (!updateResponse.Success)
						{
							throw new Exception("New project update error: " + updateResponse.Message);
						}
					}

					if (isOldMilestonePatched)
					{
						var updateResponse = recMan.UpdateRecord("wv_milestone", oldMilestonePatchObject);
						if (!updateResponse.Success)
						{
							throw new Exception("Old milestone update error: " + updateResponse.Message);
						}
					}

					if (isNewMilestonePatched)
					{
						var updateResponse = recMan.UpdateRecord("wv_milestone", newMilestonePatchObject);
						if (!updateResponse.Success)
						{
							throw new Exception("New milestone update error: " + updateResponse.Message);
						}
					}
					#endregion
				}
				#region << Create update activity >>
				var priorityString = "";
				if ((string)oldTaskObject["priority"] == "high")
				{
					priorityString = "<span class='go-red'> [high] </span>";
				}
				Utils.CreateActivity(recMan, "updated", "updated a <i class='fa fa-fw fa-tasks go-purple'></i> task [" + oldTaskObject["code"] + priorityString + "] <a href='/#/areas/projects/wv_task/view-general/sb/general/" + oldTaskObject["id"] + "'>" + System.Net.WebUtility.HtmlEncode((string)oldTaskObject["subject"]) + "</a>", null, (Guid)oldTaskObject["project_id"], (Guid)oldTaskObject["id"], null);
				#endregion
				return data;
			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}
		}

		public static void RegenAndUpdateTaskFts(EntityRecord record, RecordManager recMan)
		{
			try {
			EntityRecord oldTaskObject = null;
			if(record != null && record["id"] != null) {
				#region << Get the old task object >>
			{
				var filterObj = EntityQuery.QueryEQ("id", (Guid)record["id"]);
				var query = new EntityQuery("wv_task", "*,$task_1_n_comment.content,$$project_1_n_task.name,$$project_1_n_task.code,$$user_1_n_task_owner.username,$$user_wv_task_created_by.username", filterObj, null, null, null);
				var result = recMan.Find(query);
				if (!result.Success)
				{

					throw new Exception("Error getting the updated task: " + result.Message);
				}
				else if (result.Object == null || result.Object.Data == null || !result.Object.Data.Any())
				{
					throw new Exception("Task not found");
				}
				oldTaskObject = result.Object.Data[0];
			}
			#endregion
			}
			var ftsString = "";

			#region << Add task properties >>
			if (record.Properties.ContainsKey("subject")) {
				ftsString += " " + record["subject"];
			}
			else if(oldTaskObject != null){
				ftsString += " " + oldTaskObject["subject"];
			}
			if(record.Properties.ContainsKey("status")) {
				ftsString += " " + record["status"];
			}
			else if(oldTaskObject != null){
				ftsString += " " + oldTaskObject["status"];
			}
			if(record.Properties.ContainsKey("priority")) {
				ftsString += " " + record["priority"];
			}
			else if(oldTaskObject != null){
				ftsString += " " + oldTaskObject["priority"];
			}
			if(record.Properties.ContainsKey("code")) {
				ftsString += " " + record["code"];
			}
			else if(oldTaskObject != null){
				ftsString += " " + oldTaskObject["code"];
			}
			if(record.Properties.ContainsKey("number")) {
				ftsString += " " + record["number"];
			}
			else if(oldTaskObject != null){
				ftsString += " " + oldTaskObject["number"];
			}
			if(record.Properties.ContainsKey("description") && record["description"] != null) {
				var cleanString = Regex.Replace((string)record["description"], "<.*?>", string.Empty);
				cleanString = cleanString.Replace(System.Environment.NewLine, " ");
				cleanString = cleanString.Replace("\n", " ");
				ftsString += " " + cleanString;
			}
			else if(oldTaskObject != null && oldTaskObject["description"] != null){
				var cleanString = Regex.Replace((string)oldTaskObject["description"], "<.*?>", string.Empty);
				cleanString = cleanString.Replace(System.Environment.NewLine, " ");
				cleanString = cleanString.Replace("\n", " ");
				ftsString += " " + cleanString;
			}
			#endregion

			#region << Add comments content>>
			if(oldTaskObject != null) {
				var comments = (List<EntityRecord>)oldTaskObject["$task_1_n_comment"];
				foreach (var comment in comments)
				{
					var cleanString = Regex.Replace((string)comment["content"], "<.*?>", string.Empty);
					cleanString = cleanString.Replace(System.Environment.NewLine, " ");
					cleanString = cleanString.Replace("\n", " ");
					ftsString += " " + cleanString;					
				}
			}
			#endregion

			#region << Add project properties>>
			if(oldTaskObject != null) {
				var projects = (List<EntityRecord>)oldTaskObject["$project_1_n_task"];
				if(projects.Any()) {
					var project = projects[0];
					if (project.Properties.ContainsKey("name")) {
						ftsString += " " + project["name"];
					}			
					if (project.Properties.ContainsKey("code")) {
						ftsString += " " + project["code"];
					}					
				}	
			}
			#endregion

			#region << Add creator properties>>
			if(oldTaskObject != null) {
				var users = (List<EntityRecord>)oldTaskObject["$user_wv_task_created_by"];
				if(users.Any()) {
					var user = users[0];
					if (user.Properties.ContainsKey("username")) {
						ftsString += " " + user["username"];
					}			
				}
			}
			#endregion

			#region << Add owner properties>>
			if(oldTaskObject != null) {
				var users = (List<EntityRecord>)oldTaskObject["$user_1_n_task_owner"];
				if(users.Any()) {
					var user = users[0];
					if (user.Properties.ContainsKey("username")) {
						ftsString += " " + user["username"];
					}			
				}
				
			}
			#endregion

			var patchObject = new EntityRecord();
			patchObject["id"] = (Guid)record["id"];
			patchObject["fts"] = ftsString;

			var patchResult = recMan.UpdateRecord("wv_task", patchObject);
			if (!patchResult.Success)
			{
				throw new Exception(patchResult.Message);
			}
			}
			catch(Exception ex) {
				var boz = ex;
			}
		}

		public static dynamic UpdateBug(dynamic data, RecordManager recMan)
		{
			#region << Bug activities to be logged >>
			//status,project,priority,owner,start_date,end_date,attachment_add,attachment_delete,comment_add,comment_delete,timelog_add,timelog_update,task_created_task_deleted
			#endregion

			EntityRecord newBugObject = null;
			#region << Init checks>>
			newBugObject = (EntityRecord)data.record;
			var recordId = (Guid)data.recordId;

			if (!newBugObject.Properties.ContainsKey("status") && !newBugObject.Properties.ContainsKey("project_id"))
			{
				return data;
			}
			//Convert ids to guid
			if (newBugObject.Properties.ContainsKey("project_id") && newBugObject["project_id"] != null)
			{
				newBugObject["project_id"] = new Guid(newBugObject["project_id"].ToString());
			}
			else if (newBugObject.Properties.ContainsKey("project_id"))
			{
				newBugObject["project_id"] = null;
			}

			#endregion

			EntityRecord oldBugObject = null;
			#region << Get the old bug object >>
			{
				var filterObj = EntityQuery.QueryEQ("id", recordId);
				var query = new EntityQuery("wv_bug", "*", filterObj, null, null, null);
				var result = recMan.Find(query);
				if (!result.Success)
				{

					throw new Exception("Error getting the updated bug: " + result.Message);
				}
				else if (result.Object == null || result.Object.Data == null || !result.Object.Data.Any())
				{
					throw new Exception("Task not found");
				}
				oldBugObject = result.Object.Data[0];
				//As we will use status checks below a lot, we need to be sure the newTaskObject object (posted by the user) has it also
				if (!newBugObject.Properties.ContainsKey("status"))
				{
					newBugObject["status"] = (string)oldBugObject["status"];
				}

			}
			#endregion

			var isProjectChanged = false;
			var isStatusChanged = false;
			#region << Check what is changed >>
			var oldStatus = (string)oldBugObject["status"];
			if (newBugObject.Properties.ContainsKey("status") && oldStatus != (string)newBugObject["status"])
			{
				isStatusChanged = true;
			}

			var oldProjectId = (Guid?)oldBugObject["project_id"];
			Guid? newProjectId = null;
			if (newBugObject.Properties.ContainsKey("project_id") && oldProjectId != (Guid?)newBugObject["project_id"])
			{
				isProjectChanged = true;
				newProjectId = (Guid?)newBugObject["project_id"];
			}

			//If none of the properties of interest is changed, just return
			if (!isStatusChanged && !isProjectChanged)
			{
				return data;
			}

			#endregion

			EntityRecord oldBugProject = new EntityRecord();
			EntityRecord newBugProject = new EntityRecord();
			#region << Get the bug projects new and old >>
			{
				if (oldBugObject["project_id"] != null || isProjectChanged)
				{
					var projectFiltersList = new List<QueryObject>();
					if (oldBugObject["project_id"] != null)
					{
						var oldProjectFilterObj = EntityQuery.QueryEQ("id", (Guid)oldBugObject["project_id"]);
						projectFiltersList.Add(oldProjectFilterObj);
					}
					if (isProjectChanged && newBugObject["project_id"] != null)
					{
						var newProjectFilterObj = EntityQuery.QueryEQ("id", (Guid)newBugObject["project_id"]);
						projectFiltersList.Add(newProjectFilterObj);
					}
					var sectionFilter = EntityQuery.QueryOR(projectFiltersList.ToArray());
					var query = new EntityQuery("wv_project", "*", sectionFilter, null, null, null);
					var result = recMan.Find(query);
					if (!result.Success)
					{

						throw new Exception("Error getting the updated project: " + result.Message);
					}
					else if (result.Object == null || result.Object.Data == null || !result.Object.Data.Any())
					{
						throw new Exception("Project not found");
					}
					else
					{
						foreach (var resultObject in result.Object.Data)
						{
							if (oldBugObject["project_id"] != null && (Guid)resultObject["id"] == (Guid)oldBugObject["project_id"])
							{
								oldBugProject = resultObject;
							}
							if (isProjectChanged && newBugObject["project_id"] != null && (Guid)resultObject["id"] == (Guid)newBugObject["project_id"])
							{
								newBugProject = resultObject;
							}
						}

					}
				}
			}
			#endregion

			EntityRecord oldProjectPatchObject = new EntityRecord();
			EntityRecord newProjectPatchObject = new EntityRecord();
			var isOldProjectPatched = false;
			var isNewProjectPatched = false;

			#region << init patch objects >>
			if (oldBugProject.Properties.Count > 0)
			{
				oldProjectPatchObject["id"] = (Guid)oldBugProject["id"];
				oldProjectPatchObject["x_bugs_closed"] = (decimal)oldBugProject["x_bugs_closed"];
				oldProjectPatchObject["x_bugs_opened"] = (decimal)oldBugProject["x_bugs_opened"];
				oldProjectPatchObject["x_bugs_reopened"] = (decimal)oldBugProject["x_bugs_reopened"];
			}
			if (newBugProject.Properties.Count > 0)
			{
				newProjectPatchObject["id"] = (Guid)newBugProject["id"];
				newProjectPatchObject["x_bugs_closed"] = (decimal)newBugProject["x_bugs_closed"];
				newProjectPatchObject["x_bugs_opened"] = (decimal)newBugProject["x_bugs_opened"];
				newProjectPatchObject["x_bugs_reopened"] = (decimal)newBugProject["x_bugs_reopened"];
			}
			#endregion

			#region << Case 1 - changes based on Project >>
			if (isProjectChanged)
			{
				//Remove one from the old project <> old status if the target is not null
				if (oldProjectPatchObject.Properties.Count > 0)
				{
					oldProjectPatchObject = UpdateProjectOrMilestoneCounter(oldProjectPatchObject, (string)oldBugObject["status"], -1);
					isOldProjectPatched = true;
				}
				//Add one to the new project <> new status if the target is not null
				if (newProjectPatchObject.Properties.Count > 0)
				{
					newProjectPatchObject = UpdateProjectOrMilestoneCounter(newProjectPatchObject, (string)newBugObject["status"], 1);
					isNewProjectPatched = true;
				}

				//Regenerate the task Code
				data.record["code"] = newBugProject["code"] + "-T" + oldBugObject["number"];
			}
			else
			{
				//No change is needed based on this case
			}
			#endregion

			#region << Case 2 - changes based on Status >>
			{
				if (isStatusChanged)
				{
					if (isProjectChanged)
					{
						//the status change is already set in the new project object in case 1
					}
					else if (oldProjectPatchObject.Properties.Count > 0)
					{
						//Remove one from the old project old status
						oldProjectPatchObject = UpdateProjectOrMilestoneCounter(oldProjectPatchObject, (string)oldBugObject["status"], -1);
						//Add one from the old project new status
						oldProjectPatchObject = UpdateProjectOrMilestoneCounter(oldProjectPatchObject, (string)newBugObject["status"], 1);
						isOldProjectPatched = true;
					}
				}
				else
				{
					//do nothing
				}
			}
			#endregion

			using (SecurityContext.OpenSystemScope())
			{

				#region << Update objects >>
				if (isOldProjectPatched)
				{
					var updateResponse = recMan.UpdateRecord("wv_project", oldProjectPatchObject);
					if (!updateResponse.Success)
					{
						throw new Exception("Old project update error: " + updateResponse.Message);
					}
				}

				if (isNewProjectPatched)
				{
					var updateResponse = recMan.UpdateRecord("wv_project", newProjectPatchObject);
					if (!updateResponse.Success)
					{
						throw new Exception("New project update error: " + updateResponse.Message);
					}
				}

				#endregion
			}
			#region << Update activity >>
			var priorityString = "";
			if ((string)oldBugObject["priority"] == "high")
			{
				priorityString = "<span class='go-red'> [high] </span>";
			}
			Utils.CreateActivity(recMan, "updated", "updated a <i class='fa fa-fw fa-bug go-red'></i> bug [" + oldBugObject["code"] + priorityString + "] <a href='/#/areas/projects/wv_bug/view-general/sb/general/" + oldBugObject["id"] + "'>" + System.Net.WebUtility.HtmlEncode((string)oldBugObject["subject"]) + "</a>", null, (Guid)oldBugObject["project_id"], null, (Guid)oldBugObject["id"]);
			#endregion
			return data;
		}

		public static void RegenAndUpdateBugFts(EntityRecord record, RecordManager recMan)
		{
			
			EntityRecord oldBugObject = null;
			if(record != null && record["id"] != null) {
				#region << Get the old task object >>
			{
				var filterObj = EntityQuery.QueryEQ("id", (Guid)record["id"]);
				var query = new EntityQuery("wv_bug", "*,$bug_1_n_comment.content,$$project_1_n_bug.name,$$project_1_n_bug.code,$$user_1_n_bug_owner.username,$$user_wv_bug_created_by.username", filterObj, null, null, null);
				var result = recMan.Find(query);
				if (!result.Success)
				{

					throw new Exception("Error getting the updated bug: " + result.Message);
				}
				else if (result.Object == null || result.Object.Data == null || !result.Object.Data.Any())
				{
					throw new Exception("Bug not found");
				}
				oldBugObject = result.Object.Data[0];
			}
			#endregion
			}
			var ftsString = "";

			#region << Add bug properties >>
			if (record.Properties.ContainsKey("subject")) {
				ftsString += " " + record["subject"];
			}
			else if(oldBugObject != null){
				ftsString += " " + oldBugObject["subject"];
			}
			if(record.Properties.ContainsKey("status")) {
				ftsString += " " + record["status"];
			}
			else if(oldBugObject != null){
				ftsString += " " + oldBugObject["status"];
			}
			if(record.Properties.ContainsKey("priority")) {
				ftsString += " " + record["priority"];
			}
			else if(oldBugObject != null){
				ftsString += " " + oldBugObject["priority"];
			}
			if(record.Properties.ContainsKey("code")) {
				ftsString += " " + record["code"];
			}
			else if(oldBugObject != null){
				ftsString += " " + oldBugObject["code"];
			}
			if(record.Properties.ContainsKey("number")) {
				ftsString += " " + record["number"];
			}
			else if(oldBugObject != null){
				ftsString += " " + oldBugObject["number"];
			}
			if(record.Properties.ContainsKey("description") && record["description"] != null) {
				var cleanString = Regex.Replace((string)record["description"], "<.*?>", string.Empty);
				cleanString = cleanString.Replace(System.Environment.NewLine, " ");
				cleanString = cleanString.Replace("\n", " ");
				ftsString += " " + cleanString;
			}
			else if(oldBugObject != null && oldBugObject["description"] != null){
				var cleanString = Regex.Replace((string)oldBugObject["description"], "<.*?>", string.Empty);
				cleanString = cleanString.Replace(System.Environment.NewLine, " ");
				cleanString = cleanString.Replace("\n", " ");
				ftsString += " " + cleanString;
			}
			#endregion

			#region << Add comments content>>
			if(oldBugObject != null) {
				var comments = (List<EntityRecord>)oldBugObject["$bug_1_n_comment"];
				foreach (var comment in comments)
				{
					var cleanString = Regex.Replace((string)comment["content"], "<.*?>", string.Empty);
					cleanString = cleanString.Replace(System.Environment.NewLine, " ");
					cleanString = cleanString.Replace("\n", " ");
					ftsString += " " + cleanString;					
				}
			}
			#endregion

			#region << Add project properties>>
			if(oldBugObject != null) {
				var projects = (List<EntityRecord>)oldBugObject["$project_1_n_bug"];
				if(projects.Any()) {
					var project = projects[0];
					if (project.Properties.ContainsKey("name")) {
						ftsString += " " + project["name"];
					}			
					if (project.Properties.ContainsKey("code")) {
						ftsString += " " + project["code"];
					}					
				}	
			}
			#endregion

			#region << Add creator properties>>
			if(oldBugObject != null) {
				var users = (List<EntityRecord>)oldBugObject["$user_wv_bug_created_by"];
				if(users.Any()) {
					var user = users[0];
					if (user.Properties.ContainsKey("username")) {
						ftsString += " " + user["username"];
					}			
				}
			}
			#endregion

			#region << Add owner properties>>
			if(oldBugObject != null) {
				var users = (List<EntityRecord>)oldBugObject["$user_1_n_bug_owner"];
				if(users.Any()) {
					var user = users[0];
					if (user.Properties.ContainsKey("username")) {
						ftsString += " " + user["username"];
					}			
				}
				
			}
			#endregion

			var patchObject = new EntityRecord();
			patchObject["id"] = (Guid)record["id"];
			patchObject["fts"] = ftsString;

			var patchResult = recMan.UpdateRecord("wv_bug", patchObject);
			if (!patchResult.Success)
			{
				throw new Exception(patchResult.Message);
			}
		}


		public static dynamic ManageRelationWithProject(dynamic data, RecordManager recMan, string itemType)
		{
			var relation = (EntityRelation)data.relation;
			var attachTargetRecords = (List<EntityRecord>)data.attachTargetRecords;
			var detachTargetRecords = (List<EntityRecord>)data.detachTargetRecords;
			var originEntity = (Entity)data.originEntity;
			var targetEntity = (Entity)data.targetEntity;
			var newOriginRecord = (EntityRecord)data.originRecord;
			var newProjectRecord = new EntityRecord();
			var attachedRecordForeachObject = new EntityRecord();
			var hookedRelationName = "project_1_n_task";
			var hookedEntityName = "wv_task";
			if (itemType == "bug")
			{
				hookedRelationName = "project_1_n_bug";
				hookedEntityName = "wv_bug";
			}

			if (relation.Name == hookedRelationName)
			{

				#region << Select the project >>
				{
					newProjectRecord = new EntityRecord();
					EntityQuery query = new EntityQuery("wv_project", "code", EntityQuery.QueryEQ("id", (Guid)newOriginRecord["id"]), null, null, null);
					QueryResponse result = recMan.Find(query);
					if (!result.Success || result.Object.Data.Count == 0)
					{
						throw new Exception("could not select the new project record");
					}
					newProjectRecord = result.Object.Data.First();
				}
				#endregion


				foreach (var attachedRecord in attachTargetRecords)
				{
					#region << Select the attached >>
					{
						attachedRecordForeachObject = new EntityRecord();
						EntityQuery query = new EntityQuery(hookedEntityName, "id,number", EntityQuery.QueryEQ("id", (Guid)attachedRecord["id"]), null, null, null);
						QueryResponse result = recMan.Find(query);
						if (!result.Success || result.Object.Data.Count == 0)
						{
							throw new Exception("could not select the bug record");
						}
						attachedRecordForeachObject = result.Object.Data.First();
					}
					#endregion

					var patchObject = new EntityRecord();
					patchObject["id"] = attachedRecordForeachObject["id"];
					patchObject["code"] = newProjectRecord["code"] + "-T" + attachedRecordForeachObject["number"];
					if (itemType == "bug")
					{
						patchObject["code"] = newProjectRecord["code"] + "-B" + attachedRecordForeachObject["number"];
					}
					using (SecurityContext.OpenSystemScope())
					{
						var patchResult = recMan.UpdateRecord(hookedEntityName, patchObject);
						if (!patchResult.Success)
						{
							//nothing for now
						}
					}
				}
			}


			return data;

		}

		public static EntityRecord UpdateProjectOrMilestoneCounter(EntityRecord targetObject, string status, decimal count)
		{
			switch (status)
			{
				case "not started":
					targetObject["x_tasks_not_started"] = (decimal)targetObject["x_tasks_not_started"] + count;
					break;
				case "in progress":
					targetObject["x_tasks_in_progress"] = (decimal)targetObject["x_tasks_in_progress"] + count;
					break;
				case "completed":
					targetObject["x_tasks_completed"] = (decimal)targetObject["x_tasks_completed"] + count;
					break;
				case "opened":
					targetObject["x_bugs_opened"] = (decimal)targetObject["x_bugs_opened"] + count;
					break;
				case "closed":
					targetObject["x_bugs_closed"] = (decimal)targetObject["x_bugs_closed"] + count;
					break;
				case "reopened":
					targetObject["x_bugs_reopened"] = (decimal)targetObject["x_bugs_reopened"] + count;
					break;
			}

			return targetObject;
		}

		public static void CreateActivity(RecordManager recMan, string label, string subject, string description, Guid projectId, Guid? taskId, Guid? bugId)
		{
			var activityObj = new EntityRecord();
			activityObj["id"] = Guid.NewGuid();
			activityObj["project_id"] = projectId;
			activityObj["task_id"] = taskId;
			activityObj["bug_id"] = bugId;
			activityObj["label"] = label;
			activityObj["subject"] = subject;
			activityObj["description"] = description;

			var createResponse = recMan.CreateRecord("wv_project_activity", activityObj);
			if (!createResponse.Success)
			{
				throw new Exception(createResponse.Message);
			}
		}


		public static dynamic UpdateTimelog(dynamic data, RecordManager recMan)
		{
			var newObject = (EntityRecord)data.record;
			var recordId = (Guid)data.recordId;
			EntityRecord oldObject = null;
			#region << Get the old bug object >>
			{
				var filterObj = EntityQuery.QueryEQ("id", recordId);
				var query = new EntityQuery("wv_timelog", "*", filterObj, null, null, null);
				var result = recMan.Find(query);
				if (!result.Success)
				{

					throw new Exception("Error getting the old wv_timelog: " + result.Message);
				}
				else if (result.Object == null || result.Object.Data == null || !result.Object.Data.Any())
				{
					throw new Exception("Task not found");
				}
				oldObject = result.Object.Data[0];
			}
			#endregion

			var oldBillableString = "not billable";
			if ((bool)oldObject["billable"])
			{
				oldBillableString = "billable";
			}

			var newBillableString = "not billable";
			if ((bool)newObject["billable"])
			{
				newBillableString = "billable";
			}

			if (newObject["task_id"] != null)
			{
				var filterObj = EntityQuery.QueryEQ("id", (Guid)newObject["task_id"]);
				var query = new EntityQuery("wv_task", "*", filterObj, null, null, null);
				var result = recMan.Find(query);
				if (result.Success)
				{
					var task = result.Object.Data[0];
					//Update the x_billable_hours and x_nonbillable_hours fields
					var updatedRecord = new EntityRecord();
					updatedRecord["id"] = (Guid)task["id"];

					if(oldBillableString == "billable") {
						updatedRecord["x_billable_hours"] = (decimal)task["x_billable_hours"] - (decimal)oldObject["hours"];
					}
					else {
						updatedRecord["x_nonbillable_hours"] = (decimal)task["x_nonbillable_hours"] - (decimal)oldObject["hours"];
					}

					if(newBillableString == "billable") {
						updatedRecord["x_billable_hours"] = (decimal)task["x_billable_hours"] + (decimal)newObject["hours"];
					}
					else {
						updatedRecord["x_nonbillable_hours"] = (decimal)task["x_nonbillable_hours"] + (decimal)newObject["hours"];
					}					

					var updateRecordResult = recMan.UpdateRecord("wv_task",updatedRecord);
					if(!updateRecordResult.Success) {
						throw new Exception("Cannot update the x_billable_hours or x_nonbillable_hours fields in the related task");
					}
				}
			}
			else if (newObject["bug_id"] != null)
			{
				var filterObj = EntityQuery.QueryEQ("id", (Guid)newObject["bug_id"]);
				var query = new EntityQuery("wv_bug", "*", filterObj, null, null, null);
				var result = recMan.Find(query);
				if (result.Success)
				{
					var bug = result.Object.Data[0];
					//Update the x_billable_hours and x_nonbillable_hours fields
					var updatedRecord = new EntityRecord();
					updatedRecord["id"] = (Guid)bug["id"];

					if(oldBillableString == "billable") {
						updatedRecord["x_billable_hours"] = (decimal)bug["x_billable_hours"] - (decimal)oldObject["hours"];
					}
					else {
						updatedRecord["x_nonbillable_hours"] = (decimal)bug["x_nonbillable_hours"] - (decimal)oldObject["hours"];
					}

					if(newBillableString == "billable") {
						updatedRecord["x_billable_hours"] = (decimal)bug["x_billable_hours"] + (decimal)newObject["hours"];
					}
					else {
						updatedRecord["x_nonbillable_hours"] = (decimal)bug["x_nonbillable_hours"] + (decimal)newObject["hours"];
					}					

					var updateRecordResult = recMan.UpdateRecord("wv_bug",updatedRecord);
					if(!updateRecordResult.Success) {
						throw new Exception("Cannot update the x_billable_hours or x_nonbillable_hours fields in the related task");
					}
				}
			}


			return data;
		}
	}
}
