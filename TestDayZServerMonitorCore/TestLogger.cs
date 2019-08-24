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
        private Logger logger;

        [TestInitialize]
        public void Initialize()
        {
            clock.CurrentTime = new DateTime(2019, 8, 24, 16, 34, 12, 45).ToUniversalTime();

            Action<string> statusWriter = new Action<string>((text) => { statusTexts.Add(text); });

            logger = new Logger(clock, statusWriter);
        }

        [TestMethod]
        public void StatusWritesTextToStatusWriter()
        {
            logger.Status("this is some status text");

            Assert.AreEqual(1, statusTexts.Count);
            Assert.AreEqual("16:34:12 - this is some status text", statusTexts[0]);
        }

        [TestMethod]
        public void ErrorWritesTextToStatusWriter()
        {
            logger.Error("description of error", new Exception("some message"));

            Assert.AreEqual(1, statusTexts.Count);
            Assert.AreEqual("16:34:12 - description of error: some message", statusTexts[0]);
        }
    }
}
