using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebVella.ERP.Storage
{
    public interface IStorageObjectFactory
    {
        IStorageEntity CreateEmptyEntityObject();
        IStorageRecordPermissions CreateEmptyRecordPermissionsObject();
        IStorageField CreateEmptyFieldObject( Type type );
        IStorageRecordsList CreateEmptyRecordsListObject();
        IStorageRecordView CreateEmptyRecordViewObject();
        IStorageEntityRelation CreateEmptyEntityRelationObject();
        IStorageMultiSelectFieldOption CreateEmptyMultiSelectFieldOptionObject();
        IStorageSelectFieldOption CreateEmptySelectFieldOptionObject();
        IStorageRecordsListFilter CreateEmptyRecordsListFilterObject();
        IStorageRecordsListField CreateEmptyRecordsListFieldObject();
    }
}
