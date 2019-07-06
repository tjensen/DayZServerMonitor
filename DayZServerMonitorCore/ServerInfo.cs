using System.Collections.Generic;

namespace DayZServerMonitorCore
{
    public class ServerInfo
    {
        public ServerInfo(string address, string name, int numPlayers, int maxPlayers)
        {
            this.Address = address;
            this.Name = name;
            this.NumPlayers = numPlayers;
            this.MaxPlayers = maxPlayers;
        }

        public string Address { get; }
        public string Name { get; }
        public int NumPlayers { get; }
        public int MaxPlayers { get; }

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

            return new ServerInfo(
                string.Format("{0}:{1}", host, port), name, numPlayers, maxPlayers);
        }
    }
}
