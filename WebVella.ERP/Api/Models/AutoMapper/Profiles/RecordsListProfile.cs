using AutoMapper;
using System;
using WebVella.ERP.Database;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
{
	internal class RecordsListProfile : Profile
	{
		protected override void Configure()
		{
			Mapper.CreateMap<RecordListRelationTreeItem, InputRecordListRelationTreeItem>();
			Mapper.CreateMap<InputRecordListRelationTreeItem, RecordListRelationTreeItem>()
				.ForMember(x => x.RelationId, opt => opt.MapFrom(y => (y.RelationId.HasValue) ? y.RelationId.Value : Guid.Empty))
				.ForMember(x => x.TreeId, opt => opt.MapFrom(y => (y.TreeId.HasValue) ? y.TreeId.Value : Guid.Empty));
			Mapper.CreateMap<RecordListRelationTreeItem, DbRecordListRelationTreeItem>();
			Mapper.CreateMap<DbRecordListRelationTreeItem, RecordListRelationTreeItem>();

			Mapper.CreateMap<RecordListRelationListItem, InputRecordListRelationListItem>();
			Mapper.CreateMap<InputRecordListRelationListItem, RecordListRelationListItem>()
				.ForMember(x => x.RelationId, opt => opt.MapFrom(y => (y.RelationId.HasValue) ? y.RelationId.Value : Guid.Empty))
				.ForMember(x => x.ListId, opt => opt.MapFrom(y => (y.ListId.HasValue) ? y.ListId.Value : Guid.Empty));
			Mapper.CreateMap<RecordListRelationListItem, DbRecordListRelationListItem>();
			Mapper.CreateMap<DbRecordListRelationListItem, RecordListRelationListItem>();

			Mapper.CreateMap<RecordListListItem, InputRecordListListItem>();
			Mapper.CreateMap<InputRecordListListItem, RecordListListItem>()
				.ForMember(x => x.ListId, opt => opt.MapFrom(y => (y.ListId.HasValue) ? y.ListId.Value : Guid.Empty));
			Mapper.CreateMap<RecordListListItem, DbRecordListListItem>();
			Mapper.CreateMap<DbRecordListListItem, RecordListListItem>();

			Mapper.CreateMap<RecordListRelationViewItem, InputRecordListRelationViewItem>();
			Mapper.CreateMap<InputRecordListRelationViewItem, RecordListRelationViewItem>()
				.ForMember(x => x.RelationId, opt => opt.MapFrom(y => (y.RelationId.HasValue) ? y.RelationId.Value : Guid.Empty))
				.ForMember(x => x.ViewId, opt => opt.MapFrom(y => (y.ViewId.HasValue) ? y.ViewId.Value : Guid.Empty));
			Mapper.CreateMap<RecordListRelationViewItem, DbRecordListRelationViewItem>();
			Mapper.CreateMap<DbRecordListRelationViewItem, RecordListRelationViewItem>();

			Mapper.CreateMap<RecordListViewItem, InputRecordListViewItem>();
			Mapper.CreateMap<InputRecordListViewItem, RecordListViewItem>()
				.ForMember(x => x.ViewId, opt => opt.MapFrom(y => (y.ViewId.HasValue) ? y.ViewId.Value : Guid.Empty));
			Mapper.CreateMap<RecordListViewItem, DbRecordListViewItem>();
			Mapper.CreateMap<DbRecordListViewItem, RecordListViewItem>();

			Mapper.CreateMap<RecordListRelationFieldItem, InputRecordListRelationFieldItem>();
			Mapper.CreateMap<InputRecordListRelationFieldItem, RecordListRelationFieldItem>()
				.ForMember(x => x.RelationId, opt => opt.MapFrom(y => (y.RelationId.HasValue) ? y.RelationId.Value : Guid.Empty))
				.ForMember(x => x.FieldId, opt => opt.MapFrom(y => (y.FieldId.HasValue) ? y.FieldId.Value : Guid.Empty));
			Mapper.CreateMap<RecordListRelationFieldItem, DbRecordListRelationFieldItem>();
			Mapper.CreateMap<DbRecordListRelationFieldItem, RecordListRelationFieldItem>();

			Mapper.CreateMap<RecordListFieldItem, InputRecordListFieldItem>();
			Mapper.CreateMap<InputRecordListFieldItem, RecordListFieldItem>()
				.ForMember(x => x.FieldId, opt => opt.MapFrom(y => (y.FieldId.HasValue) ? y.FieldId.Value : Guid.Empty));
			Mapper.CreateMap<RecordListFieldItem, DbRecordListFieldItem>();
			Mapper.CreateMap<DbRecordListFieldItem, RecordListFieldItem>();

			Mapper.CreateMap<RecordListItemBase, InputRecordListItemBase>()
				.Include<RecordListFieldItem, InputRecordListFieldItem>()
				.Include<RecordListRelationFieldItem, InputRecordListRelationFieldItem>()
				.Include<RecordListViewItem, InputRecordListViewItem>()
				.Include<RecordListRelationViewItem, InputRecordListRelationViewItem>()
				.Include<RecordListListItem, InputRecordListListItem>()
				.Include<RecordListRelationListItem, InputRecordListRelationListItem>()
				.Include<RecordListRelationTreeItem, InputRecordListRelationTreeItem>();
			Mapper.CreateMap<InputRecordListItemBase, RecordListItemBase>()
				.Include<InputRecordListFieldItem, RecordListFieldItem>()
				.Include<InputRecordListRelationFieldItem, RecordListRelationFieldItem>()
				.Include<InputRecordListViewItem, RecordListViewItem>()
				.Include<InputRecordListRelationViewItem, RecordListRelationViewItem>()
				.Include<InputRecordListListItem, RecordListListItem>()
				.Include<InputRecordListRelationListItem, RecordListRelationListItem>()
				.Include<InputRecordListRelationTreeItem, RecordListRelationTreeItem>()
				.ForMember(x => x.EntityId, opt => opt.MapFrom(y => (y.EntityId.HasValue) ? y.EntityId.Value : Guid.Empty));

			Mapper.CreateMap<RecordListItemBase, DbRecordListItemBase>()
				.Include<RecordListFieldItem, DbRecordListFieldItem>()
				.Include<RecordListRelationFieldItem, DbRecordListRelationFieldItem>()
				.Include<RecordListViewItem, DbRecordListViewItem>()
				.Include<RecordListRelationViewItem, DbRecordListRelationViewItem>()
				.Include<RecordListListItem, DbRecordListListItem>()
				.Include<RecordListRelationListItem, DbRecordListRelationListItem>()
				.Include<RecordListRelationTreeItem, DbRecordListRelationTreeItem>();
			Mapper.CreateMap<DbRecordListItemBase, RecordListItemBase>()
				.Include<DbRecordListFieldItem, RecordListFieldItem>()
				.Include<DbRecordListRelationFieldItem, RecordListRelationFieldItem>()
				.Include<DbRecordListViewItem, RecordListViewItem>()
				.Include<DbRecordListRelationViewItem, RecordListRelationViewItem>()
				.Include<DbRecordListListItem, RecordListListItem>()
				.Include<DbRecordListRelationListItem, RecordListRelationListItem>()
				.Include<DbRecordListRelationTreeItem, RecordListRelationTreeItem>();

			Mapper.CreateMap<RecordListQuery, InputRecordListQuery>();
			Mapper.CreateMap<InputRecordListQuery, RecordListQuery>();
			Mapper.CreateMap<RecordListQuery, DbRecordListQuery>();
			Mapper.CreateMap<DbRecordListQuery, RecordListQuery>();

			Mapper.CreateMap<RecordListSort, InputRecordListSort>();
			Mapper.CreateMap<InputRecordListSort, RecordListSort>();
			Mapper.CreateMap<RecordListSort, DbRecordListSort>()
				.ForMember(x => x.SortType, opt => opt.MapFrom(y => GetListSortTypeId(y.SortType)));
			Mapper.CreateMap<DbRecordListSort, RecordListSort>()
				.ForMember(x => x.SortType, opt => opt.MapFrom(y => Enum.GetName(typeof(QuerySortType), y.SortType).ToLower()));

			Mapper.CreateMap<RecordList, InputRecordList>();
			Mapper.CreateMap<InputRecordList, RecordList>()
				.ForMember(x => x.Id, opt => opt.MapFrom(y => (y.Id.HasValue) ? y.Id.Value : Guid.Empty));
			Mapper.CreateMap<RecordList, DbRecordList>()
				.ForMember(x => x.Type, opt => opt.MapFrom(y => GetListTypeId(y.Type)));
			Mapper.CreateMap<DbRecordList, RecordList>()
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
