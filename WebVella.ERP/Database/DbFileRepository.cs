//using System;
//using System.Collections.Generic;
//using Npgsql;
//using System.Data;

//namespace WebVella.ERP.Database
//{
//	public class DbFileRepository
//	{
//		private const string FOLDER_SEPARATOR = "/";
//		private const string TMP_FOLDER_NAME = "tmp";

//		public DbFile Find(string filepath)
//		{
//			if (string.IsNullOrWhiteSpace(filepath))
//				throw new ArgumentException("filepath cannot be null or empty");

//			//all filepaths are lowercase and all starts with folder separator
//			filepath = filepath.ToLowerInvariant();
//			if (!filepath.StartsWith(FOLDER_SEPARATOR))
//				filepath = FOLDER_SEPARATOR + filepath;

//			using (var connection = DbContext.Current.CreateConnection())
//			{
//				var command = connection.CreateCommand("SELECT * FROM files WHERE filepath = @filepath ");
//				command.Parameters.Add(new NpgsqlParameter("@filepath", filepath));
//				DataTable dataTable = new DataTable();
//				new NpgsqlDataAdapter(command).Fill(dataTable);

//				if (dataTable.Rows.Count == 1)
//					return new DbFile(dataTable.Rows[0]);
//			}

//			return null;
//		}

//		public List<DbFile> FindAll(string startsWithPath = null, bool includeTempFiles = false, int? skip = null, int? limit = null)
//		{
//			//all filepaths are lowercase and all starts with folder separator
//			if (!string.IsNullOrWhiteSpace(startsWithPath))
//			{
//				startsWithPath = startsWithPath.ToLowerInvariant();

//				if (!startsWithPath.StartsWith(FOLDER_SEPARATOR))
//					startsWithPath = FOLDER_SEPARATOR + startsWithPath;
//			}

//			string sql = "SELECT * FROM files WHERE ";

//			//parameter.Value = parameter.Value + "%";
//			//sql = sql + " " + completeFieldName + " ILIKE " + paramName;

//			string sqlExcludeTmpFiles = string.Empty;
//			NpgsqlParameter excludeTmpFilesParam = new NpgsqlParameter("@tmp_path", "%" + FOLDER_SEPARATOR + TMP_FOLDER_NAME);
//			if (!includeTempFiles)
//			{
//				sqlExcludeTmpFiles = "filepath NOT ILIKE @tmp_path";
//			}

//			if (!string.IsNullOrWhiteSpace(startsWithPath))
//			{
				
//			}

//			//IMongoQuery finalQuery = queries.Count > 0 ? Query.And(queries) : Query.Null;
//			//var cursor = gfs.Find(finalQuery);

//			//if (skip.HasValue)
//			//	cursor.SetSkip(skip.Value);

//			//if (limit.HasValue)
//			//	cursor.SetLimit(limit.Value);

//			//IEnumerable<MongoGridFSFileInfo> files = cursor.ToList();
//			//return cursor.AsQueryable().Select(f => (IStorageFile)(new MongoStorageFile(f))).ToList();

//			throw new NotImplementedException();
//		}

//		public DbFile Create(string filepath, byte[] buffer, DateTime? modificationDate)
//		{
//			//if (string.IsNullOrWhiteSpace(filepath))
//			//	throw new ArgumentException("filepath cannot be null or empty");

//			////all filepaths are lowercase and all starts with folder separator
//			//filepath = filepath.ToLowerInvariant();
//			//if (!filepath.StartsWith(FOLDER_SEPARATOR))
//			//	filepath = FOLDER_SEPARATOR + filepath;

//			//if (Find(filepath) != null)
//			//	throw new ArgumentException(filepath + ": file already exists");

//			//BsonDocument metadata = new BsonDocument();
//			//metadata.Add(new BsonElement("owner_id", ownerId));
//			//BsonArray rolesArr = new BsonArray();
//			//if (roles != null && roles.Count > 0)
//			//{
//			//	foreach (var roleId in roles)
//			//		rolesArr.Add(BsonValue.Create(roleId));
//			//}
//			//metadata.Add(new BsonElement("available_to_roles", rolesArr));

