using DayZServerMonitorCore;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZServerMonitor
{
    public partial class DayZServerMonitorForm : Form
    {
        private static readonly int SAVED_SERVER_INDEX = 2;

        private readonly Clock clock = new Clock();
        private readonly ClientFactory clientFactory = new ClientFactory();
        private readonly Logger logger;
        private readonly Monitor monitor;
        private ProfileWatcher watcher = null;

        public DayZServerMonitorForm()
        {
            InitializeComponent();
            logger = new Logger(clock, StatusWriter);
            monitor = new Monitor(clock, clientFactory, logger);

            SelectionCombo.Items.Add(new ServerSelectionItem(new LatestServerSource("Stable", ProfileParser.GetDayZFolder(), ProfileParser.GetProfileFilename())));
            SelectionCombo.Items.Add(new ServerSelectionItem(new LatestServerSource("Experimental", ProfileParser.GetExperimentalDayZFolder(), ProfileParser.GetProfileFilename())));
            SelectionCombo.DisplayMember = "DisplayName";
            SelectionCombo.ValueMember = "Value";
            SelectionCombo.SelectedIndex = 0;
            SelectionCombo.SelectedValueChanged += new EventHandler(ServerSelectionChanged);
        }

        private void StatusWriter(string text)
        {
            MonitorStatus.Text = text;
        }

        private void ServerSelectionChanged(object sender, EventArgs e)
        {
            ServerSelectionItem item = (ServerSelectionItem)SelectionCombo.SelectedItem;
            UpdateServerSource(item.GetSource());
        }

        private void UpdateServerSource(IServerSource source)
        {
            if (watcher != null)
            {
                watcher.Dispose();
            }
            watcher = source.CreateWatcher(Poll, this);
            Poll();
        }

        private async Task<Server> GetSelectedServer()
        {
            ServerSelectionItem item = null;
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { item = (ServerSelectionItem)SelectionCombo.SelectedItem; }));
            }
            else
            {
                item = (ServerSelectionItem)SelectionCombo.SelectedItem;
            }
            return await item.GetSource().GetServer();
        }

        internal void UpdateValues(string server, string name, string players, string maxPlayers, Color playersColor)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { UpdateValues(server, name, players, maxPlayers, playersColor); }));
            }
            else
            {
                ServerValue.Text = server;
                NameValue.Text = name;
                PlayersValue.Text = players;
                PlayersValue.ForeColor = playersColor;
                PlayersValue.BackColor = SystemColors.Control; // Force readonly control to update foreground color
                MaxPlayersValue.Text = maxPlayers;
            }
        }

        internal void UpdateValues(string server, string name, int players, int maxPlayers)
        {
            Color playersColor;
            if (players == 0)
            {
                playersColor = Color.Blue;
            }
            else if (players < maxPlayers)
            {
                playersColor = Color.Green;
            }
            else
            {
                playersColor = Color.Red;
            }
            UpdateValues(server, name, players.ToString(), maxPlayers.ToString(), playersColor);
        }

        internal void UpdateValues(string server)
        {
            UpdateValues(server, "", "?", "?", Color.Gray);
        }

        internal void Initialize()
        {
            clock.CreateIntervalTimer(Poll, Monitor.POLLING_INTERVAL, this);
            Poll();
        }

        private void AddServer(Server server, string name)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { AddServer(server, name); }));
            }
            else
            {
                SelectionCombo.Items.Insert(
                        SAVED_SERVER_INDEX,
                        new ServerSelectionItem(new SavedServerSource(server, name)));
            }
        }

        private async Task SaveServer(Server server, string name)
        {
            for (int index = SAVED_SERVER_INDEX; index < SelectionCombo.Items.Count; index++)
            {
                ServerSelectionItem item = (ServerSelectionItem)SelectionCombo.Items[index];
                SavedServerSource source = (SavedServerSource)item.GetSource();
                if (server.Equals(await source.GetServer()))
                {
                    source.SetServerName(name);
                    return;
                }
            }
            AddServer(server, name);
        }

        private async Task PollAsync()
        {
            Server server = await GetSelectedServer();
            if (ServerValue.Text != server.Address)
            {
                UpdateValues(server.Address);
            }
            ServerInfo info = await this.monitor.Poll(server);
            if (info != null)
            {
                await SaveServer(new Server(info.Address), info.Name);
                UpdateValues(info.Address, info.Name, info.NumPlayers, info.MaxPlayers);
            }
        }

        private void Poll()
        {
            _ = Task.Run(async delegate { await PollAsync(); });
        }
    }
}
