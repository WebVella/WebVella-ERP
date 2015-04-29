using WebVella.ERP.Api;

namespace WebVella.ERP.Storage
{

    public interface IStoragePasswordField : IStorageField
    {
        new string DefaultValue { get; set; }

        int MaxLength { get; set; }

        PasswordFieldMaskTypes MaskType { get; set; }

        char MaskCharacter { get; set; }
    }
}