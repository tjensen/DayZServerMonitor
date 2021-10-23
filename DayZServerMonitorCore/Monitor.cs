using System;
using System.Threading;
using System.Threading.Tasks;

namespace DayZServerMonitorCore
{
    public class Monitor
    {
        public const int POLLING_INTERVAL = 60000;
        private const int MINIMUM_POLLING_INTERVAL = 45000;
        private const int SERVER_TIMEOUT = 5000;

        private readonly IClock clock;
        private readonly IClientFactory clientFactory;
        private readonly ILogger logger;
        private DateTime lastPoll;
        private (string address, Server server, DateTime dateTime) gameServerMapping = (null, null, new DateTime());
        private string previousPolledServer = null;

        public Monitor(IClock clock, IClientFactory clientFactory, ILogger logger)
        {
            this.clock = clock;
            this.clientFactory = clientFactory;
            this.logger = logger;

            lastPoll = new DateTime(0);
        }

        public async Task<ServerInfo> Poll(Server lastServer, CancellationTokenSource source)
        {
            if (lastServer.Address == previousPolledServer &&
                (clock.UtcNow() - lastPoll).TotalMilliseconds < MINIMUM_POLLING_INTERVAL)
            {
                logger.Debug("Not enough time has passed since last poll; skipping");
                return null;
            }
            lastPoll = clock.UtcNow();
            previousPolledServer = lastServer.Address;
            Server server = await GetGameServer(lastServer, source);
            ServerInfoQuerier querier = new ServerInfoQuerier(clientFactory, logger);
            return await querier.Query(server.Host, server.Port, SERVER_TIMEOUT, source);
        }

        private async Task<Server> GetGameServer(Server server, CancellationTokenSource source)
        {
            if (gameServerMapping.address != server.Address)
            {
                MasterServerQuerier masterQuerier = new MasterServerQuerier(clientFactory, logger);
                gameServerMapping = (
                    address: server.Address,
                    server: await masterQuerier.FindDayZServerInRegion(
                        server.Host, server.Port,
                        MasterServerQuerier.REGION_REST, SERVER_TIMEOUT, source),
                    dateTime: clock.UtcNow());
            }
            if (gameServerMapping.server == null)
            {
                gameServerMapping = (null, null, new DateTime());
                logger.Debug(
                    $"Server {server} is not in master list; guessing status port number");
                return new Server(server.Host, server.StatsPort);
            }
            return gameServerMapping.server;
        }
    }
}
