using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfFramework.Models.Exporter;
using WpfFramework.Resources.Localization;

//ADDITIONAL
using WpfFramework.Utilities;

namespace WpfFramework
{
    public class GlobalStaticConfiguration
    {
        // Type to search (verage type speed --> 187 chars/min)
        public static TimeSpan SearchDispatcherTimerTimeSpan => new TimeSpan(0, 0, 0, 0, 750);
        public static TimeSpan CredentialsUILockTime => new TimeSpan(0, 0, 120);

        // Filter
        public static string ApplicationFileExtensionFilter => "Application (*.exe)|*.exe";

        // Settings
        public static ApplicationName General_DefaultApplicationViewName => ApplicationName._3dPrintingCalcualtion;
        public static int General_BackgroundJobInterval => 15;
        public static int General_HistoryListEntries => 5;
        public static double Appearance_Opacity => 0.85;

        public static bool Window_AllowMultipleInstances = true;

        // Support
        public static string supportEmail = "kontakt@andreas-reitberger.de";
        public static string documentationUri = "https://andreas-reitberger.de/en/docs/3d-druckkosten-kalkulator/";

        // License
        public static string licenseUriEnvato = "codecanyon.net";

        public static string licenseUri = "andreas-reitberger.de";
        public static string licenseProductName = "AR-3DDKKV2";

        public static string goProUri = "https://andreas-reitberger.de/en/produkt-kategorie/3d-druckkosten-kalkulator/";

        // Envato
        public static string author_sales_enpoint_uri = $"https://api.envato.com/v3/market/author/sale";//$"https://api.envato.com/v3/market/buyer/purchase";
        public static string ItemId = "22886235";
        public static string accesstoken = "IMlyqC1mDVaprCxhl70ZANAJZtkIkQwz"; // "xYn434gnROSO3I6169kwUV7wlUrAOlx0"; //"etf7ESaYXhNsprPGlTxLcB0B1AsUZZ4z";


        // Templates 
        public static ObservableCollection<ExporterTemplate> defaultTemplates = new ObservableCollection<ExporterTemplate>()
        {
            new ExporterTemplate()
            {
                Id = Guid.NewGuid(),
                IsDefault = true,
                Name = Strings.DefaultExcelOfferExporterTemplate,
                TemplatePath = @"Resources\TemplatesExporter\export_template.xlsx",

                ExporterTarget = ExporterTarget.List,
                Settings = new ObservableCollection<ExporterSettings>()
                {
                    new ExporterSettings() {
                        Id = Guid.NewGuid(),
                        Coordinates = new ExcelCoordinates() {Column = "B", Row = 23},
                        Attribute = ExporterAttribute.Attributes.FirstOrDefault(attr => attr.Property == ExporterProperty.CalculationList && attr.Target == ExporterTarget.List),
                        WorkSheetName = "OfferTemplate"
                    }
                }
            },
            new ExporterTemplate()
            {
                Id = Guid.NewGuid(),
                IsDefault = true,
                Name = Strings.DefaultExcelSingleCalculationTemplate,
                TemplatePath = @"Resources\TemplatesExporter\export_single_template.xlsx",
                ExporterTarget = ExporterTarget.Single,
                Settings = new ObservableCollection<ExporterSettings>()
                {
                    new ExporterSettings() {
                        Id = Guid.NewGuid(),
                        Coordinates = new ExcelCoordinates() {Column = "B", Row = 3},
                        Attribute = ExporterAttribute.Attributes.FirstOrDefault(attr => attr.Property == ExporterProperty.CalculationPriceMaterial && attr.Target == ExporterTarget.Single),
                        WorkSheetName = "Data"
                    },
                    new ExporterSettings() {
                        Id = Guid.NewGuid(),
                        Coordinates = new ExcelCoordinates() {Column = "B", Row = 4},
                        Attribute = ExporterAttribute.Attributes.FirstOrDefault(attr => attr.Property == ExporterProperty.CalculationPriceEnergy && attr.Target == ExporterTarget.Single),
                        WorkSheetName = "Data"
                    },
                    new ExporterSettings() {
                        Id = Guid.NewGuid(),
                        Coordinates = new ExcelCoordinates() {Column = "B", Row = 5},
                        Attribute = ExporterAttribute.Attributes.FirstOrDefault(attr => attr.Property == ExporterProperty.CalculationPricePrinter && attr.Target == ExporterTarget.Single),
                        WorkSheetName = "Data"
                    },
                    new ExporterSettings() {
                        Id = Guid.NewGuid(),
                        Coordinates = new ExcelCoordinates() {Column = "B", Row = 6},
                        Attribute = ExporterAttribute.Attributes.FirstOrDefault(attr => attr.Property == ExporterProperty.CalculationPriceWorksteps && attr.Target == ExporterTarget.Single),
                        WorkSheetName = "Data"
                    },
                    new ExporterSettings() {
                        Id = Guid.NewGuid(),
                        Coordinates = new ExcelCoordinates() {Column = "B", Row = 7},
                        Attribute = ExporterAttribute.Attributes.FirstOrDefault(attr => attr.Property == ExporterProperty.CalculationPriceTax && attr.Target == ExporterTarget.Single),
                        WorkSheetName = "Data"
                    },
                    new ExporterSettings() {
                        Id = Guid.NewGuid(),
                        Coordinates = new ExcelCoordinates() {Column = "B", Row = 8},
                        Attribute = ExporterAttribute.Attributes.FirstOrDefault(attr => attr.Property == ExporterProperty.CalculationPriceHandling && attr.Target == ExporterTarget.Single),
                        WorkSheetName = "Data"
                    },
                    new ExporterSettings() {
                        Id = Guid.NewGuid(),
                        Coordinates = new ExcelCoordinates() {Column = "B", Row = 9},
                        Attribute = ExporterAttribute.Attributes.FirstOrDefault(attr => attr.Property == ExporterProperty.CalculationMargin && attr.Target == ExporterTarget.Single),
                        WorkSheetName = "Data"}
                }
            }
        };

