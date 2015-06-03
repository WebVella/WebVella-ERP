namespace WebVella.ERP.Storage
{
    public interface IStorageService
    {
        IStorageObjectFactory GetObjectFactory();
        IStorageSystemSettingsRepository GetSystemSettingsRepository();
        IStorageEntityRelationRepository GetEntityRelationRepository();
        IStorageEntityRepository GetEntityRepository();
        IStorageRecordRepository GetRecordRepository();
        IStorageFS GetFS();
    }
}