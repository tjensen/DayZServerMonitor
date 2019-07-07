using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TestDayZServerMonitorCore
{
    [TestClass]
    public class TestClient
    {
        private MockServer server;
        private Client client;

        [TestInitialize]
        public void Initialize()
        {
            server = new MockServer();

            client = new Client("127.0.0.1", server.Port);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (server != null)
            {
                server.Dispose();
            }
        }

        [TestMethod]
        public async Task RequestReturnsServerResponseAfterSendingRequestToServer()
        {
            server.Response = new byte[] { 21, 12 };

            byte[] response = await client.Request(new byte[] { 1, 2, 3, 4, 5 }, 100);

            Assert.IsTrue(server.RequestCompleted);
            CollectionAssert.AreEqual(server.Response, response);
            CollectionAssert.AreEqual(new byte[] { 1, 2, 3, 4, 5 }, server.Request);
        }
    }

    [TestClass]
    public class TestClientFactory
    {
        private MockServer server;
        private ClientFactory factory;

        [TestInitialize]
        public void Initialize()
        {
            server = new MockServer();

            factory = new ClientFactory();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (server != null)
            {
                server.Dispose();
            }
        }

        [TestMethod]
        public async Task CreateReturnsAClient()
        {
            server.Response = new byte[] { 21, 12 };

            IClient client = factory.Create("127.0.0.1", server.Port);
            byte[] response = await client.Request(new byte[] { 1, 2, 3, 4, 5 }, 100);

            Assert.IsTrue(server.RequestCompleted);
            CollectionAssert.AreEqual(server.Response, response);
            CollectionAssert.AreEqual(new byte[] { 1, 2, 3, 4, 5 }, server.Request);
        }
    }
}
