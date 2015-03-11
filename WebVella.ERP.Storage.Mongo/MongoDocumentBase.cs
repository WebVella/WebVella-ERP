#region <--- DIRECTIVES --->

using System;
using MongoDB.Bson.Serialization.Attributes;

#endregion

namespace WebVella.ERP.Storage.Mongo
{
	public abstract class MongoDocumentBase
	{
		[BsonId]
		public Guid Id { get; set; }
	}
}