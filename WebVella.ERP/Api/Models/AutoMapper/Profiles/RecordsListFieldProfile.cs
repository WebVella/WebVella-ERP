using AutoMapper;
using System;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
{
    internal class RecordsListFieldProfile : Profile
    {
		IErpService service;

		public RecordsListFieldProfile(IErpService service)
		{
			this.service = service;
		}

		protected override void Configure()
		{
			Mapper.CreateMap<RecordsListField, IStorageRecordsListField>().ConstructUsing(x => CreateEmptyRecordsListFieldObject(x));
			Mapper.CreateMap<IStorageRecordsListField, RecordsListField>();
		}

		protected IStorageRecordsListField CreateEmptyRecordsListFieldObject(RecordsListField field)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordsListFieldObject();
		}
	}
}
