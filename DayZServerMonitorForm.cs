using DayZServerMonitorCore;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZServerMonitor
{
    public partial class DayZServerMonitorForm : Form
    {
        private readonly ContextMenu contextMenu = new ContextMenu();
        private readonly DynamicIcons dynamicIcons = new DynamicIcons();
        private readonly Clock clock = new Clock();
        private readonly ClientFactory clientFactory = new ClientFactory();
        private readonly Logger logger;
        private readonly Monitor monitor;
        private readonly ServerSelectionList serverList;
        private ProfileWatcher watcher = null;

        public DayZServerMonitorForm()
        {
            InitializeComponent();
            components.Add(contextMenu);
            contextMenu.MenuItems.Add("Add Server...", AddServer_Click);
            contextMenu.MenuItems.Add("Add Profile Location...", AddProfile_Click);

            logger = new Logger(clock, StatusWriter);
            monitor = new Monitor(clock, clientFactory, logger);

            SelectionCombo.DisplayMember = "DisplayName";
            SelectionCombo.ValueMember = "Value";
            serverList = new ServerSelectionList(SelectionCombo, logger);
            RestoreSavedServers();
            SelectionCombo.SelectedValueChanged += new EventHandler(ServerSelectionChanged);
        }

        private void StatusWriter(string text)
        {
            MonitorStatus.Text = text;
        }

        private void ServerSelectionChanged(object sender, EventArgs e)
        {
            ServerSelectionItem item = serverList[SelectionCombo.SelectedIndex];
            if (item != null)
            {
                serverList.Promote(SelectionCombo.SelectedIndex);
                UpdateServerSource(item.GetSource());
            }
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
                Invoke(new MethodInvoker(delegate { item = serverList[SelectionCombo.SelectedIndex]; }));
            }
            else
            {
                item = serverList[SelectionCombo.SelectedIndex];
            }
            return await item.GetSource().GetServer(logger);
        }

        private void UpdateSystemTrayIcon(Icon icon, string players)
        {
            systemTrayIcon.Icon = icon;
            systemTrayIcon.Text = $"{Text}\nPlayers: {players}";
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
            UpdateSystemTrayIcon(dynamicIcons.GetIconForNumber((uint)players), $"{players}/{maxPlayers}");
        }

        internal void UpdateValues(string server)
        {
            UpdateValues(server, "", "?", "?", Color.Gray);
            UpdateSystemTrayIcon(dynamicIcons.GetIconForUnknown(), "?");
        }

        internal void Initialize()
        {
            clock.CreateIntervalTimer(Poll, Monitor.POLLING_INTERVAL, this);
            Poll();
        }

        private string ApplicationDataFolder()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "DayZServerMonitor");
        }

        private string ServersFilename()
        {
            return Path.Combine(ApplicationDataFolder(), "servers.xml");
        }

        private void PersistSavedServers()
        {
            try
            {
                Directory.CreateDirectory(ApplicationDataFolder());
                serverList.SaveToFilename(ServersFilename());
            }
            catch (Exception error)
            {
                Console.WriteLine($"Failed to save saved servers: {error}");
            }
        }

        private void RestoreSavedServers()
        {
            try
            {
                serverList.LoadFromFilename(ServersFilename());
            }
            catch (Exception error)
            {
                Console.WriteLine($"Failed to load saved servers: {error}");
            }
        }

        private void SaveServer(Server server, string name)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { SaveServer(server, name); }));
            }
            else
            {
                serverList.SaveServer(server, name);
                PersistSavedServers();
            }
        }

        private async Task PollAsync()
        {
            Server server = await GetSelectedServer();
            if (server == null)
            {
                UpdateValues("?");
                return;
            }

            if (ServerValue.Text != server.Address)
            {
                UpdateValues(server.Address);
            }
            ServerInfo info = await this.monitor.Poll(server);
            if (info != null)
            {
                SaveServer(new Server(info.Address), info.Name);
                UpdateValues(info.Address, info.Name, info.NumPlayers, info.MaxPlayers);
            }
        }

        private void Poll()
        {
            _ = Task.Run(async delegate { await PollAsync(); });
        }

        private void SystemTrayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WindowState = FormWindowState.Normal;
            Activate();
        }

        private void SelectionManage_Click(object sender, EventArgs e)
        {
            contextMenu.Show(SelectionManage, new Point(0, 0));
        }

        private void AddServer_Click(object sender, EventArgs e)
        {
            using (AddServerDialog dialog = new AddServerDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ServerSelectionItem item = serverList.SaveServer(new Server(dialog.IPAddress, dialog.Port));
                        SelectionCombo.SelectedItem = item;
                    }
                    catch (Exception error)
                    {
                        logger.Error("Failed to add server", error);
                    }
                }
            }
        }

        private void AddProfile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "DayZ Profiles|*_settings.DayZProfile|All files|*.*";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    ServerSelectionItem item = serverList.SaveProfile(dialog.FileName);
                    SelectionCombo.SelectedItem = item;
                }
            }
        }
    }
}
