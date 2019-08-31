using System.Windows.Forms;

namespace DayZServerMonitor
{
    public partial class AddServerDialog : Form
    {
        public AddServerDialog()
        {
            InitializeComponent();
            ipAddress.KeyPress += IPAddress_KeyPress;
        }

        public string IPAddress => ipAddress.Text;

        public int Port => int.Parse(port.Text);

        private void IPAddress_KeyPress(object sender, KeyPressEventArgs args)
        {
            if (!char.IsControl(args.KeyChar)
                && !char.IsDigit(args.KeyChar)
                && args.KeyChar != '.')
            {
                args.Handled = true;
            }
        }
    }
}
