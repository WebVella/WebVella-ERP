using WebVella.ERP.Storage;

namespace WebVella.ERP
{
    public interface IErpService
    {
        IStorageService StorageService { get; set; }
        void RunTests();
        void InitializeSystemEntities();
    }
}