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
        IStorageField CreateEmptyFieldObject(Type type);
        IStorageRecordList CreateEmptyRecordListObject();
        IStorageRecordView CreateEmptyRecordViewObject();
        IStorageEntityRelation CreateEmptyEntityRelationObject();
        IStorageMultiSelectFieldOption CreateEmptyMultiSelectFieldOptionObject();
        IStorageSelectFieldOption CreateEmptySelectFieldOptionObject();
		IStorageCurrencyType CreateEmptyCurrencyTypeObject();

		IStorageRecordListSort CreateEmptyRecordListSortObject();
		IStorageRecordListQuery CreateEmptyRecordListQueryObject();
		IStorageRecordListItemBase CreateEmptyRecordListItemBaseObject();
		IStorageRecordListFieldItem CreateEmptyRecordListFieldItemObject();
		IStorageRecordListRelationFieldItem CreateEmptyRecordListRelationFieldItemObject();

		IStorageRecordViewSidebar CreateEmptyRecordViewSidebarObject();
        IStorageRecordViewSidebarList CreateEmptyRecordViewSidebarListObject();
		IStorageRecordViewRegion CreateEmptyRecordViewRegionObject();
		IStorageRecordViewSection CreateEmptyRecordViewSectionObject();
		IStorageRecordViewRow CreateEmptyRecordViewRowObject();
		IStorageRecordViewColumn CreateEmptyRecordViewColumnObject();
		IStorageRecordViewItemBase CreateEmptyRecordViewItemBaseObject();
		IStorageRecordViewFieldItem CreateEmptyRecordViewFieldItemObject();
		IStorageRecordViewListItem CreateEmptyRecordViewListItemObject();
		IStorageRecordViewViewItem CreateEmptyRecordViewViewItemObject();
		IStorageRecordViewRelationFieldItem CreateEmptyRecordViewRelationFieldItemObject();
        IStorageRecordViewRelationViewItem CreateEmptyRecordViewRelationViewItemObject();
        IStorageRecordViewRelationListItem CreateEmptyRecordViewRelationListItemObject();
        IStorageRecordViewHtmlItem CreateEmptyRecordViewHtmlItemObject();
        
    }
}
