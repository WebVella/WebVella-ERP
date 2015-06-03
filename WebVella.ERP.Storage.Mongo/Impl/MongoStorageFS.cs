using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoStorageFS : IStorageFS
    {
        private const string FOLDER_SEPARATOR = "/";
        private const string TMP_FOLDER_NAME = "tmp";
        private MongoGridFS gfs = null;

        public MongoStorageFS()
        {
            gfs = new MongoGridFS(MongoStaticContext.Context.Server, MongoStaticContext.Context.Database.Name, new MongoGridFSSettings { Root = "files" });
        }

        /// <summary>
        /// find file by its filepath
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public IStorageFile Find(string filepath)
        {
            if (string.IsNullOrWhiteSpace(filepath))
                throw new ArgumentException("filepath cannot be null or empty");

            //all filepaths are lowercase and all starts with folder separator
            filepath = filepath.ToLowerInvariant();
            if (!filepath.StartsWith(FOLDER_SEPARATOR))
                filepath = FOLDER_SEPARATOR + filepath;

            var file = gfs.FindOne(filepath);

            if (file == null)
                return null;

            return new MongoStorageFile(file);
        }

        /// <summary>
        /// finds all files 
        /// </summary>
        /// <param name="startsWithPath"></param>
        /// <param name="includeTempFiles"></param>
        /// <param name="skip"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<IStorageFile> FindAll(string startsWithPath = null, bool includeTempFiles = false, int? skip = null, int? limit = null)
        {
            //all filepaths are lowercase and all starts with folder separator
            if (!string.IsNullOrWhiteSpace(startsWithPath))
            {
                startsWithPath = startsWithPath.ToLowerInvariant();

                if (!startsWithPath.StartsWith(FOLDER_SEPARATOR))
                    startsWithPath = FOLDER_SEPARATOR + startsWithPath;
            }

            List<IMongoQuery> queries = new List<IMongoQuery>();

            if (!includeTempFiles)
            {
                IMongoQuery tmpFolderQuery = Query.Matches("filename", new BsonRegularExpression("^(/" + TMP_FOLDER_NAME + "/)", "i"));
                IMongoQuery tmpFolderExclideQuery = Query.Not(tmpFolderQuery);
                queries.Add(tmpFolderExclideQuery);
            }

            if (!string.IsNullOrWhiteSpace(startsWithPath))
            {
                //TODO change to starts with - now is contains
                var regex = new BsonRegularExpression(string.Format(".*{0}.*", startsWithPath), "i");
                queries.Add(Query.Matches("filename", regex));
            }

            IMongoQuery finalQuery = queries.Count > 0 ? Query.And(queries) : Query.Null;
            var cursor = gfs.Find(finalQuery);

            if (skip.HasValue)
                cursor.SetSkip(skip.Value);

            if (limit.HasValue)
                cursor.SetLimit(limit.Value);

            IEnumerable<MongoGridFSFileInfo> files = cursor.ToList();
            return cursor.AsQueryable().Select(f => (IStorageFile)(new MongoStorageFile(f))).ToList();
        }

        /// <summary>
        /// create file
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="buffer"></param>
        /// <param name="ownerId"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public IStorageFile Create(string filepath, byte[] buffer, DateTime? modificationDate, Guid? ownerId = null, List<Guid> roles = null)
        {
            if (string.IsNullOrWhiteSpace(filepath))
                throw new ArgumentException("filepath cannot be null or empty");

            //all filepaths are lowercase and all starts with folder separator
            filepath = filepath.ToLowerInvariant();
            if (!filepath.StartsWith(FOLDER_SEPARATOR))
                filepath = FOLDER_SEPARATOR + filepath;

            if( Find(filepath) != null )
                throw new ArgumentException( filepath + ": file already exists");

            BsonDocument metadata = new BsonDocument();
            metadata.Add(new BsonElement("owner_id", ownerId));
            BsonArray rolesArr = new BsonArray();
            if( roles != null && roles.Count > 0 )
            {
                foreach( var roleId in roles )
                    rolesArr.Add(BsonValue.Create(roleId));
            }
            metadata.Add(new BsonElement("available_to_roles", rolesArr ));

            if (modificationDate == null)
                modificationDate = DateTime.UtcNow;

            using (var stream = gfs.OpenWrite(filepath, new MongoGridFSCreateOptions { UploadDate = modificationDate.Value, Metadata = metadata }))
            {
                if (buffer != null && buffer.Length > 0)
                    stream.Write(buffer, 0, buffer.Length);
            }
            return Find(filepath);
        }

        /// <summary>
        /// update last modification date
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="modificationDate"></param>
        /// <returns></returns>
        public IStorageFile UpdateModificationDate(string filepath, DateTime modificationDate)
        {
            if (string.IsNullOrWhiteSpace(filepath))
                throw new ArgumentException("filepath cannot be null or empty");

            //all filepaths are lowercase and all starts with folder separator
            filepath = filepath.ToLowerInvariant();
            if (!filepath.StartsWith(FOLDER_SEPARATOR))
                filepath = FOLDER_SEPARATOR + filepath;

            var transaction = MongoStaticContext.Context.CreateTransaction();
            try {
                var file = Find(filepath);
                if (file == null)
                    throw new ArgumentException("file does not exist");

                byte[] buffer = file.GetBytes();
                Delete(file.FilePath);
                var newFile = Create(filepath, buffer, modificationDate, file.OwnerId, file.AvailableToRoles);

                transaction.Commit();

                return newFile;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// update security attributes
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="ownerId"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public IStorageFile UpdateSecurityInfo(string filepath, Guid? ownerId, List<Guid> roles )
        {
            if (string.IsNullOrWhiteSpace(filepath))
                throw new ArgumentException("filepath cannot be null or empty");

            //all filepaths are lowercase and all starts with folder separator
            filepath = filepath.ToLowerInvariant();
            if (!filepath.StartsWith(FOLDER_SEPARATOR))
                filepath = FOLDER_SEPARATOR + filepath;

            var file = gfs.FindOne(filepath);
            if (file == null)
                throw new ArgumentException("file does not exist.");

            BsonDocument metadata = new BsonDocument();
            metadata.Add(new BsonElement("owner_id", ownerId));
            BsonArray rolesArr = new BsonArray();
            if (roles != null && roles.Count > 0)
            {
                foreach (var roleId in roles)
                    rolesArr.Add(BsonValue.Create(roleId));
            }
            metadata.Add(new BsonElement("available_to_roles", rolesArr));
            gfs.SetMetadata(file, metadata);

            return Find(filepath);
        }

        /// <summary>
        /// copy file from source to destination location
        /// </summary>
        /// <param name="sourceFilepath"></param>
        /// <param name="destinationFilepath"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public IStorageFile Copy(string sourceFilepath, string destinationFilepath, bool overwrite = false)
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

            if( destFile != null && overwrite == false )
                throw new Exception("Destination file already exists and no overwrite specified.");

            var transaction = MongoStaticContext.Context.CreateTransaction();
            try
            {
                byte[] buffer = srcFile.GetBytes();

                if (destFile != null && overwrite)
                    Delete(destFile.FilePath);

                var newFile = Create(destinationFilepath, buffer, srcFile.LastModificationDate, srcFile.OwnerId, srcFile.AvailableToRoles);

                transaction.Commit();
                return newFile;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// moves file from source to destination location
        /// </summary>
        /// <param name="sourceFilepath"></param>
        /// <param name="destinationFilepath"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public IStorageFile Move(string sourceFilepath, string destinationFilepath, bool overwrite = false)
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

            var transaction = MongoStaticContext.Context.CreateTransaction();
            try
            {
                //create new file
                byte[] buffer = srcFile.GetBytes();
                var newFile = Create(destinationFilepath, buffer, srcFile.LastModificationDate, srcFile.OwnerId, srcFile.AvailableToRoles);

                //delete old one
                Delete(sourceFilepath);

                transaction.Commit();
                return newFile;
            }
            catch
            {
                transaction.Rollback();
                throw;
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

            var sourceQuery = Query.EQ("filename", filepath);
            if (gfs.Exists(sourceQuery))
                gfs.Delete(sourceQuery);
        }

        /// <summary>
        /// create temp file
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public IStorageFile CreateTempFile(byte[] buffer, string extension = null)
        {
            if (!string.IsNullOrWhiteSpace(extension))
            {
                extension = extension.Trim().ToLowerInvariant();
                if (!extension.StartsWith("."))
                    extension = "." + extension;
            }

            string filename = ObjectId.GenerateNewId().ToString().ToLowerInvariant();

            var tmpFilePath = FOLDER_SEPARATOR + TMP_FOLDER_NAME + FOLDER_SEPARATOR + filename + extension ?? string.Empty;
            using (var stream = gfs.OpenWrite(tmpFilePath, new MongoGridFSCreateOptions { UploadDate = DateTime.UtcNow }))
            {
                stream.Write(buffer, 0, buffer.Length);
            }
            return Find(tmpFilePath);
        }

        /// <summary>
        /// cleanup expired temp files 
        /// </summary>
        /// <param name="expiration"></param>
        public void CleanupExpiredTempFiles(TimeSpan expiration)
        {
            var regexQuery = Query.Matches("filename", new BsonRegularExpression("^(/" + TMP_FOLDER_NAME + "/)", "i" ));
            var query = Query.And(regexQuery, Query.LT("uploadDate", DateTime.UtcNow.Subtract(expiration)));
            gfs.Delete(query);
        }
    }
}
