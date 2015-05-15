using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api
{
    public class SystemIds
    {
        public static Guid SystemEntityId { get { return new Guid("A5050AC8-5967-4CE1-95E7-A79B054F9D14"); } }
        public static Guid UserEntityId { get { return new Guid("B9CEBC3B-6443-452A-8E34-B311A73DCC8B"); } }
        public static Guid RoleEntityId { get { return new Guid("C4541FEE-FBB6-4661-929E-1724ADEC285A"); } }
    }

    public enum RecordsListTypes
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

    public enum RecordViewLayouts
    {
        OneColumn = 1,
        TwoColumns
    }

    public enum RecordViewColumns
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
        [JsonProperty(PropertyName = "currencySymbol")]
        public string CurrencySymbol { get; set; }

        [JsonProperty(PropertyName = "currencyName")]
        public string CurrencyName { get; set; }

        [JsonProperty(PropertyName = "position")]
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