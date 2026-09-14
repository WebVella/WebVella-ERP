using System;
using System.Collections.Generic;
using System.Text;

namespace WebVella.Erp.Api.Models.Mapping;

internal class ErpUserToEntityRecordStrategy : IMapStrategy<ErpUser, EntityRecord>
{
	public EntityRecord Map(ErpUser source)
	{
		var src = source;

		if (src == null)
			return null;

		EntityRecord dest = new EntityRecord();
		dest["id"] = src.Id;
		dest["username"] = src.Username;
		dest["email"] = src.Email;
		dest["password"] = src.Password;
		dest["first_name"] = src.FirstName;
		dest["last_name"] = src.LastName;
		dest["created_on"] = src.CreatedOn;
		dest["last_logged_in"] = src.LastLoggedIn;
		dest["enabled"] = src.Enabled;
		dest["verified"] = src.Verified;
		dest["preferences"] = src.Preferences ?? new ErpUserPreferences();

		return dest;
	}
}
