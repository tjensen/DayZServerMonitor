using DayZServerMonitorCore;
using System;
using System.Windows.Forms;

namespace DayZServerMonitor
{
    public class SettingsDialog : Form
    {
        private readonly ComboBox hideTaskBarIcon = new ComboBox();
        private readonly NumericUpDown maxLogViewerEntries = new NumericUpDown();

        public SettingsDialog()
        {
            InitializeComponent();
        }

        public void ShowDialog(Settings settings)
        {
            hideTaskBarIcon.SelectedItem = settings.HideTaskBarIcon;
            maxLogViewerEntries.Value = settings.MaxLogViewerEntries;
            if (ShowDialog() == DialogResult.OK)
            {
                settings.HideTaskBarIcon = (Settings.HideTaskBarIconValues)hideTaskBarIcon.SelectedItem;
                settings.MaxLogViewerEntries = (int)maxLogViewerEntries.Value;
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
                Text = "Hide Task Bar Icon:"
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
                Text = "Maximum Log Viewer Entries:"
            };
            panel.Controls.Add(maxLogViewerEntriesLabel);

            maxLogViewerEntries.Dock = DockStyle.Left;
            maxLogViewerEntries.Minimum = 1;
            maxLogViewerEntries.Maximum = int.MaxValue;
            panel.Controls.Add(maxLogViewerEntries);

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
    }
}
