using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Services
{
	public class MetaService
	{
		public List<SelectOption> GetEntitiesAsSelectOptions() {
			var entities = new EntityManager().ReadEntities().Object;
			var result = new List<SelectOption>();
			foreach (var entity in entities)
			{
				result.Add(new SelectOption(entity.Id.ToString(), entity.Name));
			}

			return result.OrderBy(x => x.Value).ToList();
		}
	}
}
