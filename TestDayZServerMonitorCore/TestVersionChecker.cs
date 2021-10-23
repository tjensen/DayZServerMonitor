using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TestDayZServerMonitorCore
{
    [TestClass]
    public class TestVersionChecker
    {
        private readonly MockLogger logger = new MockLogger();

        private static Mock<HttpMessageHandler> PrepareMockHandler(Object response)
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            var setup = handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());

            if (response is Exception)
            {
                _ = setup.ThrowsAsync(response as Exception);
            }
            else
            {
                _ = setup.ReturnsAsync(response as HttpResponseMessage);
            }

            return handlerMock;
        }

        [TestMethod]
        public async Task RequestsLatestVersionAndReturnsIt()
        {
            var handlerMock = PrepareMockHandler(
                new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("1.2.3.4\n")
                });
            var client = new HttpClient(handlerMock.Object);
            var checker = new VersionChecker(client, logger);

            var version = await checker.Check();

            Assert.AreEqual(new Version(1, 2, 3, 4), version);

            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(
                    req => req.Method == HttpMethod.Get && req.RequestUri.ToString() == "https://tjensen.github.io/DayZServerMonitor/version.txt"),
                ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod]
        public async Task CheckReturnsNullIfLatestVersionUnableToBeParsed()
        {
            var handlerMock = PrepareMockHandler(
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("INVALID-VERSION")
                });
            var client = new HttpClient(handlerMock.Object);
            var checker = new VersionChecker(client, logger);

            var version = await checker.Check();

            Assert.IsNull(version);
        }

        [TestMethod]
        public async Task CheckReturnsNullIfHttpRequestReturnsUnsuccessfulStatusCode()
        {
            var handlerMock = PrepareMockHandler(
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("1.2.3.4\n")
                });
            var client = new HttpClient(handlerMock.Object);
            var checker = new VersionChecker(client, logger);

            var version = await checker.Check();

            Assert.IsNull(version);
        }

        [TestMethod]
        public async Task CheckReturnsNullIfHttpRequestThrowsException()
        {
            var handlerMock = PrepareMockHandler(new Exception("GetAsync error"));
            var client = new HttpClient(handlerMock.Object);
            var checker = new VersionChecker(client, logger);

            var version = await checker.Check();

            Assert.IsNull(version);
        }
    }
}
