using Microsoft.AspNet.Http;
using Microsoft.Framework.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.ERP.Security;

namespace WebVella.ERP.Web.Security
{
    public class WebSecurityManager
    {
        public const string AUTH_TOKEN_KEY = "x-auth";
        public const int AUTH_REMEMBER_IDENTITY_DAYS = 30;
        public const int AUTH_CACHE_EXPIRATION_MINUTES = 5;
        public const int AUTH_TOKEN_EXPIRATION_DAYS = 2;
        public const int AUTH_TOKEN_EXTENDED_EXPIRATION_DAYS = 30;
        private static IMemoryCache cache;

        static WebSecurityManager()
        {
            var cacheOptions = new MemoryCacheOptions();
            cacheOptions.ExpirationScanFrequency = TimeSpan.FromMinutes(1);
            cache = new MemoryCache(cacheOptions);
        }

        public void Login(HttpContext context, Guid userId, DateTime? modifiedOn, bool rememberMe, IErpService service)
        {
            var identity = CreateIdentity(userId, service);

            if (identity == null)
                throw new Exception("Try to login with invalid user.");

            if (modifiedOn != identity.User.ModifiedOn)
                modifiedOn = identity.User.ModifiedOn;

            string token = AuthToken.Create(userId, modifiedOn, rememberMe).Encrypt();
            if (rememberMe)
            {
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Today.AddDays(AUTH_REMEMBER_IDENTITY_DAYS);
                context.Response.Cookies.Append(AUTH_TOKEN_KEY, token, options);
            }
            else
                context.Response.Cookies.Append(AUTH_TOKEN_KEY, token);

            context.User = new ErpPrincipal(identity);

            //TODO
            //var dataGateway = new DataGateway(service);
            //dataGateway.UpdateUserLastLoginTime(userId);
            //dataGateway.CreateLoginLog(identity.User, identity.Customer);
        }

        public void Logout(HttpContext context)
        {
            //delete is not working, so we append empty token
            //context.Response.Cookies.Delete(AUTH_TOKEN_KEY);
            context.Response.Cookies.Append(AUTH_TOKEN_KEY, "");
            context.User = null;
        }

        public void Authenticate(HttpContext context, IErpService service)
        {
            var tokenString = context.Request.Headers[AUTH_TOKEN_KEY];
            if (string.IsNullOrEmpty(tokenString))
                tokenString = context.Request.Cookies.Get(AUTH_TOKEN_KEY);

            if (tokenString != null)
            {
                AuthToken token = AuthToken.Decrypt(tokenString);
                if (token != null && token.Verify())
                {
                    var identity = GetIdentityFromCache(token.UserId);
                    if (identity == null)
                    {
                        identity = CreateIdentity(token.UserId, service);

                        //user has token, but identity cannot be created
                        //1. user is disabled 
                        //2. user is missing
                        if (identity == null)
                            return;

                        AddIdentityToCache(token.UserId, identity);
                    }

                    //when user is modified and issue old token
                    //1. we don't authenticate it
                    //2. clear identity from cache
                    if (identity.User.ModifiedOn != token.LastModified)
                        return;

                    context.User = new ErpPrincipal(identity);
                }
            }
        }

        internal ErpIdentity CreateIdentity(Guid? userId, IErpService service)
        {
            return null;
            //User userClaim = new User();
            //userClaim.Id = user.Id;
            //userClaim.FirstName = user.FirstName;
            //userClaim.LastName = user.LastName;
            //userClaim.Email = user.Email;
            //userClaim.ModifiedOn = user.ModifiedOn;
            //userClaim.Roles = user.Roles;

            //return CreateIdentity(userClaim);
        }

        internal ErpIdentity CreateIdentity(User user)
        {
            return new ErpIdentity { User = user };
        }

        internal static void AddIdentityToCache(Guid userId, ErpIdentity identity)
        {
            var options = new MemoryCacheEntryOptions();
            options.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));


            cache.Set(
                userId.ToString(),
                identity,
                options);
            // from bet4 to beta5 changed
            //context =>
            //{
            //	context.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            //	return identity;
            //});
        }

        internal static ErpIdentity GetIdentityFromCache(Guid userId)
        {
            ErpIdentity result = null;
            bool found = cache.TryGetValue<ErpIdentity>(userId.ToString(), out result);
            return result;
        }

        internal static void RemoveIdentityFromCache(Guid userId)
        {
            cache.Remove(userId.ToString());
        }
    }
}
