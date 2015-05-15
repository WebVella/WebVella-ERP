namespace WebVella.ERP.Storage
{
    public interface IStorageEmailField : IStorageField
    {
        string DefaultValue { get; set; }

        int? MaxLength { get; set; }
    }
}