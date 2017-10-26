using AutoMapper;
using WebVella.ERP.Database;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
{
	internal class EntityRelationOptionsProfile : Profile
	{
		public EntityRelationOptionsProfile()
		{
			CreateMap<EntityRelationOptionsItem, DbEntityRelationOptions>();
			CreateMap<DbEntityRelationOptions, EntityRelationOptionsItem>();
		}
	}
}
