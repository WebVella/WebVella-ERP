using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;
using System.Net;
using Newtonsoft.Json.Linq;
using WebVella.ERP.Utilities.Dynamic;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebVella.ERP.Web.Controllers
{
    public partial class ApiDevelopersController : ApiControllerBase
    {
        public ApiDevelopersController(IERPService service) : base(service)
        {
        }

		[AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/meta/developers/query/create-sample-query-data-structure")]
		public IActionResult CreateSampleQueryDataStructure()
		{
			var queryObject = EntityQuery.QueryAND(EntityQuery.QueryEQ("id", Guid.NewGuid()), EntityQuery.QueryEQ("id", Guid.NewGuid()));
			EntityQuery query = new EntityQuery("customer", "id,createdBy",queryObject );
			RecordManager rm = new RecordManager(service);
			var result = rm.Find(query);

			QueryResponse response = new QueryResponse();
			response.Success = true;
			response.Timestamp = DateTime.UtcNow;
			response.Message = "CreateSampleQueryDataStructure:DONE";
            return Json(response);
		}

		[AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/meta/developers/query/execute-sample-query")]
		public IActionResult ExecuteSampleQuery()
		{
			QueryResponse response = new QueryResponse();
			response.Success = true;
			response.Timestamp = DateTime.UtcNow;
			response.Message = "ExecuteSampleQuery:DONE";
			return Json(response);
		}



		//[AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/meta/entity")]
		//public IActionResult CreateEntity([FromBody]EntityRecord obj)
		//{
		//	var h = obj.GetProperties();

		//	var t = Json(obj);


		//	return Json(obj);
		//}

		
	}
}
