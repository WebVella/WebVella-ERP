using Npgsql;
using Storage.Net;
using Storage.Net.Blobs;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace WebVella.Erp.Database
{
	public class DbFileRepository
	{
		public const string FOLDER_SEPARATOR = "/";
		public const string TMP_FOLDER_NAME = "tmp";

		private DbContext suppliedContext = null;
		private DbContext CurrentContext
		{
			get
			{
				if (suppliedContext != null)
					return suppliedContext;
				else
					return DbContext.Current;
			}
		}

		public DbFileRepository(DbContext currentContext = null)
		{
			if (currentContext != null)
				suppliedContext = currentContext;
		}
		public DbFile Find(string filepath)
		{
			if (string.IsNullOrWhiteSpace(filepath))
				throw new ArgumentException("filepath cannot be null or empty");

			//all filepaths are lowercase and all starts with folder separator
			filepath = filepath.ToLowerInvariant();
			if (!filepath.StartsWith(FOLDER_SEPARATOR))
				filepath = FOLDER_SEPARATOR + filepath;

			using (var connection = CurrentContext.CreateConnection())
			{
				var command = connection.CreateCommand("SELECT * FROM files WHERE filepath = @filepath ");
				command.Parameters.Add(new NpgsqlParameter("@filepath", filepath));
				DataTable dataTable = new DataTable();
				new NpgsqlDataAdapter(command).Fill(dataTable);

				if (dataTable.Rows.Count == 1)
					return new DbFile(dataTable.Rows[0]);
			}

			return null;
		}

		public List<DbFile> FindAll(string startsWithPath = null, bool includeTempFiles = false, int? skip = null, int? limit = null)
		{
			//all filepaths are lowercase and all starts with folder separator
			if (!string.IsNullOrWhiteSpace(startsWithPath))
			{
				startsWithPath = startsWithPath.ToLowerInvariant();

				if (!startsWithPath.StartsWith(FOLDER_SEPARATOR))
					startsWithPath = FOLDER_SEPARATOR + startsWithPath;
			}

			string pagingSql = string.Empty;
			if (limit != null || skip != null)
			{
				pagingSql = " LIMIT ";
				if (limit.HasValue)
					pagingSql = pagingSql + limit + " ";
				else
					pagingSql = pagingSql + "ALL ";

				if (skip.HasValue)
					pagingSql = pagingSql + " OFFSET " + skip;
			}

			DataTable table = new DataTable();
			using (var connection = CurrentContext.CreateConnection())
			{
				var command = connection.CreateCommand(string.Empty);
				if (!includeTempFiles && !string.IsNullOrWhiteSpace(startsWithPath))
				{
					command.CommandText = "SELECT * FROM files WHERE filepath NOT ILIKE @tmp_path AND filepath ILIKE @startswith" + pagingSql;
					command.Parameters.Add(new NpgsqlParameter("@tmp_path", "%" + FOLDER_SEPARATOR + TMP_FOLDER_NAME));
					command.Parameters.Add(new NpgsqlParameter("@startswith", "%" + startsWithPath));
					new NpgsqlDataAdapter(command).Fill(table);
				}
				else if (!string.IsNullOrWhiteSpace(startsWithPath))
				{
					command.CommandText = "SELECT * FROM files WHERE filepath ILIKE @startswith" + pagingSql;
					command.Parameters.Add(new NpgsqlParameter("@startswith", "%" + startsWithPath));
					new NpgsqlDataAdapter(command).Fill(table);
				}
				else if (!includeTempFiles)
				{
					command.CommandText = "SELECT * FROM files WHERE filepath NOT ILIKE @tmp_path " + pagingSql;
					command.Parameters.Add(new NpgsqlParameter("@tmp_path", "%" + FOLDER_SEPARATOR + TMP_FOLDER_NAME));
					new NpgsqlDataAdapter(command).Fill(table);
				}
				else
				{
					command.CommandText = "SELECT * FROM files " + pagingSql;
					new NpgsqlDataAdapter(command).Fill(table);
				}
			}

			List<DbFile> files = new List<DbFile>();
			foreach (DataRow row in table.Rows)
				files.Add(new DbFile(row));

			return files;
		}

		public DbFile Create(string filepath, byte[] buffer, DateTime? createdOn, Guid? createdBy)
		{
			if (string.IsNullOrWhiteSpace(filepath))
				throw new ArgumentException("filepath cannot be null or empty");

			//all filepaths are lowercase and all starts with folder separator
			filepath = filepath.ToLowerInvariant();
			if (!filepath.StartsWith(FOLDER_SEPARATOR))
				filepath = FOLDER_SEPARATOR + filepath;

			if (Find(filepath) != null)
				throw new ArgumentException(filepath + ": file already exists");

			using (var connection = CurrentContext.CreateConnection())
			{
				try
				{
					uint objectId = 0;
					connection.BeginTransaction();

					if (!ErpSettings.EnableFileSystemStorage)
					{
						var manager = new NpgsqlLargeObjectManager(connection.connection);
						objectId = manager.Create();

						using (var stream = manager.OpenReadWrite(objectId))
						{
							stream.Write(buffer, 0, buffer.Length);
							stream.Close();
						}
					}


					var command = connection.CreateCommand(@"INSERT INTO files(id,object_id,filepath,created_on,modified_on,created_by,modified_by) 
															 VALUES (@id,@object_id,@filepath,@created_on,@modified_on,@created_by,@modified_by)");

					command.Parameters.Add(new NpgsqlParameter("@id", Guid.NewGuid()));
					command.Parameters.Add(new NpgsqlParameter("@object_id", (decimal)objectId));
					command.Parameters.Add(new NpgsqlParameter("@filepath", filepath));
					var date = createdOn ?? DateTime.UtcNow;
					command.Parameters.Add(new NpgsqlParameter("@created_on", date));
					command.Parameters.Add(new NpgsqlParameter("@modified_on", date));
					command.Parameters.Add(new NpgsqlParameter("@created_by", (object)createdBy ?? DBNull.Value));
					command.Parameters.Add(new NpgsqlParameter("@modified_by", (object)createdBy ?? DBNull.Value));

					command.ExecuteNonQuery();

					var result = Find(filepath);

					if(ErpSettings.EnableCloudBlobStorage)
					{
						var path = GetBlobPath(result);
						using (IBlobStorage storage = GetBlobStorage())
						{
							storage.WriteAsync(path,
								buffer).Wait();
						}
					}
					else if (ErpSettings.EnableFileSystemStorage)
					{
						var path = GetFileSystemPath(result);
						var folderPath = Path.GetDirectoryName(path);
						if (!Directory.Exists(folderPath))
							Directory.CreateDirectory(folderPath);
						using (Stream stream = File.Open(path, FileMode.CreateNew, FileAccess.ReadWrite))
						{
							stream.Write(buffer, 0, buffer.Length);
							stream.Close();
						}
					}

					connection.CommitTransaction();
				}
				catch (Exception)
				{
					connection.RollbackTransaction();
					throw;
				}
			}

			return Find(filepath);
		}

		public DbFile UpdateModificationDate(string filepath, DateTime modificationDate)
		{
			if (string.IsNullOrWhiteSpace(filepath))
				throw new ArgumentException("filepath cannot be null or empty");

			//all filepaths are lowercase and all starts with folder separator
			filepath = filepath.ToLowerInvariant();
			if (!filepath.StartsWith(FOLDER_SEPARATOR))
				filepath = FOLDER_SEPARATOR + filepath;

			using (var connection = CurrentContext.CreateConnection())
			{
				var file = Find(filepath);
				if (file == null)
					throw new ArgumentException("file does not exist");

				var command = connection.CreateCommand(@"UPDATE files SET modified_on = @modified_on WHERE id = @id");
				command.Parameters.Add(new NpgsqlParameter("@id", Guid.NewGuid()));
				command.Parameters.Add(new NpgsqlParameter("@modified_on", modificationDate));
				command.ExecuteNonQuery();

				return Find(filepath);
			}
		}

		/// <summary>
		/// copy file from source to destination location
		/// </summary>
		/// <param name="sourceFilepath"></param>
		/// <param name="destinationFilepath"></param>
		/// <param name="overwrite"></param> 
		/// <returns></returns>
		public DbFile Copy(string sourceFilepath, string destinationFilepath, bool overwrite = false)
		{
			if (string.IsNullOrWhiteSpace(sourceFilepath))
				throw new ArgumentException("sourceFilepath cannot be null or empty");

			if (string.IsNullOrWhiteSpace(destinationFilepath))
				throw new ArgumentException("destinationFilepath cannot be null or empty");

			sourceFilepath = sourceFilepath.ToLowerInvariant();
			destinationFilepath = destinationFilepath.ToLowerInvariant();

			if (!sourceFilepath.StartsWith(FOLDER_SEPARATOR))
				sourceFilepath = FOLDER_SEPARATOR + sourceFilepath;

			if (!destinationFilepath.StartsWith(FOLDER_SEPARATOR))
				destinationFilepath = FOLDER_SEPARATOR + destinationFilepath;

			var srcFile = Find(sourceFilepath);
			var destFile = Find(destinationFilepath);

			if (srcFile == null)
				throw new Exception("Source file cannot be found.");

			if (destFile != null && overwrite == false)
				throw new Exception("Destination file already exists and no overwrite specified.");

			using (var connection = CurrentContext.CreateConnection())
			{
				try
				{
					connection.BeginTransaction();

					if (destFile != null && overwrite)
						Delete(destFile.FilePath);

					var bytes = srcFile.GetBytes(connection);
					var newFile = Create(destinationFilepath, bytes, srcFile.CreatedOn, srcFile.CreatedBy);

					connection.CommitTransaction();
					return newFile;
				}
				catch
				{
					connection.RollbackTransaction();
					throw;
				}
			}
		}

		/// <summary>
		/// moves file from source to destination location
		/// </summary>
		/// <param name="sourceFilepath"></param>
		/// <param name="destinationFilepath"></param>
		/// <param name="overwrite"></param>
		/// <returns></returns>
		public DbFile Move(string sourceFilepath, string destinationFilepath, bool overwrite = false)
		{
			if (string.IsNullOrWhiteSpace(sourceFilepath))
				throw new ArgumentException("sourceFilepath cannot be null or empty");

			if (string.IsNullOrWhiteSpace(destinationFilepath))
				throw new ArgumentException("destinationFilepath cannot be null or empty");

			sourceFilepath = sourceFilepath.ToLowerInvariant();
			destinationFilepath = destinationFilepath.ToLowerInvariant();

			if (!sourceFilepath.StartsWith(FOLDER_SEPARATOR))
				sourceFilepath = FOLDER_SEPARATOR + sourceFilepath;

			if (!destinationFilepath.StartsWith(FOLDER_SEPARATOR))
				destinationFilepath = FOLDER_SEPARATOR + destinationFilepath;

			var srcFile = Find(sourceFilepath);
			var destFile = Find(destinationFilepath);

			if (srcFile == null)
				throw new Exception("Source file cannot be found.");

			if (destFile != null && overwrite == false)
				throw new Exception("Destination file already exists and no overwrite specified.");

			using (var connection = CurrentContext.CreateConnection())
			{
				try
				{
					connection.BeginTransaction();

					if (destFile != null && overwrite)
						Delete(destFile.FilePath);

					var command = connection.CreateCommand(@"UPDATE files SET filepath = @filepath WHERE id = @id");
					command.Parameters.Add(new NpgsqlParameter("@id", srcFile.Id));
					command.Parameters.Add(new NpgsqlParameter("@filepath", destinationFilepath));
					command.ExecuteNonQuery();
					if(ErpSettings.EnableCloudBlobStorage)
					{
						var srcPath = StoragePath.Combine(StoragePath.RootFolderPath, sourceFilepath);
						var destinationPath = StoragePath.Combine(StoragePath.RootFolderPath, destinationFilepath);
						using (IBlobStorage storage = GetBlobStorage())
						{
							using (Stream original = storage.OpenReadAsync(srcPath).Result)
							{
								if (original != null)
								{
									storage.WriteAsync(destinationPath, original).Wait();
									storage.DeleteAsync(sourceFilepath).Wait();
								}
							}

						}
					} 
					else if (ErpSettings.EnableFileSystemStorage)
					{
						var srcFileName = Path.GetFileName(sourceFilepath);
						var destFileName = Path.GetFileName(destinationFilepath);
						if (srcFileName != destFileName)
						{
							var fsSrcFilePath = GetFileSystemPath(srcFile);
							srcFile.FilePath = destinationFilepath;
							var fsDestFilePath = GetFileSystemPath(srcFile);
							File.Move(fsSrcFilePath, fsDestFilePath);
						}
					}

					connection.CommitTransaction();
					return Find(destinationFilepath);
				}
				catch
				{
					connection.RollbackTransaction();
					throw;
				}
			}
		}


		/// <summary>
		/// deletes file
		/// </summary>
		/// <param name="filepath"></param>
		public void Delete(string filepath)
		{
			if (string.IsNullOrWhiteSpace(filepath))
				throw new ArgumentException("filepath cannot be null or empty");

			//all filepaths are lowercase and all starts with folder separator
			filepath = filepath.ToLowerInvariant();
			if (!filepath.StartsWith(FOLDER_SEPARATOR))
				filepath = FOLDER_SEPARATOR + filepath;

			var file = Find(filepath);

			if (file == null)
				return;

			using (var connection = CurrentContext.CreateConnection())
			{
				try
				{
					connection.BeginTransaction();
					if(ErpSettings.EnableCloudBlobStorage && file.ObjectId == 0)
					{
						var path = GetBlobPath(file);
						using (IBlobStorage storage = GetBlobStorage())
						{
							if (storage.ExistsAsync(path).Result)
							{
								storage.DeleteAsync(path).Wait();
							}
						}
					} else if (ErpSettings.EnableFileSystemStorage && file.ObjectId == 0)
					{
						var path = GetFileSystemPath(file);
						if( File.Exists(path))
							File.Delete(path);
					}
					else
					{
						if( file.ObjectId != 0 )
							new NpgsqlLargeObjectManager(connection.connection).Unlink(file.ObjectId);
					}

					var command = connection.CreateCommand(@"DELETE FROM files WHERE id = @id");
					command.Parameters.Add(new NpgsqlParameter("@id", file.Id));
					command.ExecuteNonQuery();

					connection.CommitTransaction();
				}
				catch
				{
					connection.RollbackTransaction();
					throw;
				}
			}
		}

		/// <summary>
		/// create temp file
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="extension"></param>
		/// <returns></returns>
		public DbFile CreateTempFile(string filename, byte[] buffer, string extension = null)
		{
			if (!string.IsNullOrWhiteSpace(extension))
			{
				extension = extension.Trim().ToLowerInvariant();
				if (!extension.StartsWith("."))
					extension = "." + extension;
			}

			string section = Guid.NewGuid().ToString().Replace("-", "").ToLowerInvariant();
			var tmpFilePath = FOLDER_SEPARATOR + TMP_FOLDER_NAME + FOLDER_SEPARATOR + section + FOLDER_SEPARATOR + filename + extension ?? string.Empty;
			return Create(tmpFilePath, buffer, DateTime.UtcNow, null);
		}

		/// <summary>
		/// cleanup expired temp files 
		/// </summary>
		/// <param name="expiration"></param>
		public void CleanupExpiredTempFiles(TimeSpan expiration)
		{

			DataTable table = new DataTable();
			using (var connection = CurrentContext.CreateConnection())
			{
				var command = connection.CreateCommand(string.Empty);
				command.CommandText = "SELECT filepath FROM files WHERE filepath ILIKE @tmp_path";
				command.Parameters.Add(new NpgsqlParameter("@tmp_path", "%" + FOLDER_SEPARATOR + TMP_FOLDER_NAME));
				new NpgsqlDataAdapter(command).Fill(table);
			}

			foreach (DataRow row in table.Rows)
				Delete((string)row["filepath"]);
		}

		internal static IBlobStorage GetBlobStorage(string overrideConnectionString = null)
		{
			return StorageFactory.Blobs.FromConnectionString(string.IsNullOrWhiteSpace(overrideConnectionString) ? ErpSettings.CloudBlobStorageConnectionString : overrideConnectionString);
		} 

		internal static string GetFileSystemPath(DbFile file)
		{
			var guidIinitialPart = file.Id.ToString().Split(new[] { '-' })[0];
			var fileName = file.FilePath.Split(new[] { '/' }).Last();
			var depth1Folder = guidIinitialPart.Substring(0, 2);
			var depth2Folder = guidIinitialPart.Substring(2, 2);
			// BUG: https://docs.microsoft.com/en-us/dotnet/api/system.io.path.getextension?view=net-5.0
			// Path.GetExtension includes the "." which means further on we are adding double "."
			// Would probably ruin too many databases to just fix here though
			string filenameExt = Path.GetExtension(fileName);

			if (!string.IsNullOrWhiteSpace(filenameExt))
				return Path.Combine(ErpSettings.FileSystemStorageFolder, depth1Folder, depth2Folder, file.Id + "." + filenameExt);

			else
				return Path.Combine(ErpSettings.FileSystemStorageFolder, depth1Folder, depth2Folder, file.Id.ToString());
		}


		internal static string GetBlobPath(DbFile file)
		{
			var guidIinitialPart = file.Id.ToString().Split(new[] { '-' })[0];
			var fileName = file.FilePath.Split(new[] { '/' }).Last();
			var depth1Folder = guidIinitialPart.Substring(0, 2);
			var depth2Folder = guidIinitialPart.Substring(2, 2);
			string filenameExt = Path.GetExtension(fileName);

			
			if (!string.IsNullOrWhiteSpace(filenameExt))
				return StoragePath.Combine(depth1Folder, depth2Folder, file.Id + filenameExt);
			else
				return StoragePath.Combine(depth1Folder, depth2Folder, file.Id.ToString());

		}

	}
}
