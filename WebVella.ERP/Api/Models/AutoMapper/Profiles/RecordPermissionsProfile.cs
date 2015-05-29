using AutoMapper;
using System;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
{
    internal class RecordPermissionsProfile : Profile
	{
		IErpService service;

		public RecordPermissionsProfile(IErpService service)
		{
			this.service = service;
		}

		protected override void Configure()
		{
			Mapper.CreateMap<RecordPermissions, IStorageRecordPermissions>().ConstructUsing(x => CreateEmptyRecordPermissionsObject(x));
			Mapper.CreateMap<IStorageRecordPermissions, RecordPermissions>();
		}

		protected IStorageRecordPermissions CreateEmptyRecordPermissionsObject(RecordPermissions recordPermissions)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyRecordPermissionsObject();
		}
	}
}
