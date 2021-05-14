using System;
using System.Diagnostics;
using FlaUI.UIA3;
using FlaUI.Core.Definitions;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;

public class BrowserUrl {

    public static void ReadChrome()
    {
        var dt = DateTime.Now;
        Process[] procs = Process.GetProcessesByName("chrome");
        foreach (Process proc in procs)
        {
            if (proc.MainWindowHandle == IntPtr.Zero)
            {
                continue;
            }
            using (var automation = new UIA3Automation())
            {
                var app = new FlaUI.Core.Application(proc);
                var window = app.GetMainWindow(automation);
                if (window.Name.Contains("(Incognito)")) return;

                var treewalker = automation.TreeWalkerFactory.GetRawViewWalker();
                var elm1 = treewalker.GetLastChild(window);
                if (elm1 == null) { continue; } // not the right chrome.exe
                if ((DateTime.Now - dt).TotalMilliseconds > 1000) return;
                // get main window
                var elm2 = treewalker.GetLastChild(elm1);
                if ((DateTime.Now - dt).TotalMilliseconds > 1000) return;
                // get header controls
                var elm3 = treewalker.GetFirstChild(elm2);
                if ((DateTime.Now - dt).TotalMilliseconds > 1000) return;
                // get nav bar
                var elm4 = treewalker.GetNextSibling(treewalker.GetFirstChild(elm3));
                if ((DateTime.Now - dt).TotalMilliseconds > 1000) return;
                // get edit
                var elmUrlBar = elm4.FindFirstChild(cf => cf.ByControlType(ControlType.Edit)).AsTextBox();
                Console.WriteLine(elmUrlBar.Text);
            }

        }
        return;
    }

    public static void ReadEdge()
    {
        DateTime dt = DateTime.Now;
        Process[] procs = Process.GetProcessesByName("msedge");
        foreach (Process proc in procs)
        {
            if (proc.MainWindowHandle == IntPtr.Zero)
            {
                continue;
            }
            using (var automation = new UIA3Automation())
            {
                var app = new FlaUI.Core.Application(proc);
                var window = app.GetMainWindow(automation);
                if (window.Name.Contains("[InPrivate]")) return;

                var treewalker = automation.TreeWalkerFactory.GetRawViewWalker();

                var elm1 = treewalker.GetLastChild(window);
                if (elm1 == null) { continue; } // not the right process
                if ((DateTime.Now - dt).TotalMilliseconds > 1000) return;
                // get main window
                var elm2 = treewalker.GetFirstChild(elm1);
                if ((DateTime.Now - dt).TotalMilliseconds > 1000) return;
                // get browser window
                var elm3 = treewalker.GetNextSibling(treewalker.GetFirstChild(elm2));
                if ((DateTime.Now - dt).TotalMilliseconds > 1000) return;
                // get header bar
                var elm4 = treewalker.GetFirstChild(elm3);
                if ((DateTime.Now - dt).TotalMilliseconds > 1000) return;
                // get header bar
                var elm5 = elm4.FindFirstChild(cf => cf.ByName("App bar"));
                if ((DateTime.Now - dt).TotalMilliseconds > 1000) return;
                // get edit group
                var elm6 = elm5.FindFirstChild(cf => cf.ByClassName("LocationBarView"));
                if ((DateTime.Now - dt).TotalMilliseconds > 1000) return;
                // get edit
                var elmUrlBar = elm6.FindFirstChild(cf => cf.ByControlType(ControlType.Edit)).AsTextBox();
                Console.WriteLine(elmUrlBar.Text);
                return;
            }
        }
    }

    public static void ReadEdgeLegacy()
    {
        Process[] procs = Process.GetProcessesByName("ApplicationFrameHost");
        foreach (Process proc in procs)
        {
            if (proc.MainWindowHandle == IntPtr.Zero)
            {
                continue;
            }
            using (var automation = new UIA3Automation())
            {
                var app = new FlaUI.Core.Application(proc);
                var window = app.GetMainWindow(automation);
                var treewalker = automation.TreeWalkerFactory.GetRawViewWalker();

                var elm1 = window.FindFirstChild(cf => cf.ByName("Microsoft Edge"));
                if (elm1 == null) { continue; }

                // get edit
                var elmUrlBar = elm1.FindFirstDescendant(cf => cf.ByControlType(ControlType.Edit)).AsTextBox();
                Console.WriteLine(elmUrlBar.Text);
                return;
            }
        }
    }

    public static void ReadFirefox()
    {
        Process[] procs = Process.GetProcessesByName("Firefox");
        foreach (Process proc in procs)
        {
            if (proc.MainWindowHandle == IntPtr.Zero)
            {
                continue;
            }
            using (var automation = new UIA3Automation())
            {
                var app = new FlaUI.Core.Application(proc);
                var window = app.GetMainWindow(automation);
                var treewalker = automation.TreeWalkerFactory.GetRawViewWalker();
                var elm1 = window.FindFirstChild(cf => cf.ByName("Navigation"));
                if (elm1 == null) { continue; }

                // get edit
                var elmUrlBar = elm1.FindFirstDescendant(cf => cf.ByControlType(ControlType.Edit)).AsTextBox();
                Console.WriteLine(elmUrlBar.Text);
                return;
            }
        }
    }
    public static void GetAllProcesses()
    {
        Process[] procs = Process.GetProcesses();
        foreach (Process proc in procs)
        {
            if (proc.ProcessName != "svchost") System.Console.WriteLine("process " + proc.ProcessName);
        }
    }

    public static void Main(string[] args) {
        string browser = args.Length == 0 ? "firefox" : args[0];

        if (browser == "chrome")
        {
            ReadChrome();
        }
        else if (browser == "firefox")
        {
            ReadFirefox();
        }
        else if (browser == "edge_legacy")
        {
            ReadEdgeLegacy();
        }
        else
        {
            ReadEdge();
        }
    }

}
    