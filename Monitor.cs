using DayZServerMonitorCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace DayZServerMonitor
{
    internal class Monitor : IDisposable
    {
        private static readonly double POLLING_INTERVAL = 60000;
        private static readonly int SERVER_TIMEOUT = 5000;
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);
        private DateTime lastPoll;
        private Server lastServer;
        private readonly Dictionary<Server, Server> gameServerMapping =
            new Dictionary<Server, Server>();

        public Monitor() => lastPoll = new DateTime(0);

        internal async Task Poll(DayZServerMonitorForm form)
        {
            Server server = null;
            await semaphore.WaitAsync();
            try
            {
                server = await ProfileParser.GetLastServer(
                    Path.Combine(
                        ProfileParser.GetDayZFolder(),
                        ProfileParser.GetProfileFilename()));
                if (server.Equals(lastServer))
                {
                    if (DateTime.Now.Subtract(lastPoll).TotalMilliseconds > POLLING_INTERVAL)
                    {
                        lastPoll = DateTime.Now;
                        await Query(form, server);
                    }
                }
                else
                {
                    lastServer = server;
                    await Query(form, server);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while polling: {0}", e);
                form.UpdateValues(server == null ? "UNKNOWN" : server.Address);
            }
            finally
            {
                semaphore.Release();
            }
        }

        private async Task Query(DayZServerMonitorForm form, Server server)
        {
            if (!gameServerMapping.ContainsKey(server))
            {
                // Do we need to search *all* regions or only the "rest" region?
                MasterServerQuerier masterQuerier = new MasterServerQuerier(new ClientFactory());
                Server gameServer = await masterQuerier.FindDayZServerInRegion(
                    server.Host, server.Port, MasterServerQuerier.REGION_REST, SERVER_TIMEOUT);
                if (gameServer is null)
                {
                    gameServer = new Server(server.Host, server.StatsPort);
                }
                gameServerMapping.Add(server, gameServer);
            }

            Server queryServer = gameServerMapping[server];
            ServerInfoQuerier querier = new ServerInfoQuerier(new ClientFactory());
            ServerInfo info = await querier.Query(
                queryServer.Host, queryServer.Port, SERVER_TIMEOUT);
            if (info == null)
            {
                form.UpdateValues(server.Address);
            }
            else
            {
                form.UpdateValues(info.Address, info.Name, info.NumPlayers, info.MaxPlayers);
            }
        }

        internal Timer CreateTimer(ISynchronizeInvoke synchronizingObject, Action handler)
        {
            Timer timer = new System.Timers.Timer(POLLING_INTERVAL);
            timer.Elapsed += (s, e) => handler();
            timer.SynchronizingObject = synchronizingObject;
            timer.AutoReset = true;
            timer.Start();
            return timer;
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
