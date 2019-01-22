//using System;
//using System.Security.Claims;
//using System.Security.Principal;
//using WebVella.Erp.Api.Models;

//namespace WebVella.Erp.Web.Security
//{
//    public class ErpIdentity : ClaimsIdentity
//    {
//        public override string AuthenticationType { get { return "WebVellaErp"; } }

//        public override bool IsAuthenticated { get { return User != null && !String.IsNullOrWhiteSpace(User.Email); } }

//        public override string Name
//        {
//            get
//            {
//                if (User != null)
//                {
//                    return User.Username;
//                }
//                return null;
//            }
//        }

//        public ErpUser User { get; set; }
//    }
//}
