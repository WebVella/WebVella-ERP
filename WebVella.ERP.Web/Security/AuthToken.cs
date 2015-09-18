using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Net;
using WebVella.ERP.Api.Models;
using WebVella.ERP.Utilities;

namespace WebVella.ERP.Web.Security
{
    internal class AuthTokenWrapper
    {
        [JsonProperty(PropertyName = "token")]
        public AuthToken Token { get; set; }

    
    }

    internal class AuthToken
    {
        [JsonProperty(PropertyName = "lastModified")]
        public DateTime? LastModified { get; set; }

        [JsonProperty(PropertyName = "expirationDate")]
        public  DateTime ExpirationDate { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public Guid UserId { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        public AuthToken()
        {
        }

        private AuthToken( ErpUser user, DateTime expirationDate)
        {
            UserId = user.Id;
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;
            LastModified = user.ModifiedOn;
            ExpirationDate = expirationDate;
        }

        public static AuthToken Create(ErpUser user, bool extendedExpiration)
        {
            return new AuthToken(user, DateTime.UtcNow.AddDays(extendedExpiration
                                                                       ? WebSecurityUtil.AUTH_TOKEN_EXTENDED_EXPIRATION_DAYS
                                                                       : WebSecurityUtil.AUTH_TOKEN_EXPIRATION_DAYS));
        }

        public bool Verify()
        {
            return ExpirationDate > DateTime.UtcNow;
        }

        public string Encrypt()
        {
            var json = JsonConvert.SerializeObject(new TokenWrapper(this), new IsoDateTimeConverter());
            return WebUtility.UrlEncode(json);
        }

      
        public static AuthToken Decrypt(string data)
        {
            try
            {
                if (data == null)
                    return null;

                data = WebUtility.UrlDecode(data);
                var wrapper = JsonConvert.DeserializeObject<TokenWrapper>(data, new IsoDateTimeConverter());
                string tokenJson = CryptoUtility.DecryptDES(wrapper.Token);
                return JsonConvert.DeserializeObject<AuthToken>(tokenJson, new IsoDateTimeConverter());
            }
            catch
            {
                return null;
            }
        }

        private class TokenWrapper
        {
            [JsonProperty(PropertyName = "userId")]
            public Guid UserId { get; set; }

            [JsonProperty(PropertyName = "email")]
            public string Email { get; set; }

            [JsonProperty(PropertyName = "firstName")]
            public string FirstName { get; set; }

            [JsonProperty(PropertyName = "lastName")]
            public string LastName { get; set; }

            [JsonProperty(PropertyName = "token")]
            public string Token { get; set; }

            public TokenWrapper()
            {
            }

            public TokenWrapper(AuthToken token)
            {
                UserId = token.UserId;
                Email = token.Email;
                FirstName = token.FirstName;
                LastName = token.LastName;
                Token = CryptoUtility.EncryptDES(JsonConvert.SerializeObject(token, new IsoDateTimeConverter()));
            }
        }
    }
}