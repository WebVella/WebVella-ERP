using AutoMapper;
using WebVella.ERP.Storage;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
{
    internal class EntityRelationProfile : Profile
    {
        IErpService service;

        public EntityRelationProfile(IErpService service)
        {
            this.service = service;
        }

        protected override void Configure()
        {
            Mapper.CreateMap<EntityRelation, IStorageEntityRelation>().ConstructUsing(x => CreateEmptyEntityRelationObject(x));
            Mapper.CreateMap<IStorageEntityRelation, EntityRelation>();
        }

        protected IStorageEntityRelation CreateEmptyEntityRelationObject(EntityRelation relation)
        {
            var storageService = service.StorageService;
            return storageService.GetObjectFactory().CreateEmptyEntityRelationObject();
        }
    }
}