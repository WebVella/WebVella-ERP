using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Database;
using WebVella.Erp.Eql;
using WebVella.Erp.Hooks;
using WebVella.Erp.Web.Hooks;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Service;

namespace WebVella.Erp.Site.Pages
{
	public class EqlModel : BaseErpPageModel
	{
		public string Input { get; set; }
		public string Result { get; set; }
		public string Sql { get; set; }
		public string Meta { get; set; }

		List<EqlParameter> parameters = new List<EqlParameter> {
				new EqlParameter("@email", null),
				new EqlParameter("@role_name", "administrator"),
				new EqlParameter("@order_field", "first_name"),
				new EqlParameter("@order_direction", "desc"),
				new EqlParameter("@page", 1),
				new EqlParameter("@pagesize", 10)};

		public void OnGet()
		{
			//var instances = HookManager.GetHookedInstances<ITestWebHook>("eql");
			//foreach (ITestWebHook inst in instances)
			//	inst.OnGet(this);

			var users = new SecurityManager().GetUsers(new Guid("bdc56420-caf0-4030-8a0e-d264938e0cda"), new Guid("f16ec6db-626d-4c27-8de0-3e7ce542c55f"));


			const string sourceCode = @"
using System;
using System.Collections.Generic;
using WebVella.Erp.Web.Models;

public class SampleCodeVariable : ICodeVariable
{
	public object Evaluate(BaseErpPageModel pageModel)
	{
		return DateTime.Now;
	}
}";
			var value = CodeEvalService.Evaluate(sourceCode, this);
		
		
				Input =
@"SELECT *, first_name, 
		$user_role.*, 
		$user_role.$user_role_created_by.*,
		$user_role.$user_role_created_by.$user_role.*
FROM user
WHERE ( username = 'system' OR username = 'administrator' ) AND  email <> @email AND $user_role.name @@ @role_name
ORDER BY @order_field @order_direction
PAGE @page
PAGESIZE @pagesize";

			try
			{

				// create, execute, update, delete data source
				const string paramText = @"
				@email,text,null
				@role_name,text,administrator
				@order_field,text,first_name
				@order_direction,text,desc
				@page,int,1
				@pagesize,int,10";

				Guid id = new Guid("564b886b-bbdd-4505-b9df-1de439b56fd9");
				DataSourceManager dataSourceManager = new DataSourceManager();
				var ds = dataSourceManager.Get(id);
				if (ds != null)
					dataSourceManager.Delete(ds.Id);

				ds = dataSourceManager.Create("test", "testing", 1, Input, paramText);
				
				Stopwatch sw = new Stopwatch();
				sw.Start();
				var dsResult = dataSourceManager.Execute(ds.Id, parameters);
				Debug.WriteLine($"Elapsed:{sw.ElapsedMilliseconds}");

				if (ds != null)
					dataSourceManager.Delete(ds.Id);


				//1 variant - creates DbConnection internally using DbContext.Current
				{
					EqlCommand cmd = new EqlCommand(Input, parameters);
					List<EntityRecord> result = cmd.Execute();
					List<EqlFieldMeta> meta = cmd.GetMeta();
					Sql = cmd.GetSql();
					Result = JsonConvert.SerializeObject(result, Formatting.Indented);
					Meta = JsonConvert.SerializeObject(meta, Formatting.Indented);
				}

				//2 variant using DbConnection
				using (var conn = DbContext.Current.CreateConnection())
				{
					EqlCommand cmd = new EqlCommand(Input, conn, parameters);
					List<EntityRecord> result = cmd.Execute();
					List<EqlFieldMeta> meta = cmd.GetMeta();
					Sql = cmd.GetSql();
					Result = JsonConvert.SerializeObject(result, Formatting.Indented);
					Meta = JsonConvert.SerializeObject(meta, Formatting.Indented);
				}

				//3 variant using NpgsqlCommand and eventually NpgsqlTransaction
				using (var conn = new NpgsqlConnection(ErpSettings.ConnectionString))
				{
					NpgsqlTransaction trans = null;
					try
					{
						conn.Open();
						trans = conn.BeginTransaction();
						
						EqlCommand cmd = new EqlCommand(Input, conn, trans, parameters);
						List<EntityRecord> result = cmd.Execute();
						List<EqlFieldMeta> meta = cmd.GetMeta();
						Sql = cmd.GetSql();
						Result = JsonConvert.SerializeObject(result, Formatting.Indented);
						Meta = JsonConvert.SerializeObject(meta, Formatting.Indented);
						
						trans.Commit();
					}
					catch
					{
						if (trans != null)
							trans.Rollback();
						throw;
					}
					finally{
						conn.Close();
					}
				}
			}
			catch(EqlException eqlEx)
			{
				if (eqlEx.Errors.Count > 0)
					Result = eqlEx.Errors[0].Message;
				else
					Result = "Unknown eql error occurred.";
			}
			catch (Exception ex)
			{
				Result = ex.Message;
			}
		}
	}
}