namespace DayZServerMonitor
{
    partial class LogViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.logViewerSplit = new System.Windows.Forms.SplitContainer();
            this.logsDataGridView = new System.Windows.Forms.DataGridView();
            this.logEntry = new System.Windows.Forms.WebBrowser();
            this.logEntryContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.logViewerSplit)).BeginInit();
            this.logViewerSplit.Panel1.SuspendLayout();
            this.logViewerSplit.Panel2.SuspendLayout();
            this.logViewerSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logsDataGridView)).BeginInit();
            this.logEntryContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // logViewerSplit
            // 
            this.logViewerSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logViewerSplit.Location = new System.Drawing.Point(0, 0);
            this.logViewerSplit.Name = "logViewerSplit";
            this.logViewerSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // logViewerSplit.Panel1
            // 
            this.logViewerSplit.Panel1.Controls.Add(this.logsDataGridView);
            this.logViewerSplit.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // logViewerSplit.Panel2
            // 
            this.logViewerSplit.Panel2.Controls.Add(this.logEntry);
            this.logViewerSplit.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.logViewerSplit.Size = new System.Drawing.Size(800, 450);
            this.logViewerSplit.SplitterDistance = 322;
            this.logViewerSplit.TabIndex = 0;
            // 
            // logsDataGridView
            // 
            this.logsDataGridView.AllowUserToAddRows = false;
            this.logsDataGridView.AllowUserToDeleteRows = false;
            this.logsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.logsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logsDataGridView.Location = new System.Drawing.Point(0, 0);
            this.logsDataGridView.Name = "logsDataGridView";
            this.logsDataGridView.ReadOnly = true;
            this.logsDataGridView.RowHeadersWidth = 51;
            this.logsDataGridView.RowTemplate.Height = 24;
            this.logsDataGridView.Size = new System.Drawing.Size(800, 322);
            this.logsDataGridView.TabIndex = 0;
            // 
            // logEntry
            // 
            this.logEntry.AllowNavigation = false;
            this.logEntry.AllowWebBrowserDrop = false;
            this.logEntry.ContextMenuStrip = this.logEntryContextMenuStrip;
            this.logEntry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logEntry.IsWebBrowserContextMenuEnabled = false;
            this.logEntry.Location = new System.Drawing.Point(0, 0);
            this.logEntry.MinimumSize = new System.Drawing.Size(20, 20);
            this.logEntry.Name = "logEntry";
            this.logEntry.Size = new System.Drawing.Size(800, 124);
            this.logEntry.TabIndex = 0;
            this.logEntry.WebBrowserShortcutsEnabled = false;
            this.logEntry.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.LogEntry_PreviewKeyDown);
            // 
            // logEntryContextMenuStrip
            // 
            this.logEntryContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.logEntryContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.selectAllToolStripMenuItem});
            this.logEntryContextMenuStrip.Name = "logEntryContextMenuStrip";
            this.logEntryContextMenuStrip.Size = new System.Drawing.Size(141, 52);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.CopyToolStripMenuItem_Click);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.SelectAllToolStripMenuItem_Click);
            // 
            // LogViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.logViewerSplit);
            this.Icon = global::DayZServerMonitor.Properties.Resources.DayZServerMonitorIcon;
            this.Name = "LogViewer";
            this.Text = "Logs";
            this.logViewerSplit.Panel1.ResumeLayout(false);
            this.logViewerSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logViewerSplit)).EndInit();
            this.logViewerSplit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logsDataGridView)).EndInit();
            this.logEntryContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer logViewerSplit;
        private System.Windows.Forms.DataGridView logsDataGridView;
        private System.Windows.Forms.WebBrowser logEntry;
        private System.Windows.Forms.ContextMenuStrip logEntryContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
    }
}