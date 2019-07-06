namespace DayZServerMonitorCore
{
    public class Server
    {
        private readonly static int DEFAULT_PORT = 2301;
        private readonly static int STATS_PORT_OFFSET = 24714;

        public Server(string address)
        {
            string[] parts = address.Split(":".ToCharArray(), 2);
            Host = parts[0];
            Port = parts.Length == 2 ? int.Parse(parts[1]) : DEFAULT_PORT;
            if (Port > 65535)
            {
                Port >>= 16;
            }
        }

        public Server(string host, int port)
        {
            Host = host;
            Port = port;
        }

        public string Host { get; }
        public int Port { get; }

        public int StatsPort
        {
            get => Port + STATS_PORT_OFFSET;
        }

        public string Address
        {
            get => string.Format("{0}:{1}", Host, Port);
        }

        public bool Equals(Server other)
        {
            return other != null && Host == other.Host && Port == other.Port;
        }
    }
}
