using System;

namespace WebVella.Erp.Api.Models.Mapping;

internal class EntityRecordToUserFileStrategy : IMapStrategy<EntityRecord, UserFile>
{
	public UserFile Map(EntityRecord source)
	{
		if (source == null)
			return null;

		var dest = new UserFile();
		dest.Id = (Guid)source["id"];
		dest.Alt = (string)source["alt"];
		dest.Caption = (string)source["caption"];
		dest.CreatedOn = (DateTime)source["created_on"];
		dest.Height = (decimal)source["height"];
		dest.Name = (string)source["name"];
		dest.Path = (string)source["path"];
		dest.Size = (decimal)source["size"];
		dest.Type = (string)source["type"];
		dest.Width = (decimal)source["width"];
		return dest;
	}
}
