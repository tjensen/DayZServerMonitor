using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace DayZServerMonitorCore
{
    public interface IServerSource
    {
        string GetDisplayName();

        Task<Server> GetServer();

        ProfileWatcher CreateWatcher(Action action, ISynchronizeInvoke synchronizingObject);
    }
}
