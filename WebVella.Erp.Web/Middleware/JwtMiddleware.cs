using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using System.Security.Claims;
using WebVella.Erp.Api;
using Microsoft.Net.Http.Headers;

namespace WebVella.Erp.Web.Middleware
{
	public class JwtMiddleware
	{
		private readonly RequestDelegate _next;

		public JwtMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			var token = await context.GetTokenAsync("access_token");
			if (string.IsNullOrWhiteSpace(token))
			{
				token = context.Request.Headers[HeaderNames.Authorization];
				if (!string.IsNullOrWhiteSpace(token))
				{
					if (token.Length <= 7)
						token = null;
					else
						token = token.Substring(7);
				}
				else
					token = null;
			}

			if (token != null)
			{
				try
				{
					var jwtToken = await WebVella.Erp.Web.Services.AuthService.GetValidSecurityTokenAsync(token);
					if (jwtToken != null && jwtToken.Claims.Any())
					{
						var nameIdentifier = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
						if (!string.IsNullOrWhiteSpace(nameIdentifier))
						{
							var user = new SecurityManager().GetUser(new Guid(nameIdentifier));
							context.Items["User"] = user;
						}
					}
				}
				catch
				{
					// do nothing if jwt validation fails
					// user is not attached to context so request won't have access to secure routes
				}

			}

			await _next(context);
		}
	}
}
