using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public enum ModalPosition
	{
		[SelectOption(Label = "")]
		Top = 0,
		[SelectOption(Label = "modal-dialog-centered")]
		VerticallyCentered = 1,
	}
}
