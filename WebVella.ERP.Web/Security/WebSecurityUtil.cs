using Microsoft.AspNet.Http;
using Microsoft.Framework.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebVella.ERP.Api;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Web.Security
{
    public class WebSecurityUtil
    {
        internal const string AUTH_TOKEN_KEY = "erp-auth";
        internal const int AUTH_REMEMBER_IDENTITY_DAYS = 30;
        internal const int AUTH_CACHE_EXPIRATION_MINUTES = 5;
        internal const int AUTH_TOKEN_EXPIRATION_DAYS = 2;
        internal const int AUTH_TOKEN_EXTENDED_EXPIRATION_DAYS = 30;
        internal static IMemoryCache cache;

        static WebSecurityUtil()
        {
            var cacheOptions = new MemoryCacheOptions();
            cacheOptions.ExpirationScanFrequency = TimeSpan.FromMinutes(1);
            cache = new MemoryCache(cacheOptions);
        }

        public static string Login(HttpContext context, Guid userId, DateTime? modifiedOn, bool rememberMe, IErpService service)
        {
            var identity = CreateIdentity(userId, service);

            if (identity == null)
                throw new Exception("Try to login with invalid user.");

            if (modifiedOn != identity.User.ModifiedOn)
                modifiedOn = identity.User.ModifiedOn;



            ErpUser user = new SecurityManager(service).GetUser(userId);
            string token = AuthToken.Create(user, rememberMe).Encrypt();
            if (rememberMe)
            {
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Today.AddDays(AUTH_REMEMBER_IDENTITY_DAYS);
                context.Response.Cookies.Append(AUTH_TOKEN_KEY, token, options);
            }
            else
                context.Response.Cookies.Append(AUTH_TOKEN_KEY, token);

            context.User = new ErpPrincipal(identity);

            new SecurityManager(service).UpdateUserLastLoginTime(userId);

            return token;
        }

        public static void Logout(HttpContext context)
        {
            context.Response.Cookies.Append(AUTH_TOKEN_KEY, "");
            context.User = null;
        }

        public static void Authenticate(HttpContext context, IErpService service)
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
                    {
                        RemoveIdentityFromCache(identity.User.Id);

                        identity = CreateIdentity(token.UserId, service);

                        //user has token, but identity cannot be created
                        //1. user is disabled 
                        //2. user is missing
                        if (identity == null)
                            return;

                        AddIdentityToCache(token.UserId, identity);

                        return;
                    }

                    context.User = new ErpPrincipal(identity);
                }
            }
        }

        internal static void OpenScope(HttpContext context)
        {
            if (context == null)
                throw new NullReferenceException("context");

            if (context.User != null && context.User is ErpPrincipal)
            {
                var identity = (context.User as ErpPrincipal).Identity as ErpIdentity;
                if (identity != null)
                {
                    var scopeMarker = SecurityContext.OpenScope(identity.User);
                    context.Items.Add("erp_security_scope_marker", scopeMarker);
                }
            }
        }

        internal static void CloseScope(HttpContext context)
        {
            if (context == null)
                throw new NullReferenceException("context");

            IDisposable scopeMarker = context.Items["erp_security_scope_marker"] as IDisposable;
            if (scopeMarker != null)
                scopeMarker.Dispose();
        }

        internal static ErpIdentity CreateIdentity(Guid? userId, IErpService service)
        {
            SecurityManager secMan = new SecurityManager(service);
            ErpUser user = secMan.GetUser(userId.Value);

            if (user == null || !user.Enabled)
                return null;

            return new ErpIdentity { User = user };
        }

        internal static void AddIdentityToCache(Guid userId, ErpIdentity identity)
        {
            var options = new MemoryCacheEntryOptions();
            options.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            cache.Set(userId.ToString(), identity, options);
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
