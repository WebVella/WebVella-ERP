using AutoMapper;
using System;
using WebVella.ERP.Database;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
{
    internal class RecordsListProfile : Profile
    {
        public RecordsListProfile()
        {
            CreateMap<RecordListRelationTreeItem, InputRecordListRelationTreeItem>();
            CreateMap<InputRecordListRelationTreeItem, RecordListRelationTreeItem>()
                .ForMember(x => x.RelationId, opt => opt.MapFrom(y => (y.RelationId.HasValue) ? y.RelationId.Value : Guid.Empty))
                .ForMember(x => x.TreeId, opt => opt.MapFrom(y => (y.TreeId.HasValue) ? y.TreeId.Value : Guid.Empty));
            CreateMap<RecordListRelationTreeItem, DbRecordListRelationTreeItem>();
            CreateMap<DbRecordListRelationTreeItem, RecordListRelationTreeItem>();

            CreateMap<RecordListRelationListItem, InputRecordListRelationListItem>();
            CreateMap<InputRecordListRelationListItem, RecordListRelationListItem>()
                .ForMember(x => x.RelationId, opt => opt.MapFrom(y => (y.RelationId.HasValue) ? y.RelationId.Value : Guid.Empty))
                .ForMember(x => x.ListId, opt => opt.MapFrom(y => (y.ListId.HasValue) ? y.ListId.Value : Guid.Empty));
            CreateMap<RecordListRelationListItem, DbRecordListRelationListItem>();
            CreateMap<DbRecordListRelationListItem, RecordListRelationListItem>();

            CreateMap<RecordListListItem, InputRecordListListItem>();
            CreateMap<InputRecordListListItem, RecordListListItem>()
                .ForMember(x => x.ListId, opt => opt.MapFrom(y => (y.ListId.HasValue) ? y.ListId.Value : Guid.Empty));
            CreateMap<RecordListListItem, DbRecordListListItem>();
            CreateMap<DbRecordListListItem, RecordListListItem>();

            CreateMap<RecordListRelationViewItem, InputRecordListRelationViewItem>();
            CreateMap<InputRecordListRelationViewItem, RecordListRelationViewItem>()
                .ForMember(x => x.RelationId, opt => opt.MapFrom(y => (y.RelationId.HasValue) ? y.RelationId.Value : Guid.Empty))
                .ForMember(x => x.ViewId, opt => opt.MapFrom(y => (y.ViewId.HasValue) ? y.ViewId.Value : Guid.Empty));
            CreateMap<RecordListRelationViewItem, DbRecordListRelationViewItem>();
            CreateMap<DbRecordListRelationViewItem, RecordListRelationViewItem>();

            CreateMap<RecordListViewItem, InputRecordListViewItem>();
            CreateMap<InputRecordListViewItem, RecordListViewItem>()
                .ForMember(x => x.ViewId, opt => opt.MapFrom(y => (y.ViewId.HasValue) ? y.ViewId.Value : Guid.Empty));
            CreateMap<RecordListViewItem, DbRecordListViewItem>();
            CreateMap<DbRecordListViewItem, RecordListViewItem>();

            CreateMap<RecordListRelationFieldItem, InputRecordListRelationFieldItem>();
            CreateMap<InputRecordListRelationFieldItem, RecordListRelationFieldItem>()
                .ForMember(x => x.RelationId, opt => opt.MapFrom(y => (y.RelationId.HasValue) ? y.RelationId.Value : Guid.Empty))
                .ForMember(x => x.FieldId, opt => opt.MapFrom(y => (y.FieldId.HasValue) ? y.FieldId.Value : Guid.Empty));
            CreateMap<RecordListRelationFieldItem, DbRecordListRelationFieldItem>();
            CreateMap<DbRecordListRelationFieldItem, RecordListRelationFieldItem>();

            CreateMap<RecordListFieldItem, InputRecordListFieldItem>();
            CreateMap<InputRecordListFieldItem, RecordListFieldItem>()
                .ForMember(x => x.FieldId, opt => opt.MapFrom(y => (y.FieldId.HasValue) ? y.FieldId.Value : Guid.Empty));
            CreateMap<RecordListFieldItem, DbRecordListFieldItem>();
            CreateMap<DbRecordListFieldItem, RecordListFieldItem>();

            CreateMap<RecordListItemBase, InputRecordListItemBase>()
                .Include<RecordListFieldItem, InputRecordListFieldItem>()
                .Include<RecordListRelationFieldItem, InputRecordListRelationFieldItem>()
                .Include<RecordListViewItem, InputRecordListViewItem>()
                .Include<RecordListRelationViewItem, InputRecordListRelationViewItem>()
                .Include<RecordListListItem, InputRecordListListItem>()
                .Include<RecordListRelationListItem, InputRecordListRelationListItem>()
                .Include<RecordListRelationTreeItem, InputRecordListRelationTreeItem>();
            CreateMap<InputRecordListItemBase, RecordListItemBase>()
                .Include<InputRecordListFieldItem, RecordListFieldItem>()
                .Include<InputRecordListRelationFieldItem, RecordListRelationFieldItem>()
                .Include<InputRecordListViewItem, RecordListViewItem>()
                .Include<InputRecordListRelationViewItem, RecordListRelationViewItem>()
                .Include<InputRecordListListItem, RecordListListItem>()
                .Include<InputRecordListRelationListItem, RecordListRelationListItem>()
                .Include<InputRecordListRelationTreeItem, RecordListRelationTreeItem>()
                .ForMember(x => x.EntityId, opt => opt.MapFrom(y => (y.EntityId.HasValue) ? y.EntityId.Value : Guid.Empty));

            CreateMap<RecordListItemBase, DbRecordListItemBase>()
                .Include<RecordListFieldItem, DbRecordListFieldItem>()
                .Include<RecordListRelationFieldItem, DbRecordListRelationFieldItem>()
                .Include<RecordListViewItem, DbRecordListViewItem>()
                .Include<RecordListRelationViewItem, DbRecordListRelationViewItem>()
                .Include<RecordListListItem, DbRecordListListItem>()
                .Include<RecordListRelationListItem, DbRecordListRelationListItem>()
                .Include<RecordListRelationTreeItem, DbRecordListRelationTreeItem>();
            CreateMap<DbRecordListItemBase, RecordListItemBase>()
                .Include<DbRecordListFieldItem, RecordListFieldItem>()
                .Include<DbRecordListRelationFieldItem, RecordListRelationFieldItem>()
                .Include<DbRecordListViewItem, RecordListViewItem>()
                .Include<DbRecordListRelationViewItem, RecordListRelationViewItem>()
                .Include<DbRecordListListItem, RecordListListItem>()
                .Include<DbRecordListRelationListItem, RecordListRelationListItem>()
                .Include<DbRecordListRelationTreeItem, RecordListRelationTreeItem>();

            CreateMap<RecordListQuery, InputRecordListQuery>();
            CreateMap<InputRecordListQuery, RecordListQuery>();
            CreateMap<RecordListQuery, DbRecordListQuery>();
            CreateMap<DbRecordListQuery, RecordListQuery>();

            CreateMap<RecordListSort, InputRecordListSort>();
            CreateMap<InputRecordListSort, RecordListSort>();
            CreateMap<RecordListSort, DbRecordListSort>()
                .ForMember(x => x.SortType, opt => opt.MapFrom(y => GetListSortTypeId(y.SortType)));
            CreateMap<DbRecordListSort, RecordListSort>()
                .ForMember(x => x.SortType, opt => opt.MapFrom(y => Enum.GetName(typeof(QuerySortType), y.SortType).ToLower()));

            CreateMap<RecordList, InputRecordList>();
            CreateMap<InputRecordList, RecordList>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => (y.Id.HasValue) ? y.Id.Value : Guid.Empty));
            CreateMap<RecordList, DbRecordList>()
                .ForMember(x => x.Type, opt => opt.MapFrom(y => GetListTypeId(y.Type)));
            CreateMap<DbRecordList, RecordList>()
                .ForMember(x => x.Type, opt => opt.MapFrom(y => Enum.GetName(typeof(RecordListType), y.Type).ToLower()));
        }

        private RecordListType GetListTypeId(string name)
        {
            RecordListType type = RecordListType.General;

            Enum.TryParse(name, true, out type);

            return type;
        }

        private QuerySortType GetListSortTypeId(string name)
        {
            QuerySortType type = QuerySortType.Ascending;
            Enum.TryParse(name, true, out type);
            return type;
        }
    }
}
