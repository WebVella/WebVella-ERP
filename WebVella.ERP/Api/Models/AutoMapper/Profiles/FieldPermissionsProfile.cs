using AutoMapper;
using System;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
{
    internal class FieldPermissionsProfile : Profile
	{
		IErpService service;

		public FieldPermissionsProfile(IErpService service)
		{
			this.service = service;
		}

		protected override void Configure()
		{
			Mapper.CreateMap<FieldPermissions, IStorageFieldPermissions>().ConstructUsing(x => CreateEmptyFieldPermissionsObject(x));
			Mapper.CreateMap<IStorageFieldPermissions, FieldPermissions>();
		}

		protected IStorageFieldPermissions CreateEmptyFieldPermissionsObject(FieldPermissions permissions)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyFieldPermissionsObject();
		}
	}
}
