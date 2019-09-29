using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace TestDayZServerMonitorCore
{
    [TestClass]
    public class TestLogger
    {
        private readonly MockClock clock = new MockClock();
        private readonly List<string> statusTexts = new List<string>();
        private readonly List<Tuple<string, string>> logEntries = new List<Tuple<string, string>>();
        private readonly Settings settings = new Settings();
        private readonly string tempFilename = Path.GetTempFileName();
        private Logger logger;

        [TestInitialize]
        public void Initialize()
        {
            clock.CurrentTime = new DateTime(2019, 8, 24, 16, 34, 12, 45).ToUniversalTime();

            var statusWriter = new Action<string>((text) => statusTexts.Add(text));
            var logWriter = new Action<string, string>(
                (level, message) => logEntries.Add(new Tuple<string, string>(level, message)));

            logger = new Logger(settings, clock, statusWriter, logWriter);
        }

        [TestCleanup]
        public void Cleanup()
        {
            File.Delete(tempFilename);
        }

        [TestMethod]
        public void StatusWritesTextToStatusWriterAndLogWriter()
        {
            logger.Status("this is some status text");

            Assert.AreEqual(1, statusTexts.Count);
            Assert.AreEqual("16:34:12 - this is some status text", statusTexts[0]);

            Assert.AreEqual(1, logEntries.Count);
            Assert.AreEqual("STATUS", logEntries[0].Item1);
            Assert.AreEqual("this is some status text", logEntries[0].Item2);
        }

        [TestMethod]
        public void ErrorWritesTextToStatusWriterAndLogWriter()
        {
            var error = new Exception("some message");
            logger.Error("description of error", error);

            Assert.AreEqual(1, statusTexts.Count);
            Assert.AreEqual("16:34:12 - description of error: some message", statusTexts[0]);

            Assert.AreEqual(1, logEntries.Count);
            Assert.AreEqual("ERROR", logEntries[0].Item1);
            Assert.AreEqual($"description of error\r\n{error}", logEntries[0].Item2);
        }

        [TestMethod]
        public void DebugWritesTextToLogWriter()
        {
            logger.Debug("this is some debug text");

            Assert.AreEqual(0, statusTexts.Count);

            Assert.AreEqual(1, logEntries.Count);
            Assert.AreEqual("DEBUG", logEntries[0].Item1);
            Assert.AreEqual("this is some debug text", logEntries[0].Item2);
        }

        [TestMethod]
        public void LogsAreWrittenToLogPathnameWhenNotNull()
        {
            settings.LogPathname = tempFilename;

            logger.Debug("debug text");
            clock.CurrentTime += TimeSpan.FromSeconds(1);
            logger.Status("status text");
            clock.CurrentTime += TimeSpan.FromSeconds(1);
            logger.Error("error text", new Exception("exception message"));

            settings.LogPathname = null; // Closes the log file so that the test can read it

            string logContents = File.ReadAllText(tempFilename);
            Assert.AreEqual(
                "8/24/2019 4:34:12 PM - DEBUG - debug text\r\n" +
                "8/24/2019 4:34:13 PM - STATUS - status text\r\n" +
                "8/24/2019 4:34:14 PM - ERROR - error text\r\n" +
                "System.Exception: exception message\r\n",
                logContents);
        }

        [TestMethod]
        public void LogsAreAppendedToExistingFile()
        {
            File.WriteAllText(tempFilename, "existing text\r\n");
            settings.LogPathname = tempFilename;

            logger.Status("status text");

            settings.LogPathname = null; // Closes the log file so that the test can read it

            string logContents = File.ReadAllText(tempFilename);
            Assert.AreEqual(
                "existing text\r\n" +
                "8/24/2019 4:34:12 PM - STATUS - status text\r\n",
                logContents);
        }

        [TestMethod]
        public void DoesNotErrorWhenLogFileCannotBeOpened()
        {
            using (FileStream stream = File.OpenWrite(tempFilename))
            {
                settings.LogPathname = tempFilename;
            }

            Assert.AreEqual(1, logEntries.Count);
            Assert.AreEqual("ERROR", logEntries[0].Item1);
            Console.WriteLine(logEntries[0].Item2);
            Assert.IsTrue(
                logEntries[0].Item2.StartsWith(
                    $"Failed to open log file {tempFilename}\r\n" +
                    "System.IO.IOException: The process cannot access the file"));
        }
    }
}
