using AutoMapper;
using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.AutoMapper.Profiles
{
	internal class RecordPermissionsProfile : Profile
	{
		public RecordPermissionsProfile()
		{
			CreateMap<RecordPermissions, DbRecordPermissions>();
			CreateMap<DbRecordPermissions, RecordPermissions>();
		}
	}
}
