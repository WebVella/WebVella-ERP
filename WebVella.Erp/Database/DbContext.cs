using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Npgsql;

namespace WebVella.Erp.Database
{
	public class DbContext : IDisposable
	{
		private static AsyncLocal<string> currentDbContextId = new AsyncLocal<string>();
		private static ConcurrentDictionary<string, DbContext> dbContextDict = new ConcurrentDictionary<string, DbContext>();
		private readonly object lockObj = new object();
		public static DbContext Current
		{
			get
			{
				if (currentDbContextId == null || String.IsNullOrWhiteSpace(currentDbContextId.Value))
					return null;

				DbContext context = null;
				dbContextDict.TryGetValue(currentDbContextId.Value, out context);
				return context;
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

			Debug.WriteLine($"ERP CreateConnection: {currentDbContextId.Value} | Stack count: {connectionStack.Count} | Hash: {con.GetHashCode()}");
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

				Debug.WriteLine($"ERP CloseConnection: {currentDbContextId.Value} | Stack count: {connectionStack.Count} | Hash: {conn.GetHashCode()}");
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

			currentDbContextId.Value = Guid.NewGuid().ToString();
			if (!dbContextDict.TryAdd(currentDbContextId.Value, new DbContext()))
				throw new Exception("Cannot create new context and store it into context dictionary");

			Debug.WriteLine($"ERP CreateContext: {currentDbContextId.Value} | dbContextDict count: {dbContextDict.Keys.Count}");

			DbContext context;
			if (!dbContextDict.TryGetValue(currentDbContextId.Value, out context))
				throw new Exception("Cannot create new context and read it into context dictionary");

			return context;
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

			Debug.WriteLine($"ERP CloseContext BEFORE: {currentDbContextId.Value} | dbContextDict count: {dbContextDict.Keys.Count}");
			string idValue = null;
			if (currentDbContextId != null && !string.IsNullOrWhiteSpace(currentDbContextId.Value))
				idValue = currentDbContextId.Value;

			if (!string.IsNullOrWhiteSpace(idValue))
			{
				DbContext context;
				dbContextDict.TryRemove(idValue, out context);
				if (context != null)
					context.Dispose();

				currentDbContextId.Value = null;
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
