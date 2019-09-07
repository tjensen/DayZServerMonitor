using System;

namespace DayZServerMonitorCore
{
    public class Logger : ILogger
    {
        private readonly IClock clock;
        private readonly Action<string> statusWriter;
        private readonly Action<string, string> logWriter;

        public Logger(IClock clock, Action<string> statusWriter, Action<string, string> logWriter)
        {
            this.clock = clock;
            this.statusWriter = statusWriter;
            this.logWriter = logWriter;
        }

        public void Status(string text)
        {
            logWriter("STATUS", text);
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
            logWriter("ERROR", $"{text}\r\n{exception}");
            string message = $"{text}: {exception.Message}";
            StatusInternal(message);
            Console.WriteLine(message);
        }

        public void Debug(string text)
        {
            logWriter("DEBUG", text);
        }
    }
}
