using System;
using System.Security.Claims;
using System.Security.Principal;
using WebVella.ERP.Security;

namespace WebVella.ERP.Web.Security
{
    public class ErpIdentity : ClaimsIdentity
    {
            public override string AuthenticationType { get { return "WebVellaErp"; } }

            public override bool IsAuthenticated { get { return User != null && !String.IsNullOrWhiteSpace(User.Email); } }

            public User User { get; set; }
    }
}
