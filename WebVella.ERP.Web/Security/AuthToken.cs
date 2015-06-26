using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using WebVella.ERP.Utilities;

namespace WebVella.ERP.Web.Security
{
    internal class AuthToken
    {
        public Guid UserId { get; set; }
        public DateTime? LastModified { get; set; }
        public DateTime ExpirationDate { get; set; }

        public AuthToken()
        {
        }

        private AuthToken(Guid userId, DateTime? lastModified, DateTime expirationDate)
        {
            UserId = userId;
            LastModified = lastModified;
            ExpirationDate = expirationDate;
        }

        public bool Verify()
        {
            return ExpirationDate > DateTime.UtcNow;
        }

        public string Encrypt()
        {
            var json = JsonConvert.SerializeObject(this, new IsoDateTimeConverter());
            return CryptoUtility.EncryptDES(json);
        }

        public static AuthToken Create(Guid userId, DateTime? modifiedOn, bool extendedExpiration)
        {
            return null;
            //return new AuthToken(userId, modifiedOn, DateTime.UtcNow.AddDays(extendedExpiration
            //                                                           ? SecurityContext.AUTH_TOKEN_EXTENDED_EXPIRATION_DAYS
            //                                                           : SecurityContext.AUTH_TOKEN_EXPIRATION_DAYS));
        }

        public static AuthToken Decrypt(string data)
        {
            try
            {
                string json = CryptoUtility.DecryptDES(data);
                if (json == null)
                    return null;
                return JsonConvert.DeserializeObject<AuthToken>(json, new IsoDateTimeConverter());
            }
            catch
            {
                return null;
            }
        }
    }
}