using System.Threading.Tasks;
using Altinn.AccessGroups.Core;
using Altinn.AccessGroups.Core.Models;
using Altinn.AccessGroups.Interfaces;

namespace Altinn.AccessGroups.Services
{
    /// <summary>
    /// Service for the accessgroup-api.
    /// </summary>
    public class AccessGroupService : IAccessGroup
    {
        private readonly ILogger<IAccessGroup> logger;
        private readonly IAccessGroupsRepository accessGroupRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessGroupService"/> class.
        /// </summary>
        /// <param name="accessGroupRepository">The repository client for access groups.</param>
        /// <param name="logger">Logger instance.</param>
        public AccessGroupService(IAccessGroupsRepository accessGroupRepository, ILogger<IAccessGroup> logger)
        {
            this.accessGroupRepository = accessGroupRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Method for creating an AccessGroup.
        /// </summary>
        /// <param name="accessGroup">AccessGroup model for the accessgroup to insert.</param>
        /// <returns>The created accessgroup.</returns>
        public async Task<AccessGroup> CreateGroup(AccessGroup accessGroup)
        {
            return await this.accessGroupRepository.InsertAccessGroup(accessGroup);
        }

        /// <summary>
        /// Method for retrieving accessgroups.
        /// </summary>
        /// <returns>A list of all accessgroups.</returns>
        public async Task<List<AccessGroup>> GetAccessGroups()
        {
            return await this.accessGroupRepository.GetAccessGroups();
        }

        /// <summary>
        /// Method for creating an external relationship.
        /// </summary>
        /// <param name="externalRelationship">ExternalRelationship model for the external relationship to insert.</param>
        /// <returns>The created external relationship.</returns>
        public async Task<ExternalRelationship> CreateExternalRelationship(ExternalRelationship externalRelationship)
        {
            return await this.accessGroupRepository.InsertExternalRelationship(externalRelationship);
        }

        /// <summary>
        /// Method for importing external relationships based on a list.
        /// </summary>
        /// <param name="externalRelationships">A list of ExternalRelationship models.</param>
        /// <returns>A list of the created ExternalRelationship models.</returns>
        public async Task<List<ExternalRelationship>> ImportExternalRelationships(List<ExternalRelationship> externalRelationships)
        {
            List<ExternalRelationship> results = new();
            foreach (ExternalRelationship externalRelationship in externalRelationships)
            {
                results.Add(await this.CreateExternalRelationship(externalRelationship));
            }

            return results;
        }

        /// <summary>
        /// Method for retrieving a list of External Relationships.
        /// </summary>
        /// <returns>A list of ExternalRelationship models.</returns>
        public async Task<List<ExternalRelationship>> GetExternalRelationships()
        {
            return await this.accessGroupRepository.GetExternalRelationships();
        }

        /// <summary>
        /// Method for updating accessgroup.
        /// </summary>
        /// <param name="accessGroup">AccessGroup model representing the accessgroup that are to be updated.</param>
        /// <returns>A bool indicating whether or not the update was successful or not.</returns>
        /// <exception cref="NotImplementedException">Returns an exception indicating that the method is not yet implemented.</exception>
        public async Task<AccessGroup> UpdateGroup(AccessGroup accessGroup)
        {
            return await this.accessGroupRepository.UpdateAccessGroup(accessGroup);
        }

        /// <summary>
        /// Method for importing AccessGroups.
        /// </summary>
        /// <param name="accessGroups">A list of AccessGroup models.</param>
        /// <returns>A list of AccessGroup models after the import.</returns>
        public async Task<List<AccessGroup>> ImportAccessGroups(List<AccessGroup> accessGroups)
        {
            List<AccessGroup> result = new();
            foreach (AccessGroup accessGroup in accessGroups)
            {
                result.Add(await this.accessGroupRepository.InsertAccessGroup(accessGroup));
            }

            return result;
        }

        /// <summary>
        /// Method for importing categories.
        /// </summary>
        /// <param name="categories">A list of Category models.</param>
        /// <returns>A list of Category models after the import.</returns>
        public async Task<List<Category>> ImportCategories(List<Category> categories)
        {
            List<Category> result = new();
            foreach (Category category in categories)
            {
                result.Add(await this.accessGroupRepository.InsertCategory(category));
            }

            return result;
        }

        /// <summary>
        /// A method for retrieving categories.
        /// </summary>
        /// <returns>A list of all categories.</returns>
        public async Task<List<Category>> GetCategories()
        {
            return await this.accessGroupRepository.GetCategories();
        }
    }
}
