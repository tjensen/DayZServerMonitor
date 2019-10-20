using System.Drawing;
using System.Windows.Forms;

namespace DayZServerMonitorCore
{
    public class NewDayZServerMonitorForm : Form
    {
        public NewDayZServerMonitorForm(string version)
        {
            SuspendLayout();
            Text = $"DayZ Server Monitor v{version}";
            Name = "DayZServerMonitorForm";
            Size = new Size(653, 250);
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4);
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            Icon = Properties.Resources.DayZServerMonitorIcon;
            ResumeLayout(false);
        }
    }
}
