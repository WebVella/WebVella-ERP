#region <--- DIRECTIVES --->
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

#endregion

namespace WebVella.Erp.Utilities
{
    public class CryptoUtility
    {
        #region <--- Fields --->

        private const string defaultCryptKey = "BC93B776A42877CFEE808823BA8B37C83B6B0AD23198AC3AF2B5A54DCB647658";
        private static string cryptKey;
        private static readonly SymmetricAlgorithm cookieEncryptProvider = new DESCryptoServiceProvider();

        #endregion

        #region <--- Properties --->

        public static string CryptKey
        {
            get
            {
                if (string.IsNullOrEmpty(cryptKey))
                {
                    if (string.IsNullOrWhiteSpace(ErpSettings.EncriptionKey))
                        cryptKey = defaultCryptKey;

                    cryptKey = ErpSettings.EncriptionKey;
                }
                return cryptKey;
            }
        }

        #endregion

        #region <--- Methods --->

        /// <summary>
        /// 	Encrypts the text using default related key
        /// </summary>
        /// <param name="text"> The text. </param>
        /// <param name="algorithm"> The algorithm. </param>
        /// <returns> </returns>
        public static string EncryptText(string text, SymmetricAlgorithm algorithm)
        {
            return EncryptText(text, CryptKey, algorithm);
        }

        /// <summary>
        /// 	Decrypts the text using machine related key
        /// </summary>
        /// <param name="cypherText"> The cypher text. </param>
        /// <param name="algorithm"> The algorithm. </param>
        /// <returns> </returns>
        public static string DecryptText(string cypherText, SymmetricAlgorithm algorithm)
        {
            return DecryptText(cypherText, CryptKey, algorithm);
        }

        /// <summary>
        /// 	Encrypts the text using default related key
        /// </summary>
        /// <param name="inputData"> The input data. </param>
        /// <param name="algorithm"> The algorithm. </param>
        /// <returns> </returns>
        public static byte[] EncryptData(byte[] inputData, SymmetricAlgorithm algorithm)
        {
            return EncryptData(inputData, CryptKey, algorithm);
        }

        /// <summary>
        /// 	Decrypts the text using machine related key
        /// </summary>
        /// <param name="inputData"> The input data. </param>
        /// <param name="algorithm"> The algorithm. </param>
        /// <returns> </returns>
        public static byte[] DecryptData(byte[] inputData, SymmetricAlgorithm algorithm)
        {
            return DecryptData(inputData, CryptKey, algorithm);
        }

