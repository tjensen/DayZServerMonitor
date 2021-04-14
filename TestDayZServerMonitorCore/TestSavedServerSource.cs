using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;

namespace TestDayZServerMonitorCore
{
    [TestClass]
    public class TestSavedServerSource
    {
        private readonly MockLogger logger = new MockLogger();
        private readonly MockClock clock = new MockClock();
        private CancellationTokenSource source;

        [TestInitialize]
        public void Initialize()
        {
            source = new CancellationTokenSource();
        }

        [TestCleanup]
        public void Cleanup()
        {
            source.Dispose();
        }

        [TestMethod]
        public void GetDisplayNameReturnsServerAddressWhenServerNameIsUnknown()
        {
            SavedServerSource serverSource = new SavedServerSource(new Server("1.2.3.4", 5678));

            Assert.AreEqual("1.2.3.4:5678", serverSource.GetDisplayName());
        }

        [TestMethod]
        public void GetDisplayNameReturnsServerNameAndAddressWhenNameIsKnown()
        {
            SavedServerSource serverSource = new SavedServerSource(new Server("1.2.3.4", 5678), "SERVER NAME");

            Assert.AreEqual("SERVER NAME (1.2.3.4:5678)", serverSource.GetDisplayName());
        }

        [TestMethod]
        public async Task GetServerReturnsServer()
        {
            Server server = new Server("1.2.3.4", 5678);
            SavedServerSource serverSource = new SavedServerSource(server);

            Assert.AreSame(server, await serverSource.GetServer(logger, clock, source));
        }

        [TestMethod]
        public void CreateWatcherReturnsNull()
        {
            SavedServerSource serverSource = new SavedServerSource(new Server("1.2.3.4", 5678));

            Assert.IsNull(serverSource.CreateWatcher(() => { }, null));
        }

        [TestMethod]
        public void AddressContainsTheServerAddress()
        {
            SavedServerSource serverSource = new SavedServerSource(new Server("1.2.3.4", 5678));

            Assert.AreEqual("1.2.3.4:5678", serverSource.Address);
        }

        [TestMethod]
        public void AddressCanBeModified()
        {
            SavedServerSource serverSource = new SavedServerSource(new Server("1.2.3.4", 5678));

            serverSource.Address = "8.7.6.5:4321";

            Assert.AreEqual("8.7.6.5:4321", serverSource.Address);
        }

        [TestMethod]
        public void ServerNameContainsTheKnownServerName()
        {
            SavedServerSource serverSource = new SavedServerSource(new Server("1.2.3.4", 5678), "SERVER NAME");

            Assert.AreEqual("SERVER NAME", serverSource.ServerName);
        }

        [TestMethod]
        public void ServerNameCanBeModified()
        {
            SavedServerSource serverSource = new SavedServerSource(new Server("1.2.3.4", 5678), "SERVER NAME");

            serverSource.ServerName = "NEW NAME";

            Assert.AreEqual("NEW NAME", serverSource.ServerName);
        }

        [TestMethod]
        public void SaveReturnsASavedSource()
        {
            SavedServerSource serverSource = new SavedServerSource(new Server("1.2.3.4", 5678), "SERVER NAME");

            SavedSource savedSource = serverSource.Save();
            Assert.AreEqual("1.2.3.4:5678", savedSource.Address);
            Assert.AreEqual("SERVER NAME", savedSource.Name);
            Assert.IsNull(savedSource.Filename);
        }

        public void CanBeConstructedFromASavedSource()
        {
            SavedSource savedSource = new SavedSource();
            savedSource.Address = "1.2.3.4:5678";
            savedSource.Name = "SERVER NAME";
            SavedServerSource source = new SavedServerSource(savedSource);

            Assert.AreEqual("1.2.3.4:5678", source.Address);
            Assert.AreEqual("SERVER NAME", source.ServerName);
        }
    }
}
