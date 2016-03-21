using AutoMapper;
using System;
using WebVella.ERP.Database;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
{
	internal class RecordViewProfile : Profile
	{
		protected override void Configure()
		{
			Mapper.CreateMap<RecordViewRelationTreeItem, InputRecordViewRelationTreeItem>();
			Mapper.CreateMap<InputRecordViewRelationTreeItem, RecordViewRelationTreeItem>()
				 .ForMember(x => x.TreeId, opt => opt.MapFrom(y => (y.TreeId.HasValue) ? y.TreeId.Value : Guid.Empty))
				.ForMember(x => x.RelationId, opt => opt.MapFrom(y => (y.RelationId.HasValue) ? y.RelationId.Value : Guid.Empty));
			Mapper.CreateMap<RecordViewRelationTreeItem, DbRecordViewRelationTreeItem>();
			Mapper.CreateMap<DbRecordViewRelationTreeItem, RecordViewRelationTreeItem>();

			Mapper.CreateMap<RecordViewRelationListItem, InputRecordViewRelationListItem>();
			Mapper.CreateMap<InputRecordViewRelationListItem, RecordViewRelationListItem>()
				 .ForMember(x => x.ListId, opt => opt.MapFrom(y => (y.ListId.HasValue) ? y.ListId.Value : Guid.Empty))
				.ForMember(x => x.RelationId, opt => opt.MapFrom(y => (y.RelationId.HasValue) ? y.RelationId.Value : Guid.Empty));
			Mapper.CreateMap<RecordViewRelationListItem, DbRecordViewRelationListItem>();
			Mapper.CreateMap<DbRecordViewRelationListItem, RecordViewRelationListItem>();

			Mapper.CreateMap<RecordViewRelationViewItem, InputRecordViewRelationViewItem>();
			Mapper.CreateMap<InputRecordViewRelationViewItem, RecordViewRelationViewItem>()
				.ForMember(x => x.ViewId, opt => opt.MapFrom(y => (y.ViewId.HasValue) ? y.ViewId.Value : Guid.Empty))
				.ForMember(x => x.RelationId, opt => opt.MapFrom(y => (y.RelationId.HasValue) ? y.RelationId.Value : Guid.Empty));
			Mapper.CreateMap<RecordViewRelationViewItem, DbRecordViewRelationViewItem>();
			Mapper.CreateMap<DbRecordViewRelationViewItem, RecordViewRelationViewItem>();

			Mapper.CreateMap<RecordViewHtmlItem, InputRecordViewHtmlItem>();
			Mapper.CreateMap<InputRecordViewHtmlItem, RecordViewHtmlItem>();
			Mapper.CreateMap<RecordViewHtmlItem, DbRecordViewHtmlItem>();
			Mapper.CreateMap<DbRecordViewHtmlItem, RecordViewHtmlItem>();

			Mapper.CreateMap<RecordViewRelationFieldItem, InputRecordViewRelationFieldItem>();
			Mapper.CreateMap<InputRecordViewRelationFieldItem, RecordViewRelationFieldItem>()
				.ForMember(x => x.FieldId, opt => opt.MapFrom(y => (y.FieldId.HasValue) ? y.FieldId.Value : Guid.Empty))
				.ForMember(x => x.RelationId, opt => opt.MapFrom(y => (y.RelationId.HasValue) ? y.RelationId.Value : Guid.Empty));
			Mapper.CreateMap<RecordViewRelationFieldItem, DbRecordViewRelationFieldItem>();
			Mapper.CreateMap<DbRecordViewRelationFieldItem, RecordViewRelationFieldItem>();

			Mapper.CreateMap<RecordViewViewItem, InputRecordViewViewItem>();
			Mapper.CreateMap<InputRecordViewViewItem, RecordViewViewItem>()
				.ForMember(x => x.ViewId, opt => opt.MapFrom(y => (y.ViewId.HasValue) ? y.ViewId.Value : Guid.Empty));
			Mapper.CreateMap<RecordViewViewItem, DbRecordViewViewItem>();
			Mapper.CreateMap<DbRecordViewViewItem, RecordViewViewItem>();


			Mapper.CreateMap<RecordViewListItem, InputRecordViewListItem>();
			Mapper.CreateMap<InputRecordViewListItem, RecordViewListItem>()
				.ForMember(x => x.ListId, opt => opt.MapFrom(y => (y.ListId.HasValue) ? y.ListId.Value : Guid.Empty));
			Mapper.CreateMap<RecordViewListItem, DbRecordViewListItem>();
			Mapper.CreateMap<DbRecordViewListItem, RecordViewListItem>();


			Mapper.CreateMap<RecordViewFieldItem, InputRecordViewFieldItem>();
			Mapper.CreateMap<InputRecordViewFieldItem, RecordViewFieldItem>()
				.ForMember(x => x.FieldId, opt => opt.MapFrom(y => (y.FieldId.HasValue) ? y.FieldId.Value : Guid.Empty));
			Mapper.CreateMap<RecordViewFieldItem, DbRecordViewFieldItem>();
			Mapper.CreateMap<DbRecordViewFieldItem, RecordViewFieldItem>();

			Mapper.CreateMap<RecordViewItemBase, InputRecordViewItemBase>()
				.Include<RecordViewFieldItem, InputRecordViewFieldItem>()
				.Include<RecordViewListItem, InputRecordViewListItem>()
				.Include<RecordViewViewItem, InputRecordViewViewItem>()
				.Include<RecordViewRelationFieldItem, InputRecordViewRelationFieldItem>()
				.Include<RecordViewRelationViewItem, InputRecordViewRelationViewItem>()
				.Include<RecordViewRelationListItem, InputRecordViewRelationListItem>()
				.Include<RecordViewRelationTreeItem, InputRecordViewRelationTreeItem>()
				.Include<RecordViewHtmlItem, InputRecordViewHtmlItem>();
			Mapper.CreateMap<InputRecordViewItemBase, RecordViewItemBase>()
				.Include<InputRecordViewFieldItem, RecordViewFieldItem>()
				.Include<InputRecordViewListItem, RecordViewListItem>()
				.Include<InputRecordViewViewItem, RecordViewViewItem>()
				.Include<InputRecordViewRelationFieldItem, RecordViewRelationFieldItem>()
				.Include<InputRecordViewRelationViewItem, RecordViewRelationViewItem>()
				.Include<InputRecordViewRelationListItem, RecordViewRelationListItem>()
				.Include<InputRecordViewRelationTreeItem, RecordViewRelationTreeItem>()
				.Include<InputRecordViewHtmlItem, RecordViewHtmlItem>()
				.ForMember(x => x.EntityId, opt => opt.MapFrom(y => (y.EntityId.HasValue) ? y.EntityId.Value : Guid.Empty));

			Mapper.CreateMap<RecordViewItemBase, DbRecordViewItemBase>()
				.Include<RecordViewFieldItem, DbRecordViewFieldItem>()
				.Include<RecordViewListItem, DbRecordViewListItem>()
				.Include<RecordViewViewItem, DbRecordViewViewItem>()
				.Include<RecordViewRelationFieldItem, DbRecordViewRelationFieldItem>()
				.Include<RecordViewRelationViewItem, DbRecordViewRelationViewItem>()
				.Include<RecordViewRelationListItem, DbRecordViewRelationListItem>()
				.Include<RecordViewRelationTreeItem, DbRecordViewRelationTreeItem>()
				.Include<RecordViewHtmlItem, DbRecordViewHtmlItem>();
			Mapper.CreateMap<DbRecordViewItemBase, RecordViewItemBase>()
				.Include<DbRecordViewFieldItem, RecordViewFieldItem>()
				.Include<DbRecordViewListItem, RecordViewListItem>()
				.Include<DbRecordViewViewItem, RecordViewViewItem>()
				.Include<DbRecordViewRelationFieldItem, RecordViewRelationFieldItem>()
				.Include<DbRecordViewRelationViewItem, RecordViewRelationViewItem>()
				.Include<DbRecordViewRelationListItem, RecordViewRelationListItem>()
				.Include<DbRecordViewRelationTreeItem, RecordViewRelationTreeItem>()
				.Include<DbRecordViewHtmlItem, RecordViewHtmlItem>();

			Mapper.CreateMap<RecordViewColumn, InputRecordViewColumn>();
			Mapper.CreateMap<InputRecordViewColumn, RecordViewColumn>();
			Mapper.CreateMap<RecordViewColumn, DbRecordViewColumn>();
			Mapper.CreateMap<DbRecordViewColumn, RecordViewColumn>();

			Mapper.CreateMap<RecordViewRow, InputRecordViewRow>();
			Mapper.CreateMap<InputRecordViewRow, RecordViewRow>()
				.ForMember(x => x.Id, opt => opt.MapFrom(y => (y.Id.HasValue) ? y.Id.Value : Guid.Empty));
			Mapper.CreateMap<RecordViewRow, DbRecordViewRow>();
			Mapper.CreateMap<DbRecordViewRow, RecordViewRow>();

			Mapper.CreateMap<RecordViewSection, InputRecordViewSection>();
			Mapper.CreateMap<InputRecordViewSection, RecordViewSection>()
				.ForMember(x => x.Id, opt => opt.MapFrom(y => (y.Id.HasValue) ? y.Id.Value : Guid.Empty))
				.ForMember(x => x.Collapsed, opt => opt.MapFrom(y => (y.Collapsed.HasValue) ? y.Collapsed.Value : false))
				.ForMember(x => x.ShowLabel, opt => opt.MapFrom(y => (y.ShowLabel.HasValue) ? y.ShowLabel.Value : false));
			Mapper.CreateMap<RecordViewSection, DbRecordViewSection>();
			Mapper.CreateMap<DbRecordViewSection, RecordViewSection>();

			Mapper.CreateMap<RecordViewRegion, InputRecordViewRegion>();
			Mapper.CreateMap<InputRecordViewRegion, RecordViewRegion>()
				.ForMember(x => x.Render, opt => opt.MapFrom(y => (y.Render.HasValue) ? y.Render.Value : false));
			Mapper.CreateMap<RecordViewRegion, DbRecordViewRegion>();
			Mapper.CreateMap<DbRecordViewRegion, RecordViewRegion>();

			Mapper.CreateMap<RecordViewSidebarRelationViewItem, InputRecordViewSidebarRelationViewItem>();
			Mapper.CreateMap<InputRecordViewSidebarRelationViewItem, RecordViewSidebarRelationViewItem>()
				.ForMember(x => x.ViewId, opt => opt.MapFrom(y => (y.ViewId.HasValue) ? y.ViewId.Value : Guid.Empty))
				.ForMember(x => x.RelationId, opt => opt.MapFrom(y => (y.RelationId.HasValue) ? y.RelationId.Value : Guid.Empty));
			Mapper.CreateMap<RecordViewSidebarRelationViewItem, DbRecordViewSidebarRelationViewItem>();
			Mapper.CreateMap<DbRecordViewSidebarRelationViewItem, RecordViewSidebarRelationViewItem>();

			Mapper.CreateMap<RecordViewSidebarRelationListItem, InputRecordViewSidebarRelationListItem>();
			Mapper.CreateMap<InputRecordViewSidebarRelationListItem, RecordViewSidebarRelationListItem>()
				.ForMember(x => x.ListId, opt => opt.MapFrom(y => (y.ListId.HasValue) ? y.ListId.Value : Guid.Empty))
				.ForMember(x => x.RelationId, opt => opt.MapFrom(y => (y.RelationId.HasValue) ? y.RelationId.Value : Guid.Empty));
			Mapper.CreateMap<RecordViewSidebarRelationListItem, DbRecordViewSidebarRelationListItem>();
			Mapper.CreateMap<DbRecordViewSidebarRelationListItem, RecordViewSidebarRelationListItem>();

			Mapper.CreateMap<RecordViewSidebarRelationTreeItem, InputRecordViewSidebarRelationTreeItem>();
			Mapper.CreateMap<InputRecordViewSidebarRelationTreeItem, RecordViewSidebarRelationTreeItem>()
				.ForMember(x => x.TreeId, opt => opt.MapFrom(y => (y.TreeId.HasValue) ? y.TreeId.Value : Guid.Empty))
				.ForMember(x => x.RelationId, opt => opt.MapFrom(y => (y.RelationId.HasValue) ? y.RelationId.Value : Guid.Empty));
			Mapper.CreateMap<RecordViewSidebarRelationTreeItem, DbRecordViewSidebarRelationTreeItem>();
			Mapper.CreateMap<DbRecordViewSidebarRelationTreeItem, RecordViewSidebarRelationTreeItem>();

			Mapper.CreateMap<RecordViewSidebarListItem, InputRecordViewSidebarListItem>();
			Mapper.CreateMap<InputRecordViewSidebarViewItem, RecordViewSidebarViewItem>()
				.ForMember(x => x.ViewId, opt => opt.MapFrom(y => (y.ViewId.HasValue) ? y.ViewId.Value : Guid.Empty));
			Mapper.CreateMap<RecordViewSidebarViewItem, DbRecordViewSidebarViewItem>();
			Mapper.CreateMap<DbRecordViewSidebarViewItem, RecordViewSidebarViewItem>();

			Mapper.CreateMap<RecordViewSidebarListItem, InputRecordViewSidebarListItem>();
			Mapper.CreateMap<InputRecordViewSidebarListItem, RecordViewSidebarListItem>()
				.ForMember(x => x.ListId, opt => opt.MapFrom(y => (y.ListId.HasValue) ? y.ListId.Value : Guid.Empty));
			Mapper.CreateMap<RecordViewSidebarListItem, DbRecordViewSidebarListItem>();
			Mapper.CreateMap<DbRecordViewSidebarListItem, RecordViewSidebarListItem>();

			Mapper.CreateMap<RecordViewSidebarItemBase, InputRecordViewSidebarItemBase>()
				.Include<RecordViewSidebarListItem, InputRecordViewSidebarListItem>()
				.Include<RecordViewSidebarViewItem, InputRecordViewSidebarViewItem>()
				.Include<RecordViewSidebarRelationListItem, InputRecordViewSidebarRelationListItem>()
				.Include<RecordViewSidebarRelationViewItem, InputRecordViewSidebarRelationViewItem>()
				.Include<RecordViewSidebarRelationTreeItem, InputRecordViewSidebarRelationTreeItem>();
			Mapper.CreateMap<InputRecordViewSidebarItemBase, RecordViewSidebarItemBase>()
				.Include<InputRecordViewSidebarListItem, RecordViewSidebarListItem>()
				.Include<InputRecordViewSidebarViewItem, RecordViewSidebarViewItem>()
				.Include<InputRecordViewSidebarRelationListItem, RecordViewSidebarRelationListItem>()
				.Include<InputRecordViewSidebarRelationViewItem, RecordViewSidebarRelationViewItem>()
				.Include<InputRecordViewSidebarRelationTreeItem, RecordViewSidebarRelationTreeItem>()
				.ForMember(x => x.EntityId, opt => opt.MapFrom(y => (y.EntityId.HasValue) ? y.EntityId.Value : Guid.Empty));

			Mapper.CreateMap<RecordViewSidebarItemBase, DbRecordViewSidebarItemBase>()
				.Include<RecordViewSidebarListItem, DbRecordViewSidebarListItem>()
				.Include<RecordViewSidebarViewItem, DbRecordViewSidebarViewItem>()
				.Include<RecordViewSidebarRelationListItem, DbRecordViewSidebarRelationListItem>()
				.Include<RecordViewSidebarRelationViewItem, DbRecordViewSidebarRelationViewItem>()
				.Include<RecordViewSidebarRelationTreeItem, DbRecordViewSidebarRelationTreeItem>();
			Mapper.CreateMap<DbRecordViewSidebarItemBase, RecordViewSidebarItemBase>()
				.Include<DbRecordViewSidebarListItem, RecordViewSidebarListItem>()
				.Include<DbRecordViewSidebarViewItem, RecordViewSidebarViewItem>()
				.Include<DbRecordViewSidebarRelationListItem, RecordViewSidebarRelationListItem>()
				.Include<DbRecordViewSidebarRelationViewItem, RecordViewSidebarRelationViewItem>()
				.Include<DbRecordViewSidebarRelationTreeItem, RecordViewSidebarRelationTreeItem>();

			Mapper.CreateMap<RecordViewSidebar, InputRecordViewSidebar>();
			Mapper.CreateMap<InputRecordViewSidebar, RecordViewSidebar>()
				.ForMember(x => x.Render, opt => opt.MapFrom(y => (y.Render.HasValue) ? y.Render.Value : false));
			Mapper.CreateMap<RecordViewSidebar, DbRecordViewSidebar>();
			Mapper.CreateMap<DbRecordViewSidebar, RecordViewSidebar>();

			Mapper.CreateMap<RecordView, InputRecordView>();
			Mapper.CreateMap<InputRecordView, RecordView>()
				.ForMember(x => x.Id, opt => opt.MapFrom(y => (y.Id.HasValue) ? y.Id.Value : Guid.Empty));
			Mapper.CreateMap<RecordView, DbRecordView>()
				.ForMember(x => x.Type, opt => opt.MapFrom(y => GetViewTypeId(y.Type)));
			Mapper.CreateMap<DbRecordView, RecordView>()
				.ForMember(x => x.Type, opt => opt.MapFrom(y => Enum.GetName(typeof(RecordViewType), y.Type).ToLower()));

			Mapper.CreateMap<EntityRelationOptions, DbEntityRelationOptions>();
			Mapper.CreateMap<DbEntityRelationOptions, EntityRelationOptions>();
		}

		private RecordViewType GetViewTypeId(string name)
		{
			RecordViewType type = RecordViewType.General;

			Enum.TryParse(name, true, out type);

			return type;
		}
	}
}
