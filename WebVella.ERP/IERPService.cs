using System;

namespace WebVella.ERP
{
    public interface IERPService
    {
        IStorageService StorageService { get; set; }
        void RunTests();
    }
}