        // EULA
        public static string eulaOnlinePath = "https://andreas-reitberger.de/wp-content/uploads/2015/12/eula-3d-print-cost-calcualtor-english.pdf";
        public static string eulaLocalPath = @"EULA\eula-3d-print-cost-calcualtor-english.txt";

        // Fixes
        public static double FloatPointFix => 1.0;

        // Gcode Viewer
        public static double GcodeInfo_WidthCollapsed => 40;
        public static double GcodeInfo_DefaultWidthExpanded => 250;
        public static double GcodeInfo_MaxWidthExpanded => 450;
        // Gcode Multiple file parser
        public static double GcodeMultiParser_WidthCollapsed => 40;
        public static double GcodeMultiParser_DefaultWidthExpanded => 250;
        public static double GcodeMultiParser_MaxWidthExpanded => 450;

        public static string[] Gcode_ValidFileTypes = new string[] { ".gcode", ".gco", ".gc" };
        // Calculation Viewer
        public static double CalculationView_WidthCollapsed => 40;
        public static double CalculationView_DefaultWidthExpanded => 450;
        public static double CalculationView_MaxWidthExpanded => 550;
        // Repetier View
        public static double Repetier_WidthCollapsed => 40;
        public static double Repetier_DefaultWidthExpanded => 450;
        public static double Repetier_MaxWidthExpanded => 550;

        public static double RepetierServerPro_Panel_WidthCollapsed => 40;
        public static double RepetierServerPro_Panel_DefaultWidthExpanded => 250;
        public static double RepetierServerPro_Panel_MaxWidthExpanded => 550;

        public static bool RepetierServerPro_Default_ShowFunctions = true;

        // OctoPrint View
        public static double OctoPrint_WidthCollapsed => 40;
        public static double OctoPrint_DefaultWidthExpanded => 450;
        public static double OctoPrint_MaxWidthExpanded => 550;
        
        public static double OctoPrint_Panel_WidthCollapsed => 40;
        public static double OctoPrint_Panel_DefaultWidthExpanded => 250;
        public static double OctoPrint_Panel_MaxWidthExpanded => 550;

        public static bool OctoPrint_Default_ShowFunctions = true;

        public static int OctoPrint_DefaultUpdateInterval = 2;
        public static int OctoPrint_MinUpdateInterval = 1;
        public static int OctoPrint_MaxUpdateInterval = 5;

        // Additional versions
        public static string downloadWebCamVersionUri = "https://drive.google.com/open?id=1d1hiIq_qQ78dAl0CuxT7f0JsJF3UnkWL";
    }
}
