namespace WebVella.ERP.Storage
{
    public interface IStorageFileField : IStorageField
    {
        string DefaultValue { get; set; }
    }
}