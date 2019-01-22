using AutoMapper;
using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.AutoMapper.Profiles
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
