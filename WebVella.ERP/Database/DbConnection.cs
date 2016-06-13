using System;
using Npgsql;
using NpgsqlTypes;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace WebVella.ERP.Database
{
	public class DbConnection : IDisposable
	{
		private Stack<string> transactionStack = new Stack<string>();
		internal NpgsqlTransaction transaction;
		internal NpgsqlConnection connection;
		private bool initialTransactionHolder = false;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="transaction"></param>
		internal DbConnection(NpgsqlTransaction transaction)
		{
			this.transaction = transaction;
			connection = transaction.Connection;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="connectionString"></param>
		internal DbConnection(string connectionString)
		{
			transaction = null;
			connection = new NpgsqlConnection(connectionString);
			connection.Open();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="code"></param>
		/// <param name="commandType"></param>
		/// <returns></returns>
		public NpgsqlCommand CreateCommand(string code, CommandType commandType = CommandType.Text)
		{
			NpgsqlCommand command = null;
			if (transaction != null)
				command = new NpgsqlCommand(code, connection, transaction);
			else
				command = new NpgsqlCommand(code, connection);

			command.CommandType = commandType;
			return command;
		}

		/// <summary>
		/// 
		/// </summary>
		public void BeginTransaction()
		{
			if (transaction == null)
			{
				initialTransactionHolder = true;
				transaction = connection.BeginTransaction();
				DbContext.Current.EnterTransactionalState(transaction);
			}
			else
			{
				string savePointName = "tr_" + (Guid.NewGuid().ToString().Replace("-", ""));
				transaction.Save(savePointName);
				transactionStack.Push(savePointName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void CommitTransaction()
		{
			if (transaction == null)
				throw new Exception("Trying to commit non existent transaction.");

			if (transactionStack.Count() > 0)
			{
				transactionStack.Pop();
			}
			else
			{
				DbContext.Current.LeaveTransactionalState();
				if (!initialTransactionHolder)
				{
					transaction.Rollback();
					transaction = null;
					throw new Exception("Trying to commit transaction started from another connection. The transaction is rolled back.");
				}
				transaction.Commit();
				transaction = null;

			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void RollbackTransaction()
		{
			if (transaction == null)
				throw new Exception("Trying to rollback non existent transaction.");

			if (transactionStack.Count() > 0)
			{
				var savepointName = transactionStack.Pop();
				transaction.Rollback(savepointName);
			}
			else
			{
				transaction.Rollback();
				DbContext.Current.LeaveTransactionalState();
				transaction = null;
				if (!initialTransactionHolder)
					throw new Exception("Trying to rollback transaction started from another connection.The transaction is rolled back, but this exception is thrown to notify.");
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Close()
		{
			if (transaction != null && initialTransactionHolder)
			{
				transaction.Rollback();
				throw new Exception("Trying to close connection with pending transaction. The transaction is rolled back.");
			}

			if ( transactionStack.Count > 0)
				throw new Exception("Trying to close connection with pending transaction. The transaction is rolled back.");

			DbContext.Current.CloseConnection(this);
			if (transaction == null)
				connection.Close();
		}

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
				Close();
			}
		}

	}
}
