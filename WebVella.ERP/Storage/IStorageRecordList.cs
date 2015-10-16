using System;
using System.Collections.Generic;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Storage
{
	public interface IStorageRecordList
	{
		Guid Id { get; set; }

		string Name { get; set; }

		string Label { get; set; }

		bool? Default { get; set; }

		bool? System { get; set; }

		decimal? Weight { get; set; }

		string CssClass { get; set; }

        string IconName{ get; set; }

		string ViewNameOverride{ get; set; }

		int VisibleColumnsCount { get; set; }

        RecordListType Type { get; set; }

		int PageSize { get; set; }

		List<IStorageRecordListItemBase> Columns { get; set; }

		IStorageRecordListQuery Query { get; set; }

		List<IStorageRecordListSort> Sorts { get; set; }

        List<IStorageEntityRelationOptions> RelationOptions { get; set; }
    }

	public interface IStorageRecordListQuery
	{
		QueryType QueryType { get; set; }

		string FieldName { get; set; }

		string FieldValue { get; set; }

		List<IStorageRecordListQuery> SubQueries { get; set; }
	}

	public interface IStorageRecordListSort
	{
		string FieldName { get; set; }

		QuerySortType SortType { get; set; }
	}

	public interface IStorageRecordListItemBase
	{
		Guid EntityId { get; set; }
	}

	public interface IStorageRecordListFieldItem : IStorageRecordListItemBase
	{
		Guid FieldId { get; set; }
	}

	public interface IStorageRecordListRelationFieldItem : IStorageRecordListItemBase
	{
		Guid RelationId { get; set; }

		Guid FieldId { get; set; }

        string FieldLabel { get; set; }

        string FieldPlaceholder { get; set; }

        string FieldHelpText { get; set; }
        
        bool FieldRequired { get; set; }

        string FieldLookupList { get; set; }
    }

	public interface IStorageRecordListViewItem : IStorageRecordListItemBase
	{
		Guid ViewId { get; set; }
	}

	public interface IStorageRecordListRelationViewItem : IStorageRecordListItemBase
	{
		Guid RelationId { get; set; }

		Guid ViewId { get; set; }

        string FieldLabel { get; set; }

        string FieldPlaceholder { get; set; }

        string FieldHelpText { get; set; }

        bool FieldRequired { get; set; }

        string FieldLookupList { get; set; }

		string FieldManageView { get; set; }
    }

	public interface IStorageRecordListListItem : IStorageRecordListItemBase
	{
		Guid ListId { get; set; }
	}

	public interface IStorageRecordListRelationListItem : IStorageRecordListItemBase
	{
		Guid RelationId { get; set; }

		Guid ListId { get; set; }

		string FieldLabel { get; set; }

		string FieldPlaceholder { get; set; }

		string FieldHelpText { get; set; }

		bool FieldRequired { get; set; }

		string FieldLookupList { get; set; }

		string FieldManageView { get; set; }
	}
}
