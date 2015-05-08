namespace WebVella.ERP.Storage
{
    public interface IStorageCheckboxField : IStorageField
    {
        bool DefaultValue { get; set; }
    }
}