using System.Collections.Generic;

namespace HistoryBuff.Models
{
    /// <summary>
    /// Represents the entire software history log, which contains a collection of individual history records.
    /// </summary>
    public class SoftwareHistoryLog
    {
        /// <summary>
        /// Gets or sets the list of history records from the log.
        /// </summary>
        public List<HistoryRecord> HistoryRecords { get; set; } = new List<HistoryRecord>();
    }
}

