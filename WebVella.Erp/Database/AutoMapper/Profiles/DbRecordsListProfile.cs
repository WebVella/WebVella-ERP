//using AutoMapper;
//using WebVella.Erp.Database;
//using System;
//using WebVella.Erp;
//using WebVella.Erp.Api.Models;
//using WebVella.Erp.Storage;

//namespace WebVella.Erp.Database.AutoMapper.Profiles
//{
//	internal class DbRecordsListProfile : Profile
//	{
//		IErpService service;

//		public DbRecordsListProfile(IErpService service)
//		{
//			this.service = service;
//		}

//		protected override void Configure()
//		{
//			Mapper.CreateMap<RecordListRelationTreeItem, DbRecordListRelationTreeItem>();
//			Mapper.CreateMap<DbRecordListRelationTreeItem, RecordListRelationTreeItem>();
//			Mapper.CreateMap<DbRecordListRelationTreeItem, IStorageRecordListRelationTreeItem>().ConstructUsing(x => CreateEmptyRecordListRelationTreeItemObject(x));
//			Mapper.CreateMap<IStorageRecordListRelationTreeItem, DbRecordListRelationTreeItem>();

//			Mapper.CreateMap<RecordListRelationListItem, DbRecordListRelationListItem>();
//			Mapper.CreateMap<DbRecordListRelationListItem, RecordListRelationListItem>();
//			Mapper.CreateMap<DbRecordListRelationListItem, IStorageRecordListRelationListItem>().ConstructUsing(x => CreateEmptyRecordListRelationListItemObject(x));
//			Mapper.CreateMap<IStorageRecordListRelationListItem, DbRecordListRelationListItem>();

//			Mapper.CreateMap<RecordListListItem, DbRecordListListItem>();
//			Mapper.CreateMap<DbRecordListListItem, RecordListListItem>();
//			Mapper.CreateMap<DbRecordListListItem, IStorageRecordListListItem>().ConstructUsing(x => CreateEmptyRecordListListItemObject(x));
//			Mapper.CreateMap<IStorageRecordListListItem, DbRecordListListItem>();

//			Mapper.CreateMap<RecordListRelationViewItem, DbRecordListRelationViewItem>();
//			Mapper.CreateMap<DbRecordListRelationViewItem, RecordListRelationViewItem>();
//			Mapper.CreateMap<DbRecordListRelationViewItem, IStorageRecordListRelationViewItem>().ConstructUsing(x => CreateEmptyRecordListRelationViewItemObject(x));
//			Mapper.CreateMap<IStorageRecordListRelationViewItem, DbRecordListRelationViewItem>();

//			Mapper.CreateMap<RecordListViewItem, DbRecordListViewItem>();
//			Mapper.CreateMap<DbRecordListViewItem, RecordListViewItem>();
//			Mapper.CreateMap<DbRecordListViewItem, IStorageRecordListViewItem>().ConstructUsing(x => CreateEmptyRecordListViewItemObject(x));
//			Mapper.CreateMap<IStorageRecordListViewItem, DbRecordListViewItem>();

//			Mapper.CreateMap<RecordListRelationFieldItem, DbRecordListRelationFieldItem>();
//			Mapper.CreateMap<DbRecordListRelationFieldItem, RecordListRelationFieldItem>();
//			Mapper.CreateMap<DbRecordListRelationFieldItem, IStorageRecordListRelationFieldItem>().ConstructUsing(x => CreateEmptyRecordListRelationFieldItemObject(x));
//			Mapper.CreateMap<IStorageRecordListRelationFieldItem, DbRecordListRelationFieldItem>();

//			Mapper.CreateMap<RecordListFieldItem, DbRecordListFieldItem>();
//			Mapper.CreateMap<DbRecordListFieldItem, RecordListFieldItem>();
//			Mapper.CreateMap<DbRecordListFieldItem, IStorageRecordListFieldItem>().ConstructUsing(x => CreateEmptyRecordListFieldItemObject(x));
//			Mapper.CreateMap<IStorageRecordListFieldItem, DbRecordListFieldItem>();

