// ---------------------------------------------------------------
// Copyright (c) Yasir Thite All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.Sql.Fluent;
using Vazaar.Core.Api.Infrastructure.Provision.Brokers.Configurations;
using Vazaar.Core.Api.Infrastructure.Provision.Models.Configurations;
using Vazaar.Core.Api.Infrastructure.Provision.Models.Storages;
using Vazaar.Core.Api.Infrastructure.Provision.Services.Foundations.CloudManagements;

namespace Vazaar.Core.Api.Infrastructure.Provision.Services.Processings.CloudManagements
{
    public class CloudManagementProcessingService : ICloudManagementProcessingService
    {
        private readonly IConfigurationBroker configurationBroker;
        private readonly ICloudMangementService cloudMangementService;

        public CloudManagementProcessingService()
        {
            this.configurationBroker = new ConfigurationBroker();
            this.cloudMangementService = new CloudManagementService();
        }

        public async ValueTask ProcessAsync()
        {
            CloudManagementConfiguration configuration =
                this.configurationBroker.GetConfiguration();

            await ProvisionResourcesAsync(configuration);
            await DeprovisionResourcesAsync(configuration);
        }

        private async Task ProvisionResourcesAsync(CloudManagementConfiguration configuration)
        {
            string projectName = configuration.ProjectName;

            List<string> provisionEnvironments =
                RetrieveEnvironments(configuration.Up);

            foreach (string environment in provisionEnvironments)
            {
                IResourceGroup resourceGroup = await this.cloudMangementService
                    .ProvisionResourceGroupAsync(
                        projectName,
                        environment);

                IAppServicePlan appServicePlan = await this.cloudMangementService
                    .ProvisionAppServicePlanAsync(
                        projectName,
                        environment,
                        resourceGroup);

                ISqlServer sqlServer = await this.cloudMangementService
                    .ProvisionSqlServerAsync(
                        projectName,
                        environment,
                        resourceGroup);

                SqlDatabase sqlDatabase = await this.cloudMangementService
                    .ProvisionSqlDatabaseAsync(
                        projectName,
                        environment,
                        sqlServer);

                IWebApp webApp = await this.cloudMangementService
                    .ProvisionWebAppAsync(
                        projectName,
                        environment,
                        sqlDatabase.ConnectionString,
                        appServicePlan,
                        resourceGroup);
            }
        }

        private async Task DeprovisionResourcesAsync(
            CloudManagementConfiguration configuration)
        {
            string projectName = configuration.ProjectName;

            List<string> deprovisionEnvironments =
                RetrieveEnvironments(configuration.Down);

            foreach (string environment in deprovisionEnvironments)
            {
                await this.cloudMangementService.DeprovisionResourceGroupAsync(
                    projectName,
                    environment);
            }
        }

        private static List<string> RetrieveEnvironments(CloudAction cloudAction) =>
            cloudAction?.Environments ?? new List<string>();
    }
}
