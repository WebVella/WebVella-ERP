using WebVella.ERP.Storage;
using WebVella.ERP.Api;

namespace WebVella.ERP.Core
{
    public class MongoFormulaField : MongoBaseField, IStorageFormulaField
    {
        public FormulaFieldReturnType ReturnType { get; set; }

        public string FormulaText { get; set; }

        public int DecimalPlaces { get; set; }
    }
}