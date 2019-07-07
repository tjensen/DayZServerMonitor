using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace DayZServerMonitorCore
{
    public interface IClock
    {
        DateTime Now();

        Task Delay(int milliseconds, CancellationToken cancellationToken);

        IDisposable CreateIntervalTimer(
            Action action, int milliseconds, ISynchronizeInvoke synchronizingObject);
    }
}
