using HistoryBuff.Models;
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace HistoryBuff.Services
{
    /// <summary>
    /// Parses a history.txt file and populates the data models.
    /// </summary>
    public class HistoryParser
    {
        // Regex to capture the start of a history record, which is a date.
        // Example: [2019-09-23]
        private static readonly Regex RecordStartRegex = new Regex(@"^\[(?<date>\d{4}-\d{2}-\d{2})\]", RegexOptions.Compiled);

        // Regex to capture a spec code change line based on value changes.
        // Example: PLC1  SPEC NO.11 bit1 0 -> 1
        private static readonly Regex SpecCodeRegex = new Regex(
            @"^(?<section>\w+)\s+SPEC NO\.\s*(?<number>\d+)\s+bit(?<bit>\d+)\s+(?<oldVal>\d+)\s*->\s*(?<newVal>\d+)\s*$",
            RegexOptions.Compiled);

        /// <summary>
        /// Parses the specified history file.
        /// </summary>
        /// <param name="filePath">The path to the history.txt file.</param>
        /// <returns>A <see cref="SoftwareHistoryLog"/> object populated with the parsed data.</returns>
        /// <exception cref="FileNotFoundException">Thrown if the specified file does not exist.</exception>
        public SoftwareHistoryLog Parse(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("History file not found.", filePath);
            }

            var log = new SoftwareHistoryLog();
            var lines = File.ReadLines(filePath);
            HistoryRecord currentRecord = null;

            foreach (var line in lines)
            {
                var recordStartMatch = RecordStartRegex.Match(line);
                if (recordStartMatch.Success)
                {
                    // A new record is starting. If we have a pending record, add it to the log.
                    if (currentRecord != null)
                    {
                        log.HistoryRecords.Add(currentRecord);
                    }

                    // Create the new record and parse its date.
                    currentRecord = new HistoryRecord();
                    var dateString = recordStartMatch.Groups["date"].Value;
                    currentRecord.Date = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

                    // This line is the header, so we skip to the next line for the body.
                    continue;
                }

                // If we haven't found the first record header yet, skip any preceding lines.
                if (currentRecord == null) continue;

                // Ignore empty or whitespace-only lines that might exist between records.
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                // Check for the most specific line type (spec code change) first.
                var specCodeChange = ParseSpecCodeChange(line);
                if (specCodeChange != null)
                {
                    currentRecord.SpecCodeChanges.Add(specCodeChange);

                    // Map the SpecChangeIndicator to the more general ChangeIndicator for the line.
                    ChangeIndicator lineType;
                    switch (specCodeChange.ChangeType)
                    {
                        case SpecChangeIndicator.Added:
                            lineType = ChangeIndicator.Added;
                            break;
                        case SpecChangeIndicator.Removed:
                            lineType = ChangeIndicator.Removed;
                            break;
                        default: // Handles Modified
                            lineType = ChangeIndicator.Modified;
                            break;
                    }
                    currentRecord.Lines.Add(new HistoryLine { Type = lineType, OriginalText = line });
                }
                else
                {
                    // If it's not a spec code change, process it as a general history line.
                    currentRecord.Lines.Add(ParseHistoryLine(line));
                }
            }

            // Add the very last record in the file
            if (currentRecord != null)
            {
                log.HistoryRecords.Add(currentRecord);
            }

            return log;
        }

        private HistoryLine ParseHistoryLine(string line)
        {
            if (line.StartsWith(">"))
            {
                return new HistoryLine { Type = ChangeIndicator.Added, OriginalText = line.Substring(1).TrimStart() };
            }
            
            if (line.StartsWith("<"))
            {
                return new HistoryLine { Type = ChangeIndicator.Removed, OriginalText = line.Substring(1).TrimStart() };
            }
            
            // If no change indicator is present, the line is considered unchanged.
            return new HistoryLine { Type = ChangeIndicator.Unchanged, OriginalText = line };
        }

        private SpecCodeChange ParseSpecCodeChange(string line)
        {
            var match = SpecCodeRegex.Match(line);
            if (!match.Success) return null;

            var oldVal = match.Groups["oldVal"].Value;
            var newVal = match.Groups["newVal"].Value;

            // If there is no actual change, it's not a valid record.
            if (oldVal == newVal) return null;

            SpecChangeIndicator changeType;
            if (oldVal == "0" && newVal == "1")
            {
                changeType = SpecChangeIndicator.Added;
            }
            else if (oldVal == "1" && newVal == "0")
            {
                changeType = SpecChangeIndicator.Removed;
            }
            else
            {
                // Handle other changes (e.g., 2 -> 3) as modifications.
                changeType = SpecChangeIndicator.Modified;
            }

            return new SpecCodeChange
            {
                ChangeType = changeType,
                SpecCodeSection = match.Groups["section"].Value,
                SpecNumber = match.Groups["number"].Value,
                SpecBit = match.Groups["bit"].Value,
                SpecName = null, // This format does not include a spec name.
                OldValue = oldVal,
                NewValue = newVal
            };
        }
    }
}