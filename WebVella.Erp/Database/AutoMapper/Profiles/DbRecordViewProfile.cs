//using AutoMapper;
//using WebVella.Erp.Database;
//using System;
//using WebVella.Erp;
//using WebVella.Erp.Api.Models;
//using WebVella.Erp.Storage;

//namespace WebVella.Erp.Database.AutoMapper.Profiles
//{
//	internal class DbRecordViewProfile : Profile
//	{
//		IErpService service;

//		public DbRecordViewProfile(IErpService service)
//		{
//			this.service = service;
//		}

//		protected override void Configure()
//		{
//			Mapper.CreateMap<RecordViewRelationTreeItem, DbRecordViewRelationTreeItem>();
//			Mapper.CreateMap<DbRecordViewRelationTreeItem, RecordViewRelationTreeItem>();
//			Mapper.CreateMap<DbRecordViewRelationTreeItem, IStorageRecordViewRelationTreeItem>().ConstructUsing(x => CreateEmptyRecordViewRelationTreeItemObject(x));
//			Mapper.CreateMap<IStorageRecordViewRelationTreeItem, DbRecordViewRelationTreeItem>();

//			Mapper.CreateMap<RecordViewRelationListItem, DbRecordViewRelationListItem>();
//			Mapper.CreateMap<DbRecordViewRelationListItem, RecordViewRelationListItem>();
//			Mapper.CreateMap<DbRecordViewRelationListItem, IStorageRecordViewRelationListItem>().ConstructUsing(x => CreateEmptyRecordViewRelationListItemObject(x));
//			Mapper.CreateMap<IStorageRecordViewRelationListItem, DbRecordViewRelationListItem>();

//			Mapper.CreateMap<RecordViewRelationViewItem, DbRecordViewRelationViewItem>();
//			Mapper.CreateMap<DbRecordViewRelationViewItem, RecordViewRelationViewItem>();
//			Mapper.CreateMap<DbRecordViewRelationViewItem, IStorageRecordViewRelationViewItem>().ConstructUsing(x => CreateEmptyRecordViewRelationViewItemObject(x));
//			Mapper.CreateMap<IStorageRecordViewRelationViewItem, DbRecordViewRelationViewItem>();

//			Mapper.CreateMap<RecordViewHtmlItem, DbRecordViewHtmlItem>();
//			Mapper.CreateMap<DbRecordViewHtmlItem, RecordViewHtmlItem>();
//			Mapper.CreateMap<DbRecordViewHtmlItem, IStorageRecordViewHtmlItem>().ConstructUsing(x => CreateEmptyRecordViewHtmlItemObject(x));
//			Mapper.CreateMap<IStorageRecordViewHtmlItem, DbRecordViewHtmlItem>();

//			Mapper.CreateMap<RecordViewRelationFieldItem, DbRecordViewRelationFieldItem>();
//			Mapper.CreateMap<DbRecordViewRelationFieldItem, RecordViewRelationFieldItem>();
//			Mapper.CreateMap<DbRecordViewRelationFieldItem, IStorageRecordViewRelationFieldItem>().ConstructUsing(x => CreateEmptyRecordViewRelationFieldItemObject(x));
//			Mapper.CreateMap<IStorageRecordViewRelationFieldItem, DbRecordViewRelationFieldItem>();


//			Mapper.CreateMap<RecordViewViewItem, DbRecordViewViewItem>();
//			Mapper.CreateMap<DbRecordViewViewItem, RecordViewViewItem>();
//			Mapper.CreateMap<DbRecordViewViewItem, IStorageRecordViewViewItem>().ConstructUsing(x => CreateEmptyRecordViewViewItemObject(x));
//			Mapper.CreateMap<IStorageRecordViewViewItem, DbRecordViewViewItem>();


//			Mapper.CreateMap<RecordViewListItem, DbRecordViewListItem>();
//			Mapper.CreateMap<DbRecordViewListItem, RecordViewListItem>();
//			Mapper.CreateMap<DbRecordViewListItem, IStorageRecordViewListItem>().ConstructUsing(x => CreateEmptyRecordViewListItemObject(x));
//			Mapper.CreateMap<IStorageRecordViewListItem, DbRecordViewListItem>();


