using System;
using System.IO;
using System.Text.RegularExpressions;
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

        public static async Task<Server> GetLastServer(string path)
        {
            using (StreamReader reader = File.OpenText(path))
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
