using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace WebVella.Erp.Web.Utils
{
	public class Cache
	{
		private IMemoryCache cache;
		private MemoryCacheOptions defaultOptions;
		private MemoryCacheEntryOptions defaultCacheEntryOptions;

		public Cache()
		{
			MemoryCacheEntryOptions entryOptions = new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove);
			defaultCacheEntryOptions = entryOptions;

			MemoryCacheOptions options = new MemoryCacheOptions();
			defaultOptions = options;
			cache = new MemoryCache(options);
		}

		public T Get<T>(string key) where T : class
		{
			T obj = null;
			bool found = cache.TryGetValue<T>(key, out obj);
			return obj;
		}

		public void Put<T>(string key, T obj, MemoryCacheEntryOptions options = null, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null) where T : class
		{
			if (options == null)
				options = defaultCacheEntryOptions;

			if (absoluteExpiration != null)
				options.AbsoluteExpirationRelativeToNow = absoluteExpiration;
			if (slidingExpiration != null)
				options.SlidingExpiration = slidingExpiration;

			cache.Set(key, obj, options);
		}

		public void Remove(string key)
		{
			cache.Remove(key);
		}

		private void Clear()
		{
			cache = new MemoryCache(defaultOptions);
		}
	}
}
