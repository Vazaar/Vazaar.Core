// ---------------------------------------------------------------
// Copyright (c) Yasir Thite All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Vazaar.Core.Api.Infrastructure.Provision.Services.Processings.CloudManagements;

namespace Vazaar.Core.Api.Infrastructure.Provision
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ICloudManagementProcessingService cloudManagementProcessingService =
               new CloudManagementProcessingService();

            await cloudManagementProcessingService.ProcessAsync();
        }
    }
}