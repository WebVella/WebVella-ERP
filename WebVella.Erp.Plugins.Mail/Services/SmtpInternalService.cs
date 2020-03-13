using HtmlAgilityPack;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using MimeKit;
using MimeKit.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Api.Models.AutoMapper;
using WebVella.Erp.Database;
using WebVella.Erp.Eql;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Plugins.Mail.Api;
using WebVella.Erp.Utilities;
using WebVella.Erp.Web.Models;
using WebVella.Erp.Web.Pages.Application;
using WebVella.Erp.Web.Utils;

namespace WebVella.Erp.Plugins.Mail.Services
{
	internal class SmtpInternalService
	{
		private static object lockObject = new object();
		private static bool queueProcessingInProgress = false;

		#region <--- Hooks Logic --->

		public void ValidatePreCreateRecord(EntityRecord rec, List<ErrorModel> errors)
		{
			foreach (var prop in rec.Properties)
			{
				switch (prop.Key)
				{
					case "name":
						{
							var result = new EqlCommand("SELECT * FROM smtp_service WHERE name = @name", new EqlParameter("name", rec["name"])).Execute();
							if (result.Count > 0)
							{
								errors.Add(new ErrorModel
								{
									Key = "name",
									Value = (string)rec["name"],
									Message = "There is already existing service with that name. Name must be unique"
								});
							}
						}
						break;
					case "port":
						{
							if (!Int32.TryParse(rec["port"]?.ToString(), out int port))
							{
								errors.Add(new ErrorModel
								{
									Key = "port",
									Value = rec["port"]?.ToString(),
									Message = $"Port must be an integer value between 1 and 65025"
								});
							}
							else
							{
								if (port <= 0 || port > 65025)
								{
									errors.Add(new ErrorModel
									{
										Key = "port",
										Value = rec["port"]?.ToString(),
										Message = $"Port must be an integer value between 1 and 65025"
									});
								}
							}

						}
						break;
					case "default_from_email":
						{
							if (!((string)rec["default_from_email"]).IsEmail())
							{
								errors.Add(new ErrorModel
								{
									Key = "default_from_email",
									Value = (string)rec["default_from_email"],
									Message = $"Default from email address is invalid"
								});
							}
						}
						break;
					case "default_reply_to_email":
						{
							if (string.IsNullOrWhiteSpace((string)rec["default_reply_to_email"]))
								continue;

							if (!((string)rec["default_reply_to_email"]).IsEmail())
							{
								errors.Add(new ErrorModel
								{
									Key = "default_reply_to_email",
									Value = (string)rec["default_reply_to_email"],
									Message = $"Default reply to email address is invalid"
								});
							}
						}
						break;
					case "max_retries_count":
						{
							if (!Int32.TryParse(rec["max_retries_count"]?.ToString(), out int count))
							{
								errors.Add(new ErrorModel
								{
									Key = "max_retries_count",
									Value = rec["max_retries_count"]?.ToString(),
									Message = $"Number of retries on error must be an integer value between 1 and 10"
								});
							}
							else
							{
								if (count < 1 || count > 10)
								{
									errors.Add(new ErrorModel
									{
										Key = "max_retries_count",
										Value = rec["max_retries_count"]?.ToString(),
										Message = $"Number of retries on error must be an integer value between 1 and 10"
									});
								}
							}
						}
						break;
					case "retry_wait_minutes":
						{
							if (!Int32.TryParse(rec["retry_wait_minutes"]?.ToString(), out int minutes))
							{
								errors.Add(new ErrorModel
								{
									Key = "retry_wait_minutes",
									Value = rec["retry_wait_minutes"]?.ToString(),
									Message = $"Wait period between retries must be an integer value between 1 and 1440 minutes"
								});
							}
							else
							{
								if (minutes < 1 || minutes > 1440)
								{
									errors.Add(new ErrorModel
									{
										Key = "retry_wait_minutes",
										Value = rec["retry_wait_minutes"]?.ToString(),
										Message = $"Wait period between retries must be an integer value between 1 and 1440 minutes"
									});
								}
							}
						}
						break;
					case "connection_security":
						{
							if (!Int32.TryParse(rec["connection_security"] as string, out int connectionSecurityNumber))
							{
								errors.Add(new ErrorModel
								{
									Key = "connection_security",
									Value = (string)rec["connection_security"],
									Message = $"Invalid connection security setting selected."
								});
								continue;
							}

							try
							{
								var secOptions = (MailKit.Security.SecureSocketOptions)connectionSecurityNumber;
							}
							catch
							{
								errors.Add(new ErrorModel
								{
									Key = "connection_security",
									Value = (string)rec["connection_security"],
									Message = $"Invalid connection security setting selected."
								});
							}
						}
						break;
				}
			}
		}

