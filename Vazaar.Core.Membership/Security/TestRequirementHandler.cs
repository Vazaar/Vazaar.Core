// ---------------------------------------------------------------
// Copyright (c) Yasir Thite All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Microsoft.AspNetCore.Authorization;

namespace Vazaar.Core.Membership.Security
{
    public class TestRequirementHandler :
          AuthorizationHandler<TestRequirement>
    {
        protected override Task HandleRequirementAsync(
               AuthorizationHandlerContext context,
               TestRequirement requirement)
        {
            if (context.User.HasClaim(x => x.Type == "ViewTestPage" && x.Value == "true"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
