using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebVella.Erp.Utilities
{
	public static class TextExtensions
	{
		public static string ToBase64(this Encoding encoding, string text)
		{
			if (text == null)
				return null;

			byte[] textAsBytes = encoding.GetBytes(text);
			return Convert.ToBase64String(textAsBytes);
		}

		public static bool TryParseBase64(this Encoding encoding, string encodedText, out string decodedText)
		{
			if (encodedText == null)
			{
				decodedText = null;
				return false;
			}

			try
			{
				byte[] textAsBytes = Convert.FromBase64String(encodedText);
				decodedText = encoding.GetString(textAsBytes);
				return true;
			}
			catch (Exception)
			{
				decodedText = null;
				return false;
			}
		}

		public static bool IsEmail(this string text)
		{
			try
			{
				var addr = new System.Net.Mail.MailAddress(text);
				return addr.Address == text;
			}
			catch
			{
				return false;
			}
		}
	}
}