		public void ValidatePreUpdateRecord(EntityRecord rec, List<ErrorModel> errors)
		{
			foreach (var prop in rec.Properties)
			{
				switch (prop.Key)
				{
					case "name":
						{
							var result = new EqlCommand("SELECT * FROM smtp_service WHERE name = @name", new EqlParameter("name", rec["name"])).Execute();
							if (result.Count > 1)
							{
								errors.Add(new ErrorModel
								{
									Key = "name",
									Value = (string)rec["name"],
									Message = "There is already existing service with that name. Name must be unique"
								});
							}
							else if (result.Count == 1 && (Guid)result[0]["id"] != (Guid)rec["id"])
							{
								errors.Add(new ErrorModel
								{
									Key = "name",
									Value = (string)rec["name"],
									Message = "There is already existing service with that name. Name must be unique"
								});
							}
						}
						break;
					case "port":
						{
							if (!Int32.TryParse(rec["port"] as string, out int port))
							{
								errors.Add(new ErrorModel
								{
									Key = "port",
									Value = (string)rec["port"],
									Message = $"Port must be an integer value between 1 and 65025"
								});
							}
							else
							{
								if (port <= 0 || port > 65025)
								{
									errors.Add(new ErrorModel
									{
										Key = "port",
										Value = (string)rec["port"],
										Message = $"Port must be an integer value between 1 and 65025"
									});
								}
							}

						}
						break;
					case "default_from_email":
						{
							if (!((string)rec["default_from_email"]).IsEmail())
							{
								errors.Add(new ErrorModel
								{
									Key = "default_from_email",
									Value = (string)rec["default_from_email"],
									Message = $"Default from email address is invalid"
								});
							}
						}
						break;
					case "default_reply_to_email":
						{
							if (string.IsNullOrWhiteSpace((string)rec["default_reply_to_email"]))
								continue;

							if (!((string)rec["default_reply_to_email"]).IsEmail())
							{
								errors.Add(new ErrorModel
								{
									Key = "default_reply_to_email",
									Value = (string)rec["default_reply_to_email"],
									Message = $"Default reply to email address is invalid"
								});
							}
						}
						break;
					case "max_retries_count":
						{
							if (!Int32.TryParse(rec["max_retries_count"] as string, out int count))
							{
								errors.Add(new ErrorModel
								{
									Key = "max_retries_count",
									Value = (string)rec["max_retries_count"],
									Message = $"Number of retries on error must be an integer value between 1 and 10"
								});
							}
							else
							{
								if (count < 1 || count > 10)
								{
									errors.Add(new ErrorModel
									{
										Key = "max_retries_count",
										Value = (string)rec["max_retries_count"],
										Message = $"Number of retries on error must be an integer value between 1 and 10"
									});
								}
							}
						}
						break;
					case "retry_wait_minutes":
						{
							if (!Int32.TryParse(rec["retry_wait_minutes"] as string, out int minutes))
							{
								errors.Add(new ErrorModel
								{
									Key = "retry_wait_minutes",
									Value = (string)rec["retry_wait_minutes"],
									Message = $"Wait period between retries must be an integer value between 1 and 1440 minutes"
								});
							}
							else
							{
								if (minutes < 1 || minutes > 1440)
								{
									errors.Add(new ErrorModel
									{
										Key = "retry_wait_minutes",
										Value = (string)rec["retry_wait_minutes"],
										Message = $"Wait period between retries must be an integer value between 1 and 1440 minutes"
									});
								}
							}
						}
						break;
					case "connection_security":
						{
							if (!Int32.TryParse(rec["connection_security"] as string, out int connectionSecurityNumber))
							{
								errors.Add(new ErrorModel
								{
									Key = "connection_security",
									Value = (string)rec["connection_security"],
									Message = $"Invalid connection security setting selected."
								});
								continue;
							}

							try
							{
								var secOptions = (MailKit.Security.SecureSocketOptions)connectionSecurityNumber;
							}
							catch
							{
								errors.Add(new ErrorModel
								{
									Key = "connection_security",
									Value = (string)rec["connection_security"],
									Message = $"Invalid connection security setting selected."
								});
							}
						}
						break;
				}
			}
		}

