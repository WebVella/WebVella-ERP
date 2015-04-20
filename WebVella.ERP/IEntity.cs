using System.Collections.Generic;

namespace WebVella.ERP
{
    public interface IEntity : IERPObject
    {
        string Name { get; set; }

        bool System { get; set; }

        List<IField> Fields { get; set; }

        List<IView> Views { get; set; }

        List<IView> Forms { get; set; }
    }
}