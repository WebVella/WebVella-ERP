using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using WebVella.Erp.Diagnostics;

namespace WebVella.Erp.Web.Services
{
    public class MailService : BaseService
    {
        #region <--- Methods --->
        /// <summary>
        /// Sends log message through smtp settings set in the config file.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="details">The details.</param>
        /// <param name="host">The host.</param>
        /// <returns>If message is sent true else false</returns>
        public bool SendLogMessage(LogType type, string source, string message, string details, string host = null)
        {
            if (ErpSettings.EmailEnabled)
            {
                try
                {
                    List<KeyValuePair<string, string>> tags = new List<KeyValuePair<string, string>>();
                    tags.Add(new KeyValuePair<string, string>(key: "host", value: host ?? "WebVella.Erp.Web"));
                    tags.Add(new KeyValuePair<string, string>(key: "type", value: type.ToString()));
                    tags.Add(new KeyValuePair<string, string>(key: "source", value: source));
                    tags.Add(new KeyValuePair<string, string>(key: "message", value: message));
                    tags.Add(new KeyValuePair<string, string>(key: "details", value: details));

                    var client = SmtpClient(ErpSettings.EmailSMTPServerName, ErpSettings.EmailSMTPPort, ErpSettings.EmailSMTPUsername, ErpSettings.EmailSMTPPassword);
                    string html = ReplaceTagsInHtml(LogTemplate, tags);

                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(ErpSettings.EmailFrom);
                    var emails = ErpSettings.EmailTo.Split(",", StringSplitOptions.RemoveEmptyEntries);
                    foreach (var email in emails)
                    {
                        mailMessage.To.Add(email.Trim());
                    }
                    mailMessage.Subject = message;
                    mailMessage.Body = html;
                    client.Send(mailMessage);
                    return true;
                }
                catch
                {
                }
            }

            return false;

        }


        #endregion

        #region <--- Common --->

        /// <summary>
        /// Initialize SMTP client.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        private SmtpClient SmtpClient(string host, int port, string username, string password)
        {
            if (string.IsNullOrWhiteSpace(host))
                throw new Exception("Няма въведени настройки за smtp сървър за пращане на писма.");

            SmtpClient client = new SmtpClient(host, port);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(username, password);
            return client;
        }

        /// <summary>
        /// Replaces the tags in HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="tags">The tags.</param>
        /// <returns></returns>
        private string ReplaceTagsInHtml(string html, List<KeyValuePair<string, string>> tags)
        {

            StringBuilder sb = new StringBuilder(html);
            foreach (var tag in tags)
            {
                sb.Replace("{{" + tag.Key + "}}", tag.Value);
            }
            return sb.ToString();
        }

        private const string LogTemplate = @"<!DOCTYPE html>
                                            <html lang='en'>
	                                            <head>
		                                            <title>Log</title>
	                                            </head>
	                                            <body>
		                                            <table>
			                                            <tr>
				                                            <td>
					                                            <b>host: </b>{{host}}</td>
			                                            </tr>
			                                            <tr>
				                                            <td>
					                                            <b>log type: </b>{{type}}</td>
			                                            </tr>
			                                            <tr>
				                                            <td>
					                                            <b>source: </b>{{source}}</td>
			                                            </tr>
			                                            <tr>
				                                            <td>
					                                            <b>message: </b>{{message}}</td>
			                                            </tr>
			                                            <tr>
				                                            <td>
					                                            <b>details: </b>{{details}}</td>
			                                            </tr>
		                                            </table>

	                                            </body>
                                            </html>";
        private const string InvoiceTemplate = @"<!DOCTYPE html>
                                            <html lang='en'>
	                                            <head>
		                                            <title>Invoice</title>
	                                            </head>
	                                            <body>
		                                            TODO
	                                            </body>
                                            </html>";
        #endregion //<--- End Common --->
    }
}
