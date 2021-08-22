using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DayZServerMonitorCore
{
    public class ServerInfo
    {
        public ServerInfo(string address, string name, int numPlayers, int maxPlayers, string time)
        {
            this.Address = address;
            this.Name = name;
            this.NumPlayers = numPlayers;
            this.MaxPlayers = maxPlayers;
            this.Time = time;
        }

        public string Address { get; }
        public string Name { get; }
        public int NumPlayers { get; }
        public int MaxPlayers { get; }
        public string Time { get; }

        private static readonly Regex TIME_MATCHER = new Regex(@"^\d\d:\d\d");

        public override string ToString()
        {
            return $"Server at {Address} is:\r\n" +
                $"Name: {Name}\r\n" +
                $"Players: {NumPlayers}/{MaxPlayers}\r\n" +
                $"Time: {Time}";
        }

        public static ServerInfo Parse(string host, int port, byte[] buffer)
        {
            MessageParser parser = new MessageParser(buffer);

            if (!parser.GetBytes(4).TrueForAll((b) => b == 0xff))
            {
                throw new ParseException("Invalid Packet Header");
            }

            if (parser.GetByte() != 0x49)
            {
                throw new ParseException("Invalid Info Header");
            }

            _ = parser.GetByte(); // Ignore protocol version

            string name = parser.GetString();

            _ = parser.GetString(); // Ignore map
            _ = parser.GetString(); // Ignore folder
            _ = parser.GetString(); // Ignore game
            _ = parser.GetShort(); // Ignore ID

            int numPlayers = parser.GetByte();

            int maxPlayers = parser.GetByte();

            _ = parser.GetBytes(5); // Ignore bots, server type, environment, visibility, and VAC
            _ = parser.GetString(); // Ignore version

            byte flags = parser.GetByte();

            if ((flags & 0x80) != 0)
            {
                port = parser.GetShort();
            }

            if ((flags & 0x10) != 0)
            {
                _ = parser.GetLongLong(); // Ignore server Steam ID
            }

            if ((flags & 0x40) != 0)
            {
                _ = parser.GetShort(); // Ignore spectator port number
                _ = parser.GetString(); // Ignore spectator server name
            }

            string time = "unknown";

            if ((flags & 0x20) != 0)
            {
                string[] keywords = parser.GetString().Split(',');
                foreach (string keyword in keywords)
                {
                    if (TIME_MATCHER.Matches(keyword).Count > 0)
                    {
                        time = keyword;
                        break;
                    }
                }
            }

            return new ServerInfo(
                string.Format("{0}:{1}", host, port), name, numPlayers, maxPlayers, time);
        }
    }
}
