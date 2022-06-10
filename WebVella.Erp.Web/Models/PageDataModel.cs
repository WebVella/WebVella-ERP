using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Eql;
using WebVella.Erp.Web.Service;
using WebVella.Erp.Web.Services;

namespace WebVella.Erp.Web.Models
{
	public class PageDataModel
	{
		internal bool SafeCodeDataVariable { get; set; } = false;

		DataSourceManager dsMan = new DataSourceManager();
		RecordManager recMan = new RecordManager();
		EntityRelationManager relMan = new EntityRelationManager();
		EntityManager entMan = new EntityManager();
		BaseErpPageModel erpPageModel = null;
		private Dictionary<string, MPW> Properties = new Dictionary<string, MPW>();

		internal PageDataModel(BaseErpPageModel erpPageModel)
		{
			this.erpPageModel = erpPageModel ?? throw new NullReferenceException(nameof(erpPageModel));
			InitContextRelatedData(erpPageModel);
			if (erpPageModel.ErpRequestContext != null && erpPageModel.ErpRequestContext.Page != null)
				InitDataSources(erpPageModel.ErpRequestContext.Page);
		}

		private void InitContextRelatedData(BaseErpPageModel erpPageModel)
		{
			if (erpPageModel != null)
			{
				//if page request context is set, it is used, otherwise, the requestContext is used
				ErpRequestContext reqCtx = erpPageModel.ErpRequestContext;

				var currentUserMPW = new MPW(MPT.Object, erpPageModel.CurrentUser);
				Properties.Add("CurrentUser", currentUserMPW);
				if (erpPageModel.CurrentUser != null)
				{
					currentUserMPW.Properties.Add("CreatedOn", new MPW(MPT.Object, erpPageModel.CurrentUser.CreatedOn));
					currentUserMPW.Properties.Add("Email", new MPW(MPT.Object, erpPageModel.CurrentUser.Email));
					currentUserMPW.Properties.Add("FirstName", new MPW(MPT.Object, erpPageModel.CurrentUser.FirstName));
					currentUserMPW.Properties.Add("Id", new MPW(MPT.Object, erpPageModel.CurrentUser.Id));
					currentUserMPW.Properties.Add("Image", new MPW(MPT.Object, erpPageModel.CurrentUser.Image));
					currentUserMPW.Properties.Add("LastName", new MPW(MPT.Object, erpPageModel.CurrentUser.LastName));
					currentUserMPW.Properties.Add("Username", new MPW(MPT.Object, erpPageModel.CurrentUser.Username));

					var rolesMPW = new MPW(MPT.Object, erpPageModel.CurrentUser.Roles);
					currentUserMPW.Properties.Add("Roles", rolesMPW);
					if (erpPageModel.CurrentUser.Roles != null)
					{
						for (int i = 0; i < erpPageModel.CurrentUser.Roles.Count; i++)
						{
							var roleMPW = new MPW(MPT.Object, erpPageModel.CurrentUser.Roles[i]);
							rolesMPW.Properties.Add($"[{i}]", roleMPW);
							roleMPW.Properties.Add($"Id", new MPW(MPT.Object, erpPageModel.CurrentUser.Roles[i].Id));
							roleMPW.Properties.Add($"Name", new MPW(MPT.Object, erpPageModel.CurrentUser.Roles[i].Name));
						}
					}
				}

				Properties.Add("ReturnUrl", new MPW(MPT.Object, erpPageModel.ReturnUrl));

				//this is the case of with related/parent entity and related/parent record id set
				if (erpPageModel.RecordId != null && erpPageModel.ParentRecordId != null && erpPageModel.RelationId != null &&
					reqCtx != null && reqCtx.Entity != null && reqCtx.ParentEntity != null)
				{
					EntityQuery eq = new EntityQuery(reqCtx.ParentEntity.Name, "*", EntityQuery.QueryEQ("id", erpPageModel.ParentRecordId.Value));
					var response = recMan.Find(eq);
					if (!response.Success)
						throw new Exception(response.Message);
					else if (response.Object.Data.Count > 0)
						Properties.Add("ParentRecord", new MPW(MPT.EntityRecord, response.Object.Data[0]));
					else
						Properties.Add("ParentRecord", new MPW(MPT.EntityRecord, null));

					eq = new EntityQuery(reqCtx.Entity.Name, "*", EntityQuery.QueryEQ("id", erpPageModel.RecordId.Value));
					response = recMan.Find(eq);
					if (!response.Success)
						throw new Exception(response.Message);
					else if (response.Object.Data.Count > 0)
						Properties.Add("Record", new MPW(MPT.EntityRecord, response.Object.Data[0]));
					else
						Properties.Add("Record", new MPW(MPT.EntityRecord, null));
				}

				//this is the case of Create and List page with related/parent entity amd with no related/parent record set
				else if (erpPageModel.RecordId == null && erpPageModel.ParentRecordId != null && erpPageModel.RelationId != null &&
					reqCtx != null && reqCtx.Entity != null && reqCtx.ParentEntity != null)
				{
					EntityQuery eq = new EntityQuery(reqCtx.ParentEntity.Name, "*", EntityQuery.QueryEQ("id", erpPageModel.ParentRecordId));
					var response = recMan.Find(eq);
					if (!response.Success)
						throw new Exception(response.Message);
					else if (response.Object.Data.Count > 0)
						Properties.Add("ParentRecord", new MPW(MPT.EntityRecord, response.Object.Data[0]));
					else
						Properties.Add("ParentRecord", new MPW(MPT.EntityRecord, null));

					Properties.Add("Record", new MPW(MPT.EntityRecord, null));
				}

				//this is the case with no parent (relations/parents cases are checked above)
				else if (erpPageModel.RecordId != null && reqCtx != null && reqCtx.Entity != null)
				{
					EntityQuery eq = new EntityQuery(reqCtx.Entity.Name, "*", EntityQuery.QueryEQ("id", reqCtx.RecordId));
					var response = recMan.Find(eq);
					if (!response.Success)
						throw new Exception(response.Message);
					else if (response.Object.Data.Count > 0)
						Properties.Add("Record", new MPW(MPT.EntityRecord, response.Object.Data[0]));
					else
						Properties.Add("Record", new MPW(MPT.EntityRecord, null));

					Properties.Add("ParentRecord", new MPW(MPT.EntityRecord, null));
				}

				//this is the case with no entity page
				else
				{
					Properties.Add("Record", new MPW(MPT.EntityRecord, null));
					Properties.Add("ParentRecord", new MPW(MPT.EntityRecord, null));
				}

				var validationMPW = new MPW(MPT.Object, erpPageModel.Validation);
				if (erpPageModel.Validation != null)
				{
					validationMPW.Properties.Add("Message", new MPW(MPT.Object, erpPageModel.Validation.Message));
					var errorsMPW = new MPW(MPT.Object, erpPageModel.Validation.Errors);
					if (erpPageModel.Validation.Errors != null)
					{
						for (int i = 0; i < erpPageModel.Validation.Errors.Count; i++)
						{
							var errorMPW = new MPW(MPT.Object, erpPageModel.Validation.Errors[i]);
							errorsMPW.Properties.Add($"[{i}]", errorMPW);
							errorMPW.Properties.Add($"Index", new MPW(MPT.Object, erpPageModel.Validation.Errors[i].Index));
							errorMPW.Properties.Add($"IsSystem", new MPW(MPT.Object, erpPageModel.Validation.Errors[i].IsSystem));
							errorMPW.Properties.Add($"Message", new MPW(MPT.Object, erpPageModel.Validation.Errors[i].Message));
							errorMPW.Properties.Add($"PropertyName", new MPW(MPT.Object, erpPageModel.Validation.Errors[i].PropertyName));
						}
					}
					validationMPW.Properties.Add("Errors", errorsMPW);

				}
				Properties.Add("Validation", validationMPW);


				if (reqCtx != null)
				{
					Properties.Add("RecordId", new MPW(MPT.Object, reqCtx.RecordId));
					Properties.Add("ParentRecordId", new MPW(MPT.Object, reqCtx.ParentRecordId));
					Properties.Add("RelationId", new MPW(MPT.Object, reqCtx.RelationId));
					Properties.Add("PageContext", new MPW(MPT.Object, reqCtx.PageContext));

					var pageMPW = new MPW(MPT.Object, reqCtx.Page);
					if (reqCtx.Page != null)
					{
						pageMPW.Properties.Add("AppId", new MPW(MPT.Object, reqCtx.Page.AppId));
						pageMPW.Properties.Add("AreaId", new MPW(MPT.Object, reqCtx.Page.AreaId));
						pageMPW.Properties.Add("EntityId", new MPW(MPT.Object, reqCtx.Page.EntityId));
						pageMPW.Properties.Add("IconClass", new MPW(MPT.Object, reqCtx.Page.IconClass));
						pageMPW.Properties.Add("Id", new MPW(MPT.Object, reqCtx.Page.Id));
						pageMPW.Properties.Add("IsRazorBody", new MPW(MPT.Object, reqCtx.Page.IsRazorBody));
						pageMPW.Properties.Add("Label", new MPW(MPT.Object, reqCtx.Page.Label));
						pageMPW.Properties.Add("Name", new MPW(MPT.Object, reqCtx.Page.Name));
						pageMPW.Properties.Add("NodeId", new MPW(MPT.Object, reqCtx.Page.NodeId));
						pageMPW.Properties.Add("System", new MPW(MPT.Object, reqCtx.Page.System));
						pageMPW.Properties.Add("Type", new MPW(MPT.Object, reqCtx.Page.Type));
						pageMPW.Properties.Add("Weight", new MPW(MPT.Object, reqCtx.Page.Weight));
					}
					Properties.Add("Page", pageMPW);

					var parentPageMPW = new MPW(MPT.Object, reqCtx.ParentPage);
					if (reqCtx.ParentPage != null)
					{
						parentPageMPW.Properties.Add("AppId", new MPW(MPT.Object, reqCtx.ParentPage.AppId));
						parentPageMPW.Properties.Add("AreaId", new MPW(MPT.Object, reqCtx.ParentPage.AreaId));
						parentPageMPW.Properties.Add("EntityId", new MPW(MPT.Object, reqCtx.ParentPage.EntityId));
						parentPageMPW.Properties.Add("IconClass", new MPW(MPT.Object, reqCtx.ParentPage.IconClass));
						parentPageMPW.Properties.Add("Id", new MPW(MPT.Object, reqCtx.ParentPage.Id));
						parentPageMPW.Properties.Add("IsRazorBody", new MPW(MPT.Object, reqCtx.ParentPage.IsRazorBody));
						parentPageMPW.Properties.Add("Label", new MPW(MPT.Object, reqCtx.ParentPage.Label));
						parentPageMPW.Properties.Add("Name", new MPW(MPT.Object, reqCtx.ParentPage.Name));
						parentPageMPW.Properties.Add("NodeId", new MPW(MPT.Object, reqCtx.ParentPage.NodeId));
						parentPageMPW.Properties.Add("System", new MPW(MPT.Object, reqCtx.ParentPage.System));
						parentPageMPW.Properties.Add("Type", new MPW(MPT.Object, reqCtx.ParentPage.Type));
						parentPageMPW.Properties.Add("Weight", new MPW(MPT.Object, reqCtx.ParentPage.Weight));
					}
					Properties.Add("ParentPage", parentPageMPW);

					var entityMPW = new MPW(MPT.Object, reqCtx.Entity);
					if (reqCtx.Entity != null)
					{
						entityMPW.Properties.Add("Color", new MPW(MPT.Object, reqCtx.Entity.Color));
						entityMPW.Properties.Add("Fields", new MPW(MPT.Object, reqCtx.Entity.Fields));
						entityMPW.Properties.Add("IconName", new MPW(MPT.Object, reqCtx.Entity.IconName));
						entityMPW.Properties.Add("Id", new MPW(MPT.Object, reqCtx.Entity.Id));
						entityMPW.Properties.Add("Label", new MPW(MPT.Object, reqCtx.Entity.Label));
						entityMPW.Properties.Add("LabelPlural", new MPW(MPT.Object, reqCtx.Entity.LabelPlural));
						entityMPW.Properties.Add("Name", new MPW(MPT.Object, reqCtx.Entity.Name));
						entityMPW.Properties.Add("RecordScreenIdField", new MPW(MPT.Object, reqCtx.Entity.RecordScreenIdField));
						entityMPW.Properties.Add("System", new MPW(MPT.Object, reqCtx.Entity.System));
					}
					Properties.Add("Entity", entityMPW);

					var parentEntityMPW = new MPW(MPT.Object, reqCtx.ParentEntity);
					if (reqCtx.ParentEntity != null)
					{
						parentEntityMPW.Properties.Add("Color", new MPW(MPT.Object, reqCtx.ParentEntity.Color));
						parentEntityMPW.Properties.Add("Fields", new MPW(MPT.Object, reqCtx.ParentEntity.Fields));
						parentEntityMPW.Properties.Add("IconName", new MPW(MPT.Object, reqCtx.ParentEntity.IconName));
						parentEntityMPW.Properties.Add("Id", new MPW(MPT.Object, reqCtx.ParentEntity.Id));
						parentEntityMPW.Properties.Add("Label", new MPW(MPT.Object, reqCtx.ParentEntity.Label));
						parentEntityMPW.Properties.Add("LabelPlural", new MPW(MPT.Object, reqCtx.ParentEntity.LabelPlural));
						parentEntityMPW.Properties.Add("Name", new MPW(MPT.Object, reqCtx.ParentEntity.Name));
						parentEntityMPW.Properties.Add("RecordScreenIdField", new MPW(MPT.Object, reqCtx.ParentEntity.RecordScreenIdField));
						parentEntityMPW.Properties.Add("System", new MPW(MPT.Object, reqCtx.ParentEntity.System));
					}
					Properties.Add("ParentEntity", parentEntityMPW);

					var detectionMPW = new MPW(MPT.Object, reqCtx.Detection);
					if (reqCtx.Detection != null)
					{
						var deviceMPW = new MPW(MPT.Object, reqCtx.Detection.Device);
						if (reqCtx.Detection.Device != null)
						{
							deviceMPW.Properties.Add("Crawler", new MPW(MPT.Object, reqCtx.Detection.Device.Crawler));
							deviceMPW.Properties.Add("Type", new MPW(MPT.Object, reqCtx.Detection.Device.Type));
						}
						detectionMPW.Properties.Add("Device", deviceMPW);
					}
					Properties.Add("Detection", detectionMPW);

					var appMPW = new MPW(MPT.Object, reqCtx.App);
					if (reqCtx.App != null)
					{
						appMPW.Properties.Add("Author", new MPW(MPT.Object, reqCtx.App.Author));
						appMPW.Properties.Add("Color", new MPW(MPT.Object, reqCtx.App.Color));
						appMPW.Properties.Add("Description", new MPW(MPT.Object, reqCtx.App.Description));
						appMPW.Properties.Add("IconClass", new MPW(MPT.Object, reqCtx.App.IconClass));
						appMPW.Properties.Add("Id", new MPW(MPT.Object, reqCtx.App.Id));
						appMPW.Properties.Add("Label", new MPW(MPT.Object, reqCtx.App.Label));
						appMPW.Properties.Add("Name", new MPW(MPT.Object, reqCtx.App.Name));
						var sitemapMPW = new MPW(MPT.Object, reqCtx.App.Sitemap);
						if (reqCtx.App.Sitemap != null)
						{
							var areasMPW = new MPW(MPT.Object, reqCtx.App.Sitemap.Areas);
							sitemapMPW.Properties.Add("Areas", sitemapMPW);
							for (int i = 0; i < reqCtx.App.Sitemap.Areas.Count; i++)
							{
								var area = reqCtx.App.Sitemap.Areas[i];
								var areaMPW = new MPW(MPT.Object, area);
								areaMPW.Properties.Add($"Access", new MPW(MPT.Object, area.Access));
								areaMPW.Properties.Add($"Color", new MPW(MPT.Object, area.Color));
								areaMPW.Properties.Add($"Description", new MPW(MPT.Object, area.Description));
								areaMPW.Properties.Add($"IconClass", new MPW(MPT.Object, area.IconClass));
								areaMPW.Properties.Add($"Id", new MPW(MPT.Object, area.Id));
								areaMPW.Properties.Add($"Label", new MPW(MPT.Object, area.Label));
								areaMPW.Properties.Add($"Name", new MPW(MPT.Object, area.Name));

								var areaNodesMPW = new MPW(MPT.Object, area.Nodes);
								for (int j = 0; j < area.Nodes.Count; j++)
								{
									var node = area.Nodes[j];
									var nodeMPW = new MPW(MPT.Object, area);
									nodeMPW.Properties.Add($"Access", new MPW(MPT.Object, node.Access));
									nodeMPW.Properties.Add($"EntityId", new MPW(MPT.Object, node.EntityId));
									nodeMPW.Properties.Add($"GroupName", new MPW(MPT.Object, node.GroupName));
									nodeMPW.Properties.Add($"IconClass", new MPW(MPT.Object, node.IconClass));
									nodeMPW.Properties.Add($"Id", new MPW(MPT.Object, node.Id));
									nodeMPW.Properties.Add($"Label", new MPW(MPT.Object, node.Label));
									nodeMPW.Properties.Add($"Name", new MPW(MPT.Object, node.Name));
									nodeMPW.Properties.Add($"Type", new MPW(MPT.Object, node.Type));
									nodeMPW.Properties.Add($"Url", new MPW(MPT.Object, node.Url));
									nodeMPW.Properties.Add($"Weight", new MPW(MPT.Object, node.Weight));
									areaNodesMPW.Properties.Add($"[{j}]", nodeMPW);
								}
								areaMPW.Properties.Add($"Nodes", areaNodesMPW);
								areasMPW.Properties.Add($"[{i}]", areaMPW);
							}
						}
						appMPW.Properties.Add("Sitemap", sitemapMPW);
					}
					Properties.Add("App", appMPW);

					var sitemapNodeMPW = new MPW(MPT.Object, reqCtx.SitemapNode);
					if (reqCtx.SitemapNode != null)
					{
						sitemapNodeMPW.Properties.Add($"Access", new MPW(MPT.Object, reqCtx.SitemapNode.Access));
						sitemapNodeMPW.Properties.Add($"EntityId", new MPW(MPT.Object, reqCtx.SitemapNode.EntityId));
						sitemapNodeMPW.Properties.Add($"GroupName", new MPW(MPT.Object, reqCtx.SitemapNode.GroupName));
						sitemapNodeMPW.Properties.Add($"IconClass", new MPW(MPT.Object, reqCtx.SitemapNode.IconClass));
						sitemapNodeMPW.Properties.Add($"Id", new MPW(MPT.Object, reqCtx.SitemapNode.Id));
						sitemapNodeMPW.Properties.Add($"Label", new MPW(MPT.Object, reqCtx.SitemapNode.Label));
						sitemapNodeMPW.Properties.Add($"Name", new MPW(MPT.Object, reqCtx.SitemapNode.Name));
						sitemapNodeMPW.Properties.Add($"Type", new MPW(MPT.Object, reqCtx.SitemapNode.Type));
						sitemapNodeMPW.Properties.Add($"Url", new MPW(MPT.Object, reqCtx.SitemapNode.Url));
						sitemapNodeMPW.Properties.Add($"Weight", new MPW(MPT.Object, reqCtx.SitemapNode.Weight));
					}
					Properties.Add("SitemapNode", sitemapNodeMPW);

					var sitemapAreaMPW = new MPW(MPT.Object, reqCtx.SitemapArea);
					if (reqCtx.SitemapArea != null)
					{
						var area = reqCtx.SitemapArea;
						sitemapAreaMPW.Properties.Add($"Access", new MPW(MPT.Object, area.Access));
						sitemapAreaMPW.Properties.Add($"Color", new MPW(MPT.Object, area.Color));
						sitemapAreaMPW.Properties.Add($"Description", new MPW(MPT.Object, area.Description));
						sitemapAreaMPW.Properties.Add($"IconClass", new MPW(MPT.Object, area.IconClass));
						sitemapAreaMPW.Properties.Add($"Id", new MPW(MPT.Object, area.Id));
						sitemapAreaMPW.Properties.Add($"Label", new MPW(MPT.Object, area.Label));
						sitemapAreaMPW.Properties.Add($"Name", new MPW(MPT.Object, area.Name));

						var areaNodesMPW = new MPW(MPT.Object, area.Nodes);
						for (int j = 0; j < area.Nodes.Count; j++)
						{
							var node = area.Nodes[j];
							var nodeMPW = new MPW(MPT.Object, area);
							nodeMPW.Properties.Add($"Access", new MPW(MPT.Object, node.Access));
							nodeMPW.Properties.Add($"EntityId", new MPW(MPT.Object, node.EntityId));
							nodeMPW.Properties.Add($"GroupName", new MPW(MPT.Object, node.GroupName));
							nodeMPW.Properties.Add($"IconClass", new MPW(MPT.Object, node.IconClass));
							nodeMPW.Properties.Add($"Id", new MPW(MPT.Object, node.Id));
							nodeMPW.Properties.Add($"Label", new MPW(MPT.Object, node.Label));
							nodeMPW.Properties.Add($"Name", new MPW(MPT.Object, node.Name));
							nodeMPW.Properties.Add($"Type", new MPW(MPT.Object, node.Type));
							nodeMPW.Properties.Add($"Url", new MPW(MPT.Object, node.Url));
							nodeMPW.Properties.Add($"Weight", new MPW(MPT.Object, node.Weight));
							areaNodesMPW.Properties.Add($"[{j}]", nodeMPW);
						}
						sitemapAreaMPW.Properties.Add($"Nodes", areaNodesMPW);
					}
					Properties.Add("SitemapArea", sitemapAreaMPW);
				}

				EntityRecord queryRecord = new EntityRecord();
				if (reqCtx != null && reqCtx.PageContext != null && reqCtx.PageContext.HttpContext != null)
				{
					var httpCtx = reqCtx.PageContext.HttpContext;
					if (httpCtx.Request != null && httpCtx.Request.Query != null)
					{
						foreach (var key in httpCtx.Request.Query.Keys)
							queryRecord[key] = ((string)httpCtx.Request.Query[key]) ?? string.Empty;
					}
				}
				Properties.Add("RequestQuery", new MPW(MPT.EntityRecord, queryRecord));
			}
			else
			{
				Properties.Add("CurrentUser", new MPW(MPT.Object, null));
				Properties.Add("RecordId", new MPW(MPT.Object, null));
				Properties.Add("ParentRecordId", new MPW(MPT.Object, null));
				Properties.Add("Record", new MPW(MPT.EntityRecord, null));
				Properties.Add("ParentRecord", new MPW(MPT.EntityRecord, null));
				Properties.Add("ReturnUrl", new MPW(MPT.Object, null));
			}

			Properties.Add("RowRecord", new MPW(MPT.EntityRecord, null));
		}

