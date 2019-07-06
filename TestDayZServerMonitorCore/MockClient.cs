using System;
using System.Threading.Tasks;
using DayZServerMonitorCore;

namespace TestDayZServerMonitorCore
{
    internal class MockClient : IClient
    {
        public byte[] ServerResponse { get; set; }
        public byte[] ServerRequest { get; private set; }

        public int ServerTimeout { get; private set; }

        public void Dispose() { }

        public async Task<byte[]> Request(byte[] request, int timeout)
        {
            return await Task.Run(() =>
            {
                ServerRequest = request;
                ServerTimeout = timeout;
                return ServerResponse;
            });
        }

        public void Reset()
        {
            ServerRequest = null;
            ServerTimeout = 0;
        }
    }

    internal class MockClientFactory : IClientFactory
    {
        public MockClient Client { get; set; }
        public string Host { get; private set; }
        public int Port { get; private set; }

        internal MockClientFactory() { }
        internal MockClientFactory(MockClient client) => Client = client;

        public IClient Create(string host, int port)
        {
            Host = host;
            Port = port;
            return Client;
        }
    }
}
