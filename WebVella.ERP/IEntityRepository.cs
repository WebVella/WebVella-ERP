using System;
using System.Collections.Generic;

namespace WebVella.ERP
{
    public interface IEntityRepository : IRepository
    {
        IEntity Empty();
        List<IEntity> Read();
        IEntity Read(Guid id);
        IEntity Read(string name);
        bool Create(IEntity entity);
        bool Update(IEntity entity);
        bool Delete(Guid id);
        bool Save(IEntity entity);
    }
}