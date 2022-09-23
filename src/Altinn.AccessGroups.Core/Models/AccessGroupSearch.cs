using System.ComponentModel.DataAnnotations;

namespace Altinn.AccessGroups.Core.Models
{
    /// <summary>
    /// Model for performing a search for access groups available between parties
    /// </summary>
    public class AccessGroupSearch : IValidatableObject
    {
        /// <summary>
        /// The user holding the AccessGroup
        /// </summary>
        public int? CoveredByUserId { get; set; }

        /// <summary>
        /// The party holding the access Group
        /// </summary>
        public int? CoveredByPartyId { get; set; }

        /// <summary>
        /// The party the AccessGroup is hold for
        /// </summary>
        public int OfferedByPartyId { get; set; }

        /// <inheritdoc/>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResult = new List<ValidationResult>();

            if (OfferedByPartyId <= 0)
            {
                validationResult.Add(new ValidationResult("Offered by must be included"));
            }

            if (CoveredByUserId == null && CoveredByPartyId == null)
            {
                validationResult.Add(new ValidationResult("One CoveredBy value must be included"));
            }

            if (CoveredByUserId.HasValue && CoveredByUserId <= 0)
            {
                validationResult.Add(new ValidationResult("CoveredByUserId must not have a value or have a valid value"));
            }

            if (CoveredByPartyId.HasValue && CoveredByPartyId <= 0)
            {
                validationResult.Add(new ValidationResult("CoveredByPartyId must not have a value or have a valid value"));
            }

            return validationResult;
        }
    }
}
