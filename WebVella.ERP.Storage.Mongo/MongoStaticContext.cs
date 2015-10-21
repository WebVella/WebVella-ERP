#region <--- DIRECTIVES --->

using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

#endregion

namespace WebVella.ERP.Storage.Mongo
{
	internal class MongoStaticContext
	{
		private static readonly MongoStaticContext context = new MongoStaticContext();
		public static MongoStaticContext Context { get { return context; } }

		private List<object> repositories;
		public MongoServer Server { get; set; }
		public MongoDatabase Database { get; set; }

		internal IMongoRepository<MongoEntity> Entities { get; private set; }
        internal IMongoRepository<MongoSystemSettings> SystemSettings { get; private set; }
        internal IMongoRepository<MongoEntityRelation> EntityRelations { get; private set; }

        /// <summary>
        ///     Initializes the <see cref="MongoStaticContext" /> class.
        /// </summary>
        private MongoStaticContext()
		{
			repositories = new List<object>();

			if (string.IsNullOrWhiteSpace(Settings.ConnectionString))
				throw new Exception("The connection string to storage (mongo database) is not specified in config.json");

			MongoUrl mongoUrl = new MongoUrl(Settings.ConnectionString);
			Server = new MongoClient(mongoUrl).GetServer();
			Database = Server.GetDatabase(mongoUrl.DatabaseName);

			Entities = RegisterRepository<MongoEntity>("entities");
            SystemSettings = RegisterRepository<MongoSystemSettings>("system_settings");
            EntityRelations = RegisterRepository<MongoEntityRelation>("entity_relations");

            //register all mongo storage classes
            BsonClassMap.RegisterClassMap<MongoSystemSettings>();
            BsonClassMap.RegisterClassMap<MongoEntity>();
            BsonClassMap.RegisterClassMap<MongoEntityRelation>();
            BsonClassMap.RegisterClassMap<MongoEntityRelationOptions>();

            BsonClassMap.RegisterClassMap<MongoRecordList>();
            BsonClassMap.RegisterClassMap<MongoRecordListQuery>();
            BsonClassMap.RegisterClassMap<MongoRecordListSort>();
			BsonClassMap.RegisterClassMap<MongoRecordListItemBase>();
			BsonClassMap.RegisterClassMap<MongoRecordListFieldItem>();
			BsonClassMap.RegisterClassMap<MongoRecordListRelationFieldItem>();
			BsonClassMap.RegisterClassMap<MongoRecordListListItem>();
			BsonClassMap.RegisterClassMap<MongoRecordListViewItem>();
			BsonClassMap.RegisterClassMap<MongoRecordListRelationListItem>();
			BsonClassMap.RegisterClassMap<MongoRecordListRelationViewItem>();

			BsonClassMap.RegisterClassMap<MongoRecordTree>();
			BsonClassMap.RegisterClassMap<MongoRecordPermissions>();
            BsonClassMap.RegisterClassMap<MongoFieldPermissions>();

            BsonClassMap.RegisterClassMap<MongoCurrencyType>();

            BsonClassMap.RegisterClassMap<MongoRecordView>();
            BsonClassMap.RegisterClassMap<MongoRecordViewSidebar>();
            BsonClassMap.RegisterClassMap<MongoRecordViewSidebarItemBase>();
			BsonClassMap.RegisterClassMap<MongoRecordViewSidebarListItem>();
			BsonClassMap.RegisterClassMap<MongoRecordViewSidebarRelationListItem>();
			BsonClassMap.RegisterClassMap<MongoRecordViewSidebarViewItem>();
			BsonClassMap.RegisterClassMap<MongoRecordViewSidebarRelationViewItem>();
			BsonClassMap.RegisterClassMap<MongoRecordViewRegion>();
            BsonClassMap.RegisterClassMap<MongoRecordViewSection>();
            BsonClassMap.RegisterClassMap<MongoRecordViewRow>();
            BsonClassMap.RegisterClassMap<MongoRecordViewColumn>();
            BsonClassMap.RegisterClassMap<MongoRecordViewItemBase>();
            BsonClassMap.RegisterClassMap<MongoRecordViewFieldItem>();
            BsonClassMap.RegisterClassMap<MongoRecordViewListItem>();
            BsonClassMap.RegisterClassMap<MongoRecordViewViewItem>();
            BsonClassMap.RegisterClassMap<MongoRecordViewHtmlItem>();
            BsonClassMap.RegisterClassMap<MongoRecordViewRelationFieldItem>();
            BsonClassMap.RegisterClassMap<MongoRecordViewRelationListItem>();
            BsonClassMap.RegisterClassMap<MongoRecordViewRelationViewItem>();
           
            

            BsonClassMap.RegisterClassMap<MongoAutoNumberField>();
            BsonClassMap.RegisterClassMap<MongoCheckboxField>();
            BsonClassMap.RegisterClassMap<MongoCurrencyField>();
            BsonClassMap.RegisterClassMap<MongoDateField>();
            BsonClassMap.RegisterClassMap<MongoDateTimeField>();
            BsonClassMap.RegisterClassMap<MongoEmailField>();
            BsonClassMap.RegisterClassMap<MongoFileField>();
            BsonClassMap.RegisterClassMap<MongoHtmlField>();
            BsonClassMap.RegisterClassMap<MongoImageField>();
            BsonClassMap.RegisterClassMap<MongoMultiLineTextField>();
            BsonClassMap.RegisterClassMap<MongoMultiSelectField>();
            BsonClassMap.RegisterClassMap<MongoNumberField>();
            BsonClassMap.RegisterClassMap<MongoPasswordField>();
            BsonClassMap.RegisterClassMap<MongoPercentField>();
            BsonClassMap.RegisterClassMap<MongoPhoneField>();
            BsonClassMap.RegisterClassMap<MongoGuidField>();
            BsonClassMap.RegisterClassMap<MongoSelectField>();
            BsonClassMap.RegisterClassMap<MongoTextField>();
            BsonClassMap.RegisterClassMap<MongoUrlField>();
            BsonClassMap.RegisterClassMap<MongoMultiSelectFieldOption>();
            BsonClassMap.RegisterClassMap<MongoSelectFieldOption>();
        }

