using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;

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
            Assert.AreEqual(10, settings.PlayerCountThreshold);
            Assert.AreEqual(Settings.NotifyOnPlayerCountValues.NEVER, settings.NotifyOnPlayerCount);
            Assert.AreEqual(Color.Black, settings.TrayIconBackground);
            Assert.AreEqual(Color.White, settings.TrayIconForeground);
        }

        [TestMethod]
        public void ApplyAppliesNewSettings()
        {
            Settings newSettings = new Settings()
            {
                HideTaskBarIcon = Settings.HideTaskBarIconValues.ALWAYS,
                MaxLogViewerEntries = 42,
                LogPathname = "/path/to/log",
                AlwaysOnTop = true,
                PlayerCountThreshold = 37,
                NotifyOnPlayerCount = Settings.NotifyOnPlayerCountValues.WHEN_ABOVE,
                TrayIconBackground = Color.Green,
                TrayIconForeground = Color.Blue
            };

            settings.Apply(newSettings);

            Assert.AreEqual(Settings.HideTaskBarIconValues.ALWAYS, settings.HideTaskBarIcon);
            Assert.AreEqual(42, settings.MaxLogViewerEntries);
            Assert.AreEqual("/path/to/log", settings.LogPathname);
            Assert.IsTrue(settings.AlwaysOnTop);
            Assert.AreEqual(37, settings.PlayerCountThreshold);
            Assert.AreEqual(Settings.NotifyOnPlayerCountValues.WHEN_ABOVE, settings.NotifyOnPlayerCount);
            Assert.AreEqual(Color.Green, settings.TrayIconBackground);
            Assert.AreEqual(Color.Blue, settings.TrayIconForeground);
        }

        [TestMethod]
        public void SettinsgAreSerializable()
        {
            Settings settingsToSave = new Settings()
            {
                HideTaskBarIcon = Settings.HideTaskBarIconValues.ALWAYS,
                MaxLogViewerEntries = 42,
                LogPathname = "/path/to/log",
                AlwaysOnTop = true,
                PlayerCountThreshold = 55,
                NotifyOnPlayerCount = Settings.NotifyOnPlayerCountValues.WHEN_BELOW,
                TrayIconBackground = Color.Green,
                TrayIconForeground = Color.Blue
            };

            XmlSerializer serializer = new XmlSerializer(typeof(Settings));

            MemoryStream writeStream = new MemoryStream();
            using (TextWriter writer = new StreamWriter(writeStream))
            {
                serializer.Serialize(writer, settingsToSave);
            }

            MemoryStream readStream = new MemoryStream(writeStream.GetBuffer());
            Settings settingsToLoad = new Settings();
            using (TextReader reader = new StreamReader(readStream))
            {
                settingsToLoad.Apply((Settings)serializer.Deserialize(reader));
            }

            Assert.AreEqual(settingsToSave.HideTaskBarIcon, settingsToLoad.HideTaskBarIcon);
            Assert.AreEqual(settingsToSave.MaxLogViewerEntries, settingsToLoad.MaxLogViewerEntries);
            Assert.AreEqual(settingsToSave.LogPathname, settingsToLoad.LogPathname);
            Assert.AreEqual(settingsToSave.AlwaysOnTop, settingsToLoad.AlwaysOnTop);
            Assert.AreEqual(55, settingsToLoad.PlayerCountThreshold);
            Assert.AreEqual(Settings.NotifyOnPlayerCountValues.WHEN_BELOW, settingsToLoad.NotifyOnPlayerCount);
            Assert.AreEqual(settingsToSave.TrayIconBackground.ToArgb(), settingsToLoad.TrayIconBackground.ToArgb());
            Assert.AreEqual(settingsToSave.TrayIconForeground.ToArgb(), settingsToLoad.TrayIconForeground.ToArgb());
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
        public void SettingChangedIsInvokedWhenLogPathnameIsChanged()
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

        [TestMethod]
        public void SettingsChangedIsInvokedWhenPlayerCountThresholdIsChanged()
        {
            settings.PlayerCountThreshold = 23;

            Assert.AreEqual("PlayerCountThreshold", settingThatChanged);
        }

        [TestMethod]
        public void SettingsChangedIsInvokedWhenNotifyOnPlayerCountIsChanged()
        {
            settings.NotifyOnPlayerCount = Settings.NotifyOnPlayerCountValues.WHEN_ABOVE;

            Assert.AreEqual("NotifyOnPlayerCount", settingThatChanged);
        }

        [TestMethod]
        public void SettingChangedIsInvokedWhenTrayIconBackgroundIsChanged()
        {
            settings.TrayIconBackground = Color.Red;

            Assert.AreEqual("TrayIconBackground", settingThatChanged);
        }

        [TestMethod]
        public void SettingChangedIsInvokedWhenTrayIconForegroundIsChanged()
        {
            settings.TrayIconForeground = Color.Magenta;

            Assert.AreEqual("TrayIconForeground", settingThatChanged);
        }
    }
}
