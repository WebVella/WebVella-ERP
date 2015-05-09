using System;
using System.Collections.Generic;

namespace WebVella.ERP.Api.Models
{
	public class MultiSelectField : Field
	{
		public static FieldType FieldType { get { return FieldType.MultiSelectField; } }

		public IEnumerable<string> DefaultValue { get; set; }

		public IDictionary<string, string> Options { get; set; }
	}

	public class MultiSelectFieldMeta : MultiSelectField
	{
		public Guid EntityId { get; set; }

		public string EntityName { get; set; }

		public MultiSelectFieldMeta(Guid entityId, string entityName, MultiSelectField field)
		{
			EntityId = entityId;
			EntityName = entityName;
			DefaultValue = field.DefaultValue;
			Options = field.Options;
		}
	}
}