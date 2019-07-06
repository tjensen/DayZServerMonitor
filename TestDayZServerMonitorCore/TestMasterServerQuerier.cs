using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace TestDayZServerMonitorCore
{
    [TestClass]
    public class TestMasterServerQuerier
    {
        private readonly MockClient client = new MockClient();
        private MockClientFactory clientFactory;

        [TestInitialize]
        public void Initialize()
        {
            clientFactory = new MockClientFactory(client);
        }

        [TestMethod]
        public async Task FindDayZServerReturnsServerWhenMasterServerReturnsAMatch()
        {
            client.ServerResponse = new byte[] {
                0xFF, 0xFF, 0xFF, 0xFF, 0x66, 0x0A, // Header
                0x87, 0x65, 0x43, 0x21, 0x99, 0x00, // IP address and port
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00  // End
            };

            MasterServerQuerier querier = new MasterServerQuerier(clientFactory);

            Server server = await querier.FindDayZServerInRegion(
                "12.34.56.78", 12345, MasterServerQuerier.REGION_REST, 100);

            Assert.AreEqual("hl2master.steampowered.com", clientFactory.Host);
            Assert.AreEqual(27011, clientFactory.Port);
            Assert.AreEqual(100, client.ServerTimeout);
            CollectionAssert.AreEqual(
                new byte[]
                {
                    0x31, 0xFF, 0x30, 0x2E, 0x30, 0x2E, 0x30, 0x2E, 0x30, 0x3A, 0x30, 0x00, 0x5C,
                    0x67, 0x61, 0x6D, 0x65, 0x5C, 0x44, 0x61, 0x79, 0x5A, 0x5C, 0x67, 0x61, 0x6D,
                    0x65, 0x61, 0x64, 0x64, 0x72, 0x5C, 0x31, 0x32, 0x2E, 0x33, 0x34, 0x2E, 0x35,
                    0x36, 0x2E, 0x37, 0x38, 0x3A, 0x31, 0x32, 0x33, 0x34, 0x35, 0x00
                },
                client.ServerRequest);

            Assert.AreEqual("135.101.67.33", server.Host);
            Assert.AreEqual(39168, server.Port);
        }

        [TestMethod]
        public async Task FindDayZServerReturnsNullWhenMasterServerReturnsEmptyResults()
        {
            client.ServerResponse = new byte[] {
                0xFF, 0xFF, 0xFF, 0xFF, 0x66, 0x0A, // Header
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00  // End
            };

            MasterServerQuerier querier = new MasterServerQuerier(clientFactory);

            Assert.IsNull(await querier.FindDayZServerInRegion(
                "12.34.56.78", 12345, MasterServerQuerier.REGION_REST, 100));
        }

        [TestMethod]
        public async Task FindDayZServerReturnsNullWhenServerTimesOut()
        {
            MasterServerQuerier querier = new MasterServerQuerier(clientFactory);

            Assert.IsNull(await querier.FindDayZServerInRegion(
                "12.34.56.78", 12345, MasterServerQuerier.REGION_REST, 100));
        }

        [TestMethod]
        public async Task FindDayZServerReturnsNullWhenServerResponseIsInvalid()
        {
            client.ServerResponse = new byte[] { 1, 2, 3, 4 };

            MasterServerQuerier querier = new MasterServerQuerier(clientFactory);

            Assert.IsNull(await querier.FindDayZServerInRegion(
                "12.34.56.78", 12345, MasterServerQuerier.REGION_REST, 100));
        }
    }
}
