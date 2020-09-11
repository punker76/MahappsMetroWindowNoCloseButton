using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using MahApps.Metro.IconPacks;

namespace WpfFramework
{
    public static class ApplicationViewManager
    {
        // List of all applications
        public static List<ApplicationViewInfo> GetList()
        {
            var list = new List<ApplicationViewInfo>();

            foreach (ApplicationName name in Enum.GetValues(typeof(ApplicationName)))
            {
                if (name != ApplicationName.None)
                    list.Add(new ApplicationViewInfo(name));
            }

            return list;
        }

        public static string GetTranslatedNameByName(ApplicationName name)
        {
            switch (name)
            {
                /*
                case Name.MassSearchAndReplace:
                    return Resources.Localization.Strings.AppNameMassSearchAndReplace;
                case Name.SVN:
                    return Resources.Localization.Strings.AppNameSubversionServer;
                    */
                case ApplicationName._3dPrintingMaterial:
                    return Resources.Localization.Strings.AppName3dPrinterMaterialOverview;
                case ApplicationName._3dPrintingPrinter:
                    return Resources.Localization.Strings.AppName3dPrinterOverview;
                case ApplicationName._3dPrintingCalcualtion:
                    return Resources.Localization.Strings.AppName3dPrintCostCalculator;
                case ApplicationName._3dPrintingWorkstep:
                    return Resources.Localization.Strings.AppName3dPrinterWorkstep;
                case ApplicationName.MachineHourRateCalc:
                    return Resources.Localization.Strings.AppNameMachineHourRateCalculator;
                case ApplicationName.RepetierServerProDashboard:
                    return Resources.Localization.Strings.AppNameRepetierServerProDashboard;
                case ApplicationName.OctoPrintDashboard:
                    return Resources.Localization.Strings.AppNameOctoPrintDashboard;
                case ApplicationName.EventLog:
                    return Resources.Localization.Strings.AppNameEventLog;
                default:
                    return Resources.Localization.Strings.AppNameNotFound;
            }
        }

        public static Canvas GetIconByName(ApplicationName name)
        {
            var canvas = new Canvas();

            switch (name)
            {
                /*
                case Name.MassSearchAndReplace:
                    canvas.Children.Add(new PackIconModern { Kind = PackIconModernKind.PageSearch });
                    break;
                case Name.SVN:
                    canvas.Children.Add(new PackIconModern { Kind = PackIconModernKind.Server });
                    break;
                    */
                case ApplicationName._3dPrintingMaterial:
                    canvas.Children.Add(new PackIconModern { Kind = PackIconModernKind.Box });
                    break;
                case ApplicationName._3dPrintingPrinter:
                    canvas.Children.Add(new PackIconMaterial { Kind = PackIconMaterialKind.Printer3d });
                    break;
                case ApplicationName._3dPrintingCalcualtion:
                    canvas.Children.Add(new PackIconModern { Kind = PackIconModernKind.Calculator });
                    break;
                case ApplicationName._3dPrintingWorkstep:
                    canvas.Children.Add(new PackIconUnicons { Kind = PackIconUniconsKind.Constructor });
                    break;
                case ApplicationName.MachineHourRateCalc:
                    canvas.Children.Add(new PackIconModern { Kind = PackIconModernKind.Calculator });
                    break;
                case ApplicationName.RepetierServerProDashboard:
                    canvas.Children.Add(new PackIconMaterial { Kind = PackIconMaterialKind.Speedometer });
                    break;
                case ApplicationName.OctoPrintDashboard:
                    canvas.Children.Add(new PackIconSimpleIcons { Kind = PackIconSimpleIconsKind.OctopusDeploy });
                    break;
                case ApplicationName.EventLog:
                    canvas.Children.Add(new PackIconModern { Kind = PackIconModernKind.DebugStepInto });
                    break;
                default:
                    canvas.Children.Add(new PackIconModern { Kind = PackIconModernKind.SmileyFrown });
                    break;
            }

            return canvas;
        }

        //Application Names
        

        public static void InvalidateByName(ApplicationName ApplicationName)
        {
            ApplicationViewInfo app = GetList().Find(x => x.Name == ApplicationName);
            if(app != null)
            {

            }
        }
        public static void Invalidate(ApplicationViewInfo AppName)
        {
            ApplicationViewInfo app = GetList().Find(x => x.Name == AppName.Name);
            if(app != null)
            {

            }
        }
    }
}
