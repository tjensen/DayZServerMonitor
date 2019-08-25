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
        public async Task GetServerReturnsServer()
        {
            Server server = new Server("1.2.3.4", 5678);
            SavedServerSource serverSource = new SavedServerSource(server);

            Assert.AreSame(server, await serverSource.GetServer());
        }

        [TestMethod]
        public async Task GetServerReturnsNullIfConstructedWithoutParameters()
        {
            SavedServerSource serverSource = new SavedServerSource();

            Assert.IsNull(await serverSource.GetServer());
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
    }
}
