using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DayZServerMonitorCore
{
    public class Client : IClient
    {
        private readonly UdpClient client;

        public Client(string host, int port) => client = new UdpClient(host, port);

        public async Task<byte[]> Request(byte[] request, int timeout)
        {
            try
            {
                int sendResult = await WithTimeout(
                    client.SendAsync(request, request.Length), timeout);
                if (request.Length != sendResult)
                {
                    Console.WriteLine(
                        "Send returned unexpected result: {0} != {1}", sendResult, request.Length);
                }
                UdpReceiveResult response = await WithTimeout(client.ReceiveAsync(), timeout);
                return response.Buffer;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in client request: {0}", e);
                return null;
            }
        }

        private static async Task<T> WithTimeout<T>(Task<T> mainTask, int timeoutMilliseconds)
        {
            Task timeoutTask = Task.Delay(timeoutMilliseconds);
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
            return new Client(host, port);
        }
    }
}
