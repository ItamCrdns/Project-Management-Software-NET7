using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CompanyPMO_.NET.Authorization
{
    public class EmployeeIdRequirementHandler : AuthorizationHandler<EmployeeIdRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployeeIdRequirement requirement)
        {
            var idClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);

            if (idClaim != null && idClaim.Value == requirement.EmployeeId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
