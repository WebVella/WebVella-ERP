using AutoMapper;
using System;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
{
	internal class EntityProfile : Profile
	{
		IErpService service;

		public EntityProfile(IErpService service)
		{
			this.service = service;
		}

		protected override void Configure()
		{
			Mapper.CreateMap<Entity, IStorageEntity>().ConstructUsing(x => CreateEmptyEntityObject(x));
			Mapper.CreateMap<IStorageEntity, Entity>();

			Mapper.CreateMap<Entity, InputEntity>();
			Mapper.CreateMap<InputEntity, Entity>()
				.ForMember(x => x.Id, opt => opt.MapFrom(y => (y.Id.HasValue) ? y.Id.Value : Guid.Empty))
				.ForMember(x => x.System, opt => opt.MapFrom(y => (y.System.HasValue) ? y.System.Value : false))
				.ForMember(x => x.Weight, opt => opt.MapFrom(y => (y.Weight.HasValue) ? y.Weight.Value : 1));
		}

		protected IStorageEntity CreateEmptyEntityObject(Entity entity)
		{
			var storageService = service.StorageService;
			return storageService.GetObjectFactory().CreateEmptyEntityObject();
		}
	}

	//public class DateTimeTypeConverter : ITypeConverter<string, DateTime>
	//{
	//	public DateTime Convert(ResolutionContext context)
	//	{
	//		return System.Convert.ToDateTime(context.SourceValue);
	//	}
	//}
}
