#region <--- DIRECTIVES --->

using WebVella.Erp.Web.Models.AutoMapper.Profiles;
using AutoMapper.Configuration;

#endregion


namespace WebVella.Erp.Web.Models.AutoMapper
{
    public class ErpWebAutoMapperConfiguration
    {
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

				cfg.AddProfile(new AppProfile());
				cfg.AddProfile(new SitemapAreaProfile());
				cfg.AddProfile(new SitemapGroupProfile());
				cfg.AddProfile(new SitemapNodeProfile());
				cfg.AddProfile(new ErpPageProfile());
				cfg.AddProfile(new PageBodyNodeProfile());
				cfg.AddProfile(new ValidationErrorProfile());
				cfg.AddProfile(new PageDataSourceProfile());
				

			}
        }
    }
}