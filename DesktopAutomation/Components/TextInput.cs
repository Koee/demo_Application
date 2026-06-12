using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using System.Windows.Forms;

namespace DesktopAutomation.Components;

internal static class TextInput
{
    public static void ReplaceTextInEditor(Window window, string text)
    {
        WindowActivator.BringToFront(window);

        var editor = FindEditor(window);

        if (editor != null)
        {
            editor.Focus();
            editor.Click();
        }
        else
        {
            window.Focus();
        }

        Thread.Sleep(300);
        Clipboard.SetDataObject(text, true, 5, 100);

        SendKeys.SendWait("^a");
        Thread.Sleep(100);
        SendKeys.SendWait("^v");
        SendKeys.Flush();
    }

    private static AutomationElement? FindEditor(Window window)
    {
        var conditionFactory = window.Automation.ConditionFactory;

        return window.FindFirstDescendant(conditionFactory.ByControlType(ControlType.Document))
            ?? window.FindFirstDescendant(conditionFactory.ByControlType(ControlType.Edit));
    }
}
