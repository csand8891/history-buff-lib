using Microsoft.VisualStudio.TestTools.UnitTesting;
using HistoryBuff.Services;
using System.IO;
using HistoryBuff.Models;
using System;
using System.Text;

namespace HistoryBuff.Tests
{
    [TestClass]
    public class HistoryLogSummarizerTests
    {
        private HistoryParser _parser;
        private HistoryLogSummarizer _summarizer;
        private SoftwareHistoryLog _log;

        [TestInitialize]
        public void Setup()
        {
            _parser = new HistoryParser();
            _summarizer = new HistoryLogSummarizer();

            // Arrange: The test data file is copied to the output directory.
            // We build a path relative to the test assembly's location for robustness.
            var assemblyLocation = typeof(HistoryParserTests).Assembly.Location;
            var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
            var historyFilePath = Path.Combine(assemblyDirectory, "TestData", "HISTORY.TXT");

            _log = _parser.Parse(historyFilePath);
        }

        [TestMethod]
        public void GenerateHighLevelSummary_WithValidLog_ShouldReturnCorrectSummary()
        {
            // Act
            var summary = _summarizer.GenerateHighLevelSummary(_log);

            // Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(summary), "Summary should not be null or whitespace.");

            // Build the expected output for the first record to verify against.
            var expectedFirstRecordSummary = new StringBuilder();
            expectedFirstRecordSummary.AppendLine("--- RECORD: 2019-11-07 ---");
            expectedFirstRecordSummary.AppendLine("  Lines Added: 76");
            expectedFirstRecordSummary.AppendLine("  Lines Removed: 34");
            expectedFirstRecordSummary.AppendLine("  Lines Modified: 0");
            expectedFirstRecordSummary.AppendLine("  Spec Code Changes: 6");
            
            StringAssert.Contains(summary, expectedFirstRecordSummary.ToString(), "The summary for the first record is not correct.");
        }
    }
}
