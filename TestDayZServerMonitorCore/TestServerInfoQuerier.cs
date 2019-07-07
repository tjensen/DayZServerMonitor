using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace TestDayZServerMonitorCore
{
    [TestClass]
    public class TestServerInfoQuerier
    {
        private readonly MockClient client = new MockClient();
        private MockClientFactory clientFactory;

        [TestInitialize]
        public void Initialize()
        {
            clientFactory = new MockClientFactory(client);
        }

        [TestMethod]
        public async Task QueryReturnsServerInfoWhenSuccessful()
        {
            client.ServerResponse = new byte[] {
                0xFF, 0xFF, 0xFF, 0xFF,  // Response header
                0x49, // A2S_INFO header
                0x11, // Protocol
                0x53, 0x65, 0x72, 0x76, 0x65, 0x72, 0x20, 0x4E, 0x61, 0x6D, 0x65, 0x00, // Name
                0x00, // Map
                0x00, // Folder
                0x00, // Game
                0x12, 0x34, // ID
                0x2A, // Players
                0x3C, // Max Players
                0x00, // Bots
                0x65, // Server type
                0x77, // Environment
                0x00, // Visibility
                0x01, // VAC
                0x00, // Version
                0x00 // Extra Data Flag
            };
            ServerInfoQuerier querier = new ServerInfoQuerier(clientFactory);

            ServerInfo info = await querier.Query("12.34.56.78", 12345, 100);

            Assert.AreEqual(1, clientFactory.MockCalls.Count);
            Assert.AreEqual("12.34.56.78", clientFactory.MockCalls[0].Item1);
            Assert.AreEqual(12345, clientFactory.MockCalls[0].Item2);
            Assert.AreEqual(100, client.ServerTimeout);
            CollectionAssert.AreEqual(
                new byte[] {
                    0xFF, 0xFF, 0xFF, 0xFF, 0x54, 0x53, 0x6F, 0x75, 0x72, 0x63, 0x65, 0x20, 0x45,
                    0x6E, 0x67, 0x69, 0x6E, 0x65, 0x20, 0x51, 0x75, 0x65, 0x72, 0x79, 0x00
                },
                client.ServerRequest);

            Assert.AreEqual("12.34.56.78:12345", info.Address);
            Assert.AreEqual("Server Name", info.Name);
            Assert.AreEqual(42, info.NumPlayers);
            Assert.AreEqual(60, info.MaxPlayers);
        }
    }
}
