using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Api
{
	internal class Cache
    {
		private const string KEY_ENTITIES = "entities";
		private const string KEY_RELATIONS = "relations";

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
		}

		public static List<Entity> GetEntities()
		{
			var entities = GetObjectFromCache(KEY_ENTITIES) as List<Entity>;
			//if (entities != null)
			//	entities = DeepClone<List<Entity>>(entities);

			return entities;
		}

		#endregion

		#region <=== Relations ===>

		public static void AddRelations(List<EntityRelation> relations)
		{
			AddObjectToCache(KEY_RELATIONS, relations );
		}

		public static List<EntityRelation> GetRelations()
		{
			var relations = GetObjectFromCache(KEY_RELATIONS) as List<EntityRelation>;
			if (relations != null)
				relations = DeepClone<List<EntityRelation>>(relations);

			return relations;
		}

		#endregion

		public static void Clear()
		{
			RemoveObjectFromCache(KEY_RELATIONS);
			RemoveObjectFromCache(KEY_ENTITIES);
		}

		public static void ClearEntities()
		{
			RemoveObjectFromCache(KEY_ENTITIES);
		}

		private static T DeepClone<T>(T obj)
		{
			using (var ms = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(ms, obj);
				ms.Position = 0;

				return (T)formatter.Deserialize(ms);
			}
		}
	}
}
