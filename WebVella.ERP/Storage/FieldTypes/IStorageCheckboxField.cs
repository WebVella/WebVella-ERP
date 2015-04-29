namespace WebVella.ERP.Storage
{
    public interface IStorageCheckboxField : IStorageField
    {
        new bool DefaultValue { get; set; }
    }
}