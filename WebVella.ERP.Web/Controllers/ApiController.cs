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
        public IActionResult CreateField(string Id, [FromBody]InputField submitObj)
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

        #endregion


        #region << Relation Meta >>
        // Get all entity relation definitions
        // GET: api/v1/en_US/meta/relation/list/
        [AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/relation/list")]
        public IActionResult GetEntityRelationMetaList()
        {
            return DoResponse(new EntityRelationManager(service.StorageService).Read());
        }

        // Get entity relation meta
        // GET: api/v1/en_US/meta/relation/{name}/
        [AcceptVerbs(new[] { "GET" }, Route = "api/v1/en_US/meta/relation/{name}")]
        public IActionResult GetEntityRelationMeta(string name)
        {
            return DoResponse(new EntityRelationManager(service.StorageService).Read(name));
        }


        // Create an entity relation
        // POST: api/v1/en_US/meta/relation
        [AcceptVerbs(new[] { "POST" }, Route = "api/v1/en_US/meta/relation")]
        public IActionResult CreateEntityRelation([FromBody]JObject submitObj)
        {
            try
            {
                var relation = submitObj.ToObject<EntityRelation>();
                return DoResponse(new EntityRelationManager(service.StorageService).Create(relation));
            }
            catch(Exception e)
            {
                return DoBadRequestResponse(new EntityRelationResponse(), null, e);
            }
        }

        // Delete an entity relation
        // DELETE: api/v1/en_US/meta/relation/{idToken}
        [AcceptVerbs(new[] { "DELETE" }, Route = "api/v1/en_US/meta/relation/{idToken}")]
        public IActionResult DeleteEntityRelation(string idToken)
        {
            Guid newGuid;
            Guid id = Guid.Empty;
            if (Guid.TryParse(idToken, out newGuid))
            {
                return DoResponse(new EntityRelationManager(service.StorageService).Delete(newGuid));
            }
            else
            {
                return DoBadRequestResponse(new EntityRelationResponse(), "The entity relation Id should be a valid Guid", null);
            }
            
        }

        #endregion

    }
}

