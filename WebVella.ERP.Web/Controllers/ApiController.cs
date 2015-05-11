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
    public class ApiController : Controller
    {

        IERPService service;

        public ApiController(IERPService service)
        {
            this.service = service;
        }


        // Get all entity definitions
        // GET: api/v1/en_US/meta/entity/list/
        [AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/list")]
        public IActionResult GetEntityMetaList()
        {
            EntityManager manager = new EntityManager(service.StorageService);
            EntityListResponse response = manager.ReadEntities();

            if (response.Errors.Count > 0)
                Context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return Json(response);
        }

        // Get entity meta
        // GET: api/v1/en_US/meta/entity/{name}/
        [AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{Name}")]
        public IActionResult GetEntityMeta(string Name)
        {
            EntityManager manager = new EntityManager(service.StorageService);
            EntityResponse response = manager.ReadEntity(Name);

            if (response.Errors.Count > 0)
                Context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return Json(response);
        }


		//// Create an entity
		//// POST: api/v1/en_US/meta/entity
		//[AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/meta/entity")]
		//public IActionResult CreateEntity([FromBody]InputEntity submitObj)
		//{
		//    EntityManager manager = new EntityManager(service.StorageService);
		//    EntityResponse response = manager.CreateEntity(submitObj);

		//    if (response.Errors.Count > 0)
		//        Context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

		//    return Json(response);
		//}

        // Create an entity
        // POST: api/v1/en_US/meta/entity
        [AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/meta/entity")]
		public IActionResult CreateEntity([FromBody]EntityRecord obj )
        {
			var h = obj.GetProperties();
			
            var t =  Json(obj);


			return Json(obj);
        }

        [AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/meta/entity/{Id}/field")]
        public IActionResult CreateField(string Id, [FromBody]InputField submitObj)
        {
            FieldResponse response = new FieldResponse();

            Guid entityId;
            if (!Guid.TryParse(Id, out entityId))
            {
                response.Errors.Add(new ErrorModel("Id", Id, "Id parameter is not valid Guid value"));

                Context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(response);
            }

            Field field = Field.Convert(submitObj);

            EntityManager manager = new EntityManager(service.StorageService);
            response = manager.CreateField(entityId, field);

            if (response.Errors.Count > 0)
                Context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return Json(response);
        }
    }
}
