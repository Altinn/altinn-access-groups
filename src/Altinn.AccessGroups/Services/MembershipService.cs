// <copyright file="MembershipService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Altinn.AccessGroups.Core;
using Altinn.AccessGroups.Core.Models;
using Altinn.AccessGroups.Interfaces;
using Authorization.Platform.Authorization.Models;

namespace Altinn.AccessGroups.Services
{
    /// <summary>
    /// Service for the membership-api.
    /// </summary>
    public class MembershipService : IMemberships
    {
        private readonly ILogger<IMemberships> logger;
        private readonly IAccessGroupsRepository accessGroupRepository;
        private readonly IAccessGroup accessGroups;
        private readonly IAltinnRolesClient altinnRoles;

        /// <summary>
        /// Initializes a new instance of the <see cref="MembershipService"/> class.
        /// </summary>
        /// <param name="accessGroupRepository">The repository client for access groups.</param>
        /// <param name="accessGroups">The access group service implementation.</param>
        /// <param name="altinnRoles">The Altinn role integration client.</param>
        /// <param name="logger">Logger instance.</param>
        public MembershipService(IAccessGroupsRepository accessGroupRepository, IAccessGroup accessGroups, IAltinnRolesClient altinnRoles, ILogger<IMemberships> logger)
        {
            this.accessGroupRepository = accessGroupRepository;
            this.accessGroups = accessGroups;
            this.altinnRoles = altinnRoles;
            this.logger = logger;
        }

        /// <summary>
        /// Inserts a new Groupmembership based on input.
        /// </summary>
        /// <param name="input">Groupmembership model to insert.</param>
        /// <returns>A bool indicating if the insert was successful.</returns>
        public Task<bool> AddMembership(GroupMembership input)
        {
            Task<bool> result = this.accessGroupRepository.InsertGroupMembership(input);
            return result;
        }

        /// <summary>
        /// Method for listing Groupmemberships based on AccessGroupSearch model.
        /// </summary>
        /// <param name="search">AccessGroupSearch model for specifying which GroupMemberships to retrieve.</param>
        /// <returns>The Groupmemberships that corresponds to the search parameters.</returns>
        public async Task<List<AccessGroup>> ListGroupMemberships(AccessGroupSearch search)
        {
            if (!search.CoveredByUserId.HasValue)
            {
                throw new NotImplementedException();
            }

            List<Role> erRoles = await this.altinnRoles.GetDecisionPointRolesForUser((int)search.CoveredByUserId, search.OfferedByPartyId);

            List<ExternalRelationship> externalRelationships = await this.accessGroups.GetExternalRelationships();
            List<GroupMembership> groupMemberships = await this.accessGroupRepository.ListGroupmemberships(search);
            List<AccessGroup> accessGroups = await this.accessGroups.GetAccessGroups();

            HashSet<string> accessGroupCodes = externalRelationships.Where(extRel => erRoles.Any(erRole => erRole.Value == extRel.ExternalId)).Select(extRel => extRel.AccessGroupCode).ToHashSet();
            if (groupMemberships != null)
            {
                accessGroupCodes.UnionWith(groupMemberships?.Where(grpMem => grpMem.CoveredByUserId == search.CoveredByUserId || grpMem.CoveredByPartyId == search.CoveredByPartyId).Select(grpMem => grpMem.AccessGroupCode).ToHashSet());
            }

            return accessGroups.Where(ag => accessGroupCodes.Contains(ag.AccessGroupCode)).ToList();
        }

        /// <summary>
        /// Method for revoking a membership by deleting it from the membership table and storing the old data in the membershiphistory table.
        /// </summary>
        /// <param name="input">GroupMembership model representing the groupmembership to be deleted.</param>
        /// <returns>A bool indicating whether or not the revoke was successful.</returns>
        public async Task<bool> RevokeMembership(GroupMembership input)
        {
            return await this.accessGroupRepository.RevokeGroupMembership(input);
        }
    }
}
