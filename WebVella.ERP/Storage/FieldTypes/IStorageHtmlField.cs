namespace WebVella.ERP.Storage
{
    public interface IStorageHtmlField : IStorageField
    {
        string DefaultValue { get; set; }
    }
}