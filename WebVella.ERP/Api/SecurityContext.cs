using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using WebVella.ERP.Api.Models;

namespace WebVella.ERP.Api
{

    public static class SecurityContext
    {
        private static AsyncLocal<Stack<ErpUser>> userScopeStack;

        private static Stack<ErpUser> GetStack()
        {
            if (userScopeStack == null )
                userScopeStack = new AsyncLocal<Stack<ErpUser>>();
            
            if( userScopeStack.Value == null)
                userScopeStack.Value = new Stack<ErpUser>();

            return userScopeStack.Value;
        }

        public static ErpUser CurrentUser
        {
            get
            {
                var stack = GetStack();
                return stack.Count > 0 ? stack.Peek() : null;
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
            if (currentUser  != null && roles != null && roles.Any())
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
            Debug.WriteLine("SECURITY: OpenScope -> " + ( user != null ? user.Id.ToString() : "none" ) );
            GetStack().Push(user);
            return new Stopper();
        }

        private static void CloseScope()
        {
            var stack = GetStack();
            if (stack.Count > 0)
            {
                var user = stack.Pop();
                Debug.WriteLine("SECURITY: CloseScope -> " + (user != null ? user.Id.ToString() : "none"));
            }

            
        }

        private class Stopper : IDisposable
        {
            private bool _isDisposed;
            public void Dispose()
            {
                if (!_isDisposed)
                {
                    CloseScope();
                    _isDisposed = true;
                }
            }
        }
    }
}
