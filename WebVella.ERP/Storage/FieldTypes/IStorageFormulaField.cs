using WebVella.ERP.Api;

namespace WebVella.ERP.Storage
{
    public interface IStorageFormulaField : IStorageField
    {
        FormulaReturnType ReturnType { get; set; }

        string FormulaText { get; set; }

        decimal MinValue { get; set; }

        decimal MaxValue { get; set; }

        int DecimalPlaces { get; set; }

        object Value { get; set; }
    }
}