using System;
using System.Reflection;
using System.Windows.Forms;

namespace DayZServerMonitor
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (DayZServerMonitorForm form = new DayZServerMonitorForm())
            {
                form.Text += " v";
                form.Text += Assembly.GetExecutingAssembly().GetName().Version.ToString();
                form.Initialize();
                Application.Run(form);
            }
        }
    }
}
