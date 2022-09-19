using Altinn.AccessGroups.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Altinn.AccessGroups.Core;
using Altinn.AccessGroups.Test.Mocks;
using Altinn.AccessGroups.Interfaces;
using Altinn.AccessGroups.Tests.Mocks;

namespace Altinn.AccessGroups.Test.Util
{
    public static class SetupUtil
    {
        public static HttpClient GetTestClient(
            CustomWebApplicationFactory<AccessGroupController> customFactory)
        {
            WebApplicationFactory<AccessGroupController> factory = customFactory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IAccessGroupsRepository, AccessGroupRepositoryMock>();
                    services.AddSingleton<IAccessGroup, AccessGroupServiceMock>();
                });
            });
            factory.Server.AllowSynchronousIO = true;
            return factory.CreateClient();
        }

        public static HttpClient GetMembershipControllerTestClient(
    CustomWebApplicationFactory<MembershipController> customFactory)
        {
            WebApplicationFactory<MembershipController> factory = customFactory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IAccessGroupsRepository, AccessGroupRepositoryMock>();
                    services.AddSingleton<IMemberships, MembershipServiceMock>();
                });
            });
            factory.Server.AllowSynchronousIO = true;
            return factory.CreateClient();
        }
    }
}
