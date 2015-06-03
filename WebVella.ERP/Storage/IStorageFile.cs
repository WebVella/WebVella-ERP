using System;
using System.Collections.Generic;
using System.IO;

namespace WebVella.ERP.Storage
{
    public interface IStorageFile
    {
        string FilePath { get; }
        string MD5 { get; }
        DateTime LastModificationDate { get; }
        Guid? OwnerId { get;  }
        List<Guid> AvailableToRoles { get; }
        Stream GetContentStream();
        byte[] GetBytes();
    }
}
