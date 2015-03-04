using System;
using System.Diagnostics;

namespace WebVella.ERP
{
    public class ERPService : IERPService
    {
        IStorageService storage;

        public ERPService( IStorageService storage )
        {
            this.storage= storage;
        }

        public void Run()
        {
            var entrep = storage.GetEntityRepository();
            IEntity entity = entrep.Get();
            Debug.WriteLine(entity.Name);
        }
    }
}