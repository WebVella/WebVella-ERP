using AutoMapper;
using System;
using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.AutoMapper.Profiles
{
    internal class FieldPermissionsProfile : Profile
	{
		public FieldPermissionsProfile()
		{
			CreateMap<FieldPermissions, DbFieldPermissions>();
			CreateMap<DbFieldPermissions, FieldPermissions>();
		}
	}
}
