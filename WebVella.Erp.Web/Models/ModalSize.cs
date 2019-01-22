using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public enum ModalSize
	{
		[SelectOption(Label = "normal")]
		Normal = 0,
		[SelectOption(Label = "modal-sm")]
		Small = 1,
		[SelectOption(Label = "modal-lg")]
		Large = 2,
		[SelectOption(Label = "modal-xl")]
		ExtraLarge = 3,
		[SelectOption(Label = "modal-full")]
		Full = 4,
	}
}
