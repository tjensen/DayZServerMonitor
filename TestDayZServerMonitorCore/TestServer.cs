using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestDayZServerMonitorCore
{
    [TestClass]
    public class TestServer
    {
        [TestMethod]
        public void HostContainsTheServerIP()
        {
            Server server = new Server("12.34.56.78:4321");

            Assert.AreEqual("12.34.56.78", server.Host);
        }

        [TestMethod]
        public void PortContainsTheServerPort()
        {
            Server server = new Server("12.34.56.78:4321");

            Assert.AreEqual(4321, server.Port);
        }

        [TestMethod]
        public void PortContainsDefaultPortWhenNotSpecifiedInAddress()
        {
            Server server = new Server("12.34.56.78");

            Assert.AreEqual(2301, server.Port);
        }

        [TestMethod]
        public void StatsPortContainsPortNumberForReadingServerStats()
        {
            Server server = new Server("12.34.56.78:4321");

            Assert.AreEqual(29035, server.StatsPort);
        }

        [TestMethod]
        public void AddressContainsTheServerAddress()
        {
            Server server = new Server("12.34.56.78:4321");

            Assert.AreEqual("12.34.56.78:4321", server.Address);
        }

        [TestMethod]
        public void EqualsReturnsTrueIfHostsAndPortsAreEqual()
        {
            Server server1 = new Server("12.34.56.78:4321");
            Server server2 = new Server("12.34.56.78:4321");

            Assert.IsTrue(server1.Equals(server2));
        }

        [TestMethod]
        public void EqualsReturnsFalseIfOtherServerIsNull()
        {
            Server server = new Server("12.34.56.78:4321");

            Assert.IsFalse(server.Equals(null));
        }

        [TestMethod]
        public void EqualsReturnsFalseIfHostsAreDifferent()
        {
            Server server1 = new Server("12.34.56.78:4321");
            Server server2 = new Server("12.34.56.79:4321");

            Assert.IsFalse(server1.Equals(server2));
        }

        [TestMethod]
        public void EqualsReturnsFalseIfPortsAreDifferent()
        {
            Server server1 = new Server("12.34.56.78:4321");
            Server server2 = new Server("12.34.56.78:1234");

            Assert.IsFalse(server1.Equals(server2));
        }
    }
}
