#region <--- DIRECTIVES --->

using System;
using MongoDB.Bson.Serialization.Attributes;

#endregion

namespace WebVella.ERP.Storage.Mongo
{
	internal abstract class MongoDocumentBase
	{
		[BsonId]
		public Guid Id { get; set; }
	}
}