// ---------------------------------------------------------------
// Copyright (c) Yasir Thite All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Microsoft.Azure.Management.Sql.Fluent;

namespace Vazaar.Core.Api.Infrastructure.Provision.Models.Storages
{
    public class SqlDatabase
    {
        public ISqlDatabase Database { get; set; }
        public string ConnectionString { get; set; }
    }
}
