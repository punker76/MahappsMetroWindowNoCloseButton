using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WpfFramework.Models.Exporter;
using WpfFramework.Resources.Localization;

//ADDITIONAL
using WpfFramework.Utilities;

namespace WpfFramework.Models.Settings
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class SettingsInfo : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Variables
        [XmlIgnore] public bool SettingsChanged { get; set; }

        [XmlIgnore] public bool isLicensed
        {
            get
            {
                if (!LicenseManager.IsLoaded)
                    LicenseManager.Load(SecureStringHelper.ConvertToSecureString(":-CjWu6g8smE ^< h@"));
                return LicenseManager.Licenses[0].isValid;
            }
        }

        private string _settingsVersion = "0.0.0.0";
        public string SettingsVersion
        {
            get => _settingsVersion;
            set
            {
                if (value == _settingsVersion)
                    return;

                _settingsVersion = value;
                SettingsChanged = true;
            }
        }

        #region License
        private bool _lic_IsFirstStart = true;
        public bool License_IsFirstStart
        {
            get => _lic_IsFirstStart;
            set
            {
                if (value == _lic_IsFirstStart)
                    return;

                _lic_IsFirstStart = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }
        #endregion

        #region EULA
        private bool _showEULAOnStartup = true;
        public bool ShowEULAOnStartup
        {
            get => _showEULAOnStartup;
            set
            {
                if (value == _showEULAOnStartup)
                    return;

                _showEULAOnStartup = value;
                SettingsChanged = true;
            }
        }
        private bool _agreedEULA = false;
        public bool AgreedEULA
        {
            get => _agreedEULA;
            set
            {
                if (value == _agreedEULA)
                    return;

                _agreedEULA = value;
                SettingsChanged = true;
            }
        }
        
        private DateTime _agreedEULAOn;
        public DateTime AgreedEULAOn
        {
            get => _agreedEULAOn;
            set
            {
                if (value == _agreedEULAOn)
                    return;

                _agreedEULAOn = value;
                SettingsChanged = true;
            }
        }

        #endregion

        #region EventLogger
        // Overall Logging
        private bool _EventLogger_enableLogging = true;
        public bool EventLogger_EnableLogging
        {
            get => _EventLogger_enableLogging;
            set
            {
                if (value == _EventLogger_enableLogging)
                    return;

                _EventLogger_enableLogging = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }

        // Saved logs
        private int _EventLogger_savedLogs = 150;
        public int EventLogger_AmountSavedLogs
        {
            get => _EventLogger_savedLogs;
            set
            {
                if (value == _EventLogger_savedLogs)
                    return;

                _EventLogger_savedLogs = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }
        #endregion

        #region General 


        private ApplicationName _general_DefaultApplicationViewName = GlobalStaticConfiguration.General_DefaultApplicationViewName;
        public ApplicationName General_DefaultApplicationViewName
        {
            get => _general_DefaultApplicationViewName;
            set
            {
                if (value == _general_DefaultApplicationViewName)
                    return;

                _general_DefaultApplicationViewName = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }

        private int _general_BackgroundJobInterval = GlobalStaticConfiguration.General_BackgroundJobInterval;
        public int General_BackgroundJobInterval
        {
            get => _general_BackgroundJobInterval;
            set
            {
                if (value == _general_BackgroundJobInterval)
                    return;

                _general_BackgroundJobInterval = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }

        private int _general_HistoryListEntries = GlobalStaticConfiguration.General_HistoryListEntries;
        public int General_HistoryListEntries
        {
            get => _general_HistoryListEntries;
            set
            {
                if (value == _general_HistoryListEntries)
                    return;

                _general_HistoryListEntries = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }

        private bool _general_OpenCalculationResultView = true;
        public bool General_OpenCalculationResultView
        {
            get => _general_OpenCalculationResultView;
            set
            {
                if (value == _general_OpenCalculationResultView)
                    return;

                _general_OpenCalculationResultView = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }

        private bool _general_overwriteCurrencySymbol = false;
        public bool General_OverwriteCurrencySymbol
        {
            get => _general_overwriteCurrencySymbol;
            set
            {
                if (value == _general_overwriteCurrencySymbol)
                    return;

                _general_overwriteCurrencySymbol = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }

        private bool _general_overwriteNumberFormats = false;
        public bool General_OverwriteNumberFormats
        {
            get => _general_overwriteNumberFormats;
            set
            {
                if (value == _general_overwriteNumberFormats)
                    return;

                _general_overwriteNumberFormats = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }

        private string _general_CurrencySymbol = string.Empty;
        public string General_CurrencySymbol
        {
            get => _general_CurrencySymbol;
            set
            {
                if (value == _general_CurrencySymbol)
                    return;

                _general_CurrencySymbol = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }

        private string _general_OverwriteCultureCode = string.Empty;
        public string General_OverwriteCultureCode
        {
            get => _general_OverwriteCultureCode;
            set
            {
                if (value == _general_OverwriteCultureCode)
                    return;

                _general_OverwriteCultureCode = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }

        private ObservableCollection<ApplicationViewInfo> _general_ApplicationList = new ObservableCollection<ApplicationViewInfo>();
        public ObservableCollection<ApplicationViewInfo> General_ApplicationList
        {
            get => _general_ApplicationList;
            set
            {
                if (value == _general_ApplicationList)
                    return;

                _general_ApplicationList = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }

        //SvnRepositiories
        private ObservableCollection<object> _repos = new ObservableCollection<object>();
        public ObservableCollection<object> Repos
        {
            get => _repos;
            set
            {
                if (value == _repos)
                    return;

                _repos = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }

        // Window
        private bool _window_ConfirmClose;
        public bool Window_ConfirmClose
        {
            get => _window_ConfirmClose;
            set
            {
                if (value == _window_ConfirmClose)
                    return;

                _window_ConfirmClose = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }

        private bool _window_MinimizeInsteadOfTerminating;
        public bool Window_MinimizeInsteadOfTerminating
        {
            get => _window_MinimizeInsteadOfTerminating;
            set
            {
                if (value == _window_MinimizeInsteadOfTerminating)
                    return;

                _window_MinimizeInsteadOfTerminating = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }

        private bool _window_MultipleInstances;
        public bool Window_MultipleInstances
        {
            get => _window_MultipleInstances;
            set
            {
                if (value == _window_MultipleInstances)
                    return;

                _window_MultipleInstances = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }

        private bool _window_MinimizeToTrayInsteadOfTaskbar;
        public bool Window_MinimizeToTrayInsteadOfTaskbar
        {
            get => _window_MinimizeToTrayInsteadOfTaskbar;
            set
            {
                if (value == _window_MinimizeToTrayInsteadOfTaskbar)
                    return;

                _window_MinimizeToTrayInsteadOfTaskbar = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }

        // TrayIcon
        private bool _trayIcon_AlwaysShowIcon;
        public bool TrayIcon_AlwaysShowIcon
        {
            get => _trayIcon_AlwaysShowIcon;
            set
            {
                if (value == _trayIcon_AlwaysShowIcon)
                    return;

                _trayIcon_AlwaysShowIcon = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }

        // Appearance
        #region Appearance
        private string _appearance_AppTheme;
        public string Appearance_AppTheme
        {
            get => _appearance_AppTheme;
            set
            {
                if (value == _appearance_AppTheme)
                    return;

                _appearance_AppTheme = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }

        private string _appearance_Accent;
        public string Appearance_Accent
        {
            get => _appearance_Accent;
            set
            {
                if (value == _appearance_Accent)
                    return;

                _appearance_Accent = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }

        private bool _appearance_EnableTransparency;
        public bool Appearance_EnableTransparency
        {
            get => _appearance_EnableTransparency;
            set
            {
                if (value == _appearance_EnableTransparency)
                    return;

                _appearance_EnableTransparency = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }

        private double _appearance_Opacity = GlobalStaticConfiguration.Appearance_Opacity;
        public double Appearance_Opacity
        {
            get => _appearance_Opacity;
            set
            {
                if (Math.Abs(value - _appearance_Opacity) < GlobalStaticConfiguration.FloatPointFix)
                    return;

                _appearance_Opacity = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }
        #endregion

        // Localization
        #region Localization
        private string _localization_CultureCode;
        public string Localization_CultureCode
        {
            get => _localization_CultureCode;
            set
            {
                if (value == _localization_CultureCode)
                    return;

                _localization_CultureCode = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }
        #endregion

        // Autostart
        private bool _autostart_StartMinimizedInTray;
        public bool Autostart_StartMinimizedInTray
        {
            get => _autostart_StartMinimizedInTray;
            set
            {
                if (value == _autostart_StartMinimizedInTray)
                    return;

                _autostart_StartMinimizedInTray = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }


        // Update
        private bool _update_CheckForUpdatesAtStartup = true;
        public bool Update_CheckForUpdatesAtStartup
        {
            get => _update_CheckForUpdatesAtStartup;
            set
            {
                if (value == _update_CheckForUpdatesAtStartup)
                    return;

                _update_CheckForUpdatesAtStartup = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }
        #endregion

        #region Exporters
        private ObservableCollection<ExporterTemplate> _exporterTemplates = new ObservableCollection<ExporterTemplate>();
        //private ObservableCollection<ExporterTemplate> _exporterTemplates = GlobalStaticConfiguration.defaultTemplates;
        public ObservableCollection<ExporterTemplate> ExporterExcel_Templates
        {
            get => _exporterTemplates;
            set
            {
                if (value == _exporterTemplates)
                    return;

                _exporterTemplates = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }

        private string _exporterExcel_TemplatePath = "export_template.xlsx";
        public string ExporterExcel_TemplatePath
        {
            get => _exporterExcel_TemplatePath;
            set
            {
                if (value == _exporterExcel_TemplatePath)
                    return;

                _exporterExcel_TemplatePath = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }
        
        private string _exporterExcel_LastExportPath;
        public string ExporterExcel_LastExportPath
        {
            get => _exporterExcel_LastExportPath;
            set
            {
                if (value == _exporterExcel_LastExportPath)
                    return;

                _exporterExcel_LastExportPath = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }

        private string _exporterExcel_StartColumn;
        public string ExporterExcel_StartColumn
        {
            get => _exporterExcel_StartColumn;
            set
            {
                if (value == _exporterExcel_StartColumn)
                    return;

                _exporterExcel_StartColumn = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }
        
        private string _exporterExcel_StartRow;
        public string ExporterExcel_StartRow
        {
            get => _exporterExcel_StartRow;
            set
            {
                if (value == _exporterExcel_StartRow)
                    return;

                _exporterExcel_StartRow = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }

        private bool _exporterExcel_WriteToTemplate;
        public bool ExporterExcel_WriteToTemplate
        {
            get => _exporterExcel_WriteToTemplate;
            set
            {
                if (value == _exporterExcel_WriteToTemplate)
                    return;

                _exporterExcel_WriteToTemplate = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }
        
        private bool _exporterExcel_LastExportAsPdf = false;
        public bool ExporterExcel_LastExportAsPdf
        {
            get => _exporterExcel_LastExportAsPdf;
            set
            {
                if (value == _exporterExcel_LastExportAsPdf)
                    return;

                _exporterExcel_LastExportAsPdf = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }
        #endregion

        #region Others
        // Application view       
        private bool _expandApplicationView;
        public bool ExpandApplicationView
        {
            get => _expandApplicationView;
            set
            {
                if (value == _expandApplicationView)
                    return;

                _expandApplicationView = value;
                OnPropertyChanged();
                SettingsChanged = true;
            }
        }
        #endregion

        #endregion

        #region Constructor
        public SettingsInfo()
        {
            // General
            General_ApplicationList.CollectionChanged += CollectionChanged;
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SettingsChanged = true;
        }
        #endregion
    }
}
