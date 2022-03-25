// ---------------------------------------------------------------
// Copyright (c) Yasir Thite All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;

namespace Vazaar.Core.Api.Infrastructure.Provision.Brokers.Clouds
{
    public partial interface ICloudBroker
    {
        ValueTask<IAppServicePlan> CreatePlanAsync(
            string planName,
            IResourceGroup resourceGroup);
    }
}
