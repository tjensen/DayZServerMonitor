﻿using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace DayZServerMonitorCore
{
    public interface IServerSource
    {
        string GetDisplayName();

        Task<Server> GetServer(ILogger logger, IClock clock, CancellationTokenSource source);

        ProfileWatcher CreateWatcher(Action action, ISynchronizeInvoke synchronizingObject);

        SavedSource Save();
    }
}
