// ---------------------------------------------------------------
// Copyright (c) Yasir Thite All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.Sql.Fluent;
using Vazaar.Core.Api.Infrastructure.Provision.Brokers.Clouds;
using Vazaar.Core.Api.Infrastructure.Provision.Brokers.Loggings;

namespace Vazaar.Core.Api.Infrastructure.Provision.Services.Foundations.CloudManagements
{
    public class CloudManagementService : ICloudMangementService
    {
        private readonly ICloudBroker cloudBroker;
        private readonly ILoggingBroker loggingBroker;

        public CloudManagementService()
        {
            this.cloudBroker = new CloudBroker();
            this.loggingBroker = new LoggingBroker();
        }


        public async ValueTask<IResourceGroup> ProvisionResourceGroupAsync(
            string projectName,
            string environment)
        {
            string resourceGroupName =
                $"{projectName}-RESOURCES-{environment}".ToUpper();

            this.loggingBroker.LogActivity(
                message: $"Provisioning {resourceGroupName} ...");

            IResourceGroup resourceGroup =
                await this.cloudBroker.CreateResourceGroupAsync(
                    resourceGroupName);

            this.loggingBroker.LogActivity(
                $"Provisioning {resourceGroupName} Completed.");

            return resourceGroup;
        }
        public async ValueTask<IAppServicePlan> ProvisionAppServicePlanAsync(
            string projectName,
            string environment,
            IResourceGroup resourceGroup)
        {
            string appServicePlanName = $"{projectName}-PLAN-{environment}".ToUpper();
            this.loggingBroker.LogActivity(message: $"Provisioning {appServicePlanName} ...");
            
            IAppServicePlan appServicePlan = await this.cloudBroker.CreatePlanAsync(
                appServicePlanName, resourceGroup);

            this.loggingBroker.LogActivity(message: $"Provisioning {appServicePlanName} complete.");

            return appServicePlan;
        }

        public async ValueTask<ISqlServer> ProvisionSqlServerAsync(
            string projectName,
            string environment,
            IResourceGroup resourceGroup)
        {
            string sqlServerName = $"{projectName}-dbserver-{environment}".ToLower();
            this.loggingBroker.LogActivity(message: $"Provisioning {sqlServerName}...");

            ISqlServer sqlServer = await this.cloudBroker.CreateSqlServerAsync(
                sqlServerName,
                resourceGroup);

            this.loggingBroker.LogActivity(message: $"Provisioning {sqlServerName} complete.");

            return sqlServer;
        }
    }
}
