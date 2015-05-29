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
			Mapper.CreateMap<RecordsList, IStorageRecordsList>().ConstructUsing(x => CreateEmptyRecordsListObject(x));
			Mapper.CreateMap<IStorageRecordsList, RecordsList>();
		}

		protected IStorageRecordsList CreateEmptyRecordsListObject(RecordsList list)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordsListObject();
		}
	}
}
