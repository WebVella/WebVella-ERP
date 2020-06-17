using System;
using System.Collections.Generic;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Plugins.Mail
{
	public partial class MailPlugin : ErpPlugin
	{
		private static void Patch20200611(EntityManager entMan, EntityRelationManager relMan, RecordManager recMan)
		{
		
#region << ***Update data source*** Name: AllEmails >>
{
	var id = new Guid("82f0b63e-3647-4106-839c-4d5adca4f3b1");
	var name = @"AllEmails";
	var description = @"records of all emails";
	var eqlText = @"SELECT * FROM email
WHERE x_search CONTAINS @searchQuery
ORDER BY @sortBy @sortOrder
PAGE @page
PAGESIZE @pageSize";
	var sqlText = @"SELECT row_to_json( X ) FROM (
SELECT 
	 rec_email.""id"" AS ""id"",
	 rec_email.""subject"" AS ""subject"",
	 rec_email.""content_text"" AS ""content_text"",
	 rec_email.""content_html"" AS ""content_html"",
	 rec_email.""sent_on"" AS ""sent_on"",
	 rec_email.""created_on"" AS ""created_on"",
	 rec_email.""server_error"" AS ""server_error"",
	 rec_email.""retries_count"" AS ""retries_count"",
	 rec_email.""service_id"" AS ""service_id"",
	 rec_email.""priority"" AS ""priority"",
	 rec_email.""reply_to_email"" AS ""reply_to_email"",
	 rec_email.""scheduled_on"" AS ""scheduled_on"",
	 rec_email.""status"" AS ""status"",
	 rec_email.""sender"" AS ""sender"",
	 rec_email.""recipients"" AS ""recipients"",
	 rec_email.""x_search"" AS ""x_search"",
	 rec_email.""attachments"" AS ""attachments"",
	 COUNT(*) OVER() AS ___total_count___
FROM rec_email
WHERE  ( rec_email.""x_search""  ILIKE  CONCAT ( '%' , @searchQuery , '%' ) )
ORDER BY rec_email.""created_on"" DESC
LIMIT 15
OFFSET 0
) X
";
	var parametersJson = @"[{""name"":""sortBy"",""type"":""text"",""value"":""created_on"",""ignore_parse_errors"":false},{""name"":""sortOrder"",""type"":""text"",""value"":""desc"",""ignore_parse_errors"":false},{""name"":""page"",""type"":""int"",""value"":""1"",""ignore_parse_errors"":false},{""name"":""pageSize"",""type"":""int"",""value"":""15"",""ignore_parse_errors"":false},{""name"":""searchQuery"",""type"":""text"",""value"":""string.empty"",""ignore_parse_errors"":false}]";
	var fieldsJson = @"[{""name"":""id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""subject"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""content_text"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""content_html"",""type"":8,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""sent_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""created_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""server_error"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""retries_count"",""type"":12,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""service_id"",""type"":16,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""priority"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""reply_to_email"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""scheduled_on"",""type"":5,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""status"",""type"":17,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""sender"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""recipients"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""x_search"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]},{""name"":""attachments"",""type"":18,""entity_name"":"""",""relation_name"":null,""children"":[]}]";
	var weight = 10;
	var entityName =  @"email";

	new WebVella.Erp.Database.DbDataSourceRepository().Update(id,name,description,weight,eqlText,sqlText,parametersJson,fieldsJson,entityName);
}
#endregion
		}
	}
}
