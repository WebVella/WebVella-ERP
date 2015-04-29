namespace WebVella.ERP.Storage
{
    public interface IStorageMultiLineTextField : IStorageField
    {
        new string DefaultValue { get; set; }

        int MaxLength { get; set; }

        int VisibleLineNumber { get; set; }
    }
}