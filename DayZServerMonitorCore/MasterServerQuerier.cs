using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DayZServerMonitorCore
{
    public class MasterServerQuerier
    {
#pragma warning disable IDE1006 // Naming Styles
        class ServerListServer
        {
            public string addr { get; set; }
            public int appid { get; set; }
            public int gameport { get; set; }
        }
        class ServerListResponse
        {
            public bool success { get; set; }
            public IList<ServerListServer> servers { get; set; }
        }
        class ServerListAnswer
        {
            public ServerListResponse response { get; set; }
        }
#pragma warning restore IDE1006 // Naming Styles

        public const byte REGION_REST = 0xFF;
        private const string MASTER_SERVER_HOST = "hl2master.steampowered.com";
        private const int MASTER_SERVER_PORT = 27011;
        private const string API_ENDPOINT = "https://api.steampowered.com/ISteamApps/GetServersAtAddress/v1/";
        private const int DAYZ_APPID = 221100;

        private static readonly HttpClient httpClient;

        private readonly IClientFactory factory;
        private readonly ILogger logger;

        static MasterServerQuerier()
        {
            httpClient = new HttpClient();
        }

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
                using IClient client = factory.Create(MASTER_SERVER_HOST, MASTER_SERVER_PORT);
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
            catch (Exception e)
            {
                logger.Error("Error querying master server", e);
                return null;
            }
        }

        public async Task<Server> FindDayZServer(
            string host, int port, CancellationTokenSource source)
        {
            try
            {
                logger.Status($"Finding server {host}:{port} via Steam API");
                using (var response = await httpClient.GetAsync($"{API_ENDPOINT}?addr={host}"))
                {
                    response.EnsureSuccessStatusCode();
                    logger.Debug("Parsing Steam API response");
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var serializer = new JsonSerializer();
                    var answer = serializer.Deserialize<ServerListAnswer>(
                        new JsonTextReader(new StringReader(responseBody)));

                    logger.Debug("Searching for matching server in response");
                    foreach (var server in answer.response.servers)
                    {
                        if (server.appid == DAYZ_APPID && server.gameport == port)
                        {
                            logger.Debug("Matching server found!");
                            var parts = server.addr.Split(':');
                            return new Server(parts[0], int.Parse(parts[1]));
                        }
                    }
                }

                throw new ApplicationException($"Server matching {host}:{port} not found in Steam API response!");
            }
            catch (Exception e)
            {
                logger.Error("Error getting servers at address", e);
                return null;
            }
        }
    }
}
