using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using DayZServerMonitorCore;

namespace TestDayZServerMonitorCore
{
    class MockClock : IClock, IDisposable
    {
        private readonly SemaphoreSlim delayCompleted = new SemaphoreSlim(0);

        public DateTime CurrentTime { get; set; }

        public EventHandler DelayCalled;

        public MockClock()
        {
            CurrentTime = new DateTime(0);
        }

        public DateTime UtcNow()
        {
            return CurrentTime;
        }

        public async Task Delay(int milliseconds, CancellationToken cancellationToken)
        {
            OnDelayCalled(EventArgs.Empty);
            await delayCompleted.WaitAsync(cancellationToken);
            CurrentTime += new TimeSpan(0, 0, 0, 0, milliseconds);
        }

        public IDisposable CreateIntervalTimer(
            Action action, int milliseconds, ISynchronizeInvoke synchronizingObject)
        {
            return null;
        }

        public void SetDelayCompleted()
        {
            delayCompleted.Release();
        }

        protected virtual void OnDelayCalled(EventArgs args)
        {
            EventHandler handler = DelayCalled;
            handler?.Invoke(this, args);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    delayCompleted.Dispose();
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
