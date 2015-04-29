namespace WebVella.ERP.Storage
{
    public interface IStorageTextField : IStorageField
    {
        new string DefaultValue { get; set; }

        int MaxLength { get; set; }
    }
}