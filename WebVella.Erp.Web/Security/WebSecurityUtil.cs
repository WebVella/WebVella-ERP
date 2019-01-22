//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Caching.Memory;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using WebVella.Erp.Api;
//using WebVella.Erp.Api.Models;

//namespace WebVella.Erp.Web.Security
//{
//    public class WebSecurityUtil
//    {
//        internal const string AUTH_TOKEN_KEY = "erp-auth";
//        internal const int AUTH_REMEMBER_IDENTITY_DAYS = 30;
//        internal const int AUTH_CACHE_EXPIRATION_MINUTES = 5;
//        internal const int AUTH_TOKEN_EXPIRATION_DAYS = 2;
//        internal const int AUTH_TOKEN_EXTENDED_EXPIRATION_DAYS = 30;
//        internal static IMemoryCache cache;

//        static WebSecurityUtil()
//        {
//            var cacheOptions = new MemoryCacheOptions();
//            cacheOptions.ExpirationScanFrequency = TimeSpan.FromMinutes(1);
//            cache = new MemoryCache(cacheOptions);
//        }

//        public static string Login(HttpContext context, Guid userId, DateTime? modifiedOn, bool rememberMe )
//        {
//            var identity = CreateIdentity(userId);

//            if (identity == null)
//                throw new Exception("Try to login with invalid user.");

//            if (modifiedOn != identity.User.ModifiedOn)
//                modifiedOn = identity.User.ModifiedOn;

//		    ErpUser user = new SecurityManager().GetUser(userId);
//            string token = AuthToken.Create(user, rememberMe).Encrypt();
//            if (rememberMe)
//            {
//                CookieOptions options = new CookieOptions();
//                options.Expires = DateTime.Today.AddDays(AUTH_REMEMBER_IDENTITY_DAYS);
//                context.Response.Cookies.Append(AUTH_TOKEN_KEY, token, options);
//            }
//            else
//                context.Response.Cookies.Append(AUTH_TOKEN_KEY, token);

//            context.User = new ErpPrincipal(identity);

//            new SecurityManager().UpdateUserLastLoginTime(userId);

//            return token;
//        }

//		public static bool LoginWithToken(HttpContext context, string authToken)
//		{
//			var query = EntityQuery.QueryAND(EntityQuery.QueryEQ("auth_token", authToken));
//			var result = new RecordManager().Find(new EntityQuery("user", "id,last_modified_on,auth_token_expires_on", query));

//			if (result.Success && (result.Object.Data != null && result.Object.Data.Any()))
//			{
//				Guid userId = (Guid)result.Object.Data[0]["id"];
//				DateTime? lastModifiedOn = (DateTime?)result.Object.Data[0]["last_modified_on"];
//				DateTime? tokenExpiresOn = (DateTime?)result.Object.Data[0]["auth_token_expires_on"];
//				if (tokenExpiresOn.HasValue && tokenExpiresOn.Value > DateTime.UtcNow)
//				{
//					var identity = CreateIdentity(userId);

//					if (identity == null)
//						return false;

//					ErpUser user = new SecurityManager().GetUser(userId);
//					string token = AuthToken.Create(user, false).Encrypt();
//					context.Response.Cookies.Append(AUTH_TOKEN_KEY, token);
//					context.User = new ErpPrincipal(identity);
//					new SecurityManager().UpdateUserLastLoginTime(userId);
//					return true;
//				}
//				else
//					return false;
//			}
//			else
//				return false;
//		}

//		public static void Logout(HttpContext context)
//        {
//            context.Response.Cookies.Append(AUTH_TOKEN_KEY, "");
//            context.User = null;
//        }

//        public static void Authenticate(HttpContext context)
//        {
//            string tokenString = context.Request.Headers[AUTH_TOKEN_KEY];
//			if (String.IsNullOrEmpty(tokenString)) 
//			{
//				var cookie = context.Request.Cookies.FirstOrDefault(c => c.Key == AUTH_TOKEN_KEY); 
//				tokenString = cookie.Value;
//			}

