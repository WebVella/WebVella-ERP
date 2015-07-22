using AutoMapper;
using System;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
{
	internal class RecordViewProfile : Profile
	{
		IErpService service;

		public RecordViewProfile(IErpService service)
		{
			this.service = service;
		}

		protected override void Configure()
		{
			Mapper.CreateMap<RecordViewRelationListItem, IStorageRecordViewRelationListItem>().ConstructUsing(x => CreateEmptyRecordViewRelationListItemObject(x));
			Mapper.CreateMap<IStorageRecordViewRelationListItem, RecordViewRelationListItem>();
			Mapper.CreateMap<RecordViewRelationListItem, InputRecordViewRelationListItem>();
			Mapper.CreateMap<InputRecordViewRelationListItem, RecordViewRelationListItem>()
				 .ForMember(x => x.ListId, opt => opt.MapFrom(y => (y.ListId.HasValue) ? y.ListId.Value : Guid.Empty))
				.ForMember(x => x.RelationId, opt => opt.MapFrom(y => (y.RelationId.HasValue) ? y.RelationId.Value : Guid.Empty));

			Mapper.CreateMap<RecordViewRelationViewItem, IStorageRecordViewRelationViewItem>().ConstructUsing(x => CreateEmptyRecordViewRelationViewItemObject(x));
			Mapper.CreateMap<IStorageRecordViewRelationViewItem, RecordViewRelationViewItem>();
			Mapper.CreateMap<RecordViewRelationViewItem, InputRecordViewRelationViewItem>();
			Mapper.CreateMap<InputRecordViewRelationViewItem, RecordViewRelationViewItem>()
				.ForMember(x => x.ViewId, opt => opt.MapFrom(y => (y.ViewId.HasValue) ? y.ViewId.Value : Guid.Empty))
				.ForMember(x => x.RelationId, opt => opt.MapFrom(y => (y.RelationId.HasValue) ? y.RelationId.Value : Guid.Empty));

			Mapper.CreateMap<RecordViewHtmlItem, IStorageRecordViewHtmlItem>().ConstructUsing(x => CreateEmptyRecordViewHtmlItemObject(x));
			Mapper.CreateMap<IStorageRecordViewHtmlItem, RecordViewHtmlItem>();
			Mapper.CreateMap<RecordViewHtmlItem, InputRecordViewHtmlItem>();
			Mapper.CreateMap<InputRecordViewHtmlItem, RecordViewHtmlItem>();

			Mapper.CreateMap<RecordViewRelationFieldItem, IStorageRecordViewRelationFieldItem>().ConstructUsing(x => CreateEmptyRecordViewRelationFieldItemObject(x));
			Mapper.CreateMap<IStorageRecordViewRelationFieldItem, RecordViewRelationFieldItem>();
			Mapper.CreateMap<RecordViewRelationFieldItem, InputRecordViewRelationFieldItem>();
			Mapper.CreateMap<InputRecordViewRelationFieldItem, RecordViewRelationFieldItem>()
				.ForMember(x => x.FieldId, opt => opt.MapFrom(y => (y.FieldId.HasValue) ? y.FieldId.Value : Guid.Empty))
				.ForMember(x => x.RelationId, opt => opt.MapFrom(y => (y.RelationId.HasValue) ? y.RelationId.Value : Guid.Empty));


			Mapper.CreateMap<RecordViewViewItem, IStorageRecordViewViewItem>().ConstructUsing(x => CreateEmptyRecordViewViewItemObject(x));
			Mapper.CreateMap<IStorageRecordViewViewItem, RecordViewViewItem>();
			Mapper.CreateMap<RecordViewViewItem, InputRecordViewViewItem>();
			Mapper.CreateMap<InputRecordViewViewItem, RecordViewViewItem>()
				.ForMember(x => x.ViewId, opt => opt.MapFrom(y => (y.ViewId.HasValue) ? y.ViewId.Value : Guid.Empty));


			Mapper.CreateMap<RecordViewListItem, IStorageRecordViewListItem>().ConstructUsing(x => CreateEmptyRecordViewListItemObject(x));
			Mapper.CreateMap<IStorageRecordViewListItem, RecordViewListItem>();
			Mapper.CreateMap<RecordViewListItem, InputRecordViewListItem>();
			Mapper.CreateMap<InputRecordViewListItem, RecordViewListItem>()
				.ForMember(x => x.ListId, opt => opt.MapFrom(y => (y.ListId.HasValue) ? y.ListId.Value : Guid.Empty));


			Mapper.CreateMap<RecordViewFieldItem, IStorageRecordViewFieldItem>().ConstructUsing(x => CreateEmptyRecordViewFieldItemObject(x));
			Mapper.CreateMap<IStorageRecordViewFieldItem, RecordViewFieldItem>();
			Mapper.CreateMap<RecordViewFieldItem, InputRecordViewFieldItem>();
			Mapper.CreateMap<InputRecordViewFieldItem, RecordViewFieldItem>()
				.ForMember(x => x.FieldId, opt => opt.MapFrom(y => (y.FieldId.HasValue) ? y.FieldId.Value : Guid.Empty));


			Mapper.CreateMap<RecordViewItemBase, IStorageRecordViewItemBase>().ConstructUsing(x => CreateEmptyRecordViewItemBaseObject(x))
				.Include<RecordViewFieldItem, IStorageRecordViewFieldItem>()
				.Include<RecordViewListItem, IStorageRecordViewListItem>()
				.Include<RecordViewViewItem, IStorageRecordViewViewItem>()
				.Include<RecordViewRelationFieldItem, IStorageRecordViewRelationFieldItem>()
				.Include<RecordViewRelationViewItem, IStorageRecordViewRelationViewItem>()
				.Include<RecordViewRelationListItem, IStorageRecordViewRelationListItem>()
				.Include<RecordViewHtmlItem, IStorageRecordViewHtmlItem>();
			Mapper.CreateMap<IStorageRecordViewItemBase, RecordViewItemBase>()
				.Include<IStorageRecordViewFieldItem, RecordViewFieldItem>()
				.Include<IStorageRecordViewListItem, RecordViewListItem>()
				.Include<IStorageRecordViewViewItem, RecordViewViewItem>()
				.Include<IStorageRecordViewRelationFieldItem, RecordViewRelationFieldItem>()
				.Include<IStorageRecordViewRelationViewItem, RecordViewRelationViewItem>()
				.Include<IStorageRecordViewRelationListItem, RecordViewRelationListItem>()
				.Include<IStorageRecordViewHtmlItem, RecordViewHtmlItem>();
			Mapper.CreateMap<RecordViewItemBase, InputRecordViewItemBase>()
				.Include<RecordViewFieldItem, InputRecordViewFieldItem>()
				.Include<RecordViewListItem, InputRecordViewListItem>()
				.Include<RecordViewViewItem, InputRecordViewViewItem>()
				.Include<RecordViewRelationFieldItem, InputRecordViewRelationFieldItem>()
				.Include<RecordViewRelationViewItem, InputRecordViewRelationViewItem>()
				.Include<RecordViewRelationListItem, InputRecordViewRelationListItem>()
				.Include<RecordViewHtmlItem, InputRecordViewHtmlItem>();
			Mapper.CreateMap<InputRecordViewItemBase, RecordViewItemBase>()
				.Include<InputRecordViewFieldItem, RecordViewFieldItem>()
				.Include<InputRecordViewListItem, RecordViewListItem>()
				.Include<InputRecordViewViewItem, RecordViewViewItem>()
				.Include<InputRecordViewRelationFieldItem, RecordViewRelationFieldItem>()
				.Include<InputRecordViewRelationViewItem, RecordViewRelationViewItem>()
				.Include<InputRecordViewRelationListItem, RecordViewRelationListItem>()
				.Include<InputRecordViewHtmlItem, RecordViewHtmlItem>()
				.ForMember(x => x.EntityId, opt => opt.MapFrom(y => (y.EntityId.HasValue) ? y.EntityId.Value : Guid.Empty));

			Mapper.CreateMap<RecordViewColumn, IStorageRecordViewColumn>().ConstructUsing(x => CreateEmptyRecordViewColumnObject(x));
			Mapper.CreateMap<IStorageRecordViewColumn, RecordViewColumn>();
			Mapper.CreateMap<RecordViewColumn, InputRecordViewColumn>();
			Mapper.CreateMap<InputRecordViewColumn, RecordViewColumn>();


			Mapper.CreateMap<RecordViewRow, IStorageRecordViewRow>().ConstructUsing(x => CreateEmptyRecordViewRowObject(x));
			Mapper.CreateMap<IStorageRecordViewRow, RecordViewRow>();
			Mapper.CreateMap<RecordViewRow, InputRecordViewRow>();
			Mapper.CreateMap<InputRecordViewRow, RecordViewRow>()
				.ForMember(x => x.Id, opt => opt.MapFrom(y => (y.Id.HasValue) ? y.Id.Value : Guid.Empty));


			Mapper.CreateMap<RecordViewSection, IStorageRecordViewSection>().ConstructUsing(x => CreateEmptyRecordViewSectionObject(x));
			Mapper.CreateMap<IStorageRecordViewSection, RecordViewSection>();
			Mapper.CreateMap<RecordViewSection, InputRecordViewSection>();
			Mapper.CreateMap<InputRecordViewSection, RecordViewSection>()
				.ForMember(x => x.Id, opt => opt.MapFrom(y => (y.Id.HasValue) ? y.Id.Value : Guid.Empty))
				.ForMember(x => x.Collapsed, opt => opt.MapFrom(y => (y.Collapsed.HasValue) ? y.Collapsed.Value : false))
				.ForMember(x => x.ShowLabel, opt => opt.MapFrom(y => (y.ShowLabel.HasValue) ? y.ShowLabel.Value : false));


			Mapper.CreateMap<RecordViewRegion, IStorageRecordViewRegion>().ConstructUsing(x => CreateEmptyRecordViewRegionObject(x));
			Mapper.CreateMap<IStorageRecordViewRegion, RecordViewRegion>();
			Mapper.CreateMap<RecordViewRegion, InputRecordViewRegion>();
			Mapper.CreateMap<InputRecordViewRegion, RecordViewRegion>()
				.ForMember(x => x.Render, opt => opt.MapFrom(y => (y.Render.HasValue) ? y.Render.Value : false));

			Mapper.CreateMap<RecordViewSidebarRelationViewItem, IStorageRecordViewSidebarRelationViewItem>().ConstructUsing(x => CreateEmptyRecordViewSidebarRelationViewItemObject(x));
			Mapper.CreateMap<IStorageRecordViewSidebarRelationViewItem, RecordViewSidebarRelationViewItem>();
			Mapper.CreateMap<RecordViewSidebarRelationViewItem, InputRecordViewSidebarRelationViewItem>();
			Mapper.CreateMap<InputRecordViewSidebarRelationViewItem, RecordViewSidebarRelationViewItem>()
				.ForMember(x => x.ViewId, opt => opt.MapFrom(y => (y.ViewId.HasValue) ? y.ViewId.Value : Guid.Empty))
				.ForMember(x => x.RelationId, opt => opt.MapFrom(y => (y.RelationId.HasValue) ? y.RelationId.Value : Guid.Empty));

			Mapper.CreateMap<RecordViewSidebarRelationListItem, IStorageRecordViewSidebarRelationListItem>().ConstructUsing(x => CreateEmptyRecordViewSidebarRelationListItemObject(x));
			Mapper.CreateMap<IStorageRecordViewSidebarRelationListItem, RecordViewSidebarRelationListItem>();
			Mapper.CreateMap<RecordViewSidebarRelationListItem, InputRecordViewSidebarRelationListItem>();
			Mapper.CreateMap<InputRecordViewSidebarRelationListItem, RecordViewSidebarRelationListItem>()
				.ForMember(x => x.ListId, opt => opt.MapFrom(y => (y.ListId.HasValue) ? y.ListId.Value : Guid.Empty))
				.ForMember(x => x.RelationId, opt => opt.MapFrom(y => (y.RelationId.HasValue) ? y.RelationId.Value : Guid.Empty));

			Mapper.CreateMap<RecordViewSidebarViewItem, IStorageRecordViewSidebarViewItem>().ConstructUsing(x => CreateEmptyRecordViewSidebarViewItemObject(x));
			Mapper.CreateMap<IStorageRecordViewSidebarViewItem, RecordViewSidebarViewItem>();
			Mapper.CreateMap<RecordViewSidebarListItem, InputRecordViewSidebarViewItem>();
			Mapper.CreateMap<InputRecordViewSidebarViewItem, RecordViewSidebarViewItem>()
				.ForMember(x => x.ViewId, opt => opt.MapFrom(y => (y.ViewId.HasValue) ? y.ViewId.Value : Guid.Empty));

			Mapper.CreateMap<RecordViewSidebarListItem, IStorageRecordViewSidebarListItem>().ConstructUsing(x => CreateEmptyRecordViewSidebarListItemObject(x));
			Mapper.CreateMap<IStorageRecordViewSidebarListItem, RecordViewSidebarListItem>();
			Mapper.CreateMap<RecordViewSidebarListItem, InputRecordViewSidebarListItem>();
			Mapper.CreateMap<InputRecordViewSidebarListItem, RecordViewSidebarListItem>()
				.ForMember(x => x.ListId, opt => opt.MapFrom(y => (y.ListId.HasValue) ? y.ListId.Value : Guid.Empty));

			Mapper.CreateMap<RecordViewSidebarItemBase, IStorageRecordViewSidebarItemBase>().ConstructUsing(x => CreateEmptyRecordViewSidebarItemBaseObject(x))
				.Include<RecordViewSidebarListItem, IStorageRecordViewSidebarListItem>()
				.Include<RecordViewSidebarViewItem, IStorageRecordViewSidebarViewItem>()
				.Include<RecordViewSidebarRelationListItem, IStorageRecordViewSidebarRelationListItem>()
				.Include<RecordViewSidebarRelationViewItem, IStorageRecordViewSidebarRelationViewItem>();
			Mapper.CreateMap<IStorageRecordViewSidebarItemBase, RecordViewSidebarItemBase>()
				.Include<IStorageRecordViewSidebarListItem, RecordViewSidebarListItem>()
				.Include<IStorageRecordViewSidebarViewItem, RecordViewSidebarViewItem>()
				.Include<IStorageRecordViewSidebarRelationListItem, RecordViewSidebarRelationListItem>()
				.Include<IStorageRecordViewSidebarRelationViewItem, RecordViewSidebarRelationViewItem>();
			Mapper.CreateMap<RecordViewSidebarItemBase, InputRecordViewSidebarItemBase>()
				.Include<RecordViewSidebarListItem, InputRecordViewSidebarListItem>()
				.Include<RecordViewSidebarViewItem, InputRecordViewSidebarViewItem>()
				.Include<RecordViewSidebarRelationListItem, InputRecordViewSidebarRelationListItem>()
				.Include<RecordViewSidebarRelationViewItem, InputRecordViewSidebarRelationViewItem>();
			Mapper.CreateMap<InputRecordViewSidebarItemBase, RecordViewSidebarItemBase>()
				.Include<InputRecordViewSidebarListItem, RecordViewSidebarListItem>()
				.Include<InputRecordViewSidebarViewItem, RecordViewSidebarViewItem>()
				.Include<InputRecordViewSidebarRelationListItem, RecordViewSidebarRelationListItem>()
				.Include<InputRecordViewSidebarRelationViewItem, RecordViewSidebarRelationViewItem>()
				.ForMember(x => x.EntityId, opt => opt.MapFrom(y => (y.EntityId.HasValue) ? y.EntityId.Value : Guid.Empty));


			Mapper.CreateMap<RecordViewSidebar, IStorageRecordViewSidebar>().ConstructUsing(x => CreateEmptyRecordViewSidebarObject(x));
			Mapper.CreateMap<IStorageRecordViewSidebar, RecordViewSidebar>();
			Mapper.CreateMap<RecordViewSidebar, InputRecordViewSidebar>();
			Mapper.CreateMap<InputRecordViewSidebar, RecordViewSidebar>()
				.ForMember(x => x.Render, opt => opt.MapFrom(y => (y.Render.HasValue) ? y.Render.Value : false));


			Mapper.CreateMap<RecordView, IStorageRecordView>().ConstructUsing(x => CreateEmptyRecordViewObject(x))
				.ForMember(x => x.Type, opt => opt.MapFrom(y => GetViewTypeId(y.Type)));
			Mapper.CreateMap<IStorageRecordView, RecordView>()
				.ForMember(x => x.Type, opt => opt.MapFrom(y => Enum.GetName(typeof(RecordViewType), y.Type).ToLower()));
			Mapper.CreateMap<RecordView, InputRecordView>();
			Mapper.CreateMap<InputRecordView, RecordView>()
				.ForMember(x => x.Id, opt => opt.MapFrom(y => (y.Id.HasValue) ? y.Id.Value : Guid.Empty));

		}

		private RecordViewType GetViewTypeId(string name)
		{
			RecordViewType type = RecordViewType.General;

			Enum.TryParse(name, out type);

			return type;
		}

		protected IStorageRecordView CreateEmptyRecordViewObject(RecordView view)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordViewObject();
		}

		protected IStorageRecordViewSidebar CreateEmptyRecordViewSidebarObject(RecordViewSidebar sidebar)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordViewSidebarObject();
		}

		protected IStorageRecordViewSidebarItemBase CreateEmptyRecordViewSidebarItemBaseObject(RecordViewSidebarItemBase item)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordViewSidebarItemBaseObject();
		}

		protected IStorageRecordViewSidebarListItem CreateEmptyRecordViewSidebarListItemObject(RecordViewSidebarListItem item)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordViewSidebarListItemObject();
		}

		protected IStorageRecordViewSidebarViewItem CreateEmptyRecordViewSidebarViewItemObject(RecordViewSidebarViewItem item)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordViewSidebarViewItemObject();
		}

		protected IStorageRecordViewSidebarRelationListItem CreateEmptyRecordViewSidebarRelationListItemObject(RecordViewSidebarRelationListItem item)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordViewSidebarRelationListItemObject();
		}

		protected IStorageRecordViewSidebarRelationViewItem CreateEmptyRecordViewSidebarRelationViewItemObject(RecordViewSidebarRelationViewItem item)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordViewSidebarRelationViewItemObject();
		}

		protected IStorageRecordViewRegion CreateEmptyRecordViewRegionObject(RecordViewRegion region)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordViewRegionObject();
		}

		protected IStorageRecordViewSection CreateEmptyRecordViewSectionObject(RecordViewSection section)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordViewSectionObject();
		}

		protected IStorageRecordViewRow CreateEmptyRecordViewRowObject(RecordViewRow row)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordViewRowObject();
		}

		protected IStorageRecordViewColumn CreateEmptyRecordViewColumnObject(RecordViewColumn column)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordViewColumnObject();
		}

		protected IStorageRecordViewItemBase CreateEmptyRecordViewItemBaseObject(RecordViewItemBase item)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordViewItemBaseObject();
		}

		protected IStorageRecordViewFieldItem CreateEmptyRecordViewFieldItemObject(RecordViewFieldItem item)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordViewFieldItemObject();
		}

		protected IStorageRecordViewListItem CreateEmptyRecordViewListItemObject(RecordViewListItem item)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordViewListItemObject();
		}

		protected IStorageRecordViewViewItem CreateEmptyRecordViewViewItemObject(RecordViewViewItem item)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordViewViewItemObject();
		}

		protected IStorageRecordViewRelationFieldItem CreateEmptyRecordViewRelationFieldItemObject(RecordViewRelationFieldItem item)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordViewRelationFieldItemObject();
		}

		protected IStorageRecordViewHtmlItem CreateEmptyRecordViewHtmlItemObject(RecordViewHtmlItem item)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordViewHtmlItemObject();
		}

		protected IStorageRecordViewRelationViewItem CreateEmptyRecordViewRelationViewItemObject(RecordViewRelationViewItem item)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordViewRelationViewItemObject();
		}

		protected IStorageRecordViewRelationListItem CreateEmptyRecordViewRelationListItemObject(RecordViewRelationListItem item)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordViewRelationListItemObject();
		}
	}
}
