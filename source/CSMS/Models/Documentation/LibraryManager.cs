﻿using System.Collections.Generic;
using System.IO;
using System.Reflection;
using WpfFramework.Resources.Localization;

namespace WpfFramework.Models.Documentation
{
    public static class LibraryManager
    {
        private const string LicenseFolderName = "Licenses";

        public static string GetLicenseLocation()
        {
            //var s = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new DirectoryNotFoundException("Program execution directory not found, while trying to build path to license directory!"), LicenseFolderName);
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new DirectoryNotFoundException(Strings.ProgramExecutionDirectoryNotFound), LicenseFolderName);
        }

        public static List<LibraryInfo> List => new List<LibraryInfo>
        {
            new LibraryInfo("MahApps.Metro", "https://github.com/mahapps/mahapps.metro", "A toolkit for creating Metro / Modern UI styled WPF apps.", "MIT License", "https://github.com/MahApps/MahApps.Metro/blob/master/LICENSE"),
            new LibraryInfo("MahApps.Metro.IconPacks", "https://github.com/MahApps/MahApps.Metro.IconPacks", "Some awesome icons for WPF and UWP all together...", "MIT License", "https://github.com/MahApps/MahApps.Metro.IconPacks/blob/master/LICENSE"),
            new LibraryInfo("MahApps.Metro.SimpleChildWindow", "https://github.com/punker76/MahApps.Metro.SimpleChildWindow", "A ChildWindow Manager for MahApps", "MIT License", "https://github.com/punker76/MahApps.Metro.SimpleChildWindow/blob/dev/LICENSE"),
            new LibraryInfo("ControlzEx", "https://github.com/ControlzEx/ControlzEx", "Shared Controlz for WPF and ... more", "MIT License", "https://github.com/ButchersBoy/Dragablz/blob/master/LICENSE"),
            //new LibraryInfo("Heijden.DNS", "https://www.codeproject.com/Articles/23673/DNS-NET-Resolver-C", "Reusable DNS resolver component.", "Code Project Open License", "https://www.codeproject.com/info/cpol10.aspx"),
            //new LibraryInfo("Octokit", "https://github.com/octokit/octokit.net", "A GitHub API client library for .NET", "MIT License", "https://github.com/octokit/octokit.net/blob/master/LICENSE.txt"),
            new LibraryInfo("#SNMP Libary", "https://github.com/lextm/sharpsnmplib", "Sharp SNMP Library - Open Source SNMP for .NET and Mono", "MIT License", "https://github.com/lextm/sharpsnmplib/blob/master/LICENSE"),
            new LibraryInfo("Dragablz", "https://github.com/ButchersBoy/Dragablz", "Dragable and tearable tab control for WPF", "MIT License","https://github.com/ButchersBoy/Dragablz/blob/master/LICENSE"),
            //new LibraryInfo("IPNetwork", "https://github.com/lduchosal/ipnetwork", "C# library take care of complex network, IP, IPv4, IPv6, netmask, CIDR, subnet, subnetting, supernet, and supernetting calculation for .NET developers.", "BSD-2-Clause", "https://github.com/lduchosal/ipnetwork/blob/master/LICENSE"),
            new LibraryInfo("AirspaceFixer" ,"https://github.com/chris84948/AirspaceFixer", "AirspacePanel fixes all Airspace issues with WPF-hosted Winforms.", "MIT License", "https://github.com/chris84948/AirspaceFixer/blob/master/LICENSE"),
            new LibraryInfo("Newtonsoft.Json", "https://github.com/JamesNK/Newtonsoft.Json", "Json.NET is a popular high-performance JSON framework for .NET", "MIT License","https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md"),
            new LibraryInfo("LiveCharts", "https://github.com/Live-Charts/Live-Charts", "Simple, flexible, interactive & powerful charts, maps and gauges for .Net", "MIT License","https://github.com/Live-Charts/Live-Charts/blob/master/LICENSE.TXT"),
            new LibraryInfo("LiveCharts.Wpf", "https://github.com/Live-Charts/Live-Charts", "Simple, flexible, interactive & powerful charts, maps and gauges for .Net", "MIT License","https://github.com/Live-Charts/Live-Charts/blob/master/LICENSE.TXT"),
            new LibraryInfo("NETworkManager", "https://github.com/BornToBeRoot/NETworkManager", "A powerful tool for managing networks and troubleshoot network problems!", "GNU GENERAL PUBLIC LICENSE" , "https://github.com/BornToBeRoot/NETworkManager/blob/master/LICENSE"),
            //new LibraryInfo("gsGCode", "https://github.com/gradientspace/gsGCode", "C# gcode parsing and manipulation library", "MIT License" , "https://github.com/gradientspace/gsGCode/blob/master/LICENSE"),
            new LibraryInfo("Commonservicelocator", "https://github.com/unitycontainer/commonservicelocator", "Microsoft.Practices.ServiceLocation", "Microsoft Public License" , "https://github.com/unitycontainer/commonservicelocator/blob/master/LICENSE"),
            new LibraryInfo("Cyotek.Drawing.BitmapFont", "https://github.com/cyotek/Cyotek.Drawing.BitmapFont", "Component for parsing bitmap font files generated by AngelCode's BMFont utility", "MIT License" , "https://github.com/cyotek/Cyotek.Drawing.BitmapFont/blob/master/LICENSE.txt"),
            new LibraryInfo("Helix Toolkit", "https://github.com/helix-toolkit/helix-toolkit", "Helix Toolkit is a collection of 3D components for .NET.", "MIT License" , "https://github.com/helix-toolkit/helix-toolkit/blob/develop/LICENSE"),
            new LibraryInfo("Ixmilia.stl", "https://github.com/IxMilia/Stl", "A stl parser for .NET.", "Apache License 2.0" , "https://github.com/ixmilia/stl/blob/master/License.txt"),
            new LibraryInfo("WpfToolkit", "https://github.com/xceedsoftware/wpftoolkit", "All the controls missing in WPF.", "Microsoft Public License" , "https://github.com/xceedsoftware/wpftoolkit/blob/master/license.md"),
            new LibraryInfo("RestSharp", "https://github.com/restsharp/RestSharp", "Simple .NET REST Client", "Apache License 2.0" , "https://github.com/restsharp/RestSharp/blob/master/LICENSE.txt"),
            new LibraryInfo("Websocket Sharp", "https://github.com/sta/websocket-sharp", "Simple .NET websocket library", "MIT" , "https://github.com/sta/websocket-sharp/blob/master/LICENSE.txt"),
            new LibraryInfo("Log4Net", "https://github.com/sta/websocket-sharp", ".NET logging library", "Apache License 2.0" , "https://github.com/apache/logging-log4net/blob/master/LICENSE"),
            new LibraryInfo("LibVLCSharp", "https://github.com/videolan/libvlcsharp", ".NET library for playing and streaming videos", "GNU Lesser General Public License v2.1" , "https://github.com/videolan/libvlcsharp/blob/3.x/LICENSE"),
            new LibraryInfo("VideoLAN.LibVLC.Windows", "https://code.videolan.org/videolan/libvlc-nuget", ".NET library for playing and streaming videos", "GNU Lesser General Public License v2.1" , "https://code.videolan.org/videolan/libvlc-nuget/blob/master/LICENSE"),
            new LibraryInfo("Syncfusion Essential Studio", "https://www.syncfusion.com/?utm_source=nuget&utm_medium=listing", "Includes more than 1,000 components and frameworks for WinForms, WPF, ASP.NET (Web Forms, MVC, Core), UWP, Xamarin, Flutter, JavaScript, Angular, Blazor, Vue and React.", "ESSENTIAL STUDIO SOFTWARE LICENSE AGREEMENT" , "https://www.syncfusion.com/license/studio/17.4.0.39/syncfusion_essential_studio_eula.pdf"),
            new LibraryInfo("nUpdate", "https://github.com/dbforge/nUpdate", "nUpdate is a modern update system for .NET applications", "MIT" , "https://github.com/dbforge/nUpdate/blob/master/LICENSE"),
        };
    }
}
