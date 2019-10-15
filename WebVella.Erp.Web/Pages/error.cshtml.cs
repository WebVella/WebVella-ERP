using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebVella.Erp.Site.Pages
{
	[AllowAnonymous]
    public class ErrorModel : PageModel
    {
        public IActionResult OnGet()
        {
			if (HttpContext.Request.Query.ContainsKey("401"))
				Request.HttpContext.Response.StatusCode = 401; //access denied;
			if (HttpContext.Request.Query.ContainsKey("404"))
				Request.HttpContext.Response.StatusCode = 404; //page not found;

			return Page();
        }

    }
}