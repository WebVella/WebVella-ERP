using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Newtonsoft.Json;
using System;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.AutoMapper;
using WebVella.Erp.Eql;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Plugins.Mail.Services;
using WebVella.Erp.Utilities;

namespace WebVella.Erp.Plugins.Mail.Api
{
	public class SmtpService
	{
		[JsonProperty(PropertyName = "id")]
		public Guid Id { get; internal set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; internal set; }

		[JsonProperty(PropertyName = "server")]
		public string Server { get; internal set; }

		[JsonProperty(PropertyName = "port")]
		public int Port { get; internal set; }

		[JsonProperty(PropertyName = "username")]
		public string Username { get; internal set; }

		[JsonProperty(PropertyName = "password")]
		public string Password { get; internal set; }

		[JsonProperty(PropertyName = "default_sender_name")]
		public string DefaultSenderName { get; internal set; }

		[JsonProperty(PropertyName = "default_sender_email")]
		public string DefaultSenderEmail { get; internal set; }

		[JsonProperty(PropertyName = "default_reply_to_email")]
		public string DefaultReplyToEmail { get; internal set; }

		[JsonProperty(PropertyName = "max_retries_count")]
		public int MaxRetriesCount { get; internal set; }

		[JsonProperty(PropertyName = "retry_wait_minutes")]
		public int RetryWaitMinutes { get; internal set; }

		[JsonProperty(PropertyName = "is_default")]
		public bool IsDefault { get; internal set; }

		[JsonProperty(PropertyName = "is_enabled")]
		public bool IsEnabled { get; internal set; }

		[JsonProperty(PropertyName = "connection_security")]
		public SecureSocketOptions ConnectionSecurity { get; internal set; }

		internal SmtpService()
		{
		}

		public void SendEmail(string recipientName, string recipientEmail, string subject, string textBody, string htmlBody)
		{
			ValidationException ex = new ValidationException();
			
			if( string.IsNullOrEmpty(recipientEmail) )
				ex.AddError("recipientEmail", "Recipient email is not specified.");
			else if( !recipientEmail.IsEmail() )
				ex.AddError("recipientEmail", "Recipient email is not valid email address.");

			if (string.IsNullOrEmpty(subject))
				ex.AddError("subject", "Subject is required.");

			ex.CheckAndThrow();

			var message = new MimeMessage();
			if (!string.IsNullOrWhiteSpace(DefaultSenderName))
				message.From.Add(new MailboxAddress(DefaultSenderName, DefaultSenderEmail));
			else
				message.From.Add(new MailboxAddress(DefaultSenderEmail));

			if (!string.IsNullOrWhiteSpace(recipientName))
				message.To.Add(new MailboxAddress(recipientName, recipientEmail));
			else
				message.To.Add(new MailboxAddress(recipientEmail));

			if (!string.IsNullOrWhiteSpace(DefaultReplyToEmail))
				message.ReplyTo.Add(new MailboxAddress(DefaultReplyToEmail));

			message.Subject = subject;

			var bodyBuilder = new BodyBuilder();
			bodyBuilder.HtmlBody = htmlBody;
			bodyBuilder.TextBody = textBody;
			message.Body = bodyBuilder.ToMessageBody();

			using (var client = new SmtpClient())
			{
				//accept all SSL certificates (in case the server supports STARTTLS)
				client.ServerCertificateValidationCallback = (s, c, h, e) => true;

				client.Connect(Server, Port, ConnectionSecurity);

				if (!string.IsNullOrWhiteSpace(Username))
					client.Authenticate(Username, Password);

				client.Send(message);
				client.Disconnect(true);
			}

			Email email = new Email();
			email.Id = Guid.NewGuid();
			email.SenderEmail = DefaultSenderEmail;
			email.SenderName = DefaultSenderName;
			email.ReplyToEmail = DefaultReplyToEmail;
			email.RecipientEmail = recipientEmail;
			email.RecipientName = recipientName;
			email.Subject = subject;
			email.ContentHtml = htmlBody;
			email.ContentText = textBody;
			email.CreatedOn = DateTime.UtcNow;
			email.SentOn = email.CreatedOn;
			email.Priority = EmailPriority.Normal;
			email.Status = EmailStatus.Sent;
			email.ServerError = string.Empty;
			email.ScheduledOn = null;
			email.RetriesCount = 0;
			email.ServiceId = Id;

			new SmtpInternalService().SaveEmail(email);
		}