		public void HandleDefaultServiceSetup(EntityRecord rec, List<ErrorModel> errors)
		{
			if (rec.Properties.ContainsKey("is_default") && (bool)rec["is_default"])
			{

				var recMan = new RecordManager(executeHooks: false);
				var records = new EqlCommand("SELECT id,is_default FROM smtp_service").Execute();
				foreach (var record in records)
				{
					if ((bool)record["is_default"])
					{
						record["is_default"] = false;
						recMan.UpdateRecord("smtp_service", record);
					}
				}
			}
			else if (rec.Properties.ContainsKey("is_default") && (bool)rec["is_default"] == false)
			{
				var currentRecord = new EqlCommand("SELECT * FROM smtp_service WHERE id = @id", new EqlParameter("id", rec["id"])).Execute();
				if (currentRecord.Count > 0 && (bool)currentRecord[0]["is_default"])
				{
					errors.Add(new ErrorModel
					{
						Key = "is_default",
						Value = ((bool)rec["is_default"]).ToString(),
						Message = $"Forbidden. There should always be an active default service."
					});
				}
			}
		}

		public IActionResult TestSmtpServiceOnPost(RecordDetailsPageModel pageModel)
		{
			SmtpService smtpService = null;
			string recipientEmail = string.Empty;
			string subject = string.Empty;
			string content = string.Empty;

			ValidationException valEx = new ValidationException();

			if (pageModel.HttpContext.Request.Form == null)
			{
				valEx.AddError("form", "Smtp service test page missing form tag");
				valEx.CheckAndThrow();
			}


			if (!pageModel.HttpContext.Request.Form.ContainsKey("recipient_email"))
				valEx.AddError("recipient_email", "Recipient email is not specified.");
			else
			{
				recipientEmail = pageModel.HttpContext.Request.Form["recipient_email"];
				if (string.IsNullOrWhiteSpace(recipientEmail))
					valEx.AddError("recipient_email", "Recipient email is not specified");
				else if (!recipientEmail.IsEmail())
					valEx.AddError("recipient_email", "Recipient email is not a valid email address");
			}

			if (!pageModel.HttpContext.Request.Form.ContainsKey("subject"))
				valEx.AddError("subject", "Subject is not specified");
			else
			{
				subject = pageModel.HttpContext.Request.Form["subject"];
				if (string.IsNullOrWhiteSpace(subject))
					valEx.AddError("subject", "Subject is required");
			}

			if (!pageModel.HttpContext.Request.Form.ContainsKey("content"))
				valEx.AddError("content", "Content is not specified");
			else
			{
				content = pageModel.HttpContext.Request.Form["content"];
				if (string.IsNullOrWhiteSpace(content))
					valEx.AddError("content", "Content is required");
			}

			var smtpServiceId = pageModel.DataModel.GetProperty("Record.id") as Guid?;

			if (smtpServiceId == null)
				valEx.AddError("serviceId", "Invalid smtp service id");
			else
			{
				smtpService = new EmailServiceManager().GetSmtpService(smtpServiceId.Value);
				if (smtpService == null)
					valEx.AddError("serviceId", "Smtp service with specified id does not exist");
			}

			List<string> attachments = new List<string>();
			if (pageModel.HttpContext.Request.Form.ContainsKey("attachments"))
			{
				var ids = pageModel.HttpContext.Request.Form["attachments"].ToString().Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => new Guid(x));
				foreach(var id in ids )
				{
					var fileRecord = new EqlCommand("SELECT name,path FROM user_file WHERE id = @id", new EqlParameter("id", id)).Execute().FirstOrDefault();
					if (fileRecord != null)
						attachments.Add((string)fileRecord["path"]);
				}
			}

