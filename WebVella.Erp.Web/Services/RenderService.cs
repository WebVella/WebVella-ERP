using CsvHelper;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Web.Services
{
	public class RenderService
	{

		public string GetPathTypeIcon(string filePath)
		{
			var fontAwesomeIconName = "fa-file";
			if (filePath.EndsWith(".txt"))
			{
				fontAwesomeIconName = "fa-file-alt";
			}
			else if (filePath.EndsWith(".pdf"))
			{
				fontAwesomeIconName = "fa-file-pdf";
			}
			else if (filePath.EndsWith(".doc") || filePath.EndsWith(".docx"))
			{
				fontAwesomeIconName = "fa-file-word";
			}
			else if (filePath.EndsWith(".xls") || filePath.EndsWith(".xlsx"))
			{
				fontAwesomeIconName = "fa-file-excel";
			}
			else if (filePath.EndsWith(".ppt") || filePath.EndsWith(".pptx"))
			{
				fontAwesomeIconName = "fa-file-powerpoint";
			}
			else if (filePath.EndsWith(".gif") || filePath.EndsWith(".jpg")
				 || filePath.EndsWith(".jpeg") || filePath.EndsWith(".png")
				 || filePath.EndsWith(".bmp") || filePath.EndsWith(".tif"))
			{
				fontAwesomeIconName = "fa-file-image";
			}
			else if (filePath.EndsWith(".zip") || filePath.EndsWith(".zipx")
				  || filePath.EndsWith(".rar") || filePath.EndsWith(".tar")
					|| filePath.EndsWith(".gz") || filePath.EndsWith(".dmg")
					 || filePath.EndsWith(".iso"))
			{
				fontAwesomeIconName = "fa-file-archive";
			}
			else if (filePath.EndsWith(".wav") || filePath.EndsWith(".mp3")
				  || filePath.EndsWith(".fla") || filePath.EndsWith(".flac")
					|| filePath.EndsWith(".ra") || filePath.EndsWith(".rma")
					 || filePath.EndsWith(".aif") || filePath.EndsWith(".aiff")
					  || filePath.EndsWith(".aa") || filePath.EndsWith(".aac")
						|| filePath.EndsWith(".aax") || filePath.EndsWith(".ac3")
						 || filePath.EndsWith(".au") || filePath.EndsWith(".ogg")
						  || filePath.EndsWith(".avr") || filePath.EndsWith(".3ga")
							|| filePath.EndsWith(".mid") || filePath.EndsWith(".midi")
							 || filePath.EndsWith(".m4a") || filePath.EndsWith(".mp4a")
							  || filePath.EndsWith(".amz") || filePath.EndsWith(".mka")
								|| filePath.EndsWith(".asx") || filePath.EndsWith(".pcm")
								 || filePath.EndsWith(".m3u") || filePath.EndsWith(".wma")
								  || filePath.EndsWith(".xwma"))
			{
				fontAwesomeIconName = "fa-file-audio";
			}
			else if (filePath.EndsWith(".avi") || filePath.EndsWith(".mpg")
				  || filePath.EndsWith(".mp4") || filePath.EndsWith(".mkv")
					|| filePath.EndsWith(".mov") || filePath.EndsWith(".wmv")
					 || filePath.EndsWith(".vp6") || filePath.EndsWith(".264")
					  || filePath.EndsWith(".vid") || filePath.EndsWith(".rv")
						|| filePath.EndsWith(".webm") || filePath.EndsWith(".swf")
						 || filePath.EndsWith(".h264") || filePath.EndsWith(".flv")
						  || filePath.EndsWith(".mk3d") || filePath.EndsWith(".gifv")
							|| filePath.EndsWith(".oggv") || filePath.EndsWith(".3gp")
							 || filePath.EndsWith(".m4v") || filePath.EndsWith(".movie")
							  || filePath.EndsWith(".divx"))
			{
				fontAwesomeIconName = "fa-file-video";
			}
			else if (filePath.EndsWith(".c") || filePath.EndsWith(".cpp")
				  || filePath.EndsWith(".css") || filePath.EndsWith(".js")
				  || filePath.EndsWith(".py") || filePath.EndsWith(".git")
					|| filePath.EndsWith(".cs") || filePath.EndsWith(".cshtml")
					|| filePath.EndsWith(".xml") || filePath.EndsWith(".html")
					 || filePath.EndsWith(".ini") || filePath.EndsWith(".config")
					  || filePath.EndsWith(".json") || filePath.EndsWith(".h"))
			{
				fontAwesomeIconName = "fa-file-code";
			}
			else if (filePath.EndsWith(".exe") || filePath.EndsWith(".jar")
				  || filePath.EndsWith(".dll") || filePath.EndsWith(".bat")
				  || filePath.EndsWith(".pl") || filePath.EndsWith(".scr")
					|| filePath.EndsWith(".msi") || filePath.EndsWith(".app")
					|| filePath.EndsWith(".deb") || filePath.EndsWith(".apk")
					 || filePath.EndsWith(".jar") || filePath.EndsWith(".vb")
					  || filePath.EndsWith(".prg") || filePath.EndsWith(".sh"))
			{
				fontAwesomeIconName = "fa-cogs";
			}
			else if (filePath.EndsWith(".com") || filePath.EndsWith(".net")
				  || filePath.EndsWith(".org") || filePath.EndsWith(".edu")
				  || filePath.EndsWith(".gov") || filePath.EndsWith(".mil")
					|| filePath.EndsWith("/") || filePath.EndsWith(".html")
					|| filePath.EndsWith(".htm") || filePath.EndsWith(".xhtml")
					 || filePath.EndsWith(".jhtml") || filePath.EndsWith(".php")
					  || filePath.EndsWith(".php3") || filePath.EndsWith(".php4")
					 || filePath.EndsWith(".php5") || filePath.EndsWith(".phtml")
					 || filePath.EndsWith(".asp") || filePath.EndsWith(".aspx")
					 || filePath.EndsWith(".aspx") || filePath.EndsWith("?")
					 || filePath.EndsWith("#"))
			{
				fontAwesomeIconName = "fa-globe";
			}
			return fontAwesomeIconName;
		}

		public string RenderHtmlWithTemplate(string template, EntityRecord record = null, ErpRequestContext requestContext = null, ErpAppContext appContext = null)
		{
			var foundTags = Regex.Matches(template, @"(?<=\{\{)[^}]*(?=\}\})").Cast<Match>().Select(match => match.Value).Distinct().ToList();
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

				if (processedTag.StartsWith("Record[") && record != null)
				{
					//{{Record["fieldName"]}}
					var fieldName = processedTag.Replace("Record[\"", "").Replace("\"]", "").ToLowerInvariant();
					if (record.Properties.ContainsKey(fieldName) && record[fieldName] != null)
					{
						template = template.Replace("{{" + tag + "}}", record[fieldName].ToString());
					}
					else
					{
						template = template.Replace("{{" + tag + "}}", defaultValue);
					}
				}
				else if (processedTag.StartsWith("ErpRequestContext.") && requestContext != null)
				{
					var propertyPath = processedTag.Replace("ErpRequestContext.", "");
					var propertyValue = requestContext.GetPropValue(propertyPath);
					if (propertyValue != null)
					{
						template = template.Replace("{{" + tag + "}}", propertyValue.ToString());
					}
					else
					{
						template = template.Replace("{{" + tag + "}}", defaultValue);
					}
				}

				else if (processedTag.StartsWith("ErpAppContext.") && requestContext != null)
				{
					var propertyPath = processedTag.Replace("ErpAppContext.", "");
					var propertyValue = requestContext.GetPropValue(propertyPath);
					if (propertyValue != null)
					{
						template = template.Replace("{{" + tag + "}}", propertyValue.ToString());
					}
					else
					{
						template = template.Replace("{{" + tag + "}}", defaultValue);
					}
				}

				else if (processedTag.StartsWith("ListMeta.") && requestContext != null)
				{
					var propertyPath = processedTag.Replace("ListMeta.", "");
					var propertyValue = requestContext.GetPropValue(propertyPath);
					if (propertyValue != null)
					{
						template = template.Replace("{{" + tag + "}}", propertyValue.ToString());
					}
					else
					{
						template = template.Replace("{{" + tag + "}}", defaultValue);
					}
				}
				else if (processedTag.StartsWith("ViewMeta.") && requestContext != null)
				{
					var propertyPath = processedTag.Replace("ViewMeta.", "");
					var propertyValue = requestContext.GetPropValue(propertyPath);
					if (propertyValue != null)
					{
						template = template.Replace("{{" + tag + "}}", propertyValue.ToString());
					}
					else
					{
						template = template.Replace("{{" + tag + "}}", defaultValue);
					}
				}

				else if (processedTag.StartsWith("erp-allow-roles") && processedTag.Contains("="))
				{
					//Check if "erp-authorize" is present
					var authoriseTag = foundTags.FirstOrDefault(x => x.Contains("erp-authorize"));
					if (authoriseTag != null)
					{
						var tagArray = processedTag.Split('=');
						var rolesCsv = tagArray[1].Replace("\"", "").Replace("\"", "").ToLowerInvariant();
						var rolesList = rolesCsv.Split(',').ToList();
						var currentUser = SecurityContext.CurrentUser;
						if (currentUser == null && !rolesList.Any(x => x == "guest"))
						{
							return "";
						}
						var authorized = false;
						foreach (var askedRole in rolesList)
						{
							if (currentUser.Roles.Any(x => x.Name == askedRole))
							{
								authorized = true;
								break;
							}
						}
						if (!authorized)
						{
							return "";
						}
					}
				}
				else if (processedTag.StartsWith("erp-block-roles") && processedTag.Contains("="))
				{
					//Check if "erp-authorize" is present
					var authoriseTag = foundTags.FirstOrDefault(x => x.Contains("erp-authorize"));
					if (authoriseTag != null)
					{
						var tagArray = processedTag.Split('=');
						var rolesCsv = tagArray[1].Replace("\"", "").Replace("\"", "").ToLowerInvariant();
						var rolesList = rolesCsv.Split(',').ToList();
						var currentUser = SecurityContext.CurrentUser;
						if (currentUser == null && rolesList.Any(x => x == "guest"))
						{
							return "";
						}
						var authorized = true;
						foreach (var askedRole in rolesList)
						{
							if (currentUser.Roles.Any(x => x.Name == askedRole))
							{
								authorized = false;
								break;
							}
						}
						if (!authorized)
						{
							return "";
						}
					}
				}

				else if (processedTag == "CurrentUrlEncoded" && requestContext != null && requestContext.PageContext != null
					 && requestContext.PageContext.HttpContext != null && requestContext.PageContext.HttpContext.Request != null)
				{

					var currentUrl = requestContext.PageContext.HttpContext.Request.Path + requestContext.PageContext.HttpContext.Request.QueryString;
					var propertyValue = HttpUtility.UrlEncode(currentUrl);
					if (!String.IsNullOrWhiteSpace(propertyValue))
					{
						template = template.Replace("{{" + tag + "}}", propertyValue);
					}
					else
					{
						template = template.Replace("{{" + tag + "}}", defaultValue);
					}
				}
			}
			//Tags that depend on others to be already applied
			foreach (var tag in foundTags)
			{
				var processedTag = tag.Replace("{{", "").Replace("}}", "").Trim();
				if (processedTag == "erp-active-page-equals")
				{
					//erp-active-page-equals
					var tagComplete = "{{" + processedTag + "}}";
					HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
					htmlDocument.LoadHtml(template);
					var links = htmlDocument.DocumentNode.SelectNodes("//a[@href]");
					foreach (var link in links)
					{
						var hrefString = link.GetAttributeValue("href", "");
						var erpTag = link.Attributes.FirstOrDefault(x => x.Name == tagComplete);
						var emptyTag = link.Attributes.FirstOrDefault(x => x.Name == "dasdasd");
						if (erpTag != null)
						{
							if (!String.IsNullOrWhiteSpace(hrefString) && requestContext != null && requestContext.PageContext != null)
							{
								string currentUrl = requestContext.PageContext.HttpContext.Request.Path.ToString().ToLowerInvariant();
								if (currentUrl == hrefString)
								{
									var classes = link.GetClasses().ToList();
									if (classes.FirstOrDefault(x => x == "active") == null)
									{
										classes.Add("active");
										link.SetAttributeValue("class", String.Join(" ", classes));
									}
								}
							}
							link.Attributes.Remove(tagComplete);
						}
					}
					template = htmlDocument.DocumentNode.InnerHtml;
				}
				else if (processedTag == "erp-active-page-starts")
				{
					//erp-active-page-equals
					var tagComplete = "{{" + processedTag + "}}";
					HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
					htmlDocument.LoadHtml(template);
					var links = htmlDocument.DocumentNode.SelectNodes("//a[@href]");
					foreach (var link in links)
					{
						var hrefString = link.GetAttributeValue("href", "");
						var erpTag = link.Attributes.FirstOrDefault(x => x.Name == tagComplete);
						var emptyTag = link.Attributes.FirstOrDefault(x => x.Name == "dasdasd");
						if (erpTag != null)
						{
							if (!String.IsNullOrWhiteSpace(hrefString) && requestContext != null && requestContext.PageContext != null)
							{
								string currentUrl = requestContext.PageContext.HttpContext.Request.Path.ToString().ToLowerInvariant();
								if (currentUrl.StartsWith(hrefString))
								{
									var classes = link.GetClasses().ToList();
									if (classes.FirstOrDefault(x => x == "active") == null)
									{
										classes.Add("active");
										link.SetAttributeValue("class", String.Join(" ", classes));
									}
								}
							}
							link.Attributes.Remove(tagComplete);
						}
					}
					template = htmlDocument.DocumentNode.InnerHtml;
				}
			}

			return template;
		}

		public string GetSnippetFromHtml(string html, int snippetLength = 150)
		{
			var result = "";
			if (!String.IsNullOrWhiteSpace(html))
			{
				HtmlDocument doc = new HtmlDocument();
				doc.LoadHtml(html);
				var root = doc.DocumentNode;
				var sb = new StringBuilder();
				foreach (var node in root.DescendantsAndSelf())
				{
					if (!node.HasChildNodes)
					{
						string text = node.InnerText;
						if (!string.IsNullOrEmpty(text))
							sb.AppendLine(text.Trim());
					}
				}
				result = sb.ToString();

				if (result.Length > snippetLength)
				{
					result = sb.ToString().Substring(0, 150);
					result += "...";
				}
			}
			return result;
		}

		private string GetValueFromPropertyPath(dynamic Obj, string[] PropertyPath)
		{
			dynamic pathPropertyValue = null;
			var result = "";
			if (Obj == null)
			{
				return result;
			}
			//If there is property defined
			if (PropertyPath.Length > 0)
			{
				var processedProperty = PropertyPath[0];
				//Check if there is an index requested. Like ListMeta.Columns[0].Name -> the property will be "Column[0]"
				var foundIndexes = Regex.Matches(processedProperty, @"\[(.*)\]").Cast<Match>().Select(match => match.Value).Distinct().ToList();
				if (foundIndexes.Count == 0)
				{
					//No indexes are requests
					pathPropertyValue = Obj.GetType().GetProperty(processedProperty).GetValue(Obj, null);
					return pathPropertyValue.ToString();
				}
				else
				{
					//There are indexes. THis supports event the case with more then one index -> "Column[0][2]"
					int firstBracketLocation = processedProperty.IndexOf('[');
					var extractedProperty = processedProperty.Substring(0, firstBracketLocation);
					var listProperty = Obj.GetType().GetProperty(extractedProperty).GetValue(Obj, null) as IList;
					var processedIndexes = 1;
					foreach (var currentIndex in foundIndexes)
					{
						int outInt = 0;
						var processedIndex = currentIndex.Replace("[", "").Replace("]", "");
						if (Int32.TryParse(processedIndex, out outInt))
						{
							if (processedIndexes == foundIndexes.Count())
							{
								//There are no more indexes, so this should not be a list
								pathPropertyValue = listProperty[outInt];
							}
							else
							{
								//There are indexes so this should be a list and we should continue to itterate
								listProperty = listProperty[outInt] as IList;
							}
						}
						else
						{
							throw new Exception("Index in Action item template is not an int: " + processedIndex);
						}
						processedIndexes++;
					}
					PropertyPath = PropertyPath.Skip(1).ToArray();//Remove the processed property from the array path
					result = GetValueFromPropertyPath(pathPropertyValue, PropertyPath);
				}
			}
			return result;
		}

		public string GetCacheKey()
		{
			var key = ErpSettings.CacheKey;
			if (String.IsNullOrWhiteSpace(key))
				key = DateTime.Now.ToString("yyyyMMdd");

			return key;
		}

		public List<MenuItem> ConvertListToTree(List<MenuItem> list, List<MenuItem> result, Guid? parentId = null){
			if(result == null)
				result = new List<MenuItem>();

			var childItems = list.FindAll(x=> x.ParentId == parentId).OrderBy(x=> x.SortOrder).ToList();
			
			foreach (var childNode in childItems)
			{
				//create new node (do not include the Nodes)
				var newItem = new MenuItem{
					Class = childNode.Class,
					Content = childNode.Content,
					Id = childNode.Id,
					isDropdownRight = childNode.isDropdownRight,
					IsHtml = childNode.IsHtml,
					ParentId = childNode.ParentId,
					RenderWrapper = childNode.RenderWrapper,
					SortOrder = childNode.SortOrder,
					Nodes = new List<MenuItem>()
				};
				if (parentId == null)
				{
					result.Add(newItem);
				}
				else {
					var parentNode = result.First(x => x.Id == parentId);
					parentNode.Nodes.Add(newItem);
				}
				ConvertListToTree(list, result, childNode.Id);
			}
			return result;
		}
	}
}
