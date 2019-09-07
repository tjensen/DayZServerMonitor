using System;

namespace DayZServerMonitorCore
{
    public interface ILogger
    {
        void Status(string text);

        void Error(string text, Exception exception);

        void Debug(string text);
    }
}
