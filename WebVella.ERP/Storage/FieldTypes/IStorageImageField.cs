namespace WebVella.ERP.Storage
{
    public interface IStorageImageField : IStorageField
    {
        string DefaultValue { get; set; }

    }
}