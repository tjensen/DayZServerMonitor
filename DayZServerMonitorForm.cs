using DayZServerMonitorCore;
using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZServerMonitor
{
    public partial class DayZServerMonitorForm : Form
    {
        private readonly Clock clock = new Clock();
        private readonly ClientFactory clientFactory = new ClientFactory();
        private readonly ArrayList serverItems = new ArrayList();
        private readonly Monitor monitor;
        private ProfileWatcher watcher = null;

        public DayZServerMonitorForm()
        {
            InitializeComponent();
            serverItems.Add(new ServerSelectionItem(new LatestServerSource("Stable", ProfileParser.GetDayZFolder(), ProfileParser.GetProfileFilename())));
            serverItems.Add(new ServerSelectionItem(new LatestServerSource("Experimental", ProfileParser.GetExperimentalDayZFolder(), ProfileParser.GetProfileFilename())));
            SelectionCombo.DataSource = serverItems;
            SelectionCombo.DisplayMember = "DisplayName";
            SelectionCombo.ValueMember = "Value";
            SelectionCombo.SelectedValueChanged += new EventHandler(ServerSelectionChanged);

            monitor = new Monitor(clock, clientFactory);
        }

        private void ServerSelectionChanged(object sender, EventArgs e)
        {
            ServerSelectionItem item = (ServerSelectionItem)SelectionCombo.SelectedValue;
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
                Invoke(new MethodInvoker(delegate { item = (ServerSelectionItem)SelectionCombo.SelectedValue; }));
            }
            else {
                item = (ServerSelectionItem)SelectionCombo.SelectedValue;
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

        private async Task PollAsync()
        {
            ServerInfo info = await this.monitor.Poll(await GetSelectedServer());
            if (info != null)
            {
                UpdateValues(info.Address, info.Name, info.NumPlayers, info.MaxPlayers);
            }
        }

        private void Poll()
        {
            _ = Task.Run(async delegate { await PollAsync(); });
        }
    }
}
