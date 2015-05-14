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
        
        public static void Configure(IERPService service)
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
            }
        }
    }
}