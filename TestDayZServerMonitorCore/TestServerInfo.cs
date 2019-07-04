using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestDayZServerMonitorCore
{
    [TestClass]
    public class TestServerInfo
    {
        private readonly ServerInfo info = new ServerInfo("address", "name", 42, 60);

        [TestMethod]
        public void AddressContainsTheServerAddress()
        {
            Assert.AreEqual("address", info.Address);
        }

        [TestMethod]
        public void NameContainsTheServerName()
        {
            Assert.AreEqual("name", info.Name);
        }

        [TestMethod]
        public void NumPlayersContainsTheNumberOfPlayers()
        {
            Assert.AreEqual(42, info.NumPlayers);
        }

        [TestMethod]
        public void MaxPlayersContainsTheMaximumNumberOfPlayers()
        {
            Assert.AreEqual(60, info.MaxPlayers);
        }
    }
}
