using WebVella.ERP.Api;

namespace WebVella.ERP.Storage
{
    public interface IStorageFormulaField : IStorageField
    {
        FormulaFieldReturnType ReturnType { get; set; }

        string FormulaText { get; set; }

        int DecimalPlaces { get; set; }
    }
}