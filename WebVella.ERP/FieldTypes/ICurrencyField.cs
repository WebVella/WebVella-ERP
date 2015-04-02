using System;

namespace WebVella.ERP.Core
{

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

    public interface ICurrencyField : IField
    {
        new decimal DefaultValue { get; set; }

        decimal MinValue { get; set; }

        decimal MaxValue { get; set; }

        CurrencyTypes Currency { get; set; }

        decimal Value { get; set; }
    }
}