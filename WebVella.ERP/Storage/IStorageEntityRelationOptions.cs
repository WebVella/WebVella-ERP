using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.ERP.Storage
{
    public interface IStorageEntityRelationOptions 
    {
        Guid? RelationId { get; set; }
        string RelationName { get; set; }
        string Direction { get; set; }
    }
}
