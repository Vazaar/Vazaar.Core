// ---------------------------------------------------------------
// Copyright (c) Yasir Thite All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using Vazaar.Core.Membership.Entities;

namespace Vazaar.Core.Api.Brokers.Storage
{
    public static class SeedRole
    {
        internal static Role[] Roles
        {
            get
            {
                return new Role[]
                {
                    new Role{ Id = Guid.NewGuid(), Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp =  DateTime.Now.Ticks.ToString()  },
                    new Role{ Id = Guid.NewGuid(), Name = "Customer", NormalizedName = "CUSTOMER", ConcurrencyStamp =  DateTime.Now.AddMinutes(1).Ticks.ToString()  }
                };
            }
        }
    }
}
