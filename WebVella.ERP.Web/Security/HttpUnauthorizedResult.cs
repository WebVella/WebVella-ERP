using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebVella.ERP.Web.Security
{
    public class HttpUnauthorizedResult : StatusCodeResult
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

