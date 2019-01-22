using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebVella.Erp.Api;

namespace WebVella.Erp.Web.TagHelpers
{
    [HtmlTargetElement(Attributes = "wv-authorize")]
    [HtmlTargetElement(Attributes = "wv-authorize,erp-allow-roles")]
    [HtmlTargetElement(Attributes = "wv-authorize,erp-block-roles")]
    [HtmlTargetElement(Attributes = "wv-authorize,erp-allow-roles,erp-block-roles")]
    public class WvAuthorize : TagHelper
    {


		[HtmlAttributeName("erp-allow-roles")]
		public string ErpAllowRoles { get; set; } = "";

		[HtmlAttributeName("erp-block-roles")]
		public string ErpBlockRoles { get; set; } = "";

		public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var authorized = false;
            if (String.IsNullOrWhiteSpace(ErpAllowRoles) && !String.IsNullOrWhiteSpace(ErpBlockRoles)) {
                authorized = true;
            }


            if (String.IsNullOrWhiteSpace(ErpAllowRoles))
            {
                if (SecurityContext.CurrentUser != null) {
                    authorized = true;
                }
            }
            else
            {
                var rolesArray = ErpAllowRoles.Split(',');
                if (SecurityContext.CurrentUser != null)
                {
                    var userRoles = SecurityContext.CurrentUser.Roles;
                    foreach (var askedRole in rolesArray)
                    {
                        if (userRoles.Any(x => x.Name == askedRole))
                        {
                            authorized = true;
                            break;
                        }
                    }
                }
                else {
                    foreach (var askedRole in rolesArray)
                    {
                        if ("guest" == askedRole)
                        {
                            authorized = true;
                            break;
                        }
                    }
                }
            }

            if (!String.IsNullOrWhiteSpace(ErpBlockRoles))
            {
                var rolesArray = ErpBlockRoles.Split(',');
                if (SecurityContext.CurrentUser != null)
                {
                    var userRoles = SecurityContext.CurrentUser.Roles;
                    foreach (var askedRole in rolesArray)
                    {
                        if (userRoles.Any(x => x.Name == askedRole))
                        {
                            authorized = false;
                            break;
                        }
                    }
                }
                else {
                    foreach (var askedRole in rolesArray)
                    {
                        if ("guest" == askedRole)
                        {
                            authorized = false;
                            break;
                        }
                    }
                }
            }

            if (!authorized)
            {
                output.SuppressOutput();
            }

			return Task.CompletedTask;
        }

    }
}
