using System.Text.Json.Serialization;

namespace Altinn.AccessGroups.Core.Models
{
    /// <summary>
    /// Model used for Access Groups in Authorization.
    /// </summary>
    public class AccessGroup
    {
        /// <summary>
        /// Gets or sets the Access Group Code.
        /// </summary>
        public string? AccessGroupCode { get; set; }

        /// <summary>
        /// Gets or sets the Access Group Type.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AccessGroupType AccessGroupType { get; set; }

        /// <summary>
        /// Gets or sets the list of categories the Access Group is to have as parents.
        /// </summary>
        public List<string>? Categories { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Access Group is hidden.
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// Gets or sets when the Access Group was created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets when the Access Group was last modified.
        /// </summary>
        public DateTime Modified { get; set; }
    }
}
