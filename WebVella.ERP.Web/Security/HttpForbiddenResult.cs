using Microsoft.AspNet.Mvc;
using System.Net;

namespace WebVella.ERP.Web.Security
{
    public class HttpForbiddenResult : HttpStatusCodeResult
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
