using System;

namespace AlarmWorkflow.Shared.Addressing
{
    /// <summary>
    /// Represents one property within the address provider's custom data object.
    /// </summary>
    public sealed class ProviderDataDetail
    {
        /// <summary>
        /// Gets/sets the name of the property to describe.
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// Gets/sets the text to display in the UI for this property.
        /// </summary>
        public string DisplayText { get; set; }
        /// <summary>
        /// Gets/sets the description to display in the UI.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets/sets the type of this property.
        /// </summary>
        public Type Type { get; set; }
    }
}