//			Mapper.CreateMap<RecordListItemBase, DbRecordListItemBase>()
//				.Include<RecordListFieldItem, DbRecordListFieldItem>()
//				.Include<RecordListRelationFieldItem, DbRecordListRelationFieldItem>()
//				.Include<RecordListViewItem, DbRecordListViewItem>()
//				.Include<RecordListRelationViewItem, DbRecordListRelationViewItem>()
//				.Include<RecordListListItem, DbRecordListListItem>()
//				.Include<RecordListRelationListItem, DbRecordListRelationListItem>()
//				.Include<RecordListRelationTreeItem, DbRecordListRelationTreeItem>();
//			Mapper.CreateMap<DbRecordListItemBase, RecordListItemBase>()
//				.Include<DbRecordListFieldItem, RecordListFieldItem>()
//				.Include<DbRecordListRelationFieldItem, RecordListRelationFieldItem>()
//				.Include<DbRecordListViewItem, RecordListViewItem>()
//				.Include<DbRecordListRelationViewItem, RecordListRelationViewItem>()
//				.Include<DbRecordListListItem, RecordListListItem>()
//				.Include<DbRecordListRelationListItem, RecordListRelationListItem>()
//				.Include<DbRecordListRelationTreeItem, RecordListRelationTreeItem>();
//			Mapper.CreateMap<DbRecordListItemBase, IStorageRecordListItemBase>().ConstructUsing(x => CreateEmptyRecordListItemBaseObject(x))
//				.Include<DbRecordListFieldItem, IStorageRecordListFieldItem>()
//				.Include<DbRecordListRelationFieldItem, IStorageRecordListRelationFieldItem>()
//				.Include<DbRecordListViewItem, IStorageRecordListViewItem>()
//				.Include<DbRecordListRelationViewItem, IStorageRecordListRelationViewItem>()
//				.Include<DbRecordListListItem, IStorageRecordListListItem>()
//				.Include<DbRecordListRelationListItem, IStorageRecordListRelationListItem>()
//				.Include<DbRecordListRelationTreeItem, IStorageRecordListRelationTreeItem>();
//			Mapper.CreateMap<IStorageRecordListItemBase, DbRecordListItemBase>()
//				.Include<IStorageRecordListFieldItem, DbRecordListFieldItem>()
//				.Include<IStorageRecordListRelationFieldItem, DbRecordListRelationFieldItem>()
//				.Include<IStorageRecordListViewItem, DbRecordListViewItem>()
//				.Include<IStorageRecordListRelationViewItem, DbRecordListRelationViewItem>()
//				.Include<IStorageRecordListListItem, DbRecordListListItem>()
//				.Include<IStorageRecordListRelationListItem, DbRecordListRelationListItem>()
//				.Include<IStorageRecordListRelationTreeItem, DbRecordListRelationTreeItem>();

//			Mapper.CreateMap<RecordListQuery, DbRecordListQuery>();
//			Mapper.CreateMap<DbRecordListQuery, RecordListQuery>();
//			Mapper.CreateMap<DbRecordListQuery, IStorageRecordListQuery>().ConstructUsing(x => CreateEmptyRecordListQueryObject(x));
//			Mapper.CreateMap<IStorageRecordListQuery, DbRecordListQuery>();

//			Mapper.CreateMap<RecordListSort, DbRecordListSort>()
//				.ForMember(x => x.SortType, opt => opt.MapFrom(y => GetListSortTypeId(y.SortType)));
//			Mapper.CreateMap<DbRecordListSort, RecordListSort>()
//				.ForMember(x => x.SortType, opt => opt.MapFrom(y => Enum.GetName(typeof(QuerySortType), y.SortType).ToLower()));
//			Mapper.CreateMap<DbRecordListSort, IStorageRecordListSort>().ConstructUsing(x => CreateEmptyRecordListSortObject(x));
//			Mapper.CreateMap<IStorageRecordListSort, DbRecordListSort>();

//			Mapper.CreateMap<RecordList, DbRecordList>()
//				.ForMember(x => x.Type, opt => opt.MapFrom(y => GetListTypeId(y.Type)));
//			Mapper.CreateMap<DbRecordList, RecordList>()
//				.ForMember(x => x.Type, opt => opt.MapFrom(y => Enum.GetName(typeof(RecordListType), y.Type).ToLower()));
//			Mapper.CreateMap<DbRecordList, IStorageRecordList>().ConstructUsing(x => CreateEmptyRecordListObject(x));
//			Mapper.CreateMap<IStorageRecordList, DbRecordList>();
//		}

//		private RecordListType GetListTypeId(string name)
//		{
//			RecordListType type = RecordListType.General;

//			Enum.TryParse(name, true, out type);

//			return type;
//		}

//		private QuerySortType GetListSortTypeId(string name)
//		{
//			QuerySortType type = QuerySortType.Ascending;
//			Enum.TryParse(name, true, out type);
//			return type;
//		}

//		protected IStorageRecordList CreateEmptyRecordListObject(DbRecordList list)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordListObject();
//		}

//		protected IStorageRecordListSort CreateEmptyRecordListSortObject(DbRecordListSort item)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordListSortObject();
//		}

//		protected IStorageRecordListQuery CreateEmptyRecordListQueryObject(DbRecordListQuery item)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordListQueryObject();
//		}

//		protected IStorageRecordListItemBase CreateEmptyRecordListItemBaseObject(DbRecordListItemBase item)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordListItemBaseObject();
//		}

//		protected IStorageRecordListFieldItem CreateEmptyRecordListFieldItemObject(DbRecordListFieldItem item)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordListFieldItemObject();
//		}

//		protected IStorageRecordListRelationFieldItem CreateEmptyRecordListRelationFieldItemObject(DbRecordListRelationFieldItem item)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordListRelationFieldItemObject();
//		}

//		protected IStorageRecordListViewItem CreateEmptyRecordListViewItemObject(DbRecordListViewItem item)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordListViewItemObject();
//		}

//		protected IStorageRecordListRelationViewItem CreateEmptyRecordListRelationViewItemObject(DbRecordListRelationViewItem item)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordListRelationViewItemObject();
//		}

//		protected IStorageRecordListListItem CreateEmptyRecordListListItemObject(DbRecordListListItem item)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordListListItemObject();
//		}

//		protected IStorageRecordListRelationListItem CreateEmptyRecordListRelationListItemObject(DbRecordListRelationListItem item)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordListRelationListItemObject();
//		}

//		protected IStorageRecordListRelationTreeItem CreateEmptyRecordListRelationTreeItemObject(DbRecordListRelationTreeItem item)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordListRelationTreeItemObject();
//		}
//	}
//}
