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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DayZServerMonitorForm));
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
            this.PlayersValue = new System.Windows.Forms.TextBox();
            this.MaxPlayersLabelPanel = new System.Windows.Forms.Panel();
            this.MaxPlayersLabel = new System.Windows.Forms.Label();
            this.MaxPlayersValuePanel = new System.Windows.Forms.Panel();
            this.MaxPlayersValue = new System.Windows.Forms.TextBox();
            this.MonitorStatusStrip = new System.Windows.Forms.StatusStrip();
            this.MonitorStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.SelectionLabelPanel = new System.Windows.Forms.Panel();
            this.SelectionComboPanel = new System.Windows.Forms.Panel();
            this.SelectionCombo = new System.Windows.Forms.ComboBox();
            this.SelectionLabel = new System.Windows.Forms.Label();
            this.FormPanel.SuspendLayout();
            this.FormTable.SuspendLayout();
            this.ServerLabelPanel.SuspendLayout();
            this.ServerValuePanel.SuspendLayout();
            this.NameLabelPanel.SuspendLayout();
            this.NameValuePanel.SuspendLayout();
            this.PlayersLabelPanel.SuspendLayout();
            this.PlayersValuePanel.SuspendLayout();
            this.MaxPlayersLabelPanel.SuspendLayout();
            this.MaxPlayersValuePanel.SuspendLayout();
            this.MonitorStatusStrip.SuspendLayout();
            this.SelectionLabelPanel.SuspendLayout();
            this.SelectionComboPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // FormPanel
            // 
            this.FormPanel.Controls.Add(this.FormTable);
            this.FormPanel.Controls.Add(this.MonitorStatusStrip);
            this.FormPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FormPanel.Location = new System.Drawing.Point(0, 0);
            this.FormPanel.Margin = new System.Windows.Forms.Padding(4);
            this.FormPanel.Name = "FormPanel";
            this.FormPanel.Size = new System.Drawing.Size(635, 203);
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
            this.FormTable.Controls.Add(this.SelectionComboPanel, 1, 0);
            this.FormTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FormTable.Location = new System.Drawing.Point(0, 0);
            this.FormTable.Margin = new System.Windows.Forms.Padding(4);
            this.FormTable.Name = "FormTable";
            this.FormTable.RowCount = 5;
            this.FormTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.FormTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.FormTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.FormTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.FormTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.FormTable.Size = new System.Drawing.Size(635, 181);
            this.FormTable.TabIndex = 0;
            // 
            // ServerLabelPanel
            // 
            this.ServerLabelPanel.Controls.Add(this.ServerLabel);
            this.ServerLabelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ServerLabelPanel.Location = new System.Drawing.Point(4, 40);
            this.ServerLabelPanel.Margin = new System.Windows.Forms.Padding(4);
            this.ServerLabelPanel.Name = "ServerLabelPanel";
            this.ServerLabelPanel.Size = new System.Drawing.Size(119, 28);
            this.ServerLabelPanel.TabIndex = 0;
            // 
            // ServerLabel
            // 
            this.ServerLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ServerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerLabel.Location = new System.Drawing.Point(0, 0);
            this.ServerLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ServerLabel.Name = "ServerLabel";
            this.ServerLabel.Size = new System.Drawing.Size(119, 28);
            this.ServerLabel.TabIndex = 0;
            this.ServerLabel.Text = "Address";
            this.ServerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ServerValuePanel
            // 
            this.ServerValuePanel.Controls.Add(this.ServerValue);
            this.ServerValuePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ServerValuePanel.Location = new System.Drawing.Point(131, 40);
            this.ServerValuePanel.Margin = new System.Windows.Forms.Padding(4);
            this.ServerValuePanel.Name = "ServerValuePanel";
            this.ServerValuePanel.Size = new System.Drawing.Size(500, 28);
            this.ServerValuePanel.TabIndex = 1;
            // 
            // ServerValue
            // 
            this.ServerValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ServerValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerValue.Location = new System.Drawing.Point(0, 0);
            this.ServerValue.Margin = new System.Windows.Forms.Padding(4);
            this.ServerValue.Name = "ServerValue";
            this.ServerValue.ReadOnly = true;
            this.ServerValue.Size = new System.Drawing.Size(500, 27);
            this.ServerValue.TabIndex = 0;
            // 
            // NameLabelPanel
            // 
            this.NameLabelPanel.Controls.Add(this.NameLabel);
            this.NameLabelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NameLabelPanel.Location = new System.Drawing.Point(4, 76);
            this.NameLabelPanel.Margin = new System.Windows.Forms.Padding(4);
            this.NameLabelPanel.Name = "NameLabelPanel";
            this.NameLabelPanel.Size = new System.Drawing.Size(119, 28);
            this.NameLabelPanel.TabIndex = 2;
            // 
            // NameLabel
            // 
            this.NameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameLabel.Location = new System.Drawing.Point(0, 0);
            this.NameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(119, 28);
            this.NameLabel.TabIndex = 0;
            this.NameLabel.Text = "Name";
            this.NameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NameValuePanel
            // 
            this.NameValuePanel.Controls.Add(this.NameValue);
            this.NameValuePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NameValuePanel.Location = new System.Drawing.Point(131, 76);
            this.NameValuePanel.Margin = new System.Windows.Forms.Padding(4);
            this.NameValuePanel.Name = "NameValuePanel";
            this.NameValuePanel.Size = new System.Drawing.Size(500, 28);
            this.NameValuePanel.TabIndex = 3;
            // 
            // NameValue
            // 
            this.NameValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NameValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameValue.Location = new System.Drawing.Point(0, 0);
            this.NameValue.Margin = new System.Windows.Forms.Padding(4);
            this.NameValue.Name = "NameValue";
            this.NameValue.ReadOnly = true;
            this.NameValue.Size = new System.Drawing.Size(500, 27);
            this.NameValue.TabIndex = 0;
            // 
            // PlayersLabelPanel
            // 
            this.PlayersLabelPanel.Controls.Add(this.PlayersLabel);
            this.PlayersLabelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PlayersLabelPanel.Location = new System.Drawing.Point(4, 112);
            this.PlayersLabelPanel.Margin = new System.Windows.Forms.Padding(4);
            this.PlayersLabelPanel.Name = "PlayersLabelPanel";
            this.PlayersLabelPanel.Size = new System.Drawing.Size(119, 28);
            this.PlayersLabelPanel.TabIndex = 4;
            // 
            // PlayersLabel
            // 
            this.PlayersLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PlayersLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayersLabel.Location = new System.Drawing.Point(0, 0);
            this.PlayersLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PlayersLabel.Name = "PlayersLabel";
            this.PlayersLabel.Size = new System.Drawing.Size(119, 28);
            this.PlayersLabel.TabIndex = 0;
            this.PlayersLabel.Text = "Players";
            this.PlayersLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PlayersValuePanel
            // 
            this.PlayersValuePanel.Controls.Add(this.PlayersValue);
            this.PlayersValuePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PlayersValuePanel.Location = new System.Drawing.Point(131, 112);
            this.PlayersValuePanel.Margin = new System.Windows.Forms.Padding(4);
            this.PlayersValuePanel.Name = "PlayersValuePanel";
            this.PlayersValuePanel.Size = new System.Drawing.Size(500, 28);
            this.PlayersValuePanel.TabIndex = 5;
            // 
            // PlayersValue
            // 
            this.PlayersValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PlayersValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayersValue.ForeColor = System.Drawing.SystemColors.WindowText;
            this.PlayersValue.Location = new System.Drawing.Point(0, 0);
            this.PlayersValue.Margin = new System.Windows.Forms.Padding(4);
            this.PlayersValue.Name = "PlayersValue";
            this.PlayersValue.ReadOnly = true;
            this.PlayersValue.Size = new System.Drawing.Size(500, 27);
            this.PlayersValue.TabIndex = 0;
            // 
            // MaxPlayersLabelPanel
            // 
            this.MaxPlayersLabelPanel.Controls.Add(this.MaxPlayersLabel);
            this.MaxPlayersLabelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MaxPlayersLabelPanel.Location = new System.Drawing.Point(4, 148);
            this.MaxPlayersLabelPanel.Margin = new System.Windows.Forms.Padding(4);
            this.MaxPlayersLabelPanel.Name = "MaxPlayersLabelPanel";
            this.MaxPlayersLabelPanel.Size = new System.Drawing.Size(119, 29);
            this.MaxPlayersLabelPanel.TabIndex = 6;
            // 
            // MaxPlayersLabel
            // 
            this.MaxPlayersLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MaxPlayersLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaxPlayersLabel.Location = new System.Drawing.Point(0, 0);
            this.MaxPlayersLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MaxPlayersLabel.Name = "MaxPlayersLabel";
            this.MaxPlayersLabel.Size = new System.Drawing.Size(119, 29);
            this.MaxPlayersLabel.TabIndex = 0;
            this.MaxPlayersLabel.Text = "Max Players";
            this.MaxPlayersLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MaxPlayersValuePanel
            // 
            this.MaxPlayersValuePanel.Controls.Add(this.MaxPlayersValue);
            this.MaxPlayersValuePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MaxPlayersValuePanel.Location = new System.Drawing.Point(131, 148);
            this.MaxPlayersValuePanel.Margin = new System.Windows.Forms.Padding(4);
            this.MaxPlayersValuePanel.Name = "MaxPlayersValuePanel";
            this.MaxPlayersValuePanel.Size = new System.Drawing.Size(500, 29);
            this.MaxPlayersValuePanel.TabIndex = 7;
            // 
            // MaxPlayersValue
            // 
            this.MaxPlayersValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MaxPlayersValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaxPlayersValue.Location = new System.Drawing.Point(0, 0);
            this.MaxPlayersValue.Margin = new System.Windows.Forms.Padding(4);
            this.MaxPlayersValue.Name = "MaxPlayersValue";
            this.MaxPlayersValue.ReadOnly = true;
            this.MaxPlayersValue.Size = new System.Drawing.Size(500, 27);
            this.MaxPlayersValue.TabIndex = 0;
            // 
            // MonitorStatusStrip
            // 
            this.MonitorStatusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MonitorStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MonitorStatus});
            this.MonitorStatusStrip.Location = new System.Drawing.Point(0, 181);
            this.MonitorStatusStrip.Name = "MonitorStatusStrip";
            this.MonitorStatusStrip.Size = new System.Drawing.Size(635, 22);
            this.MonitorStatusStrip.SizingGrip = false;
            this.MonitorStatusStrip.TabIndex = 1;
            // 
            // MonitorStatus
            // 
            this.MonitorStatus.Name = "MonitorStatus";
            this.MonitorStatus.Size = new System.Drawing.Size(0, 16);
            // 
            // SelectionLabelPanel
            // 
            this.SelectionLabelPanel.Controls.Add(this.SelectionLabel);
            this.SelectionLabelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SelectionLabelPanel.Location = new System.Drawing.Point(3, 3);
            this.SelectionLabelPanel.Name = "SelectionLabelPanel";
            this.SelectionLabelPanel.Size = new System.Drawing.Size(121, 30);
            this.SelectionLabelPanel.TabIndex = 8;
            // 
            // SelectionComboPanel
            // 
            this.SelectionComboPanel.Controls.Add(this.SelectionCombo);
            this.SelectionComboPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SelectionComboPanel.Location = new System.Drawing.Point(130, 3);
            this.SelectionComboPanel.Name = "SelectionComboPanel";
            this.SelectionComboPanel.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.SelectionComboPanel.Size = new System.Drawing.Size(502, 30);
            this.SelectionComboPanel.TabIndex = 9;
            // 
            // SelectionCombo
            // 
            this.SelectionCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SelectionCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectionCombo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectionCombo.FormattingEnabled = true;
            this.SelectionCombo.Location = new System.Drawing.Point(1, 0);
            this.SelectionCombo.Name = "SelectionCombo";
            this.SelectionCombo.Size = new System.Drawing.Size(500, 28);
            this.SelectionCombo.TabIndex = 0;
            // 
            // SelectionLabel
            // 
            this.SelectionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SelectionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectionLabel.Location = new System.Drawing.Point(0, 0);
            this.SelectionLabel.Name = "SelectionLabel";
            this.SelectionLabel.Size = new System.Drawing.Size(121, 30);
            this.SelectionLabel.TabIndex = 0;
            this.SelectionLabel.Text = "Server";
            this.SelectionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DayZServerMonitorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 203);
            this.Controls.Add(this.FormPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(653, 250);
            this.MinimumSize = new System.Drawing.Size(653, 250);
            this.Name = "DayZServerMonitorForm";
            this.Text = "DayZ Server Monitor";
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
            this.PlayersValuePanel.PerformLayout();
            this.MaxPlayersLabelPanel.ResumeLayout(false);
            this.MaxPlayersValuePanel.ResumeLayout(false);
            this.MaxPlayersValuePanel.PerformLayout();
            this.MonitorStatusStrip.ResumeLayout(false);
            this.MonitorStatusStrip.PerformLayout();
            this.SelectionLabelPanel.ResumeLayout(false);
            this.SelectionComboPanel.ResumeLayout(false);
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
        private System.Windows.Forms.TextBox PlayersValue;
        private System.Windows.Forms.Panel MaxPlayersLabelPanel;
        private System.Windows.Forms.Label MaxPlayersLabel;
        private System.Windows.Forms.Panel MaxPlayersValuePanel;
        private System.Windows.Forms.TextBox MaxPlayersValue;
        private System.Windows.Forms.StatusStrip MonitorStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel MonitorStatus;
        private System.Windows.Forms.Panel SelectionLabelPanel;
        private System.Windows.Forms.Panel SelectionComboPanel;
        private System.Windows.Forms.ComboBox SelectionCombo;
        private System.Windows.Forms.Label SelectionLabel;
    }
}