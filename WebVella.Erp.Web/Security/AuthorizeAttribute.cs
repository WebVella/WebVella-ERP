//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.AspNetCore.Mvc.Authorization;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace WebVella.Erp.Web.Security
//{

//    public class AuthorizeAttribute : ActionFilterAttribute
//    {
//        public AuthorizeAttribute()
//        {
//        }

//        public override void OnActionExecuting(ActionExecutingContext context)
//        {
//            var action = context.ActionDescriptor;

//            //check for allow anonymous and if found skip other checks
//            var allowAnonymousFound = action.FilterDescriptors.Any(x => x.Filter is AllowAnonymousFilter);
//            if (allowAnonymousFound)
//            {
//                base.OnActionExecuting(context);
//                return;
//            }

//            bool authenticated = IsAuthenticated(context);
//            if (!authenticated)
//            {
//                context.Result = new UnauthorizedResult();
//            }

//            base.OnActionExecuting(context);
//        }

//        /// <summary>
//        /// Check if request is authenticated
//        /// </summary>
//        /// <param name="context"></param>
//        /// <returns></returns>
//        private bool IsAuthenticated(ActionExecutingContext context)
//        {
//            var principal = context.HttpContext.User;

//            if (principal == null)
//                return false;

//            var identity = principal.Identity as ErpIdentity;
//            return identity != null;
//        }

//        /// <summary>
//        /// check if request is authorized
//        /// </summary>
//        /// <param name="context"></param>
//        /// <param name="roles"></param>
//        /// <returns></returns>
//        private bool IsAuthorized(ActionExecutingContext context, string[] roles)
//        {
//            var principal = context.HttpContext.User;

//            if (principal == null)
//                return false;

//            var identity = principal.Identity as ErpIdentity;
//            return identity != null;
//        }

//    }
//}//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.AspNetCore.Mvc.Authorization;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace WebVella.Erp.Web.Security
//{

//    public class AuthorizeAttribute : ActionFilterAttribute
//    {
//        public AuthorizeAttribute()
//        {
//        }

//        public override void OnActionExecuting(ActionExecutingContext context)
//        {
//            var action = context.ActionDescriptor;

//            //check for allow anonymous and if found skip other checks
//            var allowAnonymousFound = action.FilterDescriptors.Any(x => x.Filter is AllowAnonymousFilter);
//            if (allowAnonymousFound)
//            {
//                base.OnActionExecuting(context);
//                return;
//            }

//            bool authenticated = IsAuthenticated(context);
//            if (!authenticated)
//            {
//                context.Result = new UnauthorizedResult();
//            }

//            base.OnActionExecuting(context);
//        }

//        /// <summary>
//        /// Check if request is authenticated
//        /// </summary>
//        /// <param name="context"></param>
//        /// <returns></returns>
//        private bool IsAuthenticated(ActionExecutingContext context)
//        {
//            var principal = context.HttpContext.User;

//            if (principal == null)
//                return false;

//            var identity = principal.Identity as ErpIdentity;
//            return identity != null;
//        }

//        /// <summary>
//        /// check if request is authorized
//        /// </summary>
//        /// <param name="context"></param>
//        /// <param name="roles"></param>
//        /// <returns></returns>
//        private bool IsAuthorized(ActionExecutingContext context, string[] roles)
//        {
//            var principal = context.HttpContext.User;

//            if (principal == null)
//                return false;

//            var identity = principal.Identity as ErpIdentity;
//            return identity != null;
//        }

//    }
//}