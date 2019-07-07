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
        private MockClock clock;
        private Client client;

        [TestInitialize]
        public void Initialize()
        {
            server = new MockServer();
            clock = new MockClock();
            client = new Client("127.0.0.1", server.Port, clock);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (server != null)
            {
                server.Dispose();
                clock.Dispose();
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

        [TestMethod]
        public async Task RequestThrowsTimeoutExceptionIfRequestTakesLongerThanTimeout()
        {
            clock.SetDelayCompleted();

            _ = await Assert.ThrowsExceptionAsync<TimeoutException>(
                async () => await client.Request(new byte[] { 1, 2, 3, 4, 5 }, 100));
        }
    }

    [TestClass]
    public class TestClientFactory
    {
        [TestMethod]
        public void CreateReturnsAnIClient()
        {
            ClientFactory factory = new ClientFactory();

            using (IClient client = factory.Create("127.0.0.1", 2112))
            {
                Assert.IsTrue(client is Client);
            }
        }
    }
}
