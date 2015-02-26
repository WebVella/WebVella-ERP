#region <--- DIRECTIVES --->

using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;

#endregion

namespace WebVella.ERP.Data
{
	public static class StaticDataContext
	{
		private static readonly List<object> repositories;

		public static MongoServer Server { get; set; }
		public static MongoDatabase Database { get; set; }

		/// <summary>
		///     Initializes the <see cref="MongoStaticContext" /> class.
		/// </summary>
		static StaticDataContext()
		{
			repositories = new List<object>();
		}

		/// <summary>
		///     Initializes the specified connection string.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		public static void Initialize(string connectionString)
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
		public static IRepository<TEntity> RegisterRepository<TEntity>(string collectionName = null)
			where TEntity : DocumentBase
		{
			var colName = typeof (TEntity).Name;
			if (!string.IsNullOrEmpty(collectionName))
				colName = collectionName;

			colName = colName.ToLowerInvariant();
			if (repositories.Any(x => ( x as IRepository<TEntity> != null ) && ((IRepository<TEntity>) x).Collection.Name == colName))
				throw new Exception("Collection with that name has been already registered.");

			IRepository<TEntity> repository = new Repository<TEntity>( Database, colName );
			repositories.Add(repository);
			return repository;
		}

		/// <summary>
		///     Gets the repository.
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity.</typeparam>
		/// <param name="collectionName">Name of the collection.</param>
		/// <returns></returns>
		public static IRepository<TEntity> GetRepository<TEntity>(string collectionName = null)
			where TEntity : DocumentBase
		{
			var colName = typeof (TEntity).Name;
			if (!string.IsNullOrEmpty(collectionName))
				colName = collectionName;

			colName = colName.ToLowerInvariant();
			return
				(IRepository<TEntity>)repositories.SingleOrDefault( x => (x as IRepository<TEntity> != null) && ((IRepository<TEntity>)x).Collection.Name == colName );
		}
        
        /// <summary>
        /// Creates new transaction
        /// </summary>
        /// <param name="beginImmediately"></param>
        /// <param name="options"></param>
        /// <returns></returns>
		public static Transaction CreateTransaction( bool beginImmediately = true, TransactionOptions options = null)
		{
			return options != null ? new Transaction( beginImmediately, options ) : new Transaction();
		}
	}
}