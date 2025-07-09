using Microsoft.VisualStudio.TestTools.UnitTesting;
using HistoryBuff.Services;
using System.IO;
using System;
using System.Linq;
using System.Diagnostics;
using HistoryBuff.Models;

namespace HistoryBuff.Tests
{
    [TestClass]
    public class HistoryParserTests
    {
        private HistoryParser _parser;

        [TestInitialize]
        public void Setup()
        {
            _parser = new HistoryParser();
        }

        [TestMethod]
        public void Parse_WithValidHistoryFile_ShouldReturnPopulatedLog()
        {

            // Arrange: The test data file is copied to the output directory.
            // We build a path relative to the test assembly's location for robustness.
            var assemblyLocation = typeof(HistoryParserTests).Assembly.Location;
            var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
            var historyFilePath = Path.Combine(assemblyDirectory, "TestData", "HISTORY.TXT");

            // Act
            var result = _parser.Parse(historyFilePath);

            // For debugging, write the parsed output to a file in the test's output directory.
            var outputFilePath = Path.Combine(assemblyDirectory, "parsed_output.txt");
            using (var writer = new StreamWriter(outputFilePath))
            {
                writer.WriteLine($"Parsed from: {historyFilePath}");
                writer.WriteLine($"Found {result.HistoryRecords.Count} records.");
                writer.WriteLine("=============================================");

                foreach (var record in result.HistoryRecords)
                {
                    writer.WriteLine();
                    writer.WriteLine($"--- RECORD: {record.Date:yyyy-MM-dd} ---");
                    writer.WriteLine($"({record.Lines.Count} lines, {record.SpecCodeChanges.Count} spec changes)");
                    
                    foreach (var line in record.Lines)
                    {
                        // Note: The OriginalText in the model has already been trimmed of the '>' or '<' characters.
                        writer.WriteLine($"[{line.Type}] {line.OriginalText}");
                    }
                }
            }
            Console.WriteLine($"Test output written to: {outputFilePath}");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(12, result.HistoryRecords.Count, "Should parse all 12 history records from the file.");

            // Verify first record
            var firstRecord = result.HistoryRecords.FirstOrDefault(r => r.Date.Date == new DateTime(2019, 11, 7));
            Assert.IsNotNull(firstRecord, "Could not find the record for 2019-11-07.");
            Assert.AreEqual(110, firstRecord.Lines.Count, "Should parse 110 lines for the first record.");
            Assert.AreEqual(4, firstRecord.SpecCodeChanges.Count, "The first record should have 6 spec code changes.");
            Assert.AreEqual(ChangeIndicator.Removed, firstRecord.Lines[0].Type);
            Assert.IsTrue(firstRecord.Lines[0].OriginalText.StartsWith("* cspsVer.10.0.10"));

            // Verify one of the spec code changes in the first record
            var specChange = firstRecord.SpecCodeChanges.FirstOrDefault(s => s.SpecCodeSection == "NC1");
            Assert.IsNotNull(specChange, "Could not find the NC1 spec change.");
            Assert.AreEqual(SpecChangeIndicator.Added, specChange.ChangeType);
            Assert.AreEqual("10", specChange.SpecNumber);
            Assert.AreEqual("5", specChange.SpecBit);
            Assert.AreEqual("0", specChange.OldValue);
            Assert.AreEqual("1", specChange.NewValue);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void Parse_WithNonExistentFile_ShouldThrowFileNotFoundException()
        {
            // Act
            _parser.Parse("non_existent_file.txt");
        }
    }
}
