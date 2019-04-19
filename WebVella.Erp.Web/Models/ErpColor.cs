using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public enum ErpColor
	{
		[SelectOption(Label = "white")]
		White = 0,
		[SelectOption(Label = "primary")]
		Primary = 1,
		[SelectOption(Label = "secondary")]
		Secondary = 2,
		[SelectOption(Label = "success")]
		Success = 3,
		[SelectOption(Label = "info")]
		Info = 4,
		[SelectOption(Label = "warning")]
		Warning = 5,
		[SelectOption(Label = "danger")]
		Danger = 6,
		[SelectOption(Label = "light")]
		Light = 7,
		[SelectOption(Label = "dark")]
		Dark = 8,
		[SelectOption(Label = "link")]
		Link = 9,
		[SelectOption(Label = "red")]
		Red = 10,
		[SelectOption(Label = "pink")]
		Pink = 11,
		[SelectOption(Label = "purple")]
		Purple = 12,
		[SelectOption(Label = "deep-purple")]
		DeepPurple = 13,
		[SelectOption(Label = "indigo")]
		Indigo = 14,
		[SelectOption(Label = "blue")]
		Blue = 15,
		[SelectOption(Label = "light-blue")]
		LightBlue = 16,
		[SelectOption(Label = "cyan")]
		Cyan = 17,
		[SelectOption(Label = "teal")]
		Teal = 18,
		[SelectOption(Label = "green")]
		Green = 19,
		[SelectOption(Label = "light-green")]
		LightGreen = 20,
		[SelectOption(Label = "lime")]
		Lime = 21,
		[SelectOption(Label = "yellow")]
		Yellow = 22,
		[SelectOption(Label = "amber")]
		Amber = 23,
		[SelectOption(Label = "orange")]
		Orange = 24,
		[SelectOption(Label = "deep-orange")]
		DeepOrange = 25,
		[SelectOption(Label = "brown")]
		Brown = 26,
		[SelectOption(Label = "black")]
		Black = 27,
		[SelectOption(Label = "gray")]
		Gray = 28,
		[SelectOption(Label = "blue-gray")]
		BlueGray = 29,
		[SelectOption(Label = "default")]
		Default = 30,
		[SelectOption(Label = "none")]
		None = 31
	}
}
