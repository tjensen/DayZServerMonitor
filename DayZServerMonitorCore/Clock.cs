using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace DayZServerMonitorCore
{
    public class Clock : IClock
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }

        public async Task Delay(int milliseconds, CancellationToken cancellationToken)
        {
            await Task.Delay(milliseconds, cancellationToken);
        }

        public IDisposable CreateIntervalTimer(
            Action action, int milliseconds, ISynchronizeInvoke synchronizingObject)
        {
            System.Timers.Timer timer = new System.Timers.Timer(milliseconds);
            timer.Elapsed += (s, e) => action();
            timer.SynchronizingObject = synchronizingObject;
            timer.AutoReset = true;
            timer.Start();
            return timer;
        }
    }
}
