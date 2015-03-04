namespace WebVella.ERP.Storage.Mongo
{
    internal class MongoTransactionOptions
	{
		/// <summary>
		///     If TRUE, no exception will be thrown on transaction object dispose, after transaction is rolled back.
		///     Default value is FALSE.
		/// </summary>
		public bool SilenceForgottenTransaction { get; set; }

		/// <summary>
		///     Specifies the isolation level. Default is 'mvcc'.
		/// </summary>
		public MongoTransactionIsolation Isolation { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="MongoTransactionOptions"/> class.
		/// </summary>
		public MongoTransactionOptions()
		{
			Isolation = MongoTransactionIsolation.Mvcc;
			SilenceForgottenTransaction = false;
		}
	}
}