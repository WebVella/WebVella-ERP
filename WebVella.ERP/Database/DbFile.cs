using Npgsql;
using System;
using System.Data;
using System.IO;

namespace WebVella.ERP.Database
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
			CreatedBy = (Guid?)row["created_by"];
			LastModifiedBy = (Guid?)row["modified_by"];
		}

		public Stream GetContentStream(DbConnection connection)
		{
			var manager = new NpgsqlLargeObjectManager(connection.connection);
			return manager.OpenReadWrite(ObjectId);
		}

		public byte[] GetBytes(DbConnection connection)
		{
			using (var contentStream = GetContentStream(connection))
			{
				return contentStream.Length == 0 ? null : new BinaryReader(contentStream).ReadBytes((int)contentStream.Length);
			}
		}
	}
}
