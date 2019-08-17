namespace DayZServerMonitorCore
{
    public class ServerSelectionItem
    {
        private readonly IServerSource source;

        public ServerSelectionItem(IServerSource source)
        {
            this.source = source;
        }

        public string DisplayName => source.GetDisplayName();

        public ServerSelectionItem Value => this;

        public IServerSource GetSource()
        {
            return source;
        }
    }
}
