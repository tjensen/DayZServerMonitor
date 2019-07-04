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
    }
}
