#region <--- DIRECTIVES --->

using System;

#endregion

namespace WebVella.ERP.Data
{
	public class TransactionException : Exception
	{
		public TransactionException(string message)
			: base(message)
		{
		}
	}
}