// Test API Controller, will be later substitute by the real data API controller

using Microsoft.AspNet.Mvc;
using WebVella.ERP.Api.Models;
using System;
using Newtonsoft.Json;
using System.Threading;
using System.Collections.Generic;

namespace WebVella.ERP.Web.Controllers
{
    public class ApiSandboxController : ApiControllerBase
    {

        public ApiSandboxController(IErpService service) : base(service)
        {
        }

        [AcceptVerbs(new[] { "GET" }, Route = "sandbox/api/entity/{entityName}/records/list")]
        public IActionResult GetRecordsByEntityName(string entityName)
        {
            //TODO - Test data
            var response = new EntityListResponse();
            response.Success = true;
            var responseObj = new EntityList();
            responseObj.Entities = new List<Entity>();
            response.Object = responseObj;
            switch (entityName)
            {
                case "role":

                    #region ///////////////////////// ROLES ////////////////////////

                    var roles = new List<Entity>();
                    var role = new Entity();

                    //Add Administrator
                    role = new Entity();
                    role.Id = new Guid("0b3fa332-6018-46e4-a70c-297b30c2b19c");
                    role.Name = "administrator";
                    role.Label = "Administrator";
                    role.LabelPlural = "Administrators";
                    roles.Add(role);

                    //Add Authenticated users
                    role = new Entity();
                    role.Id = new Guid("5cdf06ed-a627-4a71-a73b-43bdd390dbf1");
                    role.Name = "authenticated";
                    role.Label = "Authenticated user";
                    role.LabelPlural = "Authenticated users";
                    roles.Add(role);

                    //Add Guest
                    role = new Entity();
                    role.Id = new Guid("5cdf06ed-a627-4a71-a73b-43bdd390db22");
                    role.Name = "guest";
                    role.Label = "Guest";
                    role.LabelPlural = "Guests";
                    roles.Add(role);
                    responseObj.Entities = roles;
                break;
                #endregion

                case "area":
                    ///////////////////////// AREA ////////////////////////
                    var areas = new List<Entity>();
                    var area = new Entity();

                    //Entity 1
                    area = new Entity();
                    area.Id = Guid.NewGuid();
                    area.Name = "test";
                    area.Label = "Test";
                    area.Weight = 4;
                    area.IconName = "cloud";
                    areas.Add(area);
                    responseObj.Entities = areas;
                    break;

            }
            Thread.Sleep(100);
            //response.Object = siteMeta;
            return Json(response);
        }



    }
}