        /// <summary>
        /// 	Encrypts the text.
        /// </summary>
        /// <param name="text"> The text. </param>
        /// <param name="key"> The key. </param>
        /// <param name="algorithm"> The algorithm. </param>
        /// <returns> </returns>
        public static string EncryptText(string text, string key, SymmetricAlgorithm algorithm)
        {
            algorithm.Key = GetValidKey(key, algorithm);
            algorithm.IV = GetValidIV(key, algorithm.IV.Length);

            byte[] buffer = EncryptInternal(text, algorithm);
            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// 	Decrypts the text.
        /// </summary>
        /// <param name="cypherText"> The cypher text. </param>
        /// <param name="key"> The key. </param>
        /// <param name="algorithm"> The algorithm. </param>
        /// <returns> </returns>
        public static string DecryptText(string cypherText, string key, SymmetricAlgorithm algorithm)
        {
            algorithm.Key = GetValidKey(key, algorithm);
            algorithm.IV = GetValidIV(key, algorithm.IV.Length);

            byte[] inputBuffer = Convert.FromBase64String(cypherText);
            return DecryptInternal(inputBuffer, algorithm);
        }

        /// <summary>
        /// 	Encrypts the text.
        /// </summary>
        /// <param name="data"> The data. </param>
        /// <param name="key"> The key. </param>
        /// <param name="algorithm"> The algorithm. </param>
        /// <returns> </returns>
        public static byte[] EncryptData(byte[] data, string key, SymmetricAlgorithm algorithm)
        {
            algorithm.Key = GetValidKey(key, algorithm);
            algorithm.IV = GetValidIV(key, algorithm.IV.Length);

            return EncryptDataInternal(data, algorithm);
        }

        /// <summary>
        /// 	Decrypts the text.
        /// </summary>
        /// <param name="data"> The data. </param>
        /// <param name="key"> The key. </param>
        /// <param name="algorithm"> The algorithm. </param>
        /// <returns> </returns>
        public static byte[] DecryptData(byte[] data, string key, SymmetricAlgorithm algorithm)
        {
            algorithm.Key = GetValidKey(key, algorithm);
            algorithm.IV = GetValidIV(key, algorithm.IV.Length);

            return DecryptDataInternal(data, algorithm);
        }

        /// <summary>
        /// 	Decrypts the DES.
        /// </summary>
        /// <param name="encryptedValue"> The encrypted value. </param>
        /// <returns> </returns>
        public static string DecryptDES(string encryptedValue)
        {
            try
            {
                return encryptedValue;
                //there is a bug in Core 2 library for DES now
                //return DecryptText(encryptedValue, cookieEncryptProvider);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 	Encrypts the DES.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <returns> </returns>
        public static string EncryptDES(string value)
        {
            try
            {
                return value;
                //there is a bug in Core 2 library for DES now
                //return EncryptText(value, cookieEncryptProvider);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 	Computes MD5 hash value for specified input string
        /// </summary>
        /// <param name="inputString"> The input string. </param>
        /// <returns> </returns>
        public static string ComputeMD5Hash(string inputString)
        {
            byte[] bytes = (new UnicodeEncoding()).GetBytes(inputString);
            byte[] hashValue = (new MD5CryptoServiceProvider()).ComputeHash(bytes);
            return BitConverter.ToString(hashValue);
        }

        /// <summary>
        /// 	Computes the MD5 hash.
        /// </summary>
        /// <param name="inputString"> The input string. </param>
        /// <returns> </returns>
        public static byte[] ComputeMD5HashBytes(string inputString)
        {
            byte[] bytes = (new UnicodeEncoding()).GetBytes(inputString);
            return (new MD5CryptoServiceProvider()).ComputeHash(bytes);
        }

        /// <summary>
        /// 	Computes the odd M d5 hash.
        /// </summary>
        /// <param name="str"> The STR. </param>
        /// <returns> </returns>
        public static string ComputeOddMD5Hash(string str)
        {
            MD5 md5 = MD5.Create();
            byte[] dataMd5 = md5.ComputeHash(Encoding.Unicode.GetBytes(str));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dataMd5.Length; i++)
                sb.AppendFormat("{0:x2}", dataMd5[i]);
            return sb.ToString();
        }

        /// <summary>
        /// 	Computes the PHP like M d5 hash.
        /// </summary>
        /// <param name="text"> The text. </param>
        /// <returns> </returns>
        public static string ComputePhpLikeMD5Hash(string text)
        {
            byte[] textBytes = Encoding.Default.GetBytes(text);

            MD5CryptoServiceProvider cryptHandler;
            cryptHandler = new MD5CryptoServiceProvider();
            byte[] hash = cryptHandler.ComputeHash(textBytes);
            string ret = "";
            foreach (byte a in hash)
            {
                if (a < 16)
                    ret += "0" + a.ToString("x");
                else
                    ret += a.ToString("x");
            }
            return ret;
        }

        #endregion

        #region <--- Private Methods --->


        /// <summary>
        /// 	Gets the valid encode key.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <param name="encodeMethod"> The encode method. </param>
        /// <returns> </returns>
        private static byte[] GetValidKey(string key, SymmetricAlgorithm encodeMethod)
        {
            string result;
            if (encodeMethod.LegalKeySizes.Length > 0)
            {
                int size = encodeMethod.LegalKeySizes[0].MinSize;

                // key sizes are in bits
                while (key.Length * 8 > size &&
                       encodeMethod.LegalKeySizes[0].SkipSize > 0 &&
                       size < encodeMethod.LegalKeySizes[0].MaxSize)
                    size += encodeMethod.LegalKeySizes[0].SkipSize;

                result = key.Length * 8 > size ? key.Substring(0, (size / 8)) : key.PadRight(size / 8, ' ');
            }
            else
                result = key;

            return Encoding.ASCII.GetBytes(result);
        }

        /// <summary>
        /// 	Gets the valid encode IV.
        /// </summary>
        /// <param name="InitVector"> The init vector. </param>
        /// <param name="ValidLength"> Length of the valid. </param>
        /// <returns> </returns>
        private static byte[] GetValidIV(String InitVector, int ValidLength)
        {
            if (InitVector.Length > ValidLength)
                return Encoding.ASCII.GetBytes(InitVector.Substring(0, ValidLength));

            return Encoding.ASCII.GetBytes(InitVector.PadRight(ValidLength, ' '));
        }

        /// <summary>
        /// 	Encrypts the specified plain text.
        /// </summary>
        /// <param name="text"> The plain text. </param>
        /// <param name="algorithm"> The algorithm. </param>
        /// <returns> </returns>
        private static byte[] EncryptInternal(string text, SymmetricAlgorithm algorithm)
        {
            MemoryStream ms = new MemoryStream();
            CryptoStream encStream = new CryptoStream(ms, algorithm.CreateEncryptor(), CryptoStreamMode.Write);

            StreamWriter sw = new StreamWriter(encStream);
            sw.WriteLine(text);
            sw.Close();
            encStream.Close();

            byte[] buffer = ms.ToArray();
            ms.Close();

            return buffer;
        }

        /// <summary>
        /// 	Decrypts the specified cypher text.
        /// </summary>
        /// <param name="cypherText"> The cypher text. </param>
        /// <param name="algorithm"> The algorithm. </param>
        /// <returns> </returns>
        private static string DecryptInternal(byte[] cypherText, SymmetricAlgorithm algorithm)
        {
            MemoryStream ms = new MemoryStream(cypherText);
            CryptoStream encStream = new CryptoStream(ms, algorithm.CreateDecryptor(), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(encStream);

            string val = sr.ReadLine();
            sr.Close();
            encStream.Close();
            ms.Close();

            return val;
        }

        /// <summary>
        /// 	Encrypts the specified plain text.
        /// </summary>
        /// <param name="data"> The data. </param>
        /// <param name="algorithm"> The algorithm. </param>
        /// <returns> </returns>
        private static byte[] EncryptDataInternal(byte[] data, SymmetricAlgorithm algorithm)
        {
            MemoryStream ms = new MemoryStream();
            CryptoStream encStream = new CryptoStream(ms, algorithm.CreateEncryptor(), CryptoStreamMode.Write);
            encStream.Write(data, 0, data.Length);
            encStream.Close();

            byte[] buffer = ms.ToArray();

            return buffer;
        }

        /// <summary>
        /// 	Decrypts the specified cypher text.
        /// </summary>
        /// <param name="inputData"> The input data. </param>
        /// <param name="algorithm"> The algorithm. </param>
        /// <returns> </returns>
        private static byte[] DecryptDataInternal(byte[] inputData, SymmetricAlgorithm algorithm)
        {
            MemoryStream ms = new MemoryStream(inputData);
            CryptoStream encStream = new CryptoStream(ms, algorithm.CreateDecryptor(), CryptoStreamMode.Read);
            BinaryReader br = new BinaryReader(encStream);
            List<byte> data = new List<byte>();

            byte[] buffer;
            while ((buffer = br.ReadBytes(2048)).Length > 0)
                data.AddRange(buffer);
            encStream.Close();
            ms.Close();

            return data.ToArray();
        }

        #endregion
    }
}