		public void SetRowRecord(EntityRecord rowRecord)
		{
			Properties["RowRecord"] = new MPW(MPT.EntityRecord, rowRecord);
		}

		public void SetRecord(EntityRecord record)
		{
			Properties["Record"] = new MPW(MPT.EntityRecord, record);
		}

		private void InitDataSources(ErpPage page)
		{
			var pageDataSources = new PageService().GetPageDataSources(page.Id);
			var allDatasources = new DataSourceManager().GetAll();
			foreach (var pageDS in pageDataSources)
			{
				var ds = allDatasources.SingleOrDefault(x => x.Id == pageDS.DataSourceId);
				//if data source was deleted (and system validation for usage failed)
				//we hide this page data source
				if (ds == null)
					continue;

				Properties[pageDS.Name] = new MPW(MPT.DataSource, new DSW { DataSource = ds, PageDataSource = pageDS });
			}

		}

		public object GetPropertyValueByDataSource(string text)
		{
			if (string.IsNullOrWhiteSpace(text))
				return text;

			if (text.Trim().StartsWith("{"))
			{
				DataSourceVariable variable = null;
				try
				{
					variable = JsonConvert.DeserializeObject<DataSourceVariable>(text, new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Error });
				}
				catch
				{
					return text;
				}

				if (variable != null)
					return GetPropertyValueByDataSource(variable);
			}

