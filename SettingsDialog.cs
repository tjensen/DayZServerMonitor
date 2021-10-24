using DayZServerMonitorCore;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DayZServerMonitor
{
    public class SettingsDialog : Form
    {
        private readonly TableLayoutPanel mainPanel = new TableLayoutPanel();
        private readonly CheckBox checkForUpdates = new CheckBox();
        private readonly ComboBox hideTaskBarIcon = new ComboBox();
        private readonly CheckBox alwaysOnTop = new CheckBox();
        private readonly NumericUpDown maxLogViewerEntries = new NumericUpDown();
        private readonly CheckBox enableLogFile = new CheckBox();
        private readonly TextBox logFilename = new TextBox();
        private readonly NumericUpDown playerCountThreshold = new NumericUpDown();
        private readonly ComboBox notifyOnPlayerCount = new ComboBox();
        private readonly CheckBox[] enableStatusFile;
        private readonly TextBox[] statusFilename;
        private readonly TextBox[] statusFileContent;
        private readonly Button trayIconBackground = new Button();
        private readonly Button trayIconForeground = new Button();
        private readonly Button trayIconAlertBackground = new Button();
        private readonly Button trayIconAlertForeground = new Button();
        private bool honorCheckedChangedEvents = false;

        public SettingsDialog()
        {
            enableStatusFile = new CheckBox[Settings.NUM_STATUS_FILES];
            statusFilename = new TextBox[Settings.NUM_STATUS_FILES];
            statusFileContent = new TextBox[Settings.NUM_STATUS_FILES];
            for (int i = 0; i < Settings.NUM_STATUS_FILES; i++)
            {
                enableStatusFile[i] = new CheckBox();
                statusFilename[i] = new TextBox();
                statusFileContent[i] = new TextBox();
            }
            InitializeComponent();
        }

        public new DialogResult ShowDialog()
        {
            honorCheckedChangedEvents = true;

            try
            {
                return base.ShowDialog();
            }
            finally
            {
                honorCheckedChangedEvents = false;
            }
        }

        public void ShowDialog(Settings settings)
        {
            checkForUpdates.Checked = settings.CheckForUpdates;
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
            for (int i = 0; i < Settings.NUM_STATUS_FILES; i++)
            {
                if (settings.StatusFile[i].Pathname == null)
                {
                    enableStatusFile[i].Checked = false;
                    statusFilename[i].Text = "";
                }
                else
                {
                    enableStatusFile[i].Checked = true;
                    statusFilename[i].Text = settings.StatusFile[i].Pathname;
                }
                statusFileContent[i].Text = settings.StatusFile[i].Content;
            }
            trayIconBackground.BackColor = settings.TrayIconBackground;
            trayIconForeground.BackColor = settings.TrayIconForeground;
            trayIconAlertBackground.BackColor = settings.TrayIconAlertBackground;
            trayIconAlertForeground.BackColor = settings.TrayIconAlertForeground;

            if (ShowDialog() == DialogResult.OK)
            {
                settings.CheckForUpdates = checkForUpdates.Checked;
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
                StatusFileSetting[] newStatusFiles = new StatusFileSetting[Settings.NUM_STATUS_FILES];
                for (int i = 0; i < Settings.NUM_STATUS_FILES; i++)
                {
                    newStatusFiles[i] = new StatusFileSetting
                    {
                        Pathname = enableStatusFile[i].Checked && statusFilename[i].Text != "" ? statusFilename[i].Text : null,
                        Content = statusFileContent[i].Text
                    };
                }
                settings.StatusFile = newStatusFiles;
                settings.TrayIconBackground = trayIconBackground.BackColor;
                settings.TrayIconForeground = trayIconForeground.BackColor;
                settings.TrayIconAlertBackground = trayIconAlertBackground.BackColor;
                settings.TrayIconAlertForeground = trayIconAlertForeground.BackColor;
            }
        }

        private void InitializeSetting(Panel panel, Control control, string labelText)
        {
            Label label = new Label()
            {
                Dock = DockStyle.Fill,
                Text = labelText + ":",
                TextAlign = ContentAlignment.MiddleLeft
            };
            panel.Controls.Add(label);

            panel.Controls.Add(control);
        }

        private void InitializeColorSetting(Panel panel, Button colorButton, string labelText)
        {
            colorButton.Dock = DockStyle.Left;
            colorButton.Click += ColorButton_Click;
            InitializeSetting(panel, colorButton, labelText);
        }

        private void InitializeStatusFileSetting(Panel panel, int index)
        {
            enableStatusFile[index].Dock = DockStyle.Left;
            enableStatusFile[index].CheckedChanged += (object source, EventArgs args) => EnableStatusFile_CheckedChanged(index);
            InitializeSetting(panel, enableStatusFile[index], $"Enable Status File {index}");

            statusFilename[index].Dock = DockStyle.Fill;
            statusFilename[index].ReadOnly = true;
            statusFilename[index].Click += (object source, EventArgs args) => StatusFilename_Click(index);
            InitializeSetting(panel, statusFilename[index], $"Status Filename {index}");

            statusFileContent[index].Dock = DockStyle.Fill;
            statusFileContent[index].Height = statusFileContent[index].Font.Height * 5;
            statusFileContent[index].ScrollBars = ScrollBars.Both;
            statusFileContent[index].Multiline = true;
            statusFileContent[index].AcceptsReturn = true;
            InitializeSetting(panel, statusFileContent[index], $"Status File Content {index}");
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            Text = "Settings";
            Icon = Properties.Resources.DayZServerMonitorIcon;
            ShowInTaskbar = false;
            Width = 600;
            MinimumSize = new Size(400, 200);
            SizeChanged += SettingsDialog_SizeChanged;

            mainPanel.Dock = DockStyle.Fill;
            mainPanel.ColumnCount = 1;
            mainPanel.RowCount = 2;
            mainPanel.Padding = new Padding(4);
            mainPanel.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    ClientSize.Height - (mainPanel.Padding.Top + mainPanel.Padding.Bottom) - 35));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
            Controls.Add(mainPanel);

            TableLayoutPanel settingsPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                AutoScroll = true,
            };
            settingsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
            settingsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            mainPanel.Controls.Add(settingsPanel);

            checkForUpdates.Dock = DockStyle.Left;
            InitializeSetting(settingsPanel, checkForUpdates, "Check for updates on startup");

            hideTaskBarIcon.Dock = DockStyle.Fill;
            hideTaskBarIcon.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (var value in Enum.GetValues(typeof(Settings.HideTaskBarIconValues)))
            {
                hideTaskBarIcon.Items.Add(value);
            }
            InitializeSetting(settingsPanel, hideTaskBarIcon, "Hide Task Bar Icon");

            alwaysOnTop.Dock = DockStyle.Left;
            InitializeSetting(settingsPanel, alwaysOnTop, "Always On Top");

            maxLogViewerEntries.Dock = DockStyle.Left;
            maxLogViewerEntries.Minimum = 1;
            maxLogViewerEntries.Maximum = int.MaxValue;
            InitializeSetting(settingsPanel, maxLogViewerEntries, "Maximum Log Viewer Entries");

            enableLogFile.Dock = DockStyle.Left;
            enableLogFile.CheckedChanged += EnableLogFile_CheckedChanged;
            InitializeSetting(settingsPanel, enableLogFile, "Enable Log File");

            logFilename.Dock = DockStyle.Fill;
            logFilename.ReadOnly = true;
            logFilename.Click += LogFilename_Click;
            InitializeSetting(settingsPanel, logFilename, "Log File");

            notifyOnPlayerCount.Dock = DockStyle.Fill;
            notifyOnPlayerCount.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (var value in Enum.GetValues(typeof(Settings.NotifyOnPlayerCountValues)))
            {
                notifyOnPlayerCount.Items.Add(value);
            }
            InitializeSetting(settingsPanel, notifyOnPlayerCount, "Notify On Player Count");

            playerCountThreshold.Dock = DockStyle.Left;
            playerCountThreshold.Minimum = 0;
            playerCountThreshold.Maximum = int.MaxValue;
            InitializeSetting(settingsPanel, playerCountThreshold, "Player Count Threshold");

            for (int i = 0; i < Settings.NUM_STATUS_FILES; i++)
            {
                InitializeStatusFileSetting(settingsPanel, i);
            }

            InitializeColorSetting(settingsPanel, trayIconBackground, "Tray Icon Background");
            InitializeColorSetting(settingsPanel, trayIconForeground, "Tray Icon Foreground");

            InitializeColorSetting(
                settingsPanel, trayIconAlertBackground, "Tray Icon Alert Background");
            InitializeColorSetting(
                settingsPanel, trayIconAlertForeground, "Tray Icon Alert Foreground");

            // Add an extra empty row to fill any remaining space
            settingsPanel.Controls.Add(new Panel { Height = 0 });
            settingsPanel.Controls.Add(new Panel { Height = 0 });

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

        private void SettingsDialog_SizeChanged(object sender, EventArgs e)
        {
            mainPanel.RowStyles[0].Height = ClientSize.Height - (mainPanel.Padding.Top + mainPanel.Padding.Bottom) - 35;
        }

        private string SelectFile(string title, bool overwritePrompt)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "All Files|*.*";
                dialog.Title = title;
                dialog.OverwritePrompt = overwritePrompt;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.FileName;
                }
            }
            return null;
        }

        private bool SelectLogFile()
        {
            string filename = SelectFile("Log Filename", false);
            if (filename != null)
            {
                logFilename.Text = filename;
                enableLogFile.Checked = true;
                return true;
            }
            return false;
        }

        private void LogFilename_Click(object sender, EventArgs args)
        {
            SelectLogFile();
        }

        private void EnableLogFile_CheckedChanged(object sender, EventArgs args)
        {
            if (honorCheckedChangedEvents && enableLogFile.Checked)
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

        private bool SelectStatusFile(int index)
        {
            string filename = SelectFile($"Status Filename {index}", true);
            if (filename != null)
            {
                statusFilename[index].Text = filename;
                enableStatusFile[index].Checked = true;
                return true;
            }
            return false;
        }

        private void EnableStatusFile_CheckedChanged(int index)
        {
            if (honorCheckedChangedEvents && enableStatusFile[index].Checked)
            {
                if (!SelectStatusFile(index) && statusFilename[index].Text == "")
                {
                    enableStatusFile[index].Checked = false;
                }
            }
        }

        private void StatusFilename_Click(int index)
        {
            SelectStatusFile(index);
        }
    }
}
