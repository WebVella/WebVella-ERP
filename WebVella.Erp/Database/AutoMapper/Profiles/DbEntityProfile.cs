//using AutoMapper;
//using WebVella.Erp.Database;
//using WebVella.Erp;
//using WebVella.Erp.Api.Models;
//using WebVella.Erp.Storage;

//namespace WebVella.Erp.Database.AutoMapper.Profiles
//{
//	internal class DbEntityProfile : Profile
//	{
//		IErpService service;

//		public DbEntityProfile(IErpService service)
//		{
//			this.service = service;
//		}

//		protected override void Configure()
//		{
//			Mapper.CreateMap<Entity, DbEntity>();
//			Mapper.CreateMap<DbEntity, Entity>();
//			Mapper.CreateMap<DbEntity, IStorageEntity>().ConstructUsing(x => CreateEmptyEntityObject(x));
//			Mapper.CreateMap<IStorageEntity, DbEntity>();
//		}

//		protected IStorageEntity CreateEmptyEntityObject(DbEntity entity)
//		{
//			var storageService = service.StorageService;
//			return storageService.GetObjectFactory().CreateEmptyEntityObject();
//		}
//	}
//}