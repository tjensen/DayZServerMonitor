using DayZServerMonitorCore;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace DayZServerMonitor
{
    internal class Monitor : IDisposable
    {
        private readonly static double POLLING_INTERVAL = 60000;
        private readonly static int SEND_TIMEOUT = 1000;
        private readonly static int RECEIVE_TIMEOUT = 5000;
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);
        private DateTime lastPoll;
        private Server lastServer;

        public Monitor() => lastPoll = new DateTime(0);

        internal async Task Poll(DayZServerMonitorForm form)
        {
            await semaphore.WaitAsync();
            try
            {
                Server server = await ProfileParser.GetLastServer(
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
                    if (server is null)
                    {
                        form.UpdateValues("UNKNOWN");
                    }
                    else
                    {
                        await Query(form, server);
                    }
                }
            }
            finally
            {
                semaphore.Release();
            }
        }

        private async Task Query(DayZServerMonitorForm form, Server server)
        {
            byte[] buffer = await QueryServer.Query(server.Host, server.StatsPort, SEND_TIMEOUT, RECEIVE_TIMEOUT);
            if (buffer == null)
            {
                form.UpdateValues(server.Address);
                return;
            }

            ServerInfo info = ServerInfo.Parse(server.Host, server.Port, buffer);
            form.UpdateValues(info.Address, info.Name, info.NumPlayers, info.MaxPlayers);
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
