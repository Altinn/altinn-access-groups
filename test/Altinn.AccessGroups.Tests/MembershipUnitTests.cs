using Altinn.AccessGroups.Controllers;
using Altinn.AccessGroups.Core.Models;
using Altinn.AccessGroups.Test;
using Altinn.AccessGroups.Test.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Altinn.AccessGroups.Tests
{
    public class MembershipUnitTests : IClassFixture<CustomWebApplicationFactory<MembershipController>>
    {
        private readonly CustomWebApplicationFactory<MembershipController> _factory;

        public MembershipUnitTests(CustomWebApplicationFactory<MembershipController> factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Test case: Calling the "Hello world" GET endpoint on the MembershipController
        /// Expected: returns 200 OK with content: "Hello world!"
        /// </summary>
        [Fact]
        public async Task Get_HelloWorld()
        {
            // Act
            HttpClient client = SetupUtil.GetMembershipControllerTestClient(_factory);
            string requestUri = "authorization/api/v1/Membership/authorization/api/v1/Membership";
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri)
            {
            };

            HttpResponseMessage response = await client.SendAsync(httpRequestMessage);
            string responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Hello world!", responseContent);
        }
    }
}