			return text;
		}

		public object GetPropertyValueByDataSource(DataSourceVariable variable)
		{
			if (variable == null)
				throw new NullReferenceException(nameof(variable));

			try
			{
				object result = null;
				switch (variable.Type)
				{
					case DataSourceVariableType.DATASOURCE:
						result = GetProperty(variable.String);
						break;
					case DataSourceVariableType.CODE:
						if (SafeCodeDataVariable)
						{
							try { result = CodeEvalService.Evaluate(variable.String, erpPageModel); } catch { result = null; }
						}
						else
						{
							result = CodeEvalService.Evaluate(variable.String, erpPageModel);
						}
						break;
					case DataSourceVariableType.HTML:
						result = variable.String;
						break;
					case DataSourceVariableType.SNIPPET:
						if (SafeCodeDataVariable)
						{
							var snippet = SnippetService.GetSnippet(variable.String);
							if (snippet == null)
								result = $"Snippet '{variable.String}' is not found.";
							else
							{
								if (snippet.Name.ToLowerInvariant().EndsWith(".cs"))
								{
									string csCode = snippet.GetText();
									try { result = CodeEvalService.Evaluate(csCode, erpPageModel); } catch { result = null; }
								}
								else
								{
									result = snippet.GetText();
								}
							}
						}
						else
						{
							var snippet = SnippetService.GetSnippet(variable.String);
							if (snippet == null)
								result = $"Snippet '{variable.String}' is not found.";
							else
							{
								if (snippet.Name.ToLowerInvariant().EndsWith(".cs"))
								{
									string csCode = snippet.GetText();
									result = CodeEvalService.Evaluate(csCode, erpPageModel);
								}
								else
								{
									result = snippet.GetText();
								}
							}
						}
						break;
					default:
						throw new NotSupportedException(variable.Type.ToString());
				}
				if (result is string)
				{
					if (string.IsNullOrWhiteSpace(result as string))
						return variable.Default;
				}
				else
				{
					if (result is null)
						return variable.Default;
				}

				return result;
			}
			catch (PropertyDoesNotExistException)
			{
				return variable.Default;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public object GetProperty(string propName)
		{
			//Stopwatch sw = new Stopwatch();
			//sw.Start();
			//try
			//{
			if (string.IsNullOrWhiteSpace(propName))
				throw new ArgumentException(nameof(propName));

			//replace keyword $index with 0
			var name = propName.Trim().Replace("$index", "0");
			string[] tmpPropChain = name.Split(".", StringSplitOptions.RemoveEmptyEntries);

			List<string> completePropChain = new List<string>();
			foreach (var pName in tmpPropChain)
			{
				var indexerIdx = pName.IndexOf("[");
				if (indexerIdx != -1)
				{
					var n = pName.Substring(0, indexerIdx);
					var idx = pName.Substring(indexerIdx, pName.Length - indexerIdx);
					completePropChain.Add(n);
					completePropChain.Add(idx);
				}
				else
					completePropChain.Add(pName);
			}

			MPW mpw = null;
			var currentPropDict = Properties;
			var currentPropertyNamePath = string.Empty;
			string parentPropName = string.Empty;
			foreach (var ppName in completePropChain)
			{

				var pName = ppName.Trim();
				if (string.IsNullOrWhiteSpace(currentPropertyNamePath))
					currentPropertyNamePath = pName;
				else
					currentPropertyNamePath += $".{pName}";

				//try to get property with full key (after http post object are entered with no . split
				if (parentPropName == "Record")
				{
					var testName = propName.Trim().Substring(7); //cut the Record. in front
					if (pName != testName && currentPropDict.ContainsKey(testName))
						return currentPropDict[testName].Value;
				}

				//try to get property with full key (after http post object are entered with no . split
				if (parentPropName == "RelatedRecord")
				{
					var testName = propName.Trim().Substring(14); //cut the RelatedRecord. in front
					if (pName != testName && currentPropDict.ContainsKey(testName))
						return currentPropDict[testName].Value;
				}

				if (!currentPropDict.ContainsKey(pName))
				{
					if (!currentPropertyNamePath.EndsWith("]"))
						throw new PropertyDoesNotExistException($"Property name '{currentPropertyNamePath}' not found.");
					else
						throw new PropertyDoesNotExistException($"Property name is found, but list index is out of bounds.");
				}


				mpw = currentPropDict[pName];
				if (mpw != null && mpw.Type == MPT.DataSource)
				{
					DSW dsw = mpw.Value as DSW;
					if (dsw != null)
					{
						var result = ExecDataSource(dsw);
						if (result is List<EntityRecord> || result is EntityRecordList)
						{
							mpw = new MPW(MPT.ListEntityRecords, result);
							currentPropDict[pName] = mpw;
						}
						else if (result is EntityRecord)
						{
							mpw = new MPW(MPT.EntityRecord, result);
							currentPropDict[pName] = mpw;
						}
						else
						{
							mpw = new MPW(MPT.Object, result);
							currentPropDict[pName] = mpw;
						}
					}
				}
				currentPropDict = mpw.Properties;
				parentPropName = ppName;
			}

			return mpw.Value;
			//}
			//finally {
			//	Debug.WriteLine(">>>>>>>>>> " + propName + " >> " + sw.ElapsedMilliseconds);
			//}
		}

		private object ExecDataSource(DSW dsWrapper)
		{
			if (dsWrapper.DataSource.Type == DataSourceType.CODE)
			{
				Dictionary<string, object> arguments = new Dictionary<string, object>();
				if (dsWrapper.DataSource.Parameters != null)
				{
					foreach (var par in dsWrapper.DataSource.Parameters)
					{
						var pageDSParam = dsWrapper.PageDataSource.Parameters.SingleOrDefault(x => x.Name == par.Name);
						string value = par.Value;
						if (pageDSParam != null)
						{
							value = ProcessParameterValue(pageDSParam.Value);
							if (string.IsNullOrWhiteSpace(value))
							{
								value = ProcessParameterValue(par.Value);
								value = CheckProcessDefaultValue(value);
								var ds = dsMan.Get(dsWrapper.PageDataSource.DataSourceId);
								var dsParam = ds.Parameters.SingleOrDefault(x => x.Name == par.Name);
								if (dsParam != null)
									arguments[dsParam.Name] = dsMan.GetDataSourceParameterValue(new DataSourceParameter { Name = dsParam.Name, Type = dsParam.Type, Value = value });
								else
									arguments[par.Name] = value;
							}
							else
							{
								value = CheckProcessDefaultValue(value);
								arguments[pageDSParam.Name] = dsMan.GetDataSourceParameterValue(new DataSourceParameter { Name = pageDSParam.Name, Type = pageDSParam.Type, Value = value });
							}
						}
						else
						{
							value = ProcessParameterValue(value);
							value = CheckProcessDefaultValue(value);
							var ds = dsMan.Get(dsWrapper.PageDataSource.DataSourceId);
							var dsParam = ds.Parameters.SingleOrDefault(x => x.Name == par.Name);
							if (dsParam != null)
								arguments[dsParam.Name] = dsMan.GetDataSourceParameterValue(new DataSourceParameter { Name = dsParam.Name, Type = dsParam.Type, Value = value });
							else
								arguments[par.Name] = value;
						}
					}
				}

				arguments["PageModel"] = this;
				var codeDS = (CodeDataSource)dsWrapper.DataSource;
				if (SafeCodeDataVariable)
					try { return codeDS.Execute(arguments); } catch { return null; }
				else
					return codeDS.Execute(arguments);
			}
			else if (dsWrapper.DataSource.Type == DataSourceType.DATABASE)
			{
				var eqlParameters = new List<EqlParameter>();
				if (dsWrapper.DataSource.Parameters != null)
				{

					foreach (var par in dsWrapper.DataSource.Parameters)
					{
						var pageDSParam = dsWrapper.PageDataSource.Parameters.SingleOrDefault(x => x.Name == par.Name);
						string value = par.Value;
						if (pageDSParam != null)
						{
							value = ProcessParameterValue(pageDSParam.Value);
							if (string.IsNullOrWhiteSpace(value))
							{
								value = ProcessParameterValue(par.Value);
								value = CheckProcessDefaultValue(value);
								var ds = dsMan.Get(dsWrapper.PageDataSource.DataSourceId);
								var dsParam = ds.Parameters.SingleOrDefault(x => x.Name == par.Name);
								if (dsParam != null)
									eqlParameters.Add(dsMan.ConvertDataSourceParameterToEqlParameter(new DataSourceParameter { Name = dsParam.Name, Type = dsParam.Type, Value = value }));
								else
									eqlParameters.Add(new EqlParameter(par.Name, value));
							}
							else
							{
								value = CheckProcessDefaultValue(value);
								eqlParameters.Add(dsMan.ConvertDataSourceParameterToEqlParameter(new DataSourceParameter { Name = pageDSParam.Name, Type = pageDSParam.Type, Value = value }));
							}
						}
						else
						{
							value = ProcessParameterValue(value);
							value = CheckProcessDefaultValue(value);
							var ds = dsMan.Get(dsWrapper.PageDataSource.DataSourceId);
							var dsParam = ds.Parameters.SingleOrDefault(x => x.Name == par.Name);
							if (dsParam != null)
								eqlParameters.Add(dsMan.ConvertDataSourceParameterToEqlParameter(new DataSourceParameter { Name = dsParam.Name, Type = dsParam.Type, Value = value }));
							else
								eqlParameters.Add(new EqlParameter(par.Name, value));
						}
					}
				}

				DatabaseDataSource dbDs = (DatabaseDataSource)dsWrapper.DataSource;
				return dsMan.Execute(dbDs.Id, eqlParameters);
			}
			else
				throw new Exception("Not supported data source type.");
		}

		private string CheckProcessDefaultValue(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				switch (value.ToLowerInvariant())
				{
					case "string.empty":
						return string.Empty;
					case "guid.empty":
						return Guid.Empty.ToString();
				}
			}
			return value;
		}

