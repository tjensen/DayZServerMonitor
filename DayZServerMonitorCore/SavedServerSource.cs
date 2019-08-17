using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace DayZServerMonitorCore
{
    public class SavedServerSource : IServerSource
    {
        private readonly Server server;
        private string name;

        public SavedServerSource(Server server)
        {
            this.server = server;
            name = null;
        }

        public SavedServerSource(Server server, string name)
        {
            this.server = server;
            this.name = name;
        }

        public ProfileWatcher CreateWatcher(Action action, ISynchronizeInvoke synchronizingObject)
        {
            return null;
        }

        public string GetDisplayName()
        {
            if (name != null)
            {
                return $"{name} ({server.Address})";
            }
            return server.Address;
        }

        public void SetServerName(string newName)
        {
            name = newName;
        }

        public Task<Server> GetServer()
        {
            return Task.FromResult(server);
        }
    }
}
