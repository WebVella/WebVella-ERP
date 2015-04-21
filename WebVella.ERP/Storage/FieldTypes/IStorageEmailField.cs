namespace WebVella.ERP.Storage
{
    public interface IStorageEmailField : IStorageField
    {
        new string DefaultValue { get; set; }

        int MaxLength { get; set; }

        string Value { get; set; }
    }
}