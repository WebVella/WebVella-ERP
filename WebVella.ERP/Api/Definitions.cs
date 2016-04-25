using Newtonsoft.Json;
using System;

namespace WebVella.ERP.Api
{
    public class SystemIds
    {
        public static Guid SystemEntityId { get { return new Guid("A5050AC8-5967-4CE1-95E7-A79B054F9D14"); } }
        public static Guid UserEntityId { get { return new Guid("B9CEBC3B-6443-452A-8E34-B311A73DCC8B"); } }
        public static Guid RoleEntityId { get { return new Guid("C4541FEE-FBB6-4661-929E-1724ADEC285A"); } }
        public static Guid AreaEntityId = new Guid("CB434298-8583-4A96-BDBB-97B2C1764192");

        public static Guid UserRoleRelationId { get { return new Guid("0C4B119E-1D7B-4B40-8D2C-9E447CC656AB"); } }

        public static Guid AdministratorRoleId { get { return new Guid("BDC56420-CAF0-4030-8A0E-D264938E0CDA"); } }
        public static Guid RegularRoleId { get { return new Guid("F16EC6DB-626D-4C27-8DE0-3E7CE542C55F"); } }
        public static Guid GuestRoleId { get { return new Guid("987148B1-AFA8-4B33-8616-55861E5FD065"); } }

        public static Guid FirstUserId { get { return new Guid("EABD66FD-8DE1-4D79-9674-447EE89921C2"); } }
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

    public enum CurrencySymbolPlacement
    {
        Before = 1,
        After
    }

	[Serializable]
	public class CurrencyType
    {
        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "symbolNative")]
        public string SymbolNative { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "namePlural")]
        public string NamePlural { get; set; }

        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "decimalDigits")]
        public int DecimalDigits { get; set; }

        [JsonProperty(PropertyName = "rounding")]
        public int Rounding { get; set; }

        [JsonProperty(PropertyName = "symbolPlacement")]
        public CurrencySymbolPlacement SymbolPlacement { get; set; }
    }

    public enum FormulaFieldReturnType
    {
        Checkbox = 1,
        Currency,
        Date,
        DateTime,
        Number,
        Percent,
        Text
    }

    public enum EntityPermission
    {
        Read,
        Create,
        Update,
        Delete
    }

}