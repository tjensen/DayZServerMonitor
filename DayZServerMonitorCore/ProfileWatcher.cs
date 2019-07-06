using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace DayZServerMonitorCore
{
    public class ProfileWatcher : IDisposable
    {
        private readonly FileSystemWatcher watcher;

        public ProfileWatcher(
            string directory, string filename, ISynchronizeInvoke synchronizingObject,
            Action action)
        {
            watcher = new FileSystemWatcher(directory, filename)
            {
                NotifyFilter = NotifyFilters.LastWrite
            };
            watcher.Changed += (s, e) => Handler(action);
            watcher.SynchronizingObject = synchronizingObject;
            watcher.EnableRaisingEvents = true;
        }

        private void Handler(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in ProfileWatcher change action: {0}", e);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    watcher.Dispose();
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
