using Newtonsoft.Json;
using System;
using System.ComponentModel;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	[Serializable]
	public class ScreenMessage
	{
		[JsonProperty(PropertyName = "type")]
		public ScreenMessageType Type { get; set; } = ScreenMessageType.Success;

		[JsonProperty(PropertyName = "title")]
		public string Title { get; set; } = "";

		[JsonProperty(PropertyName = "message")]
		public string Message { get; set; } = "";
	}

	[Serializable]
	public enum ScreenMessageType
	{
		[SelectOption(Label = "success")]
		Success = 0,
		[SelectOption(Label = "info")]
		Info = 1,
		[SelectOption(Label = "warning")]
		Warning = 2,
		[SelectOption(Label = "error")]
		Error = 3
	}
}