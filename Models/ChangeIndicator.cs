namespace HistoryBuff.Models
{
    /// <summary>
    /// Indicates the type of change for a line in the history file.
    /// </summary>
    public enum ChangeIndicator
    {
        /// <summary>
        /// The line is unchanged.
        /// </summary>
        Unchanged,
        /// <summary>
        /// The line was added.
        /// </summary>
        Added,
        /// <summary>
        /// The line was removed.
        /// </summary>
        Removed,
        /// <summary>
        /// The line was modified (e.g., a spec change from one non-zero value to another).
        /// </summary>
        Modified
    }
}