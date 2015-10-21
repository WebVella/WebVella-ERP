using System;
using System.Collections.Generic;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Storage
{
	public interface IStorageRecordTree
	{
		Guid Id { get; set; }

		string Name { get; set; }

		string Label { get; set; }

		bool Default { get; set; }

		bool System { get; set; }

		decimal? Weight { get; set; }

		string CssClass { get; set; }

		string IconName { get; set; }

		Guid RelationId { get; set; }

		int DepthLimit { get; set; }

		Guid NodeParentIdFieldId { get; set; }

		Guid NodeIdFieldId { get; set; }

		Guid NodeNameFieldId { get; set; }

		Guid NodeLabelFieldId { get; set; }

		List<Guid> RootNodes { get; set; }

		List<Guid> NodeProperties { get; set; }
	}
}


