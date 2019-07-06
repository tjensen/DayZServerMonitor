
using DayZServerMonitorCore;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZServerMonitor
{
    public partial class DayZServerMonitorForm : Form
    {
        private Monitor monitor;

        public DayZServerMonitorForm()
        {
            InitializeComponent();
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

        internal void Initialize(Monitor monitor)
        {
            this.monitor = monitor;
            ProfileWatcher watcher = new ProfileWatcher(
                ProfileParser.GetDayZFolder(), ProfileParser.GetProfileFilename(), this, Poll);
            System.Timers.Timer timer = monitor.CreateTimer(this, Poll);
            Poll();
        }

        private async Task PollAsync()
        {
            await this.monitor.Poll(this);
        }

        private void Poll()
        {
            _ = Task.Run(async delegate { await PollAsync(); });
        }
    }
}
