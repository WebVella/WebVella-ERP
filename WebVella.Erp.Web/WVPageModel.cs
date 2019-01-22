using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebVella.Erp.Web
{
	public class WVPageModel : PageModel
	{
		public IActionResult OnGet()
		{
			return Page();
		}

	}
}
