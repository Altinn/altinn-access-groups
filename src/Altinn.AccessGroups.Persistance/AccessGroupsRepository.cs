﻿using System.Data;
using Altinn.AccessGroups.Core;
using Altinn.AccessGroups.Core.Models;
using Altinn.AccessGroups.Persistance.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Altinn.AccessGroups.Persistance
{
    /// <summary>
    /// Class for handling interactions with the database.
    /// </summary>
    public class AccessGroupsRepository : IAccessGroupsRepository
    {
        private readonly string connectionString;
        private readonly ILogger logger;

        private readonly string insertCategoryFunc = "select * from accessgroup.insert_category(@_categoryCode, @_categoryType)";
        private readonly string getCategories = "SELECT categorycode, categorytype FROM accessgroup.category";

        private readonly string insertAccessGroupFunc = "select * from accessgroup.insert_accessgroup(@_accessGroupCode, @_accessGroupType, @_hidden, @_categoryCodes)";
        private readonly string updateAccessGroupFunc = "select * from accessgroup.update_accessgroup(@_accessGroupCode, @_accessGroupType, @_hidden, @_categoryCodes)";
        private readonly string getAccessGroups = "SELECT accessgroup.accessgroup.accessgroupcode, accessgrouptype, hidden, created, modified, array_agg(categorycode) categorycodes FROM accessgroup.accessgroup JOIN accessgroup.accessgroupcategory ON accessgroup.accessgroup.accessgroupcode = accessgroup.accessgroupcategory.accessgroupcode GROUP BY accessgroup.accessgroup.accessgroupcode";

        private readonly string insertExternalRelationshipFunc = "select * from accessgroup.insert_externalrelationship(@_ExternalSource, @_ExternalId, @_AccessGroupCode, @_UnitTypeFilter)";
        private readonly string getExternalRelationships = "SELECT externalsource, externalid, accessgroupcode, unittypefilter FROM accessgroup.externalrelationship";

        private readonly string insertAccessGroupCategoryFunc = "select * from accessgroup.insert_accessgroupcategory(@_accessgroupcode, @_categorycode)";
        private readonly string getAccessGroupCategories = "SELECT accessgroupcode, categorycode FROM accessgroup.accessgroupcategory";

        private readonly string insertTextResourceFunc = "select * from accessgroup.insert_textresource(@_textType, @_key, @_content, @_key, @_language, @_accessgroupcode, @_categorycode)";
        private readonly string getTextResources = "SELECT texttype, key, content, language, accessgroupcode, categorycode FROM accessgroup.textresource";

        private readonly string listGroupMembershipsByPartyIdFunc = "select * from accessgroup.select_accessgroupmembership_with_coveredbypartyid(@_offeredbypartyid, @_coveredbypartyid)";
        private readonly string listGroupMembershipsByUserIdFunc = "select * from accessgroup.select_accessgroupmembership_with_coveredbyuserid(@_offeredbypartyid, @_coveredbyuserid)";
        private readonly string insertGroupMembershipFunc = "select * from accessgroup.insert_accessgroupmembership(@_offeredByPartyid, @_coveredByUserId, @_coveredByPartyId, @_accessgroupcode, @_DelegationType, @_validto)";
        private readonly string deleteGroupMembershipFunc = "select * from accessgroup.delete_accessgroupmembership(@_coveredByUserId, @_coveredByPartyId, @_offeredByPartyId, @_delegationid, @_accessgroupcode, @_validto)";

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessGroupsRepository"/> class.
        /// </summary>
        /// <param name="postgresSettings">The postgreSQL configurations for AuthorizationDB.</param>
        /// <param name="logger">logger.</param>
        public AccessGroupsRepository(IOptions<PostgreSQLSettings> postgresSettings, ILogger<AccessGroupsRepository> logger)
        {
            this.logger = logger;
            this.connectionString = string.Format(
                postgresSettings.Value.ConnectionString,
                postgresSettings.Value.AuthorizationDbPwd);
            NpgsqlConnection.GlobalTypeMapper.MapEnum<CategoryType>("accessgroup.categorytype");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<AccessGroupType>("accessgroup.accessgrouptype");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<ExternalSource>("accessgroup.externalsource");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<ExternalSource>("accessgroup.textresourcetype");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<DelegationType>("accessgroup.delegationtype");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<TextResourceType>("accessgroup.textresourcetype");
        }

        /// <inheritdoc/>
        public async Task<AccessGroup> InsertAccessGroup(AccessGroup accessGroup)
        {
            try
            {
                await using NpgsqlConnection conn = new NpgsqlConnection(this.connectionString);
                await conn.OpenAsync();

                NpgsqlCommand pgcom = new NpgsqlCommand(this.insertAccessGroupFunc, conn);

                if (accessGroup.AccessGroupCode != null)
                {
                    pgcom.Parameters.AddWithValue("_accessGroupCode", accessGroup.AccessGroupCode);
                }
                else
                {
                    pgcom.Parameters.AddWithValue("_accessGroupCode", DBNull.Value);
                }

                pgcom.Parameters.AddWithValue("_accessGroupType", accessGroup.AccessGroupType);
                pgcom.Parameters.AddWithValue("_hidden", accessGroup.Hidden);

                if (accessGroup.Categories != null)
                {
                    pgcom.Parameters.AddWithValue("_categoryCodes", accessGroup.Categories);
                }
                else
                {
                    pgcom.Parameters.AddWithValue("_categoryCodes", DBNull.Value);
                }

                using NpgsqlDataReader reader = await pgcom.ExecuteReaderAsync();
                if (reader.Read())
                {
                    return GetAccessGroup(reader);
                }

                return new AccessGroup();
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "AccessGroups // AccessGroupsRepository // InsertAccessGroup // Exception");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<AccessGroup> UpdateAccessGroup(AccessGroup accessGroup)
        {
            try
            {
                await using NpgsqlConnection conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync();

                NpgsqlCommand pgcom = new NpgsqlCommand(updateAccessGroupFunc, conn);
                pgcom.Parameters.AddWithValue("_accessGroupCode", accessGroup.AccessGroupCode);
                pgcom.Parameters.AddWithValue("_accessGroupType", accessGroup.AccessGroupType);
                pgcom.Parameters.AddWithValue("_hidden", accessGroup.Hidden);
                pgcom.Parameters.AddWithValue("_categoryCodes", accessGroup.Categories);

                using NpgsqlDataReader reader = await pgcom.ExecuteReaderAsync();
                if (reader.Read())
                {
                    return GetAccessGroup(reader);
                }

                return null;
            }
            catch (Exception e)
            {
                logger.LogError(e, "AccessGroups // AccessGroupsRepository // InsertAccessGroup // Exception");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<AccessGroup>> GetAccessGroups()
        {
            try
            {
                await using NpgsqlConnection conn = new NpgsqlConnection(this.connectionString);
                await conn.OpenAsync();

                NpgsqlCommand pgcom = new NpgsqlCommand(this.getAccessGroups, conn);

                using NpgsqlDataReader reader = await pgcom.ExecuteReaderAsync();

                List<AccessGroup> accessGroups = new();
                while (reader.Read())
                {
                    accessGroups.Add(GetAccessGroup(reader));
                }

                return accessGroups;
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "AccessGroups // AccessGroupsRepository // GetAccessGroups // Exception");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<ExternalRelationship> InsertExternalRelationship(ExternalRelationship externalrelationship)
        {
            try
            {
                await using NpgsqlConnection conn = new NpgsqlConnection(this.connectionString);
                await conn.OpenAsync();

                NpgsqlCommand pgcom = new NpgsqlCommand(this.insertExternalRelationshipFunc, conn);
                pgcom.Parameters.AddWithValue("_ExternalSource", externalrelationship.ExternalSource);
                pgcom.Parameters.AddWithValue("_ExternalId", externalrelationship.ExternalId);
                pgcom.Parameters.AddWithValue("_AccessGroupCode", externalrelationship.AccessGroupCode);
                pgcom.Parameters.AddWithValue("_UnitTypeFilter", externalrelationship.UnitTypeFilter);

                using NpgsqlDataReader reader = await pgcom.ExecuteReaderAsync();
                if (reader.Read())
                {
                    return GetExternalRelationship(reader);
                }

                return new ExternalRelationship();
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "AccessGroups // AccessGroupsRepository // InsertExternalRelationship // Exception");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<ExternalRelationship>> GetExternalRelationships()
        {
            try
            {
                await using NpgsqlConnection conn = new NpgsqlConnection(this.connectionString);
                await conn.OpenAsync();

                NpgsqlCommand pgcom = new NpgsqlCommand(this.getExternalRelationships, conn);

                using NpgsqlDataReader reader = await pgcom.ExecuteReaderAsync();

                List<ExternalRelationship> externalRelationships = new();
                while (reader.Read())
                {
                    externalRelationships.Add(GetExternalRelationship(reader));
                }

                return externalRelationships;
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "AccessGroups // AccessGroupsRepository // GetExternalRelationships // Exception");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<Category> InsertCategory(Category category)
        {
            try
            {
                await using NpgsqlConnection conn = new NpgsqlConnection(this.connectionString);
                await conn.OpenAsync();

                NpgsqlCommand pgcom = new NpgsqlCommand(this.insertCategoryFunc, conn);
                pgcom.Parameters.AddWithValue("_CategoryCode", category.CategoryCode);
                pgcom.Parameters.AddWithValue("_CategoryType", category.CategoryType);

                using NpgsqlDataReader reader = await pgcom.ExecuteReaderAsync();
                if (reader.Read())
                {
                    return GetCategory(reader);
                }

                return new Category();
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "AccessGroups // AccessGroupsRepository // InsertCategory // Exception");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<Category>> GetCategories()
        {
            try
            {
                await using NpgsqlConnection conn = new NpgsqlConnection(this.connectionString);
                await conn.OpenAsync();

                NpgsqlCommand pgcom = new NpgsqlCommand(this.getCategories, conn);

                using NpgsqlDataReader reader = await pgcom.ExecuteReaderAsync();

                List<Category> categories = new();
                while (reader.Read())
                {
                    categories.Add(GetCategory(reader));
                }

                return categories;
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "AccessGroups // AccessGroupsRepository // GetCategories // Exception");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> InsertGroupMembership(GroupMembership membership)
        {
            try
            {
                await using NpgsqlConnection conn = new NpgsqlConnection(this.connectionString);
                await conn.OpenAsync();

                NpgsqlCommand pgcom = new NpgsqlCommand(this.insertGroupMembershipFunc, conn);

                if (membership.CoveredByUserId != null)
                {
                    pgcom.Parameters.AddWithValue("_coveredByUserId", membership.CoveredByUserId);
                }
                else
                {
                    pgcom.Parameters.AddWithValue("_coveredByUserId", DBNull.Value);
                }

                if (membership.CoveredByPartyId != null)
                {
                    pgcom.Parameters.AddWithValue("_coveredByPartyId", membership.CoveredByPartyId);
                }
                else
                {
                    pgcom.Parameters.AddWithValue("_coveredByPartyId", DBNull.Value);
                }

                pgcom.Parameters.AddWithValue("_offeredByPartyId", membership.OfferedByPartyId);

                if (membership.AccessGroupCode != null)
                {
                    pgcom.Parameters.AddWithValue("_accessgroupcode", membership.AccessGroupCode);
                }
                else
                {
                    pgcom.Parameters.AddWithValue("_accessgroupcode", DBNull.Value);
                }

                pgcom.Parameters.AddWithValue("_DelegationType", membership.DelegationType);

                if (membership.ValidTo != null)
                {
                    pgcom.Parameters.AddWithValue("_validto", membership.ValidTo);
                }
                else
                {
                    pgcom.Parameters.AddWithValue("validto", DBNull.Value);
                }

                using NpgsqlDataReader reader = await pgcom.ExecuteReaderAsync();
                if (reader.Read())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "AccessGroups // AccessGroupsRepository // InsertGroupMembership // Exception");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> RevokeGroupMembership(GroupMembership input)
        {
            try
            {
                await using NpgsqlConnection conn = new NpgsqlConnection(this.connectionString);
                await conn.OpenAsync();

                NpgsqlCommand pgcom = new NpgsqlCommand(this.deleteGroupMembershipFunc, conn);

                if (input.CoveredByUserId != null)
                {
                    pgcom.Parameters.AddWithValue("_coveredByUserId", input.CoveredByUserId);
                }
                else
                {
                    pgcom.Parameters.AddWithValue("_coveredByUserId", DBNull.Value);
                }

                if (input.CoveredByPartyId != null)
                {
                    pgcom.Parameters.AddWithValue("_coveredByPartyId", input.CoveredByPartyId);
                }
                else
                {
                    pgcom.Parameters.AddWithValue("_coveredByPartyId", DBNull.Value);
                }

                pgcom.Parameters.AddWithValue("_offeredByPartyId", input.OfferedByPartyId);
                pgcom.Parameters.AddWithValue("_delegationid", input.DelegationId);

                if (input.AccessGroupCode != null)
                {
                    pgcom.Parameters.AddWithValue("_accessgroupcode", input.AccessGroupCode);
                }
                else
                {
                    pgcom.Parameters.AddWithValue("_accessgroupcode", DBNull.Value);
                }

                if (input.ValidTo != null)
                {
                    pgcom.Parameters.AddWithValue("_validto", input.ValidTo);
                }
                else
                {
                    pgcom.Parameters.AddWithValue("validto", DBNull.Value);
                }

                using NpgsqlDataReader reader = await pgcom.ExecuteReaderAsync();
                if (reader.Read())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "AccessGroups // AccessGroupsRepository // DeleteGroupMembership // Exception");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<GroupMembership>> ListGroupmemberships(AccessGroupSearch search)
        {
            try
            {
                await using NpgsqlConnection conn = new NpgsqlConnection(this.connectionString);
                await conn.OpenAsync();

                NpgsqlCommand pgcom = new NpgsqlCommand();
                if (search.CoveredByPartyId.HasValue)
                {
                    pgcom = new NpgsqlCommand(this.listGroupMembershipsByPartyIdFunc, conn);
                    pgcom.Parameters.AddWithValue("_offeredbypartyid", search.OfferedByPartyId);
                    pgcom.Parameters.AddWithValue("_coveredbypartyid", search.CoveredByPartyId);
                }
                else if (search.CoveredByUserId.HasValue)
                {
                    pgcom = new NpgsqlCommand(this.listGroupMembershipsByUserIdFunc, conn);
                    pgcom.Parameters.AddWithValue("_offeredbypartyid", search.OfferedByPartyId);
                    pgcom.Parameters.AddWithValue("_coveredbyuserid", search.CoveredByUserId);
                }

                using NpgsqlDataReader reader = await pgcom.ExecuteReaderAsync();

                if (reader.Read())
                {
                    return GetGroupMemberships(reader);
                }
                else
                {
                    return new List<GroupMembership>();
                }
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "AccessGroups // AccessGroupsRepository // ListGroupMemberships // Exception");
                throw;
            }
        }

        private static AccessGroup GetAccessGroup(NpgsqlDataReader reader)
        {
            string[] categories = (string[])reader.GetValue("CategoryCodes");

            return new AccessGroup
            {
                AccessGroupCode = reader.GetValue<string>("AccessGroupCode"),
                AccessGroupType = reader.GetValue<AccessGroupType>("AccessGroupType"),
                Hidden = reader.GetValue<bool>("Hidden"),
                Created = reader.GetValue<DateTime>("Created"),
                Modified = reader.GetValue<DateTime>("Modified"),
                Categories = categories.ToList(),
            };
        }

        private static ExternalRelationship GetExternalRelationship(NpgsqlDataReader reader)
        {
            return new ExternalRelationship
            {
                ExternalSource = reader.GetValue<ExternalSource>("ExternalSource"),
                ExternalId = reader.GetValue<string>("ExternalId"),
                AccessGroupCode = reader.GetValue<string>("AccessGroupCode"),
                UnitTypeFilter = reader.GetValue<string>("UnitTypeFilter"),
            };
        }

        private static Category GetCategory(NpgsqlDataReader reader)
        {
            return new Category
            {
                CategoryCode = reader.GetValue<string>("CategoryCode"),
                CategoryType = reader.GetValue<CategoryType>("CategoryType"),
            };
        }

        private static GroupMembership GetGroupMembership(NpgsqlDataReader reader)
        {
            return new GroupMembership
            {
                CoveredByUserId = reader.GetValue<int>("userid"),
                CoveredByPartyId = reader.GetValue<int>("partyid"),
                OfferedByPartyId = reader.GetValue<int>("offeredbyparty"),
                DelegationId = reader.GetValue<int>("delegationid"),
                AccessGroupCode = reader.GetValue<string>("accessgroupcode"),
            };
        }

        private static List<GroupMembership> GetGroupMemberships(NpgsqlDataReader reader)
        {
            List<GroupMembership> groupMemberships = new List<GroupMembership>();

            while (reader.Read())
            {
                GroupMembership groupMembership = new GroupMembership
                {
                    CoveredByUserId = reader.GetValue<int>("userid"),
                    CoveredByPartyId = reader.GetValue<int>("partyid"),
                    OfferedByPartyId = reader.GetValue<int>("offeredbyparty"),
                    DelegationId = reader.GetValue<int>("delegationid"),
                    AccessGroupCode = reader.GetValue<string>("accessgroupcode"),
                    ValidTo = reader.GetValue<DateTime>("validto"),
                };

                groupMemberships.Add(groupMembership);
            }

            return groupMemberships;
        }
    }
}
