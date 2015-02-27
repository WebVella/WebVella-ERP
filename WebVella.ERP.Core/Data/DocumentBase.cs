#region <--- DIRECTIVES --->

using System;
using MongoDB.Bson.Serialization.Attributes;

#endregion

namespace WebVella.ERP.Core.Data
{
	public abstract class DocumentBase
	{
		[BsonId]
		public Guid Id { get; set; }
	}
}