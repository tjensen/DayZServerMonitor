using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace DayZServerMonitor
{
    public partial class DayZServerMonitorForm : Form
    {
        private System.Timers.Timer timer;
        private FileSystemWatcher watcher;
        private Monitor monitor;

        public DayZServerMonitorForm()
        {
            InitializeComponent();
        }

        internal void updateValues(string server)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { updateValues(server); }));
            }
            else
            {
                this.ServerValue.Text = server;
            }
        }

        internal void Initialize(Monitor monitor)
        {
            this.monitor = monitor;
            watcher = monitor.CreateDayZProfileWatcher(this, Poll);
            timer = monitor.CreateTimer(this, Poll);
            Poll();
        }

        private async Task PollAsync()
        {
            await this.monitor.Poll(this);
        }
        
        private void Poll()
        {
            _ = Task.Run(async delegate { await PollAsync(); });
        }
    }
}
