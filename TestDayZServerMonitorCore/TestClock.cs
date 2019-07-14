using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestDayZServerMonitorCore
{
    [TestClass]
    public class TestClock
    {
        private readonly Clock clock = new Clock();

        [TestMethod]
        public void UtcNowReturnsTheCurrentDateTimeAsUTC()
        {
            DateTime now = clock.UtcNow();
            DateTime actualNow = DateTime.UtcNow;

            Assert.IsTrue(actualNow - now < new TimeSpan(0, 0, 0, 0, 100));
        }

        [TestMethod]
        public async Task DelayWaitsForGivenNumberOfMilliseconds()
        {
            DateTime before = DateTime.UtcNow;
            using (CancellationTokenSource source = new CancellationTokenSource())
            {
                await clock.Delay(100, source.Token);
            }
            TimeSpan waited = DateTime.UtcNow - before;

            Assert.IsTrue(waited > new TimeSpan(0, 0, 0, 0, 100));
            Assert.IsTrue(waited < new TimeSpan(0, 0, 0, 0, 200));
        }

        [TestMethod]
        public void DelayCanBeCancelled()
        {
            using (CancellationTokenSource source = new CancellationTokenSource())
            {
                DateTime before = DateTime.Now;
                Task task = clock.Delay(100, source.Token);
                Assert.IsFalse(task.IsCanceled);
                source.Cancel();
                Assert.IsTrue(task.IsCanceled);
                TimeSpan waited = DateTime.Now - before;

                Assert.IsTrue(waited < new TimeSpan(0, 0, 0, 0, 100));
            }
        }

        [TestMethod]
        public void CreateIntervalTimerCallsActionOnceForEveryPollingInterval()
        {
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            int count = 0;
            Action counter = new Action(() => { if (++count == 4) { _ = autoResetEvent.Set(); } });
            DateTime start = DateTime.UtcNow;
            using (IDisposable timer = clock.CreateIntervalTimer(counter, 25, null))
            {
                _ = autoResetEvent.WaitOne(60000);
            }
            DateTime finish = DateTime.UtcNow;
            Assert.AreEqual(4, count);
            double delta = (finish - start).TotalMilliseconds;
            Assert.IsTrue(delta >= 100, string.Format("Time delta ({0}) is not greater than or equal to 100", delta));
        }
    }
}
