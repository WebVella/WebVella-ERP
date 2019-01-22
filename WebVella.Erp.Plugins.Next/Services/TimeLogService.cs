using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Eql;
using WebVella.Erp.Exceptions;


//TODO develop service
namespace WebVella.Erp.Plugins.Next.Services
{
	public class TimeLogService : BaseService
	{
		public void Create(Guid? id = null, Guid? createdBy = null, DateTime? createdOn = null, DateTime? loggedOn = null, int minutes = 0, bool isBillable = true, string body = "", 
			List<string> scope = null, List<Guid> relatedRecords = null)
		{
			#region << Init >>
			if (id == null)
				id = Guid.NewGuid();

			if (createdBy == null)
				createdBy = SystemIds.SystemUserId;

			if (createdOn == null)
				createdOn = DateTime.UtcNow;

			if (loggedOn == null)
				loggedOn = DateTime.UtcNow;
			#endregion

			try
			{
				var record = new EntityRecord();
				record["id"] = id;
				record["created_by"] = createdBy;
				record["created_on"] = createdOn;
				record["logged_on"] = loggedOn;
				record["body"] = body;
				record["minutes"] = minutes;
				record["is_billable"] = isBillable;
				record["l_scope"] = JsonConvert.SerializeObject(scope);
				record["l_related_records"] = JsonConvert.SerializeObject(relatedRecords);

				var response = RecMan.CreateRecord("timelog", record);
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
				var eqlCommand = "SELECT id,created_by FROM timelog WHERE id = @recordId";
				var eqlParams = new List<EqlParameter>() { new EqlParameter("recordId", recordId) };
				var eqlResult = new EqlCommand(eqlCommand, eqlParams).Execute();
				if (!eqlResult.Any())
					throw new Exception("RecordId not found");
				if ((Guid)eqlResult[0]["created_by"] != SecurityContext.CurrentUser.Id)
					throw new Exception("Only the author can delete its comment");
			}

			var deleteResponse = new RecordManager().DeleteRecord("timelog", recordId);
			if (!deleteResponse.Success)
			{
				throw new Exception(deleteResponse.Message);
			}



		}

		public EntityRecordList GetTimelogsForPeriod(Guid? projectId, Guid? userId, DateTime startDate, DateTime endDate)
		{
			var projectRecord = new EntityRecord();
			var eqlCommand = "SELECT * from timelog WHERE logged_on > @startDate AND logged_on < @endDate ";
			var eqlParams = new List<EqlParameter>() { new EqlParameter("startDate", startDate), new EqlParameter("endDate", endDate) };

			if (projectId != null) {
				eqlCommand += " AND l_related_records CONTAINS @projectId";
				eqlParams.Add(new EqlParameter("projectId",projectId));
			}
			if (userId != null)
			{
				eqlCommand += " AND created_by = @userId";
				eqlParams.Add(new EqlParameter("userId", userId));
			}
			if (userId != null) { }

			var eqlResult = new EqlCommand(eqlCommand, eqlParams).Execute();

			return eqlResult;
		}

	}
}
