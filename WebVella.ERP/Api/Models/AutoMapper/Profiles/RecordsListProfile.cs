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
			Mapper.CreateMap<RecordList, IStorageRecordList>().ConstructUsing(x => CreateEmptyRecordListObject(x));
			Mapper.CreateMap<IStorageRecordList, RecordList>();
		}

		protected IStorageRecordList CreateEmptyRecordListObject(RecordList list)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordListObject();
		}
	}
}
