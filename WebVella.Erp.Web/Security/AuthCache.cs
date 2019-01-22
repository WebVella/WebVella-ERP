//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace WebVella.Erp.Web.Security
//{
//    internal static class AuthCache
//    {
//        public const int DEFAULT_CACHE_EXPIRATION_MINUTES = 5;
//        private static readonly List<CacheItem> cache = new List<CacheItem>();

//        public static Guid? Get(Guid id)
//        {
//            lock (cache)
//            {
//                var cacheItem = cache.SingleOrDefault(i => i.Id == id);
//                if (cacheItem != null)
//                {
//                    if (cacheItem.Expire < DateTime.UtcNow)
//                        cache.Remove(cacheItem);
//                    else
//                        return cacheItem.Id;
//                }
//                return null;
//            }
//        }

//        public static void Add(Guid id)
//        {
//            lock (cache)
//            {
//                var cacheItem = cache.SingleOrDefault(i => i.Id == id);
//                if (cacheItem != null)
//                    cache.Remove(cacheItem);

//                cache.Add(new CacheItem(id, DateTime.UtcNow.AddMinutes(DEFAULT_CACHE_EXPIRATION_MINUTES)));
//            }
//        }

//        public static void Remove(Guid id)
//        {
//            lock (cache)
//            {
//                var cacheItem = cache.SingleOrDefault(i => i.Id == id);
//                if (cacheItem != null)
//                    cache.Remove(cacheItem);
//            }
//        }

//        private class CacheItem
//        {
//            public Guid Id { get; private set; }
//            public DateTime Expire { get; private set; }

//            public CacheItem(Guid id, DateTime expire)
//            {
//                Id = id;
//                Expire = expire;
//            }
//        }
//    }
//}