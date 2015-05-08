using System;
using System.Collections.Generic;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Storage
{
    public interface IStorageEntityRepository : IStorageRepository
    {
        IStorageEntity Empty();
        IStorageEntity Convert(Entity entity);
        List<IStorageEntity> Read();
        IStorageEntity Read(Guid id);
        IStorageEntity Read(string name);
        bool Create(IStorageEntity entity);
        bool Update(IStorageEntity entity);
        bool Delete(Guid id);
        bool Save(IStorageEntity entity);

        IStorageField ConvertField(Field field);
    }
}