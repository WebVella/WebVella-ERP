using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using WebVella.ERP.Api.Models;

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

			MongoTransaction transaction = null;
			if (!MongoStaticContext.Context.TransactionInProgress)
				transaction = MongoStaticContext.Context.CreateTransaction( true, new MongoTransactionOptions { Isolation= MongoTransactionIsolation.ReadUncommitted});

			lock (lockObject)
			{
				try
				{
					cachedRelations = null;
					var created = MongoStaticContext.Context.EntityRelations.Create(mongoEntityRelation);
					var relation = Read(mongoEntityRelation.Name);
					var recRepo = new MongoRecordRepository();
					var entityRepo = new MongoEntityRepository();

					var originEntity = entityRepo.Read(relation.OriginEntityId);
					var targetEntity = entityRepo.Read(relation.TargetEntityId);

					recRepo.CreateRecordField(originEntity.Name, $"#{relation.Name}_targets", null);
					recRepo.CreateRecordField(targetEntity.Name, $"#{relation.Name}_origins", null);

					InvalidateRelationIndex(relation);

					if (transaction != null)
                    transaction.Commit();

					cachedRelations = null;
					return created;
				}
				catch
				{
					if (transaction != null)
					transaction.Rollback();

					throw;
				}
			}
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
				MongoTransaction transaction = null;
				if(!MongoStaticContext.Context.TransactionInProgress)
					transaction = MongoStaticContext.Context.CreateTransaction();

				try
				{
					var relation = Read(id);

					var recRepo = new MongoRecordRepository();
					var entityRepo = new MongoEntityRepository();
					var originEntity = entityRepo.Read(relation.OriginEntityId);
					var targetEntity = entityRepo.Read(relation.TargetEntityId);

					recRepo.RemoveRecordField(originEntity.Name, $"#{relation.Name}_targets");
					recRepo.RemoveRecordField(targetEntity.Name, $"#{relation.Name}_origins");

					InvalidateRelationIndex(relation, dropIndexes: true);

					var result = MongoStaticContext.Context.EntityRelations.Delete(Query.EQ("_id", id));

					if (transaction != null)
					transaction.Commit();

					cachedRelations = null;
					return result;
				}
				catch
				{
					if (transaction != null)
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
			var originEntity = MongoStaticContext.Context.Entities.GetById(relation.OriginEntityId);
			var originField = originEntity.Fields.Single(x => x.Id == relation.OriginFieldId);
			var targetEntity = MongoStaticContext.Context.Entities.GetById(relation.TargetEntityId);
			var targetField = targetEntity.Fields.Single(x => x.Id == relation.TargetFieldId);

			var originColletion = MongoStaticContext.Context.GetBsonCollection("rec_" + originEntity.Name);
			var originFieldName = originField.Name;
			if (originFieldName == "id")
				originFieldName = "_id";
            var originRecords = originColletion.Find(Query.EQ( originFieldName, originId));
			var originRecordsCount = originRecords.Count();
			BsonDocument originRecord = null;
			if (originRecordsCount == 0)
				throw new StorageException("There are no record with specified origin id.");
			else if (originRecordsCount > 1)
				throw new StorageException("There are more than 1 record with same origin id.");
			else
			{
				originRecord = originRecords.First();
				var targetsElementName = $"#{ relation.Name}_targets";
				
				BsonElement bsonElement = null;
				try{ bsonElement = originRecord.GetElement(targetsElementName); }catch{}

				if (bsonElement != null)
				{
					var targets = BsonTypeMapper.MapToDotNetValue(bsonElement.Value) as List<object>;
					if (targets == null)
					{
						targets = new List<object>();
					}
					if (!targets.Contains(targetId))
						targets.Add(targetId);

					originRecord[targetsElementName] = BsonTypeMapper.MapToBsonValue(targets);
				}
				else
				{
					var value = new List<object> { targetId };
					originRecord[targetsElementName] = BsonTypeMapper.MapToBsonValue(value);
				}
			}

			var targetColletion = MongoStaticContext.Context.GetBsonCollection("rec_" + targetEntity.Name);
			var targetFieldName = targetField.Name;
			if (targetFieldName == "id")
				targetFieldName = "_id";
			var targetRecords = targetColletion.Find(Query.EQ(targetFieldName, targetId));
			var targetRecordsCount = targetRecords.Count();
			BsonDocument targetRecord = null;
			if (targetRecordsCount == 0)
				throw new StorageException("There are no record with specified target id.");
			else if (targetRecordsCount > 1)
				throw new StorageException("There are more than 1 record with same target id.");
			else
			{
				targetRecord = targetRecords.First();
				var originsElementName = $"#{ relation.Name}_origins";

				BsonElement bsonElement = null;
				try{ bsonElement = targetRecord.GetElement(originsElementName); }catch{}

				if (bsonElement != null)
				{
					var origins = BsonTypeMapper.MapToDotNetValue(bsonElement.Value) as List<object>;
					if (origins == null)
					{
						origins = new List<object>();
					}
					if (!origins.Contains(originId))
						origins.Add(originId);

					targetRecord[originsElementName] = BsonTypeMapper.MapToBsonValue(origins);
				}
				else
				{
					var value = new List<object> { originId };
					targetRecord[originsElementName] = BsonTypeMapper.MapToBsonValue(value);
				}
			}

			MongoTransaction transaction = null;
			if (!MongoStaticContext.Context.TransactionInProgress)
				transaction = MongoStaticContext.Context.CreateTransaction();
			
			try {
			originColletion.Save(originRecord);
			targetColletion.Save(targetRecord);

				if (transaction != null)
					transaction.Commit();
			}
			catch
			{
				if (transaction != null)
					transaction.Rollback();

				throw;
		}

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
			var originEntity = MongoStaticContext.Context.Entities.GetById(relation.OriginEntityId);
			var originField = originEntity.Fields.Single(x => x.Id == relation.OriginFieldId);
			var targetEntity = MongoStaticContext.Context.Entities.GetById(relation.TargetEntityId);
			var targetField = targetEntity.Fields.Single(x => x.Id == relation.TargetFieldId);

			var originColletion = MongoStaticContext.Context.GetBsonCollection("rec_" + originEntity.Name);
			var originFieldName = originField.Name;
			if (originFieldName == "id")
				originFieldName = "_id";
			var originRecords = originColletion.Find(Query.EQ(originFieldName, originId));
			var originRecordsCount = originRecords.Count();
			BsonDocument originRecord = null;
			if (originRecordsCount == 0)
				throw new StorageException("There are no record with specified origin id.");
			else if (originRecordsCount > 1)
				throw new StorageException("There are more than 1 record with same origin id.");
			else
			{
				originRecord = originRecords.First();
				var targetsElementName = $"#{ relation.Name}_targets";
				BsonElement bsonElement = null;
				try { bsonElement = originRecord.GetElement(targetsElementName); } catch { }
				if (bsonElement != null)
				{
					var targets = BsonTypeMapper.MapToDotNetValue(bsonElement.Value) as List<object>;
					if (targets != null && targets.Contains(targetId))
					{
						targets.Remove(targetId);
						if (targets.Count == 0)
							targets = null;
						originRecord[targetsElementName] = BsonTypeMapper.MapToBsonValue(targets);
					}
				}
				else
					originRecord[targetsElementName] = BsonTypeMapper.MapToBsonValue(null);
			}

			var targetColletion = MongoStaticContext.Context.GetBsonCollection("rec_" + targetEntity.Name);
			var targetFieldName = targetField.Name;
			if (targetFieldName == "id")
				targetFieldName = "_id";
			var targetRecords = targetColletion.Find(Query.EQ(targetFieldName, targetId));
			var targetRecordsCount = targetRecords.Count();
			BsonDocument targetRecord = null;
			if (targetRecordsCount == 0)
				throw new StorageException("There are no record with specified target id.");
			else if (targetRecordsCount > 1)
				throw new StorageException("There are more than 1 record with same target id.");
			else
			{
				targetRecord = targetRecords.First();
				var originsElementName = $"#{ relation.Name}_origins";
				BsonElement bsonElement = null;
				try { bsonElement = targetRecord.GetElement(originsElementName); } catch { }
				if (bsonElement != null)
				{
					var origins = BsonTypeMapper.MapToDotNetValue(bsonElement.Value) as List<object>;
					if (origins != null && origins.Contains(originId))
					{
						origins.Remove(originId);
						if (origins.Count == 0)
							origins = null;
						targetRecord[originsElementName] = BsonTypeMapper.MapToBsonValue(origins);
					}
				}
				else
					targetRecord[originsElementName] = BsonTypeMapper.MapToBsonValue(null);
			}

			MongoTransaction transaction = null;
			if (!MongoStaticContext.Context.TransactionInProgress)
				transaction = MongoStaticContext.Context.CreateTransaction();

			try
			{
			originColletion.Save(originRecord);
			targetColletion.Save(targetRecord);

				if (transaction != null)
					transaction.Commit();
			}
			catch
			{
				if (transaction != null)
					transaction.Rollback();

				throw;
			}
		}

		private void InvalidateRelationIndex(IStorageEntityRelation entityRelation, bool dropIndexes = false )
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
					var originIndexName = "relation_" + entityRelation.Name + "_" + originField.Name;
					var originIndex = originIndexes.SingleOrDefault(x => x.Name == originIndexName);
					if (originIndex != null)
						originCollection.DropIndexByName(originIndexName);

					if (!dropIndexes)
					{
						IndexOptionsBuilder options = IndexOptions.SetUnique(false).SetDropDups(false).SetName(originIndexName).SetBackground(true);
						originCollection.CreateIndex(new IndexKeysBuilder().Ascending(originField.Name), options);
					}
				}

				var targetCollection = MongoStaticContext.Context.GetBsonCollection(MongoRecordRepository.RECORD_COLLECTION_PREFIX + targetEntity.Name);
				if (targetEntity == null)
					return;

				if (targetField.Name != "id")
				{
					var targetIndexes = targetCollection.GetIndexes();
					var targetIndexName = "relation_" + entityRelation.Name + "_" + targetField.Name;
					var targetIndex = targetIndexes.SingleOrDefault(x => x.Name == targetIndexName);
					if (targetIndex != null)
						targetCollection.DropIndexByName(targetIndexName);

					if (!dropIndexes)
					{
						IndexOptionsBuilder options = IndexOptions.SetUnique(false).SetDropDups(false).SetName(targetIndexName).SetBackground(true);
						targetCollection.CreateIndex(new IndexKeysBuilder().Ascending(targetField.Name), options);
					}
				}
			}
			else
			{
				//at the moment many to many relation do not need any endexes
				/*
				var originCollection = MongoStaticContext.Context.GetBsonCollection(MongoRecordRepository.RECORD_COLLECTION_PREFIX + originEntity.Name);
				if (originCollection == null)
					return;

				var originIndexes = originCollection.GetIndexes();
				var originIndexName = "relation_" + entityRelation.Name + "_targets";
				var originIndex = originIndexes.SingleOrDefault(x => x.Name == originIndexName);
				if (originIndex != null)
					originCollection.DropIndexByName(originIndexName);

				if (!dropIndexes)
				{
					var originFieldName = $"#" + entityRelation.Name + "_targets";
					IndexOptionsBuilder options = IndexOptions.SetUnique(false).SetDropDups(false).SetName(originIndexName).SetBackground(true);
					originCollection.CreateIndex(new IndexKeysBuilder().Ascending(originFieldName), options);
				}

				var targetCollection = MongoStaticContext.Context.GetBsonCollection(MongoRecordRepository.RECORD_COLLECTION_PREFIX + targetEntity.Name);
				if (targetEntity == null)
					return;

				var targetIndexes = targetCollection.GetIndexes();
				var targetIndexName = "relation_" + entityRelation.Name + "_origins";
				var targetIndex = targetIndexes.SingleOrDefault(x => x.Name == targetIndexName);
				if (targetIndex != null)
					targetCollection.DropIndexByName(targetIndexName);

				if (!dropIndexes)
				{
					var targetFieldName = $"#" + entityRelation.Name + "_origins";
					IndexOptionsBuilder options = IndexOptions.SetUnique(false).SetDropDups(false).SetName(targetIndexName).SetBackground(true);
					targetCollection.CreateIndex(new IndexKeysBuilder().Ascending(targetFieldName), options);
				}
				*/
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