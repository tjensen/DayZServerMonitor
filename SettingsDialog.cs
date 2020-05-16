using DayZServerMonitorCore;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DayZServerMonitor
{
    public class SettingsDialog : Form
    {
        private readonly ComboBox hideTaskBarIcon = new ComboBox();
        private readonly CheckBox alwaysOnTop = new CheckBox();
        private readonly NumericUpDown maxLogViewerEntries = new NumericUpDown();
        private readonly CheckBox enableLogFile = new CheckBox();
        private readonly TextBox logFilename = new TextBox();
        private readonly NumericUpDown playerCountThreshold = new NumericUpDown();
        private readonly ComboBox notifyOnPlayerCount = new ComboBox();
        private readonly Button trayIconBackground = new Button();
        private readonly Button trayIconForeground = new Button();

        public SettingsDialog()
        {
            InitializeComponent();
        }

        public void ShowDialog(Settings settings)
        {
            hideTaskBarIcon.SelectedItem = settings.HideTaskBarIcon;
            alwaysOnTop.Checked = settings.AlwaysOnTop;
            maxLogViewerEntries.Value = settings.MaxLogViewerEntries;
            if (settings.LogPathname == null)
            {
                enableLogFile.Checked = false;
                logFilename.Text = "";
            }
            else
            {
                enableLogFile.Checked = true;
                logFilename.Text = settings.LogPathname;
            }
            playerCountThreshold.Value = settings.PlayerCountThreshold;
            notifyOnPlayerCount.SelectedItem = settings.NotifyOnPlayerCount;
            trayIconBackground.BackColor = settings.TrayIconBackground;
            trayIconForeground.BackColor = settings.TrayIconForeground;

            if (ShowDialog() == DialogResult.OK)
            {
                settings.HideTaskBarIcon =
                    (Settings.HideTaskBarIconValues)hideTaskBarIcon.SelectedItem;
                settings.AlwaysOnTop = alwaysOnTop.Checked;
                settings.MaxLogViewerEntries = (int)maxLogViewerEntries.Value;
                if (enableLogFile.Checked && logFilename.Text != "")
                {
                    settings.LogPathname = logFilename.Text;
                }
                else
                {
                    settings.LogPathname = null;
                }
                settings.PlayerCountThreshold = (int)playerCountThreshold.Value;
                settings.NotifyOnPlayerCount =
                    (Settings.NotifyOnPlayerCountValues)notifyOnPlayerCount.SelectedItem;
                settings.TrayIconBackground = trayIconBackground.BackColor;
                settings.TrayIconForeground = trayIconForeground.BackColor;
            }
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            Text = "Settings";
            Icon = Properties.Resources.DayZServerMonitorIcon;
            ShowInTaskbar = false;
            Width = 600;
            MinimumSize = new Size(400, 200);

            TableLayoutPanel mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(4)
            };
            mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
            Controls.Add(mainPanel);

            TableLayoutPanel settingsPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                AutoScroll = true
            };
            mainPanel.Controls.Add(settingsPanel);

            Label hideTaskBarIconLabel = new Label()
            {
                Dock = DockStyle.Fill,
                Text = "Hide Task Bar Icon:",
                TextAlign = ContentAlignment.MiddleLeft
            };
            settingsPanel.Controls.Add(hideTaskBarIconLabel);

            hideTaskBarIcon.Dock = DockStyle.Fill;
            hideTaskBarIcon.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (var value in Enum.GetValues(typeof(Settings.HideTaskBarIconValues)))
            {
                hideTaskBarIcon.Items.Add(value);
            }
            settingsPanel.Controls.Add(hideTaskBarIcon);

            Label alwaysOnTopLabel = new Label()
            {
                Dock = DockStyle.Fill,
                Text = "Always On Top:",
                TextAlign = ContentAlignment.MiddleLeft
            };
            settingsPanel.Controls.Add(alwaysOnTopLabel);

            alwaysOnTop.Dock = DockStyle.Left;
            settingsPanel.Controls.Add(alwaysOnTop);

            Label maxLogViewerEntriesLabel = new Label()
            {
                Dock = DockStyle.Fill,
                Text = "Maximum Log Viewer Entries:",
                TextAlign = ContentAlignment.MiddleLeft
            };
            settingsPanel.Controls.Add(maxLogViewerEntriesLabel);

            maxLogViewerEntries.Dock = DockStyle.Left;
            maxLogViewerEntries.Minimum = 1;
            maxLogViewerEntries.Maximum = int.MaxValue;
            settingsPanel.Controls.Add(maxLogViewerEntries);

            Label enableLogFileLabel = new Label()
            {
                Dock = DockStyle.Fill,
                Text = "Enable Log File:",
                TextAlign = ContentAlignment.MiddleLeft
            };
            settingsPanel.Controls.Add(enableLogFileLabel);

            enableLogFile.Dock = DockStyle.Left;
            enableLogFile.CheckedChanged += EnableLogFile_CheckedChanged;
            settingsPanel.Controls.Add(enableLogFile);

            Label logFilenameLabel = new Label()
            {
                Dock = DockStyle.Fill,
                Text = "Log File:",
                TextAlign = ContentAlignment.MiddleLeft
            };
            settingsPanel.Controls.Add(logFilenameLabel);

            logFilename.Dock = DockStyle.Fill;
            logFilename.ReadOnly = true;
            logFilename.Click += LogFilename_Click;
            settingsPanel.Controls.Add(logFilename);

            Label notifyOnPlayerCountLabel = new Label()
            {
                Dock = DockStyle.Fill,
                Text = "Notify On Player Count:",
                TextAlign = ContentAlignment.MiddleLeft
            };
            settingsPanel.Controls.Add(notifyOnPlayerCountLabel);

            notifyOnPlayerCount.Dock = DockStyle.Fill;
            notifyOnPlayerCount.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (var value in Enum.GetValues(typeof(Settings.NotifyOnPlayerCountValues)))
            {
                notifyOnPlayerCount.Items.Add(value);
            }
            settingsPanel.Controls.Add(notifyOnPlayerCount);

            Label playerCountThresholdLabel = new Label()
            {
                Dock = DockStyle.Fill,
                Text = "Player Count Threshold:",
                TextAlign = ContentAlignment.MiddleLeft
            };
            settingsPanel.Controls.Add(playerCountThresholdLabel);

            playerCountThreshold.Dock = DockStyle.Left;
            playerCountThreshold.Minimum = 0;
            playerCountThreshold.Maximum = int.MaxValue;
            settingsPanel.Controls.Add(playerCountThreshold);

            Label trayIconBackgroundLabel = new Label()
            {
                Dock = DockStyle.Fill,
                Text = "Tray Icon Background:",
                TextAlign = ContentAlignment.MiddleLeft
            };
            settingsPanel.Controls.Add(trayIconBackgroundLabel);

            trayIconBackground.Dock = DockStyle.Left;
            trayIconBackground.Click += ColorButton_Click;
            settingsPanel.Controls.Add(trayIconBackground);

            Label trayIconForegroundLabel = new Label()
            {
                Dock = DockStyle.Fill,
                Text = "Tray Icon Foreground:",
                TextAlign = ContentAlignment.MiddleLeft
            };
            settingsPanel.Controls.Add(trayIconForegroundLabel);

            trayIconForeground.Dock = DockStyle.Left;
            trayIconForeground.Click += ColorButton_Click;
            settingsPanel.Controls.Add(trayIconForeground);

            TableLayoutPanel buttonPanel = new TableLayoutPanel
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                RowCount = 1
            };
            mainPanel.Controls.Add(buttonPanel);

            Button okButton = new Button()
            {
                DialogResult = DialogResult.OK,
                Text = "OK"
            };
            buttonPanel.Controls.Add(okButton);

            Button cancelButton = new Button()
            {
                DialogResult = DialogResult.Cancel,
                Text = "Cancel"
            };
            buttonPanel.Controls.Add(cancelButton);

            AcceptButton = okButton;
            CancelButton = cancelButton;

            ResumeLayout();
        }

        private bool SelectLogFile()
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "All Files|*.*";
                dialog.Title = "Log Filename";
                dialog.OverwritePrompt = false;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    logFilename.Text = dialog.FileName;
                    enableLogFile.Checked = true;
                    return true;
                }
            }
            return false;
        }

        private void LogFilename_Click(object sender, EventArgs args)
        {
            SelectLogFile();
        }

        private void EnableLogFile_CheckedChanged(object sender, EventArgs args)
        {
            if (enableLogFile.Checked)
            {
                if (!SelectLogFile() && logFilename.Text == "")
                {
                    enableLogFile.Checked = false;
                }
            }
        }

        private void ColorButton_Click(object sender, EventArgs args)
        {
            Button colorButton = (Button) sender;
            using ColorDialog dialog = new ColorDialog
            {
                Color = colorButton.BackColor
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                colorButton.BackColor = dialog.Color;
            }
        }
    }
}
