using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public enum ErpChartType
	{
		[SelectOption(Label = "line")]
		Line = 0,
		[SelectOption(Label = "bar")]
		Bar = 1,
		[SelectOption(Label = "pie")]
		Pie = 2,
		[SelectOption(Label = "doughnut")]
		Doughnut = 3,
		[SelectOption(Label = "line")] //This is correct, another dataset property will be changed to apply the area
		Area = 4,
		[SelectOption(Label = "horizontalBar")]
		HorizontalBar = 5
	}
}
