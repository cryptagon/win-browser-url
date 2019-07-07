using System;
using System.Diagnostics;
using System.Windows.Automation;

public class BrowserUrl {

    public static void ReadChrome()
    {
        // there are always multiple chrome processes, so we have to loop through all of them to find the
        // process with a Window Handle and an automation element of name "Address and search bar"
        Process[] procsChrome = Process.GetProcessesByName("chrome");
        foreach (Process chrome in procsChrome)
        {
            // the chrome process must have a window
            if (chrome.MainWindowHandle == IntPtr.Zero)
            {
                continue;
            }

            // find the automation element
            AutomationElement elm = AutomationElement.FromHandle(chrome.MainWindowHandle);

            // manually walk through the tree, searching using TreeScope.Descendants is too slow (even if it's more reliable)
            AutomationElement elmUrlBar = null;
            try
            {
                // walking path found using inspect.exe (Windows SDK) for Chrome 31.0.1650.63 m (currently the latest stable)
                var elm1 = elm.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "Google Chrome"));
                if (elm1 == null) { continue; } // not the right chrome.exe
                                                // here, you can optionally check if Incognito is enabled:
                                                //bool bIncognito = TreeWalker.RawViewWalker.GetFirstChild(TreeWalker.RawViewWalker.GetFirstChild(elm1)) != null;

                // get main window
                var elm2 = TreeWalker.RawViewWalker.GetLastChild(elm1);

                // get header controls
                var elm3 = elm2.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, ""));

                // get edi
                elmUrlBar = elm3.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));

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

        }
    }

  public static void Main() {
    ReadChrome();
  }
}
