namespace WebVella.ERP.Storage
{
    public interface IStorageNumberField : IStorageField
    {
        decimal DefaultValue { get; set; }

        decimal MinValue { get; set; }

        decimal MaxValue { get; set; }

        byte DecimalPlaces { get; set; }
    }
}