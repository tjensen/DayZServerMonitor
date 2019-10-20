using DayZServerMonitorCore;
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
           using (NewDayZServerMonitorForm form = new NewDayZServerMonitorForm(
               Assembly.GetExecutingAssembly().GetName().Version.ToString()))
            {
                Application.Run(form);
            }
        }
    }
}
