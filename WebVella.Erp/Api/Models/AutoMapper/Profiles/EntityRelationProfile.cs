using AutoMapper;
using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.AutoMapper.Profiles
{
	internal class EntityRelationProfile : Profile
	{
		public EntityRelationProfile()
		{
			CreateMap<EntityRelation, DbEntityRelation>();
			CreateMap<DbEntityRelation, EntityRelation>();
		}
	}
}