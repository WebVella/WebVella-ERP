namespace WebVella.ERP.Storage.Mongo
{
    internal enum MongoTransactionStatus
	{
		Ready,
		Failed,
		Started,
		Committed,
		Rollbacked
	}
}