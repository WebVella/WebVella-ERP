using System.Collections.Generic;

namespace WebVella.ERP
{
    public interface IEntity : IERPObject
    {
        string Name { get; set; }

        List<IField> Fields { get; set; }
    }
}