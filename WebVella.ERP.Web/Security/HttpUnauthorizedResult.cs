using Microsoft.AspNet.Mvc;
using System.Net;

namespace WebVella.ERP.Web.Security
{
    public class HttpUnauthorizedResult : HttpStatusCodeResult
    {
        public HttpUnauthorizedResult()
            : this(null)
        {
        }

        public HttpUnauthorizedResult(string statusDescription)
            : base((int)HttpStatusCode.Unauthorized)
        {
        }
    }
}

