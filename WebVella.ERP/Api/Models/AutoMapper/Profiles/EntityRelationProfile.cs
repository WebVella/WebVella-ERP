using AutoMapper;
using WebVella.ERP.Database;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
{
	internal class EntityRelationProfile : Profile
	{
		protected override void Configure()
		{
			Mapper.CreateMap<EntityRelation, DbEntityRelation>();
			Mapper.CreateMap<DbEntityRelation, EntityRelation>();
		}
	}
}