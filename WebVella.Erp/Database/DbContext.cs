using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Database
{
	public class DbContext : IDisposable
	{
		public static DbContext Current { get { return current.Value; } }
		private static AsyncLocal<DbContext> current = new AsyncLocal<DbContext>();
		private static string connectionString;

		public DbRecordRepository RecordRepository { get; private set; }
		public DbEntityRepository EntityRepository { get; private set; }
		public DbRelationRepository RelationRepository { get; private set; }
		public DbSystemSettingsRepository SettingsRepository { get; private set; }
		public NpgsqlTransaction Transaction { get { return transaction; } }

		private Stack<DbConnection> connectionStack;
		private NpgsqlTransaction transaction;

		#region <--- Context and Connection --->

		private DbContext()
		{
			connectionStack = new Stack<DbConnection>();
			RecordRepository = new DbRecordRepository();
			EntityRepository = new DbEntityRepository();
			RelationRepository = new DbRelationRepository();
			SettingsRepository = new DbSystemSettingsRepository();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public DbConnection CreateConnection()
		{
			DbConnection con = null;
			if (transaction != null)
				con = new DbConnection(transaction);
			else
				con = new DbConnection(connectionString);

			connectionStack.Push(con);

			return con;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="conn"></param>
		public bool CloseConnection(DbConnection conn)
		{
			var dbConn = connectionStack.Peek();
			if (dbConn != conn)
				throw new DbException("You are trying to close connection, before closing inner connections.");

			connectionStack.Pop();
			return connectionStack.Count == 0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="transaction"></param>
		internal void EnterTransactionalState(NpgsqlTransaction transaction)
		{
			this.transaction = transaction;
		}

		/// <summary>
		/// 
		/// </summary>
		internal void LeaveTransactionalState()
		{
			this.transaction = null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="connString"></param>
		public static DbContext CreateContext(string connString)
		{
			connectionString = connString;

			if (current.Value == null)
				current.Value = new DbContext();

			return current.Value;
		}


		public static void CloseContext()
		{
			if (current.Value != null)
			{
				if (current.Value.transaction != null)
				{
					current.Value.transaction.Rollback();
					throw new DbException("Trying to release database context in transactional state. There is open transaction in created connections.");
				}

				//if (current.Value.connectionStack.Count > 0)
				//{
				//	throw new DbException("Trying to release database context with already opened connection. Close connection before");
				//}
			}

			current.Value = null;
		}


		#endregion


		#region <--- Dispose --->

		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		public void Dispose(bool disposing)
		{
			if (disposing)
			{
				CloseContext();
			}
		}
		#endregion
	}
}
