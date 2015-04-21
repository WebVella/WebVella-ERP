namespace WebVella.ERP.Storage
{
    public interface IStorageHtmlField : IStorageField
    {
        new string DefaultValue { get; set; }

        string Value { get; set; }
    }
}