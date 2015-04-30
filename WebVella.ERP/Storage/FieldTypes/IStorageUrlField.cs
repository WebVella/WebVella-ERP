namespace WebVella.ERP.Storage
{
    public interface IStorageUrlField : IStorageField
    {
        new string DefaultValue { get; set; }

        int MaxLength { get; set; }

        bool OpenTargetInNewWindow { get; set; }
    }
}
