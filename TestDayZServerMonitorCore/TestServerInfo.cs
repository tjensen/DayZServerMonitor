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

        [TestMethod]
        public void ParseReturnsServerInfo()
        {
            ServerInfo info = ServerInfo.Parse(
                "192.168.21.12", 4321,
                new byte[] {
                    0xFF, 0xFF, 0xFF, 0xFF,  // Response header
                    0x49, // A2S_INFO header
                    0x11, // Protocol
                    0x53, 0x65, 0x72, 0x76, 0x65, 0x72, 0x20, 0x4E, 0x61, 0x6D, 0x65, 0x00, // Name
                    0x6D, 0x61, 0x70, 0x00, // Map
                    0x66, 0x6F, 0x6C, 0x64, 0x65, 0x72, 0x00, // Folder
                    0x67, 0x61, 0x6D, 0x65, 0x00, // Game
                    0x12, 0x34, // ID
                    0x2A, // Players
                    0x3C, // Max Players
                    0x00, // Bots
                    0x65, // Server type
                    0x77, // Environment
                    0x00, // Visibility
                    0x01 // VAC
            });

            Assert.AreEqual("192.168.21.12:4321", info.Address);
            Assert.AreEqual("Server Name", info.Name);
            Assert.AreEqual(42, info.NumPlayers);
            Assert.AreEqual(60, info.MaxPlayers);
        }

        [TestMethod]
        public void ParseThrowsParseExceptionWhenPacketHeaderIsWrong()
        {
            ParseException error = Assert.ThrowsException<ParseException>(() =>
            {
                ServerInfo.Parse(
                    "192.168.21.12", 4321,
                    new byte[] {
                        0x12, 0x34, 0x56, 0x78,  // Response header
                        0x49, // A2S_INFO header
                        0x11, // Protocol
                        0x53, 0x65, 0x72, 0x76, 0x65, 0x72, 0x20, 0x4E, 0x61, 0x6D, 0x65, 0x00, // Name
                        0x6D, 0x61, 0x70, 0x00, // Map
                        0x66, 0x6F, 0x6C, 0x64, 0x65, 0x72, 0x00, // Folder
                        0x67, 0x61, 0x6D, 0x65, 0x00, // Game
                        0x12, 0x34, // ID
                        0x2A, // Players
                        0x3C, // Max Players
                        0x00, // Bots
                        0x65, // Server type
                        0x77, // Environment
                        0x00, // Visibility
                        0x01 // VAC
                    });
            });
            Assert.AreEqual("Invalid Packet Header", error.Message);
        }

        [TestMethod]
        public void ParseThrowsParseExceptionWhenInfoHeaderIsWrong()
        {
            ParseException error = Assert.ThrowsException<ParseException>(() =>
            {
                ServerInfo.Parse(
                    "192.168.21.12", 4321,
                    new byte[] {
                        0xFF, 0xFF, 0xFF, 0xFF,  // Response header
                        0x42, // A2S_INFO header
                        0x11, // Protocol
                        0x53, 0x65, 0x72, 0x76, 0x65, 0x72, 0x20, 0x4E, 0x61, 0x6D, 0x65, 0x00, // Name
                        0x6D, 0x61, 0x70, 0x00, // Map
                        0x66, 0x6F, 0x6C, 0x64, 0x65, 0x72, 0x00, // Folder
                        0x67, 0x61, 0x6D, 0x65, 0x00, // Game
                        0x12, 0x34, // ID
                        0x2A, // Players
                        0x3C, // Max Players
                        0x00, // Bots
                        0x65, // Server type
                        0x77, // Environment
                        0x00, // Visibility
                        0x01 // VAC
                    });
            });
            Assert.AreEqual("Invalid Info Header", error.Message);
        }
    }
}
