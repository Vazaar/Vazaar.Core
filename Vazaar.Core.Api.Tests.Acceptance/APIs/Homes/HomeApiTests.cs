// ---------------------------------------------------------------
// Copyright (c) Yasir Thite All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using FluentAssertions;
using System.Threading.Tasks;
using Vazaar.Core.Api.Tests.Acceptance.Brokers;
using Xunit;

namespace Vazaar.Core.Api.Tests.Acceptance.APIs.Homes
{
    [Collection(nameof(ApiTestCollection))]
    public class HomeApiTests
    {
        private readonly VazaarApiBroker vazaarApiBroker;

        public HomeApiTests(VazaarApiBroker vazaarApiBroker) =>
            this.vazaarApiBroker = vazaarApiBroker;

        [Fact]
        public async Task ShouldReturnHomeMessageAsync()
        {
            // given
            string expectedMessage =
                "Hello, Mario. The princesse is in another castle.";

            // when
            string actualMessage =
                await this.vazaarApiBroker.GetHomeMessageAsync();

            // then
            actualMessage.Should().BeEquivalentTo(expectedMessage);
        }
    }
}
