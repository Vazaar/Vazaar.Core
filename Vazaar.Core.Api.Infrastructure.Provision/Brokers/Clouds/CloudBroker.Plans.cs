// ---------------------------------------------------------------
// Copyright (c) Yasir Thite All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;

namespace Vazaar.Core.Api.Infrastructure.Provision.Brokers.Clouds
{
    public partial class CloudBroker
    {
        public async ValueTask<IAppServicePlan> CreatePlanAsync(
            string planName,
            IResourceGroup resourceGroup)
        {
            return await this.azure.AppServices.AppServicePlans
                .Define(planName)
                .WithRegion(Region.IndiaCentral)
                .WithExistingResourceGroup(resourceGroup)
                .WithPricingTier(PricingTier.StandardS1)
                .WithOperatingSystem(OperatingSystem.Windows)
                .CreateAsync();
        }
    }
}
