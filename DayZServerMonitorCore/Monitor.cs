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
        private readonly string profileFilename;
        private readonly IClock clock;
        private readonly IClientFactory clientFactory;
        private DateTime lastPoll;
        private string previousPolledServer = null;

        public Monitor(string profileFilename, IClock clock, IClientFactory clientFactory)
        {
            this.profileFilename = profileFilename;
            this.clock = clock;
            this.clientFactory = clientFactory;
            lastPoll = new DateTime(0);
        }

        public async Task<ServerInfo> Poll()
        {
            await semaphore.WaitAsync();
            try
            {
                Server lastServer = await ProfileParser.GetLastServer(profileFilename);
                if (lastServer.Address == previousPolledServer &&
                    (clock.Now() - lastPoll).TotalMilliseconds < POLLING_INTERVAL)
                {
                    return null;
                }
                lastPoll = clock.Now();
                previousPolledServer = lastServer.Address;
                Server server = await GetGameServer(lastServer);
                ServerInfoQuerier querier = new ServerInfoQuerier(clientFactory);
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
                MasterServerQuerier masterQuerier = new MasterServerQuerier(clientFactory);
                gameServerMapping[server.Address] = await masterQuerier.FindDayZServerInRegion(
                    server.Host, server.Port, MasterServerQuerier.REGION_REST, SERVER_TIMEOUT);
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
