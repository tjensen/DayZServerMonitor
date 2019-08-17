using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestDayZServerMonitorCore
{
    [TestClass]
    public class TestSavedServerSource
    {
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
        public void SetServerNameUpdatesKnownServerName()
        {
            SavedServerSource serverSource = new SavedServerSource(new Server("1.2.3.4", 5678), "OLD NAME");
            serverSource.SetServerName("NEW SERVER NAME");

            Assert.AreEqual("NEW SERVER NAME (1.2.3.4:5678)", serverSource.GetDisplayName());
        }

        [TestMethod]
        public async Task GetServerReturnsServer()
        {
            Server server = new Server("1.2.3.4", 5678);
            SavedServerSource serverSource = new SavedServerSource(server);

            Assert.AreSame(server, await serverSource.GetServer());
        }

        [TestMethod]
        public void CreateWatcherReturnsNull()
        {
            SavedServerSource serverSource = new SavedServerSource(new Server("1.2.3.4", 5678));

            Assert.IsNull(serverSource.CreateWatcher(() => { }, null));
        }
    }
}
