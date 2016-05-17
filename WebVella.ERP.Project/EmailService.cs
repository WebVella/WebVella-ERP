using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
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
			var configurationBuilder = new ConfigurationBuilder()
				.SetBasePath(PlatformServices.Default.Application.ApplicationBasePath)
				.AddJsonFile("webvella-projects.config.json");
			var configuration = configurationBuilder.Build();


			_settings = new EmailSettings()
			{
			Enabled = Convert.ToBoolean(configuration["EmailSettings:enabled"]),
			Server = configuration["EmailSettings:server"],
			EnableSsl = Convert.ToBoolean(configuration["EmailSettings:enable_ssl"]),
			Port = Convert.ToInt16(configuration["EmailSettings:port"]),
			UserName = configuration["EmailSettings:username"],
			Password = configuration["EmailSettings:password"],
			DisplayName = configuration["EmailSettings:display_name"]	
			};
		}


		public void SendEmail(string toEmailAddress, string subject, string body)
		{
			SendEmail(new[] { toEmailAddress }, subject, body);
		}

		public void SendEmail(string[] toEmailAddresses, string subject, string body)
		{
			if(_settings.Enabled) {
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


}
