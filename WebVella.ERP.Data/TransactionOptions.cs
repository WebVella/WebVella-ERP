namespace WebVella.ERP.Data
{
	public class TransactionOptions
	{
		/// <summary>
		///     If TRUE, no exception will be thrown on transaction object dispose, after transaction is rolled back.
		///     Default value is FALSE.
		/// </summary>
		public bool SilenceForgottenTransaction { get; set; }

		/// <summary>
		///     Specifies the isolation level. Default is 'mvcc'.
		/// </summary>
		public TransactionIsolation Isolation { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="TransactionOptions"/> class.
		/// </summary>
		public TransactionOptions()
		{
			Isolation = TransactionIsolation.Mvcc;
			SilenceForgottenTransaction = false;
		}
	}
}