using DayZServerMonitorCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
            for (int i = 0; i < Settings.NUM_STATUS_FILES; i++)
            {
                StatusFileSetting statusFile = settings.StatusFile[i];
                Assert.IsFalse(statusFile.Enabled);
                Assert.IsNull(statusFile.Pathname);
                if (i == 0)
                {
                    Assert.AreEqual(StatusFileSetting.EXAMPLE_CONTENT, statusFile.Content);
                }
                else
                {
                    Assert.AreEqual("", statusFile.Content);
                }
            }
            Assert.AreEqual(Color.Black, settings.TrayIconBackground);
            Assert.AreEqual(Color.White, settings.TrayIconForeground);
            Assert.AreEqual(Color.DarkRed, settings.TrayIconAlertBackground);
            Assert.AreEqual(Color.Yellow, settings.TrayIconAlertForeground);
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
                StatusFile = new StatusFileSetting[] {
                    new StatusFileSetting{Pathname = $"new filename 0", Content = $"new content 0" },
                    new StatusFileSetting{Pathname = $"new filename 1", Content = $"new content 1" },
                    new StatusFileSetting{Pathname = $"new filename 2", Content = $"new content 2" },
                    new StatusFileSetting{Pathname = $"new filename 3", Content = $"new content 3" }
                },
                TrayIconBackground = Color.Green,
                TrayIconForeground = Color.Blue,
                TrayIconAlertBackground = Color.Yellow,
                TrayIconAlertForeground = Color.Maroon
            };

            settings.Apply(newSettings);

            Assert.AreEqual(Settings.HideTaskBarIconValues.ALWAYS, settings.HideTaskBarIcon);
            Assert.AreEqual(42, settings.MaxLogViewerEntries);
            Assert.AreEqual("/path/to/log", settings.LogPathname);
            Assert.IsTrue(settings.AlwaysOnTop);
            Assert.AreEqual(37, settings.PlayerCountThreshold);
            Assert.AreEqual(Settings.NotifyOnPlayerCountValues.WHEN_ABOVE, settings.NotifyOnPlayerCount);
            for (int i = 0; i < Settings.NUM_STATUS_FILES; i++)
            {
                Assert.AreEqual(newSettings.StatusFile[i].Enabled, settings.StatusFile[i].Enabled);
                Assert.AreEqual(newSettings.StatusFile[i].Pathname, settings.StatusFile[i].Pathname);
                Assert.AreEqual(newSettings.StatusFile[i].Content, settings.StatusFile[i].Content);
            }
            Assert.AreEqual(Color.Green, settings.TrayIconBackground);
            Assert.AreEqual(Color.Blue, settings.TrayIconForeground);
            Assert.AreEqual(Color.Yellow, settings.TrayIconAlertBackground);
            Assert.AreEqual(Color.Maroon, settings.TrayIconAlertForeground);
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
                StatusFile = new StatusFileSetting[] {
                    new StatusFileSetting{Pathname = $"new filename 0", Content = $"new content 0" },
                    new StatusFileSetting{Pathname = $"new filename 1", Content = $"new content 1" },
                    new StatusFileSetting{Pathname = $"new filename 2", Content = $"new content 2" },
                    new StatusFileSetting{Pathname = $"new filename 3", Content = $"new content 3" }
                },
                TrayIconBackground = Color.Green,
                TrayIconForeground = Color.Blue,
                TrayIconAlertBackground = Color.Yellow,
                TrayIconAlertForeground = Color.Maroon
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
            Assert.AreEqual(settingsToSave.MaxLogViewerEntries,
                settingsToLoad.MaxLogViewerEntries);
            Assert.AreEqual(settingsToSave.LogPathname, settingsToLoad.LogPathname);
            Assert.AreEqual(settingsToSave.AlwaysOnTop, settingsToLoad.AlwaysOnTop);
            Assert.AreEqual(55, settingsToLoad.PlayerCountThreshold);
            Assert.AreEqual(Settings.NotifyOnPlayerCountValues.WHEN_BELOW,
                settingsToLoad.NotifyOnPlayerCount);
            for (int i = 0; i < Settings.NUM_STATUS_FILES; i++)
            {
                Assert.AreEqual(
                    settingsToSave.StatusFile[i].Enabled,
                    settingsToLoad.StatusFile[i].Enabled);
                Assert.AreEqual(
                    settingsToSave.StatusFile[i].Pathname,
                    settingsToLoad.StatusFile[i].Pathname);
                Assert.AreEqual(
                    settingsToSave.StatusFile[i].Content,
                    settingsToLoad.StatusFile[i].Content);
            }
            Assert.AreEqual(settingsToSave.TrayIconBackground.ToArgb(),
                settingsToLoad.TrayIconBackground.ToArgb());
            Assert.AreEqual(settingsToSave.TrayIconForeground.ToArgb(),
                settingsToLoad.TrayIconForeground.ToArgb());
            Assert.AreEqual(settingsToSave.TrayIconAlertBackground.ToArgb(),
                settingsToLoad.TrayIconAlertBackground.ToArgb());
            Assert.AreEqual(settingsToSave.TrayIconAlertForeground.ToArgb(),
                settingsToLoad.TrayIconAlertForeground.ToArgb());
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
        public void SettingsChangedIsInvokedWhenEnableStatusFileIsChanged()
        {
            settings.StatusFile = new StatusFileSetting[] {
                new StatusFileSetting{Pathname = "filename",  Content = "content"},
                new StatusFileSetting{Pathname = "filename",  Content = "content"},
                new StatusFileSetting{Pathname = "filename",  Content = "content"},
                new StatusFileSetting{Pathname = "filename",  Content = "content"}
            };

            Assert.AreEqual("StatusFile", settingThatChanged);
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

        [TestMethod]
        public void SettingChangedIsInvokedWhenTrayIconAlertBackgroundIsChanged()
        {
            settings.TrayIconAlertBackground = Color.Yellow;

            Assert.AreEqual("TrayIconAlertBackground", settingThatChanged);
        }

        [TestMethod]
        public void SettingChangedIsInvokedWhenTrayIconAlertForegroundIsChanged()
        {
            settings.TrayIconAlertForeground = Color.Maroon;

            Assert.AreEqual("TrayIconAlertForeground", settingThatChanged);
        }
    }
}