//			//if (modificationDate == null)
//			//	modificationDate = DateTime.UtcNow;

//			//using (var stream = gfs.OpenWrite(filepath, new MongoGridFSCreateOptions { UploadDate = modificationDate.Value, Metadata = metadata }))
//			//{
//			//	if (buffer != null && buffer.Length > 0)
//			//		stream.Write(buffer, 0, buffer.Length);
//			//}
//			//return Find(filepath);

//			throw new NotImplementedException();
//		}

//		public DbFile UpdateModificationDate(string filepath, DateTime modificationDate)
//		{
//			//if (string.IsNullOrWhiteSpace(filepath))
//			//	throw new ArgumentException("filepath cannot be null or empty");

//			////all filepaths are lowercase and all starts with folder separator
//			//filepath = filepath.ToLowerInvariant();
//			//if (!filepath.StartsWith(FOLDER_SEPARATOR))
//			//	filepath = FOLDER_SEPARATOR + filepath;

//			//var transaction = MongoStaticContext.Context.CreateTransaction();
//			//try
//			//{
//			//	var file = Find(filepath);
//			//	if (file == null)
//			//		throw new ArgumentException("file does not exist");

//			//	byte[] buffer = file.GetBytes();
//			//	Delete(file.FilePath);
//			//	var newFile = Create(filepath, buffer, modificationDate, file.OwnerId, file.AvailableToRoles);

//			//	transaction.Commit();

//			//	return newFile;
//			//}
//			//catch
//			//{
//			//	transaction.Rollback();
//			//	throw;
//			//}

//			throw new NotImplementedException();
//		}

//		/// <summary>
//		/// copy file from source to destination location
//		/// </summary>
//		/// <param name="sourceFilepath"></param>
//		/// <param name="destinationFilepath"></param>
//		/// <param name="overwrite"></param>
//		/// <returns></returns>
//		public DbFile Copy(string sourceFilepath, string destinationFilepath, bool overwrite = false)
//		{
//			//if (string.IsNullOrWhiteSpace(sourceFilepath))
//			//	throw new ArgumentException("sourceFilepath cannot be null or empty");

//			//if (string.IsNullOrWhiteSpace(destinationFilepath))
//			//	throw new ArgumentException("destinationFilepath cannot be null or empty");

//			//sourceFilepath = sourceFilepath.ToLowerInvariant();
//			//destinationFilepath = destinationFilepath.ToLowerInvariant();

//			//if (!sourceFilepath.StartsWith(FOLDER_SEPARATOR))
//			//	sourceFilepath = FOLDER_SEPARATOR + sourceFilepath;

//			//if (!destinationFilepath.StartsWith(FOLDER_SEPARATOR))
//			//	destinationFilepath = FOLDER_SEPARATOR + destinationFilepath;

//			//var srcFile = Find(sourceFilepath);
//			//var destFile = Find(destinationFilepath);

//			//if (srcFile == null)
//			//	throw new Exception("Source file cannot be found.");

//			//if (destFile != null && overwrite == false)
//			//	throw new Exception("Destination file already exists and no overwrite specified.");

//			//var transaction = MongoStaticContext.Context.CreateTransaction();
//			//try
//			//{
//			//	byte[] buffer = srcFile.GetBytes();

//			//	if (destFile != null && overwrite)
//			//		Delete(destFile.FilePath);

//			//	var newFile = Create(destinationFilepath, buffer, srcFile.LastModificationDate, srcFile.OwnerId, srcFile.AvailableToRoles);

//			//	transaction.Commit();
//			//	return newFile;
//			//}
//			//catch
//			//{
//			//	transaction.Rollback();
//			//	throw;
//			//}

//			throw new NotImplementedException();
//		}

//		/// <summary>
//		/// moves file from source to destination location
//		/// </summary>
//		/// <param name="sourceFilepath"></param>
//		/// <param name="destinationFilepath"></param>
//		/// <param name="overwrite"></param>
//		/// <returns></returns>
//		public DbFile Move(string sourceFilepath, string destinationFilepath, bool overwrite = false)
//		{
//			//if (string.IsNullOrWhiteSpace(sourceFilepath))
//			//	throw new ArgumentException("sourceFilepath cannot be null or empty");

