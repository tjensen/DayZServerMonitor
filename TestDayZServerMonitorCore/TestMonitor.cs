using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TestDayZServerMonitorCore
{
    [TestClass]
    public class TestMonitor
    {
        private string tempProfile;
        private MockClock clock;
        private MockClient masterServerClient;
        private MockClient serverInfoClient;
        private MockClientFactory clientFactory;
        private Monitor monitor;

        [TestInitialize]
        public void Initialize()
        {
            tempProfile = Path.GetTempFileName();
            WriteLastMPServer("1.2.3.4:5678");

            clock = new MockClock();
            clock.CurrentTime += TimeSpan.FromSeconds(100);
            masterServerClient = new MockClient();
            serverInfoClient = new MockClient();
            clientFactory = new MockClientFactory(masterServerClient);
            clientFactory.AddClient(serverInfoClient);
            monitor = new Monitor(tempProfile, clock, clientFactory);
        }

        [TestCleanup]
        public void Cleanup()
        {
            clock.Dispose();
            masterServerClient.Dispose();
            serverInfoClient.Dispose();
            monitor.Dispose();
            File.Delete(tempProfile);
        }

        private void WriteLastMPServer(string address)
        {
            File.WriteAllText(tempProfile, string.Format("lastMPServer=\"{0}\";\n", address));
        }

        private byte[] MasterServerResponse()
        {
            return new byte[] {
                0xFF, 0xFF, 0xFF, 0xFF, 0x66, 0x0A, // Header
                0x04, 0x03, 0x02, 0x01, 0x12, 0x34, // IP address and port
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00  // End
            };
        }

        private byte[] ServerInfoResponse()
        {
            return new byte[] {
                0xFF, 0xFF, 0xFF, 0xFF,  // Response header
                0x49, // A2S_INFO header
                0x11, // Protocol
                0x53, 0x45, 0x52, 0x56, 0x45, 0x52, 0x2D, 0x4E, 0x41, 0x4D, 0x45, 0x00, // Name
                0x00, // Map
                0x00, // Folder
                0x00, // Game
                0x12, 0x34, // ID
                0x17, // Players
                0x2A, // Max Players
                0x00, // Bots
                0x65, // Server type
                0x77, // Environment
                0x00, // Visibility
                0x01, // VAC
                0x00, // Version
                0x00 // Extra Data Flag
            };
        }

        [TestMethod]
        public async Task PollReturnsServerInfoOfLastPlayedServer()
        {
            masterServerClient.ServerResponse = MasterServerResponse();
            serverInfoClient.ServerResponse = ServerInfoResponse();

            ServerInfo info = await monitor.Poll();

            Assert.AreEqual("4.3.2.1:4660", info.Address);
            Assert.AreEqual("SERVER-NAME", info.Name);
            Assert.AreEqual(23, info.NumPlayers);
            Assert.AreEqual(42, info.MaxPlayers);

            Assert.AreEqual(2, clientFactory.MockCalls.Count);
            Assert.AreEqual("hl2master.steampowered.com", clientFactory.MockCalls[0].Item1);
            Assert.AreEqual(27011, clientFactory.MockCalls[0].Item2);
            Assert.AreEqual("4.3.2.1", clientFactory.MockCalls[1].Item1);
            Assert.AreEqual(4660, clientFactory.MockCalls[1].Item2);

            Assert.AreEqual(5000, masterServerClient.ServerTimeout);
            CollectionAssert.AreEqual(
                new byte[]
                {
                    0x31, 0xFF, 0x30, 0x2E, 0x30, 0x2E, 0x30, 0x2E, 0x30, 0x3A, 0x30, 0x00, 0x5C,
                    0x67, 0x61, 0x6D, 0x65, 0x5C, 0x44, 0x61, 0x79, 0x5A, 0x5C, 0x67, 0x61, 0x6D,
                    0x65, 0x61, 0x64, 0x64, 0x72, 0x5C, 0x31, 0x2E, 0x32, 0x2E, 0x33, 0x2E, 0x34,
                    0x3A, 0x35, 0x36, 0x37, 0x38, 0x00
                },
                masterServerClient.ServerRequest);

            Assert.AreEqual(5000, serverInfoClient.ServerTimeout);
            CollectionAssert.AreEqual(
                new byte[] {
                    0xFF, 0xFF, 0xFF, 0xFF, 0x54, 0x53, 0x6F, 0x75, 0x72, 0x63, 0x65, 0x20, 0x45,
                    0x6E, 0x67, 0x69, 0x6E, 0x65, 0x20, 0x51, 0x75, 0x65, 0x72, 0x79, 0x00
                },
                serverInfoClient.ServerRequest);
        }


        [TestMethod]
        public async Task PollDoesNotQueryServerIfAtLeast60SecondsHasNotElapsedSinceTheLastCall()
        {
            masterServerClient.ServerResponse = MasterServerResponse();
            serverInfoClient.ServerResponse = ServerInfoResponse();
            _ = await monitor.Poll();
            clientFactory.Reset();

            Assert.IsNull(await monitor.Poll());

            Assert.AreEqual(0, clientFactory.MockCalls.Count);
        }

        [TestMethod]
        public async Task PollDoesQueryIfServerChangedBut60SecondsHasNotElapsed()
        {
            masterServerClient.ServerResponse = MasterServerResponse();
            serverInfoClient.ServerResponse = ServerInfoResponse();
            _ = await monitor.Poll();
            clientFactory.Reset();
            using (MockClient secondServerInfoClient = new MockClient(),
                secondMasterServerClient = new MockClient())
            {
                WriteLastMPServer("9.9.9.9:8888");
                secondMasterServerClient.ServerResponse = MasterServerResponse();
                secondServerInfoClient.ServerResponse = ServerInfoResponse();
                clientFactory.AddClient(secondMasterServerClient);
                clientFactory.AddClient(secondServerInfoClient);

                ServerInfo info = await monitor.Poll();

                Assert.AreEqual("4.3.2.1:4660", info.Address);
                Assert.AreEqual("SERVER-NAME", info.Name);
                Assert.AreEqual(23, info.NumPlayers);
                Assert.AreEqual(42, info.MaxPlayers);

                Assert.AreEqual(2, clientFactory.MockCalls.Count);
                Assert.AreEqual("hl2master.steampowered.com", clientFactory.MockCalls[0].Item1);
                Assert.AreEqual(27011, clientFactory.MockCalls[0].Item2);
                Assert.AreEqual("4.3.2.1", clientFactory.MockCalls[1].Item1);
                Assert.AreEqual(4660, clientFactory.MockCalls[1].Item2);

                CollectionAssert.AreEqual(
                    new byte[]
                    {
                    0x31, 0xFF, 0x30, 0x2E, 0x30, 0x2E, 0x30, 0x2E, 0x30, 0x3A, 0x30, 0x00, 0x5C,
                    0x67, 0x61, 0x6D, 0x65, 0x5C, 0x44, 0x61, 0x79, 0x5A, 0x5C, 0x67, 0x61, 0x6D,
                    0x65, 0x61, 0x64, 0x64, 0x72, 0x5C, 0x39, 0x2E, 0x39, 0x2E, 0x39, 0x2E, 0x39,
                    0x3A, 0x38, 0x38, 0x38, 0x38, 0x00
                    },
                    secondMasterServerClient.ServerRequest);

                CollectionAssert.AreEqual(
                    new byte[] {
                    0xFF, 0xFF, 0xFF, 0xFF, 0x54, 0x53, 0x6F, 0x75, 0x72, 0x63, 0x65, 0x20, 0x45,
                    0x6E, 0x67, 0x69, 0x6E, 0x65, 0x20, 0x51, 0x75, 0x65, 0x72, 0x79, 0x00
                    },
                    secondServerInfoClient.ServerRequest);
            }
        }

        [TestMethod]
        public async Task PollReusesMasterServerResponseWhenLastServerUnchanged()
        {
            masterServerClient.ServerResponse = MasterServerResponse();
            serverInfoClient.ServerResponse = ServerInfoResponse();
            _ = await monitor.Poll();
            clientFactory.Reset();
            using (MockClient secondServerInfoClient = new MockClient())
            {
                secondServerInfoClient.ServerResponse = ServerInfoResponse();
                clientFactory.AddClient(secondServerInfoClient);
                clock.CurrentTime += TimeSpan.FromSeconds(60);

                ServerInfo info = await monitor.Poll();

                Assert.AreEqual("4.3.2.1:4660", info.Address);
                Assert.AreEqual("SERVER-NAME", info.Name);
                Assert.AreEqual(23, info.NumPlayers);
                Assert.AreEqual(42, info.MaxPlayers);

                Assert.AreEqual(1, clientFactory.MockCalls.Count);
                Assert.AreEqual("4.3.2.1", clientFactory.MockCalls[0].Item1);
                Assert.AreEqual(4660, clientFactory.MockCalls[0].Item2);

                CollectionAssert.AreEqual(
                    new byte[] {
                    0xFF, 0xFF, 0xFF, 0xFF, 0x54, 0x53, 0x6F, 0x75, 0x72, 0x63, 0x65, 0x20, 0x45,
                    0x6E, 0x67, 0x69, 0x6E, 0x65, 0x20, 0x51, 0x75, 0x65, 0x72, 0x79, 0x00
                    },
                    secondServerInfoClient.ServerRequest);
            }
        }

        [TestMethod]
        public async Task PollSerializesConcurrentCalls()
        {
            masterServerClient.ServerResponse = MasterServerResponse();
            serverInfoClient.ServerResponse = ServerInfoResponse();
            using (MockClient secondServerInfoClient = new MockClient())
            {
                secondServerInfoClient.ServerResponse = ServerInfoResponse();

                serverInfoClient.RequestAction = () =>
                {
                    Assert.AreEqual(2, clientFactory.MockCalls.Count);
                    clientFactory.AddClient(secondServerInfoClient);
                    clock.CurrentTime += TimeSpan.FromSeconds(60);
                };

                Task<ServerInfo> firstPollTask = monitor.Poll();
                Task<ServerInfo> secondPollTask = monitor.Poll();

                _ = await firstPollTask;
                Assert.AreEqual(2, clientFactory.MockCalls.Count);
                _ = await secondPollTask;
                Assert.AreEqual(3, clientFactory.MockCalls.Count);
            }
        }
    }
}
