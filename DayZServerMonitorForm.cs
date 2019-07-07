using DayZServerMonitorCore;
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
        private readonly Monitor monitor;

        public DayZServerMonitorForm()
        {
            InitializeComponent();
            monitor = new Monitor(
                Path.Combine(
                    ProfileParser.GetDayZFolder(), ProfileParser.GetProfileFilename()),
                clock, clientFactory);
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
            ProfileWatcher watcher = new ProfileWatcher(
                ProfileParser.GetDayZFolder(), ProfileParser.GetProfileFilename(), this, Poll);
            clock.CreateIntervalTimer(Poll, Monitor.POLLING_INTERVAL, this);
            Poll();
        }

        private async Task PollAsync()
        {
            ServerInfo info = await this.monitor.Poll();
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
