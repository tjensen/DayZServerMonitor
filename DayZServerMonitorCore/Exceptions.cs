using System;

namespace DayZServerMonitorCore
{
    public class ParseException : Exception
    {
        public ParseException(string message) : base(message)
        {
        }
    }

    public class MissingLastMPServer : Exception
    {
        public MissingLastMPServer() : base("Unable to find last MP server in DayZ profile")
        {
        }
    }
}
