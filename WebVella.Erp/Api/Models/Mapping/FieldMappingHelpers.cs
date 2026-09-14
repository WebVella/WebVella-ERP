using System;
using System.Collections.Generic;
using WebVella.Erp.Database;

namespace WebVella.Erp.Api.Models.Mapping;

internal static class FieldMappingHelpers
{
	internal static void CopyFieldToInputField(Field src, InputField dest)
	{
		dest.Id = src.Id;
		dest.Name = src.Name;
		dest.Label = src.Label;
		dest.PlaceholderText = src.PlaceholderText;
		dest.Description = src.Description;
		dest.HelpText = src.HelpText;
		dest.Required = src.Required;
		dest.Unique = src.Unique;
		dest.Searchable = src.Searchable;
		dest.Auditable = src.Auditable;
		dest.System = src.System;
		dest.Permissions = src.Permissions;
		dest.EnableSecurity = src.EnableSecurity;
	}

	internal static void CopyInputFieldToField(InputField src, Field dest)
	{
		dest.Id = src.Id.HasValue ? src.Id.Value : Guid.Empty;
		dest.Name = src.Name;
		dest.Label = src.Label;
		dest.PlaceholderText = src.PlaceholderText;
		dest.Description = src.Description;
		dest.HelpText = src.HelpText;
		dest.Required = src.Required.HasValue ? src.Required.Value : false;
		dest.Unique = src.Unique.HasValue ? src.Unique.Value : false;
		dest.Searchable = src.Searchable.HasValue ? src.Searchable.Value : false;
		dest.Auditable = src.Auditable.HasValue ? src.Auditable.Value : false;
		dest.System = src.System.HasValue ? src.System.Value : false;
		dest.Permissions = src.Permissions;
		dest.EnableSecurity = src.EnableSecurity;
	}

	internal static void CopyFieldToDbBaseField(Field src, DbBaseField dest)
	{
		dest.Id = src.Id;
		dest.Name = src.Name;
		dest.Label = src.Label;
		dest.PlaceholderText = src.PlaceholderText;
		dest.Description = src.Description;
		dest.HelpText = src.HelpText;
		dest.Required = src.Required;
		dest.Unique = src.Unique;
		dest.Searchable = src.Searchable;
		dest.Auditable = src.Auditable;
		dest.System = src.System;
		dest.EnableSecurity = src.EnableSecurity;
		dest.Permissions = src.Permissions == null ? null : new DbFieldPermissions
		{
			CanRead = new List<Guid>(src.Permissions.CanRead ?? new List<Guid>()),
			CanUpdate = new List<Guid>(src.Permissions.CanUpdate ?? new List<Guid>())
		};
	}

	internal static void CopyDbBaseFieldToField(DbBaseField src, Field dest)
	{
		dest.Id = src.Id;
		dest.Name = src.Name;
		dest.Label = src.Label;
		dest.PlaceholderText = src.PlaceholderText;
		dest.Description = src.Description;
		dest.HelpText = src.HelpText;
		dest.Required = src.Required;
		dest.Unique = src.Unique;
		dest.Searchable = src.Searchable;
		dest.Auditable = src.Auditable;
		dest.System = src.System;
		dest.EnableSecurity = src.EnableSecurity;
		dest.Permissions = src.Permissions == null ? null : new FieldPermissions
		{
			CanRead = new List<Guid>(src.Permissions.CanRead ?? new List<Guid>()),
			CanUpdate = new List<Guid>(src.Permissions.CanUpdate ?? new List<Guid>())
		};
	}
}
