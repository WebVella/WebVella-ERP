#region <--- DIRECTIVES --->

using System;
using MongoDB.Bson.Serialization.Attributes;

#endregion

namespace WebVella.ERP.Data
{
	public abstract class DocumentBase
	{
		[BsonId]
		public Guid Id { get; set; }
	}
}