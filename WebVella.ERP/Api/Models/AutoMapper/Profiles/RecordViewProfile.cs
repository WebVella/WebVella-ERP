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
			Mapper.CreateMap<RecordView, IStorageRecordView>().ConstructUsing(x => CreateEmptyRecordViewObject(x));
			Mapper.CreateMap<IStorageRecordView, RecordView>();
		}

		protected IStorageRecordView CreateEmptyRecordViewObject(RecordView view)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordViewObject();
		}
	}
}
