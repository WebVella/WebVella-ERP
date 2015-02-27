#region <--- DIRECTIVES --->

using System;
using MongoDB.Bson;
using MongoDB.Driver;

#endregion

namespace WebVella.ERP.Core.Data
{
	public class Transaction : IDisposable
	{
		#region <--- Fields and Properties --->

		private readonly IDisposable connectionDisposable;

		/// <summary>
		///     Gets the status.
		/// </summary>
		/// <value>
		///     The status.
		/// </value>
		public TransactionStatus Status { get; private set; }

		/// <summary>
		///     Gets the options.
		/// </summary>
		/// <value>
		///     The options.
		/// </value>
		public TransactionOptions Options { get; private set; }

		#endregion

		#region <--- Methods --->

		/// <summary>
		///     Initializes a new instance of the <see cref="Transaction" /> class.
		/// </summary>
		internal Transaction()
			: this(true, new TransactionOptions {Isolation = TransactionIsolation.Mvcc})
		{
			connectionDisposable = StaticDataContext.Server.RequestStart(StaticDataContext.Database);
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Transaction" /> class.
		/// </summary>
		/// <param name="beginImmediately">if set to <c>true</c> [begin immediately].</param>
		/// <param name="options">The options.</param>
		/// <exception cref="System.ArgumentNullException">options</exception>
		internal Transaction(bool beginImmediately, TransactionOptions options)
		{
			if (options == null)
				throw new ArgumentNullException("options");

			Status = TransactionStatus.Ready;
			Options = options;
			connectionDisposable = StaticDataContext.Server.RequestStart(StaticDataContext.Database);

			if (beginImmediately)
				Begin();
		}

		/// <summary>
		///     Begins this instance.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="TransactionException">
		///     Cannot start already commited transaction.
		///     or
		///     Cannot start transaction in failed state.
		///     or
		///     Cannot start transaction which is already rollbacked.
		///     or
		///     Cannot start transaction which is already started.
		/// </exception>
		public bool Begin()
		{
			switch (Status)
			{
				case TransactionStatus.Commited:
					throw new TransactionException("Cannot start already commited transaction.");
				case TransactionStatus.Failed:
					throw new TransactionException("Cannot start transaction in failed state.");
				case TransactionStatus.Rollbacked:
					throw new TransactionException("Cannot start transaction which is already rollbacked.");
				case TransactionStatus.Started:
					throw new TransactionException("Cannot start transaction which is already started.");
			}

			CommandResult result = StaticDataContext.Database.RunCommand(CreateBeginTransactionCommandDocument());
			Status = result.Ok ? TransactionStatus.Started : TransactionStatus.Failed;
			return result.Ok;
		}

		/// <summary>
		///     Commits this instance.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="TransactionException">
		///     Cannot commit already commited transaction.
		///     or
		///     Cannot commit transaction in failed state.
		///     or
		///     Cannot commit transaction which is already rollbacked.
		///     or
		///     Cannot commit transaction which is not started.
		/// </exception>
		public bool Commit()
		{
			switch (Status)
			{
				case TransactionStatus.Commited:
					throw new TransactionException("Cannot commit already commited transaction.");
				case TransactionStatus.Failed:
					throw new TransactionException("Cannot commit transaction in failed state.");
				case TransactionStatus.Rollbacked:
					throw new TransactionException("Cannot commit transaction which is already rollbacked.");
				case TransactionStatus.Ready:
					throw new TransactionException("Cannot commit transaction which is not started.");
			}

			CommandResult result = StaticDataContext.Database.RunCommand(CreateCommitTransactionCommandDocument());
			Status = result.Ok ? TransactionStatus.Commited : TransactionStatus.Failed;
			return result.Ok;
		}

		/// <summary>
		///     Rollbacks this instance.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="TransactionException">
		///     Cannot rollback already commited transaction.
		///     or
		///     Cannot rollback transaction in failed state.
		///     or
		///     Cannot rollback transaction which is already rollbacked.
		///     or
		///     Cannot rollback transaction which is not started.
		/// </exception>
		public bool Rollback()
		{
			switch (Status)
			{
				case TransactionStatus.Commited:
					throw new TransactionException("Cannot rollback already commited transaction.");
				case TransactionStatus.Failed:
					throw new TransactionException("Cannot rollback transaction in failed state.");
				case TransactionStatus.Rollbacked:
					throw new TransactionException("Cannot rollback transaction which is already rollbacked.");
				case TransactionStatus.Ready:
					throw new TransactionException("Cannot rollback transaction which is not started.");
			}

			return RollbackInternal();
		}

		/// <summary>
		///     Rollbacks the internal.
		/// </summary>
		/// <returns></returns>
		private bool RollbackInternal()
		{
			if (Status == TransactionStatus.Started)
			{
				CommandResult result = StaticDataContext.Database.RunCommand(CreateRollbackTransactionCommandDocument());
				Status = result.Ok ? TransactionStatus.Rollbacked : TransactionStatus.Failed;
				return result.Ok;
			}
			return false;
		}


		/// <summary>
		///     Creates the begin transaction command document.
		/// </summary>
		/// <returns></returns>
		private CommandDocument CreateBeginTransactionCommandDocument()
		{
			var commandDocument = new CommandDocument {new BsonElement("beginTransaction", "1")};
			switch (Options.Isolation)
			{
				default:
					//case TransactionIsolation.Mvcc:
					commandDocument.Add(new BsonElement("isolation", "mvcc"));
					break;
				case TransactionIsolation.ReadUncommited:
					commandDocument.Add(new BsonElement("isolation", "readUncommitted"));
					break;
				case TransactionIsolation.Serializable:
					commandDocument.Add(new BsonElement("isolation", "serializable"));
					break;
			}

			return commandDocument;
		}

		/// <summary>
		///     Creates the commit transaction command document.
		/// </summary>
		/// <returns></returns>
		private CommandDocument CreateCommitTransactionCommandDocument()
		{
			return new CommandDocument {new BsonElement("commitTransaction", "1")};
		}

		/// <summary>
		///     Creates the rollback transaction command document.
		/// </summary>
		/// <returns></returns>
		private CommandDocument CreateRollbackTransactionCommandDocument()
		{
			return new CommandDocument {new BsonElement("rollbackTransaction", "1")};
		}

		#endregion

		#region <--- Disposable --->

		/// <summary>
		///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		///     Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing">
		///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
		///     unmanaged resources.
		/// </param>
		public void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (connectionDisposable != null)
				{
					bool shouldRollback = Status == TransactionStatus.Started;
					if (shouldRollback)
					{
						try
						{
							RollbackInternal();
						}
						catch //ignore exceptions
						{
						}
					}

					connectionDisposable.Dispose();

					if (shouldRollback && !Options.SilenceForgottenTransaction)
						throw new TransactionException(@"Started transaction was rollbacked in distructor (probably forgotten).");
				}
			}
		}

		/// <summary>
		///     Releases unmanaged resources and performs other cleanup operations before the
		///     <see cref="Transaction" /> is reclaimed by garbage collection.
		/// </summary>
		~Transaction()
		{
			Dispose(false);
		}

		#endregion
	}
}