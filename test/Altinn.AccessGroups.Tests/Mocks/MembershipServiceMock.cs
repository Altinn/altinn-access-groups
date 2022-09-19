using Altinn.AccessGroups.Core.Models;
using Altinn.AccessGroups.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altinn.AccessGroups.Tests.Mocks
{
    internal class MembershipServiceMock : IMemberships
    {
        public Task<bool> AddMembership(GroupMembership input)
        {
            throw new NotImplementedException();
        }

        public Task<List<AccessGroup>> ListGroupMemberships(AccessGroupSearch search)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RevokeMembership(GroupMembership input)
        {
            throw new NotImplementedException();
        }
    }
}
