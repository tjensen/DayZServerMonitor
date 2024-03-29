﻿using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestDayZServerMonitorCore
{
    [TestClass]
    public class TestProfileParser
    {
        private readonly MockClock clock = new MockClock();
        private string filename;
        private CancellationTokenSource source;

        [TestInitialize]
        public void Initialize()
        {
            filename = Path.GetTempFileName();
            source = new CancellationTokenSource();
        }

        [TestCleanup]
        public void Cleanup()
        {
            source.Dispose();
            File.Delete(filename);
        }

        [TestMethod]
        public void GetDayZFolderReturnsDayZFolderPath()
        {
            Assert.AreEqual(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\DayZ",
                ProfileParser.GetDayZFolder());
        }

        [TestMethod]
        public void GetExperimentalDayZFolderReturnsExperimentalDayZFolderPath()
        {
            Assert.AreEqual(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\DayZ Exp",
                ProfileParser.GetExperimentalDayZFolder());
        }

        [TestMethod]
        public void GetProfileFilenameReturnsDayZProfileFilename()
        {
            Assert.AreEqual(
                Environment.UserName + "_settings.DayZProfile",
                ProfileParser.GetProfileFilename());
        }

        [TestMethod]
        public async Task GetLastServerThrowsExceptionWhenFileDoesNotContainLastMPServerAsync()
        {
            MissingLastMPServer error = await Assert.ThrowsExceptionAsync<MissingLastMPServer>(
                () => ProfileParser.GetLastServer(filename, clock, source));
            Assert.AreEqual("Unable to find last MP server in DayZ profile", error.Message);
        }

        [TestMethod]
        public async Task GetLastServerReturnsLastMPServerValue()
        {
            File.WriteAllText(
                filename,
                "something=\"some-value\";\n" +
                "lastMPServer=\"87.65.43.21:1234\";\n" +
                "otherThing=\"other-value\";\n");

            Server server = await ProfileParser.GetLastServer(filename, clock, source);

            Assert.AreEqual("87.65.43.21", server.Host);
            Assert.AreEqual(1234, server.Port);
        }

        [TestMethod]
        public async Task GetLastServerRetriesWhenUnableToReadProfileDueToAccessViolation()
        {
            File.WriteAllText(filename, "lastMPServer=\"87.65.43.21:1234\";\n");

            using (FileStream writeStream = File.OpenWrite(filename))
            {
                clock.SetDelayCompleted();
                clock.DelayCalled = (source, args) => writeStream.Close();

                Server server = await ProfileParser.GetLastServer(filename, clock, source);

                Assert.AreEqual("87.65.43.21", server.Host);
                Assert.AreEqual(1234, server.Port);
            }
        }
    }
}