		private string ProcessParameterValue(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				return value;


			Dictionary<string, string> replacementDict = new Dictionary<string, string>();

			var foundTags = Regex.Matches(value, @"(?<=\{\{)[^}]*(?=\}\})").Cast<Match>().Select(match => match.Value).Distinct().ToList();
			foreach (var tag in foundTags)
			{
				var processedTag = tag.Replace("{{", "").Replace("}}", "").Trim();
				var defaultValue = "";
				if (processedTag.Contains("??"))
				{
					//this is a tag with a default value
					int questionMarksLocation = processedTag.IndexOf("??");
					var tagValue = processedTag.Substring(0, questionMarksLocation).Trim();
					var tagDefault = processedTag.Substring(questionMarksLocation + 2).Trim().Replace("\"", "").Replace("'", "");
					processedTag = tagValue;
					defaultValue = tagDefault;
				}

				try
				{
					var propValue = GetProperty(processedTag);
					replacementDict["{{" + tag + "}}"] = (propValue ?? defaultValue).ToString();
				}
				catch (PropertyDoesNotExistException)
				{
					replacementDict["{{" + tag + "}}"] = defaultValue;
				}
			}

			string result = value;
			foreach (var key in replacementDict.Keys)
				result = result.Replace(key, replacementDict[key]);


			return result;
		}

