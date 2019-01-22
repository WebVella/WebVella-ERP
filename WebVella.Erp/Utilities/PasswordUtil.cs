using System;
using System.Security.Cryptography;
using System.Text;

namespace WebVella.Erp.Utilities
{
    public static class PasswordUtil
    {
        private static MD5 md5Hash = MD5.Create();

        internal static string GetMd5Hash(string input)
        {
			if (string.IsNullOrWhiteSpace(input))
				return string.Empty;

            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));

            return sBuilder.ToString();
        }

        internal static bool VerifyMd5Hash(string input, string hash)
        {
            string hashOfInput = GetMd5Hash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return (0 == comparer.Compare(hashOfInput, hash));
        }

    }
}

