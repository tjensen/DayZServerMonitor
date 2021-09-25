using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DayZServerMonitorCore
{
    public class ServerInfoQuerier
    {
        private readonly IClientFactory clientFactory;
        private readonly ILogger logger;

        public ServerInfoQuerier(IClientFactory clientFactory, ILogger logger)
        {
            this.clientFactory = clientFactory;
            this.logger = logger;
        }

        public async Task<ServerInfo> Query(string host, int port, int timeout, CancellationTokenSource source)
        {
            logger.Status($"Reading DayZ server status at {host}:{port}");
            try
            {
                using IClient client = clientFactory.Create(host, port);
                List<byte> request = new List<byte>(new byte[] { 0xff, 0xff, 0xff, 0xff, 0x54 });
                request.AddRange(Encoding.UTF8.GetBytes("Source Engine Query"));
                request.Add(0);

                byte[] response = await client.Request(request.ToArray(), timeout, source);

                if (ServerInfo.IsChallenge(response, out byte[] challenge))
                {
                    logger.Status($"Received challenge from DayZ server at {host}:{port}");
                    request.AddRange(challenge);
                    response = await client.Request(request.ToArray(), timeout, source);
                }

                ServerInfo info = ServerInfo.Parse(host, port, response);

                logger.Status($"Finished reading DayZ server status at {host}:{port}");
                return info;
            }
            catch (Exception e)
            {
                logger.Error("Error reading DayZ server status", e);
                return null;
            }
        }
    }
}
