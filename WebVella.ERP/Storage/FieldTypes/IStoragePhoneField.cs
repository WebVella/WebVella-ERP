namespace WebVella.ERP.Storage
{
    public interface IStoragePhoneField : IStorageField
    {
        new string DefaultValue { get; set; }

        string Format { get; set; }

        int MaxLength { get; set; }

        string Value { get; set; }
    }
}