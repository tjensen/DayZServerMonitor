using System.Collections.Generic;

namespace DayZServerMonitorCore
{
    public class ServerInfo
    {
        private readonly string address;
        private readonly string name;
        private readonly int numPlayers;
        private readonly int maxPlayers;

        public ServerInfo(string address, string name, int numPlayers, int maxPlayers)
        {
            this.address = address;
            this.name = name;
            this.numPlayers = numPlayers;
            this.maxPlayers = maxPlayers;
        }

        public string Address { get => address; }
        public string Name { get => name; }
        public int NumPlayers { get => numPlayers; }
        public int MaxPlayers { get => maxPlayers; }

        public static ServerInfo Parse(string host, int port, byte[] buffer)
        {
            ServerInfoParser parser = new ServerInfoParser(buffer);

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

            return new ServerInfo(
                string.Format("{0}:{1}", host, port), name, numPlayers, maxPlayers);
        }
    }
}
