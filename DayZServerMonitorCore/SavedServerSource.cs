using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace DayZServerMonitorCore
{
    public class SavedServerSource : IServerSource
    {
        private Server server;

        public SavedServerSource(SavedSource source)
        {
            server = new Server(source.Address);
            ServerName = source.Name;
        }

        public SavedServerSource(Server server)
        {
            this.server = server;
            ServerName = null;
        }

        public SavedServerSource(Server server, string name)
        {
            this.server = server;
            ServerName = name;
        }

        public string Address
        {
            get => server.Address;
            set { server = new Server(value); }
        }

        public string ServerName { get; set; }

        public ProfileWatcher CreateWatcher(Action action, ISynchronizeInvoke synchronizingObject)
        {
            return null;
        }

        public string GetDisplayName()
        {
            if (ServerName != null)
            {
                return $"{ServerName} ({server.Address})";
            }
            return server.Address;
        }

        public Task<Server> GetServer(ILogger logger, IClock clock, CancellationTokenSource source)
        {
            return Task.FromResult(server);
        }

        public SavedSource Save()
        {
            SavedSource result = new SavedSource();
            result.Address = Address;
            result.Name = ServerName;
            return result;
        }
    }
}
