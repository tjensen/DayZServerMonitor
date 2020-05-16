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

    public class Settings
    {
        public event EventHandler<SettingChangedArgs> SettingChanged;

        protected virtual void OnSettingChanged([CallerMemberName] string settingName = null)
        {
            SettingChanged?.Invoke(this, new SettingChangedArgs(settingName));
        }

        public enum HideTaskBarIconValues { NEVER, WHEN_MINIMIZED, ALWAYS };
        public enum NotifyOnPlayerCountValues { NEVER, WHEN_ABOVE, WHEN_BELOW };

        private HideTaskBarIconValues hideTaskBarIcon = HideTaskBarIconValues.NEVER;
        private int maxLogViewerEntries = 100;
        private string logPathname = null;
        private bool alwaysOnTop = false;
        private int playerCountThreshold = 10;
        private NotifyOnPlayerCountValues notifyOnPlayerCount = NotifyOnPlayerCountValues.NEVER;
        private Color trayIconBackground = Color.Black;
        private Color trayIconForeground = Color.White;

        public HideTaskBarIconValues HideTaskBarIcon
        {
            get => hideTaskBarIcon;
            set {
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

        public void Apply(Settings newSettings)
        {
            HideTaskBarIcon = newSettings.HideTaskBarIcon;
            MaxLogViewerEntries = newSettings.MaxLogViewerEntries;
            LogPathname = newSettings.LogPathname;
            AlwaysOnTop = newSettings.AlwaysOnTop;
            PlayerCountThreshold = newSettings.PlayerCountThreshold;
            NotifyOnPlayerCount = newSettings.NotifyOnPlayerCount;
            TrayIconBackground = newSettings.TrayIconBackground;
            TrayIconForeground = newSettings.TrayIconForeground;
        }
    }
}
