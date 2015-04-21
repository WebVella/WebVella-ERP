namespace WebVella.ERP.Storage
{
    public interface IStorageNumberField : IStorageField
    {
        new decimal DefaultValue { get; set; }

        int MinValue { get; set; }

        int MaxValue { get; set; }

        decimal Value { get; set; }
    }
}