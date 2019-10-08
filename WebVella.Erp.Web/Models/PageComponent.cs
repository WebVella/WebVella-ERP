using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace WebVella.Erp.Web.Models
{
	public abstract class PageComponent : ViewComponent
	{
		#region << HELP_JSAPI_GENERAL_SECTION >>
		const string HELP_JSAPI_GENERAL_SECTION = @"
<h2>General</h2>
<p>
	This component has a JS API via processing ERP events. To trigger one of the supported actions you need to dispatch an ErpEvent with a
	JS function call using the template <code>&lt;script>ErpEvent.DISPATCH('FULL_COMPONENT_NAME',{htmlId:HTML_ID,action:'ACTION_NAME',payload:PAYLOAD})&lt;/script></code>. 
	If there is no need to define htmlId or payload you can use a shorhand with only the action name <code>&lt;script>ErpEvent.DISPATCH('FULL_COMPONENT_NAME','ACTION_NAME')&lt;/script></code>
</p>
		
		";

		#endregion

		public string HelpJsApiGeneralSection { get; private set; } = HELP_JSAPI_GENERAL_SECTION;
	}
}