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

	}
}
