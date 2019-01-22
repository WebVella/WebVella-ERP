//using AutoMapper;
//using WebVella.Erp.Database;
//using WebVella.Erp;
//using WebVella.Erp.Api.Models;
//using WebVella.Erp.Storage;

//namespace WebVella.Erp.Database.AutoMapper.Profiles
//{
//	internal class DbEntityRelationProfile : Profile
//    {
//		IErpService service;

//		public DbEntityRelationProfile(IErpService service)
//		{
//			this.service = service;
//		}

//		protected override void Configure()
//        {
//			Mapper.CreateMap<EntityRelation, DbEntityRelation>();
//            Mapper.CreateMap<DbEntityRelation, EntityRelation>();
//			Mapper.CreateMap<DbEntityRelation, IStorageEntityRelation>().ConstructUsing(x => CreateEmptyEntityRelationObject(x));
//			Mapper.CreateMap<IStorageEntityRelation, DbEntityRelation>();
//		}

//		protected IStorageEntityRelation CreateEmptyEntityRelationObject(DbEntityRelation relation)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyEntityRelationObject();
//		}
//	}
//}