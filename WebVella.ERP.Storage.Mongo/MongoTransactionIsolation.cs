namespace WebVella.ERP.Storage.Mongo
{
    internal enum MongoTransactionIsolation
	{
		Mvcc,
		Serializable,
		ReadUncommitted
	}
}