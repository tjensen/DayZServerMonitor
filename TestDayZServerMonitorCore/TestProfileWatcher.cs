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
        private string tempdir;
        private string filename;

        [TestInitialize]
        public void Initialize()
        {
            tempdir = Path.GetTempFileName();
            File.Delete(tempdir);
            Directory.CreateDirectory(tempdir);
            filename = Path.Combine(tempdir, "some-file.tmp");
            File.WriteAllText(filename, "initial-content");
        }

        [TestCleanup]
        public void Cleanup()
        {
            try
            {
                Directory.Delete(tempdir, true);
            }
            catch (DirectoryNotFoundException error)
            {
                Console.WriteLine("Unable to delete {0}: {1}", tempdir, error);
            }
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
        public void DoesNothingIfDirectoryDoesNotExist()
        {
            AutoResetEvent finished = new AutoResetEvent(false);
            Action action = new Action(() => finished.Set());

            Directory.Delete(tempdir, true);

            using (ProfileWatcher watcher = new ProfileWatcher(
                Path.GetDirectoryName(filename),
                Path.GetFileName(filename),
                null,
                action))
            {
                Assert.IsTrue(true);
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
