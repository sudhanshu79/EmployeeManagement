using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmployeeManagement.Security
{
    public class CanEditAdminsOwnRolesAndClaimsHandler :
        AuthorizationHandler<ManageAdminRolesAndClaimsRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageAdminRolesAndClaimsRequirement requirement)
        {
            var filterContext = context.Resource as AuthorizationFilterContext;

            if (filterContext == null)
            {
                return Task.CompletedTask;
            }

            string loggedInAdminId = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

            string adminIdToBeEdited = Convert.ToString(filterContext.RouteData.Values["id"]);

            if (!loggedInAdminId.ToLower().Equals(adminIdToBeEdited.ToLower()) && context.User.IsInRole("Admin")
                && context.User.HasClaim(c => c.Type.ToLower().Equals("edit role") && c.Value.Equals("true")))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
