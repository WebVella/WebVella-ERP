using System;

namespace WebVella.ERP
{
    public interface IStorageService
    {
        IEntityRepository GetEntityRepository();
    }
}