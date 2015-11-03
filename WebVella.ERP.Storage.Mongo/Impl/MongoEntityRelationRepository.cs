using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Builders;
using MongoDB.Bson;

namespace WebVella.ERP.Storage.Mongo
{
	public class MongoEntityRelationRepository : IStorageEntityRelationRepository
	{
		private static object lockObject = new object();
		private static List<IStorageEntityRelation> cachedRelations = null;
		private const string RELATION_COLLECTION_PREFIX = "rel_";

		/// <summary>
		/// Reads relations records
		/// </summary>
		/// <returns></returns>
		public List<IStorageEntityRelation> Read()
		{
			lock (lockObject)
			{
				if (cachedRelations == null)
					cachedRelations = MongoStaticContext.Context.EntityRelations.Get().ToList<IStorageEntityRelation>();

				return cachedRelations;
			}
		}

		/// <summary>
		/// Read single relation record specified by its id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IStorageEntityRelation Read(Guid id)
		{
			lock (lockObject)
			{
				if (cachedRelations == null)
					cachedRelations = Read();

				return cachedRelations.SingleOrDefault(x => x.Id == id);
			}
		}

		/// <summary>
		/// Read single relation record specified by its name
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IStorageEntityRelation Read(string name)
		{
			lock (lockObject)
			{
				if (cachedRelations == null)
					cachedRelations = Read();

				return cachedRelations.SingleOrDefault(x => x.Name.ToLowerInvariant() == name.ToLowerInvariant());
			}
		}

		/// <summary>
		/// Creates entity relation
		/// </summary>
		/// <param name="entity"></param>
		public bool Create(IStorageEntityRelation entityRelation)
		{
			if (entityRelation == null)
				throw new ArgumentNullException("entityRelation");

			var mongoEntityRelation = entityRelation as MongoEntityRelation;

			if (mongoEntityRelation == null)
				throw new Exception("The specified entityRelation is not mongo storage object.");


			bool result = false;
			lock (lockObject)
			{
				cachedRelations = null;
				result = MongoStaticContext.Context.EntityRelations.Create(mongoEntityRelation);
			}

			InvalidateRelationIndex(entityRelation);
			return result;
		}

		/// <summary>
		/// Updates entity relation
		/// </summary>
		/// <param name="entity"></param>
		public bool Update(IStorageEntityRelation entityRelation)
		{
			if (entityRelation == null)
				throw new ArgumentNullException("entityRelation");

			var mongoEntityRelation = entityRelation as MongoEntityRelation;

			if (mongoEntityRelation == null)
				throw new StorageException("The specified entityRelation is not mongo storage object.");

			bool result = false;
			lock (lockObject)
			{
				cachedRelations = null;
				result = MongoStaticContext.Context.EntityRelations.Update(mongoEntityRelation);
			}

			InvalidateRelationIndex(entityRelation);
			return result;

		}

		/// <summary>
		/// Deletes entity relation 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool Delete(Guid id)
		{
			lock (lockObject)
			{
				var transaction = MongoStaticContext.Context.CreateTransaction();
				try
				{

					//remove system collection for many to many collection
					var relation = Read(id);
					string relationCollectionName = RELATION_COLLECTION_PREFIX + relation.Name;
					if (MongoStaticContext.Context.Database.CollectionExists(relationCollectionName))
						MongoStaticContext.Context.Database.DropCollection(relationCollectionName);

					var result = MongoStaticContext.Context.EntityRelations.Delete(Query.EQ("_id", id));
					transaction.Commit();
					cachedRelations = null;
					return result;
				}
				catch
				{
					transaction.Rollback();
					throw;
				}
			}
		}

		/// <summary>
		/// Saves entity relation
		/// </summary>
		/// <param name="entity"></param>
		public bool Save(IStorageEntityRelation entityRelation)
		{

			if (entityRelation == null)
				throw new ArgumentNullException("entityRelation");

			var mongoEntityRelation = entityRelation as MongoEntityRelation;

			if (mongoEntityRelation == null)
				throw new StorageException("The specified entityRelation is not mongo storage object.");

			var result = false;
			lock (lockObject)
			{
				cachedRelations = null;
				result = MongoStaticContext.Context.EntityRelations.Save(mongoEntityRelation);
			}

			InvalidateRelationIndex(entityRelation);

			return result;
		}

		/// <summary>
		/// Created many to many relation record
		/// </summary>
		/// <param name="relationName"></param>
		/// <param name="originId"></param>
		/// <param name="targetId"></param>
		public void CreateManyToManyRecord(Guid relationId, Guid originId, Guid targetId)
		{
			var relation = Read(relationId);
			string relationCollectionName = RELATION_COLLECTION_PREFIX + relation.Name;
			var mongoCollection = MongoStaticContext.Context.GetBsonCollection(relationCollectionName);

			var query = Query.And(Query.EQ("relationId", relationId), Query.EQ("targetId", targetId), Query.EQ("originId", originId));
			bool recordExists = mongoCollection.Find(query).Count() > 0;

			if (recordExists)
				throw new StorageException("A record with same arguments already exists.");

			BsonDocument doc = new BsonDocument();
			doc["relationId"] = BsonValue.Create(relationId);
			doc["originId"] = BsonValue.Create(originId);
			doc["targetId"] = BsonValue.Create(targetId);

			mongoCollection.Insert(doc);
		}

