using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Newtonsoft.Json;
using System;


namespace WebVella.Erp.Plugins.Mail.Api
{
	public class SmtpService
	{
		[JsonProperty(PropertyName = "id")]
		public Guid Id { get; private set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; private set; }

		[JsonProperty(PropertyName = "server")]
		public string Server { get; private set; }

		[JsonProperty(PropertyName = "port")]
		public int Port { get; private set; }

		[JsonProperty(PropertyName = "username")]
		public string Username { get; private set; }

		[JsonProperty(PropertyName = "password")]
		public string Password { get; private set; }

		[JsonProperty(PropertyName = "default_from_name")]
		public string DefaultFromName { get; private set; }

		[JsonProperty(PropertyName = "default_from_email")]
		public string DefaultFromEmail { get; private set; }

		[JsonProperty(PropertyName = "default_reply_to_email")]
		public string DefaultReplyToEmail { get; private set; }

		[JsonProperty(PropertyName = "max_retries_count")]
		public int MaxRetriesCount { get; private set; }

		[JsonProperty(PropertyName = "max_wait_minutes")]
		public int RetryWaitMinutes { get; private set; }

		[JsonProperty(PropertyName = "is_default")]
		public bool IsDefault { get; private set; }

		[JsonProperty(PropertyName = "connection_security")]
		public SecureSocketOptions ConnectionSecurity { get; private set; }

		internal SmtpService(string name = null)
		{
			//TODO load smtp service settings
		}

		public void SendEmail(string toName, string toEmail, string subject, string textBody, string htmlBody)
		{
			//todo validation

			var message = new MimeMessage();
			if (!string.IsNullOrWhiteSpace(DefaultFromName))
				message.From.Add(new MailboxAddress(DefaultFromName, DefaultFromEmail));
			else
				message.From.Add(new MailboxAddress(DefaultFromEmail));

			if (!string.IsNullOrWhiteSpace(toName))
				message.To.Add(new MailboxAddress(toName, toEmail));
			else
				message.To.Add(new MailboxAddress(toEmail));

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

				if( !string.IsNullOrWhiteSpace(Username) )
					client.Authenticate(Username, Password);

				client.Send(message);
				client.Disconnect(true);
			}
		}

		public void QueueEmail(string toName, string toEmail, string subject, string textBody, string htmlBody)
		{

		}
	}
}
