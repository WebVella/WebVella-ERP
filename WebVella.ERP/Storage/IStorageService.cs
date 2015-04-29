namespace WebVella.ERP.Storage
{
    public interface IStorageService
    {
        IStorageEntityRepository GetEntityRepository();
        IStorageRecordRepository GetRecordRepository();
    }
}