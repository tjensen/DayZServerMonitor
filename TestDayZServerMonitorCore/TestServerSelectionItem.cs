using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestDayZServerMonitorCore
{
    [TestClass]
    public class TestServerSelectionItem
    {
        [TestMethod]
        public void DisplayNameContainsServerSourceDisplayName()
        {
            ServerSelectionItem item = new ServerSelectionItem(new SavedServerSource(new Server("4.3.2.1", 54321)));

            Assert.AreEqual("4.3.2.1:54321", item.DisplayName);
        }

        [TestMethod]
        public void ValueContainsSelf()
        {
            ServerSelectionItem item = new ServerSelectionItem(new SavedServerSource(new Server("4.3.2.1", 54321)));

            Assert.AreSame(item, item.Value);
        }

        [TestMethod]
        public void GetSourceReturnsServerSource()
        {
            IServerSource source = new SavedServerSource(new Server("4.3.2.1", 54321));
            ServerSelectionItem item = new ServerSelectionItem(source);

            Assert.AreSame(source, item.GetSource());
        }
    }
}
