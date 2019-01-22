using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebVella.Erp.Api.Models;

namespace WebVella.Erp.Web.Models
{
	public class AppEntity
	{
		[JsonProperty("entity")]
		public Entity Entity { get; set; } = new Entity();

		//If count is 0 means all entity pages are selected, the default is presented. If > 0 than the first in the list is the one to be presented
		//These are pages that have both AppId and EntityId set
		[JsonProperty("selected_pages")]
		public List<ErpPage> SelectedPages { get; set; } = new List<ErpPage>(); 

	}
}
