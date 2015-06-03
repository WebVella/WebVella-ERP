using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoStorageFile : IStorageFile
    {
        private Guid? ownerId = null;
        private List<Guid> availableToRoles = null;
        private readonly MongoGridFSFileInfo fileInfo;

        public string FilePath { get { return fileInfo.Name; } }
        public string MD5 { get { return fileInfo.MD5; } }
        public DateTime LastModificationDate { get { return fileInfo.UploadDate; } }

        public Guid? OwnerId
        {
            get
            {
                if (fileInfo.Metadata != null && fileInfo.Metadata.Names.Any(x => x == "owner_id"))
                    ownerId = (Guid?)fileInfo.Metadata["owner_id"];

                return ownerId;
            }
        }
        public List<Guid> AvailableToRoles
        {
            get
            {
                if (fileInfo.Metadata != null && fileInfo.Metadata.Names.Any(x => x == "available_to_roles"))
                    availableToRoles = fileInfo.Metadata["available_to_roles"].AsBsonArray.Select(roleId => (Guid)roleId).ToList();

                return availableToRoles;
            }
        }

        internal MongoStorageFile(MongoGridFSFileInfo fileInfo )
        {
            if (fileInfo == null)
                throw new ArgumentNullException("fileInfo");

            this.fileInfo = fileInfo;
        }

        public Stream GetContentStream()
        {
            return fileInfo.OpenRead();
        }

        public byte[] GetBytes()
        {
            var contentStream = GetContentStream();
            return contentStream.Length == 0 ? null : new BinaryReader(contentStream).ReadBytes((int)contentStream.Length);
        }
    }
}
