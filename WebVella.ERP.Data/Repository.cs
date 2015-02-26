#region <--- DIRECTIVES --->

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

#endregion

namespace WebVella.ERP.Data
{
	public class Repository<TDocument> : IRepository<TDocument> where TDocument : DocumentBase
	{
		/// <summary>
		///     Gets the collection.
		/// </summary>
		/// <value>
		///     The collection.
		/// </value>
		public MongoCollection<TDocument> Collection { get; set; }

		/// <summary>
		///     Initializes a new instance of the <see cref="MongoRepository{TDocument}" /> class.
		/// </summary>
		/// <param name="database">The database.</param>
		/// <param name="collectionName">Name of the collection.</param>
		public Repository(MongoDatabase database, string collectionName)
		{
			Collection = database.GetCollection<TDocument>(collectionName);
			if (!Collection.Exists())
				Collection.Database.CreateCollection( Collection.Name );
		}

		/// <summary>
		///     Ensures the index.
		/// </summary>
		/// <param name="keys">The keys.Sample: new IndexKeysBuilder().Ascending( "---THE NAME OF THE FIELD---" ) </param>
		/// <param name="options">
		///     The options. Sample: IndexOptions.SetName( "---THE NAME OF THE INDEX---" ).SetUnique( false
		///     ).SetBackground( true ) )
		/// </param>
		public virtual void EnsureIndex(IMongoIndexKeys keys, IMongoIndexOptions options)
		{
			if (!Collection.Exists())
				Collection.Database.CreateCollection(Collection.Name);

			Collection.EnsureIndex(keys, options);
		}

		/// <summary>
		///     Updates the specified document.
		/// </summary>
		/// <param name="document">The document.</param>
		/// <returns></returns>
		public bool Save(TDocument document)
		{
			return Collection.Save(document).DocumentsAffected > 0;
		}

		/// <summary>
		///     Deletes the specified document.
		/// </summary>
		/// <param name="document">The document.</param>
		/// <returns></returns>
		public bool Delete(TDocument document)
		{
			return Collection.Remove(Query.EQ("_id", document.Id)).DocumentsAffected > 0;
		}

		/// <summary>
		/// Deletes documents related to the specified query.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <returns></returns>
		public bool Delete(IMongoQuery query)
		{
			return Collection.Remove( query ).DocumentsAffected > 0;
		}

		/// <summary>
		/// Gets list of documents related to specified query parameters
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		/// <param name="sortBy">The sort by.</param>
		/// <param name="skip">The skip.</param>
		/// <param name="limit">The limit.</param>
		/// <returns></returns>
		public IList<TDocument> Get( Expression<Func<TDocument, bool>> predicate, IMongoSortBy sortBy = null, int? skip = null, int? limit = null )
		{
			var cursor = Collection.FindAs<TDocument>( Query<TDocument>.Where(predicate) );
			if (skip.HasValue)
				cursor.SetSkip( skip.Value );
			if (limit.HasValue)
				cursor.SetLimit( limit.Value );
			if (sortBy != null)
				cursor.SetSortOrder(sortBy);
			
			return cursor.ToList();
		}

        /// <summary>
        /// Gets list of documents related to specified query parameters
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="sortBy">The sort by.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="limit">The limit.</param>
        /// <returns></returns>
        public IList<TDocument> Get( IMongoQuery query, IMongoSortBy sortBy = null, int? skip = null, int? limit = null )
		{
			if (query == null)
				query = Query.Null;

			var cursor = Collection.FindAs<TDocument>(query);
			if (skip.HasValue)
				cursor.SetSkip( skip.Value );
			if (limit.HasValue)
				cursor.SetLimit( limit.Value );
			if (sortBy != null)
				cursor.SetSortOrder( sortBy );
			
			return cursor.ToList();
		}

        /// <summary>
        /// Gets list of documents related to specified query parameters
        /// </summary>
        /// <param name="sortBy">The sort by.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="limit">The limit.</param>
        /// <returns></returns>
        public IList<TDocument> Get( IMongoSortBy sortBy = null, int? skip = null, int? limit = null )
		{
			var cursor = Collection.FindAllAs<TDocument>();
			if (skip.HasValue)
				cursor.SetSkip(skip.Value);
			if( limit.HasValue )
				cursor.SetLimit(limit.Value);
			if (sortBy != null)
				cursor.SetSortOrder( sortBy );

			return cursor.ToList();
		}

	
		/// <summary>
		///     Gets document specified by its identifier
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public TDocument GetById(Guid id)
		{
			return Collection.FindOneByIdAs<TDocument>(id);
		}

		/// <summary>
		/// Counts documents related to specified query parameters.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		/// <returns></returns>
		public int Count(Expression<Func<TDocument, bool>> predicate)
		{
			return Collection.AsQueryable<TDocument>().Where(predicate).Count();
		}

		/// <summary>
		/// Returns true if there are at least one document related to specified query
		/// </summary>
		/// <param name="query">The query.</param>
		/// <returns></returns>
		public bool Any( IMongoQuery query )
		{
			return Collection.FindOne(query) != null;
		}

        /// <summary>
        /// Returns true if there are at least one document related to specified query parameters
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public bool Any( Expression<Func<TDocument, bool>> predicate )
		{
			return Collection.AsQueryable().Any( predicate );
		}

		public TDocument SingleOrDefault( Expression<Func<TDocument, bool>> predicate )
		{
			return Collection.AsQueryable().SingleOrDefault( predicate );
		}

		public TDocument FirstOrDefault(Expression<Func<TDocument, bool>> predicate)
		{
			return Collection.AsQueryable().FirstOrDefault( predicate );
		}

		public TDocument LastOrDefault(Expression<Func<TDocument, bool>> predicate)
		{
			return Collection.AsQueryable().LastOrDefault( predicate );
		}

		public TDocument Single( Expression<Func<TDocument, bool>> predicate )
		{
			return Collection.AsQueryable().SingleOrDefault( predicate );
		}

		public TDocument First( Expression<Func<TDocument, bool>> predicate )
		{
			return Collection.AsQueryable().FirstOrDefault( predicate );
		}

		public TDocument Last( Expression<Func<TDocument, bool>> predicate )
		{
			return Collection.AsQueryable().LastOrDefault( predicate );
		}

		
	}
}