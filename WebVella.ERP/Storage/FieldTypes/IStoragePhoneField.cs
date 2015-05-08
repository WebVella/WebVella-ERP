namespace WebVella.ERP.Storage
{
    public interface IStoragePhoneField : IStorageField
    {
        string DefaultValue { get; set; }

        string Format { get; set; }

        int MaxLength { get; set; }
    }
}