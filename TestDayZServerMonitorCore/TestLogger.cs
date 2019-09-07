using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TestDayZServerMonitorCore
{
    [TestClass]
    public class TestLogger
    {
        private readonly MockClock clock = new MockClock();
        private readonly List<string> statusTexts = new List<string>();
        private readonly List<Tuple<string, string>> logEntries = new List<Tuple<string, string>>();
        private Logger logger;

        [TestInitialize]
        public void Initialize()
        {
            clock.CurrentTime = new DateTime(2019, 8, 24, 16, 34, 12, 45).ToUniversalTime();

            var statusWriter = new Action<string>((text) => statusTexts.Add(text));
            var logWriter = new Action<string, string>(
                (level, message) => logEntries.Add(new Tuple<string, string>(level, message)));

            logger = new Logger(clock, statusWriter, logWriter);
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
    }
}
