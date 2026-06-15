using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using System.Windows.Forms;

namespace DesktopAutomation.Components;

internal static class TextInput
{
    // Focuses the text editor and replaces its content using clipboard paste for stable desktop input.
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

    // Locates the main editable area in apps like Notepad using common UI Automation control types.
    private static AutomationElement? FindEditor(Window window)
    {
        var conditionFactory = window.Automation.ConditionFactory;

        return window.FindFirstDescendant(conditionFactory.ByControlType(ControlType.Document))
            ?? window.FindFirstDescendant(conditionFactory.ByControlType(ControlType.Edit));
    }
}