		/// <summary>
		/// Deletes many to many relation record
		/// </summary>
		/// <param name="relationId"></param>
		/// <param name="originId"></param>
		/// <param name="targetId"></param>
		public void DeleteManyToManyRecord(Guid relationId, Guid originId, Guid targetId)
		{
			var relation = Read(relationId);
			string relationCollectionName = RELATION_COLLECTION_PREFIX + relation.Name;
			var mongoCollection = MongoStaticContext.Context.GetBsonCollection(relationCollectionName);
			var query = Query.And(Query.EQ("relationId", relationId), Query.EQ("targetId", targetId), Query.EQ("originId", originId));
			mongoCollection.Remove(query);
		}

		/// <summary>
		/// Reads list of ids asociated to target entity, filtered by relation and origin id
		/// </summary>
		/// <param name="relationId"></param>
		/// <param name="originId"></param>
		/// <returns></returns>
		public List<Guid> ReadManyToManyRecordByOrigin(Guid relationId, Guid originId)
		{
			var relation = Read(relationId);
			string relationCollectionName = RELATION_COLLECTION_PREFIX + relation.Name;
			var mongoCollection = MongoStaticContext.Context.GetBsonCollection(relationCollectionName);

			var query = Query.And(Query.EQ("relationId", relationId), Query.EQ("originId", originId));
			var records = mongoCollection.Find(query);
			return records.Select(x => (Guid)x["targetId"]).ToList();
		}

		/// <summary>
		/// Reads list of ids asociated to origin entity, filtered by relation and target id
		/// </summary>
		/// <param name="relationId"></param>
		/// <param name="originId"></param>
		/// <returns></returns>
		public List<Guid> ReadManyToManyRecordByTarget(Guid relationId, Guid targetId)
		{
			var relation = Read(relationId);
			string relationCollectionName = RELATION_COLLECTION_PREFIX + relation.Name;
			var mongoCollection = MongoStaticContext.Context.GetBsonCollection(relationCollectionName);

			var query = Query.And(Query.EQ("relationId", relationId), Query.EQ("targetId", targetId));
			var records = mongoCollection.Find(query);
			return records.Select(x => (Guid)x["originId"]).ToList();
		}


		private void InvalidateRelationIndex(IStorageEntityRelation entityRelation)
		{
			MongoEntity originEntity = MongoStaticContext.Context.Entities.GetById(entityRelation.OriginEntityId);
			MongoEntity targetEntity = MongoStaticContext.Context.Entities.GetById(entityRelation.TargetEntityId);

			if (originEntity == null || targetEntity == null)
				return;

			IStorageField originField = originEntity.Fields.SingleOrDefault(x => x.Id == entityRelation.OriginFieldId);
			IStorageField targetField = targetEntity.Fields.SingleOrDefault(x => x.Id == entityRelation.TargetFieldId);

			if (originField == null || targetField == null)
				return;

			if (entityRelation.RelationType != Api.Models.EntityRelationType.ManyToMany)
			{
				var originCollection = MongoStaticContext.Context.GetBsonCollection(MongoRecordRepository.RECORD_COLLECTION_PREFIX + originEntity.Name);
				if (originCollection == null)
					return;

				if (originField.Name != "id")
				{
					var originIndexes = originCollection.GetIndexes();
					var originIndexName = "relation_" + entityRelation.Id + "_" + originField.Name;
					var originIndex = originIndexes.SingleOrDefault(x => x.Name == originIndexName);
					if (originIndex != null)
						originCollection.DropIndexByName(originIndexName);

					IndexOptionsBuilder options = IndexOptions.SetUnique(false).SetDropDups(false).SetName(originIndexName).SetBackground(true);
					originCollection.CreateIndex(new IndexKeysBuilder().Ascending(originField.Name), options);
				}

				var targetCollection = MongoStaticContext.Context.GetBsonCollection(MongoRecordRepository.RECORD_COLLECTION_PREFIX + targetEntity.Name);
				if (targetEntity == null)
					return;

				if (targetField.Name != "id")
				{
					var targetIndexes = targetCollection.GetIndexes();
					var targetIndexName = "relation_" + entityRelation.Id + "_" + targetField.Name;
					var targetIndex = targetIndexes.SingleOrDefault(x => x.Name == targetIndexName);
					if (targetIndex != null)
						targetCollection.DropIndexByName(targetIndexName);

					IndexOptionsBuilder options = IndexOptions.SetUnique(false).SetDropDups(false).SetName(targetIndexName).SetBackground(true);
					targetCollection.CreateIndex(new IndexKeysBuilder().Ascending(targetField.Name), options);
				}
			}
			else
			{
				var collection = MongoStaticContext.Context.GetBsonCollection(RELATION_COLLECTION_PREFIX + entityRelation.Name);

				var indexes = collection.GetIndexes();
				var originIndexName = "relation_origin";
				var originIndex = indexes.SingleOrDefault(x => x.Name == originIndexName);
				if (originIndex != null)
					collection.DropIndexByName(originIndexName);

				IndexOptionsBuilder options = IndexOptions.SetUnique(false).SetDropDups(false).SetName(originIndexName).SetBackground(true);
				collection.CreateIndex(new IndexKeysBuilder().Ascending("relationId").Ascending("originId"), options);

				var targetIndexName = "relation_target";
				var targetIndex = indexes.SingleOrDefault(x => x.Name == targetIndexName);
				if (targetIndex != null)
					collection.DropIndexByName(targetIndexName);

				options = IndexOptions.SetUnique(false).SetDropDups(false).SetName(targetIndexName).SetBackground(true);
				collection.CreateIndex(new IndexKeysBuilder().Ascending("relationId").Ascending("targetId"), options);


			}

		}

