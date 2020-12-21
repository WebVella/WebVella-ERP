using AutoMapper;
using System;
using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.AutoMapper.Profiles
{
	internal class EntityProfile : Profile
	{
		public EntityProfile()
		{
			CreateMap<Entity, InputEntity>();
			CreateMap<InputEntity, Entity>()
				.ForMember(x => x.Id, opt => opt.MapFrom(y => (y.Id.HasValue) ? y.Id.Value : Guid.Empty))
				.ForMember(x => x.System, opt => opt.MapFrom(y => (y.System.HasValue) ? y.System.Value : false));
			//.ForMember(x => x.Weight, opt => opt.MapFrom(y => (y.Weight.HasValue) ? y.Weight.Value : 1));

			CreateMap<Entity, DbEntity>();
				//.ForMember(x => x.Id, opt => opt.MapFrom( y => y.Id	) );
			CreateMap<DbEntity, Entity>();
		}
	}
}
