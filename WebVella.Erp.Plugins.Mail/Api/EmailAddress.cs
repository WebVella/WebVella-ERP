using MimeKit;
using Newtonsoft.Json;

namespace WebVella.Erp.Plugins.Mail.Api
{
	public class EmailAddress
	{
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; } = string.Empty;

		[JsonProperty(PropertyName = "address")]
		public string Address { get; set; } = string.Empty;

		public EmailAddress()
		{ }

		public EmailAddress(string address)
		{
			Address = address;
		}

		public EmailAddress(string name, string address)
		{
			Name = name;
			Address = address;
		}

		public static EmailAddress FromMailboxAddress(MailboxAddress mbAddress)
		{
			return new EmailAddress { Name = mbAddress.Name, Address = mbAddress.Address };
		}
	}
}
