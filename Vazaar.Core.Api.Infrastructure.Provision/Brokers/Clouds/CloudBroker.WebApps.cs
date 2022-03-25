// ---------------------------------------------------------------
// Copyright (c) Yasir Thite All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------


using System.Threading.Tasks;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.AppService.Fluent.Models;
using Microsoft.Azure.Management.ResourceManager.Fluent;

namespace Vazaar.Core.Api.Infrastructure.Provision.Brokers.Clouds
{
    public partial class CloudBroker
    {
        public async ValueTask<IWebApp> CreateWebAppAsync(
            string webAppName,
            string connectionString,
            IAppServicePlan appServicePlan,
            IResourceGroup resourceGroup)
        {
            return await this.azure.AppServices.WebApps
                .Define(webAppName)
                .WithExistingWindowsPlan(appServicePlan)
                .WithExistingResourceGroup(resourceGroup)
                .WithNetFrameworkVersion(NetFrameworkVersion.Parse("v7.0"))
                .WithConnectionString(
                    name: "DefaultConnection",
                    value: connectionString,
                    type: ConnectionStringType.SQLAzure)
                .CreateAsync();
        }

    }
}
