// ---------------------------------------------------------------
// Copyright (c) Yasir Thite All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------


using Microsoft.AspNetCore.Identity;

namespace Vazaar.Core.Membership.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public Role()
            : base()
        {
        }

        public Role(string roleName)
            : base(roleName)
        {
        }
    }
}
