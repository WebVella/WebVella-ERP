using System;

namespace WebVella.ERP.Database
{
	public class DbException : Exception
	{
		public DbException(string message) : base(message)
		{
		}
	}
}
