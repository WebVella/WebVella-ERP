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

		RecordListType Type { get; set; }

		int RecordsLimit { get; set; }

		int PageSize { get; set; }

		List<IStorageRecordListItemBase> Columns { get; set; }

		IStorageRecordListQuery Query { get; set; }

		List<IStorageRecordListSort> Sorts { get; set; }
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
	}

	public interface IStorageRecordListFieldItem : IStorageRecordListItemBase
	{
		Guid FieldId { get; set; }
	}

	public interface IStorageRecordListRelationFieldItem : IStorageRecordListItemBase
	{
		Guid RelationId { get; set; }

		Guid EntityId { get; set; }

		Guid FieldId { get; set; }
	}
}
