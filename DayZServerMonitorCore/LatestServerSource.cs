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
        private readonly ILogger Logger;

        public LatestServerSource(string modifier, string profileDirectory, string profileFilename, ILogger logger)
        {
            Modifier = modifier;
            ProfileDirectory = profileDirectory;
            ProfileFilename = profileFilename;
            Logger = logger;
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
            try
            {
                return await ProfileParser.GetLastServer(Path.Combine(ProfileDirectory, ProfileFilename));
            }
            catch (Exception e)
            {
                Logger.Error("Failed to determine last played server", e);
                return null;
            }
        }
    }
}
