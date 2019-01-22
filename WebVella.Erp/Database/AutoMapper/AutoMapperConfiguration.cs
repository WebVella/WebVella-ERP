//#region <--- DIRECTIVES --->

//using System;
//using AutoMapper;
//using WebVella.Erp.Api.Models.AutoMapper.Resolvers;
//using WebVella.Erp.Database.AutoMapper.Profiles;
//using WebVella.Erp;

//#endregion


//namespace WebVella.Erp.Database.AutoMapper
//{
//    public class AutoMapperConfiguration
//    {
//        private static object lockObj = new object();
//        private static bool alreadyConfigured = false;
        
//        public static void Configure(IErpService service)
//        {
//            if (alreadyConfigured)
//                return;

//            lock( lockObj )
//            {
//                if (alreadyConfigured)
//                    return;

//                alreadyConfigured = true;

//    //            Mapper.CreateMap<Guid, string>().ConvertUsing<GuidToStringConverter>();
//    //            Mapper.CreateMap<DateTimeOffset, DateTime>().ConvertUsing<DateTimeTypeConverter>();
//    //            Mapper.AddProfile(new DbEntityRelationProfile(service));
//				//Mapper.AddProfile(new DbEntityProfile(service));
//				//Mapper.AddProfile(new DbRecordPermissionsProfile(service));
//    //            Mapper.AddProfile(new DbFieldPermissionsProfile(service));
//    //            Mapper.AddProfile(new DbFieldProfile(service));
//				//Mapper.AddProfile(new DbRecordsListProfile(service));
//				//Mapper.AddProfile(new DbRecordViewProfile(service));
//				//Mapper.AddProfile(new DbRecordTreeProfile(service));
//				//Mapper.AddProfile(new DbEntityRelationOptionsProfile(service));
//            }
//        }
//    }
//}