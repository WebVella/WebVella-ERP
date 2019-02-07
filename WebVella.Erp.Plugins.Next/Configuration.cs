using System;
using System.Collections.Generic;
using System.Text;

namespace WebVella.Erp.Plugins.Next
{
	public static class Configuration
	{
		public static List<string> AccountSearchIndexFields { get; private set; } 
			= new List<string>() { "city", "$country_1n_account.label", "email", "fax_phone", "first_name", "fixed_phone", "last_name", 
					"mobile_phone", "name", "notes", "post_code", "region", "street", "street_2", "tax_id", "type", "website" };

		public static List<string> CaseSearchIndexFields { get; private set; }
					= new List<string>() { "$account_nn_case.name", "description", "number", "priority", "$case_status_1n_case.label",
					"$case_type_1n_case.label", "subject" };

		public static List<string> ContactSearchIndexFields { get; private set; }
					= new List<string>() { "city", "$country_1n_contact.label", "$account_nn_contact.name", "email", "fax_phone", "first_name",
					"fixed_phone", "job_title", "last_name", "mobile_phone", "notes", "post_code", "region", "street", "street_2"};

		public static List<string> TaskSearchIndexFields { get; private set; }
					= new List<string>() { "key", "subject", "body", "priority", "$task_status_1n_task.label", "$task_type_1n_task.label" };
	}
}
