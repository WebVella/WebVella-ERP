using System;
using System.Collections.Generic;

namespace WebVella.ERP.Storage
{
    public interface IStorageFS
    {
        IStorageFile Find(string filepath);
        List<IStorageFile> FindAll(string filepath, bool includeTempFiles = false, int? skip = null, int? limit = null);
        IStorageFile Create(string filepath, byte[] buffer, DateTime? modificationDate = null, Guid? ownerId = null, List<Guid> roles = null);
        IStorageFile UpdateModificationDate(string filepath, DateTime createdOn);
        IStorageFile UpdateSecurityInfo(string filepath, Guid? ownerId = null, List<Guid> roles = null);
        IStorageFile Copy(string sourceFilepath, string destinationFilepath, bool overwrite = false);
        IStorageFile Move(string sourceFilepath, string destinationFilepath, bool overwrite = false);
        void Delete(string filepath);

        IStorageFile CreateTempFile(string filename, byte[] buffer, string extension = null);
        void CleanupExpiredTempFiles(TimeSpan expirationPeriod);
    }
}
