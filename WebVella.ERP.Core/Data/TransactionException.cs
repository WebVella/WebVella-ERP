#region <--- DIRECTIVES --->

using System;

#endregion

namespace WebVella.ERP.Core.Data
{
	public class TransactionException : Exception
	{
		public TransactionException(string message)
			: base(message)
		{
		}
	}
}