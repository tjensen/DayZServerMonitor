using System;

namespace DayZServerMonitorCore
{
    public class ParseException : Exception
    {
        public ParseException(string message) : base(message)
        {
        }
    }
}
