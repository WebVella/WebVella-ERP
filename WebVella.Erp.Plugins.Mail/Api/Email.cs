using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebVella.Erp.Plugins.Mail.Api
{
	internal class Email
	{
		[JsonProperty(PropertyName = "id")]
		public Guid Id { get; internal set; }

		[JsonProperty(PropertyName = "service_id")]
		public Guid ServiceId { get; internal set; }

		[JsonProperty(PropertyName = "sender")]
		public EmailAddress Sender { get; internal set; } 

		[JsonProperty(PropertyName = "recipients")]
		public List<EmailAddress> Recipients { get; internal set; }

		[JsonProperty(PropertyName = "reply_to_email")]
		public string ReplyToEmail { get; internal set; }

		[JsonProperty(PropertyName = "subject")]
		public string Subject { get; internal set; }

		[JsonProperty(PropertyName = "content_text")]
		public string ContentText { get; internal set; }

		[JsonProperty(PropertyName = "content_html")]
		public string ContentHtml { get; internal set; }

		[JsonProperty(PropertyName = "created_on")]
		public DateTime CreatedOn { get; internal set; }

		[JsonProperty(PropertyName = "sent_on")]
		public DateTime? SentOn { get; internal set; }

		[JsonProperty(PropertyName = "status")]
		public EmailStatus Status { get; internal set; }

		[JsonProperty(PropertyName = "priority")]
		public EmailPriority Priority { get; internal set; }

		[JsonProperty(PropertyName = "server_error")]
		public string ServerError { get; internal set; }

		[JsonProperty(PropertyName = "scheduled_on")]
		public DateTime? ScheduledOn { get; internal set; }

		[JsonProperty(PropertyName = "retries_count")]
		public int RetriesCount { get; internal set; }

		[JsonProperty(PropertyName = "x_search")]
		public string XSearch { get; internal set; }

		[JsonProperty(PropertyName = "attachments")]
		public List<string> Attachments { get; internal set; } = new List<string>();
	}
}
