using DayZServerMonitorCore;
using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace DayZServerMonitor
{
    public partial class DayZServerMonitorForm : Form
    {
        private readonly Label miniLabel = new Label();
        private readonly Settings settings = new Settings();
        private readonly ContextMenu contextMenu = new ContextMenu();
        private readonly DynamicIcons dynamicIcons = new DynamicIcons();
        private readonly Clock clock = new Clock();
        private readonly ClientFactory clientFactory = new ClientFactory();
        private readonly MenuItem removeSelectedServer;
        private readonly LogViewer logViewer;
        private readonly Logger logger;
        private readonly Monitor monitor;
        private readonly ServerSelectionList serverList;
        private ProfileWatcher watcher = null;
        private int lastIconUpdatePlayers = -1;
        private int lastIconUpdateMaxPlayers = -1;
        private Size normalMinimumSize;
        private Size normalMaximumSize;
        private bool draggingMiniWindow = false;
        private int dragStartX;
        private int dragStartY;

        public DayZServerMonitorForm()
        {
            InitializeComponent();

            contextMenu.MenuItems.Add("Add &Server...", AddServer_Click);
            contextMenu.MenuItems.Add("Add &Profile Location...", AddProfile_Click);
            removeSelectedServer = contextMenu.MenuItems.Add(
                "&Remove Selected Server", RemoveServer_Click);
            contextMenu.MenuItems.Add("-");
            contextMenu.MenuItems.Add("View &Logs", ViewLogs_Click);
            contextMenu.MenuItems.Add("-");
            contextMenu.MenuItems.Add("&Mini Window", MiniWindow_Click);
            contextMenu.MenuItems.Add("-");
            contextMenu.MenuItems.Add("S&ettings...", Settings_Click);

            settings.SettingChanged += Settings_SettingChanged;

            logViewer = new LogViewer(settings);
            logger = new Logger(settings, clock, StatusWriter, logViewer.Add);
            monitor = new Monitor(clock, clientFactory, logger);

            LoadSettings();

            SelectionCombo.DisplayMember = "DisplayName";
            SelectionCombo.ValueMember = "Value";
            serverList = new ServerSelectionList(SelectionCombo, logger);
            RestoreSavedServers();
            SelectionCombo.SelectedValueChanged += new EventHandler(ServerSelectionChanged);

            this.SuspendLayout();
            this.miniLabel.Hide();
            this.miniLabel.Dock = DockStyle.None;
            this.miniLabel.Location = new Point(-32, -32);
            this.miniLabel.Size = new Size(128, 128);
            this.miniLabel.BackColor = Color.Black;
            this.miniLabel.ForeColor = Color.White;
            this.miniLabel.Font = new Font(FontFamily.GenericSansSerif, 24, FontStyle.Bold);
            this.miniLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.miniLabel.Padding = new Padding(0);
            this.miniLabel.Margin = new Padding(0);
            this.miniLabel.DoubleClick += MiniLabel_DoubleClick;
            this.miniLabel.MouseDown += MiniLabel_MouseDown;
            this.miniLabel.MouseUp += MiniLabel_MouseUp;
            this.miniLabel.MouseMove += MiniLabel_MouseMove;
            this.Controls.Add(this.miniLabel);
            this.ResumeLayout();
        }

        private void StatusWriter(string text)
        {
            MonitorStatus.Text = text;
        }

        private void ServerSelectionChanged(object sender, EventArgs e)
        {
            removeSelectedServer.Enabled = serverList.IndexRemovable(
                SelectionCombo.SelectedIndex);

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
            return await item.GetSource().GetServer(logger, clock);
        }

        private void UpdateSystemTrayIcon(Icon icon, string players)
        {
            systemTrayIcon.Icon = icon;
            systemTrayIcon.Text = $"{Text}\nPlayers: {players}";
        }

        private bool CrossedThreshold(int players)
        {
            if (players < 0)
            {
                return false;
            }

            if (settings.NotifyOnPlayerCount == Settings.NotifyOnPlayerCountValues.WHEN_ABOVE)
            {
                return players > settings.PlayerCountThreshold;
            }
            else if (settings.NotifyOnPlayerCount == Settings.NotifyOnPlayerCountValues.WHEN_BELOW)
            {
                return players < settings.PlayerCountThreshold;
            }

            return false;
        }

        private bool ShouldNotify(int players)
        {
            if (players < 0)
            {
                return false;
            }

            if (settings.NotifyOnPlayerCount == Settings.NotifyOnPlayerCountValues.WHEN_ABOVE)
            {
                return players > settings.PlayerCountThreshold
                    && lastIconUpdatePlayers <= settings.PlayerCountThreshold;
            }
            else if (settings.NotifyOnPlayerCount == Settings.NotifyOnPlayerCountValues.WHEN_BELOW)
            {
                return players < settings.PlayerCountThreshold
                    && lastIconUpdatePlayers >= settings.PlayerCountThreshold;
            }

            return false;
        }

        private void UpdateSystemTrayIcon(int players, int maxPlayers)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { UpdateSystemTrayIcon(players, maxPlayers); }));
            }
            else
            {
                Color foreground = settings.TrayIconForeground;
                Color background = settings.TrayIconBackground;
                if (ShouldNotify(players))
                {
                    logger.Debug("Playing beep sound because player count crossed threshold");
                    SystemSounds.Beep.Play();
                }

                if (CrossedThreshold(players))
                {
                    foreground = settings.TrayIconAlertForeground;
                    background = settings.TrayIconAlertBackground;
                }

                lastIconUpdatePlayers = players;
                lastIconUpdateMaxPlayers = maxPlayers;

                this.miniLabel.ForeColor = foreground;
                this.miniLabel.BackColor = background;

                if (players >= 0)
                {
                    UpdateSystemTrayIcon(
                        dynamicIcons.GetIconForNumber((uint)players, foreground, background),
                        $"{players}/{maxPlayers}");
                    this.miniLabel.Text = players.ToString();
                }
                else
                {
                    UpdateSystemTrayIcon(
                        dynamicIcons.GetIconForUnknown(foreground, background), "?");
                    this.miniLabel.Text = "X";
                }
            }
        }

        private void UpdateSystemTrayIcon()
        {
            UpdateSystemTrayIcon(lastIconUpdatePlayers, lastIconUpdateMaxPlayers);
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
            UpdateSystemTrayIcon(players, maxPlayers);
        }

        internal void UpdateValues(string server)
        {
            UpdateValues(server, "", "?", "?", Color.Gray);
            UpdateSystemTrayIcon(-1, -1);
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

        private string SettingsFilename()
        {
            return Path.Combine(ApplicationDataFolder(), "settings.xml");
        }

        private void SaveSettings()
        {
            try
            {
                Directory.CreateDirectory(ApplicationDataFolder());
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                using TextWriter writer = new StreamWriter(SettingsFilename());
                serializer.Serialize(writer, settings);
            }
            catch (Exception error)
            {
                logger.Error("Failed to save settings", error);
            }
        }

        private void LoadSettings()
        {
            try
            {
                if (!File.Exists(SettingsFilename()))
                {
                    return;
                }

                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                using FileStream fs = new FileStream(SettingsFilename(), FileMode.Open);
                settings.Apply((Settings)serializer.Deserialize(fs));
            }
            catch (Exception error)
            {
                logger.Error("Failed to load settings", error);
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
            logger.Debug("Polling");
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
                logger.Debug($"Received {info}");
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
            using AddServerDialog dialog = new AddServerDialog
            {
                TopMost = settings.AlwaysOnTop
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    logger.Debug($"Adding server: {dialog.IPAddress}:{dialog.Port}");
                    ServerSelectionItem item = serverList.SaveServer(new Server(dialog.IPAddress, dialog.Port));
                    SelectionCombo.SelectedItem = item;
                }
                catch (Exception error)
                {
                    logger.Error("Failed to add server", error);
                }
            }
        }

        private void AddProfile_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "DayZ Profiles|*_settings.DayZProfile|All files|*.*"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                logger.Debug($"Adding custom profile location: {dialog.FileName}");
                ServerSelectionItem item = serverList.SaveProfile(dialog.FileName);
                SelectionCombo.SelectedItem = item;
            }
        }

        private void RemoveServer_Click(object sender, EventArgs args)
        {
            serverList.RemoveIndex(SelectionCombo.SelectedIndex);
            PersistSavedServers();
            SelectionCombo.SelectedIndex = 0;
        }

        private void ViewLogs_Click(object sender, EventArgs e)
        {
            logViewer.WindowState = FormWindowState.Normal;
            logViewer.Activate();
            logViewer.Show();
        }

        private void DayZServerMonitorForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized &&
                settings.HideTaskBarIcon == Settings.HideTaskBarIconValues.WHEN_MINIMIZED)
            {
                ShowInTaskbar = false;
            }
            else if (WindowState == FormWindowState.Normal &&
                settings.HideTaskBarIcon == Settings.HideTaskBarIconValues.WHEN_MINIMIZED)
            {
                ShowInTaskbar = true;
            }
        }

        private void Settings_SettingChanged(object sender, SettingChangedArgs args)
        {
            if (args.SettingName == nameof(settings.HideTaskBarIcon))
            {
                if (settings.HideTaskBarIcon == Settings.HideTaskBarIconValues.NEVER)
                {
                    ShowInTaskbar = true;
                }
                else if (settings.HideTaskBarIcon == Settings.HideTaskBarIconValues.WHEN_MINIMIZED)
                {
                    ShowInTaskbar = WindowState != FormWindowState.Minimized;
                }
                else if (settings.HideTaskBarIcon == Settings.HideTaskBarIconValues.ALWAYS)
                {
                    ShowInTaskbar = false;
                }
            }
            else if (args.SettingName == nameof(settings.AlwaysOnTop))
            {
                TopMost = settings.AlwaysOnTop;
            }
            else if (args.SettingName == nameof(settings.TrayIconBackground) || args.SettingName == nameof(settings.TrayIconForeground))
            {
                dynamicIcons.Reset();
                UpdateSystemTrayIcon();
            }
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            using SettingsDialog dialog = new SettingsDialog
            {
                TopMost = settings.AlwaysOnTop
            };
            dialog.ShowDialog(settings);
            SaveSettings();
        }

        private void MiniWindow_Click(object sender, EventArgs e)
        {
            this.normalMinimumSize = this.MinimumSize;
            this.normalMaximumSize = this.MaximumSize;

            this.SuspendLayout();
            this.MaximumSize = new Size(64, 64);
            this.MinimumSize = new Size(64, 64);
            this.FormPanel.Hide();
            this.FormBorderStyle = FormBorderStyle.None;
            this.miniLabel.Show();
            this.ResumeLayout();
        }

        private void MiniLabel_DoubleClick(object sender, EventArgs e)
        {
            this.SuspendLayout();
            this.MaximumSize = this.normalMaximumSize;
            this.MinimumSize = this.normalMinimumSize;
            this.miniLabel.Hide();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.FormPanel.Show();
            this.ResumeLayout();
        }

        private void MiniLabel_MouseDown(object sender, MouseEventArgs e)
        {
            this.dragStartX = e.X;
            this.dragStartY = e.Y;
            this.draggingMiniWindow = true;
        }

        private void MiniLabel_MouseUp(object sender, MouseEventArgs e)
        {
            this.draggingMiniWindow = false;
        }

        private void MiniLabel_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.draggingMiniWindow)
            {
                this.Location = new Point(
                    this.Location.X + (e.X - this.dragStartX),
                    this.Location.Y + (e.Y - this.dragStartY));
            }
        }
    }
}
