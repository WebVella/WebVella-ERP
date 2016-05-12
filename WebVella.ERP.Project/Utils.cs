using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Project
{
	public class Utils
	{

		public static List<ErrorModel> ValidateTask(List<ErrorModel> currentErrors, EntityRecord taskObject, Guid recordId)
		{
			var errorList = currentErrors;

			return errorList;
		}

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
				if(newTaskObject.Properties.ContainsKey("project_id") && newTaskObject["project_id"] != null) {
					newTaskObject["project_id"] = new Guid((string)newTaskObject["project_id"]);
				}
				else if (newTaskObject.Properties.ContainsKey("project_id")) {
					newTaskObject["project_id"] = null;
				}

				if(newTaskObject.Properties.ContainsKey("milestone_id") && newTaskObject["milestone_id"] != null) {
					newTaskObject["milestone_id"] = new Guid((string)newTaskObject["milestone_id"]);
				}
				else if (newTaskObject.Properties.ContainsKey("milestone_id")) {
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
					if(!newTaskObject.Properties.ContainsKey("status")) {
						newTaskObject["status"] = (string)oldTaskObject["status"];
					}

				}
				#endregion

				var isProjectChanged = false;
				var isMilestoneChanged = false;
				var isStatusChanged = false;
				#region << Check what is changed >>
				var oldStatus = (string)oldTaskObject["status"];
				var newStatus = "";
				if (newTaskObject.Properties.ContainsKey("status") && oldStatus != (string)newTaskObject["status"])
				{
					isStatusChanged = true;
					newStatus = (string)newTaskObject["status"];
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
								if (isProjectChanged && newTaskObject["project_id"]!= null && (Guid)resultObject["id"] == (Guid)newTaskObject["project_id"])
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
					if(oldProjectPatchObject.Properties.Count > 0) {
						oldProjectPatchObject = UpdateProjectOrTaskCounter(oldProjectPatchObject, (string)oldTaskObject["status"], -1);
						isOldProjectPatched = true;
					}
					//Add one to the new project <> new status if the target is not null
					if(newProjectPatchObject.Properties.Count > 0) {
						newProjectPatchObject = UpdateProjectOrTaskCounter(newProjectPatchObject, (string)newTaskObject["status"], 1);
						isNewProjectPatched = true;
					}
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
					if(oldMilestonePatchObject.Properties.Count > 0) {
						oldMilestonePatchObject = UpdateProjectOrTaskCounter(oldMilestonePatchObject, (string)oldTaskObject["status"], -1);
						isOldMilestonePatched = true;
					}
					//Add one to the new milestone <> new status if the target is not null
					if(newMilestonePatchObject.Properties.Count > 0) {
						newMilestonePatchObject = UpdateProjectOrTaskCounter(newMilestonePatchObject, (string)newTaskObject["status"], 1);
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
						else if(oldProjectPatchObject.Properties.Count > 0 )
						{
							//Remove one from the old project old status
							oldProjectPatchObject = UpdateProjectOrTaskCounter(oldProjectPatchObject, (string)oldTaskObject["status"], -1);
							//Add one from the old project new status
							oldProjectPatchObject = UpdateProjectOrTaskCounter(oldProjectPatchObject, (string)newTaskObject["status"], 1);
							isOldProjectPatched = true;
						}
						if (isMilestoneChanged)
						{
							//the status change is already set in the new milestone object in case 2
						}
						else if(oldMilestonePatchObject.Properties.Count > 0)
						{
							//Remove one from the old milestone old status
							oldMilestonePatchObject = UpdateProjectOrTaskCounter(oldMilestonePatchObject, (string)oldTaskObject["status"], -1);
							//Add one from the old milestone new status	
							oldMilestonePatchObject = UpdateProjectOrTaskCounter(oldMilestonePatchObject, (string)newTaskObject["status"], 1);
							isOldMilestonePatched = true;
						}
					}
					else
					{
						//do nothing
					}
				}
				#endregion

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

				#region << Create activity >>
					#region << Old Task activity >>
					//{
					//	var activityObj = new EntityRecord();
					//	activityObj["id"] = Guid.NewGuid();
					//	activityObj["task_id"] = (Guid)oldTaskObject["id"];
					//	if(newTaskProject.Properties.Count > 0) {
					//		activityObj["project_id"] = (Guid)newTaskProject["id"];
					//	}
					//	else {
					//		activityObj["project_id"] = (Guid)oldTaskProject["id"];
					//	}
					//	activityObj["subject"] = @"updated the task status from <strong>in progress</strong> to <strong>completed</strong>";
					//	activityObj["label"] = "updated";
					//	var createResponse = recMan.CreateRecord("wv_project_activity", activityObj);
					//	if (!createResponse.Success)
					//	{
					//		throw new Exception(createResponse.Message);
					//	}
					//}
					#endregion

					#region << Project activity >>
					{
					}
					#endregion

				#endregion

				return data;
			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}
		}

		public static EntityRecord UpdateProjectOrTaskCounter(EntityRecord targetObject, string status, decimal count)
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
			}

			return targetObject;
		}

	}
}
