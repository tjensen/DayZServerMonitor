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
        public void NowReturnsTheCurrentDateTime()
        {
            DateTime now = clock.Now();
            DateTime actualNow = DateTime.Now;

            Assert.IsTrue(actualNow - now < new TimeSpan(0, 0, 0, 0, 100));
        }

        [TestMethod]
        public async Task DelayWaitsForGivenNumberOfMilliseconds()
        {
            DateTime before = DateTime.Now;
            using (CancellationTokenSource source = new CancellationTokenSource())
            {
                await clock.Delay(100, source.Token);
            }
            TimeSpan waited = DateTime.Now - before;

            Assert.IsTrue(waited > new TimeSpan(0, 0, 0, 0, 100));
            Assert.IsTrue(waited < new TimeSpan(0, 0, 0, 0, 125));
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
            int count = 0;
            Action counter = new Action(() => count++);
            using (IDisposable timer = clock.CreateIntervalTimer(counter, 21, null))
            {
                Thread.Sleep(100);
            }
            Assert.AreEqual(4, count);
        }
    }
}
