namespace WebVella.ERP.Core.Data
{
	public enum TransactionIsolation
	{
		Mvcc,
		Serializable,
		ReadUncommited
	}
}