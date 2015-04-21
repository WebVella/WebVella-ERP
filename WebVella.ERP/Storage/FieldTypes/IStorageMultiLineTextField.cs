namespace WebVella.ERP.Storage
{
    public interface IStorageMultiLineTextField : IStorageField
    {
        new string DefaultValue { get; set; }

        int LineNumber { get; set; }

        string Value { get; set; }
    }
}