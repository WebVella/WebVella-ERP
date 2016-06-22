
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebVella.ERP.Web.Security
{
    public class HttpForbiddenResult : StatusCodeResult
    {
        public HttpForbiddenResult()
            : this(null)
        {
        }

        public HttpForbiddenResult(string statusDescription)
            : base((int)HttpStatusCode.Forbidden)
        {
        }
    }
}
