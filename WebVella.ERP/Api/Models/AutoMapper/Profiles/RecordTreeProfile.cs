using AutoMapper;
using System;
using System.Collections.Generic;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
{
	internal class RecordTreeProfile : Profile
	{
		IErpService service;

		public RecordTreeProfile(IErpService service)
		{
			this.service = service;
		}

		protected override void Configure()
		{
			Mapper.CreateMap<RecordTree, IStorageRecordTree>().ConstructUsing(x => CreateEmptyRecordTreeObject(x));
			Mapper.CreateMap<IStorageRecordTree, RecordTree>()
				.ForMember(x => x.RootNodes, opt => opt.MapFrom(y => PopulateRootNodes(y)));
			Mapper.CreateMap<RecordTree, InputRecordTree>();
			Mapper.CreateMap<InputRecordTree, RecordTree>()
				.ForMember(x => x.Id, opt => opt.MapFrom(y => (y.Id.HasValue) ? y.Id.Value : Guid.Empty));
		}

		protected IStorageRecordTree CreateEmptyRecordTreeObject(RecordTree item)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordTreeObject();
		}

		protected List<RecordTreeNode> PopulateRootNodes(IStorageRecordTree storageTree)
		{
			List<RecordTreeNode> nodes = new List<RecordTreeNode>();
			if (storageTree.RootNodes == null)
				return nodes;

			foreach (var id in storageTree.RootNodes)
				nodes.Add(new RecordTreeNode { RecordId = id });

			return nodes;
		}
	}
}
