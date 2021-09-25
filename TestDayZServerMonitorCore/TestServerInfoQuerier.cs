using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;

namespace TestDayZServerMonitorCore
{
    [TestClass]
    public class TestServerInfoQuerier
    {
        private readonly MockClient client = new MockClient();
        private readonly MockLogger logger = new MockLogger();
        private MockClientFactory clientFactory;
        private CancellationTokenSource source;

        [TestInitialize]
        public void Initialize()
        {
            clientFactory = new MockClientFactory(client);
            source = new CancellationTokenSource();
        }

        [TestCleanup]
        public void Cleanup()
        {
            source.Dispose();
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
            ServerInfoQuerier querier = new ServerInfoQuerier(clientFactory, logger);

            ServerInfo info = await querier.Query("12.34.56.78", 12345, 100, source);

            Assert.AreEqual(1, clientFactory.MockCalls.Count);
            Assert.AreEqual("12.34.56.78", clientFactory.MockCalls[0].Item1);
            Assert.AreEqual(12345, clientFactory.MockCalls[0].Item2);
            Assert.AreEqual(1, client.ServerTimeouts.Count);
            Assert.AreEqual(100, client.ServerTimeout);
            Assert.AreSame(source, client.Source);
            Assert.AreEqual(1, client.ServerRequests.Count);
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

            Assert.AreEqual(2, logger.StatusTexts.Count);
            Assert.AreEqual("Reading DayZ server status at 12.34.56.78:12345", logger.StatusTexts[0]);
            Assert.AreEqual("Finished reading DayZ server status at 12.34.56.78:12345", logger.StatusTexts[1]);
            Assert.AreEqual(0, logger.ErrorTexts.Count);
        }

        [TestMethod]
        public async Task QueryReturnsServerInfoWhenServerRespondsWithChallenge()
        {
            client.ServerResponses.Add(new byte[] {
                0xFF, 0xFF, 0xFF, 0xFF,
                0x41,
                0x11, 0x22, 0x33, 0x44
            });
            client.ServerResponses.Add(new byte[] {
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
            });

            ServerInfoQuerier querier = new ServerInfoQuerier(clientFactory, logger);

            ServerInfo info = await querier.Query("12.34.56.78", 12345, 100, source);

            Assert.AreEqual(1, clientFactory.MockCalls.Count);
            Assert.AreEqual("12.34.56.78", clientFactory.MockCalls[0].Item1);
            Assert.AreEqual(12345, clientFactory.MockCalls[0].Item2);
            Assert.AreEqual(2, client.ServerTimeouts.Count);
            Assert.AreEqual(100, client.ServerTimeouts[0]);
            Assert.AreEqual(100, client.ServerTimeouts[1]);
            Assert.AreSame(source, client.Source);
            Assert.AreEqual(2, client.ServerRequests.Count);
            CollectionAssert.AreEqual(
                new byte[] {
                    0xFF, 0xFF, 0xFF, 0xFF, 0x54, 0x53, 0x6F, 0x75, 0x72, 0x63, 0x65, 0x20, 0x45,
                    0x6E, 0x67, 0x69, 0x6E, 0x65, 0x20, 0x51, 0x75, 0x65, 0x72, 0x79, 0x00
                },
                client.ServerRequests[0]);
            CollectionAssert.AreEqual(
                new byte[] {
                    0xFF, 0xFF, 0xFF, 0xFF, 0x54, 0x53, 0x6F, 0x75, 0x72, 0x63, 0x65, 0x20, 0x45,
                    0x6E, 0x67, 0x69, 0x6E, 0x65, 0x20, 0x51, 0x75, 0x65, 0x72, 0x79, 0x00, 0x11,
                    0x22, 0x33, 0x44
                },
                client.ServerRequests[1]);

            Assert.AreEqual("12.34.56.78:12345", info.Address);
            Assert.AreEqual("Server Name", info.Name);
            Assert.AreEqual(42, info.NumPlayers);
            Assert.AreEqual(60, info.MaxPlayers);

            Assert.AreEqual(3, logger.StatusTexts.Count);
            Assert.AreEqual("Reading DayZ server status at 12.34.56.78:12345", logger.StatusTexts[0]);
            Assert.AreEqual("Received challenge from DayZ server at 12.34.56.78:12345", logger.StatusTexts[1]);
            Assert.AreEqual("Finished reading DayZ server status at 12.34.56.78:12345", logger.StatusTexts[2]);
            Assert.AreEqual(0, logger.ErrorTexts.Count);

        }

        [TestMethod]
        public async Task QueryReturnsNullOnError()
        {
            client.ServerResponse = new byte[] { 0 }; // Bad response
            ServerInfoQuerier querier = new ServerInfoQuerier(clientFactory, logger);

            Assert.IsNull(await querier.Query("12.34.56.78", 12345, 100, source));

            Assert.AreEqual(1, logger.StatusTexts.Count);
            Assert.AreEqual(1, logger.ErrorTexts.Count);
            Assert.AreEqual("Error reading DayZ server status", logger.ErrorTexts[0]);
            Assert.AreEqual(1, logger.ErrorExceptions.Count);
            Assert.IsInstanceOfType(logger.ErrorExceptions[0], typeof(ParseException));
        }
    }
}
