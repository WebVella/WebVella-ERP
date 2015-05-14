namespace WebVella.ERP.Storage
{
    public interface IStorageService
    {
        IStorageSystemSettingsRepository GetSystemSettingsRepository();
        IStorageEntityRelationRepository GetEntityRelationRepository();
        IStorageEntityRepository GetEntityRepository();
        IStorageRecordRepository GetRecordRepository();
    }
}