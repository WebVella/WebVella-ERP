using WebVella.ERP.Api;

namespace WebVella.ERP.Storage
{

    public interface IStoragePasswordField : IStorageField
    {
        int? MaxLength { get; set; }

        bool Encrypted { get; set; }

        PasswordFieldMaskTypes MaskType { get; set; }
    }
}