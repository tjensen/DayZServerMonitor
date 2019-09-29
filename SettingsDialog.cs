using DayZServerMonitorCore;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DayZServerMonitor
{
    public class SettingsDialog : Form
    {
        private readonly ComboBox hideTaskBarIcon = new ComboBox();
        private readonly NumericUpDown maxLogViewerEntries = new NumericUpDown();
        private readonly CheckBox enableLogFile = new CheckBox();
        private readonly TextBox logFilename = new TextBox();

        public SettingsDialog()
        {
            InitializeComponent();
        }

        public void ShowDialog(Settings settings)
        {
            hideTaskBarIcon.SelectedItem = settings.HideTaskBarIcon;
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

            if (ShowDialog() == DialogResult.OK)
            {
                settings.HideTaskBarIcon = (Settings.HideTaskBarIconValues)hideTaskBarIcon.SelectedItem;
                settings.MaxLogViewerEntries = (int)maxLogViewerEntries.Value;
                if (enableLogFile.Checked && logFilename.Text != "")
                {
                    settings.LogPathname = logFilename.Text;
                }
                else
                {
                    settings.LogPathname = null;
                }
            }
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            Text = "Settings";
            Icon = Properties.Resources.DayZServerMonitorIcon;
            ShowInTaskbar = false;

            TableLayoutPanel panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                Padding = new Padding(8),
                AutoScroll = true
            };
            Controls.Add(panel);

            Label hideTaskBarIconLabel = new Label()
            {
                Dock = DockStyle.Fill,
                Text = "Hide Task Bar Icon:",
                TextAlign = ContentAlignment.MiddleLeft
            };
            panel.Controls.Add(hideTaskBarIconLabel);

            hideTaskBarIcon.Dock = DockStyle.Fill;
            hideTaskBarIcon.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (var value in Enum.GetValues(typeof(Settings.HideTaskBarIconValues)))
            {
                hideTaskBarIcon.Items.Add(value);
            }
            panel.Controls.Add(hideTaskBarIcon);

            Label maxLogViewerEntriesLabel = new Label()
            {
                Dock = DockStyle.Fill,
                Text = "Maximum Log Viewer Entries:",
                TextAlign = ContentAlignment.MiddleLeft
            };
            panel.Controls.Add(maxLogViewerEntriesLabel);

            maxLogViewerEntries.Dock = DockStyle.Left;
            maxLogViewerEntries.Minimum = 1;
            maxLogViewerEntries.Maximum = int.MaxValue;
            panel.Controls.Add(maxLogViewerEntries);

            Label enableLogFileLabel = new Label()
            {
                Dock = DockStyle.Fill,
                Text = "Enable Log File:",
                TextAlign = ContentAlignment.MiddleLeft
            };
            panel.Controls.Add(enableLogFileLabel);

            enableLogFile.Dock = DockStyle.Left;
            panel.Controls.Add(enableLogFile);

            Label logFilenameLabel = new Label()
            {
                Dock = DockStyle.Fill,
                Text = "Log File:",
                TextAlign = ContentAlignment.MiddleLeft
            };
            panel.Controls.Add(logFilenameLabel);

            logFilename.Dock = DockStyle.Fill;
            logFilename.ReadOnly = true;
            logFilename.Click += LogFilename_Click;
            panel.Controls.Add(logFilename);

            Button okButton = new Button()
            {
                DialogResult = DialogResult.OK,
                Text = "OK"
            };
            panel.Controls.Add(okButton);

            Button cancelButton = new Button()
            {
                DialogResult = DialogResult.Cancel,
                Text = "Cancel"
            };
            panel.Controls.Add(cancelButton);

            ResumeLayout();
        }

        private void LogFilename_Click(object sender, EventArgs args)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "All Files|*.*";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    logFilename.Text = dialog.FileName;
                    enableLogFile.Checked = true;
                }
            }

        }
    }
}
