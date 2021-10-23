namespace DayZServerMonitor
{
    partial class DayZServerMonitorForm
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (watcher != null)
                {
                    watcher.Dispose();
                }
                if (dynamicIcons != null)
                {
                    dynamicIcons.Dispose();
                }
                if (logViewer != null)
                {
                    logViewer.Dispose();
                }
                if (contextMenu != null)
                {
                    contextMenu.Dispose();
                }
                if (removeSelectedServer != null)
                {
                    removeSelectedServer.Dispose();
                }
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
            this.FormPanel = new System.Windows.Forms.Panel();
            this.FormTable = new System.Windows.Forms.TableLayoutPanel();
            this.ServerLabelPanel = new System.Windows.Forms.Panel();
            this.ServerLabel = new System.Windows.Forms.Label();
            this.ServerValuePanel = new System.Windows.Forms.Panel();
            this.ServerValue = new System.Windows.Forms.TextBox();
            this.NameLabelPanel = new System.Windows.Forms.Panel();
            this.NameLabel = new System.Windows.Forms.Label();
            this.NameValuePanel = new System.Windows.Forms.Panel();
            this.NameValue = new System.Windows.Forms.TextBox();
            this.PlayersLabelPanel = new System.Windows.Forms.Panel();
            this.PlayersLabel = new System.Windows.Forms.Label();
            this.PlayersValuePanel = new System.Windows.Forms.Panel();
            this.PlayersRowSplitContainer = new System.Windows.Forms.SplitContainer();
            this.PlayersValue = new System.Windows.Forms.TextBox();
            this.ServerTimeSplitContainer = new System.Windows.Forms.SplitContainer();
            this.ServerTimeLabel = new System.Windows.Forms.Label();
            this.ServerTime = new System.Windows.Forms.TextBox();
            this.MaxPlayersLabelPanel = new System.Windows.Forms.Panel();
            this.MaxPlayersLabel = new System.Windows.Forms.Label();
            this.MaxPlayersValuePanel = new System.Windows.Forms.Panel();
            this.MaxPlayersValue = new System.Windows.Forms.TextBox();
            this.SelectionLabelPanel = new System.Windows.Forms.Panel();
            this.SelectionLabel = new System.Windows.Forms.Label();
            this.SelectionPanel = new System.Windows.Forms.Panel();
            this.SelectionTable = new System.Windows.Forms.TableLayoutPanel();
            this.SelectionComboPanel = new System.Windows.Forms.Panel();
            this.SelectionCombo = new System.Windows.Forms.ComboBox();
            this.SelectionManagePanel = new System.Windows.Forms.Panel();
            this.SelectionManage = new System.Windows.Forms.Button();
            this.MonitorStatusStrip = new System.Windows.Forms.StatusStrip();
            this.MonitorStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.systemTrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.FormPanel.SuspendLayout();
            this.FormTable.SuspendLayout();
            this.ServerLabelPanel.SuspendLayout();
            this.ServerValuePanel.SuspendLayout();
            this.NameLabelPanel.SuspendLayout();
            this.NameValuePanel.SuspendLayout();
            this.PlayersLabelPanel.SuspendLayout();
            this.PlayersValuePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PlayersRowSplitContainer)).BeginInit();
            this.PlayersRowSplitContainer.Panel1.SuspendLayout();
            this.PlayersRowSplitContainer.Panel2.SuspendLayout();
            this.PlayersRowSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ServerTimeSplitContainer)).BeginInit();
            this.ServerTimeSplitContainer.Panel1.SuspendLayout();
            this.ServerTimeSplitContainer.Panel2.SuspendLayout();
            this.ServerTimeSplitContainer.SuspendLayout();
            this.MaxPlayersLabelPanel.SuspendLayout();
            this.MaxPlayersValuePanel.SuspendLayout();
            this.SelectionLabelPanel.SuspendLayout();
            this.SelectionPanel.SuspendLayout();
            this.SelectionTable.SuspendLayout();
            this.SelectionComboPanel.SuspendLayout();
            this.SelectionManagePanel.SuspendLayout();
            this.MonitorStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // FormPanel
            // 
            this.FormPanel.Controls.Add(this.FormTable);
            this.FormPanel.Controls.Add(this.MonitorStatusStrip);
            this.FormPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FormPanel.Location = new System.Drawing.Point(0, 0);
            this.FormPanel.Name = "FormPanel";
            this.FormPanel.Size = new System.Drawing.Size(478, 171);
            this.FormPanel.TabIndex = 0;
            // 
            // FormTable
            // 
            this.FormTable.ColumnCount = 2;
            this.FormTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.FormTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.FormTable.Controls.Add(this.ServerLabelPanel, 0, 1);
            this.FormTable.Controls.Add(this.ServerValuePanel, 1, 1);
            this.FormTable.Controls.Add(this.NameLabelPanel, 0, 2);
            this.FormTable.Controls.Add(this.NameValuePanel, 1, 2);
            this.FormTable.Controls.Add(this.PlayersLabelPanel, 0, 3);
            this.FormTable.Controls.Add(this.PlayersValuePanel, 1, 3);
            this.FormTable.Controls.Add(this.MaxPlayersLabelPanel, 0, 4);
            this.FormTable.Controls.Add(this.MaxPlayersValuePanel, 1, 4);
            this.FormTable.Controls.Add(this.SelectionLabelPanel, 0, 0);
            this.FormTable.Controls.Add(this.SelectionPanel, 1, 0);
            this.FormTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FormTable.Location = new System.Drawing.Point(0, 0);
            this.FormTable.Name = "FormTable";
            this.FormTable.RowCount = 5;
            this.FormTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.FormTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.FormTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.FormTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.FormTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.FormTable.Size = new System.Drawing.Size(478, 149);
            this.FormTable.TabIndex = 0;
            // 
            // ServerLabelPanel
            // 
            this.ServerLabelPanel.Controls.Add(this.ServerLabel);
            this.ServerLabelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ServerLabelPanel.Location = new System.Drawing.Point(3, 32);
            this.ServerLabelPanel.Name = "ServerLabelPanel";
            this.ServerLabelPanel.Size = new System.Drawing.Size(89, 23);
            this.ServerLabelPanel.TabIndex = 0;
            // 
            // ServerLabel
            // 
            this.ServerLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ServerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerLabel.Location = new System.Drawing.Point(0, 0);
            this.ServerLabel.Name = "ServerLabel";
            this.ServerLabel.Size = new System.Drawing.Size(89, 23);
            this.ServerLabel.TabIndex = 0;
            this.ServerLabel.Text = "Address";
            this.ServerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ServerValuePanel
            // 
            this.ServerValuePanel.Controls.Add(this.ServerValue);
            this.ServerValuePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ServerValuePanel.Location = new System.Drawing.Point(98, 32);
            this.ServerValuePanel.Name = "ServerValuePanel";
            this.ServerValuePanel.Size = new System.Drawing.Size(377, 23);
            this.ServerValuePanel.TabIndex = 1;
            // 
            // ServerValue
            // 
            this.ServerValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ServerValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerValue.Location = new System.Drawing.Point(0, 0);
            this.ServerValue.Name = "ServerValue";
            this.ServerValue.ReadOnly = true;
            this.ServerValue.Size = new System.Drawing.Size(377, 23);
            this.ServerValue.TabIndex = 0;
            // 
            // NameLabelPanel
            // 
            this.NameLabelPanel.Controls.Add(this.NameLabel);
            this.NameLabelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NameLabelPanel.Location = new System.Drawing.Point(3, 61);
            this.NameLabelPanel.Name = "NameLabelPanel";
            this.NameLabelPanel.Size = new System.Drawing.Size(89, 23);
            this.NameLabelPanel.TabIndex = 2;
            // 
            // NameLabel
            // 
            this.NameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameLabel.Location = new System.Drawing.Point(0, 0);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(89, 23);
            this.NameLabel.TabIndex = 0;
            this.NameLabel.Text = "Name";
            this.NameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NameValuePanel
            // 
            this.NameValuePanel.Controls.Add(this.NameValue);
            this.NameValuePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NameValuePanel.Location = new System.Drawing.Point(98, 61);
            this.NameValuePanel.Name = "NameValuePanel";
            this.NameValuePanel.Size = new System.Drawing.Size(377, 23);
            this.NameValuePanel.TabIndex = 3;
            // 
            // NameValue
            // 
            this.NameValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NameValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameValue.Location = new System.Drawing.Point(0, 0);
            this.NameValue.Name = "NameValue";
            this.NameValue.ReadOnly = true;
            this.NameValue.Size = new System.Drawing.Size(377, 23);
            this.NameValue.TabIndex = 0;
            // 
            // PlayersLabelPanel
            // 
            this.PlayersLabelPanel.Controls.Add(this.PlayersLabel);
            this.PlayersLabelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PlayersLabelPanel.Location = new System.Drawing.Point(3, 90);
            this.PlayersLabelPanel.Name = "PlayersLabelPanel";
            this.PlayersLabelPanel.Size = new System.Drawing.Size(89, 23);
            this.PlayersLabelPanel.TabIndex = 4;
            // 
            // PlayersLabel
            // 
            this.PlayersLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PlayersLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayersLabel.Location = new System.Drawing.Point(0, 0);
            this.PlayersLabel.Name = "PlayersLabel";
            this.PlayersLabel.Size = new System.Drawing.Size(89, 23);
            this.PlayersLabel.TabIndex = 0;
            this.PlayersLabel.Text = "Players";
            this.PlayersLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PlayersValuePanel
            // 
            this.PlayersValuePanel.Controls.Add(this.PlayersRowSplitContainer);
            this.PlayersValuePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PlayersValuePanel.Location = new System.Drawing.Point(98, 90);
            this.PlayersValuePanel.Name = "PlayersValuePanel";
            this.PlayersValuePanel.Size = new System.Drawing.Size(377, 23);
            this.PlayersValuePanel.TabIndex = 5;
            // 
            // PlayersRowSplitContainer
            // 
            this.PlayersRowSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PlayersRowSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.PlayersRowSplitContainer.Name = "PlayersRowSplitContainer";
            // 
            // PlayersRowSplitContainer.Panel1
            // 
            this.PlayersRowSplitContainer.Panel1.Controls.Add(this.PlayersValue);
            // 
            // PlayersRowSplitContainer.Panel2
            // 
            this.PlayersRowSplitContainer.Panel2.Controls.Add(this.ServerTimeSplitContainer);
            this.PlayersRowSplitContainer.Size = new System.Drawing.Size(377, 23);
            this.PlayersRowSplitContainer.SplitterDistance = 222;
            this.PlayersRowSplitContainer.TabIndex = 0;
            // 
            // PlayersValue
            // 
            this.PlayersValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PlayersValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayersValue.ForeColor = System.Drawing.SystemColors.WindowText;
            this.PlayersValue.Location = new System.Drawing.Point(0, 0);
            this.PlayersValue.Name = "PlayersValue";
            this.PlayersValue.ReadOnly = true;
            this.PlayersValue.Size = new System.Drawing.Size(222, 23);
            this.PlayersValue.TabIndex = 1;
            // 
            // ServerTimeSplitContainer
            // 
            this.ServerTimeSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ServerTimeSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.ServerTimeSplitContainer.Name = "ServerTimeSplitContainer";
            // 
            // ServerTimeSplitContainer.Panel1
            // 
            this.ServerTimeSplitContainer.Panel1.Controls.Add(this.ServerTimeLabel);
            // 
            // ServerTimeSplitContainer.Panel2
            // 
            this.ServerTimeSplitContainer.Panel2.Controls.Add(this.ServerTime);
            this.ServerTimeSplitContainer.Size = new System.Drawing.Size(151, 23);
            this.ServerTimeSplitContainer.TabIndex = 0;
            // 
            // ServerTimeLabel
            // 
            this.ServerTimeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ServerTimeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F);
            this.ServerTimeLabel.Location = new System.Drawing.Point(0, 0);
            this.ServerTimeLabel.Name = "ServerTimeLabel";
            this.ServerTimeLabel.Size = new System.Drawing.Size(50, 23);
            this.ServerTimeLabel.TabIndex = 0;
            this.ServerTimeLabel.Text = "Time";
            this.ServerTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ServerTime
            // 
            this.ServerTime.BackColor = System.Drawing.SystemColors.Control;
            this.ServerTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ServerTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F);
            this.ServerTime.Location = new System.Drawing.Point(0, 0);
            this.ServerTime.Name = "ServerTime";
            this.ServerTime.ReadOnly = true;
            this.ServerTime.Size = new System.Drawing.Size(97, 23);
            this.ServerTime.TabIndex = 0;
            // 
            // MaxPlayersLabelPanel
            // 
            this.MaxPlayersLabelPanel.Controls.Add(this.MaxPlayersLabel);
            this.MaxPlayersLabelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MaxPlayersLabelPanel.Location = new System.Drawing.Point(3, 119);
            this.MaxPlayersLabelPanel.Name = "MaxPlayersLabelPanel";
            this.MaxPlayersLabelPanel.Size = new System.Drawing.Size(89, 27);
            this.MaxPlayersLabelPanel.TabIndex = 6;
            // 
            // MaxPlayersLabel
            // 
            this.MaxPlayersLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MaxPlayersLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaxPlayersLabel.Location = new System.Drawing.Point(0, 0);
            this.MaxPlayersLabel.Name = "MaxPlayersLabel";
            this.MaxPlayersLabel.Size = new System.Drawing.Size(89, 27);
            this.MaxPlayersLabel.TabIndex = 0;
            this.MaxPlayersLabel.Text = "Max Players";
            this.MaxPlayersLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MaxPlayersValuePanel
            // 
            this.MaxPlayersValuePanel.Controls.Add(this.MaxPlayersValue);
            this.MaxPlayersValuePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MaxPlayersValuePanel.Location = new System.Drawing.Point(98, 119);
            this.MaxPlayersValuePanel.Name = "MaxPlayersValuePanel";
            this.MaxPlayersValuePanel.Size = new System.Drawing.Size(377, 27);
            this.MaxPlayersValuePanel.TabIndex = 7;
            // 
            // MaxPlayersValue
            // 
            this.MaxPlayersValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MaxPlayersValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaxPlayersValue.Location = new System.Drawing.Point(0, 0);
            this.MaxPlayersValue.Name = "MaxPlayersValue";
            this.MaxPlayersValue.ReadOnly = true;
            this.MaxPlayersValue.Size = new System.Drawing.Size(377, 23);
            this.MaxPlayersValue.TabIndex = 0;
            // 
            // SelectionLabelPanel
            // 
            this.SelectionLabelPanel.Controls.Add(this.SelectionLabel);
            this.SelectionLabelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SelectionLabelPanel.Location = new System.Drawing.Point(2, 2);
            this.SelectionLabelPanel.Margin = new System.Windows.Forms.Padding(2);
            this.SelectionLabelPanel.Name = "SelectionLabelPanel";
            this.SelectionLabelPanel.Size = new System.Drawing.Size(91, 25);
            this.SelectionLabelPanel.TabIndex = 8;
            // 
            // SelectionLabel
            // 
            this.SelectionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SelectionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectionLabel.Location = new System.Drawing.Point(0, 0);
            this.SelectionLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.SelectionLabel.Name = "SelectionLabel";
            this.SelectionLabel.Size = new System.Drawing.Size(91, 25);
            this.SelectionLabel.TabIndex = 0;
            this.SelectionLabel.Text = "Server";
            this.SelectionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SelectionPanel
            // 
            this.SelectionPanel.Controls.Add(this.SelectionTable);
            this.SelectionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SelectionPanel.Location = new System.Drawing.Point(97, 2);
            this.SelectionPanel.Margin = new System.Windows.Forms.Padding(2);
            this.SelectionPanel.Name = "SelectionPanel";
            this.SelectionPanel.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.SelectionPanel.Size = new System.Drawing.Size(379, 25);
            this.SelectionPanel.TabIndex = 9;
            // 
            // SelectionTable
            // 
            this.SelectionTable.ColumnCount = 2;
            this.SelectionTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.SelectionTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.SelectionTable.Controls.Add(this.SelectionComboPanel, 0, 0);
            this.SelectionTable.Controls.Add(this.SelectionManagePanel, 1, 0);
            this.SelectionTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SelectionTable.Location = new System.Drawing.Point(1, 0);
            this.SelectionTable.Margin = new System.Windows.Forms.Padding(2);
            this.SelectionTable.Name = "SelectionTable";
            this.SelectionTable.RowCount = 1;
            this.SelectionTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SelectionTable.Size = new System.Drawing.Size(378, 25);
            this.SelectionTable.TabIndex = 1;
            // 
            // SelectionComboPanel
            // 
            this.SelectionComboPanel.Controls.Add(this.SelectionCombo);
            this.SelectionComboPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SelectionComboPanel.Location = new System.Drawing.Point(0, 0);
            this.SelectionComboPanel.Margin = new System.Windows.Forms.Padding(0);
            this.SelectionComboPanel.Name = "SelectionComboPanel";
            this.SelectionComboPanel.Size = new System.Drawing.Size(340, 25);
            this.SelectionComboPanel.TabIndex = 0;
            // 
            // SelectionCombo
            // 
            this.SelectionCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SelectionCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectionCombo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectionCombo.FormattingEnabled = true;
            this.SelectionCombo.Location = new System.Drawing.Point(0, 0);
            this.SelectionCombo.Margin = new System.Windows.Forms.Padding(0);
            this.SelectionCombo.Name = "SelectionCombo";
            this.SelectionCombo.Size = new System.Drawing.Size(340, 25);
            this.SelectionCombo.TabIndex = 1;
            // 
            // SelectionManagePanel
            // 
            this.SelectionManagePanel.Controls.Add(this.SelectionManage);
            this.SelectionManagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SelectionManagePanel.Location = new System.Drawing.Point(342, 0);
            this.SelectionManagePanel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.SelectionManagePanel.Name = "SelectionManagePanel";
            this.SelectionManagePanel.Size = new System.Drawing.Size(34, 25);
            this.SelectionManagePanel.TabIndex = 1;
            // 
            // SelectionManage
            // 
            this.SelectionManage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SelectionManage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectionManage.Location = new System.Drawing.Point(0, 0);
            this.SelectionManage.Margin = new System.Windows.Forms.Padding(0);
            this.SelectionManage.Name = "SelectionManage";
            this.SelectionManage.Size = new System.Drawing.Size(34, 25);
            this.SelectionManage.TabIndex = 0;
            this.SelectionManage.Text = "≡";
            this.SelectionManage.UseVisualStyleBackColor = true;
            this.SelectionManage.Click += new System.EventHandler(this.SelectionManage_Click);
            // 
            // MonitorStatusStrip
            // 
            this.MonitorStatusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MonitorStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MonitorStatus});
            this.MonitorStatusStrip.Location = new System.Drawing.Point(0, 149);
            this.MonitorStatusStrip.Name = "MonitorStatusStrip";
            this.MonitorStatusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.MonitorStatusStrip.Size = new System.Drawing.Size(478, 22);
            this.MonitorStatusStrip.SizingGrip = false;
            this.MonitorStatusStrip.TabIndex = 1;
            // 
            // MonitorStatus
            // 
            this.MonitorStatus.Name = "MonitorStatus";
            this.MonitorStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // systemTrayIcon
            // 
            this.systemTrayIcon.Text = "DayZ Server Monitor";
            this.systemTrayIcon.Visible = true;
            this.systemTrayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.SystemTrayIcon_MouseDoubleClick);
            // 
            // DayZServerMonitorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 171);
            this.Controls.Add(this.FormPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = global::DayZServerMonitor.Properties.Resources.DayZServerMonitorIcon;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(494, 210);
            this.MinimumSize = new System.Drawing.Size(494, 210);
            this.Name = "DayZServerMonitorForm";
            this.Text = "DayZ Server Monitor";
            this.Resize += new System.EventHandler(this.DayZServerMonitorForm_Resize);
            this.FormPanel.ResumeLayout(false);
            this.FormPanel.PerformLayout();
            this.FormTable.ResumeLayout(false);
            this.ServerLabelPanel.ResumeLayout(false);
            this.ServerValuePanel.ResumeLayout(false);
            this.ServerValuePanel.PerformLayout();
            this.NameLabelPanel.ResumeLayout(false);
            this.NameValuePanel.ResumeLayout(false);
            this.NameValuePanel.PerformLayout();
            this.PlayersLabelPanel.ResumeLayout(false);
            this.PlayersValuePanel.ResumeLayout(false);
            this.PlayersRowSplitContainer.Panel1.ResumeLayout(false);
            this.PlayersRowSplitContainer.Panel1.PerformLayout();
            this.PlayersRowSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PlayersRowSplitContainer)).EndInit();
            this.PlayersRowSplitContainer.ResumeLayout(false);
            this.ServerTimeSplitContainer.Panel1.ResumeLayout(false);
            this.ServerTimeSplitContainer.Panel2.ResumeLayout(false);
            this.ServerTimeSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ServerTimeSplitContainer)).EndInit();
            this.ServerTimeSplitContainer.ResumeLayout(false);
            this.MaxPlayersLabelPanel.ResumeLayout(false);
            this.MaxPlayersValuePanel.ResumeLayout(false);
            this.MaxPlayersValuePanel.PerformLayout();
            this.SelectionLabelPanel.ResumeLayout(false);
            this.SelectionPanel.ResumeLayout(false);
            this.SelectionTable.ResumeLayout(false);
            this.SelectionComboPanel.ResumeLayout(false);
            this.SelectionManagePanel.ResumeLayout(false);
            this.MonitorStatusStrip.ResumeLayout(false);
            this.MonitorStatusStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel FormPanel;
        private System.Windows.Forms.TableLayoutPanel FormTable;
        private System.Windows.Forms.Panel ServerLabelPanel;
        private System.Windows.Forms.Label ServerLabel;
        private System.Windows.Forms.Panel ServerValuePanel;
        private System.Windows.Forms.TextBox ServerValue;
        private System.Windows.Forms.Panel NameLabelPanel;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.Panel NameValuePanel;
        private System.Windows.Forms.TextBox NameValue;
        private System.Windows.Forms.Panel PlayersLabelPanel;
        private System.Windows.Forms.Label PlayersLabel;
        private System.Windows.Forms.Panel PlayersValuePanel;
        private System.Windows.Forms.Panel MaxPlayersLabelPanel;
        private System.Windows.Forms.Label MaxPlayersLabel;
        private System.Windows.Forms.Panel MaxPlayersValuePanel;
        private System.Windows.Forms.TextBox MaxPlayersValue;
        private System.Windows.Forms.StatusStrip MonitorStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel MonitorStatus;
        private System.Windows.Forms.Panel SelectionLabelPanel;
        private System.Windows.Forms.Panel SelectionPanel;
        private System.Windows.Forms.Label SelectionLabel;
        private System.Windows.Forms.NotifyIcon systemTrayIcon;
        private System.Windows.Forms.TableLayoutPanel SelectionTable;
        private System.Windows.Forms.Panel SelectionComboPanel;
        private System.Windows.Forms.ComboBox SelectionCombo;
        private System.Windows.Forms.Panel SelectionManagePanel;
        private System.Windows.Forms.Button SelectionManage;
        private System.Windows.Forms.SplitContainer PlayersRowSplitContainer;
        private System.Windows.Forms.TextBox PlayersValue;
        private System.Windows.Forms.SplitContainer ServerTimeSplitContainer;
        private System.Windows.Forms.Label ServerTimeLabel;
        private System.Windows.Forms.TextBox ServerTime;
    }
}