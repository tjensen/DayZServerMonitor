﻿using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestDayZServerMonitorCore
{
    [TestClass]
    public class TestMonitor
    {
        private readonly byte[] DEFAULT_STATS_ADDRESS = new byte[] { 0x04, 0x03, 0x02, 0x01, 0x12, 0x34 };

        private Server server;
        private MockClock clock;
        private MockClient masterServerClient;
        private MockClient serverInfoClient;
        private MockClientFactory clientFactory;
        private DayZServerMonitorCore.Monitor monitor;
        private CancellationTokenSource source;

        [TestInitialize]
        public void Initialize()
        {
            server = new Server("1.2.3.4", 5678);
            clock = new MockClock();
            clock.CurrentTime += TimeSpan.FromSeconds(100);
            masterServerClient = new MockClient();
            serverInfoClient = new MockClient();
            clientFactory = new MockClientFactory(masterServerClient);
            clientFactory.AddClient(serverInfoClient);
            monitor = new DayZServerMonitorCore.Monitor(clock, clientFactory, new MockLogger());
            source = new CancellationTokenSource();
        }

        [TestCleanup]
        public void Cleanup()
        {
            clock.Dispose();
            masterServerClient.Dispose();
            serverInfoClient.Dispose();
            source.Dispose();
        }

        private byte[] MasterServerResponse(byte[] address)
        {
            return new byte[] {
                0xFF, 0xFF, 0xFF, 0xFF, 0x66, 0x0A, // Header
                address[0], address[1], address[2], address[3], // IP Address
                address[4], address[5], // Port
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00  // End
            };
        }

        private byte[] MasterServerResponse()
        {
            return MasterServerResponse(DEFAULT_STATS_ADDRESS);
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

            ServerInfo info = await monitor.Poll(server, source);

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
            Assert.AreSame(source, masterServerClient.Source);
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
            Assert.AreSame(source, serverInfoClient.Source);
            CollectionAssert.AreEqual(
                new byte[] {
                    0xFF, 0xFF, 0xFF, 0xFF, 0x54, 0x53, 0x6F, 0x75, 0x72, 0x63, 0x65, 0x20, 0x45,
                    0x6E, 0x67, 0x69, 0x6E, 0x65, 0x20, 0x51, 0x75, 0x65, 0x72, 0x79, 0x00
                },
                serverInfoClient.ServerRequest);
        }

        [TestMethod]
        public async Task PollDoesNotQueryServerIfAtLeast45SecondsHasNotElapsedSinceTheLastCall()
        {
            masterServerClient.ServerResponse = MasterServerResponse();
            serverInfoClient.ServerResponse = ServerInfoResponse();
            _ = await monitor.Poll(server, source);
            clientFactory.Reset();
            clock.CurrentTime += TimeSpan.FromSeconds(44);

            Assert.IsNull(await monitor.Poll(server, source));

            Assert.AreEqual(0, clientFactory.MockCalls.Count);
        }

        [TestMethod]
        public async Task PollDoesQueryIfServerChangedBut60SecondsHasNotElapsed()
        {
            masterServerClient.ServerResponse = MasterServerResponse();
            serverInfoClient.ServerResponse = ServerInfoResponse();

            _ = await monitor.Poll(server, source);

            clientFactory.Reset();
            using MockClient secondServerInfoClient = new MockClient();
            using MockClient secondMasterServerClient = new MockClient();
            secondMasterServerClient.ServerResponse = MasterServerResponse();
            secondServerInfoClient.ServerResponse = ServerInfoResponse();
            clientFactory.AddClient(secondMasterServerClient);
            clientFactory.AddClient(secondServerInfoClient);

            ServerInfo info = await monitor.Poll(new Server("9.9.9.9", 8888), source);

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

        [TestMethod]
        public async Task PollReusesMasterServerResponseWhenLastServerUnchanged()
        {
            masterServerClient.ServerResponse = MasterServerResponse();
            serverInfoClient.ServerResponse = ServerInfoResponse();
            _ = await monitor.Poll(server, source);
            clientFactory.Reset();
            using MockClient secondServerInfoClient = new MockClient();
            secondServerInfoClient.ServerResponse = ServerInfoResponse();
            clientFactory.AddClient(secondServerInfoClient);
            clock.CurrentTime += TimeSpan.FromSeconds(45);

            ServerInfo info = await monitor.Poll(server, source);

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

        [TestMethod]
        public async Task PollDoesNotSaveMoreThanOneMasterServerResponse()
        {
            masterServerClient.ServerResponse = MasterServerResponse();
            serverInfoClient.ServerResponse = ServerInfoResponse();
            clientFactory.AddClient(masterServerClient);
            clientFactory.AddClient(serverInfoClient);
            _ = await monitor.Poll(server, source);
            _ = await monitor.Poll(new Server("5.5.5.5", 5555), source);
            clientFactory.Reset();
            using MockClient thirdMasterServerClient = new MockClient();
            using MockClient thirdServerInfoClient = new MockClient();
            thirdMasterServerClient.ServerResponse = MasterServerResponse(
                new byte[] { 0x09, 0x08, 0x07, 0x06, 0x76, 0x54 });
            clientFactory.AddClient(thirdMasterServerClient);
            thirdServerInfoClient.ServerResponse = ServerInfoResponse();
            clientFactory.AddClient(thirdServerInfoClient);

            ServerInfo info = await monitor.Poll(server, source);

            Assert.AreEqual(2, clientFactory.MockCalls.Count);
            Assert.AreEqual("hl2master.steampowered.com", clientFactory.MockCalls[0].Item1);
            Assert.AreEqual(27011, clientFactory.MockCalls[0].Item2);
            Assert.AreEqual("9.8.7.6", clientFactory.MockCalls[1].Item1);
            Assert.AreEqual(30292, clientFactory.MockCalls[1].Item2);
        }

        [TestMethod]
        public async Task PollDoesNotReuseGuessedGameServerPort()
        {
            masterServerClient.ServerResponse = new byte[] {
                0xFF, 0xFF, 0xFF, 0xFF, 0x66, 0x0A, // Header
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00  // End
            };
            serverInfoClient.ServerResponse = ServerInfoResponse();

            _ = await monitor.Poll(server, source);

            clientFactory.Reset();
            using MockClient secondMasterServerClient = new MockClient();
            using MockClient secondServerInfoClient = new MockClient();
            secondMasterServerClient.ServerResponse = MasterServerResponse(
                new byte[] { 0x02, 0x03, 0x04, 0x05, 0x11, 0x22 });
            clientFactory.AddClient(secondMasterServerClient);
            secondServerInfoClient.ServerResponse = ServerInfoResponse();
            clientFactory.AddClient(secondServerInfoClient);
            clock.CurrentTime += TimeSpan.FromMinutes(10);

            ServerInfo info = await monitor.Poll(server, source);

            Assert.AreEqual(2, clientFactory.MockCalls.Count);
            Assert.AreEqual("hl2master.steampowered.com", clientFactory.MockCalls[0].Item1);
            Assert.AreEqual(27011, clientFactory.MockCalls[0].Item2);
            Assert.AreEqual("2.3.4.5", clientFactory.MockCalls[1].Item1);
            Assert.AreEqual(4386, clientFactory.MockCalls[1].Item2);

            Assert.IsNotNull(info);
        }

        [TestMethod]
        public async Task PollSerializesConcurrentCalls()
        {
            masterServerClient.ServerResponse = MasterServerResponse();
            serverInfoClient.ServerResponse = ServerInfoResponse();
            using MockClient secondServerInfoClient = new MockClient();
            secondServerInfoClient.ServerResponse = ServerInfoResponse();

            serverInfoClient.RequestAction = () =>
            {
                Assert.AreEqual(2, clientFactory.MockCalls.Count);
                clientFactory.AddClient(secondServerInfoClient);
                clock.CurrentTime += TimeSpan.FromSeconds(60);
            };

            Task<ServerInfo> firstPollTask = monitor.Poll(server, source);
            Task<ServerInfo> secondPollTask = monitor.Poll(server, source);

            _ = await firstPollTask;
            _ = await secondPollTask;
            Assert.AreEqual(3, clientFactory.MockCalls.Count);
        }

        [TestMethod]
        public async Task PollGuessesGameServerPortIfNotListedByMasterServer()
        {
            masterServerClient.ServerResponse = new byte[] {
                0xFF, 0xFF, 0xFF, 0xFF, 0x66, 0x0A, // Header
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00  // End
            };
            serverInfoClient.ServerResponse = ServerInfoResponse();

            ServerInfo info = await monitor.Poll(server, source);

            Assert.AreEqual("1.2.3.4:30392", info.Address);
            Assert.AreEqual("SERVER-NAME", info.Name);
            Assert.AreEqual(23, info.NumPlayers);
            Assert.AreEqual(42, info.MaxPlayers);

            Assert.AreEqual("1.2.3.4", clientFactory.MockCalls[1].Item1);
            Assert.AreEqual(30392, clientFactory.MockCalls[1].Item2);
        }
    }
}
