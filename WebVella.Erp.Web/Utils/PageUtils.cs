using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Web.Utils
{
	public static class PageUtils
	{
		public static void GetListQueryParams(HttpContext httpContext, out int Pager, out string SortBy, out QuerySortType SortOrder,
							string prefix = "", string pageQueryString = "page", string sortByQueryString = "sortBy", string sortOrderQueryString = "sortOrder")
		{
			Pager = 1;
			SortBy = "";
			SortOrder = QuerySortType.Ascending;

			#region << Pager >>
			var pagerKey = prefix + pageQueryString;
			if (httpContext.Request.Query.ContainsKey(pagerKey) && !String.IsNullOrWhiteSpace(httpContext.Request.Query[pagerKey]))
			{
				if (Int32.TryParse(httpContext.Request.Query[pagerKey], out int outInt))
				{
					if (outInt > 0)
					{
						Pager = outInt;
					}
				}
			}
			#endregion

			#region << SortedBy >>
			var sortedByKey = prefix + sortByQueryString;
			if (httpContext.Request.Query.ContainsKey(sortedByKey) && !String.IsNullOrWhiteSpace(httpContext.Request.Query[sortedByKey]))
			{
				SortBy = ((string)httpContext.Request.Query[sortedByKey]).ToLowerInvariant();
			}
			#endregion

			#region << Pager >>
			var sortOrderKey = prefix + sortOrderQueryString;
			if (httpContext.Request.Query.ContainsKey(sortOrderKey) && !String.IsNullOrWhiteSpace(httpContext.Request.Query[sortOrderKey]))
			{
				var sortOrderValue = ((string)httpContext.Request.Query[sortOrderKey]).ToLowerInvariant();
				if (sortOrderValue == "desc")
				{
					SortOrder = QuerySortType.Descending;
				}
			}
			#endregion

		}

		public static string GetUrlQueryParamValue(HttpContext httpContext, string key)
		{
			var result = "";
			if (httpContext.Request.Query.ContainsKey(key) && !String.IsNullOrWhiteSpace(httpContext.Request.Query[key]))
			{
				result = ((string)httpContext.Request.Query[key]);
			}

			return result;
		}

		//METHOD GetComponentMeta ADDED TO PageComponentLibraryService
		//internal static PageComponentMeta GetComponentMetaFromLibrary(string componentName)
		//{
		//	var library = new PageComponentLibraryService().GetPageComponentsList();
		//	return library.FirstOrDefault(x => x.Name.ToLowerInvariant() == componentName.ToLowerInvariant());
		//}

		public static string GetCurrentUrl(HttpContext httpContext)
		{
			var currentPath = httpContext.Request.Path;
			var currentQueryString = HttpUtility.ParseQueryString(httpContext.Request.QueryString.ToString());
			if (httpContext.Request.Query.ContainsKey("returnUrl"))
			{
				currentQueryString.Remove("returnUrl");
			}
			if (String.IsNullOrWhiteSpace(currentQueryString.ToString()))
			{
				return currentPath;
			}
			else
			{
				return currentPath + "?" + currentQueryString.ToString();
			}
		}

		public static List<Filter> GetPageFiltersFromQuery(HttpContext httpContext)
		{
			var result = new List<Filter>();
			var urlQueryDict = httpContext.Request.Query;
			foreach (var queryKey in urlQueryDict.Keys)
			{
				if (queryKey.EndsWith("_v") && queryKey.Contains("q_"))
				{
					var filter = new Filter();
					//name
					var propertyNameMatches = Regex.Matches(queryKey, @"(?<=q_)[^}]*(?=_v)").Cast<Match>().Select(match => match.Value).Distinct().ToList();
					if (propertyNameMatches.Any())
					{
						filter.Name = propertyNameMatches.First().Replace("q_", "").Replace("_v", "").Trim();
					}
					//prefix
					var queryKeySplit = queryKey.Split("q_");
					filter.Prefix = queryKeySplit[0];
					//value
					filter.Value = (string)urlQueryDict[queryKey];
					if (String.IsNullOrWhiteSpace(filter.Value))
					{
						continue;
					}

					//value2
					var value2QueryKey = $"{filter.Prefix}q_{filter.Name}_v2";
					if (urlQueryDict.ContainsKey(value2QueryKey))
					{
						filter.Value2 = (string)urlQueryDict[value2QueryKey];
					}
					//type
					var typeQueryKey = $"{filter.Prefix}q_{filter.Name}_t";
					if (urlQueryDict.ContainsKey(typeQueryKey))
					{
						if (Enum.TryParse(urlQueryDict[typeQueryKey], out FilterType enumResult))
						{
							filter.Type = enumResult;
						}
					}
					result.Add(filter);
				}
			}


			return result;
		}

		public static string GenerateListPageDescription(HttpContext httpContext, string prefix = "", long totalCount = 0)
		{
			var returnHtml = "";

			if(httpContext == null)
				return "";

			var descriptionList = new List<string>();
			#region << Total Count >>
			var totalCountHtml = totalCount + " records";
			if (totalCount == 1)
			{
				totalCountHtml = totalCount + " record";
			}
			descriptionList.Add(totalCountHtml);
			#endregion

			#region << Sorted by >>
			var sortHtml = "<strong>sorted by</strong> ";
			if (totalCount == 1)
			{
				sortHtml = "<strong>sorted by</strong>";
			}

			if (httpContext.Request.Query.ContainsKey(prefix + "sortBy"))
			{
				var fieldDataName = httpContext.Request.Query[prefix + "sortBy"];
				sortHtml += fieldDataName;

				descriptionList.Add(sortHtml);
			}
			//else
			//{
			//	sortHtml = "not sorted";
			//	if (totalCount == 1)
			//	{
			//		sortHtml = "not sorted";
			//	}
			//	descriptionList.Add(sortHtml);
			//}
			#endregion

			#region << Filtered by >>
			var filters = GetPageFiltersFromQuery(httpContext);
			var filterHtml = "";
			if (filters.Count > 0)
			{
				filterHtml = "<strong>filtered by</strong> " + String.Join(',', filters.Select(x => x.Name).ToList());
				descriptionList.Add(filterHtml);
			}
			//if (String.IsNullOrWhiteSpace(filterHtml))
			//{
			//	filterHtml = "no filter";
			//}

			#endregion

			if (descriptionList.Count == 1)
			{
				returnHtml = descriptionList.First();
			}
			else if (descriptionList.Count > 1)
			{
				returnHtml += "<ul class=\"list-inline\">";
				foreach (var item in descriptionList)
				{
					returnHtml += "<li class=\"list-inline-item\">" + item + "</li>";
				}
				returnHtml += "</ul>";
			}
			return returnHtml;
		}

		public static Dictionary<Guid, List<SelectOption>> GetNodePageDictionary(Guid? appId = null)
		{
			//Generate node / pages dictionary
			var pageSrv = new PageService();
			var appPages = new List<ErpPage>();
			if (appId == null)
			{
				appPages = pageSrv.GetAll();
			}
			else
			{
				appPages = pageSrv.GetAppControlledPages(appId ?? Guid.Empty);
			}
			var nodePagesDict = new Dictionary<Guid, List<SelectOption>>();
			foreach (var page in appPages)
			{
				if (page.NodeId != null)
				{
					if (!nodePagesDict.ContainsKey(page.NodeId ?? Guid.Empty))
					{
						nodePagesDict[page.NodeId ?? Guid.Empty] = new List<SelectOption>();
					}
					var nodePages = nodePagesDict[page.NodeId ?? Guid.Empty];
					var selectOption = new SelectOption()
					{
						Value = page.Id.ToString(),
						Label = page.Name
					};
					nodePages.Add(selectOption);
					nodePagesDict[page.NodeId ?? Guid.Empty] = nodePages;
				}
			}

			return nodePagesDict;

		}

		public static string GenerateSitemapNodeUrl(SitemapNode node, SitemapArea area, App app)
		{

			switch (node.Type)
			{
				case SitemapNodeType.EntityList:
					// /{AppName}/{AreaName}/{NodeName}/l/{PageName?}
					return $"/{app.Name}/{area.Name}/{node.Name}/l";
				case SitemapNodeType.ApplicationPage:
					// /{AppName}/{AreaName}/{NodeName}/a/{PageName?}
					return $"/{app.Name}/{area.Name}/{node.Name}/a";
				case SitemapNodeType.Url:
					return node.Url;
				default:
					return "";
			}
		}

		public static string GetActionTemplate(PageUtilsActionType type, string label = "",
			string returnUrl = "", string formId = "", string iconClass = "", string btnClass = "", string titleText = "")
		{

			switch (type)
			{
				case PageUtilsActionType.Cancel:
					{
						var labelRender = !String.IsNullOrWhiteSpace(label) ? label : "Cancel";
						var btnClassRender = !String.IsNullOrWhiteSpace(btnClass) ? btnClass : "btn btn-white btn-sm";
						var linkAttribute = !String.IsNullOrWhiteSpace(returnUrl) ? $"href='{returnUrl}'" : $"onclick='goBack();'";
						return $"<a {linkAttribute} class='{btnClassRender}' title='{titleText}'>{labelRender}</a>";
					}
				case PageUtilsActionType.SubmitForm:
					{
						var btnClassRender = !String.IsNullOrWhiteSpace(btnClass) ? btnClass : "btn btn-blue btn-sm";
						var iconClassRender = !String.IsNullOrWhiteSpace(iconClass) ? iconClass : "fa fa-save go-white";
						var labelRender = !String.IsNullOrWhiteSpace(label) ? label : "Save";
						return $"<button type='submit' form='{formId}' class='{btnClassRender}' title='{titleText}'><span class='{iconClassRender}'></span> {labelRender}</button>";
					}
				case PageUtilsActionType.ConfirmAndSubmitForm:
					{
						var btnClassRender = !String.IsNullOrWhiteSpace(btnClass) ? btnClass : "btn btn-blue btn-sm";
						var iconClassRender = !String.IsNullOrWhiteSpace(iconClass) ? iconClass : "fa fa-save go-white";
						var labelRender = !String.IsNullOrWhiteSpace(label) ? label : "Save";
						return $"<button type='submit' form='{formId}' onclick='if(confirm(\"Are you sure?\")) {{return true;}} else {{return false;}}' class='{btnClassRender}' title='{titleText}'><span class='{iconClassRender}'></span> {labelRender}</button>";
					}
				case PageUtilsActionType.Disabled:
					{
						var labelRender = !String.IsNullOrWhiteSpace(label) ? label : "Locked";
						var iconClassRender = !String.IsNullOrWhiteSpace(iconClass) ? iconClass : "fa fa-fw fa-lock";
						var btnClassRender = !String.IsNullOrWhiteSpace(btnClass) ? btnClass : "btn btn-white btn-sm disabled";
						return $"<button type='button' disabled class='{btnClassRender}' title='{titleText}'><span class='{iconClassRender}'></span> {labelRender}</button>";
					}
				default:
					return "";
			}
		}

		public static PageBodyNode GetAjaxPageBodyNode(string fullComponentName, Guid pageId, string optionsString)
		{
			return new PageBodyNode()
			{
				ComponentName = fullComponentName,
				Id = Guid.Empty,
				Options = optionsString,
				PageId = pageId
			};
		}

		public static List<PageBodyNode> RecalculateContainerNodeWeights(List<PageBodyNode> pageNodes, out List<Guid> nodesToBeUpdated, Guid movedNodeId)
		{
			nodesToBeUpdated = new List<Guid>();
			var movedNode = pageNodes.FirstOrDefault(x => x.Id == movedNodeId);
			if (movedNode == null)
			{
				throw new Exception("Moved node is not found for id " + movedNodeId);
			}
			//The moved node should be updated
			nodesToBeUpdated.Add(movedNodeId);

			pageNodes = pageNodes.OrderBy(x => x.Weight).ToList();
			var nodeContainersDictionary = new Dictionary<string, List<PageBodyNode>>();

			#region << Init nodeContainersDictionary >> 
			foreach (var node in pageNodes)
			{
				var containerId = String.IsNullOrWhiteSpace(node.ContainerId) ? "===none===" : node.ContainerId;

				var parentId = node.ParentId == null ? "===none===" : node.ParentId.ToString();

				var dictKey = parentId + "-" + containerId;

				if (!nodeContainersDictionary.ContainsKey(dictKey))
					nodeContainersDictionary[dictKey] = new List<PageBodyNode>();

				nodeContainersDictionary[dictKey].Add(node);
			}
			#endregion

			#region << Reshuffle the Target Container >>
			{
				var containerId = String.IsNullOrWhiteSpace(movedNode.ContainerId) ? "===none===" : movedNode.ContainerId;

				var parentId = movedNode.ParentId == null ? "===none===" : movedNode.ParentId.ToString();

				var dictKey = parentId + "-" + containerId;

				nodeContainersDictionary[dictKey] = nodeContainersDictionary[dictKey].FindAll(x => x.Id != movedNode.Id).ToList();
				nodeContainersDictionary[dictKey].Insert(movedNode.Weight - 1, movedNode);

			}
			#endregion

			var outputNodeList = new List<PageBodyNode>();

			foreach (var key in nodeContainersDictionary.Keys)
			{
				var counter = 1;
				foreach (var node in nodeContainersDictionary[key])
				{
					if (node.Weight != counter)
					{
						node.Weight = counter;
						if (!nodesToBeUpdated.Any(x => x == node.Id))
							nodesToBeUpdated.Add(node.Id);
					}
					outputNodeList.Add(node);
					counter++;
				}
			}

			return outputNodeList;
		}

		public static JObject ConvertStringToJObject(string input)
		{
			if (String.IsNullOrWhiteSpace(input) || input == "{}" || input == "\"{}\"")
			{
				return new JObject();
			}
			return JsonConvert.DeserializeObject<JObject>(input);
		}

		public static string GetDataSourceIconBadge(DataSourceType type)
		{
			switch (type)
			{
				case DataSourceType.DATABASE:
					return $"<div class='badge badge-pill' style='font-size:18px;color:#9C27B0;'><span class='fas fa-fw fa-database'></span></div>";
				case DataSourceType.CODE:
					return $"<div class='badge badge-pill' style='font-size:18px;color:#E91E63;'><span class='fas fa-fw fa-code'></span></div>";
				default:
					return "";
			}
		}

		public static string CalculatePageUrl(Guid pageId)
		{
			var result = "";
			var pageSrv = new PageService();
			var appSrv = new AppService();
			var page = pageSrv.GetPage(pageId);
			if (page == null)
				throw new Exception("No such page");
			App pageApp = null;
			SitemapArea pageArea = null;
			SitemapNode pageNode = null;
			Entity pageEntity = null;
			Guid firstRecordId = Guid.Empty;
			if (page.AppId != null) {
				pageApp = appSrv.GetApplication(page.AppId ?? Guid.Empty);
				if (page.NodeId != null) {
					foreach (var area in pageApp.Sitemap.Areas)
					{
						foreach (var node in area.Nodes)
						{
							if (node.Id == page.NodeId) {
								pageArea = area;
								pageNode = node;
								break;
							}
						}
						if (pageArea != null) {
							break;
						}
					}
				}
			}
			if (page.EntityId != null) {
				pageEntity = new EntityManager().ReadEntity(page.EntityId ?? Guid.Empty).Object;
				if (pageEntity != null) {
					var sortsList = new List<QuerySortObject>();
					sortsList.Add(new QuerySortObject("id", QuerySortType.Ascending));
					var findRecordResponse = new RecordManager().Find(new EntityQuery(pageEntity.Name, "*", null, sortsList.ToArray(), 0, 1));
					if (!findRecordResponse.Success)
						throw new Exception(findRecordResponse.Message);
					if (findRecordResponse.Object != null && findRecordResponse.Object.Data.Any())
					{
						var record = findRecordResponse.Object.Data.First();
						firstRecordId = (Guid)record["id"];
					}
				}
			}

			if (pageApp == null && page.Type != PageType.Site)
				return string.Empty;

			switch (page.Type) {
				case PageType.Site:
					return $"/s/{page.Name}";
				case PageType.Application:
					if (page.NodeId == null)
					{
						//Case 1: Not attached to node
						return $"/{pageApp.Name}/a/{page.Name}";
					}
					else
					{
						//Case 2: Attached to node
						return $"/{pageApp.Name}/{pageArea.Name}/{pageNode.Name}/a/{page.Name}";
					}
				case PageType.RecordList:
					if (page.NodeId != null)
					{
						//Case 2: Must have a node
						return $"/{pageApp.Name}/{pageArea.Name}/{pageNode.Name}/l/{page.Name}";
					}
					else if (page.AppId != null)
					{
						//Entity Page that is not attached to a node. Select the first Area and the first node
						if (pageApp.Sitemap.Areas != null && pageApp.Sitemap.Areas.Any())
						{
							foreach (var area in pageApp.Sitemap.Areas)
							{
								if (area.Nodes != null && area.Nodes.Any())
								{
									var areaNodes = area.Nodes.FindAll(x => x.Type == SitemapNodeType.EntityList && x.EntityId == page.EntityId);
									if (areaNodes.Any())
									{
										var node = areaNodes[0];
										return $"/{pageApp.Name}/{area.Name}/{node.Name}/l/{page.Name}";
									}
								}
							}

						}

					}
					break;
				case PageType.RecordCreate:
					if (page.NodeId != null)
					{
						//Case 2: Must have a node
						return $"/{pageApp.Name}/{pageArea.Name}/{pageNode.Name}/c/{page.Name}";
					}
					else if(page.AppId != null){
						//Entity Page that is not attached to a node. Select the first Area and the first node
						if (pageApp.Sitemap.Areas != null && pageApp.Sitemap.Areas.Any()) {
							foreach (var area in pageApp.Sitemap.Areas)
							{
								if (area.Nodes != null && area.Nodes.Any())
								{
									var areaNodes = area.Nodes.FindAll(x => x.Type == SitemapNodeType.EntityList && x.EntityId == page.EntityId);
									if (areaNodes.Any())
									{
										var node = areaNodes[0];
										return $"/{pageApp.Name}/{area.Name}/{node.Name}/c/{page.Name}";
									}
								}
							}
						}

					}
					break;
				case PageType.RecordDetails:
					if (page.NodeId != null)
					{
						//Case 2: Must have a node
						return $"/{pageApp.Name}/{pageArea.Name}/{pageNode.Name}/r/{firstRecordId}/{page.Name}";
					}
					else if (page.AppId != null)
					{
						//Entity Page that is not attached to a node. Select the first Area and the first node
						if (pageApp.Sitemap.Areas != null && pageApp.Sitemap.Areas.Any())
						{
							foreach (var area in pageApp.Sitemap.Areas)
							{
								if (area.Nodes != null && area.Nodes.Any())
								{
									var areaNodes = area.Nodes.FindAll(x => x.Type == SitemapNodeType.EntityList && x.EntityId == page.EntityId);
									if (areaNodes.Any())
									{
										var node = areaNodes[0];
										return $"/{pageApp.Name}/{area.Name}/{node.Name}/r/{firstRecordId}/{page.Name}";
									}
								}
							}

						}

					}
					break;
				case PageType.RecordManage:
					if (page.NodeId != null)
					{
						//Case 2: Must have a node
						return $"/{pageApp.Name}/{pageArea.Name}/{pageNode.Name}/m/{firstRecordId}/{page.Name}";
					}
					else if (page.AppId != null)
					{
						//Entity Page that is not attached to a node. Select the first Area and the first node
						if (pageApp.Sitemap.Areas != null && pageApp.Sitemap.Areas.Any())
						{
							foreach (var area in pageApp.Sitemap.Areas)
							{
								if (area.Nodes != null && area.Nodes.Any())
								{
									var areaNodes = area.Nodes.FindAll(x => x.Type == SitemapNodeType.EntityList && x.EntityId == page.EntityId);
									if (areaNodes.Any())
									{
										var node = areaNodes[0];
										return $"/{pageApp.Name}/{area.Name}/{node.Name}/m/{firstRecordId}/{page.Name}";
									}
								}
							}

						}

					}
					break;
				default:
					throw new Exception("Unknown page type");					
			}

			return result;
		}

		public static List<ModelNode> GetPageSystemModelNodes(ErpPage page)
		{
			if (page == null)
				throw new NullReferenceException(nameof(page));

			var result = new List<ModelNode>();
			#region << Everything from context >>
			{
				//Detection
				{
					var node2 = new ModelNode()
					{
						PageDataSourceName = "Detection",
						DataType = "IDeviceResolver",
						Tags = new List<string>() { "system" }
					};
					//Device
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Device",
							DataType = "IDevice"
						};

						//Type
						{
							var node4 = new ModelNode()
							{
								PageDataSourceName = "Type",
								DataType = "Enum:DeviceType"
							};
							node3.Nodes.Add(node4);
						}

						//Crawler
						{
							var node4 = new ModelNode()
							{
								PageDataSourceName = "Crawler",
								DataType = "Boolean"
							};
							node3.Nodes.Add(node4);
						}

						node2.Nodes.Add(node3);
					}
					result.Add(node2);
				}

				//PageContext
				{
					var node2 = new ModelNode()
					{
						PageDataSourceName = "PageContext",
						DataType = "PageContext",
						Tags = new List<string>() { "system" }
					};
					result.Add(node2);
				}

				//App
				{
					var node2 = new ModelNode()
					{
						PageDataSourceName = "App",
						DataType = "App",
						Tags = new List<string>() { "system" }
					};

					//Id
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Id",
							DataType = "Guid"
						};
						node2.Nodes.Add(node3);
					}

					//Name
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Name",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//Label
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Label",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//Description
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Description",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//IconClass
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "IconClass",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//Author
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Author",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//Color
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Color",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//Sitemap
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Sitemap",
							DataType = "Sitemap"
						};

						//Areas
						{
							var node4 = new ModelNode()
							{
								PageDataSourceName = "Areas",
								DataType = "List<SitemapArea>",
								Tags = new List<string>() { "list" }
							};

							//Id
							{
								var node5 = new ModelNode()
								{
									PageDataSourceName = "Id",
									DataType = "Guid"
								};
								node4.Nodes.Add(node5);
							}

							//Weight
							{
								var node5 = new ModelNode()
								{
									PageDataSourceName = "Weight",
									DataType = "Integer"
								};
								node4.Nodes.Add(node5);
							}

							//Label
							{
								var node5 = new ModelNode()
								{
									PageDataSourceName = "Label",
									DataType = "String"
								};
								node4.Nodes.Add(node5);
							}

							//Description
							{
								var node5 = new ModelNode()
								{
									PageDataSourceName = "Description",
									DataType = "String"
								};
								node4.Nodes.Add(node5);
							}

							//Name
							{
								var node5 = new ModelNode()
								{
									PageDataSourceName = "Name",
									DataType = "String"
								};
								node4.Nodes.Add(node5);
							}

							//IconClass
							{
								var node5 = new ModelNode()
								{
									PageDataSourceName = "IconClass",
									DataType = "String"
								};
								node4.Nodes.Add(node5);
							}

							//Color
							{
								var node5 = new ModelNode()
								{
									PageDataSourceName = "Color",
									DataType = "String"
								};
								node4.Nodes.Add(node5);
							}

							//Access
							{
								var node5 = new ModelNode()
								{
									PageDataSourceName = "Access",
									DataType = "List<Guid>",
									Tags = new List<string>() { "list" }
								};
								node4.Nodes.Add(node5);
							}

							//Nodes
							{
								var node5 = new ModelNode()
								{
									PageDataSourceName = "Nodes",
									DataType = "List<SitemapNode>",
									Tags = new List<string>() { "list" }
								};

								//Id
								{
									var node6 = new ModelNode()
									{
										PageDataSourceName = "Id",
										DataType = "Guid"
									};
									node5.Nodes.Add(node6);
								}

								//Weight
								{
									var node6 = new ModelNode()
									{
										PageDataSourceName = "Weight",
										DataType = "Integer"
									};
									node5.Nodes.Add(node6);
								}

								//GroupName
								{
									var node6 = new ModelNode()
									{
										PageDataSourceName = "GroupName",
										DataType = "String"
									};
									node5.Nodes.Add(node6);
								}

								//Label
								{
									var node6 = new ModelNode()
									{
										PageDataSourceName = "Label",
										DataType = "String"
									};
									node5.Nodes.Add(node6);
								}

								//Name
								{
									var node6 = new ModelNode()
									{
										PageDataSourceName = "Name",
										DataType = "String"
									};
									node5.Nodes.Add(node6);
								}

								//IconClass
								{
									var node6 = new ModelNode()
									{
										PageDataSourceName = "IconClass",
										DataType = "String"
									};
									node5.Nodes.Add(node6);
								}

								//Url
								{
									var node6 = new ModelNode()
									{
										PageDataSourceName = "Url",
										DataType = "String"
									};
									node5.Nodes.Add(node6);
								}

								//Access
								{
									var node6 = new ModelNode()
									{
										PageDataSourceName = "Access",
										DataType = "List<Guid>",
										Tags = new List<string>() { "list" }
									};
									node5.Nodes.Add(node6);
								}

								//Type
								{
									var node6 = new ModelNode()
									{
										PageDataSourceName = "Type",
										DataType = "Enum: SitemapNodeType"
									};
									node5.Nodes.Add(node6);
								}

								//EntityId
								{
									var node6 = new ModelNode()
									{
										PageDataSourceName = "EntityId",
										DataType = "Guid?"
									};
									node5.Nodes.Add(node6);
								}

								node4.Nodes.Add(node5);
							}

							node3.Nodes.Add(node4);
						}

						node2.Nodes.Add(node3);
					}

					result.Add(node2);
				}

				//SitemapArea
				{
					var node2 = new ModelNode()
					{
						PageDataSourceName = "SitemapArea",
						DataType = "SitemapArea",
						Tags = new List<string>() { "system" }
					};

					//Id
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Id",
							DataType = "Guid"
						};
						node2.Nodes.Add(node3);
					}

					//Weight
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Weight",
							DataType = "Integer"
						};
						node2.Nodes.Add(node3);
					}

					//Label
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Label",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//Description
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Description",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//Name
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Name",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//IconClass
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "IconClass",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//Color
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Color",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//Access
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Access",
							DataType = "List<Guid>",
							Tags = new List<string>() { "list" }
						};
						node2.Nodes.Add(node3);
					}

					//Nodes
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Nodes",
							DataType = "List<SitemapNode>",
							Tags = new List<string>() { "list" }
						};

						//Id
						{
							var node4 = new ModelNode()
							{
								PageDataSourceName = "Id",
								DataType = "Guid"
							};
							node3.Nodes.Add(node4);
						}

						//Weight
						{
							var node4 = new ModelNode()
							{
								PageDataSourceName = "Weight",
								DataType = "Integer"
							};
							node3.Nodes.Add(node4);
						}

						//GroupName
						{
							var node4 = new ModelNode()
							{
								PageDataSourceName = "GroupName",
								DataType = "String"
							};
							node3.Nodes.Add(node4);
						}

						//Label
						{
							var node4 = new ModelNode()
							{
								PageDataSourceName = "Label",
								DataType = "String"
							};
							node3.Nodes.Add(node4);
						}

						//Name
						{
							var node4 = new ModelNode()
							{
								PageDataSourceName = "Name",
								DataType = "String"
							};
							node3.Nodes.Add(node4);
						}

						//IconClass
						{
							var node4 = new ModelNode()
							{
								PageDataSourceName = "IconClass",
								DataType = "String"
							};
							node3.Nodes.Add(node4);
						}

						//Url
						{
							var node4 = new ModelNode()
							{
								PageDataSourceName = "Url",
								DataType = "String"
							};
							node3.Nodes.Add(node4);
						}

						//Access
						{
							var node4 = new ModelNode()
							{
								PageDataSourceName = "Access",
								DataType = "List<Guid>",
								Tags = new List<string>() { "list" }
							};
							node3.Nodes.Add(node4);
						}

						//Type
						{
							var node4 = new ModelNode()
							{
								PageDataSourceName = "Type",
								DataType = "Enum: SitemapNodeType"
							};
							node3.Nodes.Add(node4);
						}

						//EntityId
						{
							var node4 = new ModelNode()
							{
								PageDataSourceName = "EntityId",
								DataType = "Guid?"
							};
							node3.Nodes.Add(node4);
						}

						node2.Nodes.Add(node3);
					}

					result.Add(node2);
				}

				//SitemapNode
				{
					var node2 = new ModelNode()
					{
						PageDataSourceName = "SitemapNode",
						DataType = "SitemapNode",
						Tags = new List<string>() { "system" }
					};

					//Id
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Id",
							DataType = "Guid"
						};
						node2.Nodes.Add(node3);
					}

					//Weight
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Weight",
							DataType = "Integer"
						};
						node2.Nodes.Add(node3);
					}

					//GroupName
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "GroupName",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//Label
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Label",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//Name
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Name",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//IconClass
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "IconClass",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//Url
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Url",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//Access
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Access",
							DataType = "List<Guid>",
							Tags = new List<string>() { "list" }
						};
						node2.Nodes.Add(node3);
					}

					//Type
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Type",
							DataType = "Enum: SitemapNodeType"
						};
						node2.Nodes.Add(node3);
					}

					//EntityId
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "EntityId",
							DataType = "Guid?"
						};
						node2.Nodes.Add(node3);
					}

					result.Add(node2);
				}

				//Entity
				{
					var node2 = new ModelNode()
					{
						PageDataSourceName = "Entity",
						DataType = "Entity",
						Tags = new List<string>() { "system" }
					};

					//Id
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Id",
							DataType = "Guid"
						};
						node2.Nodes.Add(node3);
					}

					//Name
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Name",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//Label
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Label",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//LabelPlural
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "LabelPlural",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//System
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "System",
							DataType = "Boolean"
						};
						node2.Nodes.Add(node3);
					}

					//IconName
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "IconName",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//Color
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Color",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//Fields
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Fields",
							DataType = "List<Field>",
							Tags = new List<string>() { "list" }
						};
						node2.Nodes.Add(node3);
					}

					//RecordScreenIdField
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "RecordScreenIdField",
							DataType = "Guid?"
						};
						node2.Nodes.Add(node3);
					}

					result.Add(node2);
				}

				//ParentEntity
				{
					var node2 = new ModelNode()
					{
						PageDataSourceName = "ParentEntity",
						DataType = "Entity",
						Tags = new List<string>() { "system" }
					};

					//Id
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Id",
							DataType = "Guid"
						};
						node2.Nodes.Add(node3);
					}

					//Name
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Name",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//Label
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Label",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//LabelPlural
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "LabelPlural",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//System
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "System",
							DataType = "Boolean"
						};
						node2.Nodes.Add(node3);
					}

					//IconName
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "IconName",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//Color
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Color",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//Fields
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Fields",
							DataType = "List<Field>",
							Tags = new List<string>() { "list" }
						};
						node2.Nodes.Add(node3);
					}

					//RecordScreenIdField
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "RecordScreenIdField",
							DataType = "Guid?"
						};
						node2.Nodes.Add(node3);
					}

					result.Add(node2);
				}

				//Page
				{
					var node2 = new ModelNode()
					{
						PageDataSourceName = "Page",
						DataType = "ErpPage",
						Tags = new List<string>() { "system" }
					};

					//Id
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Id",
							DataType = "Guid"
						};
						node2.Nodes.Add(node3);
					}

					//Weight
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Weight",
							DataType = "Integer"
						};
						node2.Nodes.Add(node3);
					}

					//Label
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Label",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//Name
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Name",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//IconClass
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "IconClass",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//System
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "System",
							DataType = "Boolean"
						};
						node2.Nodes.Add(node3);
					}

					//Type
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Type",
							DataType = "Enum: PageType"
						};
						node2.Nodes.Add(node3);
					}

					//AppId
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "AppId",
							DataType = "Guid?"
						};
						node2.Nodes.Add(node3);
					}

					//EntityId
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "EntityId",
							DataType = "Guid?"
						};
						node2.Nodes.Add(node3);
					}

					//AreaId
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "AreaId",
							DataType = "Guid?"
						};
						node2.Nodes.Add(node3);
					}

					//NodeId
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "NodeId",
							DataType = "Guid?"
						};
						node2.Nodes.Add(node3);
					}

					//IsRazorBody
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "IsRazorBody",
							DataType = "Boolean"
						};
						node2.Nodes.Add(node3);
					}

					result.Add(node2);
				}

				//ParentPage
				{
					var node2 = new ModelNode()
					{
						PageDataSourceName = "ParentPage",
						DataType = "ErpPage",
						Tags = new List<string>() { "system" }
					};

					//Id
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Id",
							DataType = "Guid"
						};
						node2.Nodes.Add(node3);
					}

					//Weight
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Weight",
							DataType = "Integer"
						};
						node2.Nodes.Add(node3);
					}

					//Label
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Label",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//Name
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Name",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//IconClass
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "IconClass",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					//System
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "System",
							DataType = "Boolean"
						};
						node2.Nodes.Add(node3);
					}

					//Type
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Type",
							DataType = "Enum: PageType"
						};
						node2.Nodes.Add(node3);
					}

					//AppId
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "AppId",
							DataType = "Guid?"
						};
						node2.Nodes.Add(node3);
					}

					//EntityId
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "EntityId",
							DataType = "Guid?"
						};
						node2.Nodes.Add(node3);
					}

					//AreaId
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "AreaId",
							DataType = "Guid?"
						};
						node2.Nodes.Add(node3);
					}

					//NodeId
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "NodeId",
							DataType = "Guid?"
						};
						node2.Nodes.Add(node3);
					}

					//IsRazorBody
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "IsRazorBody",
							DataType = "Boolean"
						};
						node2.Nodes.Add(node3);
					}

					result.Add(node2);
				}

			

				//Validation
				{
					var node2 = new ModelNode()
					{
						PageDataSourceName = "Validation",
						DataType = "ValidationException",
						Tags = new List<string>() { "system" }
					};

					//Errors
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Errors",
							DataType = "List<ValidationError>",
							Tags = new List<string>() { "list" }
						};

						//PropertyName
						{
							var node4 = new ModelNode()
							{
								PageDataSourceName = "PropertyName",
								DataType = "String"
							};
							node3.Nodes.Add(node4);
						}

						//Index
						{
							var node4 = new ModelNode()
							{
								PageDataSourceName = "Index",
								DataType = "Long"
							};
							node3.Nodes.Add(node4);
						}

						//Message
						{
							var node4 = new ModelNode()
							{
								PageDataSourceName = "Message",
								DataType = "String"
							};
							node3.Nodes.Add(node4);
						}

						//IsSystem
						{
							var node4 = new ModelNode()
							{
								PageDataSourceName = "IsSystem",
								DataType = "Boolean"
							};
							node3.Nodes.Add(node4);
						}

						node2.Nodes.Add(node3);
					}

					//Message
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Message",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					result.Add(node2);
				}

			}
			#endregion

			#region << CurrentUser >>
			{
				var node1 = new ModelNode()
				{
					PageDataSourceName = "CurrentUser",
					DataType = "ErpUser",
					Tags = new List<string>() { "system" }
				};

				//Id
				{
					var node2 = new ModelNode()
					{
						PageDataSourceName = "Id",
						DataType = "Guid"
					};
					node1.Nodes.Add(node2);
				}

				//Username
				{
					var node2 = new ModelNode()
					{
						PageDataSourceName = "Username",
						DataType = "String"
					};
					node1.Nodes.Add(node2);
				}

				//Email
				{
					var node2 = new ModelNode()
					{
						PageDataSourceName = "Email",
						DataType = "String"
					};
					node1.Nodes.Add(node2);
				}

				//FirstName
				{
					var node2 = new ModelNode()
					{
						PageDataSourceName = "FirstName",
						DataType = "String"
					};
					node1.Nodes.Add(node2);
				}

				//LastName
				{
					var node2 = new ModelNode()
					{
						PageDataSourceName = "LastName",
						DataType = "String"
					};
					node1.Nodes.Add(node2);
				}

				//Image
				{
					var node2 = new ModelNode()
					{
						PageDataSourceName = "Image",
						DataType = "String"
					};
					node1.Nodes.Add(node2);
				}

				//CreatedOn
				{
					var node2 = new ModelNode()
					{
						PageDataSourceName = "CreatedOn",
						DataType = "DateTime"
					};
					node1.Nodes.Add(node2);
				}

				//Roles
				{
					var node2 = new ModelNode()
					{
						PageDataSourceName = "Roles",
						DataType = "List<ErpRole>",
						Tags = new List<string>() { "list" }
					};

					//Id
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Id",
							DataType = "Guid"
						};
						node2.Nodes.Add(node3);
					}

					//Name
					{
						var node3 = new ModelNode()
						{
							PageDataSourceName = "Name",
							DataType = "String"
						};
						node2.Nodes.Add(node3);
					}

					node1.Nodes.Add(node2);
				}

				result.Add(node1);
			}
			#endregion

			#region << ReturnUrl >>
			{
				var node1 = new ModelNode()
				{
					PageDataSourceName = "ReturnUrl",
					DataType = "String",
					Tags = new List<string>() { "system" }
				};
				result.Add(node1);
			}
			#endregion

			#region << Record >>
			{
				//RecordId
				{
					var node2 = new ModelNode()
					{
						PageDataSourceName = "RecordId",
						DataType = "Guid?",
						Tags = new List<string>() { "system" }
					};
					result.Add(node2);
				}

				var node1 = new ModelNode()
				{
					PageDataSourceName = "Record",
					DataType = "EntityRecord",
					Tags = new List<string>() { "system" }
				};
				result.Add(node1);
			}

			if ((page.Type == PageType.RecordDetails || page.Type == PageType.RecordManage) || page.Type == PageType.RecordList)
			{
				{
					var node1 = new ModelNode()
					{
						PageDataSourceName = "ParentRecord",
						DataType = "EntityRecord",
						Tags = new List<string>() { "system" }
					};
					result.Add(node1);
				}

				//RelationId
				{
					var node2 = new ModelNode()
					{
						PageDataSourceName = "RelationId",
						DataType = "Guid?",
						Tags = new List<string>() { "system" }
					};
					result.Add(node2);
				}

				//ParentRecordId
				{
					var node2 = new ModelNode()
					{
						PageDataSourceName = "ParentRecordId",
						DataType = "Guid?",
						Tags = new List<string>() { "system" }
					};
					result.Add(node2);
				}
			}

			{
				var node1 = new ModelNode()
				{
					PageDataSourceName = "RowRecord",
					DataType = "EntityRecord",
					Tags = new List<string>() { "system" }
				};
				result.Add(node1);
			}

			{
				var node1 = new ModelNode()
				{
					PageDataSourceName = "RequestQuery",
					DataType = "EntityRecord",
					Tags = new List<string>() { "system" }
				};
				result.Add(node1);
			}


			#endregion

			var pageDataSources = new PageService().GetPageDataSources(page.Id);
			var allDatasources = new DataSourceManager().GetAll();
			foreach (var pageDS in pageDataSources)
			{
				var ds = allDatasources.SingleOrDefault(x => x.Id == pageDS.DataSourceId);

				//if data source was deleted (and system validation for usage failed)
				//we hide this page data source
				if (ds == null)
					continue;

				var node = new ModelNode()
				{
					PageDataSourceId = pageDS.Id,
					PageDataSourceName = pageDS.Name,
					DataType = ds.ResultModel,
					Tags = new List<string>() { ds.Type.GetLabel() },
					DataSourceId = ds.Id,

				};
				if (node.DataType == "List<EntityRecord>" || node.DataType == "EntityRecordList")
					node.Tags.Add("list");
				node.Nodes.AddRange(ProcessDataSourceFieldsMeta(ds.Fields));
				node.Params.AddRange(pageDS.Parameters);
				result.Add(node);
			}

			return result;
		}

		private static List<ModelNode> ProcessDataSourceFieldsMeta(List<DataSourceModelFieldMeta> fields)
		{
			List<ModelNode> result = new List<ModelNode>();
			
			if (fields == null)
				return result;

			foreach (var fieldMeta in fields)
			{
				var node = new ModelNode()
				{
					PageDataSourceId = Guid.Empty,
					PageDataSourceName = fieldMeta.Name,
					DataType = (fieldMeta.Type == FieldType.RelationField ) ? "List<EntityRecord>" : fieldMeta.Type.ToString(),
					DataSourceId = Guid.Empty
				};
				if (node.DataType == "List<EntityRecord>" || node.DataType == "EntityRecordsList")
					node.Tags.Add("list");

				node.Nodes.AddRange(ProcessDataSourceFieldsMeta(fieldMeta.Children));
				result.Add(node);

			}
			return result;
		}

		public static List<string> PropertyNamesList(Guid pageId)
		{
			var result = new List<string>();
			var page = new PageService().GetPage(pageId);
			if (page != null)
			{
				var modelNodes = GetPageSystemModelNodes(page);
				foreach (var modelNode in modelNodes)
				{
					result = GeneratePropertyNamesFromModelNode(modelNode, result, "");
				}
			}
			result = result.OrderBy(x => x).ToList();
			return result;
		}

		private static List<string> GeneratePropertyNamesFromModelNode(ModelNode modelNode, List<string> result, string parentNodePath)
		{
			var currentNodePath = parentNodePath + modelNode.PageDataSourceName;
			result.Add(currentNodePath);
			foreach (var childNode in modelNode.Nodes)
			{
				if (modelNode.DataType.StartsWith("List<") || modelNode.DataType.StartsWith("IEnumerable") || modelNode.DataType.StartsWith("Enumerable"))
				{
					result = GeneratePropertyNamesFromModelNode(childNode, result, currentNodePath + "[$index].");
				}
				else
				{
					result = GeneratePropertyNamesFromModelNode(childNode, result, currentNodePath + ".");
				}
			}

			return result;
		}

		public static string GenerateTagsFromObject(ScriptTagInclude scriptTag)
		{
			var resultStringList = new List<String>();
			if (!string.IsNullOrWhiteSpace(scriptTag.InlineContent))
			{
				#region << type >>
				if(!scriptTag.IsNomodule)
				{
					var attribute = $"type=\"{scriptTag.Type}\"";
					resultStringList.Add(attribute);
				}
				#endregion

				#region << nomodule >>
				if(scriptTag.IsNomodule)
				{
					var attribute = $"nomodule";
					resultStringList.Add(attribute);
				}
				#endregion

				//Inline script
				return "<script " + String.Join(" ", resultStringList).Trim() + " >" + scriptTag.InlineContent + "</script>";
			}
			else
			{
				//External Resource
				#region << src >>
				{
					var attribute = $"src=\"{scriptTag.Src}\"";
					if (!String.IsNullOrWhiteSpace(scriptTag.CacheBreaker))
					{
						attribute = $"src=\"{scriptTag.Src}?cb={scriptTag.CacheBreaker}\"";
					}
					resultStringList.Add(attribute);
				}
				#endregion

				#region << type >>
				if(!scriptTag.IsNomodule)
				{
					var attribute = $"type=\"{scriptTag.Type}\"";
					resultStringList.Add(attribute);
				}
				#endregion

				#region << nomodule >>
				if(scriptTag.IsNomodule)
				{
					var attribute = $"nomodule";
					resultStringList.Add(attribute);
				}
				#endregion

				return "<script " + String.Join(" ", resultStringList).Trim() + " ></script>";
			}
		}

		public static string GenerateTagsFromObject(LinkTagInclude linkTag)
		{
			var resultStringList = new List<String>();
			if (!string.IsNullOrWhiteSpace(linkTag.InlineContent))
			{
				//Inline styles
				return "<style>" + linkTag.InlineContent + "</style>";
			}
			else
			{
				//External Resource
				#region << href >>
				{
					var attribute = $"href=\"{linkTag.Href}\"";
					if (!String.IsNullOrWhiteSpace(linkTag.CacheBreaker))
					{
						attribute = $"href=\"{linkTag.Href}?cb={linkTag.CacheBreaker}\"";
					}
					resultStringList.Add(attribute);
				}
				#endregion

				#region << rel >>
				{
					var attribute = $"";
					switch (linkTag.Rel)
					{
						case RelType.Alternate:
							attribute = $"rel=\"alternate\"";
							break;
						case RelType.Author:
							attribute = $"rel=\"author\"";
							break;
						case RelType.DnsPrefetch:
							attribute = $"rel=\"dns-prefetch\"";
							break;
						case RelType.Help:
							attribute = $"rel=\"help\"";
							break;
						case RelType.Icon:
							attribute = $"rel=\"icon\"";
							break;
						case RelType.License:
							attribute = $"rel=\"license\"";
							break;
						case RelType.Next:
							attribute = $"rel=\"next\"";
							break;
						case RelType.Pingback:
							attribute = $"rel=\"pingback\"";
							break;
						case RelType.Preconnect:
							attribute = $"rel=\"preconnect\"";
							break;
						case RelType.Prefetch:
							attribute = $"rel=\"prefetch\"";
							break;
						case RelType.Preload:
							attribute = $"rel=\"preload\"";
							break;
						case RelType.Prerender:
							attribute = $"rel=\"prerender\"";
							break;
						case RelType.Prev:
							attribute = $"rel=\"prev\"";
							break;
						case RelType.Search:
							attribute = $"rel=\"search\"";
							break;
						case RelType.Stylesheet:
							attribute = $"rel=\"stylesheet\"";
							break;
						default:
							break;
					}
					resultStringList.Add(attribute);
				}
				#endregion

				#region << type >>
				{
					var attribute = $"type=\"{linkTag.Type}\"";
					resultStringList.Add(attribute);
				}
				#endregion

				#region << crossorigin >>
				{
					if (linkTag.CrossOrigin != CrossOriginType.None)
					{
						var attribute = "";
						switch (linkTag.CrossOrigin)
						{
							case CrossOriginType.Anonymous:
								attribute = "crossorigin=\"anonymous\"";
								break;
							case CrossOriginType.UseCredentials:
								attribute = "crossorigin=\"use-credentials\"";
								break;
							default:
								break;
						}
						resultStringList.Add(attribute);
					}
				}
				#endregion

				#region << integrity >>
				{
					if (!String.IsNullOrWhiteSpace(linkTag.Integrity))
					{
						var attribute = $"integrity=\"{linkTag.Integrity}\"";
						resultStringList.Add(attribute);
					}
				}
				#endregion

				return "<link " + String.Join(" ", resultStringList).Trim() + " />";
			}
		}

		public static string GenerateTagsFromObject(MetaTagInclude metaTag)
		{
			var resultStringList = new List<String>();
			if (!String.IsNullOrWhiteSpace(metaTag.Name))
			{
				resultStringList.Add($"name=\"{metaTag.Name}\"");
			}
			if (!String.IsNullOrWhiteSpace(metaTag.Content))
			{
				resultStringList.Add($"content=\"{metaTag.Content}\"");
			}
			if (!String.IsNullOrWhiteSpace(metaTag.Property))
			{
				resultStringList.Add($"property=\"{metaTag.Property}\"");
			}
			if (!String.IsNullOrWhiteSpace(metaTag.Charset))
			{
				resultStringList.Add($"charset=\"{metaTag.Charset}\"");
			}
			return "<meta " + String.Join(" ", resultStringList).Trim() + " >";
		}
	}
}
