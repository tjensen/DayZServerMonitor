using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;

namespace TestDayZServerMonitorCore
{
    [TestClass]
    public class TestProfileWatcher
    {
        private string filename;

        [TestInitialize]
        public void Initialize()
        {
            filename = Path.GetTempFileName();
        }

        [TestCleanup]
        public void Cleanup()
        {
            File.Delete(filename);
        }

        [TestMethod]
        public void CallsActionWhenFileIsChanged()
        {
            AutoResetEvent finished = new AutoResetEvent(false);
            Action action = new Action(() => finished.Set());

            using (ProfileWatcher watcher = new ProfileWatcher(
                Path.GetDirectoryName(filename),
                Path.GetFileName(filename),
                null,
                action))
            {
                Assert.IsFalse(finished.WaitOne(50, false));

                File.AppendAllText(filename, "some-data");

                Assert.IsTrue(finished.WaitOne(50, false));
            }
        }

        [TestMethod]
        public void EatsExceptionsThrownByAction()
        {
            AutoResetEvent finished = new AutoResetEvent(false);
            Action action = new Action(() =>
            {
                try
                {
                    throw new Exception();
                }
                finally
                {
                    finished.Set();
                }

            });

            using (ProfileWatcher watcher = new ProfileWatcher(
                Path.GetDirectoryName(filename),
                Path.GetFileName(filename),
                null,
                action))
            {
                File.AppendAllText(filename, "some-data");

                Assert.IsTrue(finished.WaitOne(50, false));
            }
        }

        [TestMethod]
        public void StopsWatchingFileWhenDisposed()
        {
            AutoResetEvent finished = new AutoResetEvent(false);
            Action action = new Action(() => finished.Set());

            using (ProfileWatcher watcher = new ProfileWatcher(
                Path.GetDirectoryName(filename),
                Path.GetFileName(filename),
                null,
                action))
            {
                Assert.IsFalse(finished.WaitOne(50, false));
            }

            File.AppendAllText(filename, "some-data");

            Assert.IsFalse(finished.WaitOne(50, false));
        }
    }
}
