using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WebVella.Erp.Web.Services
{
	public class AuthService
	{
		private const double JWT_TOKEN_EXPIRY_DURATION_MINUTES = 1440;
		private const double JWT_TOKEN_FORCE_REFRESH_MINUTES = 120;

		private IServiceProvider serviceProvider;

		public AuthService(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		public ErpUser Authenticate(string email, string password)
		{
			var user = new SecurityManager().GetUser(email, password);
			if (user != null && user.Enabled)
			{
				var claims = new List<Claim>();
				claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
				claims.Add(new Claim(ClaimTypes.Email, user.Email));
				user.Roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role.ToString(), role.Name)));

				var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

				var authProperties = new AuthenticationProperties
				{
					AllowRefresh = true,
					ExpiresUtc = DateTimeOffset.UtcNow.AddYears(100),
					IsPersistent = false,
					IssuedUtc = DateTimeOffset.UtcNow,
				};

				IHttpContextAccessor httpContextAccesor = (IHttpContextAccessor)serviceProvider.GetService(typeof(IHttpContextAccessor));
				httpContextAccesor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
				return user;
			}
			else
				return null;
		}

		public void Logout()
		{
			IHttpContextAccessor httpContextAccesor = (IHttpContextAccessor)serviceProvider.GetService(typeof(IHttpContextAccessor));
			httpContextAccesor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
		}

		public static ErpUser GetUser(ClaimsPrincipal principal)
		{
			if (principal == null || principal.Claims == null || principal.Claims.Count() <= 0)
				return null;

			try
			{
				var claims = principal.Claims;
				Guid userId = new Guid(claims.Single(x => x.Type == ClaimTypes.NameIdentifier.ToString()).Value);
				return new SecurityManager().GetUser(userId);
			}
			catch
			{
				//when exception occur that means schema is changed and cookie is not valid
				return null;
			}
		}

		#region <--- JWT Token related methods --->

		public static async ValueTask<string> GetTokenAsync(string email, string password)
		{
			var user = new SecurityManager().GetUser(email?.Trim()?.ToLowerInvariant(), password?.Trim());
			if (user != null && user.Enabled)
			{
				var (tokenString, token) = await BuildTokenAsync(user);
				return tokenString;
			}
			throw new Exception("Invalid email or password");
		}

		public static async ValueTask<string> GetNewTokenAsync(string tokenString)
		{
			JwtSecurityToken jwtToken = await GetValidSecurityTokenAsync(tokenString);
			if (jwtToken == null)
				return null;

			List<Claim> claims = jwtToken.Claims.ToList();
			if (claims.Count == 0)
				return null;

			//validate for active user
			var nameIdentifier = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
			if (!string.IsNullOrWhiteSpace(nameIdentifier))
			{
				var user = new SecurityManager().GetUser(new Guid(nameIdentifier));
				if (user is not null && user.Enabled)
				{
					var (newTokenString, newToken) = await BuildTokenAsync(user);
					return newTokenString;
				}
			}

			return null;
		}

#pragma warning disable 1998
		public static async ValueTask<JwtSecurityToken> GetValidSecurityTokenAsync(string token)
		{
			var mySecret = Encoding.UTF8.GetBytes(ErpSettings.JwtKey);
			var mySecurityKey = new SymmetricSecurityKey(mySecret);
			var tokenHandler = new JwtSecurityTokenHandler();
			try
			{
				tokenHandler.ValidateToken(token,
				new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidIssuer = ErpSettings.JwtIssuer,
					ValidAudience = ErpSettings.JwtAudience,
					IssuerSigningKey = mySecurityKey,
				}, out SecurityToken validatedToken);
				return validatedToken as JwtSecurityToken;
			}
			catch (Exception)
			{
				return null;
			}
		}

		private static async ValueTask<(string, JwtSecurityToken)> BuildTokenAsync(ErpUser user)
		{
			var claims = new List<Claim>();
			claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
			claims.Add(new Claim(ClaimTypes.Email, user.Email));
			user.Roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role.ToString(), role.Name)));

			DateTime tokenRefreshAfterDateTime = DateTime.UtcNow.AddMinutes(JWT_TOKEN_FORCE_REFRESH_MINUTES);
			claims.Add(new Claim(type: "token_refresh_after", value: tokenRefreshAfterDateTime.ToBinary().ToString()));

			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ErpSettings.JwtKey));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
			var tokenDescriptor = new JwtSecurityToken(ErpSettings.JwtIssuer, ErpSettings.JwtAudience, claims,
						expires: DateTime.Now.AddMinutes(JWT_TOKEN_EXPIRY_DURATION_MINUTES), signingCredentials: credentials);
			return (new JwtSecurityTokenHandler().WriteToken(tokenDescriptor), tokenDescriptor);
		}
#pragma warning restore 1998



		#endregion

	}
}
