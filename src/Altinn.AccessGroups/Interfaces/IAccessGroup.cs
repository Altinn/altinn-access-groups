// <copyright file="IAccessGroup.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Altinn.AccessGroups.Core.Models;

namespace Altinn.AccessGroups.Interfaces
{
    /// <summary>
    /// Interface for methods used for AccessGroup.
    /// </summary>
    public interface IAccessGroup
    {
        /// <summary>
        /// Method for creating an AccessGroup.
        /// </summary>
        /// <param name="accessGroup">AccessGroup model for the accessgroup to insert.</param>
        /// <returns>The created accessgroup.</returns>
        Task<AccessGroup> CreateGroup(AccessGroup accessGroup);

        /// <summary>
        /// Method for updating AccessGroup.
        /// </summary>
        /// <param name="accessGroup">AccessGroup model for the AccessGroup to be updated.</param>
        /// <returns>An ActionResult to determine the result.</returns>
        Task<bool> UpdateGroup(AccessGroup accessGroup);

        /// <summary>
        /// Method for retrieving accessgroups.
        /// </summary>
        /// <returns>A list of all accessgroups.</returns>
        Task<List<AccessGroup>> GetAccessGroups();

        /// <summary>
        /// Method for importing AccessGroups.
        /// </summary>
        /// <param name="accessGroups">A list of AccessGroup models.</param>
        /// <returns>A list of AccessGroup models after the import.</returns>
        Task<List<AccessGroup>> ImportAccessGroups(List<AccessGroup> accessGroups);

        /// <summary>
        /// Method for importing external relationships based on a list.
        /// </summary>
        /// <param name="externalRelationships">A list of ExternalRelationship models.</param>
        /// <returns>A list of the created ExternalRelationship models.</returns>
        Task<List<ExternalRelationship>> ImportExternalRelationships(List<ExternalRelationship> externalRelationships);

        /// <summary>
        /// Method for retrieving a list of External Relationships.
        /// </summary>
        /// <returns>A list of ExternalRelationship models.</returns>
        Task<List<ExternalRelationship>> GetExternalRelationships();

        /// <summary>
        /// Method for importing categories.
        /// </summary>
        /// <param name="categories">A list of Category models.</param>
        /// <returns>A list of Category models after the import.</returns>
        Task<List<Category>> ImportCategories(List<Category> categories);

        /// <summary>
        /// A method for retrieving categories.
        /// </summary>
        /// <returns>A list of all categories.</returns>
        Task<List<Category>> GetCategories();
    }
}
