using AutoMapper;
using System;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
{
    internal class EntityRelationOptionsProfile : Profile
	{
        IErpService service;

        public EntityRelationOptionsProfile(IErpService service)
        {
            this.service = service;
        }

        protected override void Configure()
		{
			Mapper.CreateMap<EntityRelationOptionsItem, IStorageEntityRelationOptions>().ConstructUsing(x => CreateEmptyEntityRelationOptionsObject(x)); ;
			Mapper.CreateMap<IStorageEntityRelationOptions, EntityRelationOptionsItem>();
		}

        protected IStorageEntityRelationOptions CreateEmptyEntityRelationOptionsObject(EntityRelationOptionsItem item)
        {
            return service.StorageService.GetObjectFactory().CreateEmptyEntityRelationOptionsObject(); 
        }
    }
}
