using System;
using System.IO;
using System.Text;

namespace DayZServerMonitorCore
{
    public class Logger : ILogger
    {
        private readonly Settings settings;
        private readonly IClock clock;
        private readonly Action<string> statusWriter;
        private readonly Action<string, string> logWriter;
        private FileStream logStream = null;

        public Logger(
            Settings settings, IClock clock,
            Action<string> statusWriter, Action<string, string> logWriter)
        {
            this.settings = settings;
            this.clock = clock;
            this.statusWriter = statusWriter;
            this.logWriter = logWriter;
            OpenLogFile();
            settings.SettingChanged += Settings_SettingChanged;
        }

        public void Status(string text)
        {
            WriteLog("STATUS", text);
            StatusInternal(text);
        }

        private void StatusInternal(string text)
        {
            DateTime now = clock.UtcNow().ToLocalTime();
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            statusWriter($"{now.TimeOfDay:c} - {text}");
        }

        public void Error(string text, Exception exception)
        {
            WriteLog("ERROR", $"{text}\r\n{exception}");
            string message = $"{text}: {exception.Message}";
            StatusInternal(message);
            Console.WriteLine(message);
        }

        public void Debug(string text)
        {
            WriteLog("DEBUG", text);
        }

        private void WriteLog(string level, string text)
        {
            logWriter(level, text);
            if (logStream != null)
            {
                try
                {
                    DateTime now = clock.UtcNow().ToLocalTime();
                    now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
                    string message = $"{now} - {level} - {text}\r\n";
                    logStream.Write(Encoding.UTF8.GetBytes(message), 0, message.Length);
                    logStream.Flush();
                }
                catch (Exception error)
                {
                    Console.WriteLine($"Failed to write to log file: {error}");
                }
            }
        }

        private void OpenLogFile()
        {
            if (logStream != null)
            {
                logStream.Close();
                logStream = null;
            }
            if (settings.LogPathname != null)
            {
                try
                {
                    logStream = File.Open(settings.LogPathname, FileMode.Append, FileAccess.Write, FileShare.Read);
                }
                catch (Exception error)
                {
                    Error($"Failed to open log file {settings.LogPathname}", error);
                }
            }
        }

        private void Settings_SettingChanged(object source, SettingChangedArgs args)
        {
            if (args.SettingName == "LogPathname")
            {
                OpenLogFile();
            }
        }
    }
}
