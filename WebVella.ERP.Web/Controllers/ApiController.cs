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
    public class ApiController : ApiControllerBase
    {
        public ApiController(IERPService service) : base(service)
        {
        }

        #region << Entity Meta >>
        // Get all entity definitions
        // GET: api/v1/en_US/meta/entity/list/
        [AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/list")]
        public IActionResult GetEntityMetaList()
        {
            return DoResponse(new EntityManager(service.StorageService).ReadEntities());
        }

        // Get entity meta
        // GET: api/v1/en_US/meta/entity/{name}/
        [AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/entity/{Name}")]
        public IActionResult GetEntityMeta(string Name)
        {
            return DoResponse(new EntityManager(service.StorageService).ReadEntity(Name));
        }


        // Create an entity
        // POST: api/v1/en_US/meta/entity
        [AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/meta/entity")]
        public IActionResult CreateEntity([FromBody]InputEntity submitObj)
        {
            return DoResponse(new EntityManager(service.StorageService).CreateEntity(submitObj));
        }

        // Delete an entity
        // DELETE: api/v1/en_US/meta/entity/{id}
        [AcceptVerbs(new[] { "DELETE" }, Route = "api/v1/en_US/meta/entity/{StringId}")]
        public IActionResult DeleteEntity(string StringId)
        {
            EntityManager manager = new EntityManager(service.StorageService);
            EntityResponse response = new EntityResponse();

            // Parse each string representation.
            Guid newGuid;
            Guid id = Guid.Empty;
            if (Guid.TryParse(StringId, out newGuid))
            {
                response = manager.DeleteEntity(newGuid);
            }
            else
            {
                response.Success = false;
                response.Message = "The entity Id should be a valid Guid";
                Context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            return DoResponse(response);
        }

        #endregion

        #region << Entity Fields >>
        [AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/meta/entity/{Id}/field")]
        public IActionResult CreateField(string Id, [FromBody]JObject submitObj)
        {
            FieldResponse response = new FieldResponse();

            Guid entityId;
            if (!Guid.TryParse(Id, out entityId))
            {
                response.Errors.Add(new ErrorModel("id", Id, "id parameter is not valid Guid value"));
                return DoResponse(response);
            }

            return DoResponse(new EntityManager(service.StorageService).CreateField(entityId, Field.ConvertField(submitObj)));
        }

        [AcceptVerbs(new[] { "PUT" }, Route = "api/v1/en_US/meta/entity/{Id}/field/{FieldId}")]
        public IActionResult UpdateField(string Id,string FieldId, [FromBody]InputField submitObj)
        {
            FieldResponse response = new FieldResponse();

            Guid entityId;
            if (!Guid.TryParse(Id, out entityId))
            {
                response.Errors.Add(new ErrorModel("id", Id, "id parameter is not valid Guid value"));
                return DoResponse(response);
            }

            Guid fieldId;
            if (!Guid.TryParse(FieldId, out fieldId))
            {
                response.Errors.Add(new ErrorModel("id", FieldId, "FieldId parameter is not valid Guid value"));
                return DoResponse(response);
            }

            return DoResponse(new EntityManager(service.StorageService).UpdateField(entityId, Field.ConvertField(submitObj)));
        }

        [AcceptVerbs(new[] { "DELETE" }, Route = "api/v1/en_US/meta/entity/{Id}/field/{FieldId}")]
        public IActionResult DeleteField(string Id, string FieldId)
        {
            FieldResponse response = new FieldResponse();

            Guid entityId;
            if (!Guid.TryParse(Id, out entityId))
            {
                response.Errors.Add(new ErrorModel("id", Id, "id parameter is not valid Guid value"));
                return DoResponse(response);
            }

            Guid fieldId;
            if (!Guid.TryParse(FieldId, out fieldId))
            {
                response.Errors.Add(new ErrorModel("id", FieldId, "FieldId parameter is not valid Guid value"));
                return DoResponse(response);
            }

            return DoResponse(new EntityManager(service.StorageService).DeleteField(entityId, fieldId));
        }


        #endregion

    }
}

