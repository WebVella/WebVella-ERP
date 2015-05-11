using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.ERP.Storage
{
    public interface IStorageSystemSettings : IStorageObject
    {
        int Version { get; set; }
    }
}
