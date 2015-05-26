#region <--- DIRECTIVES --->

using System;
using MongoDB.Bson;
using MongoDB.Driver;

#endregion

namespace WebVella.ERP.Storage.Mongo
{
    internal class MongoTransaction : IStorageTransaction, IDisposable
	{
		#region <--- Fields and Properties --->

		private readonly IDisposable connectionDisposable;

		/// <summary>
		///     Gets the status.
		/// </summary>
		/// <value>
		///     The status.
		/// </value>
		public MongoTransactionStatus Status { get; private set; }

		/// <summary>
		///     Gets the options.
		/// </summary>
		/// <value>
		///     The options.
		/// </value>
		public MongoTransactionOptions Options { get; private set; }

		#endregion

		#region <--- Methods --->

		/// <summary>
		///     Initializes a new instance of the <see cref="MongoTransaction" /> class.
		/// </summary>
		internal MongoTransaction()
			: this(true, new MongoTransactionOptions {Isolation = MongoTransactionIsolation.Mvcc})
		{
			connectionDisposable = MongoStaticContext.Context.Server.RequestStart(MongoStaticContext.Context.Database);
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="MongoTransaction" /> class.
		/// </summary>
		/// <param name="beginImmediately">if set to <c>true</c> [begin immediately].</param>
		/// <param name="options">The options.</param>
		/// <exception cref="System.ArgumentNullException">options</exception>
		internal MongoTransaction(bool beginImmediately, MongoTransactionOptions options)
		{
            if (options == null)
                Options = new MongoTransactionOptions();

			Status = MongoTransactionStatus.Ready;
			connectionDisposable = MongoStaticContext.Context.Server.RequestStart(MongoStaticContext.Context.Database);

			if (beginImmediately)
				Begin();
		}

		/// <summary>
		///     Begins this instance.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="MongoTransactionException">
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
				case MongoTransactionStatus.Commited:
					throw new MongoTransactionException("Cannot start already commited transaction.");
				case MongoTransactionStatus.Failed:
					throw new MongoTransactionException("Cannot start transaction in failed state.");
				case MongoTransactionStatus.Rollbacked:
					throw new MongoTransactionException("Cannot start transaction which is already rollbacked.");
				case MongoTransactionStatus.Started:
					throw new MongoTransactionException("Cannot start transaction which is already started.");
			}

			CommandResult result = MongoStaticContext.Context.Database.RunCommand(CreateBeginTransactionCommandDocument());
			Status = result.Ok ? MongoTransactionStatus.Started : MongoTransactionStatus.Failed;
			return result.Ok;
		}

		/// <summary>
		///     Commits this instance.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="MongoTransactionException">
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
				case MongoTransactionStatus.Commited:
					throw new MongoTransactionException("Cannot commit already commited transaction.");
				case MongoTransactionStatus.Failed:
					throw new MongoTransactionException("Cannot commit transaction in failed state.");
				case MongoTransactionStatus.Rollbacked:
					throw new MongoTransactionException("Cannot commit transaction which is already rollbacked.");
				case MongoTransactionStatus.Ready:
					throw new MongoTransactionException("Cannot commit transaction which is not started.");
			}

			CommandResult result = MongoStaticContext.Context.Database.RunCommand(CreateCommitTransactionCommandDocument());
			Status = result.Ok ? MongoTransactionStatus.Commited : MongoTransactionStatus.Failed;
			return result.Ok;
		}

		/// <summary>
		///     Rollbacks this instance.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="MongoTransactionException">
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
				case MongoTransactionStatus.Commited:
					throw new MongoTransactionException("Cannot rollback already commited transaction.");
				case MongoTransactionStatus.Failed:
					throw new MongoTransactionException("Cannot rollback transaction in failed state.");
				case MongoTransactionStatus.Rollbacked:
					throw new MongoTransactionException("Cannot rollback transaction which is already rollbacked.");
				case MongoTransactionStatus.Ready:
					throw new MongoTransactionException("Cannot rollback transaction which is not started.");
			}

			return RollbackInternal();
		}

		/// <summary>
		///     Rollbacks the internal.
		/// </summary>
		/// <returns></returns>
		private bool RollbackInternal()
		{
			if (Status == MongoTransactionStatus.Started)
			{
				CommandResult result = MongoStaticContext.Context.Database.RunCommand(CreateRollbackTransactionCommandDocument());
				Status = result.Ok ? MongoTransactionStatus.Rollbacked : MongoTransactionStatus.Failed;
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
				case MongoTransactionIsolation.ReadUncommited:
					commandDocument.Add(new BsonElement("isolation", "readUncommitted"));
					break;
				case MongoTransactionIsolation.Serializable:
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
					bool shouldRollback = Status == MongoTransactionStatus.Started;
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
						throw new MongoTransactionException(@"Started transaction was rollbacked in distructor (probably forgotten).");
				}
			}
		}

		/// <summary>
		///     Releases unmanaged resources and performs other cleanup operations before the
		///     <see cref="MongoTransaction" /> is reclaimed by garbage collection.
		/// </summary>
		~MongoTransaction()
		{
			Dispose(false);
		}

		#endregion
	}
}