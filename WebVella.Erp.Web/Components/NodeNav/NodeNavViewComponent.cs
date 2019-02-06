//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using WebVella.Erp.Web.Models;
//using WebVella.Erp.Web.Services;
//using WebVella.Erp.Web.Utils;

//namespace WebVella.Erp.Web.Components
//{

//	public class NodeNavViewComponent : ViewComponent
//    {
//		protected ErpRequestContext ErpRequestContext { get; set; }

//		public NodeNavViewComponent([FromServices]ErpRequestContext coreReqCtx)
//		{
//			ErpRequestContext = coreReqCtx;
//		}

//		public async Task<IViewComponentResult> InvokeAsync( )
//        {
//			var currentNode = ErpRequestContext.SitemapNode;
//			var currentArea = ErpRequestContext.SitemapArea;

//			var nodesWithExistingGroups = new List<SitemapNode>();
//			var nodesWithoutExistingGroups = new List<SitemapNode>();
//			var existingGroupsHashset = new HashSet<string>();
//			var groupNameHashset = new HashSet<string>();
//			var groupWithNodesCount = 0;

//			if (currentNode != null && currentArea != null)
//			{

//				foreach (var group in currentArea.Groups)
//				{
//					existingGroupsHashset.Add(group.Name);
//				}

//				foreach (var node in currentArea.Nodes)
//				{
//					//Exclude current node; //Boz-it is more convenient not to exclude it
//					//if (node.Id == currentNode.Id) {
//					//	continue;
//					//}

//					//Generate the url for the node based on the route convention
//					node.Url = PageUtils.GenerateSitemapNodeUrl(node, currentArea, ErpRequestContext.App);

//					if (existingGroupsHashset.Contains(node.GroupName))
//					{
//						nodesWithExistingGroups.Add(node);
//						if (!groupNameHashset.Contains(node.GroupName))
//						{
//							groupWithNodesCount++;
//							groupNameHashset.Add(node.GroupName);
//						}
//					}
//					else
//					{
//						nodesWithoutExistingGroups.Add(node);
//					}
//				}

//				if (nodesWithExistingGroups.Count > 1)
//				{
//					nodesWithExistingGroups = nodesWithExistingGroups.OrderBy(x => x.Weight).ThenBy(x => x.Label).ToList();
//				}

//				if (nodesWithoutExistingGroups.Count > 1)
//				{
//					nodesWithoutExistingGroups = nodesWithoutExistingGroups.OrderBy(x => x.Weight).ThenBy(x => x.Label).ToList();
//				}
//			}

//			//Render methods
//			var ddRenderSpan = 3;
//			var ddWidth = "45rem";
//			var columnsRendered = groupWithNodesCount + (nodesWithoutExistingGroups.Count > 0 ? 1 : 0);
//			if (columnsRendered == 1) {
//				ddRenderSpan = 12;
//				ddWidth = "15rem";
//			}
//			else if (columnsRendered == 2)
//			{
//				ddRenderSpan = 6;
//				ddWidth = "25rem";
//			}
//			else if (columnsRendered == 3)
//			{
//				ddRenderSpan = 4;
//				ddWidth = "35rem";
//			}


//			ViewBag.NodesWithExistingGroups = nodesWithExistingGroups;
//			ViewBag.NodesWithoutExistingGroups = nodesWithoutExistingGroups;
//			ViewBag.CurrentNode = currentNode;
//			ViewBag.CurrentArea = currentArea;
//			ViewBag.DDRenderSpan = ddRenderSpan;
//			ViewBag.DDWidth = ddWidth;
//			return await Task.FromResult<IViewComponentResult>(View("NodeNav.Default"));
//        }
//    }
//}