		/// <summary>
		///     Registers the repository.
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity.</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <exception cref="System.Exception">Collection with that name has been already registered.</exception>
		public IMongoRepository<TEntity> RegisterRepository<TEntity>(string collectionName = null)
			where TEntity : MongoDocumentBase
		{
			var colName = typeof(TEntity).Name;
			if (!string.IsNullOrEmpty(collectionName))
				colName = collectionName;

			colName = colName.ToLowerInvariant();
			if (repositories.Any(x => (x as IMongoRepository<TEntity> != null) && ((IMongoRepository<TEntity>)x).Collection.Name == colName))
				throw new Exception("Collection with that name has been already registered.");

			IMongoRepository<TEntity> repository = new MongoRepository<TEntity>(Database, colName);
			repositories.Add(repository);
			return repository;
		}

		/// <summary>
		///     Gets the repository.
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity.</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <returns></returns>
		public IMongoRepository<TEntity> GetRepository<TEntity>(string collectionName = null)
			where TEntity : MongoDocumentBase
		{
			var colName = typeof(TEntity).Name;
			if (!string.IsNullOrEmpty(collectionName))
				colName = collectionName;

			colName = colName.ToLowerInvariant();
			return (IMongoRepository<TEntity>)repositories.SingleOrDefault(x => (x as IMongoRepository<TEntity> != null)
						&& ((IMongoRepository<TEntity>)x).Collection.Name == colName);
		}

		/// <summary>
		///Get non generict BsonDocuments collection
		/// </summary>
		/// <param name="collectionName"></param>
		/// <returns></returns>
		internal MongoCollection<BsonDocument> GetBsonCollection(string collectionName)
		{
			return Database.GetCollection<BsonDocument>(collectionName);
        }

		/// <summary>
		/// Creates new transaction
		/// </summary>
		/// <param name="beginImmediately"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public MongoTransaction CreateTransaction(bool beginImmediately = true, MongoTransactionOptions options = null)
		{
            return  new MongoTransaction(beginImmediately, options);
		}
	}
}