//			Mapper.CreateMap<RecordViewFieldItem, DbRecordViewFieldItem>();
//			Mapper.CreateMap<DbRecordViewFieldItem, RecordViewFieldItem>();
//			Mapper.CreateMap<DbRecordViewFieldItem, IStorageRecordViewFieldItem>().ConstructUsing(x => CreateEmptyRecordViewFieldItemObject(x));
//			Mapper.CreateMap<IStorageRecordViewFieldItem, DbRecordViewFieldItem>();


//			Mapper.CreateMap<RecordViewItemBase, DbRecordViewItemBase>()
//				.Include<RecordViewFieldItem, DbRecordViewFieldItem>()
//				.Include<RecordViewListItem, DbRecordViewListItem>()
//				.Include<RecordViewViewItem, DbRecordViewViewItem>()
//				.Include<RecordViewRelationFieldItem, DbRecordViewRelationFieldItem>()
//				.Include<RecordViewRelationViewItem, DbRecordViewRelationViewItem>()
//				.Include<RecordViewRelationListItem, DbRecordViewRelationListItem>()
//				.Include<RecordViewRelationTreeItem, DbRecordViewRelationTreeItem>()
//				.Include<RecordViewHtmlItem, DbRecordViewHtmlItem>();
//			Mapper.CreateMap<DbRecordViewItemBase, RecordViewItemBase>()
//				.Include<DbRecordViewFieldItem, RecordViewFieldItem>()
//				.Include<DbRecordViewListItem, RecordViewListItem>()
//				.Include<DbRecordViewViewItem, RecordViewViewItem>()
//				.Include<DbRecordViewRelationFieldItem, RecordViewRelationFieldItem>()
//				.Include<DbRecordViewRelationViewItem, RecordViewRelationViewItem>()
//				.Include<DbRecordViewRelationListItem, RecordViewRelationListItem>()
//				.Include<DbRecordViewRelationTreeItem, RecordViewRelationTreeItem>()
//				.Include<DbRecordViewHtmlItem, RecordViewHtmlItem>();
//			Mapper.CreateMap<DbRecordViewItemBase, IStorageRecordViewItemBase>().ConstructUsing(x => CreateEmptyRecordViewItemBaseObject(x))
//				.Include<DbRecordViewFieldItem, IStorageRecordViewFieldItem>()
//				.Include<DbRecordViewListItem, IStorageRecordViewListItem>()
//				.Include<DbRecordViewViewItem, IStorageRecordViewViewItem>()
//				.Include<DbRecordViewRelationFieldItem, IStorageRecordViewRelationFieldItem>()
//				.Include<DbRecordViewRelationViewItem, IStorageRecordViewRelationViewItem>()
//				.Include<DbRecordViewRelationListItem, IStorageRecordViewRelationListItem>()
//				.Include<DbRecordViewRelationTreeItem, IStorageRecordViewRelationTreeItem>()
//				.Include<DbRecordViewHtmlItem, IStorageRecordViewHtmlItem>();
//			Mapper.CreateMap<IStorageRecordViewItemBase, DbRecordViewItemBase>()
//				.Include<IStorageRecordViewFieldItem, DbRecordViewFieldItem>()
//				.Include<IStorageRecordViewListItem, DbRecordViewListItem>()
//				.Include<IStorageRecordViewViewItem, DbRecordViewViewItem>()
//				.Include<IStorageRecordViewRelationFieldItem, DbRecordViewRelationFieldItem>()
//				.Include<IStorageRecordViewRelationViewItem, DbRecordViewRelationViewItem>()
//				.Include<IStorageRecordViewRelationListItem, DbRecordViewRelationListItem>()
//				.Include<IStorageRecordViewRelationTreeItem, DbRecordViewRelationTreeItem>()
//				.Include<IStorageRecordViewHtmlItem, DbRecordViewHtmlItem>();

//			Mapper.CreateMap<RecordViewColumn, DbRecordViewColumn>();
//			Mapper.CreateMap<DbRecordViewColumn, RecordViewColumn>();
//			Mapper.CreateMap<DbRecordViewColumn, IStorageRecordViewColumn>().ConstructUsing(x => CreateEmptyRecordViewColumnObject(x));
//			Mapper.CreateMap<IStorageRecordViewColumn, DbRecordViewColumn>();


