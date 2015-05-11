using System;
using System.Linq;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoSystemSettingsRepository : IStorageSystemSettingsRepository
    {
        public IStorageSystemSettings Convert(SystemSettings systemSettings)
        {
            MongoSystemSettings storageSystemSettings = new MongoSystemSettings();

            storageSystemSettings.Version = systemSettings.Version;

            return storageSystemSettings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IStorageSystemSettings Read()
        {
            return MongoStaticContext.Context.SystemSettings.Get().ToList<IStorageSystemSettings>()[0];
        }

        /// <summary>
        /// Saves system settings document
        /// </summary>
        /// <param name="entity"></param>
        public bool Save(IStorageSystemSettings systemSettings)
        {
            if (systemSettings == null)
                throw new ArgumentNullException("systemSettings");

            var mongoSystemSettings = systemSettings as MongoSystemSettings;

            if (mongoSystemSettings == null)
                throw new Exception("The specified object is not mongo storage object.");

            return MongoStaticContext.Context.SystemSettings.Save(mongoSystemSettings);
        }
    }
}
