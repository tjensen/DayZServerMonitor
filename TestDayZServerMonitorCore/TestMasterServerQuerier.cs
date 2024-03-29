﻿using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestDayZServerMonitorCore
{
    [TestClass]
    public class TestMasterServerQuerier
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
        public async Task FindDayZServerReturnsServerWhenMasterServerReturnsAMatch()
        {
            client.ServerResponse = new byte[] {
                0xFF, 0xFF, 0xFF, 0xFF, 0x66, 0x0A, // Header
                0x87, 0x65, 0x43, 0x21, 0x99, 0x00, // IP address and port
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00  // End
            };

            MasterServerQuerier querier = new MasterServerQuerier(clientFactory, logger);

            Server server = await querier.FindDayZServerInRegion(
                "12.34.56.78", 12345, MasterServerQuerier.REGION_REST, 100, source);

            Assert.AreEqual(1, clientFactory.MockCalls.Count);
            Assert.AreEqual("hl2master.steampowered.com", clientFactory.MockCalls[0].Item1);
            Assert.AreEqual(27011, clientFactory.MockCalls[0].Item2);
            Assert.AreEqual(100, client.ServerTimeout);
            Assert.AreSame(source, client.Source);
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

            Assert.AreEqual(1, logger.StatusTexts.Count);
            Assert.AreEqual("Finding server 12.34.56.78:12345 in master server list", logger.StatusTexts[0]);
        }

        [TestMethod]
        public async Task FindDayZServerReturnsNullWhenMasterServerReturnsEmptyResults()
        {
            client.ServerResponse = new byte[] {
                0xFF, 0xFF, 0xFF, 0xFF, 0x66, 0x0A, // Header
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00  // End
            };

            MasterServerQuerier querier = new MasterServerQuerier(clientFactory, logger);

            Assert.IsNull(await querier.FindDayZServerInRegion(
                "12.34.56.78", 12345, MasterServerQuerier.REGION_REST, 100, source));

            Assert.AreEqual(1, logger.StatusTexts.Count);
        }

        [TestMethod]
        public async Task FindDayZServerReturnsNullWhenServerTimesOut()
        {
            client.ServerError = new TimeoutException();
            MasterServerQuerier querier = new MasterServerQuerier(clientFactory, logger);

            Assert.IsNull(await querier.FindDayZServerInRegion(
                "12.34.56.78", 12345, MasterServerQuerier.REGION_REST, 100, source));

            Assert.AreEqual(1, logger.ErrorTexts.Count);
            Assert.AreEqual("Error querying master server", logger.ErrorTexts[0]);
            Assert.AreEqual(1, logger.ErrorExceptions.Count);
            Assert.AreSame(client.ServerError, logger.ErrorExceptions[0]);
        }

        [TestMethod]
        public async Task FindDayZServerReturnsNullWhenServerResponseIsInvalid()
        {
            client.ServerResponse = new byte[] { 1, 2, 3, 4 };

            MasterServerQuerier querier = new MasterServerQuerier(clientFactory, logger);

            Assert.IsNull(await querier.FindDayZServerInRegion(
                "12.34.56.78", 12345, MasterServerQuerier.REGION_REST, 100, source));

            Assert.AreEqual(1, logger.ErrorTexts.Count);
            Assert.AreEqual("Error querying master server", logger.ErrorTexts[0]);
            Assert.AreEqual(1, logger.ErrorExceptions.Count);
            Assert.IsInstanceOfType(logger.ErrorExceptions[0], typeof(ParseException));
        }
    }
}
