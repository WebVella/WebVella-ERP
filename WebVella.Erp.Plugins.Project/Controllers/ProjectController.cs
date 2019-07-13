using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Database;
using WebVella.Erp.Diagnostics;
using WebVella.Erp.Eql;
using WebVella.Erp.Plugins.Project.Services;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Plugins.Project.Controllers
{
	[Authorize]
	public class ProjectController : Controller
	{
		private const char RELATION_SEPARATOR = '.';
		private const char RELATION_NAME_RESULT_SEPARATOR = '$';

		RecordManager recMan;
		EntityManager entMan;
		EntityRelationManager relMan;
		SecurityManager secMan;
		IErpService erpService;

		public ProjectController(IErpService erpService)
		{
			recMan = new RecordManager();
			secMan = new SecurityManager();
			entMan = new EntityManager();
			relMan = new EntityRelationManager();
			this.erpService = erpService;
		}

		#region << Components >>
		[Route("api/v3.0/p/project/pc-post-list/create")]
		[HttpPost]
		public ActionResult CreateNewPcPostListItem([FromBody]EntityRecord record)
		{
			var response = new ResponseModel();
			#region << Init >>
			var recordId = Guid.NewGuid();

			Guid relatedRecordId = Guid.Empty;
			if (!record.Properties.ContainsKey("relatedRecordId") || record["relatedRecordId"] == null)
			{
				throw new Exception("relatedRecordId is required");
			}
			if (Guid.TryParse((string)record["relatedRecordId"], out Guid outGuid))
			{
				relatedRecordId = outGuid;
			}
			else
			{
				throw new Exception("relatedRecordId is invalid Guid");
			}

			Guid? parentId = null;
			if (record.Properties.ContainsKey("parentId") && record["parentId"] != null)
			{
				if (Guid.TryParse((string)record["parentId"], out Guid outGuid2))
				{
					parentId = outGuid2;
				}
			}

			var scope = new List<string>() { "projects" };

			var relatedRecords = new List<Guid>();
			if (record.Properties.ContainsKey("relatedRecords") && record["relatedRecords"] != null)
			{
				relatedRecords = JsonConvert.DeserializeObject<List<Guid>>((string)record["relatedRecords"]);
			}

			var subject = "";
			if (record.Properties.ContainsKey("subject") && record["subject"] != null)
			{
				subject = (string)record["subject"];
			}

			var body = "";
			if (record.Properties.ContainsKey("body") && record["body"] != null)
			{
				body = (string)record["body"];
			}

			Guid currentUserId = SystemIds.FirstUserId; //This is for web component development to allow guest submission
			if (SecurityContext.CurrentUser != null)
				currentUserId = SecurityContext.CurrentUser.Id;

			#endregion

			try
			{
				new CommentService().Create(recordId, currentUserId, DateTime.Now, body, parentId, scope, relatedRecords);
			}
			catch (Exception ex)
			{
				throw ex;
			}

			response.Success = true;
			response.Message = "Comment successfully created";

			var eqlCommand = @"SELECT *,$user_1n_comment.image,$user_1n_comment.username
					FROM comment
					WHERE id = @recordId";
			var eqlParams = new List<EqlParameter>() {
						new EqlParameter("recordId",recordId)
					};
			var eqlResult = new EqlCommand(eqlCommand, eqlParams).Execute();

			if (eqlResult.Any())
			{
				response.Object = eqlResult.First();
			}


			return Json(response);
		}

		[Route("api/v3.0/p/project/pc-post-list/delete")]
		[HttpPost]
		public ActionResult DeletePcPostListItem([FromBody]EntityRecord record)
		{
			var response = new ResponseModel();
			#region << Init >>
			Guid recordId = Guid.Empty;
			if (!record.Properties.ContainsKey("id") || record["id"] == null)
			{
				throw new Exception("id is required");
			}
			if (Guid.TryParse((string)record["id"], out Guid outGuid))
			{
				recordId = outGuid;
			}
			else
			{
				throw new Exception("id is invalid Guid");
			}

			#endregion
			try
			{
				new CommentService().Delete(recordId);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			response.Success = true;
			response.Message = "Comment successfully deleted";

			return Json(response);
		}

		[Route("api/v3.0/p/project/pc-timelog-list/create")]
		[HttpPost]
		public ActionResult CreateTimelog([FromBody]EntityRecord record)
		{
			var response = new ResponseModel();
			#region << Init >>
			var recordId = Guid.NewGuid();


			var scope = new List<string>() { "projects" };

			var relatedRecords = new List<Guid>();
			if (record.Properties.ContainsKey("relatedRecords") && record["relatedRecords"] != null)
			{
				relatedRecords = JsonConvert.DeserializeObject<List<Guid>>((string)record["relatedRecords"]);
			}

			var body = "";
			if (record.Properties.ContainsKey("body") && record["body"] != null)
			{
				body = (string)record["body"];
			}

			Guid currentUserId = SystemIds.FirstUserId; //This is for web component development to allow guest submission
			if (SecurityContext.CurrentUser != null)
				currentUserId = SecurityContext.CurrentUser.Id;


			var minutes = 0;
			if (record.Properties.ContainsKey("minutes") && record["minutes"] != null)
			{
				if (Int32.TryParse(record["minutes"].ToString(), out Int32 outInt32))
				{
					minutes = outInt32;
				}
			}

			var isBillable = false;
			if (record.Properties.ContainsKey("isBillable") && record["isBillable"] != null)
			{
				if (Boolean.TryParse(record["isBillable"].ToString(), out bool outBool))
				{
					isBillable = outBool;
				}
			}

			var loggedOn = new DateTime();
			if (record.Properties.ContainsKey("loggedOn") && record["loggedOn"] != null)
			{
				loggedOn = (DateTime)record["loggedOn"];
			}

			#endregion

			try
			{
				new TimeLogService().Create(recordId, currentUserId, DateTime.Now, loggedOn, minutes, isBillable, body, scope, relatedRecords);
			}
			catch (Exception ex)
			{
				throw ex;
			}

			response.Success = true;
			response.Message = "Timelog successfully created";

			var eqlCommand = @"SELECT *,$user_1n_timelog.image,$user_1n_timelog.username
								FROM timelog
								WHERE id = @recordId";
			var eqlParams = new List<EqlParameter>() { new EqlParameter("recordId", recordId) };
			var eqlResult = new EqlCommand(eqlCommand, eqlParams).Execute();

			if (eqlResult.Any())
			{
				response.Object = eqlResult.First();
			}

			return Json(response);
		}

		[Route("api/v3.0/p/project/pc-timelog-list/delete")]
		[HttpPost]
		public ActionResult DeleteTimelog([FromBody]EntityRecord record)
		{
			var response = new ResponseModel();
			#region << Init >>

			Guid recordId = Guid.Empty;
			if (!record.Properties.ContainsKey("id") || record["id"] == null)
			{
				throw new Exception("id is required");
			}
			if (Guid.TryParse((string)record["id"], out Guid outGuid))
			{
				recordId = outGuid;
			}
			else
			{
				throw new Exception("id is invalid Guid");
			}

			#endregion

			try
			{
				new TimeLogService().Delete(recordId);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			response.Success = true;
			response.Message = "Comment successfully deleted";


			return Json(response);
		}

		[Route("api/v3.0/p/project/timelog/start")]
		[HttpPost]
		public ActionResult StartTimeLog([FromQuery]Guid taskId)
		{
			var response = new ResponseModel();
			//Validate
			var task = new TaskService().GetTask(taskId);
			if (task == null)
			{
				response.Success = false;
				response.Message = "task not found";
				return Json(response);
			}
			if (task["timelog_started_on"] != null) {
				response.Success = false;
				response.Message = "timelog for the task already started";
				return Json(response);
			}
			try
			{
				new TaskService().StartTaskTimelog(taskId);
				response.Success = true;
				response.Message = "Log Started";
				return Json(response);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
				return Json(response);
			}
		}

		//[Route("api/v3.0/p/project/timelog/stop")]
		//[HttpPost]
		//public ActionResult StopTimeLog([FromQuery]Guid taskId)
		//{
		//	var response = new ResponseModel();
		//	//Validate

		//	using (var connection = DbContext.Current.CreateConnection())
		//	{
		//		try
		//		{
		//			connection.BeginTransaction();

		//			new TaskService().StopTaskTimelog(taskId);

		//			//Create Time log

		//			connection.CommitTransaction();

		//			response.Success = true;
		//			response.Message = "Log Stopped";
		//			return Json(response);
		//		}
		//		catch (Exception ex)
		//		{
		//			connection.RollbackTransaction();

		//			response.Success = false;
		//			response.Message = ex.Message;
		//			return Json(response);
		//		}
		//	}
		//}

		[Route("api/v3.0/p/project/task/status")]
		[HttpPost]
		public ActionResult TaskSetStatus([FromQuery]Guid taskId, [FromQuery]Guid statusId)
		{
			var response = new ResponseModel();
			//Validate
			var task = new TaskService().GetTask(taskId);
			if (task == null)
			{
				response.Success = false;
				response.Message = "task not found";
				return Json(response);
			}
			if (task["status_id"] != null && (Guid)task["status_id"] == statusId)
			{
				response.Success = false;
				response.Message = "status already set";
				return Json(response);
			}
			try
			{
				new TaskService().SetStatus(taskId, statusId);
				response.Success = true;
				response.Message = "Log Started";
				return Json(response);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
				return Json(response);
			}
		}

		[Route("api/v3.0/p/project/task/watch")]
		[HttpPost]
		public ActionResult TaskSetWatch([FromQuery]Guid? taskId = null, [FromQuery]Guid? userId = null, [FromQuery]bool startWatch = true)
		{
			var response = new ResponseModel();
			if (taskId == null) {
				response.Success = false;
				response.Message = "Missing taskId query parameter";
				return Json(response);
			}
			//Validate
			var task = new TaskService().GetTask(taskId.Value);
			if (task == null)
			{
				response.Success = false;
				response.Message = "task not found";
				return Json(response);
			}
			if (userId != null)
			{
				var userRecord = new UserService().Get(userId.Value);
				if (userRecord == null)
				{
					response.Success = false;
					response.Message = "user not found";
					return Json(response);
				}
			}
			else {
				userId = SecurityContext.CurrentUser.Id;
			}

			try
			{
				var watchRelation = new EntityRelationManager().Read("user_nn_task_watchers").Object;
				if (watchRelation == null)
					throw new Exception("Watch relation not found");

				if (startWatch)
				{
					var createRelResponse = new RecordManager().CreateRelationManyToManyRecord(watchRelation.Id, userId.Value, taskId.Value);
					if (!createRelResponse.Success)
						throw new Exception(createRelResponse.Message);

					response.Message = "Task watch started";
				}
				else {
					var removeRelResponse = new RecordManager().RemoveRelationManyToManyRecord(watchRelation.Id, userId.Value, taskId.Value);
					if (!removeRelResponse.Success)
						throw new Exception(removeRelResponse.Message);

					response.Message = "Task watch stopped";
				}

				response.Success = true;
				return Json(response);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
				return Json(response);
			}
		}


		[AllowAnonymous]
		[Route("api/v3.0/p/project/files/javascript")]
		[ResponseCache(NoStore = false, Duration = 30 * 24 * 3600)]
		[HttpGet]
		public ContentResult TimeTrackJs([FromQuery]string file = "")
		{
			if(String.IsNullOrWhiteSpace(file))
				return Content("", "text/javascript");
			try
			{
				var jsContent = FileService.GetEmbeddedTextResource(file, "WebVella.Erp.Plugins.Project.Files", "WebVella.Erp.Plugins.Project");
			
				return Content(jsContent, "text/javascript");
			}
			catch (Exception ex)
			{
				new Log().Create(LogType.Error, file + " API File get Method Error", ex);
				throw ex;
			}
		}


		#endregion

	}


}