			//we set current record to store properties which don't exist in current entity 
			EntityRecord currentRecord = pageModel.DataModel.GetProperty("Record") as EntityRecord;
			currentRecord["recipient_email"] = recipientEmail;
			currentRecord["subject"] = subject;
			currentRecord["content"] = content;
			pageModel.DataModel.SetRecord(currentRecord);

			valEx.CheckAndThrow();

			try
			{
				EmailAddress recipient = new EmailAddress( recipientEmail );
				smtpService.SendEmail(recipient, subject, string.Empty, content, attachments: attachments );
				pageModel.TempData.Put("ScreenMessage", new ScreenMessage() { Message = "Email was successfully sent", Type = ScreenMessageType.Success, Title = "Success" });
				var returnUrl = pageModel.HttpContext.Request.Query["returnUrl"];
				return new RedirectResult($"/mail/services/smtp/r/{smtpService.Id}/details?returnUrl={returnUrl}");
			}
			catch (Exception ex)
			{
				valEx.AddError("", ex.Message);
				valEx.CheckAndThrow();
				return null;
			}
		}

		public IActionResult EmailSendNowOnPost(RecordDetailsPageModel pageModel)
		{
			var emailId = (Guid)pageModel.DataModel.GetProperty("Record.id");

			var internalSmtpSrv = new SmtpInternalService();
			Email email = internalSmtpSrv.GetEmail(emailId);
			SmtpService smtpService = new EmailServiceManager().GetSmtpService(email.ServiceId);
			internalSmtpSrv.SendEmail(email, smtpService);

			if (email.Status == EmailStatus.Sent)
				pageModel.TempData.Put("ScreenMessage", new ScreenMessage() { Message = "Email was successfully sent", Type = ScreenMessageType.Success, Title = "Success" });
			else
				pageModel.TempData.Put("ScreenMessage", new ScreenMessage() { Message = email.ServerError, Type = ScreenMessageType.Error, Title = "Error" });

			var returnUrl = pageModel.HttpContext.Request.Query["returnUrl"];
			return new RedirectResult($"/mail/emails/all/r/{emailId}/details?returnUrl={returnUrl}");
		}

		#endregion

		internal void SaveEmail(Email email)
		{
			PrepareEmailXSearch(email);
			RecordManager recMan = new RecordManager();
			var response = recMan.Find(new EntityQuery("email", "*", EntityQuery.QueryEQ("id", email.Id)));
			if (response.Object != null && response.Object.Data != null && response.Object.Data.Count != 0)
				response = recMan.UpdateRecord("email", email.MapTo<EntityRecord>());
			else
				response = recMan.CreateRecord("email", email.MapTo<EntityRecord>());

			if (!response.Success)
				throw new Exception(response.Message);
			
		}


		#region <--- content manipulation --->

		public static void ProcessHtmlContent(BodyBuilder builder)
		{
			if (builder == null)
				return;

			if (string.IsNullOrWhiteSpace(builder.HtmlBody))
				return;

			try
			{
				var htmlDoc = new HtmlDocument();
				htmlDoc.Load(new MemoryStream(Encoding.UTF8.GetBytes(builder.HtmlBody)));

				if (htmlDoc.DocumentNode == null)
					return;

				foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//img[@src]"))
				{
					var src = node.Attributes["src"].Value.Split('?', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();


					if (!string.IsNullOrWhiteSpace(src) && src.StartsWith("/fs"))
					{
						try
						{
							Uri uri = new Uri(src);
							src = uri.AbsolutePath;
						}
						catch { }

						if (src.StartsWith("/fs"))
							src = src.Substring(3);

						DbFileRepository fsRepository = new DbFileRepository();
						var file = fsRepository.Find(src);
						if (file == null)
							continue;

						var bytes = file.GetBytes();

						var extension = Path.GetExtension(src).ToLowerInvariant();
						new FileExtensionContentTypeProvider().Mappings.TryGetValue(extension, out string mimeType);

						var imagePart = new MimePart(mimeType)
						{
							ContentId = MimeUtils.GenerateMessageId(),
							Content = new MimeContent(new MemoryStream(bytes)),
							ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
							ContentTransferEncoding = ContentEncoding.Base64,
							FileName = Path.GetFileName(src)
						};

						builder.LinkedResources.Add(imagePart);
						node.SetAttributeValue("src", $"cid:{imagePart.ContentId}");
					}
				}
				builder.HtmlBody = htmlDoc.DocumentNode.OuterHtml;
				if (string.IsNullOrWhiteSpace(builder.TextBody) && !string.IsNullOrWhiteSpace(builder.HtmlBody))
					builder.TextBody = ConvertToPlainText(builder.HtmlBody);
			}
			catch
			{
				return;
			}
		}


		private static string ConvertToPlainText(string html)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(html))
					return string.Empty;

