using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace DayZServerMonitorCore
{
    public class Client : IClient
    {
        private readonly UdpClient client;
        private readonly IClock clock;

        public Client(string host, int port, IClock clock)
        {
            client = new UdpClient(host, port);
            this.clock = clock;
        }

        public async Task<byte[]> Request(byte[] request, int timeout, CancellationTokenSource source)
        {
            return await WithTimeout<byte[]>(MakeRequest(request), timeout, source);
        }

        private async Task<byte[]> MakeRequest(byte[] request)
        {
            int sendResult = await client.SendAsync(request, request.Length);
            if (request.Length != sendResult)
            {
                Console.WriteLine(
                    "Send returned unexpected result: {0} != {1}", sendResult, request.Length);
            }
            UdpReceiveResult response = await client.ReceiveAsync();
            return response.Buffer;
        }

        private async Task<T> WithTimeout<T>(Task<T> mainTask, int timeout, CancellationTokenSource source)
        {
            Task timeoutTask = clock.Delay(timeout, source.Token);
            Task result = await Task.WhenAny(mainTask, timeoutTask);
            if (result.Equals(mainTask))
            {
                return mainTask.Result;
            }
            throw new TimeoutException();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    client.Dispose();
                }
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }

    public class ClientFactory : IClientFactory
    {
        public IClient Create(string host, int port)
        {
            return new Client(host, port, new Clock());
        }
    }
}
