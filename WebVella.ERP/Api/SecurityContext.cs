using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Api
{

	public class SecurityContext : IDisposable
	{
		private static ErpUser systemUser = null;
		private static AsyncLocal<SecurityContext> current;
		private Stack<ErpUser> userStack;


		private SecurityContext()
		{
			userStack = new Stack<ErpUser>();
		}

		public static ErpUser SystemUser
		{
			get
			{
				if (systemUser == null)
					systemUser = new SecurityManager().GetUser(SystemIds.SystemUserId);

				return systemUser;
			}
		}

		public static ErpUser CurrentUser
		{
			get
			{
				if (current == null || current.Value == null)
					return null;

				return current.Value.userStack.Count > 0 ? current.Value.userStack.Peek() : null;
			}
		}

		public static bool IsUserInRole(params ErpRole[] roles)
		{
			var currentUser = CurrentUser;
			if (currentUser != null && roles != null && roles.Any())
				return IsUserInRole(roles.Select(x => x.Id).ToArray());

			return false;
		}

		public static bool IsUserInRole(params Guid[] roles)
		{
			var currentUser = CurrentUser;
			if (currentUser != null && roles != null && roles.Any())
				return currentUser.Roles.Any(x => roles.Any(z => z == x.Id));

			return false;
		}

		public static bool HasEntityPermission(EntityPermission permission, Entity entity, ErpUser user = null)
		{
			if (entity == null)
				throw new ArgumentNullException("entity");

			if (user == null)
				user = CurrentUser;

			if (user != null)
			{
				//system user has unlimited permissions :)
				if (user.Id == SystemIds.SystemUserId)
					return true;

				switch (permission)
				{
					case EntityPermission.Read:
						return user.Roles.Any(x => entity.RecordPermissions.CanRead.Any(z => z == x.Id));
					case EntityPermission.Create:
						return user.Roles.Any(x => entity.RecordPermissions.CanCreate.Any(z => z == x.Id));
					case EntityPermission.Update:
						return user.Roles.Any(x => entity.RecordPermissions.CanUpdate.Any(z => z == x.Id));
					case EntityPermission.Delete:
						return user.Roles.Any(x => entity.RecordPermissions.CanDelete.Any(z => z == x.Id));
					default:
						throw new NotSupportedException("Entity permission type is not supported");
				}
			}
			else
			{
				switch (permission)
				{
					case EntityPermission.Read:
						return entity.RecordPermissions.CanRead.Any(z => z == SystemIds.GuestRoleId);
					case EntityPermission.Create:
						return entity.RecordPermissions.CanCreate.Any(z => z == SystemIds.GuestRoleId);
					case EntityPermission.Update:
						return entity.RecordPermissions.CanUpdate.Any(z => z == SystemIds.GuestRoleId);
					case EntityPermission.Delete:
						return entity.RecordPermissions.CanDelete.Any(z => z == SystemIds.GuestRoleId);
					default:
						throw new NotSupportedException("Entity permission type is not supported");
				}
			}
		}

		public static IDisposable OpenScope(ErpUser user)
		{
			if (current == null)
			{
				current = new AsyncLocal<SecurityContext>();
				current.Value = new SecurityContext();
			}
			if (current.Value == null)
				current.Value = new SecurityContext();

			current.Value.userStack.Push(user);
			return current.Value;
		}

		public static IDisposable OpenSystemScope()
		{
			return OpenScope(SystemUser);
		}

		private static void CloseScope()
		{
			if (current != null && current.Value != null)
			{
				var stack = current.Value.userStack;
				if (stack.Count > 0)
				{
					var user = stack.Pop();
					if (stack.Count == 0)
						current.Value = null;
				}
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		public void Dispose(bool disposing)
		{
			if (disposing)
				CloseScope();
		}
	}
}
