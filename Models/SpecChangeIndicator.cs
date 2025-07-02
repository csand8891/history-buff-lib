namespace HistoryBuff.Models
{
    /// <summary>
    /// Indicates the type of change for a specific spec code.
    /// </summary>
    public enum SpecChangeIndicator
    {
        /// <summary>
        /// The nature of the change is unknown or not specified.
        /// </summary>
        Unknown,
        /// <summary>
        /// The spec code was added.
        /// </summary>
        Added,
        /// <summary>
        /// The spec code was removed.
        /// </summary>
        Removed,
        /// <summary>
        /// The spec code was modified.
        /// </summary>
        Modified
    }
}