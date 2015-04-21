using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace WebVella.ERP
{
    public class ERPService : IERPService
    {
        public static IERPService Current
        {
            get; set;
        }

        public IStorageService StorageService
        {
            get; set;
        }

        public ERPService( IStorageService storage )
        {
            if (Current == null)
                Current = this;

            StorageService = storage;
        }

        public void RunTests()
        {
            EntityTests();
        }

        private void EntityTests()
        {
            Debug.WriteLine("==== START ENTITY TESTS====");
            var repository = StorageService.GetEntityRepository();

            IEntity entity = repository.Empty();
            entity.Id = new Guid("C5050AC8-5967-4CE1-95E7-A79B054F9D14");
            entity.Name = "This is an initial test entity.";
            entity.Fields = new List<IField>();

            try
            {
                repository.Create(entity);
                repository.Update(entity);
                repository.Delete(entity.Id);
            }
            catch (StorageException e)
            {
                Debug.WriteLine(e);
            }

            Debug.WriteLine("==== END ENTITY TESTS====");
        }

    }
}