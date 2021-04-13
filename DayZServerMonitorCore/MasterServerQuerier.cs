using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DayZServerMonitorCore
{
    public class MasterServerQuerier
    {
        public const byte REGION_REST = 0xFF;
        private const string MASTER_SERVER_HOST = "hl2master.steampowered.com";
        private const int MASTER_SERVER_PORT = 27011;

        private readonly IClientFactory factory;
        private readonly ILogger logger;

        public MasterServerQuerier(IClientFactory factory, ILogger logger)
        {
            this.factory = factory;
            this.logger = logger;
        }

        public async Task<Server> FindDayZServerInRegion(
            string host, int port, byte region, int timeout, CancellationTokenSource source)
        {
            try
            {
                logger.Status($"Finding server {host}:{port} in master server list");
                using (IClient client = factory.Create(MASTER_SERVER_HOST, MASTER_SERVER_PORT))
                {
                    List<byte> request = new List<byte>(new byte[] { 0x31, region });
                    request.AddRange(Encoding.UTF8.GetBytes("0.0.0.0:0"));
                    request.Add(0);
                    request.AddRange(Encoding.UTF8.GetBytes(@"\game\DayZ\gameaddr\"));
                    request.AddRange(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", host, port)));
                    request.Add(0);

                    byte[] response = await client.Request(request.ToArray(), timeout, source);
                    MessageParser parser = new MessageParser(response);
                    _ = parser.GetBytes(6);
                    IPAddress ip = parser.GetIPAddress();
                    int queryPort = parser.GetPort();
                    if (ip.Equals(IPAddress.Any) && queryPort == 0)
                    {
                        return null;
                    }
                    return new Server(ip.ToString(), queryPort);
                }
            }
            catch (Exception e)
            {
                logger.Error("Error querying master server", e);
                return null;
            }
        }
    }
}
