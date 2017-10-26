using AutoMapper;
using WebVella.ERP.Database;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
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