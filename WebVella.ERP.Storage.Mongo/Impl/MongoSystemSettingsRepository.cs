using System;
using System.Collections.Generic;
using System.Linq;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Storage.Mongo
{
    public class MongoSystemSettingsRepository : IStorageSystemSettingsRepository
    {
        public IStorageSystemSettings Convert(SystemSettings systemSettings)
        {
            MongoSystemSettings storageSystemSettings = new MongoSystemSettings();

            storageSystemSettings.Id = systemSettings.Id;
            storageSystemSettings.Version = systemSettings.Version;

            return storageSystemSettings;
        }

        /// <summary>
        /// Read system setting
        /// </summary>
        /// <returns></returns>
        public IStorageSystemSettings Read()
        {
            List<IStorageSystemSettings> settings = MongoStaticContext.Context.SystemSettings.Get().ToList<IStorageSystemSettings>();

            IStorageSystemSettings setting = null;
            if (settings != null && settings.Count > 0)
                setting = settings[0];

            return setting;
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