		public void ConvertNtoNRelations()
		{
			List<IStorageEntityRelation> relations = Read();


			foreach (var relation in relations)
			{
				if (relation.RelationType != Api.Models.EntityRelationType.ManyToMany)
					continue;

				MongoEntity originEntity = MongoStaticContext.Context.Entities.GetById(relation.OriginEntityId);
				MongoEntity targetEntity = MongoStaticContext.Context.Entities.GetById(relation.TargetEntityId);
				IStorageField originField = originEntity.Fields.SingleOrDefault(x => x.Id == relation.OriginFieldId);
				IStorageField targetField = targetEntity.Fields.SingleOrDefault(x => x.Id == relation.TargetFieldId);

				string originRelationFieldName = $"#{relation.Name}_origins";
				string targetRelationFieldName = $"#{relation.Name}_targets";

				var relationEntityRecordCollections = MongoStaticContext.Context.GetBsonCollection(RELATION_COLLECTION_PREFIX + relation.Name);
				var relationEntityRecords = relationEntityRecordCollections.FindAll().ToList();

				if (relationEntityRecords != null && relationEntityRecords.Count() > 0)
				{
					var transaction = MongoStaticContext.Context.CreateTransaction();

					try
					{
						var originEntityRecordCollection = MongoStaticContext.Context.GetBsonCollection(MongoRecordRepository.RECORD_COLLECTION_PREFIX + originEntity.Name);
						var originEntityRecords = originEntityRecordCollection.FindAll().ToList();

						if (originEntityRecords != null && originEntityRecords.Count() > 0)
						{
							foreach (var originRecord in originEntityRecords)
							{
								string originFieldName = originField.Name == "id" ? $"_id" : originField.Name;
								List<Guid> targetRecordsToCopy = relationEntityRecords.Where(r => (Guid)r["originId"] == (Guid)originRecord[originFieldName]).Select(r => (Guid)r["targetId"]).ToList();

								originRecord[targetRelationFieldName] = targetRecordsToCopy == null || targetRecordsToCopy.Count() == 0 ? BsonNull.Value : BsonValue.Create(targetRecordsToCopy);
								var updateSuccess = originEntityRecordCollection.Save(originRecord).DocumentsAffected > 0;
								//if (!updateSuccess)
								//	throw new StorageException("Failed to update record.");
							}
						}

						var targetEntityRecordCollection = MongoStaticContext.Context.GetBsonCollection(MongoRecordRepository.RECORD_COLLECTION_PREFIX + targetEntity.Name);
						var targetEntityRecords = targetEntityRecordCollection.FindAll().ToList();

						if (targetEntityRecords != null && targetEntityRecords.Count() > 0)
						{
							foreach (var targetRecord in targetEntityRecords)
							{
								string targetFieldName = targetField.Name == "id" ? $"_id" : targetField.Name;
								List<Guid> originRecordsToCopy = relationEntityRecords.Where(r => (Guid)r["targetId"] == (Guid)targetRecord[targetFieldName]).Select(r => (Guid)r["originId"]).ToList();

								targetRecord[originRelationFieldName] = originRecordsToCopy == null || originRecordsToCopy.Count() == 0 ? BsonNull.Value : BsonValue.Create(originRecordsToCopy); ;
								var updateSuccess = targetEntityRecordCollection.Save(targetRecord).DocumentsAffected > 0;
								//if (!updateSuccess)
								//	throw new StorageException("Failed to update record.");
							}
						}

						transaction.Commit();

						IndexOptionsBuilder originOptions = IndexOptions.SetUnique(false).SetDropDups(false).SetName(targetRelationFieldName).SetBackground(true);
						originEntityRecordCollection.CreateIndex(new IndexKeysBuilder().Ascending(targetRelationFieldName), originOptions);

						IndexOptionsBuilder targetOptions = IndexOptions.SetUnique(false).SetDropDups(false).SetName(originRelationFieldName).SetBackground(true);
						targetEntityRecordCollection.CreateIndex(new IndexKeysBuilder().Ascending(originRelationFieldName), targetOptions);
					}
					catch (Exception)
					{
						transaction.Rollback();
					}
				}
			}
		}
	}
}