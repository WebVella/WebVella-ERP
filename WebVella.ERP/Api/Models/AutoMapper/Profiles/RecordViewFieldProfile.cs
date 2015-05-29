using AutoMapper;
using System;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
{
	internal class RecordViewFieldProfile : Profile
    {
		IErpService service;

		public RecordViewFieldProfile(IErpService service)
		{
			this.service = service;
		}

		protected override void Configure()
		{
			Mapper.CreateMap<RecordViewField, IStorageRecordViewField>().ConstructUsing(x => CreateEmptyRecordViewFieldObject(x));
			Mapper.CreateMap<IStorageRecordViewField, RecordViewField>();
		}

		protected IStorageRecordViewField CreateEmptyRecordViewFieldObject(RecordViewField field)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordViewFieldObject();
		}
	}
}
