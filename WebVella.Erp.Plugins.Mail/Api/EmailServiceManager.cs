using Microsoft.Extensions.Caching.Memory;
using System;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.AutoMapper;
using WebVella.Erp.Eql;

namespace WebVella.Erp.Plugins.Mail.Api
{
	public class EmailServiceManager
	{
		static EmailServiceManager()
		{
			InitCache();
		}

		#region <=== General Cache Methods ===>

		private static IMemoryCache cache; 

		private static void InitCache()
		{
			if (cache != null)
				cache.Dispose();

			var cacheOptions = new MemoryCacheOptions();
			cacheOptions.ExpirationScanFrequency = TimeSpan.FromHours(1);
			cache = new MemoryCache(cacheOptions);
		}

		internal static void ClearCache()
		{
			InitCache();
		}

		private static void AddObjectToCache(string key, object obj)
		{
			var options = new MemoryCacheEntryOptions();
			options.SetAbsoluteExpiration(TimeSpan.FromHours(1));
			cache.Set(key, obj, options);
		}

		private static object GetObjectFromCache(string key)
		{
			object result = null;
			bool found = cache.TryGetValue(key, out result);
			return result;
		}

		#endregion

		#region <=== SMTP Services ===>
		
		public SmtpService GetSmtpService(Guid id)
		{
			string cacheKey = $"SMTP-{id}";
			SmtpService service = GetObjectFromCache(cacheKey) as SmtpService;
			if (service == null)
			{
				service = GetSmtpServiceInternal(id);
				if (service != null)
					AddObjectToCache(cacheKey, service);
			}
			return service;
		}

		public SmtpService GetSmtpService(string name = null)
		{
			string cacheKey = $"SMTP-{name}";
			SmtpService service = GetObjectFromCache(cacheKey) as SmtpService;
			if (service == null)
			{
				service = GetSmtpServiceInternal(name);
				if (service != null)
					AddObjectToCache(cacheKey, service);
			}
			return service;
		}

		internal SmtpService GetSmtpServiceInternal(string name = null)
		{
			EntityRecord smtpServiceRec = null;
			if (name != null)
			{
				var result = new EqlCommand("SELECT * FROM smtp_service WHERE name = @name", new EqlParameter("name", name)).Execute();
				if (result.Count == 0)
					throw new Exception($"SmtpService with name '{name}' not found.");

				smtpServiceRec = result[0];
			}
			else
			{
				var result = new EqlCommand("SELECT * FROM smtp_service WHERE is_default = @is_default", new EqlParameter("is_default", true)).Execute();
				if (result.Count == 0)
					throw new Exception($"Default SmtpService not found.");
				else if (result.Count > 1)
					throw new Exception($"More than one default SmtpService not found.");

				smtpServiceRec = result[0];
			}
			return smtpServiceRec.MapTo<SmtpService>();
		}

		internal SmtpService GetSmtpServiceInternal(Guid id)
		{
			var result = new EqlCommand("SELECT * FROM smtp_service WHERE id = @id", new EqlParameter("id", id)).Execute();
			if (result.Count == 0)
				throw new Exception($"SmtpService with id = '{id}' not found.");

			return result[0].MapTo<SmtpService>();
		}

		#endregion
	}
}
