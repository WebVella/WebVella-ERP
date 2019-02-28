using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace WebVella.Erp.Site.Pages
{
    public class ErrorModel : PageModel
    {
        public IActionResult OnGet()
        {
			if (HttpContext.Request.Query.ContainsKey("access_denied"))
				Request.HttpContext.Response.StatusCode = 401; //access denied;

			return Page();
        }

    }
}