using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace WebVella.Erp.Site.Pages
{
    public class ErrorModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }

    }
}