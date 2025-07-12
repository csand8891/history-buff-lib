using HistoryBuff.Models;
using System.Text;
using System.Linq;

namespace HistoryBuff.Services
{
    /// <summary>
    /// Provides methods for summarizing a SoftwareHistoryLog.
    /// </summary>
    public class HistoryLogSummarizer
    {
        /// <summary>
        /// Generates a high-level summary of a SoftwareHistoryLog.
        /// </summary>
        /// <param name="log">The SoftwareHistoryLog to summarize.</param>
        /// <returns>A string containing the high-level summary.</returns>
        public string GenerateHighLevelSummary(SoftwareHistoryLog log)
        {
            var summaryBuilder = new StringBuilder();

            foreach (var record in log.HistoryRecords)
            {
                var addedLines = record.Lines.Count(l => l.Type == ChangeIndicator.Added);
                var removedLines = record.Lines.Count(l => l.Type == ChangeIndicator.Removed);
                var modifiedLines = record.Lines.Count(l => l.Type == ChangeIndicator.Modified);

                summaryBuilder.AppendLine($"--- RECORD: {record.Date:yyyy-MM-dd} ---");
                summaryBuilder.AppendLine($"  Lines Added: {addedLines}");
                summaryBuilder.AppendLine($"  Lines Removed: {removedLines}");
                summaryBuilder.AppendLine($"  Lines Modified: {modifiedLines}");
                summaryBuilder.AppendLine($"  Spec Code Changes: {record.SpecCodeChanges.Count}");
                summaryBuilder.AppendLine();
            }

            return summaryBuilder.ToString();
        }
    }
}