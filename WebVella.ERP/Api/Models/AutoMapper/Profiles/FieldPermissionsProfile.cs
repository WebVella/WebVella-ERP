using AutoMapper;
using System;
using WebVella.ERP.Database;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
{
    internal class FieldPermissionsProfile : Profile
	{
		protected override void Configure()
		{
			Mapper.CreateMap<FieldPermissions, DbFieldPermissions>();
			Mapper.CreateMap<DbFieldPermissions, FieldPermissions>();
		}
	}
}
