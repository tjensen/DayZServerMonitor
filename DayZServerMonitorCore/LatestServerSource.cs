using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;

namespace DayZServerMonitorCore
{
    public class LatestServerSource : IServerSource
    {
        public LatestServerSource(SavedSource source)
        {
            Modifier = source.Filename;
            ProfileDirectory = Path.GetDirectoryName(source.Filename);
            ProfileFilename = Path.GetFileName(source.Filename);
        }

        public LatestServerSource(string modifier, string profileDirectory, string profileFilename)
        {
            Modifier = modifier;
            ProfileDirectory = profileDirectory;
            ProfileFilename = profileFilename;
        }

        public string Modifier { get; private set; }

        public string ProfileDirectory { get; private set; }
        public string ProfileFilename { get; private set; }

        public ProfileWatcher CreateWatcher(Action action, ISynchronizeInvoke synchronizingObject)
        {
            return new ProfileWatcher(ProfileDirectory, ProfileFilename, synchronizingObject, action);
        }

        public string GetDisplayName()
        {
            return $"Most Recent ({Modifier})";
        }

        public async Task<Server> GetServer(ILogger logger)
        {
            try
            {
                return await ProfileParser.GetLastServer(Path.Combine(ProfileDirectory, ProfileFilename));
            }
            catch (Exception e)
            {
                logger.Error("Failed to determine last played server", e);
                return null;
            }
        }

        public SavedSource Save()
        {
            SavedSource source = new SavedSource();
            source.Filename = Path.Combine(ProfileDirectory, ProfileFilename);
            return source;
        }
    }
}
