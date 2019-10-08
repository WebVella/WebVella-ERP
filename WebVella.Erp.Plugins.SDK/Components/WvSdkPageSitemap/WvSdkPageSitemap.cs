using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Plugins.SDK.Model;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;
using Yahoo.Yui.Compressor;
using WebVella.TagHelpers.Models;
namespace WebVella.Erp.Plugins.SDK.Components
{
	public class WvSdkPageSitemap : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync(Guid? pageId = null, WvFieldRenderMode mode = WvFieldRenderMode.Form,
			PageType? presetType = null, Guid? presetAppId = null, Guid? presetEntityId = null)
		{
			//var typeOptions = new List<SelectFieldOption>();
			//var entityOptions = new List<SelectFieldOption>();
			//var applicationOptions = new List<SelectFieldOption>();
			//var areaOptions = new List<SelectFieldOption>();
			//var nodeOptions = new List<SelectFieldOption>();
			var pageSelectionTree = new PageSelectionTree();
			var erpPage = new ErpPage();
			if (pageId == null)
			{
				if (presetType != null)
				{
					erpPage.Type = presetType ?? PageType.Site;
				}
				if (presetAppId != null)
				{
					erpPage.AppId = presetAppId.Value;
				}
				if (presetEntityId != null)
				{
					erpPage.EntityId = presetEntityId.Value;
					erpPage.Type = PageType.RecordList;
				}
			}
			var pageSrv = new PageService();
			var typeOptionsFieldId = Guid.NewGuid();
			var appOptionsFieldId = Guid.NewGuid();
			var areaOptionsFieldId = Guid.NewGuid();
			var nodeOptionsFieldId = Guid.NewGuid();
			var entityOptionsFieldId = Guid.NewGuid();

			#region << Init >>
			var apps = new AppService().GetAllApplications();
			var entities = new EntityManager().ReadEntities().Object;

			#region << ErpPage && Init it>>
			if (pageId != null)
			{
				erpPage = pageSrv.GetPage(pageId ?? Guid.Empty);
				if (erpPage == null)
				{
					ViewBag.ErrorMessage = "Error: the set pageId is not found as ErpPage!";
					return await Task.FromResult<IViewComponentResult>(View("Error"));
				}

				if (erpPage.Type == PageType.Application && erpPage.AppId == null)
				{
					ViewBag.ErrorMessage = "Error: Application should have AppId!";
					return await Task.FromResult<IViewComponentResult>(View("Error"));
				}



			}
			#endregion

			#region << Type options >>
			{
				pageSelectionTree.AllTypes = ModelExtensions.GetEnumAsSelectOptions<PageType>().OrderBy(x => x.Label).ToList();

			}
			#endregion

			#region << App options >>
			{
				foreach (var app in apps)
				{
					pageSelectionTree.AllApps.Add(new SelectOption()
					{
						Value = app.Id.ToString(),
						Label = app.Name
					});
					//Set App tree
					var appSelectionTree = new AppSelectionTree();
					appSelectionTree.AppId = app.Id;
					foreach (var area in app.Sitemap.Areas)
					{
						appSelectionTree.AllAreas.Add(new SelectOption()
						{
							Value = area.Id.ToString(),
							Label = area.Name
						});
						var areaSelectionTree = new AreaSelectionTree()
						{
							AreaId = area.Id
						};
						foreach (var node in area.Nodes)
						{
							areaSelectionTree.AllNodes.Add(new SelectOption()
							{
								Value = node.Id.ToString(),
								Label = node.Name
							});
						}
						areaSelectionTree.AllNodes = areaSelectionTree.AllNodes.OrderBy(x => x.Label).ToList();
						appSelectionTree.AreaSelectionTree.Add(areaSelectionTree);
					}
					pageSelectionTree.AppSelectionTree.Add(appSelectionTree);

					//Set Entities
					foreach (var entity in app.Entities)
					{
						appSelectionTree.Entities.Add(new SelectOption()
						{
							Value = entity.Entity.Id.ToString(),
							Label = entity.Entity.Name
						});
					}
					appSelectionTree.Entities = appSelectionTree.Entities.OrderBy(x => x.Label).ToList();
				}
				pageSelectionTree.AllApps = pageSelectionTree.AllApps.OrderBy(x => x.Label).ToList();

			}
			#endregion

			#region << Entity options >>
			foreach (var entity in entities)
			{
				pageSelectionTree.AllEntities.Add(new SelectOption()
				{
					Value = entity.Id.ToString(),
					Label = entity.Name
				});
			}
			pageSelectionTree.AllEntities = pageSelectionTree.AllEntities.OrderBy(x => x.Label).ToList();
			#endregion

			#endregion

			ViewBag.PageSelectionTree = pageSelectionTree;
			ViewBag.ErpPage = erpPage;
			ViewBag.TypeOptionsFieldId = typeOptionsFieldId;
			ViewBag.AppOptionsFieldId = appOptionsFieldId;
			ViewBag.AreaOptionsFieldId = areaOptionsFieldId;
			ViewBag.NodeOptionsFieldId = nodeOptionsFieldId;
			ViewBag.EntityOptionsFieldId = entityOptionsFieldId;

			var pageSelectionTreeJson = JsonConvert.SerializeObject(pageSelectionTree);

			ViewBag.EmbededJs = "";
			#region << Generate js script >>
			if (mode == WvFieldRenderMode.Form)
			{
				var jsCompressor = new JavaScriptCompressor();

				#region << Init Scripts >>

				var fileName = "form.js";
				var scriptEl = "<script type=\"text/javascript\">";
				var scriptTemplate = FileService.GetEmbeddedTextResource(fileName, "WebVella.Erp.Plugins.SDK.Components.WvSdkPageSitemap", "WebVella.Erp.Plugins.SDK");

				scriptTemplate = scriptTemplate.Replace("\"{{PageSelectionTreeJson}}\";", pageSelectionTreeJson);
				scriptTemplate = scriptTemplate.Replace("{{typeOptionsFieldId}}", typeOptionsFieldId.ToString());
				scriptTemplate = scriptTemplate.Replace("{{appOptionsFieldId}}", appOptionsFieldId.ToString());
				scriptTemplate = scriptTemplate.Replace("{{areaOptionsFieldId}}", areaOptionsFieldId.ToString());
				scriptTemplate = scriptTemplate.Replace("{{nodeOptionsFieldId}}", nodeOptionsFieldId.ToString());
				scriptTemplate = scriptTemplate.Replace("{{entityOptionsFieldId}}", entityOptionsFieldId.ToString());
				scriptEl += jsCompressor.Compress(scriptTemplate);
				//scriptEl += scriptTemplate;
				scriptEl += "</script>";

				ViewBag.EmbededJs = scriptEl;
				#endregion
			}
			#endregion

			ViewBag.RenderMode = mode;

			var applicationOptions = new List<SelectOption>();
			var areaOptions = new List<SelectOption>();
			var nodeOptions = new List<SelectOption>();
			var entityOptions = new List<SelectOption>();

			#region << Init Options >>
			//AppOptions

				applicationOptions = pageSelectionTree.AllApps;
				applicationOptions.Insert(0, new SelectOption() { Value = "", Label = "not selected" });
				areaOptions.Insert(0, new SelectOption() { Value = "", Label = "not selected" });
				nodeOptions.Insert(0, new SelectOption() { Value = "", Label = "not selected" });
				entityOptions = pageSelectionTree.AllEntities;
				entityOptions.Insert(0, new SelectOption() { Value = "", Label = "not selected" });

				//App is selected
				if(erpPage.AppId != null)
				{
					var treeAppOptions = pageSelectionTree.AppSelectionTree.First(x => x.AppId == erpPage.AppId);

					areaOptions = treeAppOptions.AllAreas;
					if (erpPage.AreaId == null)
					{
						areaOptions.Insert(0, new SelectOption() { Value = "", Label = "not selected" });
						nodeOptions.Insert(0, new SelectOption() { Value = "", Label = "not selected" });
					}
					else
					{
						var treeAreaOptions = treeAppOptions.AreaSelectionTree.FirstOrDefault(x => x.AreaId == erpPage.AreaId);
						if (treeAreaOptions != null)
						{
							nodeOptions = treeAreaOptions.AllNodes;
						}
					}

					if (treeAppOptions.Entities.Count > 0)
					{
						entityOptions = treeAppOptions.Entities;
					}
					else
					{
						entityOptions = pageSelectionTree.AllEntities;
					}

				}

			#endregion

			ViewBag.ApplicationOptions = applicationOptions;
			ViewBag.AreaOptions = areaOptions;
			ViewBag.NodeOptions = nodeOptions;
			ViewBag.EntityOptions = entityOptions;

			if (mode == WvFieldRenderMode.Form)
			{
				return await Task.FromResult<IViewComponentResult>(View("Form"));
			}
			else {
				return await Task.FromResult<IViewComponentResult>(View("Display"));
			}
		}
	}
}
