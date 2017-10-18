using AutoMapper;
using System;
using WebVella.ERP.Database;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
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