		public object this[string key]
		{
			get { return GetProperty(key); }
		}

		#region <--- Private types --->

		//ModelPropertyType
		private enum MPT
		{
			Object,
			DataSource,
			EntityRecord,
			ListEntityRecords
		}

		//DataSourceWrapper
		private class DSW
		{
			public DataSourceBase DataSource { get; set; }

			public PageDataSource PageDataSource { get; set; }
		}

		//ModelPropertyWrapper
		private class MPW
		{
			public MPT Type { get; set; }

			public object Value { get; set; }

			public Dictionary<string, MPW> Properties { get; private set; } = new Dictionary<string, MPW>();

			public MPW(MPT type, object value)
			{
				Type = type;
				Value = value;

				if (Type == MPT.ListEntityRecords)
				{
					List<EntityRecord> records = Value as List<EntityRecord>;
					if (records != null)
					{
						for (int i = 0; i < records.Count; i++)
						{
							var recMPW = new MPW(MPT.EntityRecord, records[i]);
							Properties[$"[{i}]"] = recMPW;
						}
					}
				}
				else if (Type == MPT.EntityRecord)
				{
					EntityRecord record = Value as EntityRecord;
					if (record != null)
					{
						foreach (var propName in record.Properties.Keys)
						{
							var propValue = record[propName.Trim()];
							//the case when set record from page post
							if (propName.StartsWith("$") && propName.Contains(".") && propValue is List<Guid>)
							{
								string[] split = propName.Split('.');
								List<EntityRecord> records = new List<EntityRecord>();
								foreach (Guid id in (List<Guid>)propValue)
								{
									EntityRecord rec = new EntityRecord();
									rec["id"] = id;
									records.Add(rec);
								}
								Properties[split[0]] = new MPW(MPT.ListEntityRecords, records);
							}
							else
							{
								if (propValue is List<EntityRecord>)
									Properties[propName] = new MPW(MPT.ListEntityRecords, propValue);
								else if (propValue is EntityRecord)
									Properties[propName] = new MPW(MPT.EntityRecord, propValue);
								else
									Properties[propName] = new MPW(MPT.Object, propValue);
							}
						}
					}
				}
			}
		}
		#endregion
	}

	public class PropertyDoesNotExistException : Exception
	{
		public PropertyDoesNotExistException(string message) : base(message) { }
	}
}
