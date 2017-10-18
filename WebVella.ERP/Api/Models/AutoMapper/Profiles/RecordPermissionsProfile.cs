using AutoMapper;
using WebVella.ERP.Database;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
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
