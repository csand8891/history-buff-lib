using System;
using System.Collections.Generic;


namespace HistoryBuff.Models
{
    public class HistoryRecord
    {
        /// <summary>
        /// The date of the history record.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The old revision number.
        /// </summary>
        public string OldRevisionNumber { get; set; }

        /// <summary>
        /// The new revision number.
        /// </summary>
        public string NewRevisionNumber { get; set; }

        /// <summary>
        /// The lines of text associated with this history record.
        /// </summary>
        public List<HistoryLine> Lines { get; set; } = new List<HistoryLine>();

        /// <summary>
        /// The specific code changes in this history record.
        /// </summary>
        public List<SpecCodeChange> SpecCodeChanges { get; set; } = new List<SpecCodeChange>();
    }
}