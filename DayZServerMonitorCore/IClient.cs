using System;
using System.Threading.Tasks;

namespace DayZServerMonitorCore
{
    public interface IClient : IDisposable
    {
        Task<byte[]> Request(byte[] request, int timeout);
    }

    public interface IClientFactory
    {
        IClient Create(string host, int port);
    }
}
