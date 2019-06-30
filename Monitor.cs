using System;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace DayZServerMonitor
{
    internal class Monitor
    {
        private readonly static double POLLING_INTERVAL = 60000;
        private readonly static Regex LastMPServerRegex = new Regex("^lastMPServer=\"(?<address>[\\d.]+):(?<port>\\d+)\";");
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);
        private DateTime lastPoll;
        private string lastServer;

        public Monitor() => lastPoll = new DateTime(0);

        internal async Task Poll(DayZServerMonitorForm form)
        {
            await semaphore.WaitAsync();
            try
            {
                string server = await GetLastServer();
                if (server != lastServer)
                {
                    lastServer = server;
                    if (server is null)
                    {
                        form.updateValues("UNKNOWN");
                    }
                    else
                    {
                        QueryServer(form, server);
                    }
                }
                else
                {
                    if (DateTime.Now.Subtract(lastPoll).TotalMilliseconds > POLLING_INTERVAL)
                    {
                        QueryServer(form, server);
                    }
                }
            }
            finally
            {
                semaphore.Release();
            }
        }

        private void QueryServer(DayZServerMonitorForm form, string server)
        {
            lastPoll = DateTime.Now;
            // TODO: Send Query to DayZ server: https://developer.valvesoftware.com/wiki/Server_queries#A2S_INFO
            form.updateValues(server);
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
                FileSystemWatcher watcher = new FileSystemWatcher();
                watcher.Path = GetDayZFolder();
                watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                watcher.Filter = GetProfileFilename();
                watcher.Changed += (s, e) => WatcherHandler(handler, s, e);
                watcher.Created += (s, e) => WatcherHandler(handler, s, e);
                watcher.Renamed += (s, e) => WatcherHandler(handler, s, e);
                watcher.Deleted += (s, e) => WatcherHandler(handler, s, e);
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

        private void WatcherHandler(Action handler, object source, FileSystemEventArgs eventArgs)
        {
            Console.WriteLine("File system changed: {0}", eventArgs);
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

        private async Task<string> GetLastServer()
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
                            return string.Format("{0}:{1}", address, port.ToString());
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
    }
}
