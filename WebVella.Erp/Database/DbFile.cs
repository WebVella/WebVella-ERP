using Npgsql;
using Storage.Net.Blobs;
using System;
using System.Data;
using System.IO;

namespace WebVella.Erp.Database
{
	public class DbFile
	{
		public Guid Id { get; set; }
		public uint ObjectId { get; set; }
		public string FilePath { get; set; }
		public Guid? CreatedBy { get; set; }
		public DateTime CreatedOn { get; set; }
		public Guid? LastModifiedBy { get; set; }
		public DateTime LastModificationDate { get; set; }

		internal DbFile(DataRow row)
		{
			Id = (Guid)row["id"];
			ObjectId = (uint)((decimal)row["object_id"]);
			FilePath = (string)row["filepath"];
			CreatedOn = (DateTime)row["created_on"];
			LastModificationDate = (DateTime)row["modified_on"];

			CreatedBy = null;
			if (row["created_by"] != DBNull.Value)
				CreatedBy = (Guid?)row["created_by"];

			LastModifiedBy = null;
			if (row["modified_by"] != DBNull.Value)
				LastModifiedBy = (Guid?)row["modified_by"];
		}

		private Stream GetContentStream(DbConnection connection, FileAccess fileAccess = FileAccess.ReadWrite, FileShare fileShare = FileShare.ReadWrite )
		{
			if (ErpSettings.EnableCloudBlobStorage && this.ObjectId == 0)
			{
				var path = DbFileRepository.GetBlobPath(this);
				using (IBlobStorage storage = DbFileRepository.GetBlobStorage())
				{
					return storage.OpenReadAsync(path).Result;
				}
			}
			else if (ErpSettings.EnableFileSystemStorage && ObjectId == 0)
			{
				var path = DbFileRepository.GetFileSystemPath(this);
				if (File.Exists(path))
					return File.Open(path, FileMode.Open, fileAccess, fileShare);

				throw new Exception($"File '{path}' was not found.");
			}
			else
			{
				if (ObjectId == 0)
					throw new Exception("Trying to get content of a file from database, but it was uploaded to file system. Check FileSystem support configuration.");

				var manager = new NpgsqlLargeObjectManager(connection.connection);
				switch (fileAccess)
				{
					case FileAccess.Read:
						return manager.OpenRead(ObjectId);
					case FileAccess.Write:
						return manager.OpenRead(ObjectId);
					case FileAccess.ReadWrite:
						return manager.OpenReadWrite(ObjectId);
				}
				return manager.OpenReadWrite(ObjectId);
			}
		}

		public byte[] GetBytes(DbConnection connection)
		{
			using (var contentStream = GetContentStream(connection, FileAccess.Read, FileShare.Read ))
			{
				return contentStream.Length == 0 ? null : new BinaryReader(contentStream).ReadBytes((int)contentStream.Length);
			}
		}

		public byte[] GetBytes()
		{
			if (ErpSettings.EnableFileSystemStorage && ObjectId == 0)
			{
				//no need for database connection and any transaction
				return GetBytes(null);
			}

			using (DbConnection connection = DbContext.Current.CreateConnection())
			{
				try
				{
					connection.BeginTransaction();
					var result = GetBytes(connection);
					connection.CommitTransaction();
					return result;
				}
				catch
				{
					connection.RollbackTransaction();
					throw;
				}
			}
		}
	}
}
