using System.Security.Claims;

namespace WebVella.ERP.Web.Security
{
    public class ErpPrincipal : ClaimsPrincipal
    {
        public ErpPrincipal(ErpIdentity identity) : base()
        {
            AddIdentity(identity);
        }
    }
}
