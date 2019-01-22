//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using WebVella.Erp.Api.Models;
//using WebVella.Erp.Utilities;

//namespace WebVella.Erp.Web.Security
//{
//    internal class AuthTokenWrapper
//    {
//        [JsonProperty(PropertyName = "token")]
//        public AuthToken Token { get; set; }

    
//    }

//    internal class AuthToken
//    {
//        [JsonProperty(PropertyName = "last_modified_on")]
//        public DateTime? LastModified { get; set; }

//        [JsonProperty(PropertyName = "expiration_date")]
//        public  DateTime ExpirationDate { get; set; }

//        [JsonProperty(PropertyName = "id")]
//        public Guid UserId { get; set; }

//        [JsonProperty(PropertyName = "email")]
//        public string Email { get; set; }

//        [JsonProperty(PropertyName = "username")]
//        public string Username { get; set; }

//        [JsonProperty(PropertyName = "first_name")]
//        public string FirstName { get; set; }

//        [JsonProperty(PropertyName = "last_name")]
//        public string LastName { get; set; }

//        [JsonProperty(PropertyName = "image")]
//        public string Image { get; set; }

//        [JsonProperty(PropertyName = "roles")]
//        public List<ErpRole> Roles { get; set; }

//        public AuthToken()
//        {
//        }

//        private AuthToken( ErpUser user, DateTime expirationDate)
//        {
//            UserId = user.Id;
//            Email = user.Email;
//			Username = user.Username;
//            FirstName = user.FirstName;
//            LastName = user.LastName;
//            LastModified = user.ModifiedOn;
//            ExpirationDate = expirationDate;
//            Roles = user.Roles;
//            Image = user.Image;
//        }

//        public static AuthToken Create(ErpUser user, bool extendedExpiration)
//        {
//            return new AuthToken(user, DateTime.UtcNow.AddDays(extendedExpiration
//                                                                       ? WebSecurityUtil.AUTH_TOKEN_EXTENDED_EXPIRATION_DAYS
//                                                                       : WebSecurityUtil.AUTH_TOKEN_EXPIRATION_DAYS));
//        }

//        public bool Verify()
//        {
//            return ExpirationDate > DateTime.UtcNow;
//        }

//        public string Encrypt()
//        {
//            var json = JsonConvert.SerializeObject(new TokenWrapper(this), new IsoDateTimeConverter());
//            return WebUtility.UrlEncode(json);
//        }

      
//        public static AuthToken Decrypt(string data)
//        {
//            try
//            {
//                if (data == null)
//                    return null;

//                data = WebUtility.UrlDecode(data);
//                var wrapper = JsonConvert.DeserializeObject<TokenWrapper>(data, new IsoDateTimeConverter());
//                string tokenJson = CryptoUtility.DecryptDES(wrapper.Token);
//                return JsonConvert.DeserializeObject<AuthToken>(tokenJson, new IsoDateTimeConverter());
//            }
//            catch
//            {
//                return null;
//            }
//        }

//        private class TokenWrapper
//        {
//            [JsonProperty(PropertyName = "id")]
//            public Guid UserId { get; set; }

//            [JsonProperty(PropertyName = "email")]
//            public string Email { get; set; }

//            [JsonProperty(PropertyName = "username")]
//            public string Username { get; set; }

//            [JsonProperty(PropertyName = "first_name")]
//            public string FirstName { get; set; }

//            [JsonProperty(PropertyName = "last_name")]
//            public string LastName { get; set; }

//            [JsonProperty(PropertyName = "image")]
//            public string Image { get; set; }

//            [JsonProperty(PropertyName = "roles")]
//            public List<Guid> Roles { get; set; }

//            [JsonProperty(PropertyName = "token")]
//            public string Token { get; set; }

//            public TokenWrapper()
//            {
//            }

//            public TokenWrapper(AuthToken token)
//            {
//                UserId = token.UserId;
//                Email = token.Email;
//				Username = token.Username;
//                FirstName = token.FirstName;
//                LastName = token.LastName;
//                Image = token.Image;
//                Roles = new List<Guid>();
//                Roles.AddRange(token.Roles.Select(x => x.Id));
//                Token = CryptoUtility.EncryptDES(JsonConvert.SerializeObject(token, new IsoDateTimeConverter()));
//            }
//        }
//    }
//}