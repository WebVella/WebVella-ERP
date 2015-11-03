using System;
using System.Collections.Generic;

namespace WebVella.ERP.Storage
{
    public interface IStorageEntityRelationRepository : IStorageRepository
    {
        List<IStorageEntityRelation> Read();
        IStorageEntityRelation Read(Guid id);
        IStorageEntityRelation Read(string name);
        bool Create(IStorageEntityRelation relation);
        bool Update(IStorageEntityRelation relation);
        bool Delete(Guid id);
        bool Save(IStorageEntityRelation relation);

        List<Guid> ReadManyToManyRecordByOrigin(Guid relationId, Guid originId);
        List<Guid> ReadManyToManyRecordByTarget(Guid relationId, Guid targetId);
        void CreateManyToManyRecord(Guid relationId, Guid originId, Guid targetId);
        void DeleteManyToManyRecord(Guid relationId, Guid originId, Guid targetId);

		void ConvertNtoNRelations();
    }
}