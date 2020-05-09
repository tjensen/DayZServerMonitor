using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestDayZServerMonitorCore
{
    [TestClass]
    public class TestSettings
    {
        private string settingThatChanged;
        private Settings settings;

        [TestInitialize]
        public void Initialize()
        {
            settingThatChanged = null;
            settings = new Settings();
            settings.SettingChanged += (source, args) => { settingThatChanged = args.SettingName; };
        }

        [TestMethod]
        public void IsConstructedWithDefaultSettings()
        {
            Assert.AreEqual(Settings.HideTaskBarIconValues.NEVER, settings.HideTaskBarIcon);
            Assert.AreEqual(100, settings.MaxLogViewerEntries);
            Assert.IsNull(settings.LogPathname);
            Assert.IsFalse(settings.AlwaysOnTop);
        }

        [TestMethod]
        public void ApplyAppliesNewSettings()
        {
            Settings newSettings = new Settings()
            {
                HideTaskBarIcon = Settings.HideTaskBarIconValues.ALWAYS,
                MaxLogViewerEntries = 42,
                LogPathname = "/path/to/log",
                AlwaysOnTop = true
            };

            settings.Apply(newSettings);

            Assert.AreEqual(Settings.HideTaskBarIconValues.ALWAYS, settings.HideTaskBarIcon);
            Assert.AreEqual(42, settings.MaxLogViewerEntries);
            Assert.AreEqual("/path/to/log", settings.LogPathname);
            Assert.IsTrue(settings.AlwaysOnTop);
        }

        [TestMethod]
        public void SettingChangedIsInvokedWhenHideTaskBarIconChanges()
        {
            settings.HideTaskBarIcon = Settings.HideTaskBarIconValues.ALWAYS;

            Assert.AreEqual("HideTaskBarIcon", settingThatChanged);
        }

        [TestMethod]
        public void SettingChangedIsInvokedWhenMaxLogViewerEntriesIsChanged()
        {
            settings.MaxLogViewerEntries = 42;

            Assert.AreEqual("MaxLogViewerEntries", settingThatChanged);
        }

        [TestMethod]
        public void SettingChangedIsInvokedWhenLogPathnameisChanged()
        {
            settings.LogPathname = "LOGFILE";

            Assert.AreEqual("LogPathname", settingThatChanged);
        }

        [TestMethod]
        public void SettingChangedIsInvokedWhenAlwaysOnTopIsChanged()
        {
            settings.AlwaysOnTop = true;

            Assert.AreEqual("AlwaysOnTop", settingThatChanged);
        }
    }
}
