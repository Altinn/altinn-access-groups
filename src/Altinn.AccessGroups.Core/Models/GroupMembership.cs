using System.Text.Json.Serialization;

namespace Altinn.AccessGroups.Core.Models
{
    /// <summary>
    /// Model representation of a group memmbership
    /// </summary>
    public class GroupMembership
    {
        /// <summary>
        /// The user id of the user having received group membership
        /// </summary>
        public int? CoveredByUserId { get; set; }

        /// <summary>
        /// The party id of the organization having received group membership
        /// </summary>
        public int? CoveredByPartyId { get; set; }

        /// <summary>
        /// The party id of the party which have given the group membership
        /// </summary>
        public int OfferedByPartyId { get; set; }

        /// <summary>
        /// The delegation id reference
        /// </summary>
        public int DelegationId { get; set; }

        /// <summary>
        /// The access group code the membership is valid for
        /// </summary>
        public string? AccessGroupCode { get; set; }

        /// <summary>
        /// The type of delegation performed which resulted in this group membership being created
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DelegationType DelegationType { get; set; }

        /// <summary>
        /// Expiration date for the group membership
        /// </summary>
        public DateTime? ValidTo { get; set; }
    }
}
