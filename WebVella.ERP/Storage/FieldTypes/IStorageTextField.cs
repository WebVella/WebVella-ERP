namespace WebVella.ERP.Storage
{
    public interface IStorageTextField : IStorageField
    {
        string DefaultValue { get; set; }

        int? MaxLength { get; set; }
    }
}