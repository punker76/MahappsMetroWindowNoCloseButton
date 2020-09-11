using System;

namespace WpfFramework.Utilities
{
    public class EventSystemRedirectSettingsArgs : EventArgs
    {
        public SettingsViewName Setting { get; set; }
        public string Args { get; set; }

        public EventSystemRedirectSettingsArgs(SettingsViewName name)
        {
            Setting = name;
            //Args = args;
        }
    }
}
