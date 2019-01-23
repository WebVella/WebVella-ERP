using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Exceptions;


//TODO develop service
namespace WebVella.Erp.Plugins.Project.Services
{
	public class FeedItemService : BaseService
	{
		public void Create(Guid? id= null, Guid? createdBy = null, DateTime? createdOn = null, 
			string subject = "", string body = "", List<string> relatedRecords = null, 
			List<string> scope = null, string type = "")
		{
			#region << Init >>
			if (id == null)
				id = Guid.NewGuid();
			
			if(createdBy == null)
				createdBy = SystemIds.SystemUserId;

			if (createdOn == null)
				createdOn = DateTime.Now;

			if (String.IsNullOrWhiteSpace(type)) {
				type = "system";
			}
			#endregion
			try
			{
				var record = new EntityRecord();
				record["id"] = id;
				record["created_by"] = createdBy;
				record["created_on"] = createdOn;
				record["subject"] = subject;
				record["body"] = body;
				record["l_related_records"] = JsonConvert.SerializeObject(relatedRecords);
				record["l_scope"] = JsonConvert.SerializeObject(scope);
				record["type"] = type;
				var createFeedResponse = new RecordManager().CreateRecord("feed_item", record);
				if (!createFeedResponse.Success)
					throw new Exception(createFeedResponse.Message);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

	}
}
