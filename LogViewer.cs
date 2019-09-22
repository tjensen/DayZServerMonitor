using DayZServerMonitorCore;
using System;
using System.Windows.Forms;

namespace DayZServerMonitor
{
    public partial class LogViewer : Form
    {
        private readonly Settings settings;

        public LogViewer(Settings settings)
        {
            InitializeComponent();

            this.settings = settings;
            this.settings.SettingChanged += Settings_SettingChanged;

            FormClosing += (source, args) => { args.Cancel = true; Hide(); };
            logsDataGridView.ColumnCount = 3;
            logsDataGridView.RowHeadersVisible = false;
            logsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            logsDataGridView.MultiSelect = false;
            logsDataGridView.AllowUserToResizeRows = false;
            logsDataGridView.Columns[0].Name = "Timestamp";
            logsDataGridView.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            logsDataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            logsDataGridView.Columns[1].Name = "Level";
            logsDataGridView.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            logsDataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            logsDataGridView.Columns[2].Name = "Message";
            logsDataGridView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            logsDataGridView.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            logsDataGridView.SelectionChanged += LogSelectionChanged;
            logEntry.Navigate("about:blank");
        }

        private void Settings_SettingChanged(object sender, SettingChangedArgs args)
        {
            if (args.SettingName == nameof(settings.MaxLogViewerEntries))
            {
                TrimEntries();
            }
        }

        public void Add(string level, string message)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { Add(level, message); }));
                return;
            }

            bool selectionAtEnd = IsScrolledToEnd();

            logsDataGridView.Rows.Add(new string[] { DateTime.Now.ToString(), level, message });

            TrimEntries();

            if (selectionAtEnd)
            {
                ScrollToEnd();
            }
        }

        private void TrimEntries()
        {
            while (logsDataGridView.Rows.Count > settings.MaxLogViewerEntries)
            {
                logsDataGridView.Rows.RemoveAt(0);
            }
        }

        private bool IsScrolledToEnd()
        {
            int firstDisplayed = logsDataGridView.FirstDisplayedScrollingRowIndex;
            int displayed = logsDataGridView.DisplayedRowCount(true);
            int lastVisible = (firstDisplayed + displayed) - 1;
            int lastIndex = logsDataGridView.RowCount - 1;
            return lastVisible == lastIndex;
        }

        private void ScrollToEnd()
        {
            logsDataGridView.FirstDisplayedScrollingRowIndex = Math.Max(
                0,
                logsDataGridView.RowCount - logsDataGridView.DisplayedRowCount(true));
        }

        private void LogSelectionChanged(object source, EventArgs args)
        {
            var cells = logsDataGridView.SelectedRows[0].Cells;
            HtmlDocument doc = logEntry.Document.OpenNew(true);
            doc.Write("<html><head>");
            doc.Write("<style>body {font-family: sans-serif; font-size: 12;}; dt {font-weight: bold;}; dd {font-family: monospace;}</style>");
            doc.Write("</head><body></body></html>");

            var dl = doc.CreateElement("dl");
            doc.Body.AppendChild(dl);

            var timestampDt = doc.CreateElement("dt");
            timestampDt.InnerText = "Timestamp";
            dl.AppendChild(timestampDt);
            var timestampDd = doc.CreateElement("dd");
            timestampDd.InnerText = cells[0].Value.ToString();
            dl.AppendChild(timestampDd);

            var levelDt = doc.CreateElement("dt");
            levelDt.InnerText = "Level";
            dl.AppendChild(levelDt);
            var levelDd = doc.CreateElement("dd");
            levelDd.InnerText = cells[1].Value.ToString();
            dl.AppendChild(levelDd);

            var messageDt = doc.CreateElement("dt");
            messageDt.InnerText = "Message";
            dl.AppendChild(messageDt);
            var messageDd = doc.CreateElement("dd");
            messageDd.InnerText = cells[2].Value.ToString();
            dl.AppendChild(messageDd);
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logEntry.Document.ExecCommand("copy", false, null);
        }

        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logEntry.Document.ExecCommand("selectAll", false, null);
        }

        private void LogEntry_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (!e.Control)
            {
                return;
            }

            if (e.KeyCode == Keys.A)
            {
                logEntry.Document.ExecCommand("selectAll", false, null);
            }
            else if (e.KeyCode == Keys.C)
            {
                logEntry.Document.ExecCommand("copy", false, null);
            }
        }
    }
}