//			Mapper.CreateMap<RecordViewRow, DbRecordViewRow>();
//			Mapper.CreateMap<DbRecordViewRow, RecordViewRow>();
//			Mapper.CreateMap<DbRecordViewRow, IStorageRecordViewRow>().ConstructUsing(x => CreateEmptyRecordViewRowObject(x));
//			Mapper.CreateMap<IStorageRecordViewRow, DbRecordViewRow>();


//			Mapper.CreateMap<RecordViewSection, DbRecordViewSection>();
//			Mapper.CreateMap<DbRecordViewSection, RecordViewSection>();
//			Mapper.CreateMap<DbRecordViewSection, IStorageRecordViewSection>().ConstructUsing(x => CreateEmptyRecordViewSectionObject(x));
//			Mapper.CreateMap<IStorageRecordViewSection, DbRecordViewSection>();


//			Mapper.CreateMap<RecordViewRegion, DbRecordViewRegion>();
//			Mapper.CreateMap<DbRecordViewRegion, RecordViewRegion>();
//			Mapper.CreateMap<DbRecordViewRegion, IStorageRecordViewRegion>().ConstructUsing(x => CreateEmptyRecordViewRegionObject(x));
//			Mapper.CreateMap<IStorageRecordViewRegion, DbRecordViewRegion>();

//			Mapper.CreateMap<RecordViewSidebarRelationViewItem, DbRecordViewSidebarRelationViewItem>();
//			Mapper.CreateMap<DbRecordViewSidebarRelationViewItem, RecordViewSidebarRelationViewItem>();
//			Mapper.CreateMap<DbRecordViewSidebarRelationViewItem, IStorageRecordViewSidebarRelationViewItem>().ConstructUsing(x => CreateEmptyRecordViewSidebarRelationViewItemObject(x));
//			Mapper.CreateMap<IStorageRecordViewSidebarRelationViewItem, DbRecordViewSidebarRelationViewItem>();

//			Mapper.CreateMap<RecordViewSidebarRelationListItem, DbRecordViewSidebarRelationListItem>();
//			Mapper.CreateMap<DbRecordViewSidebarRelationListItem, RecordViewSidebarRelationListItem>();
//			Mapper.CreateMap<DbRecordViewSidebarRelationListItem, IStorageRecordViewSidebarRelationListItem>().ConstructUsing(x => CreateEmptyRecordViewSidebarRelationListItemObject(x));
//			Mapper.CreateMap<IStorageRecordViewSidebarRelationListItem, DbRecordViewSidebarRelationListItem>();

//			Mapper.CreateMap<RecordViewSidebarRelationTreeItem, DbRecordViewSidebarRelationTreeItem>();
//			Mapper.CreateMap<DbRecordViewSidebarRelationTreeItem, RecordViewSidebarRelationTreeItem>();
//			Mapper.CreateMap<DbRecordViewSidebarRelationTreeItem, IStorageRecordViewSidebarRelationTreeItem>().ConstructUsing(x => CreateEmptyRecordViewSidebarRelationTreeItemObject(x));
//			Mapper.CreateMap<IStorageRecordViewSidebarRelationTreeItem, DbRecordViewSidebarRelationTreeItem>();

//			Mapper.CreateMap<RecordViewSidebarViewItem, DbRecordViewSidebarViewItem>();
//			Mapper.CreateMap<DbRecordViewSidebarViewItem, RecordViewSidebarViewItem>();
//			Mapper.CreateMap<DbRecordViewSidebarViewItem, IStorageRecordViewSidebarViewItem>().ConstructUsing(x => CreateEmptyRecordViewSidebarViewItemObject(x));
//			Mapper.CreateMap<IStorageRecordViewSidebarViewItem, DbRecordViewSidebarViewItem>();

