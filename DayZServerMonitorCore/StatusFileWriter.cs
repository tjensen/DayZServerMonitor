using System;
using System.IO;

namespace DayZServerMonitorCore
{
    public class StatusFileWriter
    {
        private readonly StatusFileSetting[] statusFiles;
        private readonly ILogger logger;

        public StatusFileWriter(StatusFileSetting[] statusFiles, ILogger logger)
        {
            this.statusFiles = statusFiles;
            this.logger = logger;
        }

        public void WriteStatus(
            string address, string name, int numPlayers, int maxPlayers, string time)
        {
            foreach (StatusFileSetting setting in statusFiles)
            {
                if (!setting.Enabled)
                {
                    continue;
                }

                try
                {
                    File.WriteAllText(
                        setting.Pathname,
                        InterpolateFormatString(
                            setting.Content, address, name, numPlayers, maxPlayers, time));
                }
                catch (Exception error)
                {
                    logger.Error($"Failed to write status to {setting.Pathname}", error);
                }
            }
        }

        private string InterpolateFormatString(
            string format, string address, string name, int numPlayers, int maxPlayers,
            string time)
        {
            string result = "";
            bool percent = false;

            for (int i = 0; i < format.Length; i++)
            {
                if (percent)
                {
                    result += (format[i]) switch
                    {
                        'N' => name,
                        'A' => address,
                        'P' => numPlayers.ToString(),
                        'M' => maxPlayers.ToString(),
                        'T' => time,
                        _ => $"%{format[i]}",
                    };
                    percent = false;
                }
                else if (format[i] == '%')
                {
                    percent = true;
                }
                else
                {
                    result += format[i];
                }
            }

            if (percent)
            {
                result += "%";
            }

            return result;
        }
    }
}
