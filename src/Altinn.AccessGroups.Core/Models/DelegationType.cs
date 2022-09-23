namespace Altinn.AccessGroups.Core.Models
{
    /// <summary>
    /// Enum definition for types of delegation
    /// </summary>
    public enum DelegationType
    {
        /// <summary>
        /// Delegated by user
        /// </summary>
        Brukerdelegering = 0,

        /// <summary>
        /// Delegated by a client
        /// </summary>
        Klientdelegering = 1,

        /// <summary>
        /// Delegated by a service
        /// </summary>
        Tjenestedelegering = 2
    }
}
