#region <--- DIRECTIVES --->

using System;

#endregion

namespace WebVella.ERP.Storage.Mongo
{
    internal class MongoTransactionException : Exception
	{
		public MongoTransactionException(string message)
			: base(message)
		{
		}
	}
}