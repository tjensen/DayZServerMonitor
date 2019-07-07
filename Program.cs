using DayZServerMonitorCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZServerMonitor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (DayZServerMonitorForm form = new DayZServerMonitorForm())
            {
                form.Initialize();
                Application.Run(form);
            }
        }
    }
}
