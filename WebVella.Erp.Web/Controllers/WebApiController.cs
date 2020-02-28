using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.AutoMapper;
using WebVella.Erp.Database;
using WebVella.Erp.Diagnostics;
using WebVella.Erp.Eql;
using WebVella.Erp.Jobs;
using WebVella.Erp.Utilities;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Service;
using WebVella.Erp.Web.Services;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Web.Controllers
{
	[Authorize]
	public class WebApiController : ApiControllerBase
	{
		private const char RELATION_SEPARATOR = '.';
		private const char RELATION_NAME_RESULT_SEPARATOR = '$';

		RecordManager recMan;
		EntityManager entMan;
		EntityRelationManager relMan;
		SecurityManager secMan;
		IErpService erpService;
		ErpRequestContext erpRequestContext;

		public WebApiController([FromServices] IErpService erpService, [FromServices] ErpRequestContext requestContext)
		{
			recMan = new RecordManager();
			secMan = new SecurityManager();
			entMan = new EntityManager();
			relMan = new EntityRelationManager();
			this.erpService = erpService;
			this.erpRequestContext = requestContext;
		}

		[Route("api/v3/en_US/eql")]
		[HttpPost]
		public ActionResult EqlQueryAction([FromBody]EqlQuery model)
		{
			ResponseModel response = new ResponseModel();
			response.Success = true;

			if (model == null)
				return NotFound();

			try
			{
				var eqlResult = new EqlCommand(model.Eql, model.Parameters).Execute();
				response.Object = eqlResult;
			}
			catch (EqlException eqlEx)
			{
				response.Success = false;
				foreach (var eqlError in eqlEx.Errors)
				{
					response.Errors.Add(new ErrorModel("eql", "", eqlError.Message));
				}
				return Json(response);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
				return Json(response);
			}

			return Json(response);
		}

		[Route("api/v3/en_US/eql-ds")]
		[HttpPost]
		public ActionResult DataSourceQueryAction([FromBody]JObject submitObj)
		{
			ResponseModel response = new ResponseModel();
			response.Success = true;


			if (submitObj == null)
				return NotFound();

			EqlDataSourceQuery model = new EqlDataSourceQuery();

			#region << Init SubmitObj >>
			foreach (var prop in submitObj.Properties())
			{
				switch (prop.Name.ToLower())
				{
					case "name":
						if (!string.IsNullOrWhiteSpace(prop.Value.ToString()))
							model.Name = prop.Value.ToString();
						else
						{
							throw new Exception("DataSource Name is required");
						}
						break;
					case "parameters":
						var jParams = (JArray)prop.Value;
						model.Parameters = new List<EqlParameter>();
						foreach (JObject jParam in jParams)
						{
							var name = jParam["name"].ToString();
							var value = jParam["value"].ToString();
							var eqlParam = new EqlParameter(name, value);
							model.Parameters.Add(eqlParam);
						}
						break;
				}
			}
			#endregion


			try
			{
				DataSourceManager dsMan = new DataSourceManager();
				var dataSources = dsMan.GetAll();
				var ds = dataSources.SingleOrDefault(x => x.Name == model.Name);
				if (ds == null)
				{
					response.Success = false;
					response.Message = $"DataSource with name '{model.Name}' not found.";
					return Json(response);
				}

				if (ds is DatabaseDataSource)
				{
					var list = (EntityRecordList)dsMan.Execute(ds.Id, model.Parameters);
					response.Object = new { list, total_count = list.TotalCount };
				}
				else if (ds is CodeDataSource)
				{
					Dictionary<string, object> arguments = new Dictionary<string, object>();
					foreach (var par in model.Parameters)
						arguments[par.ParameterName] = par.Value;

					response.Object = ((CodeDataSource)ds).Execute(arguments);
				}
				else
				{
					response.Success = false;
					response.Message = $"DataSource type is not supported.";
					return Json(response);
				}
			}
			catch (EqlException eqlEx)
			{
				response.Success = false;
				foreach (var eqlError in eqlEx.Errors)
				{
					response.Errors.Add(new ErrorModel("eql", "", eqlError.Message));
				}
				return Json(response);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
				return Json(response);
			}

			return Json(response);
		}

		[Route("api/v3/en_US/eql-ds-select2")]
		[HttpPost]
		public ActionResult DataSourceQueryActionForSelect2([FromBody]JObject submitObj)
		{
			if (submitObj == null)
				return NotFound();

			var result = new EntityRecord();
			result["results"] = new List<EntityRecord>();
			result["pagination"] = new EntityRecord();

			EqlDataSourceQuery model = new EqlDataSourceQuery();

			#region << Init SubmitObj >>
			foreach (var prop in submitObj.Properties())
			{
				switch (prop.Name.ToLower())
				{
					case "name":
						if (!string.IsNullOrWhiteSpace(prop.Value.ToString()))
							model.Name = prop.Value.ToString();
						else
						{
							throw new Exception("DataSource Name is required");
						}
						break;
					case "parameters":
						var jParams = (JArray)prop.Value;
						model.Parameters = new List<EqlParameter>();
						foreach (JObject jParam in jParams)
						{
							var name = jParam["name"].ToString();
							var value = jParam["value"].ToString();
							var eqlParam = new EqlParameter(name, value);
							model.Parameters.Add(eqlParam);
						}
						break;
				}
			}
			#endregion
			var page = 1;
			if (model.Parameters.Count > 0)
			{
				var pageParam = model.Parameters.FirstOrDefault(x => x.ParameterName == "page");
				if (pageParam != null)
				{
					if (int.TryParse(pageParam.Value?.ToString(), out int outInt))
					{
						page = outInt;
					}
				}
			}
			var records = new List<EntityRecord>();
			int? total = 0;
			try
			{
				DataSourceManager dsMan = new DataSourceManager();
				var dataSources = dsMan.GetAll();
				var ds = dataSources.SingleOrDefault(x => x.Name == model.Name);
				if (ds == null)
				{
					return BadRequest();
				}

				if (ds is DatabaseDataSource)
				{
					var list = (EntityRecordList)dsMan.Execute(ds.Id, model.Parameters);
					records = (List<EntityRecord>)list;
					total = list.TotalCount;
				}
				else if (ds is CodeDataSource)
				{
					Dictionary<string, object> arguments = new Dictionary<string, object>();
					foreach (var par in model.Parameters)
						arguments[par.ParameterName] = par.Value;

					var dsResult = ((CodeDataSource)ds).Execute(arguments);
					if (dsResult is EntityRecordList)
					{

						records = (List<EntityRecord>)((EntityRecordList)dsResult);
						total = ((EntityRecordList)dsResult).TotalCount;
					}
					else if (dsResult is List<EntityRecord>)
					{
						records = (List<EntityRecord>)dsResult;
						total = null;
					}
					else
					{
						return Json(dsResult);
					}
				}
				else
				{
					return BadRequest();
				}
			}
			catch
			{
				return BadRequest();
			}

			//Post process records according to requiredments {id,text}
			var processedRecords = new List<EntityRecord>();
			foreach (var record in records)
			{
				var procRec = new EntityRecord();
				if(record.Properties.ContainsKey("id")){
					procRec["id"] = record["id"].ToString();
				}
				else{
					procRec["id"] = "no-id-" + Guid.NewGuid();
				}
				if(record.Properties.ContainsKey("text")){
					procRec["text"] = record["text"].ToString();
				}
				else if(record.Properties.ContainsKey("label")){
					procRec["text"] = record["label"].ToString();
				}
				else if(record.Properties.ContainsKey("name")){
					procRec["text"] = record["name"].ToString();
				}
				else{
					procRec["text"] = procRec["id"].ToString();
				}
				processedRecords.Add(procRec);
			}
			var moreRecord = new EntityRecord();
			moreRecord["more"] = false;
			if (records.Count > 0)
			{
				if (total > page * 10)
				{
					moreRecord["more"] = true;
				}
				result["results"] = processedRecords;
			}

			result["pagination"] = moreRecord;
			return Json(result);
		}


		[Route("api/v3.0/user/preferences/toggle-sidebar-size")]
		[HttpPost]
		public ActionResult ToggleSidebarSize()
		{
			//TODO: Implement. Should Check the current size in user preferences and toggle in order "","sm","md","lg"
			var currentUser = AuthService.GetUser(User);
			var currentUserPreferences = currentUser.Preferences;
			var targetSidebarSize = "";
			switch (currentUserPreferences.SidebarSize)
			{
				case "sm":
					targetSidebarSize = "lg";
					break;
				case "lg":
					targetSidebarSize = "sm";
					break;
				default:
					targetSidebarSize = "lg";
					break;
			}
			var response = new BaseResponseModel();
			try
			{
				new UserPreferencies().SetSidebarSize(currentUser.Id, targetSidebarSize);
				response.Success = true;
				response.Message = "success";
				return Json(response);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
				new Log().Create(LogType.Error, "ToggleSidebarSize API Method Error", ex);
				return Json(response);
			}
		}

		[Route("api/v3.0/user/preferences/toggle-section-collapse")]
		[HttpPost]
		public ActionResult ToggleSection(Guid? nodeId = null, bool isCollapsed = false)
		{
			var response = new BaseResponseModel();
			try
			{
				if (nodeId == null)
					throw new Exception("nodeId query param is required");

				var userPreferencesService = new UserPreferencies();

				var currentUser = AuthService.GetUser(User);

				EntityRecord componentData = userPreferencesService.GetComponentData(currentUser.Id, "WebVella.Erp.Web.Components.PcSection");

				var collapsedNodeIds = new List<Guid>();
				var uncollapsedNodeIds = new List<Guid>();

				if (componentData == null)
				{
					componentData = new EntityRecord();
					componentData["collapsed_node_ids"] = new List<Guid>();
					componentData["uncollapsed_node_ids"] = new List<Guid>();
				}
				else
				{
					if (componentData.Properties.ContainsKey("collapsed_node_ids") && componentData["collapsed_node_ids"] != null)
					{
						if (componentData["collapsed_node_ids"] is string)
						{
							try
							{
								collapsedNodeIds = JsonConvert.DeserializeObject<List<Guid>>((string)componentData["collapsed_node_ids"]);
							}
							catch
							{
								throw new Exception("WebVella.Erp.Web.Components.PcSection component data object in user preferences not in the correct format. collapsed_node_ids should be List<Guid>");
							}
						}
						else if (componentData["collapsed_node_ids"] is List<Guid>)
						{
							collapsedNodeIds = (List<Guid>)componentData["collapsed_node_ids"];
						}
						else if (componentData["collapsed_node_ids"] is JArray)
						{
							collapsedNodeIds = ((JArray)componentData["collapsed_node_ids"]).ToObject<List<Guid>>();
						}
						else
						{
							throw new Exception("Unknown format of collapsed_node_ids");
						}
					}
					if (componentData.Properties.ContainsKey("uncollapsed_node_ids") && componentData["uncollapsed_node_ids"] != null)
					{
						if (componentData["uncollapsed_node_ids"] is string)
						{
							try
							{
								uncollapsedNodeIds = JsonConvert.DeserializeObject<List<Guid>>((string)componentData["uncollapsed_node_ids"]);
							}
							catch
							{
								throw new Exception("WebVella.Erp.Web.Components.PcSection component data object in user preferences not in the correct format. uncollapsed_node_ids should be List<Guid>");
							}
						}
						else if (componentData["uncollapsed_node_ids"] is List<Guid>)
						{
							uncollapsedNodeIds = (List<Guid>)componentData["uncollapsed_node_ids"];
						}
						else if (componentData["uncollapsed_node_ids"] is JArray)
						{
							uncollapsedNodeIds = ((JArray)componentData["uncollapsed_node_ids"]).ToObject<List<Guid>>();
						}
						else
						{
							throw new Exception("Unknown format of uncollapsed_node_ids");
						}
					}
				}

				if (isCollapsed)
				{
					//new state is collapsed
					//1. remove if it is in uncollapsed
					uncollapsedNodeIds = uncollapsedNodeIds.FindAll(x => x != nodeId.Value).ToList();
					//2. add to collapsed
					if (!collapsedNodeIds.Contains(nodeId.Value))
						collapsedNodeIds.Add(nodeId.Value);
				}
				else
				{
					//new state is uncollapsed
					//1. remove it is in collapsed
					collapsedNodeIds = collapsedNodeIds.FindAll(x => x != nodeId.Value).ToList();
					//2. add to uncollapsed
					if (!uncollapsedNodeIds.Contains(nodeId.Value))
						uncollapsedNodeIds.Add(nodeId.Value);
				}

				componentData["collapsed_node_ids"] = collapsedNodeIds;
				componentData["uncollapsed_node_ids"] = uncollapsedNodeIds;

				userPreferencesService.SetComponentData(currentUser.Id, "WebVella.Erp.Web.Components.PcSection", componentData);
				response.Success = true;
				response.Message = "success";
				return Json(response);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
				new Log().Create(LogType.Error, "ToggleSidebarSize API Method Error", ex);
				return Json(response);
			}
		}

		[Route("api/v3.0/datasource/code-compile")]
		[HttpPost]
		public ActionResult DataSourceAction([FromBody] DataSourceCodeTestModel model)
		{
			try
			{
				CodeEvalService.Compile(model.CsCode);
			}
			catch (Exception ex)
			{
				new Log().Create(LogType.Error, "DataSourceAction Code compile API Method Error", ex);
				return Json(new { success = false, message = ex.Message });
			}

			return Json(new { success = true, message = "" });
		}

		[Route("api/v3.0/datasource/test")]
		[HttpPost]
		public ActionResult DataSourceAction([FromBody]DataSourceTestModel model)
		{
			if (model == null)
				return NotFound();

			string sql = string.Empty;
			string data = "";
			List<EqlError> errors = new List<EqlError>();
			try
			{
				DataSourceManager dataSourceManager = new DataSourceManager();
				if (model.Action == "sql")
					sql = dataSourceManager.GenerateSql(model.Eql, model.Parameters);
				if (model.Action == "data")
					data = JsonConvert.SerializeObject(dataSourceManager.Execute(model.Eql, model.Parameters), Formatting.Indented);
			}
			catch (EqlException eqlEx)
			{
				errors.AddRange(eqlEx.Errors);
			}
			catch (Exception ex)
			{
				new Log().Create(LogType.Error, "DataSourceAction test API Method Error", ex);
				errors.Add(new EqlError { Message = ex.Message });
			}

			return Json(new { sql, data, errors });
		}

		[Route("api/v3.0/datasource/{dataSourceId}/test")]
		[HttpPost]
		public ActionResult DataSourceAction(Guid dataSourceId, [FromBody]DataSourceTestModel model)
		{

			if (model == null)
				return NotFound();

			string sql = string.Empty;
			string data = "";
			List<EqlError> errors = new List<EqlError>();
			try
			{
				DataSourceManager dataSourceManager = new DataSourceManager();
				var dataSource = dataSourceManager.Get(dataSourceId);
				if (dataSource == null)
				{
					errors.Add(new EqlError { Message = "DataSource Not found" });
				}

				var dataSourceEql = "";
				if (dataSource is DatabaseDataSource)
				{
					dataSourceEql = ((DatabaseDataSource)dataSource).EqlText;
				}

				var compoundParams = new List<DataSourceParameter>();
				foreach (var dsParam in dataSource.Parameters)
				{
					var pageParameter = model.ParamList.FirstOrDefault(x => x.Name == dsParam.Name);
					if (pageParameter != null)
					{
						compoundParams.Add(pageParameter);
					}
					else
					{
						compoundParams.Add(dsParam);
					}
				}

				var paramText = dataSourceManager.ConvertParamsToText(compoundParams);

				if (model.Action == "sql")
					sql = dataSourceManager.GenerateSql(dataSourceEql, paramText);
				if (model.Action == "data")
					data = JsonConvert.SerializeObject(dataSourceManager.Execute(dataSourceEql, paramText), Formatting.Indented);
			}
			catch (EqlException eqlEx)
			{
				errors.AddRange(eqlEx.Errors);
			}
			catch (Exception ex)
			{
				new Log().Create(LogType.Error, "DataSourceAction Id test API Method Error", ex);
				errors.Add(new EqlError { Message = ex.Message });
			}

			return Json(new { sql, data, errors });
		}

		//[AllowAnonymous] //Needed only when webcomponent development
		[Route("api/v3.0/page/{pageId}/node/create")]
		[HttpPost]
		public ActionResult CreatePageBodyNode(Guid pageId, [FromBody]PageBodyNode newNode)
		{
			try
			{
				var pageSrv = new PageService();

				ErpPage page = pageSrv.GetPage(pageId);
				if (page == null) //page not found
					return NotFound();

				if (newNode == null)
					return NotFound();

				if (newNode.Id == Guid.Empty)
					newNode.Id = Guid.NewGuid();

				if (page.Body == null && newNode.ParentId != null)
					throw new Exception("Cannot create child node in page with no root node.");

				//if (page.Body != null && newNode.ParentId == null)
				//	throw new Exception("Cannot create root node in page with already existing root node.");

				pageSrv.CreatePageBodyNode(newNode.Id, newNode.ParentId, pageId, newNode.NodeId, newNode.Weight,
					newNode.ComponentName, newNode.ContainerId, newNode.Options);

				var createdNode = pageSrv.GetPageNodeById(newNode.Id);

				var currentUser = AuthService.GetUser(User);
				new UserPreferencies().SdkUseComponent(currentUser.Id, newNode.ComponentName);

				return Json(createdNode);
			}
			catch (Exception exception)
			{
				new Log().Create(LogType.Error, "CreatePageBodyNode API Method Error", exception);
				return new ContentResult
				{
					Content = $"Error: {exception.Message}",
					ContentType = "text/plain",
					// change to whatever status code you want to send out
					StatusCode = 500
				};
			}
		}

		//[AllowAnonymous] //Needed only when webcomponent development
		[Route("api/v3.0/page/{pageId}/node/{nodeId}/update")]
		[HttpPost]
		public ActionResult UpdatePageBodyNode(Guid pageId, Guid nodeId, [FromBody]PageBodyNode node)
		{
			try
			{
				var pageSrv = new PageService();

				ErpPage page = pageSrv.GetPage(pageId);
				if (page == null) //page not found
					return NotFound();

				var pageNodes = pageSrv.GetPageNodes(pageId);
				var existingNode = pageNodes.SingleOrDefault(x => x.Id == nodeId);
				if (existingNode == null)
					return NotFound();

				if (existingNode.ParentId != null && node.ParentId == null)
					throw new Exception("There is only one root node and cannot update parent to null. Check for error.");

				if (nodeId == node.ParentId)
				{
					throw new Exception("Node Id and Parent Id cannot be the same");
				}

				pageSrv.UpdatePageBodyNode(nodeId, node.ParentId, pageId, node.NodeId, node.Weight,
					node.ComponentName, node.ContainerId, node.Options);

				pageNodes = pageSrv.GetPageNodes(pageId);
				return Json(pageNodes);
			}
			catch (Exception exception)
			{
				new Log().Create(LogType.Error, "UpdatePageBodyNode API Method Error", exception);
				return new ContentResult
				{
					Content = $"Error: {exception.Message}",
					ContentType = "text/plain",
					// change to whatever status code you want to send out
					StatusCode = 500
				};
			}
		}

		//[AllowAnonymous] //Needed only when webcomponent development
		[Route("api/v3.0/page/{pageId}/node/{nodeId}/move")]
		[HttpPost]
		public ActionResult MovePageBodyNode(Guid pageId, Guid nodeId, [FromBody]MovedNodeInfo moveInfo)
		{
			try
			{
				var pageSrv = new PageService();

				ErpPage page = pageSrv.GetPage(pageId);
				if (page == null) //page not found
					return NotFound();

				if (moveInfo == null)
				{
					return BadRequest("MoveInfo cannot be restored");
				}


				var pageNodes = pageSrv.GetPageNodes(pageId);

				var movedNode = pageNodes.First(x => x.Id == nodeId);
				movedNode.ParentId = moveInfo.NewParentNodeId;
				movedNode.ContainerId = moveInfo.NewContainerId;
				movedNode.Weight = moveInfo.NewIndex + 1; //Convert index to weight
				var nodesToBeUpdated = new List<Guid>();
				pageNodes = Utils.PageUtils.RecalculateContainerNodeWeights(pageNodes, out nodesToBeUpdated, nodeId);

				//Update Nodes
				foreach (var updatedNodeId in nodesToBeUpdated)
				{
					var updatedNode = pageNodes.First(x => x.Id == updatedNodeId);

					if (updatedNodeId == updatedNode.ParentId)
					{
						throw new Exception("Node Id and Parent Id cannot be the same");
					}

					pageSrv.UpdatePageBodyNode(updatedNodeId, updatedNode.ParentId, pageId, updatedNode.NodeId,
						updatedNode.Weight, updatedNode.ComponentName, updatedNode.ContainerId, updatedNode.Options);
				}

				pageNodes = pageSrv.GetPageNodes(pageId);
				return Json(pageNodes);
			}
			catch (Exception exception)
			{
				new Log().Create(LogType.Error, "MovePageBodyNode API Method Error", exception);
				return new ContentResult
				{
					Content = $"Error: {exception.Message}",
					ContentType = "text/plain",
					// change to whatever status code you want to send out
					StatusCode = 500
				};
			}
		}

		//[AllowAnonymous] //Needed only when webcomponent development
		[Route("api/v3.0/page/{pageId}/node/{nodeId}/delete")]
		[HttpPost]
		public ActionResult DeletePageBodyNode(Guid pageId, Guid nodeId)
		{
			try
			{
				var pageSrv = new PageService();
				ErpPage page = pageSrv.GetPage(pageId);
				if (page == null) //page not found
					return NotFound();

				var pageNodes = pageSrv.GetPageNodes(pageId);
				if (!pageNodes.Any(x => x.Id == nodeId))
					return NotFound();

				pageSrv.DeletePageBodyNode(nodeId);

				pageNodes = pageSrv.GetPageNodes(pageId);
				return Json(pageNodes);
			}
			catch (Exception exception)
			{
				new Log().Create(LogType.Error, "DeletePageBodyNode API Method Error", exception);
				return new ContentResult
				{
					Content = $"Error: {exception.Message}",
					ContentType = "text/plain",
					// change to whatever status code you want to send out
					StatusCode = 500
				};
			}
		}

		//[AllowAnonymous] //Needed only when webcomponent development
		[Route("api/v3.0/page/{pageId}/node/{nodeId}/options/update")]
		[HttpPost]
		public ActionResult UpdatePageBodyNodeOptions(Guid pageId, Guid nodeId, [FromBody]JObject options)
		{
			try
			{
				if (options == null)
					return NotFound();

				var pageSrv = new PageService();

				ErpPage page = pageSrv.GetPage(pageId);
				if (page == null) //page not found
					return NotFound();

				pageSrv.UpdatePageBodyNodeOptions(nodeId, options.ToString());

				var updatedNode = pageSrv.GetPageNodeById(nodeId);
				var pageNodes = pageSrv.GetPageNodes(updatedNode.PageId);
				return Json(pageNodes);
			}
			catch (Exception exception)
			{
				new Log().Create(LogType.Error, "UpdatePageBodyNodeOptions API Method Error", exception);
				return new ContentResult
				{
					Content = $"Error: {exception.Message}",
					ContentType = "text/plain",
					// change to whatever status code you want to send out
					StatusCode = 500
				};
			}
		}

		//[AllowAnonymous] //Needed only when webcomponent development
		[Route("api/v3.0/pc/{fullComponentName}/view/{renderMode}")]
		[HttpPost]
		public ActionResult PageComponentRenderViews(string fullComponentName, string renderMode, [FromBody]JObject options,
			[FromQuery] Guid? nid = null, [FromQuery] Guid? pid = null, [FromQuery] Guid? entityId = null, [FromQuery] Guid? recordId = null)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(renderMode))
					return NotFound();

				//if (nid == null)
				//	return BadRequest("The node Id is required to be set as query parameter 'nid', when requesting this component");

				if (pid == null)
					return BadRequest("The page Id is required to be set as query parameter 'pid', when requesting this component");

				var type = FileService.GetType(fullComponentName);
				if (type == null)
					return NotFound();

				var pageServ = new PageService();
				PageBodyNode pagebodyNode = null;
				ErpPage page = null;
				PageDataModel pageModel = null;

				#region << Override erpRequestContext >>
				erpRequestContext.SetSimulatedRouteData(entityId: entityId, pageId: pid, recordId: recordId);
				#endregion

				if (pid != null)
				{
					page = pageServ.GetPage(pid ?? Guid.Empty);

					if (nid != null)
					{
						pagebodyNode = pageServ.GetPageNodeById(nid ?? Guid.Empty);
					}
					else
					{
						pagebodyNode = PageUtils.GetAjaxPageBodyNode(fullComponentName, pid ?? Guid.Empty, JsonConvert.SerializeObject(options));
					}

					#region << Building simulation pageModel >>
					App app = null;
					SitemapArea area = null;
					SitemapNode node = null;
					Entity entity = null;
					EntityRecord record = null;
					//erpRequestContext
					if (page != null)
					{
						//Override 
						if (entityId != null)
							page.EntityId = entityId;

						if (page.AppId == null && page.EntityId != null)
						{
							#region << Try to get one of the attached apps >>
							var allApps = new AppService().GetAllApplications();
							foreach (var appInstance in allApps)
							{
								foreach (var areaInstance in appInstance.Sitemap.Areas)
								{
									foreach (var nodeInstance in areaInstance.Nodes)
									{
										if (nodeInstance.EntityId == page.EntityId)
										{
											page.AppId = appInstance.Id;
											if (page.Type == PageType.RecordCreate || page.Type == PageType.RecordDetails ||
											page.Type == PageType.RecordList || page.Type == PageType.RecordManage)
											{
												page.AreaId = areaInstance.Id;
												page.NodeId = nodeInstance.Id;
											}
										}
									}
								}
							}

							#endregion
						}

						if (page.AppId != null)
						{
							app = new AppService().GetApplication(page.AppId ?? Guid.Empty);
							erpRequestContext.App = app;
							if (app != null)
							{
								if (page.AreaId != null)
								{
									area = app.Sitemap.Areas.FirstOrDefault(x => x.Id == page.AreaId);
									erpRequestContext.SitemapArea = area;
									if (area != null && page.NodeId != null)
									{
										node = area.Nodes.FirstOrDefault(x => x.Id == page.NodeId);
										erpRequestContext.SitemapNode = node;
									}
								}

								if (page.EntityId != null)
								{
									entity = new EntityManager().ReadEntity(page.EntityId ?? Guid.Empty).Object;
									erpRequestContext.Entity = entity;

									//Get the first record as simulation
									if (entity != null)
									{
										QueryObject filter = null;
										if (recordId != null)
										{
											filter = EntityQuery.QueryEQ("id", recordId.Value);
										}
										var sortsList = new List<QuerySortObject>();
										sortsList.Add(new QuerySortObject("id", QuerySortType.Ascending));
										var findRecordResponse = new RecordManager().Find(new EntityQuery(entity.Name, "*", filter, sortsList.ToArray(), 0, 1));
										if (!findRecordResponse.Success)
											throw new Exception(findRecordResponse.Message);
										if (findRecordResponse.Object != null && findRecordResponse.Object.Data.Any())
										{
											record = findRecordResponse.Object.Data.First();
											erpRequestContext.RecordId = (Guid)record["id"];
										}
									}
								}
							}
						}
					}

					//currentUser
					var currentUser = AuthService.GetUser(User);


					var baseErpPageMode = BaseErpPageModel.CreatePageModelSimulation(
						erpRequestContext: erpRequestContext,
						currentUser: currentUser
					);

					pageModel = baseErpPageMode.DataModel;
					#endregion
				}

				switch (renderMode)
				{
					case "display":
						var pcContextDisplay = new PageComponentContext(pagebodyNode, pageModel, ComponentMode.Design, options);
						return ViewComponent(type, new { context = pcContextDisplay });
					case "design":
						var pcContextDesign = new PageComponentContext(pagebodyNode, pageModel, ComponentMode.Design, options);
						return ViewComponent(type, new { context = pcContextDesign });
					case "options":
						pageModel.SafeCodeDataVariable = true;
						var pcContextOptions = new PageComponentContext(pagebodyNode, pageModel, ComponentMode.Options, options);
						return ViewComponent(type, new { context = pcContextOptions });
					case "help":
						var pcContextReadme = new PageComponentContext(pagebodyNode, pageModel, ComponentMode.Help, options);
						return ViewComponent(type, new { context = pcContextReadme });
				}

				return NotFound();
			}
			catch (Exception exception)
			{
				new Log().Create(LogType.Error, "PageComponentRenderViews API Method Error", exception);
				return new ContentResult
				{
					Content = $"Error: {exception.Message}",
					ContentType = "text/plain",
					// change to whatever status code you want to send out
					StatusCode = 500
				};
			}
		}

		//[AllowAnonymous] //Needed only when webcomponent development
		[Route("api/v3.0/pc/{fullComponentName}/resource/{filename}")]
		[HttpGet]
		public ActionResult PageComponentServiceJs(string fullComponentName, string filename)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(fullComponentName))
					return NotFound();

				var assembly = FileService.GetTypeAssembly(fullComponentName);
				if (assembly == null)
					return NotFound();

				if (!FileService.EmbeddedResourceExists(filename, fullComponentName, assembly))
					return NotFound();

				var content = FileService.GetEmbeddedTextResource(filename, fullComponentName, assembly);
				switch (filename)
				{
					case "service.js":
						return Content(content, "text/javascript");
					case "options.html":
					case "design.html":
						return Content(content, "text/html");
				}

				return NotFound();
			}
			catch (Exception exception)
			{
				new Log().Create(LogType.Error, "PageComponentServiceJs API Method Error", exception);
				return new ContentResult
				{
					Content = $"Error: {exception.Message}",
					ContentType = "text/plain",
					// change to whatever status code you want to send out
					StatusCode = 500
				};
			}
		}

		[AllowAnonymous]
		[Route("api/v3.0/p/core/styles.css")]
		[ResponseCache(NoStore = false, Duration = 30 * 24 * 3600)]
		[HttpGet]
		public ContentResult StylesCss()
		{
			try
			{
				var cssContent = "";

				if (String.IsNullOrWhiteSpace(ErpAppContext.Current.StylesContent))
				{
					new ThemeService().GenerateStylesContent();
				}

				cssContent = ErpAppContext.Current.StylesContent;
				return Content(cssContent, "text/css");
			}
			catch (Exception ex)
			{
				new Log().Create(LogType.Error, "StylesCss API Method Error", ex);
				throw ex;
			}
		}


		//[Route("api/v3.0/p/core/select/font-awesome-icons")]
		//[HttpGet]
		//public ActionResult GetSelectCases([FromQuery]string search,[FromQuery]int page = 1)
		//{
		//	var pageSize = 10;
		//	var response = new ResponseModel();
		//	response.Timestamp = DateTime.UtcNow;
		//	try
		//	{
		//		var icons = RenderService.FontAwesomeIcons;
		//		var iconTotal = icons.Count();
		//		if(!String.IsNullOrWhiteSpace(search)){
		//			var filteredIcons = icons.FindAll(x=> x.Class.Contains(search) || x.Name.Contains(search)).ToList();
		//			iconTotal = filteredIcons.Count();
		//			icons = filteredIcons.Skip((page-1)*pageSize).Take(pageSize).ToList();
		//		}
		//		else{
		//			icons = icons.Skip((page-1)*pageSize).Take(pageSize).ToList();
		//		}
		//		var result = new EntityRecord();

		//		result["results"] = icons;
		//		result["pagination"] = new EntityRecord(); // more => true, false
		//		var moreRecord = new EntityRecord();
		//		moreRecord["more"] = false;

		//		if(iconTotal > page*pageSize){
		//			moreRecord["more"] = true;
		//		}

		//		result["pagination"] = moreRecord;


		//		response.Object = result;
		//		response.Success = true;
		//		response.Message = "";
		//	}
		//	catch (Exception ex)
		//	{
		//		response.Success = false;
		//		response.Message = ex.Message;
		//	}
		//	return Json(response);
		//}

		//[AllowAnonymous]
		//[Route("api/v3.0/p/core/framework.css")]
		//[ResponseCache(NoStore = false, Duration = 30 * 24 * 3600)]
		//[HttpGet]
		//public ContentResult FrameworkCss()
		//{
		//	try
		//	{
		//		var cssContent = "";

		//		if (String.IsNullOrWhiteSpace(ErpAppContext.Current.StyleFrameworkContent))
		//		{
		//			new ThemeService().GenerateStyleFrameworkContent();
		//		}

		//		cssContent = ErpAppContext.Current.StyleFrameworkContent;
		//		return Content(cssContent, "text/css");
		//	}
		//	catch (Exception ex)
		//	{
		//		new Log().Create(LogType.Error, "FrameworkCss API Method Error", ex);
		//		throw ex;
		//	}
		//}


		#region << UI component support >>

		[Produces("application/json")]
		[Route("api/v3.0/p/core/related-field-multiselect")]
		[AcceptVerbs("GET", "POST")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult RelatedFieldMultiSelect(string entityName, string fieldName, string search = "", int page = 1)
		{
			try
			{
				var response = new TypeaheadResponse();
				var errorResponse = new ResponseModel();
				var recMan = new RecordManager();
				if (String.IsNullOrWhiteSpace(entityName))
				{
					errorResponse.Message = "entity name is required";
					Response.StatusCode = (int)HttpStatusCode.BadRequest;
					return Json(errorResponse);
				}
				if (String.IsNullOrWhiteSpace(fieldName))
				{
					errorResponse.Message = "field name is required";
					Response.StatusCode = (int)HttpStatusCode.BadRequest;
					return Json(errorResponse);
				}

				var pageSize = 5 + 1; //the extra record will tell us if there are more records
				var skipPages = (page - 1) * pageSize;
				var sortList = new List<QuerySortObject>();
				sortList.Add(new QuerySortObject(fieldName, QuerySortType.Ascending));

				var query = new EntityQuery(entityName, fieldName, null, sortList.ToArray(), skipPages, pageSize);
				if (!String.IsNullOrWhiteSpace(search))
				{
					query = new EntityQuery(entityName, fieldName, EntityQuery.QueryContains(fieldName, search), sortList.ToArray(), skipPages, pageSize);
				}

				var findResult = recMan.Find(query);
				var resultRecords = new List<EntityRecord>();
				if (!findResult.Success)
				{
					errorResponse.Message = findResult.Message;
					Response.StatusCode = (int)HttpStatusCode.BadRequest;
					return Json(errorResponse);
				}

				if (findResult.Object.Data.Count > 0)
				{
					if (findResult.Object.Data.Count == 6)
					{
						response.Pagination.More = true;
						resultRecords = findResult.Object.Data.Take(5).ToList();
					}
					else
					{
						resultRecords = findResult.Object.Data;
					}

					var entity = new EntityManager().ReadEntity(entityName).Object;
					foreach (var record in resultRecords)
					{
						response.Results.Add(new TypeaheadResponseRow
						{
							Id = record[fieldName].ToString(),
							Text = record[fieldName].ToString(),
							FieldName = fieldName,
							EntityName = entity.Label,
							Color = entity.Color,
							IconName = entity.IconName
						});
					}
				}
				return new JsonResult(response);
			}
			catch (Exception ex)
			{
				new Log().Create(LogType.Error, "RelatedFieldMultiSelect API Method Error", ex);
				throw ex;
			}
		}

		[Produces("application/json")]
		[Route("api/v3.0/p/core/select-field-add-option")]
		[AcceptVerbs("PUT")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult SelectFieldAddOption([FromBody]JObject submitObj)
		{
			var response = new ResponseModel();
			var recMan = new RecordManager();
			var entMan = new EntityManager();
			var entityName = "";
			var fieldName = "";
			var optionValue = "";
			try
			{
				#region << Init SubmitObj >>
				foreach (var prop in submitObj.Properties())
				{
					switch (prop.Name.ToLower())
					{
						case "entityname":
							if (!string.IsNullOrWhiteSpace(prop.Value.ToString()))
								entityName = prop.Value.ToString();
							else
							{
								throw new Exception("EntityName is required");
							}
							break;
						case "fieldname":
							if (!string.IsNullOrWhiteSpace(prop.Value.ToString()))
								fieldName = prop.Value.ToString();
							else
							{
								throw new Exception("Field name is required");
							}
							break;
						case "value":
							if (!string.IsNullOrWhiteSpace(prop.Value.ToString()))
								optionValue = prop.Value.ToString();
							else
							{
								throw new Exception("Option value is required");
							}
							break;
					}
				}
				#endregion
				var entityMeta = entMan.ReadEntity(entityName).Object;
				if (entityMeta == null)
				{
					throw new Exception("Entity not found by the provided entityName: " + entityName);
				}
				var fieldMeta = entityMeta.Fields.FirstOrDefault(x => x.Name == fieldName);
				if (fieldMeta == null)
				{
					throw new Exception("Field not found by the provided fieldName: " + fieldMeta + " in entity " + entityName);
				}
				var optionExists = false;
				if (fieldMeta.GetFieldType() == FieldType.SelectField)
				{
					var fieldOptions = ((SelectField)fieldMeta).Options.FirstOrDefault(x => x.Value.ToLowerInvariant() == optionValue.ToLowerInvariant());
					if (fieldOptions != null)
					{
						optionExists = true;
					}
				}
				else if (fieldMeta.GetFieldType() == FieldType.MultiSelectField)
				{
					var fieldOptions = ((MultiSelectField)fieldMeta).Options.FirstOrDefault(x => x.Value.ToLowerInvariant() == optionValue.ToLowerInvariant());
					if (fieldOptions != null)
					{
						optionExists = true;
					}
				}

				if (optionExists)
				{
					throw new Exception("Record not found!");
				}

				if (fieldMeta.GetFieldType() == FieldType.SelectField)
				{
					var newOption = new SelectOption
					{
						Value = optionValue,
						Label = optionValue
					};
					var newFieldMeta = (SelectField)fieldMeta;
					newFieldMeta.Options.Add(newOption);
					var updateResponse = entMan.UpdateField(entityMeta, newFieldMeta.MapTo<InputField>());
					if (!updateResponse.Success)
					{
						throw new Exception(updateResponse.Message);
					}
				}
				else if (fieldMeta.GetFieldType() == FieldType.MultiSelectField)
				{
					var newOption = new SelectOption
					{
						Value = optionValue,
						Label = optionValue
					};
					var newFieldMeta = (MultiSelectField)fieldMeta;
					newFieldMeta.Options.Add(newOption);
					var updateResponse = entMan.UpdateField(entityMeta, newFieldMeta.MapTo<InputField>());
					if (!updateResponse.Success)
					{
						throw new Exception(updateResponse.Message);
					}
				}

				response.Success = true;
				response.Message = "Record created successfully";
			}
			catch (Exception ex)
			{
				new Log().Create(LogType.Error, "RelatedFieldMultiSelect API Method Error", ex);
				response.Success = false;
				response.Message = ex.Message;
			}
			return new JsonResult(response);
		}

		[Produces("text/html")]
		[Route("api/v3.0/{lang}/p/core/ui/field-table-data/generate/preview")]
		[AcceptVerbs("POST")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult FieldTableDataPreview([FromRoute] string lang, [FromBody]JObject submitObj)
		{
			var hasHeader = true;
			var hasHeaderColumn = false;
			string csvData = "";
			string delimiterName = "";
			#region << Init SubmitObj >>
			foreach (var prop in submitObj.Properties())
			{
				switch (prop.Name.ToLower())
				{
					case "hasheader":
						if (!string.IsNullOrWhiteSpace(prop.Value.ToString()))
						{
							var hasHeaderString = prop.Value.ToString();
							if (hasHeaderString.ToLowerInvariant() == "false")
							{
								hasHeader = false;
							}
						}
						break;
					case "hasheadercolumn":
						if (!string.IsNullOrWhiteSpace(prop.Value.ToString()))
						{
							var hasHeaderColumnString = prop.Value.ToString();
							if (hasHeaderColumnString.ToLowerInvariant() == "true")
							{
								hasHeaderColumn = true;
							}
						}
						break;
					case "csv":
						if (!string.IsNullOrWhiteSpace(prop.Value.ToString()))
						{
							csvData = prop.Value.ToString();
						}
						break;
					case "delimiter":
						if (!string.IsNullOrWhiteSpace(prop.Value.ToString()))
						{
							delimiterName = prop.Value.ToString(); //Does not work if first checked for empty string
						}
						break;
				}
			}

			var records = new List<dynamic>();
			try
			{
				records = WebVella.TagHelpers.Utilities.WvHelpers.GetCsvData(csvData, hasHeader, delimiterName);
			}
			//catch (CsvHelperException ex)
			//{
			//	//ex.Data.Values has more info...

			//	if (lang == "bg")
			//	{
			//		return Content("<div class='alert alert-danger p-2'>Грешен формат на данните. Опитайте с друг разделител.</div>");
			//	}
			//	else
			//	{
			//		return Content("<div class='alert alert-danger p-2'>Error in parsing data. Check another delimiter</div>");
			//	}
			//}
			catch
			{
				if (lang == "bg")
				{
					return Content("<div class='alert alert-danger p-2'>Грешен формат на данните. Опитайте с друг разделител.</div>");
				}
				else
				{
					return Content("<div class='alert alert-danger p-2'>Error in parsing data. Check another delimiter</div>");
				}
			}

			#endregion

			var result = new EntityRecord();
			result["hasHeader"] = hasHeader;
			result["hasHeaderColumn"] = hasHeaderColumn;
			result["data"] = records;
			result["lang"] = lang;
			return PartialView("FieldTableDataPreview", result);
		}



		#endregion

		#region << Entity Meta >>

		// Get all entity definitions
		// GET: api/v3/en_US/meta/entity/list/
		[Authorize(Roles = "administrator")]
		[AcceptVerbs(new[] { "GET" }, Route = "api/v3/en_US/meta/entity/list")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult GetEntityMetaList(string hash = null)
		{
			var bo = entMan.ReadEntities();

			//check hash and clear data if hash match
			if (bo.Success && bo.Object != null && !string.IsNullOrWhiteSpace(hash) && bo.Hash == hash)
				bo.Object = null;

			return DoResponse(bo);
		}

		// Get entity meta
		// GET: api/v3/en_US/meta/entity/id/{entityId}/
		[Authorize(Roles = "administrator")]
		[AcceptVerbs(new[] { "GET" }, Route = "api/v3/en_US/meta/entity/id/{entityId}")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult GetEntityMetaById(Guid entityId)
		{
			return DoResponse(entMan.ReadEntity(entityId));
		}

		// Get entity meta
		// GET: api/v3/en_US/meta/entity/{name}/
		[Authorize(Roles = "administrator")]
		[AcceptVerbs(new[] { "GET" }, Route = "api/v3/en_US/meta/entity/{Name}")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult GetEntityMeta(string Name)
		{
			return DoResponse(entMan.ReadEntity(Name));
		}


		// Create an entity
		// POST: api/v3/en_US/meta/entity
		[AcceptVerbs(new[] { "POST" }, Route = "api/v3/en_US/meta/entity")]
		[ResponseCache(NoStore = true, Duration = 0)]
		[Authorize(Roles = "administrator")]
		public IActionResult CreateEntity([FromBody]InputEntity submitObj)
		{
			var entity = new InputEntity
			{
				Name = submitObj.Name,
				Label = submitObj.Label,
				LabelPlural = submitObj.LabelPlural,
				System = submitObj.System,
				IconName = submitObj.IconName,
				//Weight = submitObj.Weight,
				RecordPermissions = submitObj.RecordPermissions
			};

			return DoResponse(entMan.CreateEntity(entity));
		}

		// Create an entity
		// POST: api/v3/en_US/meta/entity
		[AcceptVerbs(new[] { "PATCH" }, Route = "api/v3/en_US/meta/entity/{StringId}")]
		[ResponseCache(NoStore = true, Duration = 0)]
		[Authorize(Roles = "administrator")]
		public IActionResult PatchEntity(string StringId, [FromBody]JObject submitObj)
		{
			FieldResponse response = new FieldResponse();
			InputEntity entity = new InputEntity();

			try
			{
				if (!Guid.TryParse(StringId, out Guid entityId))
				{
					response.Errors.Add(new ErrorModel("id", StringId, "id parameter is not valid Guid value"));
					return DoResponse(response);
				}

				DbEntity storageEntity = DbContext.Current.EntityRepository.Read(entityId);
				if (storageEntity == null)
				{
					response.Timestamp = DateTime.UtcNow;
					response.Success = false;
					response.Message = "Entity with such Name does not exist!";
					return DoBadRequestResponse(response);
				}
				entity = storageEntity.MapTo<Entity>().MapTo<InputEntity>();

				Type inputEntityType = entity.GetType();

				foreach (var prop in submitObj.Properties())
				{
					int count = inputEntityType.GetProperties().Where(n => n.Name.ToLower() == prop.Name.ToLower()).Count();
					if (count < 1)
						response.Errors.Add(new ErrorModel(prop.Name, prop.Value.ToString(), "Input object contains property that is not part of the object model."));
				}

				if (response.Errors.Count > 0)
					return DoBadRequestResponse(response);

				InputEntity inputEntity = submitObj.ToObject<InputEntity>();

				foreach (var prop in submitObj.Properties())
				{
					if (prop.Name.ToLower() == "label")
						entity.Label = inputEntity.Label;
					if (prop.Name.ToLower() == "labelplural")
						entity.LabelPlural = inputEntity.LabelPlural;
					if (prop.Name.ToLower() == "system")
						entity.System = inputEntity.System;
					if (prop.Name.ToLower() == "iconname")
						entity.IconName = inputEntity.IconName;
					if (prop.Name.ToLower() == "color")
						entity.Color = inputEntity.Color;
					//if (prop.Name.ToLower() == "weight")
					//	entity.Weight = inputEntity.Weight;
					if (prop.Name.ToLower() == "recordpermissions")
						entity.RecordPermissions = inputEntity.RecordPermissions;
					if (prop.Name.ToLower() == "recordscreenidfield")
						entity.RecordScreenIdField = inputEntity.RecordScreenIdField;
				}
			}
			catch (Exception e)
			{
				new LogService().Create(Diagnostics.LogType.Error, "TErpApi:PatchEntity", e);
				return DoBadRequestResponse(response, "Input object is not in valid format! It cannot be converted.", e);
			}

			return DoResponse(entMan.UpdateEntity(entity));
		}


		// Delete an entity
		// DELETE: api/v3/en_US/meta/entity/{id}
		[Authorize(Roles = "administrator")]
		[AcceptVerbs(new[] { "DELETE" }, Route = "api/v3/en_US/meta/entity/{StringId}")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult DeleteEntity(string StringId)
		{
			EntityResponse response = new EntityResponse();

			// Parse each string representation.
			Guid id = Guid.Empty;
			if (Guid.TryParse(StringId, out Guid newGuid))
			{
				response = entMan.DeleteEntity(newGuid);
			}
			else
			{
				response.Success = false;
				response.Message = "The entity Id should be a valid Guid";
				HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
			}
			return DoResponse(response);
		}

		#endregion

		#region << Entity Fields >>

		[Authorize(Roles = "administrator")]
		[AcceptVerbs(new[] { "POST" }, Route = "api/v3/en_US/meta/entity/{Id}/field")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult CreateField(string Id, [FromBody]JObject submitObj)
		{
			FieldResponse response = new FieldResponse();

			if (!Guid.TryParse(Id, out Guid entityId))
			{
				response.Errors.Add(new ErrorModel("id", Id, "id parameter is not valid Guid value"));
				return DoResponse(response);
			}

			InputField field = new InputGuidField();
			try
			{
				field = InputField.ConvertField(submitObj);
			}
			catch (Exception e)
			{
				new LogService().Create(Diagnostics.LogType.Error, "TErpApi:CreateField", e);
				return DoBadRequestResponse(response, "Input object is not in valid format! It cannot be converted.", e);
			}

			return DoResponse(entMan.CreateField(entityId, field));
		}

		[Authorize(Roles = "administrator")]
		[AcceptVerbs(new[] { "PUT" }, Route = "api/v3/en_US/meta/entity/{Id}/field/{FieldId}")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult UpdateField(string Id, string FieldId, [FromBody]JObject submitObj)
		{
			FieldResponse response = new FieldResponse();

			if (!Guid.TryParse(Id, out Guid entityId))
			{
				response.Errors.Add(new ErrorModel("id", Id, "id parameter is not valid Guid value"));
				return DoResponse(response);
			}

			if (!Guid.TryParse(FieldId, out Guid fieldId))
			{
				response.Errors.Add(new ErrorModel("id", FieldId, "FieldId parameter is not valid Guid value"));
				return DoResponse(response);
			}

			InputField field = new InputGuidField();
			FieldType fieldType = FieldType.GuidField;

			var fieldTypeProp = submitObj.Properties().SingleOrDefault(k => k.Name.ToLower() == "fieldtype");
			if (fieldTypeProp != null)
			{
				fieldType = (FieldType)Enum.ToObject(typeof(FieldType), fieldTypeProp.Value.ToObject<int>());
			}

			Type inputFieldType = InputField.GetFieldType(fieldType);

			foreach (var prop in submitObj.Properties())
			{
				if (prop.Name.ToLower() == "entityname")
					continue;

				int count = inputFieldType.GetProperties().Where(n => n.Name.ToLower() == prop.Name.ToLower()).Count();
				if (count < 1)
					response.Errors.Add(new ErrorModel(prop.Name, prop.Value.ToString(), "Input object contains property that is not part of the object model."));
			}

			if (response.Errors.Count > 0)
				return DoBadRequestResponse(response);

			try
			{
				field = InputField.ConvertField(submitObj);
			}
			catch (Exception e)
			{
				new LogService().Create(Diagnostics.LogType.Error, "TErpApi:UpdateField", e);
				return DoBadRequestResponse(response, "Input object is not in valid format! It cannot be converted.", e);
			}

			return DoResponse(entMan.UpdateField(entityId, field));
		}

		[Authorize(Roles = "administrator")]
		[AcceptVerbs(new[] { "PATCH" }, Route = "api/v3/en_US/meta/entity/{Id}/field/{FieldId}")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult PatchField(string Id, string FieldId, [FromBody]JObject submitObj)
		{
			FieldResponse response = new FieldResponse();
			Entity entity = new Entity();
			InputField field = new InputGuidField();

			try
			{
				if (!Guid.TryParse(Id, out Guid entityId))
				{
					response.Errors.Add(new ErrorModel("Id", Id, "id parameter is not valid Guid value"));
					return DoBadRequestResponse(response, "Field was not updated!");
				}

				if (!Guid.TryParse(FieldId, out Guid fieldId))
				{
					response.Errors.Add(new ErrorModel("FieldId", FieldId, "FieldId parameter is not valid Guid value"));
					return DoBadRequestResponse(response, "Field was not updated!");
				}

				DbEntity storageEntity = DbContext.Current.EntityRepository.Read(entityId);
				if (storageEntity == null)
				{
					response.Errors.Add(new ErrorModel("Id", Id, "Entity with such Id does not exist!"));
					return DoBadRequestResponse(response, "Field was not updated!");
				}
				entity = storageEntity.MapTo<Entity>();

				Field updatedField = entity.Fields.FirstOrDefault(f => f.Id == fieldId);
				if (updatedField == null)
				{
					response.Errors.Add(new ErrorModel("FieldId", FieldId, "Field with such Id does not exist!"));
					return DoBadRequestResponse(response, "Field was not updated!");
				}

				FieldType fieldType = FieldType.GuidField;

				var fieldTypeProp = submitObj.Properties().SingleOrDefault(k => k.Name.ToLower() == "fieldtype");
				if (fieldTypeProp != null)
				{
					fieldType = (FieldType)Enum.ToObject(typeof(FieldType), fieldTypeProp.Value.ToObject<int>());
				}
				else
				{
					response.Errors.Add(new ErrorModel("fieldType", null, "fieldType is required!"));
					return DoBadRequestResponse(response, "Field was not updated!");
				}

				Type inputFieldType = InputField.GetFieldType(fieldType);
				foreach (var prop in submitObj.Properties())
				{
					if (prop.Name.ToLower() == "entityname")
						continue;

					int count = inputFieldType.GetProperties().Where(n => n.Name.ToLower() == prop.Name.ToLower()).Count();
					if (count < 1)
						response.Errors.Add(new ErrorModel(prop.Name, prop.Value.ToString(), "Input object contains property that is not part of the object model."));
				}

				if (response.Errors.Count > 0)
					return DoBadRequestResponse(response);

				InputField inputField = InputField.ConvertField(submitObj);

				foreach (var prop in submitObj.Properties())
				{
					switch (fieldType)
					{
						case FieldType.AutoNumberField:
							{
								field = new InputAutoNumberField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputAutoNumberField)field).DefaultValue = ((InputAutoNumberField)inputField).DefaultValue;
								if (prop.Name.ToLower() == "U")
									((InputAutoNumberField)field).DisplayFormat = ((InputAutoNumberField)inputField).DisplayFormat;
								if (prop.Name.ToLower() == "startingnumber")
									((InputAutoNumberField)field).StartingNumber = ((InputAutoNumberField)inputField).StartingNumber;
							}
							break;
						case FieldType.CheckboxField:
							{
								field = new InputCheckboxField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputCheckboxField)field).DefaultValue = ((InputCheckboxField)inputField).DefaultValue;
							}
							break;
						case FieldType.CurrencyField:
							{
								field = new InputCurrencyField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputCurrencyField)field).DefaultValue = ((InputCurrencyField)inputField).DefaultValue;
								if (prop.Name.ToLower() == "minvalue")
									((InputCurrencyField)field).MinValue = ((InputCurrencyField)inputField).MinValue;
								if (prop.Name.ToLower() == "maxvalue")
									((InputCurrencyField)field).MaxValue = ((InputCurrencyField)inputField).MaxValue;
								if (prop.Name.ToLower() == "currency")
									((InputCurrencyField)field).Currency = ((InputCurrencyField)inputField).Currency;
							}
							break;
						case FieldType.DateField:
							{
								field = new InputDateField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputDateField)field).DefaultValue = ((InputDateField)inputField).DefaultValue;
								if (prop.Name.ToLower() == "format")
									((InputDateField)field).Format = ((InputDateField)inputField).Format;
								if (prop.Name.ToLower() == "usecurrenttimeasdefaultvalue")
									((InputDateField)field).UseCurrentTimeAsDefaultValue = ((InputDateField)inputField).UseCurrentTimeAsDefaultValue;
							}
							break;
						case FieldType.DateTimeField:
							{
								field = new InputDateTimeField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputDateTimeField)field).DefaultValue = ((InputDateTimeField)inputField).DefaultValue;
								if (prop.Name.ToLower() == "format")
									((InputDateTimeField)field).Format = ((InputDateTimeField)inputField).Format;
								if (prop.Name.ToLower() == "usecurrenttimeasdefaultvalue")
									((InputDateTimeField)field).UseCurrentTimeAsDefaultValue = ((InputDateTimeField)inputField).UseCurrentTimeAsDefaultValue;
							}
							break;
						case FieldType.EmailField:
							{
								field = new InputEmailField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputEmailField)field).DefaultValue = ((InputEmailField)inputField).DefaultValue;
								if (prop.Name.ToLower() == "maxlength")
									((InputEmailField)field).MaxLength = ((InputEmailField)inputField).MaxLength;
							}
							break;
						case FieldType.FileField:
							{
								field = new InputFileField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputFileField)field).DefaultValue = ((InputFileField)inputField).DefaultValue;
							}
							break;
						case FieldType.HtmlField:
							{
								field = new InputHtmlField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputHtmlField)field).DefaultValue = ((InputHtmlField)inputField).DefaultValue;
							}
							break;
						case FieldType.ImageField:
							{
								field = new InputImageField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputImageField)field).DefaultValue = ((InputImageField)inputField).DefaultValue;
							}
							break;
						case FieldType.MultiLineTextField:
							{
								field = new InputMultiLineTextField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputMultiLineTextField)field).DefaultValue = ((InputMultiLineTextField)inputField).DefaultValue;
								if (prop.Name.ToLower() == "maxlength")
									((InputMultiLineTextField)field).MaxLength = ((InputMultiLineTextField)inputField).MaxLength;
								if (prop.Name.ToLower() == "visiblelinenumber")
									((InputMultiLineTextField)field).VisibleLineNumber = ((InputMultiLineTextField)inputField).VisibleLineNumber;
							}
							break;
						case FieldType.MultiSelectField:
							{
								field = new InputMultiSelectField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputMultiSelectField)field).DefaultValue = ((InputMultiSelectField)inputField).DefaultValue;
								if (prop.Name.ToLower() == "options")
									((InputMultiSelectField)field).Options = ((InputMultiSelectField)inputField).Options;
							}
							break;
						case FieldType.NumberField:
							{
								field = new InputNumberField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputNumberField)field).DefaultValue = ((InputNumberField)inputField).DefaultValue;
								if (prop.Name.ToLower() == "minvalue")
									((InputNumberField)field).MinValue = ((InputNumberField)inputField).MinValue;
								if (prop.Name.ToLower() == "maxvalue")
									((InputNumberField)field).MaxValue = ((InputNumberField)inputField).MaxValue;
								if (prop.Name.ToLower() == "decimalplaces")
									((InputNumberField)field).DecimalPlaces = ((InputNumberField)inputField).DecimalPlaces;
							}
							break;
						case FieldType.PasswordField:
							{
								field = new InputPasswordField();
								if (prop.Name.ToLower() == "maxlength")
									((InputPasswordField)field).MaxLength = ((InputPasswordField)inputField).MaxLength;
								if (prop.Name.ToLower() == "minlength")
									((InputPasswordField)field).MinLength = ((InputPasswordField)inputField).MinLength;
								if (prop.Name.ToLower() == "encrypted")
									((InputPasswordField)field).Encrypted = ((InputPasswordField)inputField).Encrypted;
							}
							break;
						case FieldType.PercentField:
							{
								field = new InputPercentField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputPercentField)field).DefaultValue = ((InputPercentField)inputField).DefaultValue;
								if (prop.Name.ToLower() == "minvalue")
									((InputPercentField)field).MinValue = ((InputPercentField)inputField).MinValue;
								if (prop.Name.ToLower() == "maxvalue")
									((InputPercentField)field).MaxValue = ((InputPercentField)inputField).MaxValue;
								if (prop.Name.ToLower() == "decimalplaces")
									((InputPercentField)field).DecimalPlaces = ((InputPercentField)inputField).DecimalPlaces;
							}
							break;
						case FieldType.PhoneField:
							{
								field = new InputPhoneField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputPhoneField)field).DefaultValue = ((InputPhoneField)inputField).DefaultValue;
								if (prop.Name.ToLower() == "format")
									((InputPhoneField)field).Format = ((InputPhoneField)inputField).Format;
								if (prop.Name.ToLower() == "maxlength")
									((InputPhoneField)field).MaxLength = ((InputPhoneField)inputField).MaxLength;
							}
							break;
						case FieldType.GuidField:
							{
								field = new InputGuidField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputGuidField)field).DefaultValue = ((InputGuidField)inputField).DefaultValue;
								if (prop.Name.ToLower() == "generatenewid")
									((InputGuidField)field).GenerateNewId = ((InputGuidField)inputField).GenerateNewId;
							}
							break;
						case FieldType.SelectField:
							{
								field = new InputSelectField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputSelectField)field).DefaultValue = ((InputSelectField)inputField).DefaultValue;
								if (prop.Name.ToLower() == "options")
									((InputSelectField)field).Options = ((InputSelectField)inputField).Options;
							}
							break;
						case FieldType.TextField:
							{
								field = new InputTextField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputTextField)field).DefaultValue = ((InputTextField)inputField).DefaultValue;
								if (prop.Name.ToLower() == "maxlength")
									((InputTextField)field).MaxLength = ((InputTextField)inputField).MaxLength;
							}
							break;
						case FieldType.UrlField:
							{
								field = new InputUrlField();
								if (prop.Name.ToLower() == "defaultvalue")
									((InputUrlField)field).DefaultValue = ((InputUrlField)inputField).DefaultValue;
								if (prop.Name.ToLower() == "maxlength")
									((InputUrlField)field).MaxLength = ((InputUrlField)inputField).MaxLength;
								if (prop.Name.ToLower() == "opentargetinnewwindow")
									((InputUrlField)field).OpenTargetInNewWindow = ((InputUrlField)inputField).OpenTargetInNewWindow;
							}
							break;
					}

					if (prop.Name.ToLower() == "label")
						field.Label = inputField.Label;
					else if (prop.Name.ToLower() == "placeholdertext")
						field.PlaceholderText = inputField.PlaceholderText;
					else if (prop.Name.ToLower() == "description")
						field.Description = inputField.Description;
					else if (prop.Name.ToLower() == "helptext")
						field.HelpText = inputField.HelpText;
					else if (prop.Name.ToLower() == "required")
						field.Required = inputField.Required;
					else if (prop.Name.ToLower() == "unique")
						field.Unique = inputField.Unique;
					else if (prop.Name.ToLower() == "searchable")
						field.Searchable = inputField.Searchable;
					else if (prop.Name.ToLower() == "auditable")
						field.Auditable = inputField.Auditable;
					else if (prop.Name.ToLower() == "system")
						field.System = inputField.System;
				}
			}
			catch (Exception e)
			{
				new LogService().Create(Diagnostics.LogType.Error, "TErpApi:PatchField", e);
				return DoBadRequestResponse(response, "Input object is not in valid format! It cannot be converted.", e);
			}

			return DoResponse(entMan.UpdateField(entity, field));
		}

		[Authorize(Roles = "administrator")]
		[AcceptVerbs(new[] { "DELETE" }, Route = "api/v3/en_US/meta/entity/{Id}/field/{FieldId}")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult DeleteField(string Id, string FieldId)
		{
			FieldResponse response = new FieldResponse();

			if (!Guid.TryParse(Id, out Guid entityId))
			{
				response.Errors.Add(new ErrorModel("id", Id, "id parameter is not valid Guid value"));
				return DoResponse(response);
			}

			if (!Guid.TryParse(FieldId, out Guid fieldId))
			{
				response.Errors.Add(new ErrorModel("id", FieldId, "FieldId parameter is not valid Guid value"));
				return DoResponse(response);
			}

			return DoResponse(entMan.DeleteField(entityId, fieldId));
		}

		#endregion

		#region << Relation Meta >>
		// Get all entity relation definitions
		// GET: api/v3/en_US/meta/relation/list/
		[Authorize(Roles = "administrator")]
		[AcceptVerbs(new[] { "GET" }, Route = "api/v3/en_US/meta/relation/list")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult GetEntityRelationMetaList(string hash = null)
		{
			var response = new EntityRelationManager().Read();

			//check hash and clear data if hash match
			if (response.Success && response.Object != null && !string.IsNullOrWhiteSpace(hash) && response.Hash == hash)
				response.Object = null;

			return DoResponse(response);
		}

		// Get entity relation meta
		// GET: api/v3/en_US/meta/relation/{name}/
		[Authorize(Roles = "administrator")]
		[AcceptVerbs(new[] { "GET" }, Route = "api/v3/en_US/meta/relation/{name}")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult GetEntityRelationMeta(string name)
		{
			return DoResponse(new EntityRelationManager().Read(name));
		}


		// Create an entity relation
		// POST: api/v3/en_US/meta/relation
		[Authorize(Roles = "administrator")]
		[AcceptVerbs(new[] { "POST" }, Route = "api/v3/en_US/meta/relation")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult CreateEntityRelation([FromBody]JObject submitObj)
		{
			try
			{
				if (submitObj["id"].IsNullOrEmpty())
					submitObj["id"] = Guid.NewGuid();
				var relation = submitObj.ToObject<EntityRelation>();
				return DoResponse(new EntityRelationManager().Create(relation));
			}
			catch (Exception e)
			{
				new LogService().Create(Diagnostics.LogType.Error, "TErpApi:CreateEntityRelation", e);
				return DoBadRequestResponse(new EntityRelationResponse(), null, e);
			}
		}

		// Update an entity relation
		// PUT: api/v3/en_US/meta/relation/id
		[Authorize(Roles = "administrator")]
		[AcceptVerbs(new[] { "PUT" }, Route = "api/v3/en_US/meta/relation/{RelationIdString}")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult UpdateEntityRelation(string RelationIdString, [FromBody]JObject submitObj)
		{
			FieldResponse response = new FieldResponse();

			if (!Guid.TryParse(RelationIdString, out Guid relationId))
			{
				response.Errors.Add(new ErrorModel("id", RelationIdString, "id parameter is not valid Guid value"));
				return DoResponse(response);
			}

			try
			{
				var relation = submitObj.ToObject<EntityRelation>();
				return DoResponse(new EntityRelationManager().Update(relation));
			}
			catch (Exception e)
			{
				new LogService().Create(Diagnostics.LogType.Error, "TErpApi:UpdateEntityRelation", e);
				return DoBadRequestResponse(new EntityRelationResponse(), null, e);
			}
		}

		// Delete an entity relation
		// DELETE: api/v3/en_US/meta/relation/{idToken}
		[Authorize(Roles = "administrator")]
		[AcceptVerbs(new[] { "DELETE" }, Route = "api/v3/en_US/meta/relation/{idToken}")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult DeleteEntityRelation(string idToken)
		{
			Guid id = Guid.Empty;
			if (Guid.TryParse(idToken, out Guid newGuid))
			{
				return DoResponse(new EntityRelationManager().Delete(newGuid));
			}
			else
			{
				return DoBadRequestResponse(new EntityRelationResponse(), "The entity relation Id should be a valid Guid", null);
			}

		}

		#endregion

		#region << Records >>

		// Update an entity record relation records for origin record
		// POST: api/v3/en_US/record/relation
		[AcceptVerbs(new[] { "POST" }, Route = "api/v3/en_US/record/relation")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult UpdateEntityRelationRecord([FromBody]InputEntityRelationRecordUpdateModel model)
		{

			var recMan = new RecordManager();
			var entMan = new EntityManager();
			BaseResponseModel response = new BaseResponseModel { Timestamp = DateTime.UtcNow, Success = true, Errors = new List<ErrorModel>() };

			if (model == null)
			{
				response.Errors.Add(new ErrorModel { Message = "Invalid model." });
				response.Success = false;
				return DoResponse(response);
			}

			EntityRelation relation = null;
			if (string.IsNullOrWhiteSpace(model.RelationName))
			{
				response.Errors.Add(new ErrorModel { Message = "Invalid relation name.", Key = "relationName" });
				response.Success = false;
				return DoResponse(response);
			}
			else
			{
				relation = new EntityRelationManager().Read(model.RelationName).Object;
				if (relation == null)
				{
					response.Errors.Add(new ErrorModel { Message = "Invalid relation name. No relation with that name.", Key = "relationName" });
					response.Success = false;
					return DoResponse(response);
				}
			}

			var originEntity = entMan.ReadEntity(relation.OriginEntityId).Object;
			var targetEntity = entMan.ReadEntity(relation.TargetEntityId).Object;
			var originField = originEntity.Fields.Single(x => x.Id == relation.OriginFieldId);
			var targetField = targetEntity.Fields.Single(x => x.Id == relation.TargetFieldId);

			if (model.DetachTargetFieldRecordIds != null && model.DetachTargetFieldRecordIds.Any() && targetField.Required && relation.RelationType != EntityRelationType.ManyToMany)
			{
				response.Errors.Add(new ErrorModel { Message = "Cannot detach records, when target field is required.", Key = "originFieldRecordId" });
				response.Success = false;
				return DoResponse(response);
			}

			EntityQuery query = new EntityQuery(originEntity.Name, "id," + originField.Name, EntityQuery.QueryEQ("id", model.OriginFieldRecordId), null, null, null);
			QueryResponse result = recMan.Find(query);
			if (result.Object.Data.Count == 0)
			{
				response.Errors.Add(new ErrorModel { Message = "Origin record was not found. Id=[" + model.OriginFieldRecordId + "]", Key = "originFieldRecordId" });
				response.Success = false;
				return DoResponse(response);
			}

			var originRecord = result.Object.Data[0];
			object originValue = originRecord[originField.Name];

			var attachTargetRecords = new List<EntityRecord>();
			var detachTargetRecords = new List<EntityRecord>();

			foreach (var targetId in model.AttachTargetFieldRecordIds)
			{
				query = new EntityQuery(targetEntity.Name, "id," + targetField.Name, EntityQuery.QueryEQ("id", targetId), null, null, null);
				result = recMan.Find(query);
				if (result.Object.Data.Count == 0)
				{
					response.Errors.Add(new ErrorModel { Message = "Attach target record was not found. Id=[" + targetEntity.Id + "]", Key = "targetRecordId" });
					response.Success = false;
					return DoResponse(response);
				}
				else if (attachTargetRecords.Any(x => (Guid)x["id"] == targetId))
				{
					response.Errors.Add(new ErrorModel { Message = "Attach target id was duplicated. Id=[" + targetEntity.Id + "]", Key = "targetRecordId" });
					response.Success = false;
					return DoResponse(response);
				}
				attachTargetRecords.Add(result.Object.Data[0]);
			}

			foreach (var targetId in model.DetachTargetFieldRecordIds)
			{
				query = new EntityQuery(targetEntity.Name, "id," + targetField.Name, EntityQuery.QueryEQ("id", targetId), null, null, null);
				result = recMan.Find(query);
				if (result.Object.Data.Count == 0)
				{
					response.Errors.Add(new ErrorModel { Message = "Detach target record was not found. Id=[" + targetEntity.Id + "]", Key = "targetRecordId" });
					response.Success = false;
					return DoResponse(response);
				}
				else if (detachTargetRecords.Any(x => (Guid)x["id"] == targetId))
				{
					response.Errors.Add(new ErrorModel { Message = "Detach target id was duplicated. Id=[" + targetEntity.Id + "]", Key = "targetRecordId" });
					response.Success = false;
					return DoResponse(response);
				}
				detachTargetRecords.Add(result.Object.Data[0]);
			}

			using (var connection = DbContext.Current.CreateConnection())
			{
				connection.BeginTransaction();

				try
				{
					switch (relation.RelationType)
					{
						case EntityRelationType.OneToOne:
						case EntityRelationType.OneToMany:
							{
								foreach (var record in detachTargetRecords)
								{
									record[targetField.Name] = null;

									var updResult = recMan.UpdateRecord(targetEntity, record);
									if (!updResult.Success)
									{
										connection.RollbackTransaction();
										response.Errors = updResult.Errors;
										response.Message = "Target record id=[" + record["id"] + "] detach operation failed.";
										response.Success = false;
										return DoResponse(response);
									}
								}

								foreach (var record in attachTargetRecords)
								{
									var patchObject = new EntityRecord();
									patchObject["id"] = (Guid)record["id"];
									patchObject[targetField.Name] = originValue;

									var updResult = recMan.UpdateRecord(targetEntity, patchObject);
									if (!updResult.Success)
									{
										connection.RollbackTransaction();
										response.Errors = updResult.Errors;
										response.Message = "Target record id=[" + record["id"] + "] attach operation failed.";
										response.Success = false;
										return DoResponse(response);
									}
								}
							}
							break;
						case EntityRelationType.ManyToMany:
							{
								foreach (var record in detachTargetRecords)
								{
									QueryResponse updResult = recMan.RemoveRelationManyToManyRecord(relation.Id, (Guid)originValue, (Guid)record[targetField.Name]);

									if (!updResult.Success)
									{
										connection.RollbackTransaction();
										response.Errors = updResult.Errors;
										response.Message = "Target record id=[" + record["id"] + "] detach operation failed.";
										response.Success = false;
										return DoResponse(response);
									}
								}

								foreach (var record in attachTargetRecords)
								{
									QueryResponse updResult = recMan.CreateRelationManyToManyRecord(relation.Id, (Guid)originValue, (Guid)record[targetField.Name]);

									if (!updResult.Success)
									{
										connection.RollbackTransaction();
										response.Errors = updResult.Errors;
										response.Message = "Target record id=[" + record["id"] + "] attach  operation failed.";
										response.Success = false;
										return DoResponse(response);
									}
								}
							}
							break;
						default:
							{
								connection.RollbackTransaction();
								throw new Exception("Not supported relation type");
							}
					}

					connection.CommitTransaction();
				}
				catch (Exception ex)
				{
					connection.RollbackTransaction();
					new LogService().Create(Diagnostics.LogType.Error, "TErpApi:UpdateEntityRelationRecord", ex);
					response.Success = false;
					response.Message = ex.Message;
					return DoResponse(response);
				}
			}

			return DoResponse(response);
		}


		// Update an entity record relation records for target record
		// POST: api/v3/en_US/record/relation/reverse
		[AcceptVerbs(new[] { "POST" }, Route = "api/v3/en_US/record/relation/reverse")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult UpdateEntityRelationRecordReverse([FromBody]InputEntityRelationRecordReverseUpdateModel model)
		{

			var recMan = new RecordManager();
			var entMan = new EntityManager();
			BaseResponseModel response = new BaseResponseModel { Timestamp = DateTime.UtcNow, Success = true, Errors = new List<ErrorModel>() };

			if (model == null)
			{
				response.Errors.Add(new ErrorModel { Message = "Invalid model." });
				response.Success = false;
				return DoResponse(response);
			}

			EntityRelation relation = null;
			if (string.IsNullOrWhiteSpace(model.RelationName))
			{
				response.Errors.Add(new ErrorModel { Message = "Invalid relation name.", Key = "relationName" });
				response.Success = false;
				return DoResponse(response);
			}
			else
			{
				relation = new EntityRelationManager().Read(model.RelationName).Object;
				if (relation == null)
				{
					response.Errors.Add(new ErrorModel { Message = "Invalid relation name. No relation with that name.", Key = "relationName" });
					response.Success = false;
					return DoResponse(response);
				}
			}

			var originEntity = entMan.ReadEntity(relation.OriginEntityId).Object;
			var targetEntity = entMan.ReadEntity(relation.TargetEntityId).Object;
			var originField = originEntity.Fields.Single(x => x.Id == relation.OriginFieldId);
			var targetField = targetEntity.Fields.Single(x => x.Id == relation.TargetFieldId);

			if (model.DetachOriginFieldRecordIds != null && model.DetachOriginFieldRecordIds.Any() && originField.Required && relation.RelationType != EntityRelationType.ManyToMany)
			{
				response.Errors.Add(new ErrorModel { Message = "Cannot detach records, when origin field is required.", Key = "originFieldRecordId" });
				response.Success = false;
				return DoResponse(response);
			}

			EntityQuery query = new EntityQuery(targetEntity.Name, "id," + targetField.Name, EntityQuery.QueryEQ("id", model.TargetFieldRecordId), null, null, null);
			QueryResponse result = recMan.Find(query);
			if (result.Object.Data.Count == 0)
			{
				response.Errors.Add(new ErrorModel { Message = "Target record was not found. Id=[" + model.TargetFieldRecordId + "]", Key = "targetFieldRecordId" });
				response.Success = false;
				return DoResponse(response);
			}

			var targetRecord = result.Object.Data[0];
			object targetValue = targetRecord[targetField.Name];

			var attachOriginRecords = new List<EntityRecord>();
			var detachOriginRecords = new List<EntityRecord>();

			foreach (var originId in model.AttachOriginFieldRecordIds)
			{
				query = new EntityQuery(originEntity.Name, "id," + originField.Name, EntityQuery.QueryEQ("id", originId), null, null, null);
				result = recMan.Find(query);
				if (result.Object.Data.Count == 0)
				{
					response.Errors.Add(new ErrorModel { Message = "Attach origin record was not found. Id=[" + originEntity.Id + "]", Key = "originRecordId" });
					response.Success = false;
					return DoResponse(response);
				}
				else if (attachOriginRecords.Any(x => (Guid)x["id"] == originId))
				{
					response.Errors.Add(new ErrorModel { Message = "Attach origin id was duplicated. Id=[" + originEntity.Id + "]", Key = "originRecordId" });
					response.Success = false;
					return DoResponse(response);
				}
				attachOriginRecords.Add(result.Object.Data[0]);
			}

			foreach (var originId in model.DetachOriginFieldRecordIds)
			{
				query = new EntityQuery(originEntity.Name, "id," + originField.Name, EntityQuery.QueryEQ("id", originId), null, null, null);
				result = recMan.Find(query);
				if (result.Object.Data.Count == 0)
				{
					response.Errors.Add(new ErrorModel { Message = "Detach origin record was not found. Id=[" + originEntity.Id + "]", Key = "originRecordId" });
					response.Success = false;
					return DoResponse(response);
				}
				else if (detachOriginRecords.Any(x => (Guid)x["id"] == originId))
				{
					response.Errors.Add(new ErrorModel { Message = "Detach origin id was duplicated. Id=[" + originEntity.Id + "]", Key = "originRecordId" });
					response.Success = false;
					return DoResponse(response);
				}
				detachOriginRecords.Add(result.Object.Data[0]);
			}

			using (var connection = DbContext.Current.CreateConnection())
			{
				connection.BeginTransaction();

				try
				{
					switch (relation.RelationType)
					{
						case EntityRelationType.OneToOne:
						case EntityRelationType.OneToMany:
							{
								foreach (var record in detachOriginRecords)
								{
									record[originField.Name] = null;

									var updResult = recMan.UpdateRecord(originEntity, record);
									if (!updResult.Success)
									{
										connection.RollbackTransaction();
										response.Errors = updResult.Errors;
										response.Message = "Origin record id=[" + record["id"] + "] detach operation failed.";
										response.Success = false;
										return DoResponse(response);
									}
								}

								foreach (var record in attachOriginRecords)
								{
									var patchObject = new EntityRecord();
									patchObject["id"] = (Guid)record["id"];
									patchObject[originField.Name] = targetValue;

									var updResult = recMan.UpdateRecord(originEntity, patchObject);
									if (!updResult.Success)
									{
										connection.RollbackTransaction();
										response.Errors = updResult.Errors;
										response.Message = "Origin record id=[" + record["id"] + "] attach operation failed.";
										response.Success = false;
										return DoResponse(response);
									}
								}
							}
							break;
						case EntityRelationType.ManyToMany:
							{
								foreach (var record in detachOriginRecords)
								{
									QueryResponse updResult = recMan.RemoveRelationManyToManyRecord(relation.Id, (Guid)record[originField.Name], (Guid)targetValue);

									if (!updResult.Success)
									{
										connection.RollbackTransaction();
										response.Errors = updResult.Errors;
										response.Message = "Origin record id=[" + record["id"] + "] detach operation failed.";
										response.Success = false;
										return DoResponse(response);
									}
								}

								foreach (var record in attachOriginRecords)
								{
									QueryResponse updResult = recMan.CreateRelationManyToManyRecord(relation.Id, (Guid)record[originField.Name], (Guid)targetValue);

									if (!updResult.Success)
									{
										connection.RollbackTransaction();
										response.Errors = updResult.Errors;
										response.Message = "Origin record id=[" + record["id"] + "] attach  operation failed.";
										response.Success = false;
										return DoResponse(response);
									}
								}
							}
							break;
						default:
							{
								connection.RollbackTransaction();
								throw new Exception("Not supported relation type");
							}
					}

					connection.CommitTransaction();
				}
				catch (Exception ex)
				{
					connection.RollbackTransaction();
					new LogService().Create(Diagnostics.LogType.Error, "TErpApi:UpdateEntityRelationRecordReverse", ex);
					response.Success = false;
					response.Message = ex.Message;
					return DoResponse(response);
				}
			}

			return DoResponse(response);
		}


		// Get an entity record list
		// GET: api/v3/en_US/record/{entityName}/list
		[AcceptVerbs(new[] { "GET" }, Route = "api/v3/en_US/record/{entityName}/{recordId}")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult GetRecord(Guid recordId, string entityName, string fields = "*")
		{
			QueryObject filterObj = EntityQuery.QueryEQ("id", recordId);

			EntityQuery query = new EntityQuery(entityName, fields, filterObj, null, null, null);

			QueryResponse result = recMan.Find(query);
			if (!result.Success)
				return DoResponse(result);

			return Json(result);
		}

		// Get an entity record list
		// GET: api/v3/en_US/record/{entityName}/list
		[AcceptVerbs(new[] { "DELETE" }, Route = "api/v3/en_US/record/{entityName}/{recordId}")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult DeleteRecord(Guid recordId, string entityName)
		{
			//Create transaction
			var result = new QueryResponse();
			using (var connection = DbContext.Current.CreateConnection())
			{
				try
				{
					connection.BeginTransaction();
					result = recMan.DeleteRecord(entityName, recordId);
					connection.CommitTransaction();
				}
				catch (Exception ex)
				{
					connection.RollbackTransaction();
					new LogService().Create(Diagnostics.LogType.Error, "TErpApi:DeleteRecord", ex);
					var response = new ResponseModel
					{
						Success = false,
						Timestamp = DateTime.UtcNow,
						Message = "Error while delete the record: " + ex.Message,
						Object = null
					};
					return Json(response);
				}
			}

			return DoResponse(result);
		}

		// Get an entity records by field and regex
		// GET: api/v3/en_US/record/{entityName}/regex
		[AcceptVerbs(new[] { "POST" }, Route = "api/v3/en_US/record/{entityName}/regex/{fieldName}")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult GetRecordsByFieldAndRegex(string fieldName, string entityName, [FromBody]EntityRecord patternObj)
		{

			QueryObject filterObj = EntityQuery.QueryRegex(fieldName, patternObj["pattern"]);

			EntityQuery query = new EntityQuery(entityName, "*", filterObj, null, null, null);

			QueryResponse result = recMan.Find(query);
			if (!result.Success)
				return DoResponse(result);
			return Json(result);
		}


		// Create an entity record
		// POST: api/v3/en_US/record/{entityName}
		[AcceptVerbs(new[] { "POST" }, Route = "api/v3/en_US/record/{entityName}")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult CreateEntityRecord(string entityName, [FromBody]EntityRecord postObj)
		{
			//Find and change properties starting with _$ to $$ - angular does not post $$ propery names
			postObj = Helpers.FixDoubleDollarSignProblem(postObj);

			if (!postObj.GetProperties().Any(x => x.Key == "id"))
				postObj["id"] = Guid.NewGuid();
			else if (string.IsNullOrEmpty(postObj["id"] as string))
				postObj["id"] = Guid.NewGuid();


			//Create transaction
			var result = new QueryResponse();
			using (var connection = DbContext.Current.CreateConnection())
			{
				try
				{
					connection.BeginTransaction();
					result = recMan.CreateRecord(entityName, postObj);
					connection.CommitTransaction();
				}
				catch (Exception ex)
				{
					connection.RollbackTransaction();
					new LogService().Create(Diagnostics.LogType.Error, "TErpApi:CreateEntityRecord", ex);
					var response = new ResponseModel
					{
						Success = false,
						Timestamp = DateTime.UtcNow,
						Message = "Error while saving the record: " + ex.Message,
						Object = null
					};
					return Json(response);
				}
			}

			return DoResponse(result);
		}

		[AcceptVerbs(new[] { "POST" }, Route = "api/v3/en_US/record/{entityName}/with-relation/{relationName}/{relatedRecordId}")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult CreateEntityRecordWithRelation(string entityName, string relationName, Guid relatedRecordId, [FromBody]EntityRecord postObj)
		{
			var validationErrors = new List<ErrorModel>();

			//1.Validate relationName
			//1.1. Relation exists
			var relation = relMan.Read().Object.SingleOrDefault(x => x.Name == relationName);
			string targetEntityName = String.Empty;
			string targetFieldName = String.Empty;
			var relatedRecord = new EntityRecord();
			var relatedRecordResponse = new QueryResponse();
			if (relation == null)
			{
				var error = new ErrorModel
				{
					Key = "relationName",
					Value = relationName,
					Message = "A relation with this name, does not exist"
				};
				validationErrors.Add(error);
			}
			else
			{
				//1.2. Relation is correct - entityName is part of this relation
				if (relation.OriginEntityName != entityName && relation.TargetEntityName != entityName)
				{
					var error = new ErrorModel
					{
						Key = "relationName",
						Value = relationName,
						Message = "This is not the correct relation, as it does not include the requested entity: " + entityName
					};
					validationErrors.Add(error);
				}
				else
				{
					if (relation.OriginEntityName == entityName)
					{
						relatedRecordResponse = recMan.Find(new EntityQuery(relation.TargetEntityName, "*", EntityQuery.QueryEQ("id", relatedRecordId)));
						targetFieldName = relation.TargetFieldName;
					}
					else
					{
						relatedRecordResponse = recMan.Find(new EntityQuery(relation.OriginEntityName, "*", EntityQuery.QueryEQ("id", relatedRecordId)));
						targetFieldName = relation.OriginFieldName;
					}
					//2. Validate parentRecordId
					//2.1. parentRecordId exists

					if (!relatedRecordResponse.Object.Data.Any())
					{
						var error = new ErrorModel
						{
							Key = "parentRecordId",
							Value = relatedRecordId.ToString(),
							Message = "There is no parent record with this Id in the entity: " + entityName
						};
						validationErrors.Add(error);
					}
					else
					{
						relatedRecord = relatedRecordResponse.Object.Data.First();
						//2.2. Record has value in the related field		
						if (!relatedRecord.Properties.ContainsKey(targetFieldName) || relatedRecord[targetFieldName] == null)
						{
							var error = new ErrorModel
							{
								Key = "parentRecordId",
								Value = relatedRecordId.ToString(),
								Message = "The parent record does not have field " + targetFieldName + " or its value is null"
							};
							validationErrors.Add(error);
						}
					}
				}
			}


			if (postObj == null)
				postObj = new EntityRecord();

			if (validationErrors.Count > 0)
			{
				var response = new ResponseModel
				{
					Success = false,
					Timestamp = DateTime.UtcNow,
					Errors = validationErrors,
					Message = "Validation error occurred!",
					Object = null
				};
				return Json(response);
			}

			if (!postObj.GetProperties().Any(x => x.Key == "id"))
				postObj["id"] = Guid.NewGuid();
			else if (string.IsNullOrEmpty(postObj["id"] as string))
				postObj["id"] = Guid.NewGuid();


			//Create transaction
			var result = new QueryResponse();
			using (var connection = DbContext.Current.CreateConnection())
			{
				try
				{
					connection.BeginTransaction();

					//Add the relation field value if the relation is 1:1 or 1:N
					if (relation.RelationType == EntityRelationType.OneToOne || relation.RelationType == EntityRelationType.OneToMany)
					{
						//if currentEntity is origin -> update the parent record
						if (relation.OriginEntityName == entityName)
						{
							throw new Exception("We need a case to finish this");
						}
						else
						{
							//if currentEntity is target -> get the target field and assing the correct id value of the origin 
							postObj[relation.TargetFieldName] = relatedRecord[relation.OriginFieldName];
						}
					}

					result = recMan.CreateRecord(entityName, postObj);

					//Create a relation record if it is N:N
					if (relation.RelationType == EntityRelationType.ManyToMany)
					{
						var response = new QueryResponse();
						if (relation.OriginEntityName == entityName && relation.TargetEntityName == entityName)
						{
							throw new Exception("current entity is both target and origin, cannot find relation direction. Probably needs to be extended");
						}
						else if (relation.TargetEntityName == entityName)
						{
							//if current is target -> create relation
							response = recMan.CreateRelationManyToManyRecord(relation.Id, relatedRecordId, (Guid)postObj["id"]);
						}
						else
						{
							//if current is origin -> create relation	
							response = recMan.CreateRelationManyToManyRecord(relation.Id, (Guid)postObj["id"], relatedRecordId);
						}
						if (!response.Success)
						{
							throw new Exception(response.Message);
						}
					}

					connection.CommitTransaction();
				}
				catch (Exception ex)
				{
					connection.RollbackTransaction();
					new LogService().Create(Diagnostics.LogType.Error, "TErpApi:CreateEntityRecordWithRelation", ex);
					var response = new ResponseModel
					{
						Success = false,
						Timestamp = DateTime.UtcNow,
						Message = "Error while saving the record: " + ex.Message,
						Object = null
					};
					return Json(response);
				}
			}

			return DoResponse(result);
		}


		// Update an entity record
		// PUT: api/v3/en_US/record/{entityName}/{recordId}
		[AcceptVerbs(new[] { "PUT" }, Route = "api/v3/en_US/record/{entityName}/{recordId}")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult UpdateEntityRecord(string entityName, Guid recordId, [FromBody]EntityRecord postObj)
		{
			//Find and change properties starting with _$ to $$ - angular does not post $$ propery names
			postObj = Helpers.FixDoubleDollarSignProblem(postObj);


			if (!postObj.Properties.ContainsKey("id"))
			{
				postObj["id"] = recordId;
			}

			//clear authentication cache
			if (entityName == "user")
			{
				throw new Exception("Management of user record should be implemented");
				//WebSecurityUtil.RemoveIdentityFromCache(recordId);
			}
			//Create transaction
			var result = new QueryResponse();
			using (var connection = DbContext.Current.CreateConnection())
			{
				try
				{
					connection.BeginTransaction();
					result = recMan.UpdateRecord(entityName, postObj);
					connection.CommitTransaction();
				}
				catch (Exception ex)
				{
					connection.RollbackTransaction();
					new LogService().Create(Diagnostics.LogType.Error, "TErpApi:UpdateEntityRecord", ex);
					var response = new ResponseModel
					{
						Success = false,
						Timestamp = DateTime.UtcNow,
						Message = "Error while saving the record: " + ex.Message,
						Object = null
					};
					return Json(response);
				}
			}

			return DoResponse(result);
		}

		// Patch an entity record
		// PATCH: api/v3/en_US/record/{entityName}/{recordId}
		[AcceptVerbs(new[] { "PATCH" }, Route = "api/v3/en_US/record/{entityName}/{recordId}")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult PatchEntityRecord(string entityName, Guid recordId, [FromBody]EntityRecord postObj)
		{
			//clear authentication cache
			if (entityName == "user")
			{
				throw new Exception("Management of user record should be implemented");
				//WebSecurityUtil.RemoveIdentityFromCache(recordId);
			}
			postObj["id"] = recordId;

			//Create transaction
			var result = new QueryResponse();
			using (var connection = DbContext.Current.CreateConnection())
			{
				try
				{
					connection.BeginTransaction();
					result = recMan.UpdateRecord(entityName, postObj);
					connection.CommitTransaction();
				}
				catch (Exception ex)
				{
					connection.RollbackTransaction();
					new LogService().Create(Diagnostics.LogType.Error, "TErpApi:PatchEntityRecord", ex);
					var response = new ResponseModel
					{
						Success = false,
						Timestamp = DateTime.UtcNow,
						Message = "Error while saving the record: " + ex.Message,
						Object = null
					};
					return Json(response);
				}
			}

			return DoResponse(result);
		}

		// GET: api/v3/en_US/record/{entityName}/list
		[AcceptVerbs(new[] { "GET" }, Route = "api/v3/en_US/record/{entityName}/list")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult GetRecordsByEntityName(string entityName, string ids = "", string fields = "", int? limit = null)
		{
			var response = new QueryResponse();
			var recordIdList = new List<Guid>();
			var fieldList = new List<string>();

			if (!String.IsNullOrWhiteSpace(ids) && ids != "null")
			{
				var idStringList = ids.Split(',');
				var outGuid = Guid.Empty;
				foreach (var idString in idStringList)
				{
					if (Guid.TryParse(idString, out outGuid))
					{
						recordIdList.Add(outGuid);
					}
					else
					{
						response.Message = "One of the record ids is not a Guid";
						response.Timestamp = DateTime.UtcNow;
						response.Success = false;
						response.Object.Data = null;
					}
				}
			}

			if (!String.IsNullOrWhiteSpace(fields) && fields != "null")
			{
				var fieldsArray = fields.Split(',');
				var hasId = false;
				foreach (var fieldName in fieldsArray)
				{
					if (fieldName == "id")
					{
						hasId = true;
					}
					fieldList.Add(fieldName);
				}
				if (!hasId)
				{
					fieldList.Add("id");
				}
			}

			var QueryList = new List<QueryObject>();
			foreach (var recordId in recordIdList)
			{
				QueryList.Add(EntityQuery.QueryEQ("id", recordId));
			}

			QueryObject recordsFilterObj = null;
			if (QueryList.Count > 0)
			{
				recordsFilterObj = EntityQuery.QueryOR(QueryList.ToArray());
			}

			var columns = "*";
			if (fieldList.Count > 0)
			{
				if (!fieldList.Contains("id"))
				{
					fieldList.Add("id");
				}
				columns = String.Join(",", fieldList.Select(x => x.ToString()).ToArray());
			}

			//var sortRulesList = new List<QuerySortObject>();
			//var sortRule = new QuerySortObject("id",QuerySortType.Descending);
			//sortRulesList.Add(sortRule);
			//EntityQuery query = new EntityQuery(entityName, columns, recordsFilterObj, sortRulesList.ToArray(), null, null);

			EntityQuery query = new EntityQuery(entityName, columns, recordsFilterObj, null, null, null);
			if (limit != null && limit > 0)
			{
				query = new EntityQuery(entityName, columns, recordsFilterObj, null, null, limit);
			}

			var queryResponse = recMan.Find(query);
			if (!queryResponse.Success)
			{
				response.Message = queryResponse.Message;
				response.Timestamp = DateTime.UtcNow;
				response.Success = false;
				response.Object = null;
				return DoResponse(response);
			}


			response.Message = "Success";
			response.Timestamp = DateTime.UtcNow;
			response.Success = true;
			response.Object.Data = queryResponse.Object.Data;
			return DoResponse(response);
		}

		private QueryResponse CreateErrorResponse(string message)
		{
			var response = new QueryResponse
			{
				Success = false,
				Timestamp = DateTime.UtcNow,
				Message = message,
				Object = null
			};
			return response;
		}

		// Import list records to csv
		// POST: api/v3/en_US/record/{entityName}/list/{listName}/import
		[AcceptVerbs(new[] { "POST" }, Route = "api/v3/en_US/record/{entityName}/import")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult ImportEntityRecordsFromCsv(string entityName, [FromBody]JObject postObject)
		{
			string fileTempPath = "";

			if (!postObject.IsNullOrEmpty() && postObject.Properties().Any(p => p.Name == "fileTempPath"))
			{
				fileTempPath = postObject["fileTempPath"].ToString();
			}

			ImportExportManager ieManager = new ImportExportManager();
			ResponseModel response = ieManager.ImportEntityRecordsFromCsv(entityName, fileTempPath);

			return DoResponse(response);

		}


		// Import list records to csv
		// POST: api/v3/en_US/record/{entityName}/list/{listName}/import
		[AcceptVerbs(new[] { "POST" }, Route = "api/v3/en_US/record/{entityName}/import-evaluate")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult EvaluateImportEntityRecordsFromCsv(string entityName, [FromBody]JObject postObject)
		{
			ImportExportManager ieManager = new ImportExportManager();
			ResponseModel response = ieManager.EvaluateImportEntityRecordsFromCsv(entityName, postObject, controller: this);

			return DoResponse(response);
		}

		[AcceptVerbs(new[] { "GET" }, Route = "api/v3/en_US/quick-search")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult GetQuickSearch(string query = "", string entityName = "", string lookupFieldsCsv = "", string sortField = "", string sortType = "asc", string returnFieldsCsv = "",
				string matchMethod = "EQ", bool matchAllFields = false, int skipRecords = 0, int limitRecords = 5, string findType = "records", string forceFiltersCsv = "")
		{
			//forceFiltersCsv -> should be in the format "fieldName1:dataType1:eqValue1,fieldName2:dataType2:eqValue2"
			var response = new ResponseModel();
			var responseObject = new EntityRecord();
			try
			{
				if (String.IsNullOrWhiteSpace(entityName) || String.IsNullOrWhiteSpace(lookupFieldsCsv) || String.IsNullOrWhiteSpace(query) || String.IsNullOrWhiteSpace(returnFieldsCsv))
				{
					throw new Exception("missing params. All params are required");
				}

				var lookupFieldsList = new List<string>();
				foreach (var field in lookupFieldsCsv.Split(','))
				{
					lookupFieldsList.Add(field);
				}

				QueryObject matchesFilter = null;
				#region <<Generate filters >>
				switch (matchMethod.ToLowerInvariant())
				{
					case "contains":
						if (lookupFieldsList.Count > 1)
						{
							var filterList = new List<QueryObject>();
							foreach (var field in lookupFieldsList)
							{
								filterList.Add(EntityQuery.QueryContains(field, query));
							}
							if (matchAllFields)
							{
								matchesFilter = EntityQuery.QueryAND(filterList.ToArray());
							}
							else
							{
								matchesFilter = EntityQuery.QueryOR(filterList.ToArray());
							}

						}
						else
						{
							matchesFilter = EntityQuery.QueryContains(lookupFieldsList[0], query);
						}
						break;
					case "startswith":
						if (lookupFieldsList.Count > 1)
						{
							var filterList = new List<QueryObject>();
							foreach (var field in lookupFieldsList)
							{
								filterList.Add(EntityQuery.QueryStartsWith(field, query));
							}
							if (matchAllFields)
							{
								matchesFilter = EntityQuery.QueryAND(filterList.ToArray());
							}
							else
							{
								matchesFilter = EntityQuery.QueryOR(filterList.ToArray());
							}

						}
						else
						{
							matchesFilter = EntityQuery.QueryStartsWith(lookupFieldsList[0], query);
						}
						break;
					case "fts":
						if (lookupFieldsList.Count > 1)
						{
							var filterList = new List<QueryObject>();
							foreach (var field in lookupFieldsList)
							{
								filterList.Add(EntityQuery.QueryFTS(field, query));
							}
							if (matchAllFields)
							{
								matchesFilter = EntityQuery.QueryAND(filterList.ToArray());
							}
							else
							{
								matchesFilter = EntityQuery.QueryOR(filterList.ToArray());
							}

						}
						else
						{
							matchesFilter = EntityQuery.QueryFTS(lookupFieldsList[0], query);
						}
						break;
					default: // EQ
						if (lookupFieldsList.Count > 1)
						{
							var filterList = new List<QueryObject>();
							foreach (var field in lookupFieldsList)
							{
								filterList.Add(EntityQuery.QueryEQ(field, query));
							}
							if (matchAllFields)
							{
								matchesFilter = EntityQuery.QueryAND(filterList.ToArray());
							}
							else
							{
								matchesFilter = EntityQuery.QueryOR(filterList.ToArray());
							}

						}
						else
						{
							matchesFilter = EntityQuery.QueryEQ(lookupFieldsList[0], query);
						}
						break;

				}
				#endregion

				#region << Generate force filters >>
				var forceFilters = new List<QueryObject>();
				if (!String.IsNullOrWhiteSpace(forceFiltersCsv))
				{
					foreach (var forceFilter in forceFiltersCsv.Split(','))
					{
						var filterArray = forceFilter.Split(':');
						if (filterArray.Length == 3)
						{
							switch (filterArray[1].ToLowerInvariant())
							{
								case "guid":
									var filterValueGuid = new Guid(filterArray[2]);
									forceFilters.Add(EntityQuery.QueryEQ(filterArray[0], filterValueGuid));
									break;
								case "bool":
									if (filterArray[2] == "true")
									{
										forceFilters.Add(EntityQuery.QueryEQ(filterArray[0], true));
									}
									else
									{
										forceFilters.Add(EntityQuery.QueryEQ(filterArray[0], false));
									}
									break;
								case "datetime":
									var filterValueDate = Convert.ToDateTime(filterArray[2]);
									forceFilters.Add(EntityQuery.QueryEQ(filterArray[0], filterValueDate));
									break;
								case "int":
									var filterValueInt = Convert.ToInt64(filterArray[2]);
									forceFilters.Add(EntityQuery.QueryEQ(filterArray[0], filterValueInt));
									break;
								case "string":
									forceFilters.Add(EntityQuery.QueryEQ(filterArray[0], filterArray[2]));
									break;
								default:
									break;

							}
						}
					}

				}

				if (forceFilters.Count > 0)
				{
					var forceFilterQuery = EntityQuery.QueryAND(forceFilters.ToArray());
					matchesFilter = EntityQuery.QueryAND(forceFilterQuery, matchesFilter);
				}

				#endregion


				var sortsList = new List<QuerySortObject>();
				#region << Generate Sorts >>
				if (!String.IsNullOrWhiteSpace(sortField))
				{
					if (sortType.ToLowerInvariant() == "desc")
					{
						sortsList.Add(new QuerySortObject(sortField, QuerySortType.Descending));
					}
					else
					{
						sortsList.Add(new QuerySortObject(sortField, QuerySortType.Ascending));
					}
				}

				#endregion

				if (findType.ToLowerInvariant() == "records" || findType.ToLowerInvariant() == "records-and-count" || findType.ToLowerInvariant() == "records&count")
				{
					var matchQueryResponse = recMan.Find(new EntityQuery(entityName, returnFieldsCsv, matchesFilter, sortsList.ToArray(), skipRecords, limitRecords));
					if (!matchQueryResponse.Success)
					{
						throw new Exception(matchQueryResponse.Message);
					}
					responseObject["records"] = matchQueryResponse.Object.Data;
				}

				if (findType.ToLowerInvariant() == "count" || findType.ToLowerInvariant() == "records-and-count" || findType.ToLowerInvariant() == "records&count")
				{
					var matchQueryResponse = recMan.Count(new EntityQuery(entityName, returnFieldsCsv, matchesFilter));
					if (!matchQueryResponse.Success)
					{
						throw new Exception(matchQueryResponse.Message);
					}
					responseObject["count"] = matchQueryResponse.Object;
				}



				response.Success = true;
				response.Message = "Quick search success";
				response.Object = responseObject;
				return Json(response);
			}
			catch (Exception ex)
			{
				new LogService().Create(Diagnostics.LogType.Error, "TErpApi:GetQuickSearch", ex);
				response.Success = false;
				response.Message = ex.Message;
				response.Object = null;
				return Json(response);
			}
		}

		#endregion

		#region << Files >>

		[HttpGet]
		[Route("/fs/{fileName}")]
		[Route("/fs/{root}/{fileName}")]
		[Route("/fs/{root}/{root2}/{fileName}")]
		[Route("/fs/{root}/{root2}/{root3}/{fileName}")]
		[Route("/fs/{root}/{root2}/{root3}/{root4}/{fileName}")]
		public IActionResult Download([FromRoute] string root, [FromRoute] string root2, [FromRoute] string root3, [FromRoute] string root4, [FromRoute] string fileName)
		{
			//we added ROOT routing parameter as workaround for conflict with razorpages routing and wildcard controller routing
			//in particular we have problem with ApplicationNodePage where routing pattern is  "/{AppName}/{AreaName}/{NodeName}/a/{PageName?}"

			if (string.IsNullOrWhiteSpace(fileName))
				return DoPageNotFoundResponse();

			var filePathArray = new List<string>();
			if (root != null) filePathArray.Add(root);
			if (root2 != null) filePathArray.Add(root2);
			if (root3 != null) filePathArray.Add(root3);
			if (root4 != null) filePathArray.Add(root4);

			var filePath = "/" + String.Join("/", filePathArray) + "/" + fileName;

			filePath = filePath.ToLowerInvariant();

			DbFileRepository fsRepository = new DbFileRepository();
			var file = fsRepository.Find(filePath);

			if (file == null)
			{

				if (ErpSettings.DevelopmentMode)
				{
					//Hardcoded image for development
					WebClient wc = new WebClient();
					byte[] bytes = wc.DownloadData($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/_content/WebVella.Erp.Web/assets/missing-image.png");

					return File(bytes, "image/png");
				}
				else
					return DoPageNotFoundResponse();
			}
			//check for modification
			string headerModifiedSince = Request.Headers["If-Modified-Since"];
			if (headerModifiedSince != null)
			{
				if (DateTime.TryParse(headerModifiedSince, out DateTime isModifiedSince))
				{
					if (isModifiedSince <= file.LastModificationDate)
					{
						Response.StatusCode = 304;
						return new EmptyResult();
					}
				}
			}

			HttpContext.Response.Headers.Add("last-modified", file.LastModificationDate.ToString());
			const int durationInSeconds = 60 * 60 * 24 * 30; //30 days caching of these resources
			HttpContext.Response.Headers[HeaderNames.CacheControl] = "public,max-age=" + durationInSeconds;

			var extension = Path.GetExtension(filePath).ToLowerInvariant();
			new FileExtensionContentTypeProvider().Mappings.TryGetValue(extension, out string mimeType);


			IDictionary<string, StringValues> queryCollection = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(HttpContext.Request.QueryString.ToString());
			string action = queryCollection.Keys.Any(x => x == "action") ? ((string)queryCollection["action"]).ToLowerInvariant() : "";
			string requestedMode = queryCollection.Keys.Any(x => x == "mode") ? ((string)queryCollection["mode"]).ToLowerInvariant() : "";
			string width = queryCollection.Keys.Any(x => x == "width") ? ((string)queryCollection["width"]).ToLowerInvariant() : "";
			string height = queryCollection.Keys.Any(x => x == "height") ? ((string)queryCollection["height"]).ToLowerInvariant() : "";
			bool isImage = extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif";

			int widthInt = 0;
			if (!String.IsNullOrWhiteSpace(width) && int.TryParse(width, out int outWidthInt))
			{
				widthInt = outWidthInt;
			}
			int heightInt = 0;
			if (!String.IsNullOrWhiteSpace(height) && int.TryParse(height, out int outHeightInt))
			{
				heightInt = outHeightInt;
			}

			if (isImage && (widthInt > 0 || heightInt > 0))
			{
				if (string.IsNullOrWhiteSpace(action))
					action = "resize";

				var fileContent = file.GetBytes();
				using (Image<Rgba32> image = SixLabors.ImageSharp.Image.Load(fileContent))
				{
					switch (mimeType.ToLowerInvariant())
					{
						case "image/gif":
						case "image/png":
							image.Mutate(x => x.BackgroundColor(Rgba32.White));
							break;
						default:
							break;
					}

					switch (action)
					{
						default:
						case "resize":
							{
								Size size = ParseSize(queryCollection);
								MemoryStream outStream = new MemoryStream();

								ResizeMode mode;
								switch (requestedMode)
								{
									case "boxpad":
										mode = ResizeMode.BoxPad;
										break;
									case "crop":
										mode = ResizeMode.Crop;
										break;
									case "min":
										mode = ResizeMode.Min;
										break;
									case "max":
										mode = ResizeMode.Max;
										break;
									case "stretch":
										mode = ResizeMode.Stretch;
										break;
									default:
										mode = ResizeMode.Pad;
										break;
								}

								var resizeOptions = new ResizeOptions
								{
									Mode = mode,
									Size = new SixLabors.Primitives.Size(size.Width, size.Height)
								};
								image.Mutate(x => x.Resize(resizeOptions).BackgroundColor(Rgba32.White));
								image.SaveAsJpeg(outStream);
								outStream.Seek(0, SeekOrigin.Begin);
								return File(outStream, mimeType);
							}
					}
				}
			}

			return File(file.GetBytes(), mimeType);
		}

		/// <summary>
		/// Parse width and height parameters from query string
		/// </summary>
		/// <param name="queryCollection"></param>
		/// <returns></returns>
		private Size ParseSize(IDictionary<string, StringValues> queryCollection)
		{
			string width = queryCollection.Keys.Any(x => x == "width") ? (string)queryCollection["width"] : "";
			string height = queryCollection.Keys.Any(x => x == "height") ? (string)queryCollection["height"] : "";
			Size size = new Size();

			// First cater for single dimensions.
			if (width != "" && height == "")
			{

				width = width.Replace("px", string.Empty);
				size = new Size(Int32.Parse(width), 0);
			}

			if (width == "" && height != "")
			{
				height = height.Replace("px", string.Empty);
				size = new Size(0, Int32.Parse(height));
			}

			// Both supplied
			if (width != "" && height != "")
			{
				width = width.Replace("px", string.Empty);
				height = height.Replace("px", string.Empty);
				size = new Size(Int32.Parse(width), Int32.Parse(height));
			}

			return size;
		}

		[AcceptVerbs(new[] { "POST" }, Route = "/fs/upload/")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult UploadFile([FromForm] IFormFile file)
		{
			//var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"').ToLowerInvariant();
			//Trim('"') was removed from Core2
			var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim().ToLowerInvariant();
			if (fileName.StartsWith("\"", StringComparison.InvariantCulture))
				fileName = fileName.Substring(1);

			if (fileName.EndsWith("\"", StringComparison.InvariantCulture))
				fileName = fileName.Substring(0, fileName.Length - 1);

			DbFileRepository fsRepository = new DbFileRepository();
			var createdFile = fsRepository.CreateTempFile(fileName, ReadFully(file.OpenReadStream()));

			return DoResponse(new FSResponse(new FSResult { Url = createdFile.FilePath, Filename = fileName }));

		}

		[AcceptVerbs(new[] { "POST" }, Route = "/fs/move/")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult MoveFile([FromBody]JObject submitObj)
		{
			string source = submitObj["source"].Value<string>();
			string target = submitObj["target"].Value<string>();
			bool overwrite = false;
			if (submitObj["overwrite"] != null)
				overwrite = submitObj["overwrite"].Value<bool>();

			source = source.ToLowerInvariant();
			target = target.ToLowerInvariant();

			var fileName = target.Split(new char[] { '/' }).LastOrDefault();

			DbFileRepository fsRepository = new DbFileRepository();
			var sourceFile = fsRepository.Find(source);

			var movedFile = fsRepository.Move(source, target, overwrite);
			return DoResponse(new FSResponse(new FSResult { Url = movedFile.FilePath, Filename = fileName }));

		}

		[AcceptVerbs(new[] { "DELETE" }, Route = "{*filepath}")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult DeleteFile([FromRoute] string filepath)
		{
			filepath = filepath.ToLowerInvariant();

			var fileName = filepath.Split(new char[] { '/' }).LastOrDefault();

			DbFileRepository fsRepository = new DbFileRepository();
			var sourceFile = fsRepository.Find(filepath);

			fsRepository.Delete(filepath);
			return DoResponse(new FSResponse(new FSResult { Url = filepath, Filename = fileName }));
		}

		private static byte[] ReadFully(Stream input)
		{
			byte[] buffer = new byte[16 * 1024];
			using (MemoryStream ms = new MemoryStream())
			{
				int read;
				while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
				{
					ms.Write(buffer, 0, read);
				}
				return ms.ToArray();
			}
		}

		#endregion

		#region << Plugins >>
		[Authorize(Roles = "administrator")]
		[AcceptVerbs(new[] { "GET" }, Route = "api/v3/en_US/plugin/list")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult GetPlugins()
		{
			var responseObj = new ResponseModel
			{
				Object = erpService.Plugins,
				Success = true,
				Timestamp = DateTime.UtcNow
			};
			return DoResponse(responseObj);
		}
		#endregion

		#region << Jobs >>

		[Authorize(Roles = "administrator")]
		[AcceptVerbs(new[] { "GET" }, Route = "api/v3/en_US/jobs")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult GetJobs(DateTime? startFromDate = null, DateTime? startToDate = null, DateTime? finishedFromDate = null,
			DateTime? finishedToDate = null, string typeName = null, int? status = null, int? priority = null, Guid? schedulePlanId = null, int? page = null, int? pageSize = null)
		{
			ResponseModel response = new ResponseModel { Timestamp = DateTime.UtcNow, Success = true, Errors = new List<ErrorModel>() };

			try
			{
				int totalCount;
				response.Object = JobManager.Current.GetJobs(out totalCount, startFromDate, startToDate, finishedFromDate, finishedToDate,
					typeName, status, priority, schedulePlanId, page, pageSize);
			}
			catch (Exception e)
			{
				new LogService().Create(Diagnostics.LogType.Error, "ErpApi:GetJobs", e);
				response.Success = false;
				response.Message = e.Message + e.StackTrace;
			}

			return DoResponse(response);
		}



		#endregion

		#region << SchedulePlans >>

		[Authorize(Roles = "administrator")]
		[AcceptVerbs(new[] { "PUT" }, Route = "api/v3/en_US/scheduleplan/{planId}")]
		public IActionResult UpdateSchedulePlan(Guid planId, [FromBody]JObject postObject)
		{
			ResponseModel response = new ResponseModel { Timestamp = DateTime.UtcNow, Success = true, Errors = new List<ErrorModel>() };

			try
			{
				SchedulePlan schedulePlan = ScheduleManager.Current.GetSchedulePlan(planId);

				if (schedulePlan == null)
				{
					response.Errors.Add(new ErrorModel { Message = $"Schedule plan with such id was not found. Id[{planId}]." });
					response.Success = false;
					return DoResponse(response);
				}

				if (postObject.IsNullOrEmpty())
				{
					response.Errors.Add(new ErrorModel { Message = $"Schedule plan with such id was not found. Id[{planId}]." });
					response.Success = false;
					return DoResponse(response);
				}

				#region << Validate >>

				foreach (var prop in postObject.Properties())
				{
					switch (prop.Name)
					{
						case "name":
							{
								if (!string.IsNullOrWhiteSpace((string)postObject["name"]))
								{
									schedulePlan.Name = (string)postObject["name"];
								}
								else
								{
									response.Errors.Add(new ErrorModel("name", (string)postObject["name"], "Name is required field and cannot be empty."));
								}
							}
							break;
						case "type":
							{
								if (!string.IsNullOrWhiteSpace(postObject["type"].ToString()))
								{
									if (int.TryParse(postObject["type"].ToString(), out int type))
									{
										if (type >= 1 && type <= 4)
											schedulePlan.Type = (SchedulePlanType)type;
										else
											response.Errors.Add(new ErrorModel("type", postObject["type"].ToString(), "The value of the type is out of range of valid values."));
									}
									else
										response.Errors.Add(new ErrorModel("type", postObject["type"].ToString(), "Type is invalid integer value."));
								}
								else
								{
									response.Errors.Add(new ErrorModel("type", postObject["type"].ToString(), "Type is required field and cannot be empty."));
								}
							}
							break;
						case "job_type_id":
							{
								if (Guid.TryParse(postObject["job_type_id"].ToString(), out Guid jobTypeId))
								{
									if (JobManager.JobTypes.Any(t => t.Id == jobTypeId))
									{
										schedulePlan.JobTypeId = jobTypeId;
									}
									else
									{
										response.Errors.Add(new ErrorModel("job_type_id", postObject["job_type_id"].ToString(), "There is no job type with such id."));
									}
								}
								else
								{
									response.Errors.Add(new ErrorModel("job_type_id", postObject["job_type_id"].ToString(), "Job type id is not valid."));
								}
							}
							break;
						case "start_date":
							{
								schedulePlan.StartDate = DateTime.UtcNow;

								if (!string.IsNullOrWhiteSpace(postObject["start_date"].ToString()))
								{
									if (DateTime.TryParse(postObject["start_date"].ToString(), out DateTime startDate))
									{
										startDate = (DateTime)postObject["start_date"];
										schedulePlan.StartDate = startDate.ToUniversalTime();
									}
									else
									{
										response.Errors.Add(new ErrorModel("start_date", postObject["start_date"].ToString(), "The value of start date field is not valid."));
									}
								}
							}
							break;
						case "end_date":
							{
								if (!string.IsNullOrWhiteSpace(postObject["end_date"].ToString()))
								{
									if (DateTime.TryParse(postObject["end_date"].ToString(), out DateTime endDate))
									{
										endDate = (DateTime)postObject["end_date"];
										schedulePlan.StartDate = endDate.ToUniversalTime();
									}
									else
									{
										response.Errors.Add(new ErrorModel("end_date", postObject["end_date"].ToString(), "The value of end date field is not valid."));
									}
								}
							}
							break;
						case "schedule_days":
							{
								string days = postObject["schedule_days"].ToString();
								if (!string.IsNullOrWhiteSpace(days))
								{
									schedulePlan.ScheduledDays = JsonConvert.DeserializeObject<SchedulePlanDaysOfWeek>(postObject["schedule_days"].ToString());
								}
								else
								{
									response.Errors.Add(new ErrorModel("schedule_days", postObject["schedule_days"].ToString(), "Schedule days is required field and cannot be empty."));
								}
							}
							break;
						case "interval_in_minutes":
							{
								if (int.TryParse(postObject["interval_in_minutes"].ToString(), out int interval))
								{
									schedulePlan.IntervalInMinutes = interval;
								}
								else
								{
									response.Errors.Add(new ErrorModel("interval_in_minutes", postObject["interval_in_minutes"].ToString(), "The value of Interval in minutes field is not valid."));
								}
							}
							break;
						case "start_timespan":
							{
								if (DateTime.TryParse(postObject["start_timespan"].ToString(), out DateTime startTimespan))
								{
									startTimespan = ((DateTime)postObject["start_timespan"]);
									schedulePlan.StartTimespan = startTimespan.Hour * 60 + startTimespan.Minute;
								}
								else
								{
									response.Errors.Add(new ErrorModel("start_timespan", postObject["start_timespan"].ToString(), "The value of start timespan is not valid."));
								}
							}
							break;
						case "end_timespan":
							{
								if (DateTime.TryParse(postObject["end_timespan"].ToString(), out DateTime endTimespan))
								{
									endTimespan = ((DateTime)postObject["end_timespan"]);
									schedulePlan.EndTimespan = endTimespan.Hour * 60 + endTimespan.Minute;
									if (schedulePlan.EndTimespan == 0) //that's mean 12PM
										schedulePlan.EndTimespan = 1440;
								}
								else
								{
									response.Errors.Add(new ErrorModel("end_timespan", postObject["end_timespan"].ToString(), "The value of end timespan is not valid."));
								}
							}
							break;
						case "enabled":
							{
								schedulePlan.Enabled = (bool)postObject["enabled"];
							}
							break;
					}
				}

				if (schedulePlan.StartDate >= schedulePlan.EndDate)
				{
					if (postObject.Properties().Any(p => p.Name == "start_date"))
						response.Errors.Add(new ErrorModel("start_date", postObject["start_date"].ToString(), "Start date must be before end date."));
					else
						response.Errors.Add(new ErrorModel("end_date", postObject["end_date"].ToString(), "End date must be greater than start date."));
				}

				if ((schedulePlan.Type == SchedulePlanType.Daily || schedulePlan.Type == SchedulePlanType.Interval) && !schedulePlan.ScheduledDays.HasOneSelectedDay())
					response.Errors.Add(new ErrorModel("schedule_days", postObject["schedule_days"].ToString(), "At least one day have to be selected for schedule days field."));

				if (schedulePlan.Type == SchedulePlanType.Interval && schedulePlan.IntervalInMinutes <= 0 || schedulePlan.IntervalInMinutes >= 1440)
					response.Errors.Add(new ErrorModel("interval_in_minutes", postObject["interval_in_minutes"].ToString(), "The value of Interval in minutes field must be greater than 0 and less or  equal than 1440."));

				if (response.Errors.Count > 0)
				{
					response.Success = false;
					return DoResponse(response);
				}

				#endregion

				schedulePlan.NextTriggerTime = ScheduleManager.Current.FindSchedulePlanNextTriggerDate(schedulePlan);
				ScheduleManager.Current.UpdateSchedulePlan(schedulePlan);
			}
			catch (Exception e)
			{
				new LogService().Create(Diagnostics.LogType.Error, "TErpApi:UpdateSchedulePlan", e);
				response.Success = false;
				response.Timestamp = DateTime.UtcNow;
				response.Message = e.Message + e.StackTrace;
			}

			response.Success = true;
			response.Timestamp = DateTime.UtcNow;
			var responseRecord = new EntityRecord();
			var responseList = new List<SchedulePlan> {
				ScheduleManager.Current.GetSchedulePlan(planId)
			};
			responseRecord["data"] = responseList;
			response.Object = responseRecord;
			response.Message = "Schedule plan updated successfully";

			return DoResponse(response);
		}

		[Authorize(Roles = "administrator")]
		[AcceptVerbs(new[] { "POST" }, Route = "api/v3/en_US/scheduleplan/{planId}/trigger")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult TriggerNowSchedulePlan(Guid planId)
		{
			BaseResponseModel response = new BaseResponseModel { Timestamp = DateTime.UtcNow, Success = true, Errors = new List<ErrorModel>() };

			try
			{
				var schedulePlan = ScheduleManager.Current.GetSchedulePlan(planId);

				if (schedulePlan == null)
				{
					response.Errors.Add(new ErrorModel { Message = $"Schedule plan with such id was not found. Id[{planId}]." });
					response.Success = false;
					return DoResponse(response);
				}

				ScheduleManager.Current.TriggerNowSchedulePlan(schedulePlan);
			}
			catch (Exception e)
			{
				new LogService().Create(Diagnostics.LogType.Error, "TErpApi:TriggerNowSchedulePlan", e);
				response.Success = false;
				response.Message = e.Message + e.StackTrace;
			}

			response.Success = true;
			response.Timestamp = DateTime.UtcNow;
			response.Message = "Schedule plan triggered successfully";
			return DoResponse(response);
		}

		[Authorize(Roles = "administrator")]
		[AcceptVerbs(new[] { "GET" }, Route = "api/v3/en_US/scheduleplan/list")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult GetSchedulePlansList()
		{
			ResponseModel response = new ResponseModel { Timestamp = DateTime.UtcNow, Success = true, Errors = new List<ErrorModel>() };

			try
			{
				var responseRecord = new EntityRecord();
				responseRecord["data"] = ScheduleManager.Current.GetSchedulePlans().MapTo<OutputSchedulePlan>();
				response.Object = responseRecord;
			}
			catch (Exception e)
			{
				new LogService().Create(Diagnostics.LogType.Error, "TErpApi:GetSchedulePlansList", e);
				response.Success = false;
				response.Message = e.Message + e.StackTrace;
			}

			return DoResponse(response);
		}

		[Authorize(Roles = "administrator")]
		[AcceptVerbs(new[] { "GET" }, Route = "api/v3/en_US/scheduleplan/{planId}")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult GetSchedulePlan(Guid planId)
		{
			ResponseModel response = new ResponseModel { Timestamp = DateTime.UtcNow, Success = true, Errors = new List<ErrorModel>() };

			try
			{
				var schedulePlan = ScheduleManager.Current.GetSchedulePlan(planId);

				if (schedulePlan == null)
				{
					response.Errors.Add(new ErrorModel { Message = $"Schedule plan with such id was not found. Id[{planId}]." });
					response.Success = false;
					return DoResponse(response);
				}

				var responseRecord = new EntityRecord();
				responseRecord["data"] = schedulePlan.MapTo<OutputSchedulePlan>();
				response.Object = responseRecord;
			}
			catch (Exception e)
			{
				new LogService().Create(Diagnostics.LogType.Error, "TErpApi:GetSchedulePlan", e);
				response.Success = false;
				response.Message = e.Message + e.StackTrace;
			}

			return DoResponse(response);
		}

		[Authorize(Roles = "administrator")]
		[AcceptVerbs(new[] { "GET" }, Route = "api/v3/en_US/scheduleplan/test")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult CreateTestSchedulePlan(Guid planId)
		{
			ResponseModel response = new ResponseModel { Timestamp = DateTime.UtcNow, Success = true, Errors = new List<ErrorModel>() };

			try
			{
				Guid offerSchedulePlanId = Guid.NewGuid();
				SchedulePlan offerSchedulePlan = ScheduleManager.Current.GetSchedulePlan(offerSchedulePlanId);

				if (offerSchedulePlan == null)
				{
					offerSchedulePlan = new SchedulePlan
					{
						Id = offerSchedulePlanId,
						Name = "Offer schedule plan Test",
						Type = SchedulePlanType.Daily,
						StartDate = DateTime.UtcNow,
						EndDate = null,
						ScheduledDays = new SchedulePlanDaysOfWeek()
						{
							ScheduledOnMonday = true,
							ScheduledOnTuesday = true,
							ScheduledOnWednesday = true,
							ScheduledOnThursday = true,
							ScheduledOnFriday = true,
							ScheduledOnSaturday = true,
							ScheduledOnSunday = true
						},
						//IntervalInMinutes = 1,
						//StartTimespan = 0,
						//EndTimespan = 1440,
						JobTypeId = new Guid("70f06b11-2aee-40d5-b8ef-de1a2d8bbb59"),
						JobAttributes = null,
						Enabled = true,
						LastModifiedBy = null
					};

					ScheduleManager.Current.CreateSchedulePlan(offerSchedulePlan);
				}
				response.Object = offerSchedulePlan.MapTo<OutputSchedulePlan>();
			}
			catch (Exception e)
			{
				new LogService().Create(Diagnostics.LogType.Error, "TErpApi:CreateTestSchedulePlan", e);
				response.Success = false;
				response.Message = e.Message + e.StackTrace;
			}

			return DoResponse(response);
		}

		#endregion

		#region << System log >>
		[Authorize(Roles = "administrator")]
		[AcceptVerbs(new[] { "GET" }, Route = "api/v3/en_US/system-log")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult GetSystemLog(DateTime? fromDate = null, DateTime? untilDate = null, string type = "",
			string source = "", string message = "", string notificationStatus = "", int page = 1, int pageSize = 15)
		{
			ResponseModel response = new ResponseModel { Timestamp = DateTime.UtcNow, Success = true, Errors = new List<ErrorModel>() };
			var recMan = new RecordManager();
			var skipRecords = (page - 1) * pageSize;
			try
			{
				//Filters
				var filterList = new List<QueryObject>();
				if (fromDate != null)
				{
					filterList.Add(EntityQuery.QueryGT("created_on", fromDate));
				}
				if (untilDate != null)
				{
					filterList.Add(EntityQuery.QueryLT("created_on", untilDate));
				}
				if (!String.IsNullOrWhiteSpace(type))
				{
					filterList.Add(EntityQuery.QueryEQ("type", type));
				}
				if (!String.IsNullOrWhiteSpace(source))
				{
					filterList.Add(EntityQuery.QueryContains("source", source));
				}
				if (!String.IsNullOrWhiteSpace(message))
				{
					filterList.Add(EntityQuery.QueryContains("message", message));
				}
				if (!String.IsNullOrWhiteSpace(notificationStatus))
				{
					filterList.Add(EntityQuery.QueryEQ("notificationStatus", notificationStatus));
				}

				var selectFilters = EntityQuery.QueryAND(filterList.ToArray());

				//Sort
				var sortList = new List<QuerySortObject> {
					new QuerySortObject("created_on", QuerySortType.Descending)
				};

				//Fields
				var columns = "*";

				//Query
				var query = new EntityQuery("system_log", columns, selectFilters, sortList.ToArray(), skipRecords, pageSize);
				var queryResponse = recMan.Find(query);
				if (!queryResponse.Success)
				{
					throw new Exception("Error getting the records: " + queryResponse.Message);
				}
				response.Object = queryResponse.Object.Data;
			}
			catch (Exception e)
			{
				new LogService().Create(Diagnostics.LogType.Error, "TErpApi:GetSystemLog", e);
				response.Success = false;
				response.Message = e.Message + e.StackTrace;
			}

			return DoResponse(response);
		}
		#endregion

		#region << UserFile >>

		[AcceptVerbs(new[] { "GET" }, Route = "api/v3/en_US/user_file")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult GetUserFileList(string type = "", string search = "", int sort = 1, int page = 1, int pageSize = 30)
		{
			ResponseModel response = new ResponseModel { Timestamp = DateTime.UtcNow, Success = true, Errors = new List<ErrorModel>() };

			try
			{
				response.Object = new UserFileService().GetFilesList(type, search, sort, page, pageSize);
			}
			catch (Exception e)
			{
				new LogService().Create(Diagnostics.LogType.Error, "TErpApi:GetUserFileList", e);
				response.Success = false;
				response.Message = e.Message + e.StackTrace;
			}

			return DoResponse(response);
		}

		[AcceptVerbs(new[] { "POST" }, Route = "api/v3/en_US/user_file")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult UploadUserFile([FromBody]JObject submitObj)
		{
			ResponseModel response = new ResponseModel { Timestamp = DateTime.UtcNow, Success = true, Errors = new List<ErrorModel>() };
			var filePath = "";
			var fileAlt = "";
			var fileCaption = "";
			#region << Init SubmitObj >>
			foreach (var prop in submitObj.Properties())
			{
				switch (prop.Name.ToLower())
				{
					case "path":
						if (!string.IsNullOrWhiteSpace(prop.Value.ToString()))
							filePath = prop.Value.ToString();
						else
						{
							throw new Exception("File path is required");
						}
						break;
					case "alt":
						if (!string.IsNullOrWhiteSpace(prop.Value.ToString()))
							fileAlt = prop.Value.ToString();
						else
						{
							fileAlt = null;
						}
						break;
					case "caption":
						if (!string.IsNullOrWhiteSpace(prop.Value.ToString()))
							fileCaption = prop.Value.ToString();
						else
						{
							fileCaption = null;
						}
						break;
				}
			}

			#endregion
			try
			{
				response.Object = new UserFileService().CreateUserFile(filePath, fileAlt, fileCaption);
			}
			catch (Exception e)
			{
				new LogService().Create(Diagnostics.LogType.Error, "TErpApi:UploadUserFile", e);
				response.Success = false;
				response.Message = e.Message + e.StackTrace;
			}

			return DoResponse(response);
		}


		[AcceptVerbs(new[] { "POST" }, Route = "/ckeditor/drop-upload-url")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult UploadDropCKEditor(IFormFile upload)
		{
			var response = new EntityRecord();
			byte[] fileBytes = null;
			try
			{
				if (upload != null)
				{
					using (var ms = new MemoryStream())
					{
						upload.CopyTo(ms);
						fileBytes = ms.ToArray();
					}
					var tempPath = "tmp/" + Guid.NewGuid() + "/" + upload.FileName;
					var tempFile = new DbFileRepository().Create(tempPath, fileBytes, null, null);

					var newFile = new UserFileService().CreateUserFile(tempFile.FilePath, null, null);

					string url = "/fs" + newFile.Path;

					response["uploaded"] = 1;
					response["fileName"] = upload.FileName;
					response["url"] = url;
					return Json(response);

				}
				else
				{
					return Json(response);
				}
			}
			catch (Exception ex)
			{
				new LogService().Create(Diagnostics.LogType.Error, "TErpApi:UploadDropCKEditor", ex);
				response["uploaded"] = 0;
				response["error"] = new EntityRecord();
				var message = new EntityRecord();
				message["message"] = ex.Message;
				response["error"] = message;
				return Json(response);
			}

		}


		[AcceptVerbs(new[] { "POST" }, Route = "/ckeditor/image-upload-url")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult UploadFileManagerCKEditor(IFormFile upload)
		{
			byte[] fileBytes = null;
			string CKEditorFuncNum = HttpContext.Request.Query["CKEditorFuncNum"].ToString();
			try
			{
				using (var ms = new MemoryStream())
				{
					upload.CopyTo(ms);
					fileBytes = ms.ToArray();
				}
				var tempPath = "tmp/" + Guid.NewGuid() + "/" + upload.FileName;
				var tempFile = new DbFileRepository().Create(tempPath, fileBytes, null, null);

				var newFile = new UserFileService().CreateUserFile(tempFile.FilePath, null, null);

				string url = "/fs" + newFile.Path;
				string vMessage = "";
				var vOutput = @"<html><body><script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + url + "\", \"" + vMessage + "\");</script></body></html>";

				return Content(vOutput, "text/html");
			}
			catch (Exception ex)
			{
				new LogService().Create(Diagnostics.LogType.Error, "TErpApi:UploadFileManagerCKEditor", ex);
				var vOutput = @"<html><body><script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"\", \"" + ex.Message + "\");</script></body></html>";
				return Content(vOutput, "text/html");
			}
		}

		[AcceptVerbs(new[] { "POST" }, Route = "/fs/upload-user-file-multiple/")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult UploadUserFileMultiple([FromForm] List<IFormFile> files)
		{

			var resultRecords = new List<EntityRecord>();
			var response = new ResponseModel { Timestamp = DateTime.UtcNow, Success = true, Errors = new List<ErrorModel>() };

			using (var connection = DbContext.Current.CreateConnection())
			{
				connection.BeginTransaction();

				try
				{

					var currentUser = AuthService.GetUser(User);

					foreach (var file in files)
					{
						var fileBuffer = ReadFully(file.OpenReadStream());
						var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim().ToLowerInvariant();
						if (fileName.StartsWith("\"", StringComparison.InvariantCulture))
							fileName = fileName.Substring(1);

						if (fileName.EndsWith("\"", StringComparison.InvariantCulture))
							fileName = fileName.Substring(0, fileName.Length - 1);

						var recMan = new RecordManager();
						DbFileRepository fsRepository = new DbFileRepository();
						string section = Guid.NewGuid().ToString().Replace("-", "").ToLowerInvariant();
						var filePath = "/user_file/" + currentUser.Id + "/" + section + "/" + fileName;
						var createdFile = fsRepository.Create(filePath, fileBuffer, DateTime.Now, currentUser.Id);
						var userFileId = Guid.NewGuid();

						var userFileRecord = new EntityRecord();
						#region << record fill >>
						userFileRecord["id"] = userFileId;
						userFileRecord["created_on"] = DateTime.Now;
						userFileRecord["name"] = fileName;
						userFileRecord["size"] = Math.Round((decimal)(file.Length / 1024), 0);
						userFileRecord["path"] = filePath;

						var mimeType = MimeMapping.MimeUtility.GetMimeMapping(filePath);
						var fileExtension = Path.GetExtension(filePath);
						if (mimeType.StartsWith("image"))
						{
							var dimensionsRecord = Helpers.GetImageDimension(fileBuffer);
							userFileRecord["width"] = (decimal)dimensionsRecord["width"];
							userFileRecord["height"] = (decimal)dimensionsRecord["height"];
							userFileRecord["type"] = "image";
						}
						else if (mimeType.StartsWith("video"))
						{
							userFileRecord["type"] = "video";
						}
						else if (mimeType.StartsWith("audio"))
						{
							userFileRecord["type"] = "audio";
						}
						else if (fileExtension == ".doc" || fileExtension == ".docx" || fileExtension == ".odt" || fileExtension == ".rtf"
						 || fileExtension == ".txt" || fileExtension == ".pdf" || fileExtension == ".html" || fileExtension == ".htm" || fileExtension == ".ppt"
						  || fileExtension == ".pptx" || fileExtension == ".xls" || fileExtension == ".xlsx" || fileExtension == ".ods" || fileExtension == ".odp")
						{
							userFileRecord["type"] = "document";
						}
						else
						{
							userFileRecord["type"] = "other";
						}
						#endregion

						var recordCreateResult = recMan.CreateRecord("user_file", userFileRecord);
						if (!recordCreateResult.Success)
						{
							throw new Exception(recordCreateResult.Message);
						}
						resultRecords.Add(userFileRecord);
					}
					connection.CommitTransaction();
					response.Success = true;
					response.Object = resultRecords;
					return DoResponse(response);
				}
				catch (Exception ex)
				{
					connection.RollbackTransaction();
					response.Success = false;
					response.Message = ex.Message;
					return DoResponse(response);
				}
			}
		}

		[AcceptVerbs(new[] { "POST" }, Route = "/fs/upload-file-multiple/")]
		[ResponseCache(NoStore = true, Duration = 0)]
		public IActionResult UploadFileMultiple([FromForm] List<IFormFile> files)
		{

			var resultRecords = new List<EntityRecord>();
			var response = new ResponseModel { Timestamp = DateTime.UtcNow, Success = true, Errors = new List<ErrorModel>() };

			using (var connection = DbContext.Current.CreateConnection())
			{
				connection.BeginTransaction();

				try
				{
					foreach (var file in files)
					{
						var fileBuffer = ReadFully(file.OpenReadStream());
						var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim().ToLowerInvariant();
						if (fileName.StartsWith("\"", StringComparison.InvariantCulture))
							fileName = fileName.Substring(1);

						if (fileName.EndsWith("\"", StringComparison.InvariantCulture))
							fileName = fileName.Substring(0, fileName.Length - 1);

						var recMan = new RecordManager();
						DbFileRepository fsRepository = new DbFileRepository();
						DbFile dbFile = fsRepository.CreateTempFile(fileName, fileBuffer);

						var resultRec = new EntityRecord();

						resultRec["id"] = dbFile.Id;
						resultRec["created_on"] = DateTime.Now;
						resultRec["name"] = fileName;
						resultRec["size"] = Math.Round((decimal)(file.Length / 1024), 0);
						resultRec["path"] = dbFile.FilePath;

						var mimeType = MimeMapping.MimeUtility.GetMimeMapping(dbFile.FilePath);
						var fileExtension = Path.GetExtension(dbFile.FilePath);
						if (mimeType.StartsWith("image"))
						{
							var dimensionsRecord = Helpers.GetImageDimension(fileBuffer);
							resultRec["width"] = (decimal)dimensionsRecord["width"];
							resultRec["height"] = (decimal)dimensionsRecord["height"];
							resultRec["type"] = "image";
						}
						else if (mimeType.StartsWith("video"))
						{
							resultRec["type"] = "video";
						}
						else if (mimeType.StartsWith("audio"))
						{
							resultRec["type"] = "audio";
						}
						else if (fileExtension == ".doc" || fileExtension == ".docx" || fileExtension == ".odt" || fileExtension == ".rtf"
						 || fileExtension == ".txt" || fileExtension == ".pdf" || fileExtension == ".html" || fileExtension == ".htm" || fileExtension == ".ppt"
						  || fileExtension == ".pptx" || fileExtension == ".xls" || fileExtension == ".xlsx" || fileExtension == ".ods" || fileExtension == ".odp")
						{
							resultRec["type"] = "document";
						}
						else
						{
							resultRec["type"] = "other";
						}

						resultRecords.Add(resultRec);
					}

					connection.CommitTransaction();
					response.Success = true;
					response.Object = resultRecords;
					return DoResponse(response);
				}
				catch (Exception ex)
				{
					connection.RollbackTransaction();
					response.Success = false;
					response.Message = ex.Message;
					return DoResponse(response);
				}
			}
		}


		#endregion

		#region << Utils >>

		public static Stream GenerateStreamFromString(string s)
		{
			var stream = new MemoryStream();
			var writer = new StreamWriter(stream);
			writer.Write(s);
			writer.Flush();
			stream.Position = 0;
			return stream;
		}
		#endregion

		#region <== Snippets ===>

		[AcceptVerbs(new[] { "GET" }, Route = "api/v3/en_US/snippets")]
		public IActionResult GetSnippetNames(string search = "", int page = 1, int pageSize = 30)
		{
			var response = new TypeaheadResponse();
			var snippets = SnippetService.Snippets.Keys.OrderBy(x => x).ToList();
			if (string.IsNullOrWhiteSpace(search))
				return new JsonResult(snippets.Skip(page - 1).Take(pageSize).ToList());
			else
				return new JsonResult(snippets.Where(x => x.ToLowerInvariant().Contains(search.ToLowerInvariant())).Skip(page - 1).Take(pageSize).ToList());
		}

		[AcceptVerbs(new[] { "GET" }, Route = "api/v3/en_US/snippet")]
		public IActionResult GetSnippetText([FromQuery]string name)
		{
			ResponseModel response = new ResponseModel { Timestamp = DateTime.UtcNow, Success = true, Errors = new List<ErrorModel>() };

			try
			{
				var snippet = SnippetService.GetSnippet(name);
				if (snippet == null)
					throw new Exception($"Snippet '{name}' is not found.");
				else
					response.Object = snippet.GetText();
			}
			catch (Exception e)
			{
				new LogService().Create(Diagnostics.LogType.Error, "GetSnippetNames", e);
				response.Success = false;
				response.Message = e.Message + e.StackTrace;
			}

			return DoResponse(response);
		}

		#endregion
	}
}