//			Mapper.CreateMap<RecordViewSidebarListItem, DbRecordViewSidebarListItem>();
//			Mapper.CreateMap<DbRecordViewSidebarListItem, RecordViewSidebarListItem>();
//			Mapper.CreateMap<DbRecordViewSidebarListItem, IStorageRecordViewSidebarListItem>().ConstructUsing(x => CreateEmptyRecordViewSidebarListItemObject(x));
//			Mapper.CreateMap<IStorageRecordViewSidebarListItem, DbRecordViewSidebarListItem>();

//			Mapper.CreateMap<RecordViewSidebarItemBase, DbRecordViewSidebarItemBase>()
//				.Include<RecordViewSidebarListItem, DbRecordViewSidebarListItem>()
//				.Include<RecordViewSidebarViewItem, DbRecordViewSidebarViewItem>()
//				.Include<RecordViewSidebarRelationListItem, DbRecordViewSidebarRelationListItem>()
//				.Include<RecordViewSidebarRelationViewItem, DbRecordViewSidebarRelationViewItem>()
//				.Include<RecordViewSidebarRelationTreeItem, DbRecordViewSidebarRelationTreeItem>();
//			Mapper.CreateMap<DbRecordViewSidebarItemBase, RecordViewSidebarItemBase>()
//				.Include<DbRecordViewSidebarListItem, RecordViewSidebarListItem>()
//				.Include<DbRecordViewSidebarViewItem, RecordViewSidebarViewItem>()
//				.Include<DbRecordViewSidebarRelationListItem, RecordViewSidebarRelationListItem>()
//				.Include<DbRecordViewSidebarRelationViewItem, RecordViewSidebarRelationViewItem>()
//				.Include<DbRecordViewSidebarRelationTreeItem, RecordViewSidebarRelationTreeItem>();
//			Mapper.CreateMap<DbRecordViewSidebarItemBase, IStorageRecordViewSidebarItemBase>().ConstructUsing(x => CreateEmptyRecordViewSidebarItemBaseObject(x))
//				.Include<DbRecordViewSidebarListItem, IStorageRecordViewSidebarListItem>()
//				.Include<DbRecordViewSidebarViewItem, IStorageRecordViewSidebarViewItem>()
//				.Include<DbRecordViewSidebarRelationListItem, IStorageRecordViewSidebarRelationListItem>()
//				.Include<DbRecordViewSidebarRelationViewItem, IStorageRecordViewSidebarRelationViewItem>()
//				.Include<DbRecordViewSidebarRelationTreeItem, IStorageRecordViewSidebarRelationTreeItem>();
//			Mapper.CreateMap<IStorageRecordViewSidebarItemBase, DbRecordViewSidebarItemBase>()
//				.Include<IStorageRecordViewSidebarListItem, DbRecordViewSidebarListItem>()
//				.Include<IStorageRecordViewSidebarViewItem, DbRecordViewSidebarViewItem>()
//				.Include<IStorageRecordViewSidebarRelationListItem, DbRecordViewSidebarRelationListItem>()
//				.Include<IStorageRecordViewSidebarRelationViewItem, DbRecordViewSidebarRelationViewItem>()
//				.Include<IStorageRecordViewSidebarRelationTreeItem, DbRecordViewSidebarRelationTreeItem>();


//			Mapper.CreateMap<RecordViewSidebar, DbRecordViewSidebar>();
//			Mapper.CreateMap<DbRecordViewSidebar, RecordViewSidebar>();
//			Mapper.CreateMap<DbRecordViewSidebar, IStorageRecordViewSidebar>().ConstructUsing(x => CreateEmptyRecordViewSidebarObject(x));
//			Mapper.CreateMap<IStorageRecordViewSidebar, DbRecordViewSidebar>();


//			Mapper.CreateMap<RecordView, DbRecordView>()
//				.ForMember(x => x.Type, opt => opt.MapFrom(y => GetViewTypeId(y.Type)));
//			Mapper.CreateMap<DbRecordView, RecordView>()
//				.ForMember(x => x.Type, opt => opt.MapFrom(y => Enum.GetName(typeof(RecordViewType), y.Type).ToLower()));
//			Mapper.CreateMap<DbRecordView, IStorageRecordView>().ConstructUsing(x => CreateEmptyRecordViewObject(x));
//			Mapper.CreateMap<IStorageRecordView, DbRecordView>();

