using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.Erp.Web.Models
{
	public class PageBodyNode
	{
		[JsonProperty("id")]
		public Guid Id { get; set; }
		 
		[JsonProperty("parent_id")]
		public Guid? ParentId { get; set; }

		[JsonProperty("page_id")]
		public Guid PageId { get; set; }

		//Could be used from a component that has embedded layout nodes. 
		//In most cases null. If node has Id it should not be allowed to be removed from the Page Designer Layout by the user
		[JsonProperty("node_id")]
		public Guid? NodeId { get; set; } = null;

		[JsonProperty("container_id")]
		public string ContainerId { get; set; } = Guid.Empty.ToString();

		[JsonProperty("weight")]
		public int Weight { get; set; } = 1;

		[JsonProperty("component_name")]
		public string ComponentName { get; set; } = "";

		[JsonProperty("options")]
		public string Options { get; set; } = ""; //Json of the specific configuration selected from the user for this page

		[JsonProperty("nodes")]
		public List<PageBodyNode> Nodes { get; set; } = new List<PageBodyNode>();
	}
}
