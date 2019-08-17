using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;

namespace DayZServerMonitorCore
{
    public class LatestServerSource : IServerSource
    {
        private readonly string Modifier;
        private readonly string ProfileDirectory;
        private readonly string ProfileFilename;

        public LatestServerSource(string modifier, string profileDirectory, string profileFilename)
        {
            Modifier = modifier;
            ProfileDirectory = profileDirectory;
            ProfileFilename = profileFilename;
        }

        public ProfileWatcher CreateWatcher(Action action, ISynchronizeInvoke synchronizingObject)
        {
            return new ProfileWatcher(ProfileDirectory, ProfileFilename, synchronizingObject, action);
        }

        public string GetDisplayName()
        {
            return $"Most Recent ({Modifier})";
        }

        public async Task<Server> GetServer()
        {
            return await ProfileParser.GetLastServer(Path.Combine(ProfileDirectory, ProfileFilename));
        }
    }
}
