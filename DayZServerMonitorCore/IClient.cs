using System;
using System.Threading;
using System.Threading.Tasks;

namespace DayZServerMonitorCore
{
    public interface IClient : IDisposable
    {
        Task<byte[]> Request(byte[] request, int timeout, CancellationTokenSource source);
    }

    public interface IClientFactory
    {
        IClient Create(string host, int port);
    }
}
