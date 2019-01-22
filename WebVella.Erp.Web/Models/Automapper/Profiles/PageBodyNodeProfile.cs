using AutoMapper;
using System;
using System.Data;

namespace WebVella.Erp.Web.Models.AutoMapper.Profiles
{
	public class PageBodyNodeProfile : Profile
	{
		public PageBodyNodeProfile()
		{
			CreateMap<DataRow, PageBodyNode>().ConvertUsing(source => DataRowToPageBodyNode(source));
		}

		private static PageBodyNode DataRowToPageBodyNode(DataRow data)
		{
			if (data == null)
				return null;

			PageBodyNode model = new PageBodyNode();
	
			model.Id = (Guid)data["id"];
			model.PageId = (Guid)data["page_id"];
			model.Weight = (int)data["weight"];
			model.ParentId = data["parent_id"] == DBNull.Value ? (Guid?) null : (Guid?)data["parent_id"];
			model.NodeId = data["node_id"] == DBNull.Value ? (Guid?)null : (Guid?)data["node_id"];

			string optionsJson = data["options"] == DBNull.Value ? null : (string)data["options"];
			if (!string.IsNullOrWhiteSpace(optionsJson))
				model.Options = optionsJson;
				//model.Options = JsonConvert.DeserializeObject<string>(optionsJson);

			model.ComponentName = data["component_name"] == DBNull.Value ? null : (string)data["component_name"];
			model.ContainerId = data["container_id"] == DBNull.Value ? null : (string)data["container_id"];

			//CHILD NODES WILL BE RESTORED IN SERVICE LOGIC

			return model;
		}
	}
}
