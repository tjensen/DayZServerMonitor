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
                        await QueryServer(form, server);
                    }
                }
                else
                {
                    if (DateTime.Now.Subtract(lastPoll).TotalMilliseconds > POLLING_INTERVAL)
                    {
                        await QueryServer(form, server);
                    }
                }
            }
            finally
            {
                semaphore.Release();
            }
        }

        private async Task<T> WithTimeout<T>(Task<T> mainTask, int timeoutMilliseconds)
        {
            Task timeoutTask = Task.Delay(timeoutMilliseconds);
            Task result = await Task.WhenAny(mainTask, timeoutTask);
            if (result.Equals(mainTask))
            {
                return mainTask.Result;
            }
            throw new TimeoutException();
        }

        private Tuple<string, int, int> ParseQueryResponse(byte[] response)
        {
            ServerInfoParser parser = new ServerInfoParser(response);

            if (!parser.GetBytes(4).TrueForAll((b) => b == 0xff))
            {
                throw new Exception("Invalid Packet Header");
            }

            if (parser.GetByte() != 0x49)
            {
                throw new Exception("Invalid Info Header");
            }

            _ = parser.GetByte(); // Ignore protocol version

            string name = parser.GetString();

            _ = parser.GetString(); // Ignore map
            _ = parser.GetString(); // Ignore folder
            _ = parser.GetString(); // Ignore game
            _ = parser.GetShort(); // Ignore ID

            int players = parser.GetByte();

            int maxPlayers = parser.GetByte();

            return new Tuple<string, int, int>(name, players, maxPlayers);
        }

        private async Task QueryServer(DayZServerMonitorForm form, Server server)
        {
            Tuple<string, int, int> serverInfo = null;

            lastPoll = DateTime.Now;

            try
            {
                // Send Query to DayZ server: https://developer.valvesoftware.com/wiki/Server_queries#A2S_INFO
                using (UdpClient client = new UdpClient(server.Item1, server.Item2 + 24714))
                {
                    List<byte> request = new List<byte>(new byte[] { 0xff, 0xff, 0xff, 0xff, 0x54 });
                    request.AddRange(Encoding.ASCII.GetBytes("Source Engine Query"));
                    request.Add(0);
                    int sendResult = await WithTimeout(client.SendAsync(request.ToArray(), request.Count), SEND_TIMEOUT);
                    if (request.Count == sendResult)
                    {
                        UdpReceiveResult response = await WithTimeout(client.ReceiveAsync(), RECEIVE_TIMEOUT);
                        serverInfo = ParseQueryResponse(response.Buffer);
                    }
                    else
                    {
                        Console.WriteLine("Send returned unexpected result: {0} != {1}", sendResult, request.Count);
                    }
                }

                if (serverInfo == null)
                {
                    form.UpdateValues(string.Format("{0}:{1}", server.Item1, server.Item2));
                }
                else
                {
                    form.UpdateValues(string.Format("{0}:{1}", server.Item1, server.Item2), serverInfo.Item1, serverInfo.Item2, serverInfo.Item3);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to query server: {0}", e);
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
