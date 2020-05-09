﻿using System;
using System.Runtime.CompilerServices;

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

        private HideTaskBarIconValues hideTaskBarIcon = HideTaskBarIconValues.NEVER;
        private int maxLogViewerEntries = 100;
        private string logPathname = null;
        private bool alwaysOnTop = false;

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

        public void Apply(Settings newSettings)
        {
            HideTaskBarIcon = newSettings.HideTaskBarIcon;
            MaxLogViewerEntries = newSettings.MaxLogViewerEntries;
            LogPathname = newSettings.LogPathname;
            AlwaysOnTop = newSettings.AlwaysOnTop;
        }
    }
}