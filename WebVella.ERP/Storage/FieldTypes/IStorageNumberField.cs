namespace WebVella.ERP.Storage
{
    public interface IStorageNumberField : IStorageField
    {
        new decimal DefaultValue { get; set; }

        decimal MinValue { get; set; }

        decimal MaxValue { get; set; }

        byte DecimalPlaces { get; set; }
    }
}