namespace WebVella.ERP.Storage
{
    public interface IStorageImageField : IStorageField
    {
        new string DefaultValue { get; set; }

        string TargetEntityType { get; set; }

        string RelationshipName { get; set; }
    }
}