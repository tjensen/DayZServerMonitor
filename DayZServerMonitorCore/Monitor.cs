using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DayZServerMonitorCore
{
    public class Monitor : IDisposable
    {
        public static readonly int POLLING_INTERVAL = 60000;
        private static readonly int SERVER_TIMEOUT = 5000;

        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);
        private readonly Dictionary<string, Server> gameServerMapping =
            new Dictionary<string, Server>();
        private readonly IClock clock;
        private readonly IClientFactory clientFactory;
        private readonly ILogger logger;
        private DateTime lastPoll;
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
                    (clock.UtcNow() - lastPoll).TotalMilliseconds < POLLING_INTERVAL)
                {
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
            if (!gameServerMapping.ContainsKey(server.Address))
            {
                MasterServerQuerier masterQuerier = new MasterServerQuerier(clientFactory, logger);
                gameServerMapping[server.Address] = await masterQuerier.FindDayZServerInRegion(
                    server.Host, server.Port, MasterServerQuerier.REGION_REST, SERVER_TIMEOUT);
            }
            if (gameServerMapping[server.Address] == null)
            {
                gameServerMapping[server.Address] = new Server(server.Host, server.StatsPort);
            }
            return gameServerMapping[server.Address];
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
