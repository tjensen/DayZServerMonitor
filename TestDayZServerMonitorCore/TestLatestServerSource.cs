using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestDayZServerMonitorCore
{
    [TestClass]
    public class TestLatestServerSource
    {
        private MockLogger logger = new MockLogger();
        private string filename;
        private LatestServerSource serverSource;

        [TestInitialize]
        public void Initialize()
        {
            filename = Path.GetTempFileName();
            serverSource = new LatestServerSource("Stable", Path.GetDirectoryName(filename), Path.GetFileName(filename), logger);
        }

        [TestCleanup]
        public void Cleanup()
        {
            File.Delete(filename);
        }

        [TestMethod]
        public void GetDisplayNameReturnsDisplayName()
        {
            Assert.AreEqual("Most Recent (Stable)", serverSource.GetDisplayName());
        }

        [TestMethod]
        public async Task GetServerReturnsMostRecentlyPlayedServer()
        {
            using (FileStream fs = File.OpenWrite(filename))
            {
                fs.Write(Encoding.UTF8.GetBytes("lastMPServer=\"12.34.56.78:12345\";\n"));
            }

            Server server = await serverSource.GetServer();

            Assert.AreEqual("12.34.56.78", server.Host);
            Assert.AreEqual(12345, server.Port);
        }

        [TestMethod]
        public async Task GetServerReturnsNullWhenGettingTheLastPlayedServerFails()
        {
            Assert.IsNull(await serverSource.GetServer());

            Assert.AreEqual(1, logger.ErrorTexts.Count);
            Assert.AreEqual("Failed to determine last played server", logger.ErrorTexts[0]);
            Assert.AreEqual(1, logger.ErrorExceptions.Count);
            Assert.IsInstanceOfType(logger.ErrorExceptions[0], typeof(MissingLastMPServer));
        }

        [TestMethod]
        public void CreateWatcherReturnsNewProfileWatcher()
        {
            using (AutoResetEvent actionCalled = new AutoResetEvent(false))
            {
                Action action = new Action(() => { actionCalled.Set(); });
                using (ProfileWatcher watcher = serverSource.CreateWatcher(action, null))
                {
                    Assert.IsFalse(actionCalled.WaitOne(50, false));

                    File.WriteAllText(filename, "new contents");

                    Assert.IsTrue(actionCalled.WaitOne(50, false));
                }
            }
        }
    }
}
