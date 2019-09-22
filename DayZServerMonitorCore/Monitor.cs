using System;
using System.Threading;
using System.Threading.Tasks;

namespace DayZServerMonitorCore
{
    public class Monitor : IDisposable
    {
        public const int POLLING_INTERVAL = 60000;
        private const int MINIMUM_POLLING_INTERVAL = 45000;
        private const int SERVER_TIMEOUT = 5000;

        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);
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

        public async Task<ServerInfo> Poll(Server lastServer)
        {
            await semaphore.WaitAsync();
            try
            {
                if (lastServer.Address == previousPolledServer &&
                    (clock.UtcNow() - lastPoll).TotalMilliseconds < MINIMUM_POLLING_INTERVAL)
                {
                    logger.Debug("Not enough time has passed since last poll; skipping");
                    return null;
                }
                lastPoll = clock.UtcNow();
                previousPolledServer = lastServer.Address;
                Server server = await GetGameServer(lastServer);
                ServerInfoQuerier querier = new ServerInfoQuerier(clientFactory, logger);
                return await querier.Query(server.Host, server.Port, SERVER_TIMEOUT);
            }
            finally
            {
                semaphore.Release();
            }
        }

        private async Task<Server> GetGameServer(Server server)
        {
            if (gameServerMapping.address != server.Address
                || clock.UtcNow() - gameServerMapping.dateTime >= TimeSpan.FromMinutes(10))
            {
                MasterServerQuerier masterQuerier = new MasterServerQuerier(clientFactory, logger);
                gameServerMapping = (
                    address: server.Address,
                    server: await masterQuerier.FindDayZServerInRegion(
                        server.Host, server.Port,
                        MasterServerQuerier.REGION_REST, SERVER_TIMEOUT),
                    dateTime: clock.UtcNow());
            }
            if (gameServerMapping.server == null)
            {
                logger.Debug(
                    $"Server {server} is not in master list; guessing status port number");
                gameServerMapping = (
                    address: server.Address,
                    server: new Server(server.Host, server.StatsPort),
                    dateTime: clock.UtcNow());
            }
            return gameServerMapping.server;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    semaphore.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