				HtmlDocument doc = new HtmlDocument();
				doc.LoadHtml(html);

				StringWriter sw = new StringWriter();
				ConvertTo(doc.DocumentNode, sw);
				sw.Flush();
				return sw.ToString();
			}
			catch
			{
				return string.Empty;
			}
		}

		private static void ConvertContentTo(HtmlNode node, TextWriter outText)
		{
			foreach (HtmlNode subnode in node.ChildNodes)
			{
				ConvertTo(subnode, outText);
			}
		}

		private static void ConvertTo(HtmlNode node, TextWriter outText)
		{
			string html;
			switch (node.NodeType)
			{
				case HtmlNodeType.Comment:
					// don't output comments
					break;

				case HtmlNodeType.Document:
					ConvertContentTo(node, outText);
					break;

				case HtmlNodeType.Text:
					// script and style must not be output
					string parentName = node.ParentNode.Name;
					if ((parentName == "script") || (parentName == "style"))
						break;

					// get text
					html = ((HtmlTextNode)node).Text;

					// is it in fact a special closing node output as text?
					if (HtmlNode.IsOverlappedClosingElement(html))
						break;

					// check the text is meaningful and not a bunch of white spaces
					if (html.Trim().Length > 0)
					{
						outText.Write(HtmlEntity.DeEntitize(html));
					}
					break;

				case HtmlNodeType.Element:
					switch (node.Name)
					{
						case "p":
							// treat paragraphs as crlf
							outText.Write(Environment.NewLine);
							break;
						case "br":
							outText.Write(Environment.NewLine);
							break;
						case "a":
							HtmlAttribute att = node.Attributes["href"];
							outText.Write($"<{att.Value}>");
							break;
					}

					if (node.HasChildNodes)
					{
						ConvertContentTo(node, outText);
					}
					break;
			}
		}

		#endregion


		internal Email GetEmail(Guid id)
		{
			var result = new EqlCommand("SELECT * FROM email WHERE id = @id", new EqlParameter("id", id)).Execute();
			if (result.Count == 1)
				return result[0].MapTo<Email>();

			return null;
		}

		internal void PrepareEmailXSearch(Email email)
		{
			var recipientsText = string.Join(" ", email.Recipients.Select(x => $"{x.Name} {x.Address}") );
			email.XSearch = $"{email.Sender?.Name} {email.Sender?.Address} {recipientsText} {email.Subject} {email.ContentText} {email.ContentHtml}";
		}

		internal void SendEmail(Email email, SmtpService service)
		{
			try
			{

				if (service == null)
				{
					email.ServerError = "SMTP service not found";
					email.Status = EmailStatus.Aborted;
					return; //save email in finally block will save changes
				}
				else if (!service.IsEnabled)
				{
					email.ServerError = "SMTP service is not enabled";
					email.Status = EmailStatus.Aborted;
					return; //save email in finally block will save changes
				}

				var message = new MimeMessage();
				if (!string.IsNullOrWhiteSpace(email.Sender?.Name))
					message.From.Add(new MailboxAddress(email.Sender?.Name, email.Sender?.Address));
				else
					message.From.Add(new MailboxAddress(email.Sender?.Address));

				foreach (var recipient in email.Recipients)
				{
					if (!string.IsNullOrWhiteSpace(recipient.Name))
						message.To.Add(new MailboxAddress(recipient.Name, recipient.Address));
					else
						message.To.Add(new MailboxAddress(recipient.Address));
				}

				if (!string.IsNullOrWhiteSpace(email.ReplyToEmail))
					message.ReplyTo.Add(new MailboxAddress(email.ReplyToEmail));
				else
					message.ReplyTo.Add(new MailboxAddress(email.Sender?.Address));

				message.Subject = email.Subject;

				var bodyBuilder = new BodyBuilder();
				bodyBuilder.HtmlBody = email.ContentHtml;
				bodyBuilder.TextBody = email.ContentText;

				if (email.Attachments != null && email.Attachments.Count > 0)
				{
					foreach (var att in email.Attachments )
					{
						var filepath = att;

						if (!filepath.StartsWith("/"))
							filepath = "/" + filepath;

						filepath = filepath.ToLowerInvariant();

						if (filepath.StartsWith("/fs"))
							filepath = filepath.Substring(3);

						DbFileRepository fsRepository = new DbFileRepository();
						var file = fsRepository.Find(filepath);
						var bytes = file.GetBytes();

						var extension = Path.GetExtension(filepath).ToLowerInvariant();
						new FileExtensionContentTypeProvider().Mappings.TryGetValue(extension, out string mimeType);

						var attachment = new MimePart(mimeType)
						{
							Content = new MimeContent(new MemoryStream(bytes)),
							ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
							ContentTransferEncoding = ContentEncoding.Base64,
							FileName = Path.GetFileName(filepath)
						};

						bodyBuilder.Attachments.Add(attachment);
					}
				}
				ProcessHtmlContent(bodyBuilder);
				message.Body = bodyBuilder.ToMessageBody();

				using (var client = new SmtpClient())
				{
					//accept all SSL certificates (in case the server supports STARTTLS)
					client.ServerCertificateValidationCallback = (s, c, h, e) => true;

					client.Connect(service.Server, service.Port, service.ConnectionSecurity);

					if (!string.IsNullOrWhiteSpace(service.Username))
						client.Authenticate(service.Username, service.Password);

					client.Send(message);
					client.Disconnect(true);
				}
				email.SentOn = DateTime.UtcNow;
				email.Status = EmailStatus.Sent;
				email.ScheduledOn = null;
				email.ServerError = null;
			}
			catch (Exception ex)
			{
				email.SentOn = null;
				email.ServerError = ex.Message;
				email.RetriesCount++;
				if (email.RetriesCount >= service.MaxRetriesCount)
				{
					email.ScheduledOn = null;
					email.Status = EmailStatus.Aborted;
				}
				else
				{
					email.ScheduledOn = DateTime.UtcNow.AddMinutes(service.RetryWaitMinutes);
					email.Status = EmailStatus.Pending;
				}

			}
			finally
			{
				new SmtpInternalService().SaveEmail(email);
			}
		}

		public void ProcessSmtpQueue()
		{
			lock (lockObject)
			{
				if (queueProcessingInProgress)
					return;

				queueProcessingInProgress = true;
			}

			try
			{
				List<Email> pendingEmails = new List<Email>();
				do
				{
					EmailServiceManager serviceManager = new EmailServiceManager();

					pendingEmails = new EqlCommand("SELECT * FROM email WHERE status = @status AND scheduled_on <> NULL" +
													" AND scheduled_on < @scheduled_on  ORDER BY priority DESC, scheduled_on ASC PAGE 1 PAGESIZE 10",
								new EqlParameter("status", ((int)EmailStatus.Pending).ToString()),
								new EqlParameter("scheduled_on", DateTime.UtcNow)).Execute().MapTo<Email>();

					foreach (var email in pendingEmails)
					{
						var service = serviceManager.GetSmtpService(email.ServiceId);
						if (service == null)
						{
							email.Status = EmailStatus.Aborted;
							email.ServerError = "SMTP service not found.";
							email.ScheduledOn = null;
							SaveEmail(email);
							continue;
						}
						else
						{
							SendEmail(email, service);
						}
					}
				}
				while (pendingEmails.Count > 0);

			}
			finally
			{
				lock (lockObject)
				{
					queueProcessingInProgress = false;
				}
			}
		}
	}
}
