#region <--- DIRECTIVES --->

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Driver;

#endregion

namespace WebVella.ERP.Storage.Mongo
{
	internal interface IMongoRepository<TDocument> where TDocument : MongoDocumentBase
	{
		MongoCollection<TDocument> Collection { get; set; }
        bool Create(TDocument entity);
        bool Update(TDocument entity);
        bool Save(TDocument entity);
		bool Delete(TDocument entity);
		bool Delete( IMongoQuery query );
		IList<TDocument> Get( IMongoQuery query, IMongoSortBy sortBy = null, int? skip = null, int? limit = null );
		IList<TDocument> Get( Expression<Func<TDocument, bool>> predicate, IMongoSortBy sortBy = null, int? skip = null, int? limit = null );
		IList<TDocument> Get( IMongoSortBy sortBy = null, int? skip = null, int? limit = null );
		bool Any( Expression<Func<TDocument, bool>> predicate );
		bool Any( IMongoQuery query );
        TDocument SingleOrDefault( Expression<Func<TDocument, bool>> predicate );
        TDocument FirstOrDefault( Expression<Func<TDocument, bool>> predicate );
        TDocument LastOrDefault( Expression<Func<TDocument, bool>> predicate );
        TDocument Single( Expression<Func<TDocument, bool>> predicate );
        TDocument First( Expression<Func<TDocument, bool>> predicate );
        TDocument Last( Expression<Func<TDocument, bool>> predicate );
        TDocument GetById(Guid id);
		int Count( Expression<Func<TDocument, bool>> predicate );

		void CreateIndex(IMongoIndexKeys keys, IMongoIndexOptions options);
	}
}