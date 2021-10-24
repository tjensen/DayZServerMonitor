using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace DayZServerMonitorCore
{
    public class SettingChangedArgs : EventArgs
    {
        public SettingChangedArgs(string settingName)
        {
            SettingName = settingName;
        }

        public string SettingName { get; set; }
    }

    public class StatusFileSetting
    {
        public const string EXAMPLE_CONTENT = "Name: %N\r\nAddress: %A\r\nNum Players: %P\r\nMax Players: %M";

        public string Pathname;
        public string Content = "";

        public bool Enabled
        {
            get => Pathname != null;
        }
    }

    public class Settings
    {
        public event EventHandler<SettingChangedArgs> SettingChanged;

        protected virtual void OnSettingChanged([CallerMemberName] string settingName = null)
        {
            SettingChanged?.Invoke(this, new SettingChangedArgs(settingName));
        }

        public enum HideTaskBarIconValues { NEVER, WHEN_MINIMIZED, ALWAYS };
        public enum NotifyOnPlayerCountValues { NEVER, WHEN_ABOVE, WHEN_BELOW };
        public const int NUM_STATUS_FILES = 4;

        private bool checkForUpdates = true;
        private HideTaskBarIconValues hideTaskBarIcon = HideTaskBarIconValues.NEVER;
        private int maxLogViewerEntries = 100;
        private string logPathname = null;
        private bool alwaysOnTop = false;
        private int playerCountThreshold = 10;
        private NotifyOnPlayerCountValues notifyOnPlayerCount = NotifyOnPlayerCountValues.NEVER;
        private StatusFileSetting[] statusFile = {
            new StatusFileSetting{ Content = StatusFileSetting.EXAMPLE_CONTENT },
            new StatusFileSetting(),
            new StatusFileSetting(),
            new StatusFileSetting()
        };
        private Color trayIconBackground = Color.Black;
        private Color trayIconForeground = Color.White;
        private Color trayIconAlertBackground = Color.DarkRed;
        private Color trayIconAlertForeground = Color.Yellow;

        public bool CheckForUpdates
        {
            get => checkForUpdates;
            set
            {
                checkForUpdates = value;
                OnSettingChanged();
            }
        }

        public HideTaskBarIconValues HideTaskBarIcon
        {
            get => hideTaskBarIcon;
            set
            {
                hideTaskBarIcon = value;
                OnSettingChanged();
            }
        }

        public int MaxLogViewerEntries
        {
            get => maxLogViewerEntries;
            set
            {
                maxLogViewerEntries = value;
                OnSettingChanged();
            }
        }

        public string LogPathname
        {
            get => logPathname;
            set
            {
                logPathname = value;
                OnSettingChanged();
            }
        }

        public bool AlwaysOnTop
        {
            get => alwaysOnTop;
            set
            {
                alwaysOnTop = value;
                OnSettingChanged();
            }
        }

        public int PlayerCountThreshold
        {
            get => playerCountThreshold;
            set
            {
                playerCountThreshold = value;
                OnSettingChanged();
            }
        }

        public NotifyOnPlayerCountValues NotifyOnPlayerCount
        {
            get => notifyOnPlayerCount;
            set
            {
                notifyOnPlayerCount = value;
                OnSettingChanged();
            }
        }

        public StatusFileSetting[] StatusFile
        {
            get => statusFile;
            set
            {
                statusFile = value;
                OnSettingChanged();
            }
        }

        public void SetStatusFile(int index, StatusFileSetting newStatusFile)
        {
            statusFile[index] = newStatusFile;
            OnSettingChanged("StatusFile");
        }

        [XmlIgnore]
        public Color TrayIconBackground
        {
            get => trayIconBackground;
            set
            {
                trayIconBackground = value;
                OnSettingChanged();
            }
        }

        [XmlIgnore]
        public Color TrayIconForeground
        {
            get => trayIconForeground;
            set
            {
                trayIconForeground = value;
                OnSettingChanged();
            }
        }

        public int TrayIconBackgroundARGB
        {
            get => TrayIconBackground.ToArgb();
            set => TrayIconBackground = Color.FromArgb(value);
        }

        public int TrayIconForegroundARGB
        {
            get => TrayIconForeground.ToArgb();
            set => TrayIconForeground = Color.FromArgb(value);
        }

        [XmlIgnore]
        public Color TrayIconAlertBackground
        {
            get => trayIconAlertBackground;
            set
            {
                trayIconAlertBackground = value;
                OnSettingChanged();
            }
        }

        [XmlIgnore]
        public Color TrayIconAlertForeground
        {
            get => trayIconAlertForeground;
            set
            {
                trayIconAlertForeground = value;
                OnSettingChanged();
            }
        }

        public int TrayIconAlertBackgroundARGB
        {
            get => TrayIconAlertBackground.ToArgb();
            set => TrayIconAlertBackground = Color.FromArgb(value);
        }

        public int TrayIconAlertForegroundARGB
        {
            get => TrayIconAlertForeground.ToArgb();
            set => TrayIconAlertForeground = Color.FromArgb(value);
        }

        public void Apply(Settings newSettings)
        {
            CheckForUpdates = newSettings.CheckForUpdates;
            HideTaskBarIcon = newSettings.HideTaskBarIcon;
            MaxLogViewerEntries = newSettings.MaxLogViewerEntries;
            LogPathname = newSettings.LogPathname;
            AlwaysOnTop = newSettings.AlwaysOnTop;
            PlayerCountThreshold = newSettings.PlayerCountThreshold;
            NotifyOnPlayerCount = newSettings.NotifyOnPlayerCount;
            StatusFile = newSettings.StatusFile;
            TrayIconBackground = newSettings.TrayIconBackground;
            TrayIconForeground = newSettings.TrayIconForeground;
            TrayIconAlertBackground = newSettings.TrayIconAlertBackground;
            TrayIconAlertForeground = newSettings.TrayIconAlertForeground;
        }
    }
}
