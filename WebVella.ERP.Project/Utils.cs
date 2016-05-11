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

			//If status is updated we need to check the old one and fix the milestone and project counters

			var record = (EntityRecord)data.record;
			var recordId = (Guid)data.recordId;

			if (!record.Properties.ContainsKey("status"))
			{
				return data;
			}
			EntityRecord updatedTask = null;
			#region << Get the updated project >>
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
				updatedTask = result.Object.Data[0];
			}
			#endregion

			var oldStatus = (string)updatedTask["status"];
			var newStatus = (string)record["status"];
			if (newStatus == oldStatus)
			{
				return data;
			}

			EntityRecord taskProject = null;
			#region << Get the updated project >>
			{
				if (updatedTask["project_id"] == null)
				{
					throw new Exception("Project cannot be null");
				}
				var filterObj = EntityQuery.QueryEQ("id", (Guid)updatedTask["project_id"]);
				var query = new EntityQuery("wv_project", "*", filterObj, null, null, null);
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
					taskProject = result.Object.Data[0];
				}
			}
			#endregion

			EntityRecord taskMilestone = null;
			#region << Get the updated milestone >>
			{
				if (updatedTask["milestone_id"] != null)
				{
					var filterObj = EntityQuery.QueryEQ("id", (Guid)updatedTask["milestone_id"]);
					var query = new EntityQuery("wv_milestone", "*", filterObj, null, null, null);
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
						taskMilestone = result.Object.Data[0];
					}
				}
			}
			#endregion

			#region << Manage the project counter >>
			{
				var patchObject = new EntityRecord();
				patchObject["id"] = (Guid)taskProject["id"];
				switch (oldStatus)
				{
					case "not started":
						if ((decimal)taskProject["x_tasks_not_started"] > 1)
						{
							patchObject["x_tasks_not_started"] = (decimal)taskProject["x_tasks_not_started"] - 1;
						}
						else
						{
							patchObject["x_tasks_not_started"] = 0;
						}
						break;
					case "in progress":
						if ((decimal)taskProject["x_tasks_in_progress"] > 1)
						{
							patchObject["x_tasks_in_progress"] = (decimal)taskProject["x_tasks_in_progress"] - 1;
						}
						else
						{
							patchObject["x_tasks_in_progress"] = 0;
						}
						break;
					case "completed":
						if ((decimal)taskProject["x_tasks_completed"] > 1)
						{
							patchObject["x_tasks_completed"] = (decimal)taskProject["x_tasks_completed"] - 1;
						}
						else
						{
							patchObject["x_tasks_completed"] = 0;
						}
						break;
				}
				switch (newStatus)
				{
					case "not started":
						patchObject["x_tasks_not_started"] = (decimal)taskProject["x_tasks_not_started"] + 1;
						break;
					case "in progress":
						patchObject["x_tasks_in_progress"] = (decimal)taskProject["x_tasks_in_progress"] + 1;
						break;
					case "completed":
						patchObject["x_tasks_completed"] = (decimal)taskProject["x_tasks_completed"] + 1;
						break;
				}
				var updateResponse = recMan.UpdateRecord("wv_project", patchObject);
				if (!updateResponse.Success)
				{
					throw new Exception(updateResponse.Message);
				}
			}
			#endregion

			#region << Manage the milestone counter >>
			{
				if (taskMilestone != null)
				{
					var patchObject = new EntityRecord();
					patchObject["id"] = (Guid)taskMilestone["id"];
					switch (oldStatus)
					{
						case "not started":
							if ((decimal)taskMilestone["x_tasks_not_started"] > 1)
							{
								patchObject["x_tasks_not_started"] = (decimal)taskMilestone["x_tasks_not_started"] - 1;
							}
							else
							{
								patchObject["x_tasks_not_started"] = 0;
							}
							break;
						case "in progress":
							if ((decimal)taskMilestone["x_tasks_in_progress"] > 1)
							{
								patchObject["x_tasks_in_progress"] = (decimal)taskMilestone["x_tasks_in_progress"] - 1;
							}
							else
							{
								patchObject["x_tasks_in_progress"] = 0;
							}
							break;
						case "completed":
							if ((decimal)taskMilestone["x_tasks_completed"] > 1)
							{
								patchObject["x_tasks_completed"] = (decimal)taskMilestone["x_tasks_completed"] - 1;
							}
							else
							{
								patchObject["x_tasks_completed"] = 0;
							}
							break;
					}
					switch (newStatus)
					{
						case "not started":
							patchObject["x_tasks_not_started"] = (decimal)taskMilestone["x_tasks_not_started"] + 1;
							break;
						case "in progress":
							patchObject["x_tasks_in_progress"] = (decimal)taskMilestone["x_tasks_in_progress"] + 1;
							break;
						case "completed":
							patchObject["x_tasks_completed"] = (decimal)taskMilestone["x_tasks_completed"] + 1;
							break;
					}
					var updateResponse = recMan.UpdateRecord("wv_milestone", patchObject);
					if (!updateResponse.Success)
					{
						throw new Exception(updateResponse.Message);
					}
				}
			}
			#endregion

			return data;
		}

	}
}
