using DayZServerMonitorCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace DayZServerMonitor
{
    using Server = Tuple<string, int>;

    internal class Monitor : IDisposable
    {
        private readonly static double POLLING_INTERVAL = 60000;
        private readonly static int SEND_TIMEOUT = 1000;
        private readonly static int RECEIVE_TIMEOUT = 5000;
        private readonly static Regex LastMPServerRegex = new Regex("^lastMPServer=\"(?<address>[\\d.]+):(?<port>\\d+)\";");
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);
        private DateTime lastPoll;
        private Server lastServer;

        public Monitor() => lastPoll = new DateTime(0);

        internal async Task Poll(DayZServerMonitorForm form)
        {
            await semaphore.WaitAsync();
            try
            {
                Server server = await GetLastServer();
                if (server != lastServer)
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
                else
                {
                    if (DateTime.Now.Subtract(lastPoll).TotalMilliseconds > POLLING_INTERVAL)
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
            byte[] buffer = await QueryServer.Query(server.Item1, server.Item2 + 24714, SEND_TIMEOUT, RECEIVE_TIMEOUT);
            if (buffer == null)
            {
                form.UpdateValues(string.Format("{0}:{1}", server.Item1, server.Item2));
                return;
            }

            ServerInfo info = ServerInfo.Parse(server.Item1, server.Item2, buffer);
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

        internal FileSystemWatcher CreateDayZProfileWatcher(ISynchronizeInvoke synchronizingObject, Action handler)
        {
            try
            {
                FileSystemWatcher watcher = new FileSystemWatcher
                {
                    Path = GetDayZFolder(),
                    NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                    Filter = GetProfileFilename()
                };
                watcher.Changed += (s, e) => WatcherHandler(handler);
                watcher.Created += (s, e) => WatcherHandler(handler);
                watcher.Renamed += (s, e) => WatcherHandler(handler);
                watcher.Deleted += (s, e) => WatcherHandler(handler);
                watcher.SynchronizingObject = synchronizingObject;
                watcher.EnableRaisingEvents = true;
                return watcher;
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to set up file system watcher: {0}", e);
                return null;
            }
        }

        private void WatcherHandler(Action handler)
        {
            try
            {
                handler();
            }
            catch (Exception error)
            {
                Console.WriteLine("Handler raised exception: {0}", error);
            }
        }

        private string GetDayZFolder()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DayZ");
        }

        private string GetProfileFilename()
        {
            return String.Format("{0}_settings.DayZProfile", Environment.UserName);
        }

        private async Task<Server> GetLastServer()
        {
            try
            {
                string filename = Path.Combine(GetDayZFolder(), GetProfileFilename());
                using (StreamReader reader = File.OpenText(filename))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        Match results = LastMPServerRegex.Match(line);
                        if (results.Success)
                        {
                            string address = results.Groups["address"].Value;
                            // For some reason, DayZ writes the server port number shifted 16 bits to the left.
                            int port = int.Parse(results.Groups["port"].Value) >> 16;
                            return new Server(address, port);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to read last played DayZ server: {0}", e);
            }
            return null;
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
