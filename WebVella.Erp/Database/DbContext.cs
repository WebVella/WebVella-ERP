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
		private static AsyncLocal<string> currendDbContextId = new AsyncLocal<string>();
		private static Dictionary<string, DbContext> dbContextDict = new Dictionary<string, DbContext>();
		private readonly object lockObj = new object();
		public static DbContext Current
		{
			get
			{
				if (currendDbContextId == null || String.IsNullOrWhiteSpace(currendDbContextId.Value))
					return null;
				return dbContextDict.ContainsKey(currendDbContextId.Value) ? dbContextDict[currendDbContextId.Value] : null;
			}
		}
		//private static AsyncLocal<DbContext> current = new AsyncLocal<DbContext>();
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
			RecordRepository = new DbRecordRepository(this);
			EntityRepository = new DbEntityRepository(this);
			RelationRepository = new DbRelationRepository(this);
			SettingsRepository = new DbSystemSettingsRepository(this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public DbConnection CreateConnection()
		{
			DbConnection con = null;
			if (transaction != null)
				con = new DbConnection(transaction, this);
			else
				con = new DbConnection(connectionString, this);

			connectionStack.Push(con);

			Debug.WriteLine($"ERP CreateConnection: {currendDbContextId.Value} | Stack count: {connectionStack.Count} | Hash: {con.GetHashCode()}");
			StackTrace t = new StackTrace();
			Debug.WriteLine($"========== ERP CreateConnection Stack =====");
			Debug.WriteLine($"{t.ToString()}");
			return con;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="conn"></param>
		public bool CloseConnection(DbConnection conn)
		{
			lock (lockObj)
			{
				var dbConn = connectionStack.Peek();
				if (dbConn != conn)
					throw new DbException("You are trying to close connection, before closing inner connections.");

				connectionStack.Pop();

				Debug.WriteLine($"ERP CloseConnection: {currendDbContextId.Value} | Stack count: {connectionStack.Count} | Hash: {conn.GetHashCode()}");
				return connectionStack.Count == 0;
			}
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

			currendDbContextId.Value = Guid.NewGuid().ToString();
			dbContextDict[currendDbContextId.Value] = new DbContext();

			Debug.WriteLine($"ERP CreateContext: {currendDbContextId.Value} | dbContextDict count: {dbContextDict.Keys.Count}");

			return dbContextDict[currendDbContextId.Value];
		}


		public static void CloseContext()
		{
			if (Current != null)
			{
				if (Current.transaction != null)
				{
					Current.transaction.Rollback();
					throw new DbException("Trying to release database context in transactional state. There is open transaction in created connections.");
				}

				//if (current.Value.connectionStack.Count > 0)
				//{
				//	throw new DbException("Trying to release database context with already opened connection. Close connection before");
				//}
			}

			Debug.WriteLine($"ERP CloseContext BEFORE: {currendDbContextId.Value} | dbContextDict count: {dbContextDict.Keys.Count}");
			if (currendDbContextId != null && dbContextDict.ContainsKey(currendDbContextId.Value))
			{
				dbContextDict.Remove(currendDbContextId.Value);
				currendDbContextId.Value = null;
			}
			Debug.WriteLine($"ERP CloseContext AFTER: dbContextDict count: {dbContextDict.Keys.Count}");

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
