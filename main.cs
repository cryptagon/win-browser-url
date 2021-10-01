using System;
using System.Diagnostics;
using FlaUI.UIA3;
using FlaUI.Core.Definitions;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;

public class BrowserUrl {

    static bool DEBUG = false;

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
            try
            {
                using (var automation = new UIA3Automation())
                {
                    var app = new FlaUI.Core.Application(proc);
                    var window = app.GetMainWindow(automation);
                    if (window == null) continue;
                    if (window.Name.Contains("(Incognito)")) return;

                    var treewalker = automation.TreeWalkerFactory.GetRawViewWalker();
                    var elm1 = treewalker.GetLastChild(window);
                    if ((DateTime.Now - dt).TotalMilliseconds > 1000) return;
                    if (BrowserUrl.DEBUG) Console.WriteLine("1 " + elm1);
                    var elm1Children = elm1.FindAllChildren();
                    if (elm1Children.Length == 1) elm1 = elm1Children[0];

                    // get main window
                    var elm2 = treewalker.GetLastChild(elm1);
                    if ((DateTime.Now - dt).TotalMilliseconds > 1000) return;
                    if (BrowserUrl.DEBUG) Console.WriteLine("2 " + elm2);
                    // get header controls
                    var elm3 = treewalker.GetFirstChild(elm2);
                    if ((DateTime.Now - dt).TotalMilliseconds > 1000) return;
                    if (BrowserUrl.DEBUG) Console.WriteLine("3 " + elm3);
                    // get edit
                    var elmUrlBar = elm3.FindFirstDescendant(cf => cf.ByControlType(ControlType.Edit)).AsTextBox();
                    if (elmUrlBar == null) continue;
                    Console.WriteLine(elmUrlBar.Text);
                    return;
                }
            } catch (Exception e) {
                continue;
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
            try
            {
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
                    var elm2 = elm1.FindFirstDescendant(cf => cf.ByClassName("TopContainerView"));
                    if ((DateTime.Now - dt).TotalMilliseconds > 1000) return;
                    if (BrowserUrl.DEBUG) Console.WriteLine("2 " + elm2);
                    // get header bar
                    var elm3 = elm2.FindFirstChild(cf => cf.ByName("App bar"));
                    if ((DateTime.Now - dt).TotalMilliseconds > 1000) return;
                    if (BrowserUrl.DEBUG) Console.WriteLine("3 " + elm3);
                    // get edit group
                    var elm4 = elm3.FindFirstChild(cf => cf.ByClassName("LocationBarView"));
                    if ((DateTime.Now - dt).TotalMilliseconds > 1000) return;
                    if (BrowserUrl.DEBUG) Console.WriteLine("4 " + elm4);
                    // get edit
                    var elmUrlBar = elm4.FindFirstDescendant(cf => cf.ByControlType(ControlType.Edit)).AsTextBox();
                    Console.WriteLine(elmUrlBar.Text);
                    return;
                }
            }
            catch (Exception)
            {
                continue;
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
            try
            {
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
            catch (Exception)
            {
                continue;
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
            try
            {
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
            catch (Exception)
            {
                continue;
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
        string browser = args.Length == 0 ? "all" : args[0];

        if (browser == "all")
        {
            ReadChrome();
            ReadFirefox();
            ReadEdge();
            ReadEdgeLegacy();
        }
        else if (browser == "chrome")
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
    