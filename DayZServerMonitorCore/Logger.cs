using System;

namespace DayZServerMonitorCore
{
    public class Logger : ILogger
    {
        private readonly IClock clock;
        private readonly Action<string> statusWriter;

        public Logger(IClock clock, Action<string> statusWriter)
        {
            this.clock = clock;
            this.statusWriter = statusWriter;
        }

        public void Status(string text)
        {
            DateTime now = clock.UtcNow().ToLocalTime();
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            statusWriter($"{now.TimeOfDay:c} - {text}");
        }

        public void Error(string text, Exception exception)
        {
            Status($"{text}: {exception.Message}");
        }
    }
}
