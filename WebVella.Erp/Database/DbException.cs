using System;

namespace WebVella.Erp.Database
{
	public class DbException : Exception
	{
		public DbException(string message) : base(message)
		{
		}
	}
}
