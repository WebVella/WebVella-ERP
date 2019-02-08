using Newtonsoft.Json;
using System;

namespace WebVella.Erp.Plugins.Mail.Api
{
	public class Email
	{
		[JsonProperty(PropertyName = "id")]
		public Guid Id { get; internal set; }

	}
}
