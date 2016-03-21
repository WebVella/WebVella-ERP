using AutoMapper;
using WebVella.ERP.Database;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
{
	internal class RecordPermissionsProfile : Profile
	{
		protected override void Configure()
		{
			Mapper.CreateMap<RecordPermissions, DbRecordPermissions>();
			Mapper.CreateMap<DbRecordPermissions, RecordPermissions>();
		}
	}
}
