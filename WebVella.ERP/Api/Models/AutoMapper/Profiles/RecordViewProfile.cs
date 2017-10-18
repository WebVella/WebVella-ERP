using AutoMapper;
using System;
using WebVella.ERP.Database;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
{
    internal class RecordViewProfile : Profile
    {
        public RecordViewProfile()
        {
            CreateMap<RecordViewRelationTreeItem, InputRecordViewRelationTreeItem>();
            CreateMap<InputRecordViewRelationTreeItem, RecordViewRelationTreeItem>()
                 .ForMember(x => x.TreeId, opt => opt.MapFrom(y => (y.TreeId.HasValue) ? y.TreeId.Value : Guid.Empty))
                .ForMember(x => x.RelationId, opt => opt.MapFrom(y => (y.RelationId.HasValue) ? y.RelationId.Value : Guid.Empty));
            CreateMap<RecordViewRelationTreeItem, DbRecordViewRelationTreeItem>();
            CreateMap<DbRecordViewRelationTreeItem, RecordViewRelationTreeItem>();

            CreateMap<RecordViewRelationListItem, InputRecordViewRelationListItem>();
            CreateMap<InputRecordViewRelationListItem, RecordViewRelationListItem>()
                 .ForMember(x => x.ListId, opt => opt.MapFrom(y => (y.ListId.HasValue) ? y.ListId.Value : Guid.Empty))
                .ForMember(x => x.RelationId, opt => opt.MapFrom(y => (y.RelationId.HasValue) ? y.RelationId.Value : Guid.Empty));
            CreateMap<RecordViewRelationListItem, DbRecordViewRelationListItem>();
            CreateMap<DbRecordViewRelationListItem, RecordViewRelationListItem>();

            CreateMap<RecordViewRelationViewItem, InputRecordViewRelationViewItem>();
            CreateMap<InputRecordViewRelationViewItem, RecordViewRelationViewItem>()
                .ForMember(x => x.ViewId, opt => opt.MapFrom(y => (y.ViewId.HasValue) ? y.ViewId.Value : Guid.Empty))
                .ForMember(x => x.RelationId, opt => opt.MapFrom(y => (y.RelationId.HasValue) ? y.RelationId.Value : Guid.Empty));
            CreateMap<RecordViewRelationViewItem, DbRecordViewRelationViewItem>();
            CreateMap<DbRecordViewRelationViewItem, RecordViewRelationViewItem>();

            CreateMap<RecordViewHtmlItem, InputRecordViewHtmlItem>();
            CreateMap<InputRecordViewHtmlItem, RecordViewHtmlItem>();
            CreateMap<RecordViewHtmlItem, DbRecordViewHtmlItem>();
            CreateMap<DbRecordViewHtmlItem, RecordViewHtmlItem>();

            CreateMap<RecordViewRelationFieldItem, InputRecordViewRelationFieldItem>();
            CreateMap<InputRecordViewRelationFieldItem, RecordViewRelationFieldItem>()
                .ForMember(x => x.FieldId, opt => opt.MapFrom(y => (y.FieldId.HasValue) ? y.FieldId.Value : Guid.Empty))
                .ForMember(x => x.RelationId, opt => opt.MapFrom(y => (y.RelationId.HasValue) ? y.RelationId.Value : Guid.Empty));
            CreateMap<RecordViewRelationFieldItem, DbRecordViewRelationFieldItem>();
            CreateMap<DbRecordViewRelationFieldItem, RecordViewRelationFieldItem>();

            CreateMap<RecordViewViewItem, InputRecordViewViewItem>();
            CreateMap<InputRecordViewViewItem, RecordViewViewItem>()
                .ForMember(x => x.ViewId, opt => opt.MapFrom(y => (y.ViewId.HasValue) ? y.ViewId.Value : Guid.Empty));
            CreateMap<RecordViewViewItem, DbRecordViewViewItem>();
            CreateMap<DbRecordViewViewItem, RecordViewViewItem>();


            CreateMap<RecordViewListItem, InputRecordViewListItem>();
            CreateMap<InputRecordViewListItem, RecordViewListItem>()
                .ForMember(x => x.ListId, opt => opt.MapFrom(y => (y.ListId.HasValue) ? y.ListId.Value : Guid.Empty));
            CreateMap<RecordViewListItem, DbRecordViewListItem>();
            CreateMap<DbRecordViewListItem, RecordViewListItem>();


            CreateMap<RecordViewFieldItem, InputRecordViewFieldItem>();
            CreateMap<InputRecordViewFieldItem, RecordViewFieldItem>()
                .ForMember(x => x.FieldId, opt => opt.MapFrom(y => (y.FieldId.HasValue) ? y.FieldId.Value : Guid.Empty));
            CreateMap<RecordViewFieldItem, DbRecordViewFieldItem>();
            CreateMap<DbRecordViewFieldItem, RecordViewFieldItem>();

            CreateMap<RecordViewItemBase, InputRecordViewItemBase>()
                .Include<RecordViewFieldItem, InputRecordViewFieldItem>()
                .Include<RecordViewListItem, InputRecordViewListItem>()
                .Include<RecordViewViewItem, InputRecordViewViewItem>()
                .Include<RecordViewRelationFieldItem, InputRecordViewRelationFieldItem>()
                .Include<RecordViewRelationViewItem, InputRecordViewRelationViewItem>()
                .Include<RecordViewRelationListItem, InputRecordViewRelationListItem>()
                .Include<RecordViewRelationTreeItem, InputRecordViewRelationTreeItem>()
                .Include<RecordViewHtmlItem, InputRecordViewHtmlItem>();
            CreateMap<InputRecordViewItemBase, RecordViewItemBase>()
                .Include<InputRecordViewFieldItem, RecordViewFieldItem>()
                .Include<InputRecordViewListItem, RecordViewListItem>()
                .Include<InputRecordViewViewItem, RecordViewViewItem>()
                .Include<InputRecordViewRelationFieldItem, RecordViewRelationFieldItem>()
                .Include<InputRecordViewRelationViewItem, RecordViewRelationViewItem>()
                .Include<InputRecordViewRelationListItem, RecordViewRelationListItem>()
                .Include<InputRecordViewRelationTreeItem, RecordViewRelationTreeItem>()
                .Include<InputRecordViewHtmlItem, RecordViewHtmlItem>()
                .ForMember(x => x.EntityId, opt => opt.MapFrom(y => (y.EntityId.HasValue) ? y.EntityId.Value : Guid.Empty));

            CreateMap<RecordViewItemBase, DbRecordViewItemBase>()
                .Include<RecordViewFieldItem, DbRecordViewFieldItem>()
                .Include<RecordViewListItem, DbRecordViewListItem>()
                .Include<RecordViewViewItem, DbRecordViewViewItem>()
                .Include<RecordViewRelationFieldItem, DbRecordViewRelationFieldItem>()
                .Include<RecordViewRelationViewItem, DbRecordViewRelationViewItem>()
                .Include<RecordViewRelationListItem, DbRecordViewRelationListItem>()
                .Include<RecordViewRelationTreeItem, DbRecordViewRelationTreeItem>()
                .Include<RecordViewHtmlItem, DbRecordViewHtmlItem>();
            CreateMap<DbRecordViewItemBase, RecordViewItemBase>()
                .Include<DbRecordViewFieldItem, RecordViewFieldItem>()
                .Include<DbRecordViewListItem, RecordViewListItem>()
                .Include<DbRecordViewViewItem, RecordViewViewItem>()
                .Include<DbRecordViewRelationFieldItem, RecordViewRelationFieldItem>()
                .Include<DbRecordViewRelationViewItem, RecordViewRelationViewItem>()
                .Include<DbRecordViewRelationListItem, RecordViewRelationListItem>()
                .Include<DbRecordViewRelationTreeItem, RecordViewRelationTreeItem>()
                .Include<DbRecordViewHtmlItem, RecordViewHtmlItem>();

            CreateMap<RecordViewColumn, InputRecordViewColumn>();
            CreateMap<InputRecordViewColumn, RecordViewColumn>();
            CreateMap<RecordViewColumn, DbRecordViewColumn>();
            CreateMap<DbRecordViewColumn, RecordViewColumn>();

            CreateMap<RecordViewRow, InputRecordViewRow>();
            CreateMap<InputRecordViewRow, RecordViewRow>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => (y.Id.HasValue) ? y.Id.Value : Guid.Empty));
            CreateMap<RecordViewRow, DbRecordViewRow>();
            CreateMap<DbRecordViewRow, RecordViewRow>();

            CreateMap<RecordViewSection, InputRecordViewSection>();
            CreateMap<InputRecordViewSection, RecordViewSection>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => (y.Id.HasValue) ? y.Id.Value : Guid.Empty))
                .ForMember(x => x.Collapsed, opt => opt.MapFrom(y => (y.Collapsed.HasValue) ? y.Collapsed.Value : false))
                .ForMember(x => x.ShowLabel, opt => opt.MapFrom(y => (y.ShowLabel.HasValue) ? y.ShowLabel.Value : false));
            CreateMap<RecordViewSection, DbRecordViewSection>();
            CreateMap<DbRecordViewSection, RecordViewSection>();

            CreateMap<RecordViewRegion, InputRecordViewRegion>();
            CreateMap<InputRecordViewRegion, RecordViewRegion>()
                .ForMember(x => x.Render, opt => opt.MapFrom(y => (y.Render.HasValue) ? y.Render.Value : false));
            CreateMap<RecordViewRegion, DbRecordViewRegion>();
            CreateMap<DbRecordViewRegion, RecordViewRegion>();

            CreateMap<RecordViewSidebarRelationViewItem, InputRecordViewSidebarRelationViewItem>();
            CreateMap<InputRecordViewSidebarRelationViewItem, RecordViewSidebarRelationViewItem>()
                .ForMember(x => x.ViewId, opt => opt.MapFrom(y => (y.ViewId.HasValue) ? y.ViewId.Value : Guid.Empty))
                .ForMember(x => x.RelationId, opt => opt.MapFrom(y => (y.RelationId.HasValue) ? y.RelationId.Value : Guid.Empty));
            CreateMap<RecordViewSidebarRelationViewItem, DbRecordViewSidebarRelationViewItem>(MemberList.None);
            CreateMap<DbRecordViewSidebarRelationViewItem, RecordViewSidebarRelationViewItem>();

            CreateMap<RecordViewSidebarRelationListItem, InputRecordViewSidebarRelationListItem>();
            CreateMap<InputRecordViewSidebarRelationListItem, RecordViewSidebarRelationListItem>()
                .ForMember(x => x.ListId, opt => opt.MapFrom(y => (y.ListId.HasValue) ? y.ListId.Value : Guid.Empty))
                .ForMember(x => x.RelationId, opt => opt.MapFrom(y => (y.RelationId.HasValue) ? y.RelationId.Value : Guid.Empty));
            CreateMap<RecordViewSidebarRelationListItem, DbRecordViewSidebarRelationListItem>(MemberList.None);
            CreateMap<DbRecordViewSidebarRelationListItem, RecordViewSidebarRelationListItem>();

            CreateMap<RecordViewSidebarRelationTreeItem, InputRecordViewSidebarRelationTreeItem>();
            CreateMap<InputRecordViewSidebarRelationTreeItem, RecordViewSidebarRelationTreeItem>()
                .ForMember(x => x.TreeId, opt => opt.MapFrom(y => (y.TreeId.HasValue) ? y.TreeId.Value : Guid.Empty))
                .ForMember(x => x.RelationId, opt => opt.MapFrom(y => (y.RelationId.HasValue) ? y.RelationId.Value : Guid.Empty));
            CreateMap<RecordViewSidebarRelationTreeItem, DbRecordViewSidebarRelationTreeItem>(MemberList.None);
            CreateMap<DbRecordViewSidebarRelationTreeItem, RecordViewSidebarRelationTreeItem>();

            CreateMap<RecordViewSidebarViewItem, InputRecordViewSidebarViewItem>();
            CreateMap<InputRecordViewSidebarViewItem, RecordViewSidebarViewItem>()
                .ForMember(x => x.ViewId, opt => opt.MapFrom(y => (y.ViewId.HasValue) ? y.ViewId.Value : Guid.Empty));
            CreateMap<RecordViewSidebarViewItem, DbRecordViewSidebarViewItem>(MemberList.None);
            CreateMap<DbRecordViewSidebarViewItem, RecordViewSidebarViewItem>();

            CreateMap<RecordViewSidebarListItem, InputRecordViewSidebarListItem>();
            CreateMap<InputRecordViewSidebarListItem, RecordViewSidebarListItem>()
                .ForMember(x => x.ListId, opt => opt.MapFrom(y => (y.ListId.HasValue) ? y.ListId.Value : Guid.Empty));
            CreateMap<RecordViewSidebarListItem, DbRecordViewSidebarListItem>(MemberList.None);
            CreateMap<DbRecordViewSidebarListItem, RecordViewSidebarListItem>();

            CreateMap<RecordViewSidebarItemBase, InputRecordViewSidebarItemBase>()
                .Include<RecordViewSidebarListItem, InputRecordViewSidebarListItem>()
                .Include<RecordViewSidebarViewItem, InputRecordViewSidebarViewItem>()
                .Include<RecordViewSidebarRelationListItem, InputRecordViewSidebarRelationListItem>()
                .Include<RecordViewSidebarRelationViewItem, InputRecordViewSidebarRelationViewItem>()
                .Include<RecordViewSidebarRelationTreeItem, InputRecordViewSidebarRelationTreeItem>();
            CreateMap<InputRecordViewSidebarItemBase, RecordViewSidebarItemBase>()
                .Include<InputRecordViewSidebarListItem, RecordViewSidebarListItem>()
                .Include<InputRecordViewSidebarViewItem, RecordViewSidebarViewItem>()
                .Include<InputRecordViewSidebarRelationListItem, RecordViewSidebarRelationListItem>()
                .Include<InputRecordViewSidebarRelationViewItem, RecordViewSidebarRelationViewItem>()
                .Include<InputRecordViewSidebarRelationTreeItem, RecordViewSidebarRelationTreeItem>()
                .ForMember(x => x.EntityId, opt => opt.MapFrom(y => (y.EntityId.HasValue) ? y.EntityId.Value : Guid.Empty));

            CreateMap<RecordViewSidebarItemBase, DbRecordViewSidebarItemBase>()
                .Include<RecordViewSidebarListItem, DbRecordViewSidebarListItem>()
                .Include<RecordViewSidebarViewItem, DbRecordViewSidebarViewItem>()
                .Include<RecordViewSidebarRelationListItem, DbRecordViewSidebarRelationListItem>()
                .Include<RecordViewSidebarRelationViewItem, DbRecordViewSidebarRelationViewItem>()
                .Include<RecordViewSidebarRelationTreeItem, DbRecordViewSidebarRelationTreeItem>()
                .ForSourceMember(x => x.DataName, opt => opt.Ignore())
                .ForSourceMember(x => x.EntityLabel, opt => opt.Ignore())
                .ForSourceMember(x => x.EntityName, opt => opt.Ignore())
                .ForSourceMember(x => x.EntityLabelPlural, opt => opt.Ignore());
            CreateMap<DbRecordViewSidebarItemBase, RecordViewSidebarItemBase>()
                .Include<DbRecordViewSidebarListItem, RecordViewSidebarListItem>()
                .Include<DbRecordViewSidebarViewItem, RecordViewSidebarViewItem>()
                .Include<DbRecordViewSidebarRelationListItem, RecordViewSidebarRelationListItem>()
                .Include<DbRecordViewSidebarRelationViewItem, RecordViewSidebarRelationViewItem>()
                .Include<DbRecordViewSidebarRelationTreeItem, RecordViewSidebarRelationTreeItem>();

            CreateMap<RecordViewSidebar, InputRecordViewSidebar>();
            CreateMap<InputRecordViewSidebar, RecordViewSidebar>()
                .ForMember(x => x.Render, opt => opt.MapFrom(y => (y.Render.HasValue) ? y.Render.Value : false));
            CreateMap<RecordViewSidebar, DbRecordViewSidebar>(MemberList.None);
            CreateMap<DbRecordViewSidebar, RecordViewSidebar>();

            CreateMap<RecordView, InputRecordView>();
            CreateMap<InputRecordView, RecordView>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => (y.Id.HasValue) ? y.Id.Value : Guid.Empty));
            CreateMap<RecordView, DbRecordView>()
                .ForMember(x => x.Type, opt => opt.MapFrom(y => GetViewTypeId(y.Type)));
            CreateMap<DbRecordView, RecordView>()
                .ForMember(x => x.Type, opt => opt.MapFrom(y => Enum.GetName(typeof(RecordViewType), y.Type).ToLower()));

            CreateMap<EntityRelationOptions, DbEntityRelationOptions>();
            CreateMap<DbEntityRelationOptions, EntityRelationOptions>();
            CreateMap<EntityRelationOptionsItem, EntityRelationOptions>();
            CreateMap<EntityRelationOptions, EntityRelationOptionsItem>();
        }

        private RecordViewType GetViewTypeId(string name)
        {
            RecordViewType type = RecordViewType.General;

            Enum.TryParse(name, true, out type);

            return type;
        }
    }
}
