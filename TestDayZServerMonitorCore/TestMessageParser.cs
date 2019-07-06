using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace TestDayZServerMonitorCore
{
    [TestClass]
    public class TestMessageParser
    {
        MessageParser parser;

        [TestMethod]
        public void GetByteThrowsParseExceptionWhenBufferIsEmpty()
        {
            parser = new MessageParser(new byte[] { });
            ParseException e = Assert.ThrowsException<ParseException>(() => parser.GetByte());
            Assert.AreEqual("Insufficient bytes remaining: 0 < 1", e.Message);
        }

        [TestMethod]
        public void GetByteReturnsFirstByteOfBuffer()
        {
            parser = new MessageParser(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            Assert.AreEqual(1, parser.GetByte());
        }

        [TestMethod]
        public void GetByteRemovesByteAsItIsReturned()
        {
            parser = new MessageParser(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            _ = parser.GetByte();
            Assert.AreEqual(2, parser.GetByte());
        }

        [TestMethod]
        public void GetBytesThrowsParseExceptionWhenBufferHasInsufficientBytes()
        {
            parser = new MessageParser(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            ParseException e = Assert.ThrowsException<ParseException>(() => parser.GetBytes(9));
            Assert.AreEqual("Insufficient bytes remaining: 8 < 9", e.Message);
        }

        [TestMethod]
        public void GetBytesReturnsFirstNBytesOfBuffer()
        {
            parser = new MessageParser(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            CollectionAssert.AreEqual(new byte[] { 1, 2, 3, 4 }, parser.GetBytes(4));
        }

        [TestMethod]
        public void GetBytesRemovesBytseAsTheyAreReturned()
        {
            parser = new MessageParser(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            _ = parser.GetBytes(4);
            CollectionAssert.AreEqual(new byte[] { 5, 6 }, parser.GetBytes(2));
        }

        [TestMethod]
        public void GetShortThrowsExceptionWhenBufferHasLessThanTwoBytes()
        {
            parser = new MessageParser(new byte[] { 1 });
            ParseException e = Assert.ThrowsException<ParseException>(() => parser.GetShort());
            Assert.AreEqual("Insufficient bytes remaining: 1 < 2", e.Message);
        }

        [TestMethod]
        public void GetShortReturnsFirstTwoBytesOfBufferAs16BitInteger()
        {
            parser = new MessageParser(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            Assert.AreEqual(513, parser.GetShort());
        }

        [TestMethod]
        public void GetShortRemovesBytesAsTheyAreReturned()
        {
            parser = new MessageParser(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            _ = parser.GetShort();
            Assert.AreEqual(1027, parser.GetShort());
        }

        [TestMethod]
        public void GetPortThrowsExceptionWhenBufferHasLessThanTwoBytes()
        {
            parser = new MessageParser(new byte[] { 1 });
            ParseException e = Assert.ThrowsException<ParseException>(() => parser.GetPort());
            Assert.AreEqual("Insufficient bytes remaining: 1 < 2", e.Message);
        }

        [TestMethod]
        public void GetPortReturnsFirstTwoBytesOfBufferAsNetworkOrderPortNumber()
        {
            parser = new MessageParser(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            Assert.AreEqual(258, parser.GetPort());
        }

        [TestMethod]
        public void GetPortReturnsPortNumberAsUnsignedInteger()
        {
            parser = new MessageParser(new byte[] { 0x99, 0xAA });
            Assert.AreEqual(39338, (int)parser.GetPort());
        }

        [TestMethod]
        public void GetPortRemovesBytesAsTheyAreReturned()
        {
            parser = new MessageParser(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            _ = parser.GetPort();
            Assert.AreEqual(772, parser.GetPort());
        }

        [TestMethod]
        public void GetIPAddressThrowsExceptionWhenBufferHasLessThanFourBytes()
        {
            parser = new MessageParser(new byte[] { 1, 2, 3 });
            ParseException e = Assert.ThrowsException<ParseException>(() => parser.GetIPAddress());
            Assert.AreEqual("Insufficient bytes remaining: 3 < 4", e.Message);
        }

        [TestMethod]
        public void GetIPAddressReturnsFirstFourBytesOfBufferAsNetworkOrderIPAddress()
        {
            parser = new MessageParser(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            Assert.AreEqual(IPAddress.Parse("1.2.3.4"), parser.GetIPAddress());
        }

        [TestMethod]
        public void GetIPAddressRemovesBytesAsTheyAreReturned()
        {
            parser = new MessageParser(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            _ = parser.GetIPAddress();
            Assert.AreEqual(IPAddress.Parse("5.6.7.8"), parser.GetIPAddress());
        }

        [TestMethod]
        public void GetStringReturnsEmptyStringWhenBufferIsEmpty()
        {
            parser = new MessageParser(new byte[] { });
            Assert.AreEqual("", parser.GetString());
        }

        [TestMethod]
        public void GetStringReturnsRemainingBytesAsStringWhenBufferContainsNoZeroBytes()
        {
            parser = new MessageParser(new byte[] { 72, 101, 108, 108, 111 });
            Assert.AreEqual("Hello", parser.GetString());
        }

        [TestMethod]
        public void GetStringReturnsBytesUpToFirstZeroByteAsString()
        {
            parser = new MessageParser(new byte[] { 72, 105, 0, 66, 121, 101 });
            Assert.AreEqual("Hi", parser.GetString());
        }

        [TestMethod]
        public void GetStringRemovesBytesIncludingZeroByteAsTheyAreReturned()
        {
            parser = new MessageParser(new byte[] { 72, 105, 0, 66, 121, 101 });
            _ = parser.GetString();
            Assert.AreEqual(66, parser.GetByte());
        }
    }
}