//		}

//		private RecordViewType GetViewTypeId(string name)
//		{
//			RecordViewType type = RecordViewType.General;

//			Enum.TryParse(name, true, out type);

//			return type;
//		}

//		protected IStorageRecordView CreateEmptyRecordViewObject(DbRecordView view)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordViewObject();
//		}

//		protected IStorageRecordViewSidebar CreateEmptyRecordViewSidebarObject(DbRecordViewSidebar sidebar)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordViewSidebarObject();
//		}

//		protected IStorageRecordViewSidebarItemBase CreateEmptyRecordViewSidebarItemBaseObject(DbRecordViewSidebarItemBase item)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordViewSidebarItemBaseObject();
//		}

//		protected IStorageRecordViewSidebarListItem CreateEmptyRecordViewSidebarListItemObject(DbRecordViewSidebarListItem item)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordViewSidebarListItemObject();
//		}

//		protected IStorageRecordViewSidebarViewItem CreateEmptyRecordViewSidebarViewItemObject(DbRecordViewSidebarViewItem item)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordViewSidebarViewItemObject();
//		}

//		protected IStorageRecordViewSidebarRelationListItem CreateEmptyRecordViewSidebarRelationListItemObject(DbRecordViewSidebarRelationListItem item)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordViewSidebarRelationListItemObject();
//		}

//		protected IStorageRecordViewSidebarRelationViewItem CreateEmptyRecordViewSidebarRelationViewItemObject(DbRecordViewSidebarRelationViewItem item)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordViewSidebarRelationViewItemObject();
//		}

//		protected IStorageRecordViewRegion CreateEmptyRecordViewRegionObject(DbRecordViewRegion region)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordViewRegionObject();
//		}

//		protected IStorageRecordViewSection CreateEmptyRecordViewSectionObject(DbRecordViewSection section)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordViewSectionObject();
//		}

//		protected IStorageRecordViewRow CreateEmptyRecordViewRowObject(DbRecordViewRow row)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordViewRowObject();
//		}

//		protected IStorageRecordViewColumn CreateEmptyRecordViewColumnObject(DbRecordViewColumn column)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordViewColumnObject();
//		}

//		protected IStorageRecordViewItemBase CreateEmptyRecordViewItemBaseObject(DbRecordViewItemBase item)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordViewItemBaseObject();
//		}

//		protected IStorageRecordViewFieldItem CreateEmptyRecordViewFieldItemObject(DbRecordViewFieldItem item)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordViewFieldItemObject();
//		}

//		protected IStorageRecordViewListItem CreateEmptyRecordViewListItemObject(DbRecordViewListItem item)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordViewListItemObject();
//		}

//		protected IStorageRecordViewViewItem CreateEmptyRecordViewViewItemObject(DbRecordViewViewItem item)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordViewViewItemObject();
//		}

//		protected IStorageRecordViewRelationFieldItem CreateEmptyRecordViewRelationFieldItemObject(DbRecordViewRelationFieldItem item)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordViewRelationFieldItemObject();
//		}

//		protected IStorageRecordViewHtmlItem CreateEmptyRecordViewHtmlItemObject(DbRecordViewHtmlItem item)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordViewHtmlItemObject();
//		}

//		protected IStorageRecordViewRelationViewItem CreateEmptyRecordViewRelationViewItemObject(DbRecordViewRelationViewItem item)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordViewRelationViewItemObject();
//		}

//		protected IStorageRecordViewRelationListItem CreateEmptyRecordViewRelationListItemObject(DbRecordViewRelationListItem item)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordViewRelationListItemObject();
//		}

//		protected IStorageRecordViewRelationTreeItem CreateEmptyRecordViewRelationTreeItemObject(DbRecordViewRelationTreeItem item)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordViewRelationTreeItemObject();
//		}

//		protected IStorageRecordViewSidebarRelationTreeItem CreateEmptyRecordViewSidebarRelationTreeItemObject(DbRecordViewSidebarRelationTreeItem item)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyRecordViewSidebarRelationTreeItemObject();
//		}
//	}
//}
