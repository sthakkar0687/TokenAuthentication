using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TokenAuthentication.API.Authorization
{
    public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            if (!context.User.HasClaim(p => p.Type == ClaimTypes.DateOfBirth))
                return Task.CompletedTask;
            var dob = Convert.ToDateTime(context.User.FindFirst(p => p.Type == ClaimTypes.DateOfBirth).Value);
            var userAge = DateTime.Today.Year - dob.Year;
            if (dob > DateTime.Today.AddYears(-userAge))
                userAge--;
            if (userAge >= requirement.MinimumAge)
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
