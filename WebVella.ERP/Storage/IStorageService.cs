using System;
using WebVella.ERP.QueryDriver;

namespace WebVella.ERP.Storage
{
    public interface IStorageService
    {
        IStorageEntityRepository GetEntityRepository();
        IStorageQueryRepository GetQueryRepository();
    }
}