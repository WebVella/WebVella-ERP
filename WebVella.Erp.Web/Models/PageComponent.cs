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
<p>The variables are as follows:</p>
<table class=""table table-bordered table-sm"">
	<thead>
		<tr>
			<th width=""240"">variable name</th>
			<th>description</th>
		</tr>
	</thead>
	<tbody>
		<tr>
			<th><code>FULL_COMPONENT_NAME</code></th>
			<td>The full name of the component that inclused its namespace and class name. Ex. 'WebVella.Erp.Web.Components.PcForm'</td>
		</tr>
		<tr>
			<th><code>HTML_ID</code></th>
			<td>(Optional) The id of the component element in the page body. It is usually generated as a string starting with ""wv-"" plus the page node GUID  You can review it in the upper right corner of the options or help modals. If it is set to null, any component withing this page will process the event. If it is set to a GUID string, only the corresponding page component will handle it.</td>
		</tr>
		<tr>
			<th><code>ACTION_NAME</code></th>
			<td>Action name as defined in the components API. See below.</td>
		</tr>
		<tr>
			<th><code>PAYLOAD</code></th>
			<td>Js Object as defined in the components API. See below.</td>
		</tr>
	</tbody>
</table>		
		";

		#endregion

		public string HelpJsApiGeneralSection { get; private set; } = HELP_JSAPI_GENERAL_SECTION;
	}
}