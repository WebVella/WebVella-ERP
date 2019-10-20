#region <--- DIRECTIVES --->

using AutoMapper.Configuration;
using System;
using WebVella.Erp.Api.Models.AutoMapper.Profiles;
using WebVella.Erp.Api.Models.AutoMapper.Resolvers;

#endregion


namespace WebVella.Erp.Api.Models.AutoMapper
{
	public class ErpAutoMapperConfiguration
    {
        //Global object to store mapping exspressions
        public static MapperConfigurationExpression MappingExpressions = new MapperConfigurationExpression();

        private static object lockObj = new object();
        private static bool alreadyConfigured = false;

        public static void Configure(MapperConfigurationExpression cfg)
        {
            if (alreadyConfigured)
                return;

            lock (lockObj)
            {
                if (alreadyConfigured)
                    return;

                alreadyConfigured = true;

                cfg.CreateMap<Guid, string>().ConvertUsing<GuidToStringConverter>();
                cfg.CreateMap<DateTimeOffset, DateTime>().ConvertUsing<DateTimeTypeConverter>();
                cfg.AddProfile(new EntityRelationProfile());
                cfg.AddProfile(new EntityProfile());
                cfg.AddProfile(new RecordPermissionsProfile());
                cfg.AddProfile(new FieldPermissionsProfile());
                cfg.AddProfile(new FieldProfile());
                cfg.AddProfile(new EntityRelationOptionsProfile());
                cfg.AddProfile(new JobProfile());
                cfg.AddProfile(new UserFileProfile());
				cfg.AddProfile(new CurrencyProfile());
				cfg.AddProfile(new DataSourceProfile());
				cfg.CreateMap<EntityRecord, ErpUser>().ConvertUsing(new ErpUserConverter());
                cfg.CreateMap<ErpUser, EntityRecord>().ConvertUsing(new ErpUserConverterOposite());
                cfg.CreateMap<EntityRecord, ErpRole>().ConvertUsing(new ErpRoleConverter());
				cfg.AddProfile(new ErrorModelProfile());
				cfg.AddProfile(new SearchResultProfile());
				cfg.AddProfile(new DatabaseNNRelationRecordProfile());


			}
        }
    }
}