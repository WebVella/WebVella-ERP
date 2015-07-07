using AutoMapper;
using System;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
{
	internal class RecordsListProfile : Profile
	{
		IErpService service;

		public RecordsListProfile(IErpService service)
		{
			this.service = service;
		}

		protected override void Configure()
		{
			Mapper.CreateMap<RecordListRelationFieldItem, IStorageRecordListRelationFieldItem>().ConstructUsing(x => CreateEmptyRecordListRelationFieldItemObject(x));
			Mapper.CreateMap<IStorageRecordListRelationFieldItem, RecordListRelationFieldItem>();
			Mapper.CreateMap<RecordListRelationFieldItem, InputRecordListRelationFieldItem>();
			Mapper.CreateMap<InputRecordListRelationFieldItem, RecordListRelationFieldItem>()
				.ForMember(x => x.RelationId, opt => opt.MapFrom(y => (y.RelationId.HasValue) ? y.RelationId.Value : Guid.Empty))
				.ForMember(x => x.EntityId, opt => opt.MapFrom(y => (y.EntityId.HasValue) ? y.EntityId.Value : Guid.Empty))
				.ForMember(x => x.FieldId, opt => opt.MapFrom(y => (y.FieldId.HasValue) ? y.FieldId.Value : Guid.Empty));

			Mapper.CreateMap<RecordListFieldItem, IStorageRecordListFieldItem>().ConstructUsing(x => CreateEmptyRecordListFieldItemObject(x));
			Mapper.CreateMap<IStorageRecordListFieldItem, RecordListFieldItem>();
			Mapper.CreateMap<RecordListFieldItem, InputRecordListFieldItem>();
			Mapper.CreateMap<InputRecordListFieldItem, RecordListFieldItem>()
				.ForMember(x => x.FieldId, opt => opt.MapFrom(y => (y.FieldId.HasValue) ? y.FieldId.Value : Guid.Empty));

			Mapper.CreateMap<RecordListItemBase, IStorageRecordListItemBase>().ConstructUsing(x => CreateEmptyRecordListItemBaseObject(x))
				.Include<RecordListFieldItem, IStorageRecordListFieldItem>()
				.Include<RecordListRelationFieldItem, IStorageRecordListRelationFieldItem>();
            Mapper.CreateMap<IStorageRecordListItemBase, RecordListItemBase>()
				.Include<IStorageRecordListFieldItem, RecordListFieldItem>()
				.Include<IStorageRecordListRelationFieldItem, RecordListRelationFieldItem>();
			Mapper.CreateMap<RecordListItemBase, InputRecordListItemBase>()
				.Include<RecordListFieldItem, InputRecordListFieldItem>()
				.Include<RecordListRelationFieldItem, InputRecordListRelationFieldItem>();
			Mapper.CreateMap<InputRecordListItemBase, RecordListItemBase>()
				.Include<InputRecordListFieldItem, RecordListFieldItem>()
				.Include<InputRecordListRelationFieldItem, RecordListRelationFieldItem>();

			Mapper.CreateMap<RecordListQuery, IStorageRecordListQuery>().ConstructUsing(x => CreateEmptyRecordListQueryObject(x));
			Mapper.CreateMap<IStorageRecordListQuery, RecordListQuery>();
			Mapper.CreateMap<RecordListQuery, InputRecordListQuery>();
			Mapper.CreateMap<InputRecordListQuery, RecordListQuery>();

            Mapper.CreateMap<RecordListSort, IStorageRecordListSort>().ConstructUsing(x => CreateEmptyRecordListSortObject(x))
				.ForMember(x => x.SortType, opt => opt.MapFrom(y => GetListSortTypeId(y.SortType)));
			Mapper.CreateMap<IStorageRecordListSort, RecordListSort>()
				.ForMember(x => x.SortType, opt => opt.MapFrom(y => Enum.GetName(typeof(QuerySortType), y.SortType).ToLower()));
			Mapper.CreateMap<RecordListSort, InputRecordListSort>();
			Mapper.CreateMap<InputRecordListSort, RecordListSort>();

			Mapper.CreateMap<RecordList, IStorageRecordList>().ConstructUsing(x => CreateEmptyRecordListObject(x))
				.ForMember(x => x.Type, opt => opt.MapFrom(y => GetListTypeId(y.Type)));
			Mapper.CreateMap<IStorageRecordList, RecordList>()
				.ForMember(x => x.Type, opt => opt.MapFrom(y => Enum.GetName(typeof(RecordListType), y.Type).ToLower()));
			Mapper.CreateMap<RecordList, InputRecordList>();
			Mapper.CreateMap<InputRecordList, RecordList>()
				.ForMember(x => x.Id, opt => opt.MapFrom(y => (y.Id.HasValue) ? y.Id.Value : Guid.Empty));
		}

		private RecordListType GetListTypeId(string name)
		{
			RecordListType type = RecordListType.General;

			Enum.TryParse(name, out type);

			return type;
		}

		private QuerySortType GetListSortTypeId(string name)
		{
			QuerySortType type = QuerySortType.Ascending;

			Enum.TryParse(name, out type);

			return type;
		}

		protected IStorageRecordList CreateEmptyRecordListObject(RecordList list)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordListObject();
		}

		protected IStorageRecordListSort CreateEmptyRecordListSortObject(RecordListSort item)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordListSortObject();
		}

		protected IStorageRecordListQuery CreateEmptyRecordListQueryObject(RecordListQuery item)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordListQueryObject();
		}

		protected IStorageRecordListItemBase CreateEmptyRecordListItemBaseObject(RecordListItemBase item)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordListItemBaseObject();
		}

		protected IStorageRecordListFieldItem CreateEmptyRecordListFieldItemObject(RecordListFieldItem item)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordListFieldItemObject();
		}

		protected IStorageRecordListRelationFieldItem CreateEmptyRecordListRelationFieldItemObject(RecordListRelationFieldItem item)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordListRelationFieldItemObject();
		}
	}
}
