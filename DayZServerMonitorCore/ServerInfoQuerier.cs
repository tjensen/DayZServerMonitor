using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DayZServerMonitorCore
{
    public class ServerInfoQuerier
    {
        private readonly IClientFactory clientFactory;

        public ServerInfoQuerier(IClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public async Task<ServerInfo> Query(string host, int port, int timeout)
        {
            using (IClient client = clientFactory.Create(host, port))
            {
                List<byte> request = new List<byte>(new byte[] { 0xff, 0xff, 0xff, 0xff, 0x54 });
                request.AddRange(Encoding.UTF8.GetBytes("Source Engine Query"));
                request.Add(0);

                byte[] response = await client.Request(request.ToArray(), timeout);

                return ServerInfo.Parse(host, port, response);
            }
        }
    }
}
