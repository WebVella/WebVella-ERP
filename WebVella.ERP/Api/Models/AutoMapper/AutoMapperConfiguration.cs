#region <--- DIRECTIVES --->

using System;
using AutoMapper;
using WebVella.ERP.Api.Models.AutoMapper.Profiles;
using WebVella.ERP.Api.Models.AutoMapper.Resolvers;

#endregion


namespace WebVella.ERP.Api.Models.AutoMapper
{
    public class AutoMapperConfiguration
    {
        private static object lockObj = new object();
        private static bool alreadyConfigured = false;
        
        public static void Configure(IErpService service)
        {
            if (alreadyConfigured)
                return;

            lock( lockObj )
            {
                if (alreadyConfigured)
                    return;

                alreadyConfigured = true;

                Mapper.CreateMap<Guid, string>().ConvertUsing<GuidToStringConverter>();
                Mapper.CreateMap<DateTimeOffset, DateTime>().ConvertUsing<DateTimeTypeConverter>();
                Mapper.AddProfile(new EntityRelationProfile(service));
				Mapper.AddProfile(new EntityProfile(service));
				Mapper.AddProfile(new RecordPermissionsProfile(service));
                Mapper.AddProfile(new FieldPermissionsProfile(service));
                Mapper.AddProfile(new FieldProfile(service));
				Mapper.AddProfile(new RecordsListProfile(service));
				Mapper.AddProfile(new RecordViewProfile(service));
				Mapper.AddProfile(new RecordTreeProfile(service));
				Mapper.AddProfile(new EntityRelationOptionsProfile(service));
                
                //Mapper.AddProfile(new RecordViewFieldProfile(service));

                Mapper.CreateMap<EntityRecord, ErpUser>().ConvertUsing(new ErpUserConverter());
                Mapper.CreateMap<ErpUser, EntityRecord>().ConvertUsing(new ErpUserConverterOposite());
                Mapper.CreateMap<EntityRecord, ErpRole>().ConvertUsing(new ErpRoleConverter());
            }
        }
    }
}