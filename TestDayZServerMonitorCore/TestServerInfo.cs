using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TestDayZServerMonitorCore
{
    [TestClass]
    public class TestServerInfo
    {
        private readonly ServerInfo info = new ServerInfo("address", "name", 42, 60, "TIME");

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
        public void TimeContainsTheServerTime()
        {
            Assert.AreEqual("TIME", info.Time);
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
                    0x01, // VAC
                    0x31, 0x2E, 0x30, 0x00, // Version
                    0x00 // Extra Data Flag
            });

            Assert.AreEqual("192.168.21.12:4321", info.Address);
            Assert.AreEqual("Server Name", info.Name);
            Assert.AreEqual(42, info.NumPlayers);
            Assert.AreEqual(60, info.MaxPlayers);
            Assert.AreEqual("unknown", info.Time);
        }

        [TestMethod]
        public void ParseReturnsServerReportedPortWhenExtraFlagIsSet()
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
                    0x01, // VAC
                    0x31, 0x2E, 0x30, 0x00, // Version
                    0x80, // Extra Data Flags
                    0x21, 0x12 // Server Game Port
            });

            Assert.AreEqual("192.168.21.12:4641", info.Address);

            // The server's keywords extra flag is not set
            Assert.AreEqual("unknown", info.Time);
        }

        [TestMethod]
        public void ParserReturnsServerReportedTimeWhenKeywordsExtraFlagIsSet()
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
                    0x01, // VAC
                    0x31, 0x2E, 0x30, 0x00, // Version
                    0x20, // Extra Data Flags
                    0x30, 0x31, 0x3A, 0x32, 0x33, 0x00 // Server Keywords
            });

            Assert.AreEqual("01:23", info.Time);

            // The server's game port extra flag is not set
            Assert.AreEqual("192.168.21.12:4321", info.Address);
        }

        [TestMethod]
        public void ParserExtractsTimeFromKeywordsWhenThereAreOtherKeywords()
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
                    0x01, // VAC
                    0x31, 0x2E, 0x30, 0x00, // Version
                    0x20, // Extra Data Flags
                    // Server Keywords: "foo,10:59,bar"
                    0x66, 0x6F, 0x6F, 0x2C, 0x31, 0x30, 0x3A, 0x35, 0x39, 0x2C, 0x62, 0x61, 0x72,
                    0x00
            });

            Assert.AreEqual("10:59", info.Time);
        }

        [TestMethod]
        public void ParseReturnsUnknownTimeIfNoKeywordLooksLikeATime()
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
                    0x01, // VAC
                    0x31, 0x2E, 0x30, 0x00, // Version
                    0x20, // Extra Data Flags
                    // Server Keywords: "foo,bar,baz"
                    0x66, 0x6F, 0x6F, 0x2C, 0x62, 0x61, 0x72, 0x2C, 0x62, 0x61, 0x7A,
                    0x00
            });

            Assert.AreEqual("unknown", info.Time);
        }

        [TestMethod]
        public void ParseReturnsServerTimeWhenOtherExtraFlagsAreSet()
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
                    0x01, // VAC
                    0x31, 0x2E, 0x30, 0x00, // Version
                    0xF1, // Extra Data Flags
                    0x21, 0x12, // Server Game Port
                    0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, // Server Steam ID
                    0x55, 0xAA, // Spectator port number
                    0x62, 0x75, 0x7A, 0x00, // Spectator server name: "buz"
                    // Server Keywords: "foo,12:42,bar"
                    0x66, 0x6F, 0x6F, 0x2C, 0x31, 0x32, 0x3A, 0x34, 0x32, 0x2C, 0x62, 0x61, 0x72,
                    0x00
            });

            Assert.AreEqual("12:42", info.Time);

            // The server's game port extra flag is also set
            Assert.AreEqual("192.168.21.12:4641", info.Address);
        }

        [TestMethod]
        public void ParseThrowsParseExceptionWhenPacketHeaderIsWrong()
        {
            ParseException error = Assert.ThrowsException<ParseException>(() =>
            {
                _ = ServerInfo.Parse(
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
                        0x01, // VAC
                        0x31, 0x2E, 0x30, 0x00, // Version
                        0x00 // Extra Data Flag
                    });
            });
            Assert.AreEqual("Invalid Packet Header", error.Message);
        }

        [TestMethod]
        public void ParseThrowsParseExceptionWhenInfoHeaderIsWrong()
        {
            ParseException error = Assert.ThrowsException<ParseException>(() =>
            {
                _ = ServerInfo.Parse(
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
                        0x01, // VAC
                        0x31, 0x2E, 0x30, 0x00, // Version
                        0x00 // Extra Data Flag
                    });
            });
            Assert.AreEqual("Invalid Info Header (received 0x42; expected 0x49)", error.Message);
        }

        [TestMethod]
        public void IsChallengeReturnsTrueWhenBufferContainsServerChallenge()
        {
            Assert.IsTrue(
                ServerInfo.IsChallenge(
                    new byte[] {
                        0xFF, 0xFF, 0xFF, 0xFF,
                        0x41,
                        0x0A, 0xBC, 0x21, 0x12
                    },
                    out byte[] challenge));

            CollectionAssert.AreEqual(new byte[] { 0x0A, 0xBC, 0x21, 0x12 }, challenge);
        }

        [TestMethod]
        public void IsChallengeReturnsFalseWhenServerChallengeContainsInsufficientBytes()
        {
            Assert.IsFalse(
                ServerInfo.IsChallenge(
                    new byte[] { 0xFF },
                    out byte[] challenge));

            Assert.IsNull(challenge);
        }

        [TestMethod]
        public void IsChallengeReturnsFalseWhenBufferDoesNotContainServerChallenge()
        {
            Assert.IsFalse(
                ServerInfo.IsChallenge(
                    new byte[] {
                        0xFF, 0xFF, 0xFF, 0xFF,
                        0x49,
                        0x0A, 0xBC, 0x21, 0x12
                    },
                    out byte[] challenge));

            Assert.IsNull(challenge);
        }

        [TestMethod]
        public void ToStringReturnsInformationAboutServer()
        {
            Assert.AreEqual(
                "Server at address is:\r\nName: name\r\nPlayers: 42/60\r\nTime: TIME",
                info.ToString());
        }
    }
}
