using System;

namespace WebVella.ERP.Api
{
    public enum ViewTypes
    {
        SearchPopup = 1,
        List,
        Custom
    }

    public enum FilterOperatorTypes
    {
        Equals = 1,
        NotEqualTo,
        StartsWith,
        Contains,
        DoesNotContain,
        LessThan,
        GreaterThan,
        LessOrEqual,
        GreaterOrEqual,
        Includes,
        Excludes,
        Within
    }

    public enum FormLayouts
    {
        OneColumn = 1,
        TwoColumns
    }

    public enum FormColumns
    {
        Left = 1,
        Right
    }

    public enum CurrencyPosition
    {
        BeforeTheNumber,
        AfterTheNumber
    }

    public class CurrencyTypes
    {
        public string CurrencySymbol { get; set; }
        public string CurrencyName { get; set; }
        public CurrencyPosition Position { get; set; }
    }

    public enum FormulaFieldReturnType
    {
        Checkbox,
        Currency,
        Date,
        DateTime,
        Number,
        Percent,
        Text
    }

    public enum PasswordFieldMaskTypes
    {
        None,
        MaskAllCharacters,
        LastFourCharactersClear,
        CreditCardNumber,
        NationalInsuranceNumber,
        SocialSecurityNumber,
        SocialInsuranceNumber
    }

}