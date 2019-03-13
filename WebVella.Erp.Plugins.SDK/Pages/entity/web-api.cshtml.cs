using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.AutoMapper;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Plugins.SDK.Utils;
using WebVella.Erp.Web;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Plugins.SDK.Pages.ErpEntity
{
	public class EntityWebApiModel : BaseErpPageModel
	{
		public EntityWebApiModel([FromServices]ErpRequestContext reqCtx) { ErpRequestContext = reqCtx; }

		public Entity ErpEntity { get; set; }

		public string DomainRoot { get; set; }

		public string EntityMetaUrlRoot { get; set; }

		public string EntityRecordUrlRoot { get; set; }

		public string EQLUrlRoot { get; set; }

		public Guid? SampleFieldId { get; set; } = null;

		public List<string> HeaderToolbar { get; private set; } = new List<string>();

		public IActionResult OnGet()
		{
			var initResult = Init();
			if (initResult != null)
				return initResult;

			var entMan = new EntityManager();
			var recMan = new RecordManager();

			#region << InitPage >>

			ErpEntity = entMan.ReadEntity(ParentRecordId ?? Guid.Empty).Object;

			if (ErpEntity == null)
				return NotFound();

			if (string.IsNullOrWhiteSpace(ReturnUrl))
				ReturnUrl = $"/sdk/objects/entity/r/{ErpEntity.Id}/";

			DomainRoot = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;
			EntityMetaUrlRoot = "/api/v3/en_US/meta/entity/";

			EntityRecordUrlRoot = "/api/v3/en_US/record/";

			EQLUrlRoot = "/api/v3/en_US/eql";

			if (ErpEntity.Fields.Any()) {
				SampleFieldId = ErpEntity.Fields[0].Id;
			}

			HeaderToolbar.AddRange(AdminPageUtils.GetEntityAdminSubNav(ErpEntity, "web-api"));

			#endregion

			BeforeRender();
			return Page();
		}
	}
}
