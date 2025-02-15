﻿using WpfFramework.Models.Settings;
using WpfFramework.Properties;
using WpfFramework.Utilities;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Markup;
using System.Globalization;

namespace WpfFramework
{
    public partial class App
    {
        // Single instance identifier
        private const string GUID = "6A3F34B2-161F-4F70-A8BC-A19C40F79CFD";
        private string SyncfusionKey = "";
        private Mutex _mutex;
        private DispatcherTimer _dispatcherTimer;

        private bool _singleInstanceClose;

        protected override void OnStartup(StartupEventArgs e)
        {
            /*
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(
                    CultureInfo.CurrentCulture.IetfLanguageTag)));
                    */
            base.OnStartup(e);
        }

        public App()
        {
            ShutdownMode = ShutdownMode.OnLastWindowClose;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Detect the current configuration
            //Models.Settings.ConfigurationManager.Detect();

            // Detect the current configuration
            Models.Settings.ConfigurationManager.Detect();

            // Get assembly informations   
            AssemblyManager.Load();

            // Load application settings (profiles/Profiles/clients are loaded when needed)
            try
            {
                // Update integrated settings %LocalAppData%\{AssemblyName} (custom settings path)
                if (Settings.Default.UpgradeRequired)
                {
                    Settings.Default.Upgrade();
                    Settings.Default.UpgradeRequired = false;
                }

                SettingsManager.Load();
                
                if (AssemblyManager.Current.Version > new Version(SettingsManager.Current.SettingsVersion))
                    SettingsManager.Update(AssemblyManager.Current.Version, new Version(SettingsManager.Current.SettingsVersion));
            }
            catch (InvalidOperationException)
            {
                SettingsManager.InitDefault();
                WpfFramework.Models.Settings.ConfigurationManager.Current.ShowSettingsResetNoteOnStartup = true;
            }
            
            // Load localization (requires settings to be loaded first)

            var culture = LocalizationManager.GetInstance(SettingsManager.Current.Localization_CultureCode).Culture;
            if (SettingsManager.Current.General_OverwriteCurrencySymbol)
            {
                culture = new CultureInfo(culture.Name, true);
                culture.NumberFormat.CurrencySymbol = SettingsManager.Current.General_CurrencySymbol;
                string overwriteCultureCode = SettingsManager.Current.General_OverwriteCultureCode;
                var temp = CultureInfo
                            .GetCultures(CultureTypes.AllCultures)
                            .Where(c => !c.IsNeutralCulture && c.Name == overwriteCultureCode).FirstOrDefault();
                if(SettingsManager.Current.General_OverwriteNumberFormats)
                    culture.NumberFormat = temp.NumberFormat;
                if (temp != null && !string.IsNullOrEmpty(overwriteCultureCode))
                {
                    FrameworkElement.LanguageProperty.OverrideMetadata(
                    typeof(FrameworkElement),
                    new FrameworkPropertyMetadata(
                        XmlLanguage.GetLanguage(
                        temp.IetfLanguageTag)));
                }
                else
                {
                    FrameworkElement.LanguageProperty.OverrideMetadata(
                    typeof(FrameworkElement),
                    new FrameworkPropertyMetadata(
                        XmlLanguage.GetLanguage(
                        culture.IetfLanguageTag)));
                }

            }
            else 
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(
                    culture.IetfLanguageTag)));

            //WpfFramework.Resources.Localization.Strings.Culture = LocalizationManager.Culture;

            WpfFramework.Resources.Localization.Strings.Culture = culture;
            // Create mutex
            _mutex = new Mutex(true, "{" + GUID + "}");
            var mutexIsAcquired = _mutex.WaitOne(TimeSpan.Zero, true);

            // Release mutex
            if (mutexIsAcquired)
                _mutex.ReleaseMutex();

            if (SettingsManager.Current.Window_MultipleInstances || mutexIsAcquired)
            {
                if (SettingsManager.Current.General_BackgroundJobInterval != 0)
                {
                    _dispatcherTimer = new DispatcherTimer
                    {
                        Interval = TimeSpan.FromMinutes(SettingsManager.Current.General_BackgroundJobInterval)
                    };

                    _dispatcherTimer.Tick += DispatcherTimer_Tick;

                    _dispatcherTimer.Start();
                }

                StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
            }
            else
            {
                // Bring the already running application into the foreground
                SingleInstance.PostMessage((IntPtr)SingleInstance.HWND_BROADCAST, SingleInstance.WM_SHOWME, IntPtr.Zero, IntPtr.Zero);

                _singleInstanceClose = true;
                Shutdown();
            }
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            Save();
        }

        protected override void OnSessionEnding(SessionEndingCancelEventArgs e)
        {
            base.OnSessionEnding(e);

            e.Cancel = true;

            Shutdown();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // Save settings, when the application is normally closed
            if (_singleInstanceClose)
                return;

            _dispatcherTimer?.Stop();

            Save();
        }

        private void Save()
        {
            // Save local settings (custom settings path in AppData/Local)
            Settings.Default.Save();

            if (SettingsManager.Current.SettingsChanged) // This will also create the "Settings" folder, if it does not exist
                SettingsManager.Save();

            if (CredentialManager.CredentialsChanged)
                CredentialManager.Save();

            if (LicenseManager.LicensesChanged)
                LicenseManager.Save();
        }
    }
}
