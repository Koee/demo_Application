using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using FlaUIApplication = FlaUI.Core.Application;

namespace DesktopAutomation.Components;

internal static class WindowFinder
{
    public static Window WaitForMainWindow(
        FlaUIApplication app,
        UIA3Automation automation,
        string expectedName,
        int timeoutMs = 10000)
    {
        var start = DateTime.Now;

        while ((DateTime.Now - start).TotalMilliseconds < timeoutMs)
        {
            var window = app.GetMainWindow(automation);

            if (window != null)
            {
                return window;
            }

            Thread.Sleep(300);
        }

        throw new Exception($"Cannot find main window: {expectedName}");
    }
}
