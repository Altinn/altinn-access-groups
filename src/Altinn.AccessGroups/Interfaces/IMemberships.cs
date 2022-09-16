// <copyright file="IMemberships.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Altinn.AccessGroups.Core.Models;

namespace Altinn.AccessGroups.Interfaces
{
    /// <summary>
    /// Interface for membership related methods.
    /// </summary>
    public interface IMemberships
    {
        /// <summary>
        /// Method for listing GroupMemberships based on search parameters.
        /// </summary>
        /// <param name="search">AccessGroupSearch model containing the search parameters.</param>
        /// <returns>The response from the ListGroupMemberships method.</returns>
        Task<List<AccessGroup>> ListGroupMemberships(AccessGroupSearch search);

        /// <summary>
        /// Method for adding GroupMembership.
        /// </summary>
        /// <param name="input">The GroupMembership model to be inserted.</param>
        /// <returns>An ActionResult to determine the result.</returns>
        Task<bool> AddMembership(GroupMembership input);

        /// <summary>
        /// Method for revoking GroupMembership.
        /// </summary>
        /// <param name="input">The GroupMembership model to be deleted.</param>
        /// <returns>An ActionResult to determine the result.</returns>
        Task<bool> RevokeMembership(GroupMembership input);
    }
}
