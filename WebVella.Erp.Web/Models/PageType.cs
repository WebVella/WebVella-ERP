using System.ComponentModel;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public enum PageType
	{
		[SelectOption(Label = "home")]
		Home = 0,
		[SelectOption(Label = "site")]
		Site = 1,
		[SelectOption(Label = "application")]
		Application = 2,
		[SelectOption(Label = "record list")]
		RecordList = 3,
		[SelectOption(Label = "record create")]
		RecordCreate = 4,
		[SelectOption(Label = "record details")]
		RecordDetails = 5,
		[SelectOption(Label = "record manage")]
		RecordManage = 6
	}
}
