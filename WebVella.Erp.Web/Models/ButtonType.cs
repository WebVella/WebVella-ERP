using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public enum ButtonType
	{
		[SelectOption(Label = "button")]
		Button = 0,
		[SelectOption(Label = "submit")]
		Submit = 1,
		[SelectOption(Label = "Link As Button")]
		LinkAsButton = 2,
		[SelectOption(Label = "Button Link")]
		ButtonLink = 3,
	}
}
