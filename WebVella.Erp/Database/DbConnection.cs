using System;
using Npgsql;
using NpgsqlTypes;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Text;

namespace WebVella.Erp.Database
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
		public NpgsqlCommand CreateCommand(string code, CommandType commandType = CommandType.Text, List<NpgsqlParameter> parameters = null)
		{
			NpgsqlCommand command = null;
			if (transaction != null)
				command = new NpgsqlCommand(code, connection, transaction);
			else
				command = new NpgsqlCommand(code, connection);

			command.CommandType = commandType;
			if (parameters != null)
				command.Parameters.AddRange(parameters.ToArray());

			return command;
		}

		public bool AcquireAdvisoryLock(long key)
		{
			NpgsqlCommand command = CreateCommand("SELECT pg_try_advisory_xact_lock(@key);");
			command.Parameters.Add(new NpgsqlParameter("@key", key));
			using (var reader = command.ExecuteReader())
			{

				try
				{
					if (reader.Read())
						return (bool)reader[0];
					else
						return false;

				}
				finally
				{
					reader.Close();
				}
			}
		}

		public bool AcquireAdvisoryLock(string key)
		{
			Int64 hashCode = 0;
			if (!string.IsNullOrEmpty(key))
			{
				//Unicode Encode Covering all characterset
				byte[] byteContents = Encoding.Unicode.GetBytes(key);
				System.Security.Cryptography.SHA256 hash =
				new System.Security.Cryptography.SHA256CryptoServiceProvider();
				byte[] hashText = hash.ComputeHash(byteContents);
				//32Byte hashText separate
				//hashCodeStart = 0~7  8Byte
				//hashCodeMedium = 8~23  8Byte
				//hashCodeEnd = 24~31  8Byte
				//and Fold
				Int64 hashCodeStart = BitConverter.ToInt64(hashText, 0);
				Int64 hashCodeMedium = BitConverter.ToInt64(hashText, 8);
				Int64 hashCodeEnd = BitConverter.ToInt64(hashText, 24);
				hashCode = hashCodeStart ^ hashCodeMedium ^ hashCodeEnd;
			}
			
			return AcquireAdvisoryLock(hashCode);
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

			if (transactionStack.Count > 0)
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
