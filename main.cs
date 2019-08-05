using System;
using System.Diagnostics;
using System.Windows.Automation;

public class BrowserUrl {

    public static void ReadChrome()
    {
        Process[] procsChrome = Process.GetProcessesByName("chrome");
        foreach (Process chrome in procsChrome)
        {
            if (chrome.MainWindowHandle == IntPtr.Zero)
            {
                continue;
            }

            // find the automation element
            AutomationElement elm = AutomationElement.FromHandle(chrome.MainWindowHandle);
            bool bIncognito = elm.GetCurrentPropertyValue(AutomationElement.NameProperty).ToString().Contains("(Incognito)");
            if (bIncognito) continue;

            // manually walk through the tree, searching using TreeScope.Descendants is too slow (even if it's more reliable)
            AutomationElement elmUrlBar = null;
            try
            {
                var elm1 = elm.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "Google Chrome"));
                if (elm1 == null) { continue; } // not the right chrome.exe

                // get main window
                var elm2 = TreeWalker.RawViewWalker.GetLastChild(elm1);

                // get header controls
                var elm3 = elm2.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, ""));

                // get edit
                elmUrlBar = elm3.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                continue;
            }

            // make sure it's valid
            if (elmUrlBar == null)
            {
                // it's not..
                continue;
            }

            System.Console.WriteLine(elmUrlBar.GetCurrentPropertyValue(ValuePatternIdentifiers.ValueProperty));
            return;
        }
    }
    public static void ReadEdge()
    {
        Process[] procs = Process.GetProcessesByName("ApplicationFrameHost");
        foreach (Process proc in procs)
        {
            if (proc.MainWindowHandle == IntPtr.Zero)
            {
                continue;
            }

            // find the automation element
            AutomationElement elm = AutomationElement.FromHandle(proc.MainWindowHandle);

            // manually walk through the tree, searching using TreeScope.Descendants is too slow (even if it's more reliable)
            AutomationElement elmUrlBar = null;
            try
            {
                var elm1 = elm.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "Microsoft Edge"));
                if (elm1 == null) { continue; }

                // get edit
                elmUrlBar = elm1.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                continue;
            }

            // make sure it's valid
            if (elmUrlBar == null)
            {
                // it's not..
                continue;
            }

            System.Console.WriteLine(elmUrlBar.GetCurrentPropertyValue(ValuePatternIdentifiers.ValueProperty));
            return;
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

            // find the automation element
            AutomationElement elm = AutomationElement.FromHandle(proc.MainWindowHandle);

            // manually walk through the tree, searching using TreeScope.Descendants is too slow (even if it's more reliable)
            AutomationElement elmUrlBar = null;
            try
            {
                var elm1 = elm.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "Navigation Toolbar"));
                if (elm1 == null) { continue; }

                // get edit
                elmUrlBar = elm1.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));

            }
            catch (Exception ex)
            {
                // Chrome has probably changed something, and above walking needs to be modified. :(
                // put an assertion here or something to make sure you don't miss it
                System.Console.WriteLine(ex);
                continue;
            }

            // make sure it's valid
            if (elmUrlBar == null)
            {
                // it's not..
                continue;
            }

            System.Console.WriteLine(elmUrlBar.GetCurrentPropertyValue(ValuePatternIdentifiers.ValueProperty));
            return;
        }
    }
    public static void Main(string[] args) {
        string browser = args.Length == 0 ? "chrome" : args[0];

        if (browser == "chrome")
        {
            ReadChrome();
        }
        else if (browser == "firefox")
        {
            ReadFirefox();
        }
        else
        {
            ReadEdge();
        }
    }

}
    