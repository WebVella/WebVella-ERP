using AutoMapper;
using System;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
{
    internal class RecordsListFilterProfile : Profile
    {
		IErpService service;

		public RecordsListFilterProfile(IErpService service)
		{
			this.service = service;
		}

		protected override void Configure()
		{
			Mapper.CreateMap<RecordsListFilter, IStorageRecordsListFilter>().ConstructUsing(x => CreateEmptyRecordsListFilterObject(x));
			Mapper.CreateMap<IStorageRecordsListFilter, RecordsListFilter>();
		}

		protected IStorageRecordsListFilter CreateEmptyRecordsListFilterObject(RecordsListFilter filter)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordsListFilterObject();
		}
	}
}
