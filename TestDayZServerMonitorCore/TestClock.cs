using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
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
            using (CancellationTokenSource source = new CancellationTokenSource())
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                await clock.Delay(100, source.Token);
                stopwatch.Stop();

                Assert.IsTrue(
                    stopwatch.Elapsed >= new TimeSpan(0, 0, 0, 0, 100),
                    $"{stopwatch.Elapsed} should be greater than or equal to 100ms");
                Assert.IsTrue(
                    stopwatch.Elapsed < new TimeSpan(0, 0, 0, 0, 200),
                    $"{stopwatch.Elapsed} should be less than 200ms");
            }
        }

        [TestMethod]
        public void DelayCanBeCancelled()
        {
            using (CancellationTokenSource source = new CancellationTokenSource())
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                Task task = clock.Delay(100, source.Token);
                Assert.IsFalse(task.IsCanceled);
                source.Cancel();
                Assert.IsTrue(task.IsCanceled);
                stopwatch.Stop();

                Assert.IsTrue(stopwatch.Elapsed < new TimeSpan(0, 0, 0, 0, 100));
            }
        }

        [TestMethod]
        public void CreateIntervalTimerCallsActionOnceForEveryPollingInterval()
        {
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            int count = 0;
            Action counter = new Action(() => { if (++count == 4) { _ = autoResetEvent.Set(); } });
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            using (IDisposable timer = clock.CreateIntervalTimer(counter, 25, null))
            {
                _ = autoResetEvent.WaitOne(60000);
            }
            stopwatch.Stop();
            Assert.AreEqual(4, count);
            Assert.IsTrue(
                stopwatch.Elapsed >= new TimeSpan(0, 0, 0, 0, 100),
                $"Time delta ({stopwatch.Elapsed}) is not greater than or equal to 100");
        }
    }
}
