using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Plugins.Project.Utils
{
	public static class EntityRecordUtils
	{
		public static List<EntityRecord> ConvertRecordListToTree(List<EntityRecord> input, List<EntityRecord> result, Guid? parentId = null, 
					string parentIdFieldName = "parent_id", string childNodesName = "nodes", string createdDateFieldName = "created_on", string sortOrder = "asc" ) {

			var childNodes = input.FindAll(x => (Guid?)x[parentIdFieldName] == parentId).ToList();

			if (sortOrder.ToLowerInvariant() == "desc")
			{
				childNodes = childNodes.OrderByDescending(x => (DateTime)x[createdDateFieldName]).ToList();
			}
			else {
				childNodes = childNodes.OrderBy(x => (DateTime)x[createdDateFieldName]).ToList();
			}

			foreach (var childNode in childNodes)
			{
				if (parentId == null)
				{
					result.Add(childNode);
				}
				else {
					var parentNode = result.First(x => (Guid)x["id"] == parentId);
					if (!parentNode.Properties.ContainsKey(childNodesName)) {
						parentNode[childNodesName] = new List<EntityRecord>();
					}
					((List<EntityRecord>)parentNode[childNodesName]).Add(childNode);
				}
				ConvertRecordListToTree(input, result, (Guid)childNode["id"], parentIdFieldName, childNodesName, createdDateFieldName, sortOrder);
			}

			return result;
		}
	}
}
