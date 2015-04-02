
using System;

namespace WebVella.ERP.Core
{
    public enum MaskTypes
    {
        None,
        MaskAllCharacters,
        LastFourCharactersClear,
        CreditCardNumber,
        NationalInsuranceNumber,
        SocialSecurityNumber,
        SocialInsuranceNumber
    }

    public interface IPasswordField : IField
    {
        new string DefaultValue { get; set; }

        int MaxLength { get; set; }

        MaskTypes MaskType { get; set; }

        char MaskCharacter { get; set; }

        string Value { get; set; }
    }
}