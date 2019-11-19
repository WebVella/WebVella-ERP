using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Utilities;

namespace WebVella.Erp.Api
{
	internal class Cache
    {
		private const string KEY_ENTITIES = "entities";
		private const string KEY_ENTITIES_HASH = "entities_hash";
		private const string KEY_RELATIONS = "relations";
		private const string KEY_RELATIONS_HASH = "relations_hash";

		private static IMemoryCache cache;

		static Cache()
		{
			var cacheOptions = new MemoryCacheOptions();
			cacheOptions.ExpirationScanFrequency = TimeSpan.FromHours(1);
			cache = new MemoryCache(cacheOptions);
		}

		#region <=== General Methods ===>
		
		private static void AddObjectToCache(string key, object obj)
		{
			var options = new MemoryCacheEntryOptions();
			options.SetAbsoluteExpiration(TimeSpan.FromHours(1));
			cache.Set(key, obj, options);
		}

		internal static object GetObjectFromCache(string key)
		{
			object result = null;
			bool found = cache.TryGetValue(key, out result);
			return result;
		}

		internal static void RemoveObjectFromCache(string key)
		{
			cache.Remove(key);
		}

		#endregion

		#region <=== Entities ===>
		
		public static void AddEntities(List<Entity> entities)
		{
			AddObjectToCache(KEY_ENTITIES, entities);
			if (entities != null)
			{
				var hash = CryptoUtility.ComputeOddMD5Hash(JsonConvert.SerializeObject(entities));
				AddObjectToCache(KEY_ENTITIES_HASH, hash);
			}
			else
				RemoveObjectFromCache(KEY_ENTITIES_HASH);

		}

		public static List<Entity> GetEntities()
		{
			return GetObjectFromCache(KEY_ENTITIES) as List<Entity>;
		}

		public static string GetEntitiesHash()
		{
			return GetObjectFromCache(KEY_ENTITIES_HASH) as string;
		}

		#endregion

		#region <=== Relations ===>

		public static void AddRelations(List<EntityRelation> relations)
		{
			AddObjectToCache(KEY_RELATIONS, relations );
			if (relations != null)
			{
				var hash = CryptoUtility.ComputeOddMD5Hash(JsonConvert.SerializeObject(relations));
				AddObjectToCache(KEY_RELATIONS_HASH, hash);
			}
			else
				RemoveObjectFromCache(KEY_RELATIONS_HASH);
		}

		public static List<EntityRelation> GetRelations()
		{
			return GetObjectFromCache(KEY_RELATIONS) as List<EntityRelation>;
		}

		public static string GetRelationsHash()
		{
			return GetObjectFromCache(KEY_RELATIONS_HASH) as string;
		}

		#endregion

		public static void Clear()
		{
			lock (EntityManager.lockObj)
			{
				RemoveObjectFromCache(KEY_RELATIONS);
				RemoveObjectFromCache(KEY_ENTITIES);
				RemoveObjectFromCache(KEY_RELATIONS_HASH);
				RemoveObjectFromCache(KEY_ENTITIES_HASH);
			}
		}

		public static void ClearEntities()
		{
			lock (EntityManager.lockObj)
			{
				RemoveObjectFromCache(KEY_ENTITIES);
				RemoveObjectFromCache(KEY_ENTITIES_HASH);
			}
		}

		public static void ClearRelations()
		{
			lock (EntityManager.lockObj)
			{
				RemoveObjectFromCache(KEY_RELATIONS);
				RemoveObjectFromCache(KEY_RELATIONS_HASH);
			}
		}
		
	}
}
