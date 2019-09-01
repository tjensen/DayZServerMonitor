using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace DayZServerMonitorCore
{
    public interface IServerSource
    {
        string GetDisplayName();

        Task<Server> GetServer(ILogger logger);

        ProfileWatcher CreateWatcher(Action action, ISynchronizeInvoke synchronizingObject);

        SavedSource Save();
    }
}
