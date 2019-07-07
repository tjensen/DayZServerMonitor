using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DayZServerMonitorCore
{
    public class MasterServerQuerier
    {
        public readonly static byte REGION_REST = 0xFF;
        private readonly static string MASTER_SERVER_HOST = "hl2master.steampowered.com";
        private readonly static int MASTER_SERVER_PORT = 27011;

        private readonly IClientFactory factory;

        public MasterServerQuerier(IClientFactory factory)
        {
            this.factory = factory;
        }

        public async Task<Server> FindDayZServerInRegion(
            string host, int port, byte region, int timeout)
        {
            try
            {
                using (IClient client = factory.Create(MASTER_SERVER_HOST, MASTER_SERVER_PORT))
                {
                    List<byte> request = new List<byte>(new byte[] { 0x31, region });
                    request.AddRange(Encoding.UTF8.GetBytes("0.0.0.0:0"));
                    request.Add(0);
                    request.AddRange(Encoding.UTF8.GetBytes(@"\game\DayZ\gameaddr\"));
                    request.AddRange(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", host, port)));
                    request.Add(0);

                    byte[] response = await client.Request(request.ToArray(), timeout);
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
                Console.WriteLine("Error querying master server: {0}", e);
                return null;
            }
        }
    }
}