//			if (tokenString != null)
//            {
//                AuthToken token = AuthToken.Decrypt(tokenString);
//                if (token != null && token.Verify())
//                {
//                    var identity = GetIdentityFromCache(token.UserId);
//                    if (identity == null)
//                    {
//                        identity = CreateIdentity(token.UserId);

//                        //user has token, but identity cannot be created
//                        //1. user is disabled 
//                        //2. user is missing
//                        if (identity == null)
//                            return;

//                        AddIdentityToCache(token.UserId, identity);
//                    }

//                    //when user is modified and issue old token
//                    //1. we don't authenticate it
//                    //2. clear identity from cache
//                    if (identity.User.ModifiedOn != token.LastModified)
//                    {
//                        RemoveIdentityFromCache(identity.User.Id);

//                        identity = CreateIdentity(token.UserId);

//                        //user has token, but identity cannot be created
//                        //1. user is disabled 
//                        //2. user is missing
//                        if (identity == null)
//                            return;

//                        AddIdentityToCache(token.UserId, identity);

//                        return;
//                    }

//                    context.User = new ErpPrincipal(identity);
//                }
//            }
//        }

//        internal static ErpIdentity CreateIdentity(Guid? userId)
//        {
//            SecurityManager secMan = new SecurityManager();
//            ErpUser user = secMan.GetUser(userId.Value);

//            if (user == null || !user.Enabled)
//                return null;

//            return new ErpIdentity { User = user };
//        }

//        internal static void AddIdentityToCache(Guid userId, ErpIdentity identity)
//        {
//            var options = new MemoryCacheEntryOptions();
//            options.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
//            cache.Set(userId.ToString(), identity, options);
//        }

//        internal static ErpIdentity GetIdentityFromCache(Guid userId)
//        {
//            ErpIdentity result = null;
//            bool found = cache.TryGetValue<ErpIdentity>(userId.ToString(), out result);
//            return result;
//        }

//        internal static void RemoveIdentityFromCache(Guid userId)
//        {
//			if (userId == null)
//				return;

//            cache.Remove(userId.ToString());
//        }

//        internal static object GetCurrentUserPermissions(HttpContext context)
//        {
//            if (context == null)
//                throw new NullReferenceException("context");

//            ErpUser user = null;
//            if (context.User != null && context.User is ErpPrincipal)
//            {
//                var identity = (context.User as ErpPrincipal).Identity as ErpIdentity;
//                if (identity != null)
//                    user = identity.User;
//            }

//            EntityManager entMan = new EntityManager();
//            var entities = entMan.ReadEntities().Object;

//            List<object> permissions = new List<object>();
//            foreach (var entity in entities)
//            {
//                bool canRead = false;
//                bool canCreate = false;
//                bool canUpdate = false;
//                bool canDelete = false;

//                if (user != null)
//                {
//                    canRead = user.Roles.Any(x => entity.RecordPermissions.CanRead.Any(z => z == x.Id));
//                    canCreate = user.Roles.Any(x => entity.RecordPermissions.CanCreate.Any(z => z == x.Id));
//                    canUpdate = user.Roles.Any(x => entity.RecordPermissions.CanUpdate.Any(z => z == x.Id));
//                    canDelete = user.Roles.Any(x => entity.RecordPermissions.CanDelete.Any(z => z == x.Id));
//                }
//                else
//                {
//                    canRead = entity.RecordPermissions.CanRead.Any(z => z == SystemIds.GuestRoleId);
//                    canCreate = entity.RecordPermissions.CanCreate.Any(z => z == SystemIds.GuestRoleId);
//                    canUpdate = entity.RecordPermissions.CanUpdate.Any(z => z == SystemIds.GuestRoleId);
//                    canDelete = entity.RecordPermissions.CanDelete.Any(z => z == SystemIds.GuestRoleId);
//                }

//                if (canRead || canCreate || canUpdate || canDelete)
//                    permissions.Add(new
//                    {
//                        entityId = entity.Id,
//                        entityName = entity.Name,
//                        canRead = canRead,
//                        canCreate = canCreate,
//                        canUpdate = canUpdate,
//                        canDelete = canDelete
//                    });
//            }

//            return permissions;
//        }
//    }
//}
