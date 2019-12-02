using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace System.Web.Mvc
{
	public static class WvTaghelperExtension
	{
		public static IHtmlContent WvJsonRaw(this IHtmlHelper helper, string input)
		{

			return new HtmlString(helper.Raw(input).ToString().Replace("</script>", "</s\\cript>"));
		}
	}
}
