// ---------------------------------------------------------------
// Copyright (c) Yasir Thite All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Microsoft.AspNetCore.Authorization;

namespace Vazaar.Core.Membership.Security
{
    public class ApiRequirementHandler :
          AuthorizationHandler<ApiRequirement>
    {
        protected override Task HandleRequirementAsync(
               AuthorizationHandlerContext context,
               ApiRequirement requirement)
        {
            var claim = context.User.FindFirst("ViewTestPage");
            if (claim != null && bool.Parse(claim.Value))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