		public void SendEmail(string recipientName, string recipientEmail, string senderName, string senderEmail, string subject, string textBody, string htmlBody)
		{
			ValidationException ex = new ValidationException();

			if (string.IsNullOrEmpty(recipientEmail))
				ex.AddError("recipientEmail", "Recipient email is not specified.");
			else if (!recipientEmail.IsEmail())
				ex.AddError("recipientEmail", "Recipient email is not valid email address.");

			if (string.IsNullOrEmpty(subject))
				ex.AddError("subject", "Subject is required.");

			ex.CheckAndThrow();

			var message = new MimeMessage();
			if (!string.IsNullOrWhiteSpace(senderName))
				message.From.Add(new MailboxAddress(senderName, senderEmail));
			else
				message.From.Add(new MailboxAddress(senderEmail));

			if (!string.IsNullOrWhiteSpace(recipientName))
				message.To.Add(new MailboxAddress(recipientName, recipientEmail));
			else
				message.To.Add(new MailboxAddress(recipientEmail));

			if (!string.IsNullOrWhiteSpace(DefaultReplyToEmail))
				message.ReplyTo.Add(new MailboxAddress(DefaultReplyToEmail));

			message.Subject = subject;

			var bodyBuilder = new BodyBuilder();
			bodyBuilder.HtmlBody = htmlBody;
			bodyBuilder.TextBody = textBody;
			message.Body = bodyBuilder.ToMessageBody();

			using (var client = new SmtpClient())
			{
				//accept all SSL certificates (in case the server supports STARTTLS)
				client.ServerCertificateValidationCallback = (s, c, h, e) => true;

				client.Connect(Server, Port, ConnectionSecurity);

				if (!string.IsNullOrWhiteSpace(Username))
					client.Authenticate(Username, Password);

				client.Send(message);
				client.Disconnect(true);
			}

			Email email = new Email();
			email.Id = Guid.NewGuid();
			email.SenderEmail = DefaultSenderEmail;
			email.SenderName = DefaultSenderName;
			email.ReplyToEmail = DefaultReplyToEmail;
			email.RecipientEmail = recipientEmail;
			email.RecipientName = recipientName;
			email.Subject = subject;
			email.ContentHtml = htmlBody;
			email.ContentText = textBody;
			email.CreatedOn = DateTime.UtcNow;
			email.SentOn = email.CreatedOn;
			email.Priority = EmailPriority.Normal;
			email.Status = EmailStatus.Sent;
			email.ServerError = string.Empty;
			email.ScheduledOn = null;
			email.RetriesCount = 0;
			email.ServiceId = Id;

			new SmtpInternalService().SaveEmail(email);
		}

		public void QueueEmail(string recipientName, string recipientEmail, string subject, string textBody, string htmlBody, EmailPriority priority = EmailPriority.Normal  )
		{
			ValidationException ex = new ValidationException();

			if (string.IsNullOrEmpty(recipientEmail))
				ex.AddError("recipientEmail", "Recipient email is not specified.");
			else if (!recipientEmail.IsEmail())
				ex.AddError("recipientEmail", "Recipient email is not valid email address.");

			if (string.IsNullOrEmpty(subject))
				ex.AddError("subject", "Subject is required.");

			ex.CheckAndThrow();

			Email email = new Email();
			email.Id = Guid.NewGuid();
			email.SenderEmail = DefaultSenderEmail;
			email.SenderName = DefaultSenderName;
			email.ReplyToEmail = DefaultReplyToEmail;
			email.RecipientEmail = recipientEmail;
			email.RecipientName = recipientName;
			email.Subject = subject;
			email.ContentHtml = htmlBody;
			email.ContentText = textBody;
			email.CreatedOn = DateTime.UtcNow;
			email.SentOn = null;
			email.Priority = priority;
			email.Status = EmailStatus.Pending; 
			email.ServerError = string.Empty;
			email.ScheduledOn = email.CreatedOn;
			email.RetriesCount = 0;
			email.ServiceId = Id;
			new SmtpInternalService().SaveEmail(email);
		}

		public void QueueEmail(string recipientName, string recipientEmail, string senderName, string senderEmail, string subject, string textBody, string htmlBody, EmailPriority priority = EmailPriority.Normal)
		{
			ValidationException ex = new ValidationException();

			if (string.IsNullOrEmpty(recipientEmail))
				ex.AddError("recipientEmail", "Recipient email is not specified.");
			else if (!recipientEmail.IsEmail())
				ex.AddError("recipientEmail", "Recipient email is not valid email address.");

			if (string.IsNullOrEmpty(subject))
				ex.AddError("subject", "Subject is required.");

			ex.CheckAndThrow();

			Email email = new Email();
			email.Id = Guid.NewGuid();
			email.SenderEmail = senderEmail;
			email.SenderName = senderName;
			email.ReplyToEmail = DefaultReplyToEmail;
			email.RecipientEmail = recipientEmail;
			email.RecipientName = recipientName;
			email.Subject = subject;
			email.ContentHtml = htmlBody;
			email.ContentText = textBody;
			email.CreatedOn = DateTime.UtcNow;
			email.SentOn = null;
			email.Priority = priority;
			email.Status = EmailStatus.Pending;
			email.ServerError = string.Empty;
			email.ScheduledOn = email.CreatedOn;
			email.RetriesCount = 0;
			email.ServiceId = Id;
			new SmtpInternalService().SaveEmail(email);
		}
	}
}
