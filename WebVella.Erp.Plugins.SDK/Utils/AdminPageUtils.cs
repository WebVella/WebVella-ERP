using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Web.Models;

namespace WebVella.Erp.Plugins.SDK.Utils
{
	public static class AdminPageUtils
	{
		public static List<string> GetPageAdminSubNav(ErpPage page, string pageName = "") {
			if (page == null)
				return new List<string>();
			//pageName - currently a hack as admin pages are not ordinary pages
			return new List<string>()
			{
				$"<a href='/sdk/objects/page/r/{(page != null ? page.Id : Guid.Empty)}' class='btn btn-link btn-sm {(pageName == "details" || pageName == "manage" ? "active" : "")}'>Details</a>",
				$"<a href='/sdk/objects/page/r/{(page != null ? page.Id : Guid.Empty)}/generated-body' class='btn btn-link btn-sm {(pageName == "generated-body" || pageName == "manage-generated" ? "active" : "")}'>Generated Body {(!page.IsRazorBody ?"<span class=\"badge go-bkg-green go-white\">on</span>":"")}</a>",
				$"<a href='/sdk/objects/page/r/{(page != null ? page.Id : Guid.Empty)}/custom-body' class='btn btn-link btn-sm {(pageName == "custom-body" || pageName == "manage-custom" ? "active" : "")}'>Custom Body {(page.IsRazorBody ?"<span class=\"badge go-bkg-green go-white\">on</span>":"")}</a>",
				$"<a href='/sdk/objects/page/r/{(page != null ? page.Id : Guid.Empty)}/model' class='btn btn-link btn-sm {(pageName == "model" ? "active" : "")}'>Data Model</a>",
			};
		}

		public static List<string> GetAppAdminSubNav(App app, string pageName = "")
		{
			if (app == null)
				return new List<string>();
			//pageName - currently a hack as admin pages are not ordinary pages
			return new List<string>()
			{
				$"<a href='/sdk/objects/application/r/{(app != null ? app.Id : Guid.Empty)}' class='btn btn-link btn-sm  {(pageName == "details" || pageName == "manage" ? "active" : "")}'>Details</a>",
				$"<a href='/sdk/objects/application/r/{(app != null ? app.Id : Guid.Empty)}/sitemap' class='btn btn-link btn-sm {(pageName == "sitemap" || pageName == "sitemap-manage" ? "active" : "")}'>Sitemap</a>",
				$"<a href='/sdk/objects/application/r/{(app != null ? app.Id : Guid.Empty)}/pages' class='btn btn-link btn-sm {(pageName == "pages"  ? "active" : "")}'>Home Pages</a>"
			};
		}

		public static List<string> GetEntityAdminSubNav(Entity entity, string pageName = "")
		{
			if (entity == null)
				return new List<string>();
			//pageName - currently a hack as admin pages are not ordinary pages
			return new List<string>()
			{
				$"<a href='/sdk/objects/entity/r/{(entity != null ? entity.Id : Guid.Empty)}' class='btn btn-link btn-sm  {(pageName == "details" || pageName == "manage" ? "active" : "")}'>Details</a>",
				$"<a href='/sdk/objects/entity/r/{(entity != null ? entity.Id : Guid.Empty)}/rl/fields/l' class='btn btn-link btn-sm {(pageName == "fields" || pageName == "create-field" ? "active" : "")}'>Fields</a>",
				$"<a href='/sdk/objects/entity/r/{(entity != null ? entity.Id : Guid.Empty)}/rl/relations/l' class='btn btn-link btn-sm {(pageName == "relations" ? "active" : "")}'>Relations</a>",
				$"<a href='/sdk/objects/entity/r/{(entity != null ? entity.Id : Guid.Empty)}/rl/pages/l' class='btn btn-link btn-sm {(pageName == "pages" ? "active" : "")}'>Pages</a>",
				$"<a href='/sdk/objects/entity/r/{(entity != null ? entity.Id : Guid.Empty)}/rl/data/l' class='btn btn-link btn-sm {(pageName == "data" ? "active" : "")}'>Data</a>",
				$"<a href='/sdk/objects/entity/r/{(entity != null ? entity.Id : Guid.Empty)}/web-api' class='btn btn-link btn-sm {(pageName == "web-api" ? "active" : "")}'>Web Api</a>",

			};
		}

		public static List<string> GetJobAdminSubNav(string pageName = "")
		{
			//pageName - currently a hack as admin pages are not ordinary pages
			return new List<string>()
			{
				$"<a href='/sdk/server/job/l/plan' class='btn btn-link btn-sm  {(pageName == "plan" ? "active" : "")}'>Schedule Plans</a>",
				$"<a href='/sdk/server/job/l/list' class='btn btn-link btn-sm {(pageName == "job" ? "active" : "")}'>Background Jobs</a>",
			};
		}

		public static List<EntityRecord> GetFieldCards() {
			var FieldCards = new List<EntityRecord>();

			#region << Fill Field Cards >>
			{
				var card = new EntityRecord();
				card["label"] = "Auto increment number";
				card["class"] = "fas fa-sort-numeric-down";
				card["description"] = "If you need a automatically incremented number with each new record, this is the field you need. You can customize the display format also.";
				card["type"] = "1";
				FieldCards.Add(card);
			}
			{
				var card = new EntityRecord();
				card["label"] = "Checkbox";
				card["class"] = "far fa-check-square";
				card["description"] = "The simple on and off switch. This field allows you to get a True (checked) or False (unchecked) value.";
				card["type"] = "2";
				FieldCards.Add(card);
			}
			{
				var card = new EntityRecord();
				card["label"] = "Currency";
				card["class"] = "fas fa-dollar-sign";
				card["description"] = "A currency amount can be entered and will be represented in a suitable formatted way";
				card["type"] = "3";
				FieldCards.Add(card);
			}
			{
				var card = new EntityRecord();
				card["label"] = "Date";
				card["class"] = "far fa-calendar-alt";
				card["description"] = "A data pickup field, that can be later converting according to a provided pattern";
				card["type"] = "4";
				FieldCards.Add(card);
			}
			{
				var card = new EntityRecord();
				card["label"] = "Date & Time";
				card["class"] = "far fa-clock";
				card["description"] = "A date and time can be picked up and later presented according to a provided pattern";
				card["type"] = "5";
				FieldCards.Add(card);
			}
			{
				var card = new EntityRecord();
				card["label"] = "Email";
				card["class"] = "far fa-envelope";
				card["description"] = "An email can be entered by the user, which will be validated and presented accordingly";
				card["type"] = "6";
				FieldCards.Add(card);
			}
			{
				var card = new EntityRecord();
				card["label"] = "File";
				card["class"] = "far fa-file";
				card["description"] = "File upload field. Files will be stored within the system.";
				card["type"] = "7";
				FieldCards.Add(card);
			}
            {
                var card = new EntityRecord();
                card["label"] = "Geography";
                card["class"] = "fas fa-map-marked-alt";
                card["description"] = "Geography field (requires Postgis).  Your GeoJson or Text will be stored in the database as geography.";
                card["type"] = "21";
                FieldCards.Add(card);
            }
            {
				var card = new EntityRecord();
				card["label"] = "HTML";
				card["class"] = "fas fa-code";
				card["description"] = "Provides the ability of entering and presenting an HTML code. Supports multiple input languages.";
				card["type"] = "8";
				FieldCards.Add(card);
			}
			{
				var card = new EntityRecord();
				card["label"] = "Image";
				card["class"] = "far fa-image";
				card["description"] = "Image upload field. Images will be stored within the system";
				card["type"] = "9";
				FieldCards.Add(card);
			}
			{
				var card = new EntityRecord();
				card["label"] = "Textarea";
				card["class"] = "fas fa-paragraph";
				card["description"] = "A textarea for plain text input. Will be cleaned and stripped from any web unsafe content";
				card["type"] = "10";
				FieldCards.Add(card);
			}
			{
				var card = new EntityRecord();
				card["label"] = "Multiple select";
				card["class"] = "fas fa-check-double";
				card["description"] = "Multiple values can be selected from a provided list";
				card["type"] = "11";
				FieldCards.Add(card);
			}
			{
				var card = new EntityRecord();
				card["label"] = "Number";
				card["class"] = "fas fa-dice-six";
				card["description"] = "Only numbers are allowed. Leading zeros will be stripped.";
				card["type"] = "12";
				FieldCards.Add(card);
			}
			{
				var card = new EntityRecord();
				card["label"] = "Password";
				card["class"] = "fas fa-key";
				card["description"] = "This field is suitable for submitting passwords or other data that needs to stay encrypted in the database";
				card["type"] = "13";
				FieldCards.Add(card);
			}
			{
				var card = new EntityRecord();
				card["label"] = "Percent";
				card["class"] = "fas fa-percentage";
				card["description"] = "This will automatically present any number you enter as a percent value";
				card["type"] = "14";
				FieldCards.Add(card);
			}
			{
				var card = new EntityRecord();
				card["label"] = "Phone";
				card["class"] = "fas fa-phone";
				card["description"] = "Will allow only valid phone numbers to be submitted";
				card["type"] = "15";
				FieldCards.Add(card);
			}
			{
				var card = new EntityRecord();
				card["label"] = "Identifier GUID";
				card["class"] = "fas fa-fingerprint";
				card["description"] = "Very important field for any entity to entity relation and required by it";
				card["type"] = "16";
				FieldCards.Add(card);
			}
			{
				var card = new EntityRecord();
				card["label"] = "Select / Dropdown";
				card["class"] = "fas fa-caret-square-down";
				card["description"] = "One value can be selected from a provided list";
				card["type"] = "17";
				FieldCards.Add(card);
			}
			{
				var card = new EntityRecord();
				card["label"] = "Text";
				card["class"] = "fas fa-font";
				card["description"] = "A simple text field. One of the most needed field nevertheless";
				card["type"] = "18";
				FieldCards.Add(card);
			}
			{
				var card = new EntityRecord();
				card["label"] = "URL";
				card["class"] = "fas fa-globe-africa";
				card["description"] = "This field will validate local and static URLs. Will present them accordingly";
				card["type"] = "19";
				FieldCards.Add(card);
			}
			#endregion


			return FieldCards;
		}

		public static List<ErpRole> GetUserRoles() {
			var result = new List<ErpRole>();
			var allRoles = new SecurityManager().GetAllRoles().OrderBy(x=> x.Name).ToList();
			result.Add(allRoles.First(x => x.Id == SystemIds.GuestRoleId));
			result.Add(allRoles.First(x => x.Id == SystemIds.RegularRoleId));
			result.Add(allRoles.First(x => x.Id == SystemIds.AdministratorRoleId));
			result.AddRange(allRoles.FindAll(x => x.Id != SystemIds.GuestRoleId && x.Id != SystemIds.RegularRoleId && x.Id != SystemIds.AdministratorRoleId));

			return result;
		}
	}
}
