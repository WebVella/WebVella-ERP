namespace WebVella.ERP.Storage
{
    public interface IStorageService
    {
        IStorageSystemSettingsRepository GetSystemSettingsRepository();
        IStorageEntityRepository GetEntityRepository();
        IStorageRecordRepository GetRecordRepository();
    }
}