using System;
using WebVella.ERP.QueryDriver;

namespace WebVella.ERP
{
    public interface IStorageService
    {
        IEntityRepository GetEntityRepository();
        IEntityQueryRepository GetEntityQueryRepository();
    }
}