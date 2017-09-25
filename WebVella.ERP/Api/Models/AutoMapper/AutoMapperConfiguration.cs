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
		
		public static void Configure()
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
				Mapper.AddProfile(new EntityRelationProfile());
				Mapper.AddProfile(new EntityProfile());
				Mapper.AddProfile(new RecordPermissionsProfile());
				Mapper.AddProfile(new FieldPermissionsProfile());
				Mapper.AddProfile(new FieldProfile());
				Mapper.AddProfile(new RecordsListProfile());
				Mapper.AddProfile(new RecordViewProfile());
				Mapper.AddProfile(new RecordTreeProfile());
				Mapper.AddProfile(new EntityRelationOptionsProfile());
				Mapper.AddProfile(new JobProfile());
				Mapper.AddProfile(new UserFileProfile());
				//Mapper.AddProfile(new RecordViewFieldProfile(service));

				Mapper.CreateMap<EntityRecord, ErpUser>().ConvertUsing(new ErpUserConverter());
				Mapper.CreateMap<ErpUser, EntityRecord>().ConvertUsing(new ErpUserConverterOposite());
				Mapper.CreateMap<EntityRecord, ErpRole>().ConvertUsing(new ErpRoleConverter());
			}
		}
	}
}