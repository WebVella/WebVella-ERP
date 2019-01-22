//using AutoMapper;
//using WebVella.Erp.Database;
//using System;
//using System.Collections.Generic;
//using WebVella.Erp;
//using WebVella.Erp.Api.Models;
//using WebVella.Erp.Storage;

//namespace WebVella.Erp.Database.AutoMapper.Profiles
//{
//	internal class DbRecordTreeProfile : Profile
//	{
//		IErpService service;

//		public DbRecordTreeProfile(IErpService service)
//		{
//			this.service = service;
//		}

//		protected override void Configure()
//		{
//			Mapper.CreateMap<RecordTree, DbRecordTree>()
//				.ForMember(x => x.RootNodes, opt => opt.MapFrom(y => PopulateRootNodesToStorage(y)));
//			Mapper.CreateMap<DbRecordTree, RecordTree>()
//				.ForMember(x => x.RootNodes, opt => opt.MapFrom(y => PopulateRootNodes(y)));
//			Mapper.CreateMap<DbRecordTree, IStorageRecordTree>().ConstructUsing(x => CreateEmptyRecordTreeObject(x));
//			Mapper.CreateMap<IStorageRecordTree, DbRecordTree>();
//		}

//		protected IStorageRecordTree CreateEmptyRecordTreeObject(DbRecordTree item)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordTreeObject();
//		}

//		protected List<RecordTreeNode> PopulateRootNodes(DbRecordTree storageTree)
//		{
//			List<RecordTreeNode> nodes = new List<RecordTreeNode>();
//			if (storageTree.RootNodes == null)
//				return nodes;

//			foreach (var id in storageTree.RootNodes)
//				nodes.Add(new RecordTreeNode { RecordId = id });

//			return nodes;
//		}

//		protected List<Guid> PopulateRootNodesToStorage(RecordTree tree)
//		{
//			List<Guid> nodeIds = new List<Guid>();
//			if (tree.RootNodes == null)
//				return nodeIds;

//			foreach (var node in tree.RootNodes)
//				nodeIds.Add( node.RecordId );

//			return nodeIds;
//		}
//	}
//}
