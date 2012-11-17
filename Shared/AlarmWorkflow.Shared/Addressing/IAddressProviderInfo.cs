using System.Collections.Generic;

namespace AlarmWorkflow.Shared.Addressing
{
    /// <summary>
    /// Defines mechanisms for an <see cref="IAddressProvider"/> to expose some information about itself.
    /// This is mostly needed for editing purposes.
    /// </summary>
    public interface IAddressProviderInfo
    {
        /// <summary>
        /// Returns detailed description about the custom data object the address provider manages.
        /// </summary>
        /// <returns>Detailed description about the custom data object the address provider manages.</returns>
        IEnumerable<ProviderDataDetail> GetDataDetails();
    }
}
