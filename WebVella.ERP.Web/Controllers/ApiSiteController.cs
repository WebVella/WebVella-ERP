// Test API Controller, will be later substitute by the real data API controller

using Microsoft.AspNet.Mvc;
using WebVella.ERP.Web.Models;
using System;
using Newtonsoft.Json;

namespace WebVella.ERP.Web.Controllers
{
    public class ApiSiteController : Controller
    {
		// GET: api/site/meta
		[AcceptVerbs(new[] { "GET" }, Route = "api/site/meta")]
		public IActionResult SiteMeta()
        {
			//TODO - Test data
			var response = new ResponseObj();
			response.Success = true;
			var siteMeta = new SiteMeta();
			var area = new Area();
			var areaSection = new AreaSection();
			var entity = new Entity();

			//Area 1
			area = new Area();
			area.Id = Guid.NewGuid();
			area.Name = "Higher Ed";
			area.Label = "Logistics";
			area.Weight = 1;
			area.HasDashboard = true;

			//Area1 SectionNull
			areaSection = new AreaSection();
			areaSection.Id = Guid.Empty;
			areaSection.Name = "null";
			areaSection.Label = "Null";
			areaSection.Weight = 1;
			//// Entity 1
			entity = new Entity();
			entity.Id = Guid.NewGuid();
			entity.Name = "order";
			entity.IsSystem = false;
			entity.Label = "Order";
			entity.LabelPlural = "Orders";
			entity.IconName = "shopping-cart";
			entity.Weight = 0;
			areaSection.Entities.Add(entity);
			//// Add Section to Area
			area.Sections.Add(areaSection);

			//Area1 Section 1
			areaSection = new AreaSection();
			areaSection.Id = Guid.Empty;
			areaSection.Name = "section1";
			areaSection.Label = "Area Section 1";
			areaSection.Weight = 1;
			//// Entity 2
			entity = new Entity();
			entity.Id = Guid.NewGuid();
			entity.Name = "entity2";
			entity.IsSystem = false;
			entity.Label = "entity 2";
			entity.LabelPlural = "entity 2s";
			entity.IconName = "files-o";
			entity.Weight = 1;
			areaSection.Entities.Add(entity);
			//// Entity 3
			entity = new Entity();
			entity.Id = Guid.NewGuid();
			entity.Name = "entity3";
			entity.IsSystem = false;
			entity.Label = "entity 3";
			entity.LabelPlural = "entity 3s";
			entity.IconName = "users";
			entity.Weight = 2;
			areaSection.Entities.Add(entity);
			//// Add Section to Area
			area.Sections.Add(areaSection);

			//Area1 Section2
			areaSection = new AreaSection();
			areaSection.Id = Guid.Empty;
			areaSection.Name = "section2";
			areaSection.Label = "Area Section 2";
			areaSection.Weight = 2;
			//// Entity 4
			entity = new Entity();
			entity.Id = Guid.NewGuid();
			entity.Name = "entity4";
			entity.IsSystem = false;
			entity.Label = "entity 4";
			entity.LabelPlural = "entity 4s";
			entity.IconName = "cog";
			entity.Weight = 1;
			areaSection.Entities.Add(entity);
			//// Entity 5
			entity = new Entity();
			entity.Id = Guid.NewGuid();
			entity.Name = "entity5";
			entity.IsSystem = false;
			entity.Label = "entity 5";
			entity.LabelPlural = "entity 5s";
			entity.IconName = "line-chart";
			entity.Weight = 2;
			areaSection.Entities.Add(entity);
			//// Add Section to Area
			area.Sections.Add(areaSection);

			////Add Area to SiteMeta
			siteMeta.Areas.Add(area);

			//Area 2
			area = new Area();
			area.Id = Guid.NewGuid();
			area.Name = "area2";
			area.Label = "Area 2";
			area.Weight = 2;
			area.HasDashboard = true;

			////Add Area to SiteMeta
			siteMeta.Areas.Add(area);

			//Area 3
			area = new Area();
			area.Id = Guid.NewGuid();
			area.Name = "area3";
			area.Label = "Area 3";
			area.Weight = 3;
			area.HasDashboard = true;

			////Add Area to SiteMeta
			siteMeta.Areas.Add(area);

			//Area 4
			area = new Area();
			area.Id = Guid.NewGuid();
			area.Name = "area4";
			area.Label = "Area 4";
			area.Weight = 4;
			area.HasDashboard = true;

			////Add Area to SiteMeta
			siteMeta.Areas.Add(area);

			//Area 5
			area = new Area();
			area.Id = Guid.NewGuid();
			area.Name = "area5";
			area.Label = "Area 5";
			area.Weight = 5;
			area.HasDashboard = true;

			////Add Area to SiteMeta
			siteMeta.Areas.Add(area);



			response.Object = siteMeta;
			return Json(response);
        }
    }
}
