using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public enum SitemapNodeType
	{
		[SelectOption(Label = "entity list")]
		EntityList = 1,
		[SelectOption(Label = "application page")]
		ApplicationPage = 2,
		[SelectOption(Label = "url")]
		Url = 3,
	}
}
