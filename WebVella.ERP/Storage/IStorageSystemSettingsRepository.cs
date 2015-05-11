using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Storage
{
    public interface IStorageSystemSettingsRepository : IStorageRepository
    {
        IStorageSystemSettings Convert(SystemSettings systemSettings);
        IStorageSystemSettings Read();
        bool Save(IStorageSystemSettings entity);
    }
}