//			//if (string.IsNullOrWhiteSpace(destinationFilepath))
//			//	throw new ArgumentException("destinationFilepath cannot be null or empty");

//			//sourceFilepath = sourceFilepath.ToLowerInvariant();
//			//destinationFilepath = destinationFilepath.ToLowerInvariant();

//			//if (!sourceFilepath.StartsWith(FOLDER_SEPARATOR))
//			//	sourceFilepath = FOLDER_SEPARATOR + sourceFilepath;

//			//if (!destinationFilepath.StartsWith(FOLDER_SEPARATOR))
//			//	destinationFilepath = FOLDER_SEPARATOR + destinationFilepath;

//			//var srcFile = Find(sourceFilepath);
//			//var destFile = Find(destinationFilepath);

//			//if (srcFile == null)
//			//	throw new Exception("Source file cannot be found.");

//			//if (destFile != null && overwrite == false)
//			//	throw new Exception("Destination file already exists and no overwrite specified.");

//			//var transaction = MongoStaticContext.Context.CreateTransaction();
//			//try
//			//{
//			//	if (overwrite)
//			//		Delete(destinationFilepath);

//			//	//create new file
//			//	byte[] buffer = srcFile.GetBytes();
//			//	var newFile = Create(destinationFilepath, buffer, srcFile.LastModificationDate, srcFile.OwnerId, srcFile.AvailableToRoles);

//			//	//delete old one
//			//	Delete(sourceFilepath);

//			//	transaction.Commit();
//			//	return newFile;
//			//}
//			//catch
//			//{
//			//	transaction.Rollback();
//			//	throw;
//			//}

//			throw new NotImplementedException();
//		}


//		/// <summary>
//		/// deletes file
//		/// </summary>
//		/// <param name="filepath"></param>
//		public void Delete(string filepath)
//		{
//			//if (string.IsNullOrWhiteSpace(filepath))
//			//	throw new ArgumentException("filepath cannot be null or empty");

//			////all filepaths are lowercase and all starts with folder separator
//			//filepath = filepath.ToLowerInvariant();
//			//if (!filepath.StartsWith(FOLDER_SEPARATOR))
//			//	filepath = FOLDER_SEPARATOR + filepath;

//			//var sourceQuery = Query.EQ("filename", filepath);
//			//if (gfs.Exists(sourceQuery))
//			//	gfs.Delete(sourceQuery);

//			throw new NotImplementedException();
//		}

//		/// <summary>
//		/// create temp file
//		/// </summary>
//		/// <param name="buffer"></param>
//		/// <param name="extension"></param>
//		/// <returns></returns>
//		public DbFile CreateTempFile(string filename, byte[] buffer, string extension = null)
//		{
//			//if (!string.IsNullOrWhiteSpace(extension))
//			//{
//			//	extension = extension.Trim().ToLowerInvariant();
//			//	if (!extension.StartsWith("."))
//			//		extension = "." + extension;
//			//}

//			//string section = ObjectId.GenerateNewId().ToString().ToLowerInvariant();
//			//var tmpFilePath = FOLDER_SEPARATOR + TMP_FOLDER_NAME + FOLDER_SEPARATOR + section + FOLDER_SEPARATOR + filename + extension ?? string.Empty;
//			//using (var stream = gfs.OpenWrite(tmpFilePath, new MongoGridFSCreateOptions { UploadDate = DateTime.UtcNow }))
//			//{
//			//	stream.Write(buffer, 0, buffer.Length);
//			//}
//			//return Find(tmpFilePath);

//			throw new NotImplementedException();
//		}

//		/// <summary>
//		/// cleanup expired temp files 
//		/// </summary>
//		/// <param name="expiration"></param>
//		public void CleanupExpiredTempFiles(TimeSpan expiration)
//		{
//			//var regexQuery = Query.Matches("filename", new BsonRegularExpression("^(/" + TMP_FOLDER_NAME + "/)", "i"));
//			//var query = Query.And(regexQuery, Query.LT("uploadDate", DateTime.UtcNow.Subtract(expiration)));
//			//gfs.Delete(query);

//			throw new NotImplementedException();
//		}

//	}
//}
