using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using WebVella.ERP.Projects.Models;

namespace WebVella.ERP.Projects
{
	public class EmailService
	{
		private readonly EmailSettings _settings;

		public EmailService()
		{

			_settings = new EmailSettings()
			{
			Enabled = true,
			Server = "mail.gmail.com",
			EnableSsl = false,
			Port = 25,
			UserName = "fake@gmail.com",
			Password = "test",
			DisplayName = "Peter Dorko"	
			};
		}


		public void SendEmail(string toEmailAddress, string subject, string body)
		{
			SendEmail(new[] { toEmailAddress }, subject, body);
		}

		public void SendEmail(string[] toEmailAddresses, string subject, string body)
		{
			MailMessage message = new MailMessage();
			MailAddress sender = new MailAddress(_settings.UserName, _settings.DisplayName);

			SmtpClient smtp = new SmtpClient()
			{
				Host = _settings.Server,
				Port = _settings.Port,
				EnableSsl = _settings.EnableSsl,
				UseDefaultCredentials = false,
				Credentials = new System.Net.NetworkCredential(_settings.UserName, _settings.Password),
				DeliveryMethod = SmtpDeliveryMethod.Network
			};
			message.From = sender;

			foreach (var strEmail in toEmailAddresses)
				message.To.Add(new MailAddress(strEmail.Trim()));

			message.Subject = subject;
			message.Body = body;
			message.IsBodyHtml = true;
			//smtp.Send(message);
			smtp.SendMailAsync(message);

		}

	}


}
