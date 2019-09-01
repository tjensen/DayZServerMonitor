using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
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
            serverSource = new LatestServerSource("Stable", Path.GetDirectoryName(filename), Path.GetFileName(filename));
        }

        [TestCleanup]
        public void Cleanup()
        {
            File.Delete(filename);
        }

        [TestMethod]
        public void ModifierContainsTheModifier()
        {
            Assert.AreEqual("Stable", serverSource.Modifier);
        }

        [TestMethod]
        public void GetDisplayNameReturnsDisplayName()
        {
            Assert.AreEqual("Most Recent (Stable)", serverSource.GetDisplayName());
        }

        [TestMethod]
        public void ProfileDirectoryContainsTheProfileDirectory()
        {
            Assert.AreEqual(Path.GetDirectoryName(filename), serverSource.ProfileDirectory);
        }

        [TestMethod]
        public void ProfileFilenameContainsTheProfileFilename()
        {
            Assert.AreEqual(Path.GetFileName(filename), serverSource.ProfileFilename);
        }

        [TestMethod]
        public async Task GetServerReturnsMostRecentlyPlayedServer()
        {
            File.WriteAllText(
                filename, "lastMPServer=\"12.34.56.78:12345\";\n");

            Server server = await serverSource.GetServer(logger);

            Assert.AreEqual("12.34.56.78", server.Host);
            Assert.AreEqual(12345, server.Port);
        }

        [TestMethod]
        public async Task GetServerReturnsNullWhenGettingTheLastPlayedServerFails()
        {
            Assert.IsNull(await serverSource.GetServer(logger));

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

        [TestMethod]
        public void SaveReturnsASavedSource()
        {
            LatestServerSource source = new LatestServerSource("whatever", @"X:\path\to", "filename.DayZProfile");
            SavedSource savedSource = source.Save();

            Assert.AreEqual(@"X:\path\to\filename.DayZProfile", savedSource.Filename);
            Assert.IsNull(savedSource.Address);
            Assert.IsNull(savedSource.Name);
        }

        [TestMethod]
        public void CanBeConstructedFromASavedSource()
        {
            SavedSource savedSource = new SavedSource();
            savedSource.Filename = @"X:\path\to\some.DayZProfile";
            LatestServerSource source = new LatestServerSource(savedSource);

            Assert.AreEqual(@"X:\path\to", source.ProfileDirectory);
            Assert.AreEqual("some.DayZProfile", source.ProfileFilename);
        }
    }
}
