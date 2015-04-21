namespace WebVella.ERP.Storage
{
    public interface IStorageFileField : IStorageField
    {
        new string DefaultValue { get; set; }

        string Value { get; set; }
    }
}