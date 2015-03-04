#region <--- DIRECTIVES --->

using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;

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

        /// <summary>
        ///     Initializes the <see cref="MongoStaticContext" /> class.
        /// </summary>
        private MongoStaticContext()
		{
			repositories = new List<object>();
		}

		/// <summary>
		///     Initializes the specified connection string.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		public void Initialize(string connectionString)
		{
			MongoUrl mongoUrl = new MongoUrl(connectionString);
			Server = new MongoClient(mongoUrl).GetServer();
			Database = Server.GetDatabase(mongoUrl.DatabaseName);
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
			var colName = typeof (TEntity).Name;
			if (!string.IsNullOrEmpty(collectionName))
				colName = collectionName;

			colName = colName.ToLowerInvariant();
			if (repositories.Any(x => ( x as IMongoRepository<TEntity> != null ) && ((IMongoRepository<TEntity>) x).Collection.Name == colName))
				throw new Exception("Collection with that name has been already registered.");

			IMongoRepository<TEntity> repository = new MongoRepository<TEntity>( Database, colName );
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
			var colName = typeof (TEntity).Name;
			if (!string.IsNullOrEmpty(collectionName))
				colName = collectionName;

			colName = colName.ToLowerInvariant();
			return
				(IMongoRepository<TEntity>)repositories.SingleOrDefault( x => (x as IMongoRepository<TEntity> != null) && ((IMongoRepository<TEntity>)x).Collection.Name == colName );
		}
        
        /// <summary>
        /// Creates new transaction
        /// </summary>
        /// <param name="beginImmediately"></param>
        /// <param name="options"></param>
        /// <returns></returns>
		public MongoTransaction CreateTransaction( bool beginImmediately = true, MongoTransactionOptions options = null)
		{
			return options != null ? new MongoTransaction( beginImmediately, options ) : new MongoTransaction();
		}
	}
}