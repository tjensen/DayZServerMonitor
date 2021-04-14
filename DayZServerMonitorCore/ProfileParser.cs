using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace DayZServerMonitorCore
{
    public class ProfileParser
    {
        private readonly static Regex LastMPServerRegex = new Regex(
            "^lastMPServer=\"(?<address>.*)\";");

        public static string GetDayZFolder()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DayZ");
        }

        public static string GetExperimentalDayZFolder()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DayZ Exp");
        }

        public static string GetProfileFilename()
        {
            return String.Format("{0}_settings.DayZProfile", Environment.UserName);
        }

        public static async Task<Server> GetLastServer(string path, IClock clock, CancellationTokenSource source)
        {
            try
            {
                return await GetLastServerInternal(path);
            }
            catch (IOException)
            {
                await clock.Delay(1000, source.Token);
                return await GetLastServerInternal(path);
            }
        }

        private static async Task<Server> GetLastServerInternal(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        Match results = LastMPServerRegex.Match(line);
                        if (results.Success)
                        {
                            return new Server(results.Groups["address"].Value);
                        }
                    }
                    throw new MissingLastMPServer();
                }
            }
        }
    }
}
