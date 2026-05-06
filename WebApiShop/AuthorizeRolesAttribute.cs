using Microsoft.AspNetCore.Authorization;

namespace WebApiShop
{
    /// <summary>
    /// Custom authorize attribute that accepts one or more AppRoles constants.
    /// Usage: [AuthorizeRoles(AppRoles.Admin)]
    ///        [AuthorizeRoles(AppRoles.Admin, AppRoles.User)]
    /// </summary>
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params string[] roles)
        {
            Roles = string.Join(",", roles);
        }
    }
}
