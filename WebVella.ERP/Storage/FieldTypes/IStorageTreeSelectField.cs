using System;

namespace WebVella.ERP.Storage
{
    public interface IStorageTreeSelectField : IStorageField
    {
		Guid RelatedEntityId { get; set; }

		Guid RelationId { get; set; }

		Guid SelectedTreeId { get; set; }

		string SelectionType { get; set; }

		string SelectionTarget { get; set; }
	}
}
