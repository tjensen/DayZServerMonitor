using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DayZServerMonitorCore;

namespace TestDayZServerMonitorCore
{
    internal class MockClient : IClient
    {
        internal byte[] ServerResponse { get; set; }
        internal Exception ServerError { get; set; }
        internal byte[] ServerRequest { get; private set; }
        internal int ServerTimeout { get; private set; }
        internal Action RequestAction { get; set; }
        internal CancellationTokenSource Source { get; set; }

        public Task<byte[]> Request(byte[] request, int timeout, CancellationTokenSource source)
        {
            RequestAction?.Invoke();
            ServerRequest = request;
            ServerTimeout = timeout;
            Source = source;
            if (ServerError != null)
            {
                throw ServerError;
            }
            return Task.FromResult(ServerResponse);
        }

        internal void Reset()
        {
            ServerRequest = null;
            ServerTimeout = 0;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }

    internal class MockClientFactory : IClientFactory
    {
        private readonly List<MockClient> clients;

        internal List<Tuple<string, int>> MockCalls { get; private set; }

        internal MockClientFactory()
        {
            clients = new List<MockClient>();
            MockCalls = new List<Tuple<string, int>>();
        }

        internal MockClientFactory(MockClient client)
        {
            clients = new List<MockClient> { client };
            MockCalls = new List<Tuple<string, int>>();
        }

        public IClient Create(string host, int port)
        {
            MockCalls.Add(new Tuple<string, int>(host, port));
            MockClient client = clients[0];
            clients.RemoveAt(0);
            return client;
        }

        internal void AddClient(MockClient client)
        {
            clients.Add(client);
        }

        internal void Reset()
        {
            MockCalls.Clear();
        }
    }
}
