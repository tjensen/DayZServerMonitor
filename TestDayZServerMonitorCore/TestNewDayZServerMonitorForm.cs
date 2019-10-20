using Microsoft.VisualStudio.TestTools.UnitTesting;
using DayZServerMonitorCore;
using System.Windows.Forms;

namespace TestDayZServerMonitorCore
{
    [TestClass]
    public class TestNewDayZServerMonitorForm
    {
        [TestMethod]
        public void DisplaysServerMonitorForm()
        {
            using (NewDayZServerMonitorForm form = new NewDayZServerMonitorForm("1.2.3.4567"))
            {
                Assert.AreEqual("DayZ Server Monitor v1.2.3.4567", form.Text);
                Assert.AreEqual("DayZServerMonitorForm", form.Name);
                Assert.IsFalse(form.MaximizeBox);
                Assert.AreEqual(FormBorderStyle.FixedDialog, form.FormBorderStyle);
                Assert.AreEqual(AutoScaleMode.Font, form.AutoScaleMode);
            }
        }
    }
}
