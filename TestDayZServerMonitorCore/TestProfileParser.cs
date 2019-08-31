using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TestDayZServerMonitorCore
{
    [TestClass]
    public class TestProfileParser
    {
        private string filename;

        [TestInitialize]
        public void Initialize()
        {
            filename = Path.GetTempFileName();
        }

        [TestCleanup]
        public void Cleanup()
        {
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
                () => ProfileParser.GetLastServer(filename));
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

            Server server = await ProfileParser.GetLastServer(filename);

            Assert.AreEqual("87.65.43.21", server.Host);
            Assert.AreEqual(1234, server.Port);
        }
    }
}
