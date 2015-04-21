using System.Collections.Generic;

namespace WebVella.ERP.Storage
{
    public interface IStorageEntity : IStorageObject
    {
        string Name { get; set; }

        bool System { get; set; }

        List<IStorageField> Fields { get; set; }

        List<IStorageView> Views { get; set; }

        List<IStorageForm> Forms { get; set; }
    }
}