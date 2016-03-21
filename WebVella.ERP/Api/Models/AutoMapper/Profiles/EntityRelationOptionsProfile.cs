using AutoMapper;
using WebVella.ERP.Database;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
{
	internal class EntityRelationOptionsProfile : Profile
	{
		protected override void Configure()
		{
			Mapper.CreateMap<EntityRelationOptionsItem, DbEntityRelationOptions>();
			Mapper.CreateMap<DbEntityRelationOptions, EntityRelationOptionsItem>();
		}
	}
}
