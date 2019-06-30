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
            this.FormPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
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
            this.FormPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.ServerLabelPanel.SuspendLayout();
            this.ServerValuePanel.SuspendLayout();
            this.NameLabelPanel.SuspendLayout();
            this.NameValuePanel.SuspendLayout();
            this.PlayersLabelPanel.SuspendLayout();
            this.PlayersValuePanel.SuspendLayout();
            this.MaxPlayersLabelPanel.SuspendLayout();
            this.MaxPlayersValuePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // FormPanel
            // 
            this.FormPanel.Controls.Add(this.tableLayoutPanel1);
            this.FormPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FormPanel.Location = new System.Drawing.Point(0, 0);
            this.FormPanel.Name = "FormPanel";
            this.FormPanel.Size = new System.Drawing.Size(365, 145);
            this.FormPanel.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.67123F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 72.32877F));
            this.tableLayoutPanel1.Controls.Add(this.ServerLabelPanel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ServerValuePanel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.NameLabelPanel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.NameValuePanel, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.PlayersLabelPanel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.PlayersValuePanel, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.MaxPlayersLabelPanel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.MaxPlayersValuePanel, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(365, 145);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // ServerLabelPanel
            // 
            this.ServerLabelPanel.Controls.Add(this.ServerLabel);
            this.ServerLabelPanel.Location = new System.Drawing.Point(3, 3);
            this.ServerLabelPanel.Name = "ServerLabelPanel";
            this.ServerLabelPanel.Size = new System.Drawing.Size(94, 30);
            this.ServerLabelPanel.TabIndex = 0;
            // 
            // ServerLabel
            // 
            this.ServerLabel.AutoSize = true;
            this.ServerLabel.Location = new System.Drawing.Point(0, 0);
            this.ServerLabel.Name = "ServerLabel";
            this.ServerLabel.Size = new System.Drawing.Size(55, 20);
            this.ServerLabel.TabIndex = 0;
            this.ServerLabel.Text = "Server";
            // 
            // ServerValuePanel
            // 
            this.ServerValuePanel.Controls.Add(this.ServerValue);
            this.ServerValuePanel.Location = new System.Drawing.Point(103, 3);
            this.ServerValuePanel.Name = "ServerValuePanel";
            this.ServerValuePanel.Size = new System.Drawing.Size(259, 30);
            this.ServerValuePanel.TabIndex = 1;
            // 
            // ServerValue
            // 
            this.ServerValue.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ServerValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ServerValue.Enabled = false;
            this.ServerValue.Location = new System.Drawing.Point(0, 0);
            this.ServerValue.Name = "ServerValue";
            this.ServerValue.ReadOnly = true;
            this.ServerValue.Size = new System.Drawing.Size(259, 26);
            this.ServerValue.TabIndex = 0;
            // 
            // NameLabelPanel
            // 
            this.NameLabelPanel.Controls.Add(this.NameLabel);
            this.NameLabelPanel.Location = new System.Drawing.Point(3, 39);
            this.NameLabelPanel.Name = "NameLabelPanel";
            this.NameLabelPanel.Size = new System.Drawing.Size(94, 30);
            this.NameLabelPanel.TabIndex = 2;
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(0, 0);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(51, 20);
            this.NameLabel.TabIndex = 0;
            this.NameLabel.Text = "Name";
            // 
            // NameValuePanel
            // 
            this.NameValuePanel.Controls.Add(this.NameValue);
            this.NameValuePanel.Location = new System.Drawing.Point(103, 39);
            this.NameValuePanel.Name = "NameValuePanel";
            this.NameValuePanel.Size = new System.Drawing.Size(259, 30);
            this.NameValuePanel.TabIndex = 3;
            // 
            // NameValue
            // 
            this.NameValue.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.NameValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NameValue.Enabled = false;
            this.NameValue.Location = new System.Drawing.Point(0, 0);
            this.NameValue.Name = "NameValue";
            this.NameValue.ReadOnly = true;
            this.NameValue.Size = new System.Drawing.Size(259, 26);
            this.NameValue.TabIndex = 0;
            // 
            // PlayersLabelPanel
            // 
            this.PlayersLabelPanel.Controls.Add(this.PlayersLabel);
            this.PlayersLabelPanel.Location = new System.Drawing.Point(3, 75);
            this.PlayersLabelPanel.Name = "PlayersLabelPanel";
            this.PlayersLabelPanel.Size = new System.Drawing.Size(94, 30);
            this.PlayersLabelPanel.TabIndex = 4;
            // 
            // PlayersLabel
            // 
            this.PlayersLabel.AutoSize = true;
            this.PlayersLabel.Location = new System.Drawing.Point(0, 0);
            this.PlayersLabel.Name = "PlayersLabel";
            this.PlayersLabel.Size = new System.Drawing.Size(60, 20);
            this.PlayersLabel.TabIndex = 0;
            this.PlayersLabel.Text = "Players";
            // 
            // PlayersValuePanel
            // 
            this.PlayersValuePanel.Controls.Add(this.PlayersValue);
            this.PlayersValuePanel.Location = new System.Drawing.Point(103, 75);
            this.PlayersValuePanel.Name = "PlayersValuePanel";
            this.PlayersValuePanel.Size = new System.Drawing.Size(259, 30);
            this.PlayersValuePanel.TabIndex = 5;
            // 
            // PlayersValue
            // 
            this.PlayersValue.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.PlayersValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PlayersValue.Enabled = false;
            this.PlayersValue.Location = new System.Drawing.Point(0, 0);
            this.PlayersValue.Name = "PlayersValue";
            this.PlayersValue.ReadOnly = true;
            this.PlayersValue.Size = new System.Drawing.Size(259, 26);
            this.PlayersValue.TabIndex = 0;
            // 
            // MaxPlayersLabelPanel
            // 
            this.MaxPlayersLabelPanel.Controls.Add(this.MaxPlayersLabel);
            this.MaxPlayersLabelPanel.Location = new System.Drawing.Point(3, 111);
            this.MaxPlayersLabelPanel.Name = "MaxPlayersLabelPanel";
            this.MaxPlayersLabelPanel.Size = new System.Drawing.Size(94, 31);
            this.MaxPlayersLabelPanel.TabIndex = 6;
            // 
            // MaxPlayersLabel
            // 
            this.MaxPlayersLabel.AutoSize = true;
            this.MaxPlayersLabel.Location = new System.Drawing.Point(0, 0);
            this.MaxPlayersLabel.Name = "MaxPlayersLabel";
            this.MaxPlayersLabel.Size = new System.Drawing.Size(93, 20);
            this.MaxPlayersLabel.TabIndex = 0;
            this.MaxPlayersLabel.Text = "Max Players";
            // 
            // MaxPlayersValuePanel
            // 
            this.MaxPlayersValuePanel.Controls.Add(this.MaxPlayersValue);
            this.MaxPlayersValuePanel.Location = new System.Drawing.Point(103, 111);
            this.MaxPlayersValuePanel.Name = "MaxPlayersValuePanel";
            this.MaxPlayersValuePanel.Size = new System.Drawing.Size(259, 31);
            this.MaxPlayersValuePanel.TabIndex = 7;
            // 
            // MaxPlayersValue
            // 
            this.MaxPlayersValue.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.MaxPlayersValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MaxPlayersValue.Enabled = false;
            this.MaxPlayersValue.Location = new System.Drawing.Point(0, 0);
            this.MaxPlayersValue.Name = "MaxPlayersValue";
            this.MaxPlayersValue.ReadOnly = true;
            this.MaxPlayersValue.Size = new System.Drawing.Size(259, 26);
            this.MaxPlayersValue.TabIndex = 0;
            // 
            // DayZServerMonitorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 145);
            this.Controls.Add(this.FormPanel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(381, 184);
            this.MinimumSize = new System.Drawing.Size(381, 184);
            this.Name = "DayZServerMonitorForm";
            this.ShowIcon = false;
            this.Text = "DayZ Server Monitor";
            this.FormPanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ServerLabelPanel.ResumeLayout(false);
            this.ServerLabelPanel.PerformLayout();
            this.ServerValuePanel.ResumeLayout(false);
            this.ServerValuePanel.PerformLayout();
            this.NameLabelPanel.ResumeLayout(false);
            this.NameLabelPanel.PerformLayout();
            this.NameValuePanel.ResumeLayout(false);
            this.NameValuePanel.PerformLayout();
            this.PlayersLabelPanel.ResumeLayout(false);
            this.PlayersLabelPanel.PerformLayout();
            this.PlayersValuePanel.ResumeLayout(false);
            this.PlayersValuePanel.PerformLayout();
            this.MaxPlayersLabelPanel.ResumeLayout(false);
            this.MaxPlayersLabelPanel.PerformLayout();
            this.MaxPlayersValuePanel.ResumeLayout(false);
            this.MaxPlayersValuePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel FormPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel ServerLabelPanel;
        private System.Windows.Forms.Label ServerLabel;
        private System.Windows.Forms.Panel ServerValuePanel;
        private System.Windows.Forms.Panel NameLabelPanel;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.Panel NameValuePanel;
        private System.Windows.Forms.Panel PlayersLabelPanel;
        private System.Windows.Forms.Panel PlayersValuePanel;
        private System.Windows.Forms.Panel MaxPlayersLabelPanel;
        private System.Windows.Forms.Panel MaxPlayersValuePanel;
        private System.Windows.Forms.Label PlayersLabel;
        private System.Windows.Forms.Label MaxPlayersLabel;
        private System.Windows.Forms.TextBox ServerValue;
        private System.Windows.Forms.TextBox NameValue;
        private System.Windows.Forms.TextBox PlayersValue;
        private System.Windows.Forms.TextBox MaxPlayersValue;
    }
}

